using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Runtime.InteropServices;
using static APIManager;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public static APIManager apiManager;
    private static int id, userId;
    private static int level = 1;
    private static int hungryGauge = 100;
    private static int heartGauge = 100;
    private static int coin = 100000;
    private static int currentExp = 0, maxExp=100;

    private static int maxFig = 0, maxYudal = 0, maxActopus = 0, maxFish = 0;

    public Text levelText;
    public Text hungryText;
    public Text heartText;
    public Text coinText;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void RegisterMessageHandler();
#endif

    public void ApplyCharacterData(APIManager.Character c)
    {
        id = c.id;
        userId = c.user_id;
        level = c.level;
        currentExp = c.exp;
        coin = c.money;
        hungryGauge = c.hungry_gauge;
        heartGauge = c.heart_gauge;

        maxActopus = c.max_actopus;
        maxFig = c.max_fig;
        maxYudal = c.max_yudal;
        maxFish = c.max_fish;

        // 경험치 기반 maxExp 재계산
        maxExp = Mathf.RoundToInt(1000 * Mathf.Pow(1.2f, level - 1));

        UpdateUI();
    }

    // 저장 API에 보낼 CharacterSaveData로 변환
    public APIManager.CharacterSaveData ToSaveData()
    {
        return new APIManager.CharacterSaveData
        {
            id = id,
            level = level,
            exp = currentExp,
            money = coin,
            hungry_gauge = hungryGauge,
            heart_gauge = heartGauge,
            max_actopus = maxActopus,
            max_fig = maxFig,
            max_yudal = maxYudal,
            max_fish = maxFish
        };
    }

    public void SaveCharacter(APIManager apiManager)
    {
        if (apiManager == null)
        {
            Debug.LogError("APIManager가 null입니다!");
            return;
        }

        // LevelManager에 저장된 데이터를 백엔드용 데이터로 변환
        APIManager.CharacterSaveData saveData = ToSaveData();

        // 백엔드에 전송
        StartCoroutine(apiManager.SaveCharacterData(saveData, (res) =>
        {
            if (res.status == "success")
            {
                Debug.Log("백엔드에 캐릭터 상태 저장 성공!");
            }
            else
            {
                Debug.LogWarning("백엔드 저장 문제: " + res.message);
            }
        }));
    }

    // Vue에서 id 가져오기
    public void SetUserIdFromWeb(string webId)
    {
        if (int.TryParse(webId, out int parsedId))
        {
            id = parsedId;
            Debug.Log("LevelManager: User ID set from web: " + id);
        }
    }

    public int GetUserId() => id;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 최초 Manager 유지
            Debug.Log("LevelManager created and will persist across scenes.");
        }
        else
        {
            Destroy(gameObject); // 새로 생긴 건 삭제
            Debug.Log("Duplicate LevelManager destroyed.");
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //private void OnDestroy()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        RegisterMessageHandler();
#endif
        // 처음 씬 로드 시 UI Text 연결
        FindUIText();
        UpdateUI();
    }

    // 씬 로드 시 UI Text 연결
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindUIText();
        UpdateUI();
    }

    // 씬에서 UI Text 찾기
    private void FindUIText()
    {
        levelText = GameObject.Find("LevelText")?.GetComponent<Text>();
        hungryText = GameObject.Find("HungryText")?.GetComponent<Text>();
        heartText = GameObject.Find("HeartText")?.GetComponent<Text>();
        coinText = GameObject.Find("CoinText")?.GetComponent<Text>();
    }

    // UI 업데이트
    private void UpdateUI()
    {
        if (levelText != null)
            levelText.text = "Lv. " + level;

        if (hungryText != null)
            hungryText.text = hungryGauge + "%";

        if (heartText != null)
            heartText.text = heartGauge + "%";

        if (coinText != null)
            coinText.text = coin+ "원";
    }

    public void BuyFood(int price, int hungryValue)
    {
        Debug.Log("구매!");
        if (coin >= price)
        {
            coin -= price;
            AddHungry(hungryValue);

            UpdateUI();
        }
        else
        {
            Debug.Log("돈이 부족합니다!");
            StoreManager.Instance.ShowErrorBox();
        }
    }

    // 배고픔 증감
    public void AddHungry(int amount)
    {
        hungryGauge += amount;
        if (hungryGauge > 100) hungryGauge = 100;

        UpdateUI();
    }

    // 체력 증감
    public void AddHeart(int amount)
    {
        heartGauge += amount;
        if (heartGauge > 100) heartGauge = 100;

        UpdateUI();
    }

    // 코인 증감
    public void AddCoin(int amount)
    {
        coin += amount;
        Debug.Log("얻은 코인:" + amount + ", 현재 코인: " + coin);
        UpdateUI();
    }

    // 코인 값 가져오기
    public int GetCoin() => coin;
    public int GetHungryLevel() => hungryGauge;
    public int GetHeartLevel() => heartGauge;

    // 배고픔/체력 체크 후 씬 전환
    public void CheckGauge(int hungryCost, int heartCost)
    {
        if (hungryGauge + hungryCost < 0 || heartGauge + heartCost < 0)
        {
            UIManager.instance.ShowTalkPanel();
            return;
        }

        AddHungry(hungryCost);
        AddHeart(heartCost);

        UIManager.instance.TrySceneChange();
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
        Debug.Log("경험치 +" + amount + " (현재: " + currentExp + "/" + maxExp + ")");

        // 레벨업 체크
        while (currentExp >= maxExp)
        {
            currentExp -= maxExp;   // 넘친 경험치는 이월
            LevelUp();
        }
    }

    // 레벨업 처리
    private void LevelUp()
    {
        level++;

        // 레벨업 할수록 maxExp 증가
        maxExp = Mathf.RoundToInt(1000 * Mathf.Pow(1.2f, level - 1));
        // 예: Lv1=100, Lv2=150, Lv3=200, Lv4=250 ...

        Debug.Log("레벨업! 현재 레벨: " + level + " (다음 레벨업 필요 EXP: " + maxExp + ")");
    }

    public void SaveHighScore(string gameName, int score)
    {
        switch (gameName)
        {
            case "Muhwagua":
                if (score > maxFig) maxFig = score;
                break;
            case "Youdal":
                if (score > maxYudal) maxYudal = score;
                break;
            case "Octopus":
                if (score > maxActopus) maxActopus = score;
                break;
            case "Sashimi":
                if (score > maxFish) maxFish = score;
                break;
        }
    }
}
