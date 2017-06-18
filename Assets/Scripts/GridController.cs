using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRow
{
    public Transform[] col = new Transform[10];
}

public class GridController : MonoBehaviour {

    public static int numRows = 20;
    public static int numCols = 10;

    public GridRow[] gameGridRow = new GridRow[20];

    public void printRow(int y)
    {
		string str = "";
		for (int x = 0; x < numCols; x++) {
			Transform block = GetBlockAtGridPos (x, y);
			if (block == null) {
				str += "_";
			} else {
				str += "X";
			}
		}
		Debug.Log("GridRow: "+ y + " |" + str + "|" );

    }
		
    void Awake () {
        for(int i=0; i < numRows; i++)
        {
            gameGridRow[i] = new GridRow();
        }
    }

	#region GRID

	public Transform GetBlockAtGridPos(Vector2 pos)
	{
		return gameGridRow [(int)pos.y].col [(int)pos.x];
	}

	public Transform GetBlockAtGridPos(int x, int y)
	{
		return gameGridRow [y].col [x];
	}

	public void SetBlockAtGridPos(Vector2 pos, Transform block)
	{
		gameGridRow [(int)pos.y].col [(int)pos.x] = block;
	}

	public void SetBlockAtGridPos (int x, int y, Transform block)
	{
		//Debug.Log ("SetBlockAtGridPosXY");
		gameGridRow [y].col [x] = block;
	}

	public bool IsInsideGrid(Vector2 pos) {
		int x = (int)pos.x;
		int y = (int)pos.y;
		return (x >= 0 && x < numCols && y >= 0 && y < numRows);
	}


    public bool IsValidGridPosition(Transform shape)
    {
		foreach (Transform child in shape) {
			if (child.gameObject.tag.Equals ("Block")) {
				Vector2 gridPos = GridPosition (child.position);
				if (!IsInsideGrid (gridPos)) {
					return false;
				}

				Transform block = GetBlockAtGridPos (gridPos);
				if (block != null && block.parent != shape) {
					return false;
				}
			}
		}
        return true;
    }



    // Update Game Grid with new position for shape
    public void UpdateGrid(Transform shape)
    {
        //Debug.Log("Updating Grid...");
        // Remove this shape from its old grid positions
        for(int y = 0; y < numRows; y++)
        {
            for(int x = 0; x < numCols; x++)
            {
				//Debug.Log ("Scanning: "+ x + " , " + y);
				Transform block = GetBlockAtGridPos(x, y);
                if(block != null && block.parent == shape)
                {
					
					SetBlockAtGridPos (x, y, null);
                }
            }
        }
        // Add this shape to current grid positions
        foreach (Transform child in shape)
        {
            if(child.gameObject.tag.Equals("Block"))
            {
                Vector2 gridPos = GridPosition(child.position);
                gameGridRow[(int)gridPos.y].col[(int)gridPos.x] = child;
                //Debug.Log(string.Format("Found {0}->{1} at row {2} col {3}", shape.name, child.name, gridPos.y, gridPos.x));
            }
        }
    }

	public void GridPositionOfShape(Transform shape) 
	{
		foreach (Transform child in shape) {
			if (child.gameObject.tag.Equals ("Block")) {
				Vector2 gridPos = GridPosition (child.position);
				Debug.Log(string.Format("Found {0}->{1} at row {2} col {3}", shape.name, child.name, gridPos.y, gridPos.x));
				Debug.Log (child.position.y + " , " + child.position.x);
			}
		}
	}
    private Vector2 GridPosition(Vector2 vec)
    {
        return new Vector2(Mathf.Round(vec.x), Mathf.Round(vec.y));
    }

	#endregion

	#region ROWS

	public bool IsRowFull(int y)
	{
		for (int x = 0; x < numCols; x++) {
			Transform block = GetBlockAtGridPos (x, y);
			if (block == null) {
				return false;
			}
		}
		return true;
	}

	public void ClearRow(int y)
	{
		for (int x = 0; x < numCols; x++) {
			Transform block = GetBlockAtGridPos (x, y);
			if (block != null) {
				Destroy (block.gameObject);
				SetBlockAtGridPos (x, y, null);
			}
		}
	}

	public void DropRow(int y) {
		for (int x = 0; x < numCols; x++) {
			Transform block = GetBlockAtGridPos (x, y);
			if (block != null) {
				SetBlockAtGridPos (x, y - 1, block);
				SetBlockAtGridPos (x, y, null);

				block.position += Vector3.down;
			}
		}
	}

	public void DropRowsAbove(int y)
	{
		for (int i = y; i < numRows; i++) {
			DropRow (i);
		}
	}

	IEnumerator ClearCompletedRows()
	{
		//Debug.Log ("Clearing completed rows...");
		// Delete full rows, and move other rows down.
		for (int y = 0; y < numRows; y++) {
			if (IsRowFull (y)) {
				ClearRow (y);
				DropRowsAbove (y + 1);
				--y;
				yield return new WaitForSeconds (0.8f);
			}
		}

		// Destroy any empty shapes.
		foreach (Transform shape in Controllers.Game.blockHolder) {
			// Shapes contain blocks and a pivot point
			if (shape.childCount <= 1) {
				Destroy (shape.gameObject);
			}
		}

		Controllers.Game.SpawnShape ();

		yield break;
	}
	#endregion

	public void PlacedShape() 
	{
		StartCoroutine (ClearCompletedRows ());
	}

}
