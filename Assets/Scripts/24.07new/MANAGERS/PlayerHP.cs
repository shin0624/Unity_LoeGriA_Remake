using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHP : MonoBehaviour
{
   //플레이어 HP 관리 매니저
   public static PlayerHP Instance;//플레이어 hp 인스턴스
   public float MaxHP = 100.0f;//최대 HP
   public float CurrentHP;//현재 HP
   public event Action OnHPChanged; // hp 변경 이벤트

   private void Awake() 
    {
        if(Instance==null)
        {
            Instance = this;
            dontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        CurrentHP = MaxHP;//초기 HP 설정
    }

    public void TakeDamage(float damage)//데미지를 받았을 때 : HP 변경을 알린다.
    {
        CurrentHP = Mathf.Clamp(CurrentHP - damage, 0, MaxHP);//HP 감소
        OnHPChanged?.Invoke();//HP 변경 이벤트 호출
        if(CurrentHP <=0)
        {
            PlayerDeath();//플레이어 사망
        }
    }

    public void Heal(float healAmount)//HP 회복
    {
        CurrentHP = Mathf.Clamp(CurrentHP + healAmount, 0, MaxHP);//HP 회복
        OnHPChanged?.Invoke();//HP 변경 이벤트 호출
    }

    public void PlayerDeath()//HP 소진으로 플레이어 사망 시
    {
        Debug.Log("Player is Dead");
        //플레이어 사망 처리
    }
}
