using Pathfinding;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BossAI : MonoBehaviour
{
    public bool roaming = true;

    public Seeker seeker;

    public float moveSpeed;

    public float nextWPDistance;

    Path path;

    Coroutine moveCoroutine;

    bool reachDestination = true;

    public bool updateContinuesPath;

    Rigidbody2D rb;

    public GameObject targetChase;
    public float timeBtwSpell;
    private float spellcooldown;
    public float timeBtwTele;
    private float telecooldown;
    public GameObject spell;



    public ScoreUI scoreUI;
    public int enemyScore = 100;

    public BoxCollider2D boxCollider2D;

    [SerializeField] EnemyHealthBar healthBar;
    //public SpriteRenderer characterSR;

    public bool isAttackable = false;
    public float distanceToAttack;
    public float stopDistance;

    public GameObject fireball;
    public float fireballSpeed;
    public float timeBtwfire;
    private float cooldown;
    public Transform firePos;

    private Animator anim;

    public int currentHealth;
    public int maxHealth;

    private bool alive = true;

    public int damgeForSeries;

    public LayerMask playerLayer;

    public GameObject popupDamagePrefab;
    private Vector3 EnemyScale;

    //knockback
    public float knockbackTime = 0.2f;
    public float knockbackForce = 15f;
    public bool gettingKnockback = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        EnemyScale = transform.localScale;
        InvokeRepeating("CalculatePath", 0f, 0.5f);
        reachDestination = true;

        scoreUI = FindAnyObjectByType<ScoreUI>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        healthBar.UpdateEnemyHealth(currentHealth, maxHealth);
        
        rb = GetComponent<Rigidbody2D>();

        timeBtwTele = 5f;
        telecooldown = timeBtwTele;

        spellcooldown = 10f;

        cooldown = timeBtwfire;
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            if (FindObjectOfType<PlayerControl>() != null)
            {
                Vector3 playerPos = FindObjectOfType<PlayerControl>().transform.position;
                if (isAttackable && Vector2.Distance(transform.position, playerPos) <= distanceToAttack)
                {
                    cooldown -= Time.deltaTime;
                    spellcooldown -= Time.deltaTime;
                    telecooldown -= Time.deltaTime;
                    if (cooldown < 0f)
                    {
                        cooldown = timeBtwfire;
                        anim.SetTrigger("attack");
                        if (spellcooldown < 0f)
                        {

                            if (moveCoroutine != null) StopCoroutine(moveCoroutine);
                            spellcooldown = timeBtwSpell;
                            anim.SetTrigger("castSpell");
                        }
                        float distanceToPlayer = Vector2.Distance(transform.position, playerPos);
                        
                        if (distanceToPlayer > 10f && telecooldown < 0f)
                        {
                            if (moveCoroutine != null) StopCoroutine(moveCoroutine);

                            anim.SetTrigger("teleport");

                            anim.SetTrigger("attack");

                            telecooldown = timeBtwTele;
                        }
                    }


                }
            }
        }

    }

    void CalculatePath()
    {
        if (FindObjectOfType<PlayerControl>() != null && alive)
        {
            Vector2 targetPos = FindTarget();

            if (seeker.IsDone() && (reachDestination || updateContinuesPath))
            {
                seeker.StartPath(transform.position, targetPos, OnPathComplete);
            }
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            MoveToTarget();
        }
    }

    void MoveToTarget()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveToTargetCoroutine());
    }

    IEnumerator MoveToTargetCoroutine()
    {

        int currentWP = 0;
        Vector3 playerPos = FindObjectOfType<PlayerControl>().transform.position;
        while (currentWP < path.vectorPath.Count)
        {

            if (Vector2.Distance(transform.position, playerPos) <= stopDistance)
            {
                anim.SetBool("isMove", false);
                yield return null;
                continue;
            }
            else anim.SetBool("isMove", true);

            if (!gettingKnockback)
            {
                Vector2 direction = ((Vector2)path.vectorPath[currentWP] - (Vector2)transform.position).normalized;
                Vector2 force = direction * moveSpeed * Time.deltaTime;
                transform.position += (Vector3)force;

                float distance = Vector2.Distance(transform.position, path.vectorPath[currentWP]);
                if (distance < nextWPDistance)
                {
                    currentWP++;
                }

                if (force.x != 0)
                {
                    if (force.x > 0)
                    {

                        transform.localScale = new Vector3(-EnemyScale.x, EnemyScale.y, EnemyScale.z);

                    }
                    else
                    {
                        transform.localScale = new Vector3(EnemyScale.x, EnemyScale.y, EnemyScale.z);
                    }

                }

            }
            yield return null;
        }

        reachDestination = true;
    }

    Vector2 FindTarget()
    {
        Vector3 playerPos = FindObjectOfType<PlayerControl>().transform.position;

        if (roaming == true)
        {
            return (Vector2)playerPos + (Random.Range(10f, 50f) * new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized);
        }
        else
        {
            return playerPos;
        }
    }

    public void EnemyShootFireBall()
    {

        var fireballTmp = Instantiate(fireball, firePos.position, Quaternion.identity);
        Rigidbody2D rb = fireballTmp.GetComponent<Rigidbody2D>();
        Vector3 playerPos = FindObjectOfType<PlayerControl>().transform.position;
        Vector3 direction = playerPos - firePos.transform.position;
        rb.AddForce(direction.normalized * fireballSpeed, ForceMode2D.Impulse);
    }



    public void ChangeHealth(int amount, Vector3 damageSource)
    {
        currentHealth += amount;
        //Debug.Log(transform.position);
        GameObject instance = Instantiate(popupDamagePrefab, transform.position, Quaternion.identity);
        instance.GetComponentInChildren<TMP_Text>().text = amount.ToString();
        //valueText.text = amount.ToString();

        healthBar.UpdateEnemyHealth(currentHealth, maxHealth);
        if (amount <= 0)
        {
            getKnockback(damageSource);
        }

        if (currentHealth <= 0)
        {



            alive = false;
            anim.SetTrigger("die");
            if (scoreUI != null)
            {
                scoreUI.AddScore(enemyScore);
            }
            boxCollider2D.enabled = false;

        }
    }

    public void getKnockback(Vector3 damageSource)
    {
        if (rb == null) return;

        gettingKnockback = true;
        Vector3 diff = (transform.position - damageSource).normalized * knockbackForce;
        rb.AddForce(diff, ForceMode2D.Impulse);
        StartCoroutine(knockbackCoroutine());
    }
    private IEnumerator knockbackCoroutine()
    {
        yield return new WaitForSeconds(knockbackTime);
        rb.linearVelocity = Vector3.zero;
        gettingKnockback = false;
    }

    public void WaitAndDisable()
    {
        gameObject.SetActive(false);
    }

    private Vector3 GetSpawnPosition(PlayerControl player)
    {
        float spawnDistance = 5f;
        if (player == null) return transform.position;
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * spawnDistance;


        return player.transform.position + offset;

    }


   

    public void CastSpellToPlayer()
    {


        PlayerControl player = FindObjectOfType<PlayerControl>();
        if (player != null)
        {
            player.movePower = 0f;
            var spellTmp = Instantiate(spell, player.transform.position + new Vector3(0f, 2.3f, 0f), Quaternion.identity);
            // Teleport to player position
            StartCoroutine(ResetPlayerAfterDelay(player, 2.5f, spellTmp));
        }
    }

    private IEnumerator ResetPlayerAfterDelay(PlayerControl player, float delay, GameObject spellTmp)
    {
        yield return new WaitForSeconds(delay);

        if (spellTmp != null)
            Destroy(spellTmp); 

        if (player != null)
            player.movePower = 5f;
    }

    public void TeleportToPlayer()
    {


        PlayerControl player = FindObjectOfType<PlayerControl>();
        if (player != null)
        {
            // Stop current movement
            //if (moveCoroutine != null) StopCoroutine(moveCoroutine);

            // Get player position

            //Vector3 playerPos = GetSpawnPosition(player)
            Vector3 playerPos = GetSpawnPosition(player);

            // Teleport to player position
            transform.position = playerPos;

            // Optional: Add a slight offset so boss doesn't spawn directly on player

            //transform.position = playerPos + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

            // Reset pathfinding
            reachDestination = true;

            // Start cooldown
            Debug.Log("Teleport");

            anim.ResetTrigger("teleport");
            // Optional: Trigger animation if you have one
            //anim.SetTrigger("teleport"); // Make sure to add this trigger to your animator if you use it
        }


    }
}