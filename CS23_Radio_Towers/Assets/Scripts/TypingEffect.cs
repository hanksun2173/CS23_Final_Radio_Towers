using System.Collections;
using UnityEngine;
using TMPro; // Import TextMeshPro

public class TypingEffect : MonoBehaviour
{
    public TMP_Text textMeshPro; // Reference to the TextMeshPro component
    public float typingSpeed = 0.1f; // Speed of typing in seconds

    private string fullText; // The complete text to be typed

    private void Start()
    {
        fullText = textMeshPro.text; // Store the full text
        textMeshPro.text = string.Empty; // Clear the text
        // Don't start typing automatically
    }

    private void OnEnable()
    {
        // Start typing when the GameObject becomes active
        if (!string.IsNullOrEmpty(fullText))
        {
            textMeshPro.text = string.Empty; // Clear the text
            StartCoroutine(TypeText()); // Start typing animation
        }
    }

    public void StartTyping()
    {
        // Public method to manually start typing
        if (!string.IsNullOrEmpty(fullText))
        {
            StopAllCoroutines(); // Stop any existing typing
            textMeshPro.text = string.Empty; // Clear the text
            StartCoroutine(TypeText()); // Start typing animation
        }
    }

    // Coroutine to simulate typing effect
    IEnumerator TypeText()
    {
        foreach (char letter in fullText)
        {
            textMeshPro.text += letter; // Append each letter to the text
            yield return new WaitForSeconds(typingSpeed); // Wait for the specified duration
        }
    }
}