using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Util//기능성 함수들을 관리하는 스크립트 
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        // 어떤 오브젝트에게 이벤트를 붙일지 모르니, 오브젝트에 UI_EventHandler가 없다면 추가해주는 구문이 필요
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }


    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)//GameObject 타입 전용 메서드-->실행 로직은 동일하므로 코드 재사용
    {
        Transform transform = FindChild<Transform>(go, name, recursive);//GameObject는 transform을 갖고 있으므로, transform이 null이면 null리턴, 아니면 transform.gameObject를 리턴
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    //최상위 객체와 이름을 받음, 만약 이름을 입력하지 않으면 이름 비교x, 타입에 해당하면 리턴,recursive : 자식을 찾을 때 하위 자식 하나만 찾을 것인지 자식의 자식까지 재귀적으로 찾을 것인지 여부
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object//유니티오브젝트만 찾을 것임을 조건으로 명시
    {// T : Button, Text 등 찾고자하는 컴포넌트
        if (go == null)//최상위 객체가 null일 때=null 리턴
            return null;

        if (recursive == false)//하위 자식 하나만 탐색
        {
            for(int i = 0; i < go.transform.childCount; i++)// childCount를 사용하여 최상위객체의 자식 개수만큼 루프 후 GetChild(i)로 가져온다.
            {
              Transform transform =   go.transform.GetChild(i);//Transform으로 리턴 : GameObject와 Transform은 서로 왔다갔다 할 수 있는 개념
                if (string.IsNullOrEmpty(name) || transform.name == name)//이름 입력 여부와 일치 여부 확인
                {
                    T component = transform.GetComponent<T>();//이름 여부까지 통과했다면 컴포넌트 유무를 확인
                    if (component != null)//컴포넌트가 있다면 리턴
                        return component;
                }
            }
        }
        else//자식의 자식까지 모두 탐색
        {
            foreach(T component in go.GetComponentsInChildren<T>())// GetComponentsInChildren으로 게임 오브젝트가 갖고있는 T 타입 컴포넌트를 하나하나 스캔
            {
                if (string.IsNullOrEmpty(name) || component.name == name)//혹시 이름을 입력하지 않은 경우, 무조건 T타입 하나만 찾으면 반환되도록 isNullOrEmpty를 이용. name이 Empty이거나 내가 찾는 name이면 리턴되도록
                    return component;
            }
        }
        return null;//찾지 못한 경우 null 리턴
    }
}
