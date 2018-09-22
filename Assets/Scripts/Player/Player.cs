// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

////////// DESCRIPTION //////////

[RequireComponent(typeof(CharacterController))]
public class Player : NetworkBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public float speed = 18;
    public float sprintMultiplier = 1.4f;
    public float angularSpeed = 500;
    public float pickupDist = 8f;
    public int id; // localid, netid


    // private
    Vector3 inp;
    bool rotateInput = true;
    int score;

    bool sprintInput;
    float animHelloTime;


    // references
    CharacterController cc;
    Animator anim;
    NetworkAnimator netanim;
    AudioSource feetSound;

    public Text controlText;
    public Transform pickupCenter;

    public Cart Cart { get; private set; }
    public ComicBubble Bubble { get; private set; }


    // --------------------- BASE METHODS ------------------

    public override void OnStartLocalPlayer() {
        //set id
        id = ElementManager.NumPlayers;
        ElementManager.instance.AddPlayer(this);

        //set color
        Texture2D texcolors = ElementManager.instance.playerTextures[id];
        GetComponentInChildren<SkinnedMeshRenderer>().material.mainTexture = texcolors;
        GetComponentInChildren<MeshRenderer>().material.mainTexture = texcolors;
    }


    void Start () {
        if (!isLocalPlayer) return;//Destroy(this);

        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        netanim = GetComponent<NetworkAnimator>();
        feetSound = GetComponent<AudioSource>();
        Cart = GetComponentInChildren<Cart>();
        Bubble = GetComponentInChildren<ComicBubble>();

        GameManager.instance.OnPlay += AnimStart;
        GameManager.instance.OnGameover += AnimEnd;
        animHelloTime = Time.time + Random.Range(2f, 4f);
    }
	
	void Update () {
        if (!isLocalPlayer) return;//Destroy(this);


        if (Input.GetKeyDown(KeyCode.Tab))
            ToggleControlType();

        inp = InputManager.instance.GetInput(InputId).To3().normalized;
        sprintInput = InputManager.instance.GetSprintInput(InputId);

        if (GameManager.Playing) {
            Move();
            ProjectToArea();
        }

        DealWithAnimations();
        DealWithSound();
	}

    private void FixedUpdate() {

    }



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void ToggleControlType() {
        rotateInput = !rotateInput;
        string textString = "control: " + (rotateInput ? "camera" : "player") + "-relative\nTAB to change";
        if(controlText!=null)
            controlText.text = textString;
    }


    void Move() {
        // Overcooked style
        Vector3 inputRotated = rotateInput ? (Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * inp) : inp;

        float signedAngle = Vector3.SignedAngle(transform.forward, inputRotated, Vector3.up);
        float rotAmount = angularSpeed * Time.deltaTime;
        rotAmount = Mathf.Min(rotAmount, Mathf.Abs(signedAngle)); // clamp it if it's smaller
        transform.Rotate(Vector3.up * rotAmount * Mathf.Sign(signedAngle));

        float angle = Vector3.Angle(transform.forward, inputRotated);
        //project movement onto direction
        if (angle <= 90) {
            float dot = Vector3.Dot(transform.forward, inputRotated); // projection for smooth results
            float runspeed = speed * (sprintInput ? sprintMultiplier : 1);
            cc.Move(transform.forward * runspeed * inputRotated.magnitude * dot * Time.deltaTime);
        }
    }

    void ProjectToArea() {
        //to avoid bugs
        Vector3 temp = transform.position;
        Rect area = ElementManager.instance.playArea;
        temp.x = Mathf.Clamp(temp.x, area.xMin, area.xMax);
        temp.z = Mathf.Clamp(temp.z, area.yMin, area.yMax);
        temp.y = 0;
        transform.position = temp;
    }


    void DealWithAnimations() {
        if (GameManager.Menu && Time.time > animHelloTime) {
            netanim.SetTrigger("hello");
            animHelloTime = Time.time + Random.Range(4f, 8f);
        }
        if (GameManager.Playing) {
            bool running = inp.magnitude > .1f;
            anim.SetBool("running", running);
        }
    }

    void AnimStart() { netanim.SetTrigger("start"); }
    void AnimEnd()   { netanim.SetTrigger("end"); }
    public void AnimButton() { netanim.SetTrigger("button"); }


    void DealWithSound() {
        feetSound.volume = inp.magnitude / 20;
        feetSound.pitch = Mathf.Lerp(feetSound.pitch, Random.Range(.8f, 1.2f), Time.deltaTime / 2);
    }


    // queries
    public bool CloseEnough(Transform t) {
        Vector3 v = pickupCenter.position - t.transform.position;
        v.y = 0;
        return Vector3.SqrMagnitude(v) <= pickupDist * pickupDist;
    }
    public int InputId { get { return 1; } }



    // other
    private void OnDrawGizmos() {
        /*
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, inputRotated*3);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + Vector3.up, moveDirection*3);
        */
    }





    //------------------------------------------------- OLD MOVEMENT METHODS -----------------------------------------------------------------
    /*
    void Move2() {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        Vector3 displacement = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * input.normalized; // camera
        
        //float dirZ = Vector3.Dot(transform.forward, input);
        //float dirX = Vector3.Dot(transform.right, input);

        float angle = input.magnitude>0? Vector3.SignedAngle(transform.forward, displacement, Vector3.up) : 0;
        Vector3 angularVel = Vector3.up * Mathf.Clamp(-angularSpeed, angularSpeed, angle);// * angularSpeed;
        Vector3 vel = transform.forward * input.magnitude * speed;

        //rb.angularVelocity = angularVel * Mathf.Deg2Rad;
        //rb.velocity = vel;
        transform.Rotate(angularVel * Time.deltaTime, Space.World);
        transform.Translate(vel * Time.deltaTime, Space.World);
    }

    void Move3() {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 displacement = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * input.normalized; // camera
        float goalAngle = transform.eulerAngles.y;
        if(displacement.magnitude>.05f)
            goalAngle = Mathf.Atan2(displacement.z, displacement.x)*Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, 90-goalAngle, ref ref1, .2f);
        transform.eulerAngles = Vector3.up * angle;

        transform.position += transform.forward * speed * displacement.magnitude * Time.deltaTime;
    }


    void Move4() {
      input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
      inputRotated = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * input.normalized;

      if (inputRotated.magnitude>0) {
          lastDir = inputRotated;
      }
      //else {
      //    inputRotated += lastDir * .01f;
      //}
      //Vector3 smoothTo = inputRotated.magnitude > 0 ? inputRotated : lastDir;

      moveDirection = Vector3.Lerp(moveDirection, lastDir, Time.deltaTime * 6);
      moveDirection.Normalize();
      transform.LookAt(transform.position + moveDirection);
      cc.Move(moveDirection * speed * input.magnitude * Time.deltaTime);
   }
   


   void MovePlayerRelative() {
        inp = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 angularVel = Vector3.up * inp.x * angularSpeed * Mathf.Lerp(.6f, 1f, Mathf.Abs(inp.z)) * Mathf.Sign(inp.z);
        Vector3 vel = transform.forward * inp.z * speed;
        transform.Rotate(angularVel *  Time.deltaTime, Space.World);
        cc.Move(vel * Time.deltaTime);
    }
   

    void MoveCameraRelative() {
        inp = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        inputRotated = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * inp.normalized;

        if (inputRotated.magnitude > 0.05f) {
            lastDir = inputRotated;
        }
        else {
            lastDir = transform.forward;
        }

        Quaternion a = transform.rotation;
        transform.LookAt(transform.position + lastDir);
        Quaternion b = transform.rotation;
        transform.rotation = Quaternion.Lerp(a, b, Time.deltaTime * 4);

        //moveDirection = Vector3.Lerp(moveDirection, lastDir, Time.deltaTime * 4);
        //moveDirection.Normalize();
        cc.Move(transform.forward * speed * inp.magnitude * Time.deltaTime);
    } 
     
    */

}