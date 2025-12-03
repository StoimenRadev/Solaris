using UnityEngine;

public class SmoothFreeCamera : MonoBehaviour
{
    [Header("Startup Options")]
    public bool autoPositionAtStart = false;
    public Vector3 startRotation = new Vector3(20f, 0f, 0f);
    public Transform sunTransform;
    public float sunRadius = 250f;
    public float startOffset = 100f;

    [Header("Movement")]
    public float moveSpeed = 10f;
    public float fastMultiplier = 3f;
    public float acceleration = 10f;

    [Header("Mouse Look")]
    public float sensitivity = 3f;
    public float lookSmooth = 12f;
    private Vector2 smoothVelocity;
    private Vector2 currentLook;

    [Header("Zoom (Scroll)")]
    public float zoomSpeed = 20f;
    public float zoomSmooth = 10f;
    private float zoomVelocity;

    private Vector3 currentVelocity;
    private Vector3 desiredVelocity;

    // NEW: camera control toggle
    private bool cameraControl = true;

    void Start()
    {
        if (autoPositionAtStart && sunTransform != null)
        {
            Vector3 offsetDirection = Vector3.back;
            transform.position = sunTransform.position + offsetDirection * (sunRadius + startOffset);
            transform.rotation = Quaternion.Euler(startRotation);
        }
        else
        {
            currentLook = new Vector2(transform.eulerAngles.y, -transform.eulerAngles.x);
        }

        // Cursor hidden by default while controlling camera
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Toggle camera control with Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cameraControl = !cameraControl;

            if (cameraControl)
            {
                // Resume camera control: hide cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                // Pause camera control: show cursor for UI
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        // Only allow camera movement when cameraControl is true
        if (cameraControl)
        {
            HandleLook();
            HandleMovement();
            HandleZoom();
        }
    }

    void HandleLook()
    {
        Vector2 mouseDelta = new Vector2(
            Input.GetAxisRaw("Mouse X"),
            Input.GetAxisRaw("Mouse Y")
        );

        smoothVelocity = Vector2.Lerp(smoothVelocity, mouseDelta, Time.deltaTime * lookSmooth);
        currentLook += smoothVelocity * sensitivity;

        currentLook.y = Mathf.Clamp(currentLook.y, -90f, 90f);
        transform.rotation = Quaternion.Euler(-currentLook.y, currentLook.x, 0f);
    }

    void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");   // A / D
        float z = Input.GetAxisRaw("Vertical");     // W / S

        float y = 0f;
        if (Input.GetKey(KeyCode.Space)) y += 1f;
        if (Input.GetKey(KeyCode.LeftControl)) y -= 1f;

        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) speed *= fastMultiplier;

        Vector3 target = (transform.forward * z) + (transform.right * x) + (transform.up * y);

        desiredVelocity = target.normalized * speed;
        currentVelocity = Vector3.Lerp(currentVelocity, desiredVelocity, Time.deltaTime * acceleration);
        transform.position += currentVelocity * Time.deltaTime;
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoomVelocity = Mathf.Lerp(zoomVelocity, scroll * zoomSpeed, Time.deltaTime * zoomSmooth);
        transform.position += transform.forward * zoomVelocity;
    }
}
