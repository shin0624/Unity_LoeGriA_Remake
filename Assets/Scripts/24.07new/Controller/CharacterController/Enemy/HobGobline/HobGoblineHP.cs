using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class HobGoblineHP : MonoBehaviour
{
    //ȩ ��� hp ���� ��ũ��Ʈ. ���� �����̱⿡ pool���� �������� �ʰ� ���� ��ü�� �ٷ��. hp�� 0�� �Ǹ� dead�ִϸ��̼� ��� �� ��Ȱ��ȭ
    [Header("HobGobline HP UI")]
    [SerializeField] private GameObject hpBarUI;//ü�� �� ui
    [SerializeField] private Transform hpBarPosition;//ü�� �� ��ġ�� �׻� ���ʹ� �Ӹ� ���� ����.
    private TextMeshProUGUI hpText;
    private GameObject hpBarInstance;// ü�� �� �ν��Ͻ�
    private Image hpFill;

    [Header("HP Value")]
    public float maxHP = 80.0f;
    public float currentHP;

    private void Init()
    {
        hpBarInstance = Instantiate(hpBarUI, hpBarPosition.position, Quaternion.identity, hpBarPosition);//���ʹ��� �Ӹ� ���� �� ������Ʈ ��ġ�� ü�� �ٸ� ��ġ��Ű�� �ν��Ͻ��� ����
        hpFill = hpBarInstance.GetComponentInChildren<Image>();//ü�¹� ui �ν��Ͻ��� �ڽ��� ü�¹� �̹����� �Ҵ�
        hpFill.transform.position = hpBarInstance.transform.position;
        hpText = hpBarInstance.GetComponentInChildren<TextMeshProUGUI>();//ü�¹� ui �ν��Ͻ��� �ڽ��� ü�� �ؽ�Ʈ�� �Ҵ�
        hpText.text = $"{currentHP}";//ü�� �ؽ�Ʈ ������Ʈ
        hpBarInstance.SetActive(true);
    }

    void Start()
    {
        Init();
        UpdateHPBar();
    }

    void Update()
    {
        //ü�� �ٰ� �Ӹ� ���� �����ǵ��� ��ġ ������Ʈ
        if(hpBarInstance!=null && hpText!=null)
        {
            hpBarInstance.transform.position = hpBarPosition.position;
            hpText.transform.position = hpBarPosition.position;
        }
    }

    public void TakeDamage(float damage)// ȩ ��� ��Ʈ�ѷ��� OnHit()�޼��忡 ����
    {
        currentHP = Mathf.Clamp(currentHP - damage, 0, maxHP);//�������� ���� hp�� �� ui ������Ʈ
        UpdateHPBar();
    }

    private void UpdateHPBar()// ü�� �� ������Ʈ
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

    public void Die()// ��� ��
    {
        if(currentHP!=0)
        {
            currentHP = 0;
        }
        PassiveHPBar();
        gameObject.SetActive(false);
    }
}
