using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseHitHandler : MonoBehaviour, IDamageable
{
    //���� ������ �߻�Ŭ���� ���̽� ��Ʈ �ڵ鷯 ����
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
    IDamageable �������̽��� OnHit �޼��带 ����. �Ļ�Ŭ�������� �������̵��Ͽ� ���
    damage : �÷��̾ ������ ���ϴ� ������ ��. ü�� ���ҷ��� ����
    hitpoint : ������ ������ ��Ȯ�� ��ġ. ��ƼŬ ȿ���� �ǰ� ȿ���� ǥ���� ��ġ ������ ���. RaycastHit.point ���� ���޹���
    hitNormal : �浹�� �߻��� ǥ���� ���� ����. ƨ�ܳ����� �����̳� ����Ʈ�� ������ ������ �� ���. RaycastHit.normal ���� ���޹���
    knocBackForce : �ǰ� �� �з������� ���� ũ��. �Ÿ��� ���� �����ϴ� �˹� ȿ�� ���� � ���.
    
    */
}
