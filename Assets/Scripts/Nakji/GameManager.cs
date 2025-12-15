using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject[] octopusPrefabs; // 3종류 낙지 프리팹
    private Vector2[] holePositions;     // 7개 구멍 위치

    public float spawnInterval = 2f;
    private int score = 0;

    public Text scoreText;
    public Text timeText; // 제한 시간 표시용

    public GameObject gameOverUI; // GameOver UI 패널
    public GameObject coinPanel;
    public Text coinText;

    private float timer = 60f; // 제한 시간 60초
    private bool isGameOver = false;
    private bool isGameStarted = false; // 게임 시작 여부

    void Awake()
    {
        Application.targetFrameRate = 75;

        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        holePositions = new Vector2[]
        {
            new Vector2(-3.3f, 0f),
            new Vector2(3.8f, -0.7f),
            new Vector2(-6.9f, -2.4f),
            new Vector2(-0.1f, -2.4f),
            new Vector2(7f, -3f),
            new Vector2(-3.6f, -4.8f),
            new Vector2(3.2f, -4.8f)
        };

        UpdateScoreUI();
        UpdateTimeUI();

        InvokeRepeating("SpawnOctopuses", 1f, spawnInterval);
    }

    void Update()
    {
        if (!isGameStarted || isGameOver) return; // 게임 시작 전에는 아무것도 안 함

        // 시간 감소
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = 0f;
            StartCoroutine(GameOverRoutine());
        }

        UpdateTimeUI();
    }

    void SpawnOctopuses()
    {
        if (!isGameStarted || isGameOver) return; // 시작 전에는 낙지 생성 X

        // 1~7개의 구멍을 랜덤 선택
        int count = Random.Range(1, 8);

        List<int> availableHoles = new List<int>();
        for (int i = 0; i < holePositions.Length; i++)
            availableHoles.Add(i);

        for (int i = 0; i < count; i++)
        {
            if (availableHoles.Count == 0) break;

            int holeIndex = Random.Range(0, availableHoles.Count);
            Vector2 holePos = holePositions[availableHoles[holeIndex]];
            availableHoles.RemoveAt(holeIndex);

            // 낙지 종류 무작위 선택
            int octopusType = Random.Range(0, octopusPrefabs.Length);
            GameObject octoObj = Instantiate(octopusPrefabs[octopusType], holePos, Quaternion.identity);

            // 낙지 올라오는 모션 실행
            Octopus octopus = octoObj.GetComponent<Octopus>();
            octopus.AppearAt(holePos);
        }
    }

    public void AddScore(int scoreValue)
    {
        score += scoreValue;
        Debug.Log("Score Added: " + scoreValue + ", Total Score: " + score);

        scoreText.text = "점수: " + score;
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "점수: " + score;
        }
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
            NakjiSFXManager.Instance.PlayGameOverSFX();

            // 3초 동안 GameOver UI 표시
            yield return new WaitForSecondsRealtime(3f);

            gameOverUI.SetActive(false);
            coinPanel.SetActive(true);
            NakjiSFXManager.Instance.PlayMoneySFX();

            int coinScore = Mathf.RoundToInt(score * 0.8f);
            coinText.text = "+" + coinScore.ToString();

            int getExp = Mathf.RoundToInt(score * 0.3f);

            if (LevelManager.instance != null)
            {
                LevelManager.instance.AddCoin(coinScore);
                LevelManager.instance.AddExp(getExp);
                LevelManager.instance.SaveHighScore("Octopus", score);
            }

            // 4초 동안 코인 패널 표시
            yield return new WaitForSecondsRealtime(4f);

            coinPanel.SetActive(false);

            // 예시: 메인 씬으로 돌아가기
            MenuManager.cameFromGame = true;
            SceneManager.LoadScene("World");
        }

        Debug.Log("게임 종료!");
    }

    public void StartGame()
    {
        Debug.Log("게임 시작!");

        isGameStarted = true;
    }
}
