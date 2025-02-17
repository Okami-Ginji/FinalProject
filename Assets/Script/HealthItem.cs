using UnityEngine;

public class HealthItem : MonoBehaviour
{
    public int healAmount = 4; // Lượng máu hồi phục
   private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();

            if (player.currentHealth < 10)
            {
                // Hồi máu nhưng không vượt quá 150
                player.ChangeHealth(Mathf.Min(healAmount, 10 - player.currentHealth));

                if (audioManager != null)
                {
                    audioManager.PlaySFX(audioManager.healthClip);
                }

                Destroy(gameObject);
            }
        }
    }
}