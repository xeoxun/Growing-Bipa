using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 클래스 이름을 파일명과 동일하게 NpcControl로 변경
public class NpcControl : MonoBehaviour
{
    public GameObject npcPanel;     // NPC 패널
    public Text dialogueText;       // 대화 표시 Text
    public Button nextButton;       // "다음" / "시작" 버튼
    public string npcName;          // 인스펙터에서 설정할 NPC 이름

    private Dictionary<string, string[]> npcDialogues = new Dictionary<string, string[]>();
    private string[] dialogues;     // 현재 선택된 NPC 대사
    private int dialogueIndex = 0;

    // GameManager 참조를 위한 변수
    // [SerializeField]를 사용하면 private이어도 인스펙터 창에서 직접 연결 가능
    public GameManager gameManager;
    public GameManager_s gameManager_s;
    public GameManager_y gameManager_y;
    public GameManager_m gameManager_m;

    void Awake()
    {
        // 1. GameManager를 찾는 가장 확실한 방법: 직접 연결 또는 싱글톤
        // 인스펙터에서 gameManager가 연결되지 않았다면, 싱글톤 패턴으로 찾기
        if (gameManager == null)
        {
            gameManager = GameManager.instance; // GameManager가 싱글톤일 경우
        }
    }

    void Start()
    {
        // 게임 시작 시에는 항상 시간이 멈추도록 설정
        Time.timeScale = 0f;

        InitializeDialogues(); // 대사 초기화 로직을 별도 함수로 분리

        // npcName에 맞는 대사 불러오기
        if (npcDialogues.ContainsKey(npcName))
            dialogues = npcDialogues[npcName];
        else
            dialogues = new string[] { "..." }; // 이름이 없으면 기본 대사

        // UI 설정
        if (npcPanel != null)
            npcPanel.SetActive(true);

        // 버튼 리스너 추가 (중복 추가 방지를 위해 기존 리스너 제거 후 추가)
        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(OnNextButtonClicked);
        }

        ShowDialogue();
    }

    // 대사 데이터를 초기화하는 함수
    void InitializeDialogues()
    {
        npcDialogues["무아가"] = new string[]
        {
            "어서오라구! 나는 무화과 농장을 운영하고 있는 무아가라고 해.",
            "무화과 따기 체험을 하러 온 거 같은데, 그럼 이 게임에 대해 설명을 해줄게.",
            "보이는 것과 같이 하늘에서 떨어지는 무화과를 바구니로 받아내는 게임이야.",
            "하지만 무화과 종류에 따라 점수가 달라지니까 신중하게 받아야 해!",
            "준비가 되었다면, 시작 버튼을 눌러!"
        };

        npcDialogues["쪼낙"] = new string[]
        {
            "나는 여기 북항의 선장 쪼물락낙지, 이름하여 쪼낙이라고 혀~ 아따~ 여기까지 오느라 욕봤네~!",
            "목포에 와서 직접 잡은 세발낙지를 먹을라믄 쪼까 노동이 필요한디...",
            "여기 있는 뿅망치로 올라오는 낙지를 잡으면 되야. 간단혀!",
            "근디 독먹은 낙지를 잡아불면 점수가 깎이니께 조심해서 잡아야혀!",
            "준비가 되믄, 시작 버튼을 눌러브러~"
        };
        // ... (나머지 대사들도 여기에 포함)
        npcDialogues["회장님"] = new string[]
        {
          "워메!! 회 먹으러 멀리서 여기까지 온겨? 아따 반갑소잉~",
          "나한테는 회장님이라 부르면 댜. 회를 너무 사랑해서 사람들이 그래 불러~",
          "근디 회를 더 맛나게 먹을라믄 일을 해야 맛난텐데.. ",
          "여기 주문이 들어오면 생선을 꺼내서 칼로 회떠서, 접시에 옮겨서 접시를 클릭하면 주문을 뺄 수 있어야.",
          "손 조심하고 준비되믄 시작 버튼을 눌러~"
        };
        npcDialogues["유달산 방범대장"] = new string[]
        {
          "유달산 등반 할라고 여기까지 온 겨?",
          "여기 유달산 일등바위까지 올라갈라믄 길이 쪼매 험해서 조심히 올라가야뎌.",
          "나뭇가지나 돌을 발견하면 무조건 피하고, 별을 먹으면 점수를 받을 수 있어야!",
          "몸 다 풀었으면 시작 버튼을 눌러~"
        };
    }

    void ShowDialogue()
    {
        if (dialogueText == null || dialogues == null || dialogues.Length == 0) return;

        dialogueText.text = dialogues[dialogueIndex];

        // 버튼 텍스트 변경
        Text buttonText = nextButton.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            if (dialogueIndex == dialogues.Length - 1)
            {
                buttonText.text = "시작";
            }
            else
            {
                buttonText.text = "다음";
            }
        }
    }

    void OnNextButtonClicked()
    {
        // 다음 대사가 남아있다면
        if (dialogueIndex < dialogues.Length - 1)
        {
            dialogueIndex++;
            ShowDialogue();
        }
        // 마지막 대사라면 (시작 버튼을 눌렀다면)
        else
        {
            StartGameSequence(npcName);
        }
    }

    private void StartGameSequence(string npcName)
    {
        if (npcPanel != null)
            npcPanel.SetActive(false);

        // 게임 시간을 다시 흐르게 함
        Time.timeScale = 1f;


        switch (npcName)
        {
            case "무아가":
                if (GameManager_m.Instance != null)
                    GameManager_m.Instance.StartGame();
                else
                    Debug.LogError("무아가용 GameManager를 찾을 수 없습니다.");
                break;

            case "쪼낙":
                if (GameManager.instance != null)
                    GameManager.instance.StartGame();
                else
                    Debug.LogError("쪼낙용 GameManager를 찾을 수 없습니다.");
                break;

            case "회장님":
                if (GameManager_s.Instance != null)
                    GameManager_s.Instance.StartGame();
                else
                    Debug.LogError("회장님용 GameManager_s를 찾을 수 없습니다.");
                break;

            case "유달산 방범대장":
                if (GameManager_y.Instance != null)
                    GameManager_y.Instance.StartGame();
                else
                    Debug.LogError("유달산 방범대장용 GameManager를 찾을 수 없습니다.");
                break;

            default:
                Debug.LogWarning($"'{npcName}'에 해당하는 GameManager가 없습니다!");
                break;
        }
    }
}