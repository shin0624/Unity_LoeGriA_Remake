using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblineAttack : MonoBehaviour
{
    //��� ���ʹ��� ���� ������ ����ϴ� ��ũ��Ʈ. ����� ���� �⺻ ���ʹ̶� �⺻ ���ݸ�, �⺻ ���ݵ������� ������ �ִ�.
    [SerializeField] private GoblineAttackData goblineAttackData;//��ũ���ͺ� ������Ʈ ��ũ��Ʈ�� ����
    [SerializeField] private PlayerHP playerHP;//�÷��̾� ü���� ����
    public GoblineAttack(GoblineAttackData data)//�ʱ�ȭ�� ������
    {
        goblineAttackData = data;
    }
    public void PerformAttack(GoblineAttackData damageData )
    {
        playerHP.TakeDamage(damageData.goblineAttackDamage);//�÷��̾�� �������� ������.
    }
}
