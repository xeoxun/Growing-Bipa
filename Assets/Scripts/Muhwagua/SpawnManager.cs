using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] prefabs;       // 5개의 오브젝트 프리팹

    public float minX, maxX;

    public float spawnY;          // 화면 상단
    public float bottomY = -6f;

    public float spawnIntervalMin;
    public float spawnIntervalMax;

    public float minGravityScale; // 최소 중력 배율
    public float maxGravityScale; // 최대 중력 배율

    public int maxSimultaneousObjects = 10;

    private void Awake()
    {
        Application.targetFrameRate = 75;
    }

    void Start()
    {
        for (int i = 0; i < maxSimultaneousObjects; i++)
        {
            StartCoroutine(SpawnObjectRoutine());
        }
    }

    IEnumerator SpawnObjectRoutine()
    {
        // 코루틴 시작 시 랜덤 초기 지연
        float initialDelay = Random.Range(0f, spawnIntervalMax);
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            // 랜덤 프리팹 선택
            int index = Random.Range(0, prefabs.Length);
            GameObject obj = Instantiate(prefabs[index]);

            // 랜덤 X 위치
            float randomX = Random.Range(minX, maxX);
            obj.transform.position = new Vector3(randomX, spawnY, 0);

            // Rigidbody2D에 랜덤 GravityScale 적용
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = Random.Range(minGravityScale, maxGravityScale);
            }

            StartCoroutine(DestroyWhenOutOfBounds(obj));

            // 다음 스폰까지 랜덤 시간 대기
            float waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator DestroyWhenOutOfBounds(GameObject obj)
    {
        while (obj != null)
        {
            if (obj.transform.position.y < bottomY)
            {
                Destroy(obj);
                yield break; // 끝내기
            }
            yield return null;
        }
    }
}
