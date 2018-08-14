using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

    private Vector2 inputValue;
    public Vector2 InputValue { // use this to access VJ values
        get { return inputValue; }
    }
    private bool isdown;
    private bool released;
    public bool IsDown { get { return isdown; } }
    public bool Released { get { return released; } }


    public float maxStickDisplacementPercent = 0.7f; // factor to limit stick movement

    private Image container;
    private Image stick;


    void Start() {
        container = GetComponent<Image>();
        stick = transform.GetChild(0).GetComponent<Image>(); //this command is used because there is only one child in hierarchy
        inputValue = Vector3.zero;
    }



    // most important function
    public void OnDrag(PointerEventData ped) {
        Vector2 touchPosition = Vector2.zero;
        Vector2 containerRadius = container.rectTransform.sizeDelta / 2 * maxStickDisplacementPercent;

        //To get InputDirection
        RectTransformUtility.ScreenPointToLocalPointInRectangle(container.rectTransform, ped.position, ped.pressEventCamera, out touchPosition);
        //if(debugForThis) Debug.Log("position: " + touchPosition);

        inputValue.x = (touchPosition.x / containerRadius.x);
        inputValue.y = (touchPosition.y / containerRadius.y);

        inputValue = (inputValue.magnitude > 1) ? inputValue.normalized : inputValue;

        //to define the area in which joystick can move around
        stick.rectTransform.anchoredPosition = new Vector3(inputValue.x * containerRadius.x, inputValue.y * containerRadius.y);
    }

    public void OnPointerDown(PointerEventData ped) {
        OnDrag(ped);
        isdown = true;
    }

    public void OnPointerUp(PointerEventData ped) {
        isdown = false;
        inputValue = Vector3.zero;
        stick.rectTransform.anchoredPosition = Vector3.zero;
        StartCoroutine("OnRelease");
    }
    IEnumerator OnRelease() {
        released = true;
        yield return null;
        released = false;
    }

}