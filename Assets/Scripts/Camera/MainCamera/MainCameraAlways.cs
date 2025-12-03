using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class MainCameraAlways : MonoBehaviour
{
    private Camera mainCam;

    void Awake()
    {
        mainCam = GetComponent<Camera>();

        // Always set tag
        gameObject.tag = "MainCamera";

        // Always set the main camera as active
        ForceAsMainCamera();
    }

    void Update()
    {
        // Keep enforcing it each frame (in case another camera is enabled)
        ForceAsMainCamera();
    }

    private void ForceAsMainCamera()
    {
        Camera[] allCameras = Camera.allCameras;

        foreach (Camera cam in allCameras)
        {
            if (cam != mainCam)
            {
                // disable additional cameras (planet cameras)
                cam.enabled = false;
            }
        }

        // this must be the active camera
        if (!mainCam.enabled)
            mainCam.enabled = true;

        // ensure it stays tagged correctly
        if (mainCam.tag != "MainCamera")
            mainCam.tag = "MainCamera";
    }
}
