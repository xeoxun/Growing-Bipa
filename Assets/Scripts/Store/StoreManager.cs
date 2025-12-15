using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
//using static UnityEditor.Timeline.TimelinePlaybackControls;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance;

    public RectTransform errorBox;

    public Text coinText;
    public Text hungryText;
    public Text heartText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        // 매 프레임 갱신하거나, 이벤트로 바꿔도 됨
        //UpdateUI();
    }

    //void UpdateUI()
    //{
    //    if (LevelManager.instance != null)
    //    {
    //        coinText.text = LevelManager.instance.GetCoin() + " 원";
    //        hungryText.text = LevelManager.instance.GetCoin() + "%";
    //        heartText.text = LevelManager.instance.GetCoin() + "%";
    //    }
    //}

    public void ToWorld()
    {
        MenuManager.cameFromGame = true;
        SceneManager.LoadScene("World");
    }

    public void ShowErrorBox()
    {
        if (errorBox == null) return;

        float originalX = 400f;   // 원래 위치
        float targetX = -300f;    // 나타날 위치

        errorBox.anchoredPosition = new Vector2(originalX, errorBox.anchoredPosition.y);

        SoundManager.Instance.PlaySlidePanel();
        // 이동
        errorBox.DOAnchorPosX(targetX, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            // 3초 뒤 다시 원래 자리로 이동
            errorBox.DOAnchorPosX(originalX, 0.5f).SetDelay(2f).SetEase(Ease.InBack);
        });
    }
}
