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
        PlayerSpawnManager playerSpawnManager = Managers.SpawnManager;// �Ŵ��� Ŭ������ �̱��� �ν��Ͻ� ����
        SceneManager.sceneLoaded += OnSceneLoaded;// �� �ε� �� ȣ��ǵ��� �̺�Ʈ ���

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
         // �� ������ �����Ǿ�� �� ��ġ ������Ʈ�� ã�´�.(Empty Object)
        switch (scene.name)
        {
            case "Olenamento_WestField": SpawnPoint = GameObject.Find("WestFieldSpawnPoint");
                break;
            case "Olenamento": SpawnPoint = GameObject.Find("OlenamentoSpawnPoint");
                break;
        }
        if(SpawnPoint!=null)
        {
            Player.transform.position = SpawnPoint.transform.position;//��������Ʈ�� �����ϸ� �� ��ġ�� ������Ų��.
            Debug.Log($"Spawn Point : {SpawnPoint.name}");
        }
    }
}
