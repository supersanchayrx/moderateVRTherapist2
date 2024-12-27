using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDoubtVoices : MonoBehaviour
{
    public TextToSpeech ttsScript;
    public navmeshTherapist navmeshTherapistScript;
    public bool canTalk;

    public Canvas affirmationsCanvas;

    private void Start()
    {
        affirmationsCanvas.gameObject.SetActive(false);
        canTalk = true;
    }

    private void Update()
    {
        if(canTalk && navmeshTherapistScript.therapistNearPlayer)
        {
            canTalk = false;
            StartCoroutine(therapistSpeech());
        }
    }






    IEnumerator therapistSpeech()
    {
        ttsScript.startTTs("Hello I know you are hearing voices. We will fix that! Bur first, Look at the mirror and tell me what do you see");

        yield return new WaitForSeconds(15f);

        ttsScript.startTTs("You see the mirror is so distorted, this is representing the state of your mind. You need to believe in yourself again.");

        yield return new WaitForSeconds(8f);
        ttsScript.startTTs("Let's start with somee positive affirmations!");

        yield return new WaitForSeconds(2f);

        affirmationsCanvas.gameObject.SetActive(true);

    }
}
