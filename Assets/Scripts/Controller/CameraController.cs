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

    private float pitch = 0.0f;//���� ȸ��
    private float yaw = 0.0f;//�¿� ȸ��

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
        pitch = Mathf.Clamp(pitch, -30.0f, 60.0f);//���� ���� ����
    }

    private void UpdateCameraPosition()//ȸ�� ����
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0.0f);
        transform.position = player.position + rotation * offset;
        transform.LookAt(player.position + Vector3.up * 1.5f);//�÷��̾ �ٶ󺸵���
    }
}
