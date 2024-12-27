using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class selfDoubtVoices : MonoBehaviour
{
    public TextToSpeech ttsScript;
    public navmeshTherapist navmeshTherapistScript;
    public bool canTalk;

    public Canvas affirmationsCanvas;

    public affirmationsScript affirms;

    public Animator anim;

    ColorAdjustments colorAdjustments;
    public Volume vol;
    private void Start()
    {
        affirmationsCanvas.gameObject.SetActive(false);
        canTalk = true;

        vol.profile.TryGet(out colorAdjustments);

    }

    private void Update()
    {
        if(canTalk && navmeshTherapistScript.therapistNearPlayer && affirms.affirmationCount<5)
        {
            canTalk = false;
            StartCoroutine(therapistSpeech());
        }

        if(affirms.affirmationCount>=5 && canTalk)
        {
            canTalk=false;
            StartCoroutine(therapistSpeech(true));
        }
    }






    IEnumerator therapistSpeech()
    {
        ttsScript.startTTs("Hello I know you are hearing voices. We will fix that! Bur first, Look at the mirror and tell me what do you see");

        yield return new WaitForSeconds(15f);

        ttsScript.startTTs("You see the mirror is so distorted, this is representing the state of your mind. You need to believe in yourself again.");
        yield return new WaitForSeconds(3f);
        anim.SetTrigger("Point");

        yield return new WaitForSeconds(10f);
        ttsScript.startTTs("Let's start with somee positive affirmations!");

        yield return new WaitForSeconds(2f);

        affirmationsCanvas.gameObject.SetActive(true);

    }

    IEnumerator therapistSpeech(bool endSpeech)
    {
        ttsScript.startTTs("See you successfully silenced all the voices inside you! This is just one many excercises by Moderate Ltd that help you get rid of self doubts");

        yield return new WaitForSeconds(15f);

        Debug.Log("flashing yo ahh abhi");

        yield return new WaitForSeconds(3f);

        float initialExposure = colorAdjustments.postExposure.value;

        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            // Lerp exposure value over time
            colorAdjustments.postExposure.Override(Mathf.Lerp(initialExposure, 40f, elapsedTime / 2f));
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        SceneManager.LoadScene("MainScene");
    }
}
