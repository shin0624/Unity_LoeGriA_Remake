using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager//�̱������� ������ Managers�� �̹� �����Ƿ� InputManager�� �Ϲ� ��ũ��Ʈ�� ����
{
    public Action KeyAction = null;//��������Ʈ -->������Ʈ���� ��ǲ�Ŵ����� �Է��� üũ�ϰ� �Է��� �ִٸ� ������
    public Action<Define.MouseEvent> MouseAction = null;//���콺 �̺�Ʈ�� �������� �з��Ͽ� ������ �� �ֵ���

    bool _pressed = false;//���콺 ��ư ���� ����

    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())//UI��ư Ŭ�� ���� �Ǵ��� ���� EventSystem�� ����� ���� �߰�
            return;//UI�� Ŭ���� ��Ȳ�̸� �ٷ� ����(���� ȭ�� �� UI��ư Ŭ�� �� ĳ���� �̵����� ���ֵ��� �ʵ���)


        if (Input.anyKey && KeyAction != null)
        {
            KeyAction.Invoke();
        }

        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                MouseAction.Invoke(Define.MouseEvent.Press);//���콺�� ���ȴٸ� Press
                _pressed = true;
            }
            else
            {
                if (_pressed)//������ �ѹ��̶� Press�Ǿ��ٸ� Click�̺�Ʈ �߻�
                    MouseAction.Invoke(Define.MouseEvent.Click);
                _pressed = false;
            }

        }
        //���� Dragged���¸� �߰��ϰ��� �Ѵٸ� GetMouseButton(0)���°� �����ð� ���ӵǸ� Dragged���·� ��ȯ
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
