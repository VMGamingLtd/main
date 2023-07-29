using UnityEditor;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Gao.Bundles
{
    public class MD5HashCalculator
    {
        public static string CalculateMD5Hash(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = System.IO.File.OpenRead(filePath))
                {
                    byte[] hashBytes = md5.ComputeHash(stream);
                    StringBuilder sb = new StringBuilder();

                    foreach (byte b in hashBytes)
                    {
                        sb.Append(b.ToString("x2"));
                    }

                    return sb.ToString();
                }
            }
        }
    }


    public class ManifestParser
    {
        public static string ASSETS_ROOT_FOLDER_PATH = @"Assets\AssetBundles";

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

        public class ManifestUnity
        {
            public string BundleName { get; set; }
            public string[] Assets { get; set; }
        }

        public class ManifestGao
        {
            public class AssetGao
            {
                public string FilePath { get; set; }
                public string Hash { get; set; }
            }

            public string BundleName { get; set; }

            public AssetGao[] Assets { get; set; }
        }

        public static ManifestUnity ParseManifest(string bundleName)
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
                ManifestUnity manifestData = new ManifestUnity();
                manifestData.BundleName = bundleName;

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
        public static ManifestUnity[] ParseManifests(string[] bundleNames)
        {
            ManifestUnity[] manifestDatas = new ManifestUnity[bundleNames.Length];
            int i = 0;
            foreach (string bundleName in bundleNames)
            {
                ManifestUnity manifestData = ParseManifest(bundleName);
                manifestDatas[i++] = manifestData;
            }
            return manifestDatas;
        }

        public static ManifestGao MakeManifestGao(ManifestUnity manifestUnity)
        {
            ManifestGao manifestGao = new ManifestGao();
            manifestGao.BundleName = manifestUnity.BundleName;

            if (manifestUnity.Assets != null)
            {
                manifestGao.Assets = new ManifestGao.AssetGao[manifestUnity.Assets.Length];
                for (int i = 0; i < manifestUnity.Assets.Length; i++)
                {
                    string assetPath = manifestUnity.Assets[i];
                    string assetFilePath = Path.Combine(assetPath);
                    string assetHash = MD5HashCalculator.CalculateMD5Hash(assetFilePath);
                    ManifestGao.AssetGao assetGao = new ManifestGao.AssetGao();
                    assetGao.FilePath = assetPath;
                    assetGao.Hash = assetHash;
                    manifestGao.Assets[i] = assetGao;
                }
            }

            return manifestGao;
        }

        public static ManifestGao[] MakeManifestsGao(ManifestUnity[] manifestUnitys)
        {
            ManifestGao[] manifestGaos = new ManifestGao[manifestUnitys.Length];
            int i = 0;
            foreach (ManifestUnity manifestUnity in manifestUnitys)
            {
                ManifestGao manifestGao = MakeManifestGao(manifestUnity);
                manifestGaos[i++] = manifestGao;
            }
            return manifestGaos;
        }

        public static void WriteManifestGao(ManifestGao manifestGao)
        {
            // Convert mainifestGao to yaml
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            string manifestGaoYaml = serializer.Serialize(manifestGao);

            string manifestGaoFilePath = Path.Combine(ASSETS_ROOT_FOLDER_PATH, $"{manifestGao.BundleName}.manifest.gao");

            // write manifestGaoYaml string content to manifestGaoFilePath  file
            File.WriteAllText(manifestGaoFilePath, manifestGaoYaml);

        }
        public static void WriteManifestGao(ManifestGao[] manifestGaos)
        {
            foreach (ManifestGao manifestGao in manifestGaos)
            {
                WriteManifestGao(manifestGao);
            }
        }
           




        [MenuItem("Assets/Gao Test")]
        static void GaoTest()
        {
            string[] bundleNames = GetAllBundleNames();
            ManifestUnity[] manifestUnitys = ParseManifests(bundleNames);
            ManifestGao[] manifestGaos = MakeManifestsGao(manifestUnitys);
            WriteManifestGao(manifestGaos);

            //@@@@@@@@@@@@@@@@@@@@
            /*
            Debug.Log("bundleNames.Length: " + bundleNames.Length);
            foreach (var name in bundleNames)
            {
                Debug.Log(name);
                ManifestUnity manifestData = ParseManifest(name);
                string str = JsonConvert.SerializeObject(manifestData, Formatting.Indented);
                Debug.Log(str);
            }
            */
            //@@@@@@@@@@@@@@@@@@@@@@
        }

    }
}