using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BG_Parallax : MonoBehaviour {

      private float length, startposX;
      //startposY;
      public GameObject cam;
      public float parallaxEffect;
      //parallaxUpEffect;

      void Start(){
                  startposX = transform.position.x;
                  //startposY = transform.position.y;
                  var sr = GetComponent<SpriteRenderer>();
                  if (sr != null)
                  {
                        length = sr.bounds.size.x;
                  }
                  else
                  {
                        Debug.LogWarning($"[BG_Parallax] No SpriteRenderer found on {gameObject.name}, parallax may not work correctly.");
                        length = 0f;
                  }
      }

      void FixedUpdate(){
            float temp = (cam.transform.position.x * (1 - parallaxEffect));
            float distX = (cam.transform.position.x * parallaxEffect);
            //float distY = (cam.transform.position.y * parallaxUpEffect);
            transform.position = new Vector3(startposX + distX, transform.position.y, transform.position.z);
            //transform.position = new Vector3(startposX + distX, startposY + distY, 1);
            if (temp > startposX + length){
                  startposX += length;
            }
            else if (temp < startposX - length){
                  startposX -= length;
            }
      }

}
