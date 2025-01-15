using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    //������ �����Ǵ� �÷��̾� ��ġ�� �����ϱ� ���� ��ũ��Ʈ.

    [SerializeField] private GameObject Player;
    
    private void Awake()
    {
        if(Player==null)
        {
            Player = GameObject.FindWithTag("Player");
        }
        SceneManager.sceneLoaded+=OnSceneLoaded;   
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject spawnPoint = null;
        if(scene.name == "Olenamento")
        {
            spawnPoint = GameObject.Find("OlenamentoSpawnPoint");  
        }
        else if(scene.name == "Olenamento_WestField")
        {
            spawnPoint = GameObject.Find("WestFieldSpawnPoint");
        }
        else if(scene.name == "Olenamento_WestField_BossStage")
        {
            spawnPoint = GameObject.Find("WestBossStageSpawnPoint");
        }

        SetSpawnPosition(spawnPoint);
    }

    private void SetSpawnPosition(GameObject sp)
    {
        if(sp!=null)
        {
            Player.transform.position = sp.transform.position;
            Player.transform.rotation = sp.transform.rotation;
            Debug.Log($"player.transform.position : {Player.transform.position}, sp position : {sp.transform.position}");
        }
    }


}
