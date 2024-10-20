using UnityEngine;
using System;
using System.Collections;

//public class FireAnimation : MonoBehaviour
//{
//    public float burnDuration = 5f; // Thời gian thiêu đốt
//    public float burnDamage = 2f; // Sát thương mỗi giây

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        Debug.Log("Va chạm với: " + other.name); // Kiểm tra xem va chạm với đối tượng nào

//        if (other.CompareTag("Enemy"))
//        {
//            // Tạo vùng thiêu đốt
//            FireArea fireArea = GetComponent<FireArea>();
//            if (fireArea != null)
//            {
//                fireArea.Initialize(other.transform, burnDuration, burnDamage);
//            }
//            Destroy(gameObject); // Hủy chiêu thức lửa sau va chạm
//        }
//    }
//}
// Fireball.cs: Xử lý va chạm với quái
public class Fireball1 : MonoBehaviour
{
    public float fireballSpeed = 5f;
    public float attackRange1 = 5f;
    public float burnDuration = 5f; // Thời gian thiêu đốt
    public float burnDamage = 10f; // Sát thương mỗi giây
    public GameObject fireballPrefab;
    public LayerMask enemyLayers;
    public Transform firePoint;

    //private void Update()
    //{
    //    // Kiểm tra khi người chơi nhấn phím bắn kỹ năng, ví dụ phím Space hoặc phím F
    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        FireAnimation();
    //    }
    //}

    // Hàm xử lý bắn lửa
    //void FireAnimation()
    //{
    //    GameObject fireball = Instantiate(gameObject, firePoint.position, firePoint.rotation);
    //    Rigidbody2D rbFireball = fireball.GetComponent<Rigidbody2D>();

    //    GameObject closestEnemy = FindClosestEnemy();

    //    if (closestEnemy != null)
    //    {
    //        Vector2 direction = (closestEnemy.transform.position - firePoint.position).normalized;
    //        rbFireball.velocity = direction * fireballSpeed;

    //        FireArea fireArea = fireball.GetComponent<FireArea>();
    //        if (fireArea != null)
    //        {
    //            fireArea.Initialize(closestEnemy.transform, burnDuration, burnDamage);
    //        }
    //    }
    //    else
    //    {
    //        rbFireball.velocity = firePoint.right * fireballSpeed; // Nếu không có quái, bắn thẳng
    //    }
    //}
    void FireAnimation()
    {
        // Tạo viên cầu lửa từ prefab, không phải từ chính gameObject
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rbFireball = fireball.GetComponent<Rigidbody2D>();

        GameObject closestEnemy = FindClosestEnemy();

        if (closestEnemy != null)
        {
            Vector2 direction = (closestEnemy.transform.position - firePoint.position).normalized;
            rbFireball.velocity = direction * fireballSpeed;

            FireArea fireArea = fireball.GetComponent<FireArea>();
            if (fireArea != null)
            {
                fireArea.Initialize(closestEnemy.transform, burnDuration, burnDamage);
            }
        }
        else
        {
            rbFireball.velocity = firePoint.right * fireballSpeed; // Nếu không có quái, bắn thẳng
        }
    }

    GameObject FindClosestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(firePoint.position, attackRange1, enemyLayers);

        if (enemies.Length == 0)
        {
            return null;
        }

        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D enemy in enemies)
        {
            float distance = Vector2.Distance(firePoint.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.gameObject;
            }
        }

        return closestEnemy;
    }
}

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        // Tạo vùng thiêu đốt
    //        FireArea fireArea = GetComponent<FireArea>();
    //        if (fireArea != null)
    //        {
    //            fireArea.Initialize(other.transform, burnDuration, burnDamage);
    //        }

    //        Destroy(gameObject); // Hủy viên lửa sau khi va chạm
    //    }
    //}
//}



