using TMPro;
using UnityEngine;

public class ExpUI : MonoBehaviour
{
    private int level;
    public float currentExp ;
    public float maxExp = 100;
    public UnityEngine.UI.Image fillBar;
    public TextMeshProUGUI valueText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        level = 0;
        currentExp = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void UpdateBar(float enemyExp)
    {
        currentExp += (float)enemyExp;
        
        if (currentExp >= maxExp)
        {
            currentExp -= (float)maxExp; 
            level++;
            
        }
        fillBar.fillAmount = (float)currentExp / (float)maxExp;
        valueText.text = "Level " + level;
    }
}
