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
    private bool IsJumping;//���� ����
    private bool JumpInput; // ���� ��û �÷���
    private Define.PlayerState state; // ���� ����(�÷��̾�)
    private Vector3 MoveVector;// �̵� ����

//-----------���ʹ� �˹� ���� ������------
    [SerializeField] private float attackRange = 5.0f; // ���� ����
    [SerializeField] private float knockBackForce = 500.0f; // �˹� ��
    [SerializeField] private LayerMask enemyLayer; // �� ���̾�
    [SerializeField] private float attackDelay = 0.5f; // ���� ������
    
    
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
        gameObject.layer = LayerMask.NameToLayer("Player");  // �÷��̾� ������Ʈ�� Player ���̾�� ����

        Cursor.lockState = CursorLockMode.Locked;//���콺 Ŀ���� ȭ�� �ȿ��� ����
        Cursor.visible = false;//Ŀ���� �Ⱥ��̰� ����

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
            StartCoroutine(AttackRountine());//���� �ڷ�ƾ ����
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

    

    private IEnumerator AttackRountine()// Attack�޼��带 �ڷ�ƾ���� ����. �ִϸ��̼� Ŭ���� ���� ���� ���� �� ���� �ؼ�
    {
        SetState(Define.PlayerState.ATTACK, "ATTACK");
        Anim.SetBool("IsAttack", true);

        yield return new WaitForSeconds(attackDelay);//���� ������ ������ ��ŭ ��� �� ���.

        //���� ���� ����
        Vector3 rayOrigin = transform.position + Vector3.up * 1.5f;
        Vector3 rayDirection = transform.forward;
        float rayRadius = 1.0f;//��ü ����ĳ��Ʈ�� ������

        //��ü ����ĳ��Ʈ�� ����Ͽ� ���� ���� ����. ���� ����ĳ��Ʈ���� �� �ڿ������� ���� ���� ����
        RaycastHit[] hits = Physics.SphereCastAll(rayOrigin, rayRadius, rayDirection, attackRange, enemyLayer);// ���ʹ� ���̾ ���� �����ؼ� ���ʹ̿��Ը� ȿ���� ���������� ��.
        
        //����� �ð�ȭ
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
                    //�Ÿ��� ���� �˹� �� ����
                      float distanceMultiplier = 1 - (hit.distance / attackRange);
                      float finalKnockbackForce = knockBackForce * distanceMultiplier; // Ÿ�� �������� �� ���� �˹� ȿ���� ������

                    damageable.OnHit(10.0f, hit.point, hit.normal, finalKnockbackForce);//10�� ������, ������ ������ ��ġ, �浹 ǥ���� ��������, �з������� ���� ũ�⸦ �Ű������� ����.
                    // ray�� ���� �ݶ��̴��� ���� ���ʹ��� idamageable �������̽��� ã�Ƽ� OnHit �޼��带 ȣ��
                }

                // if(enemyRb!=null)
                // {
                //     //���� �������� �˹� ���� ���
                //     Vector3 knockbackDirection = (hit.collider.transform.position - transform.position).normalized;
                //     knockbackDirection.y = 0;

                //     //�Ÿ��� ���� �˹� �� ����
                //     float distanceMultiplier = 1 - (hit.distance / attackRange);
                //     float finalKnockbackForce = knockBackForce * distanceMultiplier; // Ÿ�� �������� �� ���� �˹� ȿ���� ������

                //     enemyRb.AddForce(knockbackDirection * finalKnockbackForce, ForceMode.Impulse);//�˹� ����. Impulse�� �������� ���� ���ϴ� ���   
                // }
            }
        }
        yield return new WaitForSeconds(Anim.GetCurrentAnimatorStateInfo(0).length);//�ִϸ��̼� Ŭ�� ���̸�ŭ ���
        ResetAttack();//���� �ִϸ��̼� ����
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
