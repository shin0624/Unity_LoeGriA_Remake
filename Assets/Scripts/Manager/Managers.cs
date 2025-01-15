using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Managers : MonoBehaviour
{
    static Managers s_instance;//유일성 보장 가능한 싱글톤
    static Managers Instance { get { Init(); return s_instance; } }//외부에서 GetInstance 호출 시 Init()으로 널체크 후 객체를 만들고 반환하는 형태로 돌아갈 것
                                                                   //외부에서 Manager인스턴스를 쓰고자 할 때 사용하게 될 함수
    DataManager _data = new DataManager();
    public static DataManager Data { get { return Instance._data; } }

    InputManager _input = new InputManager();//인풋매니저 생성자 선언
    public static InputManager Input { get { return Instance._input; } }

    PoolManager _pool = new PoolManager();//오브젝트 풀링을 관리할 풀 매니저 선언
    public static PoolManager Pool { get { return Instance._pool; } }

    ResourceManager _resource = new ResourceManager();//리소스매니저 생성자 선언
    public static ResourceManager Resource { get { return Instance._resource; } }

    SceneManagerEX _scene = new SceneManagerEX();
    public static SceneManagerEX Scene { get { return Instance._scene; } }

    UIManager ui = new UIManager();
    public static UIManager UI { get { return Instance.ui; } }//UI를 관리할 UIManager 연결

    AudioManager Ado = new AudioManager();
    public static AudioManager audioManager { get { return Instance.Ado; } } // 오디오 매니저를 관리할 audiomanager연결

    PlayerSpawnManager spawnManager = new PlayerSpawnManager();
    public static PlayerSpawnManager playerSpawn {get {return Instance.spawnManager;}}

    void Start()
    {
        Init();
    }


    void Update()
    {
        _input.OnUpdate();// Managers가 마우스, 키보드 등의 입력 체크를 수행
    }

    static void Init()
    {
        if (s_instance == null)//인스턴스 값이 null일때
        {
            GameObject go = GameObject.Find("@Managers");// --> @Managers 오브젝트를 찾아본다
            if (go == null)//@Managers 오브젝트가 없다면
            {
                go = new GameObject { name = "@Managers" };// 오브젝트를 새로 만든다
                go.AddComponent<Managers>();//새로 만든 오브젝트에 Managers 스크립트를 붙여준다
            }

            DontDestroyOnLoad(go);//게임 오브젝트는 마음대로 삭제되어서는 안되기 때문에 선언.
            s_instance = go.GetComponent<Managers>();
            //만약 @Manager오브젝트를 발견했다면 Managers 스크립트를 가져온다

            s_instance._data.Init();//Data는 게임 시작 시 한번에 불러오며, 씬 변경 시 클리어 할 필요 X
            s_instance._pool.Init();
            //s_instance._sound.Init();
        }
    }

    public static void Clear()
    {
       // Sound.Clear();
        Input.Clear();
        Scene.Clear();
        UI.Clear();

        Pool.Clear();
    }
}