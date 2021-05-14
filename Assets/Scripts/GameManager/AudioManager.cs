using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource steps;
    [SerializeField] private AudioSource wind;
    [SerializeField] private AudioSource water;

    private void Awake()
    {
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        while (true)
        {
            if (Random.value > 0.5)
            {
                wind.Play();
            }
            if (Random.value > 0.5)
            {
                water.Play();
            }
            yield return new WaitForSeconds(10);
        }
    }

    public void PlayWalking()
    {
        if (steps.isPlaying) return;
        steps.Play();
    }
    
    public void StopWalking()
    {
        if (!steps.isPlaying) return;
        steps.Stop();
    }
}
