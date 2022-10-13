using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //JoyconLib�̕ϐ�
    private static readonly Joycon.Button[] m_buttons =
       Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    //private Joycon m_joyconL;
    private Joycon m_joyconR;

    //Animator�̕ϐ�
    public Animator animator;
    public int para = 0;
    private Vector3 v1 = new Vector3(0.9f, 0.9f, 0.9f);
    private Vector3 accel, accelBefore;
    bool slashBool = false;

    //�R���[�`���Ŏ��Ԑ��䂷��
    private float interval = 1.5f;
    private float tmpTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        //JoyconLib�̏�����
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) return;
        //m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);

        //Animator�̏�����
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_joycons == null || m_joycons.Count <= 0) return;

        

        //�R���[�`���Ŏ�t���Ԃ𐧌�����
        tmpTime += Time.deltaTime;

        if (tmpTime >= interval)
        {

            foreach (var joycon in m_joycons)
            {
                accel = joycon.GetAccel();
            }

            accelBefore = accel;

            if (accel.x >= v1.x || accel.y >= v1.y)// || accel.z >= v1.z)
            {
                /*if (para <= 5)
                {
                    para++;
                    animator.SetInteger("slash1", para);
                }*/
                if (slashBool == false)
                {
                    slashBool = true;
                }
                else
                {
                    slashBool = false;
                }
                animator.SetBool("slash2", slashBool);

                slashBool = false;
            }
            if (para > 4)
            {
                para = 0;
            }
            
        }
        
        
    }
}