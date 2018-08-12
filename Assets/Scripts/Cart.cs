// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class Cart : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public


    // private
    List<List<int>>[] positions; // const
    bool[] free;
    List<Box> carrying;


    // references
    public static Cart instance;
    public Transform[] posTransform;


    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
    }

    void Start () {
        carrying = new List<Box>();
        SetupPositions();
        free = new bool[6];
        for (int i = 0; i < 6; i++) {
            free[i] = true;
        }
	}
	
	void Update () {
        
	}

    private void OnTriggerEnter(Collider other) {
        //if deposit, check & leave
        Deposit dep = other.GetComponent<Deposit>();
        if (dep != null) {
            foreach(Box b in carrying) {
                if (b != null) {
                    if (b.size == dep.size) {
                        Deposit(b, dep);
                    }
                }
            }
        }
        carrying.RemoveAll(b => b.Deposited);
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void SetupPositions() {
        positions = new List<List<int>>[5];
        positions[1] = new List<List<int>> {
            new List<int> {0},
            new List<int> {3},
            new List<int> {1},
            new List<int> {4},
            new List<int> {2},
            new List<int> {5}
        };
        positions[2] = new List<List<int>> {
            new List<int> {0,3},
            new List<int> {1,4},
            new List<int> {2,5},
            new List<int> {0,1},
            new List<int> {3,4},
            new List<int> {1,2},
            new List<int> {4,5}
        };
        positions[4] = new List<List<int>> {
            new List<int> {1,2,4,5},
            new List<int> {0,1,3,4}
        };
    }

    public void Pickup(Box box) {
        List<int> positions = FreePosition(box.size);
        foreach (int p in positions) free[p] = false;
        carrying.Add(box);
    }

    void Deposit(Box box, Deposit dep) {
        foreach (int p in box.positions) free[p] = true;
        //carrying.Remove(box);
        box.DepositBox(dep);
    }


	// queries
    public List<int> FreePosition(int box) {
        //check them in order, if not found return null
        for (int i = 0; i < positions[box].Count; i++) { // position
            bool result = true;
            foreach (int p in positions[box][i]) result = result && free[p];
            if (result) return positions[box][i];
        }
        return null;
    }

    public bool HasSpaceFor(int box) {
        //box is either 1, 2, 4
        return FreePosition(box) != null;
        /*
        switch (box) {
            case 1: return free[0, 0] || free[0, 1] || free[0, 2] || free[1, 0] || free[1,1] || free[1,2];
            case 2: return (free[0, 0] && free[1, 0]) || (free[0, 1] && free[1, 1]) || (free[0, 2] && free[1, 2]) || // vertical (3)
                    (free[0, 0] && free[0, 1]) || (free[1, 0] && free[1, 1]) || (free[0, 1] && free[0, 2]) || (free[1, 1] && free[1, 2]); // horizontal (4)
            case 4: return (free[0, 0] && free[0, 1] && free[1, 0] && free[1, 1]) || (free[0, 1] && free[0, 2] && free[1, 1] && free[1, 2]);
            default: return false;
            case 1: return free[0] || free[1] || free[2] || free[3] || free[4] || free[5];
            case 2: return (free[] && free[]) || (free[] && free[]) || (free[] && free[]) ||
                    (free[] && free[]) || (free[] && free[]) || (free[] && free[]) || (free[] && free[]);
            case 4: return (free[0] && free[1] && free[3] && free[4]) || (free[1] && free[2] && free[4] && free[5]);
        }*/
    }

    public Vector3 TransfFromPos(List<int> positions) {
        Vector3 result = Vector3.zero;
        foreach (int p in positions) result += posTransform[p].position;
        return result / positions.Count;
    }




    // other

}