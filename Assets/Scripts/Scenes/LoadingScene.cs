using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
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
        //로딩의 진행정도 op를 AcyncOperation 형으로 반환
        //LoadScene : 동기방식 -> 불러올 씬을 한꺼번에 불러오고 다른 모든 것이 불러오는 동안 기다리는 방식. 로드 중 다린 작업 불가
        op.allowSceneActivation = false;
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
                    yield break;
                }
            }
        }  
    }
}
