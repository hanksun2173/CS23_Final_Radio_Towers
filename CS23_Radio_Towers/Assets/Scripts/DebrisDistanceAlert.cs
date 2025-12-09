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
            Vector3 alertPos = playerPosition + new Vector3(0, 2f, 0); // 2 units above player
            GameObject alert = Instantiate(exclamationMarkPrefab, alertPos, Quaternion.identity);
            Destroy(alert, alertDuration);
        }
    }
}