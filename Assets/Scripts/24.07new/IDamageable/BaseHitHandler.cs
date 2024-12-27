using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseHitHandler : MonoBehaviour, IDamageable
{
    //재사용 가능한 추상클래스 베이스 히트 핸들러 생성
    protected Animator animator;
    protected NavMeshAgent agent;
    protected Rigidbody rb;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    public abstract void OnHit(float damage, Vector3 hitPoint, Vector3 hitNormal, float knockBackForce);
    /* 
    IDamageable 인터페이스의 OnHit 메서드를 구현. 파생클래스에서 오버라이드하여 사용
    damage : 플레이어가 적에게 가하는 데미지 값. 체력 감소량을 결정
    hitpoint : 공격이 적중한 정확한 위치. 파티클 효과나 피격 효과를 표시할 위치 지정에 사용. RaycastHit.point 값을 전달받음
    hitNormal : 충돌이 발생한 표면의 법선 벡터. 튕겨나가는 방향이나 이펙트의 방향을 결정할 때 사용. RaycastHit.normal 값을 전달받음
    knocBackForce : 피격 시 밀려나가는 힘의 크기. 거리에 따라 감소하는 넉백 효과 적용 등에 사용.
    
    */
}
