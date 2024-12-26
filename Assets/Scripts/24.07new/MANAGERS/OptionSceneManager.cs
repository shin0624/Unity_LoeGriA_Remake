using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionSceneManager : BaseScene
{
    //옵션 씬 매니저

    public Button HomeButton;
    public Sprite HomeButtonClicked;
    private Image HomeButtonImage;

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Options;
    }

    void Start()
    {
        HomeButtonImage = HomeButton.GetComponent<Image>();
        HomeButton.GetComponent<Button>().onClick.AddListener(OnHomeButtonClicked);

    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Login");
        }
    }

    void OnHomeButtonClicked()
    {
        HomeButtonImage.sprite = HomeButtonClicked;
        SceneManager.LoadScene("Login");
    }





    public override void Clear()
    {
        Debug.Log("Option Scene Clear");
    }
}
