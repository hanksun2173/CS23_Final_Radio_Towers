using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PuzzleMASTER : MonoBehaviour
{
	public GameObject displayText; 

	public PuzzleClicker[] thePieces; 
	public int numberClicks = 0; //track clicks

	public int winSpawnIndex = 1;

	public GameObject winCanvas; 
	//add timer?
	// Reference to the AudioSource for radio messages
	public AudioSource radioAudioSource;
	// Assign the radio message clip in the inspector
	public AudioClip radioMessageClip;

    void Start()
    {
		winCanvas.SetActive(false);
		displayText.SetActive(false);
        thePieces = FindObjectsOfType<PuzzleClicker>();
		Debug.Log("Found " + thePieces.Length + "pieces");
    }

	//whenever a puzzlePiece is clicked: 
    public void ResetAll(){
		for(int i=0; i< thePieces.Length; i++){
				thePieces[i].TurnOffPower();
		}
		numberClicks++;
		//Debug.Log("Clicks so far: " + numberClicks);
	}


	public void PuzzleComplete(){
		winCanvas.SetActive(true);
		

		// Debug.Log("YOU DID IT! PUZZLE COMPLETE in " + numberClicks + " clicks!");
	}

	public void DisplayRadioMessage()
    {
		displayText.SetActive(true);
		// Play radio message audio if assigned
		if (radioAudioSource != null && radioMessageClip != null)
		{
			radioAudioSource.clip = radioMessageClip;
			radioAudioSource.Play();
		}
    }

	public void MoveToNext ()
    {
		if (GameHandler.Instance != null)
            {
                // Mark the current tower as completed since player won
                Debug.Log("[PuzzleMASTER] Marking current tower as completed...");
                GameHandler.MarkCurrentTowerCompleted();
                Debug.Log("[PuzzleMASTER] Tower marked complete!");
                
                // Set spawn point for return to MainScene
                GameHandler.Instance.SetSpawnIndex(winSpawnIndex);
            }
            else
            {
                Debug.LogError("[PuzzleMASTER] GameHandler.Instance is null!");
            }
        SceneManager.LoadScene("MainScene");
        
    }

}
