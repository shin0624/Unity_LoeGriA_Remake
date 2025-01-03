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
    [SerializeField] public int poolSize = 6;//풀 크기
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
            GameObject enemy = Instantiate(enemyPrefab, transform);//에너미 생성
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

    public void ReturnEnemy(GameObject enemy)//에너미를 풀로 반환하는 메서드
    {
        enemy.SetActive(false);//비활성화
        pool.Enqueue(enemy);//풀로 반환
    }
    private Vector3 GetValidSpawnPosition()
    {
        for(int i=0; i<maxAttempts; i++)
        {
            //랜덤 방향 계산
            Vector2 randomCircle = Random.insideUnitCircle*spawnRange;
            Vector3 randomPoint = spawnTrigger.transform.position + new Vector3(randomCircle.x, 5.5f,randomCircle.y);//터레인 지형 높이를 맞추어 스폰해야 함

            NavMeshHit hit;
            //navmesh 상의 가장 가까운 위치 찾기
            if(NavMesh.SamplePosition(randomPoint, out hit, spawnRange, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        //유효한 위치를 찾지 못한 경우에는 트리거 위치를 반환.
        Debug.Log("Don`t find valid position");
        return spawnTrigger.transform.position;
    }
}
