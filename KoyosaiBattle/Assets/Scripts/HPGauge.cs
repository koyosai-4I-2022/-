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
     public int maxHp = 400;

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
        InitializeHPGauge();
        slider.maxValue = maxHp;
        UIController.instance.playerData.HitPoint = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        //���݂�HitPoint��slider�ɓK�p
        slider.value = UIController.instance.playerData.HitPoint;
    }

    public void InitializeHPGauge()
    {
        //Slider��Value��������
        slider.value = maxHp;
    }

    //��_���[�W�֐�
    /*public void Damage()
    {
        if (slider.value > 0)
        {
            UIController.instance.playerData.HitPoint -= 10;
            slider.value = UIController.instance.playerData.HitPoint;
        }
    }*/
}
