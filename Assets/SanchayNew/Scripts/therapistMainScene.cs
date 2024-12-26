using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class therapistMainScene : MonoBehaviour
{
    public navmeshTherapist navmeshTherapistScript;
    Animator anim;
    public TextToSpeech textToSpeechScript;

    public bool goTocar, goToPlayer;

    public GameObject stepHereCanvas;

    //float safetyCheckTimer=0f;

    private void Start()
    {
        anim = GetComponent<Animator>();

        anim.SetTrigger("wave");
        goTocar = false;
        goToPlayer = true;
        stepHereCanvas.SetActive(false);
    }

    private void Update()
    {
        if (goToPlayer && navmeshTherapistScript.therapistNearPlayer)
        {
            goToPlayer = false;
            StartCoroutine(gotoCar());
        }
    }
    IEnumerator gotoCar()
    {
        textToSpeechScript.startTTs(textToSpeechScript.textMessage);
        yield return new WaitForSeconds(2f);
        anim.SetTrigger("wave");

        yield return new WaitForSeconds(10f);

        navmeshTherapistScript.setTransform(1, true);
        stepHereCanvas.SetActive(true);
    }
}
