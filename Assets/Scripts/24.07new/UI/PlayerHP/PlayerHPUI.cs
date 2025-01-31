using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHPUI : MonoBehaviour
{
    //플레이어 HP UI 관리 스크립트
    [SerializeField] private Image hpBar;
    [SerializeField] private TextMeshProUGUI hpText;
    void Start()
    {
        PlayerHP.Instance.OnHPChanged += UpdateHPBar;// PlyaerHP 스크립트의 OnHPChanged 이벤트를 구독하여 체력 바 업데이트.
        UpdateHPBar();
    }
    private void UpdateHPBar()
    {
        hpBar.fillAmount = PlayerHP.Instance.CurrentHP; // 플레이어의 hp변수값을 가져와서 적용
        hpText.text = $"{PlayerHP.Instance.CurrentHP}"; // hp 텍스트 업데이트
    }


}
