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

    float time = 0;

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

    }

    //��_���[�W�֐�
    void Damage(int damage)
    {
        if(slider.value > 0)
            slider.value -= damage;
    }
}
