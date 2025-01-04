using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblineAttack : MonoBehaviour
{
    //고블린 에너미의 공격 연산을 담당하는 스크립트. 고블린은 가장 기본 에너미라서 기본 공격명, 기본 공격데미지만 가지고 있다.
    [SerializeField] private GoblineAttackData goblineAttackData;//스크립터블 오브젝트 스크립트를 참조
    [SerializeField] private PlayerHP playerHP;//플레이어 체력을 참조
    public GoblineAttack(GoblineAttackData data)//초기화용 생성자
    {
        goblineAttackData = data;
    }
    public void PerformAttack(GoblineAttackData damageData )
    {
        playerHP.TakeDamage(damageData.goblineAttackDamage);//플레이어에게 데미지를 입힌다.
    }
}
