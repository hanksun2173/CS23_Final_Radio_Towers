using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PuzzleClicker : MonoBehaviour {

	private bool selected;
	public SpriteRenderer[] theSquares;
	public Color makeWhite = new Color(2.5f, 2.5f, 2.5f, 1);
	public Color makeGreen = new Color(1.5f, 2.5f, 1.5f, 1);
	public Color makeYellow = new Color(2.5f, 1.5f, 0.5f, 1);

	private bool canTurn = true;
	int currentRotation = 0;
	float[] rotations = {0f, -90f, 180f, 90f};

	public PuzzleConnector[] myConnectors;

	public bool isPowered = false;
	public bool isPowerStart = false;
	public bool isEnd = false;
	public bool isBomb = false;

	// Store the original color of each square
    private Color[] originalColors;

	void Start(){
		// Cache the original color of each square
        if (theSquares != null)
        {
            originalColors = new Color[theSquares.Length];
            for (int i = 0; i < theSquares.Length; i++)
            {
                if (theSquares[i] != null)
                    originalColors[i] = theSquares[i].color;
            }
        }
	}

	void Update () {
		if (!isPowerStart){
			IsPieceConnected();
		}
		if (Input.GetMouseButtonUp (0)) {
				canTurn = true;
				if (isPowered){
					SetAllSquaresColor(makeGreen);
				} 
				// else {
				//     SetAllSquaresColor(makeWhite);
				// }
		}
	}

	void OnMouseOver(){
		// theSquare.color = makeGreen;
		if (canTurn){
			if (Input.GetMouseButtonDown (0)) {
			
				if (currentRotation <=2){
					currentRotation++;
				}
				else {
					currentRotation= 0;
				}
				RotatePiece();
			}
			if (Input.GetMouseButtonDown(1)){
				if (currentRotation >=1){
					currentRotation--;
				}
				else {
					currentRotation= 3;
				}
				RotatePiece();
			}
    	}	 
	}

	void RotatePiece(){
		Vector3 myRotation = new Vector3(0,0,rotations[currentRotation]);
		transform.localEulerAngles = myRotation; 
		canTurn = false;
		GetComponentInParent<PuzzleMASTER>().ResetAll();
	}

	void OnMouseExit(){
		if (isPowered && !isEnd && !isBomb){
			SetAllSquaresColor(makeGreen);
		} 
		// else {
		//     SetAllSquaresColor(makeWhite);
		// }
	}

	void IsPieceConnected(){
		
		int poweredConnectors = 0;
		for (int i = 0; i < myConnectors.Length; i++){
			if (myConnectors[i].isConnectedToPower==true){
				poweredConnectors++;
			}
		}
		if (poweredConnectors > 0 && !isEnd && !isBomb){
			isPowered= true;
			SetAllSquaresColor(makeGreen);
		} else{
			isPowered= false;
			ResetAllSquaresToOriginal();
		} 
		/*
		if (!isPowerStart && !isEnd){
			if (myConnectors[0].isConnectedToPower==true || myConnectors[1].isConnectedToPower==true){
				isPowered = true;
				theSquare.color = makeGreen;
			}else{
				isPowered = false;
			}
		}
		*/
	}

	//reset power (command sent by PuzzleMASTER)
	public void TurnOffPower(){
		Debug.Log("Hello we should be turning stuff off");
		if (isPowerStart == false){
			isPowered = false;
			for (int i = 0; i < myConnectors.Length; i++){
				myConnectors[i].isConnectedToPower = false;
				Debug.Log("Power off");
			}
			IsPieceConnected();
		}
	} 

	//End Complete:
	public void EndYellow(){
		SetAllSquaresColor(makeGreen);
		StartCoroutine(EndYellowComplete());
	}
	IEnumerator EndYellowComplete(){
		yield return new WaitForSeconds(0.4f);
		SetAllSquaresColor(makeWhite);
		yield return new WaitForSeconds(0.2f);
		SetAllSquaresColor(makeGreen);
		yield return new WaitForSeconds(0.4f);
		SetAllSquaresColor(makeWhite);
		yield return new WaitForSeconds(0.2f);
		SetAllSquaresColor(makeGreen);
		yield return new WaitForSeconds(0.4f);
		SetAllSquaresColor(makeWhite);
		yield return new WaitForSeconds(0.2f);
		SetAllSquaresColor(makeGreen);
	}


	//If a bomb is triggered:
	public void EndRed(){
		SetAllSquaresColor(makeGreen); //I justst this to rd fr this bomb piece
		StartCoroutine(BombDestroy());
	}
	IEnumerator BombDestroy(){
		yield return new WaitForSeconds(0.2f);
		SetAllSquaresColor(makeWhite);
		yield return new WaitForSeconds(0.2f);
		SetAllSquaresColor(makeGreen);
		yield return new WaitForSeconds(0.2f);
		SetAllSquaresColor(makeWhite);
		yield return new WaitForSeconds(0.2f);
		SetAllSquaresColor(makeGreen);
		yield return new WaitForSeconds(0.2f);
		SetAllSquaresColor(makeWhite);
		yield return new WaitForSeconds(0.2f);
		SetAllSquaresColor(makeGreen);
		Destroy(gameObject);
	}

	void SetColorToSquares(Color color) {
		foreach (var square in theSquares) {
			square.color = color;
		}
	}

	// Helper to set all squares to a color
    private void SetAllSquaresColor(Color color)
    {
        if (theSquares != null)
        {
            foreach (var sr in theSquares)
            {
                if (sr != null)
                    sr.color = color;
            }
        }
    }

    // Helper to reset all squares to their original color
    private void ResetAllSquaresToOriginal()
    {
        if (theSquares != null && originalColors != null)
        {
            for (int i = 0; i < theSquares.Length; i++)
            {
                if (theSquares[i] != null)
                    theSquares[i].color = originalColors[i];
            }
        }
    }
}