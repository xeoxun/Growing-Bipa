using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager_y : MonoBehaviour
{
    public static GameManager_y Instance;

    public int score = 0;
    public Text scoreText;

    public GameObject gameOverUI;
    public GameObject coinPanel;
    public Text coinText;

    private bool isGameOver = false;
    public bool IsGameOver => isGameOver; // PlayerControl에서 접근 가능

    private bool isGameStarted = false;   // NPC 대화 끝나야 true

    void Awake()
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
    }

    void Start()
    {
        UpdateScoreUI();
    }

    void Update()
    {
        if (!isGameStarted || isGameOver) return;  // 게임 시작 전엔 로직 정지

    }

    // NPC 대화 끝나면 호출되는 함수
    public void StartGame()
    {
        isGameStarted = true;

        Debug.Log("게임 시작!");
    }

    public void AddScore(int amount)
    {
        if (!isGameStarted || isGameOver) return;  // 게임 시작 전/종료 후 점수 무효

        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "점수: " + score.ToString();
    }

    public void GameOver()
    {
        if (!isGameStarted || isGameOver) return;  // 게임 시작 전엔 GameOver 불가
        isGameOver = true;

        Time.timeScale = 0f; // 게임 정지

        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        gameOverUI.SetActive(true);
        YoudalSFXManager.Instance.PlayGameOverSFX();

        // 3초 동안 기다린 후
        yield return new WaitForSecondsRealtime(3f);

        gameOverUI.SetActive(false);
        coinPanel.SetActive(true);
        YoudalSFXManager.Instance.PlayMoneySFX();

        int coinScore = Mathf.RoundToInt(score * 1.2f);
        coinText.text = "+" + coinScore.ToString();

        int getExp = Mathf.RoundToInt(score * 0.3f);

        if (LevelManager.instance != null)
        {
            LevelManager.instance.AddCoin(coinScore);
            LevelManager.instance.AddExp(getExp);
            LevelManager.instance.SaveHighScore("Youdal", score);
        }

        yield return new WaitForSecondsRealtime(4f);

        coinPanel.SetActive(false);

        MenuManager.cameFromGame = true;
        SceneManager.LoadScene("World");
    }
}
