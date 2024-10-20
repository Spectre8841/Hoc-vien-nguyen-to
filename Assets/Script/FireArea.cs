using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArea : MonoBehaviour
{
    private Transform enemyTransform;
    private float burnDuration;
    private float burnDamage;
    private float burnTimer;
    private bool isBurning = false; // Biến kiểm soát để không bị kích hoạt nhiều lần

    public void Initialize(Transform enemy, float duration, float damage)
    {
        //enemyTransform = enemy;
        //burnDuration = duration;
        //burnDamage = damage;
        //burnTimer = 0f;

        //// Bắt đầu thiêu đốt
        //StartCoroutine(BurnEnemy());
        if (!isBurning) // Kiểm tra nếu chưa thiêu đốt, tránh gây sát thương lặp lại
        {
            enemyTransform = enemy;
            burnDuration = duration;
            burnDamage = damage;
            burnTimer = 0f;

            // Bắt đầu thiêu đốt
            StartCoroutine(BurnEnemy());
            isBurning = true; // Kích hoạt trạng thái thiêu đốt
        }
    }

    private IEnumerator BurnEnemy()
    {
        while (burnTimer < burnDuration)
        {
            if (enemyTransform != null)
            {
                // Gây sát thương mỗi giây
                EnemyStats enemyStats = enemyTransform.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(burnDamage);
                }
            }
            else
            {
                break;  // Nếu quái không còn tồn tại, dừng vòng lặp
            }
            burnTimer += 1f;
            yield return new WaitForSeconds(1f);
        }
        Destroy(gameObject);  // Sau khi hết thời gian, hủy vùng thiêu đốt
    }
}

