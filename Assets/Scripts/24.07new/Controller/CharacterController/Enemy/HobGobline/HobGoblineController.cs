using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class HobGoblineController : MonoBehaviour
{
    //서쪽 숲 보스 홉 고블린 컨트롤러. 고블린의 enemyController와 유사하게, 상태 변화 기능은 컨트롤러에, 공격 등 데미지 관련은 HobGoblineAttack클래스에, 체력 관련은 HobGoblineHP에 작성.
    //--------------------------홉 고블린 기본 변수--------------------------
    private bool isInitialized = false;//초기화 여부 플래그
    private GameObject player;
    private Define.HobGoblineState state;// 홉 고블린 상태 변수
    private GameObject manager;
    private AudioManager audioManager;
    //---------------------------계산 변수----------------------------------
    private float distanceToPlayer;//플레이어와의 거리
    private float walkingRange = 8.0f;// 플레이어와의 거리가 이 값에 다다를 때 까지 walking으로 플레이어를 쫒는다.
    private float runningRange = 5.0f;//플레이어와의 거리가 이 값에 다다를 때 까지 running으로 플레이어를 쫒는다.
    private float walkingSpeed = 2.0f;// 홉 고블린 속도
    private float runningSpeed = 3.0f;//홉 고블린 running속도

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private HobGoblineAttack hobGoblineAttack;
    
    void Init()//초기화 및 할당
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
        SetBool(Define.HobGoblineState.IDLE);//첫 상태는 IDLE
    }

    
    void Update()
    {
        DistanceCheck();//플레이어와의 거리는 매 프레임 계산하여 홉 고블린의 상태 전이에 영향을 주도록 한다.
        FSM();
    }

    private void FSM()//홉 고블린 상태 전이 FSM
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

    private void DistanceCheck()//플레이어와의 거리계산 후, 값에 따라 상태를 변경
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if(distanceToPlayer <= runningRange)//플레이어와의 거리가 아주 가까워졌다면 공격.
        {
            SetBool(Define.HobGoblineState.ATTACK);
        }
        else if(distanceToPlayer <=walkingRange)//플레이어와의 거리를 홉 고블린이 따라잡을 정도로 가까워졌다면 RUN
        {
            SetBool(Define.HobGoblineState.RUN);
        }
        else//그 이외에는 걸어서 플레이어를 추격한다. 
        {
            SetBool(Define.HobGoblineState.WALK);
        }

    }

    private void RunToPlayer()//플레이어를 쫒아 달려가는 메서드
    {
        if(!agent.enabled) return;//예외처리
        agent.speed = walkingSpeed;
        agent.SetDestination(player.transform.position);//플레이어를 목적지로 설정.
    }

    private void WalkToPlayer()//플레이어를 쫒아 걸어가는 메서드
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


    private void SetBool(Define.HobGoblineState NewState)// 상태변경 메서드. 홉 고블린의 상태전이 파라미터가 bool이므로 setstate를 setbool로 변경
    {
        if (state ==NewState) return; //상태가 동일하면 종료
        //모든 bool값을 초기화
        anim.SetBool("IDLE", false);
        anim.SetBool("WALK", false);
        anim.SetBool("RUN", false);
        anim.SetBool("DEAD", false);
        anim.SetBool("ATTACK", false);
        anim.SetBool("DAMAGE", false);

        switch(NewState)// 새로운 상태에 따라 bool 값 설정
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
                PerformAttack();// 공격 수행
                break;

            case Define.HobGoblineState.DAMAGE:
                anim.SetBool("DAMAGE", true);
                break;
        }
        state = NewState;
        Debug.Log($"HobGobline state changed to: {NewState}");
        
    }
}
