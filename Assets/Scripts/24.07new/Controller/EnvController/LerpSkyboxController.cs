using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpSkyboxController : MonoBehaviour
{
    //선형보간을 사용한 스카이박스 컨트롤러. --> 기존 스카이박스 컨트롤러의 단점인 부드럽지 못한 변환을 고쳐보기 위해 제작

    public Material dayMat;
    public Material nightMat;

    public GameObject dayLight;
    public GameObject nightLight;

    public Color dayFog;
    public Color nightFog;

    private float rotationSpeed = 1.5f;//스카이박스 로테이션 속도
    private float currentRotation = 0f;//현재 로테이션 값(0 ~ 360) --> 360은 너무 길어서 160정도가 적당할듯..
    private bool isDay = true;//낮인지 밤인지 구분을 위해 생성
    private bool isTransitioning = false;//자연스러운 변환을 위해, 변환이 진행 중임을 나타내는 플래그 선언->중복실행 방지


    void Update()
    {
        currentRotation += Time.deltaTime * rotationSpeed;
        RenderSettings.skybox.SetFloat("_Rotation", currentRotation);//스카이박스의 _Rotation 값에 접근해서 float값을 바꾸어준다. 로컬 시간 * 1.5f로 시간에 따라 변화하도록 설정

        if (currentRotation >= 166f)//스카이박스 로테이션이 160까지 왔다면 초기화 후 다른 스카이박스로 변경.
        {
            currentRotation = 0f;
            if(!isTransitioning)
            {
                StartCoroutine(TransitionSkybox());
            }

        }
    }

    private IEnumerator TransitionSkybox()//선형보간을 통해 자연스러운 스카이박스 전환이 가능하도록 하는 코루틴 선언
    {
        isTransitioning = true;
        //시작값 선언부
        Material startMat = RenderSettings.skybox;
        Material endMat = isDay ? nightMat : dayMat;

        Color startFog = RenderSettings.fogColor;
        Color endFog = isDay ? nightFog : dayFog;

        GameObject startLight = isDay ? dayLight: nightLight;
        GameObject endLight = isDay ? nightLight: dayLight;

        float duration = 2.0f;//전환에 걸리는 시간(초)
        float elapsed = 0f;//경과시간 초기화 = 0

        while(elapsed < duration)//경과시간이 전환시간보다 작을 동안 루프 실행
        {
            elapsed += Time.deltaTime;//시간이 경과함에 따라 업데이트
            float t = elapsed / duration;//현재 전환상태를 0 ~ 1 사이 값으로 나타낸다. elapsed = duration이 되면 t = 1

            RenderSettings.skybox.Lerp(startMat, endMat, t);//경과시간/전환시간에 따라 선형보간
            RenderSettings.fogColor = Color.Lerp(startFog, endFog, t);
            startLight.SetActive(false);
            endLight.SetActive(true);
            yield return null;//한 프레임 대기 후 다음 프레임에서 루프 재개
        }

        //전환 완료 후 최종 설정
        RenderSettings.skybox = endMat;
        RenderSettings.fogColor = endFog;
        startLight.SetActive(false);
        endLight.SetActive(true);
        isDay = !isDay;
        isTransitioning = false;
    }

   
}
