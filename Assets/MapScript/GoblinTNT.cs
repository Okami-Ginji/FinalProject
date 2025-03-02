using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GoblinTNT : MonoBehaviour
{
    
    public GameObject Dynamidprefab;
    public float dynamidSpeed;
    private float cooldown;
    private Animator anim;
    public bool isAttackable = false;
    public float distanceToAttack;
    public float timeBtwfire;
    public Transform dynamidPos;
    void Start()
    {
        anim = GetComponent<Animator>();
        

    }

    // Update is called once per frame
    void Update()
    {
        
        if (FindObjectOfType<PlayerControl>() != null)
        {
           
            Vector3 playerPos = FindObjectOfType<PlayerControl>().transform.position;
           
            if (isAttackable && Vector2.Distance(transform.position, playerPos) <= distanceToAttack)
            {
                
                cooldown -= Time.deltaTime;
               

                if (cooldown < 2f)
                {
                   
                    cooldown = timeBtwfire;
                    anim.SetTrigger("GoblinFire");
                    GoblinShoot();


                }
            }
        }


    }


    public void GoblinShoot()
    {
     

        var fireballTmp = Instantiate(Dynamidprefab, transform.position, Quaternion.identity);
        

        Rigidbody2D rb = fireballTmp.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            
            return;
        }

        Vector3 playerPos = FindFirstObjectByType<PlayerControl>().transform.position;
        Vector3 direction = playerPos - transform.position;
        

        rb.AddForce(direction.normalized * dynamidSpeed, ForceMode2D.Impulse);
        
    }

}
