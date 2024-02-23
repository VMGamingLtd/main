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
                Debug.Log("Environment: Development");
                env.Add("GAOS_URL", "https://local.galacticodyssey.space/gaos");
                env.Add("GAOS_WS", "wss://local.galacticodyssey.space/gaos/ws");
                env.Add("IS_PROFILE_HTTP_CALLS", "true");
                env.Add("RELEASE_URL", "https://local.galacticodyssey.space/release");

                env.Add("IS_SEND_GAME_DATA_DIFF", "true");

                env.Add("IS_DEBUG", "true");
                env.Add("IS_DEBUG_GAME_DATA", "true");
            }
            else if (envName == "Test")
            {
                Debug.Log("Environment: Test");
                env.Add("GAOS_URL", "https://test.galacticodyssey.space/gaos");
                env.Add("GAOS_WS", "wss://test.galacticodyssey.space/gaos/ws");
                env.Add("IS_PROFILE_HTTP_CALLS", "false");
                env.Add("RELEASE_URL", "https://test.galacticodyssey.space/release");

                env.Add("IS_SEND_GAME_DATA_DIFF", "true");

                env.Add("IS_DEBUG", "false");
                env.Add("IS_DEBUG_GAME_DATA", "false");

            }
            else
            {
                throw new System.Exception($"Environment not found: {envName}");
            }


            return env;
        }

        public static Dictionary<string, string> GetEnvironment()
        {
            //return Environment.GetEnvironment_("Test");
            return GetEnvironment_("Development");
        }

    }


}