using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    private AudioSource audioSource;

    [Header("BGM Clips")]
    public AudioClip menuWorldBGM;
    public AudioClip storeBGM;
    public AudioClip houseBGM;
    public AudioClip festivalBGM;

    public AudioClip sashimiBGM;
    public AudioClip nakjiBGM;
    public AudioClip muhwaguaBGM;
    public AudioClip youdalBGM;

    private void Awake()
    {
        // 싱글톤 처리
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.loop = true;
            audioSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
            return; // 중복 방지
        }
    }

    public void PlayBGM(string sceneName)
    {
        AudioClip nextClip = GetClipByScene(sceneName);

        if (nextClip == null)
            return;

        // 이미 같은 BGM이면 재생 안함
        if (audioSource.isPlaying && audioSource.clip == nextClip) return;

        audioSource.Stop();
        audioSource.clip = nextClip;
        audioSource.Play();
    }

    private AudioClip GetClipByScene(string sceneName)
    {
        switch (sceneName)
        {
            case "Menu":
            case "World": return menuWorldBGM;
            case "Store": return storeBGM;
            case "House": return houseBGM;
            case "Festival": return festivalBGM;
            case "Fish": return sashimiBGM;
            case "Nakji": return nakjiBGM;
            case "Muhwagua": return muhwaguaBGM;
            case "Mountain": return youdalBGM;
            default: return null;
        }
    }

    /// <summary>
    /// 씬 전환 시 자동 재생용
    /// </summary>
    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        PlayBGM(scene.name);
    }
}
