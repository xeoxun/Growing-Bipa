using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDrag : MonoBehaviour
{
    public Transform clothTransform;   // 행주 위치 (에디터에서 할당)
    public float clothRadius = 1.0f;   // 행주 반경(영역 크기)

    public bool isHoldingKnife = false;  // 칼 들고 있는지
    private bool isAnimating = false;

    private Camera mainCam;

    public float swingAngle = 40f;
    public float swingDuration = 0.3f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        mainCam = Camera.main;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (isAnimating) return;  // 애니메이션 중에는 입력 무시

        if (isHoldingKnife)
        {
            Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;

            if (!isHoldingKnife)
            {
                if (IsMouseOverKnife())
                {
                    isHoldingKnife = true;
                    GameManager_s.Instance.isHoldingKnife = true;

                    Debug.Log("칼 들었음");
                }
            }
            else
            {
                if (IsPointInClothArea(mouseWorldPos))
                {
                    isHoldingKnife = false;
                    GameManager_s.Instance.isHoldingKnife = false;

                    transform.position = originalPosition;
                    transform.rotation = originalRotation;

                    Debug.Log("행주 위에 칼 내려놓음");
                }
                else
                {
                    // 코루틴 실행
                    StartCoroutine(SwingKnife());
                }
            }
        }
    }

    IEnumerator SwingKnife()
    {
        isAnimating = true;

        if (SashimiSoundManager.Instance != null)
        {
            SashimiSoundManager.Instance.PlayKnifeSFX();
        }
        
        transform.rotation = Quaternion.Euler(0, 0, swingAngle);

        yield return new WaitForSeconds(swingDuration);

        transform.rotation = originalRotation;

        isAnimating = false;

        Debug.Log("갈로 썰기");
    }

    bool IsMouseOverKnife()
    {
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Collider2D col = GetComponent<Collider2D>();
        if (col == null) return false;

        return col.OverlapPoint(mousePos);
    }

    bool IsPointInClothArea(Vector3 point)
    {
        return Vector3.Distance(clothTransform.position, point) <= clothRadius;
    }
}
