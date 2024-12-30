using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPool : MonoBehaviour
{
    //에너미 생성, 파괴를 담당하는 스크립트
    [SerializeField] private GameObject enemyPrefab;//에너미 프리팹
    [SerializeField] private int poolSize = 6;//풀 크기
    [SerializeField] private float spawnRange = 10.0f; // 랜덤 스폰 범위
    [SerializeField] private int maxAttempts = 6; // 유효 위치 탐색 최대 시도 횟수
    private Queue<GameObject> pool = new Queue<GameObject>();//에너미 오브젝트 풀
    [SerializeField] private NavMeshSurface navMeshSurface;//에너미가 스폰될 네비메시 서페이스
    [SerializeField] private GameObject spawnTrigger;
     private void Awake() {//초기화 : 풀 크기만큼 오브젝트 생성

        if(navMeshSurface==null)
        {
            navMeshSurface = FindAnyObjectByType<NavMeshSurface>();
        }
        for(int i=0; i<poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);//에너미 생성
            enemy.SetActive(false);//비활성화
            pool.Enqueue(enemy);//풀에 추가
        }
        if(spawnTrigger==null)
        {
            GameObject.Find("EnemyPoolTrigger");
        }
    }
    
    public GameObject GetEnemy()// 에너미를 풀에서 꺼내 활성화시킨다.
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

    public void ReturnEnemy(GameObject enemy)//에너미를 풀로 반환하는 메서드
    {
        enemy.SetActive(false);//비활성화
        pool.Enqueue(enemy);//풀로 반환
    }
    private Vector3 RandomSpawnPosition()
    {
        NavMeshHit hit;
        bool validPositionFound = false;

        // NavMesh 상에서 유효한 랜덤 위치를 찾을 때까지 반복
        do
        {
            Vector3 randomDirection = Random.insideUnitSphere * spawnRange; // 구체 범위의 랜덤 방향 생성
            randomDirection += spawnTrigger.transform.position; // 트리거 위치를 기준으로 범위 설정
            validPositionFound = NavMesh.SamplePosition(randomDirection, out hit, spawnRange, NavMesh.AllAreas); // 유효성 검사
        } while (!validPositionFound);

        return hit.position; // 유효한 위치 반환

    }
}
