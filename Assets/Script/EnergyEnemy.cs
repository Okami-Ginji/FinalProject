using UnityEngine;

public class EnergyEnemy : MonoBehaviour
{
    
    [SerializeField]private GameObject energyObject;
    private float energyEnemy = 20 ;
    public ExpUI expUI;

    private void Start()
    {
        expUI = FindAnyObjectByType<ExpUI>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {           
            Destroy(energyObject);  
            expUI.UpdateBar(energyEnemy);
        }

    }
}
