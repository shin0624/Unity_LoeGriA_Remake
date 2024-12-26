using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    //���ʹ̰� NavMeshSurface ���� ������ �� �ְ� �ϴ� ��ũ��Ʈ.

    [SerializeField] NavMeshSurface NMS;// ���̾��Ű ���� navmeshsurface�� ����
    private float SpawnRange = 20.0f;//���� ������ �ִ� �ݰ�

    private void Start()
    {
        if(NMS==null)
        {
            NMS = FindObjectOfType<NavMeshSurface>();//�Ҵ���� �ʾ��� ��� �ڵ����� ã�´�.
        }
    }

    public void SpawnEnemyOnNavMesh(GameObject enemy)// �׺�޽� ���� ��������Ʈ�� �����ϰ�, ���ʹ̸� ��ġ��Ų��.
    {
        Vector3 RandomPoint = GetRandomPointInNavMesh();
        enemy.transform.position = RandomPoint;
        enemy.SetActive(false);
    }

    private Vector3 GetRandomPointInNavMesh()// ������ ����Ʈ�� ã�� ��ȯ�ϴ� �޼���
    {
        NavMeshHit hit;

        //NMS �󿡼� ��ȿ�� ������ġ�� ã�� ������ �ݺ�
        do
        {
            Vector3 RandomDirection = Random.insideUnitSphere * SpawnRange;
            RandomDirection += transform.position;
            NavMesh.SamplePosition(RandomDirection, out hit, SpawnRange, NavMesh.AllAreas);
        } while (!hit.hit);

        return hit.position;

    }
}
