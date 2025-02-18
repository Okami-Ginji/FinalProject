﻿using UnityEngine;
using TMPro;
using System.Collections;


public class PlayerControl : MonoBehaviour
{
    public float movePower = 10f;
    public float jumpPower = 15f; //Set Gravity Scale in Rigidbody2D Component to 5

    //Dash
    public float dashBoost = 5f;
    public float dashTime;
    private float _dashTime = 0.5f;
    bool isDashing = false;

    public GameObject ghostEffect;
    public float ghostDelay = 0.05f;
    private Coroutine dashEffectCoroutine;


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

    public int currentHealth;
    public int maxHealth = 10;

    public GameObject ammo;
    //public int ammoAmount;
    private int ammoAmountCopy;
    public float timeToReload;
    private float cooldown;

    public HealthBar healthBar;

    // âm thanh bước chân
    public AudioSource footstepSound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();      
        healthBar.UpdateBar(currentHealth, maxHealth);     
        cooldown = timeToReload;
        Reload();
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
    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    anim.SetBool("isJump", false);
    //}


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
                transform.localScale = new Vector3(0.2f, 0.2f, 0);
            }
            else
            {
                transform.localScale = new Vector3(-0.2f, 0.2f, 0);
            }         
        }

        if (Input.GetKeyDown(KeyCode.Space) && _dashTime <= 0 && isDashing == false)
        {
            movePower += dashBoost;
            _dashTime = dashTime;
            isDashing = true;
            StartDashEffect();
        }
        if (_dashTime <= 0 && isDashing == true)
        {
            movePower -= dashBoost;
            isDashing = false;
            StopDashEffect();
        }
        else
        {
            _dashTime -= Time.deltaTime;

        }

        //âm thanh khi di chuyển
        if (moveVelocity.magnitude > 0)
        {
            if (!footstepSound.isPlaying)
            {
                footstepSound.Play();
            }
        }
        else
        {
            footstepSound.Stop();
        }
    }
    void StopDashEffect()
    {
        if (dashEffectCoroutine != null) StopCoroutine(dashEffectCoroutine);
    }

    void StartDashEffect()
    {
        if (dashEffectCoroutine != null) StopCoroutine(dashEffectCoroutine);
        dashEffectCoroutine = StartCoroutine(DashEffectCoroutine());
    }

    IEnumerator DashEffectCoroutine()
    {
        while (true)
        {
            GameObject ghost = Instantiate(ghostEffect, transform.position, transform.rotation);
            Sprite currentSprite = GetComponentInChildren<SpriteRenderer>().sprite;
            ghost.GetComponentInChildren<SpriteRenderer>().sprite = currentSprite;

            Destroy(ghost, 0.5f);
            yield return new WaitForSeconds(ghostDelay);
        }
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
        if(ammoAmountCopy > 0)
        {
            timeBtwFire -= Time.deltaTime;
            if (Input.GetMouseButton(0) && timeBtwFire < 0)
            {
                anim.SetTrigger("attack");
                FireBall();
                timeBtwFire = TimeBtwFire;
            }
        }
        else if(ammoAmountCopy <= 0)
        {
            cooldown -= Time.deltaTime;
            if (cooldown < 0f)
            {
                cooldown = timeToReload;
                Reload();
            }
            
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
        ammoAmountCopy -= 1;
        Debug.Log(ammo.transform.GetChild(ammoAmountCopy).name);
        ammo.transform.GetChild(ammoAmountCopy).gameObject.SetActive(false);
    }

    void Reload()
    {       
        ammoAmountCopy = ammo.transform.childCount;
        for (int i = 0; i < ammo.transform.childCount; i++) {
            ammo.transform.GetChild(i).gameObject.SetActive(true);
        }
         
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

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        healthBar.UpdateBar(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            alive = false;
            anim.SetTrigger("die");          
        }
        
    }

    public void WaitAndDisable()
    {
        gameObject.SetActive(false);
    }
}