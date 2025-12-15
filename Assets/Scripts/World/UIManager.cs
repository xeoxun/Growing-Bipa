using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public Text UiPlaceName;
    public DOTween dotAnim;
    public static UIManager instance;

    public RectTransform talkPanel;

    public Button stopBtn;

    //public static bool showExitGroup = false;

    private Dictionary<string, string> placeSceneMap = new Dictionary<string, string>
    {
        { "유달산", "Mountain" },
        { "무화과", "Muhwagua" },
        { "낙지잡이", "Nakji" },
        { "회 센터", "Fish" },
        { "집", "House" },
        { "항구포차", "Store" },
        { "관광", "Festival" }
    };

    void Awake()
    {
        transform.DOKill();
        Time.timeScale = 1f;

        if (instance == null)
        {
            instance = this;
            //           DontDestroyOnLoad(gameObject);  // 씬 전환해도 유지
            //SceneManager.sceneLoaded += OnSceneLoaded; // 씬 로드 콜백 등록
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // 씬이 로드될 때 호출
    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    // World 씬에서만 stopBtn 연결
    //    if (scene.name == "World")
    //    {
    //        stopBtn = GameObject.Find("StopBtn")?.GetComponent<Button>();
    //        if (stopBtn != null)
    //        {
    //            stopBtn.onClick.RemoveAllListeners();
    //            //stopBtn.onClick.AddListener(MoveMain);
    //        }

    //        UiPlaceName = GameObject.Find("Text")?.GetComponent<Text>();
    //    }
    //}

    public void SetPlace(string placeName)
    {
        UiPlaceName.text = placeName;
    }

    public void TrySceneChange()
    {
        if (UiPlaceName == null) return;

        string place = UiPlaceName.text;
        if (placeSceneMap.TryGetValue(place, out string sceneName))
        {
            Debug.Log($"{place} 맵으로 이동합니다.");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning($"UiPlaceName '{place}'에 해당하는 씬이 없습니다!");
        }
    }

    public void ShowTalkPanel()
    {
        if (talkPanel == null) return;

        SoundManager.Instance.PlaySlidePanel();

        talkPanel.DOAnchorPos(new Vector2(380f, 140f), 0.5f)
             .SetEase(Ease.OutExpo)
             .OnComplete(() =>
             {
                 DOVirtual.DelayedCall(2f, () =>
                 {
                     talkPanel.DOAnchorPos(new Vector2(-400f, 140f), 0.5f)
                              .SetEase(Ease.InExpo);
                 });
             });
    }
}
