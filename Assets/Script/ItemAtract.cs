using UnityEngine;

public class ItemAtract : MonoBehaviour
{
    public float attractRange = 1f;
    public float attractSpeed = 1.5f;

    private Transform player;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerControl>().transform;
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
}
