﻿// (c) Simone Guggiari 2018

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
        b.transform.parent = transform;
        b.transform.position = shelfT.position + offsetH * currentCol + offsetV * currentRow;
        b.transform.eulerAngles = shelfT.eulerAngles + new Vector3(0, Random.Range(-angleVar, angleVar), 0);
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



	// queries
    Vector3 offsetH { get { return shelfT.right * increment; } }
    Vector3 offsetV { get { return shelfT.up * 2; } }
    int increment { get { return size < 4 ? 1 : 2; } }
    int numCols { get { return size < 4 ? 6 : 3; } }



	// other
	
}