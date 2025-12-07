using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TowerWinLight : MonoBehaviour
{
    [Header("Tower Settings")]
    public int towerId; // The ID of the tower this light represents
    
    [Header("Light Component")]
    public Light2D spotLight; // Reference to the Light 2D component
    
    [Header("Colors")]
    public Color completedColor = Color.green; // Green when tower is completed
    public Color incompleteColor = Color.red; // Red when not completed
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get Light 2D component if not assigned
        if (spotLight == null)
            spotLight = GetComponent<Light2D>();
            
        // Wait a frame for GameHandler to be ready, then check tower completion
        StartCoroutine(DelayedLightUpdate());
    }
    
    System.Collections.IEnumerator DelayedLightUpdate()
    {
        // Wait longer for GameHandler to fully process tower completion
        yield return new WaitForSeconds(0.5f);
        UpdateLightColor();
        
        // If still not completed, try again after another delay
        yield return new WaitForSeconds(1f);
        UpdateLightColor();
    }

    void UpdateLightColor()
    {
        // Format tower ID to match GameHandler format (e.g., "Tower1")
        string towerIdString = "Tower" + towerId.ToString();
        
        // Check if this tower has been completed
        if (GameHandler.HasTowerBeenCompleted(towerIdString))
        {
            SetLightColor(completedColor);
        }
        else
        {
            SetLightColor(incompleteColor);
        }
    }
    
    void SetLightColor(Color color)
    {
        // Update spot light color
        if (spotLight != null)
        {
            spotLight.color = color;
        }
    }
    
    // Public method to refresh the light state (call this when tower completion changes)
    public void RefreshLightState()
    {
        UpdateLightColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
