using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEngine.Events;

public class AiChat : MonoBehaviour
{
    private string apiUrl = "https://7974-152-59-217-39.ngrok-free.app/v1/chat/completions"; // Your API endpoint
    public TextMeshProUGUI userInput, aiResponse;

    public UnityEvent<string> OnAIResponseReceived; // Event to notify when AI response is ready

    public void SendMessageToAI(string userMessage)
    {
        StartCoroutine(SendPostRequest(userMessage));
    }

    private IEnumerator SendPostRequest(string userMessage)
    {
        // Create the message data structure for the API request
        string jsonData = JsonUtility.ToJson(new ChatRequest
        {
            model = "therapistaismallestquantizednew",
            messages = new Message[] {
                new Message { role = "system", content = "You are a helpful Ai therapist created by Moderate Limited. Your name is Brian. Your purpose is to help user relax and practice mindfulness." },
                new Message { role = "user", content = userMessage }
            },
            temperature = 0.7f,
            max_tokens = 50,
            stream = false
        });

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Request failed: " + request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Response: " + responseText);

            AIResponse response = JsonUtility.FromJson<AIResponse>(responseText);
            if (response != null && response.choices.Length > 0)
            {
                string aiMessage = response.choices[0].message.content;
                OnAIResponseReceived?.Invoke(aiMessage); // Trigger the event with the AI's response
            }
            else
            {
                Debug.Log("No valid response from AI.");
            }
        }
    }

    [System.Serializable]
    public class ChatRequest
    {
        public string model;
        public Message[] messages;
        public float temperature;
        public int max_tokens;
        public bool stream;
    }

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    public class AIResponse
    {
        public string id;
        public string model;
        public Choice[] choices;
    }

    [System.Serializable]
    public class Choice
    {
        public int index;
        public Message message;
        public string finish_reason;
    }
}
