using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2, 256)] public int resolution = 10;
    public bool autoUpdate = true;

    public enum FaceRenderMask
    {
        All,
        Top,
        Bottom,
        Left,
        Right,
        Front,
        Back
    };

    public FaceRenderMask faceRenderMask;

    [SerializeField] MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;
    [HideInInspector] public List<GameObject> eventObjects = new();

    [HideInInspector] public bool shapeSettingsFoldout;
    [HideInInspector] public bool colourSettingsFoldout;

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
                SetPlanetLayer(meshObj);
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

    private void FixMeshPosition()
    {
        for (int i = 0; i < 6; i++)
        {
            meshFilters[i].transform.localPosition = new Vector3(-12.44094f, 6.144372f, -1.966287f);
        }
    }

    private GameObject FindPlayerTemplate()
    {
        const string parentPath = "/PlanetParent";
        GameObject parent = GameObject.Find(parentPath);
        if (parent == null)
        {
            Debug.LogError($"parent not found: {parentPath}");
            throw new System.Exception($"parent not found: {parentPath}");
        }

        // iterate over parent's children
        foreach (Transform child in parent.transform)
        {
            if (child.gameObject.name == "PlayerOnPlanetTemplate")
            {
                return child.gameObject;
            }
        }
        Debug.LogError("template not found");
        throw new System.Exception("template not found");
    }

    private GameObject FindPlayer()
    {
        // iterate over parent's children
        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.name == "Player")
            {
                return child.gameObject;
            }
        }

        return null;
    }

    private void AddPlayerToPlanet()
    {
        // Add the player to the planet
        GameObject player = FindPlayerTemplate();
        // clone the player
        player = Instantiate(player);
        SetPlanetLayer(player);
        // set name of the player
        player.name = "Player";
        player.transform.parent = transform;
        player.transform.localPosition = new Vector3(-12.277f, 5.64f, -4.415f);
        // set the scale of the player
        player.transform.localScale = new Vector3(0.08888598f, 0.08888598f, 0.08888598f);
    }

    static void SetPlanetLayer(GameObject gobj)
    {
        int layer = LayerMask.NameToLayer("Planet");
        gobj.layer = layer;
        // ensure player is active
        gobj.SetActive(true);
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();

        if (!Application.isPlaying)
        {
            FixMeshPosition();
            GameObject player = FindPlayer();
            if (player == null)
            {
                AddPlayerToPlanet();
            }
        }
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
                // if in playing mode, spawn event objects on the surface
                if (Application.isPlaying)
                {
                    SpawnEventObjectsOnSurface(meshFilters[i].sharedMesh, 56);
                }
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

    private void AddEventObjectToList(GameObject eventObject)
    {
        EventIcon eventIcon = eventObject.GetComponent<EventIcon>();
        eventIcon.SetPlanet(gameObject);
        eventObjects.Add(eventObject);
    }

    public void RecreateEventObject(string Name, Vector3 position, EventIconType iconType, float currentQuantity, float maxQuantity)
    {
        GameObject eventObject = new(Name);
        eventObject.transform.parent = transform;
        eventObject.transform.position = position;
        var component = eventObject.AddComponent<EventIcon>();
        component.name = Name;
        component.iconType = iconType;
        component.CurrentQuantity = currentQuantity;
        component.MaxQuantity = maxQuantity;
        AddEventObjectToList(eventObject);
    }

    public void SpawnEventObjectsOnSurface(Mesh mesh, int numberOfObjects)
    {
        // Quit if in the editor
        if (!Application.isPlaying)
        {
            return;
        }

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

            if (elevation < -0.06f)
            {
                eventObject.name = "Whale";
                var component = eventObject.AddComponent<EventIcon>();
                component.iconType = EventIconType.Fish;
                component.MaxQuantity = Random.Range(500f, 2500f);
                component.Elevation = elevation;
            }
            else if (elevation < -0.03f)
            {
                var option = Random.Range(0, 3);
                if (option == 0)
                {
                    eventObject.name = "Kuleoma";
                    var component = eventObject.AddComponent<EventIcon>();
                    component.iconType = EventIconType.Fish;
                    component.MaxQuantity = Random.Range(1000f, 5000f);
                    component.Elevation = elevation;
                }
                else if (option == 1)
                {
                    eventObject.name = "Octopus";
                    var component = eventObject.AddComponent<EventIcon>();
                    component.iconType = EventIconType.Fish;
                    component.MaxQuantity = Random.Range(1000f, 4000f);
                    component.Elevation = elevation;
                }
                else
                {
                    eventObject.name = "Shark";
                    var component = eventObject.AddComponent<EventIcon>();
                    component.iconType = EventIconType.Fish;
                    component.MaxQuantity = Random.Range(600f, 2000f);
                    component.Elevation = elevation;
                }
            }
            else if (elevation < 0.0f)
            {
                var option = Random.Range(0, 2);
                if (option == 0)
                {
                    eventObject.name = "Fish";
                    var component = eventObject.AddComponent<EventIcon>();
                    component.iconType = EventIconType.Fish;
                    component.MaxQuantity = Random.Range(2000f, 10000f);
                    component.Elevation = elevation;
                }
                else
                {
                    eventObject.name = "SeaTurtle";
                    var component = eventObject.AddComponent<EventIcon>();
                    component.iconType = EventIconType.Fish;
                    component.MaxQuantity = Random.Range(1000f, 4000f);
                    component.Elevation = elevation;
                }
            }
            else if (elevation < 0.01f)
            {
                var option = Random.Range(0, 3);
                if (option == 0)
                {
                    eventObject.name = "SilicaSand";
                    var component = eventObject.AddComponent<EventIcon>();
                    component.iconType = EventIconType.Mineral;
                    component.MaxQuantity = Random.Range(500000f, 1000000f);
                    component.Elevation = elevation;
                }
                else if (option == 1)
                {
                    eventObject.name = "Limestone";
                    var component = eventObject.AddComponent<EventIcon>();
                    component.iconType = EventIconType.Mineral;
                    component.MaxQuantity = Random.Range(4000f, 10000f);
                    component.Elevation = elevation;
                }
                else
                {
                    eventObject.name = "Clay";
                    var component = eventObject.AddComponent<EventIcon>();
                    component.iconType = EventIconType.Mineral;
                    component.MaxQuantity = Random.Range(100000f, 200000f);
                    component.Elevation = elevation;
                }
            }
            else if (elevation < 0.03f)
            {
                var option = Random.Range(0, 4);
                if (option == 0)
                {
                    eventObject.name = "FibrousLeaves";
                    var component = eventObject.AddComponent<EventIcon>();
                    component.iconType = EventIconType.Plant;
                    component.MaxQuantity = Random.Range(500000f, 1000000f);
                    component.Elevation = elevation;
                }
                else if (option == 1)
                {
                    eventObject.name = "RedHorn";
                    var component = eventObject.AddComponent<EventIcon>();
                    component.iconType = EventIconType.FurAnimal;
                    component.MaxQuantity = Random.Range(1000f, 2000f);
                    component.Elevation = elevation;
                }
                else if (option == 2)
                {
                    eventObject.name = "Bantir";
                    var component = eventObject.AddComponent<EventIcon>();
                    component.iconType = EventIconType.MilkAnimal;
                    component.MaxQuantity = Random.Range(2000f, 3000f);
                    component.Elevation = elevation;
                }
                else
                {
                    eventObject.name = "ProteinBeans";
                    var component = eventObject.AddComponent<EventIcon>();
                    component.iconType = EventIconType.Plant;
                    component.MaxQuantity = Random.Range(1000f, 2000f);
                    component.Elevation = elevation;
                }
            }
            else
            {
                var option = Random.Range(0, 5);
                if (option == 0)
                {
                    eventObject.name = "IronOre";
                    var component = eventObject.AddComponent<EventIcon>();
                    component.iconType = EventIconType.Mineral;
                    component.MaxQuantity = Random.Range(1000f, 100000f);
                    component.Elevation = elevation;
                }
                else if (option == 1)
                {
                    eventObject.name = "CopperOre";
                    var component = eventObject.AddComponent<EventIcon>();
                    component.iconType = EventIconType.Mineral;
                    component.MaxQuantity = Random.Range(1000f, 100000f);
                    component.Elevation = elevation;
                }
                else if (option == 2)
                {
                    eventObject.name = "SilverOre";
                    var component = eventObject.AddComponent<EventIcon>();
                    component.iconType = EventIconType.Mineral;
                    component.MaxQuantity = Random.Range(1000f, 100000f);
                    component.Elevation = elevation;
                }
                else if (option == 3)
                {
                    eventObject.name = "GoldOre";
                    var component = eventObject.AddComponent<EventIcon>();
                    component.iconType = EventIconType.Mineral;
                    component.MaxQuantity = Random.Range(1000f, 100000f);
                    component.Elevation = elevation;
                }
                else if (option == 4)
                {
                    eventObject.name = "Coal";
                    var component = eventObject.AddComponent<EventIcon>();
                    component.iconType = EventIconType.Mineral;
                    component.MaxQuantity = Random.Range(1000f, 100000f);
                    component.Elevation = elevation;
                }
                else
                {
                    eventObject.name = "Stone";
                    var component = eventObject.AddComponent<EventIcon>();
                    component.iconType = EventIconType.Mineral;
                    component.MaxQuantity = Random.Range(1000f, 100000f);
                    component.Elevation = elevation;
                }
            }

            AddEventObjectToList(eventObject);
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