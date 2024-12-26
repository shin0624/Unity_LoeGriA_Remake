using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//EventSystem이 클릭, 드래그 등의 이벤트를 탐지했을 때 신호를 발생->UI에서 받아 콜백
public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler//IBeginDragHandler : 오브젝트 드래그 시 발생 / IDragHandler : 드래그 상태에서 옮겨다닐 때 발생
{
    //1) 이미지 드래그 이벤트 구현
    //Action을 이용하여 추가하고싶은 함수를 연동--> UI의 화면 표현을 담당하는 UI_Button에서 호출하여 다룰 것.

    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;//InputManager 스크립트에서의 Invoke를 사용하여 대리자를 호출하는 형식과 유사하게 진행

#if IBeginDragHandler인터페이스
    {
    public void OnBeginDrag(PointerEventData eventData)//IBeginDragHandler의 인터페이스
    {
        if(OnBeginDragHandler!=null)
            OnBeginDragHandler.Invoke(eventData);//OnBeginDragHandler가 null이 아닌 경우 = "OnBeginDragHandler 호출 시" 를 의미.-->invoke로 데이터 전달
    }
#endif

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
            OnClickHandler.Invoke(eventData);
    }



    public void OnDrag(PointerEventData eventData)//IDragHandler의 인터페이스
    {
        //transform.position = eventData.position;//드래그 중(마우스 클릭상태) 마우스 위치 반환-->드래그 시 오브젝트 이동 가능
        //위의 트랜스폼 변환은 UI_Button의 람다식으로 옮겨짐
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }

   
}
