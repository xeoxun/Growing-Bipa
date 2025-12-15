using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;        // 텍스트 컴포넌트 연결
    public Button nextButton;        // 버튼 연결

    public SpriteRenderer background; // 배경 SpriteRenderer 연결
    public Sprite mokpo1;            // mokpo1 배경 이미지
    public Sprite mokpo2;            // mokpo2 배경 이미지

    // 배경별 대사 저장
    private string[][] dialoguesByBackground = new string[][]
    {
        new string[] { // mokpo1 대사
            "오늘 하루도 너무 알차게 보냈어.",
            "목포 밤바다가 이렇게 아름다울 줄 몰랐네.",
            "옆에 대반동에는 목포 앞바다가 훤히 보이는 카페가 엄청 많다며?!",
            "꼭 놀러가봐야겠어!",
            "(목포대교를 바라보다 잠이 드는 비파..)"
        },
        new string[] { // mokpo2 대사
            "오늘은 조금 힘들었지만 즐거웠어.",
            "지는 노을에서 목포대교도 너무 예쁘다.",
            "목포에는 드라이브 코스도 많다며? 나중에 한번 해봐야지ㅎㅎ",
            "(한참을 밖을 바라보다 잠드는 비파..)"
        }
    };

    private string[] currentDialogues;
    private int currentIndex = 0;

    void Start()
    {
        // 랜덤 배경 선택
        int randomIndex = Random.Range(0, 2);

        if (randomIndex == 0)
        {
            background.sprite = mokpo1;
            currentDialogues = dialoguesByBackground[0];
        }
        else
        {
            background.sprite = mokpo2;
            currentDialogues = dialoguesByBackground[1];
        }

        // 첫 대사 출력
        dialogueText.text = currentDialogues[currentIndex];

        // 버튼 클릭 이벤트 등록
        nextButton.onClick.AddListener(NextDialogue);
    }

    void NextDialogue()
    {
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
        dialogueText.text = "(체력이 100%로 회복되었습니다.)";

        // 레벨 매니저 회복 실행
        if (LevelManager.instance != null)
        {
            LevelManager.instance.AddHeart(100);
        }

        // 2초 대기 (텍스트 보여주기 위해)
        yield return new WaitForSeconds(2f);

        // 월드 씬으로 전환
        MenuManager.cameFromGame = true;
        SceneManager.LoadScene("World");
    }
}
