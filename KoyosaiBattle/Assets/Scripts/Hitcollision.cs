using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitcollision : MonoBehaviour
{
    [SerializeField]
    [Tooltip("EF_HIT_M_null")]
    private ParticleSystem particle;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("swordcolor002"))
        {
            //if (Attack.instance.replicator.isLocal)
            //return;
            UIController.instance.playerData.HitPoint -= 2;
            // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�����B
            ParticleSystem newParticle = Instantiate(particle);
            //�@�p�[�e�B�N�������ꏊ���擾���A���̈ʒu�ɐ�������B
            Vector3 hitPos = collision.ClosestPointOnBounds(this.transform.position);
            newParticle.transform.position = hitPos;
            //�@�p�[�e�B�N���̔���
            newParticle.Play();
            //�@�C���X�^���X�������p�[�e�B�N���̏���
            Destroy(newParticle.gameObject, 1.0f);
        } 
    }
}