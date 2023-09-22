using UnityEngine;

public class Wispers : MonoBehaviour
{
    private AudioSource _audio;
    public AudioClip music;
    public AudioClip wispers;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }
    
    public void PlaySFX()
    {
        float rnd = Random.Range(-1.0f, 1.1f);
        _audio.panStereo = rnd;
    }

    public void TurnOffLight(Component sender, object data)
    {
        _audio.clip = wispers;
        _audio.Play();
    }
    public void TurnOnLight(Component sender, object data)
    {
        _audio.clip = music;
        _audio.Play();
    }
}
