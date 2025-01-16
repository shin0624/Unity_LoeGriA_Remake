using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HobGoblineAttack : MonoBehaviour
{
    // ���� ĳ���� ȩ ����� ���� ��� ��ũ��Ʈ. ȩ ����� �� 3���� ���� ����� �ְ�, count�� ��� �� count���� �ٸ� ���� ����� ����ǵ��� �Ѵ�.
    [SerializeField] private HobGoblineAttackData [] attackDataArray;//���� �����͸� ������ �迭.
    [SerializeField] private PlayerHP playerHP;
    [SerializeField] private Animator anim;
    private bool isAttacking = false;
    private int attackCounter = 0;//���� ī��Ʈ.
    
    private void Init()
    {
        Managers mg = FindAnyObjectByType<Managers>();
        if(playerHP==null)
        {
            playerHP = mg.GetComponent<PlayerHP>();
        }
        if(anim==null)
        {
            anim = GetComponent<Animator>();
        }
        AttackDataArrayNullCheck();
    }
    private void AttackDataArrayNullCheck()
    {
        if(attackDataArray.Length==0)
        {
            Debug.LogError("HobGobline Attack Data Array is NULL");
        }
    }

    void Start()
    {
        Init();
    }

    public void PerformAttack()
    {
        if(isAttacking)
        {
            Debug.LogWarning("Attack is already in progress!");
            return;
        }
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        if(attackDataArray.Length==0)
        {
            isAttacking = false;
            Debug.LogError("HobGobline Attack Data Array is NULL");
            yield break;
        }
        HobGoblineAttackData attackData = attackDataArray[attackCounter];//ī��Ʈ�� �ε����� �Ͽ� ȩ ��� ���� �迭�� ����
        anim.SetBool(attackData.hobGoblineAttackName, true);//�ε����� �´� ������ �ִϸ����Ϳ��� ����
        if(playerHP!=null)
        {
            playerHP.TakeDamage(attackData.hobGoblineBaseAttackDamage);//�÷��̾� hp�� �����ؼ� ��ũ���ͺ� ������Ʈ�� �ۼ��� �������� ����
            Debug.Log($"now count : {attackCounter}, now attack : {attackData.hobGoblineAttackName}");
        }
        else
        {
            Debug.LogError("PlayerHP is NULL");
        }
        yield return null;//�ִϸ��̼� ���� ������ ���� �� ������ ���

        float attackAnimLength = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(attackAnimLength);
        anim.SetBool(attackData.hobGoblineAttackName, false);//���� ���� ó��

        attackCounter++;//���� ī��Ʈ ���� 
        if(attackCounter >=attackDataArray.Length)//ī��Ʈ�� ���� ��� ������ŭ ���Ҵٸ� �ʱ�ȭ �� �ٽ� ����.
        {
            attackCounter = 0;//���� a -> ���� b -> ���� c -> ���� a...�� �ݺ��� ��.
        }
        isAttacking = false;
    }
    
    public void ResetAttackAnimation(string attackName)//���� �ִϸ��̼��� ������ ȣ���ؼ� ���� ó��
    {
        anim.SetBool(attackName, false);
    }
}
