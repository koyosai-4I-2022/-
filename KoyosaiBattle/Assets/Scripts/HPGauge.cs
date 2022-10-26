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
        UIController.instance.playerData.HitPoint = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        //���݂�HitPoint��slider�ɓK�p
        slider.value = UIController.instance.playerData.HitPoint;
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
