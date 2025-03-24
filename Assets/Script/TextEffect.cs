using System.Collections;
using TMPro;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float typingSpeed = 0.05f; // T?c ð? g? ch?

    private string fullText;

    private void Start()
    {
        fullText = textMeshPro.text; // Lýu n?i dung g?c
        textMeshPro.text = ""; // Xóa n?i dung ban ð?u
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in fullText)
        {
            textMeshPro.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
