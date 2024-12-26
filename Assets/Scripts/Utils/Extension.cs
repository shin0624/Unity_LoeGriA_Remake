using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension //Extension Method �׽�Ʈ
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
       
        return Util.GetOrAddComponent<T>(go);
    }

    public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);//-->UI_Button���� BindEvent�� ���̴� �۾��� �ܼ�ȭ�ϱ� ���� ���, Click���� �����
    }


}
