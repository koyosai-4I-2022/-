using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerRequestController : MonoBehaviour
{
    [SerializeField]
    Text Message;

    [SerializeField]
    InputField userName;
    [SerializeField]
    InputField userID;

    static int Id = -1;
    static long Score = 100;

    static string BASEURL = "http://localhost:8000/";

    void Start()
    {

    }
    async void Update()
    {

    }

    // Server�����p�̃��\�b�h
    // response��json��Task�ɂ���ĕԂ�
    // var result = Method
    // reslut.Result ��string�Ŏ擾�ł���

    // ���[�U�̈ꗗ���擾
    public static async Task<string> GetUsers()
    {
        var client = new HttpClient();
        var result = await client.GetAsync($"{GetBASEURL()}users");
        var json = await result.Content.ReadAsStringAsync();
        
        return json;
    }
    // ������ID�̃��[�U���擾
    public static async Task<string> GetUser(int id)
    {
        var client = new HttpClient();
        var result = await client.GetAsync($"{GetBASEURL()}users/{id}");
        var json = await result.Content.ReadAsStringAsync();
        
        return json;
    }
    // �����̖��O�̃��[�U��o�^����
    public static async Task<string> PostUser(string name)
	{
        string jsonStr = $"{{ \"name\" : \"{name}\" }}";

        var client = new HttpClient();

        var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

        var result = await client.PostAsync($"{GetBASEURL()}users", content);
        var json = await result.Content.ReadAsStringAsync();

        var j = JsonUtility.FromJson<PostUserJson>(json);
		SetID(j.id);
        
        return json;
    }
    // �����Ŏw�肵��ID�̃��[�U�̖��O������������
    // ID���ȗ��Ō��݂�ID���g��
    public static async Task<string> PutUser(string name, int id = -1)
    {
        if(id == -1)
            id = GetID();

        string jsonStr = $"{{ \"name\" : \"{name}\" }}";

        var client = new HttpClient();

        var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

        var result = await client.PutAsync($"{GetBASEURL()}users/{id}", content);
        var json = await result.Content.ReadAsStringAsync();

        var j = JsonUtility.FromJson<PostUserJson>(json);
        SetID(j.id);
        
        return json;
    }
    // �����Ŏw�肵��ID�̃��[�U���폜����
    public static async Task<string> DeleteUserJson(int id)
	{
        var client = new HttpClient();
        var result = await client.DeleteAsync($"{GetBASEURL()}users/{id}");
        var json = await result.Content.ReadAsStringAsync();

        return json;
    }
    // ID�Ŏw�肵�����[�U�̃X�R�A���擾����
    public static async Task<string> GetScore(int id)
    {
        var client = new HttpClient();
        var result = await client.GetAsync($"{GetBASEURL()}users/{id}/scores");
        var json = await result.Content.ReadAsStringAsync();
        
        return json;
    }
    // �����Ŏw�肵��ID�̃��[�U�̃X�R�A��o�^����
    // ID�ȗ��Ō��݂�ID�g�p
    public static async Task<string> PostScore(int score, int id = -1)
    {
        if(id == -1)
            id = GetID();

        string jsonStr = $"{{ \"rate\" : \"{score}\" }}";

        var client = new HttpClient();

        var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

        var result = await client.PostAsync($"{GetBASEURL()}users/{id}/scores", content);
        var json = await result.Content.ReadAsStringAsync();

        var j = JsonUtility.FromJson<PostUserJson>(json);
        SetID(j.id);
        
        return json;
    }
    // �����L���O���擾
    public static async Task<RankingJson> GetRanking()
    {
        var client = new HttpClient();
        var result = await client.GetAsync($"{GetBASEURL()}ranking");
        var json = await result.Content.ReadAsStringAsync();

        json = "{ \"Users\" : " + json + "}";
        Debug.Log(json);
        return JsonUtility.FromJson<RankingJson>(json);
    }
    // user�̃����L���O���擾
    public static async Task<string> UserRanking(int id)
    {
        var client = new HttpClient();
        var result = await client.GetAsync($"{GetBASEURL()}users/{id}/ranking");
        var json = await result.Content.ReadAsStringAsync();

        return json;
    }

    // �e�L�X�g�ɕ\�����邽�߂̃��\�b�h
    void Log(string msg)
    {
        Message.text += msg + "\n";

        var array = Message.text.Split('\n');

        if(array.Length > 7)
        {
            string s = "";
            for(int i = 1; i < array.Length; i++)
            {
                if(array[i] != String.Empty) s += array[i] + "\n";
            }
            Message.text = s;
        }
    }

    public static void SetID(int id) => Id = id;
    public static void SetName(long score) => Score = score;
    public static int GetID() => Id;
    public static long GetScore() => Score;
    public static String GetBASEURL() => BASEURL;

    [Serializable]
    public class PostUserJson
	{
        public string name;
        public int id;
	}
    [Serializable]
    public class ScoreJson
	{
        public int score;
        public int id;
	}
    [Serializable]
    public class RankingJson
	{
        public RankingUser[] Users;

        [Serializable]
        public class RankingUser
		{
            public int id;
            public string name;
            public int rate;
            public int rank;
		}
	}
    [Serializable]
    public class RankingAroundJson
	{
        public RankingJson.RankingUser[] top_three;
        public RankingJson.RankingUser self;
        public RankingJson.RankingUser[] higher_around_rank_users;
        public RankingJson.RankingUser[] lower_around_rank_users;

    }
}
