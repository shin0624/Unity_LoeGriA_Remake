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
            audioSource.PlayOneShot(clip);// playonshot으로 재생해야 clip끼리 중첩되어도 동시재생 가능
        }
    }
    public void StopSound(AudioClip clip)//사운드 스탑
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop(); // 현재 재생 중인 사운드 중지
            audioSource.loop = false; // 루프 해제
            audioSource.clip = null; // 클립 해제
        }
    }

    public void LoopSound(AudioClip clip)//사운드 루프. playOnShout은 단발성이라 루프 안됨
    {
        if (audioSource != null && clip != null)
        {
            if (audioSource.isPlaying && audioSource.clip == clip)
            {
                return;//이미 해당 클립이 재생 중이면 재생을 중복으로 시작하지 않도록.
            }
            audioSource.clip = clip;//오디오소스에 클립 설정.
            audioSource.loop = true;//루프 설정
            audioSource.Play();
        }
    }

}
