using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackParticleManager : MonoBehaviour
{
    [SerializeField] private ParticlePool attackParticlePool;//공격 파티클 풀
    [SerializeField] private ParticlePool hitParticlePool;//히트 파티클 풀
    [SerializeField] private ParticlePool skillParticlePool01;//스킬1 파티클 풀
    private float StartDelay;//파티클 시스템 시작 딜레이. 공격 애니메이션 종료시간과 동기화.
    private ParticlePool pool;

    public void PlayAttackParticle(int attackNumber, Vector3 position, Quaternion rotation)//공격 시 파티클 재생
    {
        switch(attackNumber)
        {
            case 0 : pool = attackParticlePool;StartDelay = 0.66f; break;//기본 공격일 경우
            case 1 : pool = skillParticlePool01; StartDelay = 1.18f; break;//스킬1 공격일 경우
        }

       if (pool != null)//파티클 풀이 존재하면
        {
            GameObject particle = pool.GetParticle();//파티클 오브젝트 풀에서 파티클 가져오기
            particle.transform.position = position;//위치 설정
            particle.transform.rotation = rotation;//회전 설정

            ParticleSystem ps = particle.GetComponent<ParticleSystem>();//파티클 시스템 컴포넌트 가져오기
            if (ps != null)//파티클 시스템이 존재하면
            {
                var particleMainModule = ps.main;
                particleMainModule.startDelay = StartDelay;//시작 딜레이 설정. 파티클 시스템이 나타나는 시간을 공격 애니메이션 종료 시간과 동기화
                ps.Play();//파티클 재생
                StartCoroutine(ReturnToPoolAfterDuration(particle, ps.main.duration + ps.main.startLifetime.constant));//파티클 재생 시간 후 풀로 반환
            }
        }
    }

    public void PlayHitParticle(Vector3 hitPoint, Vector3 hitNormal)// 피격 시 파티클 재생 
    {
        if (hitParticlePool != null)//히트 파티클 풀이 존재하면
        {
            GameObject particle = hitParticlePool.GetParticle();//파티클 오브젝트 풀에서 파티클 가져오기
            particle.transform.position = hitPoint;//위치 설정
            particle.transform.rotation = Quaternion.LookRotation(hitNormal);//회전 설정

            ParticleSystem ps = particle.GetComponent<ParticleSystem>();//파티클 시스템 컴포넌트 가져오기
            if (ps != null)
            {
                ps.Play();
                StartCoroutine(ReturnToPoolAfterDuration(particle, ps.main.duration + ps.main.startLifetime.constant));
            }
        }
    }

    private IEnumerator ReturnToPoolAfterDuration(GameObject particle, float delay)// 지정된 시간이 지난 후 파티클 풀로 반환
    {
        yield return new WaitForSeconds(delay);//지정된 시간만큼 대기
        if (attackParticlePool!=null && particle.name.Contains("Attack"))// 공격 파티클이 존재하면
        {
            attackParticlePool.ReturnParticle(particle);//공격 파티클 풀로 반환
        }
        else if(hitParticlePool!=null) //히트 파티클이 존재하면
        {
            hitParticlePool.ReturnParticle(particle);//히트 파티클 풀로 반환
        }
    }
}
