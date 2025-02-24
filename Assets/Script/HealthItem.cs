using UnityEngine;

public class HealthItem : MonoBehaviour
{
    public int healAmount = 4; 
    private AudioManager audioManager;
    private ItemSpawner itemSpawner; 

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        itemSpawner = FindObjectOfType<ItemSpawner>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();

            if (player.currentHealth < 10)
            {               
                player.ChangeHealth(Mathf.Min(healAmount, 10 - player.currentHealth));
                
                if (audioManager != null)
                {
                    audioManager.PlaySFX(audioManager.healthClip);
                }
               
                if (itemSpawner != null)
                {
                    itemSpawner.OnItemPickedUp();
                }

                Destroy(gameObject); 
            }
        }
    }
}
