using UnityEngine;

public class DebrisIgnoreColliders : MonoBehaviour
{
    private void Awake()
    {
        // Just set as trigger and auto-destroy after 5 seconds - no collision detection at all
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.isTrigger = true;
        }
        

    }
}
