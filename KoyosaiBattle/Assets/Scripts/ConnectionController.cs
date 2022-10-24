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
    /// �ePC�̎��ʗp(�ΐ킷��PC�œ������O�͔�����)
    [SerializeField]
    string PCNumber;
    /// ���[��ID
    /// �������[��ID��PC��ڑ�����
    [SerializeField]
    string RoomID;
    /// strix cloud�p
    [SerializeField]
    string appID;
    // strix cloud�p
    [SerializeField]
    string masterID;

    // message�\���p
    [SerializeField]
    Text message;

    // connection/create�̃{�^���e�L�X�g
    [SerializeField]
    Text buttom1;
    // join�̃{�^���e�L�X�g
    [SerializeField]
    Text buttom2;

    // ���[��ID����͂���t�B�[���h
    [SerializeField]
    InputField ConnectionID;
    // PCID����͂���t�B�[���h
    [SerializeField]
    InputField PCID;

    // �ڑ�����������ڑ��p��UI���������߂̃p�l��
    [SerializeField]
    GameObject panel;

    [SerializeField]
    PlayerData playerData;

    // 
    [SerializeField]
    UIController uiController;

    // �}�X�^�[�T�[�o�ɐڑ��o���Ă��邩������
    bool isConnectMaster = false;
    

    // �����蔻��Ɏg�p����^�O�p
    static string PlayerTag;

    void Start()
    {
        // �ŏ���join�͕\�����Ȃ�
        buttom2.gameObject.SetActive(false);

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            playerData.gameObject.name = "PC" + PCID.text;

        }
    }
    // Connection�{�^���̃N���b�N�C�x���g
    public void ConnectClick()
	{
        string conID = ConnectionID.text;
        string pcID = PCID.text;

        // �}�X�^�[�T�[�o�ɐڑ��ł��Ă��鎞�̓��[��������
        if(isConnectMaster)
        {
            if(pcID != null && conID != null)
            {
                CreateRoom(conID, pcID);
                buttom1.text = "Connect";
            }
        }
        // �}�X�^�[�T�[�o�ɐڑ����Ă��Ȃ��Ƃ��̓}�X�^�[�T�[�o�ɐڑ������ăe�L�X�g��Create�ɂ��AJoin�{�^����\��
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
    // Join�{�^���̃N���b�N�C�x���g
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
    // ���[���̍쐬
    public void CreateRoom(string roomID, string num)
    {
        StrixNetwork strixNetwork = StrixNetwork.instance;

		RoomProperties roomProperties = new RoomProperties
		{
            // ���[���̏��
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
                SceneChange();
            },
            e => //Faild
            {
                Log("Faild Create Room");
                Log(e.cause.Message);
            }
        );
    }
    // ���[���ɎQ��
	public void JoinRoom(string roomID, string num)
	{
		StrixNetwork strixNetwork = StrixNetwork.instance;

        // ���[�����������Č��������炻�̃��[���ɓ���
		strixNetwork.SearchJoinableRoom(
			condition: ConditionBuilder.Builder().Field("stringKey").EqualTo(roomID + roomID).Build(),
			order: new Order("memberCount", OrderType.Ascending),
			limit: 10,
			offset: 0,
			handler: searchResults =>
			{
				var foundRooms = searchResults.roomInfoCollection;
				Log(foundRooms.Count + " rooms found.");

                // �����������[���̐����O�Ȃ�I��
				if(foundRooms.Count == 0)
				{
					Log("No joinable rooms found.");
					return;
				}

                var roomInfo = foundRooms.GetEnumerator();
                foreach(var room in foundRooms)
				{
                    // ���������[���ɎQ��
                    RoomJoinArgs roomJoinArgs = new RoomJoinArgs();

                    if(strixNetwork.room != null)
                        break;

                    strixNetwork.JoinRoom(
                         host: room.host,
                         port: room.port,
                         protocol: room.protocol,
                         roomId: room.roomId,
                         playerName: num,
                         handler: __ => { Log("Room joined."); SceneChange();  },
                         failureHandler: joinError => Log("Join failed.Reason: " + joinError.cause)
                    );
                }
			},
				failureHandler: searchError => Log("Search failed.Reason: " + searchError.cause)
		);

	}
    // �I���̐ڑ��̉���
	void OnApplicationQuit()
    {
        StrixNetwork strixNetwork = StrixNetwork.instance;
        var room = strixNetwork.room;
        // ���[���ɓ����Ă��Ȃ�
        if(room == null)
		{
            strixNetwork.DisconnectMasterServer();
            strixNetwork.Destroy();
            return;
		}
        // �ޏo���鎞�Ƀ����o�[�������̂݁����[�����폜
        if(room.GetMemberCount() == 1)
        {
            strixNetwork.DeleteRoom(room.GetPrimaryKey(), _ => { Debug.Log("Sucess Delete Room"); strixNetwork.DisconnectMasterServer(); strixNetwork.Destroy(); }, _ => { });
        }
        // �ޏo���鎞�Ƀ����o�[�����������ȊO�ɂ����遨���[������ޏo
        else
        {
            strixNetwork.LeaveRoom(_ => { Debug.Log("Sucess Leave Room"); strixNetwork.DisconnectMasterServer(); strixNetwork.Destroy(); }, _ => { });
        }
    }
    // �ڑ���ʂ�����͉�ʂւ̑J��
    void SceneChange()
	{
        //panel.SetActive(false);
        //foreach(var mem in StrixNetwork.instance.roomMembers)
            //Debug.Log($"{mem.Value.GetName()}:{mem.Value.GetUid()}");
		uiController.state = UIController.PlayState.InputSelecting;
		StrixNetwork.instance.selfRoomMember.SetPrimaryKey(-1);
	}
	// �e�L�X�g�ɕ\�����邽�߂̃��\�b�h
	void Log(string msg)
    {
        message.text += msg + "\n";

        var array = message.text.Split('\n');

        if(array.Length > 11)
        {
            string s = "";
            for(int i = 1;i < array.Length;i++)
			{
                s += array[i] + "\n";
            }
            message.text = s;
        }
    }
}