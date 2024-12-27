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
        if (animator == null)
            Debug.LogError($"{gameObject.name} is missing an Animator component.");

    agent = GetComponent<NavMeshAgent>();
        if (agent == null)
            Debug.LogError($"{gameObject.name} is missing a NavMeshAgent component.");

    rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError($"{gameObject.name} is missing a Rigidbody component.");
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
