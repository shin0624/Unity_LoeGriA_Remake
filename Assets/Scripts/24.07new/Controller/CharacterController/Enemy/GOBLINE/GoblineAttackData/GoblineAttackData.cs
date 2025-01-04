using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoblineAttackData", menuName = "Attack System/Gobline/Gobline Attack Data")]
//��� ���ʹ��� ���� �����͸� �����ϴ� ��ũ���ͺ� ������Ʈ. ����� ���� �⺻ ���ʹ̶� �⺻ ���ݸ�, �⺻ ���ݵ������� ������ �ִ�.
public class GoblineAttackData : ScriptableObject
{
    public string goblineAttackName;
    public float goblineAttackDamage;
}
