using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGauge : MonoBehaviour
{
    //SerializeField��Inspector����Slider�𑀍�
    [SerializeField]
    Slider energy;

    //Slider��MaxValue�Ɠ����ɂ���
    public int maxEnergy = 200;

    //���Ԍo�߂�\���ϐ�
    private float timer;

    //1�b�Ԃ̃G�l���M�[�����x
    public int LossPerSec;

    //�֐��̎Q��
    public static EnergyGauge instance;
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
        energy.value = maxEnergy;

        LossPerSec = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            EnergyLossPerSec(10);
        }

        if (Input.GetKeyUp(KeyCode.Space))
            EnergyLossPerSec(1);

        /*
        if (PlayerMotion.instance.run)
        {
            EnergyLossPerSec(5);
        }
        */

        //Energy�����镔��
        timer += Time.deltaTime;
        if (timer * LossPerSec >= 1)
        {
            energy.value -= 1;
            timer = 0;
        }
    }

    public void EnergyLossPerSec (int i)
    {
        if (energy.value > 0)
        {
            LossPerSec = i;
        }
    }
}
