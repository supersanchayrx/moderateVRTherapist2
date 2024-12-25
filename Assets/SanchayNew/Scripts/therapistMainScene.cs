using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class therapistMainScene : MonoBehaviour
{
    public navmeshTherapist navmeshTherapistScript;
    Animator anim;
    public TextToSpeech textToSpeechScript;

    private bool goTocar;

    public GameObject stepHereCanvas;

    float safetyCheckTimer=0f;

    private void Start()
    {
        anim = GetComponent<Animator>();

        anim.SetTrigger("wave");
        goTocar = false;
        stepHereCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!goTocar)
        {
            safetyCheckTimer += Time.deltaTime;
        }

        if (safetyCheckTimer > 2f)
        {
            goTocar = true;
            safetyCheckTimer = 0f;
        }

        if (goTocar && navmeshTherapistScript.reached)
        {
            goTocar = false;
            StartCoroutine(gotoCar());
        }
    }
    IEnumerator gotoCar()
    {
        textToSpeechScript.startTTs(textToSpeechScript.textMessage);
        yield return new WaitForSeconds(4f);
        anim.SetTrigger("wave");

        yield return new WaitForSeconds(22f);

        navmeshTherapistScript.setTransform(1, true);
        stepHereCanvas.SetActive(true);
    }
}
