// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

////////// DESCRIPTION //////////

public class Cart : NetworkBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    const int numPos = 6;

    // private
    List<List<int>>[] positions; // const
    bool[] free;
    List<Box> carrying;
    float lastDepositTimestamp;


    // references
    public Player Owner { get; private set; }
    public Transform[] posTransform;


    // --------------------- BASE METHODS ------------------

    void Start () {
        Owner = transform.parent.GetComponent<Player>();
        SetupPositions();
        carrying = new List<Box>();
        free = Enumerable.Repeat(true, numPos).ToArray(); // 6 times free
    }
	
	void Update () {
        
	}

    private void OnTriggerEnter(Collider other) {
        //if deposit, check & leave
        Deposit dep = other.GetComponent<Deposit>();
        if (dep != null) {
            TryDepositAll(dep);
        }
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void SetupPositions() {
        //tuples of where different sized boxes fit
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
        box.PickupBox(this);
        List<int> positions = FreePosition(box.packSize);
        foreach (int p in positions) free[p] = false;
        carrying.Add(box);
    }

    void TryDepositAll(Deposit dep) {
        //TODO cleanup
        bool atLeastOneFits = false;
        int numBoxes = carrying.Count;

        foreach (Box b in carrying) {
            if (b != null && b.packSize == dep.packSize) {
                atLeastOneFits = true;
                if (dep.HasSpace()) {
                    //actual deposit
                    Deposit(b, dep);
                }
                else {
                    Owner.Bubble.Speak(SpeechType.ShelfFull);
                }
            }
        }
        if (!atLeastOneFits && numBoxes > 0 && Time.time - lastDepositTimestamp > 2f) {
            Owner.Bubble.Speak(SpeechType.NotRightFit);
        }
        carrying.RemoveAll(b => b.Deposited);
    }

    void Deposit(Box box, Deposit dep) {
        foreach (int p in box.Positions) free[p] = true;
        box.DepositBox(dep);
        lastDepositTimestamp = Time.time;
        Owner.Bubble.Speak(SpeechType.BoxDeposit);
        AudioManager.Play("box_drop");
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
        return FreePosition(box) != null;
    }

    public Vector3 TransfFromPos(List<int> positions) {
        //simple average
        Vector3 result = Vector3.zero;
        foreach (int p in positions) result += posTransform[p].position;
        return result / positions.Count;
    }




    // other

}