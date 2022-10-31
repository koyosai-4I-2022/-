using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //Text�Ŏ��Ԃ�\��
    [SerializeField]
    Text text;

    //1�b���͂��邽��
    private float timer;
    
    //���Ԍo�߂̃J�E���g
    [SerializeField]
    private int count;

    //instance�ŎQ�Ƃ��邽��
    public static Timer instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //������
        InitializeTimer();
    }

    // Update is called once per frame
    void Update()
    {
        //Playing��ԂłȂ����A���s���Ȃ�
        if (UIController.instance.state != UIController.PlayState.Playing)
        {
            return;
        }

        //1�b��1���J�E���g������
        timer += Time.deltaTime;
        if (timer >= 1 && count > 0)
        {
            count--;
            timer = 0;
        }
        //Text��count��\������
        text.text = string.Format("{0:00}",count) ; 
    }

    public void InitializeTimer()
    {
        //������
        timer = 0;
        count = 180;
    }
}
