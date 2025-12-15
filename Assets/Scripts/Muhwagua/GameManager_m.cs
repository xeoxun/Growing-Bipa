using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager_m : MonoBehaviour
{
    public static GameManager_m Instance;   // 싱글톤

    private int totalScore = 0;

    public Text scoreText;       // Canvas에 있는 UI Text
    public Text timeText;        // 제한 시간 표시용 Text

    public GameObject gameOverUI; // GameOver UI 패널
    public GameObject coinPanel;
    public Text getcoinText;

    private float timer = 60f;   // 제한 시간 60초
    private bool isGameOver = false;
    private bool isGameStarted = false; // 게임 시작 여부

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 중복 제거
            return;
        }

        Application.targetFrameRate = 75;
    }

    private void Start()
    {
        Debug.Log("Start() 함수 실행!");
        UpdateScoreUI();
        UpdateTimeUI();
    }

    private void Update()
    {
        if (!isGameStarted || isGameOver) return; // 시작 전에는 카운트다운 멈춤

        // 시간 감소
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = 0f;
            StartCoroutine(GameOverRoutine());
        }

        UpdateTimeUI();
    }

    public void AddScore(int score)
    {
        if (isGameOver) return;

        totalScore += score;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "점수: " + totalScore;
    }

    void UpdateTimeUI()
    {
        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private IEnumerator GameOverRoutine()
    {
        isGameOver = true;

        if (gameOverUI != null)
        {
            Time.timeScale = 0f;

            gameOverUI.SetActive(true);
            MhgSFXManager.Instance.PlayGameOverSFX();

            yield return new WaitForSecondsRealtime(3f);

            gameOverUI.SetActive(false);
            coinPanel.SetActive(true);
            MhgSFXManager.Instance.PlayMoneySFX();

            int coinScore = Mathf.RoundToInt(totalScore * 1.2f);
            Debug.Log("이번 판 획득 코인: " + coinScore);

            getcoinText.text = "+" + coinScore;

            int getExp = Mathf.RoundToInt(totalScore * 0.3f);

            yield return new WaitUntil(() => LevelManager.instance != null);

            if (LevelManager.instance != null)
            {
                LevelManager.instance.AddCoin(coinScore);
                LevelManager.instance.AddExp(getExp);
                LevelManager.instance.SaveHighScore("Muhwagua", totalScore);
                Debug.Log("저장 완료!");
            }
            else Debug.Log("LevelManager 없음!");



            yield return new WaitForSecondsRealtime(4f);

            coinPanel.SetActive(false);

            Destroy(gameObject);

            MenuManager.cameFromGame = true;
            SceneManager.LoadScene("World");
        }

        Debug.Log("게임 종료!");
    }

    // 외부에서 호출할 시작 함수
    public void StartGame()
    {
        Debug.Log("게임 시작!");

        isGameStarted = true;
    }
}
