using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus : MonoBehaviour
{
    private float riseHeight = 1.9f;
    public float riseDuration = 0.5f;
    public float stayDuration = 1f;
    public float fallDuration = 0.5f;

    public int scoreValue;
    private bool isHit = false;
    private bool isFalling = false;

    public void AppearAt(Vector2 basePos)
    {
        transform.position = basePos;

        Vector2 targetPos = basePos + Vector2.up * riseHeight;
        StartCoroutine(RiseAndFall(targetPos));
    }

    IEnumerator RiseAndFall(Vector2 targetPos)
    {
        float time = 0f;
        Vector2 startPos = transform.position;

        // 올라오기
        while (time < riseDuration)
        {
            transform.position = Vector2.Lerp(startPos, targetPos, time / riseDuration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;

        // 머무는 시간
        yield return new WaitForSeconds(stayDuration);

        // 내려가기
        isFalling = true;
        time = 0f;
        while (time < fallDuration)
        {
            transform.position = Vector2.Lerp(targetPos, startPos, time / fallDuration);
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    void OnMouseDown()
    {
        // 이미 처리된 경우나 파괴된 경우 무시
        if (isHit || isFalling || !this || !gameObject || !enabled) return;

        // 파괴가 예약되어 있는지 다시 한 번 확인
        if (!gameObject.activeInHierarchy) return;

        isHit = true;
        NakjiSFXManager.Instance.PlayHammerSFX();

        // 점수 추가
        if (GameManager.instance != null)
        {
            GameManager.instance.AddScore(scoreValue);
        }

        // 파괴
        Destroy(gameObject);
    }
}
