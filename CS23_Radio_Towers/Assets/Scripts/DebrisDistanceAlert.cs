using UnityEngine;

public class DebrisDistanceAlert : MonoBehaviour
{
    [SerializeField]
    private float alertDistance = 5f; // Distance threshold for alert
    [SerializeField]
    private GameObject exclamationMarkPrefab; // Assign prefab in inspector
    [SerializeField]
    private float alertDuration = 2f;
    private bool alertShown = false;

    void Update()
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

    private void ShowAlert(Vector3 playerPosition)
    {
        if (exclamationMarkPrefab != null)
        {
            // Alert appears at debris's x, player's y + offset, player's z
            Vector3 alertPos = new Vector3(transform.position.x, playerPosition.y + 3.5f, playerPosition.z);
            GameObject alert = Instantiate(exclamationMarkPrefab, alertPos, Quaternion.identity);
            Destroy(alert, alertDuration);
        }
    }
}