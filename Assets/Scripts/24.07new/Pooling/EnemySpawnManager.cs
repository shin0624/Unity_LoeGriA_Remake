using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    //에너미 스폰을 실행하는 스크립트.
    [SerializeField] private EnemyPool enemyPool;
    [SerializeField] private Collider poolingTrigger;
    private const string playerTag = "Player";
    private bool enemySpawned  = false;//에너미의 과도한 스폰을 막기 위한 플래그
    void Start()
    {
        // EnemyPool 초기화
        if (enemyPool == null)
        {
            enemyPool = FindAnyObjectByType<EnemyPool>();
            if (enemyPool == null)
            {
                Debug.LogError("EnemyPool is not assigned or found in the scene!");
                return;
            }
        }

        // Trigger Collider 초기화
        if (poolingTrigger == null)
        {
            poolingTrigger = GetComponent<BoxCollider>();
            if (poolingTrigger == null)
            {
                Debug.LogError("Pooling trigger (BoxCollider) is not assigned or found in the object!");
            }
        }
    }

    private void OnTriggerEnter(Collider other) { //스폰 지점에 플레이어가 들어오면 에너미 자동 스폰
        if(other.CompareTag(playerTag) && !enemySpawned)
        {
            for(int i = 0; i<enemyPool.poolSize; i++)
            {
                enemyPool.GetEnemy(); // 모든 에너미 활성화
            }
            enemySpawned = true;//에너미 생성 완료 상태
            
        }
    }

}
