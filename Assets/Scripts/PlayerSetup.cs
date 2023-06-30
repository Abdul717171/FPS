using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class PlayerSetup : MonoBehaviour
{
    public Movement movement;
    public Camera playerCamera;
    private bool isCameraEnabled = true;

    void Start()
    {
        movement = FindAnyObjectByType<Movement>();
        playerCamera = GetComponent<Camera>();
    }
    private void Update()
    {
        // Toggle camera state when "e" key is pressed
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            isCameraEnabled = !isCameraEnabled;

            if (isCameraEnabled)
            {
                // Enable the camera
                playerCamera.gameObject.SetActive(true);
                playerCamera.enabled = true;
            }
            else
            {
                // Disable the camera
                playerCamera.enabled = false;
                // Find another camera in the scene and enable it
                Camera[] cameras = FindObjectsOfType<Camera>();
                foreach (Camera camera in cameras)
                {
                    if (camera != playerCamera)
                    {
                        camera.enabled = true;
                        break;
                    }
                }
            }
        }
    }
    public void isLocalPlayer()
    {
        movement.enabled = true;
        playerCamera.gameObject.SetActive(true);
    }
}
