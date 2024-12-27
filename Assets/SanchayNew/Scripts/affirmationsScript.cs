using Meta.WitAi.Json;
using Oculus.Voice;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class affirmationsScript : MonoBehaviour
{
    public bool recording;

    public TextToSpeech ttsScript;

    [Header("Wit configs")]
    [SerializeField] private AppVoiceExperience appVoiceExperience;
    [SerializeField] public TMP_InputField transcribedText;

    public UnityEvent saidSomethingNice;
    public UnityEvent saidSomethingBad;

    public AudioSource whisperVoices, sereneVoices;


    public InputActionReference voiceCommands;

    public int affirmationCount;

    public float initialVolume;

    //public ChangeRenderTexture CRT;

    private void Awake()
    {
        appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener((transcription) =>
        {
            transcribedText.text = transcription;
        });

        appVoiceExperience.VoiceEvents.OnPartialTranscription.AddListener((transcription) =>
        {
            transcribedText.text = transcription;
        });

        appVoiceExperience.VoiceEvents.OnResponse.AddListener(OnVoiceResponse);
        appVoiceExperience.VoiceEvents.OnError.AddListener(OnVoiceError);

        voiceCommands.action.Enable();
        voiceCommands.action.performed += sayAffirmations;

    }

    private void Start()
    {
        affirmationCount = 0;
        initialVolume = whisperVoices.volume;
        sereneVoices.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        appVoiceExperience.VoiceEvents.OnFullTranscription.RemoveAllListeners();
        appVoiceExperience.VoiceEvents.OnPartialResponse.RemoveAllListeners();
        appVoiceExperience.VoiceEvents.OnResponse.RemoveAllListeners();
        appVoiceExperience.VoiceEvents.OnError.RemoveAllListeners();

        voiceCommands.action.Disable();
        voiceCommands.action.performed -= sayAffirmations;
    }

    void sayAffirmations(InputAction.CallbackContext context)
    {
        if(!recording)
        {
            recording = true;
            appVoiceExperience.Activate();
        }
        else
        {
            recording = false;
            appVoiceExperience.Deactivate();
        }
    }

    void OnVoiceResponse(WitResponseNode response)
    {
        string intentName = response["intents"][0]["name"];

        if(!string.IsNullOrEmpty(intentName))
        {
            switch (intentName)
            {
                case "negative_affirmation":
                    //negative affirmation case
                    HandleNegativeAffirmation();
                    break;
                case "affirmation1":
                    HandleAffirmation();
                    break;
                case "affirmation2":
                    HandleAffirmation();
                    break;
                case "affirmation3":
                    HandleAffirmation();
                    break;
                case "affirmation4":
                    HandleAffirmation();
                    break;
                case "affirmation5":
                    HandleAffirmation();
                    break;
                case "affirmation6":
                    HandleAffirmation();
                    break;
                case "affirmation7":
                    HandleAffirmation();
                    break;
            }
        }

        else
        {
            Debug.Log("No scope intent");
        }

        appVoiceExperience.Deactivate();
        recording = false;
    }

    void OnVoiceError(string error, string message)
    {
        Debug.Log("error");
    }

    private void HandleNegativeAffirmation()
    {
        Debug.Log("Negative affirmation intent detected!");

        ttsScript.startTTs("Please don't say anything bad about yourself! You are a great person and deserve all the love! Let's try the affirmations again");
        saidSomethingBad?.Invoke();
        // Add your logic for negative affirmation here
    }

    private void HandleAffirmation()
    {
        Debug.Log("Affirmation intent detected!");

        saidSomethingNice?.Invoke();
        ttsScript.startTTs("Yes good job! See the mirror is becoming clearer and the voices are getting fainter. Let's continue with the affirmations");
        //CRT.ResizeRenderTexture();
        affirmationCount++;
        // Add your logic for affirmation here
    }

    private void UpdateAudioVolume()
    {
        // Calculate the new volume proportionally
        float newVolume = Mathf.Lerp(initialVolume, 0f, (float)affirmationCount / 5);
        whisperVoices.volume = newVolume;

        // If the counter reaches the max, stop the audio
        if (affirmationCount >= 5)
        {
            whisperVoices.gameObject.SetActive(false);
            sereneVoices.gameObject.SetActive(true);

        }
    }
}
