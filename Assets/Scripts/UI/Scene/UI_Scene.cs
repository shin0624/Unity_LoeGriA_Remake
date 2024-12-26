using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
    public override void Init()//Start()에서 소팅 요청 시 실행 x
    {
        Managers.UI.SetCanvas(gameObject, false);//팝업 -> 소팅 미요청
    }

}
