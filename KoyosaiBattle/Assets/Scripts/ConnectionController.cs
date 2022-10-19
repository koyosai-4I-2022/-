using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoftGear.Strix.Client;
using SoftGear.Strix.Net;
using SoftGear.Strix;
using SoftGear.Strix.Unity;
using SoftGear.Strix.Unity.Runtime;
using System;
using SoftGear.Strix.Client.Core.Model.Manager.Filter;
using SoftGear.Strix.Client.Core.Model.Manager.Filter.Builder;
using SoftGear.Strix.Client.Core.Request;

public class ConnectionController : MonoBehaviour
{
    /// 各PCの識別用(対戦するPCで同じ名前は避ける)
    [SerializeField]
    string PCNumber;
    /// ルームID
    /// 同じルームIDのPCを接続する
    [SerializeField]
    string RoomID;
    /// strix cloud用
    [SerializeField]
    string appID;
    // strix cloud用
    [SerializeField]
    string masterID;

    // message表示用
    [SerializeField]
    Text text;

    // connection/createのボタンテキスト
    [SerializeField]
    Text buttom1;
    // joinのボタンテキスト
    [SerializeField]
    Text buttom2;

    // ルームIDを入力するフィールド
    [SerializeField]
    InputField ConnectionID;
    // PCIDを入力するフィールド
    [SerializeField]
    InputField PCID;

    // 接続成功したら接続用のUIを消すためのパネル
    [SerializeField]
    GameObject panel;

    // マスターサーバに接続出来ているかを入れる
    bool isConnectMaster = false;

    void Start()
    {
        // 最初はjoinは表示しない
        buttom2.gameObject.SetActive(false);
    }

    void Update()
    {

    }
    // Connectionボタンのクリックイベント
    public void ConnectClick()
	{
        string conID = ConnectionID.text;
        string pcID = PCID.text;

        // マスターサーバに接続できている時はルームをつくる
        if(isConnectMaster)
        {
            if(pcID != null && conID != null)
            {
                CreateRoom(conID, pcID);
                buttom1.text = "Connect";
            }
        }
        // マスターサーバに接続していないときはマスターサーバに接続をしてテキストをCreateにし、Joinボタンを表示
        else
        {
            if(pcID != null)
            {
                Connect(pcID);
                buttom1.text = "Create";
                buttom2.gameObject.SetActive(true);
                isConnectMaster = true;
            }
        }
	}
    // Joinボタンのクリックイベント
    public void JoinClick()
	{
        string conID = ConnectionID.text;
        string pcID = PCID.text;

        if(isConnectMaster)
        {
            if(pcID != null && conID != null)
            {
                JoinRoom(conID, pcID);
            }
        }
    }
    public void Connect(string num)
    {
        StrixNetwork strixNetwork = StrixNetwork.instance;
        strixNetwork.applicationId = appID;
        strixNetwork.playerName = num;
        string master = masterID;

        strixNetwork.ConnectMasterServer(
            host: master,
            port: 9122,
            connectEventHandler: _ =>
            {
                Log("Sucess Connect Master");
            },
            errorEventHandler: _ =>
            {
                Log("Faild Connect Master");
            });

    }
    // ルームの作成
    public void CreateRoom(string roomID, string num)
    {
        StrixNetwork strixNetwork = StrixNetwork.instance;

		RoomProperties roomProperties = new RoomProperties
		{
            // ルームの情報
			name = "Room" + roomID,
			capacity = 3,
			stringKey = roomID + roomID,
			properties = new Dictionary<string, object>
		    {
			    { "description", "Room" + roomID }
		    }
		};
        strixNetwork.CreateRoom(
            roomProperties,
            new RoomMemberProperties()
            {
                name = num
            },
            _ => //Sucess
            {
                var room = strixNetwork.room;
                var roomMember = strixNetwork.selfRoomMember;

                Log("Success Create Room");
                //Log("Sucess Create Room"
                //    + "\nRoom name:" + room.GetName()
                //    + "\nRoom capacity:" + room.GetCapacity()
                //    + "\nRoom password:" + room.GetPassword()
                //    + "\nRoom String Key:" + room.GetStringKey()
                //    + "\nRoom Menber Count:" + room.GetMemberCount()
                //    + "\nRoom member name:" + roomMember.GetName());
                panel.SetActive(false);
            },
            e => //Faild
            {
                Log("Faild Create Room");
                Log(e.cause.Message);
            }
        );
    }
    // ルームに参加
	public void JoinRoom(string roomID, string num)
	{
		StrixNetwork strixNetwork = StrixNetwork.instance;

        // ルームを検索して見つかったらそのルームに入る
		strixNetwork.SearchJoinableRoom(
			condition: ConditionBuilder.Builder().Field("stringKey").EqualTo(roomID + roomID).Build(),
			order: new Order("memberCount", OrderType.Ascending),
			limit: 10,
			offset: 0,
			handler: searchResults =>
			{
				var foundRooms = searchResults.roomInfoCollection;
				Log(foundRooms.Count + " rooms found.");

                // 見つかったルームの数が０なら終了
				if(foundRooms.Count == 0)
				{
					Log("No joinable rooms found.");
					return;
				}

                var roomInfo = foundRooms.GetEnumerator();
                foreach(var room in foundRooms)
				{
                    // 見つけたルームに参加
                    RoomJoinArgs roomJoinArgs = new RoomJoinArgs();

                    if(strixNetwork.room != null)
                        break;

                    Debug.Log($"host:{room.host}\n"
                        + $"port:{room.port}"
                        + $"protocol:{room.protocol}"
                         + $"roomID:{room.roomId}");

                    strixNetwork.JoinRoom(
                         host: room.host,
                         port: room.port,
                         protocol: room.protocol,
                         roomId: room.roomId,
                         playerName: num,
                         handler: __ => { Log("Room joined."); panel.SetActive(false);  },
                         failureHandler: joinError => Log("Join failed.Reason: " + joinError.cause)
                    );
                }
			},
				failureHandler: searchError => Log("Search failed.Reason: " + searchError.cause)
		);

	}
    // 終了の接続の解除
	void OnApplicationQuit()
    {
        StrixNetwork strixNetwork = StrixNetwork.instance;
        var room = strixNetwork.room;
        // ルームに入っていない
        if(room == null)
		{
            strixNetwork.DisconnectMasterServer();
            strixNetwork.Destroy();
            return;
		}
        // 退出する時にメンバーが自分のみ→ルームを削除
        if(room.GetMemberCount() == 1)
        {
            strixNetwork.DeleteRoom(room.GetPrimaryKey(), _ => { Debug.Log("Sucess Delete Room"); strixNetwork.DisconnectMasterServer(); strixNetwork.Destroy(); }, _ => { });
        }
        // 退出する時にメンバーが自分自分以外にもいる→ルームから退出
        else
        {
            strixNetwork.LeaveRoom(_ => { Debug.Log("Sucess Leave Room"); strixNetwork.DisconnectMasterServer(); strixNetwork.Destroy(); }, _ => { });
        }
    }
    // テキストに表示するためのメソッド
    void Log(string msg)
    {
        text.text += msg + "\n";

        var array = text.text.Split('\n');

        if(array.Length > 8)
        {
            string s = "";
            for(int i = 1;i < array.Length;i++)
			{
                s += array[i] + "\n";
            }
            text.text = s;
        }
    }
}