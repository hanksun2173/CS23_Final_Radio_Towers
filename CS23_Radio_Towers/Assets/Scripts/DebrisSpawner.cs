using UnityEngine;

public class DebrisSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _debrisPrefab;

    [SerializeField]
    private float _minimumSpawnTime;

    [SerializeField]
    private float _maximumSpawnTime;


    private float _timeUntilSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        SetTimerUntilSpawn();
        
    }

    // Update is called once per frame
    void Update()
    {
        _timeUntilSpawn -= Time.deltaTime;
        if (_timeUntilSpawn <= 0f)
        {
            Instantiate(_debrisPrefab, transform.position, Quaternion.identity);
            SetTimerUntilSpawn();
        }

    }
    
    private void SetTimerUntilSpawn()
    {
        _timeUntilSpawn = Random.Range(_minimumSpawnTime, _maximumSpawnTime);
    }
}
