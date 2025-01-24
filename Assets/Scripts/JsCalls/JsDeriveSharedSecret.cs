using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Js 
{
    public class JsDeriveSharedSecret
    {
        public readonly static string CLASS_NAME = typeof(JsDeriveSharedSecret).Name;

        [DllImport("__Internal")]
        public static extern int Step1_GenerateKeyPair_async();
        [DllImport("__Internal")]
        public static extern bool Step1_GenerateKeyPair_async_isFinished(int idx);
        [DllImport("__Internal")]
        public static extern bool Step1_GenerateKeyPair_async_isError(int idx);
        [DllImport("__Internal")]
        public static extern int Step1_GenerateKeyPair_async_getResult(int idx);

        [DllImport("__Internal")]
        public static extern string Step2_Get_mySpkiPubKeyBase64(int ecdhContext);

        [DllImport("__Internal")]
        public static extern int Step3_Import_serverPubkey_async(int ecdhContext, string serverPubKeyBase64);
        [DllImport("__Internal")]
        public static extern bool Step3_Import_serverPubkey_async_isFinished(int idx, int ecdhContext, string serverPubKeyBase64);
        [DllImport("__Internal")]
        public static extern bool Step3_Import_serverPubkey_async_isError(int idx, int ecdhContext, string serverPubKeyBase64);

        [DllImport("__Internal")]
        public static extern string Step4_Get_sharedSecret(int ecdhContext);

        public class Step1_GenerateKeyPair_coroutineResult
        {
            public bool isFinished = false;
            public bool isError = false;
            public int edhcContext = -1;
        }
        public static IEnumerator Step1_GenerateKeyPair(Step1_GenerateKeyPair_coroutineResult coroutineResult)
        {
            const string METHOD_NAME = "Step1_GenerateKeyPair()";
            int idx = Step1_GenerateKeyPair_async();
            while (!Step1_GenerateKeyPair_async_isFinished(idx))
            {
                coroutineResult.isFinished = false;
                yield return null;
            }
            if (Step1_GenerateKeyPair_async_isError(idx))
            {
                coroutineResult.isFinished = true;
                coroutineResult.isError = true;
            }
            else
            {
                coroutineResult.isFinished = true;
                coroutineResult.isError = false;
                coroutineResult.edhcContext = Step1_GenerateKeyPair_async_getResult(idx);
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 2600: idx: {idx}, edhcContext: {coroutineResult.edhcContext}");
            }
        }

        public class Step3_Import_serverPubkey_coroutineResult
        {
            public bool isFinished = false;
            public bool isError = false;
        }
        public static IEnumerator Step3_Import_serverPubkey_async(Step3_Import_serverPubkey_coroutineResult coroutineResult, int ecdhContext, string serverPubKeyBase64)
        {
            int idx = Step3_Import_serverPubkey_async(ecdhContext, serverPubKeyBase64);
            while (!Step3_Import_serverPubkey_async_isFinished(idx, ecdhContext, serverPubKeyBase64))
            {
                coroutineResult.isFinished = false;
                yield return null;
            }
            if (Step3_Import_serverPubkey_async_isError(idx, ecdhContext, serverPubKeyBase64))
            {
                coroutineResult.isFinished = true;
                coroutineResult.isError = true;
            }
            else
            {
                coroutineResult.isFinished = true;
                coroutineResult.isError = false;
            }
        }
    }
}
