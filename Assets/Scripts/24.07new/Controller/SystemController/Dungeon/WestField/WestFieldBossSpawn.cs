using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WestFieldBossSpawn : MonoBehaviour
{
    //서쪽 숲 보스는 보스 스테이지 타임라인이 종료된 후 바로 BossSpawnPoint에서 활성화된다. 그 전까지는 비활성화
    [SerializeField] private GameObject bossSpawnPoint;
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private GameObject bossHopGobline;
    void Start()
    {
        InitBoss();
        timeline.stopped+=LateSetActiveBoss;//타임라인이 종료되면 보스 활성화가 시작
    }

    void LateSetActiveBoss(PlayableDirector pd)//보스 활성화(타임라인 종료 후 이벤트)
    {
        bossHopGobline.SetActive(true);
    }

    void InitBoss()
    {
        bossHopGobline.transform.position = bossSpawnPoint.transform.position;//보스의 위치를 스폰 포인트로 잡는다.
        bossHopGobline.SetActive(false);//보스는 처음에 비활성화 상태
    }
}
