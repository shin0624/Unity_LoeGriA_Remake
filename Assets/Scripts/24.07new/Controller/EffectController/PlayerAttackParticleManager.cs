using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackParticleManager : MonoBehaviour
{
    [SerializeField] private ParticlePool attackParticlePool;//���� ��ƼŬ Ǯ
    [SerializeField] private ParticlePool hitParticlePool;//��Ʈ ��ƼŬ Ǯ
    [SerializeField] private ParticlePool skillParticlePool01;//��ų1 ��ƼŬ Ǯ
    private float StartDelay;//��ƼŬ �ý��� ���� ������. ���� �ִϸ��̼� ����ð��� ����ȭ.
    private ParticlePool pool;

    public void PlayAttackParticle(int attackNumber, Vector3 position, Quaternion rotation)//���� �� ��ƼŬ ���
    {
        switch(attackNumber)
        {
            case 0 : pool = attackParticlePool;StartDelay = 0.66f; break;//�⺻ ������ ���
            case 1 : pool = skillParticlePool01; StartDelay = 1.18f; break;//��ų1 ������ ���
        }

       if (pool != null)//��ƼŬ Ǯ�� �����ϸ�
        {
            GameObject particle = pool.GetParticle();//��ƼŬ ������Ʈ Ǯ���� ��ƼŬ ��������
            particle.transform.position = position;//��ġ ����
            particle.transform.rotation = rotation;//ȸ�� ����

            ParticleSystem ps = particle.GetComponent<ParticleSystem>();//��ƼŬ �ý��� ������Ʈ ��������
            if (ps != null)//��ƼŬ �ý����� �����ϸ�
            {
                var particleMainModule = ps.main;
                particleMainModule.startDelay = StartDelay;//���� ������ ����. ��ƼŬ �ý����� ��Ÿ���� �ð��� ���� �ִϸ��̼� ���� �ð��� ����ȭ
                ps.Play();//��ƼŬ ���
                StartCoroutine(ReturnToPoolAfterDuration(particle, ps.main.duration + ps.main.startLifetime.constant));//��ƼŬ ��� �ð� �� Ǯ�� ��ȯ
            }
        }
    }

    public void PlayHitParticle(Vector3 hitPoint, Vector3 hitNormal)// �ǰ� �� ��ƼŬ ��� 
    {
        if (hitParticlePool != null)//��Ʈ ��ƼŬ Ǯ�� �����ϸ�
        {
            GameObject particle = hitParticlePool.GetParticle();//��ƼŬ ������Ʈ Ǯ���� ��ƼŬ ��������
            particle.transform.position = hitPoint;//��ġ ����
            particle.transform.rotation = Quaternion.LookRotation(hitNormal);//ȸ�� ����

            ParticleSystem ps = particle.GetComponent<ParticleSystem>();//��ƼŬ �ý��� ������Ʈ ��������
            if (ps != null)
            {
                ps.Play();
                StartCoroutine(ReturnToPoolAfterDuration(particle, ps.main.duration + ps.main.startLifetime.constant));
            }
        }
    }

    private IEnumerator ReturnToPoolAfterDuration(GameObject particle, float delay)// ������ �ð��� ���� �� ��ƼŬ Ǯ�� ��ȯ
    {
        yield return new WaitForSeconds(delay);//������ �ð���ŭ ���
        if (attackParticlePool!=null && particle.name.Contains("Attack"))// ���� ��ƼŬ�� �����ϸ�
        {
            attackParticlePool.ReturnParticle(particle);//���� ��ƼŬ Ǯ�� ��ȯ
        }
        else if(hitParticlePool!=null) //��Ʈ ��ƼŬ�� �����ϸ�
        {
            hitParticlePool.ReturnParticle(particle);//��Ʈ ��ƼŬ Ǯ�� ��ȯ
        }
    }
}
