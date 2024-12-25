using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class therapistDialogues : MonoBehaviour
{
    public string[] dialogues;
    public float timer = 0f;

    public bool interacted = false;
    public bool playerFollowed = true;
    public bool reachedPlayer = false;
    public bool playerConnected = false;

    public bool playerAtMine = false;

    public float retryTimer, reachedDistance;

    public int currentDialogue;

    public bool canTalk = true;

    int ttsRequestCount = 5;
    int currentttsRequests = 0;

    Animator anim;
    navmeshTherapist therapistScript;
    TextToSpeech ttsScript;

    public bool vineConnected = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        therapistScript = GetComponent<navmeshTherapist>();
        ttsScript = GetComponent<TextToSpeech>();
    }

    private void Update()
    {

        if (Vector3.Distance(this.gameObject.transform.position, therapistScript.player.position) <= reachedDistance)
        {
            reachedPlayer = true;
        }

        else
        {
            reachedPlayer = false;
        }

        if (reachedPlayer && !interacted && !anim.GetBool("isWalking"))
        {
            Debug.Log("starting the first time interaction");
            StartCoroutine(moveTonextPoint(2));
        }

        if (!interacted && playerAtMine && playerFollowed && canTalk)
        {
            //playerFollowed = true;
            //interacted = true;
            Debug.Log("starting Mine interaction");
            currentDialogue = 1;
            StartCoroutine(moveTonextPoint(2));
        }

        if (interacted && !playerFollowed && !anim.GetBool("isWalking") && therapistScript.reached && therapistScript.therapistAtMine)
        {
            Debug.Log("setting off the timer");
            timer += Time.deltaTime;
        }

        else if (interacted && playerAtMine && vineConnected)
        {
            vineConnected = false;
            timer = 0f;
            //switch to next position and next dialogue
            currentDialogue = 2;
            Debug.Log("hello why is this triggering ?");
            //ttsScript.startTTs(dialogues[currentDialogue]);
        }

        else
        {
            timer = 0f;
        }

        if (timer > retryTimer)
        {
            timer = 0f;
            //go to player again and repeat the last dialogue
            StartCoroutine(moveTonextPoint(0, true));
        }


        if ((Vector3.Distance(this.gameObject.transform.position, therapistScript.player.position) <= reachedDistance && anim.GetBool("isWalking")) || playerAtMine)
        {
            playerFollowed = true;
        }

        else
        {
            playerFollowed = false;
        }
    }


    IEnumerator moveTonextPoint(int i)
    {
        interacted = true;
        canTalk = false;
        if (currentttsRequests < ttsRequestCount && reachedPlayer)
        {
            ttsScript.startTTs(dialogues[currentDialogue]);
            currentttsRequests++;
        }
        yield return new WaitForSeconds(15f);
        therapistScript.setTransform(i);
        yield return new WaitForSeconds(5f);
        //playerFollowed = false;
        //interacted = false;
    }

    IEnumerator moveTonextPoint(int i, bool retryRequest)
    {
        /*playerFollowed = true;
        if (currentttsRequests < ttsRequestCount && reachedPlayer)
        {
            ttsScript.startTTs(dialogues[currentDialogue]);
            currentttsRequests++;
        }*/
        //playerFollowed=true;
        //yield return new WaitForSeconds(1f);
        therapistScript.setTransform(i);
        while (!reachedPlayer)
        {
            yield return null;
        }
        //ttsScript.startTTs(dialogues[currentDialogue]);
        interacted = false;
        yield return new WaitForSeconds(0.2f);
        //playerFollowed = false;
        //interacted=false;
    }

}
