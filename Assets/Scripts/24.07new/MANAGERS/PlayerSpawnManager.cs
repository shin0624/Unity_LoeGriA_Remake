using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    //������ �����Ǵ� �÷��̾� ��ġ�� �����ϱ� ���� ��ũ��Ʈ.

    [SerializeField] private GameObject Player;
    private GameObject SpawnPoint;

    private void Awake() 
    {
        // �� ��ȯ �� PlayerSpawnManager�� �ٸ� ������ �����ǵ��� ����
        DontDestroyOnLoad(gameObject);

        // �� �ε� �ø��� ���� ����Ʈ�� �����ϵ��� �̺�Ʈ ����
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���ο� ���� �ε�� ������ ���� ����Ʈ�� �����մϴ�.
        SetSpawnPoint(scene.name);
    }

    private void SetSpawnPoint(string sceneName)
    {
        // �� ������ �����Ǿ�� �� ��ġ ������Ʈ�� ã�´�.(Empty Object)
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
                Debug.LogWarning($"[{sceneName}]�� ���� ���� ����Ʈ�� ���ǵ��� �ʾҽ��ϴ�.");
                return;
        }

        if(SpawnPoint!=null)
        {
            Player.transform.position = SpawnPoint.transform.position;//��������Ʈ�� �����ϸ� �� ��ġ�� ������Ų��.
            Debug.Log($"Spawn Point : {SpawnPoint.name}");
        }
        else
        {
            Debug.LogWarning($"{sceneName}���� ���� ����Ʈ�� ã�� ����.");
        }
    }
    private void OnDestroy()
    {
        // ���� ����� �� �̺�Ʈ ������ ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
