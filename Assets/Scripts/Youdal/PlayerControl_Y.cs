using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl_Y : MonoBehaviour
{
    private float[] lanes = { -3.8f, 0f, 4f }; // x축 위치만 변함
    private int currentLane = 1; // 시작 위치: lanes[1] = 0

    private Rigidbody2D rb;
    private Vector2 targetPosition;

    private bool isMoving = false;
    public float moveSpeed;

    void Awake()
    {
        Application.targetFrameRate = 75;
        rb = GetComponent<Rigidbody2D>();
        // y 고정
        targetPosition = new Vector2(lanes[currentLane], -3.2f);
        rb.position = targetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentLane < lanes.Length - 1)
            {
                currentLane++;
                MoveToLane();
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentLane > 0)
            {
                currentLane--;
                MoveToLane();
            }
        }
    }

    void MoveToLane()
    {
        targetPosition = new Vector2(lanes[currentLane], -3.2f);
        rb.position = targetPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Star"))
        {
            // 별 충돌 → 점수 획득
            YoudalSFXManager.Instance.PlayStarSFX();
            GameManager_y.Instance.AddScore(100);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Stone") || collision.CompareTag("Branch"))
        {
            // 돌이나 나뭇가지 충돌 → 게임오버 처리
            GameManager_y.Instance.GameOver();
        }
    }
}
