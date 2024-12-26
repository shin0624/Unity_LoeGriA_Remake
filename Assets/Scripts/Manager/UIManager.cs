using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    //UI������Ʈ�� �ִ� Canvas --> SortOrder ������ ���� ����. 
    //UI�� �˾���, �� ������ ������, �Ŵ������� �˾��� on/off ��û �� UI�� sortorder�� �����ߴٰ� �������Ѿ� ��
    //���� �������� ����� �˾��� ���� �����ؾ� �ϹǷ�, LIFO�� ���ñ����� �˾� ����

    //��� �� --> Managers.UI.ShowPopupUI<UI_Button>(); Managers.UI.ClosePopupUI(ui);
    int _order = 10;//�ֱٿ� ����� order�� ����

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();//���ӿ�����Ʈ�� �ش��ϴ� ������Ʈ�� �����ϴ� ���� _popupStack ����
    UI_Scene _sceneUI = null;

    public GameObject Root //UI�� ������ �� Hierarchy�� �� ������Ʈ UI_Root�� �����Ͽ� ����ó�� ���. UI_Root �Ʒ��� �˾����� ��ġ�� ��
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");// --> @UI_Root ������Ʈ�� ã�ƺ���
            if (root == null)//@UI_Root ������Ʈ�� ���ٸ�
                root = new GameObject { name = "@UI_Root" };// ������Ʈ�� ���� �����
            return root;
        }

    }


    public void  SetCanvas(GameObject go, bool sort = true)//�ܺο��� �˾� ���� UI�� ���� �� UIManager���� �ش� �˾��� _order�� ä��� ��û���� UI���� �켱������ ���ϱ� ���� �޼���
    {
      Canvas canvas =   Util.GetOrAddComponent<Canvas>(go);//ĵ���� ��ü�� GetOrAddComponent�� �̾ƿ�
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;//�������̵� ���� : ĵ���� �ȿ� ĵ������ ��ø�ؼ� ���� ��, �� �θ� � ���� ������ �ش� ĵ������ �ڽŸ��� sort order�� ���� �ɼ�.
      
        if (sort)//���� ��û �� --> canvas�� sort order�� _order�� �ٲٰ� ++���ش�.
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else//���� �̿�û �� --> UI_Popup�� ���þ��� �Ϲ� UI. �� ����ġ â, ü�� â �� �⺻���� ǥ�õǴ� UI�� ���
        {
            canvas.sortingOrder = 0;
        }
            
    }


    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base// inven, Scene �̿��� ��������� ���� �� �����ϴ� �޼���
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;//���� name�� ���� �ʴ´ٸ� T�� �̸��� �״�� ����ϵ���, �־��� Ÿ�� �̸��� �״�� ����� �� �ִ� typeof(T).Name�� ���
        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        return go.GetOrAddComponent<T>();
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    //name���� Asset/Resources/UI/Popup�� �ִ� �������� �̸��� �ǳ���, T���� Script/UI/Popup�� �ִ� UI_Button ��ũ��Ʈ�� �ǳ���
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;//���� name�� ���� �ʴ´ٸ� T�� �̸��� �״�� ����ϵ���, �־��� Ÿ�� �̸��� �״�� ����� �� �ִ� typeof(T).Name�� ���

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");

         T popup =Util.GetOrAddComponent<T>(go);//������Ʈ�� �����´�. ������ �ش� ������Ʈ�� �߰� �� ������

        _popupStack.Push(popup);
        //_order++; --> ShowPopup���� ��� �˾��� �ƴ϶�, Hierarchy�� �巡�׵������ ���� UI�˾��� �������� �Ǹ� _order++ ó�� �Ұ�-->UI_Popup���� ++ó���� �ϵ��� �� ��.

        go.transform.SetParent(Root.transform);//���ӿ�����Ʈ go�� �θ�� root�� ����
        
        return popup;
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene   //���� ǥ�õǴ� �⺻ UI ȣ���� ���� �޼���
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;//���� name�� ���� �ʴ´ٸ� T�� �̸��� �״�� ����ϵ���, �־��� Ÿ�� �̸��� �״�� ����� �� �ִ� typeof(T).Name�� ���

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");

        T sceneUI = Util.GetOrAddComponent<T>(go);//������Ʈ�� �����´�. ������ �ش� ������Ʈ�� �߰� �� ������
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);//���ӿ�����Ʈ go�� �θ�� root�� ����

        return sceneUI;
    }





    public void ClosePopupUI(UI_Popup popup)//�˾��� ������� �������� ���� ��츦 ���� �޼���-->������ ������ �˾��� �´��� �׽�Ʈ
    {
        if (_popupStack.Count == 0) return;

        if (_popupStack.Peek() != popup)//Peek : ������ ��Ҹ� ������.
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        CloseAllPopupUI();
    }


    public void ClosePopupUI()//�˾��� �ݴ� �޼���-->���� ��Ҹ� �ϳ��� �����ϸ鼭 �ݴ´�
    {
        if(_popupStack.Count==0) return;//���� �ϳ��� �ȵ���ִٸ� �ٷ� ����
        
        UI_Popup popup = _popupStack.Pop();//�ϳ��� ����ִٸ� Pop. ���� �ֱٿ� ��� �˾��� popup�� �ְ� Destroy�Ѵ�.
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        _order--;
    }
    public void CloseAllPopupUI()//���� �� ��� �˾��� �ݴ� �޼���
    {
        while (_popupStack.Count>0)
            ClosePopupUI();
    }
  
    public void Clear()//UI_Popup�� UI_Scene�� Ư�� ���� ���ӵǹǷ�, Ŭ���� �ʿ�
    {
        CloseAllPopupUI();
        _sceneUI= null;

    }
}
