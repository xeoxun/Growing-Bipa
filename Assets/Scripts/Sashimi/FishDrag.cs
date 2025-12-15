using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishDrag : MonoBehaviour
{
    private bool isDragging = true;     // 생성 직후 드래그 상태
    private bool isOverBoard = false;   // 도마 위 여부

    private int clickCount = 0;
    public GameObject sashimiPrefab;

    private Camera mainCamera;

    // 도마 위 물고기 상태 (공용)
    public static bool isFishOnBoard = false;

    private bool placedOnBoard = false; // 이 물고기가 실제로 도마에 올라간 적이 있는지

    public void SetCamera(Camera cam)
    {
        mainCamera = cam;
    }

    void Update()
    {
        if (isDragging)
        {
            // 마우스 따라가기
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;

            // 마우스 놓기
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;

                // 도마 위에 다른 물고기가 없을 때만 놓기
                if (isOverBoard && !isFishOnBoard)
                {
                    GameObject board = GameObject.FindGameObjectWithTag("Board");
                    transform.position = new Vector3(board.transform.position.x, board.transform.position.y, transform.position.z);
                    transform.localScale = transform.localScale * 2f;

                    isFishOnBoard = true;
                    placedOnBoard = true; // 실제 도마 위에 올라감
                }
                else
                {
                    Destroy(gameObject); // 조건 불충족 시 삭제
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!placedOnBoard) return;
            if (GameManager_s.Instance != null && !GameManager_s.Instance.isHoldingKnife) return;

            if (GameManager_s.Instance.isHoldingKnife)
            {
                clickCount++;
                Debug.Log("물고기 클릭: " + clickCount);

                if (clickCount >= 4)
                {
                    SpawnSashimi();
                    clickCount = 0;
                }
            }
        }
    }

    void SpawnSashimi()
    {
        if (sashimiPrefab == null)
        {
            Debug.LogError("sashimiPrefab이 null입니다!");
            return;
        }

        // 사시미 생성
        Vector3 pos = transform.position;
        pos.z = 0f;

        Instantiate(sashimiPrefab, pos, Quaternion.identity);

        Debug.Log("Spawned sashimi at " + pos);

        // 다음 프레임에서 물고기 제거
        StartCoroutine(DestroyNextFrame());
    }

    IEnumerator DestroyNextFrame()
    {
        yield return null; // 다음 프레임까지 기다림
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Board"))
        {
            isOverBoard = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Board") && isDragging)
        {
            isOverBoard = false;
        }
    }

    private void OnDestroy()
    {
        // 진짜 도마 위에 있었던 물고기만 플래그 해제
        if (placedOnBoard)
        {
            isFishOnBoard = false;
        }
    }
}
