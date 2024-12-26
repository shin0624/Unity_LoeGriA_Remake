using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Popup : UI_Base
{
    public override void Init()//Start()���� ���� ��û �� ���� x
    {
        Managers.UI.SetCanvas(gameObject, true);//�˾� -> ���� ��û
    }

    public virtual void ClosePopupUI()//UI_Popup�� ��ӹ��� ������Ʈ���� ClosePopupUI ȣ�� �� �ڵ����� Managers�� ClosePopupUI ����
    {
        Managers.UI.ClosePopupUI(this); 
    }
}
