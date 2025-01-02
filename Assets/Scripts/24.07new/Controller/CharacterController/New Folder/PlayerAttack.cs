using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IPlayerAttack
{
    /*플레이어의 공격 관련 로직을 관리. 입력을 받고 공격을 실행하는 클래스는 PlayerController이며, 두 클래스가 연동하여 공격 동작을 실행.
    PlayerController의 공격 코루틴, 파티클 생성, 리셋 어택 등 공격 관련 기능(메서드, 변수, 코루틴 등)을 모두 가져옴. */

    //-----------플레이어 공격 관련 변수-------------
    [SerializeField] private Animator anim;
    public bool IsAttack{get; private set;}
    //-----------공격 연계 관련 변수-------------
    private int comboStep = 0;//콤보가 이어지는 횟수
    private float comboTimer = 0.0f;//콤보 연계를 위해 시간을 재는 타이머
    public float comboResetTime = 1.0f;//콤보 연계 종료 타이머
    
    //-----------기본 공격 관련 변수-------------
    public float baseDamage = 10.0f;// 기본 공격 데미지
    [SerializeField] private float attackRange = 5.0f; // 공격 범위
    [SerializeField] private float knockBackForce = 500.0f; // 넉백 힘
    [SerializeField] private LayerMask enemyLayer; // 적 레이어
    [SerializeField] private float attackDelay = 0.5f; // 공격 딜레이

    //-----------파티클 등 이펙트 관련 변수들-------------
    [SerializeField] private PlayerAttackParticleManager particleManager;//플레이어 공격 이펙트 매니저

    public void PerformAttack()
    {
        if(IsAttack) return;//공격 애니메이션이 진행 중이면 중복 공격을 방지. 공격 하나가 끝난 후 다음 공격으로 이어져야 하기 때문.
        StartCoroutine(AttackRoutine());//공격 코루틴 실행
    
    }

    private IEnumerator AttackRoutine()// Attack메서드를 코루틴으로 변경. 애니메이션 클립과 실제 공격 판정 간 간극 해소
    {
        anim.SetBool("IsAttack", true);
        CallAttackParticle(transform.position, transform.rotation);//공격 파티클 호출
        yield return new WaitForSeconds(attackDelay);//공격 딜레이 변수값 만큼 대기 후 재생.

        //실제 공격 판정
        Vector3 rayOrigin = transform.position + Vector3.up * 1.5f;
        Vector3 rayDirection = transform.forward;
        float rayRadius = 1.0f;//구체 레이캐스트의 반지름

        //구체 레이캐스트를 사용하여 넓은 범위 감지. 단일 레이캐스트보다 더 자연스러운 무기 판정 가능
        RaycastHit[] hits = Physics.SphereCastAll(rayOrigin, rayRadius, rayDirection, attackRange, enemyLayer);// 에너미 레이어를 새로 설정해서 에너미에게만 효과가 가해지도록 함.
        
        //디버그 시각화
        DrawRayLine(rayOrigin, rayDirection, attackRange, rayRadius);

        foreach(RaycastHit hit in hits)
        {
            if(hit.collider.CompareTag("Enemy"))
            {
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();// ray가 닿은 콜라이더를 가진 에너미의 idamageable 인터페이스를 찾아서 OnHit 메서드를 호출
                if(damageable != null)
                {
                    //거리에 따른 넉백 힘 감소
                      float distanceMultiplier = 1 - (hit.distance / attackRange);
                      float finalKnockbackForce = knockBackForce * distanceMultiplier; // 타격 지점에서 멀 수록 넉백 효과가 약해짐

                    damageable.OnHit(baseDamage, hit.point, hit.normal, finalKnockbackForce);//10의 데미지, 공격이 적중한 위치, 충돌 표면의 법선벡터, 밀려나가는 힘의 크기를 매개변수로 전달.     
                }
            }
        }
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);//애니메이션 클립 길이만큼 대기
        ResetAttack();//공격 애니메이션 리셋

    }

    private void CallAttackParticle(Vector3 particlePosition, Quaternion particleRotation)//공격 파티클 호출 메서드
    {
        particlePosition = transform.position + transform.forward * 1.5f; //파티클 위치를 검 위치로
        particleRotation = transform.rotation;
        particleManager.PlayAttackParticle(particlePosition, particleRotation);//공격 파티클 재생
    }

    private void DrawRayLine(Vector3 rayOrigin, Vector3 rayDirection, float attackRange, float rayRadius)// 디버그 시각화 메서드
    {
        Debug.DrawRay(rayOrigin, rayDirection * attackRange, Color.red, 3.0f);
        Debug.DrawLine(
            rayOrigin + rayDirection * attackRange + Vector3.up * rayRadius,
            rayOrigin + rayDirection * attackRange - Vector3.up * rayRadius,
            Color.blue,
            3.0f
        );
    }

    private void ResetAttack()
    {
        anim.SetBool("IsAttack", false); // 공격 애니메이션 리셋
    }
    


}
