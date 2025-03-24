using UnityEngine;
using System.Collections;


public class Meteor : MonoBehaviour
{
    public float speed = 10f;
    public float explosionRadius = 0.5f; // Bán kính gây sát thương
    public int damage = 50;

    private GameObject target;
    private Rigidbody2D rb;
    
    public void SetTarget(GameObject enemy)
    {
        target = enemy;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
            {
                Explode();
                
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Explode()
    {
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                if (enemyAI != null)
                {
                    enemyAI.ChangeHealth(-damage, transform.position);
                    enemyAI.getKnockback(transform.position);
                }
            }
            else if (enemy.CompareTag("EnemySeries"))
            {
                EnemyAISeries enemySeries = enemy.GetComponent<EnemyAISeries>();
                if (enemySeries != null)
                {
                    enemySeries.ChangeHealth(-damage, transform.position);
                    enemySeries.getKnockback(transform.position);
                }
            }
        }

        // Hủy thiên thạch sau khi nổ
        Destroy(gameObject);
    }

    

}
