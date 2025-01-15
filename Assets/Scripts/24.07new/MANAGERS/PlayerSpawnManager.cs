using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    /*������ �����Ǵ� �÷��̾� ��ġ�� �����ϱ� ���� ��ũ��Ʈ.
        250115 ���� �ذ�
        - �ľǵ� ���� : ���� olenamento ���� ū �ͷ��� �ϳ��� ���� �������� ������ �� ������ �����ϰ� ��� ���ε�, ������ �ͷ����� ����� �� ����Ƽ������ �� ��ȯ ��Ŀ���� ������ spawnpoint ������ �Ұ�
        - ���� : ������ �ͷ����� ����ϹǷ� ���� ��ǥ�谡 ���Ӽ��� ������ ��, DontDestroyOnLoad�� �����Ǵ� �÷��̾� ������Ʈ�� �� ��ȯ �߿����� ���� ��ġ�� �����ϰ� ��, ���ο� ���� ���� ����Ʈ�� ã��
        ��ġ�� �����ϱ� ���� �̹� �÷��̾ ���� ��ġ���� ������ �� �� ����
        -> �ذ�� : SceneManager.activeSceneChanged �̺�Ʈ�� ����Ͽ� �� ��ȯ ���� ������ ����
                    �� ��ȯ�� ���۵� �� �÷��̾ ã�� ��Ȱ��ȭ �ϰ�, ������ ��ġ ����
                    ���ο� ������ ��������Ʈ�� ã�� �Ŀ��� �÷��̾ Ȱ��ȭ
                    WaitForEndOfFrame�� ����Ͽ� �� �ε� �ϷḦ ����
                    --> �̷��� �ؼ� �� ��ȯ �� �÷��̾ ���� ��ġ���� �������Ǵ� ���� �����ϰ�, �׻� ���ο� ���� ��������Ʈ���� �����ϰ� ��.
    */
    [SerializeField] private GameObject Player;
    private GameObject spawnPoint;
    private Vector3 lastPosition;//�� ��ȯ �� ������ ��ġ �����
    
    private void Awake()
    {
        InitPlayer();
        SceneManager.sceneLoaded+=OnSceneLoaded;//�� �ε� �̺�Ʈ���� ���� ����ǵ��� ��.
    }

    private void OnEnable() //�� ��ȯ�� ���۵� �� ȣ��
    {
        SceneManager.activeSceneChanged+=OnActiveSceneChanged;//�� ��ȯ ������ �÷��̾ ��Ȱ��ȭ
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

    private void OnActiveSceneChanged(Scene prevScene, Scene nextScene)//�� ��ȯ�� ���۵� �� �÷��̾ ��Ȱ��ȭ�Ѵ�.
    {
        if(Player!=null)
        {
            lastPosition = Player.transform.position;//������ ��ġ�� ����
            Player.SetActive(false);//�÷��̾ ��Ȱ��ȭ
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SetPlayerPositionWithDelay(scene));
    }

    private IEnumerator SetPlayerPositionWithDelay(Scene scene)//��������Ʈ Ž�� �� ��ġ ���� �ڷ�ƾ 
    {
        yield return new WaitForEndOfFrame();//���� ������ �ε�� �� ���� ���

        spawnPoint = null;//���� ����Ʈ�� ã�´�.
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

        if(spawnPoint != null && Player != null)//�÷��̾ ��Ȱ��ȭ�� ���¿��� ��ġ�� �����Ѵ�.
        {
            Player.transform.position = spawnPoint.transform.position;
            Player.transform.rotation = spawnPoint.transform.rotation;

            Player.SetActive(true);//��ġ������ �Ϸ�� �� �÷��̾� Ȱ��ȭ 
        }
        else
        {
            Debug.LogError($"Failed to find spawn point in scene {scene.name}");
            if(Player!=null)
            {
                Player.transform.position = lastPosition;
                Player.SetActive(true);//���� ��������Ʈ�� ã�� ������ ��� ������ ��ġ�� ����
            }
        }
    }
}
