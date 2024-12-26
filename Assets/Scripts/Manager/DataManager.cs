using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// Serializable(����ȭ)-->Ŭ���� ���ο� �ִ� ������, ������ ������ byte������ �����ͷ� ����� ��.
//json�� ��� pubilc �����͸� String���� ��ȯ�ϴ� ���̶� �Ѱ谡 ������
//����ȭ�� ��� class�� ���� ��ü�� byte�������� ��ȯ�ϴ� ���̶� private�� ����� �޾����� �� ������ private �����ͱ��� ��ȯ ����
//**json���Ͽ� �ּ��޸� �ȵ�, json ��Ģ-->[] : List  , {} : Struct
#if Data_Contents���̵�
[Serializable]
public class Stat
{
    public int level;
    public int hp;
    public int attack;
}

[Serializable]
public class StatData : ILoader<int, Stat>
{
    public List<Stat> stats = new List<Stat>();//stats : json������ "stats"

    public Dictionary<int, Stat> MakeDict()//ILoader �������̽� ����
    {
        Dictionary<int, Stat> dict = new Dictionary<int, Stat>();

        foreach (Stat stat in stats)
            dict.Add(stat.level, stat);
        return dict;
    }
}
#endif
public interface ILoader<Key, Value>//��ųʸ��� ������ �߰��Ǹ� ���� �޾ƿ;� �� key,value���� �ٸ� �� �����Ƿ�, �����͸� �ε��ϴ� �������̽��� �߰��Ͽ� ����
{
    Dictionary<Key, Value> MakeDict();
}


public class DataManager
{
    //������ Ŭ���̾�Ʈ���� ���� ������ ������ ����ϱ� ������, ���� ������ �Ȱ��� �����. ���� json �Ǵ� xml ���-->json���� " StatData"���� �ۼ��� ���� �ܾ�ͼ� ���
    //Stats ������ ��ųʸ��� ����ְ� �ϸ� ���� ����, ���� ���� �� ���� ���� ������ �����ϹǷ�, ����Ʈ������ stats�� ��ųʸ��� ��ȯ
    public Dictionary<int, Stat> statDict { get; private set; } = new Dictionary<int, Stat>();

    public void Init()
    {
        statDict = LoadJson<StatData, int, Stat>("StatData").MakeDict();//Loader�� �Լ� ���� ��� ��-->Json�ε� �� StatData������ ��ȯ�� ��.
    }

    Loader LoadJson<Loader, key, Value>(string path) where Loader : ILoader<key, Value>//key, value�� ���� ILoader�� ����ִ� Ŭ������ ����� �� �ִ� �Լ� ����
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");//�ؽ�Ʈ ������ �о�������� TextAsset(���) ���-->textAsset�� ��ȯŸ���� string
                                                                                  //StatData�� �ε��� ��, �޸𸮿� ������� �� �ֵ��� ��ȯ �۾��� �ʿ�-->JsonUtility
        return JsonUtility.FromJson<Loader>(textAsset.text);//ToJson : Ŭ������ �Ǿ��ִ� ���� json���� ��ȯ / FromJson : json���� ���� ���� Ŭ������ ��ȯ
                                                                       //-->���Ͽ� �ִ� ������ �޸𸮷� �ҷ��ͼ�, ���� Serializable�� ������ �����Ͽ� data�� �ε�.  

    }
}
