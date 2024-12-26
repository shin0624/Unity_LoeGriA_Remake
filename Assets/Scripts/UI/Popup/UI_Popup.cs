using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Popup : UI_Base
{
    public override void Init()//Start()에서 소팅 요청 시 실행 x
    {
        Managers.UI.SetCanvas(gameObject, true);//팝업 -> 소팅 요청
    }

    public virtual void ClosePopupUI()//UI_Popup을 상속받은 컴포넌트들은 ClosePopupUI 호출 시 자동으로 Managers의 ClosePopupUI 실행
    {
        Managers.UI.ClosePopupUI(this); 
    }
}
