using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEX 
{
    public BaseScene CurrentScene  { get { return GameObject.FindObjectOfType<BaseScene>(); } }//@Scene�� ����ִ� Login Scene ������Ʈ�� ObjectŸ������ �����Ͽ� ����

    public void LoadScene(Define.Scene type)//����(SceneManager) LoadScene������ string�� ���ڷ� �޾�����, Define���� enumŸ������ Scene����� �����ϰ� ������ enumŸ���� �̿�
    {
        Managers.Clear();//���ʿ��� �޸� Ŭ����
        SceneManager.LoadScene(GetSceneName(type));
        //-->LoadScene ���� �� Clear�� ���� �� ���೻�� ���� �� ���� Scene���� �Ѿ
    }

    string GetSceneName(Define.Scene type)//Scene�� type�� �־��ָ� string�� ��ȯ�ϴ� �Լ��� ����
    {
        //C#�� ���÷��� ������� DefineŬ������ enum���� �����Ѵ�.
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
