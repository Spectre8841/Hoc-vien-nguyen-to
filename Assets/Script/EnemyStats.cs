using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    public float maxHealth = 50f;  // Máu tối đa của quái
    private float currentHealth;    // Máu hiện tại của quái
    public Slider healthBare;

    private Animator animator;          // Animator để xử lý các hoạt ảnh như "die1"
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;  // Khởi tạo máu đầy đủ khi bắt đầu
        if (healthBare != null)
        {
            healthBare.maxValue = maxHealth;
            healthBare.value = currentHealth;
        }
        animator = GetComponent<Animator>();
    }

    // Hàm xử lý sát thương khi quái bị đánh
    public void TakeDamage(float damage)
    {
        if (isDead) return;         // Nếu quái đã chết thì không xử lý thêm

        currentHealth -= damage;    // Trừ đi lượng sát thương nhận được
        if (healthBare != null)
        {
            healthBare.value = currentHealth;
        }
        Debug.Log("Quái nhận sát thương: " + damage);

        // Nếu quái bị đánh trúng nhưng chưa chết
        if (currentHealth > 0)
        {
            //animator.SetTrigger("takehit1"); // Kích hoạt animation bị đánh trúng
        }
        else if (!isDead)
        {
            Die(); // Nếu quái hết máu
        }

    }

    // Hàm xử lý khi quái chết
    void Die()
    {
        isDead = true;              // Đánh dấu quái đã chết
        animator.SetTrigger("die1");    // Phát hoạt ảnh chết
        Debug.Log("Quái đã chết!");

        // Có thể thêm hiệu ứng biến mất hoặc loại bỏ đối tượng quái sau khi chết
        Destroy(gameObject, 1f);    // Xóa quái sau 1 giây
    }
}

