using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishClick : MonoBehaviour
{
    private Vector3 originalPos;

    void Start()
    {
        // Dish 초기 위치 저장
        originalPos = transform.position;
    }

    void OnMouseDown()
    {
        if (SashimiSoundManager.Instance != null)
        {
            SashimiSoundManager.Instance.PlayDishSFX();
        }

        // Dish 자식(사시미)들을 따로 저장
        Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }

        // 화면 오른쪽 밖 위치 (예: x축으로 8f 이동)
        Vector3 offscreenPos = transform.position + new Vector3(8f, 0, 0);

        Sequence seq = DOTween.Sequence();

        // Dish 이동
        seq.Append(transform.DOMove(offscreenPos, 0.5f).SetEase(Ease.InBack));

        // 이동 완료 후 사시미 삭제
        seq.AppendCallback(() =>
        {
            foreach (var child in children)
            {
                    Debug.Log("Child: " + child.name);

                    // 사시미 이름 가져오기
                    SashimiDrag sashimi = child.GetComponent<SashimiDrag>();
                if (sashimi != null)
                {
                    // 주문 처리
                    OrderManager orderManager = FindObjectOfType<OrderManager>();
                    if (orderManager != null)
                    {
                        orderManager.CompleteSashimi(sashimi.sashimiName);
                        Debug.Log(sashimi.sashimiName + " 카운드 감소!");
                    }
                }

                // 사시미 제거
                Destroy(child.gameObject);
            }
        });

        // Dish 원래 위치로 복귀
        seq.Append(transform.DOMove(originalPos, 0.5f).SetEase(Ease.OutBack));
    }
}
