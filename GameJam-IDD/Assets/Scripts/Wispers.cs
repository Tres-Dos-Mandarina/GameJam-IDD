using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wispers : MonoBehaviour
{
    public AudioSource audio;

    private void Update()
    {
        Invoke("PlaySFX",5);
    }
    public void PlaySFX()
    {
        float rnd = Random.Range(-1.0f, 1.1f);
        audio.panStereo = rnd;
    }
}
