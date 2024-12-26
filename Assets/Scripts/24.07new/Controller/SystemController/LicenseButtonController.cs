using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LicenseButtonController : MonoBehaviour
{
    //라이센스 팝업 관리 스크립트. 라이센스는 씬이 아닌 팝업으로 보여주기 위해 버튼과 두 캔버스를 할당
    public Canvas UICanvas;
    public Canvas PopupCanvas;
    public Button LicenseButton;

    private void Start()
    {
        PopupCanvas.gameObject.SetActive(false);//버튼을 클릭하지 않으면 팝업창은 false
        LicenseButton.onClick.AddListener(OpenPopup);//버튼 클릭 시 팝업창 true
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))//esc키를 누르면 팝업 종료
        {
            if(PopupCanvas.gameObject.activeSelf)
            {
                ClosePopup();
            }
        }
    }

    void OpenPopup()
    {
        UICanvas.gameObject.SetActive(false);
        PopupCanvas.gameObject.SetActive(true);
    }
    void ClosePopup()
    {
        PopupCanvas.gameObject.SetActive(false);
        UICanvas.gameObject.SetActive(true);
    }
}
