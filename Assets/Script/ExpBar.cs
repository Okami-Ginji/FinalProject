using TMPro;
using UnityEngine;

public class ExpBar : MonoBehaviour
{
    private int level;
    public float currentExp =0 ;
    public float maxExp = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        level = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public UnityEngine.UI.Image fillBar;
    public TextMeshProUGUI valueText;

    public void UpdateBar(float enemyExp)
    {
        currentExp += enemyExp;
        fillBar.fillAmount = (float)currentExp / (float)maxExp;
        
        if (currentExp >= maxExp)
        {
            level++;
            fillBar.fillAmount = 0;
        }
        valueText.text = "Level " + level;
    }
}
