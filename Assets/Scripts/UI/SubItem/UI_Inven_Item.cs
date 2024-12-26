using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base
{
    enum GameObjects 
    {
        ItemIcon,
        ItemNameText,
    }

    string _name;//유저가 갖고있는 아이템의 이름

    void Start()
    {
        Init();
    }

    public override void Init()
    {
       Bind<GameObject>(typeof(GameObjects));
        //UI_Inven 스크립트에서의 바인드와 차이 : 각각의 컴포넌트를 찾아서 바인딩하는 것이 아니라, 본 스크립트에서 새로 만든 열거체 내에서 정의된 요소들을 들고있는 GameObject를 바인딩
        //-->즉, ItemIcon과 ItemNameText를 들고있는 오브젝트인 "UI_Inven_Item"을 찾아 바인딩.
        Get<GameObject>((int)GameObjects.ItemNameText).GetComponent<Text>().text = _name;// Get으로 ItemNameText를 가져오고, GetComponent로 Text컴포넌트를 가져와 텍스트를 수정.

        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent((PointerEventData) => { Debug.Log($"아이템 클릭 {_name}"); }) ;//Get으로 ItemIcon을 가져와서 PointerEventData를 받는다. 화면 상에서 아이템 아이콘을 클릭하면 로그가 뜨도록 우선 설정.
    
    }


    public void SetInfo(string name)//유저 아이템의 name을 받아서 Init()의 텍스트수정부분에 넣어준다.
    {
        _name = name;
    }
}
