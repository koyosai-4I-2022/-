using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGauge : MonoBehaviour
{
    //SerializeField��Inspector����Slider�𑀍�
    [SerializeField]
    Slider slider;

    //Slider��MaxValue�Ɠ����ɂ���
     public int maxHp = 100;

    //���Ԍo�߂�\���ϐ�
    private float timer;

    //�֐��̎Q��
    public static HPGauge instance;
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
        //Slider��Value��������
        slider.value = maxHp;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Space����������_���[�W���󂯂�HP������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Damage(10);
        }

        //1�b��1����HP������
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            Damage(1);
            timer = 0;
        }
    }

    //��_���[�W�֐�
    public void Damage(int damage)
    {
        if(slider.value > 0)
            slider.value -= damage;
    }
}
