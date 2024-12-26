using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// Serializable(직렬화)-->클래스 내부에 있는 데이터, 변수의 값들을 byte형식의 데이터로 만드는 것.
//json의 경우 pubilc 데이터를 String으로 변환하는 것이라 한계가 있지만
//직렬화의 경우 class의 상태 자체를 byte형식으로 변환하는 것이라 private과 상속을 받았으면 그 상위의 private 데이터까지 변환 가능
//**json파일에 주석달면 안됨, json 규칙-->[] : List  , {} : Struct
#if Data_Contents로이동
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
    public List<Stat> stats = new List<Stat>();//stats : json파일의 "stats"

    public Dictionary<int, Stat> MakeDict()//ILoader 인터페이스 구현
    {
        Dictionary<int, Stat> dict = new Dictionary<int, Stat>();

        foreach (Stat stat in stats)
            dict.Add(stat.level, stat);
        return dict;
    }
}
#endif
public interface ILoader<Key, Value>//딕셔너리가 여러개 추가되면 각각 받아와야 할 key,value값이 다를 수 있으므로, 데이터를 로드하는 인터페이스를 추가하여 정리
{
    Dictionary<Key, Value> MakeDict();
}


public class DataManager
{
    //서버와 클라이언트에서 같은 데이터 파일을 사용하기 때문에, 파일 포맷을 똑같이 맞춘다. 보통 json 또는 xml 사용-->json파일 " StatData"에서 작성한 값을 긁어와서 사용
    //Stats 내용을 딕셔너리로 들고있게 하면 유저 스탯, 몬스터 스탯 등 여러 유형 관리에 유용하므로, 리스트형식의 stats를 딕셔너리로 변환
    public Dictionary<int, Stat> statDict { get; private set; } = new Dictionary<int, Stat>();

    public void Init()
    {
        statDict = LoadJson<StatData, int, Stat>("StatData").MakeDict();//Loader형 함수 실제 사용 시-->Json로드 후 StatData형으로 반환될 것.
    }

    Loader LoadJson<Loader, key, Value>(string path) where Loader : ILoader<key, Value>//key, value를 가진 ILoader를 들고있는 클래스만 사용할 수 있는 함수 선언
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");//텍스트 파일을 읽어오기위한 TextAsset(경로) 사용-->textAsset은 반환타입이 string
                                                                                  //StatData를 로드한 후, 메모리에 들고있을 수 있도록 변환 작업이 필요-->JsonUtility
        return JsonUtility.FromJson<Loader>(textAsset.text);//ToJson : 클래스로 되어있는 값을 json으로 변환 / FromJson : json형식 파일 값을 클래스로 변환
                                                                       //-->파일에 있는 스탯을 메모리로 불러와서, 위의 Serializable된 정보를 참조하여 data에 로드.  

    }
}
