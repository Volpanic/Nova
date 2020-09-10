using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioSourceFadeIn : MonoBehaviour
{
    public AudioSource audioSource;

    [Range(0.1f,10f)]
    public float timeToFadeIn = 1.0f;

    private float initialVolume = 0;
    private float timer = 0;

    private bool fadeIn = true;

    // Start is called before the first frame update
    void Start()
    {
        initialVolume = audioSource.volume;
        audioSource.volume = 0;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        fadeIn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn)
        {
            timer = Numbers.Approach(timer, timeToFadeIn, Time.deltaTime);
            audioSource.volume = (timer / timeToFadeIn) * initialVolume;
        }
        else
        {
            timer = Numbers.Approach(timer, 0, Time.deltaTime);
            audioSource.volume = (timer / timeToFadeIn) * initialVolume;

            if(audioSource.volume <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
