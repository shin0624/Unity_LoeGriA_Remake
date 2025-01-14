using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SceneChangeManager : MonoBehaviour
{
    // ��, ��, ��, �� ����Ʈ�� ����� �� ��ȯ ��ũ��Ʈ. �� ����Ʈ�� ���� �ٸ� �ʵ�� �����ؾ� �ϱ� ������ ���뼺 ���� �ۼ���

    [SerializeField] private GameObject Player;
    [SerializeField] private Animator GateAnimator; // ����Ʈ ���� �ִϸ�����
    [SerializeField] private GameObject PopupUI; // ���� ������ �Ѿ ������ ���� ui 
    [SerializeField] private Button YesButton; // �� ��ư
    [SerializeField] private Button NoButton; // �ƴϿ� ��ư

    [SerializeField] private Sprite YesButtonClicked;
    [SerializeField] private Sprite NoButtonClicked;

     private Image YesButtonImage;
     private Image NoButtonImage;

    [SerializeField] private string NextSceneName = "Olenamento_WestField"; // ���� �� �̸�
    private bool IsPlayerInRange = false; // �÷��̾ ����Ʈ ���� �ȿ� �ִ��� Ȯ��

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PopupUI.SetActive(false);// ����Ʈ�� ��� ������ ui�� �����.

        YesButtonImage = YesButton.GetComponent<Image>();
        NoButtonImage = NoButton.GetComponent<Image>();
        
        YesButton.onClick.AddListener(OnYesButtonClicked);// ��ư�� �̺�Ʈ ������ ���
        NoButton.onClick.AddListener(OnNoButtonClicked);//��ư�� ������ ���
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))//�÷��̾ �� �տ� �Դٸ� ui ȣ��
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            IsPlayerInRange = true;
            PopupUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))//�÷��̾ �� �տ��� �������� ui �ݱ�
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            IsPlayerInRange = false;
            PopupUI.SetActive(false);
        }
    }

    private void OnYesButtonClicked()// �� ��ư�� ������ �˾��� �ݰ� ����Ʈ ���� �ִϸ��̼� ����
    {
        PopupUI.SetActive(false);
        StartCoroutine(OpenGate());
    }
    private void OnNoButtonClicked()
    {
        PopupUI.SetActive(false);
    }


    private IEnumerator OpenGate()// ����Ʈ ���� �ִϸ��̼� ���� �� �� ����
    {
        GateAnimator.SetBool("DoorOpen", true);// ����Ʈ ���� �ִϸ��̼� ������ ���� true���� �Ѱ��ش�.
        yield return new WaitForSeconds(GateAnimator.GetCurrentAnimatorStateInfo(0).length);// �ִϸ��̼��� ���� �� ���� ��� 
        LoadingScene.LoadScene(NextSceneName);//�� ��ȯ
    }


}
