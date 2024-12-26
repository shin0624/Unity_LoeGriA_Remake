using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        AudioManager audioManager = this;
        DontDestroyOnLoad(gameObject);      
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        if(audioSource!=null && clip!=null)
        {
            audioSource.PlayOneShot(clip);// playonshot���� ����ؾ� clip���� ��ø�Ǿ ������� ����
        }
    }
    public void StopSound(AudioClip clip)//���� ��ž
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop(); // ���� ��� ���� ���� ����
            audioSource.loop = false; // ���� ����
            audioSource.clip = null; // Ŭ�� ����
        }
    }

    public void LoopSound(AudioClip clip)//���� ����. playOnShout�� �ܹ߼��̶� ���� �ȵ�
    {
        if (audioSource != null && clip != null)
        {
            if (audioSource.isPlaying && audioSource.clip == clip)
            {
                return;//�̹� �ش� Ŭ���� ��� ���̸� ����� �ߺ����� �������� �ʵ���.
            }
            audioSource.clip = clip;//������ҽ��� Ŭ�� ����.
            audioSource.loop = true;//���� ����
            audioSource.Play();
        }
    }

}
