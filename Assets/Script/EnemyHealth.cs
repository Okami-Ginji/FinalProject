using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 6;
    private Animator anim;
    //public SpriteRenderer characterSR;

    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    public void CalculatorDamage(int amount)
    {
        ChangeHealth(amount);
    }
    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth <= 0)
        {
            anim.SetTrigger("die");          
        }
    }

    public void WaitAndDisable()
    {       
        gameObject.SetActive(false);
    }

}
