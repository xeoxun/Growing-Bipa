using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using Newtonsoft.Json;

public class APIManager : MonoBehaviour
{
    private string baseUrl = "http://localhost:8080"; // SpringBoot 기본 포트
    public static APIManager Instance;
    /* ===========================================================
     * 1. /user/characters  (캐릭터 기본 정보 전달)
     * =========================================================== */

    [System.Serializable]
    public class UserIdWrapper
    {
        public UserId user;
    }

    [System.Serializable]
    public class UserId
    {
        public int id;
    }

    [System.Serializable]
    public class Character
    {
        public int id;
        public int user_id;     
        public int level;
        public int exp;
        public int money;
        public int hungry_gauge;  
        public int heart_gauge;   
        public int max_actopus;
        public int max_fig;
        public int max_yudal;
        public int max_fish;
    }

    [System.Serializable]
    public class CharactersResponse
    {
        public Character characters;
    }

    public IEnumerator GetCharacterData(int userId, System.Action<Character> callback)
    {
        UserIdWrapper req = new UserIdWrapper
        {
            user = new UserId { id = userId }
        };

        string jsonData = JsonConvert.SerializeObject(req);

        using (UnityWebRequest request = new UnityWebRequest(baseUrl + "/user/characters", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                CharactersResponse res = JsonConvert.DeserializeObject<CharactersResponse>(request.downloadHandler.text);
                Debug.Log("캐릭터 정보 로딩 완료");

                Character c = res.characters;
                Debug.Log($"id: {c.id}");
                Debug.Log($"userId: {c.user_id}");
                Debug.Log($"level: {c.level}");
                Debug.Log($"exp: {c.exp}");
                Debug.Log($"money: {c.money}");
                Debug.Log($"hungry_gauge: {c.hungry_gauge}");
                Debug.Log($"heartGauge: {c.heart_gauge}");
                Debug.Log($"maxActopus: {c.max_actopus}");
                Debug.Log($"maxFig: {c.max_fig}");
                Debug.Log($"maxYudal: {c.max_yudal}");
                Debug.Log($"maxFish: {c.max_fish}");
                Debug.Log("------------------------------");

                callback?.Invoke(res.characters); // 콜백 호출
            }
            else
            {
                Debug.LogError("캐릭터 요청 실패: " + request.error);
            }
        }
    }

    /* ===========================================================
     * 2. /game/shop
     * =========================================================== */

    [System.Serializable]
    public class ShopRequest
    {
        public ShopPlace place;
    }

    [System.Serializable]
    public class ShopPlace
    {
        public string division;
    }

    [System.Serializable]
    public class ShopItem
    {
        public string place_name;
        public string address;
        public int hungry_gauge;
        public int heart_gauge;
        public int first_price;
        public string first_menu;
    }

    [System.Serializable]
    public class ShopResponse
    {
        public ShopItem[] place;
    }

    public IEnumerator GetShopData(string division, System.Action<ShopResponse> callback)
    {
        ShopRequest req = new ShopRequest
        {
            place = new ShopPlace { division = division }
        };

        string jsonData = JsonConvert.SerializeObject(req);

        using (UnityWebRequest request = new UnityWebRequest(baseUrl + "/game/shop", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ShopResponse res = JsonConvert.DeserializeObject<ShopResponse>(request.downloadHandler.text);

                if (res.place != null && res.place.Length > 0)
                {
                    Debug.Log("shop 요청 성공, 첫 메뉴: " + res.place[0].first_menu);
                }
                else
                {
                    Debug.LogWarning("shop 요청 성공, 하지만 받은 가게 데이터가 없습니다.");
                }

                // 콜백 호출
                callback?.Invoke(res);
            }
            else
            {
                Debug.LogError("shop 요청 실패: " + request.error);
                callback?.Invoke(null); // 실패 시 null 전달
            }
        }
    }


    /* ===========================================================
     * 3. /game/save  (캐릭터 저장)
     * =========================================================== */

    [System.Serializable]
    public class CharacterSaveData
    {
        public int id;
        public int level;
        public int exp;
        public int money;
        public int hungry_gauge;
        public int heart_gauge;
        public int max_actopus;
        public int max_fig;
        public int max_yudal;
        public int max_fish;
    }

    [System.Serializable]
    public class SaveWrapper
    {
        public CharacterSaveData characters;
    }

    [System.Serializable]
    public class SaveResponse
    {
        public string status;
        public string message;
    }

    public IEnumerator SaveCharacterData(CharacterSaveData saveData, System.Action<SaveResponse> callback)
    {
        SaveWrapper wrapper = new SaveWrapper { characters = saveData };
        string jsonData = JsonConvert.SerializeObject(wrapper);

        using (UnityWebRequest request = new UnityWebRequest(baseUrl + "/game/save", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                SaveResponse res = JsonConvert.DeserializeObject<SaveResponse>(request.downloadHandler.text);
                Debug.Log("저장 결과: " + res.message);
            }
            else
            {
                Debug.LogError("저장 실패: " + request.error);
            }
        }
    }

    /* ===========================================================
     * 4. /place/select_place
     * =========================================================== */

    [System.Serializable]
    public class SelectPlaceRequest
    {
        public SelectPlaceData place;
    }

    [System.Serializable]
    public class SelectPlaceData
    {
        public string place_name;
    }

    [System.Serializable]
    public class SelectedPlaceResponse
    {
        public PlaceDetail place;
    }

    [System.Serializable]
    public class PlaceDetail
    {
        public string place_name;
        public string category;
        public string address;
        public string business_hours;
        public string[] menu;
    }

    public IEnumerator SelectPlace(string placeName, System.Action<SelectedPlaceResponse> callback = null)
    {
        SelectPlaceRequest req = new SelectPlaceRequest
        {
            place = new SelectPlaceData { place_name = placeName }
        };

        string jsonData = JsonConvert.SerializeObject(req);

        using (UnityWebRequest request = new UnityWebRequest(baseUrl + "/place/select_place", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("place_name 전송 완료: " + placeName);
                if (callback != null)
                {
                    SelectedPlaceResponse res = JsonConvert.DeserializeObject<SelectedPlaceResponse>(request.downloadHandler.text);
                    callback(res);
                }
            }
            else
            {
                Debug.LogError("선택 실패: " + request.error);
            }
        }
    }
}
