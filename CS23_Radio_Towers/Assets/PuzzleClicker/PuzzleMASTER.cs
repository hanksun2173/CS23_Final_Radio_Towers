using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleMASTER : MonoBehaviour
{

	public PuzzleClicker[] thePieces; 
	public int numberClicks = 0; //track clicks

	public int winSpawnIndex = 1;

	public GameObject winCanvas; 
	//add timer?

    void Start()
    {
		winCanvas.SetActive(false);
        thePieces = FindObjectsOfType<PuzzleClicker>();
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

	public void MoveToNext ()
    {
		if (GameHandler.Instance != null)
            {
                // Mark the current tower as completed since player won
                GameHandler.MarkCurrentTowerCompleted();
                
                // Set spawn point for return to MainScene
                GameHandler.Instance.SetSpawnIndex(winSpawnIndex);
            }
        SceneManager.LoadScene("MainScene");
        
    }

}
