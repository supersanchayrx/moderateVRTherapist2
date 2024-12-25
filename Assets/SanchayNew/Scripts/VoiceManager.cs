using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Oculus.Voice;
using System.Reflection;
using Meta.WitAi.CallbackHandlers;
using UnityEngine.Events;
using System.Linq;
using Oculus.Assistant.VoiceCommand.Configuration;
using UnityEngine.InputSystem;

public class VoiceManager : MonoBehaviour
{

    public bool canvasOn=false;
    [Header("Wit configs")]
    [SerializeField] private AppVoiceExperience appVoiceExperience;
    [SerializeField] public TextMeshProUGUI transcribedText;

    [Header("Voice Events")]
    [SerializeField] private UnityEvent transcriptionStarted;
    [SerializeField] public UnityEvent transcriptionCompleted;

    public bool voiceCommandAvailable = false;
    public bool isTranscribing = false;

    public GameObject transcriberCanvas;


    public InputActionReference voiceCommands;
    private void Awake()
    {
        voiceCommands.action.Enable();
        voiceCommands.action.performed += talkToTherapist;
        InputSystem.onDeviceChange += onDeviceChange;
        // Ensure the voice events are properly set up
        appVoiceExperience.VoiceEvents.OnPartialTranscription.AddListener(OnPartialTranscription);
        appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener(OnFullTranscription);
        transcriberCanvas.SetActive(false);
    }

    private void OnDestroy()
    {
        voiceCommands.action.Disable();
        voiceCommands.action.performed -= talkToTherapist;
        InputSystem.onDeviceChange -= onDeviceChange;


        // Clean up listeners when the object is destroyed
        appVoiceExperience.VoiceEvents.OnPartialTranscription.RemoveListener(OnPartialTranscription);
        appVoiceExperience.VoiceEvents.OnFullTranscription.RemoveListener(OnFullTranscription);
    }


    void talkToTherapist(InputAction.CallbackContext context)
    {
        if (!voiceCommandAvailable)
        {
            StartTranscription();
        }


        else
        {
            StopTranscription();
        }
    }

    private void StartTranscription()
    {
        if (!voiceCommandAvailable)
        {
            transcriberCanvas.SetActive(true);
            voiceCommandAvailable = true;
            appVoiceExperience.Activate(); // Activate voice input
            isTranscribing = true;
            transcriptionStarted?.Invoke();
            Debug.Log("Transcription started.");
            canvasOn= true;
        }
    }

    private void StopTranscription()
    {
        if (voiceCommandAvailable)
        {
            appVoiceExperience.Deactivate(); // Deactivate voice input
            isTranscribing = false;
            Debug.Log("Transcription stopped.");
            transcriptionCompleted?.Invoke();
            OnRequestCompleted();
        }
    }

    private void OnPartialTranscription(string transcription)
    {
        if (voiceCommandAvailable && isTranscribing)
        {
            transcribedText.text = transcription; // Update text with partial transcription
            Debug.Log(transcription);
        }
    }

    private void OnFullTranscription(string transcription)
    {
        if (voiceCommandAvailable && isTranscribing)
        {
            transcribedText.text = transcription; // Update text with full transcription
            StopTranscription(); // Automatically stop transcription after full transcription
            transcriptionCompleted?.Invoke();
            Debug.Log(transcription);

        }
    }

    private void OnRequestCompleted()
    {
        voiceCommandAvailable = false; // Reset flag after a request is completed
        //transcriberCanvas.SetActive(false);
    }


    private void onDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Disconnected:
                voiceCommands.action.Disable();
                voiceCommands.action.performed -= talkToTherapist;
                break;

            case InputDeviceChange.Reconnected:
                voiceCommands.action.Enable();
                voiceCommands.action.performed += talkToTherapist;
                break;
        }
    }
}




