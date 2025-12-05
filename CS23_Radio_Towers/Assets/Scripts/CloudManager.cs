using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class CloudManager : MonoBehaviour
{

    public GameObject[] clouds;
      private GameObject target;
      public float camSpeed = 4.0f;
      Vector3 offset;

      void Start(){
            target = GameObject.FindWithTag("Player");
      }

      void FixedUpdate () {
            Vector3 targetPos = target.transform.position + offset;
            Vector2 pos = Vector2.Lerp((Vector2)transform.position, (Vector2)targetPos, camSpeed * Time.fixedDeltaTime);
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
      }
}

