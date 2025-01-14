using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class WestFieldTimelineFinished : MonoBehaviour
{
    [SerializeField] private PlayableDirector pd;
    [SerializeField] private GameObject timelineSettings;
    
    void OnEnable() 
    {
        pd.stopped+=OnTimelineFinished;
    }

    
    void Update()
    {
        
    }
    void OnTimelineFinished(PlayableDirector pd)
    {
        timelineSettings.SetActive(false);
    }
}
