using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth ;
    public int maxHealth = 10;
    private Animator anim;
    private bool alive = true;

    void Start()
    {
        anim =  GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    public void CalculatorDamage(int amount)
    {
        ChangeHealth(amount);
    }
    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth <= 0 && alive)
        {
            alive = false;
            anim.SetTrigger("die");
            StartCoroutine(WaitAndDisable()); 
        }
    }

    IEnumerator WaitAndDisable()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

}
