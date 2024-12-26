using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public AudioSource Ado;

    void Start()
    {
        if(Ado!=null)
        {
            Ado.Play();
            Ado.loop = true;
        }
        else
        {
            Debug.LogError("Audio Source is NULL !");
        }
    }

}
