using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingAreaTrigger : MonoBehaviour
{
    // �÷��̾ Ư�� ������ ���� ������Ʈ�� Ʈ���ŵǾ��� �� Pool���� ���ʹ̵��� ���� Ȱ��ȭ��Ű�� ��ũ��Ʈ.

    private EnemySpawner enemySpawner;

    private void Start()
    {
        enemySpawner= GetComponent<EnemySpawner>();
    }

    private void OnTriggerEnter(Collider other)//Ʈ���ſ� �÷��̾ ���� ��
    {
        if(other.CompareTag("Player"))
        {
            GameObject Enemy = Managers.ObjectPoolerInstance.GetPooledObject();//������Ʈ Ǯ�� �ν��Ͻ��� �����Ͽ� Ȱ��ȭ üũ �� ���ʹ̸� ����
            if(Enemy!=null)
            {
                enemySpawner.SpawnEnemyOnNavMesh(Enemy);//�׺�޽� ���� ���ʹ̸� �����ϴ� �Լ��� ����.
            }
        }
    }
}
