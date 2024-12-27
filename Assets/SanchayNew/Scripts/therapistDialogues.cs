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

    public bool firstTime=true;

    int ttsRequestCount = 5;
    int currentttsRequests = 0;

    public bool playerCompletedInstruction=false;

    Animator anim;
    navmeshTherapist therapistScript;
    TextToSpeech ttsScript;

    public StormManager stormManager;

    public Transform wisdomTree;

    public bool vineConnected = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        therapistScript = GetComponent<navmeshTherapist>();
        ttsScript = GetComponent<TextToSpeech>();
        firstTime = true;
    }

    private void Update()
    {
        switch (currentDialogue)
        {
            case 0:
                Debug.Log("this is wisdom tree ");
                break;
            case 1:
                Debug.Log("this is chaotic interaction");
                break;
            case 2:
                Debug.Log("this is puzzle instruction");
                break;
            case 3:
                Debug.Log("this is puzzle Feedback");
                break;
            case 5:
                Debug.Log("this is final victory");
                break;
        }

        if (Vector3.Distance(this.gameObject.transform.position, therapistScript.player.position) <= reachedDistance)
        {
            reachedPlayer = true;
        }

        else
        {
            reachedPlayer = false;
        }

        if (reachedPlayer && !interacted && !anim.GetBool("isWalking") && !firstTime && stormManager.stormStarted)
        {
            Debug.Log("starting the first time interaction");
            StartCoroutine(moveTonextPoint(2));
        }

        if (!interacted && playerAtMine && playerFollowed && canTalk && !firstTime)
        {
            //playerFollowed = true;
            //interacted = true;
            Debug.Log("starting Mine interaction");
            currentDialogue = 2;
            StartCoroutine(moveTonextPoint(2));
        }

        if (interacted && !playerFollowed && !anim.GetBool("isWalking") && therapistScript.reached && therapistScript.therapistAtMine && !firstTime && !therapistScript.therapistNearPlayer)
        {
            Debug.Log("setting off the timer");
            timer += Time.deltaTime;
        }

        else if (interacted && playerAtMine && vineConnected && !firstTime)
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


        if ((Vector3.Distance(this.gameObject.transform.position, therapistScript.player.position) <= reachedDistance && anim.GetBool("isWalking")) || playerAtMine && !firstTime)
        {
            playerFollowed = true;
        }

        else
        {
            playerFollowed = false;
        }

        if(reachedPlayer && firstTime && canTalk)
        {
            StartCoroutine(pointToWisdomTree());
        }

        /*if(playerCompletedInstruction)
        {
            playerCompletedInstruction = false;
            currentDialogue++;
        }*/
    }


    public IEnumerator moveTonextPoint(int i)
    {
        interacted = true;
        canTalk = false;
        if (currentttsRequests < ttsRequestCount && reachedPlayer)
        {
            ttsScript.startTTs(dialogues[currentDialogue]);
            currentttsRequests++;
        }
        yield return new WaitForSeconds(13f);
        therapistScript.setTransform(i);
        yield return new WaitForSeconds(3f);
        //playerFollowed = false;
        //interacted = false;
    }

    public IEnumerator moveTonextPoint(int i, bool retryRequest)
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


    IEnumerator pointToWisdomTree()
    {
        canTalk = false;
        ttsScript.startTTs(dialogues[currentDialogue]);

        yield return new WaitForSeconds(6f);
        anim.SetTrigger("Point");
        yield return new WaitForSeconds(4f);
        Vector3 direction = wisdomTree.position - transform.position;
        direction.y= 0;
        if (direction!=Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Vector3 eulerAngles = targetRotation.eulerAngles;

            eulerAngles.y -= 30f;
            Quaternion lookRot = Quaternion.Euler(eulerAngles);


            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 5f * Time.deltaTime);
        }

        

        Debug.Log("told player about wisdom Tree");


        yield return new WaitForSeconds(10f);
        canTalk = true;

        currentDialogue=1;
        firstTime = false;
    }

}
