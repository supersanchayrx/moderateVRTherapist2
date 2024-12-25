using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TranscriberEnabler : MonoBehaviour
{
    // Start is called before the first frame update
    public InputActionProperty menuButton;
    public GameObject canvas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (menuButton.action.WasPressedThisFrame())
        {
            if(canvas.activeSelf == true)
            {
                canvas.SetActive(false);
            }
            else if(canvas.activeSelf == false)
            {
                canvas.SetActive(true);
            }
        }
    }
}
