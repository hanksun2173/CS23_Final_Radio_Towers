using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudManager : MonoBehaviour
{

	public GameObject[] clouds;

	private GameObject target;
	public float camSpeed = 4.0f;

	public float leftOffset= 10f;

	float theCloudTmer = 1f;

	void Start(){
		target = GameObject.FindWithTag("Player");
	}

	void FixedUpdate () {
		//follow:
		Vector2 leftOfPlayer = new Vector2(target.transform.position.x - leftOffset, target.transform.position.y);

		Vector2 pos = Vector2.Lerp ((Vector2)transform.position, leftOfPlayer, camSpeed * Time.fixedDeltaTime);
		transform.position = new Vector3 (pos.x, pos.y, transform.position.z);

		//timer
		theCloudTmer -= 0.01f; 
		if (theCloudTmer <= 0)
		{
			SpawnCloud();
			theCloudTmer = Random.Range(0.8f,3.5f);
		}
	}

	void SpawnCloud()
	{
		float Ypos = Random.Range(transform.position.y -5, transform.position.y + 5);
		Vector2 spawnLocation = new Vector2(transform.position.x, Ypos);

		int cloudNum = Random.Range(0, clouds.Length);
		Instantiate (clouds[cloudNum], spawnLocation, Quaternion.identity);
	}


}
