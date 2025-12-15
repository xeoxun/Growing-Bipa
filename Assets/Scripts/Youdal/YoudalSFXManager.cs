using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoudalSFXManager : MonoBehaviour
{
    public static YoudalSFXManager Instance;

    private AudioSource audioSource;

    [Header("UI Effect Sounds")]
    public AudioClip starClip;

    public AudioClip gameoverClip;
    public AudioClip moneyClip;
    // ÇÊ¿äÇÑ È¿°úÀ½ Ãß°¡

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // »Ð¸ÁÄ¡ ÈÖµÎ¸¦¶§
    public void PlayStarSFX()
    {
        PlaySFX(starClip);
    }

    public void PlayGameOverSFX()
    {
        PlaySFX(gameoverClip);
    }

    public void PlayMoneySFX()
    {
        PlaySFX(moneyClip);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.PlayOneShot(clip);
    }
}
