using UnityEngine;

public class OrbitingSword : MonoBehaviour
{
    public Transform player;
    public float orbitRadius = 1.2f;
    public float orbitSpeed = 180f;
    private float angle;

    public float knockbackForce = 5f;
    public void Initialize(Transform playerTransform, float offsetAngle)
    {
        this.player = playerTransform;
        this.angle = offsetAngle;
    }
    public void UpdateAngle(float newAngle)
    {
        this.angle = newAngle;
    }

    void LateUpdate()
    {
        if (player == null)
        {
            return;
        }
        Vector2 orbitCenter = player.position;
        angle += orbitSpeed * Time.deltaTime;
        float radians = angle * Mathf.Deg2Rad;

        Vector2 orbitPosition = orbitCenter + new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * orbitRadius;
        transform.position = orbitPosition;

        float angleToForward = angle - 45f;
        transform.rotation = Quaternion.Euler(0, 0, angleToForward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyAI enemy = collision.GetComponent<EnemyAI>();
            
            if (enemy != null)
            {
                enemy.getKnockback(transform.position);
                enemy.ChangeHealth(-3, transform.position);
                

                Rigidbody2D enemyRb = collision.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    enemy.getKnockback(transform.position);
                }
            }
        }
        else if (collision.CompareTag("EnemySeries"))
        {
            
            EnemyAISeries aISeries = collision.GetComponent<EnemyAISeries>();
            if (aISeries != null)
            {
                
                aISeries.getKnockback(transform.position);
                aISeries.ChangeHealth(-3, transform.position);

                Rigidbody2D enemyRb = collision.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    aISeries.getKnockback(transform.position);
                }
            }
        }
    }
}
