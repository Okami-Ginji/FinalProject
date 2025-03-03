using UnityEngine;

public class HealthItem : MonoBehaviour
{
    public int healAmount = 4; // Lượng máu hồi phục
    private AudioManager audioManager;
    [SerializeField] private GameObject heal;
    private EnemyAI enemy;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();

            if (player.currentHealth < player.maxHealth)
            {              
                player.ChangeHealth(Mathf.Min(healAmount, player.maxHealth - player.currentHealth));

                if (audioManager != null)
                {
                    audioManager.PlaySFX(audioManager.healthClip);
                }

                Destroy(heal);
            }
        }
        if (collision.CompareTag("Enemy"))
        {
            if (enemy.currentHealth <= 0)
                Instantiate(heal, enemy.transform.position, Quaternion.identity);
        }
    }
}