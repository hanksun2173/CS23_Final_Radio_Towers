using UnityEngine;

public class SpinnyPlatform : MonoBehaviour
{
    public float rotateSpeed = 90f; 
    public float maxAngle = 45f; 
    
    private float currentAngle = 0f;
    private float direction = 1f; 
    private float startingRotation;
    
    void Start()
    {
        startingRotation = transform.eulerAngles.z;
    }

    void Update()
    {

        currentAngle += direction * rotateSpeed * Time.deltaTime;
        
        if (currentAngle >= maxAngle)
        {
            currentAngle = maxAngle;
            direction = -1f; 
        }
        else if (currentAngle <= -maxAngle)
        {
            currentAngle = -maxAngle;
            direction = 1f;
        }
        
        transform.rotation = Quaternion.Euler(0, 0, startingRotation + currentAngle);
    }

}
