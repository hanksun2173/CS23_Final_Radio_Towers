// =============================
//   Debris Distance Alert
// =============================
// Shows an exclamation alert when the player is near debris.

using UnityEngine;

public class DebrisDistanceAlert : MonoBehaviour
{
    [Header("Alert Settings")]
    [SerializeField] private float alertDistance = 5f; // Distance threshold for alert
    [SerializeField] private GameObject exclamationMarkPrefab; // Assign prefab in inspector
    [SerializeField] private float alertDuration = 2f;

    private bool alertShown = false;

    // =============================
    //      Unity Methods
    // =============================

    /// <summary>
    /// Checks player distance and shows alert if within range.
    /// </summary>
    private void Update()
    {
        if (alertShown) return;
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < alertDistance)
        {
            ShowAlert(player.transform.position);
            alertShown = true;
        }
    }

    // =============================
    //      Alert Logic
    // =============================

    /// <summary>
    /// Instantiates the alert prefab above the player.
    /// </summary>
    private void ShowAlert(Vector3 playerPosition)
    {
        if (exclamationMarkPrefab != null)
        {
            Vector3 alertPos = new Vector3(transform.position.x, playerPosition.y + 3.5f, playerPosition.z);
            GameObject alert = Instantiate(exclamationMarkPrefab, alertPos, Quaternion.identity);
            Destroy(alert, alertDuration);
        }
    }
}