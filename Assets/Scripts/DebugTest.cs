using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameManager_m in scene: " + (GameManager_m.Instance != null));
        Debug.Log("GameManager_m object in hierarchy: " + GameObject.FindObjectOfType<GameManager_m>());
    }
}
