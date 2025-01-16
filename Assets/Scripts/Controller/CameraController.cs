using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0.0f, 5.0f, -7.0f);
    [SerializeField] private float sensitivity = 5.0f;

    private float pitch = 0.0f;//상하 회전
    private float yaw = 0.0f;//좌우 회전

    private void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void LateUpdate() 
    {
        HandleCameraRotation();
        UpdateCameraPosition();
    }

    private void HandleCameraRotation()
    {
        yaw+=Input.GetAxis("Mouse X") * sensitivity;
        pitch-=Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, -30.0f, 60.0f);//상하 각도 제한
    }

    private void UpdateCameraPosition()//회전 적용
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0.0f);
        transform.position = player.position + rotation * offset;
        transform.LookAt(player.position + Vector3.up * 1.5f);//플레이어를 바라보도록
    }
}
