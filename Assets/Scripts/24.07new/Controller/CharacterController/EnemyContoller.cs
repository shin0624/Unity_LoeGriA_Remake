using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //A* �˰��� ���� Ŭ����
    
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private NavMeshAgent Agent; // Bake�� NavMesh���� Ȱ���� ���ʹ�
    [SerializeField]
    private Animator Anim;

    [SerializeField, Range(0f, 20.0f)]
    private float ChaseRange = 12.0f;//�÷��̾� �߰� ���� ����
    [SerializeField, Range(0f, 20.0f)]
    private float DetectionRange = 8.0f;// �÷��̾� Ž�� �Ÿ�
    [SerializeField, Range(0f, 20.0f)]
    private float AttackRange = 2.0f;// ���� ���� ����

    private Define.EnemyState state;//���ʹ� ���� ����
    private float DistanceToPlayer;//�÷��̾���� �Ÿ��� ������ ����

    private List<Vector3> Path = new List<Vector3>();// A*�˰������� ���� ��θ������� ����Ʈ
    private int CurrentPathIndex = 0;// ���ʹ̰� ���� �̵����� ��� ������ �ε���. ó������ Path[0]���� �̵�.

    [SerializeField] private AudioSource Ado;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        state = Define.EnemyState.IDLE;//�ʱ���� : IDLE
        Agent = GetComponent<NavMeshAgent>();
        Agent.isStopped = true;
        Ado = GetComponent<AudioSource>();

        BeginPatrol();//ó���� Ž�� ����
    }

    private void Update()
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
                EnemySoundPlay();
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
            EnemySoundPlay();
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


    private void SetState(Define.EnemyState NewState, string AnimationTrigger)// ���º��� �޼���
    {
        if (state != NewState) { state = NewState; Anim.SetTrigger(AnimationTrigger); }//���ʿ��� ���� ������ �ּ�ȭ. ������ ���¿� �°� �ִϸ������� Ʈ���Ÿ� �ٲپ��ش�
   
   
    }



    private void EnemySoundPlay()
    {
        Ado.Play();
        Ado.loop = false;
        Debug.Log("ROAR!!");
    }
}
