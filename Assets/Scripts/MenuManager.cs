using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static APIManager;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public APIManager apiManager;

    public GameObject mainGroup; // Main Group 오브젝트
    public GameObject exitGroup; // Exit Group 오브젝트
    public GameObject tagPanel;
    public GameObject bipaFace;

    public GameObject worldCamera;
    public GameObject menuCamera;

    public static bool cameFromGame = false;

    public int userId;

    void Start()
    {
        if (cameFromGame)
            ToWorld();
        else ToMenu();
    }

    public void Awake()
    {
        Instance = this;
    }
    public void StartGame()
    {
        ToWorld();

        //int testUserId = 1;
        int userId = 4; //LevelManager.instance.GetUserId();

        Debug.Log("MenuManager.StartGame(): Using UserId = " + userId);

        StartCoroutine(apiManager.GetCharacterData(userId, (character) =>
        {
            LevelManager.instance.ApplyCharacterData(character);
        }));
    }

    public void ToWorld()
    {
        mainGroup.SetActive(false);
        exitGroup.SetActive(false);
        tagPanel.SetActive(true);
        bipaFace.SetActive(false);

        worldCamera.SetActive(true);
        menuCamera.SetActive(false);
    }

    public void ToMenu()
    {
        mainGroup.SetActive(true);
        exitGroup.SetActive(false);
        tagPanel.SetActive(false);
        bipaFace.SetActive(true);

        menuCamera.SetActive(true);
        worldCamera.SetActive(false);
    }

    public void StopGame()
    {
        mainGroup.SetActive(false);
        exitGroup.SetActive(true);
        tagPanel.SetActive(false);
        bipaFace.SetActive(true);

        menuCamera.SetActive(true);
        worldCamera.SetActive(false);
    }

    public void SaveGame()
    {
        if (LevelManager.instance != null && apiManager != null)
        {
            LevelManager.instance.SaveCharacter(apiManager);
        }
        else
        {
            Debug.LogWarning("LevelManager 또는 APIManager가 존재하지 않습니다!");
        }
    }

    public void CloseGame()
    {
        //ToMenu();
        Application.Quit();
    }
}
