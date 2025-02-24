using UnityEngine;
using Pathfinding;
using ClearSky;
using System.Collections;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using TMPro;

public class EnemyAISeries : MonoBehaviour
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
    public int damage;

    public ExpUI expUI;
    public float enemyExp;

    public ScoreUI scoreUI;
    public int enemyScore = 100;

    public BoxCollider2D boxCollider2D;

    [SerializeField] EnemyHealthBar healthBar;
    //public SpriteRenderer characterSR;

    public bool isAttackable = false;
    public float distanceToAttack;
    public float stopDistance;

    public float timeBtwfire;
    private float cooldown;
    public Transform attackPos;

    private Animator anim;

    public int currentHealth;
    public int maxHealth;

    private bool alive = true;

    public GameObject popupDamagePrefab;

    public LayerMask playerLayer;


    private Vector3 EnemyScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        EnemyScale = transform.localScale;
        InvokeRepeating("CalculatePath", 0f, 0.5f);
        reachDestination = true;
        expUI =FindAnyObjectByType<ExpUI>();
        scoreUI = FindAnyObjectByType<ScoreUI>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        healthBar.UpdateEnemyHealth(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            if (FindObjectOfType<PlayerControl>() != null)
            {
                Vector3 playerPos = FindObjectOfType<PlayerControl>().transform.position;
                if (isAttackable &&  Vector2.Distance(transform.position, playerPos) <= distanceToAttack)
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

    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPos.position, distanceToAttack, playerLayer);
        if(hits.Length > 0)
        {
            hits[0].GetComponent<PlayerControl>().ChangeHealth(-damage);
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        Debug.Log(transform.position);
        GameObject instance = Instantiate(popupDamagePrefab, transform.position, Quaternion.identity);
        instance.GetComponentInChildren<TMP_Text>().text = amount.ToString();
        //valueText.text = amount.ToString();

        healthBar.UpdateEnemyHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            alive = false;
            anim.SetTrigger("die");
            if (expUI != null)
            {
                expUI.UpdateBar(enemyExp);
            }
            if (scoreUI != null)
            {
                scoreUI.AddScore(enemyScore);
            }
            boxCollider2D.enabled = false;

        }
    }

    public void WaitAndDisable()
    {
        gameObject.SetActive(false);
    }


}
