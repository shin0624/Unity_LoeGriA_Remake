using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    //UI컴포넌트에 있는 Canvas --> SortOrder 관리를 위해 생성. 
    //UI를 팝업용, 씬 용으로 나누고, 매니저에서 팝업의 on/off 요청 시 UI별 sortorder를 저장했다가 연동시켜야 함
    //가장 마지막에 띄워진 팝업을 먼저 삭제해야 하므로, LIFO인 스택구조로 팝업 관리

    //사용 시 --> Managers.UI.ShowPopupUI<UI_Button>(); Managers.UI.ClosePopupUI(ui);
    int _order = 10;//최근에 사용한 order를 저장

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();//게임오브젝트에 해당하는 컴포넌트를 관리하는 스택 _popupStack 생성
    UI_Scene _sceneUI = null;

    public GameObject Root //UI를 생성할 때 Hierarchy상에 빈 오브젝트 UI_Root를 생성하여 폴더처럼 사용. UI_Root 아래에 팝업들이 위치할 것
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");// --> @UI_Root 오브젝트를 찾아본다
            if (root == null)//@UI_Root 오브젝트가 없다면
                root = new GameObject { name = "@UI_Root" };// 오브젝트를 새로 만든다
            return root;
        }

    }


    public void  SetCanvas(GameObject go, bool sort = true)//외부에서 팝업 등의 UI가 켜질 때 UIManager에게 해당 팝업의 _order를 채우는 요청으로 UI와의 우선순위를 정하기 위한 메서드
    {
      Canvas canvas =   Util.GetOrAddComponent<Canvas>(go);//캔버스 객체를 GetOrAddComponent로 뽑아옴
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;//오버라이드 소팅 : 캔버스 안에 캔버스가 중첩해서 있을 때, 그 부모가 어떤 값을 가지던 해당 캔버스는 자신만의 sort order를 갖는 옵션.
      
        if (sort)//소팅 요청 시 --> canvas의 sort order를 _order로 바꾸고 ++해준다.
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else//소팅 미요청 시 --> UI_Popup과 관련없는 일반 UI. 즉 경험치 창, 체력 창 등 기본으로 표시되는 UI일 경우
        {
            canvas.sortingOrder = 0;
        }
            
    }


    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base// inven, Scene 이외의 서브아이템 생성 시 접근하는 메서드
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;//만약 name을 넣지 않는다면 T의 이름을 그대로 사용하도록, 넣어준 타입 이름을 그대로 사용할 수 있는 typeof(T).Name을 사용
        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        return go.GetOrAddComponent<T>();
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    //name에는 Asset/Resources/UI/Popup에 있는 프리팹의 이름을 건네줌, T에는 Script/UI/Popup에 있는 UI_Button 스크립트를 건네줌
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;//만약 name을 넣지 않는다면 T의 이름을 그대로 사용하도록, 넣어준 타입 이름을 그대로 사용할 수 있는 typeof(T).Name을 사용

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");

         T popup =Util.GetOrAddComponent<T>(go);//컴포넌트를 가져온다. 없으면 해당 컴포넌트를 추가 후 가져옴

        _popupStack.Push(popup);
        //_order++; --> ShowPopup으로 띄운 팝업이 아니라, Hierarchy에 드래그드롭으로 씬에 UI팝업을 만들어놓게 되면 _order++ 처리 불가-->UI_Popup에서 ++처리를 하도록 할 것.

        go.transform.SetParent(Root.transform);//게임오브젝트 go의 부모는 root로 지정
        
        return popup;
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene   //씬에 표시되는 기본 UI 호출을 위한 메서드
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;//만약 name을 넣지 않는다면 T의 이름을 그대로 사용하도록, 넣어준 타입 이름을 그대로 사용할 수 있는 typeof(T).Name을 사용

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");

        T sceneUI = Util.GetOrAddComponent<T>(go);//컴포넌트를 가져온다. 없으면 해당 컴포넌트를 추가 후 가져옴
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);//게임오브젝트 go의 부모는 root로 지정

        return sceneUI;
    }





    public void ClosePopupUI(UI_Popup popup)//팝업이 순서대로 삭제되지 않을 경우를 위한 메서드-->삭제될 차례의 팝업이 맞는지 테스트
    {
        if (_popupStack.Count == 0) return;

        if (_popupStack.Peek() != popup)//Peek : 마지막 요소를 엿본다.
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        CloseAllPopupUI();
    }


    public void ClosePopupUI()//팝업을 닫는 메서드-->스택 요소를 하나씩 추출하면서 닫는다
    {
        if(_popupStack.Count==0) return;//만약 하나도 안들어있다면 바로 리턴
        
        UI_Popup popup = _popupStack.Pop();//하나라도 들어있다면 Pop. 가장 최근에 띄운 팝업을 popup에 넣고 Destroy한다.
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        _order--;
    }
    public void CloseAllPopupUI()//스택 내 모든 팝업을 닫는 메서드
    {
        while (_popupStack.Count>0)
            ClosePopupUI();
    }
  
    public void Clear()//UI_Popup과 UI_Scene은 특정 씬에 종속되므로, 클리어 필요
    {
        CloseAllPopupUI();
        _sceneUI= null;

    }
}
