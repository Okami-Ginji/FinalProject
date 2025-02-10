using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;


public class HealthBar : MonoBehaviour
{
    public UnityEngine.UI.Image fillBar;
    public TextMeshProUGUI valueText;
    public Animator healthTextAnim;

    public void UpdateBar(int currentHealth, int maxHealth)
    {
        healthTextAnim.Play("HealthText");
        fillBar.fillAmount = (float)currentHealth/(float)maxHealth;
        valueText.text = currentHealth + " / " + maxHealth;
    }
}
