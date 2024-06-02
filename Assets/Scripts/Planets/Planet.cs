using System;
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
    public List<GameObject> eventObjects = new();

    [HideInInspector] public bool shapeSettingsFoldout;
    [HideInInspector] public bool colourSettingsFoldout;

    ShapeGenerator shapeGenerator = new();
    ColourGenerator colourGenerator = new();

    readonly ResourceSpawn resourceSpawn = new();

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

    public void RefreshPlanet()
    {
        if (autoUpdate)
        {
            OnShapeSettingsUpdated();
            OnColourSettingsUpdated();
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh(false);
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
            GenerateMesh(true);
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

    void GenerateMesh(bool refresh)
    {
        if (!refresh)
        {
            CreateStartResources();
        }

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].ConstructMesh();
                // if in playing mode, spawn event objects on the surface
                if (Application.isPlaying && !refresh)
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

    public void RecreateEventObject(string Name, Vector3 position, EventIconType iconType, float currentQuantity, float minQuantityRange, float maxQuantityRange,
        int recipeIndex, int recipeIndex2, int recipeIndex3, int recipeIndex4, string recipeProduct, string recipeProduct2, string recipeProduct3, string recipeProduct4,
        Guid recipeGuid, Guid recipeGuid2, Guid recipeGuid3, Guid recipeGuid4, EventSize eventSize, int eventLevel)
    {
        GameObject eventObject = new(Name)
        {
            tag = "EventIcon"
        };
        eventObject.transform.parent = transform;
        eventObject.transform.localPosition = position;
        var component = eventObject.AddComponent<EventIcon>();
        component.name = Name;
        component.IconType = iconType;
        component.EventSize = eventSize;
        component.EventLevel = eventLevel;
        component.CurrentQuantity = currentQuantity;
        component.MinQuantityRange = minQuantityRange;
        component.MaxQuantityRange = maxQuantityRange;
        component.RecipeGuid = recipeGuid;
        component.RecipeGuid2 = recipeGuid2;
        component.RecipeGuid3 = recipeGuid3;
        component.RecipeGuid4 = recipeGuid4;
        component.RecipeIndex = recipeIndex;
        component.RecipeIndex2 = recipeIndex2;
        component.RecipeIndex3 = recipeIndex3;
        component.RecipeIndex4 = recipeIndex4;
        component.RecipeProduct = recipeProduct;
        component.RecipeProduct2 = recipeProduct2;
        component.RecipeProduct3 = recipeProduct3;
        component.RecipeProduct4 = recipeProduct4;
        SphereCollider sphereCollider = eventObject.AddComponent<SphereCollider>();
        sphereCollider.radius = 0.05f;
        sphereCollider.isTrigger = true;
        sphereCollider.includeLayers |= (1 << 0) | (1 << 7);
        AddEventObjectToList(eventObject);
    }

    public void SpawnEventObjectsOnSurface(Mesh mesh, int numberOfObjects)
    {
        GameObject player = transform.Find("Player").gameObject;
        Transform playerTransform = player.transform;
        DiscoveryManager discoveryManager = new();
        List<Vector3> existingCoordinates = new();

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

            // Create a new empty GameObject with a sphere collider
            GameObject eventObject = new("EventObject")
            {
                tag = "EventIcon"
            };
            SphereCollider sphereCollider = eventObject.AddComponent<SphereCollider>();
            sphereCollider.radius = 0.05f;
            sphereCollider.isTrigger = true;

            // Set the position of the eventObject at the random point
            eventObject.transform.position = randomPoint;

            // Make the new object a child of the planet's GameObject
            eventObject.transform.parent = transform;

            // Move the object on the z-axis by adding 3.6 to its current z position
            eventObject.transform.position += new Vector3(0f, 0f, 3.6f);

            existingCoordinates.Add(eventObject.transform.position);

            float distance = Vector3.Distance(eventObject.transform.position, playerTransform.position);

            // we prohibit od spawning any random resources near player location
            if (distance < 0.5f)
            {
                Destroy(eventObject);
                continue;
            }
            else
            {
                // check and avoid overlap of each event icon on the planet
                foreach (var coordinate in existingCoordinates)
                {
                    float distance2 = Vector3.Distance(coordinate, eventObject.transform.position);

                    if (distance2 == 0)
                    {
                        continue;
                    }
                    else if (distance2 < 0.05)
                    {
                        Vector3 offset = new(0.2f, 0f, 0f);
                        eventObject.transform.position += offset;
                    }
                }
            }

            if (elevation < -0.06f)
            {
                resourceSpawn.SpawnResource(eventObject, "Whale", EventIconType.Fish, elevation);
            }
            else if (elevation < -0.03f)
            {
                var option = UnityEngine.Random.Range(0, 3);
                if (option == 0)
                {
                    resourceSpawn.SpawnResource(eventObject, "Kuleoma", EventIconType.Fish, elevation);
                }
                else if (option == 1)
                {
                    resourceSpawn.SpawnResource(eventObject, "Octopus", EventIconType.Fish, elevation);
                }
                else
                {
                    resourceSpawn.SpawnResource(eventObject, "Shark", EventIconType.Fish, elevation);
                }
            }
            else if (elevation < 0.0f)
            {
                var option = UnityEngine.Random.Range(0, 2);
                if (option == 0)
                {
                    resourceSpawn.SpawnResource(eventObject, "SmallFish", EventIconType.Fish, elevation);
                }
                else
                {
                    resourceSpawn.SpawnResource(eventObject, "SeaTurtle", EventIconType.Fish, elevation);
                }
            }
            else if (elevation < 0.01f)
            {
                var option = UnityEngine.Random.Range(0, 3);
                if (option == 0)
                {
                    resourceSpawn.SpawnResource(eventObject, "SilicaSand", EventIconType.Mineral, elevation);
                }
                else if (option == 1)
                {
                    resourceSpawn.SpawnResource(eventObject, "Limestone", EventIconType.Mineral, elevation);
                }
                else
                {
                    resourceSpawn.SpawnResource(eventObject, "Clay", EventIconType.Mineral, elevation);
                }
            }
            else if (elevation < 0.03f)
            {
                var option = UnityEngine.Random.Range(0, 4);
                if (option == 0)
                {
                    resourceSpawn.SpawnResource(eventObject, "FibrousLeaves", EventIconType.Plant, elevation);
                }
                else if (option == 1)
                {
                    resourceSpawn.SpawnResource(eventObject, "RedHorn", EventIconType.FurAnimal, elevation);
                }
                else if (option == 2)
                {
                    resourceSpawn.SpawnResource(eventObject, "Bantir", EventIconType.MilkAnimal, elevation);
                }
                else if (option == 3)
                {
                    resourceSpawn.SpawnResource(eventObject, "Wood", EventIconType.Plant, elevation);
                }
                else if (option == 4)
                {
                    resourceSpawn.SpawnResource(eventObject, "ProteinBeans", EventIconType.Plant, elevation);
                }
            }
            else
            {
                var option = UnityEngine.Random.Range(0, 10);
                if (option == 0)
                {
                    resourceSpawn.SpawnResource(eventObject, "IronOre", EventIconType.Mineral, elevation);
                }
                else if (option == 1)
                {
                    resourceSpawn.SpawnResource(eventObject, "CopperOre", EventIconType.Mineral, elevation);
                }
                else if (option == 2)
                {
                    resourceSpawn.SpawnResource(eventObject, "SilverOre", EventIconType.Mineral, elevation);
                }
                else if (option == 3)
                {
                    resourceSpawn.SpawnResource(eventObject, "GoldOre", EventIconType.Mineral, elevation);
                }
                else if (option == 4)
                {
                    resourceSpawn.SpawnResource(eventObject, "Coal", EventIconType.Mineral, elevation);
                }
                else if (option == 5)
                {
                    resourceSpawn.SpawnResource(eventObject, "Wood", EventIconType.Plant, elevation);
                }
                else if (option == 6)
                {
                    resourceSpawn.SpawnResource(eventObject, "Sulfur", EventIconType.Plant, elevation);
                }
                else if (option == 7)
                {
                    resourceSpawn.SpawnResource(eventObject, "Saltpeter", EventIconType.Plant, elevation);
                }
                else if (option == 8)
                {
                    resourceSpawn.SpawnResource(eventObject, "Stone", EventIconType.Mineral, elevation);
                }
                else
                {
                    if (elevation > 0.03f)
                    {
                        discoveryManager.CreateRandomCave();
                        return;
                    }
                }
            }

            AddEventObjectToList(eventObject);
        }
    }

    private void CreateStartResources()
    {
        CreateManualEventObject("FibrousLeaves", -0.3f, -0.1f, 0f);
        CreateManualEventObject("Wood", -0.1f, -0.35f, 0f);
        CreateManualEventObject("IronOre", 0.12f, -0.28f, 0f);
        CreateManualEventObject("Coal", 0.3f, -0.2f, 0f);
    }

    /// <summary>
    /// Manually create a resource near player start location. Offsets are based off players XYZ coordinates.
    /// This is to spawn resources necessary to perform the first tasks, like FibrousLeaves, Coal, Wood and IronOre.
    /// </summary>
    /// <param name="resourceName"></param>
    /// <param name="offsetX"></param>
    /// <param name="offsetY"></param>
    /// <param name="offsetZ"></param>
    private void CreateManualEventObject(string resourceName, float offsetX, float offsetY, float offsetZ)
    {
        GameObject player = transform.Find("Player").gameObject;
        Transform playerTransform = player.transform;

        GameObject eventObjectStart = new("EventObject")
        {
            tag = "EventIcon"
        };

        eventObjectStart.transform.parent = transform;

        SphereCollider sphereCollider = eventObjectStart.AddComponent<SphereCollider>();
        sphereCollider.radius = 0.05f;
        sphereCollider.isTrigger = true;

        Vector3 positionOffset = new(offsetX, offsetY, offsetZ);
        eventObjectStart.transform.SetPositionAndRotation(playerTransform.position + positionOffset, playerTransform.rotation);
        eventObjectStart.transform.localScale = playerTransform.localScale;

        if (resourceName == "FibrousLeaves")
        {
            resourceSpawn.SpawnResource(eventObjectStart, "FibrousLeaves", EventIconType.Plant, 0);
        }
        else if (resourceName == "Wood")
        {
            resourceSpawn.SpawnResource(eventObjectStart, "Wood", EventIconType.Plant, 0);
        }
        else if (resourceName == "IronOre")
        {
            resourceSpawn.SpawnResource(eventObjectStart, "IronOre", EventIconType.Mineral, 0);
        }
        else if (resourceName == "Coal")
        {
            resourceSpawn.SpawnResource(eventObjectStart, "Coal", EventIconType.Mineral, 0);
        }

        AddEventObjectToList(eventObjectStart);
    }

    public void CreateDungeonObject(EventSize eventSize, EventIconType eventType, int eventLevel,
        float offsetX, float offsetY, float offsetZ)
    {
        GameObject player = transform.Find("Player").gameObject;
        Transform playerTransform = player.transform;

        GameObject eventObjectStart = new("EventObject")
        {
            tag = "EventIcon"
        };

        eventObjectStart.transform.parent = transform;

        SphereCollider sphereCollider = eventObjectStart.AddComponent<SphereCollider>();
        sphereCollider.radius = 0.05f;
        sphereCollider.isTrigger = true;

        Vector3 positionOffset = new(offsetX, offsetY, offsetZ);
        eventObjectStart.transform.SetPositionAndRotation(playerTransform.position + positionOffset, playerTransform.rotation);
        eventObjectStart.transform.localScale = playerTransform.localScale;

        if (eventType == EventIconType.VolcanicCave)
        {
            resourceSpawn.SpawnEvent(eventObjectStart, "VolcanicCave", eventSize, EventIconType.VolcanicCave, eventLevel);
        }
        else if (eventType == EventIconType.IceCave)
        {
            resourceSpawn.SpawnEvent(eventObjectStart, "IceCave", eventSize, EventIconType.IceCave, eventLevel);
        }
        else if (eventType == EventIconType.HiveNest)
        {
            resourceSpawn.SpawnEvent(eventObjectStart, "HiveNest", eventSize, EventIconType.HiveNest, eventLevel);
        }
        else if (eventType == EventIconType.CyberHideout)
        {
            resourceSpawn.SpawnEvent(eventObjectStart, "CyberHideout", eventSize, EventIconType.CyberHideout, eventLevel);
        }

        AddEventObjectToList(eventObjectStart);
    }

    Vector3 GetRandomPointOnMesh(Mesh mesh)
    {
        // Get a random triangle from the mesh
        int triangleIndex = UnityEngine.Random.Range(0, mesh.triangles.Length / 3);
        int vertexIndex1 = mesh.triangles[triangleIndex * 3];
        int vertexIndex2 = mesh.triangles[triangleIndex * 3 + 1];
        int vertexIndex3 = mesh.triangles[triangleIndex * 3 + 2];

        // Get random barycentric coordinates
        float u = UnityEngine.Random.Range(0f, 1f);
        float v = UnityEngine.Random.Range(0f, 1f - u);

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