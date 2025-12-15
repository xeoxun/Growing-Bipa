using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NakjiSFXManager : MonoBehaviour
{
    public static NakjiSFXManager Instance;

    private AudioSource audioSource;

    [Header("UI Effect Sounds")]
    public AudioClip hammerClip;
    public AudioClip OctopusClip;

    public AudioClip gameoverClip;
    public AudioClip moneyClip;
    // 필요한 효과음 추가

    private void Awake()
    {
        // 씬 내에서 싱글톤 역할만
        if (Instance == null)
        {
            Instance = this;

            // AudioSource 확인
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.loop = false;
            audioSource.playOnAwake = false;
            audioSource.ignoreListenerPause = true; // UI 사운드가 Time.timeScale=0에서도 재생
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // 씬 내 중복 제거
            return;
        }
    }

    // 뿅망치 휘두를때
    public void PlayHammerSFX()
    {
        PlaySFX(hammerClip);
    }

    public void PlayOctopusSFX()
    {
        PlaySFX(OctopusClip);
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
