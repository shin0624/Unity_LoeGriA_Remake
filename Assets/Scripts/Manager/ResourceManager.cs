using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManager
{

    public T Load<T>(string path) where T : Object//프리팹을 로드하는 메서드는 제네릭타입
    {
        //prefab --> 원본을 pool에서 찾아 반환
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');//프리팹 경로가 .../prefab 형식이므로, 슬래시를 인덱스로 설정
            if (index >= 0)
                name = name.Substring(index + 1);// 경로에서 슬래시 이후의 프리팹 이름만 잘라서 name에 저장.

            GameObject go = Managers.Pool.GetOriginal(name);//Pool에서 게임오브젝트를 찾는다.
            if (go != null)
                return go as T; // 게임오브젝트를 찾았다면 반환, 찾지못했다면 Resources.Load로 로드.

        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)//프리팹(original)을 로드하는 메서드
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");//Prefabs폴더 내에 있는 프리팹을 가져오도록 지정
        //ResourceManager를 이용한 Instantiate()를 선언할 때는 Prefabs/ 를 안붙여도 될 것.
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");//만약 프리팹이 null이면 경로와 함꼐 로그표시
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;//original의 컴포넌트가 Poolable일 경우(풀링 대상인 경우)--> 처음 생성될 경우 Pool을 생성한 후 프리팹을 pop, 처음이 아닐 경우 Pool에서 대기하던 프리팹을 Pop

        //풀링 대상이 아닐 경우 아래 코드에서 실행
        GameObject go = Object.Instantiate(original, parent);//오브젝트의 원본은 prefab --> Instantiate()를 사용하여 카피본을 생성하는 개념
        go.name = original.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Poolable poolable = go.GetComponent<Poolable>();//Destroy 전, 게임오브젝트의 컴포넌트가 Poolable인 경우(풀링대상)를 체크하여, 풀링 대상이면 풀에 반환. 아니라면 삭제
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }
        Object.Destroy(go);
    }

    //-->이후 코드 상에서 사용 시 GameObject Tank;   Tank = Managers.Resource.Instantiate("Tank"); 형식으로 사용하면 됨
}