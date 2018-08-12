// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class Deposit : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public int size;
    public float angleVar = 10;

    // private
    int currentRow, currentCol;
    int numRows = 4;
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
        b.transform.position = shelfT.position + offsetH * currentCol + offsetV * currentRow;
        b.transform.eulerAngles =shelfT.eulerAngles + new Vector3(0, Random.Range(-angleVar, angleVar), 0);
        currentCol++;
        if (currentCol == numCols) {
            currentCol = 0;
            currentRow++;
            if (currentRow == numRows) full = true;
        }
    }

    public bool CanAcceptBox() {
        return !full;
    }

    public void Clear() {
        foreach (Box b in boxes) Destroy(b.gameObject);
        boxes.Clear();
        full = false;
        currentRow = currentCol = 0;
    }



    // queries
    Vector3 offsetH { get { return -transform.right * increment; } }
    Vector3 offsetV { get { return transform.up * 2; } }
    int increment { get { return size < 4 ? 1 : 2; } }
    int numCols { get { return size < 4 ? 6 : 3; } }
    float angle { get { return size == 2 ? 90 : 0; } }

    int SpacesAvailable() {
        return numRows * numCols;
    }

    public float PercentFilled() {
        return (float)boxes.Count / SpacesAvailable();
    }


	// other
	
}