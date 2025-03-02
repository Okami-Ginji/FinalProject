using UnityEngine;

public class DynamidControl : MonoBehaviour
{
   
    private Animator anim; 

    void Start()
    {
        anim = GetComponent<Animator>();
    }

   
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            anim.SetBool("isBoom", false);

            
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero; 
                rb.bodyType = RigidbodyType2D.Kinematic; 
            }

            Destroy(gameObject, 1f); 
        }
    }







}
