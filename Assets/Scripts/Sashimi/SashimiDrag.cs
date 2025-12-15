using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SashimiDrag : MonoBehaviour
{
    public GameObject dish; // Dish 오브젝트(씬에서 자동 탐색)

    private bool isClicked = false;
    public bool isDish = false;

    public string sashimiName;

    void Start()
    {
        if (dish == null)
            dish = GameObject.Find("Dish");
    }

    void OnMouseDown()
    {
        if (isClicked || isDish) return;

        isClicked = true;

        // Dish BoxCollider2D 가져오기
        BoxCollider2D dishCollider = dish.GetComponent<BoxCollider2D>();
        if (dishCollider == null)
        {
            Debug.LogError("Dish에 BoxCollider2D가 없습니다!");
            return;
        }

        float centerX = dishCollider.bounds.center.x;
        float maxX = dishCollider.bounds.max.x;

        // Dish 오른쪽 끝 위치 계산
        float targetX = Mathf.Lerp(centerX, maxX, 0.3f); // 오른쪽 끝
        float targetY = dishCollider.bounds.center.y; // y는 중앙
        Vector3 targetPos = new Vector3(targetX, targetY, transform.position.z);

        // Dish 크기에 맞는 scale 계산
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("Sashimi에 SpriteRenderer가 필요합니다!");
            return;
        }

        float sashimiWidth = sr.bounds.size.x;
        float sashimiHeight = sr.bounds.size.y;

        float targetWidth = dishCollider.bounds.size.x;
        float targetHeight = dishCollider.bounds.size.y;

        // 오른쪽 끝에 들어갈 수 있도록 scale 조정 (너비 기준)
        float scaleFactor = Mathf.Min(targetWidth / sashimiWidth, targetHeight / sashimiHeight);
        Vector3 targetScale = transform.localScale * scaleFactor;

        // DOTween으로 이동 + 크기 조정
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(targetPos, 0.5f).SetEase(Ease.InOutSine));
        seq.Join(transform.DOScale(targetScale, 0.5f).SetEase(Ease.InOutSine));
        seq.OnComplete(() =>
        {
            // 이동 완료 후 Dish의 자식으로 설정
            transform.parent = dish.transform;
            isDish = true;
        });

        isClicked = false;
    }
}