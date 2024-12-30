using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHP : MonoBehaviour
{
    // ���ʹ� HP ���� ��ũ��Ʈ.
    public float MaxHP = 50.0f;
    public float CurrentHP;
    [SerializeField] private GameObject hpBarUI;//ü�� �� ui
    [SerializeField] private Transform hpBarPosition;//ü�� �� ��ġ�� �׻� ���ʹ� �Ӹ� ���� ����.
    [SerializeField] private float respawnTime = 5.0f;
    [SerializeField] private EnemyPool enemyPool;//������Ʈ Ǯ������ �����ϴ� ���ʹ��� Ȱ��ȭ/��Ȱ��ȭ�� ü�¿� ���� ����
    private GameObject hpBarInstance;// ü�� �� �ν��Ͻ�
    private Image hpFill;
    void Start()
    {
        StartInit();
        UpdateHPBar();//�ʱ� ü�� �� ���� ������Ʈ
    }
    
    void Update()
    {
        //ü�� �ٰ� �Ӹ� ���� �����ǵ��� ��ġ ������Ʈ
        if(hpBarInstance!=null)
        {
            hpBarInstance.transform.position = hpBarPosition.position;
        }
    }

    private void StartInit()//�ʱ�ȭ
    {
        if(enemyPool==null)
        {
            enemyPool = FindAnyObjectByType<EnemyPool>();
        }
        CurrentHP = MaxHP;
        hpBarInstance.SetActive(false);
        hpBarInstance = Instantiate(hpBarUI, hpBarPosition.position, Quaternion.identity, hpBarPosition);//���ʹ��� �Ӹ� ���� �� ������Ʈ ��ġ�� ü�� �ٸ� ��ġ��Ű�� �ν��Ͻ��� ����
        hpFill = hpBarInstance.GetComponentInChildren<Image>();//ü�¹� ui �ν��Ͻ��� �ڽ��� ü�¹� �̹����� �Ҵ�
        hpFill.transform.position = hpBarInstance.transform.position;
    }

    public void TakeDamage(float damage)// ���ʹ� ��Ʈ�ѷ��� OnHit()�޼��忡 ����
    {
        CurrentHP = Mathf.Clamp(CurrentHP - damage, 0, MaxHP);//�������� ���� hp�� �� ui ������Ʈ
        UpdateHPBar();
        if(CurrentHP <=0)//hp = 0�̸� ���
        {
            Die();
        }
    }
    private void UpdateHPBar()// ü�� �� ������Ʈ
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
        Invoke(nameof(Respawn), 5.0f);//5�� �� ������(��Ȱ��ȭ)
    }
    private void Respawn()// ���ʹ� ��� �� ü���� �ٽ� max�� �Ͽ� ��Ȱ��ȭ
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
