using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{  
    //----------------- 고블린 에너미 기본 변수 -----------------
    [SerializeField] private GameObject Player;
    [SerializeField] private NavMeshAgent Agent; // Bake된 NavMesh에서 활동할 에너미
    [SerializeField] private Animator Anim;
    [SerializeField] private Rigidbody rb;
    private Define.EnemyState state;//에너미 상태 변수
    private float currentHitTime = 0.0f; // 현재 피격 시간
    [SerializeField] private PlayerAttackParticleManager particleManager; // 에너미 공격 이펙트 매니저
 //----------------- 범위, 거리 변수 -----------------
    [SerializeField, Range(0f, 20.0f)] private float ChaseRange = 12.0f;//플레이어 추격 가능 범위
    [SerializeField, Range(0f, 20.0f)] private float DetectionRange = 8.0f;// 플레이어 탐지 거리
    [SerializeField, Range(0f, 20.0f)] private float AttackRange = 2.0f;// 공격 가능 범위
    [SerializeField] private float hitRecoveryTime = 0.5f; // 피격 후 회복 시간
    private List<Vector3> Path = new List<Vector3>();// A*알고리즘으로 계산된 경로를저장할 리스트
    private int CurrentPathIndex = 0;// 에너미가 현재 이동중인 경로 지점의 인덱스. 처음에는 Path[0]으로 이동.
    private float DistanceToPlayer;//플레이어와의 거리를 저장할 변수

 //----------------- 소리, 기타  -----------------
    [SerializeField] private AudioClip roarSound;
    [SerializeField] private AudioClip hitSound;
    private AudioManager audioManager;
    private Coroutine hitCoroutine; // hit 상태 처리를 코루틴으로 수행

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        state = Define.EnemyState.IDLE;//초기상태 : IDLE
        Agent = GetComponent<NavMeshAgent>();
        Agent.isStopped = true;
        rb = GetComponent<Rigidbody>();
        audioManager = FindAnyObjectByType<AudioManager>();
        BeginPatrol();//처음에 탐지 시작
    }

    private void Update()//Hit상태는 코루틴에서 처리하니까 switch문에서 제외
    {
        DistanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);//플레이어와 에너미 사이의 거리를 계산
        switch (state)
        {
            case Define.EnemyState.IDLE:
            case Define.EnemyState.WALKING:
                Patrol();//경로에 따라 탐색을 계속 진행
                break;
            case Define.EnemyState.RUNNING:
                UpdateChase();// 플레이어를 추격
                break;
            case Define.EnemyState.ATTACK:
                UpdateAttack();//플레이어를 공격
                break;
        }
    }

    private void Patrol()// 탐색상태
    {
        if (Agent.isOnNavMesh && Path.Count > 0)
        {
            Agent.isStopped = false;
            Agent.speed = 3.0f;

            if (DistanceToPlayer <= DetectionRange && state != Define.EnemyState.ATTACK)//탐지 범위 내에 플레이어가 존재하면 && 공격 상태가 아닐 때 추격을 시작한다.
            {
                audioManager.PlaySound(roarSound);
                SetState(Define.EnemyState.RUNNING, "RUNNING");
                return;
            }

            if (!Agent.hasPath || Agent.remainingDistance < 1.0f)//현재 경로가 없거나, 목표 지점에 도달하면
            {
                if (CurrentPathIndex < Path.Count)//경로 상의 다음 지점으로 이동
                {
                    Agent.SetDestination(Path[CurrentPathIndex]);
                    CurrentPathIndex++;//현재 이동중인 경로의 다음 경로로 이동할 것.
                }
                else// 경로 끝에 도달하면 새 경로 계산
                {
                    CalculateNewPath();
                }
            }
        }
    }

    private void BeginPatrol()
    {
        SetState(Define.EnemyState.WALKING, "WALKING"); // 걸어다니며 탐색 시작
        Agent.isStopped = false;
        CalculateNewPath();// 새로운 경로를 계산
    }

    private void UpdateAttack()// 공격 후 -> 플레이어와의 거리가 공격 가능 범위를 넘어간 상태 && 플레이어와의 거리가 아직 탐지 범위에 포함될 때 다시 쫒아가 플레이어를 공격해야 함.
    {
        if (Agent.isOnNavMesh)
        {
            Agent.isStopped = true;//공격 시 그 자리에서 멈춤
            SetState(Define.EnemyState.ATTACK, "ATTACK");
            if (DistanceToPlayer > AttackRange)// 공격범위를 벗어났다면
            {
                UpdateChase();
                return;
            }
        }
    }

    private void UpdateChase()
    {
        if (Agent.isOnNavMesh)
        {
            
            SetState(Define.EnemyState.RUNNING, "RUNNING");
            Agent.isStopped = false;
            Agent.speed = 4.0f;
            Agent.destination = Player.transform.position;// 목적지를 플레이어 포지션으로 설정하여 추격
            if (DistanceToPlayer > ChaseRange)//플레이어와의 거리가 추격 가능 범위를 벗어났다면
            {
                BeginPatrol();//탐지 상태로 전환 
            }
            else if (DistanceToPlayer < AttackRange)//공격 가능 범위까지 다가갔다면
            {
                UpdateAttack();
                return;
            }
        }
    }

    private void CalculateNewPath()// 새로운 경로를 계산하는 메서드
    {
        Vector3 RandomDirection = Random.insideUnitSphere * 10.0f;// 반경 1을 갖는 구 안의 임의 지점 * 10으로 경로 설정
        RandomDirection += transform.position;// 에너미 포지션 값에 랜덤 값을 더한다

        NavMeshHit hit;
        //SamplePosition((Vector3 sourcePosition, out NavmeshHit hit, float maxDistance, int areaMask)
        // 샘플포지션 메서드 : areaMask에 해당하는 NavMesh 중에서, maxDistance 반경 내에서 sourcePosition에 최근접한 위치를 찾아 hit에 담는다.
        if (NavMesh.SamplePosition(RandomDirection, out hit, 10.0f, NavMesh.AllAreas))
        {
            Path.Clear();
            Path.Add(hit.position);//A* 알고리즘에 해당하는 경로 설정
            CurrentPathIndex = 0;// 현재위치를 담는 변수 초기화
            Agent.SetDestination(Path[CurrentPathIndex]);
        }
        
    }

    public void OnHit(float damage, Vector3 hitPoint, Vector3 hitNormal, float knockbackForece)//피격 처리 메서드
    {
        if (state == Define.EnemyState.HIT) return;//피격 상태일 때는 추가 피격을 받지 않는다. 피격 회복시간을 매우 짧게 설정
        if(hitCoroutine!=null)
        {
            StopCoroutine(hitCoroutine);//이전 Hit코루틴이 실행 중이라면 중지.
        }
        hitPoint = transform.position;//피격 파티클 출력 지점을 적의 중심으로 설정
        hitCoroutine = StartCoroutine(HitRoutine(hitPoint, knockbackForece));//새로운 피격 코루틴 시작
        particleManager.PlayHitParticle(hitPoint,hitNormal);//피격 파티클 재생
    }

    private IEnumerator HitRoutine(Vector3 hitPoint, float knockbackForece)// Hit상태를 코루틴으로 관리. 이를 통해 피격 판정과 애니메이션 속도 간 동기화 및 피격 후 상태 고정을 예방한다.
    {
        //상태 변화 및 애니메이션 설정
        SetState(Define.EnemyState.HIT, "HIT");
        Agent.isStopped = true;
        currentHitTime = 0.0f;

        if(rb!=null)//넉백 효과 및 사운드 재생
        {
            Vector3 knockbackDirection = (transform.position - hitPoint).normalized;
            knockbackDirection.y = 0;
            
            rb.AddForce(knockbackDirection * knockbackForece, ForceMode.Impulse);
        }
        if(audioManager!=null && hitSound!=null)
        {
            audioManager.PlaySound(hitSound);
        }

        //Hit 애니메이션 길이만큼 대기
        float hitAnimLength = Anim.GetCurrentAnimatorStateInfo(0).length;// 현재 재생중인 애니메이션의 길이를 반환
        yield return new WaitForSeconds(hitAnimLength);

        //Hit 상태에서 복귀
        currentHitTime = 0.0f;
        Agent.isStopped = false;

         // Hit 후 플레이어와의 거리에 따라 적절한 상태로 전환
        if (DistanceToPlayer <= AttackRange)// 공격 당한 후 플레이어와의 거리가 공격 가능 범위 내에 있다면
        {
            SetState(Define.EnemyState.ATTACK, "ATTACK");
        }
        else if (DistanceToPlayer <= DetectionRange)// 공격 당한 후 플레이어를 쫒아갈 수 있다면
        {
            SetState(Define.EnemyState.RUNNING, "RUNNING");
        }
        else//그 외의 경우
        {
            SetState(Define.EnemyState.WALKING, "WALKING");
            BeginPatrol();
        }
        
        hitCoroutine = null;
    }

    private void SetState(Define.EnemyState NewState, string AnimationTrigger)// 상태변경 메서드
    {
        if (state != NewState) 
        {
            state = NewState;
            Anim.ResetTrigger("IDLE");
            Anim.ResetTrigger("WALKING");
            Anim.ResetTrigger("RUNNING");
            Anim.ResetTrigger("HIT");
            Anim.ResetTrigger("ATTACK");
            Anim.SetTrigger(AnimationTrigger);
            Debug.Log($"Enemy state changed to: {NewState} with animation: {AnimationTrigger}");
        }
    }
    
}
