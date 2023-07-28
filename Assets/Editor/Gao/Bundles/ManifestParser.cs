using UnityEditor;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

using System.Collections.Generic;

namespace Gao.Bundles
{

    public class ManifestParser 
    {
        public static string ASSETS_ROOT_FOLDER_PATH = "Assets\\AssetBundles";

        public static string[] GetAllBundleNames()
        { 
            // read all files with 'manifest' file extension in the root folder
            string[] manifestFiles = Directory.GetFiles(ASSETS_ROOT_FOLDER_PATH, "*.manifest", SearchOption.TopDirectoryOnly);

            // strip extension from file names
            for (int i = 0; i < manifestFiles.Length; i++)
            {
                manifestFiles[i] = Path.GetFileNameWithoutExtension(manifestFiles[i]);
            }

            return manifestFiles;
        }

        public class ManifestData
        {
            public string[] Assets { get; set; }
        }

        public static ManifestData ParseManifest(string bundleName)
        {
            string manifestFilePath = Path.Combine(ASSETS_ROOT_FOLDER_PATH, bundleName + ".manifest");

            // read manifest file
            using (StreamReader reader = new StreamReader(manifestFilePath))
            {
                string manifestContent = reader.ReadToEnd();
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();
                Dictionary<string, object> manifestDataDict = deserializer.Deserialize<Dictionary<string, object>>(manifestContent);
                ManifestData manifestData = new ManifestData();

                if (manifestDataDict.ContainsKey("Assets"))
                {
                    var val = manifestDataDict["Assets"];
                    List<object> list = val as List<object>;

                    manifestData.Assets = new string[list.Count];
                    for (int i = 0; i < list.Count; i++)
                    {
                        string valStr = list[i] as string;
                        manifestData.Assets[i] = valStr;
                    }
                }

                return manifestData;
            }
        }

        public static ManifestData[] ParseManifests(string[] bundleNames)
        {
            ManifestData[] manifestDatas = new ManifestData[bundleNames.Length];
            int i = 0;
            foreach (string bundleName in bundleNames)
            {
                ManifestData manifestData = ParseManifest(bundleName);
                manifestDatas[i++] = manifestData;
            }
            return manifestDatas;
        }

        [MenuItem("Assets/Gao Test")]
        static void GaoTest()
        {
            string[] bundleNames = GetAllBundleNames();

            //@@@@@@@@@@@@@@@@@@@@
            Debug.Log("bundleNames.Length: " + bundleNames.Length);
            foreach (var name in bundleNames)
            {
                Debug.Log(name);
                ManifestData manifestData = ParseManifest(name);
                string str = JsonConvert.SerializeObject(manifestData, Formatting.Indented);
                Debug.Log(str);
            }
            //@@@@@@@@@@@@@@@@@@@@@@
        }

    }
}