using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0, 10);
    public float swingAngle = -50f; // 반시계방향 50도
    public float swingDuration = 0.1f; // 돌아오는 시간

    private bool isSwinging = false;

    void Update()
    {
        // 1. 마우스를 따라다님
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = offset.z;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = worldPosition;

        // 2. 마우스 클릭 시 회전
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            StartCoroutine(SwingHammer());
        }
    }

    System.Collections.IEnumerator SwingHammer()
    {
        isSwinging = true;
        // 1. 회전 적용
        transform.rotation = Quaternion.Euler(0, 0, swingAngle);

        // 2. 잠깐 대기
        yield return new WaitForSeconds(swingDuration);

        // 3. 원래대로 복귀
        transform.rotation = Quaternion.Euler(0, 0, 0);

        isSwinging = false;
    }
}
