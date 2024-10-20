using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerStats : MonoBehaviour
{
    Animator anim;
    public Slider healthBar;   // Thanh máu
    public Slider manaBar;     // Thanh mana
    public float maxHealth = 100f;  // Máu tối đa của nhân vật
    public float currentHealth;    // Máu hiện tại của nhân vật
    public float maxMana = 50f;     // Mana tối đa của nhân vật
    public float currentMana;       // Mana hiện tại của nhân vật
    public float attackDamage = 10f; // Damage khi đánh chay

    public float manaRegenRate = 5f;  // Tốc độ hồi phục mana mỗi giây
    public float healthRegenRate = 2f; // Tốc độ hồi phục máu mỗi giây (nếu có)
    public GameOverManager gameOverManager; // Tham chiếu tới GameOverManager

    void Start()
    {
        anim = GetComponent<Animator>();
        // Khởi tạo máu và mana đầy khi bắt đầu game
        currentHealth = maxHealth;
        currentMana = maxMana;
        // Cập nhật UI ngay từ khi bắt đầu
        UpdateHealthBar();
        UpdateManaBar();
    }

    void Update()
    {
        // Gọi hàm hồi phục máu và mana nếu cần
        RegenerateMana();
        RegenerateHealth();
    }

    // Hồi phục mana mỗi giây
    void RegenerateMana()
    {
        if (currentMana < maxMana)
        {
            currentMana += manaRegenRate * Time.deltaTime;
            currentMana = Mathf.Clamp(currentMana, 0, maxMana); // Đảm bảo không vượt quá mana tối đa
            UpdateManaBar();
        }
    }

    // Hồi phục máu mỗi giây (nếu game cho phép)
    void RegenerateHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healthRegenRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Đảm bảo không vượt quá máu tối đa
            UpdateHealthBar();
        }
    }

    // Gây sát thương cho nhân vật
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            anim.SetTrigger("hurt"); // Nếu chưa chết, chuyển sang trạng thái hurt
        }
    }

    // Hồi máu cho nhân vật
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Giới hạn máu không vượt quá mức tối đa
        UpdateHealthBar();
    }

    // Hàm xử lý khi nhân vật chết
    // Cập nhật thanh máu
    void UpdateHealthBar()
    {
        healthBar.value = currentHealth / maxHealth;
    }

    // Cập nhật thanh mana
    void UpdateManaBar()
    {
        manaBar.value = currentMana / maxMana;
    }
    void Die()
    {
        Debug.Log("Nhân vật đã chết!");
        anim.SetTrigger("die");
        // Thực hiện các hành động khác khi chết (ví dụ: game over, hồi sinh, v.v.)
        gameOverManager.OnPlayerDeath();
    }
}

