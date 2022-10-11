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
    public bool shoot;

    //�X�e�B�b�N��setting
    private float degree;
    CameraMotion camdeg = new CameraMotion();
    //CameraMotion camdeg;
    public bool isCalledOnce;

    // Start is called before the first frame update
    void Start()
    {
        //JoyconLib�̏�����
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) return;
        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);

        //Animator�̏�����
        shoot = true;
        Animator = GetComponent<Animator>();
        Animator.SetBool("shoot", shoot);

    }

    // Update is called once per frame
    void Update()
    {
        //JoyconLib
        if (m_joycons == null || m_joycons.Count <= 0) return;
        float[] Lstick = m_joyconL.GetStick();
        float[] Rstick = m_joyconR.GetStick();

        //Animator��shoot�̏�Ԃ̎�
        if (shoot == true)
        {
            //�E�X�e�B�b�N��Player�̌�����ς���
            transform.Rotate(new Vector3(0, Rstick[0] * 3, 0));

            //���X�e�B�b�N�̓|������������p�x�𓾂�
            degree = Mathf.Atan2(Lstick[0], Lstick[1]);

            //�m�F�p(�J���������ɒu�����ꍇ)
            if (isCalledOnce == false)
            {
                isCalledOnce = true;
                Vector3 ObjPos = transform.position;
                Camera.main.transform.RotateAround(ObjPos, Vector3.up, transform.localEulerAngles.y - camdeg.getCamera_deg());
                Debug.Log("�I�u�W�F�N�g�̌����ɃJ�����ړ�");
            }
            

            //Walk Shoot Front
            if (Lstick[0] != 0 || Lstick[1] != 0)//�X�e�B�b�N��|���Ă���Ƃ�
            {
                //�O���ւ̈ړ�
                if (degree * 180 / Mathf.PI > -40 && degree * 180 / Mathf.PI < 40)
                {
                    //�O�ɑ���(ZR�{�^���������Ă���ꍇ)
                    if (m_joyconL.GetButton(m_buttons[12]))
                    {
                        Vector3 vector = new Vector3(0, 0, 1);
                        transform.Translate(vector * Time.deltaTime * 3);
                        Animator.Play("Run_gunMiddle_AR");
                    }
                    //�O�ɕ���
                    else Animator.Play("WalkFront_Shoot_AR");
                }

            }
            //Walk Shoot Back
            if (degree * 180 / Mathf.PI < -140 || degree * 180 / Mathf.PI > 140)
            {
                Animator.Play("WalkBack_Shoot_AR");
            }

            //Walk Shoot Left
            if (degree * 180 / Mathf.PI >= -140 && degree * 180 / Mathf.PI <= -40)
            {
                Animator.Play("WalkLeft_Shoot_AR");
            }

            //Walk Shoot Right
            if (degree * 180 / Mathf.PI >= 40 && degree * 180 / Mathf.PI <= 140)
            {
                Animator.Play("WalkRight_Shoot_AR");
            }

            //���X�e�B�b�N��|���Ă���Ƃ�
            if (Lstick[0] != 0 || Lstick[1] != 0)
            {
                //Player���X�e�B�b�N��|���������ɐi��
                Vector3 vector = new Vector3(Mathf.Sin(degree), 0, Mathf.Cos(degree));
                transform.Translate(vector * Time.deltaTime * 3);
            }
        }
    }
}