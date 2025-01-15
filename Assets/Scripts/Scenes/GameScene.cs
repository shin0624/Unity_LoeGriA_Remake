using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : BaseScene
// "Game" 씬에서 사용할 스크립트
{
#if Coroutine
    //Coroutine : 서브루틴을 일시 정지하고 재개할 수 있음 -->일시 정지 시 함수 내 변수에 저장되었던 값이 그대로 이어짐(사라지지 않음)
    //기능 1. 오래 걸리는 작업을 잠시 끊거나 원하는 타이밍에 함수를 잠시 stop/복원-->시간 관리에 유용
    // 2. 원하는 타입이나 클래스로 return 가능

    class CoroutineTest : IEnumerable//코루틴 구현 시 붙여주어야 할 인터페이스 IEnumerable
    {
        public IEnumerator GetEnumerator()
        {//코루틴 리턴 시 붙이는 yield --> yield return 1,2,3,4.. 할 경우 return 1 실행 후 return 2, 3, 4 하나하나씩 복원하여 다음 상태로 넘어갈 수 있음.
          // 코루틴에서의 완전 종료는 yield break(일반 함수에서의 return 에 해당)

            for( int i=0;i<100000;i++)
            {
                if (i % 10000 == 0)
                    yield return null;//100000 번 반복에서 10000번째 마다 휴식 후 아래의 foreach문으로 값이 넘어감. foreach문에서 다음 동작을 결정한 뒤 수행 후 다시 코루틴으로 돌아옴
            }      
        }
    }

     IEnumerator ExplodeAfterSeconds(float seconds)//coroutine응용-> 일정 초 후에 발동하는 스킬
    {
        Debug.Log("explode endter");
        yield return new WaitForSeconds(seconds);//일정시간(second) 만큼 대기 후 리턴.-->StartCoroutine(실행할 스킬, 대기시간)실행.
        Debug.Log("explode execute!");
        
    }
#endif
    private bool isPaused = false;
    private GameObject OptionPopup;

    //인게임 일시정지 화면 컨트롤러 스크립트
    public GameObject PauseUI;
    public Button QuitButton;
    public Button ResumeButton;
    public Button OptionButton;

    public Sprite QuitButtonClicked;
    public Sprite ResumeButtonClicked;
    public Sprite OptionButtonClicked;

    private Image QuitButtonImage;
    private Image ResumeButtonImage;
    private Image OptionButtonImage;

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Game;  
    }

    void Start()
    {
        PauseUI.gameObject.SetActive(false);

        QuitButtonImage = QuitButton.GetComponent<Image>();
        ResumeButtonImage = ResumeButton.GetComponent<Image>();
        OptionButtonImage = OptionButton.gameObject.GetComponent<Image>();

        QuitButton.GetComponent<Button>().onClick.AddListener(OnQuitButtonClicked);
        ResumeButton.GetComponent<Button>().onClick.AddListener(OnResumeButtonClicked);
        OptionButton.GetComponent<Button>().onClick.AddListener(OnOptionButtonClicked);
    }

    private void Update()
    {
        // Esc 키 입력 처리
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                if (OptionPopup != null)
                {
                    ClearPopup();
                }
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        if (PauseUI != null)
        {
            isPaused = true;
            PauseUI.gameObject.SetActive(true);
            CursorVisible();
            Time.timeScale = 0f; // 게임을 일시 정지
        }
    }


    private void ResumeGame()
    {
        if (PauseUI != null)
        {
            isPaused = false;
            PauseUI.gameObject.SetActive(false);
            CursorInvisible();
            Time.timeScale = 1f; // 게임을 재개합니다.
        }
    }


    void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnResumeButtonClicked()
    {
        ResumeGame();
    }
    void OnOptionButtonClicked()
    { 
        if(OptionPopup ==null)
        {
            GameObject OptionPrefab = Resources.Load<GameObject>("Prefabs/UI/UI_Option");
            if(OptionPrefab!=null)
            {
                OptionPopup = Instantiate(OptionPrefab, transform);//경로상에 미리 저장해두었던 옵션 팝업을 프리팹화하여 화면에 띄운다.
                PauseUI.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("UI_Option prefab not found at Assets/Resources/Prefabs/UI/");
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUI.gameObject.SetActive(true) ;
            ClearPopup();
        }
    }

    void ClearPopup()
    {
        if(OptionPopup!=null)
        {
            Destroy(OptionPopup);
            OptionPopup = null;
        }
    }

    public override void Clear()
    {
        Debug.Log("Game End");
    }

    private void CursorVisible()
    {
        if(Cursor.lockState == CursorLockMode.Locked && !Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    private void CursorInvisible()
    {
        if(Cursor.lockState == CursorLockMode.None && Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}
