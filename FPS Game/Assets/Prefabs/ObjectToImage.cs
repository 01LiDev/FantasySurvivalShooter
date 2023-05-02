using System.IO;
using UnityEngine;
using UnityEditor;

public class ObjectToImage : MonoBehaviour
{
    public GameObject objectToCapture;
    public int imageWidth = 512;
    public int imageHeight = 512;
    public string outputPath = "Assets/ObjectImage.png";

    [ContextMenu("Capture Object Image")]
    public void CaptureObjectImage()
    {
        // Create a temporary camera for capturing the image
        GameObject tempCameraObject = new GameObject("Temporary Camera");
        Camera tempCamera = tempCameraObject.AddComponent<Camera>();

        // Set the temporary camera's position and rotation to match the main camera
        tempCamera.transform.position = Camera.main.transform.position;
        tempCamera.transform.rotation = Camera.main.transform.rotation;

        // Set the camera's background color to transparent
        tempCamera.clearFlags = CameraClearFlags.SolidColor;
        tempCamera.backgroundColor = new Color(0, 0, 0, 0);

        // Create a RenderTexture to store the captured image
        RenderTexture renderTexture = new RenderTexture(imageWidth, imageHeight, 24);
        renderTexture.antiAliasing = 8;
        tempCamera.targetTexture = renderTexture;

        // Set the object layer to a unique layer, so the camera only renders the object
        int objectLayer = 31;
        objectToCapture.layer = objectLayer;
        SetLayerRecursively(objectToCapture.transform, objectLayer);
        tempCamera.cullingMask = 1 << objectLayer;

        // Render the object using the temporary camera
        tempCamera.Render();

        // Read the pixels from the RenderTexture and create a Texture2D
        RenderTexture.active = renderTexture;
        Texture2D texture = new Texture2D(imageWidth, imageHeight, TextureFormat.RGBA32, false);
        texture.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
        texture.Apply();

        // Save the Texture2D as a PNG image
        byte[] pngData = texture.EncodeToPNG();
        File.WriteAllBytes(outputPath, pngData);

        // Clean up
        RenderTexture.active = null;
        DestroyImmediate(renderTexture);
        DestroyImmediate(tempCameraObject);

        Debug.Log($"Object image saved to: {outputPath}");
    }

    private void SetLayerRecursively(Transform transform, int layer)
    {
        transform.gameObject.layer = layer;

        foreach (Transform child in transform)
        {
            SetLayerRecursively(child, layer);
        }
    }
}
