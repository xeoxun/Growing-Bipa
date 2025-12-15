using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleControl : MonoBehaviour
{
    public int scoreValue;   // 프리팹마다 Inspector에서 설정

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Basket"))  // 바구니에 부딪힘
        {
            MhgSFXManager.Instance.PlayBasketSFX();

            GameManager_m.Instance.AddScore(scoreValue);
            Destroy(gameObject);   // 점수 더하고 사라짐
        }
    }
}
