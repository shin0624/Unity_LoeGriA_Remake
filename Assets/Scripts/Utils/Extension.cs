using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension //Extension Method 테스트
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
       
        return Util.GetOrAddComponent<T>(go);
    }

    public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);//-->UI_Button에서 BindEvent를 붙이는 작업을 단순화하기 위해 사용, Click에서 사용함
    }


}
