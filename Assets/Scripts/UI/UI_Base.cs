using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour//UI ���� Ÿ���� ���̽��� �Ǵ� ��ũ��Ʈ(UI_Button���� �ۼ��ߴ� ���� ����)
{
    public abstract void Init();// UI_Base�� ��ӹ��� ��ũ��Ʈ���� Init()�� �� �� �ְ� abstract�� ���� ����



    //�������� Type�� �־����� Dictionary�� ����. ButtonŸ��, TextŸ���� ����Ƽ���� ������Ʈ�� ����Ʈ�� ������
    Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

   protected void Bind<T>(Type type) where T : UnityEngine.Object//Buttons, Texts���� �Ѱ��ָ� ���� ��ġ�� ������Ʈ�� �ڵ� �����ϵ��� �ϴ� �Լ�. Reflection �̿�
    {
        //Button �Ǵ� Text�� �ڽ����� �ΰ� �ִ� ������Ʈ�� ã�ƾ� �ϹǷ�, Bind �Լ��� ���׸����� ����
        string[] names = Enum.GetNames(type);//C#���� �ִ� ���. ����ü �׸��� string �迭�� ��ȯ�� �� �ִ�.

        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];//����ü �׸��� string���� ��ȯ������ dictionary�� �ֱ� ���� key, value�� �ʿ�->key�� ���׸�Ÿ��, value�� ������Ʈ �迭
        _objects.Add(typeof(T), objects);

        //��������(1)�� �ڵ�ȭ�� ���� ���� ����
        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))//������Ʈ Ÿ���� �ƴ϶� ���ӿ�����Ʈ ��ü�� �Ѱ��ִ� ���-->GameObject���� FindChild�� �ϳ� �� �����Ѵ�.(TŸ�� X)
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);//�ֻ��� �θ�, �̸��� ���ڷ� �ִ´�.
                                                                           //������ ���� ã�� ������Ʈ �̸��� objects�迭�� �־���� ��-->GameObject�� ������ �� ������ �̿��Ͽ� �ֻ��� ��ü(UI_Button)�� �ڽ� �� ���� �̸��� �ִ� �� ã�ƾ� ��

            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");

        }
    }

  public  T Get<T>(int idx) where T : UnityEngine.Object//Bind�� ã�� ������Ʈ�� ������ ���� Get�Լ�
    {
        UnityEngine.Object[] objects = null;
        //TryGetValue�� ���, key���� T�� Ÿ��, value�� object�迭
        if (_objects.TryGetValue(typeof(T), out objects) == false)//ã�ƿ��� �� ���� �� null ����
            return null;
        return objects[idx] as T;//ã�Ҵٸ� objects �ε�����ȣ ���� �� T�� ĳ����(objects�� Ÿ���� UnityEngine.Object�̹Ƿ�)
    }

    //��ư, �ؽ�Ʈ, �̹��� ,���ӿ�����Ʈ ���� ���� �� ���� Get ������ �ؾ��ϴ� ���ŷο��� �ذ��ϱ� ����, �ٷ� Get�� ����� �� �ֵ��� �������
    protected TextMeshProUGUI GetText(int idx) { return Get<TextMeshProUGUI>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }

    public static void BindEvent(GameObject go, Action<PointerEventData>action, Define.UIEvent type = Define.UIEvent.Click)//UI�̺�Ʈ�� �߰��ϴ� �Լ� ����-->���ӿ�����Ʈ, �ݹ����� ������ Action�Լ�, � UIEvent�� ������ �������� ���ڷ� ����(�⺻ click)
    {
        // � ������Ʈ���� �̺�Ʈ�� ������ �𸣴�, ������Ʈ�� UI_EventHandler�� ���ٸ� �߰����ִ� ������ �ʿ�
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
