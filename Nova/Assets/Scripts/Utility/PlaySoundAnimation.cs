using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to play a certain sound in a animation or other unity event.
/// </summary>
public class PlaySoundAnimation : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }
}
