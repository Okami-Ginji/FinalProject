using UnityEngine;

public class ItemAtract : MonoBehaviour
{
    public float attractRange = 1f;
    public float attractSpeed = 1.5f;

    private Transform player;
    public AudioManager_PlayScreen audioManager;
    private bool Collected = false;
    private void Start()
    {
        player = FindAnyObjectByType<PlayerControl>().transform;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager_PlayScreen>();
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);


        if (distance <= attractRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, attractSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !Collected)
        {
            Collected = true;
            if (audioManager != null)
            {
                audioManager.PlaySFX(audioManager.expClip); 
            }
            Destroy(gameObject); 
        }
    }
}
