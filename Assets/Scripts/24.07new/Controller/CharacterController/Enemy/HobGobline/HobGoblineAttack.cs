using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HobGoblineAttack : MonoBehaviour
{
    // 보스 캐릭터 홉 고블린의 공격 명령 스크립트. 홉 고블린은 총 3개의 공격 모션이 있고, count를 세어서 각 count마다 다른 공격 모션이 수행되도록 한다.
    [SerializeField] private HobGoblineAttackData [] attackDataArray;//공격 데이터를 저장할 배열.
    [SerializeField] private PlayerHP playerHP;
    [SerializeField] private Animator anim;
    private bool isAttacking = false;
    private int attackCounter = 0;//공격 카운트.
    
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
        HobGoblineAttackData attackData = attackDataArray[attackCounter];//카운트를 인덱스로 하여 홉 고블린 공격 배열에 접근
        anim.SetBool(attackData.hobGoblineAttackName, true);//인덱스에 맞는 공격을 애니메이터에서 설정
        if(playerHP!=null)
        {
            playerHP.TakeDamage(attackData.hobGoblineBaseAttackDamage);//플레이어 hp에 접근해서 스크립터블 오브젝트에 작성된 데미지를 전달
            Debug.Log($"now count : {attackCounter}, now attack : {attackData.hobGoblineAttackName}");
        }
        else
        {
            Debug.LogError("PlayerHP is NULL");
        }
        yield return null;//애니메이션 상태 갱신을 위해 한 프레임 대기

        float attackAnimLength = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(attackAnimLength);
        anim.SetBool(attackData.hobGoblineAttackName, false);//공격 종료 처리

        attackCounter++;//공격 카운트 증가 
        if(attackCounter >=attackDataArray.Length)//카운트가 공격 모션 갯수만큼 돌았다면 초기화 후 다시 증가.
        {
            attackCounter = 0;//공격 a -> 공격 b -> 공격 c -> 공격 a...로 반복될 것.
        }
        isAttacking = false;
    }
    
    public void ResetAttackAnimation(string attackName)//공격 애니메이션의 끝에서 호출해서 종료 처리
    {
        anim.SetBool(attackName, false);
    }
}
