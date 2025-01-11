using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblineAttack : MonoBehaviour
{
    //고블린 에너미의 공격 명령을 하는 스크립트. 고블린은 가장 기본 에너미라서 기본 공격명, 기본 공격데미지만 가지고 있다.
    [SerializeField] private GoblineAttackData goblineAttackData;//스크립터블 오브젝트 스크립트를 참조
    [SerializeField] private PlayerHP playerHP;
    void Start() 
    {
      Managers mg = FindAnyObjectByType<Managers>();
      playerHP = mg.GetComponent<PlayerHP>();  
    }
    
    public void PerformAttack(GoblineAttackData damageData )
    {
        if(playerHP!=null)
        {
            playerHP.TakeDamage(damageData.goblineAttackDamage);//플레이어에게 데미지를 입힌다.
        }
        else
        {
            Debug.LogError("PlayerHP is null");
        }
        
    }
}
