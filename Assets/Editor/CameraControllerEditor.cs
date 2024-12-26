
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor
{
    //ī�޶� ���� ���ÿ� ���� inspector�� �ٸ� �޴��� ���������� ������ ����
    public override void OnInspectorGUI()
    {
        CameraController cont = (CameraController)target;

        cont._mode = (Define.CameraMode)EditorGUILayout.EnumPopup("Camera Mode", cont._mode);
        if (cont._mode == Define.CameraMode.QuarterView)
        {
            cont._delta = EditorGUILayout.Vector3Field("Delta", cont._delta);
            cont._player = (GameObject)EditorGUILayout.ObjectField("Player", cont._player, typeof(GameObject), true);
        }
        else if (cont._mode == Define.CameraMode.FirstPersonView)
        {
            cont.mouseSpeed = EditorGUILayout.FloatField("Mouse Speed", cont.mouseSpeed);
            cont.rotationSmoothTime = EditorGUILayout.FloatField("Rotation Smooth Time", cont.rotationSmoothTime);
            cont._player = (GameObject)EditorGUILayout.ObjectField("Player", cont._player, typeof(GameObject), true);
            cont.cam = (Camera)EditorGUILayout.ObjectField("Camera", cont.cam, typeof(Camera), true);
            cont.fpvOffset = EditorGUILayout.Vector3Field("First Person Offset", cont.fpvOffset);
        }
        if (GUI.changed) { EditorUtility.SetDirty(cont); }
    }
}

