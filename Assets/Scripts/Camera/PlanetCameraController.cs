using UnityEngine;
using UnityEngine.EventSystems; // Needed for UI detection

public class PlanetCameraController : MonoBehaviour
{
    [Header("Default camera distance")]
    public float defaultDistance = 100f;

    [Header("Camera initial offset direction (world space)")]
    public Vector3 offsetDirection = new Vector3(0, 0, -1);

    [Header("Rotation settings")]
    public float rotationSpeed = 100f;

    [Header("Free move settings")]
    public float moveSpeed = 5f;
    public float boostMultiplier = 2f;
    public float zoomSpeed = 5f;
    public float verticalSpeed = 3f;

    [Header("Smooth return settings")]
    public float returnSpeed = 5f;

    private Transform targetPlanet = null;
    private float distanceFromSurface;
    private float yaw = 0f;
    private float pitch = 0f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isReturning = false;
    private bool blockMovementThisFrame = false;

    private enum CameraState
    {
        CursorMode,
        FreeMove,
        PlanetRotate
    }

    private CameraState currentState = CameraState.CursorMode;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        distanceFromSurface = defaultDistance;
        EnterCursorMode();
    }

    void Update()
    {
        HandleModeSwitch();

        if (isReturning)
        {
            SmoothReturnToStart();
            return;
        }

        switch (currentState)
        {
            case CameraState.FreeMove:
                FreeMoveControls();
                break;
            case CameraState.PlanetRotate:
                PlanetRotateControls();
                break;
            case CameraState.CursorMode:
                if (targetPlanet != null)
                    PlanetZoomOnly();
                break;
        }
    }

    #region Mode Switching
    void HandleModeSwitch()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            switch (currentState)
            {
                case CameraState.CursorMode:
                    if (targetPlanet != null)
                        EnterPlanetRotateMode(); // Cursor → Planet-follow
                    else
                        EnterFreeMoveMode();     // Cursor → Free-move
                    break;

                case CameraState.FreeMove:
                case CameraState.PlanetRotate:
                    EnterCursorMode();           // Free-move or Planet → Cursor
                    break;
            }
        }
    }

    void EnterCursorMode()
    {
        currentState = CameraState.CursorMode;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void EnterFreeMoveMode()
    {
        currentState = CameraState.FreeMove;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        blockMovementThisFrame = true; // <--- PREVENT MOVEMENT ON ENTRY
    }

    void EnterPlanetRotateMode()
    {
        currentState = CameraState.PlanetRotate;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    #endregion

    #region Free Move
    void FreeMoveControls()
    {
        // Block movement for the first frame after entering free mode
        if (blockMovementThisFrame)
        {
            blockMovementThisFrame = false;
            return;
        }

        // Completely block camera movement if interacting with UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            speed *= boostMultiplier;

        Vector3 move = transform.forward * Input.GetAxis("Vertical") +
                       transform.right * Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.Space)) move += Vector3.up * verticalSpeed;
        if (Input.GetKey(KeyCode.LeftControl)) move -= Vector3.up * verticalSpeed;

        transform.position += move * speed * Time.deltaTime;

        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, -85f, 85f);
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        transform.position += transform.forward * Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
    }
    #endregion

    #region Planet Follow
    public void SetTargetPlanet(Transform planet)
    {
        if (planet == null) return;

        // Cancel any ongoing return
        isReturning = false;

        targetPlanet = planet;

        // Check for PlanetData to get custom distance
        PlanetData data = planet.GetComponent<PlanetData>();
        distanceFromSurface = (data != null) ? data.cameraDistance : defaultDistance;

        Vector3 dir = offsetDirection.normalized;
        pitch = Mathf.Asin(dir.y) * Mathf.Rad2Deg;
        yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        UpdatePlanetCameraPosition();
        EnterPlanetRotateMode();
    }

    void PlanetRotateControls()
    {
        if (targetPlanet == null) return;

        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, -85f, 85f);
        }

        distanceFromSurface -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        distanceFromSurface = Mathf.Max(0.5f, distanceFromSurface);

        UpdatePlanetCameraPosition();
    }

    void PlanetZoomOnly()
    {
        if (targetPlanet == null) return;

        distanceFromSurface -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        distanceFromSurface = Mathf.Max(0.5f, distanceFromSurface);

        UpdatePlanetCameraPosition();
    }

    void UpdatePlanetCameraPosition()
    {
        if (targetPlanet == null) return;

        Vector3 direction = Quaternion.Euler(pitch, yaw, 0f) * Vector3.forward;
        transform.position = targetPlanet.position + direction.normalized * distanceFromSurface;
        transform.LookAt(targetPlanet.position);
    }
    #endregion

    #region Return to Start
    public void ReturnToStartPosition()
    {
        if (isReturning) return;
        isReturning = true;
        targetPlanet = null;
        EnterCursorMode();
    }

    void SmoothReturnToStart()
    {
        transform.position = Vector3.Lerp(transform.position, startPosition, Time.deltaTime * returnSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, Time.deltaTime * returnSpeed);

        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        if (Vector3.Distance(transform.position, startPosition) < 0.01f &&
            Quaternion.Angle(transform.rotation, startRotation) < 0.1f)
        {
            transform.position = startPosition;
            transform.rotation = startRotation;
            isReturning = false;
            distanceFromSurface = defaultDistance;
        }
    }
    #endregion

    #region Single Button Transit
    /// <summary>
    /// Call this for a single button that toggles between planet and start
    /// </summary>
    public void TogglePlanetTransit(Transform planet)
    {
        if (targetPlanet == null)
        {
            SetTargetPlanet(planet);
        }
        else
        {
            ReturnToStartPosition();
        }
    }
    #endregion
}
