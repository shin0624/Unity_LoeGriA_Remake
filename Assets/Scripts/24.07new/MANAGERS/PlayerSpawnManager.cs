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
        PlayerSpawnManager playerSpawnManager = Managers.SpawnManager;// 매니저 클래스의 싱글톤 인스턴스 참조
        SceneManager.sceneLoaded += OnSceneLoaded;// 씬 로드 시 호출되도록 이벤트 등록

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
         // 각 씬마다 스폰되어야 할 위치 오브젝트를 찾는다.(Empty Object)
        switch (scene.name)
        {
            case "Olenamento_WestField": SpawnPoint = GameObject.Find("WestFieldSpawnPoint");
                break;
            case "Olenamento": SpawnPoint = GameObject.Find("OlenamentoSpawnPoint");
                break;
        }
        if(SpawnPoint!=null)
        {
            Player.transform.position = SpawnPoint.transform.position;//스폰포인트가 존재하면 그 위치로 스폰시킨다.
            Debug.Log($"Spawn Point : {SpawnPoint.name}");
        }
    }
}
