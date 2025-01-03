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
    [SerializeField] public int poolSize = 6;//Ǯ ũ��
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
            GameObject enemy = Instantiate(enemyPrefab, transform);//���ʹ� ����
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
        GameObject enemy;
        if (pool.Count > 0)
        {
            enemy = pool.Dequeue();
        }
        else
        {
            enemy = Instantiate(enemyPrefab, transform);
        }
        Vector3 spawnPos = GetValidSpawnPosition();
        enemy.transform.position = spawnPos;
        enemy.SetActive(true);
        return enemy;
        
    }

    public void ReturnEnemy(GameObject enemy)//���ʹ̸� Ǯ�� ��ȯ�ϴ� �޼���
    {
        enemy.SetActive(false);//��Ȱ��ȭ
        pool.Enqueue(enemy);//Ǯ�� ��ȯ
    }
    private Vector3 GetValidSpawnPosition()
    {
        for(int i=0; i<maxAttempts; i++)
        {
            //���� ���� ���
            Vector2 randomCircle = Random.insideUnitCircle*spawnRange;
            Vector3 randomPoint = spawnTrigger.transform.position + new Vector3(randomCircle.x, 5.5f,randomCircle.y);//�ͷ��� ���� ���̸� ���߾� �����ؾ� ��

            NavMeshHit hit;
            //navmesh ���� ���� ����� ��ġ ã��
            if(NavMesh.SamplePosition(randomPoint, out hit, spawnRange, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        //��ȿ�� ��ġ�� ã�� ���� ��쿡�� Ʈ���� ��ġ�� ��ȯ.
        Debug.Log("Don`t find valid position");
        return spawnTrigger.transform.position;
    }
}
