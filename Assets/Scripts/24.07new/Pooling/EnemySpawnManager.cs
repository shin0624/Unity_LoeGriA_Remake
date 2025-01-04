using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    //���ʹ� ������ �����ϴ� ��ũ��Ʈ.
    [SerializeField] private EnemyPool enemyPool;
    [SerializeField] private Collider poolingTrigger;
    private const string playerTag = "Player";
    private bool enemySpawned  = false;//���ʹ��� ������ ������ ���� ���� �÷���
    void Start()
    {
        // EnemyPool �ʱ�ȭ
        if (enemyPool == null)
        {
            enemyPool = FindAnyObjectByType<EnemyPool>();
            if (enemyPool == null)
            {
                Debug.LogError("EnemyPool is not assigned or found in the scene!");
                return;
            }
        }

        // Trigger Collider �ʱ�ȭ
        if (poolingTrigger == null)
        {
            poolingTrigger = GetComponent<BoxCollider>();
            if (poolingTrigger == null)
            {
                Debug.LogError("Pooling trigger (BoxCollider) is not assigned or found in the object!");
            }
        }
    }

    private void OnTriggerEnter(Collider other) { //���� ������ �÷��̾ ������ ���ʹ� �ڵ� ����
        if(other.CompareTag(playerTag) && !enemySpawned)
        {
            for(int i = 0; i<enemyPool.poolSize; i++)
            {
                enemyPool.GetEnemy(); // ��� ���ʹ� Ȱ��ȭ
            }
            enemySpawned = true;//���ʹ� ���� �Ϸ� ����
            
        }
    }

}
