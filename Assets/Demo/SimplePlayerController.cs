using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClearSky;
using System;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UI;

namespace ClearSky
{
    public class SimplePlayerController : MonoBehaviour
    {
        public float movePower = 10f;
        public float dashSpeed = 2000f;
        public float dashDuration = 0.2f;
        private bool isDashing = false;

        private Rigidbody2D rb;
        private Animator anim;
        private PlayerStats playerStats; // Thêm tham chiếu tới PlayerStats
        Vector3 movement;
        private int direction = 1;
        //bool isJumping = false;
        private bool alive = true;
        public GameObject attackEffect; // Thêm hiệu ứng cho đòn đánh chay
        public float attackRange = 3f;
        public float attackRange1 = 10f;
        public float attackRate = 0.2f; // Kho?ng th?i gian gi?a các ?òn ?ánh
        //private float nextAttackTime = 0f;
        public LayerMask enemyLayers;
        public Transform attackPoint;

        public GameObject fireballPrefab; // Kỹ năng 1: Fireball
        public Transform firePoint; // Điểm xuất phát của kỹ năng Fireball
        public float fireballSpeed = 10f;
        public float fireballCooldown = 3f; // Thời gian hồi chiêu cho kỹ năng Fireball
        private float fireballNextTime; // Thời gian cho phép sử dụng lại kỹ năng Fireball
        private bool isFireballCooldown = false; // Trạng thái hồi chiêu
        public Image fireballCooldownImage; // Hình ảnh mô phỏng hồi chiêu
        private float fireballCooldownTimer; // Timer để theo dõi thời gian hồi chiêu

        public GameObject fireAnimationPrefab; // Kỹ năng 2: Lửa
        public Transform firePoint1; // Điểm xuất phát của kỹ năng Lửa
        public float fireAnimationSpeed = 10f; // Tốc độ của lửa
        public float burnDuration = 5f; // Thời gian thiêu đốt
        public float burnDamage = 2f; // Sát thương mỗi giây
        public Image fireCooldownImage; // Hình ảnh mô phỏng hồi chiêu cho kỹ năng Lửa
        private float fireNextTime; // Thời gian cho phép sử dụng lại kỹ năng Lửa
        private bool isFireCooldown = false; // Trạng thái hồi chiêu
        private float fireCooldown = 4f; // Thời gian hồi chiêu cho kỹ năng Lửa
        private float fireCooldownTimer; // Timer để theo dõi thời gian hồi chiêu

        public GameObject lightningPrefab; // Kỹ năng 3: Lightning Strike
        public float strikeCooldown = 10f; // Thời gian hồi chiêu của kỹ năng
        private float nextStrikeTime = 0f;
        public float attackCooldown = 0.5f; // Thời gian giữa các đòn đánh (0.5 giây)
        private bool isCooldown = false; // Để kiểm tra nếu đòn đánh đang trong thời gian hồi chiêu
        public Weapon weapon; // Tham chiếu tới vũ khí


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            playerStats = GetComponent<PlayerStats>(); // Khởi tạo tham chiếu đến PlayerStats
        }

