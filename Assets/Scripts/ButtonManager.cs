using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject popupPanel;

    public Button stopButton;
    public Button yesButton;
    public Button noButton;

    void Start()
    {
        popupPanel.SetActive(false); // 시작할 때는 숨김
    }

    public void ToWorld()
    {
        Debug.Log("게임 종료");

        MenuManager.cameFromGame = true;
        SceneManager.LoadScene("World");
        
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowPopup()
    {
        Debug.Log("멈춤 버튼 눌림!");
        Time.timeScale = 0f;  // 게임 오브젝트 정지

        popupPanel.SetActive(true);
    }
}
