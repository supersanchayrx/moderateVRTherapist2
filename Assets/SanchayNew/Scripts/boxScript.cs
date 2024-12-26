using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class boxScript : MonoBehaviour
{

    public LayerMask terrainLayer;
    public therapistDialogues therapistDialogueScript;
    public FloatingBox floatingBoxScript;
    public GameObject openText;
    public StormManager stormManager;
    public TextToSpeech ttsScript;
    public navmeshTherapist navmeshScript;

    Animator anim;

    public InputActionReference openBox;

    public bool canTalk=true;

    public bool movingToPlayer=false;



    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Awake()
    {
        openBox.action.Enable();
        openBox.action.performed += openBoxAnim;
    }



    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.gameObject.layer == terrainLayer)
        {
            therapistDialogueScript.playerCompletedInstruction = true;
            openText.SetActive(true);
        }*/

        if (((1 << collision.gameObject.layer) & terrainLayer) != 0)
        {
            if(!movingToPlayer)
            {
                movingToPlayer = true;
                navmeshScript.setTransform(0);
            }
            therapistDialogueScript.playerCompletedInstruction = true;
            openText.SetActive(true);

            if (canTalk)
            {
                canTalk = false;
                ttsScript.startTTs("Now Open the box");
            }
            //StartCoroutine(therapistDialogueScript.moveTonextPoint(0, false));
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        /*if (collision.gameObject.layer == terrainLayer)
        {
            therapistDialogueScript.playerCompletedInstruction = true;
            openText.SetActive(true);
        }*/

        if (!movingToPlayer)
        {
            movingToPlayer = true;
            navmeshScript.setTransform(0);
        }

        if (((1 << collision.gameObject.layer) & terrainLayer) != 0)
        {
            therapistDialogueScript.playerCompletedInstruction = true;
            openText.SetActive(true);
        }
    }


    void openBoxAnim(InputAction.CallbackContext context)
    {
        if (floatingBoxScript.canOpen)
        {
            floatingBoxScript.canOpen = false;
            anim.SetTrigger("Trigger");

            StartCoroutine(stormManager.StartStorm());

        }
    }

    /*IEnumerator startStorm()
    {
        yield return new WaitForSeconds(2f);

        stormManager.StartStorm();
    }*/

    private void OnDestroy()
    {
        openBox.action.Disable();
        openBox.action.performed -= openBoxAnim;
    }
}
