using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController playerInstance; // �÷��̾� ��Ʈ�ѷ� �̱��� �ν��Ͻ�

//-----------�÷��̾� �ൿ ���� ������ ------
    [SerializeField] private Animator Anim;//�ִϸ����� ������Ʈ
    [SerializeField, Range(1.0f, 10.0f)] private float RunningSpeed;//�޸��� �ӵ�
    [SerializeField, Range(1.0f, 10.0f)] private float JumpPower;//���� ����
    [SerializeField, Range(1.0f, 10.0f)] private float MoveSpeed;//�̵� �ӵ�
    [SerializeField] private Rigidbody rb;//������ٵ� ������Ʈ
    [SerializeField] private PlayerAttack playerAttack;//PlayerAttackŬ������ ����
    private bool IsJumping;//���� ����
    private bool JumpInput; // ���� ��û �÷���
    private Define.PlayerState state; // ���� ����(�÷��̾�)
    private Vector3 MoveVector;// �̵� ����
    
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
        if(playerAttack==null)
        {
            playerAttack = GetComponent<PlayerAttack>();//�÷��̾� ���� ������Ʈ�� ���ٸ� �߰�
        }
        gameObject.layer = LayerMask.NameToLayer("Player");  // �÷��̾� ������Ʈ�� Player ���̾�� ����
        
        Cursor.lockState = CursorLockMode.Locked;//���콺 Ŀ���� ȭ�� �ȿ��� ����

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
            CallPlayerAttack(Define.PlayerState.ATTACK, "ATTACK", 0);//���콺 ���� ��ư Ŭ�� ��, ���� �޼��� ȣ��
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            CallPlayerAttack(Define.PlayerState.SLASH, "SLASH", 1);//RŰ �Է� ��, ��ų ���� �޼��� ȣ��
        }
        if(Input.GetMouseButtonDown(1))
        {
            CallPlayerAttack(Define.PlayerState.ATTACK02, "ATTACK02", 2);//���콺 ������ ��ư Ŭ�� ��, ��ų ���� �޼��� ȣ��
        }
    }

    private void CallPlayerAttack(Define.PlayerState NewState, string AnimationTrigger, int attackNumber)//���� �޼��� ȣ��
    {
        if(state!=NewState)//���� ���°� �ƴ� ������ ���� ��û
        {
            SetState(NewState, AnimationTrigger);
            playerAttack.PerformAttack(attackNumber);
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Bridge"))// ���� �����ϸ� �������� ����
        {
            IsJumping = false;
        }
    }

    private void SetState(Define.PlayerState NewState, string AnimationTrigger)// �ִϸ��̼� ���� ���� �޼���
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
