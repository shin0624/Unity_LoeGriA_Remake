using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPool : MonoBehaviour
{
    //���ʹ� ����, �ı��� ����ϴ� ��ũ��Ʈ
    [SerializeField] private GameObject enemyPrefab;//���ʹ� ������
    [SerializeField] private int poolSize = 6;//Ǯ ũ��
    [SerializeField] private float spawnRange = 10.0f; // ���� ���� ����
    [SerializeField] private int maxAttempts = 6; // ��ȿ ��ġ Ž�� �ִ� �õ� Ƚ��
    private Queue<GameObject> pool = new Queue<GameObject>();//���ʹ� ������Ʈ Ǯ
    [SerializeField] private NavMeshSurface navMeshSurface;//���ʹ̰� ������ �׺�޽� �����̽�
    [SerializeField] private GameObject spawnTrigger;
     private void Awake() {//�ʱ�ȭ : Ǯ ũ�⸸ŭ ������Ʈ ����

        if(navMeshSurface==null)
        {
            navMeshSurface = FindAnyObjectByType<NavMeshSurface>();
        }
        for(int i=0; i<poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);//���ʹ� ����
            enemy.SetActive(false);//��Ȱ��ȭ
            pool.Enqueue(enemy);//Ǯ�� �߰�
        }
        if(spawnTrigger==null)
        {
            GameObject.Find("EnemyPoolTrigger");
        }
    }
    
    public GameObject GetEnemy()// ���ʹ̸� Ǯ���� ���� Ȱ��ȭ��Ų��.
    {
        if (pool.Count > 0)
        {
            GameObject enemy = pool.Dequeue();
            enemy.transform.position = RandomSpawnPosition();
            enemy.SetActive(true);
            return enemy;
        }
        else
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position = RandomSpawnPosition();
            enemy.SetActive(true);
            return enemy;
        }
    }

    public void ReturnEnemy(GameObject enemy)//���ʹ̸� Ǯ�� ��ȯ�ϴ� �޼���
    {
        enemy.SetActive(false);//��Ȱ��ȭ
        pool.Enqueue(enemy);//Ǯ�� ��ȯ
    }
    private Vector3 RandomSpawnPosition()
    {
        NavMeshHit hit;
        bool validPositionFound = false;

        // NavMesh �󿡼� ��ȿ�� ���� ��ġ�� ã�� ������ �ݺ�
        do
        {
            Vector3 randomDirection = Random.insideUnitSphere * spawnRange; // ��ü ������ ���� ���� ����
            randomDirection += spawnTrigger.transform.position; // Ʈ���� ��ġ�� �������� ���� ����
            validPositionFound = NavMesh.SamplePosition(randomDirection, out hit, spawnRange, NavMesh.AllAreas); // ��ȿ�� �˻�
        } while (!validPositionFound);

        return hit.position; // ��ȿ�� ��ġ ��ȯ

    }
}
