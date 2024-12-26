using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LoadingSceneTimelinert : MonoBehaviour
{
    //로딩 중 타임라인 재생을 위한 스크립트.
    public PlayableDirector pd;

    void Start()
    {
        //리소스 폴더에 있는 타임라인 에셋을 불러와 재생.
        PlayableAsset timeline = Resources.Load<PlayableAsset>("Prefabs/LoadingTimelinerTimeline");
        if(timeline!=null && pd!=null)
        {
            pd.playableAsset = timeline;
            pd.Play();
        }
        else
        {
            Debug.LogError("타임라인 또는 Playable Director를 찾을 수 없습니다.");
        }
    }


}
