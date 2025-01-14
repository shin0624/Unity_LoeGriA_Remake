using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    /*
    250115 수정 : 씬 활성화와 sceneloaded 이벤트 호출 간 시점 불일치 떄문에 의도한 스폰 지점에 플레이어가 스폰되지 않음.

    nextScene 값이 로딩 씬에서 설정되지만, PlayerSpawnManger는 이 정보를 알지 못한 채 동작해버림

    -> 해결 방법 : LoadingScene과 PlayerSpawnManager 간 씬 활성화 시점 동기화. PlayerSpawnManager에서, 스폰 포인트 설정 지점을 OnEnable로 변경하고, LoadingScene에서 op.allowSceneActivation 설정 전에 동작을 추가하여
    씬 활성화 후 명시적으로 활성화된 씬을 설정.
    */
    public static string nextScene;

   [Header("Loading")]
    public Image Progress;

    private void Start()
    {
        StartCoroutine(LoadSceneCoroutine()) ;//코루틴 호출->로딩 씬 시작 시 다음 씬을 비동기적으로 로드
    }
    public static void LoadScene(string SceneName)//정적으로 선언하여 다른 스크립트에서 쉽게 호출 가능
    {
        nextScene = SceneName;
        SceneManager.LoadScene("Loading");
    }

    IEnumerator LoadSceneCoroutine()//로딩씬 코루틴 
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);//비동기 방식의 씬 로드를 사용하여, 씬 로드 중에도 다른 작업이 가능
        
        op.allowSceneActivation = false;//로딩의 진행정도 op를 AcyncOperation 형으로 반환

        //op.allowSceneActivation --> 씬의 로딩이 끝나면 자동으로 불러온 씬으로 이동할 것인가를 묻는 옵션. false로 설정하여 로딩 완료 시 다음 씬으로 전환되지 않고 대기 -> true가 될 때 마무리 로딩 후 씬 전환

        float timer = 0.0f;
        while(!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                Progress.fillAmount = Mathf.Lerp(Progress.fillAmount, op.progress, timer);
                if (Progress.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                Progress.fillAmount = Mathf.Lerp(Progress.fillAmount, 1f, timer);
                if (Progress.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield return new WaitUntil(()=> op.isDone);//비동기 씬 로드 작업 완료 후 활성화되기까지 대기-> Scenemanager.SetActiveScene 호출 시 씬이 활성화되지 않아 발생했던 문제 해결
                    Scene activeScene = SceneManager.GetSceneByName(nextScene);//활성화된 씬 설정
                    if(activeScene.IsValid())//액티브 씬 유효성 체크
                    {
                        SceneManager.SetActiveScene(activeScene);
                    }
                    yield break;
                }
            }
        }  
    }
}
