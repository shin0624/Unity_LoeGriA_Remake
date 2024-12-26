using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxController : MonoBehaviour
{
    //스카이박스 컨트롤러. --> Cubemap 또는 Panoramic으로 설정된 skybox의 Rotation값을 변화시켜 하늘의 변화를 보여준다.
    //DayFog는 마땅한 값을 못찾아서 없이 함

    public Material dayMat;
    public Material nightMat;

    public GameObject dayLight;
    public GameObject nightLight;

    //public Color dayFog;
    public Color nightFog;

    private float rotationSpeed =1.5f;//스카이박스 로테이션 속도
    private float currentRotation = 0f;//현재 로테이션 값(0 ~ 360) --> 360은 너무 길어서 160정도가 적당할듯..
    private bool isDay = true;//낮인지 밤인지 구분을 위해 생성

    void Update()
    {
        currentRotation += Time.deltaTime * rotationSpeed;
        RenderSettings.skybox.SetFloat("_Rotation", currentRotation);//스카이박스의 _Rotation 값에 접근해서 float값을 바꾸어준다. 로컬 시간 * 1.5f로 시간에 따라 변화하도록 설정
        
        if(currentRotation>=166f)//스카이박스 로테이션이 160까지 왔다면 초기화 후 다른 스카이박스로 변경.
        {
            currentRotation = 0f;
            ToggleSkybox();

        }
    }

   private void ToggleSkybox()
    {
        isDay = !isDay;//낮밤 전환
        if(isDay)
        {
            RenderSettings.skybox = dayMat;//낮으로 세팅
            dayLight.SetActive(true);
            nightLight.SetActive(false);
            //RenderSettings.fogColor = dayFog;
        }
        else
        {
            RenderSettings.skybox = nightMat;//밤으로 세팅
            dayLight.SetActive(false);
            nightLight.SetActive(true);
            RenderSettings.fogColor = nightFog;
        }
    }
}
