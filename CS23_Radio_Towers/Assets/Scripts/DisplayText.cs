using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayText : MonoBehaviour
{
    [Header("Trigger Object")]
    public GameObject triggerObject; // Object that will trigger the text display
    
    [Header("Text Settings")]
    public float displayDuration = 3f; // How long to show text (0 = infinite)
    public bool hideOnStart = true; // Hide text at start
    
    private TextMeshProUGUI tmpText;
    private bool isDisplaying = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get TextMeshPro component
        tmpText = GetComponent<TextMeshProUGUI>();
        
        if (hideOnStart)
        {
            HideText();
        }
        
        // Add collision detection to the trigger object
        if (triggerObject != null)
        {
            TriggerDetector detector = triggerObject.GetComponent<TriggerDetector>();
            if (detector == null)
            {
                detector = triggerObject.AddComponent<TriggerDetector>();
            }
            detector.displayText = this;
        }
    }
    
    public void ShowText()
    {
        if (isDisplaying) return;
        
        isDisplaying = true;
        
        // Show the TextMeshPro element (typewriter animation will handle the text display)
        if (tmpText != null)
        {
            tmpText.gameObject.SetActive(true);
        }
        
        // Hide after duration if specified
        if (displayDuration > 0)
        {
            StartCoroutine(HideAfterDelay());
        }
    }
    
    public void HideText()
    {
        if (tmpText != null)
        {
            tmpText.gameObject.SetActive(false);
        }
        
        isDisplaying = false;
    }
    
    private System.Collections.IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        HideText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

// Helper component for collision detection
public class TriggerDetector : MonoBehaviour
{
    [HideInInspector]
    public DisplayText displayText;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && displayText != null)
        {
            displayText.ShowText();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && displayText != null)
        {
            displayText.ShowText();
        }
    }
}
