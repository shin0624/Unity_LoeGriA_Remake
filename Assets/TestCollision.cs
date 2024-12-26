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
        Debug.Log($"collision! @{collision.gameObject.name}");//�ε��� ������Ʈ�� �̸��� ���
        //ȣ�� ����
        //1. �� Ȥ�� ��뿡�� RigidBody�� �־�� ��(isKinematic : off)
        //2. ������ Collider�� �־�� ��(isTrigger : off)
        //3. ��뿡�� Collider�� �־�� ��(isTrigger : off) 

    }

    private void OnTriggerEnter(Collider other)
        //�浹 ���� ���� ��ü�� �������� �Ǵ��ϴ� ��(������ �������)�� Ʈ����
    {
        Debug.Log($"trigger! @{other.gameObject.name}");
        //ȣ�� ����
        //1. ���� ��뿡�� ��� Collider�� �־�� ��
        //2. �� �� �ϳ��� isTrigger : on
        //3. �� �� �ϳ��� RigidBody�� �־�� ��
    }
   
    void Start()
    {
        
    }

    
    void Update()
    {
#if Raycast
        //RayCasting

        //Vector3.forward�� ���� z���� �ѹ������θ� Ray�� �߻�ǹǷ�, Player�� �ٶ󺸴� �������� Ray�� ��� ���� ������ǥ�� ������ǥ�� ��ȯ�غ���
        //transform.TransformDirection�� ����Ͽ� ���� ������ Player �ü����� ���ش�
        Vector3 look = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position + Vector3.up, look * 10, Color.red);


        Debug.DrawRay(transform.position + Vector3.up, transform.forward*10, Color.red);
        //DrawRay ���� : ������ġ Start, Rayũ�� Start+direction, Ray�� �÷�-->Start+dir �̱⶧���� ����� ũ�� ��� ���(forward�������� ũ�� = 1)
        //�׳� transform.position�� ������ Player�� ���� �Ʒ�(�߿� �ش�)���� Ray�� ���۵ǹǷ� up (0,1,0)���� ������ġ�� �÷��ش�


        RaycastHit hit;//Ray�� ���� ��ü�� ������ ����-->outŸ������ ����� ��.

        if( Physics.Raycast(transform.position, Vector3.forward, out hit, 10 ))
        //Vector3.origin : ������ǥ-->Player�� ��ġ / Vector3.direction : ���� ���ϴ� ����-->forward�ܹ��� / Maxdistance : �ִ� �� �� �ִ� �Ÿ�
        //bool���� ������-->Ray�� ���� �� ��ü�� ������� true

        if (Physics.Raycast(transform.position + Vector3.up, look, out hit, 10))
         {
         Debug.Log($"RayCast{hit.collider.gameObject.name}!");
        //Ray�� �浹�� ��ü�� hit��� �ϰ�, �̰��� �ö��̴��� ����� ���ӿ�����Ʈ�� �̸��� ǥ���ϵ���
         }

        //�� ���� ���� �� ��ü�� �����ϴ� Ray�� ���� �� :

        RaycastHit[] hits; //����ĳ��Ʈ �迭 ���
         hits = Physics.RaycastAll(transform.position + Vector3.up, look, 10);
         foreach(RaycastHit hit in hits)
         {
             Debug.Log($"RayCast{hit.collider.gameObject.name}!");
        }
        //RayCast ���� : Player�� ī�޶� ���̰� ������ �������� ��-->Ray�� ��� ���� ������ ī�޶� ��ġ�� ������ Player���� ������ �� �� ���� 
#endif

//Local <-> World <-> ViewPoint <-> Screen(Pixel) ��ǥ�� �� ��ȯ --->3��Ī ���ӿ��� ���콺 Ŭ���� ��ġ�� Player�� �̵���Ű�� ���� �� ���

#if Raycast2
        if (Input.GetMouseButtonDown(0))
        {
            // Debug.Log(Input.mousePosition);//���콺 �����Ͱ� ����Ű�� ��ġ�� �ȼ���ǥ(��ũ�� �� ��ǥ)�� ��ȯ. ��ũ�� ��ǥ�� x,y�� ���������Ƿ� z�� ���� �׻� 0
            Debug.Log(Camera.main.ViewportToScreenPoint(Input.mousePosition));//���콺 �����Ͱ� ����Ű�� ��ǥ�� ������Ʈ(0~1���� ����)�� ��ȯ. ��ũ���� ����
            Vector3 mouspos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));//��ũ�� �� ��ǥ�� ���� ��ǥ�� ��ȯ. Vector3��ü���� x,y ���콺��ġ, ����ī�޶��� Near��(ī�޶�� ����ü ���� ��ġ-->�ٰŸ�)�� ���ڷ� �Ͽ� �Ѱ���
            Vector3 dir = mouspos - Camera.main.transform.position;//���콺 ��ġ - ī�޶� ��ġ = ī�޶� ��ġ���� ����ü�� ���� ���⺤��
            dir = dir.normalized;//normalized�� ����Ͽ� ũ�⸦ 1�� ���߾��ش�

            Debug.DrawRay(Camera.main.transform.position, dir * 100.0f, Color.green, 1.0f);//DrayWay�� Ray�� ǥ��

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, dir, out hit, 100.0f))//����ĳ��Ʈ �Ǿ��ٸ� �α׿� ǥ�õǵ���
            {
               Debug.Log($"RayCast Camera @{hit.collider.gameObject.name}");
            }
        }

#endif
//Ray�� ScreenPointToRay�� ����ϴ� ��
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

//Ư�� ���̾ Raycast�ǵ��� �ϴ� �� (1) ��Ʈ shift����
#if Raycast4
        int mask = (1 << 8);//8��Ʈ Shift����-->Layer 8�� "Monster"�� ������ ��ü���� ����ĳ������ �ɸ����� ǥ��. ���̾�� int32���̹Ƿ� 8��°�� monster���̾�� ����ǵ���.
        //9�� ���̾ Wall�� �����ϰ� �ٴ�(Plain)�� ����������, monster, wall �� ���̾ ����ĳ���� ����� ����ϰ��� �Ѵٸ� int mask = (1 << 8) | (1<<9) �� ���� OR�� ���
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f, mask))//���ڿ� mask �߰�
            {
                Debug.Log($"RayCast Camera @{hit.collider.gameObject.name}");
            }//-->�ٴڿ� Ray�� ����� ���� �ƹ��͵� ���x / monster���̾ ������ cube1,cube2�� ����ĳ�������� ������ �αװ� ��µ�

        }
#endif

//Ư�� ���̾ Raycast�ǵ��� �ϴ� �� (2) LayerMask �� ����
#if Raycast4_moveToPlayerController
        LayerMask mask = LayerMask.GetMask("Monster");//���̾��ũ �� mask�� ���� �� GetMask()�Լ��� ���̾� �̸��� ��� ��(OR�� ���뵵 ����)
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f, mask))//���ڿ� mask �߰�
            {
                Debug.Log($"RayCast Camera @{hit.collider.gameObject.name}");
            }//-->�ٴڿ� Ray�� ����� ���� �ƹ��͵� ���x / monster���̾ ������ cube1,cube2�� ����ĳ�������� ������ �αװ� ��µ�

        }
#endif
    }
}
