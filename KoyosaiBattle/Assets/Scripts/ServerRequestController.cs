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

    string BASEURL = "http://localhost:8000/";

    void Start()
    {

    }
    async void Update()
    {

    }
    public void ClickGetUsers()
	{

	}
    public void ClickGetUser()
	{

	}
    public void ClickPostUser()
	{

	}
    public void ClickPutUser()
	{

	}
    public void ClickDeleteUser()
	{

	}
    public void ClickGetScore()
	{

	}
    public void ClickPostScore()
	{

	}
    public void ClickGetRanking()
	{

	}
    public void ClickGetUserRanking()
	{

	}


    // Server�����p�̃��\�b�h
    // response��json��Task�ɂ���ĕԂ�
    // var result = Method
    // reslut.Result ��string�Ŏ擾�ł���

    // ���[�U�̈ꗗ���擾
    public async Task<string> GetUsers()
    {
        var client = new HttpClient();
        var result = await client.GetAsync($"{BASEURL}users");
        var json = await result.Content.ReadAsStringAsync();

        Log(json);
        return json;
    }
    // ������ID�̃��[�U���擾
    public async Task<string> GetUser(int id)
    {
        var client = new HttpClient();
        var result = await client.GetAsync($"{BASEURL}users/{id}");
        var json = await result.Content.ReadAsStringAsync();

        Log(json);
        return json;
    }
    // �����̖��O�̃��[�U��o�^����
    public async Task<string> PostUser(string name)
	{
        string jsonStr = $"{{ \"name\" : \"{name}\" }}";

        var client = new HttpClient();

        var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

        var result = await client.PostAsync($"{BASEURL}users", content);
        var json = await result.Content.ReadAsStringAsync();

        var j = JsonUtility.FromJson<PostUserJson>(json);
		SetID(j.id);

        Log(json);
        return json;
    }
    // �����Ŏw�肵��ID�̃��[�U�̖��O������������
    // ID���ȗ��Ō��݂�ID���g��
    public async Task<string> PutUser(string name, int id = -1)
    {
        if(id == -1)
            id = GetID();

        string jsonStr = $"{{ \"name\" : \"{name}\" }}";

        var client = new HttpClient();

        var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

        var result = await client.PutAsync($"{BASEURL}users/{id}", content);
        var json = await result.Content.ReadAsStringAsync();

        var j = JsonUtility.FromJson<PostUserJson>(json);
        SetID(j.id);

        Log(json);
        return json;
    }
    // �����Ŏw�肵��ID�̃��[�U���폜����
    public async Task<string> DeleteUserJson(int id)
	{
        var client = new HttpClient();
        var result = await client.DeleteAsync($"{BASEURL}users/{id}");
        var json = await result.Content.ReadAsStringAsync();

        return json;
    }
    // ID�Ŏw�肵�����[�U�̃X�R�A���擾����
    public async Task<string> GetScore(int id)
    {
        var client = new HttpClient();
        var result = await client.GetAsync($"{BASEURL}users/{id}/scores");
        var json = await result.Content.ReadAsStringAsync();

        Log(json);
        return json;
    }
    // �����Ŏw�肵��ID�̃��[�U�̃X�R�A��o�^����
    // ID�ȗ��Ō��݂�ID�g�p
    public async Task<string> PostScore(int score, int id = -1)
    {
        if(id == -1)
            id = GetID();

        string jsonStr = $"{{ \"rate\" : \"{score}\" }}";

        var client = new HttpClient();

        var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

        var result = await client.PostAsync($"{BASEURL}users/{id}/scores", content);
        var json = await result.Content.ReadAsStringAsync();

        var j = JsonUtility.FromJson<PostUserJson>(json);
        SetID(j.id);

        Log(json);
        return json;
    }
    // �����L���O���擾
    public async Task<string> GetRanking()
    {
        var client = new HttpClient();
        var result = await client.GetAsync($"{BASEURL}ranking");
        var json = await result.Content.ReadAsStringAsync();

        Log(json);
        return json;
    }
    // user�̃����L���O���擾
    public async Task<string> UserRanking(int id)
    {
        var client = new HttpClient();
        var result = await client.GetAsync($"{BASEURL}users/{id}/ranking");
        var json = await result.Content.ReadAsStringAsync();

        Log(json);
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

    [Serializable]
    class PostUserJson
	{
        public string name;
        public int id;
	}
}
