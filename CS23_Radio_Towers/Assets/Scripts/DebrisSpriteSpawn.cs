using UnityEngine;

public class DebrisSpriteSpawn : MonoBehaviour
{
    [Header("Debris Sprite Configuration")]
    [Tooltip("Array of possible debris sprites to randomly choose from")]
    public Sprite[] debrisSprites;
    
    [Tooltip("The SpriteRenderer component of this debris object")]
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Assign a random sprite from the array
        AssignRandomSprite();
    }
    
    private void AssignRandomSprite()
    {
        // Check if we have sprites in the array and a valid SpriteRenderer
        if (debrisSprites != null && debrisSprites.Length > 0 && spriteRenderer != null)
        {
            // Pick a random index from the debris sprites array
            int randomIndex = Random.Range(0, debrisSprites.Length);
            
            // Assign the randomly selected sprite to the SpriteRenderer
            spriteRenderer.sprite = debrisSprites[randomIndex];
            
            Debug.Log($"[DebrisSpriteSpawn] Assigned random debris sprite {randomIndex} ({debrisSprites[randomIndex].name})");
        }
        else
        {
            // Log warnings for missing components or empty array
            if (spriteRenderer == null)
                Debug.LogWarning("[DebrisSpriteSpawn] No SpriteRenderer component found on this GameObject!");
            
            if (debrisSprites == null || debrisSprites.Length == 0)
                Debug.LogWarning("[DebrisSpriteSpawn] No debris sprites assigned in the array!");
        }
    }
    
    // Public method to reassign sprite (useful if you want to change it during runtime)
    public void RandomizeSprite()
    {
        AssignRandomSprite();
    }
}
