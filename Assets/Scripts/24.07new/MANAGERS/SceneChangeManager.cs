using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SceneChangeManager : MonoBehaviour
{
    // 동, 서, 남, 북 게이트에 사용할 씬 전환 스크립트. 각 게이트를 거쳐 다른 필드로 진행해야 하기 때문에 재사용성 높게 작성함

    [SerializeField] private GameObject Player;
    [SerializeField] private Animator GateAnimator; // 게이트 오픈 애니메이터
    [SerializeField] private GameObject PopupUI; // 다음 씬으로 넘어갈 것인지 묻는 ui 
    [SerializeField] private Button YesButton; // 예 버튼
    [SerializeField] private Button NoButton; // 아니오 버튼

    [SerializeField] private Sprite YesButtonClicked;
    [SerializeField] private Sprite NoButtonClicked;

     private Image YesButtonImage;
     private Image NoButtonImage;

    [SerializeField] private string NextSceneName = "Olenamento_WestField"; // 다음 씬 이름
    private bool IsPlayerInRange = false; // 플레이어가 게이트 범위 안에 있는지 확인

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PopupUI.SetActive(false);// 게이트에 닿기 전에는 ui를 감춘다.

        YesButtonImage = YesButton.GetComponent<Image>();
        NoButtonImage = NoButton.GetComponent<Image>();
        
        YesButton.onClick.AddListener(OnYesButtonClicked);// 버튼에 이벤트 리스너 등록
        NoButton.onClick.AddListener(OnNoButtonClicked);//버튼에 리스너 등록
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))//플레이어가 문 앞에 왔다면 ui 호출
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            IsPlayerInRange = true;
            PopupUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))//플레이어가 문 앞에서 떨어지면 ui 닫기
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            IsPlayerInRange = false;
            PopupUI.SetActive(false);
        }
    }

    private void OnYesButtonClicked()// 예 버튼을 누르면 팝업을 닫고 게이트 오픈 애니메이션 실행
    {
        PopupUI.SetActive(false);
        StartCoroutine(OpenGate());
    }
    private void OnNoButtonClicked()
    {
        PopupUI.SetActive(false);
    }


    private IEnumerator OpenGate()// 게이트 오픈 애니메이션 실행 후 씬 변경
    {
        GateAnimator.SetBool("DoorOpen", true);// 게이트 오픈 애니메이션 실행을 위해 true값을 넘겨준다.
        yield return new WaitForSeconds(GateAnimator.GetCurrentAnimatorStateInfo(0).length);// 애니메이션이 끝날 때 까지 대기 
        LoadingScene.LoadScene(NextSceneName);//씬 전환
    }


}
