// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

////////// DESCRIPTION //////////

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    public enum ControlType { PlayerRelative, CameraRelative }

    // public
    public ControlType controlType = ControlType.PlayerRelative;
    public float speed = 5;
    public float angularSpeed = 180;
    public float pickupArea = 3f;



    // private
    float ref1;
    Vector3 inp;
    bool animStart, animEnd;
    float animHelloTime;
    Vector3 moveDirection;
    Vector3 lastDir;
    Vector3 inputRotated;



    // references
    public static Player instance;
    CharacterController cc;
    Animator anim;
    public Transform pickupCenter;
    AudioSource feet;
    public Text controlText;

    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
    }

    void Start () {
        animHelloTime = Time.time + Random.Range(2f, 4f);
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        feet = GetComponent<AudioSource>();
	}
	
	void Update () {
        if (GameManager.Playing) {
            SetToGround();

            if (Input.GetKeyDown(KeyCode.Tab)){
                controlType = (ControlType)(1 - (int)controlType); // switch it
            }
            /*
            if (controlType == ControlType.PlayerRelative)
                MovePlayerRelative();
            else if (controlType == ControlType.CameraRelative)
                MoveCameraRelative();
            */
            MoveOvercooked();
        }
        else {
            inp = Vector3.zero;
        }
        DealWithAnimations();
        feet.volume = inp.normalized.magnitude/20;
        feet.pitch = Mathf.Lerp(feet.pitch, Random.Range(.8f, 1.2f), Time.deltaTime * 2);

        UpdateControlUI();

	}

    private void FixedUpdate() {
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands
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

    void MoveOvercooked() {
        inp = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        inp.Normalize();
        inp = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * inp;

        float signedAngle = Vector3.SignedAngle(transform.forward, inp, Vector3.up);
        float rotAmount = angularSpeed * Time.deltaTime;
        rotAmount = Mathf.Min(rotAmount, Mathf.Abs(signedAngle)); // clamp it if it's smaller
        transform.Rotate(Vector3.up * rotAmount * Mathf.Sign(signedAngle));

        float angle = Vector3.Angle(transform.forward, inp);
        //project movement onto direction
        if (angle <= 90) {
            float dot = Vector3.Dot(transform.forward, inp); // projection for smooth results
            cc.Move(transform.forward * speed * inp.magnitude * dot* Time.deltaTime);
        }
        
    }

    //-------------------------------------------------------------------------

    void SetToGround() {
        //to avoid bugs
        Vector3 temp = transform.position;
        temp.y = 0;
        transform.position = temp;
    }

    void DealWithAnimations() {
        //start
        if (Time.time > animHelloTime) {
            anim.SetTrigger("hello");
            animHelloTime = Time.time + Random.Range(4f, 8f);
        }

        //play
        if (GameManager.Playing && !animStart) {
            animStart = true;
            anim.SetTrigger("start");
        }
        bool running = inp.magnitude > .1f;
        anim.SetBool("running", running);

        //end
        if (GameManager.Gameover && !animEnd) {
            animEnd = true;
            anim.SetTrigger("end");
        }
    }

    public void PushButtonAnim() {
        anim.SetTrigger("button");

    }

    void UpdateControlUI() {
        string ct = controlType == ControlType.PlayerRelative ? "player" : "camera";
        controlText.text = "control: " + ct + "-relative\nTAB to change";
    }

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
    }*/

    /*
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
}*/



    // queries
    public bool CloseEnough(Transform t) {
        Vector3 vec = pickupCenter.position - t.transform.position;
        vec.y = 0;
        float dist = Vector3.SqrMagnitude(vec);
        return dist <= pickupArea * pickupArea;
    }


    // other
    private void OnDrawGizmos() {
        //Gizmos.DrawWireSphere(pickupCenter.position, pickupArea);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, inputRotated*3);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position+Vector3.up, moveDirection*3);
    }

}