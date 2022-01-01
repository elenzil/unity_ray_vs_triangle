using UnityEngine;

[ExecuteInEditMode]
public class MainController : MonoBehaviour
{
    [SerializeField]
    private MeshFilter TheTriangle;

    [SerializeField]
    private LineRenderer TheRay;

    [SerializeField]
    private GameObject TheIntersection;

    [SerializeField]
    private Transform[] TriangleCorners;

    [SerializeField]
    private Transform[] RayMarkers;

    [SerializeField]
    private Material MaterialIntersection;

    [SerializeField]
    private Material MaterialNoIntersection;

    private void Start()
    {
        if (TriangleCorners.Length != 3)
        {
            Debug.LogError("Must have 3 triangle corners");
        }
        if (RayMarkers.Length != 2)
        {
            Debug.LogError("Must have 2 ray markers");
        }
    }

    private void Update() {
        UpdateTriangle();
        UpdateRay();
        UpdateIntersection();
    }
    private void UpdateIntersection()
    {
        Vector3 dir = (RayMarkers[1].position - RayMarkers[0].position).normalized;
        Ray ray = new Ray(RayMarkers[0].position, dir);
        float t = IntersectionMath.IntersectRayTriangle(ray, TriangleCorners[0].position, TriangleCorners[1].position, TriangleCorners[2].position);

        if (float.IsNaN(t))
        {
            TheRay.material = MaterialNoIntersection;
            TheIntersection.SetActive(false);
        }
        else
        {
            TheRay.material = MaterialIntersection;
            TheIntersection.SetActive(true);
            TheIntersection.transform.position = ray.origin + t * ray.direction;
        }
    }

    private void UpdateRay()
    {
        TheRay.positionCount = 2;
        TheRay.SetPosition(0, RayMarkers[0].position);
        TheRay.SetPosition(1, RayMarkers[0].position + (RayMarkers[1].position - RayMarkers[0].position) * 100.0f);

    }

    private void UpdateTriangle() {
        Mesh m = TheTriangle.mesh;

        m.Clear();

        m.vertices = new Vector3[] {
            TriangleCorners[0].position,
            TriangleCorners[1].position,
            TriangleCorners[2].position,
            TriangleCorners[0].position,
            TriangleCorners[2].position,
            TriangleCorners[1].position,
        };

        m.uv = new Vector2[] {
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(0, 0),
            new Vector2(1, 1),
            new Vector2(0, 1),
        };

        m.triangles = new int[] { 0, 1, 2, 3, 4, 5 };

    }

}
