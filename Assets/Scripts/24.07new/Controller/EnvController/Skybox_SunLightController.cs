using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental;

public class Skybox_SunLightController : MonoBehaviour
{
    // ��ī�̹ڽ��� _Rotation ��ȭ�� Directional Light x�� ȸ���� ��ȭ�� �߰��Ͽ� ���� ǥ���� ���Ǽ��� ���ϱ� ���� ��ũ��Ʈ
    //���� �� ���̿� Ȳȥ�� �߰���
    [Header("SkyBoxMaterial")]
    [SerializeField] private Material dayMat;
    [SerializeField] private Material nightMat;
    [SerializeField] private Material twilightMat;

    [Header("Light Setting")]
    [SerializeField] private Light SceneLight;//���� ������ ���
    [SerializeField] private Color nightLightColor = new Color(0.3524386f, 0.3782536f, 0.4528302f, 1.0f);//�㿡�� ��¦ ������ �ϴϱ� ���� �Ի����� ����
    [SerializeField] private Color twilightLightColor = new Color(1f, 0.4646714f, 0f);//Ȳȥ ���׸���� ������ ���� �������� ������ ���� 

    [Header("Fog Setting")]
    [SerializeField] private Color nightFog = new Color(0.1666073f, 0.2164471f, 0.3018868f, 1.0f);
    [SerializeField] private Color twilightFog = new Color(0.4245283f, 0.1782218f, 0.1782218f, 1.0f);//Ȳȥ �Ȱ���(������ + Ȳ����)

    [Header("Rotation Setting")]
    [SerializeField] private float rotationSpeed = 1.5f;//��ī�̹ڽ� �����̼� �ӵ�
    [SerializeField] private float currentRotation = 0f;//���� �����̼� ��(0 ~ 360) --> Ȳȥ�� �߰��Ͽ����� 360 ������ Ȳȥ �� 360���� �� ������.
   
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
        RenderSettings.skybox.SetFloat("_Rotation", currentRotation);//��ī�̹ڽ��� _Rotation ���� �����ؼ� float���� �ٲپ��ش�. ���� �ð� * 1.5f�� �ð��� ���� ��ȭ�ϵ��� ����

        //���� ȸ������ ��ī�̹ڽ� �����̼ǿ� ���߾� ����
        //SceneLight.transform.rotation = Quaternion.Euler(currentRotation - 90f, 170f, 0f);

        if (currentRotation >= 360f)//��ī�̹ڽ� �����̼��� 160���� �Դٸ� �ʱ�ȭ �� �ٸ� ��ī�̹ڽ��� ����.
        {
            currentRotation = 0f;
            ToggleSkybox();

        }
        else if(currentRotation >=250f && currentRotation < 360f && currentTimeOfDay==TimeOfDay.Day)
        {
            //��ī�̹ڽ� �����̼��� 250<=_Rotation<360�̸� Ȳȥ�� �ƴ� ��
            ToggleSkybox();
        }

    }

    private void ToggleSkybox()
    {
        if (currentTimeOfDay == TimeOfDay.Day)//���� �� -> Ȳȥ����
        {
            currentTimeOfDay = TimeOfDay.Twilight;
            SetTwilight();
            Debug.Log($"CurrentTimeOfDay : {currentTimeOfDay}");
        }
        else if(currentTimeOfDay == TimeOfDay.Twilight)//Ȳȥ�� �� -> ������
        {
            currentTimeOfDay = TimeOfDay.Night;
            SetNight();
            Debug.Log($"CurrentTimeOfDay : {currentTimeOfDay}");
        }
        else if(currentTimeOfDay == TimeOfDay.Night)//���� �� -> ������
        {
            currentTimeOfDay = TimeOfDay.Day;
            SetDay();
            Debug.Log($"CurrentTimeOfDay : {currentTimeOfDay}");
        }
    }
    
    private void SetDay()
    {
        RenderSettings.skybox = dayMat;//������ ����
        //SceneLight.color = dayLightColor;
        SceneLight.color = Color.Lerp(nightLightColor, Color.white, currentRotation);
        RenderSettings.fogColor = Color.clear;
    }
    private void SetTwilight()
    {
        currentTimeOfDay = TimeOfDay.Twilight;//Ȳȥ���� ����
        RenderSettings.skybox = twilightMat;
        SceneLight.color = twilightLightColor;
        RenderSettings.fogColor = twilightFog;
    }
    private void SetNight()
    {
        RenderSettings.skybox = nightMat;//������ ����
        SceneLight.color = nightLightColor;
        RenderSettings.fogColor = nightFog;
    }
}
