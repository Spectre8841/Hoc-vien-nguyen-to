using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // Kiểm tra va chạm với quái
        {
            EnemyStats enemyStats = other.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(30); // Gọi hàm TakeDamage của quái, truyền số sát thương vào
            }
            Destroy(gameObject); // Sau khi va chạm, phá hủy quả cầu lửa
        }
    }
}
