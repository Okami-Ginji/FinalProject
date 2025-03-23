using UnityEngine;

public class FireBall : MonoBehaviour
{
    public int damage;
    public float damgeForce;
    public bool isPlayerShoot;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isPlayerShoot)
        {
            collision.gameObject.GetComponent<PlayerControl>().ChangeHealth(-damage);
            collision.gameObject.GetComponent<PlayerControl>().Hurt(damgeForce);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemy") && isPlayerShoot)
        {
            collision.gameObject.GetComponent<EnemyAI>().ChangeHealth(-damage,transform.position);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("EnemySeries") && isPlayerShoot)
        {
            collision.gameObject.GetComponent<EnemyAISeries>().ChangeHealth(-damage, transform.position);
            Destroy(gameObject);
        }
    }
}
