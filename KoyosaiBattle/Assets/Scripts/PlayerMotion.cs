using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    //JoyconLib�̕ϐ�
    private static readonly Joycon.Button[] m_buttons =
       Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;
    
    //Animator�̕ϐ�
    public Animator Animator;
    
    //�X�e�B�b�N��setting
    private float degree;

    //gurd��run�̏�Ԃ�\���ϐ�
    public bool guard;
    public bool run;

    //�֐��̎Q��
    public static PlayerMotion instance;
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
        //JoyconLib�̏�����
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) return;
        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);

        //Animator�̏�����
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //JoyconLib
        if (m_joycons == null || m_joycons.Count <= 0) return;
        float[] Lstick = m_joyconL.GetStick();
        float[] Rstick = m_joyconR.GetStick();

        //�G�l���M�[��3�ȏ゠��Ƃ�
        if (EnergyGauge.instance.CanUse(3))
        {
            //ZL�{�^�����������Ƃ�
            if (m_joyconL.GetButtonDown(m_buttons[12]))
            {
                guard = true;
                //�G�l���M�[��3�g�p����
                EnergyGauge.instance.EnergyLoss(3);
            }
        }

        //�K�[�h���(ZL�{�^���������Ă���ꍇ)
        if (guard)
        {
            //�V�[���h�̕\��
            ShieldDisplay.instance.Create();
        }

        //ZL�{�^���𗣂����Ƃ�
        if (m_joyconL.GetButtonUp(m_buttons[12]))
        {
            ShieldDisplay.instance.Destroy();
            guard = false;
        }

        //�G�l���M�[��2�ȏ゠��Ƃ�
        if (EnergyGauge.instance.CanUse(2))
        {
            //L�{�^�����������Ƃ�
            if (m_joyconL.GetButtonDown(m_buttons[11]))
            {
                run = true;
                //�G�l���M�[��2�g�p����
                EnergyGauge.instance.EnergyLoss(2);
            }
        }

        //L�{�^���𗣂����Ƃ�
        if (m_joyconL.GetButtonUp(m_buttons[11]))
        {
            run = false;
        }

        //�E�X�e�B�b�N��Player�̌�����ς���
        transform.Rotate(new Vector3(0, Rstick[0] * 3, 0));

        //���X�e�B�b�N�̓|������������p�x�𓾂�
        degree = Mathf.Atan2(Lstick[0], Lstick[1]);

        //�K�[�h���ĂȂ���Ԃ̎�
        if (!guard)
        {
            //Walk Shoot Front
            if (Lstick[0] != 0 || Lstick[1] != 0)//�X�e�B�b�N��|���Ă���Ƃ�
            {
                //�O���ւ̈ړ�
                if (degree * 180 / Mathf.PI > -15 && degree * 180 / Mathf.PI < 15)
                {
                    //�O�ɑ���(L�{�^���������Ă���ꍇ)
                    if (m_joyconL.GetButton(m_buttons[11]))
                    {
                        Vector3 vector = new Vector3(0, 0, 1);
                        //transform.Translate(vector * Time.deltaTime * 3);
                        Animator.Play("run1");
                        //�G�l���M�[�g�p����+1����
                        //EnergyGauge.instance.EnergyLossPerSec(en + 1);
                        //run = true;
                    }
                    //�O�ɕ���
                    else Animator.Play("walk1");
                }

            }

            //�΂ߍ��O
            if (degree * 180 / Mathf.PI <= -15 && degree * 180 / Mathf.PI >= -75)
            {
                Animator.Play("Jog Forward Diagonal");
            }

            //Walk Left
            if (degree * 180 / Mathf.PI < -75 && degree * 180 / Mathf.PI > -105)
            {
                Animator.Play("strafe2");
            }

            //�΂ߍ���
            if (degree * 180 / Mathf.PI <= -105 && degree * 180 / Mathf.PI >= -165)
            {
                Animator.Play("Jog Backward Diagonal");
            }

            //Walk Back
            if (degree * 180 / Mathf.PI < -165 || degree * 180 / Mathf.PI > 165)
            {
                Animator.Play("walk2");
            }

            //�΂߉E��
            if (degree * 180 / Mathf.PI >= 105 && degree * 180 / Mathf.PI <= 165)
            {
                Animator.Play("Jog Backward Diagonal (1)");
            }

            //Walk Right
            if (degree * 180 / Mathf.PI > 75 && degree * 180 / Mathf.PI < 105)
            {
                Animator.Play("strafe1");
            }

            //�΂߉E�O
            if (degree * 180 / Mathf.PI >= 15 && degree * 180 / Mathf.PI <= 75)
            {
                Animator.Play("Jog Forward Diagonal (1)");
            }

            //���X�e�B�b�N��|���Ă���Ƃ�
            if (Lstick[0] != 0 || Lstick[1] != 0)
            {
                if (run)
                {
                    //Player���X�e�B�b�N��|���������ɐi��
                    Vector3 Runvector = new Vector3(Mathf.Sin(degree), 0, Mathf.Cos(degree));
                    transform.Translate(Runvector * Time.deltaTime * 30);
                }
                //Player���X�e�B�b�N��|���������ɐi��
                Vector3 vector = new Vector3(Mathf.Sin(degree), 0, Mathf.Cos(degree));
                transform.Translate(vector * Time.deltaTime * 5);
            }
        }
    }
}
