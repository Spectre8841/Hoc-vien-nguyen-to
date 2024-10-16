using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClearSky;

namespace ClearSky
{
    public class SimplePlayerController : MonoBehaviour
    {
        public float movePower = 10f;
        //public float jumpPower = 15f; //Set Gravity Scale in Rigidbody2D Component to 5
        //public float moveSpeed = 5f;
        public float dashSpeed = 2000f;
        public float dashDuration = 0.2f;
        private bool isDashing = false;

        private Rigidbody2D rb;
        private Animator anim;
        Vector3 movement;
        private int direction = 1;
        //bool isJumping = false;
        private bool alive = true;
        public GameObject attackEffect; // Thêm hiệu ứng cho đòn đánh chay
        public float attackRange = 1f;
        public LayerMask enemyLayers;

        public GameObject fireballPrefab; // Kỹ năng 1: Fireball
        public Transform firePoint; // Điểm xuất phát của kỹ năng Fireball
        public float fireballSpeed = 10f;

        public GameObject shieldPrefab; // Kỹ năng 2: Shield
        public float shieldDuration = 5f; // Thời gian tồn tại của lá chắn

        public GameObject lightningPrefab; // Kỹ năng 3: Lightning Strike
        public float strikeCooldown = 10f; // Thời gian hồi chiêu của kỹ năng
        private float nextStrikeTime = 0f;


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
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
                //Jump();
                Run();
                if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
                {
                    StartCoroutine(Dash());
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            anim.SetBool("isJump", false);
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
            if (Input.GetKeyDown(KeyCode.Q))
            {
                anim.SetTrigger("attack");  
            }
            // Kiểm tra kẻ địch trong tầm đánh
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);

            // Gây sát thương lên kẻ địch
            //foreach (Collider2D enemy in hitEnemies)
            //    {
            //        enemy.GetComponent<EnemyStats>().TakeDamage(attackEffect.GetComponent<AttackEffect>().damage);
            //    }
        }
        //void Hurt()
        //{
        //    if (Input.GetKeyDown(KeyCode.Alpha2))
        //    {
        //        anim.SetTrigger("hurt");
        //        if (direction == 1)
        //            rb.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
        //        else
        //            rb.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
        //    }
        //}
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
            anim.SetTrigger("attack");
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rbFireball = fireball.GetComponent<Rigidbody2D>();
            rbFireball.velocity = firePoint.right * fireballSpeed;
        }

        // Kỹ năng 2: Shield (F)
        void ActivateShield()
        {
            anim.SetTrigger("attack");
            GameObject shield = Instantiate(shieldPrefab, transform.position, Quaternion.identity, transform);
            Destroy(shield, shieldDuration);
        }

        // Kỹ năng 3: Lightning Strike (V)
        void CastLightningStrike()
        {
            if (Time.time >= nextStrikeTime)
            {
                anim.SetTrigger("attack");
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
                ActivateShield();
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