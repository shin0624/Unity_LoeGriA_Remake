using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{  
    //----------------- ��� ���ʹ� �⺻ ���� -----------------
    [SerializeField] private GameObject Player;
    [SerializeField] private NavMeshAgent Agent; // Bake�� NavMesh���� Ȱ���� ���ʹ�
    [SerializeField] private Animator Anim;
    [SerializeField] private Rigidbody rb;
    private Define.EnemyState state;//���ʹ� ���� ����
    private float currentHitTime = 0.0f; // ���� �ǰ� �ð�
    [SerializeField] private PlayerAttackParticleManager particleManager; // ���ʹ� ���� ����Ʈ �Ŵ���
 //----------------- ����, �Ÿ� ���� -----------------
    [SerializeField, Range(0f, 20.0f)] private float ChaseRange = 12.0f;//�÷��̾� �߰� ���� ����
    [SerializeField, Range(0f, 20.0f)] private float DetectionRange = 8.0f;// �÷��̾� Ž�� �Ÿ�
    [SerializeField, Range(0f, 20.0f)] private float AttackRange = 2.0f;// ���� ���� ����
    [SerializeField] private float hitRecoveryTime = 0.5f; // �ǰ� �� ȸ�� �ð�
    private List<Vector3> Path = new List<Vector3>();// A*�˰������� ���� ��θ������� ����Ʈ
    private int CurrentPathIndex = 0;// ���ʹ̰� ���� �̵����� ��� ������ �ε���. ó������ Path[0]���� �̵�.
    private float DistanceToPlayer;//�÷��̾���� �Ÿ��� ������ ����

 //----------------- �Ҹ�, ��Ÿ  -----------------
    [SerializeField] private AudioClip roarSound;
    [SerializeField] private AudioClip hitSound;
    private AudioManager audioManager;
    private Coroutine hitCoroutine; // hit ���� ó���� �ڷ�ƾ���� ����

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        state = Define.EnemyState.IDLE;//�ʱ���� : IDLE
        Agent = GetComponent<NavMeshAgent>();
        Agent.isStopped = true;
        rb = GetComponent<Rigidbody>();
        audioManager = FindAnyObjectByType<AudioManager>();
        BeginPatrol();//ó���� Ž�� ����
    }

    private void Update()//Hit���´� �ڷ�ƾ���� ó���ϴϱ� switch������ ����
    {
        DistanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);//�÷��̾�� ���ʹ� ������ �Ÿ��� ���
        switch (state)
        {
            case Define.EnemyState.IDLE:
            case Define.EnemyState.WALKING:
                Patrol();//��ο� ���� Ž���� ��� ����
                break;
            case Define.EnemyState.RUNNING:
                UpdateChase();// �÷��̾ �߰�
                break;
            case Define.EnemyState.ATTACK:
                UpdateAttack();//�÷��̾ ����
                break;
        }
    }

    private void Patrol()// Ž������
    {
        if (Agent.isOnNavMesh && Path.Count > 0)
        {
            Agent.isStopped = false;
            Agent.speed = 3.0f;

            if (DistanceToPlayer <= DetectionRange && state != Define.EnemyState.ATTACK)//Ž�� ���� ���� �÷��̾ �����ϸ� && ���� ���°� �ƴ� �� �߰��� �����Ѵ�.
            {
                audioManager.PlaySound(roarSound);
                SetState(Define.EnemyState.RUNNING, "RUNNING");
                return;
            }

            if (!Agent.hasPath || Agent.remainingDistance < 1.0f)//���� ��ΰ� ���ų�, ��ǥ ������ �����ϸ�
            {
                if (CurrentPathIndex < Path.Count)//��� ���� ���� �������� �̵�
                {
                    Agent.SetDestination(Path[CurrentPathIndex]);
                    CurrentPathIndex++;//���� �̵����� ����� ���� ��η� �̵��� ��.
                }
                else// ��� ���� �����ϸ� �� ��� ���
                {
                    CalculateNewPath();
                }
            }
        }
    }

    private void BeginPatrol()
    {
        SetState(Define.EnemyState.WALKING, "WALKING"); // �ɾ�ٴϸ� Ž�� ����
        Agent.isStopped = false;
        CalculateNewPath();// ���ο� ��θ� ���
    }

    private void UpdateAttack()// ���� �� -> �÷��̾���� �Ÿ��� ���� ���� ������ �Ѿ ���� && �÷��̾���� �Ÿ��� ���� Ž�� ������ ���Ե� �� �ٽ� �i�ư� �÷��̾ �����ؾ� ��.
    {
        if (Agent.isOnNavMesh)
        {
            Agent.isStopped = true;//���� �� �� �ڸ����� ����
            SetState(Define.EnemyState.ATTACK, "ATTACK");
            if (DistanceToPlayer > AttackRange)// ���ݹ����� ����ٸ�
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
            Agent.destination = Player.transform.position;// �������� �÷��̾� ���������� �����Ͽ� �߰�
            if (DistanceToPlayer > ChaseRange)//�÷��̾���� �Ÿ��� �߰� ���� ������ ����ٸ�
            {
                BeginPatrol();//Ž�� ���·� ��ȯ 
            }
            else if (DistanceToPlayer < AttackRange)//���� ���� �������� �ٰ����ٸ�
            {
                UpdateAttack();
                return;
            }
        }
    }

    private void CalculateNewPath()// ���ο� ��θ� ����ϴ� �޼���
    {
        Vector3 RandomDirection = Random.insideUnitSphere * 10.0f;// �ݰ� 1�� ���� �� ���� ���� ���� * 10���� ��� ����
        RandomDirection += transform.position;// ���ʹ� ������ ���� ���� ���� ���Ѵ�

        NavMeshHit hit;
        //SamplePosition((Vector3 sourcePosition, out NavmeshHit hit, float maxDistance, int areaMask)
        // ���������� �޼��� : areaMask�� �ش��ϴ� NavMesh �߿���, maxDistance �ݰ� ������ sourcePosition�� �ֱ����� ��ġ�� ã�� hit�� ��´�.
        if (NavMesh.SamplePosition(RandomDirection, out hit, 10.0f, NavMesh.AllAreas))
        {
            Path.Clear();
            Path.Add(hit.position);//A* �˰��� �ش��ϴ� ��� ����
            CurrentPathIndex = 0;// ������ġ�� ��� ���� �ʱ�ȭ
            Agent.SetDestination(Path[CurrentPathIndex]);
        }
        
    }

    public void OnHit(float damage, Vector3 hitPoint, Vector3 hitNormal, float knockbackForece)//�ǰ� ó�� �޼���
    {
        if (state == Define.EnemyState.HIT) return;//�ǰ� ������ ���� �߰� �ǰ��� ���� �ʴ´�. �ǰ� ȸ���ð��� �ſ� ª�� ����
        if(hitCoroutine!=null)
        {
            StopCoroutine(hitCoroutine);//���� Hit�ڷ�ƾ�� ���� ���̶�� ����.
        }
        hitPoint = transform.position;//�ǰ� ��ƼŬ ��� ������ ���� �߽����� ����
        hitCoroutine = StartCoroutine(HitRoutine(hitPoint, knockbackForece));//���ο� �ǰ� �ڷ�ƾ ����
        particleManager.PlayHitParticle(hitPoint,hitNormal);//�ǰ� ��ƼŬ ���
    }

    private IEnumerator HitRoutine(Vector3 hitPoint, float knockbackForece)// Hit���¸� �ڷ�ƾ���� ����. �̸� ���� �ǰ� ������ �ִϸ��̼� �ӵ� �� ����ȭ �� �ǰ� �� ���� ������ �����Ѵ�.
    {
        //���� ��ȭ �� �ִϸ��̼� ����
        SetState(Define.EnemyState.HIT, "HIT");
        Agent.isStopped = true;
        currentHitTime = 0.0f;

        if(rb!=null)//�˹� ȿ�� �� ���� ���
        {
            Vector3 knockbackDirection = (transform.position - hitPoint).normalized;
            knockbackDirection.y = 0;
            
            rb.AddForce(knockbackDirection * knockbackForece, ForceMode.Impulse);
        }
        if(audioManager!=null && hitSound!=null)
        {
            audioManager.PlaySound(hitSound);
        }

        //Hit �ִϸ��̼� ���̸�ŭ ���
        float hitAnimLength = Anim.GetCurrentAnimatorStateInfo(0).length;// ���� ������� �ִϸ��̼��� ���̸� ��ȯ
        yield return new WaitForSeconds(hitAnimLength);

        //Hit ���¿��� ����
        currentHitTime = 0.0f;
        Agent.isStopped = false;

         // Hit �� �÷��̾���� �Ÿ��� ���� ������ ���·� ��ȯ
        if (DistanceToPlayer <= AttackRange)// ���� ���� �� �÷��̾���� �Ÿ��� ���� ���� ���� ���� �ִٸ�
        {
            SetState(Define.EnemyState.ATTACK, "ATTACK");
        }
        else if (DistanceToPlayer <= DetectionRange)// ���� ���� �� �÷��̾ �i�ư� �� �ִٸ�
        {
            SetState(Define.EnemyState.RUNNING, "RUNNING");
        }
        else//�� ���� ���
        {
            SetState(Define.EnemyState.WALKING, "WALKING");
            BeginPatrol();
        }
        
        hitCoroutine = null;
    }

    private void SetState(Define.EnemyState NewState, string AnimationTrigger)// ���º��� �޼���
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
