using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WestFieldUIController : MonoBehaviour
{
    [SerializeField] private GameObject PopupUI; 
    [SerializeField] private Button bossStageButton; // ���� �� ��ư
    [SerializeField] private Button nextTownButton; // ���� ���� ��ư
    [SerializeField] private Button resumeButton;//���ư��� ��ư

    [SerializeField] private Sprite bossStageButtonClicked;
    [SerializeField] private Sprite nextTownButtonClicked;
    [SerializeField] private Sprite resumeButtonClicked;

     private Image bossStageButtonImage;
     private Image nextTownButtonImage;
    private Image resumeButtonImage;

    private string bossStageSceneName = "Olenamento_WestField_BossStage"; // ���� �� �̸�
    private string nextTownSceneName = "";
    private bool IsPlayerInRange = false; // �÷��̾ ����Ʈ ���� �ȿ� �ִ��� Ȯ��

    void Start()
    {
        PopupUI.SetActive(false);// Ʈ���ſ� ��� ������ ui�� �����.

        bossStageButtonImage = bossStageButton.GetComponent<Image>();
        nextTownButtonImage = nextTownButton.GetComponent<Image>();
        resumeButtonImage = resumeButton.GetComponent<Image>();
        
        bossStageButton.onClick.AddListener(OnBossStageButtonClicked);// ��ư�� �̺�Ʈ ������ ���
        nextTownButton.onClick.AddListener(OnNextTownButtonClicked);//��ư�� ������ ���
        resumeButton.onClick.AddListener(OnResumeButtonClicked);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))//�÷��̾ Ʈ���� �տ� �Դٸ� ui ȣ��
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            IsPlayerInRange = true;
            PopupUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))//�÷��̾ Ʈ���ſ��� �������� ui �ݱ�
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            IsPlayerInRange = false;
            PopupUI.SetActive(false);
        }
    }

    private void OnBossStageButtonClicked()//������ ��ư Ŭ�� ��
    {
        PopupUI.SetActive(false);
        SceneManager.LoadScene(bossStageSceneName);
    }

    private void OnNextTownButtonClicked()//���� ���� ��ư Ŭ�� ��
    {
        PopupUI.SetActive(false);
        SceneManager.LoadScene(nextTownSceneName);
    }

    private void OnResumeButtonClicked()//���ư��� ��ư Ŭ�� ��
    {
        PopupUI.SetActive(false);
    }
}
