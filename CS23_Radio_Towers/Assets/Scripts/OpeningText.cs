// OpeningText.cs
// Attach this script to any GameObject in your scene.
// Setup instructions at the bottom of this file.

using System.Collections;
using UnityEngine;
using TMPro;

public class OpeningText : MonoBehaviour
{
    [Header("Lines to display (one per entry)")]
    [TextArea(2, 5)]
    public string[] openingLines;

    [Header("Delay between lines (seconds)")]
    public float lineDelay = 2f;

    [Header("Typewriter speed (seconds per char)")]
    public float typewriterSpeed = 0.04f;

    [Header("Text UI Reference")]
    public TextMeshProUGUI textUI; // Assign in inspector

    private void Start()
    {
        if (textUI == null)
        {
            Debug.LogError("[OpeningText] Text UI is not assigned!");
            return;
        }
        if (openingLines == null || openingLines.Length == 0)
        {
            Debug.LogWarning("[OpeningText] No lines to display.");
            return;
        }
        StartCoroutine(DisplayLinesCoroutine());
    }

    private IEnumerator DisplayLinesCoroutine()
    {
        for (int i = 0; i < openingLines.Length; i++)
        {
            yield return StartCoroutine(TypeLine(openingLines[i]));
            yield return new WaitForSeconds(lineDelay);
        }
        // After last line, fade to TutorialLvl
        FadeManager fadeManager = FindObjectOfType<FadeManager>();
        if (fadeManager != null)
        {
            fadeManager.FadeToScene("TutorialLvl");
        }
        else
        {
            Debug.LogError("[OpeningText] FadeManager not found in scene!");
        }
    }

    private IEnumerator TypeLine(string line)
    {
        textUI.text = "";
        foreach (char c in line)
        {
            textUI.text += c;
            yield return new WaitForSeconds(typewriterSpeed);
        }
    }
}

/*
--- SETUP INSTRUCTIONS ---
1. In your Unity scene, create a Canvas (if you don't have one already).
2. Inside the Canvas, create a UI > Text - TextMeshPro object.
3. Resize and position the TextMeshProUGUI as desired for your opening text.
4. Create an empty GameObject (e.g., "OpeningTextManager") and attach this OpeningText script to it.
5. In the Inspector for the OpeningText component:
   - Drag your TextMeshProUGUI object into the "Text UI Reference" field.
   - Enter your lines of text in the "Lines to display" array (one per entry).
   - Set the "Delay between lines" to your desired value.
   - Set the "Typewriter speed" to your desired value (lower is faster).
6. Play the scene. The lines will appear one after another automatically, with a typewriter effect.
*/
