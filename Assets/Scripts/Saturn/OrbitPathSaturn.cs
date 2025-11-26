using UnityEngine;

[ExecuteAlways]
public class OrbitPathSaturn : MonoBehaviour
{
    [Header("Orbit Parameters")]
    public float aphelion = 1500f; // distance from Sun to aphelion
    public float perihelion = 1000f; // distance from Sun to perihelion
    [Range(0f, 0.99f)]
    public float eccentricity = 0.5f;

    [Header("Visual Settings")]
    public int segments = 180; // number of points to draw orbit
    public LineRenderer lineRenderer;

    [Header("Computed")]
    public float a; // semi-major axis
    public float b; // semi-minor axis
    public float c; // distance from center to focus
    public Vector3 focus1; // left focus
    public Vector3 focus2; // right focus

    void Awake()
    {
        if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();
        CalculateOrbit();
        DrawOrbit();
    }

    public void CalculateOrbit()
    {
        // Semi-major axis
        a = (aphelion + perihelion) / 2f;

        // Foci distance from center
        c = a * eccentricity;

        // Semi-minor axis from eccentricity
        b = Mathf.Sqrt(a * a - c * c);

        // Foci positions along x-axis
        focus1 = new Vector3(-c, 0f, 0f);
        focus2 = new Vector3(c, 0f, 0f);
    }

    public void DrawOrbit()
    {
        if (!lineRenderer) return;

        lineRenderer.positionCount = segments + 1;

        for (int i = 0; i <= segments; i++)
        {
            float theta = 2f * Mathf.PI * i / segments;
            float x = a * Mathf.Cos(theta);
            float y = b * Mathf.Sin(theta);
            lineRenderer.SetPosition(i, new Vector3(x, 0f, y));
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        CalculateOrbit();
        DrawOrbit();
    }
#endif
}
