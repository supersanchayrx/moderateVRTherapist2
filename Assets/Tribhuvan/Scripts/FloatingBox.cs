using Oculus.Assistant.VoiceCommand.Configuration;
using Oculus.Voice;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FloatingBox : MonoBehaviour
{
    public float floatAmplitude = 0.5f; // Range of floating motion
    public float floatSpeed = 1f; // Speed of floating motion
    private Vector3 startPosition;

    Rigidbody rb;

    bool canFLoat=true;

    private XRGrabInteractable grabInteractable;

    public LayerMask terrainLayer;

    //public therapistDialogues therapistDialogueScript;


    public bool canOpen = false;



   // public StormManager stormManager;


    /*private void Awake()
    {
        openBox.action.Enable();
        openBox.action.performed +=openBoxAnim;
    }*/


    void Start()
    {
        startPosition = transform.position;
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        rb = GetComponent<Rigidbody>();

        canOpen = false ;
    }

    void Update()
    {

        if(canFLoat)
        {
            float newY = startPosition.y + floatAmplitude * Mathf.Sin(Time.time * floatSpeed);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        /*if (canOpen && )
        {
            canOpen = false ;
        }*/
        
    }


    void OnGrab(SelectEnterEventArgs args)
    {
        canFLoat = false;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        rb.isKinematic = false;
        canOpen=true;
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }
}