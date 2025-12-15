using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FishDrag fish = collision.GetComponent<FishDrag>();

        if (fish != null && Input.GetMouseButtonUp(0))
        {
            // 마우스를 도마 위에서 뗐을 때 해당 위치에 고정
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = collision.transform.position.z;

            collision.transform.position = mousePos;
            Debug.Log($"{collision.name} 도마 위에 놓임");
        }
    }
}
