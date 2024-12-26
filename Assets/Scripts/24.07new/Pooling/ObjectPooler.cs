using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    //오브젝트 풀링 기능을 구현한 스크립트
    //에너미들을 미리 생성 후 비활성화 시켜놓고, 플레이어가 트리거 내에 진입하면 그때 활성화시킨다. 에너미의 체력이 다하면 다시 비활성화되었다가, 일정 시간 후 다시 활성화된다.

    public static ObjectPooler Instance = Managers.ObjectPoolerInstance;
    [SerializeField] private GameObject EnemyPrefab;
    private int PoolSize = 10;
    private List<GameObject> Pool;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Pool = new List<GameObject>();//게임오브젝트 타입을 넣는 리스트를 생성
        for(int i=0;i<PoolSize;i++)// Pool 리스트 크기만큼 반복
        {
            GameObject obj = Instantiate(EnemyPrefab);//에너미 프리팹을 Instantiate로 하이어라키에 생성한다.
            obj.SetActive(false);//비활성화 시켜놓음.
            Pool.Add(obj);//리스트에 에너미들을 추가한다.
        }
    }
    
    public GameObject GetPooledObject()// Pool의 각 오브젝트들 마다 하이어라키에 활성화되어있는지 체크하고, 오브젝트를 리턴.
    {
        foreach (var obj in Pool)
        {
            if (!obj.activeInHierarchy) { return obj; }
        }
        return null;
    }

    public void ReturnToPool(GameObject obj)// 풀링되었던 오브젝트를 비활성화시킴
    {
        obj.SetActive(false);
    }

}
