using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHP : MonoBehaviour
{
   //�÷��̾� HP ���� �Ŵ���
   public static PlayerHP Instance;//�÷��̾� hp �ν��Ͻ�
   public float MaxHP = 100.0f;//�ִ� HP
   public float CurrentHP;//���� HP
   public event Action OnHPChanged; // hp ���� �̺�Ʈ

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
        CurrentHP = MaxHP;//�ʱ� HP ����
    }

    public void TakeDamage(float damage)//�������� �޾��� �� : HP ������ �˸���.
    {
        CurrentHP = Mathf.Clamp(CurrentHP - damage, 0, MaxHP);//HP ����
        OnHPChanged?.Invoke();//HP ���� �̺�Ʈ ȣ��
        if(CurrentHP <=0)
        {
            PlayerDeath();//�÷��̾� ���
        }
    }

    public void Heal(float healAmount)//HP ȸ��
    {
        CurrentHP = Mathf.Clamp(CurrentHP + healAmount, 0, MaxHP);//HP ȸ��
        OnHPChanged?.Invoke();//HP ���� �̺�Ʈ ȣ��
    }

    public void PlayerDeath()//HP �������� �÷��̾� ��� ��
    {
        Debug.Log("Player is Dead");
        //�÷��̾� ��� ó��
    }
}
