using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
  public enum Audio
    {
        Main = 0,
        Stage = 1,
    };

    public List<AudioClip> audioList = new List<AudioClip>();

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SetSound(SoundEffect.Audio.Main);
    }

    public void SetSound(Audio index)
    {
        audioSource.clip = audioList[(int)index];
        audioSource.Play();
    }
}
