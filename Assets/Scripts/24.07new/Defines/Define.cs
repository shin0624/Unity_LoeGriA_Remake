using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum PlayerState
    {
        IDLE,
        WALKING,
        RUNNING,
        ATTACK,
        JUMPING,

    };

    public enum EnemyState
    {
        IDLE,
        WALKING,
        RUNNING,
        ATTACK,
        READY,
        READYOFF,
        HIT,
        DIE,
    
    };

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
        Options,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }


    public enum UIEvent
    {
        Click,
        Drag,
    }


    public enum MouseEvent
    {
        Press,
        Click,

    };


    public enum CameraMode //사용할 카메라 모드를 하나씩 추가하기 위한 스크립트
    {
        QuarterView,
        FirstPersonView,

    };


}
