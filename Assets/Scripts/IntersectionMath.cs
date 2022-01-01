using UnityEngine;

public static class IntersectionMath
{
    const float kEpsilon = 0.000001f;

    /// <summary>
    /// Ray-versus-triangle intersection test suitable for ray-tracing etc.
    /// Port of Möller–Trumbore algorithm c++ version from:
    /// https://en.wikipedia.org/wiki/Möller–Trumbore_intersection_algorithm
    /// </summary>
    /// <returns><c>The distance along the ray to the intersection</c> if one exists, <c>NaN</c> if one does not.</returns>
    /// <param name="ray">Le ray.</param>
    /// <param name="v0">A vertex of the triangle.</param>
    /// <param name="v1">A vertex of the triangle.</param>
    /// <param name="v2">A vertex of the triangle.</param>
    public static float IntersectRayTriangle(Ray ray, Vector3 v0, Vector3 v1, Vector3 v2)
    {

        // edges from v1 & v2 to v0.     
        Vector3 e1 = v1 - v0;
        Vector3 e2 = v2 - v0;

        Vector3 h = Vector3.Cross(ray.direction, e2);
        float a = Vector3.Dot(e1, h);
        if ((a > -kEpsilon) && (a < kEpsilon))
        {
            return float.NaN;
        }

        float f = 1.0f / a;

        Vector3 s = ray.origin - v0;
        float u = f * Vector3.Dot(s, h);
        if ((u < 0.0f) || (u > 1.0f))
        {
            return float.NaN;
        }

        Vector3 q = Vector3.Cross(s, e1);
        float v = f * Vector3.Dot(ray.direction, q);
        if ((v < 0.0f) || (u + v > 1.0f))
        {
            return float.NaN;
        }

        float t = f * Vector3.Dot(e2, q);
        if (t > kEpsilon)
        {
            return t;
        }
        else
        {
            return float.NaN;
        }
    }

    /// <author>Dollarslice - http://answers.unity.com/comments/1879009/view.html</author>
    public static bool IntersectRayTriangle(
          in Ray ray,
          in Vector3 v0,
          in Vector3 v1,
          in Vector3 v2,
          out Vector3 IntersectionPoint)
    {
        IntersectionPoint = Vector3.zero;

        Vector3 rayOrigin = ray.origin;
        Vector3 rayVector = ray.direction;

        const float EPSILON = 0.0000001f;
        Vector3 edge1, edge2, h, s, q;
        float a, f, u, v;

        edge1 = v1 - v0;
        edge2 = v2 - v0;
        h = Vector3.Cross(rayVector, edge2);
        a = Vector3.Dot(edge1, h);
        if (a > -EPSILON && a < EPSILON)
            return false;    // This ray is parallel to this triangle.
        f = 1.0f / a;
        s = rayOrigin - v0;
        u = f * Vector3.Dot(s, h);
        if (u < 0.0f || u > 1.0f)
            return false;
        q = Vector3.Cross(s, edge1);
        v = f * Vector3.Dot(rayVector, q);
        if (v < 0.0f || u + v > 1.0f)
            return false;
        // At this stage we can compute t to find out where the intersection point is on the line.
        float t = f * Vector3.Dot(edge2, q);
        if (t > EPSILON) // ray intersection
        {
            IntersectionPoint = rayOrigin + rayVector * t;
            return true;
        }
        else // This means that there is a line intersection but not a ray intersection.
            return false;
    }
}
