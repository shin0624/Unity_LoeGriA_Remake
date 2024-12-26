using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PoolManager//Resource Manager를 보조하여 Object Pooling을 관리하는 스크립트
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
                Push(Create()); //순서  : Init()에서 Create()로 복사본 생성 후 _poolStack에 저장(Push)
        }

        Poolable Create()//새로운 객체(복사본)을 생성하여 Pollable로 반환하는 메서드--count횟수만큼 반복
        {
            GameObject go = Object.Instantiate<GameObject>(Original);//원본(Original)을 Instantiate해서 복사본 go를 생성
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.parent = Root;//Root에 poolable의 부모 연결
            poolable.gameObject.SetActive(false);//인스펙터의 활성화/비활성화 체크버튼을 false로 설정-->업데이트문을 받지 않고 수면 상태가 됨
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            //_poolStack 내의 복사본이 있다면 반환, 없다면 새로 만들어준다.
            Poolable poolable;

            if (_poolStack.Count > 0)//하나라도 대기상태라면 꺼내서 pop한다
                poolable = _poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);

            //DontDestroyOnLoad 해제 용도
            if (parent == null)
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;

            poolable.transform.parent = parent;
            poolable.IsUsing = true;

            return poolable;
        }

    }
    #endregion
    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;//GameObject로 생성해도 무방

    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);//풀링할 객체가 있다면 _root로 생성
        }
    }

    public void Push(Poolable poolable)//객체 사용 후 풀에 반환(push)하는 메서드
    {
        string name = poolable.gameObject.name;//객체의 이름을 가져온다
        //예외) 에디터 상에서 드래그로 객체 생성 시, pool이 없는 상태에서 push를 하는 경우가 발생했을 경우를 방지하기 위해 아래를 추가
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
        pool.Root.parent = _root;//생성된 pool의 _root객체를 @Pool_Root 밑으로 보낸다

        _pool.Add(original.name, pool);
    }

    public Poolable Pop(GameObject original, Transform parent = null)//pooling된 오브젝트 유무 확인 후 사용. 오리지널 객체와 부모(옵션)을 인자로 받음
    {
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original);

        return _pool[original.name].Pop(parent);//오리지널의 이름을 key로 하여 해당하는 객체의 parent를 pop
    }

    public GameObject GetOriginal(string name)//원본을 여러번 찾을 필요 없이 한번 찾은 원본은 바로 사용하게 하는 메서드
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
