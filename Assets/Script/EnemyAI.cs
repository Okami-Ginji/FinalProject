using UnityEngine;
using Pathfinding;
using ClearSky;
using System.Collections;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using TMPro;

public class EnemyAI : MonoBehaviour
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

    [SerializeField] private GameObject energyObject;
    [SerializeField] private GameObject heal;


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

                    if (cooldown < 0f)
                    {
                        cooldown = timeBtwfire;
                        anim.SetTrigger("attack");
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
                    if (force.x < 0)
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

    //void EnemyShootFireBall()
    //{       
    //    anim.SetTrigger("attack");
    //    var fireballTmp = Instantiate(fireball, transform.position, Quaternion.identity);
    //    Rigidbody2D rb = fireballTmp.GetComponent<Rigidbody2D>();
    //    Vector3 playerPos = FindObjectOfType<PlayerControl>().transform.position;
    //    Vector3 direction = playerPos - transform.position;
    //    rb.AddForce(direction.normalized * fireballSpeed, ForceMode2D.Impulse);
    //}

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
            DropItems();


            alive = false;
            anim.SetTrigger("die");
            if (scoreUI != null)
            {
                scoreUI.AddScore(enemyScore);
            }
            boxCollider2D.enabled = false;

        }
    }
    private void DropItems()
    {
        Vector3 dropOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.3f, 0.8f), 0);

        
        if (energyObject != null && heal != null)
        {
            Instantiate(energyObject, transform.position + dropOffset, Quaternion.identity);
            int chance = Random.Range(1, 11);
            if (chance < 3)
            {
                Instantiate(heal, transform.position - dropOffset, Quaternion.identity);
            }

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


}
