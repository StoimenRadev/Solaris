using UnityEngine;

[ExecuteAlways]
public class OrbitPathSaturn : MonoBehaviour
{
    [Header("Orbit Parameters — Saturn")]
    public float a = 6000f;                        // semi-major axis
    [Range(0f, 0.99f)]
    public float eccentricity = 0.0565f;           // Saturn eccentricity
    public float planetYearLength = 10759.22f;       // Saturn year in Earth days
    public float startTheta = 1.55f;    // approximate Feb 19, 2026

    [Header("Planet / Visuals")]
    public GameObject planetPrefab;
    public Transform planetInstance;

    [Header("Orbit Drawing")]
    public LineRenderer lineRenderer;
    public Material orbitMaterial;
    public int segments = 180;

    [Header("Time Reference")]
    public TimeDisplay timeDisplay;
    [Range(0f, 365f)]
    public float manualDayOfYear = 0f;

    [HideInInspector] public float b;
    private float c;

    void Awake()
    {
        if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();
        if (orbitMaterial != null) lineRenderer.material = orbitMaterial;

        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;

        CalculateOrbit();
        DrawOrbit();
        SpawnPlanet();

        float day = manualDayOfYear;
        if (timeDisplay != null)
            day = timeDisplay.DayOfYear;

        UpdatePlanetPosition(day);
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
            float x = a * Mathf.Cos(theta) - c; // Sun at focus
            float z = b * Mathf.Sin(theta);
            lineRenderer.SetPosition(i, new Vector3(x, 0f, z));
        }
    }

    public void SpawnPlanet()
    {
        if (planetInstance != null) return;
        if (!planetPrefab) return;

        planetInstance = Instantiate(planetPrefab, Vector3.zero, Quaternion.identity, transform).transform;
    }

    public Vector3 GetPosition(float theta)
    {
        float x = a * Mathf.Cos(theta) - c;
        float z = b * Mathf.Sin(theta);
        return new Vector3(x, 0f, z);
    }

    public void UpdatePlanetPosition(float dayOfYear)
    {
        if (planetInstance == null) return;

        float orbitFraction = dayOfYear / planetYearLength;
        float M = orbitFraction * 2f * Mathf.PI;
        float E = SolveKepler(M, eccentricity);

        float theta = 2f * Mathf.Atan(Mathf.Sqrt((1 + eccentricity) / (1 - eccentricity)) * Mathf.Tan(E / 2f));
        theta += startTheta;
        theta = Mathf.Repeat(theta, 2f * Mathf.PI);

        planetInstance.position = GetPosition(theta);
    }

    float SolveKepler(float M, float e, int maxIter = 10)
    {
        float E = M;
        for (int i = 0; i < maxIter; i++)
            E = E - (E - e * Mathf.Sin(E) - M) / (1 - e * Mathf.Cos(E));
        return E;
    }
    #endregion

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();
        if (orbitMaterial != null) lineRenderer.material = orbitMaterial;

        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;

        CalculateOrbit();
        DrawOrbit();
    }
#endif
}