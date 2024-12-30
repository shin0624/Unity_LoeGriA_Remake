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


    public enum CameraMode //����� ī�޶� ��带 �ϳ��� �߰��ϱ� ���� ��ũ��Ʈ
    {
        QuarterView,
        FirstPersonView,

    };


}
