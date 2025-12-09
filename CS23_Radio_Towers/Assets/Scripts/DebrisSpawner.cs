using UnityEngine;

public class DebrisSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _debrisGameObject;

    [SerializeField]
    private float _minimumSpawnTime = 5f; // Much slower spawning

    [SerializeField]
    private float _maximumSpawnTime = 10f;

    [SerializeField]
    private int maxDebrisCount = 10; // Limit total debris in scene

    private float _timeUntilSpawn;

    void Awake()
    {
        SetTimerUntilSpawn();
    }

    void Update()
    {
        _timeUntilSpawn -= Time.deltaTime;
        if (_timeUntilSpawn <= 0f)
        {
            if (_debrisGameObject != null)
            {
                GameObject newDebris = Instantiate(_debrisGameObject, transform.position, Quaternion.identity);
            }
            SetTimerUntilSpawn();
        }
    }
    
    private void SetTimerUntilSpawn()
    {
        _timeUntilSpawn = Random.Range(_minimumSpawnTime, _maximumSpawnTime);
    }
}
