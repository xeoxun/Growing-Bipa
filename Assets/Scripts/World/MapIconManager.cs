using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapIconManager : MonoBehaviour
{
    public float floatDistance = 0.5f;  // 위아래로 움직일 거리
    public float duration = 1.5f;       // 왕복 시간

    private Vector3 originalPosition;
    private Vector3 originalScale;

    public string placeName;
    public GameObject nameTag;

    private RectTransform nameTagRect;

    public int hungryCost;
    public int heartCost;

    private bool isNearPortal = false;   // 씬 전환

    void Start()
    {
        transform.DOKill();

        originalScale = transform.localScale;
        originalPosition = transform.position;

        // Y 방향으로 floatDistance만큼 올라갔다 내려가기 (무한 반복)
        transform.DOMoveY(originalPosition.y + floatDistance, duration)
            .SetLoops(-1, LoopType.Yoyo) // 무한 반복, 위아래 왕복
            .SetEase(Ease.InOutSine);   // 부드러운 easing

        nameTagRect = nameTag.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isNearPortal && Input.GetKeyDown(KeyCode.Return))  // Enter 키
        {
            LevelManager.instance.CheckGauge(hungryCost, heartCost);            
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 캐릭터 태그: "Player"
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySlidePanel();

            // 크기를 1.2배로 키우고 그 상태로 유지
            transform.DOScale(originalScale * 1.2f, 0.3f)
                .SetEase(Ease.OutSine);

            // PlaceTag 활성화
            UIManager.instance.SetPlace(placeName);

            nameTagRect.DOAnchorPos(new Vector2(-150f, 2.5f), 0.5f)
               .SetEase(Ease.OutExpo);

            isNearPortal = true;

            Debug.Log($"플레이어가 {placeName} 아이콘과 충돌함!");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            transform.DOScale(originalScale, 0.3f)
                .SetEase(Ease.OutSine);

            // PlaceTag 숨김
            nameTagRect.DOAnchorPos(new Vector2(300f, -2.5f), 0.5f)
                .SetEase(Ease.InExpo);

            isNearPortal = false;
        }
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}
