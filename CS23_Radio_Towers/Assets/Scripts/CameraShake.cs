using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour{

       public Transform shakeTarget;
       public float durationTime = 2.5f;
       public float magnitude = 0.3f;
       public float shakeInterval = 30f;
       private float shakeTimer = 0f;

       void Update(){
              if (shakeInterval > 0f)
              {
                     shakeTimer += Time.deltaTime;
                     if (shakeTimer >= shakeInterval)
                     {
                            shakeTimer -= shakeInterval;
                            ShakeCamera(durationTime, magnitude);
                     }
              }
              // if (Input.GetKeyDown(KeyCode.P)){
              //       ShakeCamera(durationTime,magnitude);
              // }
       }

       void Awake()
       {
              if (shakeTarget == null && Camera.main != null)
              {
                     shakeTarget = Camera.main.transform;
              }
       }

       //use this to call from another script, like when the player gets hit
       public void ShakeCamera(float durationTime2, float magnitude2){
              // Safety: don't shake a transform that is a parent/ancestor of the player — that would move the player
              GameObject player = GameObject.FindGameObjectWithTag("Player");
              if (player != null && shakeTarget != null) {
                     // if the player is a child of the shake target, shaking the target will move the player
                if (player.transform.IsChildOf(shakeTarget) || player.transform == shakeTarget) {
                return;
                }
              }

              if (shakeTarget == null) {
                     return;
              }

              StartCoroutine(ShakeMe(durationTime2, magnitude2));
       }

       //the screenshake!
       public IEnumerator ShakeMe(float durationTime, float magnitude){
              Vector3 origPos = shakeTarget.localPosition;
              float elapsedTime = 0.0f;

              while (elapsedTime < durationTime){
                     float sX = Random.Range(-1f, 1f) * magnitude;
                     float sY = Random.Range(-1f, 1f) * magnitude;

                     shakeTarget.localPosition = new Vector3((origPos.x+sX), (origPos.y+sY), origPos.z);
                     elapsedTime += Time.deltaTime;
                     yield return null;
              }
              shakeTarget.localPosition = origPos;
       }

}