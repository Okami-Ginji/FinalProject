using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;

    public Animator animator;

    public Vector3 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        transform.position += moveInput * moveSpeed * Time.deltaTime;
        animator.SetFloat("speed", moveInput.sqrMagnitude);
        if(moveInput.x != 0)
        {
            if(moveInput.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 0);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 0);
            }
        }

    }
}
