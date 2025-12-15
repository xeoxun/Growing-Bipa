using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FestivalManager : MonoBehaviour
{
    public Text dialogueText;        // 텍스트 컴포넌트 연결
    public Button nextButton2;        // 버튼 연결

    public SpriteRenderer background; // 배경 SpriteRenderer 연결
    public Sprite festival1;            // mokpo1 배경 이미지
    public Sprite festival2;            // mokpo2 배경 이미지

    // 배경별 대사 저장
    private string[][] dialoguesByBackground = new string[][]
    {
        new string[] { // festival1 대사
            "평화광장에서 해상W쇼를 하고 있잖아?",
            "음악분수가 굉장히 예쁘넹!",
            "(분수쇼를 보며 힐링하는 비파..)"
        },
        new string[] { // mokpo2 대사
            "저녁에 대반동에서는 해상케이블카를 볼 수 있네?",
            "나도 한번 타보고 싶다!",
            "(대반동 경치를 보며 힐링하는 비파..)"
        }
    };

    private string[] currentDialogues;
    private int currentIndex = 0;

    void Start()
    {
        Debug.Log("시작!");

        // 랜덤 배경 선택
        int randomIndex = Random.Range(0, 2);

        if (randomIndex == 0)
        {
            background.sprite = festival1;
            currentDialogues = dialoguesByBackground[0];
        }
        else
        {
            background.sprite = festival2;
            currentDialogues = dialoguesByBackground[1];
        }

        // 첫 대사 출력
        dialogueText.text = currentDialogues[currentIndex];

        // 버튼 클릭 이벤트 등록
        nextButton2.onClick.AddListener(NextDialogue);
    }

    void NextDialogue()
    {
        Debug.Log("버튼 눌림");
        currentIndex++;

        if (currentIndex < currentDialogues.Length)
        {
            dialogueText.text = currentDialogues[currentIndex];
        }
        else
        {
            StartCoroutine(EndDialogueRoutine());
        }
    }

    private IEnumerator EndDialogueRoutine()
    {
        dialogueText.text = "(체력이 5만큼, 배고픔이 -5만큼 증감하였습니다.)";

        // 레벨 매니저 회복 실행
        if (LevelManager.instance != null)
        {
            LevelManager.instance.AddHeart(5);
            LevelManager.instance.AddHungry(-5);

        }

        // 2초 대기 (텍스트 보여주기 위해)
        yield return new WaitForSeconds(2f);

        // 월드 씬으로 전환
        MenuManager.cameFromGame = true;
        SceneManager.LoadScene("World");
    }
}
