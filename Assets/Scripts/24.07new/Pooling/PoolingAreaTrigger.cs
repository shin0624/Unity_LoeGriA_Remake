using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingAreaTrigger : MonoBehaviour
{
    // 플레이어가 특정 영역의 게임 오브젝트에 트리거되었을 때 Pool에서 에너미들을 꺼내 활성화시키는 스크립트.

    private EnemySpawner enemySpawner;

    private void Start()
    {
        enemySpawner= GetComponent<EnemySpawner>();
    }

    private void OnTriggerEnter(Collider other)//트리거에 플레이어가 접근 시
    {
        if(other.CompareTag("Player"))
        {
            GameObject Enemy = Managers.ObjectPoolerInstance.GetPooledObject();//오브젝트 풀러 인스턴스에 접근하여 활성화 체크 후 에너미를 리턴
            if(Enemy!=null)
            {
                enemySpawner.SpawnEnemyOnNavMesh(Enemy);//네비메시 위에 에너미를 스폰하는 함수를 실행.
            }
        }
    }
}
