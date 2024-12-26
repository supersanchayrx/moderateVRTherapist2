/*using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class AiChat : MonoBehaviour
{
    public string apiUrl = "https://7974-152-59-217-39.ngrok-free.app/v1/chat/completions"; // Your API endpoint
    public TMP_InputField userInput, aiResponse;

    public UnityEvent<string> OnAIResponseReceived; // Event to notify when AI response is ready

    public bool responseRecieved=false;

    public async void SendMessageToAI(string userMessage)
    {
        Debug.Log("sending message"+userMessage);

        string aiMessage = await SendPostRequestAsync(userMessage);

        if (!string.IsNullOrEmpty(aiMessage))
        {
            OnAIResponseReceived?.Invoke(aiMessage); // Trigger the event with the AI's response
        }
    }

    private async Task<string> SendPostRequestAsync(string userMessage)
    {
        // Create the message data structure for the API request
        string jsonData = JsonUtility.ToJson(new ChatRequest
        {
            model = "therapistaismallestquantizednew",
            messages = new Message[]
            {
                new Message { role = "system", content = "You are a helpful Ai therapist created by Moderate Limited. Your name is Brian. Your purpose is to help user relax and practice mindfulness." },
                new Message { role = "user", content = userMessage }
            },
            temperature = 0.7f,
            max_tokens = 50,
            stream = false
        });

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Request failed: " + request.error);
                return null;
            }
            else
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("Response: " + responseText);
                aiResponse.text = responseText;
                responseRecieved = true;


                AIResponse response = JsonUtility.FromJson<AIResponse>(responseText);
                if (response != null && response.choices.Length > 0)
                {
                    return response.choices[0].message.content;
                }
                else
                {
                    Debug.Log("No valid response from AI.");
                    return null;
                }
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
*/

using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.Events;
using Newtonsoft.Json; // Use Newtonsoft.Json for robust JSON handling

public class AiChat : MonoBehaviour
{
    public string apiUrl = "https://7974-152-59-217-39.ngrok-free.app/v1/chat/completions"; // Your API endpoint
    public TMP_InputField userInput, aiResponse;

    public UnityEvent<string> OnAIResponseReceived; // Event to notify when AI response is ready

    public bool responseReceived = false;

    public async void SendMessageToAI(string userMessage)
    {
        Debug.Log("Sending message: " + userMessage);


        string aiMessage = await SendPostRequestAsync(userMessage);

        if (!string.IsNullOrEmpty(aiMessage))
        {
            OnAIResponseReceived?.Invoke(aiMessage); // Trigger the event with the AI's response
        }
    }

    /*private async Task<string> SendPostRequestAsync(string userMessage)
    {
        // Create the message data structure for the API request
        var chatRequest = new ChatRequest
        {
            model = "therapistaismallestquantizednew",
            messages = new Message[]
            {
                new Message { role = "system", content = "You are a helpful AI therapist created by Moderate Limited. Your name is Brian. Your purpose is to help users relax and practice mindfulness." },
                new Message { role = "user", content = userMessage }
            },
            temperature = 0.7f,
            max_tokens = 50,
            stream = false
        };

        string jsonData = JsonConvert.SerializeObject(chatRequest); // Use Newtonsoft.Json

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log("JSON Payload: " + jsonData);

            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Request failed: " + request.error);
                return null;
            }
            else
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("Response: " + responseText);
                aiResponse.text = responseText;
                responseReceived = true;

                AIResponse response = JsonConvert.DeserializeObject<AIResponse>(responseText); // Deserialize response
                if (response != null && response.choices.Length > 0)
                {
                    return response.choices[1].message.content;;
                }
                else
                {
                    Debug.Log("No valid response from AI.");
                    return null;
                }
            }
        }
    }*/

    private async Task<string> SendPostRequestAsync(string userMessage)
    {
        // Create the message data structure for the API request
        string jsonData = JsonUtility.ToJson(new ChatRequest
        {
            model = "therapistaismallestquantizednew",
            messages = new Message[]
            {
            new Message { role = "system", content = "You are a helpful AI therapist created by Moderate Limited. Your name is Brian. Your purpose is to help user relax and practice mindfulness." },
            new Message { role = "user", content = userMessage }
            },
            temperature = 0.7f,
            max_tokens = 50,
            stream = false
        });

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Request failed: " + request.error);
                return null;
            }
            else
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("Response: " + responseText);

                // Parse the JSON response to extract the AI's message content
                AIResponse response = JsonUtility.FromJson<AIResponse>(responseText);
                if (response != null && response.choices.Length > 0)
                {
                    string aiContent = response.choices[0].message.content; // Extract content
                    aiResponse.text = aiContent; // Set the AI response text
                    responseReceived = true;
                    return aiContent;
                }
                else
                {
                    Debug.Log("No valid response from AI.");
                    return null;
                }
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
