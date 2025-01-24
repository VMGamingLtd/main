function make_GAO_DeriveSharedSecret() {
    var EDHC_CONTEXT = {};
    var EDHC_CONTEXT_COUNT = 0;

    function allocReturnString(returnStr) {
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    }

    function arrayBufferToBase64(buffer) {
        let binary = '';
        const bytes = new Uint8Array(buffer);
        const len = bytes.byteLength;

        for (let i = 0; i < len; i++) {
            binary += String.fromCharCode(bytes[i]);
        }

        return btoa(binary);
    }

    function base64ToArrayBuffer(base64) {
        const binaryString = atob(base64); // decode base64 to binary
        const len = binaryString.length;
        const bytes = new Uint8Array(len);
        for (let i = 0; i < len; i++) {
            bytes[i] = binaryString.charCodeAt(i);
        }
        return bytes.buffer;
    }


    async function Step1_GenerateKeyPair() {
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

    var Step1_GenerateKeyPair_async_results = {};
    var Step1_GenerateKeyPair_async_results_idx = 0;

    function Step1_GenerateKeyPair_async() {
        ++Step1_GenerateKeyPair_async_results_idx;
        Step1_GenerateKeyPair_async_results[Step1_GenerateKeyPair_async_results_idx] = {
            finished: false,
            result: null,
            error: null
        };
        Step1_GenerateKeyPair().then(function (edhcContext) {
            Step1_GenerateKeyPair_async_results[Step1_GenerateKeyPair_async_results_idx].finished = true;
            Step1_GenerateKeyPair_async_results[Step1_GenerateKeyPair_async_results_idx].result = edhcContext;
        }, function (err) {
            console.error("Step1_GenerateKeyPair_async error:", err);
            Step1_GenerateKeyPair_async_results[Step1_GenerateKeyPair_async_results_idx].finished = true;
            Step1_GenerateKeyPair_async_results[Step1_GenerateKeyPair_async_results_idx].error = err;
        });
        return Step1_GenerateKeyPair_async_results_idx;
    }

    function Step1_GenerateKeyPair_async_isFinished(idx) {
        return Step1_GenerateKeyPair_async_results[idx].finished;
    }
    function Step1_GenerateKeyPair_async_isError(idx) {
        if (Step1_GenerateKeyPair_async_results[idx].error) {
            delete Step1_GenerateKeyPair_async_results[idx];
            return true;
        } else {
            return false;
        }
    }
    function Step1_GenerateKeyPair_async_getResult(idx) {
        var result = Step1_GenerateKeyPair_async_results[idx].result;
        delete Step1_GenerateKeyPair_async_results[idx];
        return result;
    }


    function Step2_Get_mySpkiPubKeyBase64(edhcContext) {
        var mySpkiPubKeyBase64 = EDHC_CONTEXT[edhcContext].mySpkiPubKeyBase64;
        return allocReturnString(mySpkiPubKeyBase64);
    }

    async function Step3_Import_serverPubkey(edhcContext, _serverPubKeyBase64) {
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

        var myKeyPair = EDHC_CONTEXT[edhcContext].myKeyPair;

        var sharedSecret = await crypto.subtle.deriveBits(
            {
                name: "ECDH",
                public: serverPubKey
            },
            myKeyPair.privateKey,
            256
        );

        var hashedSecretBuffer = await crypto.subtle.digest("SHA-256", sharedSecret);
        var hashedSecret = new Uint8Array(hashedSecretBuffer);

        EDHC_CONTEXT[edhcContext].sharedSecret = hashedSecret;
    }

    var Step3_Import_serverPubkey_async_results = {};
    var Step3_Import_serverPubkey_async_results_idx = 0;

    function Step3_Import_serverPubkey_async(edhcContext, _serverPubKeyBase64) {
        ++Step3_Import_serverPubkey_async_results_idx;
        Step3_Import_serverPubkey_async_results[Step3_Import_serverPubkey_async_results_idx] = {
            finished: false,
            error: null
        };
        Step3_Import_serverPubkey(edhcContext, _serverPubKeyBase64).then(function () {
            Step3_Import_serverPubkey_async_results[Step3_Import_serverPubkey_async_results_idx].finished = true;
        }, function (err) {
            console.error("Step3_Import_serverPubkey_async error:", err);
            Step3_Import_serverPubkey_async_results[Step3_Import_serverPubkey_async_results_idx].finished = true;
            Step3_Import_serverPubkey_async_results[Step3_Import_serverPubkey_async_results_idx].error = err;
        });
        return Step3_Import_serverPubkey_async_results_idx;
    }

    function Step3_Import_serverPubkey_async_isFinished(idx) {
        return Step3_Import_serverPubkey_async_results[idx].finished;
    }
    function Step3_Import_serverPubkey_async_isError(idx) {
        if (Step3_Import_serverPubkey_async_results[idx].error) {
            delete Step3_Import_serverPubkey_async_results[idx];
            return true;
        } else {
            delete Step3_Import_serverPubkey_async_results[idx];
            return false;
        }
    }

    function Step4_Get_sharedSecret(edhcContext) {
        var sharedSecret = EDHC_CONTEXT[edhcContext].sharedSecret;

        // base64 encode sharedSecret
        var binaryData = sharedSecret;
        var binaryString = '';
        var len = binaryData.byteLength;
        for (let i = 0; i < len; i++) {
            binaryString += String.fromCharCode(binaryData[i]);
        }
        var binaryString64 = window.btoa(binaryString);

        delete EDHC_CONTEXT[edhcContext];
        return allocReturnString(binaryString64);
    }

    return {
        Step1_GenerateKeyPair_async: Step1_GenerateKeyPair_async,
        Step1_GenerateKeyPair_async_isFinished: Step1_GenerateKeyPair_async_isFinished,
        Step1_GenerateKeyPair_async_isError: Step1_GenerateKeyPair_async_isError,
        Step1_GenerateKeyPair_async_getResult: Step1_GenerateKeyPair_async_getResult,

        Step2_Get_mySpkiPubKeyBase64: Step2_Get_mySpkiPubKeyBase64,

        Step3_Import_serverPubkey_async: Step3_Import_serverPubkey_async,
        Step3_Import_serverPubkey_async_isFinished: Step3_Import_serverPubkey_async_isFinished,
        Step3_Import_serverPubkey_async_isError: Step3_Import_serverPubkey_async_isError,

        Step4_Get_sharedSecret: Step4_Get_sharedSecret
    }
}

GAO_DeriveSharedSecret = make_GAO_DeriveSharedSecret();
