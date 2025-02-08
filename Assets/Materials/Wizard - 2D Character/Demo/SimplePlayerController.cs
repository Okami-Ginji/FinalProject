using UnityEngine;

namespace ClearSky
{
    public class SimplePlayerController : MonoBehaviour
    {
        public float movePower = 10f;
        public float jumpPower = 15f; //Set Gravity Scale in Rigidbody2D Component to 5

        private Rigidbody2D rb;
        private Animator anim;
        Vector3 movement;
        private int direction = 1;
        bool isJumping = false;
        private bool alive = true;

        public GameObject fireball;
        public Transform firePos;

        public float TimeBtwFire = 0.2f;
        private float timeBtwFire;
        public float fireballForce;

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
                RotaleFirePos();
                Hurt();
                Die();
                Attack();
                //Jump();
                Run();

            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            anim.SetBool("isJump", false);
        }


        void Run()
        {
            Vector3 moveVelocity = Vector3.zero;
            moveVelocity.x = Input.GetAxis("Horizontal");
            moveVelocity.y = Input.GetAxis("Vertical");
            transform.position += moveVelocity * movePower * Time.deltaTime;
            anim.SetFloat("speed", moveVelocity.sqrMagnitude);
            if (moveVelocity.x != 0)
            {
                if (moveVelocity.x > 0)
                {
                    transform.localScale = new Vector3(0.5f, 0.5f, 0);
                }
                else
                {
                    transform.localScale = new Vector3(-0.5f, 0.5f, 0);
                }
            }
            //anim.SetBool("isRun", false);


            //if (Input.GetAxisRaw("Horizontal") < 0)
            //{
            //    direction = -1;
            //    moveVelocity = Vector3.left;

            //    transform.localScale = new Vector3(direction, 1, 1);
            //    if (!anim.GetBool("isJump"))
            //        anim.SetBool("isRun", true);

            //}
            //if (Input.GetAxisRaw("Horizontal") > 0)
            //{
            //    direction = 1;
            //    moveVelocity = Vector3.right;

            //    transform.localScale = new Vector3(direction, 1, 1);
            //    if (!anim.GetBool("isJump"))
            //        anim.SetBool("isRun", true);

            //}
            //transform.position += moveVelocity * movePower * Time.deltaTime;
        }
        void Jump()
        {
            if ((Input.GetButtonDown("Jump") || Input.GetAxisRaw("Vertical") > 0)
            && !anim.GetBool("isJump"))
            {
                isJumping = true;
                anim.SetBool("isJump", true);
            }
            if (!isJumping)
            {
                return;
            }

            rb.linearVelocity = Vector2.zero;

            Vector2 jumpVelocity = new Vector2(0, jumpPower);
            rb.AddForce(jumpVelocity, ForceMode2D.Impulse);

            isJumping = false;
        }
        void Attack()
        {
            timeBtwFire -= Time.deltaTime;
            if (Input.GetMouseButton(0) && timeBtwFire < 0)
            {
                anim.SetTrigger("attack");

                FireBall();
            }
        }

        void RotaleFirePos()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 lookDir = mousePos - firePos.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            firePos.rotation = rotation;

            if (transform.eulerAngles.z > 90 && transform.eulerAngles.z< 270)
                firePos.localScale = new Vector3(1, -1, 0);
            else firePos.localScale = new Vector3(1, 1, 0);
        }
        void FireBall()
        {
            timeBtwFire = TimeBtwFire;
            GameObject fireballTmp = Instantiate(fireball, firePos.position, Quaternion.identity);
            Rigidbody2D rb = fireballTmp.GetComponent<Rigidbody2D>();
            rb.AddForce(firePos.right * fireballForce, ForceMode2D.Impulse);
        }
        void Hurt()
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                anim.SetTrigger("hurt");
                if (direction == 1)
                    rb.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
                else
                    rb.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
            }
        }
        void Die()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                anim.SetTrigger("die");
                alive = false;
            }
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