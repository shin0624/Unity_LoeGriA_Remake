using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IPlayerAttack
{
    /*�÷��̾��� ���� ���� ������ ����. �Է��� �ް� ������ �����ϴ� Ŭ������ PlayerController�̸�, �� Ŭ������ �����Ͽ� ���� ������ ����.
    PlayerController�� ���� �ڷ�ƾ, ��ƼŬ ����, ���� ���� �� ���� ���� ���(�޼���, ����, �ڷ�ƾ ��)�� ��� ������. */

    //-----------�÷��̾� ���� ���� ����-------------
    [SerializeField] private Animator anim;
    public bool IsAttack{get; private set;}
    //-----------�޺� ���� ���� ����-------------
    private int comboStep = 0;//�޺��� �̾����� Ƚ��
    private float comboTimer = 0.0f;//�޺� ���踦 ���� �ð��� ��� Ÿ�̸�
    public float comboResetTime = 1.0f;//�޺� ���� ���� Ÿ�̸�
    [SerializeField] private int maxComboStep;//�ִ� �޺� Ƚ��
    
    //-----------�⺻ ���� ���� ����-------------
    public float baseDamage = 10.0f;// �⺻ ���� ������
    [SerializeField] private float baseKnockBackForce = 500.0f; // �˹� ��
    [SerializeField] private float attackRange = 5.0f; // ���� ����
    [SerializeField] private LayerMask enemyLayer; // �� ���̾�
    [SerializeField] private float attackDelay = 0.5f; // ���� ������

    //-----------��ų 1 ���� ���� ����-------------
    public float skillDamage01 = 20.0f;// ��ų 1 ���� ������
    [SerializeField] private float KnockBackForce01 = 1000.0f; // ��ų 1 �˹� ��

    //-----------��ƼŬ �� ����Ʈ ���� ������-------------
    [SerializeField] private PlayerAttackParticleManager particleManager;//�÷��̾� ���� ����Ʈ �Ŵ���

    public void PerformAttack(int attackNumber)
    {
        if(IsAttack) return;//���� �ִϸ��̼��� ���� ���̸� �ߺ� ������ ����. ���� �ϳ��� ���� �� ���� �������� �̾����� �ϱ� ����.
        switch(attackNumber)
        {
            case 0 : StartCoroutine(AttackRoutine(attackNumber, baseKnockBackForce, baseDamage)); break;//�⺻ ���� �ڷ�ƾ ����
            case 1 : StartCoroutine(AttackRoutine(attackNumber, KnockBackForce01, skillDamage01)); break; // ��ų 1 ���� �ڷ�ƾ ����
            case 2 : StartCoroutine(AttackRoutine(attackNumber, 800, 15)); break;//�⺻ �޺� ���� �ڷ�ƾ ����
        }
    }

    private IEnumerator AttackRoutine(int attackNumber, float knockBackForce, float damage)// Attack�޼��带 �ڷ�ƾ���� ����. �ִϸ��̼� Ŭ���� ���� ���� ���� �� ���� �ؼ�
    {
        anim.SetBool("IsAttack", true);
        CallAttackParticle(attackNumber, transform.position, transform.rotation);//���� ��ƼŬ ȣ��
        yield return new WaitForSeconds(attackDelay);//���� ������ ������ ��ŭ ��� �� ���.

        //���� ���� ����
        Vector3 rayOrigin = transform.position + Vector3.up * 1.5f;
        Vector3 rayDirection = transform.forward;
        float rayRadius = 1.0f;//��ü ����ĳ��Ʈ�� ������

        //��ü ����ĳ��Ʈ�� ����Ͽ� ���� ���� ����. ���� ����ĳ��Ʈ���� �� �ڿ������� ���� ���� ����
        RaycastHit[] hits = Physics.SphereCastAll(rayOrigin, rayRadius, rayDirection, attackRange, enemyLayer);// ���ʹ� ���̾ ���� �����ؼ� ���ʹ̿��Ը� ȿ���� ���������� ��.
        
        //����� �ð�ȭ
        DrawRayLine(rayOrigin, rayDirection, attackRange, rayRadius);

        foreach(RaycastHit hit in hits)
        {
            if(hit.collider.CompareTag("Enemy"))
            {
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();// ray�� ���� �ݶ��̴��� ���� ���ʹ��� idamageable �������̽��� ã�Ƽ� OnHit �޼��带 ȣ��
                if(damageable != null)
                {
                    //�Ÿ��� ���� �˹� �� ����
                      float distanceMultiplier = 1 - (hit.distance / attackRange);
                      float finalKnockbackForce = knockBackForce * distanceMultiplier; // Ÿ�� �������� �� ���� �˹� ȿ���� ������

                    damageable.OnHit(damage, hit.point, hit.normal, finalKnockbackForce);//10�� ������, ������ ������ ��ġ, �浹 ǥ���� ��������, �з������� ���� ũ�⸦ �Ű������� ����.     
                }
            }
        }
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);//�ִϸ��̼� Ŭ�� ���̸�ŭ ���
        ResetAttack();//���� �ִϸ��̼� ����

    }

    private void CallAttackParticle(int attackNumber, Vector3 particlePosition, Quaternion particleRotation)//���� ��ƼŬ ȣ�� �޼���
    {
        particlePosition = transform.position + transform.up * 1.5f; //��ƼŬ ��ġ�� �� ��ġ��
        particleRotation = transform.rotation;
        particleManager.PlayAttackParticle(attackNumber, particlePosition, particleRotation);//���� ��ƼŬ ���
    }

    private void DrawRayLine(Vector3 rayOrigin, Vector3 rayDirection, float attackRange, float rayRadius)// ����� �ð�ȭ �޼���
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
        anim.SetBool("IsAttack", false); // ���� �ִϸ��̼� ����
    }

}
