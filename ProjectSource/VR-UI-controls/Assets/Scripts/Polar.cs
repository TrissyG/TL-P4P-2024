using UnityEngine;

public class Polar
{
    public float radius, inclination, azimuth;

    public static Vector3 ToCartesian(float radius, float inclination, float azimuth)
    {
        float x = radius * Mathf.Sin(inclination * Mathf.Deg2Rad) * Mathf.Cos(azimuth * Mathf.Deg2Rad);
        float y = radius * Mathf.Cos(inclination * Mathf.Deg2Rad);
        float z = radius * Mathf.Sin(inclination * Mathf.Deg2Rad) * Mathf.Sin(azimuth * Mathf.Deg2Rad);
        return new Vector3(x, y, z);
    }

    public Polar(float radius, float inclination, float azimuth)
    {
        this.radius = radius;
        this.inclination = inclination;
        this.azimuth = azimuth;
    }

    public Vector3 ToCartesian()
    {
        return Polar.ToCartesian(radius, inclination, azimuth);
    }
}