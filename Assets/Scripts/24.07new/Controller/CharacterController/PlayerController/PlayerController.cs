using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController playerInstance; // 플레이어 컨트롤러 싱글톤 인스턴스

//-----------플레이어 행동 관련 변수들 ------
    [Header("Player Settings")]
    [SerializeField] private Animator anim;//애니메이터 컴포넌트
    [SerializeField] private float rotationSpeed = 720.0f;//회전 속도
    [SerializeField] private float jumpForce = 5.0f;//점프 힘
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float runningValue = 1.2f;
    [SerializeField] private Rigidbody rb;//리지드바디 컴포넌트
    [SerializeField] private Camera mainCam;
    private bool IsJumping;//점프 유무

    [Header("Player Attack")]
    [SerializeField] private PlayerAttack playerAttack;//PlayerAttack클래스를 참조
    
    private bool jumpInput; // 점프 요청 플래그
    private Define.PlayerState state; // 상태 변수(플레이어)
    private Vector3 moveDirection;// 이동 벡터

    private void Awake()
    {
        if(playerInstance == null)
        {
            playerInstance = this;
            DontDestroyOnLoad(gameObject);//씬이 전환되어도 플레이어가 사라지지 않도록 조치함 
        }
        else
        {
            Destroy(gameObject);
            return; //이미 인스턴스가 존재한다면 제거
        }
        
    }

    private void Init()
    {
        if(playerAttack==null)
        {
            playerAttack = GetComponent<PlayerAttack>();//플레이어 공격 컴포넌트가 없다면 추가
        }
        gameObject.layer = LayerMask.NameToLayer("Player");  // 플레이어 오브젝트를 Player 레이어로 설정
        if(rb==null)
        {
            rb = GetComponent<Rigidbody>();
        }
        if(anim==null)
        {
            anim = GetComponent<Animator>();
        }
        if(mainCam==null)
        {
            mainCam = Camera.main;
        }
        rb.freezeRotation = true;//리지드바디 회전 고정
        IsJumping = false;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        HandleInput();//매 프레임 호출되는 업데이트에서 입력을 감지.
        HandleActionRequest();
    }

    private void FixedUpdate()// 이동과 점프가 물리기반 움직임이므로, 물리엔진의 업데이트 속도에 맞춰 일정하게 호출되는 FixedUpdate()에서 Move와 Jump를 호출하는 것이 일관성 유지, 성능 측면에서 효과적
    {
        HandleMoveRequest();
        HandleJumpInput();
    }
    
    private void HandleActionRequest()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true;// 점프요청 true
        }
        if(Input.GetMouseButtonDown(0))
        {
            CallPlayerAttack(Define.PlayerState.ATTACK, "ATTACK", 0);//마우스 왼쪽 버튼 클릭 시, 기본 공격 메서드 호출
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            CallPlayerAttack(Define.PlayerState.SLASH, "SLASH", 1);//R키 입력 시, 스킬 공격 메서드 호출
        }
        if(Input.GetMouseButtonDown(1))
        {
            CallPlayerAttack(Define.PlayerState.ATTACK02, "ATTACK02", 2);//마우스 오른쪽 버튼 클릭 시,  기본공격2 메서드 호출
        }
    }

    private void HandleInput()// 사용자 입력 감지 메서드. 입력감지와 물리연산 처리는 분리하도록 한다.
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");//카메라 기준으로 이동 방향 계산

        Vector3 forward = mainCam.transform.forward;
        Vector3 right = mainCam.transform.right;

        forward.y =0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        moveDirection = (forward * vertical + right*horizontal).normalized;
    }

    private void CallPlayerAttack(Define.PlayerState NewState, string AnimationTrigger, int attackNumber)//공격 메서드 호출
    {
        if(state!=NewState)//공격 상태가 아닐 때에만 공격 요청
        {
            SetState(NewState, AnimationTrigger);
            playerAttack.PerformAttack(attackNumber);
        }
    }

    private void HandleMoveRequest()// 이동 메서드
    {
        float CurrentSpeed = moveSpeed;
        if(moveDirection.magnitude > 0.1f)// 이동벡터 값 변화 시, 즉 입력으로 인한 이동이 있을 시에 연산하도록
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed*Time.deltaTime);

            if (Input.GetKey(KeyCode.LeftShift))// 왼쪽 시프트 키를 누르면 runningValue 만큼 현재 스피드에 곱해지면서 달리는 효과 연출. 이에 대한 애니메이션 클립도 추가
            {
                CurrentSpeed *= runningValue;
                SetState(Define.PlayerState.RUNNING, "RUNNING");
            }
            else
            {
                SetState(Define.PlayerState.WALKING, "WALKING");
            }
            rb.MovePosition(rb.position + moveDirection.normalized * CurrentSpeed * Time.fixedDeltaTime);  //Rigidbody를 이용하여 이동하게 하여 벽 통과를 방지.
        }
        else
        {
            SetState(Define.PlayerState.IDLE, "IDLE");
        }
    }

    private void HandleJumpInput()// 점프 메서드
    {
        if(jumpInput)//점프 키 입력 시
        {
            if (!IsJumping)//점프 중이 아니면
            {
                SetState(Define.PlayerState.JUMPING, "JUMPING");
                IsJumping = true;//점프하도록
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);// ForceMode.Impulse란 질량을 사용하여 리지드바디에 순간적인 힘 충격을 추가하는 기능. 
            }
            jumpInput = false;//점프요청 초기화
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Bridge"))// 땅에 착지하면 점프상태 리셋
        {
            IsJumping = false;
        }
    }

    private void SetState(Define.PlayerState NewState, string AnimationTrigger)// 애니메이션 상태 변경 메서드
    {
        if (state != NewState) 
        {
            state = NewState;
            anim.ResetTrigger("IDLE");
            anim.ResetTrigger("WALKING");
            anim.ResetTrigger("RUNNING");
            anim.ResetTrigger("JUMPING");
            anim.ResetTrigger("ATTACK");
            anim.SetTrigger(AnimationTrigger);
        }
    }
}
