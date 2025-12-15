using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager_s : MonoBehaviour
{
    public static GameManager_s Instance;

    public GameObject sashimiPrefab;

    [HideInInspector]
    public bool isHoldingKnife = false;

    private bool isGameOver = false;
    private bool isGameStarted = false;   //  NPC 대화 끝나야 true

    private float timer = 60f; // 제한 시간 60초
    private int score = 0;

    public Text scoreText;
    public Text timeText; // 제한 시간 표시용

    public GameObject gameOverUI; // GameOver UI 패널
    public GameObject coinPanel;
    public Text coinText;

    // 클릭 횟수 관리
    private Dictionary<GameObject, int> fishClickCounts = new Dictionary<GameObject, int>();

    private void Start()
    {
        UpdateScoreUI();
        UpdateTimeUI();
    }

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

    private void Update()
    {
        if (!isGameStarted || isGameOver) return;   //  게임 시작 전에는 Update 중지

        // 시간 감소
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = 0f;
            StartCoroutine(GameOverRoutine());
        }

        UpdateTimeUI();
    }

    // NPC 대화 끝나면 이 함수를 호출해서 게임 시작
    public void StartGame()
    {
        isGameStarted = true;
        Time.timeScale = 1f; // 혹시 멈춰있을 수도 있으니 정상화

        Debug.Log("게임 시작!");
    }

    // 물고기 클릭 이벤트
    public void RegisterFishClick(GameObject fish, GameObject sashimiObj)
    {
        if (!isGameStarted) return;   // 게임 시작 전엔 클릭 무시
        if (!isHoldingKnife) return;  // 칼 안 들면 무시

        if (!fishClickCounts.ContainsKey(fish))
            fishClickCounts[fish] = 0;

        SashimiSoundManager.Instance.PlayFishSFX();

        fishClickCounts[fish]++;
        Debug.Log($"{fish.name} 클릭 {fishClickCounts[fish]}회");

        if (fishClickCounts[fish] >= 3)
        {
            fish.SetActive(false);
            if (sashimiObj != null)
            {
                Instantiate(sashimiObj, fish.transform.position, Quaternion.identity);
            }
            fishClickCounts.Remove(fish);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
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

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "점수: " + score;
        }
    }

    private IEnumerator GameOverRoutine()
    {
        isGameOver = true;

        if (gameOverUI != null)
        {
            Time.timeScale = 0f;
            
            gameOverUI.SetActive(true);
            SashimiSoundManager.Instance.PlayGameOverSFX();

            // 3초 동안 GameOver UI 표시
            yield return new WaitForSecondsRealtime(3f);

            gameOverUI.SetActive(false);
            coinPanel.SetActive(true);
            SashimiSoundManager.Instance.PlayMoneySFX();

            int coinScore = Mathf.RoundToInt(score * 1.3f);
            coinText.text = "+" + coinScore.ToString();

            int getExp = Mathf.RoundToInt(score * 0.3f);

            if (LevelManager.instance != null)
            {
                LevelManager.instance.AddCoin(coinScore);
                LevelManager.instance.AddExp(getExp);
                LevelManager.instance.SaveHighScore("Sashimi", score);
            }

            // 4초 동안 코인 패널 표시
            yield return new WaitForSecondsRealtime(4f);

            coinPanel.SetActive(false);

            Time.timeScale = 1f;

            // 예시: 메인 씬으로 돌아가기
            MenuManager.cameFromGame = true;
            SceneManager.LoadScene("World");
        }

        Debug.Log("게임 종료!");
    }
}
