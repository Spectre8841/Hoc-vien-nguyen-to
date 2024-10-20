using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointTrigger : MonoBehaviour
{
    public string nextSceneName;  // Tên của scene tiếp theo

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem nhân vật chính có chạm vào checkpoint không
        if (collision.CompareTag("Player"))
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        // Tải scene tiếp theo bằng tên
        SceneManager.LoadScene(nextSceneName);
    }
}
