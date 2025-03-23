using UnityEngine;

public class BallFireRotate : MonoBehaviour
{
    public Transform player;
    public float ballRadius = 1.2f;
    public float ballSpeed = 180f;
    private float angle;

    public float knockbackForce = 5f;

    public void Initialize(Transform playerTransform, float offsetAngle)
    {
        this.player = playerTransform;
        this.angle = offsetAngle;
    }

    void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        Vector2 orbitCenter = player.position;
        angle += ballSpeed * Time.deltaTime;
        float radians = angle * Mathf.Deg2Rad;

        Vector2 ballPosition = orbitCenter + new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * ballRadius;
        transform.position = ballPosition;

        transform.rotation = Quaternion.identity;
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
