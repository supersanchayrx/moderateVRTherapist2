using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class StormManager : MonoBehaviour
{
    public ParticleSystem rain, thunder;
    public Material skyBoxCloudy;

    [Range(0f, 20f)] public float maxThunderValue = 20f; 
    [Range(0f, 250f)] public float maxRainValue = 250f;
    [Range(0f, 0.45f)] public float maxVigIntensity = 0.45f;
    [Range(0f, 25f)] public float maxSaturation = 25f;
    [Range(0f, 23f)] public float maxHue = 23f;
    [Range(0f, 19f)] public float maxContrast = 19f;
    [Range(0f, 0.623f)] public float maxVolume = 0.623f;


    public Color finalColor;


    [Range(0f, 1f)] public float rotationSpeed;

    [Range(0.45f, 1f)] public float exposureValue = 0.25f;

    [Range(0f, 1f)] public float masterSlider = 0f; // 0 -> storm is very strong, 1 -> clear sky

    private float lastMasterSliderValue;

    public float rainValue, thunderValue, vigIntensity,sat,hue,con,currentVolume;

    public Color currentColor, startinColor;

    public Volume vol;

    public AudioSource rainBgm, forestSounds;

    public therapistDialogues therapistDialogueScript;

    UnityEngine.Rendering.Universal.Vignette vig;
    ColorAdjustments colorAdjustments;

    public Transform player;
    Transform startingPlayerTransform;

    public bool stormStarted;

    public TextToSpeech ttsScript;


    private void Start()
    {
        lastMasterSliderValue = masterSlider;

        player = GameObject.Find("Player").GetComponent<Transform>();

        vol.profile.TryGet(out vig);
        vol.profile.TryGet(out colorAdjustments);

        startinColor = colorAdjustments.colorFilter.value;
        currentColor = startinColor;

        masterSlider = 1f;
        UpdateStormParameters();

        startingPlayerTransform = player;

        stormStarted=false;

        //StartCoroutine(StartStorm());
    }

    private void Update()
    {
        float newRotation = skyBoxCloudy.GetFloat("_Rotation") + rotationSpeed * Time.deltaTime;
        skyBoxCloudy.SetFloat("_Rotation", newRotation);

        if (!Mathf.Approximately(lastMasterSliderValue, masterSlider))
        {
            // Update last value
            lastMasterSliderValue = masterSlider;

            UpdateStormParameters();
        }

        if (masterSlider == 0.8f)
            masterSlider = 1f;

        if(masterSlider==1f && stormStarted)
        {
            ttsScript.startTTs("Yayy you did it!!  You calmed yourself by practicing Neuro therapeutic excercise developed by moderate limited. This is just one of many ways we help you calm your mind! Thanks for playing");
            StartCoroutine(ending());
        }

        if(stormStarted && masterSlider!=1f && forestSounds.gameObject.activeSelf)
        {
            forestSounds.gameObject.SetActive(false);
        }

        else if(stormStarted && masterSlider ==1f && !forestSounds.gameObject.activeSelf)
        {
            forestSounds.gameObject.SetActive(true);
        }
    }

    private void UpdateStormParameters()
    {
        thunderValue = Mathf.Lerp(maxThunderValue, 0f, masterSlider);
        rainValue = Mathf.Lerp(maxRainValue, 0f, masterSlider);
        exposureValue = Mathf.Lerp(0.45f, 1f, masterSlider);
        vigIntensity = Mathf.Lerp(maxVigIntensity, 0f, masterSlider);
        hue = Mathf.Lerp(maxHue, 0f, masterSlider);
        sat = Mathf.Lerp(maxSaturation, 0f, masterSlider);
        con = Mathf.Lerp(maxContrast, 0f, masterSlider);
        currentColor = Color.Lerp(startinColor,finalColor,masterSlider);
        currentVolume = Mathf.Lerp(maxVolume, 0f, masterSlider);


        // Apply changes
        ModifyExposure();
        ModifyLightning();
        ModifyRain();
        ModifyVigennete();
        modifyColors();
        modifyRainSounds();

        if (stormStarted)
        {
            ttsScript.startTTs("Look around the storm is calming down");
        }
    }

    private void ModifyExposure()
    {
        skyBoxCloudy.SetFloat("_Exposure", exposureValue);
    }

    private void ModifyLightning()
    {
        var thunderEmission = thunder.emission;
        thunderEmission.rateOverTime = thunderValue;
    }

    private void ModifyRain()
    {
        var rainEmission = rain.emission;
        rainEmission.rateOverTime = rainValue;
    }

    private void ModifyVigennete()
    {
        vig.intensity.Override(vigIntensity);
    }


    private void modifyColors()
    {
        colorAdjustments.saturation.Override(sat);
        colorAdjustments.hueShift.Override(hue);
        colorAdjustments.contrast.Override(con);
        colorAdjustments.colorFilter.Override(currentColor);
    }

    private void modifyRainSounds()
    {
        rainBgm.volume = currentVolume;
    }

    public IEnumerator StartStorm()
    {
        Debug.Log("flashing yo ahh abhi");

        yield return new WaitForSeconds(4f);

        float initialExposure = colorAdjustments.postExposure.value;

        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            // Lerp exposure value over time
            colorAdjustments.postExposure.Override(Mathf.Lerp(initialExposure, 40f, elapsedTime / 2f));
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        player.position = startingPlayerTransform.position;

        yield return new WaitForSeconds(1f);

        float resetElapsedTime = 0f;
        float exposureDuringWait = colorAdjustments.postExposure.value;
        while (resetElapsedTime < 2f)
        {
            colorAdjustments.postExposure.Override(Mathf.Lerp(exposureDuringWait, 0f, resetElapsedTime / 2f));
            resetElapsedTime += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        masterSlider = 0f;
        UpdateStormParameters();

        //therapistDialogueScript.playerCompletedInstruction = true;
        therapistDialogueScript.firstTime = false;
        therapistDialogueScript.currentDialogue = 1;

        stormStarted = true;
    }

    private IEnumerator ending()
    {


        Debug.Log("flashing yo ahh abhi");

        yield return new WaitForSeconds(20f);

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
