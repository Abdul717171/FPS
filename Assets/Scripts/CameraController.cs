using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;
    private bool isCameraEnabled = true;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        // Toggle camera state when "e" key is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            isCameraEnabled = !isCameraEnabled;

            if (isCameraEnabled)
            {
                // Enable the camera
                mainCamera.gameObject.SetActive(true);
                mainCamera.enabled = true;
            }
            else
            {
                // Disable the camera
                mainCamera.enabled = false;
                // Find another camera in the scene and enable it
                Camera[] cameras = FindObjectsOfType<Camera>();
                foreach (Camera camera in cameras)
                {
                    if (camera != mainCamera)
                    {
                        camera.enabled = true;
                        break;
                    }
                }
            }
        }
    }
}
