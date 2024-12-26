using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager//싱글톤으로 구현된 Managers가 이미 있으므로 InputManager는 일반 스크립트로 생성
{
    public Action KeyAction = null;//델리게이트 -->업데이트에서 인풋매니저가 입력을 체크하고 입력이 있다면 전파함
    public Action<Define.MouseEvent> MouseAction = null;//마우스 이벤트를 종류별로 분류하여 실행할 수 있도록

    bool _pressed = false;//마우스 버튼 눌림 유무

    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())//UI버튼 클릭 여부 판단을 위해 EventSystem을 사용한 조건 추가
            return;//UI가 클릭된 상황이면 바로 리턴(게임 화면 내 UI버튼 클릭 시 캐릭터 이동으로 간주되지 않도록)


        if (Input.anyKey && KeyAction != null)
        {
            KeyAction.Invoke();
        }

        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                MouseAction.Invoke(Define.MouseEvent.Press);//마우스가 눌렸다면 Press
                _pressed = true;
            }
            else
            {
                if (_pressed)//기존에 한번이라도 Press되었다면 Click이벤트 발생
                    MouseAction.Invoke(Define.MouseEvent.Click);
                _pressed = false;
            }

        }
        //만약 Dragged상태를 추가하고자 한다면 GetMouseButton(0)상태가 일정시간 지속되면 Dragged상태로 변환
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
