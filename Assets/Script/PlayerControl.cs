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
    public float dashCooldown = 2f;
    private float _dashCooldown = 0f;
    bool isDashing = false;

    public GameObject ghostEffect;
    public float ghostDelay = 0.05f;
    private Coroutine dashEffectCoroutine;


    private Rigidbody2D rb;
    private Animator anim;
    Vector3 movement;
    
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
    //public AudioSource footstepSound;

    private Vector3 playerScale;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();      
        healthBar.UpdateBar(currentHealth, maxHealth);     
        cooldown = timeToReload;
        Reload();
        playerScale = transform.localScale;
    }

    private void Update()
    {
        Restart();
        if (alive)
        {
            RotaleFirePos();          
            Die();
            Attack();           
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
                //transform.localScale = new Vector3(0.2f, 0.2f, 0);
                transform.localScale = new Vector3(playerScale.x, playerScale.y, playerScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-playerScale.x, playerScale.y, playerScale.z);
                //transform.localScale = new Vector3(-0.2f, 0.2f, 0);
            }         
        }
        if (Input.GetKeyDown(KeyCode.Space) && _dashTime <= 0 && isDashing == false && _dashCooldown <= 0)
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
            _dashCooldown = dashCooldown;
            StopDashEffect();
        }
        else
        {
            _dashTime -= Time.deltaTime;
            
        }
        if (_dashCooldown > 0)
        {
            _dashCooldown -= Time.deltaTime; 
            if (_dashCooldown < 0) _dashCooldown = 0; 
        }

        ////âm thanh khi di chuyển
        //if (moveVelocity.magnitude > 0)
        //{
        //    if (!footstepSound.isPlaying)
        //    {
        //        footstepSound.Play();
        //    }
        //}
        //else
        //{
        //    footstepSound.Stop();
        //}
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
            ghost.transform.localScale = transform.localScale;
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
                //FireBall();
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
      
        if (ammoAmountCopy <= 0)
        {          
            return; 
        }

        timeBtwFire = TimeBtwFire;

        GameObject fireballTmp = Instantiate(fireball, firePos.position, firePos.transform.rotation);
        Rigidbody2D rb = fireballTmp.GetComponent<Rigidbody2D>();
        rb.AddForce(firePos.right * fireballForce, ForceMode2D.Impulse);
        ammoAmountCopy--;
        if (ammoAmountCopy >= 0 && ammoAmountCopy < ammo.transform.childCount)
        {
            ammo.transform.GetChild(ammoAmountCopy).gameObject.SetActive(false);
        }
    }

    void Reload()
    {       
        ammoAmountCopy = ammo.transform.childCount;
        for (int i = 0; i < ammo.transform.childCount; i++) {
            ammo.transform.GetChild(i).gameObject.SetActive(true);
        }
         
    }
    public void Hurt(float damageForce)
    {
        if (alive)
        {
            anim.SetTrigger("hurt");
          
            float direction = Mathf.Sign(transform.localScale.x);
          
            rb.linearVelocity = new Vector2(-direction * damageForce, rb.linearVelocity.y);
            
            StartCoroutine(StopKnockback());
        }
    }
  
    IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(0.2f);  
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);  
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
        if (alive)
        {
            currentHealth += amount;
            healthBar.UpdateBar(currentHealth, maxHealth);
            if (currentHealth <= 0)
            {
                alive = false;
                anim.SetTrigger("die");
            }
        }
    }

    public void WaitAndDisable()
    {
        gameObject.SetActive(false);
    }
}