using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIRequestHandler : MonoBehaviour
{
    // URL of the API endpoint
    [SerializeField] private string apiUrl = "http://127.0.0.1:1234/v1/chat/completions";

    // Called to send the API request
    public void CallAPI()
    {
        StartCoroutine(SendGetRequest());
    }

    // Coroutine to handle the GET request
    private IEnumerator SendGetRequest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            // Send the request and wait for the response
            yield return webRequest.SendWebRequest();

            // Check if there was an error
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
            else
            {
                // Successfully received response
                Debug.Log($"Response: {webRequest.downloadHandler.text}");
            }
        }
    }
}
