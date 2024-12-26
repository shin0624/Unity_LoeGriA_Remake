using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Scene
{   //�κ��丮 ����
    // Init() ���� --> ���� �κ��丮�� ������ ��, ������ �����ִ� ������ �����Ͽ� �ݺ��� ���� �־��־�� ��
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

        Bind<GameObject>(typeof(GameObjects));//���ӿ�����Ʈ Ÿ���� ���ε�

        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);//����Ƽ�������� ������" UI_Inven"�� ���ε� GridPanel�� �̾ƿ´�.
        foreach(Transform child in gridPanel.transform)//���ӿ�����Ʈ�� ����ִ� �ڽ��� ��� ��ȸ�ϸ�,
            Managers.Resource.Destroy(child.gameObject);//�������Ҵ� Destroy�� ����Ͽ� ��� ����(���� �����տ� �ӽ÷� �־���Ҵ� �ڽ� ������Ʈ���� �����Ƿ� ��� ����)

        
        for(int i=0; i<8; i++)//ä����� �ϴ� ������ ������ŭ �ݺ��ϸ�
        {
            Managers.UI.MakeSubItem<UI_Inven_Item>();

            //UI_Inven_Item�� �����ؼ� UI_Inven/GridPanel �������� �ڽ����� �ٿ��־�� ��.
            //GameObject item = Managers.Resource.Instantiate("UI/Scene/UI_Inven_Item");//Instantiate�� ����Ͽ� ������ �������� ���� -->���� ��ο� �ִ� ���ϸ� �������� �̿��Ͽ� �������� ������ �� �ִ�.
          
            GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(gridPanel.transform).gameObject;

            //item.transform.SetParent(gridPanel.transform);//������ ������ ������ �������� SetParent()�� �̿��Ͽ� gridPanel�� �ڽ����� �����Ѵ�.
            //--> MakeSubItem�� �Ű������� �θ� Ʈ�������� �߰������Ƿ� �� ������ ����

            UI_Inven_Item invenItem =  item.GetOrAddComponent<UI_Inven_Item>(); //-->�Ǵ� ������ �ν����Ϳ��� UI_Inven��ũ��Ʈ�� ������Ʈ�� �߰��ص� �� -->�ش� ���� �Ǵ� ������Ʈ �߰����� �Ϸ��ؾ� ����Ƽ ���� �� ui�� ��µ�
            invenItem.SetInfo($"����{i}��");
            //-->GetOrAddComponent�� GameObject ��ü���� ȣ���ϵ��� ����-->�ش� �޼��带 Extension Method�� ����� GameObject��ü���� ���� ����
        
        }
    }


  
}
