using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class HobGoblineController : MonoBehaviour
{
    //���� �� ���� ȩ ��� ��Ʈ�ѷ�. ����� enemyController�� �����ϰ�, ���� ��ȭ ����� ��Ʈ�ѷ���, ���� �� ������ ������ HobGoblineAttackŬ������, ü�� ������ HobGoblineHP�� �ۼ�.
    //--------------------------ȩ ��� �⺻ ����--------------------------
    private bool isInitialized = false;//�ʱ�ȭ ���� �÷���
    private GameObject player;
    private Define.HobGoblineState state;// ȩ ��� ���� ����
    private GameObject manager;
    private AudioManager audioManager;
    //---------------------------��� ����----------------------------------
    private float distanceToPlayer;//�÷��̾���� �Ÿ�
    private float walkingRange = 8.0f;// �÷��̾���� �Ÿ��� �� ���� �ٴٸ� �� ���� walking���� �÷��̾ �i�´�.
    private float runningRange = 5.0f;//�÷��̾���� �Ÿ��� �� ���� �ٴٸ� �� ���� running���� �÷��̾ �i�´�.
    private float walkingSpeed = 2.0f;// ȩ ��� �ӵ�
    private float runningSpeed = 3.0f;//ȩ ��� running�ӵ�

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private HobGoblineAttack hobGoblineAttack;
    
    void Init()//�ʱ�ȭ �� �Ҵ�
    {
        if (isInitialized) return;
        
        player ??= GameObject.FindWithTag("Player");
        manager ??= GameObject.Find("@Managers");
        audioManager ??= manager.GetComponent<AudioManager>();
        agent ??= GetComponent<NavMeshAgent>();
        anim ??= GetComponent<Animator>();
        rb ??= GetComponent<Rigidbody>();
        hobGoblineAttack??=GetComponent<HobGoblineAttack>();
        distanceToPlayer = 0.0f;

        isInitialized = true;
    }
    
    void Start()
    {
        Init();
        SetBool(Define.HobGoblineState.IDLE);//ù ���´� IDLE
    }

    
    void Update()
    {
        DistanceCheck();//�÷��̾���� �Ÿ��� �� ������ ����Ͽ� ȩ ����� ���� ���̿� ������ �ֵ��� �Ѵ�.
        FSM();
    }

    private void FSM()//ȩ ��� ���� ���� FSM
    {
        switch(state)
        {
            case Define.HobGoblineState.IDLE:

            case Define.HobGoblineState.WALK:
                WalkToPlayer();
                break;

            case Define.HobGoblineState.RUN:
                RunToPlayer();
                break;

            case Define.HobGoblineState.ATTACK:
                PerformAttack();
                break;

            case Define.HobGoblineState.DAMAGE:
                break;

            case Define.HobGoblineState.DEAD:
                break;
        }   
    }

    private void DistanceCheck()//�÷��̾���� �Ÿ���� ��, ���� ���� ���¸� ����
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if(distanceToPlayer <= runningRange)//�÷��̾���� �Ÿ��� ���� ��������ٸ� ����.
        {
            SetBool(Define.HobGoblineState.ATTACK);
        }
        else if(distanceToPlayer <=walkingRange)//�÷��̾���� �Ÿ��� ȩ ����� �������� ������ ��������ٸ� RUN
        {
            SetBool(Define.HobGoblineState.RUN);
        }
        else//�� �̿ܿ��� �ɾ �÷��̾ �߰��Ѵ�. 
        {
            SetBool(Define.HobGoblineState.WALK);
        }

    }

    private void RunToPlayer()//�÷��̾ �i�� �޷����� �޼���
    {
        if(!agent.enabled) return;//����ó��
        agent.speed = walkingSpeed;
        agent.SetDestination(player.transform.position);//�÷��̾ �������� ����.
    }

    private void WalkToPlayer()//�÷��̾ �i�� �ɾ�� �޼���
    {
        if(!agent.enabled) return;
        agent.speed = runningSpeed;
        agent.SetDestination(player.transform.position);
    }

    private void PerformAttack()
    {
        if(hobGoblineAttack!=null)
        {
            hobGoblineAttack.PerformAttack();
        }
        else
        {
            Debug.LogError("HobGoblineAttack Component is NULL");
        }
    }


    private void SetBool(Define.HobGoblineState NewState)// ���º��� �޼���. ȩ ����� �������� �Ķ���Ͱ� bool�̹Ƿ� setstate�� setbool�� ����
    {
        if (state ==NewState) return; //���°� �����ϸ� ����
        //��� bool���� �ʱ�ȭ
        anim.SetBool("IDLE", false);
        anim.SetBool("WALK", false);
        anim.SetBool("RUN", false);
        anim.SetBool("DEAD", false);
        anim.SetBool("ATTACK", false);
        anim.SetBool("DAMAGE", false);

        switch(NewState)// ���ο� ���¿� ���� bool �� ����
        {
            case Define.HobGoblineState.IDLE:
                anim.SetBool("IDLE", true);
                break;

            case Define.HobGoblineState.WALK:
                anim.SetBool("WALK", true);
                break;

            case Define.HobGoblineState.RUN:
                anim.SetBool("RUN", true);
                break;

            case Define.HobGoblineState.DEAD:
                anim.SetBool("DEAD", true);
                break;

            case Define.HobGoblineState.ATTACK:
                anim.SetBool("ATTACK", true);
                PerformAttack();// ���� ����
                break;

            case Define.HobGoblineState.DAMAGE:
                anim.SetBool("DAMAGE", true);
                break;
        }
        state = NewState;
        Debug.Log($"HobGobline state changed to: {NewState}");
        
    }
}
