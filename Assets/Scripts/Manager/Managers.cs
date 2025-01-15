using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Managers : MonoBehaviour
{
    static Managers s_instance;//���ϼ� ���� ������ �̱���
    static Managers Instance { get { Init(); return s_instance; } }//�ܺο��� GetInstance ȣ�� �� Init()���� ��üũ �� ��ü�� ����� ��ȯ�ϴ� ���·� ���ư� ��
                                                                   //�ܺο��� Manager�ν��Ͻ��� ������ �� �� ����ϰ� �� �Լ�
    DataManager _data = new DataManager();
    public static DataManager Data { get { return Instance._data; } }

    InputManager _input = new InputManager();//��ǲ�Ŵ��� ������ ����
    public static InputManager Input { get { return Instance._input; } }

    PoolManager _pool = new PoolManager();//������Ʈ Ǯ���� ������ Ǯ �Ŵ��� ����
    public static PoolManager Pool { get { return Instance._pool; } }

    ResourceManager _resource = new ResourceManager();//���ҽ��Ŵ��� ������ ����
    public static ResourceManager Resource { get { return Instance._resource; } }

    SceneManagerEX _scene = new SceneManagerEX();
    public static SceneManagerEX Scene { get { return Instance._scene; } }

    UIManager ui = new UIManager();
    public static UIManager UI { get { return Instance.ui; } }//UI�� ������ UIManager ����

    AudioManager Ado = new AudioManager();
    public static AudioManager audioManager { get { return Instance.Ado; } } // ����� �Ŵ����� ������ audiomanager����

    PlayerSpawnManager spawnManager = new PlayerSpawnManager();
    public static PlayerSpawnManager playerSpawn {get {return Instance.spawnManager;}}

    void Start()
    {
        Init();
    }


    void Update()
    {
        _input.OnUpdate();// Managers�� ���콺, Ű���� ���� �Է� üũ�� ����
    }

    static void Init()
    {
        if (s_instance == null)//�ν��Ͻ� ���� null�϶�
        {
            GameObject go = GameObject.Find("@Managers");// --> @Managers ������Ʈ�� ã�ƺ���
            if (go == null)//@Managers ������Ʈ�� ���ٸ�
            {
                go = new GameObject { name = "@Managers" };// ������Ʈ�� ���� �����
                go.AddComponent<Managers>();//���� ���� ������Ʈ�� Managers ��ũ��Ʈ�� �ٿ��ش�
            }

            DontDestroyOnLoad(go);//���� ������Ʈ�� ������� �����Ǿ�� �ȵǱ� ������ ����.
            s_instance = go.GetComponent<Managers>();
            //���� @Manager������Ʈ�� �߰��ߴٸ� Managers ��ũ��Ʈ�� �����´�

            s_instance._data.Init();//Data�� ���� ���� �� �ѹ��� �ҷ�����, �� ���� �� Ŭ���� �� �ʿ� X
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