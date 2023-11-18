using System.Collections.Generic;
using UnityEngine;
public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;
    public bool autoUpdate = true;
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
    public FaceRenderMask faceRenderMask;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;
    public List<GameObject> eventObjects = new();

    [HideInInspector]
    public bool shapeSettingsFoldout;
    [HideInInspector]
    public bool colourSettingsFoldout;

    ShapeGenerator shapeGenerator = new();
    ColourGenerator colourGenerator = new();

    void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colourGenerator.UpdateSettings(colourSettings);
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] direction = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;

            terrainFaces[i] = new(shapeGenerator, meshFilters[i].sharedMesh, resolution, direction[i]);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }
    public void OnColourSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColours();
        }
    }

    void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].ConstructMesh();
                SpawnEventObjectsOnSurface(meshFilters[i].sharedMesh, 4);
            }
        }

        colourGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    void GenerateColours()
    {
        colourGenerator.UpdateColours();
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].UpdateUVs(colourGenerator);
            }
        }
    }

    public void RecreateEventObject(string Name, Vector3 position)
    {
        GameObject eventObject = new(Name);
        eventObject.transform.parent = transform;
        eventObject.transform.position = position;
        eventObject.AddComponent<EventIcon>();
        eventObjects.Add(eventObject);
    }

    public void SpawnEventObjectsOnSurface(Mesh mesh, int numberOfObjects)
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            // Get a random point on the mesh surface
            Vector3 randomPoint = GetRandomPointOnMesh(mesh);

            // Use the ShapeGenerator to calculate the elevation at the random point
            float elevation = shapeGenerator.CaulculateUnscaledElevation(randomPoint.normalized);

            // Create a new empty GameObject
            GameObject eventObject = new("EventObject");

            // Set the position of the eventObject at the random point
            eventObject.transform.position = randomPoint;

            // Make the new object a child of the planet's GameObject
            eventObject.transform.parent = transform;

            // Move the object on the z-axis by adding 3.6 to its current z position
            eventObject.transform.position += new Vector3(0f, 0f, 3.6f);

            if (elevation < 0.0f)
            {
                Debug.Log("Ocean EventObject at elevation: " + elevation);
                eventObject.name = "Fish";

            }
            else if (elevation < 0.02f)
            {
                Debug.Log("Land EventObject at elevation: " + elevation);
                eventObject.name = "FibrousLeaves";
            }
            else
            {
                Debug.Log("Mountain EventObject at elevation: " + elevation);
                eventObject.name = "IronOre";
            }
            eventObject.AddComponent<EventIcon>();
            eventObjects.Add(eventObject);
        }
    }

    Vector3 GetRandomPointOnMesh(Mesh mesh)
    {
        // Get a random triangle from the mesh
        int triangleIndex = Random.Range(0, mesh.triangles.Length / 3);
        int vertexIndex1 = mesh.triangles[triangleIndex * 3];
        int vertexIndex2 = mesh.triangles[triangleIndex * 3 + 1];
        int vertexIndex3 = mesh.triangles[triangleIndex * 3 + 2];

        // Get random barycentric coordinates
        float u = Random.Range(0f, 1f);
        float v = Random.Range(0f, 1f - u);

        // Calculate the barycentric coordinates for the third vertex
        float w = 1 - u - v;

        // Calculate the random point on the triangle using barycentric coordinates
        Vector3 vertex1 = mesh.vertices[vertexIndex1];
        Vector3 vertex2 = mesh.vertices[vertexIndex2];
        Vector3 vertex3 = mesh.vertices[vertexIndex3];

        Vector3 randomPoint = u * vertex1 + v * vertex2 + w * vertex3;

        return randomPoint;
    }

}


