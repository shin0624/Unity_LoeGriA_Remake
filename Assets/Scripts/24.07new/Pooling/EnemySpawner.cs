using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    //에너미가 NavMeshSurface 위에 스폰될 수 있게 하는 스크립트.

    [SerializeField] NavMeshSurface NMS;// 하이어라키 상의 navmeshsurface를 참조
    private float SpawnRange = 20.0f;//스폰 가능한 최대 반경

    private void Start()
    {
        if(NMS==null)
        {
            NMS = FindObjectOfType<NavMeshSurface>();//할당되지 않았을 경우 자동으로 찾는다.
        }
    }

    public void SpawnEnemyOnNavMesh(GameObject enemy)// 네비메시 위에 랜덤포인트를 지정하고, 에너미를 위치시킨다.
    {
        Vector3 RandomPoint = GetRandomPointInNavMesh();
        enemy.transform.position = RandomPoint;
        enemy.SetActive(false);
    }

    private Vector3 GetRandomPointInNavMesh()// 랜덤한 포인트를 찾아 반환하는 메서드
    {
        NavMeshHit hit;

        //NMS 상에서 유효한 랜덤위치를 찾을 때까지 반복
        do
        {
            Vector3 RandomDirection = Random.insideUnitSphere * SpawnRange;
            RandomDirection += transform.position;
            NavMesh.SamplePosition(RandomDirection, out hit, SpawnRange, NavMesh.AllAreas);
        } while (!hit.hit);

        return hit.position;

    }
}
