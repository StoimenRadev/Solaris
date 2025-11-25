using UnityEngine;

public class FreeMovableCamera : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float fastMultiplier = 3f;
    public float acceleration = 10f;
    public float damping = 10f;

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

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
        HandleZoom();
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
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            speed *= fastMultiplier;

        Vector3 target = (transform.forward * z + transform.right * x).normalized;

        desiredVelocity = target * speed;

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
