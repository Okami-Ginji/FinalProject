using UnityEngine;
using Pathfinding;
using ClearSky;
using System.Collections;

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

    public SpriteRenderer characterSR;

    public bool isShootable = false;
    public GameObject fireball;
    public float fireballSpeed;
    public float timeBtwfire;
    private float cooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("CalculatePath", 0f, 0.5f);
        reachDestination = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShootable)
        {
            cooldown -= Time.deltaTime;

            if (cooldown < 0f)
            {
                cooldown = timeBtwfire;
                EnemyShootFireBall();
            }
        }
        
    }

    void CalculatePath()
    {
        Vector2 targetPos = FindTarget();

        if (seeker.IsDone() && (reachDestination || updateContinuesPath))
        {
            seeker.StartPath(transform.position, targetPos, OnPathComplete);
        }
    }

    void OnPathComplete(Path p) {
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
        //reachDestination = false;
        while (currentWP < path.vectorPath.Count)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWP] - (Vector2)transform.position).normalized;
            Vector2 force = direction * moveSpeed * Time.deltaTime;
            transform.position += (Vector3)force;

            float distance = Vector2.Distance(transform.position, path.vectorPath[currentWP]);
            if(distance < nextWPDistance)
            {
                currentWP++;
            }

            if(force.x != 0)
                if (force.x < 0)
                characterSR.transform.localScale = new Vector3(-1, 1, 0);
            else
                characterSR.transform.localScale = new Vector3(1, 1, 0);
            yield return null;
        }     
        
        reachDestination = true;
    }
    Vector2 FindTarget()
    {
        Vector3 playerPos = FindObjectOfType<SimplePlayerController>().transform.position;
        if(roaming == true)
        {
            return (Vector2)playerPos + (Random.Range(10f,50f) * new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f)).normalized);
        }
        else
        {
            return playerPos;
        }
    }

    void EnemyShootFireBall()
    {
        
        var fireballTmp = Instantiate(fireball, transform.position, Quaternion.identity);
        Rigidbody2D rb = fireballTmp.GetComponent<Rigidbody2D>();
        Vector3 playerPos = FindObjectOfType<SimplePlayerController>().transform.position;
        Vector3 direction = playerPos - transform.position;
        rb.AddForce(direction.normalized * fireballSpeed, ForceMode2D.Impulse);
    }
}
