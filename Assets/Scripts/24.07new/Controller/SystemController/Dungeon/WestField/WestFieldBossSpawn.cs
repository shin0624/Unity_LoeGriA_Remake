using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WestFieldBossSpawn : MonoBehaviour
{
    //���� �� ������ ���� �������� Ÿ�Ӷ����� ����� �� �ٷ� BossSpawnPoint���� Ȱ��ȭ�ȴ�. �� �������� ��Ȱ��ȭ
    [SerializeField] private GameObject bossSpawnPoint;
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private GameObject bossHopGobline;
    void Start()
    {
        InitBoss();
        timeline.stopped+=LateSetActiveBoss;//Ÿ�Ӷ����� ����Ǹ� ���� Ȱ��ȭ�� ����
    }

    void LateSetActiveBoss(PlayableDirector pd)//���� Ȱ��ȭ(Ÿ�Ӷ��� ���� �� �̺�Ʈ)
    {
        bossHopGobline.SetActive(true);
    }

    void InitBoss()
    {
        bossHopGobline.transform.position = bossSpawnPoint.transform.position;//������ ��ġ�� ���� ����Ʈ�� ��´�.
        bossHopGobline.SetActive(false);//������ ó���� ��Ȱ��ȭ ����
    }
}
