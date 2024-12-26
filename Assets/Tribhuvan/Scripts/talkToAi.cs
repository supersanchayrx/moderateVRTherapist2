using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class talkToAi : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VoiceManager voiceManager; // Reference to the VoiceManager
    [SerializeField] private AiChat aiChat; // Reference to the AiChat script
    [SerializeField] private TextToSpeech textToSpeech; // Reference to the TextToSpeech script

    [Header("Voice Transcription to AI")]
    public UnityEvent onVoiceToTextProcessed; // Event triggered after AI processes the input

    private void Start()
    {
        // Subscribe to the AI response event once at the start
        aiChat.OnAIResponseReceived.AddListener(SendAIResponseToTTS);
    }

    private void OnEnable()
    {
        // Listen for the transcription completion event
        voiceManager.transcriptionCompleted.AddListener(ProcessAIResponse);
    }

    private void OnDisable()
    {
        // Remove listeners when disabled
        voiceManager.transcriptionCompleted.RemoveListener(ProcessAIResponse);
        aiChat.OnAIResponseReceived.RemoveListener(SendAIResponseToTTS); // Unsubscribe from the event
    }

    // This function is called once the transcription is completed
    private void ProcessAIResponse()
    {
        if (voiceManager.transcriptionCompletedbool)
        {
            voiceManager.transcriptionCompletedbool=false;
            string userMessage = voiceManager.transcribedText.text; // Get the transcribed message

            // Send the message to the AI for processing
            aiChat.SendMessageToAI(userMessage);

            // The listener for the AI response is already set up in Start(), so no need to subscribe here
        }
    }

    // This function is called once the AI response is received
    private void SendAIResponseToTTS(string aiResponse)
    {
        if(aiChat.responseReceived)
        {
            aiChat.responseReceived = false;
            textToSpeech.startTTs(aiResponse);

            // Trigger the event
            onVoiceToTextProcessed?.Invoke();
        }
        // Send the AI response to the TextToSpeech system for conversion to speech
        
    }

}
