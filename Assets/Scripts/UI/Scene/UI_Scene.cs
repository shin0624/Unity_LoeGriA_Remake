using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
    public override void Init()//Start()���� ���� ��û �� ���� x
    {
        Managers.UI.SetCanvas(gameObject, false);//�˾� -> ���� �̿�û
    }

}
