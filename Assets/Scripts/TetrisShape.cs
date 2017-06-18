using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShapeType
{
    I,
    T,
    O,
    L,
    J,
    S,
    Z
}

public class TetrisShape : MonoBehaviour {

    public ShapeType type;

	public Transform rotationPivot;
	public float dropInterval = 0.8f;
    public float fastDropInterval = 0.01f;
    public bool isDropping = true;


    private float lastDrop = 0;

	// Use this for initialization
	void Start () {
		if (!Controllers.Grid.IsValidGridPosition (transform)) {
			Controllers.Game.GameOver ();
		} else {
			Controllers.Grid.UpdateGrid (transform);
		}

	}
	
	// Update is called once per frame
	void Update () {
        ShapeUpdate();
	}

    public void ShapeUpdate()
    {
        if(!isDropping)
        {
            return;
        }
        //Debug.Log(Time.time + " / " + lastFall + " / " + dropInterval );
        if(Time.time - lastDrop >= dropInterval)
        {
            // Update position
            transform.position += Vector3.down;

            // If the new position is valid, update the grid, otherwise revert the position.
           if(Controllers.Grid.IsValidGridPosition(transform))
            {
                Controllers.Grid.UpdateGrid(transform);
//				if (type == ShapeType.I) {
//					Controllers.Grid.GridPositionOfShape (transform);
//				}
            } else
            {
                transform.position -= Vector3.down;
                isDropping = false;
				Controllers.Grid.PlacedShape ();
            }

            lastDrop = Time.time;
        }


    }

	public void DropShape()
	{
		dropInterval = fastDropInterval;
	}

	public void RotateShapeClockwise() 
	{
		RotateShape (true);
	}

	public void RotateShapeCounterClockwise()
	{
		RotateShape (false);
	}

	public void RotateShape(bool clockwise)
	{
		float rotationDegree = (clockwise) ? 90.0f : -90.0f;

		transform.RotateAround (rotationPivot.position, Vector3.forward, rotationDegree);

		// Check if rotation is valid.
		if (Controllers.Grid.IsValidGridPosition (transform)) {
			Controllers.Grid.UpdateGrid (transform);
		} else {
			transform.RotateAround (rotationPivot.position, Vector3.forward, -rotationDegree);
		}
			
	}

	public void MoveHorizontal(Vector2 direction)
	{
		float delta = (direction.Equals (Vector2.right)) ? 1.0f : -1.0f;
		Vector3 vec = new Vector3 (delta, 0, 0);
		transform.position += vec;

		if (Controllers.Grid.IsValidGridPosition (transform)) {
			Controllers.Grid.UpdateGrid (transform);
		} else {
			transform.position -= vec; 
		}
	}

}
