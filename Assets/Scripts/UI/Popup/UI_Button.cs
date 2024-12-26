using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

//개선사항 --> 1) Button의 Onclick 연결 시 하이어라키에서 인자를 드래그드롭하는 상황
// --> 하이어라키의 오브젝트 이름만 건네주면 자동으로 Onclick에서 매핑되도록 자동화 ->void Bind()로 자동화 할 것

// 2) Text 사용 시 SerializeField로 하나하나 변수 선언 후 인스펙터에서 연결하는 상황

public class UI_Button : UI_Popup
{
#if UI인자전달예시
    //[SerializeField]
    //Text _text;
    //TextMeshProUGUI _text;//버튼 클릭 시 canvas 상의 Text 숫자가 증가하도록 하기 위해, 유니티 내에서 인자를 넘겨줄 텍스트 변수를 설정
    //TextMeshPro를 사용하므로, Text타입 인자는 인스펙터에서 넘겨줄 수 없음-->TMPro 네임스페이스를 선언하고, 텍스트를 TextMeshProUGUI  타입으로 선언해야 함.
#endif
    enum Buttons 
    {
         PointButton
    }

    enum Texts
    {
        PointText,
        ScoreText,
    }

    enum GameObjects //Bind 사용 시 컴포넌트 타입 뿐만 아니라, 게임오브젝트 자체(ex GameObject obj)를 넘겨주고자 할 때를 위하여 작성
    { 
        TestObject,
    }

    enum Images
    {
       ItemIcon,
    }

    private void Start()
    {
        Init();
#if BindEvent메서드로이동
        GameObject go =  GetImage((int)Images.ItemIcon).gameObject;
        UI_EventHandler evt = go.GetComponent<UI_EventHandler>();//컴포넌트 수동 연결 시. --> 이벤트핸들러를 GetComponent로 추출
        evt.OnDragHandler += ((PointerEventData data) => { evt.gameObject.transform.position = data.position; });//UI_EventHandler에서 Invoke로 대리자를 호출했으니, 람다식으로 작성
       
#endif
     }

    public override void Init()
    {
        base.Init();//Init()의 부모 호출

        Bind<Button>(typeof(Buttons));//Buttons열거체 형식을 넘기겠다고 호출-->Buttons 열거체 타입의 Button이라는 컴포넌트를 찾아 해당하는 것을 매핑한다
        Bind<TextMeshProUGUI>(typeof(Texts));//Texts열거체 형식을 넘기겠다고 호출
        Bind<GameObject>(typeof(GameObjects));

        Bind<Image>(typeof(Images));//이미지 타입의 images 바인딩

        //Get<TextMeshProUGUI>((int)Texts.ScoreText).text = "Bind Test";//TextMeshPro를 사용하므로, Text타입 인자는 인스펙터에서 넘겨줄 수 없음-->TMPro 네임스페이스를 선언하고, 텍스트를 TextMeshProUGUI  타입으로 선언해야 함.
        // GetText((int)Texts.ScoreText).text = "BindTest";

        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);//Hierarchy에서 드래그드롭으로 UI이벤트를 연결하지 않고, 한 줄에 자동 처리되도록 함

        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        BindEvent(go, (PointerEventData data) => { go.gameObject.transform.position = data.position; }, Define.UIEvent.Drag);//드래그이벤트 구현

    }



    int _score = 0;

  public void OnButtonClicked(PointerEventData data)//꼭 public으로 해주어야 UI에서 실행됨
    {  
        _score++;
        GetText((int)Texts.ScoreText).text = $"score ={_score}";

        //_text.text = $"Score : {_score}"; 
    }
}
