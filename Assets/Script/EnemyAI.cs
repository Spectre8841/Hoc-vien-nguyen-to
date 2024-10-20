using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Animator anim;
    public float detectionRange = 5f;   // Tầm phát hiện của quái
    public float moveSpeed = 2f;        // Tốc độ di chuyển của quái
    public Transform player;            // Tham chiếu đến nhân vật chính
    public float attackRange = 1.5f;    // Tầm tấn công của quái
    private bool isChasing = false;     // Trạng thái đuổi theo nhân vật chính
    private bool isAttacking = false;   // Trạng thái tấn công
    private bool canAttack = true;
    public float attackCooldown = 1f; // Thời gian chờ giữa các lần tấn công
    private Animator animator;
    private bool isDead = false;        // Kiểm tra xem quái đã chết chưa
    public float health = 50f;         // Máu của quái
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;
        // Tính khoảng cách từ quái đến nhân vật chính
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Nếu nhân vật chính trong tầm phát hiện
        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            isChasing = true;
            isAttacking = false;
        }
        // Nếu nhân vật chính trong tầm tấn công
        else if (distanceToPlayer <= attackRange && !isAttacking)
        {
            isChasing = false;
            isAttacking = true;
            AttackPlayer();
        }
        else
        {
            isChasing = false;
            isAttacking = false;
        }
        // Cập nhật hoạt ảnh
        UpdateAnimations();
    }
    void UpdateAnimations()
    {
        // Nếu quái đang đuổi theo nhân vật chính
        if (isChasing)
        {
            ChasePlayer();
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);  // Tắt hoạt ảnh chạy nếu không đuổi theo
        }

        // Nếu quái đang tấn công nhân vật chính
        if (isAttacking)
        {
            animator.SetBool("isAttacking", true);  // Kích hoạt hoạt ảnh tấn công
        }
        else
        {
            animator.SetBool("isAttacking", false);  // Tắt hoạt ảnh tấn công
        }
    }

    void ChasePlayer()
    {
        animator.SetTrigger("run1");
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void AttackPlayer()
    {
        if (canAttack)
        {
            // Gây sát thương cho nhân vật chính
            animator.SetTrigger("attackeye");
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(20f); // Gây 20 điểm sát thương
            }

            // Ngăn chặn việc tấn công liên tục
            canAttack = false;
            Invoke("ResetAttack", attackCooldown);
        }
    }

    void ResetAttack()
    {
        canAttack = true; // Cho phép tấn công lại sau thời gian chờ
        //animator.ResetTrigger("attackeye"); // Reset trigger tấn công
    }
    public void TakeDamage(float damage)
    {
        if (isDead) return;  // Không nhận sát thương nếu đã chết
        health -= damage;

        // Nếu quái bị đánh trúng nhưng chưa chết
        if (health > 0)
        {
            animator.SetTrigger("takehit1"); // Kích hoạt animation bị đánh trúng
        }
        else if (!isDead)
        {
            Die(); // Nếu quái hết máu
        }
    }
    void Die()
    {
        isDead = true;
        animator.SetTrigger("die1"); // Kích hoạt animation chết
        // Thêm logic để xử lý sau khi quái chết (ví dụ: phá hủy đối tượng sau một khoảng thời gian)
        Destroy(gameObject, 1f); // Xóa quái sau 1 giây
    }
}

