using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController playerInstance; // �÷��̾� ��Ʈ�ѷ� �̱��� �ν��Ͻ�

    [SerializeField]
    private Animator Anim;//�ִϸ����� ������Ʈ

    [SerializeField, Range(1.0f, 10.0f)]
    private float RunningSpeed;//�޸��� �ӵ�

    [SerializeField, Range(1.0f, 10.0f)]
    private float JumpPower;//���� ����

    [SerializeField, Range(1.0f, 10.0f)]
    private float MoveSpeed;//�̵� �ӵ�

    [SerializeField]
    private Rigidbody rb;//������ٵ� ������Ʈ

    private bool IsJumping;//���� ����
    private bool JumpInput; // ���� ��û �÷���

    private Define.PlayerState state; // ���� ����(�÷��̾�)
    private Vector3 MoveVector;// �̵� ����

    public static bool HitForEnemy;

    private void Awake()
    {
        if(playerInstance == null)
        {
            playerInstance = this;
            DontDestroyOnLoad(gameObject);//���� ��ȯ�Ǿ �÷��̾ ������� �ʵ��� ��ġ�� 
        }
        else
        {
            Destroy(gameObject);
            return; //�̹� �ν��Ͻ��� �����Ѵٸ� ����
        }
    }

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;//���콺 Ŀ���� ȭ�� �ȿ��� ����
        //Cursor.visible = false;//Ŀ���� �Ⱥ��̰� ����
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;//������ٵ� ȸ�� ����
        IsJumping = false;
    }

    void Update()
    {
        HandleInput();//�� ������ ȣ��Ǵ� ������Ʈ���� �Է��� ����.
    }

    private void FixedUpdate()// �̵��� ������ ������� �������̹Ƿ�, ���������� ������Ʈ �ӵ��� ���� �����ϰ� ȣ��Ǵ� FixedUpdate()���� Move�� Jump�� ȣ���ϴ� ���� �ϰ��� ����, ���� ���鿡�� ȿ����
    {
            Move();
            Jump();
    }

    private void HandleInput()// ����� �Է� ���� �޼���. �Է°����� �������� ó���� �и��ϵ��� �Ѵ�.
    {
        float h = Input.GetAxisRaw("Horizontal");// �����̵� �Է°�
        float v = Input.GetAxisRaw("Vertical");// �����̵� �Է°� 

        MoveVector = transform.forward * v + transform.right * h;//�Է¿� ���� �̵� ������ ���� ���

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpInput = true;// ������û true
        }
        if(Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Move()// �̵� �޼���
    {
        float CurrentSpeed = MoveSpeed;
        if(MoveVector.magnitude > 0)// �̵����� �� ��ȭ ��, �� �Է����� ���� �̵��� ���� �ÿ� �����ϵ���
        {
            if (Input.GetKey(KeyCode.LeftShift))// ���� ����Ʈ Ű�� ������ RunningSpeed ��ŭ ���� ���ǵ忡 �������鼭 �޸��� ȿ�� ����. �̿� ���� �ִϸ��̼� Ŭ���� �߰�
            {
                CurrentSpeed *= RunningSpeed;
                SetState(Define.PlayerState.RUNNING, "RUNNING");
            }
            else
            {
                SetState(Define.PlayerState.WALKING, "WALKING");
            }
            rb.MovePosition(rb.position + MoveVector.normalized * CurrentSpeed * Time.fixedDeltaTime);  //Rigidbody�� �̿��Ͽ� �̵��ϰ� �Ͽ� �� ����� ����.
        }
        else
        {
            SetState(Define.PlayerState.IDLE, "IDLE");
        }
    }

    private void Jump()// ���� �޼���
    {
        if(JumpInput)//���� Ű �Է� ��
        {
            if (!IsJumping)//���� ���� �ƴϸ�
            {
                SetState(Define.PlayerState.JUMPING, "JUMPING");
                IsJumping = true;//�����ϵ���
                rb.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);// ForceMode.Impulse�� ������ ����Ͽ� ������ٵ� �������� �� ����� �߰��ϴ� ���. 
            }
            JumpInput = false;//������û �ʱ�ȭ
        }
    }

    private void Attack()// ���� �޼���
    {
        SetState(Define.PlayerState.ATTACK, "ATTACK");
        Anim.SetBool("IsAttack", true);
        // KnockBackȿ�� ����
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5.0f))// ���� �� ���ʹ̰� ���� ���� �ִ��� �Ǵ�
        {
            if(hit.collider.CompareTag("Enemy"))
            {
                Rigidbody EnemyRB = hit.collider.GetComponent<Rigidbody>();// ���� ���� �� ���ʹ̰� �¾Ҵٸ� ���ʹ��� rb�� �����´�.
                Debug.Log($"Enemy RB = {EnemyRB}");
                if(EnemyRB!=null)
                {
                    Vector3 KnockBackDirection = hit.collider.transform.position - -transform.position;// �ڷ� �˹�Ǵ� ������ �� �ݶ��̴� ��ġ - �� ��ġ��ŭ
                    KnockBackDirection.y = 0;// ���������� ������ ���� �ʰ�. ����������θ� �˹� �߻�

                    float KnockBackForce = 0.5f;// �˹� ��
                    EnemyRB.AddForce(KnockBackDirection* KnockBackForce, ForceMode.Impulse);//���� rb�� ���� ���Ѵ�. ������ �浹�� impulse�� ���
                    HitForEnemy = true;
                }
            }
        }
    }

    private void ResetAttack()
    {
        Anim.SetBool("IsAttack", false); // ���� �ִϸ��̼� ����
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Bridge"))// ���� �����ϸ� �������� ����
        {
            IsJumping = false;
        }

    }

    private void SetState(Define.PlayerState NewState, string AnimationTrigger)// �ִϸ��̼� ���� ���� �޼���
    {
        if (state != NewState) { state = NewState;  Anim.SetTrigger(AnimationTrigger); }
    }


}
