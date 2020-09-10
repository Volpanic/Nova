using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SavePoint : MonoBehaviour
{
    private bool activated = false;
    private Animator animator;
    private AudioSource audioSource;
    private Light2D lightSource;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        lightSource = GetComponent<Light2D>();

        lightSource.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //collision.gameObject.GetComponent<PlayerController>().
            StartFire(collision.gameObject);
        }
    }

    public void StartFire(GameObject player)
    {
        if (!activated)
        {
            animator.Play("SaveTorchFlicker");
            audioSource.Play();

            //Only have one fire active at once
            SavePoint[] points = FindObjectsOfType<SavePoint>();

            foreach (SavePoint point in points)
            {
                if (point != this)
                {
                    point.StopFire();
                }
            }

            GameSaveData gsd = new GameSaveData(transform.position.x, transform.position.y, player.GetComponent<PlayerController>());
            SaveGame.Save(gsd);

            player.GetComponent<PlayerController>().RefillHealth();
            //lightSource.enabled = true;
            activated = true;
        }
    }

    public void StopFire()
    {
        animator.Play("ani_save_torch_idle");
        audioSource.Stop();
        lightSource.intensity = 0;
        lightSource.enabled = false;
        activated = false;
    }
}
