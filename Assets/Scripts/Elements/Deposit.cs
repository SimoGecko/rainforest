// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

////////// DESCRIPTION //////////

public class Deposit : NetworkBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public int packSize;

    // private
    const float angleVar = 10;
    const int numRows = 3; // make sure this is updated

    int currentRow, currentCol;
    bool full;
    List<Box> boxes = new List<Box>();


    // references
    public Transform shelfT;
	
	
	// --------------------- BASE METHODS ------------------
	void Start () {
        currentRow = currentCol = 0;
	}
	
	void Update () {
        
	}

	
	
	// --------------------- CUSTOM METHODS ----------------
	
	
	// commands
    public void PositionBox(Box b) {
        if (full) return;
        boxes.Add(b);

        b.transform.parent = transform;
        b.transform.position = shelfT.position + OffsetH * currentCol + OffsetV * currentRow;
        b.transform.eulerAngles =shelfT.eulerAngles + new Vector3(0, Random.Range(-angleVar, angleVar), 0);

        IncreaseIndex();
        
    }

    void IncreaseIndex() {
        currentCol++;
        if (currentCol == NumCols) {
            currentCol = 0;
            currentRow++;
            if (currentRow == numRows) full = true;
        }
    }

    

    public void Clear() {
        foreach (Box b in boxes) Destroy(b.gameObject);
        boxes.Clear();
        full = false;
        currentRow = currentCol = 0;
    }



    // queries
    public bool HasSpace() {
        return !full;
    }

    public float PercentFilled() {
        return (float) boxes.Count / (numRows * NumCols);
    }

    Vector3 OffsetH { get { return -transform.right * Increment; } }
    Vector3 OffsetV { get { return transform.up * 2; } }
    int Increment { get { return packSize < 4 ? 1 : 2; } }
    int NumCols { get { return packSize < 4 ? 6 : 3; } }
    


	// other
	
}