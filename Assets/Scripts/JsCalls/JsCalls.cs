using System;
using System.Runtime.InteropServices;
namespace Js
{

	public class JsCalls
    {
        [DllImport("__Internal")]
        public static extern void Restart();

        /*
        [DllImport("__Internal")]
        public static extern int DeriveSharedSecret__Step1_GenerateKeyPair();

        [DllImport("__Internal")]
        public static extern string DeriveSharedSecret__Step2_Get_mySpkiPubKeyBase64(int ecdhContext);

        [DllImport("__Internal")]
        public static extern void DeriveSharedSecret__Step3_Import_serverPubkey(int ecdhContext, string serverPubKeyBase64);

        [DllImport("__Internal")]
        public static extern string DeriveSharedSecret__Step4_Get_sharedSecret(int ecdhContext);
        */

    }
}
