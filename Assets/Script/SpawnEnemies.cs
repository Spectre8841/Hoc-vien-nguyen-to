using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemyPrefab;  // Prefab của quái
    public int numberOfEnemies = 8; // Số lượng quái cần spawn
    public Vector2 minSpawnPosition; // Vị trí tối thiểu (góc dưới bên trái map)
    public Vector2 maxSpawnPosition; // Vị trí tối đa (góc trên bên phải map)
    public LayerMask obstacleLayer;  // Layer đại diện cho các vật cản (cây cối, đồ vật)

    public float spawnRadius = 0.5f; // Bán kính kiểm tra vị trí hợp lệ
    bool hasSpawnedEnemies = false;

    void Start()
    {
        if (!hasSpawnedEnemies)
        {
            SpawnRandomEnemies();
            hasSpawnedEnemies = true; // Đảm bảo hàm chỉ chạy một lần
        }
    }

    void SpawnRandomEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector2 spawnPosition = GenerateValidSpawnPosition();

            // Kiểm tra nếu tìm được vị trí hợp lệ
            if (spawnPosition != Vector2.zero)
            {
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
    Vector2 GenerateValidSpawnPosition()
    {
        int maxAttempts = 10;  // Số lần thử tìm vị trí hợp lệ
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            // Tạo vị trí ngẫu nhiên
            Vector2 potentialPosition = new Vector2(
                Random.Range(minSpawnPosition.x, maxSpawnPosition.x),
                Random.Range(minSpawnPosition.y, maxSpawnPosition.y)
            );

            // Kiểm tra xem có va chạm với vật cản không
            if (!Physics2D.OverlapCircle(potentialPosition, spawnRadius, obstacleLayer))
            {
                return potentialPosition; // Vị trí hợp lệ
            }
        }

        // Nếu không tìm thấy vị trí hợp lệ sau số lần thử, trả về Vector2.zero
        return Vector2.zero;
    }
}


