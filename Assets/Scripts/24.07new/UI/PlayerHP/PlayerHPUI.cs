using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    //�÷��̾� ü�� UI ���� ��ũ��Ʈ
    [SerializeField] private Image hpBar;
    void Start()
    {
        PlayerHP.Instance.OnHPChanged += UpdateHPBar;// PlyaerHP ��ũ��Ʈ�� OnHPChanged �̺�Ʈ�� �����Ͽ� ü�� �� ������Ʈ.
        UpdateHPBar();
    }
    private void UpdateHPBar()
    {
        hpBar.fillAmount = PlayerHP.Instance.CurrentHP; // �÷��̾��� hp�������� �����ͼ� ����
    }


}
