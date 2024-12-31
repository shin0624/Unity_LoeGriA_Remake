using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHPUI : MonoBehaviour
{
    //�÷��̾� HP UI ���� ��ũ��Ʈ
    [SerializeField] private Image hpBar;
    [SerializeField] private TextMeshProUGUI hpText;
    void Start()
    {
        PlayerHP.Instance.OnHPChanged += UpdateHPBar;// PlyaerHP ��ũ��Ʈ�� OnHPChanged �̺�Ʈ�� �����Ͽ� ü�� �� ������Ʈ.
        UpdateHPBar();
    }
    private void UpdateHPBar()
    {
        hpBar.fillAmount = PlayerHP.Instance.CurrentHP; // �÷��̾��� hp�������� �����ͼ� ����
        hpText.text = $"{PlayerHP.Instance.CurrentHP}"; // hp �ؽ�Ʈ ������Ʈ
    }


}
