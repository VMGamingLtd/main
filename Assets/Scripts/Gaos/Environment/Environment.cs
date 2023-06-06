using System.Collections.Generic;

namespace Gaos.Environment
{
    public static class Environment
    {
        public static Dictionary<string, string> GetEnvironment_(string envName)
        {
            Dictionary<string, string> env = new Dictionary<string, string>();

            if (envName == "Development")
            {
                env.Add("GAOS_URL", "https://local.galacticodyssey.space/gaos");
                env.Add("GAOS_WS", "wss://local.galacticodyssey.space/gaos/ws");
            }
            else if (envName == "Test")
            {
                env.Add("GAOS_URL", "https://test.galacticodyssey.space/gaos");
                env.Add("GAOS_WS", "wss://test.galacticodyssey.space/gaos/ws");

            }
            else
            {
                throw new System.Exception($"Environment not found: {envName}");
            }


            return env;
        }

        public static Dictionary<string, string> GetEnvironment()
        {
            return Environment.GetEnvironment_("Development");
        }

    }


}