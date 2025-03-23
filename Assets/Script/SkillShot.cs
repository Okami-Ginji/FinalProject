using System.Collections.Generic;
using UnityEngine;

public class SkillShot : MonoBehaviour
{
    public float speed = 10f;
    public float maxSize = 3f;
    public float growthRate = 1f;
    public int damage = 20;
    public float lifetime = 2f;

    private Vector2 direction;
    private HashSet<Collider2D> hitEnemies = new HashSet<Collider2D>();

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
        transform.localScale += Vector3.one * growthRate * Time.deltaTime;
        if (transform.localScale.x > maxSize) transform.localScale = Vector3.one * maxSize;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !hitEnemies.Contains(collision) )
        {
            hitEnemies.Add(collision);
            EnemyAI enemy = collision.GetComponent<EnemyAI>();
            
            if (enemy != null)
            {
                enemy.ChangeHealth(-damage, transform.position);              
            }
        }
        else if ( collision.CompareTag("EnemySeries") && !hitEnemies.Contains(collision))
        {
            hitEnemies.Add(collision);
            EnemyAISeries aISeries = collision.GetComponent<EnemyAISeries>();
            if (aISeries != null)
            {               
                aISeries.ChangeHealth(-damage, transform.position);
            }
        }
    }
}