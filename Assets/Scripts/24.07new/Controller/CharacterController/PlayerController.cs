using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private static PlayerController playerInstance; // 플레이어 컨트롤러 싱글톤 인스턴스

//-----------플레이어 행동 관련 변수들 ------
    [SerializeField] private Animator Anim;//애니메이터 컴포넌트
    [SerializeField, Range(1.0f, 10.0f)] private float RunningSpeed;//달리는 속도
    [SerializeField, Range(1.0f, 10.0f)] private float JumpPower;//점프 높이
    [SerializeField, Range(1.0f, 10.0f)] private float MoveSpeed;//이동 속도
    [SerializeField] private Rigidbody rb;//리지드바디 컴포넌트
    private bool IsJumping;//점프 유무
    private bool JumpInput; // 점프 요청 플래그
    private Define.PlayerState state; // 상태 변수(플레이어)
    private Vector3 MoveVector;// 이동 벡터

//-----------에너미 넉백 관련 변수들------
    [SerializeField] private float attackRange = 5.0f; // 공격 범위
    [SerializeField] private float knockBackForce = 500.0f; // 넉백 힘
    [SerializeField] private LayerMask enemyLayer; // 적 레이어
    [SerializeField] private float attackDelay = 0.5f; // 공격 딜레이
    
    
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

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");  // 플레이어 오브젝트를 Player 레이어로 설정

        Cursor.lockState = CursorLockMode.Locked;//마우스 커서를 화면 안에서 고정
        Cursor.visible = false;//커서가 안보이게 설정

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;//리지드바디 회전 고정
        IsJumping = false;
    }

    void Update()
    {
        HandleInput();//매 프레임 호출되는 업데이트에서 입력을 감지.
    }

    private void FixedUpdate()// 이동과 점프가 물리기반 움직임이므로, 물리엔진의 업데이트 속도에 맞춰 일정하게 호출되는 FixedUpdate()에서 Move와 Jump를 호출하는 것이 일관성 유지, 성능 측면에서 효과적
    {
            Move();
            Jump();
    }

    private void HandleInput()// 사용자 입력 감지 메서드. 입력감지와 물리연산 처리는 분리하도록 한다.
    {
        float h = Input.GetAxisRaw("Horizontal");// 수평이동 입력값
        float v = Input.GetAxisRaw("Vertical");// 수직이동 입력값 

        MoveVector = transform.forward * v + transform.right * h;//입력에 따라 이동 방향의 벡터 계산
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpInput = true;// 점프요청 true
        }
        if(Input.GetMouseButtonDown(0))
        {
            StartCoroutine(AttackRountine());//공격 코루틴 시작
        }
    }

    private void Move()// 이동 메서드
    {
        float CurrentSpeed = MoveSpeed;
        if(MoveVector.magnitude > 0)// 이동벡터 값 변화 시, 즉 입력으로 인한 이동이 있을 시에 연산하도록
        {
            if (Input.GetKey(KeyCode.LeftShift))// 왼쪽 시프트 키를 누르면 RunningSpeed 만큼 현재 스피드에 곱해지면서 달리는 효과 연출. 이에 대한 애니메이션 클립도 추가
            {
                CurrentSpeed *= RunningSpeed;
                SetState(Define.PlayerState.RUNNING, "RUNNING");
            }
            else
            {
                SetState(Define.PlayerState.WALKING, "WALKING");
            }
            rb.MovePosition(rb.position + MoveVector.normalized * CurrentSpeed * Time.fixedDeltaTime);  //Rigidbody를 이용하여 이동하게 하여 벽 통과를 방지.
        }
        else
        {
            SetState(Define.PlayerState.IDLE, "IDLE");
        }
    }

    private void Jump()// 점프 메서드
    {
        if(JumpInput)//점프 키 입력 시
        {
            if (!IsJumping)//점프 중이 아니면
            {
                SetState(Define.PlayerState.JUMPING, "JUMPING");
                IsJumping = true;//점프하도록
                rb.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);// ForceMode.Impulse란 질량을 사용하여 리지드바디에 순간적인 힘 충격을 추가하는 기능. 
            }
            JumpInput = false;//점프요청 초기화
        }
    }

    

    private IEnumerator AttackRountine()// Attack메서드를 코루틴으로 변경. 애니메이션 클립과 실제 공격 판정 간 간극 해소
    {
        SetState(Define.PlayerState.ATTACK, "ATTACK");
        Anim.SetBool("IsAttack", true);

        yield return new WaitForSeconds(attackDelay);//공격 딜레이 변수값 만큼 대기 후 재생.

        //실제 공격 판정
        Vector3 rayOrigin = transform.position + Vector3.up * 1.5f;
        Vector3 rayDirection = transform.forward;
        float rayRadius = 1.0f;//구체 레이캐스트의 반지름

        //구체 레이캐스트를 사용하여 넓은 범위 감지. 단일 레이캐스트보다 더 자연스러운 무기 판정 가능
        RaycastHit[] hits = Physics.SphereCastAll(rayOrigin, rayRadius, rayDirection, attackRange, enemyLayer);// 에너미 레이어를 새로 설정해서 에너미에게만 효과가 가해지도록 함.
        
        //디버그 시각화
        Debug.DrawRay(rayOrigin, rayDirection * attackRange, Color.red, 3.0f);
        Debug.DrawLine(
            rayOrigin + rayDirection * attackRange + Vector3.up * rayRadius,
            rayOrigin + rayDirection * attackRange - Vector3.up * rayRadius,
            Color.blue,
            3.0f
        );
        foreach(RaycastHit hit in hits)
        {
            if(hit.collider.CompareTag("Enemy"))
            {
               Debug.Log($"Hit object: {hit.collider.gameObject.name} | Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
                //Rigidbody enemyRb = hit.collider.GetComponent<Rigidbody>();
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if(damageable != null)
                {
                    //거리에 따른 넉백 힘 감소
                      float distanceMultiplier = 1 - (hit.distance / attackRange);
                      float finalKnockbackForce = knockBackForce * distanceMultiplier; // 타격 지점에서 멀 수록 넉백 효과가 약해짐

                    damageable.OnHit(10.0f, hit.point, hit.normal, finalKnockbackForce);//10의 데미지, 공격이 적중한 위치, 충돌 표면의 법선벡터, 밀려나가는 힘의 크기를 매개변수로 전달.
                    // ray가 닿은 콜라이더를 가진 에너미의 idamageable 인터페이스를 찾아서 OnHit 메서드를 호출
                }

                // if(enemyRb!=null)
                // {
                //     //수평 방향으로 넉백 방향 계산
                //     Vector3 knockbackDirection = (hit.collider.transform.position - transform.position).normalized;
                //     knockbackDirection.y = 0;

                //     //거리에 따른 넉백 힘 감소
                //     float distanceMultiplier = 1 - (hit.distance / attackRange);
                //     float finalKnockbackForce = knockBackForce * distanceMultiplier; // 타격 지점에서 멀 수록 넉백 효과가 약해짐

                //     enemyRb.AddForce(knockbackDirection * finalKnockbackForce, ForceMode.Impulse);//넉백 적용. Impulse는 순간적인 힘을 가하는 모드   
                // }
            }
        }
        yield return new WaitForSeconds(Anim.GetCurrentAnimatorStateInfo(0).length);//애니메이션 클립 길이만큼 대기
        ResetAttack();//공격 애니메이션 리셋
    }
    
    private void ResetAttack()
    {
        Anim.SetBool("IsAttack", false); // 공격 애니메이션 리셋
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
        Anim.ResetTrigger("IDLE");
        Anim.ResetTrigger("WALKING");
        Anim.ResetTrigger("RUNNING");
        Anim.ResetTrigger("JUMPING");
        Anim.ResetTrigger("ATTACK");
        Anim.SetTrigger(AnimationTrigger);
    }
    }
}
