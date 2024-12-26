using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PoolManager//Resource Manager�� �����Ͽ� Object Pooling�� �����ϴ� ��ũ��Ʈ
{
    #region Pool
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();
        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++)
                Push(Create()); //����  : Init()���� Create()�� ���纻 ���� �� _poolStack�� ����(Push)
        }

        Poolable Create()//���ο� ��ü(���纻)�� �����Ͽ� Pollable�� ��ȯ�ϴ� �޼���--countȽ����ŭ �ݺ�
        {
            GameObject go = Object.Instantiate<GameObject>(Original);//����(Original)�� Instantiate�ؼ� ���纻 go�� ����
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.parent = Root;//Root�� poolable�� �θ� ����
            poolable.gameObject.SetActive(false);//�ν������� Ȱ��ȭ/��Ȱ��ȭ üũ��ư�� false�� ����-->������Ʈ���� ���� �ʰ� ���� ���°� ��
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            //_poolStack ���� ���纻�� �ִٸ� ��ȯ, ���ٸ� ���� ������ش�.
            Poolable poolable;

            if (_poolStack.Count > 0)//�ϳ��� �����¶�� ������ pop�Ѵ�
                poolable = _poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);

            //DontDestroyOnLoad ���� �뵵
            if (parent == null)
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;

            poolable.transform.parent = parent;
            poolable.IsUsing = true;

            return poolable;
        }

    }
    #endregion
    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;//GameObject�� �����ص� ����

    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);//Ǯ���� ��ü�� �ִٸ� _root�� ����
        }
    }

    public void Push(Poolable poolable)//��ü ��� �� Ǯ�� ��ȯ(push)�ϴ� �޼���
    {
        string name = poolable.gameObject.name;//��ü�� �̸��� �����´�
        //����) ������ �󿡼� �巡�׷� ��ü ���� ��, pool�� ���� ���¿��� push�� �ϴ� ��찡 �߻����� ��츦 �����ϱ� ���� �Ʒ��� �߰�
        if (_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }
    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;//������ pool�� _root��ü�� @Pool_Root ������ ������

        _pool.Add(original.name, pool);
    }

    public Poolable Pop(GameObject original, Transform parent = null)//pooling�� ������Ʈ ���� Ȯ�� �� ���. �������� ��ü�� �θ�(�ɼ�)�� ���ڷ� ����
    {
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original);

        return _pool[original.name].Pop(parent);//���������� �̸��� key�� �Ͽ� �ش��ϴ� ��ü�� parent�� pop
    }

    public GameObject GetOriginal(string name)//������ ������ ã�� �ʿ� ���� �ѹ� ã�� ������ �ٷ� ����ϰ� �ϴ� �޼���
    {
        if (_pool.ContainsKey(name) == false)
            return null;


        return _pool[name].Original;
    }

    public void Clear()
    {
        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }
}
