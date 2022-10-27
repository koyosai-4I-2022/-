using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;

public class Attack : MonoBehaviour
{
    // �����p
    [SerializeField]
    public StrixReplicator replicator;

    public static Attack instance;

    //JoyconLib�̕ϐ�
    private static readonly Joycon.Button[] m_buttons =
       Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    //private Joycon m_joyconL;
    private Joycon m_joyconR;

    //Animator�̕ϐ�
    public Animator animator;
    public int para = 0;
    private Vector3 v1 = new Vector3(0.9f, 0.5f, 0.9f);
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
        /*
        // �������ꂽ�Q�[���I�u�W�F�N�g�̏ꍇ�������s��Ȃ�
        if(!replicator.isLocal)
            return;
        */

        if (m_joycons == null || m_joycons.Count <= 0) return;

        

        //�R���[�`���Ŏ�t���Ԃ𐧌�����
        tmpTime += Time.deltaTime;

        if (tmpTime >= interval)
        {
            tmpTime = 0f;

            foreach (var joycon in m_joycons)
            {
                accel = joycon.GetAccel();
            }

            var m = accel.sqrMagnitude;
            if(m > 2f) Debug.Log($"{accel}:{m}");
            accelBefore = accel;

            if (m > 2f)// || accel.z >= v1.z)
            {
                /*if (para <= 5)
                {
                    para++;
                    animator.SetInteger("slash1", para);
                }*/
                slashBool = !slashBool;
                // �璷
                //if (slashBool == false)
                //{
                //    slashBool = !slashBool;
                //}
                //else
                //{
                //    slashBool = !slashBool;
                //}
                m_joyconR.SetRumble(160, 320, 0.6f, 200);
                animator.SetBool("slash2", slashBool);

                
            }
            if (para > 4)
            {
                para = 0;
            }
            
        }
        
        
    }
}