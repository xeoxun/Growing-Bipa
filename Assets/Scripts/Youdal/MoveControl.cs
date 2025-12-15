using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControl : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;
    private float speed = 2f;
    private float t = 0f;

    private bool isMoving = false;

    // 크기 관련
    private float startScale = 0.1f;   // 시작 크기
    private float endScale = 1.5f;     // 최종 크기

    // 레인 좌표
    private static readonly Vector2[] startPositions = new Vector2[]
    {
        new Vector2(-0.3f, 3.4f),  // 1레인 시작
        new Vector2(0.1f, 3.4f),   // 2레인 시작
        new Vector2(0.4f, 3.4f)    // 3레인 시작
    };

    private static readonly Vector2[] endPositions = new Vector2[]
    {
        new Vector2(-4.2f, -5f),   // 1레인 끝
        new Vector2(0.1f, -5f),    // 2레인 끝
        new Vector2(4.4f, -5f)     // 3레인 끝
    };

    // 초기화
    public void Init(int lane, float moveSpeed)
    {
        startPos = startPositions[lane];
        endPos = endPositions[lane];
        speed = moveSpeed;

        transform.position = startPos;
        transform.localScale = Vector3.one * startScale; // 시작 크기 적용
        t = 0f;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;

        // 진행 비율
        t += Time.deltaTime * speed / Vector2.Distance(startPos, endPos);

        // 위치 이동
        transform.position = Vector2.Lerp(startPos, endPos, t);

        // 크기 변화
        float scale = Mathf.Lerp(startScale, endScale, t);
        transform.localScale = Vector3.one * scale;

        // 도착하면 제거
        if (t >= 1f)
        {
            Destroy(gameObject);
        }
    }

    // 제어용 함수
    public void StopMove() => isMoving = false;
    public void ResumeMove() => isMoving = true;
    public void SetSpeed(float newSpeed) => speed = newSpeed;
}
