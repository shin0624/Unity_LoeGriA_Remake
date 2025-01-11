using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblineAttack : MonoBehaviour
{
    //��� ���ʹ��� ���� ����� �ϴ� ��ũ��Ʈ. ����� ���� �⺻ ���ʹ̶� �⺻ ���ݸ�, �⺻ ���ݵ������� ������ �ִ�.
    [SerializeField] private GoblineAttackData goblineAttackData;//��ũ���ͺ� ������Ʈ ��ũ��Ʈ�� ����
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
            playerHP.TakeDamage(damageData.goblineAttackDamage);//�÷��̾�� �������� ������.
        }
        else
        {
            Debug.LogError("PlayerHP is null");
        }
        
    }
}
