using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    //JoyconLibの変数
    private static readonly Joycon.Button[] m_buttons =
       Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;
    
    //Animatorの変数
    public Animator Animator;
    public bool shoot;

    //スティックのsetting
    private float degree;
    CameraMotion camdeg = new CameraMotion();
    //CameraMotion camdeg;
    public bool isCalledOnce;

    // Start is called before the first frame update
    void Start()
    {
        //JoyconLibの初期化
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) return;
        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);

        //Animatorの初期化
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

        //Animatorがshootの状態の時
        if (shoot == true)
        {
            //右スティックでPlayerの向きを変える
            transform.Rotate(new Vector3(0, Rstick[0] * 3, 0));

            //左スティックの倒した向きから角度を得る
            degree = Mathf.Atan2(Lstick[0], Lstick[1]);

            //確認用(カメラを後ろに置いた場合)
            if (isCalledOnce == false)
            {
                isCalledOnce = true;
                Vector3 ObjPos = transform.position;
                Camera.main.transform.RotateAround(ObjPos, Vector3.up, transform.localEulerAngles.y - camdeg.getCamera_deg());
                Debug.Log("オブジェクトの向きにカメラ移動");
            }
            

            //Walk Shoot Front
            if (Lstick[0] != 0 || Lstick[1] != 0)//スティックを倒しているとき
            {
                //前方への移動
                if (degree * 180 / Mathf.PI > -40 && degree * 180 / Mathf.PI < 40)
                {
                    //前に走る(ZRボタンを押している場合)
                    if (m_joyconL.GetButton(m_buttons[12]))
                    {
                        Vector3 vector = new Vector3(0, 0, 1);
                        transform.Translate(vector * Time.deltaTime * 3);
                        Animator.Play("Run_gunMiddle_AR");
                    }
                    //前に歩く
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

            //左スティックを倒しているとき
            if (Lstick[0] != 0 || Lstick[1] != 0)
            {
                //Playerがスティックを倒した方向に進む
                Vector3 vector = new Vector3(Mathf.Sin(degree), 0, Mathf.Cos(degree));
                transform.Translate(vector * Time.deltaTime * 3);
            }
        }
    }
}
