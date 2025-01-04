using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoblineAttackData", menuName = "Attack System/Gobline/Gobline Attack Data")]
//고블린 에너미의 공격 데이터를 관리하는 스크립터블 오브젝트. 고블린은 가장 기본 에너미라서 기본 공격명, 기본 공격데미지만 가지고 있다.
public class GoblineAttackData : ScriptableObject
{
    public string goblineAttackName;
    public float goblineAttackDamage;
}
