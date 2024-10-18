using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public float attackDamage = 10f; // Sát th??ng c?a v? khí
    public float attackRate = 1f; // Kho?ng th?i gian gi?a các ?òn ?ánh
    private float nextAttackTime = 0f;

    public LayerMask enemyLayers; // L?p c?a k? ??ch ?? phát hi?n va ch?m
    public Transform attackPoint; // ?i?m xu?t phát c?a ?òn t?n công (có th? là v? trí c?a v? khí)
    public float attackRange = 5f; // T?m ?ánh c?a v? khí

    // Hàm g?i khi v? khí th?c hi?n ?òn ?ánh
    public void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            // T?o hi?u ?ng ?ánh vào quái
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                // G?i hàm TakeDamage c?a quái
                enemy.GetComponent<EnemyStats>().TakeDamage(attackDamage);
            }

            nextAttackTime = Time.time + 1f / attackRate; // C?p nh?t th?i gian cho ?òn ?ánh ti?p theo
        }
    }

    // V? hình tròn ?? ki?m tra t?m ?ánh (trong editor)
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        // Thiết lập màu sắc cho Gizmos (màu xanh lục cho dễ nhìn)
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange); // V? m?t hình tròn ?? xem t?m ?ánh
    }
}

