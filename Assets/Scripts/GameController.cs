// VR Tetris
// - Catsync
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Transform blockHolder;
    public TetrisShape currentShape;

	public GameObject[] shapeTypes;

	public bool gameActive = true;

	// Use this for initialization
	void Start () {
        Debug.Log("Starting GameController");
		SpawnShape();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnShape()
	{
		if (!gameActive) {
			return;
		}
		// Span a random shape.
		//Debug.Log ("Spawning new shape...");
		int rnd = Random.Range (0, shapeTypes.Length);
		GameObject shape = Instantiate (shapeTypes [rnd]);
		shape.transform.position = transform.position;
		currentShape = shape.GetComponent<TetrisShape> ();
		shape.transform.parent = blockHolder;
	}

	public void GameOver() 
	{
		currentShape.isDropping = false;
		currentShape = null;
		gameActive = false;
		Debug.Log ("GAME OVER!");
		Destroy (this.gameObject);
	}
}
