// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

////////// DESCRIPTION //////////


public class PlayerPickup : NetworkBehaviour {
    // --------------------- VARIABLES ---------------------

    // public


    // private
    List<Box> boxesInRange = new List<Box>();
    List<Button> buttonsInRange = new List<Button>();
    GameObject highlighted;

    // references
    Player player;


    // --------------------- BASE METHODS ------------------
    void Start () {
        player = GetComponent<Player>();
	}
	
	void Update () {
        if (!GameManager.Playing) return;

        SetHighlightColor();

        if (InputManager.instance.GetInteractInput(player.id)) {
            Tap();
        }
	}

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Box>() != null) {
            Box b = other.GetComponent<Box>();
            if(!boxesInRange.Contains(b))
                boxesInRange.Add(b);
        }
        else if (other.GetComponent<Button>() != null) {
            Button b = other.GetComponent<Button>();
            if (!buttonsInRange.Contains(b))
                buttonsInRange.Add(b);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<Box>() != null) boxesInRange.Remove(other.GetComponent<Box>());
        else if (other.GetComponent<Button>() != null) buttonsInRange.Remove(other.GetComponent<Button>());
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void SetHighlightColor() {
        if (highlighted != null) highlighted.GetComponentInChildren<MeshRenderer>().material = ElementManager.instance.normalMat;
        highlighted = ClosestInteractable();
        if (highlighted != null) highlighted.GetComponentInChildren<MeshRenderer>().material = ElementManager.instance.highlightMat;
    }

    public void Tap() {//either space or button
        FilterBoxes();

        Box closestBox = ClosestBox();
        Button closestButton = ClosestButton();

        //select closest not null to push
        if (closestBox != null && closestButton == null) {
            closestBox.Tap(player);
        }
        else if(closestBox == null && closestButton != null) {
            closestButton.Tap(player);
        }
        else if(closestBox != null && closestButton != null) {
            //must chose closest
            if (DistToMe(closestBox.transform) < DistToMe(closestButton.transform))
                closestBox.Tap(player);
            else
                closestButton.Tap(player);
        }
    }


    void FilterBoxes() {
        boxesInRange.RemoveAll(b => b == null);
        boxesInRange.RemoveAll(x => x.OnCart || x.Deposited);
    }


    // queries
    Button ClosestButton() {
        if(buttonsInRange.Count>0)
            return buttonsInRange.Aggregate((x, y) => DistToMe(x.transform) < DistToMe(y.transform) ? x : y);
        return null;
    }
    Box ClosestBox() {
        if (boxesInRange.Count > 0)
            return boxesInRange.Aggregate((x, y) => DistToMe(x.transform) < DistToMe(y.transform) ? x : y);
        return null;
    }

    GameObject ClosestInteractable() {//either space or button
        FilterBoxes();

        Box closestBox = ClosestBox();
        Button closestButton = ClosestButton();

        //select closest not null to push
        if (closestBox != null && closestButton == null) {
            return closestBox.gameObject;
        } else if (closestBox == null && closestButton != null) {
            return closestButton.gameObject;
        } else if (closestBox != null && closestButton != null) {
            //must chose closest
            if (DistToMe(closestBox.transform) < DistToMe(closestButton.transform))
                return closestBox.gameObject;
            else
               return closestButton.gameObject;
        }
        return null;
    }

    float DistToMe(Transform t) {
        return Vector3.SqrMagnitude(t.position - transform.position);
    }


    // other

}