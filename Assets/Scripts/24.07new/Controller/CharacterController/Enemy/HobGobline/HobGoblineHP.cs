using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class HobGoblineHP : MonoBehaviour
{
    //홉 고블린 hp 관련 스크립트. 보스 몬스터이기에 pool에서 관리하지 않고 단일 개체로 다룬다. hp가 0이 되면 dead애니메이션 재생 후 비활성화
    [Header("HobGobline HP UI")]
    [SerializeField] private GameObject hpBarUI;//체력 바 ui
    [SerializeField] private Transform hpBarPosition;//체력 바 위치는 항상 에너미 머리 위로 고정.
    private TextMeshProUGUI hpText;
    private GameObject hpBarInstance;// 체력 바 인스턴스
    private Image hpFill;

    [Header("HP Value")]
    public float maxHP = 80.0f;
    public float currentHP;

    private void Init()
    {
        hpBarInstance = Instantiate(hpBarUI, hpBarPosition.position, Quaternion.identity, hpBarPosition);//에너미의 머리 위의 빈 오브젝트 위치에 체력 바를 위치시키고 인스턴스로 설정
        hpFill = hpBarInstance.GetComponentInChildren<Image>();//체력바 ui 인스턴스의 자식인 체력바 이미지를 할당
        hpFill.transform.position = hpBarInstance.transform.position;
        hpText = hpBarInstance.GetComponentInChildren<TextMeshProUGUI>();//체력바 ui 인스턴스의 자식인 체력 텍스트를 할당
        hpText.text = $"{currentHP}";//체력 텍스트 업데이트
        hpBarInstance.SetActive(true);
    }

    void Start()
    {
        Init();
        UpdateHPBar();
    }

    void Update()
    {
        //체력 바가 머리 위에 고정되도록 위치 업데이트
        if(hpBarInstance!=null && hpText!=null)
        {
            hpBarInstance.transform.position = hpBarPosition.position;
            hpText.transform.position = hpBarPosition.position;
        }
    }

    public void TakeDamage(float damage)// 홉 고블린 컨트롤러의 OnHit()메서드에 선언
    {
        currentHP = Mathf.Clamp(currentHP - damage, 0, maxHP);//데미지에 따라서 hp값 및 ui 업데이트
        UpdateHPBar();
    }

    private void UpdateHPBar()// 체력 바 업데이트
    {
        if(hpFill!=null)
        {
            hpFill.fillAmount = currentHP/maxHP;
        }
        hpText.text = $"{currentHP}";
    }

    public void ActiveHPBar()
    {
        hpBarInstance.SetActive(true);
    }

    public void PassiveHPBar()
    {
        hpBarInstance.SetActive(false);
    }

    public void Die()// 사망 시
    {
        if(currentHP!=0)
        {
            currentHP = 0;
        }
        PassiveHPBar();
        gameObject.SetActive(false);
    }
}
