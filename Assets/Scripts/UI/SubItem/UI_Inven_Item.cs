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

    string _name;//������ �����ִ� �������� �̸�

    void Start()
    {
        Init();
    }

    public override void Init()
    {
       Bind<GameObject>(typeof(GameObjects));
        //UI_Inven ��ũ��Ʈ������ ���ε�� ���� : ������ ������Ʈ�� ã�Ƽ� ���ε��ϴ� ���� �ƴ϶�, �� ��ũ��Ʈ���� ���� ���� ����ü ������ ���ǵ� ��ҵ��� ����ִ� GameObject�� ���ε�
        //-->��, ItemIcon�� ItemNameText�� ����ִ� ������Ʈ�� "UI_Inven_Item"�� ã�� ���ε�.
        Get<GameObject>((int)GameObjects.ItemNameText).GetComponent<Text>().text = _name;// Get���� ItemNameText�� ��������, GetComponent�� Text������Ʈ�� ������ �ؽ�Ʈ�� ����.

        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent((PointerEventData) => { Debug.Log($"������ Ŭ�� {_name}"); }) ;//Get���� ItemIcon�� �����ͼ� PointerEventData�� �޴´�. ȭ�� �󿡼� ������ �������� Ŭ���ϸ� �αװ� �ߵ��� �켱 ����.
    
    }


    public void SetInfo(string name)//���� �������� name�� �޾Ƽ� Init()�� �ؽ�Ʈ�����κп� �־��ش�.
    {
        _name = name;
    }
}
