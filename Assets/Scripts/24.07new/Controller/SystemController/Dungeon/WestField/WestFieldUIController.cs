using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WestFieldUIController : MonoBehaviour
{
    [SerializeField] private GameObject PopupUI; 
    [SerializeField] private Button bossStageButton; // 보스 방 버튼
    [SerializeField] private Button nextTownButton; // 다음 마을 버튼
    [SerializeField] private Button resumeButton;//돌아가기 버튼

    [SerializeField] private Sprite bossStageButtonClicked;
    [SerializeField] private Sprite nextTownButtonClicked;
    [SerializeField] private Sprite resumeButtonClicked;

     private Image bossStageButtonImage;
     private Image nextTownButtonImage;
    private Image resumeButtonImage;

    private string bossStageSceneName = "Olenamento_WestField_BossStage"; // 다음 씬 이름
    private string nextTownSceneName = "";
    private bool IsPlayerInRange = false; // 플레이어가 게이트 범위 안에 있는지 확인

    void Start()
    {
        PopupUI.SetActive(false);// 트리거에 닿기 전에는 ui를 감춘다.

        bossStageButtonImage = bossStageButton.GetComponent<Image>();
        nextTownButtonImage = nextTownButton.GetComponent<Image>();
        resumeButtonImage = resumeButton.GetComponent<Image>();
        
        bossStageButton.onClick.AddListener(OnBossStageButtonClicked);// 버튼에 이벤트 리스너 등록
        nextTownButton.onClick.AddListener(OnNextTownButtonClicked);//버튼에 리스너 등록
        resumeButton.onClick.AddListener(OnResumeButtonClicked);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))//플레이어가 트리거 앞에 왔다면 ui 호출
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            IsPlayerInRange = true;
            PopupUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))//플레이어가 트리거에서 떨어지면 ui 닫기
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            IsPlayerInRange = false;
            PopupUI.SetActive(false);
        }
    }

    private void OnBossStageButtonClicked()//보스방 버튼 클릭 시
    {
        PopupUI.SetActive(false);
        SceneManager.LoadScene(bossStageSceneName);
    }

    private void OnNextTownButtonClicked()//다음 마을 버튼 클릭 시
    {
        PopupUI.SetActive(false);
        SceneManager.LoadScene(nextTownSceneName);
    }

    private void OnResumeButtonClicked()//돌아가기 버튼 클릭 시
    {
        PopupUI.SetActive(false);
    }
}
