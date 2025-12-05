using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed = 0.5f;
    private Vector2 destination;
    public int distance = 2500; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        destination = new Vector2(transform.position.x + 2500, transform.position.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       Vector2 pos = Vector2.Lerp((Vector2)transform.position, destination, speed * Time.fixedDeltaTime);
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        
        float distancetoEnd = Vector3.Distance(transform.position, destination);    
        if (distancetoEnd <= 5)
        {
            Destroy(gameObject);
        }
    }
}
