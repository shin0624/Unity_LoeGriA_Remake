using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    //������Ʈ Ǯ�� ����� ������ ��ũ��Ʈ
    //���ʹ̵��� �̸� ���� �� ��Ȱ��ȭ ���ѳ���, �÷��̾ Ʈ���� ���� �����ϸ� �׶� Ȱ��ȭ��Ų��. ���ʹ��� ü���� ���ϸ� �ٽ� ��Ȱ��ȭ�Ǿ��ٰ�, ���� �ð� �� �ٽ� Ȱ��ȭ�ȴ�.

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
        Pool = new List<GameObject>();//���ӿ�����Ʈ Ÿ���� �ִ� ����Ʈ�� ����
        for(int i=0;i<PoolSize;i++)// Pool ����Ʈ ũ�⸸ŭ �ݺ�
        {
            GameObject obj = Instantiate(EnemyPrefab);//���ʹ� �������� Instantiate�� ���̾��Ű�� �����Ѵ�.
            obj.SetActive(false);//��Ȱ��ȭ ���ѳ���.
            Pool.Add(obj);//����Ʈ�� ���ʹ̵��� �߰��Ѵ�.
        }
    }
    
    public GameObject GetPooledObject()// Pool�� �� ������Ʈ�� ���� ���̾��Ű�� Ȱ��ȭ�Ǿ��ִ��� üũ�ϰ�, ������Ʈ�� ����.
    {
        foreach (var obj in Pool)
        {
            if (!obj.activeInHierarchy) { return obj; }
        }
        return null;
    }

    public void ReturnToPool(GameObject obj)// Ǯ���Ǿ��� ������Ʈ�� ��Ȱ��ȭ��Ŵ
    {
        obj.SetActive(false);
    }

}
