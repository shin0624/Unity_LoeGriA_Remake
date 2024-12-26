using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManager
{

    public T Load<T>(string path) where T : Object//�������� �ε��ϴ� �޼���� ���׸�Ÿ��
    {
        //prefab --> ������ pool���� ã�� ��ȯ
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');//������ ��ΰ� .../prefab �����̹Ƿ�, �����ø� �ε����� ����
            if (index >= 0)
                name = name.Substring(index + 1);// ��ο��� ������ ������ ������ �̸��� �߶� name�� ����.

            GameObject go = Managers.Pool.GetOriginal(name);//Pool���� ���ӿ�����Ʈ�� ã�´�.
            if (go != null)
                return go as T; // ���ӿ�����Ʈ�� ã�Ҵٸ� ��ȯ, ã�����ߴٸ� Resources.Load�� �ε�.

        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)//������(original)�� �ε��ϴ� �޼���
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");//Prefabs���� ���� �ִ� �������� ���������� ����
        //ResourceManager�� �̿��� Instantiate()�� ������ ���� Prefabs/ �� �Ⱥٿ��� �� ��.
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");//���� �������� null�̸� ��ο� �Բ� �α�ǥ��
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;//original�� ������Ʈ�� Poolable�� ���(Ǯ�� ����� ���)--> ó�� ������ ��� Pool�� ������ �� �������� pop, ó���� �ƴ� ��� Pool���� ����ϴ� �������� Pop

        //Ǯ�� ����� �ƴ� ��� �Ʒ� �ڵ忡�� ����
        GameObject go = Object.Instantiate(original, parent);//������Ʈ�� ������ prefab --> Instantiate()�� ����Ͽ� ī�Ǻ��� �����ϴ� ����
        go.name = original.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Poolable poolable = go.GetComponent<Poolable>();//Destroy ��, ���ӿ�����Ʈ�� ������Ʈ�� Poolable�� ���(Ǯ�����)�� üũ�Ͽ�, Ǯ�� ����̸� Ǯ�� ��ȯ. �ƴ϶�� ����
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }
        Object.Destroy(go);
    }

    //-->���� �ڵ� �󿡼� ��� �� GameObject Tank;   Tank = Managers.Resource.Instantiate("Tank"); �������� ����ϸ� ��
}