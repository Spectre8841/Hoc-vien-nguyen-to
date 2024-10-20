using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI; // Tham chiếu tới UI Game Over

    // Hàm này sẽ được gọi khi người chơi chết
    public void OnPlayerDeath()
    {
        // Kích hoạt giao diện Game Over
        gameOverUI.SetActive(true);

        // Dừng thời gian để ngừng tất cả hoạt động trong game
        Time.timeScale = 0f;
    }

    // Hàm này sẽ được gọi khi người chơi bấm vào nút "Play Again"
    public void RestartGame()
    {
        // Đặt lại thời gian về bình thường
        Time.timeScale = 0.5f;

        // Tải lại scene đầu tiên (level 1)
        SceneManager.LoadScene(3); // 0 là chỉ số của scene level 1 trong Build Settings
    }
}

