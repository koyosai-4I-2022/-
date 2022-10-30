using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGauge : MonoBehaviour
{
    //SerializeField��Inspector����Slider�𑀍�
    [SerializeField]
    Slider energy;

    //�F��ς��邽��
    [SerializeField]
    Image sliderImage;

    //Slider��MaxValue�Ɠ����ɂ���
    public int maxEnergy = 10;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerMotion.instance.guard)
        {
            //Energy���������镔��
            if (maxEnergy > energy.value)
                energy.value += Time.deltaTime;
        }
        
        //�Q�[�W�̐F��ς���
        if (energy.value >= 5)//�G�l���M�[��5�ȏ�̎�
            sliderImage.color = new Color32(253, 244, 1, 255);//���F
        else sliderImage.color = new Color32(253, 161, 1, 255);//�I�����W
    }

    //�G�l���M�[����֐�
    public void EnergyLoss (int i)
    {
        if (CanUse(i))
        {
            energy.value  -= i;
        }
    }

    //�G�l���M�[�������ȏ゠�邩�𔻒f����֐�
    public bool CanUse(int k)
    {
        if (energy.value - k >= 0)
            return true;
        else return false;
    }
}