        private void Update()
        {
            Restart();
            if (alive)
            {
                //Hurt();
                Die();
                Attack();
                UseSkills();
                Run();
                if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
                {
                    StartCoroutine(Dash());
                }
                if (isFireballCooldown)
                {
                    if (Time.time >= fireballNextTime)
                    {
                        isFireballCooldown = false; // Kỹ năng có thể sử dụng lại
                        fireballCooldownImage.fillAmount = 0; // Reset hình ảnh
                    }
                    else
                    {
                        // Cập nhật thời gian hồi chiêu
                        fireballCooldownTimer = (fireballNextTime - Time.time) / fireballCooldown; // Tính toán tỷ lệ hồi chiêu
                        fireballCooldownImage.fillAmount = fireballCooldownTimer; // Cập nhật tỷ lệ hồi chiêu cho hình ảnh
                    }
                }
                // Kiểm tra trạng thái hồi chiêu cho kỹ năng FireAnimation
                if (isFireCooldown)
                {
                    if (Time.time >= fireNextTime)
                    {
                        isFireCooldown = false; // Kỹ năng có thể sử dụng lại
                        fireCooldownImage.fillAmount = 0; // Reset hình ảnh
                    }
                    else
                    {
                        // Cập nhật thời gian hồi chiêu
                        fireCooldownTimer = (fireNextTime - Time.time) / fireCooldown; // Tính toán tỷ lệ hồi chiêu
                        fireCooldownImage.fillAmount = fireCooldownTimer; // Mờ dần hình ảnh (giảm fillAmount)
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Nếu va chạm với quái (kẻ thù)
            if (other.gameObject.CompareTag("Enemy"))
            {
                playerStats.TakeDamage(10f); // Nhận 10 sát thương từ quái
                StartCoroutine(HurtAndRecover()); // Gọi Coroutine để xử lý Hurt và trở về trạng thái bình thường
                //Hurt(); // Gọi hàm Hurt để chuyển sang trạng thái bị thương
            }
        }
        private IEnumerator HurtAndRecover()
        {
            // Kích hoạt trạng thái hurt
            anim.SetTrigger("hurt");

            // Chờ một khoảng thời gian trước khi trở về trạng thái idle (ví dụ 0.5 giây)
            yield return new WaitForSeconds(0.5f);

            // Trở về trạng thái idle sau khoảng thời gian chờ
            anim.SetTrigger("idle");
        }
        void Run()
        {
            Vector3 moveVelocity = Vector3.zero;
            anim.SetBool("isRun", false);


            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                direction = -1;
                moveVelocity = Vector3.left;

                transform.localScale = new Vector3(direction, 1, 1);
                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);

            }
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                direction = 1;
                moveVelocity = Vector3.right;

                transform.localScale = new Vector3(direction, 1, 1);
                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);

            }
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                moveVelocity += Vector3.up; // Moving up
                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);
            }
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                moveVelocity += Vector3.down; // Moving down
                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);
            }
            transform.position += moveVelocity * movePower * Time.deltaTime;
        }
        void Attack()
        {
            if (Input.GetKeyDown(KeyCode.Q) && !isCooldown)
            {
                isCooldown = true;
                anim.SetTrigger("attack");
                weapon.Attack();
                //// Tìm kẻ địch trong tầm tấn công
                //Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

                // Gây sát thương lên kẻ địch
                foreach (Collider2D enemy in hitEnemies)
                {
                    Debug.Log("Đã tấn công quái: " + enemy.name);
                    //enemy.GetComponent<EnemyStats>().TakeDamage(playerStats.attackDamage); // Sử dụng sát thương từ PlayerStats
                    EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
                    if (enemyStats != null)
                    {
                        enemyStats.TakeDamage(playerStats.attackDamage);
                    }
                }

                StartCoroutine(ResetAttackCooldown());
            }
        }
        // Hồi chiêu sau khi đánh
        IEnumerator ResetAttackCooldown()
        {
            yield return new WaitForSeconds(attackCooldown);
            isCooldown = false;
        }

        // Hàm này để vẽ hình tròn biểu diễn phạm vi tấn công khi bạn phát triển trong Unity Editor
        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }

        void Hurt()
        {
            anim.SetTrigger("hurt");
            playerStats.TakeDamage(10f); // Gọi phương thức TakeDamage từ PlayerStats
            if (direction == 1)
                rb.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
            else
                rb.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
        }

        void Die()
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                anim.SetTrigger("die");
                alive = false;
            }
        }
        // Kỹ năng 1: Fireball (R)
        void CastFireball()
        {
            // Kiểm tra xem kỹ năng có đang hồi chiêu không
            if (isFireballCooldown)
            {
                Debug.Log("Kỹ năng Fireball đang hồi chiêu!");
                return; // Nếu đang hồi chiêu, không thực hiện
            }
            //anim.SetTrigger("fireball"); // Kích hoạt animation

            // Tạo quả cầu lửa từ prefab tại vị trí firePoint
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rbFireball = fireball.GetComponent<Rigidbody2D>();

            // Tìm quái gần nhất
            GameObject closestEnemy = FindClosestEnemy();

            if (closestEnemy != null)
            {
                // Hướng về quái gần nhất
                Vector2 direction = (closestEnemy.transform.position - firePoint.position).normalized;
                rbFireball.velocity = direction * fireballSpeed;
            }
            else
            {
                // Nếu không có quái gần, quả cầu lửa bay thẳng
                rbFireball.velocity = firePoint.right * fireballSpeed;
            }

            // Thêm Collider để xử lý va chạm
            fireball.GetComponent<Collider2D>().isTrigger = true; // Đảm bảo quả cầu lửa có thể va chạm với các đối tượng
            // Bắt đầu thời gian hồi chiêu
            isFireballCooldown = true;
            fireballNextTime = Time.time + fireballCooldown; // Đặt thời gian hồi chiêu
        }

        GameObject FindClosestEnemy()
        {
            // Lấy tất cả các quái trong phạm vi xung quanh firePoint
            Collider2D[] enemies = Physics2D.OverlapCircleAll(firePoint.position, attackRange1, enemyLayers);

            if (enemies.Length == 0)
            {
                return null; // Không có quái
            }

            GameObject closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (Collider2D enemy in enemies)
            {
                float distanceToEnemy = Vector2.Distance(firePoint.position, enemy.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = enemy.gameObject;
                }
            }

            return closestEnemy;
        }

        // Kỹ năng 2: Shield (F)
        void CastFireAnimation()
        {
            // Kiểm tra xem kỹ năng có đang hồi chiêu không
            if (isFireCooldown)
            {
                Debug.Log("Kỹ năng Lửa đang hồi chiêu!");
                return; // Nếu đang hồi chiêu, không thực hiện
            }

            // Kích hoạt animation
            //anim.SetTrigger("fireAnimation");

            // Tạo kỹ năng lửa từ prefab tại vị trí firePoint
            GameObject fireAnimation = Instantiate(fireAnimationPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rbFireAnimation = fireAnimation.GetComponent<Rigidbody2D>();

            // Tìm quái gần nhất
            GameObject closestEnemy = FindClosestEnemy1();

            if (closestEnemy != null)
            {
                // Hướng về quái gần nhất
                Vector2 direction = (closestEnemy.transform.position - firePoint.position).normalized;
                rbFireAnimation.velocity = direction * fireAnimationSpeed;

                // Gọi hàm để tạo vùng thiêu đốt khi va chạm
                FireArea fireArea = fireAnimation.GetComponent<FireArea>();
                if (fireArea != null)
                {
                    fireArea.Initialize(closestEnemy.transform, burnDuration, burnDamage);
                }
            }
            else
            {
                // Nếu không có quái gần, quả cầu lửa bay thẳng
                rbFireAnimation.velocity = firePoint.right * fireAnimationSpeed;
            }

            // Bắt đầu hồi chiêu
            isFireCooldown = true;
            fireNextTime = Time.time + fireCooldown;
        }

        GameObject FindClosestEnemy1()
        {
            // Lấy tất cả các quái trong phạm vi xung quanh firePoint
            Collider2D[] enemies = Physics2D.OverlapCircleAll(firePoint.position, attackRange1, enemyLayers);

            if (enemies.Length == 0)
            {
                return null; // Không có quái
            }

            GameObject closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (Collider2D enemy in enemies)
            {
                float distanceToEnemy = Vector2.Distance(firePoint.position, enemy.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = enemy.gameObject;
                }
            }

            return closestEnemy;
        }

            // Kỹ năng 3: Lightning Strike (V)
            void CastLightningStrike()
        {
            if (Time.time >= nextStrikeTime)
            {
                anim.SetTrigger("lookat");
                Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.z = 0;
                Instantiate(lightningPrefab, targetPosition, Quaternion.identity);
                nextStrikeTime = Time.time + strikeCooldown;
            }
        }

        // Sử dụng các kỹ năng
        void UseSkills()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                CastFireball();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                CastFireAnimation();
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                CastLightningStrike();
            }
        }

        // Kỹ năng Lướt (Dash)
        IEnumerator Dash()
        {
            isDashing = true;
            float originalSpeed = movePower;
            movePower = dashSpeed;

            yield return new WaitForSeconds(dashDuration);

            movePower = originalSpeed;
            isDashing = false;
        }
        void Restart()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                anim.SetTrigger("idle");
                alive = true;
            }
        }
    }
}