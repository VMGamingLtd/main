function GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async() {
    return GAO_DeriveSharedSecret.Step1_GenerateKeyPair_async(); 
}

function GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_isFinished(idx) {
    return GAO_DeriveSharedSecret.Step1_GenerateKeyPair_async_isFinished(idx);
}

function GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_isError(idx) {
    return GAO_DeriveSharedSecret.Step1_GenerateKeyPair_async_isError(idx);
}

function GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_getResult(idx) {
    return GAO_DeriveSharedSecret.Step1_GenerateKeyPair_async_getResult(idx);
}


function GAO_DeriveSharedSecret__Step2_Get_mySpkiPubKeyBase64(edhcContext) {
    return GAO_DeriveSharedSecret.Step2_Get_mySpkiPubKeyBase64(edhcContext);
}

function GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async(edhcContext, serverPubkeyBase64) {
    return GAO_DeriveSharedSecret.Step3_Import_serverPubkey_async(edhcContext, serverPubkeyBase64);
}

function GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_isFinished(edhcContext) {
    return GAO_DeriveSharedSecret.Step3_Import_serverPubkey_async_isFinished(edhcContext);
}

function GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_isError(edhcContext) {
    return GAO_DeriveSharedSecret.Step3_Import_serverPubkey_async_isError(edhcContext);
}

function GAO_DeriveSharedSecret__Step4_Get_sharedSecret(edhcContext) {
    return GAO_DeriveSharedSecret.Step4_Get_sharedSecret(edhcContext);
}


// JavaScript source code
mergeInto(LibraryManager.library, {

    Step1_GenerateKeyPair_async: GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async,
    Step1_GenerateKeyPair_async_isFinished: GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_isFinished,
    Step1_GenerateKeyPair_async_isError: GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_isError,
    Step1_GenerateKeyPair_async_getResult: GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_getResult,

    Step2_Get_mySpkiPubKeyBase64: GAO_DeriveSharedSecret__Step2_Get_mySpkiPubKeyBase64,

    Step3_Import_serverPubkey_async: GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async,
    Step3_Import_serverPubkey_async_isFinished: GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_isFinished,
    Step3_Import_serverPubkey_async_isError: GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_isError,

    Step4_Get_sharedSecret: GAO_DeriveSharedSecret__Step4_Get_sharedSecret
});
