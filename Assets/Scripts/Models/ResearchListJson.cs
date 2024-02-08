namespace Assets.Scripts.Models
{
    internal class ResearchListJson
    {
        public static string json = @"
{
  ""header"": ""Available science projects"",
  ""projects"": [
    {
      ""index"": 0,
      ""projectName"": ""ScienceProject"",
      ""projectProduct"": ""BASIC"",
      ""projectType"": ""LABORATORY"",
      ""projectClass"": ""CLASS-F"",
      ""experience"": 1,
      ""researchTime"": 10,
      ""hasRequirements"": false,
      ""requirementsList"": [
        {
          ""index"": 23,
          ""name"": ""ResearchDevice"",
          ""product"": ""BASIC"",
          ""type"": ""LABORATORY"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 1
        }
      ],
      ""rewardsList"": [
        {
          ""index"": 0,
          ""name"": ""ResearchPoint"",
          ""product"": ""BASIC"",
          ""type"": ""LABORATORY"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 1
        }
      ]
    }
  ]
}
";
    }
}
