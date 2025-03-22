using System;
using TMPro;
using UnityEngine;

public class ExpUI : MonoBehaviour
{
    public int level;
    public int currentLevel;
    public float currentExp ;
    public float maxExp = 100;
    public UnityEngine.UI.Image fillBar;
    public TextMeshProUGUI expText;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        level = 0;
        currentExp = 0;
        UpdateUI();
              
    }

    

    public void UpdateBar(float enemyExp)
    {
        currentExp += enemyExp;
        while (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            level++;
        }       
        UpdateUI();      
    }

    public void UpdateUI()
    {
        fillBar.fillAmount = currentExp / maxExp;
        expText.text = "Level " + level;
    }
    public int GetLevel()
    {
        Debug.Log(level);
        return level;
    }
}
