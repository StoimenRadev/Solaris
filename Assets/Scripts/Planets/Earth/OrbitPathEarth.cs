using UnityEngine;

[ExecuteAlways]
public class OrbitPathEarth : MonoBehaviour
{
    [Header("Orbit Parameters — Earth")]
    [Tooltip("Distance from Sun to aphelion (in Unity units)")]
    public float aphelion = 3600f;
    [Tooltip("Distance from Sun to perihelion (in Unity units)")]
    public float perihelion = 3500f;
    [Range(0f, 0.99f)]
    public float eccentricity = 0.0167f;
    public float planetYearLength = 365.25f; // in Earth days

    [Header("Planet / Visuals")]
    [Tooltip("Use either a prefab to spawn a planet, or drag a pre-placed planet in the scene")]
    public GameObject planetPrefab;
    public Transform planetInstance; // drag your scene planet here if not using prefab

    [Header("Orbit Drawing")]
    public LineRenderer lineRenderer;
    public Material orbitMaterial;
    public int segments = 180;

    [Header("Time Reference")]
    public TimeDisplay timeDisplay; // reference to your TimeDisplay script
    [Range(0f, 365f)]
    public float manualDayOfYear = 0f;

    // Computed ellipse parameters
    [HideInInspector] public float a; // semi-major axis
    [HideInInspector] public float b; // semi-minor axis
    private float c; // focal distance

    void Awake()
    {
        if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();
        if (orbitMaterial != null) lineRenderer.material = orbitMaterial;

        lineRenderer.startColor = Color.magenta;
        lineRenderer.endColor = Color.magenta;

        CalculateOrbit();
        DrawOrbit();
        SpawnPlanet();
    }

    void Update()
    {
        if (planetInstance == null) return;

        float day = manualDayOfYear;
        if (timeDisplay != null)
            day = timeDisplay.DayOfYear;

        UpdatePlanetPosition(day);
    }

    #region Orbit Calculations
    public void CalculateOrbit()
    {
        a = (aphelion + perihelion) / 2f; // semi-major axis
        c = a * eccentricity;
        b = Mathf.Sqrt(a * a - c * c);
    }

    public void DrawOrbit()
    {
        if (!lineRenderer) return;
        lineRenderer.positionCount = segments + 1;

        for (int i = 0; i <= segments; i++)
        {
            float theta = 2f * Mathf.PI * i / segments;
            float x = a * Mathf.Cos(theta);
            float z = b * Mathf.Sin(theta);
            lineRenderer.SetPosition(i, new Vector3(x, 0f, z));
        }
    }

    public void SpawnPlanet()
    {
        if (planetInstance != null) return; // already assigned (scene planet)

        if (!planetPrefab) return; // no prefab assigned

        planetInstance = Instantiate(planetPrefab, Vector3.zero, Quaternion.identity, transform).transform;
    }

    public Transform GetPlanetInstance()
    {
        if (planetInstance == null)
            SpawnPlanet();
        return planetInstance;
    }

    public Vector3 GetPosition(float theta)
    {
        float x = a * Mathf.Cos(theta);
        float z = b * Mathf.Sin(theta);
        return new Vector3(x, 0f, z); // Y = 0 ensures orbit plane
    }

    public void UpdatePlanetPosition(float dayOfYear)
    {
        if (planetInstance == null) return;

        // Orbit fraction
        float orbitFraction = dayOfYear / planetYearLength;

        // Mean anomaly
        float M = orbitFraction * 2f * Mathf.PI;

        // Solve Kepler
        float E = SolveKepler(M, eccentricity);

        // True anomaly
        float theta = 2f * Mathf.Atan(Mathf.Sqrt((1 + eccentricity) / (1 - eccentricity)) * Mathf.Tan(E / 2f));

        Vector3 pos = GetPosition(theta);
        planetInstance.position = pos;
    }

    float SolveKepler(float M, float e, int maxIter = 10)
    {
        float E = M;
        for (int i = 0; i < maxIter; i++)
        {
            E = E - (E - e * Mathf.Sin(E) - M) / (1 - e * Mathf.Cos(E));
        }
        return E;
    }
    #endregion

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();
        if (orbitMaterial != null) lineRenderer.material = orbitMaterial;

        lineRenderer.startColor = Color.magenta;
        lineRenderer.endColor = Color.magenta;

        CalculateOrbit();
        DrawOrbit();
    }
#endif
}