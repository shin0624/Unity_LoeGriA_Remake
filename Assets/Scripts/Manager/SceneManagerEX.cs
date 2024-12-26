using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEX 
{
    public BaseScene CurrentScene  { get { return GameObject.FindObjectOfType<BaseScene>(); } }//@Scene이 들고있는 Login Scene 컴포넌트를 Object타입으로 변경하여 리턴

    public void LoadScene(Define.Scene type)//원본(SceneManager) LoadScene에서는 string을 인자로 받았으나, Define에서 enum타입으로 Scene목록을 관리하고 있으니 enum타입을 이용
    {
        Managers.Clear();//불필요한 메모리 클리어
        SceneManager.LoadScene(GetSceneName(type));
        //-->LoadScene 실행 시 Clear로 현재 씬 수행내용 삭제 후 다음 Scene으로 넘어감
    }

    string GetSceneName(Define.Scene type)//Scene의 type을 넣어주면 string을 반환하는 함수를 선언
    {
        //C#의 리플렉션 기능으로 Define클래스의 enum값을 추출한다.
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
