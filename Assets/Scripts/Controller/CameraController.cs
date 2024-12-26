using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public Define.CameraMode _mode = Define.CameraMode.FirstPersonView;//Define에서 정의한 카메라모드 중 쿼터뷰를 기본으로 적용
    
    [SerializeField]
    public Vector3 _delta = new Vector3(0.0f, 2.0f, -10.0f);//Player 기준으로 얼마나 떨어져있는지에 대한 방향벡터
    
    [SerializeField]
    public GameObject _player = null;//카메라가 적용될 플레이어

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

    void LateUpdate()//카메라 컨트롤 연산을 Update문에 넣으면, PlayerController의 Update문 내에 있는 버튼이벤트와 실행 순서가 섞임-->플레이어 이동 시 떨림 발생
        //LateUpdate()는 무조건 Update()문이 끝난 후에 실행되므로 실행 순서를 고정할 수 있다.
    {
       
        if (_mode == Define.CameraMode.QuarterView)
        {
            
            
            //카메라 시야를 오브젝트가 막고있어 Player가 보이지 않을 때 카메라가 오브젝트를 통과하도록 구현 
            RaycastHit hit;
            if(Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall_entrance (1)")))
            {//만약 카메라 시야를 오브젝트가 막고있다면, 우선 Player와 오브젝트 간 거리를 구한다
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;//Ray가 충돌한 좌표인 hit.point에서 Player의 위치를 빼면 방향벡터가 나올 것이고, magnitude를 하면 방향벡터의 크기가 나올 것. 이 값보다 조금 더 앞으로 당겨서 Player를 비출 것이므로 작은 상수값을 곱해준다. 
              //새로 바뀔 카메라 위치 = Player위치를 기준으로 _delta가 normalized된 방향 * dist
                transform.position = _player.transform.position + _delta.normalized * dist;
            }
            else
            {    
                transform.position = _player.transform.position + _delta;//카메라 포지선 = 플레이어 포지션 + 방향벡터-->카메라가 플레이어를 따라 이동
                transform.LookAt(_player.transform);//LookAt()함수 : 카메라가 무조건 플레이어의 좌표를 주시하도록 함
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

    public void SetQuarterView(Vector3 delta)//QuarterView를 코드상으로 세팅하고자 할 때 사용
    {
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
    }
}
