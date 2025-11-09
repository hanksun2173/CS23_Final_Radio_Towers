using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public Transform[] points;
    private int i;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = points[0].position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, points[i].position) < 0.01f)
        {
            i++;
            if (i >= points.Length)
            {
                i = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

    // Unity message methods are case-sensitive: use OnCollisionEnter2D / OnCollisionExit2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if this platform is active and valid before parenting
            if (gameObject.activeInHierarchy && transform != null)
            {
                try
                {
                    // parent the player's transform to this platform so it moves together
                    collision.transform.SetParent(transform);
                    Debug.Log("[MovingPlatform] Player parented to moving platform");
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"[MovingPlatform] Could not parent player: {e.Message}");
                }
            }
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Immediately unparent the player safely without using coroutines
            SafelyUnparentPlayer(collision.transform);
        }
    }
    
    private void SafelyUnparentPlayer(Transform playerTransform)
    {
        // Check if the player transform still exists and is valid
        if (playerTransform != null)
        {
            try
            {
                // Check if the player is actually parented to this platform
                if (playerTransform.parent == transform)
                {
                    playerTransform.SetParent(null);
                    Debug.Log("[MovingPlatform] Player safely unparented from platform");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[MovingPlatform] Could not unparent player: {e.Message}");
            }
        }
    }
}
