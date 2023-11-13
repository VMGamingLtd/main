using System.Collections.Generic;
using UnityEngine;

namespace Gaos.Environment
{
    public class Environment : MonoBehaviour
    {
        public static Dictionary<string, string> GetEnvironment_(string envName)
        {
            Dictionary<string, string> env = new Dictionary<string, string>();

            if (envName == "Development")
            {
                env.Add("GAOS_URL", "https://local.galacticodyssey.space/gaos");
                env.Add("GAOS_WS", "wss://local.galacticodyssey.space/gaos/ws");
                env.Add("IS_PROFILE_HTTP_CALLS", "true");
                env.Add("RELEASE_URL", "https://local.galacticodyssey.space/release");
            }
            else if (envName == "Test")
            {
                env.Add("GAOS_URL", "https://test.galacticodyssey.space/gaos");
                env.Add("GAOS_WS", "wss://test.galacticodyssey.space/gaos/ws");
                env.Add("IS_PROFILE_HTTP_CALLS", "false");
                env.Add("RELEASE_URL", "https://test.galacticodyssey.space/release");

            }
            else
            {
                throw new System.Exception($"Environment not found: {envName}");
            }


            return env;
        }

        public static Dictionary<string, string> GetEnvironment()
        {
            //return Environment.GetEnvironment_("Development");
            return GetEnvironment_("Development");
        }

    }


}