using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRenderTexture : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera targetCamera; // The camera rendering to the Render Texture

    [Header("Render Texture Settings")]
    public RenderTexture targetRenderTexture; // The Render Texture to resize

    [Header("Default Dimensions")]
    public int defaultWidth = 10; // Default width
    public int defaultHeight = 500; // Default height

    [Header("Dynamic Dimensions")]
    public int[] widthArray = { 20, 50, 100, 200, 350 }; // Width options
    public int fixedHeight = 500; // Height remains fixed
    private int currentIndex = 0; // Index to track the array position

    void OnEnable()
    {
        // Ensure default size on entering Play Mode
        ResetToDefaultSize();
    }

    void Start()
    {
        if (targetCamera == null || targetRenderTexture == null)
        {
            Debug.LogError("Target Camera or Render Texture is not assigned!");
            return;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResizeRenderTexture();
        }
    }

    void OnDisable()
    {
        // Ensure default size on exiting Play Mode
        ResetToDefaultSize();
    }

    public void ResizeRenderTexture()
    {
        // Release the current Render Texture
        targetRenderTexture.Release();

        // Get the next width from the array
        int newWidth = widthArray[currentIndex];
        int newHeight = fixedHeight;

        // Update Render Texture dimensions
        SetRenderTextureSize(newWidth, newHeight);

        Debug.Log($"Render Texture resized to {newWidth}x{newHeight}");

        // Increment index and loop back if at the end
        currentIndex = (currentIndex + 1) % widthArray.Length;
    }

    void SetRenderTextureSize(int width, int height)
    {
        targetRenderTexture.width = width;
        targetRenderTexture.height = height;
        targetRenderTexture.Create(); // Re-create the Render Texture

        // Ensure the camera uses the updated Render Texture
        targetCamera.targetTexture = targetRenderTexture;
    }

    void ResetToDefaultSize()
    {
        if (targetRenderTexture != null)
        {
            targetRenderTexture.Release();
            targetRenderTexture.width = defaultWidth;
            targetRenderTexture.height = defaultHeight;
            targetRenderTexture.Create();

            // Ensure the camera uses the default Render Texture
            targetCamera.targetTexture = targetRenderTexture;
            currentIndex = 0; // Reset index to start from the first array element

            Debug.Log($"Render Texture reset to default size: {defaultWidth}x{defaultHeight}");
        }
    }
}

