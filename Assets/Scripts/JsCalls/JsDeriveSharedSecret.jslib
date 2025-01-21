var EDHC_CONTEXT = {};
var EDHC_CONTEXT_COUNT = 0;

function allocReturnString(returnStr) {
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
}


async function GAO_DeriveSharedSecret__Step1_GenerateKeyPair() {
    var myKeyPair = await crypto.subtle.generateKey(
        {
            name: "ECDH",
            namedCurve: "P-256"
        },
        true,
        ["deriveKey", "deriveBits"]
    );
    console.dir(myKeyPair);

    var mySpkiPubKey = await crypto.subtle.exportKey(
        "spki",
        myKeyPair.publicKey
    );

    var mySpkiPubKeyBase64 = arrayBufferToBase64(mySpkiPubKey);

    ++EDHC_CONTEXT_COUNT;
    EDHC_CONTEXT[EDHC_CONTEXT_COUNT] = {
        myKeyPair: myKeyPair,
        mySpkiPubKeyBase64: mySpkiPubKeyBase64
    }

    return EDHC_CONTEXT_COUNT;
}

GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_results = {};
GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_results_idx = 0;

function GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async() {
    ++GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_results_idx;
    GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_results[GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_results_idx] = {
        finished: false,
        result: null,
        error: null
    };
    GAO_DeriveSharedSecret__Step1_GenerateKeyPair().then(function (edhcContext) {
        GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_results[GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_results_idx].finished = true;
        GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_results[GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_results_idx].result = edhcContext;
    }, function (err) {
        GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_results[GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_results_idx].finished = true;
        GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_results[GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_results_idx].err = err;
    });
    return GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_results_idx;
}

function GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_isFinished(idx) {
    return GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_results[idx].finished;
}
function GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_isError(idx) {
    if (GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_results[idx].error) {
        delete GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_results[idx];
        return true;
    } else {
        return false;
    }
}
function GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_getResult(idx) {
    var result = GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_results[idx].result;
    delete GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_results[idx];
    return result;
}


function GAO_DeriveSharedSecret__Step2_Get_mySpkiPubKeyBase64(edhcContext) {
    var mySpkiPubKeyBase64 = EDHC_CONTEXT[edhcContext].mySpkiPubKeyBase64;
    return allocReturnString(mySpkiPubKeyBase64);
}

async function GAO_DeriveSharedSecret__Step3_Import_serverPubkey(edhcContext, _serverPubKeyBase64) {
    var serverPubKeyBase64 = UTF8ToString(_serverPubKeyBase64);
    var serverPubKeyArrayBuffer = base64ToArrayBuffer(serverPubKeyBase64);
    var serverPubKey = await crypto.subtle.importKey(
        "spki",            // SPKI for public keys
        serverPubKeyArrayBuffer,
        {
            name: "ECDH",
            namedCurve: "P-256"
        },
        true,              // extractable
        []
    );

    var sharedSecret = await crypto.subtle.deriveBits(
        {
            name: "ECDH",
            public: serverPubKey
        },
        myKeyPair.privateKey,
        256
    );

    console.log("@@@@@@@@@@@@@@@@@@@@@ ecdh: Shared secret (raw bits):", new Uint8Array(sharedSecret));

    var hashedSecretBuffer = await crypto.subtle.digest("SHA-256", sharedSecret);
    var hashedSecret = new Uint8Array(hashedSecretBuffer);

    console.log("@@@@@@@@@@@@@@@@@@@@ ecdh: Hashed shared secret:", hashedSecret);

    EDHC_CONTEXT[edhcContext].sharedSecret = sharedSecret;
}

var GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_results = {};
var GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_results_idx = 0;

function GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async(edhcContext, _serverPubKeyBase64) {
    ++GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_results_idx;
    GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_results[GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_results_idx] = {
        finished: false,
        error: null
    };
    GAO_DeriveSharedSecret__Step3_Import_serverPubkey(edhcContext, _serverPubKeyBase64).then(function () {
        GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_results[GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_results_idx].finished = true;
    }, function (err) {
        GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_results[GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_results_idx].finished = true;
        GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_results[GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_results_idx].error = err;
    });
    return GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_results_idx; 
}

function GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_isFinished(idx) {
    return GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_results[idx].finished;
}
function GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_isError(idx) {
    if (GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_results[idx].error) {
        delete GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_results[idx];
        return true;
    } else {
        delete GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_results[idx];
        return false;
    }
}

function GAO_DeriveSharedSecret__Step4_Get_sharedSecret(edhcContext) {
    var sharedSecret = EDHC_CONTEXT[edhcContext].sharedSecret;

    // base64 encode sharedSecret
    var binaryData = sharedSecret;
    var binaryString = '';
    var len = binaryData.byteLength;
    for (let i = 0; i < len; i++) {
        binaryString += String.fromCharCode(binaryData[i]);
    }
    let binaryString64 = window.btoa(binaryString);

    delete EDHC_CONTEXT[edhcContext];
    return allocReturnString(binaryString64);
}




// JavaScript source code
mergeInto(LibraryManager.library, {
    DeriveSharedSecret__Step1_GenerateKeyPair_async: GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async,
    DeriveSharedSecret__Step1_GenerateKeyPair_async_isFinished: GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_isFinished,
    DeriveSharedSecret__Step1_GenerateKeyPair_async_isError: GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_isError,
    DeriveSharedSecret__Step1_GenerateKeyPair_async_getResult: GAO_DeriveSharedSecret__Step1_GenerateKeyPair_async_getResult,

    DeriveSharedSecret__Step3_Import_serverPubkey_async: GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async,
    DeriveSharedSecret__Step3_Import_serverPubkey_async_isFinished: GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_isFinished,
    DeriveSharedSecret__Step3_Import_serverPubkey_async_isError: GAO_DeriveSharedSecret__Step3_Import_serverPubkey_async_isError,

    DeriveSharedSecret__Step2_Get_mySpkiPubKeyBase64: GAO_DeriveSharedSecret__Step2_Get_mySpkiPubKeyBase64,
    DeriveSharedSecret__Step3_Import_serverPubkey: GAO_DeriveSharedSecret__Step3_Import_serverPubkey,
    DeriveSharedSecret__Step4_Get_sharedSecret: GAO_DeriveSharedSecret__Step4_Get_sharedSecret
});
