using UnityEngine;

public class FireBall : MonoBehaviour
{
    public int damage;
    public bool isPlayerShoot;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isPlayerShoot)
        {
            collision.gameObject.GetComponent<PlayerControl>().ChangeHealth(-damage);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemy") && isPlayerShoot)
        {
            collision.gameObject.GetComponent<EnemyAI>().ChangeHealth(-damage);
            Destroy(gameObject);
        }
    }
}
