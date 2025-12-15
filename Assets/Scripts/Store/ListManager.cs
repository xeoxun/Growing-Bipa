using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListManager : MonoBehaviour
{
    public static ListManager Instance;

    public APIManager apiManager;

    public GameObject buttonPrefab;     // 생성할 Button 프리팹
    public Transform panelContainer;    // Panel의 Content 부분 (LayoutGroup 달린 곳)

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // 예: "food" 카테고리로 스토어 데이터 가져오기
        LoadShopData("cafe");
    }

    public void ClickCafe()
    {
        LoadShopData("cafe");
    }

    public void ClickRestaurant()
    {
        LoadShopData("restaurant");
    }

    // API 호출
    public void LoadShopData(string division)
    {
        StartCoroutine(apiManager.GetShopData(division, OnShopDataReceived));
    }

    // API 응답 콜백
    private void OnShopDataReceived(APIManager.ShopResponse response)
    {
        if (response == null || response.place == null || response.place.Length == 0)
        {
            Debug.LogWarning("API에서 받은 가게 데이터가 없습니다.");
            return;
        }

        // 기존 버튼들 삭제
        foreach (Transform child in panelContainer)
        {
            Destroy(child.gameObject);
        }

        // 버튼 생성
        foreach (var store in response.place)
        {
            GameObject newButtonObj = Instantiate(buttonPrefab, panelContainer);

            Text[] texts = newButtonObj.GetComponentsInChildren<Text>();
            foreach (Text t in texts)
            {
                if (t.name == "StoreNameText") t.text = store.place_name;
                if (t.name == "AddressText") t.text = store.address;
                if (t.name == "MenuText") t.text = "대표메뉴: " + store.first_menu;
                if (t.name == "HungryCostText") t.text = "배고픔: +" + store.hungry_gauge;
                if (t.name == "PriceText") t.text = "가격: " + store.first_price;
            }

            Button btn = newButtonObj.GetComponent<Button>();
            int price = store.first_price;
            int hungryValue = store.hungry_gauge;
            string placeName = store.place_name;

            btn.onClick.AddListener(() =>
            {
                Debug.Log("가게 클릭: " + placeName);

                // 서버로 place_name 전송 (UI 반응 필요 없음)
                if (apiManager != null)
                {
                    //StartCoroutine(apiManager.SelectPlace(placeName));

                    StartCoroutine(apiManager.SelectPlace(placeName, (res) => {
                        // 서버에서 반환한 place 객체 출력
                        Debug.Log("=== 서버 응답 확인 ===");
                        Debug.Log("place_name: " + res.place.place_name);
                        Debug.Log("category: " + res.place.category);
                        Debug.Log("address: " + res.place.address);
                        Debug.Log("business_hours: " + res.place.business_hours);

                        if (res.place.menu != null)
                        {
                            Debug.Log("menu:");
                            foreach (var item in res.place.menu)
                            {
                                Debug.Log(" - " + item);
                            }
                        }
                    }));
                }

                // 레벨 매니저에서 코인/배고픔 처리
                if (LevelManager.instance != null)
                {
                    LevelManager.instance.BuyFood(price, hungryValue);
                }
            });
        }
    }
}
