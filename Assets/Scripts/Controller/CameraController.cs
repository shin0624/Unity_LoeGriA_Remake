using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public Define.CameraMode _mode = Define.CameraMode.FirstPersonView;//Define���� ������ ī�޶��� �� ���ͺ並 �⺻���� ����
    
    [SerializeField]
    public Vector3 _delta = new Vector3(0.0f, 2.0f, -10.0f);//Player �������� �󸶳� �������ִ����� ���� ���⺤��
    
    [SerializeField]
    public GameObject _player = null;//ī�޶� ����� �÷��̾�

    [Header("FPV")]
    public float mouseSpeed;
    public float rotationSmoothTime = 0.01f;
    private float smoothXRotation;
    private float smoothYRotation;
    private float yRotation;
    private float xRotation;
    public Camera cam;
    public Vector3 fpvOffset = new Vector3(0.0f, 2.026f, -4.756f);

    void Start()
    {
        if(_mode ==Define.CameraMode.FirstPersonView)
        {
            cam = Camera.main;
            cam.transform.SetParent(_player.transform);
            cam.transform.localPosition = fpvOffset;
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        }
    }

    void LateUpdate()//ī�޶� ��Ʈ�� ������ Update���� ������, PlayerController�� Update�� ���� �ִ� ��ư�̺�Ʈ�� ���� ������ ����-->�÷��̾� �̵� �� ���� �߻�
        //LateUpdate()�� ������ Update()���� ���� �Ŀ� ����ǹǷ� ���� ������ ������ �� �ִ�.
    {
       
        if (_mode == Define.CameraMode.QuarterView)
        {
            
            
            //ī�޶� �þ߸� ������Ʈ�� �����־� Player�� ������ ���� �� ī�޶� ������Ʈ�� ����ϵ��� ���� 
            RaycastHit hit;
            if(Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall_entrance (1)")))
            {//���� ī�޶� �þ߸� ������Ʈ�� �����ִٸ�, �켱 Player�� ������Ʈ �� �Ÿ��� ���Ѵ�
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;//Ray�� �浹�� ��ǥ�� hit.point���� Player�� ��ġ�� ���� ���⺤�Ͱ� ���� ���̰�, magnitude�� �ϸ� ���⺤���� ũ�Ⱑ ���� ��. �� ������ ���� �� ������ ��ܼ� Player�� ���� ���̹Ƿ� ���� ������� �����ش�. 
              //���� �ٲ� ī�޶� ��ġ = Player��ġ�� �������� _delta�� normalized�� ���� * dist
                transform.position = _player.transform.position + _delta.normalized * dist;
            }
            else
            {    
                transform.position = _player.transform.position + _delta;//ī�޶� ������ = �÷��̾� ������ + ���⺤��-->ī�޶� �÷��̾ ���� �̵�
                transform.LookAt(_player.transform);//LookAt()�Լ� : ī�޶� ������ �÷��̾��� ��ǥ�� �ֽ��ϵ��� ��
            }
        }
        else if(_mode ==Define.CameraMode.FirstPersonView)
        {
             if (cam.transform.parent != _player.transform)
            {
                cam.transform.SetParent(_player.transform);
                cam.transform.localPosition = fpvOffset;
                cam.transform.localRotation = Quaternion.identity;
            }

            float mouseX = Input.GetAxisRaw("Mouse X") * mouseSpeed * Time.deltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSpeed * Time.deltaTime;

            smoothXRotation = Mathf.SmoothDamp(smoothXRotation, mouseX, ref smoothXRotation, rotationSmoothTime);
            smoothYRotation = Mathf.SmoothDamp(smoothYRotation, mouseY, ref smoothYRotation, rotationSmoothTime);

            yRotation += smoothXRotation;
            xRotation -= smoothYRotation;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            _player.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        
        }

        
    }

    public void SetQuarterView(Vector3 delta)//QuarterView�� �ڵ������ �����ϰ��� �� �� ���
    {
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
    }
}
