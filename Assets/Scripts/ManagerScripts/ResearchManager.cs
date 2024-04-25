using RecipeManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchManager : MonoBehaviour
{
    public Dictionary<int, GameObject> Connections;
    public Dictionary<int, GameObject> Projects;
    public Transform ConnectionsList;
    public Transform ProjectsList;
    public List<GameObject> AvailableConnections;
    public List<GameObject> AvailableProjects;
    public Image fillImg;
    public CoroutineManager coroutineManager;
    public BuildingIncrementor buildingIncrementor;
    public static bool FirstTimeStarted = false;
    public static bool ResearchStarted = false;

    private List<ResearchDataJson> projectsDataList;

    [Serializable]
    private class JsonArray
    {
        public List<ResearchDataJson> projects;
    }

    private void Awake()
    {
        string jsonText = Assets.Scripts.Models.ResearchListJson.json;
        JsonArray jsonArray = JsonUtility.FromJson<JsonArray>(jsonText);
        projectsDataList = jsonArray?.projects;

        AvailableConnections = new List<GameObject>();
        AvailableProjects = new List<GameObject>();
    }

    public void InitializeResearchManager()
    {
        if (!FirstTimeStarted &&
            ConnectionsList.gameObject.activeSelf &&
            ProjectsList.gameObject.activeSelf)
        {
            FillResearchLists();
            InitializeConnectionsMap();
            InitializeProjectsMap();
            _ = StartCoroutine(nameof(ValidatedResearchProjects));
            FirstTimeStarted = true;
        }
    }

    /// <summary>
    /// Researches a specific science project based on his object name in Unity hierarchy.
    /// Also checks if it has any requirements and reduces the ResearchPoints as mentioned
    /// in the ResearchListJson.
    /// </summary>
    /// <param name="startingObject"></param>
    public void ResearchProject(GameObject startingObject)
    {
        if (!ResearchStarted && int.TryParse(startingObject.name, out int researchProjectID))
        {
            var projectData = projectsDataList[researchProjectID];

            if (projectData != null)
            {
                fillImg = startingObject.transform.Find("FillCircle").GetComponent<Image>();

                if (projectData.projectName == Constants.ScienceProjects)
                {
                    if (Planet0Buildings.Planet0ResearchDevice > 0)
                    {
                        _ = StartCoroutine(nameof(StartResearch), projectData);
                    }
                }
                else
                {
                    if (projectData.hasRequirements)
                    {
                        bool startProject = false;
                        foreach (var childData in projectData.requirementsList)
                        {
                            if (childData != null && childData.name == Constants.ResearchPoint &&
                                Player.ResearchPoints >= childData.quantity &&
                                Planet0Buildings.Planet0ResearchDevice > 0)
                            {
                                Player.ResearchPoints -= childData.quantity;
                                startProject = true;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (startProject)
                        {
                            _ = StartCoroutine(nameof(StartResearch), projectData);
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("Project data is null");
            }
        }
    }

    /// <summary>
    /// A simple unity research timer that also fills the image amount progress bar
    /// and in the end sets the science project as finished.
    /// Also checks the Player static class and sets a bool variable based on the
    /// project name to True.
    /// </summary>
    /// <param name="projectData"></param>
    /// <returns></returns>
    public IEnumerator StartResearch(ResearchDataJson projectData)
    {
        ResearchStarted = true;
        float startTime = 0f;

        while (startTime < projectData.researchTime)
        {
            float currentFillAmount = Mathf.Lerp(0f, 1f, startTime / projectData.researchTime);
            fillImg.fillAmount = currentFillAmount;
            startTime += Time.deltaTime;
            yield return null;
        }

        string thisName = projectData.projectName + "Research";

        if (projectData.projectName == Constants.ScienceProjects)
        {
            Player.ResearchPoints++;
        }
        else if (projectData.projectName == Constants.SteamPower)
        {
            Planet0Buildings.BoilerUnlocked = true;
            Planet0Buildings.SteamGeneratorUnlocked = true;
            buildingIncrementor.InitializeAvailableBuildings();
        }

        ResearchStarted = false;
        coroutineManager.researchPointsText.text = Player.ResearchPoints.ToString();

        // after research is finished, we make sure that a bool representing the research progress is also switched to true
        System.Reflection.FieldInfo targetVariable = typeof(Player).GetField(thisName);

        if (targetVariable != null && targetVariable.FieldType == typeof(bool))
        {
            targetVariable.SetValue(null, true);
        }

        _ = StartCoroutine(nameof(ValidatedResearchProjects));
    }

    /// <summary>
    /// Fills the list in ResearchManager with objects that are already created in Unity hierarchy
    /// in their Transform parents.
    /// </summary>
    private void FillResearchLists()
    {
        for (int i = 0; i < projectsDataList.Count; i++)
        {
            var projectData = projectsDataList[i];
            AvailableConnections.Add(ConnectionsList.Find(projectData?.index.ToString()).gameObject);
            AvailableProjects.Add(ProjectsList.Find(projectData?.index.ToString()).gameObject);
        }
    }

    private void InitializeConnectionsMap()
    {
        Connections = new Dictionary<int, GameObject>();

        for (int i = 0; i < projectsDataList.Count; i++)
        {
            var projectData = projectsDataList[i];
            Connections.Add(projectData.index, AvailableConnections[i]);
        }
    }

    private void InitializeProjectsMap()
    {
        Projects = new Dictionary<int, GameObject>();

        for (int i = 0; i < projectsDataList.Count; i++)
        {
            var projectData = projectsDataList[i];
            Projects.Add(projectData.index, AvailableProjects[i]);
        }
    }

    /// <summary>
    /// This validation is necessary when starting game or anytime we need to refresh
    /// the visibility of the objects based on the current state of the game progress.
    /// </summary>
    private IEnumerator ValidatedResearchProjects()
    {
        if (!Player.ScienceProjectsResearch)
        {
            foreach (var project in Projects.Keys)
            {
                if (project != 0)
                {
                    Projects.TryGetValue(project, out var projectObject);
                    projectObject.SetActive(false);
                }
            }

            foreach (var connection in Connections.Values)
            {
                connection.SetActive(false);
            }

            yield return null;
        }
        else
        {
            foreach (var project in Projects.Keys)
            {
                if (project > 0 && project < 7)
                {
                    Projects.TryGetValue(project, out var projectObject);
                    projectObject.SetActive(true);
                    Animation animation = projectObject.GetComponent<Animation>();
                    animation.Play("ResearchProject");
                    yield return new WaitForSeconds(0.1f);
                }
            }

            foreach (var connection in Connections.Keys)
            {
                if (connection > 0 && connection < 7)
                {
                    Connections.TryGetValue(connection, out var connectionObject);
                    connectionObject.SetActive(true);
                    Image image = connectionObject.GetComponent<Image>();
                    image.fillAmount = 0;
                    float currentAmount = 0f;
                    while (currentAmount < 0.2f)
                    {
                        float totalAmount = Mathf.Lerp(0f, 1f, currentAmount / 0.1f);
                        image.fillAmount += totalAmount;
                        currentAmount += Time.deltaTime;
                        yield return null;
                    }
                }
            }

            if (!Player.PoweredEngineeringResearch)
            {
                if (Connections.TryGetValue(7, out var connectionObject)) connectionObject.SetActive(false);
                if (Projects.TryGetValue(7, out var projectObject)) projectObject.SetActive(false);
            }
        }
    }
}
