using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitPathColor : MonoBehaviour
{
    public Color defaultColor = Color.violet; // default orbit color
    private LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        SetColor(defaultColor); // initialize
    }

    public void SetColor(Color c)
    {
        if (lr != null)
        {
            lr.startColor = c;
            lr.endColor = c;
            if (lr.material != null) lr.material.color = c; // ensure material matches
        }
    }

    public void RestoreDefault()
    {
        SetColor(defaultColor);
    }
}
