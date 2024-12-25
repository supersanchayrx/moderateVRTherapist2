using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AIChatManager : MonoBehaviour
{
    // URL of the local API endpoint
    private string apiUrl = "http://127.0.0.1:1234/v1/chat/completions";

    // Start the coroutine to send a request to the API
    public void StartChat(string userMessage)
    {
        StartCoroutine(SendPostRequest(userMessage));
    }

    // Coroutine to send POST request
    private IEnumerator SendPostRequest(string userMessage)
    {
        // Create the message data structure for the API request
        string jsonData = JsonUtility.ToJson(new ChatRequest
        {
            model = "moderatetherapistmodel",
            messages = new Message[]
            {
                new Message { role = "system", content = "You are an AI therapist. Designed by Moderate LTD. Your name is Brian You are an empathetic non judgemental therapist designed to help user with mindfulness and relaxation." },
                new Message { role = "user", content = userMessage }
            },
            temperature = 0.7f,
            max_tokens = 200,  // Increased max_tokens for longer responses
            stream = false
        });

        // Create UnityWebRequest with the appropriate headers
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Wait for the request to complete
        yield return request.SendWebRequest();

        // Check for any errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Request failed: " + request.error);
        }
        else
        {
            // Parse the response JSON and log it
            string responseText = request.downloadHandler.text;
            Debug.Log("Response: " + responseText);

            // Optionally, you can extract specific data from the JSON response here
            AIResponse response = JsonUtility.FromJson<AIResponse>(responseText);
            if (response.choices.Length > 0)
            {
                Debug.Log("AI Response: " + response.choices[0].message.content);
            }
        }
    }
}

// Helper classes to represent the structure of the request and response

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
    public Choice[] choices;
}

[System.Serializable]
public class Choice
{
    public Message message;
}