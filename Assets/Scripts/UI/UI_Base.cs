using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour//UI 매핑 타입의 베이스가 되는 스크립트(UI_Button에서 작성했던 내용 이전)
{
    public abstract void Init();// UI_Base를 상속받은 스크립트에서 Init()을 쓸 수 있게 abstract로 공통 정의



    //여러가지 Type을 넣었으니 Dictionary로 관리. Button타입, Text타입을 유니티엔진 오브젝트의 리스트로 관리함
    Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

   protected void Bind<T>(Type type) where T : UnityEngine.Object//Buttons, Texts값을 넘겨주면 값과 겹치는 오브젝트를 자동 저장하도록 하는 함수. Reflection 이용
    {
        //Button 또는 Text를 자식으로 두고 있는 오브젝트를 찾아야 하므로, Bind 함수를 제네릭으로 선언
        string[] names = Enum.GetNames(type);//C#에만 있는 기능. 열거체 항목을 string 배열로 반환할 수 있다.

        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];//열거체 항목을 string으로 변환했으니 dictionary에 넣기 위해 key, value가 필요->key는 제네릭타입, value는 오브젝트 배열
        _objects.Add(typeof(T), objects);

        //개선사항(1)의 자동화를 위해 루프 선언
        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))//컴포넌트 타입이 아니라 게임오브젝트 자체를 넘겨주는 경우-->GameObject전용 FindChild를 하나 더 생성한다.(T타입 X)
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);//최상위 부모, 이름을 인자로 넣는다.
                                                                           //루프를 돌며 찾은 오브젝트 이름을 objects배열에 넣어줘야 함-->GameObject에 접근할 수 있음을 이용하여 최상위 객체(UI_Button)의 자식 중 같은 이름이 있는 지 찾아야 함

            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");

        }
    }

  public  T Get<T>(int idx) where T : UnityEngine.Object//Bind로 찾은 오브젝트를 꺼내서 쓰는 Get함수
    {
        UnityEngine.Object[] objects = null;
        //TryGetValue를 사용, key값은 T의 타입, value는 object배열
        if (_objects.TryGetValue(typeof(T), out objects) == false)//찾아오는 데 실패 시 null 리턴
            return null;
        return objects[idx] as T;//찾았다면 objects 인덱스번호 추출 후 T로 캐스팅(objects의 타입이 UnityEngine.Object이므로)
    }

    //버튼, 텍스트, 이미지 ,게임오브젝트 등이 쓰일 때 마다 Get 선언을 해야하는 번거로움을 해결하기 위해, 바로 Get을 사용할 수 있도록 만들어줌
    protected TextMeshProUGUI GetText(int idx) { return Get<TextMeshProUGUI>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }

    public static void BindEvent(GameObject go, Action<PointerEventData>action, Define.UIEvent type = Define.UIEvent.Click)//UI이벤트를 추가하는 함수 선언-->게임오브젝트, 콜백으로 연동할 Action함수, 어떤 UIEvent에 적용할 것인지를 인자로 지정(기본 click)
    {
        // 어떤 오브젝트에게 이벤트를 붙일지 모르니, 오브젝트에 UI_EventHandler가 없다면 추가해주는 구문이 필요
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }
}
