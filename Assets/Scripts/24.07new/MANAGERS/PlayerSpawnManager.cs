using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    /*씬마다 스폰되는 플레이어 위치를 제어하기 위한 스크립트.
        250115 오류 해결
        - 파악된 문제 : 현재 olenamento 맵은 큰 터레인 하나를 여러 구역으로 나누어 각 씬에서 동일하게 사용 중인데, 동일한 터레인을 사용할 때 유니티에서의 씬 전환 메커니즘 문제로 spawnpoint 지정이 불가
        - 이유 : 동일한 터레인을 사용하므로 월드 좌표계가 연속성을 가지게 됨, DontDestroyOnLoad로 유지되는 플레이어 오브젝트가 씬 전환 중에서도 기존 위치를 유지하게 됨, 새로운 씬의 스폰 포인트를 찾아
        위치를 설정하기 전에 이미 플레이어가 이전 위치에서 렌더링 될 수 있음
        -> 해결법 : SceneManager.activeSceneChanged 이벤트를 사용하여 씬 전환 시작 지점을 감지
                    씬 전환이 시작될 때 플레이어를 찾아 비활성화 하고, 마지막 위치 저장
                    새로운 씬에서 스폰포인트를 찾은 후에만 플레이어를 활성화
                    WaitForEndOfFrame을 사용하여 씬 로드 완료를 보장
                    --> 이렇게 해서 씬 전환 시 플레이어가 이전 위치에서 렌더링되는 것을 방지하고, 항상 새로운 씬의 스폰포인트에서 시작하게 됨.
    */
    [SerializeField] private GameObject Player;
    private GameObject spawnPoint;
    private Vector3 lastPosition;//씬 전환 전 마지막 위치 저장용
    
    private void Awake()
    {
        InitPlayer();
        SceneManager.sceneLoaded+=OnSceneLoaded;//씬 로드 이벤트보다 먼저 실행되도록 함.
    }

    private void OnEnable() //씬 전환이 시작될 때 호출
    {
        SceneManager.activeSceneChanged+=OnActiveSceneChanged;//씬 전환 직전에 플레이어를 비활성화
    }
    private void OnDisable() 
    {
        SceneManager.activeSceneChanged-=OnActiveSceneChanged;
    }

    private void InitPlayer()
    {
        if(Player==null)
        {
            Player = GameObject.FindWithTag("Player");
        }
    }

    private void OnActiveSceneChanged(Scene prevScene, Scene nextScene)//씬 전환이 시작될 때 플레이어를 비활성화한다.
    {
        if(Player!=null)
        {
            lastPosition = Player.transform.position;//마지막 위치를 저장
            Player.SetActive(false);//플레이어를 비활성화
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SetPlayerPositionWithDelay(scene));
    }

    private IEnumerator SetPlayerPositionWithDelay(Scene scene)//스폰포인트 탐색 및 위치 설정 코루틴 
    {
        yield return new WaitForEndOfFrame();//씬이 완전히 로드될 때 까지 대기

        spawnPoint = null;//스폰 포인트를 찾는다.
        switch(scene.name)
        {
            case "Olenamento":
                spawnPoint = GameObject.Find("OlenamentoSpawnPoint"); 
                break;

            case "Olenamento_WestField":
                spawnPoint = GameObject.Find("WestFieldSpawnPoint");
                break;

            case "Olenamento_WestField_BossStage":
                spawnPoint = GameObject.Find("WestBossStageSpawnPoint");
                break;
        }

        if(spawnPoint != null && Player != null)//플레이어가 비활성화된 상태에서 위치를 설정한다.
        {
            Player.transform.position = spawnPoint.transform.position;
            Player.transform.rotation = spawnPoint.transform.rotation;

            Player.SetActive(true);//위치설정이 완료된 후 플레이어 활성화 
        }
        else
        {
            Debug.LogError($"Failed to find spawn point in scene {scene.name}");
            if(Player!=null)
            {
                Player.transform.position = lastPosition;
                Player.SetActive(true);//만약 스폰포인트를 찾지 못했을 경우 마지막 위치로 복귀
            }
        }
    }
}
