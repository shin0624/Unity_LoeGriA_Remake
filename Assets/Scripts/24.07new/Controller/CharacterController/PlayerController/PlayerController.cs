using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController playerInstance; // �÷��̾� ��Ʈ�ѷ� �̱��� �ν��Ͻ�

//-----------�÷��̾� �ൿ ���� ������ ------
    [Header("Player Settings")]
    [SerializeField] private Animator anim;//�ִϸ����� ������Ʈ
    [SerializeField] private float rotationSpeed = 720.0f;//ȸ�� �ӵ�
    [SerializeField] private float jumpForce = 5.0f;//���� ��
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float runningValue = 1.2f;
    [SerializeField] private Rigidbody rb;//������ٵ� ������Ʈ
    [SerializeField] private Camera mainCam;
    private bool IsJumping;//���� ����

    [Header("Player Attack")]
    [SerializeField] private PlayerAttack playerAttack;//PlayerAttackŬ������ ����
    
    private bool jumpInput; // ���� ��û �÷���
    private Define.PlayerState state; // ���� ����(�÷��̾�)
    private Vector3 moveDirection;// �̵� ����

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

    private void Init()
    {
        if(playerAttack==null)
        {
            playerAttack = GetComponent<PlayerAttack>();//�÷��̾� ���� ������Ʈ�� ���ٸ� �߰�
        }
        gameObject.layer = LayerMask.NameToLayer("Player");  // �÷��̾� ������Ʈ�� Player ���̾�� ����
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
        rb.freezeRotation = true;//������ٵ� ȸ�� ����
        IsJumping = false;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        HandleInput();//�� ������ ȣ��Ǵ� ������Ʈ���� �Է��� ����.
        HandleActionRequest();
    }

    private void FixedUpdate()// �̵��� ������ ������� �������̹Ƿ�, ���������� ������Ʈ �ӵ��� ���� �����ϰ� ȣ��Ǵ� FixedUpdate()���� Move�� Jump�� ȣ���ϴ� ���� �ϰ��� ����, ���� ���鿡�� ȿ����
    {
        HandleMoveRequest();
        HandleJumpInput();
    }
    
    private void HandleActionRequest()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true;// ������û true
        }
        if(Input.GetMouseButtonDown(0))
        {
            CallPlayerAttack(Define.PlayerState.ATTACK, "ATTACK", 0);//���콺 ���� ��ư Ŭ�� ��, �⺻ ���� �޼��� ȣ��
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            CallPlayerAttack(Define.PlayerState.SLASH, "SLASH", 1);//RŰ �Է� ��, ��ų ���� �޼��� ȣ��
        }
        if(Input.GetMouseButtonDown(1))
        {
            CallPlayerAttack(Define.PlayerState.ATTACK02, "ATTACK02", 2);//���콺 ������ ��ư Ŭ�� ��,  �⺻����2 �޼��� ȣ��
        }
    }

    private void HandleInput()// ����� �Է� ���� �޼���. �Է°����� �������� ó���� �и��ϵ��� �Ѵ�.
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");//ī�޶� �������� �̵� ���� ���

        Vector3 forward = mainCam.transform.forward;
        Vector3 right = mainCam.transform.right;

        forward.y =0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        moveDirection = (forward * vertical + right*horizontal).normalized;
    }

    private void CallPlayerAttack(Define.PlayerState NewState, string AnimationTrigger, int attackNumber)//���� �޼��� ȣ��
    {
        if(state!=NewState)//���� ���°� �ƴ� ������ ���� ��û
        {
            SetState(NewState, AnimationTrigger);
            playerAttack.PerformAttack(attackNumber);
        }
    }

    private void HandleMoveRequest()// �̵� �޼���
    {
        float CurrentSpeed = moveSpeed;
        if(moveDirection.magnitude > 0.1f)// �̵����� �� ��ȭ ��, �� �Է����� ���� �̵��� ���� �ÿ� �����ϵ���
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed*Time.deltaTime);

            if (Input.GetKey(KeyCode.LeftShift))// ���� ����Ʈ Ű�� ������ runningValue ��ŭ ���� ���ǵ忡 �������鼭 �޸��� ȿ�� ����. �̿� ���� �ִϸ��̼� Ŭ���� �߰�
            {
                CurrentSpeed *= runningValue;
                SetState(Define.PlayerState.RUNNING, "RUNNING");
            }
            else
            {
                SetState(Define.PlayerState.WALKING, "WALKING");
            }
            rb.MovePosition(rb.position + moveDirection.normalized * CurrentSpeed * Time.fixedDeltaTime);  //Rigidbody�� �̿��Ͽ� �̵��ϰ� �Ͽ� �� ����� ����.
        }
        else
        {
            SetState(Define.PlayerState.IDLE, "IDLE");
        }
    }

    private void HandleJumpInput()// ���� �޼���
    {
        if(jumpInput)//���� Ű �Է� ��
        {
            if (!IsJumping)//���� ���� �ƴϸ�
            {
                SetState(Define.PlayerState.JUMPING, "JUMPING");
                IsJumping = true;//�����ϵ���
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);// ForceMode.Impulse�� ������ ����Ͽ� ������ٵ� �������� �� ����� �߰��ϴ� ���. 
            }
            jumpInput = false;//������û �ʱ�ȭ
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
            anim.ResetTrigger("IDLE");
            anim.ResetTrigger("WALKING");
            anim.ResetTrigger("RUNNING");
            anim.ResetTrigger("JUMPING");
            anim.ResetTrigger("ATTACK");
            anim.SetTrigger(AnimationTrigger);
        }
    }
}
