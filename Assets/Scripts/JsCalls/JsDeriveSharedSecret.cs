using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Js 
{
    public class JsDeriveSharedSecret
    {

        [DllImport("__Internal")]
        public static extern int DeriveSharedSecret__Step1_GenerateKeyPair_async();
        [DllImport("__Internal")]
        public static extern bool DeriveSharedSecret__Step1_GenerateKeyPair_async_isFinished();
        [DllImport("__Internal")]
        public static extern bool DeriveSharedSecret__Step1_GenerateKeyPair_async_isError();
        [DllImport("__Internal")]
        public static extern int DeriveSharedSecret__Step1_GenerateKeyPair_async_getResult();

        [DllImport("__Internal")]
        public static extern string DeriveSharedSecret__Step2_Get_mySpkiPubKeyBase64(int ecdhContext);

        [DllImport("__Internal")]
        public static extern int DeriveSharedSecret__Step3_Import_serverPubkey_async(int ecdhContext, string serverPubKeyBase64);
        [DllImport("__Internal")]
        public static extern bool DeriveSharedSecret__Step3_Import_serverPubkey_async_isFinished(int ecdhContext, string serverPubKeyBase64);
        [DllImport("__Internal")]
        public static extern bool DeriveSharedSecret__Step3_Import_serverPubkey_async_isError(int ecdhContext, string serverPubKeyBase64);

        [DllImport("__Internal")]
        public static extern string DeriveSharedSecret__Step4_Get_sharedSecret(int ecdhContext);

        public class DeriveSharedSecret__Step1_GenerateKeyPair_coroutineResult
        {
            public bool isFinished = false;
            public bool isError = false;
            public int edhcContext = -1;
        }
        public static IEnumerator DeriveSharedSecret__Step1_GenerateKeyPair(DeriveSharedSecret__Step1_GenerateKeyPair_coroutineResult coroutineResult)
        {
            while (!DeriveSharedSecret__Step1_GenerateKeyPair_async_isFinished())
            {
                coroutineResult.isFinished = false;
                yield return null;
            }
            if (DeriveSharedSecret__Step1_GenerateKeyPair_async_isError())
            {
                coroutineResult.isFinished = true;
                coroutineResult.isError = true;
            }
            else
            {
                coroutineResult.isFinished = true;
                coroutineResult.isError = false;
                coroutineResult.edhcContext = DeriveSharedSecret__Step1_GenerateKeyPair_async_getResult();
            }
        }

        public class DeriveSharedSecret__Step3_Import_serverPubkey_coroutineResult
        {
            public bool isFinished = false;
            public bool isError = false;
        }
        public static IEnumerator DeriveSharedSecret__Step3_Import_serverPubkey_async(DeriveSharedSecret__Step3_Import_serverPubkey_coroutineResult coroutineResult, int ecdhContext, string serverPubKeyBase64)
        {
            while(!DeriveSharedSecret__Step3_Import_serverPubkey_async_isFinished(ecdhContext, serverPubKeyBase64))
            {
                coroutineResult.isFinished = false;
                yield return null;
            }
            if (DeriveSharedSecret__Step3_Import_serverPubkey_async_isError(ecdhContext, serverPubKeyBase64))
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
