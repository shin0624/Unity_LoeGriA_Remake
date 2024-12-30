using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHP : MonoBehaviour
{
    // 에너미 HP 관리 스크립트.
    public float MaxHP = 50.0f;
    public float CurrentHP;
    [SerializeField] private GameObject hpBarUI;//체력 바 ui
    [SerializeField] private Transform hpBarPosition;//체력 바 위치는 항상 에너미 머리 위로 고정.
    [SerializeField] private float respawnTime = 5.0f;
    [SerializeField] private EnemyPool enemyPool;//오브젝트 풀링으로 관리하는 에너미의 활성화/비활성화를 체력에 따라 결정
    private GameObject hpBarInstance;// 체력 바 인스턴스
    private Image hpFill;
    void Start()
    {
        StartInit();
        UpdateHPBar();//초기 체력 바 상태 업데이트
    }
    
    void Update()
    {
        //체력 바가 머리 위에 고정되도록 위치 업데이트
        if(hpBarInstance!=null)
        {
            hpBarInstance.transform.position = hpBarPosition.position;
        }
    }

    private void StartInit()//초기화
    {
        if(enemyPool==null)
        {
            enemyPool = FindAnyObjectByType<EnemyPool>();
        }
        CurrentHP = MaxHP;
        hpBarInstance.SetActive(false);
        hpBarInstance = Instantiate(hpBarUI, hpBarPosition.position, Quaternion.identity, hpBarPosition);//에너미의 머리 위의 빈 오브젝트 위치에 체력 바를 위치시키고 인스턴스로 설정
        hpFill = hpBarInstance.GetComponentInChildren<Image>();//체력바 ui 인스턴스의 자식인 체력바 이미지를 할당
        hpFill.transform.position = hpBarInstance.transform.position;
    }

    public void TakeDamage(float damage)// 에너미 컨트롤러의 OnHit()메서드에 선언
    {
        CurrentHP = Mathf.Clamp(CurrentHP - damage, 0, MaxHP);//데미지에 따라서 hp값 및 ui 업데이트
        UpdateHPBar();
        if(CurrentHP <=0)//hp = 0이면 사망
        {
            Die();
        }
    }
    private void UpdateHPBar()// 체력 바 업데이트
    {
        if(hpFill!=null)
        {
            hpFill.fillAmount = CurrentHP/MaxHP;
        }
    }
    private void Die()
    {
        enemyPool.ReturnEnemy(gameObject);
        hpBarInstance.SetActive(false);
        Invoke(nameof(Respawn), 5.0f);//5초 후 리스폰(재활성화)
    }
    private void Respawn()// 에너미 사망 시 체력을 다시 max로 하여 재활성화
    {
        CurrentHP = MaxHP;
        hpBarInstance.SetActive(true);
        enemyPool.GetEnemy();
    }
    public void ActiveHPBar()
    {
        hpBarInstance.SetActive(true);
    }

}
