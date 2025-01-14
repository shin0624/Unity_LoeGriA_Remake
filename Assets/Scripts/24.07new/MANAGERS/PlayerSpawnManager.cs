using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    //씬마다 스폰되는 플레이어 위치를 제어하기 위한 스크립트.

    [SerializeField] private GameObject Player;
    private GameObject SpawnPoint;

    private void Awake() 
    {
        // 씬 전환 시 PlayerSpawnManager가 다른 씬으로 유지되도록 설정
        DontDestroyOnLoad(gameObject);

        // 씬 로드 시마다 스폰 포인트를 설정하도록 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 새로운 씬이 로드될 때마다 스폰 포인트를 설정합니다.
        SetSpawnPoint(scene.name);
    }

    private void SetSpawnPoint(string sceneName)
    {
        // 각 씬마다 스폰되어야 할 위치 오브젝트를 찾는다.(Empty Object)
         switch (sceneName)
        {
            case "Olenamento_WestField":
                SpawnPoint = GameObject.Find("WestFieldSpawnPoint");
                break;
            case "Olenamento":
                SpawnPoint = GameObject.Find("OlenamentoSpawnPoint");
                break;
            case "Olenamento_WestField_BossStage":
                SpawnPoint = GameObject.Find("WestBossStageSpawnPoint");
                break;
            default:
                Debug.LogWarning($"[{sceneName}]에 대한 스폰 포인트가 정의되지 않았습니다.");
                return;
        }

        if(SpawnPoint!=null)
        {
            Player.transform.position = SpawnPoint.transform.position;//스폰포인트가 존재하면 그 위치로 스폰시킨다.
            Debug.Log($"Spawn Point : {SpawnPoint.name}");
        }
        else
        {
            Debug.LogWarning($"{sceneName}에서 스폰 포인트를 찾지 못함.");
        }
    }
    private void OnDestroy()
    {
        // 씬이 변경될 때 이벤트 구독을 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
