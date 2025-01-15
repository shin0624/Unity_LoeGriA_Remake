using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginScene : BaseScene
{
    //Login 씬에서 사용할 스크립트. login 씬에서 특정 액션 발동 시 Loading Scene을 거쳐 Game씬으로 넘어가도록 구현 / 유니티 File->BuildSetting 필수

    //버튼객체
    public Button StartButton;
    public Button OptionButton;
    public Button QuitButton;
    public AudioSource ado;

    //클릭 시 이미지 변경되도록 할 수 있는 클릭드 스프라이트 객체
    public Sprite StartButtonClicked;
    public Sprite OptionButtonClicked;
    public Sprite QuitButtonClicked;
    

    //참조할 이미지 객체
    private Image StartButtonImage;
    private Image OptionButtonImage;
    private Image QuitButtonImage;


    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Login;  
    }
    private void Start()
    {
        if(ado!=null)
        {
            ado.Play();
            ado.loop = true;
        }
        StartButtonImage = StartButton.GetComponent<Image>();
        OptionButtonImage = OptionButton.GetComponent<Image>();
        QuitButtonImage = QuitButton.GetComponent<Image>();

        StartButton.GetComponent<Button>().onClick.AddListener(OnStartButtonClicked);
        OptionButton.GetComponent<Button>().onClick.AddListener(OnOptionButtonClicked);
        QuitButton.GetComponent<Button>().onClick.AddListener(OnQuitButtonClicked);
    }

    private void Update()
    {
    
    }

    void OnStartButtonClicked()
    {
        StartButtonImage.sprite = StartButtonClicked;
        LoadingScene.LoadScene("Olenamento");//각 버튼 클릭 시 Scenemanager.Loadscene이 아닌 로딩씬의 메서드를 호출.
    }
    void OnOptionButtonClicked()
    {
        OptionButtonImage.sprite = OptionButtonClicked;
        LoadingScene.LoadScene("Options");
    }
    void OnQuitButtonClicked()
    {
        QuitButtonImage.sprite = QuitButtonClicked;
        Application.Quit();
        // 에디터에서 실행 중인 경우, 에디터도 종료되도록 설정 (UnityEditor 네임스페이스 사용)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
 
    public override void Clear()
    {
        Debug.Log("Login Scene Clear");
    }

}
