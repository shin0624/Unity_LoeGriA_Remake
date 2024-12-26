using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Scene
{   //인벤토리 구현
    // Init() 실행 --> 실제 인벤토리를 오픈할 때, 유저가 갖고있는 정보를 참조하여 반복문 내에 넣어주어야 함
    enum GameObjects 
    { 
        GridPanel

    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));//게임오브젝트 타입을 바인딩

        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);//유니티엔진에서 프리팹" UI_Inven"에 매핑된 GridPanel을 뽑아온다.
        foreach(Transform child in gridPanel.transform)//게임오브젝트가 들고있는 자식을 모두 순회하며,
            Managers.Resource.Destroy(child.gameObject);//만들어놓았던 Destroy를 사용하여 모두 제거(현재 프리팹에 임시로 넣어놓았던 자식 오브젝트들이 있으므로 모두 삭제)

        
        for(int i=0; i<8; i++)//채우고자 하는 아이템 개수만큼 반복하며
        {
            Managers.UI.MakeSubItem<UI_Inven_Item>();

            //UI_Inven_Item을 생성해서 UI_Inven/GridPanel 프리팹의 자식으로 붙여주어야 함.
            //GameObject item = Managers.Resource.Instantiate("UI/Scene/UI_Inven_Item");//Instantiate를 사용하여 아이템 프리팹을 생성 -->지정 경로에 있는 동일명 프리팹을 이용하여 아이템을 생성할 수 있다.
          
            GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(gridPanel.transform).gameObject;

            //item.transform.SetParent(gridPanel.transform);//위에서 생성한 아이템 프리팹을 SetParent()를 이용하여 gridPanel의 자식으로 지정한다.
            //--> MakeSubItem의 매개변수로 부모 트랜스폼을 추가했으므로 위 구문은 삭제

            UI_Inven_Item invenItem =  item.GetOrAddComponent<UI_Inven_Item>(); //-->또는 프리팹 인스펙터에서 UI_Inven스크립트를 컴포넌트로 추가해도 됨 -->해당 구문 또는 컴포넌트 추가까지 완료해야 유니티 실행 시 ui가 출력됨
            invenItem.SetInfo($"무기{i}번");
            //-->GetOrAddComponent를 GameObject 객체에서 호출하도록 변경-->해당 메서드를 Extension Method로 만들어 GameObject객체에서 접근 가능
        
        }
    }


  
}
