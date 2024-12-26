using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental;

public class Skybox_SunLightController : MonoBehaviour
{
    // 스카이박스의 _Rotation 변화에 Directional Light x축 회전값 변화를 추가하여 낮밤 표현의 현실성을 더하기 위한 스크립트
    //낮과 밤 사이에 황혼을 추가함
    [Header("SkyBoxMaterial")]
    [SerializeField] private Material dayMat;
    [SerializeField] private Material nightMat;
    [SerializeField] private Material twilightMat;

    [Header("Light Setting")]
    [SerializeField] private Light SceneLight;//단일 조명을 사용
    [SerializeField] private Color nightLightColor = new Color(0.3524386f, 0.3782536f, 0.4528302f, 1.0f);//밤에도 살짝 보여야 하니까 연한 먹색으로 설정
    [SerializeField] private Color twilightLightColor = new Color(1f, 0.4646714f, 0f);//황혼 마테리얼과 유사한 밝은 오렌지색 조명을 설정 

    [Header("Fog Setting")]
    [SerializeField] private Color nightFog = new Color(0.1666073f, 0.2164471f, 0.3018868f, 1.0f);
    [SerializeField] private Color twilightFog = new Color(0.4245283f, 0.1782218f, 0.1782218f, 1.0f);//황혼 안개색(붉은색 + 황갈색)

    [Header("Rotation Setting")]
    [SerializeField] private float rotationSpeed = 1.5f;//스카이박스 로테이션 속도
    [SerializeField] private float currentRotation = 0f;//현재 로테이션 값(0 ~ 360) --> 황혼을 추가하였으니 360 이전에 황혼 후 360도달 시 밤으로.
   
    private enum TimeOfDay { Day, Twilight, Night }
    private TimeOfDay currentTimeOfDay = TimeOfDay.Day;

    private void Awake()
    {
        GameObject SceneLightObject = GameObject.Find("Directional Light");
        if(SceneLightObject!=null)
        {
            SceneLight = SceneLightObject.GetComponent<Light>();
        }
    }

    void Update()
    {
        currentRotation += Time.deltaTime * rotationSpeed;
        RenderSettings.skybox.SetFloat("_Rotation", currentRotation);//스카이박스의 _Rotation 값에 접근해서 float값을 바꾸어준다. 로컬 시간 * 1.5f로 시간에 따라 변화하도록 설정

        //조명 회전값을 스카이박스 로테이션에 맞추어 조정
        //SceneLight.transform.rotation = Quaternion.Euler(currentRotation - 90f, 170f, 0f);

        if (currentRotation >= 360f)//스카이박스 로테이션이 160까지 왔다면 초기화 후 다른 스카이박스로 변경.
        {
            currentRotation = 0f;
            ToggleSkybox();

        }
        else if(currentRotation >=250f && currentRotation < 360f && currentTimeOfDay==TimeOfDay.Day)
        {
            //스카이박스 로테이션이 250<=_Rotation<360이며 황혼이 아닐 때
            ToggleSkybox();
        }

    }

    private void ToggleSkybox()
    {
        if (currentTimeOfDay == TimeOfDay.Day)//낮일 때 -> 황혼으로
        {
            currentTimeOfDay = TimeOfDay.Twilight;
            SetTwilight();
            Debug.Log($"CurrentTimeOfDay : {currentTimeOfDay}");
        }
        else if(currentTimeOfDay == TimeOfDay.Twilight)//황혼일 때 -> 밤으로
        {
            currentTimeOfDay = TimeOfDay.Night;
            SetNight();
            Debug.Log($"CurrentTimeOfDay : {currentTimeOfDay}");
        }
        else if(currentTimeOfDay == TimeOfDay.Night)//밤일 때 -> 낮으로
        {
            currentTimeOfDay = TimeOfDay.Day;
            SetDay();
            Debug.Log($"CurrentTimeOfDay : {currentTimeOfDay}");
        }
    }
    
    private void SetDay()
    {
        RenderSettings.skybox = dayMat;//낮으로 세팅
        //SceneLight.color = dayLightColor;
        SceneLight.color = Color.Lerp(nightLightColor, Color.white, currentRotation);
        RenderSettings.fogColor = Color.clear;
    }
    private void SetTwilight()
    {
        currentTimeOfDay = TimeOfDay.Twilight;//황혼으로 세팅
        RenderSettings.skybox = twilightMat;
        SceneLight.color = twilightLightColor;
        RenderSettings.fogColor = twilightFog;
    }
    private void SetNight()
    {
        RenderSettings.skybox = nightMat;//밤으로 세팅
        SceneLight.color = nightLightColor;
        RenderSettings.fogColor = nightFog;
    }
}
