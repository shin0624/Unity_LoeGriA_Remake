using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//EventSystem�� Ŭ��, �巡�� ���� �̺�Ʈ�� Ž������ �� ��ȣ�� �߻�->UI���� �޾� �ݹ�
public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler//IBeginDragHandler : ������Ʈ �巡�� �� �߻� / IDragHandler : �巡�� ���¿��� �Űܴٴ� �� �߻�
{
    //1) �̹��� �巡�� �̺�Ʈ ����
    //Action�� �̿��Ͽ� �߰��ϰ���� �Լ��� ����--> UI�� ȭ�� ǥ���� ����ϴ� UI_Button���� ȣ���Ͽ� �ٷ� ��.

    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;//InputManager ��ũ��Ʈ������ Invoke�� ����Ͽ� �븮�ڸ� ȣ���ϴ� ���İ� �����ϰ� ����

#if IBeginDragHandler�������̽�
    {
    public void OnBeginDrag(PointerEventData eventData)//IBeginDragHandler�� �������̽�
    {
        if(OnBeginDragHandler!=null)
            OnBeginDragHandler.Invoke(eventData);//OnBeginDragHandler�� null�� �ƴ� ��� = "OnBeginDragHandler ȣ�� ��" �� �ǹ�.-->invoke�� ������ ����
    }
#endif

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
            OnClickHandler.Invoke(eventData);
    }



    public void OnDrag(PointerEventData eventData)//IDragHandler�� �������̽�
    {
        //transform.position = eventData.position;//�巡�� ��(���콺 Ŭ������) ���콺 ��ġ ��ȯ-->�巡�� �� ������Ʈ �̵� ����
        //���� Ʈ������ ��ȯ�� UI_Button�� ���ٽ����� �Ű���
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }

   
}
