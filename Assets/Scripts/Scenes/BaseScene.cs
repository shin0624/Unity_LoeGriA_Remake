using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    //사용할 모든 scene의 최상위 부모 클래스

    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;//get은 public으로 열어두고, set은 protected로 막아둔다

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        // 프리팹화 한 EventSystem을 여기서 호출--> EventSystem을 들고있는 오브젝트가 있는지 체크 후 없다면 생성해줌
       Object obj =  GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }

    public abstract void Clear();//scene이 종료되었을 때 사용
 
}
