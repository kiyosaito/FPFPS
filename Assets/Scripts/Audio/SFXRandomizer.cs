using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXRandomizer : MonoBehaviour
{
    #region Variables
    public AudioClip[] soundVariations;
    public AudioSource audioSource;
    #endregion
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void SoundEffect()
    {
        int randomElement = Random.Range(0, soundVariations.Length);
        audioSource.clip = soundVariations[randomElement];
        audioSource.Play();
    }
    public void SoundEffectStop()
    {
        audioSource.Stop();
    }
}
