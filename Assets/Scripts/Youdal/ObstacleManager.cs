using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    private float spawnInterval = 2.5f;     // 기본 생성 주기
    private float timer;
    private float difficultyTimer;

    private float obstacleSpeed = 3f;       // 기본 장애물 속도

    void Update()
    {
        // 스폰 타이머
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObstacles();
            timer = 0f;
        }

        // 난이도 상승 타이머
        difficultyTimer += Time.deltaTime;
        if (difficultyTimer >= 10f) // 10초마다 난이도 상승
        {
            IncreaseDifficulty();
            difficultyTimer = 0f;
        }
    }

    void SpawnObstacles()
    {
        int laneCount = Random.Range(1, 3);

        List<int> availableLanes = new List<int>() { 0, 1, 2 };
        List<int> chosenLanes = new List<int>();

        for (int i = 0; i < laneCount; i++)
        {
            int index = Random.Range(0, availableLanes.Count);
            chosenLanes.Add(availableLanes[index]);
            availableLanes.RemoveAt(index);
        }

        foreach (int lane in chosenLanes)
        {
            int prefabIndex = Random.Range(0, obstaclePrefabs.Length);
            GameObject obstacle = Instantiate(obstaclePrefabs[prefabIndex], Vector3.zero, Quaternion.identity);

            MoveControl move = obstacle.AddComponent<MoveControl>();
            move.Init(lane, obstacleSpeed); // 속도 반영
        }
    }

    void IncreaseDifficulty()
    {
        // 스폰 주기를 줄이되 최소 0.8초까지
        spawnInterval = Mathf.Max(0.8f, spawnInterval - 0.3f);

        // 장애물 속도를 올림
        obstacleSpeed += 0.4f;

        GameManager_y.Instance.AddScore(10);

        Debug.Log($"난이도 상승! spawnInterval={spawnInterval}, obstacleSpeed={obstacleSpeed}");
    }
}
