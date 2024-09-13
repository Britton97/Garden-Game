using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPitchRandomizer : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 2f)]
    private float pitchVariation = 0.1f;
    [SerializeField] 
    [Range(-1f, 1f)]
    float pitchOffset = 0;
    [SerializeField]
    private DataFloat_SO globalvolume;
    [SerializeField]
    private DataFloat_SO specificVolume;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.pitch = Random.Range(1 - pitchVariation, 1 + pitchVariation) + pitchOffset;
        audioSource.volume = globalvolume.data * specificVolume.data;
        audioSource.PlayOneShot(clip);
    }
}
