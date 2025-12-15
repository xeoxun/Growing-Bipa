using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawn : MonoBehaviour
{
    public GameObject fishPrefab; // 복제할 프리팹 (원본과 같은 모습)
    public GameObject sashimiPrefab;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        if (SashimiSoundManager.Instance != null)
        {
            SashimiSoundManager.Instance.PlayFishSFX();
        }

        GameObject newFish = Instantiate(fishPrefab);

        // 복제본에 붙은 FishSpawn 스크립트가 있으면 제거 (복제본이 다시 복제 못 하도록)
        FishSpawn spawnScript = newFish.GetComponent<FishSpawn>();
        if (spawnScript != null)
        {
            Destroy(spawnScript);
        }

        FishDrag dragScript = newFish.AddComponent<FishDrag>();
        dragScript.SetCamera(mainCamera);

        dragScript.sashimiPrefab = sashimiPrefab;
        Debug.Log(dragScript.sashimiPrefab);

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newFish.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);
    }
}
