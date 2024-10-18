using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Để sử dụng Slider

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthBare;  // Thanh máu
    public EnemyAI enemyAI;   // Đối tượng quái

    void Start()
    {
        healthBare.maxValue = enemyAI.health; // Đặt giá trị tối đa là máu của quái
        healthBare.value = enemyAI.health;    // Đặt giá trị hiện tại
    }

    void Update()
    {
        // Cập nhật thanh máu theo lượng máu hiện tại
        healthBare.value = enemyAI.health;

        // Nếu quái chết, ẩn hoặc phá hủy thanh máu
        if (enemyAI.health <= 0)
        {
            Destroy(gameObject); // Hoặc ẩn Canvas của thanh máu nếu muốn
        }
    }
}

