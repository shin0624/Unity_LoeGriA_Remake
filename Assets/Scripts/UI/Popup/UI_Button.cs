using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

//�������� --> 1) Button�� Onclick ���� �� ���̾��Ű���� ���ڸ� �巡�׵���ϴ� ��Ȳ
// --> ���̾��Ű�� ������Ʈ �̸��� �ǳ��ָ� �ڵ����� Onclick���� ���εǵ��� �ڵ�ȭ ->void Bind()�� �ڵ�ȭ �� ��

// 2) Text ��� �� SerializeField�� �ϳ��ϳ� ���� ���� �� �ν����Ϳ��� �����ϴ� ��Ȳ

public class UI_Button : UI_Popup
{
#if UI�������޿���
    //[SerializeField]
    //Text _text;
    //TextMeshProUGUI _text;//��ư Ŭ�� �� canvas ���� Text ���ڰ� �����ϵ��� �ϱ� ����, ����Ƽ ������ ���ڸ� �Ѱ��� �ؽ�Ʈ ������ ����
    //TextMeshPro�� ����ϹǷ�, TextŸ�� ���ڴ� �ν����Ϳ��� �Ѱ��� �� ����-->TMPro ���ӽ����̽��� �����ϰ�, �ؽ�Ʈ�� TextMeshProUGUI  Ÿ������ �����ؾ� ��.
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

    enum GameObjects //Bind ��� �� ������Ʈ Ÿ�� �Ӹ� �ƴ϶�, ���ӿ�����Ʈ ��ü(ex GameObject obj)�� �Ѱ��ְ��� �� ���� ���Ͽ� �ۼ�
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
#if BindEvent�޼�����̵�
        GameObject go =  GetImage((int)Images.ItemIcon).gameObject;
        UI_EventHandler evt = go.GetComponent<UI_EventHandler>();//������Ʈ ���� ���� ��. --> �̺�Ʈ�ڵ鷯�� GetComponent�� ����
        evt.OnDragHandler += ((PointerEventData data) => { evt.gameObject.transform.position = data.position; });//UI_EventHandler���� Invoke�� �븮�ڸ� ȣ��������, ���ٽ����� �ۼ�
       
#endif
     }

    public override void Init()
    {
        base.Init();//Init()�� �θ� ȣ��

        Bind<Button>(typeof(Buttons));//Buttons����ü ������ �ѱ�ڴٰ� ȣ��-->Buttons ����ü Ÿ���� Button�̶�� ������Ʈ�� ã�� �ش��ϴ� ���� �����Ѵ�
        Bind<TextMeshProUGUI>(typeof(Texts));//Texts����ü ������ �ѱ�ڴٰ� ȣ��
        Bind<GameObject>(typeof(GameObjects));

        Bind<Image>(typeof(Images));//�̹��� Ÿ���� images ���ε�

        //Get<TextMeshProUGUI>((int)Texts.ScoreText).text = "Bind Test";//TextMeshPro�� ����ϹǷ�, TextŸ�� ���ڴ� �ν����Ϳ��� �Ѱ��� �� ����-->TMPro ���ӽ����̽��� �����ϰ�, �ؽ�Ʈ�� TextMeshProUGUI  Ÿ������ �����ؾ� ��.
        // GetText((int)Texts.ScoreText).text = "BindTest";

        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);//Hierarchy���� �巡�׵������ UI�̺�Ʈ�� �������� �ʰ�, �� �ٿ� �ڵ� ó���ǵ��� ��

        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        BindEvent(go, (PointerEventData data) => { go.gameObject.transform.position = data.position; }, Define.UIEvent.Drag);//�巡���̺�Ʈ ����

    }



    int _score = 0;

  public void OnButtonClicked(PointerEventData data)//�� public���� ���־�� UI���� �����
    {  
        _score++;
        GetText((int)Texts.ScoreText).text = $"score ={_score}";

        //_text.text = $"Score : {_score}"; 
    }
}
