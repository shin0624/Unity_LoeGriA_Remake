using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class TestCollision : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"collision! @{collision.gameObject.name}");//부딛힌 오브젝트의 이름을 출력
        //호출 조건
        //1. 나 혹은 상대에게 RigidBody가 있어야 함(isKinematic : off)
        //2. 나에게 Collider가 있어야 함(isTrigger : off)
        //3. 상대에게 Collider가 있어야 함(isTrigger : off) 

    }

    private void OnTriggerEnter(Collider other)
        //충돌 범위 내에 물체가 들어갔는지를 판단하는 것(물리와 상관없이)이 트리거
    {
        Debug.Log($"trigger! @{other.gameObject.name}");
        //호출 조건
        //1. 나와 상대에게 모두 Collider가 있어야 함
        //2. 둘 중 하나는 isTrigger : on
        //3. 둘 중 하나는 RigidBody가 있어야 함
    }
   
    void Start()
    {
        
    }

    
    void Update()
    {
#if Raycast
        //RayCasting

        //Vector3.forward를 쓰면 z방향 한방향으로만 Ray가 발사되므로, Player가 바라보는 방향으로 Ray를 쏘기 위해 로컬좌표를 월드좌표로 변환해본다
        //transform.TransformDirection을 사용하여 방향 기준을 Player 시선으로 해준다
        Vector3 look = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position + Vector3.up, look * 10, Color.red);


        Debug.DrawRay(transform.position + Vector3.up, transform.forward*10, Color.red);
        //DrawRay 인자 : 시작위치 Start, Ray크기 Start+direction, Ray의 컬러-->Start+dir 이기때문에 방향과 크기 모두 고려(forward단위벡터 크기 = 1)
        //그냥 transform.position만 넣으면 Player의 가장 아래(발에 해당)에서 Ray가 시작되므로 up (0,1,0)으로 시작위치를 올려준다


        RaycastHit hit;//Ray가 닿은 물체를 리턴할 변수-->out타입으로 연결될 것.

        if( Physics.Raycast(transform.position, Vector3.forward, out hit, 10 ))
        //Vector3.origin : 시작좌표-->Player의 위치 / Vector3.direction : 내가 원하는 방향-->forward단방향 / Maxdistance : 최대 갈 수 있는 거리
        //bool값을 리턴함-->Ray를 쐈을 때 물체에 닿았으면 true

        if (Physics.Raycast(transform.position + Vector3.up, look, out hit, 10))
         {
         Debug.Log($"RayCast{hit.collider.gameObject.name}!");
        //Ray가 충돌한 물체를 hit라고 하고, 이것의 컬라이더가 적용된 게임오브젝트의 이름을 표시하도록
         }

        //한 직선 위의 두 물체를 관통하는 Ray를 원할 때 :

        RaycastHit[] hits; //레이캐스트 배열 사용
         hits = Physics.RaycastAll(transform.position + Vector3.up, look, 10);
         foreach(RaycastHit hit in hits)
         {
             Debug.Log($"RayCast{hit.collider.gameObject.name}!");
        }
        //RayCast 응용 : Player와 카메라 사이가 벽으로 막혀있을 때-->Ray를 쏘아 벽에 닿으면 카메라 위치를 벽보다 Player에게 가까이 할 수 있음 
#endif

//Local <-> World <-> ViewPoint <-> Screen(Pixel) 좌표계 간 변환 --->3인칭 게임에서 마우스 클릭한 위치로 Player를 이동시키고 싶을 때 사용

#if Raycast2
        if (Input.GetMouseButtonDown(0))
        {
            // Debug.Log(Input.mousePosition);//마우스 포인터가 가리키는 위치를 픽셀좌표(스크린 상 좌표)로 변환. 스크린 좌표는 x,y만 갖고있으므로 z축 값은 항상 0
            Debug.Log(Camera.main.ViewportToScreenPoint(Input.mousePosition));//마우스 포인터가 가리키는 좌표를 뷰포인트(0~1사이 비율)로 변환. 스크린과 유사
            Vector3 mouspos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));//스크린 상 좌표를 월드 좌표로 변환. Vector3객체에는 x,y 마우스위치, 메인카메라의 Near값(카메라와 절두체 사이 위치-->근거리)을 인자로 하여 넘겨줌
            Vector3 dir = mouspos - Camera.main.transform.position;//마우스 위치 - 카메라 위치 = 카메라 위치에서 절두체로 가는 방향벡터
            dir = dir.normalized;//normalized를 사용하여 크기를 1로 맞추어준다

            Debug.DrawRay(Camera.main.transform.position, dir * 100.0f, Color.green, 1.0f);//DrayWay로 Ray를 표현

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, dir, out hit, 100.0f))//레이캐스트 되었다면 로그에 표시되도록
            {
               Debug.Log($"RayCast Camera @{hit.collider.gameObject.name}");
            }
        }

#endif
//Ray와 ScreenPointToRay를 사용하는 법
#if Raycast3
        if (Input.GetMouseButtonDown(0))
        {
           
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
            RaycastHit hit;
            if(Physics.Raycast(ray , out hit, 100.0f))
            {
                Debug.Log($"RayCast Camera @{hit.collider.gameObject.name}");
            }

        }
#endif

//특정 레이어만 Raycast되도록 하는 법 (1) 비트 shift연산
#if Raycast4
        int mask = (1 << 8);//8비트 Shift연산-->Layer 8번 "Monster"를 적용한 물체에만 레이캐스팅이 걸리도록 표현. 레이어는 int32형이므로 8번째인 monster레이어에만 적용되도록.
        //9번 레이어를 Wall로 설정하고 바닥(Plain)에 적용했으니, monster, wall 두 레이어에 레이캐스팅 결과를 출력하고자 한다면 int mask = (1 << 8) | (1<<9) 와 같이 OR을 사용
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f, mask))//인자에 mask 추가
            {
                Debug.Log($"RayCast Camera @{hit.collider.gameObject.name}");
            }//-->바닥에 Ray를 쏘았을 때는 아무것도 출력x / monster레이어를 적용한 cube1,cube2를 레이캐스팅했을 때에만 로그가 출력됨

        }
#endif

//특정 레이어만 Raycast되도록 하는 법 (2) LayerMask 형 선언
#if Raycast4_moveToPlayerController
        LayerMask mask = LayerMask.GetMask("Monster");//레이어마스크 형 mask를 선언 후 GetMask()함수로 레이어 이름을 적어도 됨(OR문 적용도 가능)
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f, mask))//인자에 mask 추가
            {
                Debug.Log($"RayCast Camera @{hit.collider.gameObject.name}");
            }//-->바닥에 Ray를 쏘았을 때는 아무것도 출력x / monster레이어를 적용한 cube1,cube2를 레이캐스팅했을 때에만 로그가 출력됨

        }
#endif
    }
}
