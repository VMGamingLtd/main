/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./node_modules/@protobufjs/aspromise/index.js":
/*!*****************************************************!*\
  !*** ./node_modules/@protobufjs/aspromise/index.js ***!
  \*****************************************************/
/***/ ((module) => {


module.exports = asPromise;

/**
 * Callback as used by {@link util.asPromise}.
 * @typedef asPromiseCallback
 * @type {function}
 * @param {Error|null} error Error, if any
 * @param {...*} params Additional arguments
 * @returns {undefined}
 */

/**
 * Returns a promise from a node-style callback function.
 * @memberof util
 * @param {asPromiseCallback} fn Function to call
 * @param {*} ctx Function context
 * @param {...*} params Function arguments
 * @returns {Promise<*>} Promisified function
 */
function asPromise(fn, ctx/*, varargs */) {
    var params  = new Array(arguments.length - 1),
        offset  = 0,
        index   = 2,
        pending = true;
    while (index < arguments.length)
        params[offset++] = arguments[index++];
    return new Promise(function executor(resolve, reject) {
        params[offset] = function callback(err/*, varargs */) {
            if (pending) {
                pending = false;
                if (err)
                    reject(err);
                else {
                    var params = new Array(arguments.length - 1),
                        offset = 0;
                    while (offset < params.length)
                        params[offset++] = arguments[offset];
                    resolve.apply(null, params);
                }
            }
        };
        try {
            fn.apply(ctx || null, params);
        } catch (err) {
            if (pending) {
                pending = false;
                reject(err);
            }
        }
    });
}


/***/ }),

/***/ "./node_modules/@protobufjs/base64/index.js":
/*!**************************************************!*\
  !*** ./node_modules/@protobufjs/base64/index.js ***!
  \**************************************************/
/***/ ((__unused_webpack_module, exports) => {



/**
 * A minimal base64 implementation for number arrays.
 * @memberof util
 * @namespace
 */
var base64 = exports;

/**
 * Calculates the byte length of a base64 encoded string.
 * @param {string} string Base64 encoded string
 * @returns {number} Byte length
 */
base64.length = function length(string) {
    var p = string.length;
    if (!p)
        return 0;
    var n = 0;
    while (--p % 4 > 1 && string.charAt(p) === "=")
        ++n;
    return Math.ceil(string.length * 3) / 4 - n;
};

// Base64 encoding table
var b64 = new Array(64);

// Base64 decoding table
var s64 = new Array(123);

// 65..90, 97..122, 48..57, 43, 47
for (var i = 0; i < 64;)
    s64[b64[i] = i < 26 ? i + 65 : i < 52 ? i + 71 : i < 62 ? i - 4 : i - 59 | 43] = i++;

/**
 * Encodes a buffer to a base64 encoded string.
 * @param {Uint8Array} buffer Source buffer
 * @param {number} start Source start
 * @param {number} end Source end
 * @returns {string} Base64 encoded string
 */
base64.encode = function encode(buffer, start, end) {
    var parts = null,
        chunk = [];
    var i = 0, // output index
        j = 0, // goto index
        t;     // temporary
    while (start < end) {
        var b = buffer[start++];
        switch (j) {
            case 0:
                chunk[i++] = b64[b >> 2];
                t = (b & 3) << 4;
                j = 1;
                break;
            case 1:
                chunk[i++] = b64[t | b >> 4];
                t = (b & 15) << 2;
                j = 2;
                break;
            case 2:
                chunk[i++] = b64[t | b >> 6];
                chunk[i++] = b64[b & 63];
                j = 0;
                break;
        }
        if (i > 8191) {
            (parts || (parts = [])).push(String.fromCharCode.apply(String, chunk));
            i = 0;
        }
    }
    if (j) {
        chunk[i++] = b64[t];
        chunk[i++] = 61;
        if (j === 1)
            chunk[i++] = 61;
    }
    if (parts) {
        if (i)
            parts.push(String.fromCharCode.apply(String, chunk.slice(0, i)));
        return parts.join("");
    }
    return String.fromCharCode.apply(String, chunk.slice(0, i));
};

var invalidEncoding = "invalid encoding";

/**
 * Decodes a base64 encoded string to a buffer.
 * @param {string} string Source string
 * @param {Uint8Array} buffer Destination buffer
 * @param {number} offset Destination offset
 * @returns {number} Number of bytes written
 * @throws {Error} If encoding is invalid
 */
base64.decode = function decode(string, buffer, offset) {
    var start = offset;
    var j = 0, // goto index
        t;     // temporary
    for (var i = 0; i < string.length;) {
        var c = string.charCodeAt(i++);
        if (c === 61 && j > 1)
            break;
        if ((c = s64[c]) === undefined)
            throw Error(invalidEncoding);
        switch (j) {
            case 0:
                t = c;
                j = 1;
                break;
            case 1:
                buffer[offset++] = t << 2 | (c & 48) >> 4;
                t = c;
                j = 2;
                break;
            case 2:
                buffer[offset++] = (t & 15) << 4 | (c & 60) >> 2;
                t = c;
                j = 3;
                break;
            case 3:
                buffer[offset++] = (t & 3) << 6 | c;
                j = 0;
                break;
        }
    }
    if (j === 1)
        throw Error(invalidEncoding);
    return offset - start;
};

/**
 * Tests if the specified string appears to be base64 encoded.
 * @param {string} string String to test
 * @returns {boolean} `true` if probably base64 encoded, otherwise false
 */
base64.test = function test(string) {
    return /^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$/.test(string);
};


/***/ }),

/***/ "./node_modules/@protobufjs/codegen/index.js":
/*!***************************************************!*\
  !*** ./node_modules/@protobufjs/codegen/index.js ***!
  \***************************************************/
/***/ ((module) => {


module.exports = codegen;

/**
 * Begins generating a function.
 * @memberof util
 * @param {string[]} functionParams Function parameter names
 * @param {string} [functionName] Function name if not anonymous
 * @returns {Codegen} Appender that appends code to the function's body
 */
function codegen(functionParams, functionName) {

    /* istanbul ignore if */
    if (typeof functionParams === "string") {
        functionName = functionParams;
        functionParams = undefined;
    }

    var body = [];

    /**
     * Appends code to the function's body or finishes generation.
     * @typedef Codegen
     * @type {function}
     * @param {string|Object.<string,*>} [formatStringOrScope] Format string or, to finish the function, an object of additional scope variables, if any
     * @param {...*} [formatParams] Format parameters
     * @returns {Codegen|Function} Itself or the generated function if finished
     * @throws {Error} If format parameter counts do not match
     */

    function Codegen(formatStringOrScope) {
        // note that explicit array handling below makes this ~50% faster

        // finish the function
        if (typeof formatStringOrScope !== "string") {
            var source = toString();
            if (codegen.verbose)
                console.log("codegen: " + source); // eslint-disable-line no-console
            source = "return " + source;
            if (formatStringOrScope) {
                var scopeKeys   = Object.keys(formatStringOrScope),
                    scopeParams = new Array(scopeKeys.length + 1),
                    scopeValues = new Array(scopeKeys.length),
                    scopeOffset = 0;
                while (scopeOffset < scopeKeys.length) {
                    scopeParams[scopeOffset] = scopeKeys[scopeOffset];
                    scopeValues[scopeOffset] = formatStringOrScope[scopeKeys[scopeOffset++]];
                }
                scopeParams[scopeOffset] = source;
                return Function.apply(null, scopeParams).apply(null, scopeValues); // eslint-disable-line no-new-func
            }
            return Function(source)(); // eslint-disable-line no-new-func
        }

        // otherwise append to body
        var formatParams = new Array(arguments.length - 1),
            formatOffset = 0;
        while (formatOffset < formatParams.length)
            formatParams[formatOffset] = arguments[++formatOffset];
        formatOffset = 0;
        formatStringOrScope = formatStringOrScope.replace(/%([%dfijs])/g, function replace($0, $1) {
            var value = formatParams[formatOffset++];
            switch ($1) {
                case "d": case "f": return String(Number(value));
                case "i": return String(Math.floor(value));
                case "j": return JSON.stringify(value);
                case "s": return String(value);
            }
            return "%";
        });
        if (formatOffset !== formatParams.length)
            throw Error("parameter count mismatch");
        body.push(formatStringOrScope);
        return Codegen;
    }

    function toString(functionNameOverride) {
        return "function " + (functionNameOverride || functionName || "") + "(" + (functionParams && functionParams.join(",") || "") + "){\n  " + body.join("\n  ") + "\n}";
    }

    Codegen.toString = toString;
    return Codegen;
}

/**
 * Begins generating a function.
 * @memberof util
 * @function codegen
 * @param {string} [functionName] Function name if not anonymous
 * @returns {Codegen} Appender that appends code to the function's body
 * @variation 2
 */

/**
 * When set to `true`, codegen will log generated code to console. Useful for debugging.
 * @name util.codegen.verbose
 * @type {boolean}
 */
codegen.verbose = false;


/***/ }),

/***/ "./node_modules/@protobufjs/eventemitter/index.js":
/*!********************************************************!*\
  !*** ./node_modules/@protobufjs/eventemitter/index.js ***!
  \********************************************************/
/***/ ((module) => {


module.exports = EventEmitter;

/**
 * Constructs a new event emitter instance.
 * @classdesc A minimal event emitter.
 * @memberof util
 * @constructor
 */
function EventEmitter() {

    /**
     * Registered listeners.
     * @type {Object.<string,*>}
     * @private
     */
    this._listeners = {};
}

/**
 * Registers an event listener.
 * @param {string} evt Event name
 * @param {function} fn Listener
 * @param {*} [ctx] Listener context
 * @returns {util.EventEmitter} `this`
 */
EventEmitter.prototype.on = function on(evt, fn, ctx) {
    (this._listeners[evt] || (this._listeners[evt] = [])).push({
        fn  : fn,
        ctx : ctx || this
    });
    return this;
};

/**
 * Removes an event listener or any matching listeners if arguments are omitted.
 * @param {string} [evt] Event name. Removes all listeners if omitted.
 * @param {function} [fn] Listener to remove. Removes all listeners of `evt` if omitted.
 * @returns {util.EventEmitter} `this`
 */
EventEmitter.prototype.off = function off(evt, fn) {
    if (evt === undefined)
        this._listeners = {};
    else {
        if (fn === undefined)
            this._listeners[evt] = [];
        else {
            var listeners = this._listeners[evt];
            for (var i = 0; i < listeners.length;)
                if (listeners[i].fn === fn)
                    listeners.splice(i, 1);
                else
                    ++i;
        }
    }
    return this;
};

/**
 * Emits an event by calling its listeners with the specified arguments.
 * @param {string} evt Event name
 * @param {...*} args Arguments
 * @returns {util.EventEmitter} `this`
 */
EventEmitter.prototype.emit = function emit(evt) {
    var listeners = this._listeners[evt];
    if (listeners) {
        var args = [],
            i = 1;
        for (; i < arguments.length;)
            args.push(arguments[i++]);
        for (i = 0; i < listeners.length;)
            listeners[i].fn.apply(listeners[i++].ctx, args);
    }
    return this;
};


/***/ }),

/***/ "./node_modules/@protobufjs/fetch/index.js":
/*!*************************************************!*\
  !*** ./node_modules/@protobufjs/fetch/index.js ***!
  \*************************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = fetch;

var asPromise = __webpack_require__(/*! @protobufjs/aspromise */ "./node_modules/@protobufjs/aspromise/index.js"),
    inquire   = __webpack_require__(/*! @protobufjs/inquire */ "./node_modules/@protobufjs/inquire/index.js");

var fs = inquire("fs");

/**
 * Node-style callback as used by {@link util.fetch}.
 * @typedef FetchCallback
 * @type {function}
 * @param {?Error} error Error, if any, otherwise `null`
 * @param {string} [contents] File contents, if there hasn't been an error
 * @returns {undefined}
 */

/**
 * Options as used by {@link util.fetch}.
 * @typedef FetchOptions
 * @type {Object}
 * @property {boolean} [binary=false] Whether expecting a binary response
 * @property {boolean} [xhr=false] If `true`, forces the use of XMLHttpRequest
 */

/**
 * Fetches the contents of a file.
 * @memberof util
 * @param {string} filename File path or url
 * @param {FetchOptions} options Fetch options
 * @param {FetchCallback} callback Callback function
 * @returns {undefined}
 */
function fetch(filename, options, callback) {
    if (typeof options === "function") {
        callback = options;
        options = {};
    } else if (!options)
        options = {};

    if (!callback)
        return asPromise(fetch, this, filename, options); // eslint-disable-line no-invalid-this

    // if a node-like filesystem is present, try it first but fall back to XHR if nothing is found.
    if (!options.xhr && fs && fs.readFile)
        return fs.readFile(filename, function fetchReadFileCallback(err, contents) {
            return err && typeof XMLHttpRequest !== "undefined"
                ? fetch.xhr(filename, options, callback)
                : err
                ? callback(err)
                : callback(null, options.binary ? contents : contents.toString("utf8"));
        });

    // use the XHR version otherwise.
    return fetch.xhr(filename, options, callback);
}

/**
 * Fetches the contents of a file.
 * @name util.fetch
 * @function
 * @param {string} path File path or url
 * @param {FetchCallback} callback Callback function
 * @returns {undefined}
 * @variation 2
 */

/**
 * Fetches the contents of a file.
 * @name util.fetch
 * @function
 * @param {string} path File path or url
 * @param {FetchOptions} [options] Fetch options
 * @returns {Promise<string|Uint8Array>} Promise
 * @variation 3
 */

/**/
fetch.xhr = function fetch_xhr(filename, options, callback) {
    var xhr = new XMLHttpRequest();
    xhr.onreadystatechange /* works everywhere */ = function fetchOnReadyStateChange() {

        if (xhr.readyState !== 4)
            return undefined;

        // local cors security errors return status 0 / empty string, too. afaik this cannot be
        // reliably distinguished from an actually empty file for security reasons. feel free
        // to send a pull request if you are aware of a solution.
        if (xhr.status !== 0 && xhr.status !== 200)
            return callback(Error("status " + xhr.status));

        // if binary data is expected, make sure that some sort of array is returned, even if
        // ArrayBuffers are not supported. the binary string fallback, however, is unsafe.
        if (options.binary) {
            var buffer = xhr.response;
            if (!buffer) {
                buffer = [];
                for (var i = 0; i < xhr.responseText.length; ++i)
                    buffer.push(xhr.responseText.charCodeAt(i) & 255);
            }
            return callback(null, typeof Uint8Array !== "undefined" ? new Uint8Array(buffer) : buffer);
        }
        return callback(null, xhr.responseText);
    };

    if (options.binary) {
        // ref: https://developer.mozilla.org/en-US/docs/Web/API/XMLHttpRequest/Sending_and_Receiving_Binary_Data#Receiving_binary_data_in_older_browsers
        if ("overrideMimeType" in xhr)
            xhr.overrideMimeType("text/plain; charset=x-user-defined");
        xhr.responseType = "arraybuffer";
    }

    xhr.open("GET", filename);
    xhr.send();
};


/***/ }),

/***/ "./node_modules/@protobufjs/float/index.js":
/*!*************************************************!*\
  !*** ./node_modules/@protobufjs/float/index.js ***!
  \*************************************************/
/***/ ((module) => {



module.exports = factory(factory);

/**
 * Reads / writes floats / doubles from / to buffers.
 * @name util.float
 * @namespace
 */

/**
 * Writes a 32 bit float to a buffer using little endian byte order.
 * @name util.float.writeFloatLE
 * @function
 * @param {number} val Value to write
 * @param {Uint8Array} buf Target buffer
 * @param {number} pos Target buffer offset
 * @returns {undefined}
 */

/**
 * Writes a 32 bit float to a buffer using big endian byte order.
 * @name util.float.writeFloatBE
 * @function
 * @param {number} val Value to write
 * @param {Uint8Array} buf Target buffer
 * @param {number} pos Target buffer offset
 * @returns {undefined}
 */

/**
 * Reads a 32 bit float from a buffer using little endian byte order.
 * @name util.float.readFloatLE
 * @function
 * @param {Uint8Array} buf Source buffer
 * @param {number} pos Source buffer offset
 * @returns {number} Value read
 */

/**
 * Reads a 32 bit float from a buffer using big endian byte order.
 * @name util.float.readFloatBE
 * @function
 * @param {Uint8Array} buf Source buffer
 * @param {number} pos Source buffer offset
 * @returns {number} Value read
 */

/**
 * Writes a 64 bit double to a buffer using little endian byte order.
 * @name util.float.writeDoubleLE
 * @function
 * @param {number} val Value to write
 * @param {Uint8Array} buf Target buffer
 * @param {number} pos Target buffer offset
 * @returns {undefined}
 */

/**
 * Writes a 64 bit double to a buffer using big endian byte order.
 * @name util.float.writeDoubleBE
 * @function
 * @param {number} val Value to write
 * @param {Uint8Array} buf Target buffer
 * @param {number} pos Target buffer offset
 * @returns {undefined}
 */

/**
 * Reads a 64 bit double from a buffer using little endian byte order.
 * @name util.float.readDoubleLE
 * @function
 * @param {Uint8Array} buf Source buffer
 * @param {number} pos Source buffer offset
 * @returns {number} Value read
 */

/**
 * Reads a 64 bit double from a buffer using big endian byte order.
 * @name util.float.readDoubleBE
 * @function
 * @param {Uint8Array} buf Source buffer
 * @param {number} pos Source buffer offset
 * @returns {number} Value read
 */

// Factory function for the purpose of node-based testing in modified global environments
function factory(exports) {

    // float: typed array
    if (typeof Float32Array !== "undefined") (function() {

        var f32 = new Float32Array([ -0 ]),
            f8b = new Uint8Array(f32.buffer),
            le  = f8b[3] === 128;

        function writeFloat_f32_cpy(val, buf, pos) {
            f32[0] = val;
            buf[pos    ] = f8b[0];
            buf[pos + 1] = f8b[1];
            buf[pos + 2] = f8b[2];
            buf[pos + 3] = f8b[3];
        }

        function writeFloat_f32_rev(val, buf, pos) {
            f32[0] = val;
            buf[pos    ] = f8b[3];
            buf[pos + 1] = f8b[2];
            buf[pos + 2] = f8b[1];
            buf[pos + 3] = f8b[0];
        }

        /* istanbul ignore next */
        exports.writeFloatLE = le ? writeFloat_f32_cpy : writeFloat_f32_rev;
        /* istanbul ignore next */
        exports.writeFloatBE = le ? writeFloat_f32_rev : writeFloat_f32_cpy;

        function readFloat_f32_cpy(buf, pos) {
            f8b[0] = buf[pos    ];
            f8b[1] = buf[pos + 1];
            f8b[2] = buf[pos + 2];
            f8b[3] = buf[pos + 3];
            return f32[0];
        }

        function readFloat_f32_rev(buf, pos) {
            f8b[3] = buf[pos    ];
            f8b[2] = buf[pos + 1];
            f8b[1] = buf[pos + 2];
            f8b[0] = buf[pos + 3];
            return f32[0];
        }

        /* istanbul ignore next */
        exports.readFloatLE = le ? readFloat_f32_cpy : readFloat_f32_rev;
        /* istanbul ignore next */
        exports.readFloatBE = le ? readFloat_f32_rev : readFloat_f32_cpy;

    // float: ieee754
    })(); else (function() {

        function writeFloat_ieee754(writeUint, val, buf, pos) {
            var sign = val < 0 ? 1 : 0;
            if (sign)
                val = -val;
            if (val === 0)
                writeUint(1 / val > 0 ? /* positive */ 0 : /* negative 0 */ 2147483648, buf, pos);
            else if (isNaN(val))
                writeUint(2143289344, buf, pos);
            else if (val > 3.4028234663852886e+38) // +-Infinity
                writeUint((sign << 31 | 2139095040) >>> 0, buf, pos);
            else if (val < 1.1754943508222875e-38) // denormal
                writeUint((sign << 31 | Math.round(val / 1.401298464324817e-45)) >>> 0, buf, pos);
            else {
                var exponent = Math.floor(Math.log(val) / Math.LN2),
                    mantissa = Math.round(val * Math.pow(2, -exponent) * 8388608) & 8388607;
                writeUint((sign << 31 | exponent + 127 << 23 | mantissa) >>> 0, buf, pos);
            }
        }

        exports.writeFloatLE = writeFloat_ieee754.bind(null, writeUintLE);
        exports.writeFloatBE = writeFloat_ieee754.bind(null, writeUintBE);

        function readFloat_ieee754(readUint, buf, pos) {
            var uint = readUint(buf, pos),
                sign = (uint >> 31) * 2 + 1,
                exponent = uint >>> 23 & 255,
                mantissa = uint & 8388607;
            return exponent === 255
                ? mantissa
                ? NaN
                : sign * Infinity
                : exponent === 0 // denormal
                ? sign * 1.401298464324817e-45 * mantissa
                : sign * Math.pow(2, exponent - 150) * (mantissa + 8388608);
        }

        exports.readFloatLE = readFloat_ieee754.bind(null, readUintLE);
        exports.readFloatBE = readFloat_ieee754.bind(null, readUintBE);

    })();

    // double: typed array
    if (typeof Float64Array !== "undefined") (function() {

        var f64 = new Float64Array([-0]),
            f8b = new Uint8Array(f64.buffer),
            le  = f8b[7] === 128;

        function writeDouble_f64_cpy(val, buf, pos) {
            f64[0] = val;
            buf[pos    ] = f8b[0];
            buf[pos + 1] = f8b[1];
            buf[pos + 2] = f8b[2];
            buf[pos + 3] = f8b[3];
            buf[pos + 4] = f8b[4];
            buf[pos + 5] = f8b[5];
            buf[pos + 6] = f8b[6];
            buf[pos + 7] = f8b[7];
        }

        function writeDouble_f64_rev(val, buf, pos) {
            f64[0] = val;
            buf[pos    ] = f8b[7];
            buf[pos + 1] = f8b[6];
            buf[pos + 2] = f8b[5];
            buf[pos + 3] = f8b[4];
            buf[pos + 4] = f8b[3];
            buf[pos + 5] = f8b[2];
            buf[pos + 6] = f8b[1];
            buf[pos + 7] = f8b[0];
        }

        /* istanbul ignore next */
        exports.writeDoubleLE = le ? writeDouble_f64_cpy : writeDouble_f64_rev;
        /* istanbul ignore next */
        exports.writeDoubleBE = le ? writeDouble_f64_rev : writeDouble_f64_cpy;

        function readDouble_f64_cpy(buf, pos) {
            f8b[0] = buf[pos    ];
            f8b[1] = buf[pos + 1];
            f8b[2] = buf[pos + 2];
            f8b[3] = buf[pos + 3];
            f8b[4] = buf[pos + 4];
            f8b[5] = buf[pos + 5];
            f8b[6] = buf[pos + 6];
            f8b[7] = buf[pos + 7];
            return f64[0];
        }

        function readDouble_f64_rev(buf, pos) {
            f8b[7] = buf[pos    ];
            f8b[6] = buf[pos + 1];
            f8b[5] = buf[pos + 2];
            f8b[4] = buf[pos + 3];
            f8b[3] = buf[pos + 4];
            f8b[2] = buf[pos + 5];
            f8b[1] = buf[pos + 6];
            f8b[0] = buf[pos + 7];
            return f64[0];
        }

        /* istanbul ignore next */
        exports.readDoubleLE = le ? readDouble_f64_cpy : readDouble_f64_rev;
        /* istanbul ignore next */
        exports.readDoubleBE = le ? readDouble_f64_rev : readDouble_f64_cpy;

    // double: ieee754
    })(); else (function() {

        function writeDouble_ieee754(writeUint, off0, off1, val, buf, pos) {
            var sign = val < 0 ? 1 : 0;
            if (sign)
                val = -val;
            if (val === 0) {
                writeUint(0, buf, pos + off0);
                writeUint(1 / val > 0 ? /* positive */ 0 : /* negative 0 */ 2147483648, buf, pos + off1);
            } else if (isNaN(val)) {
                writeUint(0, buf, pos + off0);
                writeUint(2146959360, buf, pos + off1);
            } else if (val > 1.7976931348623157e+308) { // +-Infinity
                writeUint(0, buf, pos + off0);
                writeUint((sign << 31 | 2146435072) >>> 0, buf, pos + off1);
            } else {
                var mantissa;
                if (val < 2.2250738585072014e-308) { // denormal
                    mantissa = val / 5e-324;
                    writeUint(mantissa >>> 0, buf, pos + off0);
                    writeUint((sign << 31 | mantissa / 4294967296) >>> 0, buf, pos + off1);
                } else {
                    var exponent = Math.floor(Math.log(val) / Math.LN2);
                    if (exponent === 1024)
                        exponent = 1023;
                    mantissa = val * Math.pow(2, -exponent);
                    writeUint(mantissa * 4503599627370496 >>> 0, buf, pos + off0);
                    writeUint((sign << 31 | exponent + 1023 << 20 | mantissa * 1048576 & 1048575) >>> 0, buf, pos + off1);
                }
            }
        }

        exports.writeDoubleLE = writeDouble_ieee754.bind(null, writeUintLE, 0, 4);
        exports.writeDoubleBE = writeDouble_ieee754.bind(null, writeUintBE, 4, 0);

        function readDouble_ieee754(readUint, off0, off1, buf, pos) {
            var lo = readUint(buf, pos + off0),
                hi = readUint(buf, pos + off1);
            var sign = (hi >> 31) * 2 + 1,
                exponent = hi >>> 20 & 2047,
                mantissa = 4294967296 * (hi & 1048575) + lo;
            return exponent === 2047
                ? mantissa
                ? NaN
                : sign * Infinity
                : exponent === 0 // denormal
                ? sign * 5e-324 * mantissa
                : sign * Math.pow(2, exponent - 1075) * (mantissa + 4503599627370496);
        }

        exports.readDoubleLE = readDouble_ieee754.bind(null, readUintLE, 0, 4);
        exports.readDoubleBE = readDouble_ieee754.bind(null, readUintBE, 4, 0);

    })();

    return exports;
}

// uint helpers

function writeUintLE(val, buf, pos) {
    buf[pos    ] =  val        & 255;
    buf[pos + 1] =  val >>> 8  & 255;
    buf[pos + 2] =  val >>> 16 & 255;
    buf[pos + 3] =  val >>> 24;
}

function writeUintBE(val, buf, pos) {
    buf[pos    ] =  val >>> 24;
    buf[pos + 1] =  val >>> 16 & 255;
    buf[pos + 2] =  val >>> 8  & 255;
    buf[pos + 3] =  val        & 255;
}

function readUintLE(buf, pos) {
    return (buf[pos    ]
          | buf[pos + 1] << 8
          | buf[pos + 2] << 16
          | buf[pos + 3] << 24) >>> 0;
}

function readUintBE(buf, pos) {
    return (buf[pos    ] << 24
          | buf[pos + 1] << 16
          | buf[pos + 2] << 8
          | buf[pos + 3]) >>> 0;
}


/***/ }),

/***/ "./node_modules/@protobufjs/inquire/index.js":
/*!***************************************************!*\
  !*** ./node_modules/@protobufjs/inquire/index.js ***!
  \***************************************************/
/***/ ((module) => {


module.exports = inquire;

/**
 * Requires a module only if available.
 * @memberof util
 * @param {string} moduleName Module to require
 * @returns {?Object} Required module if available and not empty, otherwise `null`
 */
function inquire(moduleName) {
    try {
        var mod = eval("quire".replace(/^/,"re"))(moduleName); // eslint-disable-line no-eval
        if (mod && (mod.length || Object.keys(mod).length))
            return mod;
    } catch (e) {} // eslint-disable-line no-empty
    return null;
}


/***/ }),

/***/ "./node_modules/@protobufjs/path/index.js":
/*!************************************************!*\
  !*** ./node_modules/@protobufjs/path/index.js ***!
  \************************************************/
/***/ ((__unused_webpack_module, exports) => {



/**
 * A minimal path module to resolve Unix, Windows and URL paths alike.
 * @memberof util
 * @namespace
 */
var path = exports;

var isAbsolute =
/**
 * Tests if the specified path is absolute.
 * @param {string} path Path to test
 * @returns {boolean} `true` if path is absolute
 */
path.isAbsolute = function isAbsolute(path) {
    return /^(?:\/|\w+:)/.test(path);
};

var normalize =
/**
 * Normalizes the specified path.
 * @param {string} path Path to normalize
 * @returns {string} Normalized path
 */
path.normalize = function normalize(path) {
    path = path.replace(/\\/g, "/")
               .replace(/\/{2,}/g, "/");
    var parts    = path.split("/"),
        absolute = isAbsolute(path),
        prefix   = "";
    if (absolute)
        prefix = parts.shift() + "/";
    for (var i = 0; i < parts.length;) {
        if (parts[i] === "..") {
            if (i > 0 && parts[i - 1] !== "..")
                parts.splice(--i, 2);
            else if (absolute)
                parts.splice(i, 1);
            else
                ++i;
        } else if (parts[i] === ".")
            parts.splice(i, 1);
        else
            ++i;
    }
    return prefix + parts.join("/");
};

/**
 * Resolves the specified include path against the specified origin path.
 * @param {string} originPath Path to the origin file
 * @param {string} includePath Include path relative to origin path
 * @param {boolean} [alreadyNormalized=false] `true` if both paths are already known to be normalized
 * @returns {string} Path to the include file
 */
path.resolve = function resolve(originPath, includePath, alreadyNormalized) {
    if (!alreadyNormalized)
        includePath = normalize(includePath);
    if (isAbsolute(includePath))
        return includePath;
    if (!alreadyNormalized)
        originPath = normalize(originPath);
    return (originPath = originPath.replace(/(?:\/|^)[^/]+$/, "")).length ? normalize(originPath + "/" + includePath) : includePath;
};


/***/ }),

/***/ "./node_modules/@protobufjs/pool/index.js":
/*!************************************************!*\
  !*** ./node_modules/@protobufjs/pool/index.js ***!
  \************************************************/
/***/ ((module) => {


module.exports = pool;

/**
 * An allocator as used by {@link util.pool}.
 * @typedef PoolAllocator
 * @type {function}
 * @param {number} size Buffer size
 * @returns {Uint8Array} Buffer
 */

/**
 * A slicer as used by {@link util.pool}.
 * @typedef PoolSlicer
 * @type {function}
 * @param {number} start Start offset
 * @param {number} end End offset
 * @returns {Uint8Array} Buffer slice
 * @this {Uint8Array}
 */

/**
 * A general purpose buffer pool.
 * @memberof util
 * @function
 * @param {PoolAllocator} alloc Allocator
 * @param {PoolSlicer} slice Slicer
 * @param {number} [size=8192] Slab size
 * @returns {PoolAllocator} Pooled allocator
 */
function pool(alloc, slice, size) {
    var SIZE   = size || 8192;
    var MAX    = SIZE >>> 1;
    var slab   = null;
    var offset = SIZE;
    return function pool_alloc(size) {
        if (size < 1 || size > MAX)
            return alloc(size);
        if (offset + size > SIZE) {
            slab = alloc(SIZE);
            offset = 0;
        }
        var buf = slice.call(slab, offset, offset += size);
        if (offset & 7) // align to 32 bit
            offset = (offset | 7) + 1;
        return buf;
    };
}


/***/ }),

/***/ "./node_modules/@protobufjs/utf8/index.js":
/*!************************************************!*\
  !*** ./node_modules/@protobufjs/utf8/index.js ***!
  \************************************************/
/***/ ((__unused_webpack_module, exports) => {



/**
 * A minimal UTF8 implementation for number arrays.
 * @memberof util
 * @namespace
 */
var utf8 = exports;

/**
 * Calculates the UTF8 byte length of a string.
 * @param {string} string String
 * @returns {number} Byte length
 */
utf8.length = function utf8_length(string) {
    var len = 0,
        c = 0;
    for (var i = 0; i < string.length; ++i) {
        c = string.charCodeAt(i);
        if (c < 128)
            len += 1;
        else if (c < 2048)
            len += 2;
        else if ((c & 0xFC00) === 0xD800 && (string.charCodeAt(i + 1) & 0xFC00) === 0xDC00) {
            ++i;
            len += 4;
        } else
            len += 3;
    }
    return len;
};

/**
 * Reads UTF8 bytes as a string.
 * @param {Uint8Array} buffer Source buffer
 * @param {number} start Source start
 * @param {number} end Source end
 * @returns {string} String read
 */
utf8.read = function utf8_read(buffer, start, end) {
    var len = end - start;
    if (len < 1)
        return "";
    var parts = null,
        chunk = [],
        i = 0, // char offset
        t;     // temporary
    while (start < end) {
        t = buffer[start++];
        if (t < 128)
            chunk[i++] = t;
        else if (t > 191 && t < 224)
            chunk[i++] = (t & 31) << 6 | buffer[start++] & 63;
        else if (t > 239 && t < 365) {
            t = ((t & 7) << 18 | (buffer[start++] & 63) << 12 | (buffer[start++] & 63) << 6 | buffer[start++] & 63) - 0x10000;
            chunk[i++] = 0xD800 + (t >> 10);
            chunk[i++] = 0xDC00 + (t & 1023);
        } else
            chunk[i++] = (t & 15) << 12 | (buffer[start++] & 63) << 6 | buffer[start++] & 63;
        if (i > 8191) {
            (parts || (parts = [])).push(String.fromCharCode.apply(String, chunk));
            i = 0;
        }
    }
    if (parts) {
        if (i)
            parts.push(String.fromCharCode.apply(String, chunk.slice(0, i)));
        return parts.join("");
    }
    return String.fromCharCode.apply(String, chunk.slice(0, i));
};

/**
 * Writes a string as UTF8 bytes.
 * @param {string} string Source string
 * @param {Uint8Array} buffer Destination buffer
 * @param {number} offset Destination offset
 * @returns {number} Bytes written
 */
utf8.write = function utf8_write(string, buffer, offset) {
    var start = offset,
        c1, // character 1
        c2; // character 2
    for (var i = 0; i < string.length; ++i) {
        c1 = string.charCodeAt(i);
        if (c1 < 128) {
            buffer[offset++] = c1;
        } else if (c1 < 2048) {
            buffer[offset++] = c1 >> 6       | 192;
            buffer[offset++] = c1       & 63 | 128;
        } else if ((c1 & 0xFC00) === 0xD800 && ((c2 = string.charCodeAt(i + 1)) & 0xFC00) === 0xDC00) {
            c1 = 0x10000 + ((c1 & 0x03FF) << 10) + (c2 & 0x03FF);
            ++i;
            buffer[offset++] = c1 >> 18      | 240;
            buffer[offset++] = c1 >> 12 & 63 | 128;
            buffer[offset++] = c1 >> 6  & 63 | 128;
            buffer[offset++] = c1       & 63 | 128;
        } else {
            buffer[offset++] = c1 >> 12      | 224;
            buffer[offset++] = c1 >> 6  & 63 | 128;
            buffer[offset++] = c1       & 63 | 128;
        }
    }
    return offset - start;
};


/***/ }),

/***/ "./node_modules/protobufjs/index.js":
/*!******************************************!*\
  !*** ./node_modules/protobufjs/index.js ***!
  \******************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {

// full library entry point.


module.exports = __webpack_require__(/*! ./src/index */ "./node_modules/protobufjs/src/index.js");


/***/ }),

/***/ "./node_modules/protobufjs/src/common.js":
/*!***********************************************!*\
  !*** ./node_modules/protobufjs/src/common.js ***!
  \***********************************************/
/***/ ((module) => {


module.exports = common;

var commonRe = /\/|\./;

/**
 * Provides common type definitions.
 * Can also be used to provide additional google types or your own custom types.
 * @param {string} name Short name as in `google/protobuf/[name].proto` or full file name
 * @param {Object.<string,*>} json JSON definition within `google.protobuf` if a short name, otherwise the file's root definition
 * @returns {undefined}
 * @property {INamespace} google/protobuf/any.proto Any
 * @property {INamespace} google/protobuf/duration.proto Duration
 * @property {INamespace} google/protobuf/empty.proto Empty
 * @property {INamespace} google/protobuf/field_mask.proto FieldMask
 * @property {INamespace} google/protobuf/struct.proto Struct, Value, NullValue and ListValue
 * @property {INamespace} google/protobuf/timestamp.proto Timestamp
 * @property {INamespace} google/protobuf/wrappers.proto Wrappers
 * @example
 * // manually provides descriptor.proto (assumes google/protobuf/ namespace and .proto extension)
 * protobuf.common("descriptor", descriptorJson);
 *
 * // manually provides a custom definition (uses my.foo namespace)
 * protobuf.common("my/foo/bar.proto", myFooBarJson);
 */
function common(name, json) {
    if (!commonRe.test(name)) {
        name = "google/protobuf/" + name + ".proto";
        json = { nested: { google: { nested: { protobuf: { nested: json } } } } };
    }
    common[name] = json;
}

// Not provided because of limited use (feel free to discuss or to provide yourself):
//
// google/protobuf/descriptor.proto
// google/protobuf/source_context.proto
// google/protobuf/type.proto
//
// Stripped and pre-parsed versions of these non-bundled files are instead available as part of
// the repository or package within the google/protobuf directory.

common("any", {

    /**
     * Properties of a google.protobuf.Any message.
     * @interface IAny
     * @type {Object}
     * @property {string} [typeUrl]
     * @property {Uint8Array} [bytes]
     * @memberof common
     */
    Any: {
        fields: {
            type_url: {
                type: "string",
                id: 1
            },
            value: {
                type: "bytes",
                id: 2
            }
        }
    }
});

var timeType;

common("duration", {

    /**
     * Properties of a google.protobuf.Duration message.
     * @interface IDuration
     * @type {Object}
     * @property {number|Long} [seconds]
     * @property {number} [nanos]
     * @memberof common
     */
    Duration: timeType = {
        fields: {
            seconds: {
                type: "int64",
                id: 1
            },
            nanos: {
                type: "int32",
                id: 2
            }
        }
    }
});

common("timestamp", {

    /**
     * Properties of a google.protobuf.Timestamp message.
     * @interface ITimestamp
     * @type {Object}
     * @property {number|Long} [seconds]
     * @property {number} [nanos]
     * @memberof common
     */
    Timestamp: timeType
});

common("empty", {

    /**
     * Properties of a google.protobuf.Empty message.
     * @interface IEmpty
     * @memberof common
     */
    Empty: {
        fields: {}
    }
});

common("struct", {

    /**
     * Properties of a google.protobuf.Struct message.
     * @interface IStruct
     * @type {Object}
     * @property {Object.<string,IValue>} [fields]
     * @memberof common
     */
    Struct: {
        fields: {
            fields: {
                keyType: "string",
                type: "Value",
                id: 1
            }
        }
    },

    /**
     * Properties of a google.protobuf.Value message.
     * @interface IValue
     * @type {Object}
     * @property {string} [kind]
     * @property {0} [nullValue]
     * @property {number} [numberValue]
     * @property {string} [stringValue]
     * @property {boolean} [boolValue]
     * @property {IStruct} [structValue]
     * @property {IListValue} [listValue]
     * @memberof common
     */
    Value: {
        oneofs: {
            kind: {
                oneof: [
                    "nullValue",
                    "numberValue",
                    "stringValue",
                    "boolValue",
                    "structValue",
                    "listValue"
                ]
            }
        },
        fields: {
            nullValue: {
                type: "NullValue",
                id: 1
            },
            numberValue: {
                type: "double",
                id: 2
            },
            stringValue: {
                type: "string",
                id: 3
            },
            boolValue: {
                type: "bool",
                id: 4
            },
            structValue: {
                type: "Struct",
                id: 5
            },
            listValue: {
                type: "ListValue",
                id: 6
            }
        }
    },

    NullValue: {
        values: {
            NULL_VALUE: 0
        }
    },

    /**
     * Properties of a google.protobuf.ListValue message.
     * @interface IListValue
     * @type {Object}
     * @property {Array.<IValue>} [values]
     * @memberof common
     */
    ListValue: {
        fields: {
            values: {
                rule: "repeated",
                type: "Value",
                id: 1
            }
        }
    }
});

common("wrappers", {

    /**
     * Properties of a google.protobuf.DoubleValue message.
     * @interface IDoubleValue
     * @type {Object}
     * @property {number} [value]
     * @memberof common
     */
    DoubleValue: {
        fields: {
            value: {
                type: "double",
                id: 1
            }
        }
    },

    /**
     * Properties of a google.protobuf.FloatValue message.
     * @interface IFloatValue
     * @type {Object}
     * @property {number} [value]
     * @memberof common
     */
    FloatValue: {
        fields: {
            value: {
                type: "float",
                id: 1
            }
        }
    },

    /**
     * Properties of a google.protobuf.Int64Value message.
     * @interface IInt64Value
     * @type {Object}
     * @property {number|Long} [value]
     * @memberof common
     */
    Int64Value: {
        fields: {
            value: {
                type: "int64",
                id: 1
            }
        }
    },

    /**
     * Properties of a google.protobuf.UInt64Value message.
     * @interface IUInt64Value
     * @type {Object}
     * @property {number|Long} [value]
     * @memberof common
     */
    UInt64Value: {
        fields: {
            value: {
                type: "uint64",
                id: 1
            }
        }
    },

    /**
     * Properties of a google.protobuf.Int32Value message.
     * @interface IInt32Value
     * @type {Object}
     * @property {number} [value]
     * @memberof common
     */
    Int32Value: {
        fields: {
            value: {
                type: "int32",
                id: 1
            }
        }
    },

    /**
     * Properties of a google.protobuf.UInt32Value message.
     * @interface IUInt32Value
     * @type {Object}
     * @property {number} [value]
     * @memberof common
     */
    UInt32Value: {
        fields: {
            value: {
                type: "uint32",
                id: 1
            }
        }
    },

    /**
     * Properties of a google.protobuf.BoolValue message.
     * @interface IBoolValue
     * @type {Object}
     * @property {boolean} [value]
     * @memberof common
     */
    BoolValue: {
        fields: {
            value: {
                type: "bool",
                id: 1
            }
        }
    },

    /**
     * Properties of a google.protobuf.StringValue message.
     * @interface IStringValue
     * @type {Object}
     * @property {string} [value]
     * @memberof common
     */
    StringValue: {
        fields: {
            value: {
                type: "string",
                id: 1
            }
        }
    },

    /**
     * Properties of a google.protobuf.BytesValue message.
     * @interface IBytesValue
     * @type {Object}
     * @property {Uint8Array} [value]
     * @memberof common
     */
    BytesValue: {
        fields: {
            value: {
                type: "bytes",
                id: 1
            }
        }
    }
});

common("field_mask", {

    /**
     * Properties of a google.protobuf.FieldMask message.
     * @interface IDoubleValue
     * @type {Object}
     * @property {number} [value]
     * @memberof common
     */
    FieldMask: {
        fields: {
            paths: {
                rule: "repeated",
                type: "string",
                id: 1
            }
        }
    }
});

/**
 * Gets the root definition of the specified common proto file.
 *
 * Bundled definitions are:
 * - google/protobuf/any.proto
 * - google/protobuf/duration.proto
 * - google/protobuf/empty.proto
 * - google/protobuf/field_mask.proto
 * - google/protobuf/struct.proto
 * - google/protobuf/timestamp.proto
 * - google/protobuf/wrappers.proto
 *
 * @param {string} file Proto file name
 * @returns {INamespace|null} Root definition or `null` if not defined
 */
common.get = function get(file) {
    return common[file] || null;
};


/***/ }),

/***/ "./node_modules/protobufjs/src/converter.js":
/*!**************************************************!*\
  !*** ./node_modules/protobufjs/src/converter.js ***!
  \**************************************************/
/***/ ((__unused_webpack_module, exports, __webpack_require__) => {


/**
 * Runtime message from/to plain object converters.
 * @namespace
 */
var converter = exports;

var Enum = __webpack_require__(/*! ./enum */ "./node_modules/protobufjs/src/enum.js"),
    util = __webpack_require__(/*! ./util */ "./node_modules/protobufjs/src/util.js");

/**
 * Generates a partial value fromObject conveter.
 * @param {Codegen} gen Codegen instance
 * @param {Field} field Reflected field
 * @param {number} fieldIndex Field index
 * @param {string} prop Property reference
 * @returns {Codegen} Codegen instance
 * @ignore
 */
function genValuePartial_fromObject(gen, field, fieldIndex, prop) {
    var defaultAlreadyEmitted = false;
    /* eslint-disable no-unexpected-multiline, block-scoped-var, no-redeclare */
    if (field.resolvedType) {
        if (field.resolvedType instanceof Enum) { gen
            ("switch(d%s){", prop);
            for (var values = field.resolvedType.values, keys = Object.keys(values), i = 0; i < keys.length; ++i) {
                // enum unknown values passthrough
                if (values[keys[i]] === field.typeDefault && !defaultAlreadyEmitted) { gen
                    ("default:")
                        ("if(typeof(d%s)===\"number\"){m%s=d%s;break}", prop, prop, prop);
                    if (!field.repeated) gen // fallback to default value only for
                                             // arrays, to avoid leaving holes.
                        ("break");           // for non-repeated fields, just ignore
                    defaultAlreadyEmitted = true;
                }
                gen
                ("case%j:", keys[i])
                ("case %i:", values[keys[i]])
                    ("m%s=%j", prop, values[keys[i]])
                    ("break");
            } gen
            ("}");
        } else gen
            ("if(typeof d%s!==\"object\")", prop)
                ("throw TypeError(%j)", field.fullName + ": object expected")
            ("m%s=types[%i].fromObject(d%s)", prop, fieldIndex, prop);
    } else {
        var isUnsigned = false;
        switch (field.type) {
            case "double":
            case "float": gen
                ("m%s=Number(d%s)", prop, prop); // also catches "NaN", "Infinity"
                break;
            case "uint32":
            case "fixed32": gen
                ("m%s=d%s>>>0", prop, prop);
                break;
            case "int32":
            case "sint32":
            case "sfixed32": gen
                ("m%s=d%s|0", prop, prop);
                break;
            case "uint64":
                isUnsigned = true;
                // eslint-disable-next-line no-fallthrough
            case "int64":
            case "sint64":
            case "fixed64":
            case "sfixed64": gen
                ("if(util.Long)")
                    ("(m%s=util.Long.fromValue(d%s)).unsigned=%j", prop, prop, isUnsigned)
                ("else if(typeof d%s===\"string\")", prop)
                    ("m%s=parseInt(d%s,10)", prop, prop)
                ("else if(typeof d%s===\"number\")", prop)
                    ("m%s=d%s", prop, prop)
                ("else if(typeof d%s===\"object\")", prop)
                    ("m%s=new util.LongBits(d%s.low>>>0,d%s.high>>>0).toNumber(%s)", prop, prop, prop, isUnsigned ? "true" : "");
                break;
            case "bytes": gen
                ("if(typeof d%s===\"string\")", prop)
                    ("util.base64.decode(d%s,m%s=util.newBuffer(util.base64.length(d%s)),0)", prop, prop, prop)
                ("else if(d%s.length >= 0)", prop)
                    ("m%s=d%s", prop, prop);
                break;
            case "string": gen
                ("m%s=String(d%s)", prop, prop);
                break;
            case "bool": gen
                ("m%s=Boolean(d%s)", prop, prop);
                break;
            /* default: gen
                ("m%s=d%s", prop, prop);
                break; */
        }
    }
    return gen;
    /* eslint-enable no-unexpected-multiline, block-scoped-var, no-redeclare */
}

/**
 * Generates a plain object to runtime message converter specific to the specified message type.
 * @param {Type} mtype Message type
 * @returns {Codegen} Codegen instance
 */
converter.fromObject = function fromObject(mtype) {
    /* eslint-disable no-unexpected-multiline, block-scoped-var, no-redeclare */
    var fields = mtype.fieldsArray;
    var gen = util.codegen(["d"], mtype.name + "$fromObject")
    ("if(d instanceof this.ctor)")
        ("return d");
    if (!fields.length) return gen
    ("return new this.ctor");
    gen
    ("var m=new this.ctor");
    for (var i = 0; i < fields.length; ++i) {
        var field  = fields[i].resolve(),
            prop   = util.safeProp(field.name);

        // Map fields
        if (field.map) { gen
    ("if(d%s){", prop)
        ("if(typeof d%s!==\"object\")", prop)
            ("throw TypeError(%j)", field.fullName + ": object expected")
        ("m%s={}", prop)
        ("for(var ks=Object.keys(d%s),i=0;i<ks.length;++i){", prop);
            genValuePartial_fromObject(gen, field, /* not sorted */ i, prop + "[ks[i]]")
        ("}")
    ("}");

        // Repeated fields
        } else if (field.repeated) { gen
    ("if(d%s){", prop)
        ("if(!Array.isArray(d%s))", prop)
            ("throw TypeError(%j)", field.fullName + ": array expected")
        ("m%s=[]", prop)
        ("for(var i=0;i<d%s.length;++i){", prop);
            genValuePartial_fromObject(gen, field, /* not sorted */ i, prop + "[i]")
        ("}")
    ("}");

        // Non-repeated fields
        } else {
            if (!(field.resolvedType instanceof Enum)) gen // no need to test for null/undefined if an enum (uses switch)
    ("if(d%s!=null){", prop); // !== undefined && !== null
        genValuePartial_fromObject(gen, field, /* not sorted */ i, prop);
            if (!(field.resolvedType instanceof Enum)) gen
    ("}");
        }
    } return gen
    ("return m");
    /* eslint-enable no-unexpected-multiline, block-scoped-var, no-redeclare */
};

/**
 * Generates a partial value toObject converter.
 * @param {Codegen} gen Codegen instance
 * @param {Field} field Reflected field
 * @param {number} fieldIndex Field index
 * @param {string} prop Property reference
 * @returns {Codegen} Codegen instance
 * @ignore
 */
function genValuePartial_toObject(gen, field, fieldIndex, prop) {
    /* eslint-disable no-unexpected-multiline, block-scoped-var, no-redeclare */
    if (field.resolvedType) {
        if (field.resolvedType instanceof Enum) gen
            ("d%s=o.enums===String?(types[%i].values[m%s]===undefined?m%s:types[%i].values[m%s]):m%s", prop, fieldIndex, prop, prop, fieldIndex, prop, prop);
        else gen
            ("d%s=types[%i].toObject(m%s,o)", prop, fieldIndex, prop);
    } else {
        var isUnsigned = false;
        switch (field.type) {
            case "double":
            case "float": gen
            ("d%s=o.json&&!isFinite(m%s)?String(m%s):m%s", prop, prop, prop, prop);
                break;
            case "uint64":
                isUnsigned = true;
                // eslint-disable-next-line no-fallthrough
            case "int64":
            case "sint64":
            case "fixed64":
            case "sfixed64": gen
            ("if(typeof m%s===\"number\")", prop)
                ("d%s=o.longs===String?String(m%s):m%s", prop, prop, prop)
            ("else") // Long-like
                ("d%s=o.longs===String?util.Long.prototype.toString.call(m%s):o.longs===Number?new util.LongBits(m%s.low>>>0,m%s.high>>>0).toNumber(%s):m%s", prop, prop, prop, prop, isUnsigned ? "true": "", prop);
                break;
            case "bytes": gen
            ("d%s=o.bytes===String?util.base64.encode(m%s,0,m%s.length):o.bytes===Array?Array.prototype.slice.call(m%s):m%s", prop, prop, prop, prop, prop);
                break;
            default: gen
            ("d%s=m%s", prop, prop);
                break;
        }
    }
    return gen;
    /* eslint-enable no-unexpected-multiline, block-scoped-var, no-redeclare */
}

/**
 * Generates a runtime message to plain object converter specific to the specified message type.
 * @param {Type} mtype Message type
 * @returns {Codegen} Codegen instance
 */
converter.toObject = function toObject(mtype) {
    /* eslint-disable no-unexpected-multiline, block-scoped-var, no-redeclare */
    var fields = mtype.fieldsArray.slice().sort(util.compareFieldsById);
    if (!fields.length)
        return util.codegen()("return {}");
    var gen = util.codegen(["m", "o"], mtype.name + "$toObject")
    ("if(!o)")
        ("o={}")
    ("var d={}");

    var repeatedFields = [],
        mapFields = [],
        normalFields = [],
        i = 0;
    for (; i < fields.length; ++i)
        if (!fields[i].partOf)
            ( fields[i].resolve().repeated ? repeatedFields
            : fields[i].map ? mapFields
            : normalFields).push(fields[i]);

    if (repeatedFields.length) { gen
    ("if(o.arrays||o.defaults){");
        for (i = 0; i < repeatedFields.length; ++i) gen
        ("d%s=[]", util.safeProp(repeatedFields[i].name));
        gen
    ("}");
    }

    if (mapFields.length) { gen
    ("if(o.objects||o.defaults){");
        for (i = 0; i < mapFields.length; ++i) gen
        ("d%s={}", util.safeProp(mapFields[i].name));
        gen
    ("}");
    }

    if (normalFields.length) { gen
    ("if(o.defaults){");
        for (i = 0; i < normalFields.length; ++i) {
            var field = normalFields[i],
                prop  = util.safeProp(field.name);
            if (field.resolvedType instanceof Enum) gen
        ("d%s=o.enums===String?%j:%j", prop, field.resolvedType.valuesById[field.typeDefault], field.typeDefault);
            else if (field.long) gen
        ("if(util.Long){")
            ("var n=new util.Long(%i,%i,%j)", field.typeDefault.low, field.typeDefault.high, field.typeDefault.unsigned)
            ("d%s=o.longs===String?n.toString():o.longs===Number?n.toNumber():n", prop)
        ("}else")
            ("d%s=o.longs===String?%j:%i", prop, field.typeDefault.toString(), field.typeDefault.toNumber());
            else if (field.bytes) {
                var arrayDefault = "[" + Array.prototype.slice.call(field.typeDefault).join(",") + "]";
                gen
        ("if(o.bytes===String)d%s=%j", prop, String.fromCharCode.apply(String, field.typeDefault))
        ("else{")
            ("d%s=%s", prop, arrayDefault)
            ("if(o.bytes!==Array)d%s=util.newBuffer(d%s)", prop, prop)
        ("}");
            } else gen
        ("d%s=%j", prop, field.typeDefault); // also messages (=null)
        } gen
    ("}");
    }
    var hasKs2 = false;
    for (i = 0; i < fields.length; ++i) {
        var field = fields[i],
            index = mtype._fieldsArray.indexOf(field),
            prop  = util.safeProp(field.name);
        if (field.map) {
            if (!hasKs2) { hasKs2 = true; gen
    ("var ks2");
            } gen
    ("if(m%s&&(ks2=Object.keys(m%s)).length){", prop, prop)
        ("d%s={}", prop)
        ("for(var j=0;j<ks2.length;++j){");
            genValuePartial_toObject(gen, field, /* sorted */ index, prop + "[ks2[j]]")
        ("}");
        } else if (field.repeated) { gen
    ("if(m%s&&m%s.length){", prop, prop)
        ("d%s=[]", prop)
        ("for(var j=0;j<m%s.length;++j){", prop);
            genValuePartial_toObject(gen, field, /* sorted */ index, prop + "[j]")
        ("}");
        } else { gen
    ("if(m%s!=null&&m.hasOwnProperty(%j)){", prop, field.name); // !== undefined && !== null
        genValuePartial_toObject(gen, field, /* sorted */ index, prop);
        if (field.partOf) gen
        ("if(o.oneofs)")
            ("d%s=%j", util.safeProp(field.partOf.name), field.name);
        }
        gen
    ("}");
    }
    return gen
    ("return d");
    /* eslint-enable no-unexpected-multiline, block-scoped-var, no-redeclare */
};


/***/ }),

/***/ "./node_modules/protobufjs/src/decoder.js":
/*!************************************************!*\
  !*** ./node_modules/protobufjs/src/decoder.js ***!
  \************************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = decoder;

var Enum    = __webpack_require__(/*! ./enum */ "./node_modules/protobufjs/src/enum.js"),
    types   = __webpack_require__(/*! ./types */ "./node_modules/protobufjs/src/types.js"),
    util    = __webpack_require__(/*! ./util */ "./node_modules/protobufjs/src/util.js");

function missing(field) {
    return "missing required '" + field.name + "'";
}

/**
 * Generates a decoder specific to the specified message type.
 * @param {Type} mtype Message type
 * @returns {Codegen} Codegen instance
 */
function decoder(mtype) {
    /* eslint-disable no-unexpected-multiline */
    var gen = util.codegen(["r", "l"], mtype.name + "$decode")
    ("if(!(r instanceof Reader))")
        ("r=Reader.create(r)")
    ("var c=l===undefined?r.len:r.pos+l,m=new this.ctor" + (mtype.fieldsArray.filter(function(field) { return field.map; }).length ? ",k,value" : ""))
    ("while(r.pos<c){")
        ("var t=r.uint32()");
    if (mtype.group) gen
        ("if((t&7)===4)")
            ("break");
    gen
        ("switch(t>>>3){");

    var i = 0;
    for (; i < /* initializes */ mtype.fieldsArray.length; ++i) {
        var field = mtype._fieldsArray[i].resolve(),
            type  = field.resolvedType instanceof Enum ? "int32" : field.type,
            ref   = "m" + util.safeProp(field.name); gen
            ("case %i: {", field.id);

        // Map fields
        if (field.map) { gen
                ("if(%s===util.emptyObject)", ref)
                    ("%s={}", ref)
                ("var c2 = r.uint32()+r.pos");

            if (types.defaults[field.keyType] !== undefined) gen
                ("k=%j", types.defaults[field.keyType]);
            else gen
                ("k=null");

            if (types.defaults[type] !== undefined) gen
                ("value=%j", types.defaults[type]);
            else gen
                ("value=null");

            gen
                ("while(r.pos<c2){")
                    ("var tag2=r.uint32()")
                    ("switch(tag2>>>3){")
                        ("case 1: k=r.%s(); break", field.keyType)
                        ("case 2:");

            if (types.basic[type] === undefined) gen
                            ("value=types[%i].decode(r,r.uint32())", i); // can't be groups
            else gen
                            ("value=r.%s()", type);

            gen
                            ("break")
                        ("default:")
                            ("r.skipType(tag2&7)")
                            ("break")
                    ("}")
                ("}");

            if (types.long[field.keyType] !== undefined) gen
                ("%s[typeof k===\"object\"?util.longToHash(k):k]=value", ref);
            else gen
                ("%s[k]=value", ref);

        // Repeated fields
        } else if (field.repeated) { gen

                ("if(!(%s&&%s.length))", ref, ref)
                    ("%s=[]", ref);

            // Packable (always check for forward and backward compatiblity)
            if (types.packed[type] !== undefined) gen
                ("if((t&7)===2){")
                    ("var c2=r.uint32()+r.pos")
                    ("while(r.pos<c2)")
                        ("%s.push(r.%s())", ref, type)
                ("}else");

            // Non-packed
            if (types.basic[type] === undefined) gen(field.resolvedType.group
                    ? "%s.push(types[%i].decode(r))"
                    : "%s.push(types[%i].decode(r,r.uint32()))", ref, i);
            else gen
                    ("%s.push(r.%s())", ref, type);

        // Non-repeated
        } else if (types.basic[type] === undefined) gen(field.resolvedType.group
                ? "%s=types[%i].decode(r)"
                : "%s=types[%i].decode(r,r.uint32())", ref, i);
        else gen
                ("%s=r.%s()", ref, type);
        gen
                ("break")
            ("}");
        // Unknown fields
    } gen
            ("default:")
                ("r.skipType(t&7)")
                ("break")

        ("}")
    ("}");

    // Field presence
    for (i = 0; i < mtype._fieldsArray.length; ++i) {
        var rfield = mtype._fieldsArray[i];
        if (rfield.required) gen
    ("if(!m.hasOwnProperty(%j))", rfield.name)
        ("throw util.ProtocolError(%j,{instance:m})", missing(rfield));
    }

    return gen
    ("return m");
    /* eslint-enable no-unexpected-multiline */
}


/***/ }),

/***/ "./node_modules/protobufjs/src/encoder.js":
/*!************************************************!*\
  !*** ./node_modules/protobufjs/src/encoder.js ***!
  \************************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = encoder;

var Enum     = __webpack_require__(/*! ./enum */ "./node_modules/protobufjs/src/enum.js"),
    types    = __webpack_require__(/*! ./types */ "./node_modules/protobufjs/src/types.js"),
    util     = __webpack_require__(/*! ./util */ "./node_modules/protobufjs/src/util.js");

/**
 * Generates a partial message type encoder.
 * @param {Codegen} gen Codegen instance
 * @param {Field} field Reflected field
 * @param {number} fieldIndex Field index
 * @param {string} ref Variable reference
 * @returns {Codegen} Codegen instance
 * @ignore
 */
function genTypePartial(gen, field, fieldIndex, ref) {
    return field.resolvedType.group
        ? gen("types[%i].encode(%s,w.uint32(%i)).uint32(%i)", fieldIndex, ref, (field.id << 3 | 3) >>> 0, (field.id << 3 | 4) >>> 0)
        : gen("types[%i].encode(%s,w.uint32(%i).fork()).ldelim()", fieldIndex, ref, (field.id << 3 | 2) >>> 0);
}

/**
 * Generates an encoder specific to the specified message type.
 * @param {Type} mtype Message type
 * @returns {Codegen} Codegen instance
 */
function encoder(mtype) {
    /* eslint-disable no-unexpected-multiline, block-scoped-var, no-redeclare */
    var gen = util.codegen(["m", "w"], mtype.name + "$encode")
    ("if(!w)")
        ("w=Writer.create()");

    var i, ref;

    // "when a message is serialized its known fields should be written sequentially by field number"
    var fields = /* initializes */ mtype.fieldsArray.slice().sort(util.compareFieldsById);

    for (var i = 0; i < fields.length; ++i) {
        var field    = fields[i].resolve(),
            index    = mtype._fieldsArray.indexOf(field),
            type     = field.resolvedType instanceof Enum ? "int32" : field.type,
            wireType = types.basic[type];
            ref      = "m" + util.safeProp(field.name);

        // Map fields
        if (field.map) {
            gen
    ("if(%s!=null&&Object.hasOwnProperty.call(m,%j)){", ref, field.name) // !== undefined && !== null
        ("for(var ks=Object.keys(%s),i=0;i<ks.length;++i){", ref)
            ("w.uint32(%i).fork().uint32(%i).%s(ks[i])", (field.id << 3 | 2) >>> 0, 8 | types.mapKey[field.keyType], field.keyType);
            if (wireType === undefined) gen
            ("types[%i].encode(%s[ks[i]],w.uint32(18).fork()).ldelim().ldelim()", index, ref); // can't be groups
            else gen
            (".uint32(%i).%s(%s[ks[i]]).ldelim()", 16 | wireType, type, ref);
            gen
        ("}")
    ("}");

            // Repeated fields
        } else if (field.repeated) { gen
    ("if(%s!=null&&%s.length){", ref, ref); // !== undefined && !== null

            // Packed repeated
            if (field.packed && types.packed[type] !== undefined) { gen

        ("w.uint32(%i).fork()", (field.id << 3 | 2) >>> 0)
        ("for(var i=0;i<%s.length;++i)", ref)
            ("w.%s(%s[i])", type, ref)
        ("w.ldelim()");

            // Non-packed
            } else { gen

        ("for(var i=0;i<%s.length;++i)", ref);
                if (wireType === undefined)
            genTypePartial(gen, field, index, ref + "[i]");
                else gen
            ("w.uint32(%i).%s(%s[i])", (field.id << 3 | wireType) >>> 0, type, ref);

            } gen
    ("}");

        // Non-repeated
        } else {
            if (field.optional) gen
    ("if(%s!=null&&Object.hasOwnProperty.call(m,%j))", ref, field.name); // !== undefined && !== null

            if (wireType === undefined)
        genTypePartial(gen, field, index, ref);
            else gen
        ("w.uint32(%i).%s(%s)", (field.id << 3 | wireType) >>> 0, type, ref);

        }
    }

    return gen
    ("return w");
    /* eslint-enable no-unexpected-multiline, block-scoped-var, no-redeclare */
}


/***/ }),

/***/ "./node_modules/protobufjs/src/enum.js":
/*!*********************************************!*\
  !*** ./node_modules/protobufjs/src/enum.js ***!
  \*********************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = Enum;

// extends ReflectionObject
var ReflectionObject = __webpack_require__(/*! ./object */ "./node_modules/protobufjs/src/object.js");
((Enum.prototype = Object.create(ReflectionObject.prototype)).constructor = Enum).className = "Enum";

var Namespace = __webpack_require__(/*! ./namespace */ "./node_modules/protobufjs/src/namespace.js"),
    util = __webpack_require__(/*! ./util */ "./node_modules/protobufjs/src/util.js");

/**
 * Constructs a new enum instance.
 * @classdesc Reflected enum.
 * @extends ReflectionObject
 * @constructor
 * @param {string} name Unique name within its namespace
 * @param {Object.<string,number>} [values] Enum values as an object, by name
 * @param {Object.<string,*>} [options] Declared options
 * @param {string} [comment] The comment for this enum
 * @param {Object.<string,string>} [comments] The value comments for this enum
 * @param {Object.<string,Object<string,*>>|undefined} [valuesOptions] The value options for this enum
 */
function Enum(name, values, options, comment, comments, valuesOptions) {
    ReflectionObject.call(this, name, options);

    if (values && typeof values !== "object")
        throw TypeError("values must be an object");

    /**
     * Enum values by id.
     * @type {Object.<number,string>}
     */
    this.valuesById = {};

    /**
     * Enum values by name.
     * @type {Object.<string,number>}
     */
    this.values = Object.create(this.valuesById); // toJSON, marker

    /**
     * Enum comment text.
     * @type {string|null}
     */
    this.comment = comment;

    /**
     * Value comment texts, if any.
     * @type {Object.<string,string>}
     */
    this.comments = comments || {};

    /**
     * Values options, if any
     * @type {Object<string, Object<string, *>>|undefined}
     */
    this.valuesOptions = valuesOptions;

    /**
     * Reserved ranges, if any.
     * @type {Array.<number[]|string>}
     */
    this.reserved = undefined; // toJSON

    // Note that values inherit valuesById on their prototype which makes them a TypeScript-
    // compatible enum. This is used by pbts to write actual enum definitions that work for
    // static and reflection code alike instead of emitting generic object definitions.

    if (values)
        for (var keys = Object.keys(values), i = 0; i < keys.length; ++i)
            if (typeof values[keys[i]] === "number") // use forward entries only
                this.valuesById[ this.values[keys[i]] = values[keys[i]] ] = keys[i];
}

/**
 * Enum descriptor.
 * @interface IEnum
 * @property {Object.<string,number>} values Enum values
 * @property {Object.<string,*>} [options] Enum options
 */

/**
 * Constructs an enum from an enum descriptor.
 * @param {string} name Enum name
 * @param {IEnum} json Enum descriptor
 * @returns {Enum} Created enum
 * @throws {TypeError} If arguments are invalid
 */
Enum.fromJSON = function fromJSON(name, json) {
    var enm = new Enum(name, json.values, json.options, json.comment, json.comments);
    enm.reserved = json.reserved;
    return enm;
};

/**
 * Converts this enum to an enum descriptor.
 * @param {IToJSONOptions} [toJSONOptions] JSON conversion options
 * @returns {IEnum} Enum descriptor
 */
Enum.prototype.toJSON = function toJSON(toJSONOptions) {
    var keepComments = toJSONOptions ? Boolean(toJSONOptions.keepComments) : false;
    return util.toObject([
        "options"       , this.options,
        "valuesOptions" , this.valuesOptions,
        "values"        , this.values,
        "reserved"      , this.reserved && this.reserved.length ? this.reserved : undefined,
        "comment"       , keepComments ? this.comment : undefined,
        "comments"      , keepComments ? this.comments : undefined
    ]);
};

/**
 * Adds a value to this enum.
 * @param {string} name Value name
 * @param {number} id Value id
 * @param {string} [comment] Comment, if any
 * @param {Object.<string, *>|undefined} [options] Options, if any
 * @returns {Enum} `this`
 * @throws {TypeError} If arguments are invalid
 * @throws {Error} If there is already a value with this name or id
 */
Enum.prototype.add = function add(name, id, comment, options) {
    // utilized by the parser but not by .fromJSON

    if (!util.isString(name))
        throw TypeError("name must be a string");

    if (!util.isInteger(id))
        throw TypeError("id must be an integer");

    if (this.values[name] !== undefined)
        throw Error("duplicate name '" + name + "' in " + this);

    if (this.isReservedId(id))
        throw Error("id " + id + " is reserved in " + this);

    if (this.isReservedName(name))
        throw Error("name '" + name + "' is reserved in " + this);

    if (this.valuesById[id] !== undefined) {
        if (!(this.options && this.options.allow_alias))
            throw Error("duplicate id " + id + " in " + this);
        this.values[name] = id;
    } else
        this.valuesById[this.values[name] = id] = name;

    if (options) {
        if (this.valuesOptions === undefined)
            this.valuesOptions = {};
        this.valuesOptions[name] = options || null;
    }

    this.comments[name] = comment || null;
    return this;
};

/**
 * Removes a value from this enum
 * @param {string} name Value name
 * @returns {Enum} `this`
 * @throws {TypeError} If arguments are invalid
 * @throws {Error} If `name` is not a name of this enum
 */
Enum.prototype.remove = function remove(name) {

    if (!util.isString(name))
        throw TypeError("name must be a string");

    var val = this.values[name];
    if (val == null)
        throw Error("name '" + name + "' does not exist in " + this);

    delete this.valuesById[val];
    delete this.values[name];
    delete this.comments[name];
    if (this.valuesOptions)
        delete this.valuesOptions[name];

    return this;
};

/**
 * Tests if the specified id is reserved.
 * @param {number} id Id to test
 * @returns {boolean} `true` if reserved, otherwise `false`
 */
Enum.prototype.isReservedId = function isReservedId(id) {
    return Namespace.isReservedId(this.reserved, id);
};

/**
 * Tests if the specified name is reserved.
 * @param {string} name Name to test
 * @returns {boolean} `true` if reserved, otherwise `false`
 */
Enum.prototype.isReservedName = function isReservedName(name) {
    return Namespace.isReservedName(this.reserved, name);
};


/***/ }),

/***/ "./node_modules/protobufjs/src/field.js":
/*!**********************************************!*\
  !*** ./node_modules/protobufjs/src/field.js ***!
  \**********************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = Field;

// extends ReflectionObject
var ReflectionObject = __webpack_require__(/*! ./object */ "./node_modules/protobufjs/src/object.js");
((Field.prototype = Object.create(ReflectionObject.prototype)).constructor = Field).className = "Field";

var Enum  = __webpack_require__(/*! ./enum */ "./node_modules/protobufjs/src/enum.js"),
    types = __webpack_require__(/*! ./types */ "./node_modules/protobufjs/src/types.js"),
    util  = __webpack_require__(/*! ./util */ "./node_modules/protobufjs/src/util.js");

var Type; // cyclic

var ruleRe = /^required|optional|repeated$/;

/**
 * Constructs a new message field instance. Note that {@link MapField|map fields} have their own class.
 * @name Field
 * @classdesc Reflected message field.
 * @extends FieldBase
 * @constructor
 * @param {string} name Unique name within its namespace
 * @param {number} id Unique id within its namespace
 * @param {string} type Value type
 * @param {string|Object.<string,*>} [rule="optional"] Field rule
 * @param {string|Object.<string,*>} [extend] Extended type if different from parent
 * @param {Object.<string,*>} [options] Declared options
 */

/**
 * Constructs a field from a field descriptor.
 * @param {string} name Field name
 * @param {IField} json Field descriptor
 * @returns {Field} Created field
 * @throws {TypeError} If arguments are invalid
 */
Field.fromJSON = function fromJSON(name, json) {
    return new Field(name, json.id, json.type, json.rule, json.extend, json.options, json.comment);
};

/**
 * Not an actual constructor. Use {@link Field} instead.
 * @classdesc Base class of all reflected message fields. This is not an actual class but here for the sake of having consistent type definitions.
 * @exports FieldBase
 * @extends ReflectionObject
 * @constructor
 * @param {string} name Unique name within its namespace
 * @param {number} id Unique id within its namespace
 * @param {string} type Value type
 * @param {string|Object.<string,*>} [rule="optional"] Field rule
 * @param {string|Object.<string,*>} [extend] Extended type if different from parent
 * @param {Object.<string,*>} [options] Declared options
 * @param {string} [comment] Comment associated with this field
 */
function Field(name, id, type, rule, extend, options, comment) {

    if (util.isObject(rule)) {
        comment = extend;
        options = rule;
        rule = extend = undefined;
    } else if (util.isObject(extend)) {
        comment = options;
        options = extend;
        extend = undefined;
    }

    ReflectionObject.call(this, name, options);

    if (!util.isInteger(id) || id < 0)
        throw TypeError("id must be a non-negative integer");

    if (!util.isString(type))
        throw TypeError("type must be a string");

    if (rule !== undefined && !ruleRe.test(rule = rule.toString().toLowerCase()))
        throw TypeError("rule must be a string rule");

    if (extend !== undefined && !util.isString(extend))
        throw TypeError("extend must be a string");

    /**
     * Field rule, if any.
     * @type {string|undefined}
     */
    if (rule === "proto3_optional") {
        rule = "optional";
    }
    this.rule = rule && rule !== "optional" ? rule : undefined; // toJSON

    /**
     * Field type.
     * @type {string}
     */
    this.type = type; // toJSON

    /**
     * Unique field id.
     * @type {number}
     */
    this.id = id; // toJSON, marker

    /**
     * Extended type if different from parent.
     * @type {string|undefined}
     */
    this.extend = extend || undefined; // toJSON

    /**
     * Whether this field is required.
     * @type {boolean}
     */
    this.required = rule === "required";

    /**
     * Whether this field is optional.
     * @type {boolean}
     */
    this.optional = !this.required;

    /**
     * Whether this field is repeated.
     * @type {boolean}
     */
    this.repeated = rule === "repeated";

    /**
     * Whether this field is a map or not.
     * @type {boolean}
     */
    this.map = false;

    /**
     * Message this field belongs to.
     * @type {Type|null}
     */
    this.message = null;

    /**
     * OneOf this field belongs to, if any,
     * @type {OneOf|null}
     */
    this.partOf = null;

    /**
     * The field type's default value.
     * @type {*}
     */
    this.typeDefault = null;

    /**
     * The field's default value on prototypes.
     * @type {*}
     */
    this.defaultValue = null;

    /**
     * Whether this field's value should be treated as a long.
     * @type {boolean}
     */
    this.long = util.Long ? types.long[type] !== undefined : /* istanbul ignore next */ false;

    /**
     * Whether this field's value is a buffer.
     * @type {boolean}
     */
    this.bytes = type === "bytes";

    /**
     * Resolved type if not a basic type.
     * @type {Type|Enum|null}
     */
    this.resolvedType = null;

    /**
     * Sister-field within the extended type if a declaring extension field.
     * @type {Field|null}
     */
    this.extensionField = null;

    /**
     * Sister-field within the declaring namespace if an extended field.
     * @type {Field|null}
     */
    this.declaringField = null;

    /**
     * Internally remembers whether this field is packed.
     * @type {boolean|null}
     * @private
     */
    this._packed = null;

    /**
     * Comment for this field.
     * @type {string|null}
     */
    this.comment = comment;
}

/**
 * Determines whether this field is packed. Only relevant when repeated and working with proto2.
 * @name Field#packed
 * @type {boolean}
 * @readonly
 */
Object.defineProperty(Field.prototype, "packed", {
    get: function() {
        // defaults to packed=true if not explicity set to false
        if (this._packed === null)
            this._packed = this.getOption("packed") !== false;
        return this._packed;
    }
});

/**
 * @override
 */
Field.prototype.setOption = function setOption(name, value, ifNotSet) {
    if (name === "packed") // clear cached before setting
        this._packed = null;
    return ReflectionObject.prototype.setOption.call(this, name, value, ifNotSet);
};

/**
 * Field descriptor.
 * @interface IField
 * @property {string} [rule="optional"] Field rule
 * @property {string} type Field type
 * @property {number} id Field id
 * @property {Object.<string,*>} [options] Field options
 */

/**
 * Extension field descriptor.
 * @interface IExtensionField
 * @extends IField
 * @property {string} extend Extended type
 */

/**
 * Converts this field to a field descriptor.
 * @param {IToJSONOptions} [toJSONOptions] JSON conversion options
 * @returns {IField} Field descriptor
 */
Field.prototype.toJSON = function toJSON(toJSONOptions) {
    var keepComments = toJSONOptions ? Boolean(toJSONOptions.keepComments) : false;
    return util.toObject([
        "rule"    , this.rule !== "optional" && this.rule || undefined,
        "type"    , this.type,
        "id"      , this.id,
        "extend"  , this.extend,
        "options" , this.options,
        "comment" , keepComments ? this.comment : undefined
    ]);
};

/**
 * Resolves this field's type references.
 * @returns {Field} `this`
 * @throws {Error} If any reference cannot be resolved
 */
Field.prototype.resolve = function resolve() {

    if (this.resolved)
        return this;

    if ((this.typeDefault = types.defaults[this.type]) === undefined) { // if not a basic type, resolve it
        this.resolvedType = (this.declaringField ? this.declaringField.parent : this.parent).lookupTypeOrEnum(this.type);
        if (this.resolvedType instanceof Type)
            this.typeDefault = null;
        else // instanceof Enum
            this.typeDefault = this.resolvedType.values[Object.keys(this.resolvedType.values)[0]]; // first defined
    } else if (this.options && this.options.proto3_optional) {
        // proto3 scalar value marked optional; should default to null
        this.typeDefault = null;
    }

    // use explicitly set default value if present
    if (this.options && this.options["default"] != null) {
        this.typeDefault = this.options["default"];
        if (this.resolvedType instanceof Enum && typeof this.typeDefault === "string")
            this.typeDefault = this.resolvedType.values[this.typeDefault];
    }

    // remove unnecessary options
    if (this.options) {
        if (this.options.packed === true || this.options.packed !== undefined && this.resolvedType && !(this.resolvedType instanceof Enum))
            delete this.options.packed;
        if (!Object.keys(this.options).length)
            this.options = undefined;
    }

    // convert to internal data type if necesssary
    if (this.long) {
        this.typeDefault = util.Long.fromNumber(this.typeDefault, this.type.charAt(0) === "u");

        /* istanbul ignore else */
        if (Object.freeze)
            Object.freeze(this.typeDefault); // long instances are meant to be immutable anyway (i.e. use small int cache that even requires it)

    } else if (this.bytes && typeof this.typeDefault === "string") {
        var buf;
        if (util.base64.test(this.typeDefault))
            util.base64.decode(this.typeDefault, buf = util.newBuffer(util.base64.length(this.typeDefault)), 0);
        else
            util.utf8.write(this.typeDefault, buf = util.newBuffer(util.utf8.length(this.typeDefault)), 0);
        this.typeDefault = buf;
    }

    // take special care of maps and repeated fields
    if (this.map)
        this.defaultValue = util.emptyObject;
    else if (this.repeated)
        this.defaultValue = util.emptyArray;
    else
        this.defaultValue = this.typeDefault;

    // ensure proper value on prototype
    if (this.parent instanceof Type)
        this.parent.ctor.prototype[this.name] = this.defaultValue;

    return ReflectionObject.prototype.resolve.call(this);
};

/**
 * Decorator function as returned by {@link Field.d} and {@link MapField.d} (TypeScript).
 * @typedef FieldDecorator
 * @type {function}
 * @param {Object} prototype Target prototype
 * @param {string} fieldName Field name
 * @returns {undefined}
 */

/**
 * Field decorator (TypeScript).
 * @name Field.d
 * @function
 * @param {number} fieldId Field id
 * @param {"double"|"float"|"int32"|"uint32"|"sint32"|"fixed32"|"sfixed32"|"int64"|"uint64"|"sint64"|"fixed64"|"sfixed64"|"string"|"bool"|"bytes"|Object} fieldType Field type
 * @param {"optional"|"required"|"repeated"} [fieldRule="optional"] Field rule
 * @param {T} [defaultValue] Default value
 * @returns {FieldDecorator} Decorator function
 * @template T extends number | number[] | Long | Long[] | string | string[] | boolean | boolean[] | Uint8Array | Uint8Array[] | Buffer | Buffer[]
 */
Field.d = function decorateField(fieldId, fieldType, fieldRule, defaultValue) {

    // submessage: decorate the submessage and use its name as the type
    if (typeof fieldType === "function")
        fieldType = util.decorateType(fieldType).name;

    // enum reference: create a reflected copy of the enum and keep reuseing it
    else if (fieldType && typeof fieldType === "object")
        fieldType = util.decorateEnum(fieldType).name;

    return function fieldDecorator(prototype, fieldName) {
        util.decorateType(prototype.constructor)
            .add(new Field(fieldName, fieldId, fieldType, fieldRule, { "default": defaultValue }));
    };
};

/**
 * Field decorator (TypeScript).
 * @name Field.d
 * @function
 * @param {number} fieldId Field id
 * @param {Constructor<T>|string} fieldType Field type
 * @param {"optional"|"required"|"repeated"} [fieldRule="optional"] Field rule
 * @returns {FieldDecorator} Decorator function
 * @template T extends Message<T>
 * @variation 2
 */
// like Field.d but without a default value

// Sets up cyclic dependencies (called in index-light)
Field._configure = function configure(Type_) {
    Type = Type_;
};


/***/ }),

/***/ "./node_modules/protobufjs/src/index-light.js":
/*!****************************************************!*\
  !*** ./node_modules/protobufjs/src/index-light.js ***!
  \****************************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


var protobuf = module.exports = __webpack_require__(/*! ./index-minimal */ "./node_modules/protobufjs/src/index-minimal.js");

protobuf.build = "light";

/**
 * A node-style callback as used by {@link load} and {@link Root#load}.
 * @typedef LoadCallback
 * @type {function}
 * @param {Error|null} error Error, if any, otherwise `null`
 * @param {Root} [root] Root, if there hasn't been an error
 * @returns {undefined}
 */

/**
 * Loads one or multiple .proto or preprocessed .json files into a common root namespace and calls the callback.
 * @param {string|string[]} filename One or multiple files to load
 * @param {Root} root Root namespace, defaults to create a new one if omitted.
 * @param {LoadCallback} callback Callback function
 * @returns {undefined}
 * @see {@link Root#load}
 */
function load(filename, root, callback) {
    if (typeof root === "function") {
        callback = root;
        root = new protobuf.Root();
    } else if (!root)
        root = new protobuf.Root();
    return root.load(filename, callback);
}

/**
 * Loads one or multiple .proto or preprocessed .json files into a common root namespace and calls the callback.
 * @name load
 * @function
 * @param {string|string[]} filename One or multiple files to load
 * @param {LoadCallback} callback Callback function
 * @returns {undefined}
 * @see {@link Root#load}
 * @variation 2
 */
// function load(filename:string, callback:LoadCallback):undefined

/**
 * Loads one or multiple .proto or preprocessed .json files into a common root namespace and returns a promise.
 * @name load
 * @function
 * @param {string|string[]} filename One or multiple files to load
 * @param {Root} [root] Root namespace, defaults to create a new one if omitted.
 * @returns {Promise<Root>} Promise
 * @see {@link Root#load}
 * @variation 3
 */
// function load(filename:string, [root:Root]):Promise<Root>

protobuf.load = load;

/**
 * Synchronously loads one or multiple .proto or preprocessed .json files into a common root namespace (node only).
 * @param {string|string[]} filename One or multiple files to load
 * @param {Root} [root] Root namespace, defaults to create a new one if omitted.
 * @returns {Root} Root namespace
 * @throws {Error} If synchronous fetching is not supported (i.e. in browsers) or if a file's syntax is invalid
 * @see {@link Root#loadSync}
 */
function loadSync(filename, root) {
    if (!root)
        root = new protobuf.Root();
    return root.loadSync(filename);
}

protobuf.loadSync = loadSync;

// Serialization
protobuf.encoder          = __webpack_require__(/*! ./encoder */ "./node_modules/protobufjs/src/encoder.js");
protobuf.decoder          = __webpack_require__(/*! ./decoder */ "./node_modules/protobufjs/src/decoder.js");
protobuf.verifier         = __webpack_require__(/*! ./verifier */ "./node_modules/protobufjs/src/verifier.js");
protobuf.converter        = __webpack_require__(/*! ./converter */ "./node_modules/protobufjs/src/converter.js");

// Reflection
protobuf.ReflectionObject = __webpack_require__(/*! ./object */ "./node_modules/protobufjs/src/object.js");
protobuf.Namespace        = __webpack_require__(/*! ./namespace */ "./node_modules/protobufjs/src/namespace.js");
protobuf.Root             = __webpack_require__(/*! ./root */ "./node_modules/protobufjs/src/root.js");
protobuf.Enum             = __webpack_require__(/*! ./enum */ "./node_modules/protobufjs/src/enum.js");
protobuf.Type             = __webpack_require__(/*! ./type */ "./node_modules/protobufjs/src/type.js");
protobuf.Field            = __webpack_require__(/*! ./field */ "./node_modules/protobufjs/src/field.js");
protobuf.OneOf            = __webpack_require__(/*! ./oneof */ "./node_modules/protobufjs/src/oneof.js");
protobuf.MapField         = __webpack_require__(/*! ./mapfield */ "./node_modules/protobufjs/src/mapfield.js");
protobuf.Service          = __webpack_require__(/*! ./service */ "./node_modules/protobufjs/src/service.js");
protobuf.Method           = __webpack_require__(/*! ./method */ "./node_modules/protobufjs/src/method.js");

// Runtime
protobuf.Message          = __webpack_require__(/*! ./message */ "./node_modules/protobufjs/src/message.js");
protobuf.wrappers         = __webpack_require__(/*! ./wrappers */ "./node_modules/protobufjs/src/wrappers.js");

// Utility
protobuf.types            = __webpack_require__(/*! ./types */ "./node_modules/protobufjs/src/types.js");
protobuf.util             = __webpack_require__(/*! ./util */ "./node_modules/protobufjs/src/util.js");

// Set up possibly cyclic reflection dependencies
protobuf.ReflectionObject._configure(protobuf.Root);
protobuf.Namespace._configure(protobuf.Type, protobuf.Service, protobuf.Enum);
protobuf.Root._configure(protobuf.Type);
protobuf.Field._configure(protobuf.Type);


/***/ }),

/***/ "./node_modules/protobufjs/src/index-minimal.js":
/*!******************************************************!*\
  !*** ./node_modules/protobufjs/src/index-minimal.js ***!
  \******************************************************/
/***/ ((__unused_webpack_module, exports, __webpack_require__) => {


var protobuf = exports;

/**
 * Build type, one of `"full"`, `"light"` or `"minimal"`.
 * @name build
 * @type {string}
 * @const
 */
protobuf.build = "minimal";

// Serialization
protobuf.Writer       = __webpack_require__(/*! ./writer */ "./node_modules/protobufjs/src/writer.js");
protobuf.BufferWriter = __webpack_require__(/*! ./writer_buffer */ "./node_modules/protobufjs/src/writer_buffer.js");
protobuf.Reader       = __webpack_require__(/*! ./reader */ "./node_modules/protobufjs/src/reader.js");
protobuf.BufferReader = __webpack_require__(/*! ./reader_buffer */ "./node_modules/protobufjs/src/reader_buffer.js");

// Utility
protobuf.util         = __webpack_require__(/*! ./util/minimal */ "./node_modules/protobufjs/src/util/minimal.js");
protobuf.rpc          = __webpack_require__(/*! ./rpc */ "./node_modules/protobufjs/src/rpc.js");
protobuf.roots        = __webpack_require__(/*! ./roots */ "./node_modules/protobufjs/src/roots.js");
protobuf.configure    = configure;

/* istanbul ignore next */
/**
 * Reconfigures the library according to the environment.
 * @returns {undefined}
 */
function configure() {
    protobuf.util._configure();
    protobuf.Writer._configure(protobuf.BufferWriter);
    protobuf.Reader._configure(protobuf.BufferReader);
}

// Set up buffer utility according to the environment
configure();


/***/ }),

/***/ "./node_modules/protobufjs/src/index.js":
/*!**********************************************!*\
  !*** ./node_modules/protobufjs/src/index.js ***!
  \**********************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


var protobuf = module.exports = __webpack_require__(/*! ./index-light */ "./node_modules/protobufjs/src/index-light.js");

protobuf.build = "full";

// Parser
protobuf.tokenize         = __webpack_require__(/*! ./tokenize */ "./node_modules/protobufjs/src/tokenize.js");
protobuf.parse            = __webpack_require__(/*! ./parse */ "./node_modules/protobufjs/src/parse.js");
protobuf.common           = __webpack_require__(/*! ./common */ "./node_modules/protobufjs/src/common.js");

// Configure parser
protobuf.Root._configure(protobuf.Type, protobuf.parse, protobuf.common);


/***/ }),

/***/ "./node_modules/protobufjs/src/mapfield.js":
/*!*************************************************!*\
  !*** ./node_modules/protobufjs/src/mapfield.js ***!
  \*************************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = MapField;

// extends Field
var Field = __webpack_require__(/*! ./field */ "./node_modules/protobufjs/src/field.js");
((MapField.prototype = Object.create(Field.prototype)).constructor = MapField).className = "MapField";

var types   = __webpack_require__(/*! ./types */ "./node_modules/protobufjs/src/types.js"),
    util    = __webpack_require__(/*! ./util */ "./node_modules/protobufjs/src/util.js");

/**
 * Constructs a new map field instance.
 * @classdesc Reflected map field.
 * @extends FieldBase
 * @constructor
 * @param {string} name Unique name within its namespace
 * @param {number} id Unique id within its namespace
 * @param {string} keyType Key type
 * @param {string} type Value type
 * @param {Object.<string,*>} [options] Declared options
 * @param {string} [comment] Comment associated with this field
 */
function MapField(name, id, keyType, type, options, comment) {
    Field.call(this, name, id, type, undefined, undefined, options, comment);

    /* istanbul ignore if */
    if (!util.isString(keyType))
        throw TypeError("keyType must be a string");

    /**
     * Key type.
     * @type {string}
     */
    this.keyType = keyType; // toJSON, marker

    /**
     * Resolved key type if not a basic type.
     * @type {ReflectionObject|null}
     */
    this.resolvedKeyType = null;

    // Overrides Field#map
    this.map = true;
}

/**
 * Map field descriptor.
 * @interface IMapField
 * @extends {IField}
 * @property {string} keyType Key type
 */

/**
 * Extension map field descriptor.
 * @interface IExtensionMapField
 * @extends IMapField
 * @property {string} extend Extended type
 */

/**
 * Constructs a map field from a map field descriptor.
 * @param {string} name Field name
 * @param {IMapField} json Map field descriptor
 * @returns {MapField} Created map field
 * @throws {TypeError} If arguments are invalid
 */
MapField.fromJSON = function fromJSON(name, json) {
    return new MapField(name, json.id, json.keyType, json.type, json.options, json.comment);
};

/**
 * Converts this map field to a map field descriptor.
 * @param {IToJSONOptions} [toJSONOptions] JSON conversion options
 * @returns {IMapField} Map field descriptor
 */
MapField.prototype.toJSON = function toJSON(toJSONOptions) {
    var keepComments = toJSONOptions ? Boolean(toJSONOptions.keepComments) : false;
    return util.toObject([
        "keyType" , this.keyType,
        "type"    , this.type,
        "id"      , this.id,
        "extend"  , this.extend,
        "options" , this.options,
        "comment" , keepComments ? this.comment : undefined
    ]);
};

/**
 * @override
 */
MapField.prototype.resolve = function resolve() {
    if (this.resolved)
        return this;

    // Besides a value type, map fields have a key type that may be "any scalar type except for floating point types and bytes"
    if (types.mapKey[this.keyType] === undefined)
        throw Error("invalid key type: " + this.keyType);

    return Field.prototype.resolve.call(this);
};

/**
 * Map field decorator (TypeScript).
 * @name MapField.d
 * @function
 * @param {number} fieldId Field id
 * @param {"int32"|"uint32"|"sint32"|"fixed32"|"sfixed32"|"int64"|"uint64"|"sint64"|"fixed64"|"sfixed64"|"bool"|"string"} fieldKeyType Field key type
 * @param {"double"|"float"|"int32"|"uint32"|"sint32"|"fixed32"|"sfixed32"|"int64"|"uint64"|"sint64"|"fixed64"|"sfixed64"|"bool"|"string"|"bytes"|Object|Constructor<{}>} fieldValueType Field value type
 * @returns {FieldDecorator} Decorator function
 * @template T extends { [key: string]: number | Long | string | boolean | Uint8Array | Buffer | number[] | Message<{}> }
 */
MapField.d = function decorateMapField(fieldId, fieldKeyType, fieldValueType) {

    // submessage value: decorate the submessage and use its name as the type
    if (typeof fieldValueType === "function")
        fieldValueType = util.decorateType(fieldValueType).name;

    // enum reference value: create a reflected copy of the enum and keep reuseing it
    else if (fieldValueType && typeof fieldValueType === "object")
        fieldValueType = util.decorateEnum(fieldValueType).name;

    return function mapFieldDecorator(prototype, fieldName) {
        util.decorateType(prototype.constructor)
            .add(new MapField(fieldName, fieldId, fieldKeyType, fieldValueType));
    };
};


/***/ }),

/***/ "./node_modules/protobufjs/src/message.js":
/*!************************************************!*\
  !*** ./node_modules/protobufjs/src/message.js ***!
  \************************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = Message;

var util = __webpack_require__(/*! ./util/minimal */ "./node_modules/protobufjs/src/util/minimal.js");

/**
 * Constructs a new message instance.
 * @classdesc Abstract runtime message.
 * @constructor
 * @param {Properties<T>} [properties] Properties to set
 * @template T extends object = object
 */
function Message(properties) {
    // not used internally
    if (properties)
        for (var keys = Object.keys(properties), i = 0; i < keys.length; ++i)
            this[keys[i]] = properties[keys[i]];
}

/**
 * Reference to the reflected type.
 * @name Message.$type
 * @type {Type}
 * @readonly
 */

/**
 * Reference to the reflected type.
 * @name Message#$type
 * @type {Type}
 * @readonly
 */

/*eslint-disable valid-jsdoc*/

/**
 * Creates a new message of this type using the specified properties.
 * @param {Object.<string,*>} [properties] Properties to set
 * @returns {Message<T>} Message instance
 * @template T extends Message<T>
 * @this Constructor<T>
 */
Message.create = function create(properties) {
    return this.$type.create(properties);
};

/**
 * Encodes a message of this type.
 * @param {T|Object.<string,*>} message Message to encode
 * @param {Writer} [writer] Writer to use
 * @returns {Writer} Writer
 * @template T extends Message<T>
 * @this Constructor<T>
 */
Message.encode = function encode(message, writer) {
    return this.$type.encode(message, writer);
};

/**
 * Encodes a message of this type preceeded by its length as a varint.
 * @param {T|Object.<string,*>} message Message to encode
 * @param {Writer} [writer] Writer to use
 * @returns {Writer} Writer
 * @template T extends Message<T>
 * @this Constructor<T>
 */
Message.encodeDelimited = function encodeDelimited(message, writer) {
    return this.$type.encodeDelimited(message, writer);
};

/**
 * Decodes a message of this type.
 * @name Message.decode
 * @function
 * @param {Reader|Uint8Array} reader Reader or buffer to decode
 * @returns {T} Decoded message
 * @template T extends Message<T>
 * @this Constructor<T>
 */
Message.decode = function decode(reader) {
    return this.$type.decode(reader);
};

/**
 * Decodes a message of this type preceeded by its length as a varint.
 * @name Message.decodeDelimited
 * @function
 * @param {Reader|Uint8Array} reader Reader or buffer to decode
 * @returns {T} Decoded message
 * @template T extends Message<T>
 * @this Constructor<T>
 */
Message.decodeDelimited = function decodeDelimited(reader) {
    return this.$type.decodeDelimited(reader);
};

/**
 * Verifies a message of this type.
 * @name Message.verify
 * @function
 * @param {Object.<string,*>} message Plain object to verify
 * @returns {string|null} `null` if valid, otherwise the reason why it is not
 */
Message.verify = function verify(message) {
    return this.$type.verify(message);
};

/**
 * Creates a new message of this type from a plain object. Also converts values to their respective internal types.
 * @param {Object.<string,*>} object Plain object
 * @returns {T} Message instance
 * @template T extends Message<T>
 * @this Constructor<T>
 */
Message.fromObject = function fromObject(object) {
    return this.$type.fromObject(object);
};

/**
 * Creates a plain object from a message of this type. Also converts values to other types if specified.
 * @param {T} message Message instance
 * @param {IConversionOptions} [options] Conversion options
 * @returns {Object.<string,*>} Plain object
 * @template T extends Message<T>
 * @this Constructor<T>
 */
Message.toObject = function toObject(message, options) {
    return this.$type.toObject(message, options);
};

/**
 * Converts this message to JSON.
 * @returns {Object.<string,*>} JSON object
 */
Message.prototype.toJSON = function toJSON() {
    return this.$type.toObject(this, util.toJSONOptions);
};

/*eslint-enable valid-jsdoc*/

/***/ }),

/***/ "./node_modules/protobufjs/src/method.js":
/*!***********************************************!*\
  !*** ./node_modules/protobufjs/src/method.js ***!
  \***********************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = Method;

// extends ReflectionObject
var ReflectionObject = __webpack_require__(/*! ./object */ "./node_modules/protobufjs/src/object.js");
((Method.prototype = Object.create(ReflectionObject.prototype)).constructor = Method).className = "Method";

var util = __webpack_require__(/*! ./util */ "./node_modules/protobufjs/src/util.js");

/**
 * Constructs a new service method instance.
 * @classdesc Reflected service method.
 * @extends ReflectionObject
 * @constructor
 * @param {string} name Method name
 * @param {string|undefined} type Method type, usually `"rpc"`
 * @param {string} requestType Request message type
 * @param {string} responseType Response message type
 * @param {boolean|Object.<string,*>} [requestStream] Whether the request is streamed
 * @param {boolean|Object.<string,*>} [responseStream] Whether the response is streamed
 * @param {Object.<string,*>} [options] Declared options
 * @param {string} [comment] The comment for this method
 * @param {Object.<string,*>} [parsedOptions] Declared options, properly parsed into an object
 */
function Method(name, type, requestType, responseType, requestStream, responseStream, options, comment, parsedOptions) {

    /* istanbul ignore next */
    if (util.isObject(requestStream)) {
        options = requestStream;
        requestStream = responseStream = undefined;
    } else if (util.isObject(responseStream)) {
        options = responseStream;
        responseStream = undefined;
    }

    /* istanbul ignore if */
    if (!(type === undefined || util.isString(type)))
        throw TypeError("type must be a string");

    /* istanbul ignore if */
    if (!util.isString(requestType))
        throw TypeError("requestType must be a string");

    /* istanbul ignore if */
    if (!util.isString(responseType))
        throw TypeError("responseType must be a string");

    ReflectionObject.call(this, name, options);

    /**
     * Method type.
     * @type {string}
     */
    this.type = type || "rpc"; // toJSON

    /**
     * Request type.
     * @type {string}
     */
    this.requestType = requestType; // toJSON, marker

    /**
     * Whether requests are streamed or not.
     * @type {boolean|undefined}
     */
    this.requestStream = requestStream ? true : undefined; // toJSON

    /**
     * Response type.
     * @type {string}
     */
    this.responseType = responseType; // toJSON

    /**
     * Whether responses are streamed or not.
     * @type {boolean|undefined}
     */
    this.responseStream = responseStream ? true : undefined; // toJSON

    /**
     * Resolved request type.
     * @type {Type|null}
     */
    this.resolvedRequestType = null;

    /**
     * Resolved response type.
     * @type {Type|null}
     */
    this.resolvedResponseType = null;

    /**
     * Comment for this method
     * @type {string|null}
     */
    this.comment = comment;

    /**
     * Options properly parsed into an object
     */
    this.parsedOptions = parsedOptions;
}

/**
 * Method descriptor.
 * @interface IMethod
 * @property {string} [type="rpc"] Method type
 * @property {string} requestType Request type
 * @property {string} responseType Response type
 * @property {boolean} [requestStream=false] Whether requests are streamed
 * @property {boolean} [responseStream=false] Whether responses are streamed
 * @property {Object.<string,*>} [options] Method options
 * @property {string} comment Method comments
 * @property {Object.<string,*>} [parsedOptions] Method options properly parsed into an object
 */

/**
 * Constructs a method from a method descriptor.
 * @param {string} name Method name
 * @param {IMethod} json Method descriptor
 * @returns {Method} Created method
 * @throws {TypeError} If arguments are invalid
 */
Method.fromJSON = function fromJSON(name, json) {
    return new Method(name, json.type, json.requestType, json.responseType, json.requestStream, json.responseStream, json.options, json.comment, json.parsedOptions);
};

/**
 * Converts this method to a method descriptor.
 * @param {IToJSONOptions} [toJSONOptions] JSON conversion options
 * @returns {IMethod} Method descriptor
 */
Method.prototype.toJSON = function toJSON(toJSONOptions) {
    var keepComments = toJSONOptions ? Boolean(toJSONOptions.keepComments) : false;
    return util.toObject([
        "type"           , this.type !== "rpc" && /* istanbul ignore next */ this.type || undefined,
        "requestType"    , this.requestType,
        "requestStream"  , this.requestStream,
        "responseType"   , this.responseType,
        "responseStream" , this.responseStream,
        "options"        , this.options,
        "comment"        , keepComments ? this.comment : undefined,
        "parsedOptions"  , this.parsedOptions,
    ]);
};

/**
 * @override
 */
Method.prototype.resolve = function resolve() {

    /* istanbul ignore if */
    if (this.resolved)
        return this;

    this.resolvedRequestType = this.parent.lookupType(this.requestType);
    this.resolvedResponseType = this.parent.lookupType(this.responseType);

    return ReflectionObject.prototype.resolve.call(this);
};


/***/ }),

/***/ "./node_modules/protobufjs/src/namespace.js":
/*!**************************************************!*\
  !*** ./node_modules/protobufjs/src/namespace.js ***!
  \**************************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = Namespace;

// extends ReflectionObject
var ReflectionObject = __webpack_require__(/*! ./object */ "./node_modules/protobufjs/src/object.js");
((Namespace.prototype = Object.create(ReflectionObject.prototype)).constructor = Namespace).className = "Namespace";

var Field    = __webpack_require__(/*! ./field */ "./node_modules/protobufjs/src/field.js"),
    util     = __webpack_require__(/*! ./util */ "./node_modules/protobufjs/src/util.js"),
    OneOf    = __webpack_require__(/*! ./oneof */ "./node_modules/protobufjs/src/oneof.js");

var Type,    // cyclic
    Service,
    Enum;

/**
 * Constructs a new namespace instance.
 * @name Namespace
 * @classdesc Reflected namespace.
 * @extends NamespaceBase
 * @constructor
 * @param {string} name Namespace name
 * @param {Object.<string,*>} [options] Declared options
 */

/**
 * Constructs a namespace from JSON.
 * @memberof Namespace
 * @function
 * @param {string} name Namespace name
 * @param {Object.<string,*>} json JSON object
 * @returns {Namespace} Created namespace
 * @throws {TypeError} If arguments are invalid
 */
Namespace.fromJSON = function fromJSON(name, json) {
    return new Namespace(name, json.options).addJSON(json.nested);
};

/**
 * Converts an array of reflection objects to JSON.
 * @memberof Namespace
 * @param {ReflectionObject[]} array Object array
 * @param {IToJSONOptions} [toJSONOptions] JSON conversion options
 * @returns {Object.<string,*>|undefined} JSON object or `undefined` when array is empty
 */
function arrayToJSON(array, toJSONOptions) {
    if (!(array && array.length))
        return undefined;
    var obj = {};
    for (var i = 0; i < array.length; ++i)
        obj[array[i].name] = array[i].toJSON(toJSONOptions);
    return obj;
}

Namespace.arrayToJSON = arrayToJSON;

/**
 * Tests if the specified id is reserved.
 * @param {Array.<number[]|string>|undefined} reserved Array of reserved ranges and names
 * @param {number} id Id to test
 * @returns {boolean} `true` if reserved, otherwise `false`
 */
Namespace.isReservedId = function isReservedId(reserved, id) {
    if (reserved)
        for (var i = 0; i < reserved.length; ++i)
            if (typeof reserved[i] !== "string" && reserved[i][0] <= id && reserved[i][1] > id)
                return true;
    return false;
};

/**
 * Tests if the specified name is reserved.
 * @param {Array.<number[]|string>|undefined} reserved Array of reserved ranges and names
 * @param {string} name Name to test
 * @returns {boolean} `true` if reserved, otherwise `false`
 */
Namespace.isReservedName = function isReservedName(reserved, name) {
    if (reserved)
        for (var i = 0; i < reserved.length; ++i)
            if (reserved[i] === name)
                return true;
    return false;
};

/**
 * Not an actual constructor. Use {@link Namespace} instead.
 * @classdesc Base class of all reflection objects containing nested objects. This is not an actual class but here for the sake of having consistent type definitions.
 * @exports NamespaceBase
 * @extends ReflectionObject
 * @abstract
 * @constructor
 * @param {string} name Namespace name
 * @param {Object.<string,*>} [options] Declared options
 * @see {@link Namespace}
 */
function Namespace(name, options) {
    ReflectionObject.call(this, name, options);

    /**
     * Nested objects by name.
     * @type {Object.<string,ReflectionObject>|undefined}
     */
    this.nested = undefined; // toJSON

    /**
     * Cached nested objects as an array.
     * @type {ReflectionObject[]|null}
     * @private
     */
    this._nestedArray = null;
}

function clearCache(namespace) {
    namespace._nestedArray = null;
    return namespace;
}

/**
 * Nested objects of this namespace as an array for iteration.
 * @name NamespaceBase#nestedArray
 * @type {ReflectionObject[]}
 * @readonly
 */
Object.defineProperty(Namespace.prototype, "nestedArray", {
    get: function() {
        return this._nestedArray || (this._nestedArray = util.toArray(this.nested));
    }
});

/**
 * Namespace descriptor.
 * @interface INamespace
 * @property {Object.<string,*>} [options] Namespace options
 * @property {Object.<string,AnyNestedObject>} [nested] Nested object descriptors
 */

/**
 * Any extension field descriptor.
 * @typedef AnyExtensionField
 * @type {IExtensionField|IExtensionMapField}
 */

/**
 * Any nested object descriptor.
 * @typedef AnyNestedObject
 * @type {IEnum|IType|IService|AnyExtensionField|INamespace|IOneOf}
 */

/**
 * Converts this namespace to a namespace descriptor.
 * @param {IToJSONOptions} [toJSONOptions] JSON conversion options
 * @returns {INamespace} Namespace descriptor
 */
Namespace.prototype.toJSON = function toJSON(toJSONOptions) {
    return util.toObject([
        "options" , this.options,
        "nested"  , arrayToJSON(this.nestedArray, toJSONOptions)
    ]);
};

/**
 * Adds nested objects to this namespace from nested object descriptors.
 * @param {Object.<string,AnyNestedObject>} nestedJson Any nested object descriptors
 * @returns {Namespace} `this`
 */
Namespace.prototype.addJSON = function addJSON(nestedJson) {
    var ns = this;
    /* istanbul ignore else */
    if (nestedJson) {
        for (var names = Object.keys(nestedJson), i = 0, nested; i < names.length; ++i) {
            nested = nestedJson[names[i]];
            ns.add( // most to least likely
                ( nested.fields !== undefined
                ? Type.fromJSON
                : nested.values !== undefined
                ? Enum.fromJSON
                : nested.methods !== undefined
                ? Service.fromJSON
                : nested.id !== undefined
                ? Field.fromJSON
                : Namespace.fromJSON )(names[i], nested)
            );
        }
    }
    return this;
};

/**
 * Gets the nested object of the specified name.
 * @param {string} name Nested object name
 * @returns {ReflectionObject|null} The reflection object or `null` if it doesn't exist
 */
Namespace.prototype.get = function get(name) {
    return this.nested && this.nested[name]
        || null;
};

/**
 * Gets the values of the nested {@link Enum|enum} of the specified name.
 * This methods differs from {@link Namespace#get|get} in that it returns an enum's values directly and throws instead of returning `null`.
 * @param {string} name Nested enum name
 * @returns {Object.<string,number>} Enum values
 * @throws {Error} If there is no such enum
 */
Namespace.prototype.getEnum = function getEnum(name) {
    if (this.nested && this.nested[name] instanceof Enum)
        return this.nested[name].values;
    throw Error("no such enum: " + name);
};

/**
 * Adds a nested object to this namespace.
 * @param {ReflectionObject} object Nested object to add
 * @returns {Namespace} `this`
 * @throws {TypeError} If arguments are invalid
 * @throws {Error} If there is already a nested object with this name
 */
Namespace.prototype.add = function add(object) {

    if (!(object instanceof Field && object.extend !== undefined || object instanceof Type  || object instanceof OneOf || object instanceof Enum || object instanceof Service || object instanceof Namespace))
        throw TypeError("object must be a valid nested object");

    if (!this.nested)
        this.nested = {};
    else {
        var prev = this.get(object.name);
        if (prev) {
            if (prev instanceof Namespace && object instanceof Namespace && !(prev instanceof Type || prev instanceof Service)) {
                // replace plain namespace but keep existing nested elements and options
                var nested = prev.nestedArray;
                for (var i = 0; i < nested.length; ++i)
                    object.add(nested[i]);
                this.remove(prev);
                if (!this.nested)
                    this.nested = {};
                object.setOptions(prev.options, true);

            } else
                throw Error("duplicate name '" + object.name + "' in " + this);
        }
    }
    this.nested[object.name] = object;
    object.onAdd(this);
    return clearCache(this);
};

/**
 * Removes a nested object from this namespace.
 * @param {ReflectionObject} object Nested object to remove
 * @returns {Namespace} `this`
 * @throws {TypeError} If arguments are invalid
 * @throws {Error} If `object` is not a member of this namespace
 */
Namespace.prototype.remove = function remove(object) {

    if (!(object instanceof ReflectionObject))
        throw TypeError("object must be a ReflectionObject");
    if (object.parent !== this)
        throw Error(object + " is not a member of " + this);

    delete this.nested[object.name];
    if (!Object.keys(this.nested).length)
        this.nested = undefined;

    object.onRemove(this);
    return clearCache(this);
};

/**
 * Defines additial namespaces within this one if not yet existing.
 * @param {string|string[]} path Path to create
 * @param {*} [json] Nested types to create from JSON
 * @returns {Namespace} Pointer to the last namespace created or `this` if path is empty
 */
Namespace.prototype.define = function define(path, json) {

    if (util.isString(path))
        path = path.split(".");
    else if (!Array.isArray(path))
        throw TypeError("illegal path");
    if (path && path.length && path[0] === "")
        throw Error("path must be relative");

    var ptr = this;
    while (path.length > 0) {
        var part = path.shift();
        if (ptr.nested && ptr.nested[part]) {
            ptr = ptr.nested[part];
            if (!(ptr instanceof Namespace))
                throw Error("path conflicts with non-namespace objects");
        } else
            ptr.add(ptr = new Namespace(part));
    }
    if (json)
        ptr.addJSON(json);
    return ptr;
};

/**
 * Resolves this namespace's and all its nested objects' type references. Useful to validate a reflection tree, but comes at a cost.
 * @returns {Namespace} `this`
 */
Namespace.prototype.resolveAll = function resolveAll() {
    var nested = this.nestedArray, i = 0;
    while (i < nested.length)
        if (nested[i] instanceof Namespace)
            nested[i++].resolveAll();
        else
            nested[i++].resolve();
    return this.resolve();
};

/**
 * Recursively looks up the reflection object matching the specified path in the scope of this namespace.
 * @param {string|string[]} path Path to look up
 * @param {*|Array.<*>} filterTypes Filter types, any combination of the constructors of `protobuf.Type`, `protobuf.Enum`, `protobuf.Service` etc.
 * @param {boolean} [parentAlreadyChecked=false] If known, whether the parent has already been checked
 * @returns {ReflectionObject|null} Looked up object or `null` if none could be found
 */
Namespace.prototype.lookup = function lookup(path, filterTypes, parentAlreadyChecked) {

    /* istanbul ignore next */
    if (typeof filterTypes === "boolean") {
        parentAlreadyChecked = filterTypes;
        filterTypes = undefined;
    } else if (filterTypes && !Array.isArray(filterTypes))
        filterTypes = [ filterTypes ];

    if (util.isString(path) && path.length) {
        if (path === ".")
            return this.root;
        path = path.split(".");
    } else if (!path.length)
        return this;

    // Start at root if path is absolute
    if (path[0] === "")
        return this.root.lookup(path.slice(1), filterTypes);

    // Test if the first part matches any nested object, and if so, traverse if path contains more
    var found = this.get(path[0]);
    if (found) {
        if (path.length === 1) {
            if (!filterTypes || filterTypes.indexOf(found.constructor) > -1)
                return found;
        } else if (found instanceof Namespace && (found = found.lookup(path.slice(1), filterTypes, true)))
            return found;

    // Otherwise try each nested namespace
    } else
        for (var i = 0; i < this.nestedArray.length; ++i)
            if (this._nestedArray[i] instanceof Namespace && (found = this._nestedArray[i].lookup(path, filterTypes, true)))
                return found;

    // If there hasn't been a match, try again at the parent
    if (this.parent === null || parentAlreadyChecked)
        return null;
    return this.parent.lookup(path, filterTypes);
};

/**
 * Looks up the reflection object at the specified path, relative to this namespace.
 * @name NamespaceBase#lookup
 * @function
 * @param {string|string[]} path Path to look up
 * @param {boolean} [parentAlreadyChecked=false] Whether the parent has already been checked
 * @returns {ReflectionObject|null} Looked up object or `null` if none could be found
 * @variation 2
 */
// lookup(path: string, [parentAlreadyChecked: boolean])

/**
 * Looks up the {@link Type|type} at the specified path, relative to this namespace.
 * Besides its signature, this methods differs from {@link Namespace#lookup|lookup} in that it throws instead of returning `null`.
 * @param {string|string[]} path Path to look up
 * @returns {Type} Looked up type
 * @throws {Error} If `path` does not point to a type
 */
Namespace.prototype.lookupType = function lookupType(path) {
    var found = this.lookup(path, [ Type ]);
    if (!found)
        throw Error("no such type: " + path);
    return found;
};

/**
 * Looks up the values of the {@link Enum|enum} at the specified path, relative to this namespace.
 * Besides its signature, this methods differs from {@link Namespace#lookup|lookup} in that it throws instead of returning `null`.
 * @param {string|string[]} path Path to look up
 * @returns {Enum} Looked up enum
 * @throws {Error} If `path` does not point to an enum
 */
Namespace.prototype.lookupEnum = function lookupEnum(path) {
    var found = this.lookup(path, [ Enum ]);
    if (!found)
        throw Error("no such Enum '" + path + "' in " + this);
    return found;
};

/**
 * Looks up the {@link Type|type} or {@link Enum|enum} at the specified path, relative to this namespace.
 * Besides its signature, this methods differs from {@link Namespace#lookup|lookup} in that it throws instead of returning `null`.
 * @param {string|string[]} path Path to look up
 * @returns {Type} Looked up type or enum
 * @throws {Error} If `path` does not point to a type or enum
 */
Namespace.prototype.lookupTypeOrEnum = function lookupTypeOrEnum(path) {
    var found = this.lookup(path, [ Type, Enum ]);
    if (!found)
        throw Error("no such Type or Enum '" + path + "' in " + this);
    return found;
};

/**
 * Looks up the {@link Service|service} at the specified path, relative to this namespace.
 * Besides its signature, this methods differs from {@link Namespace#lookup|lookup} in that it throws instead of returning `null`.
 * @param {string|string[]} path Path to look up
 * @returns {Service} Looked up service
 * @throws {Error} If `path` does not point to a service
 */
Namespace.prototype.lookupService = function lookupService(path) {
    var found = this.lookup(path, [ Service ]);
    if (!found)
        throw Error("no such Service '" + path + "' in " + this);
    return found;
};

// Sets up cyclic dependencies (called in index-light)
Namespace._configure = function(Type_, Service_, Enum_) {
    Type    = Type_;
    Service = Service_;
    Enum    = Enum_;
};


/***/ }),

/***/ "./node_modules/protobufjs/src/object.js":
/*!***********************************************!*\
  !*** ./node_modules/protobufjs/src/object.js ***!
  \***********************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = ReflectionObject;

ReflectionObject.className = "ReflectionObject";

var util = __webpack_require__(/*! ./util */ "./node_modules/protobufjs/src/util.js");

var Root; // cyclic

/**
 * Constructs a new reflection object instance.
 * @classdesc Base class of all reflection objects.
 * @constructor
 * @param {string} name Object name
 * @param {Object.<string,*>} [options] Declared options
 * @abstract
 */
function ReflectionObject(name, options) {

    if (!util.isString(name))
        throw TypeError("name must be a string");

    if (options && !util.isObject(options))
        throw TypeError("options must be an object");

    /**
     * Options.
     * @type {Object.<string,*>|undefined}
     */
    this.options = options; // toJSON

    /**
     * Parsed Options.
     * @type {Array.<Object.<string,*>>|undefined}
     */
    this.parsedOptions = null;

    /**
     * Unique name within its namespace.
     * @type {string}
     */
    this.name = name;

    /**
     * Parent namespace.
     * @type {Namespace|null}
     */
    this.parent = null;

    /**
     * Whether already resolved or not.
     * @type {boolean}
     */
    this.resolved = false;

    /**
     * Comment text, if any.
     * @type {string|null}
     */
    this.comment = null;

    /**
     * Defining file name.
     * @type {string|null}
     */
    this.filename = null;
}

Object.defineProperties(ReflectionObject.prototype, {

    /**
     * Reference to the root namespace.
     * @name ReflectionObject#root
     * @type {Root}
     * @readonly
     */
    root: {
        get: function() {
            var ptr = this;
            while (ptr.parent !== null)
                ptr = ptr.parent;
            return ptr;
        }
    },

    /**
     * Full name including leading dot.
     * @name ReflectionObject#fullName
     * @type {string}
     * @readonly
     */
    fullName: {
        get: function() {
            var path = [ this.name ],
                ptr = this.parent;
            while (ptr) {
                path.unshift(ptr.name);
                ptr = ptr.parent;
            }
            return path.join(".");
        }
    }
});

/**
 * Converts this reflection object to its descriptor representation.
 * @returns {Object.<string,*>} Descriptor
 * @abstract
 */
ReflectionObject.prototype.toJSON = /* istanbul ignore next */ function toJSON() {
    throw Error(); // not implemented, shouldn't happen
};

/**
 * Called when this object is added to a parent.
 * @param {ReflectionObject} parent Parent added to
 * @returns {undefined}
 */
ReflectionObject.prototype.onAdd = function onAdd(parent) {
    if (this.parent && this.parent !== parent)
        this.parent.remove(this);
    this.parent = parent;
    this.resolved = false;
    var root = parent.root;
    if (root instanceof Root)
        root._handleAdd(this);
};

/**
 * Called when this object is removed from a parent.
 * @param {ReflectionObject} parent Parent removed from
 * @returns {undefined}
 */
ReflectionObject.prototype.onRemove = function onRemove(parent) {
    var root = parent.root;
    if (root instanceof Root)
        root._handleRemove(this);
    this.parent = null;
    this.resolved = false;
};

/**
 * Resolves this objects type references.
 * @returns {ReflectionObject} `this`
 */
ReflectionObject.prototype.resolve = function resolve() {
    if (this.resolved)
        return this;
    if (this.root instanceof Root)
        this.resolved = true; // only if part of a root
    return this;
};

/**
 * Gets an option value.
 * @param {string} name Option name
 * @returns {*} Option value or `undefined` if not set
 */
ReflectionObject.prototype.getOption = function getOption(name) {
    if (this.options)
        return this.options[name];
    return undefined;
};

/**
 * Sets an option.
 * @param {string} name Option name
 * @param {*} value Option value
 * @param {boolean} [ifNotSet] Sets the option only if it isn't currently set
 * @returns {ReflectionObject} `this`
 */
ReflectionObject.prototype.setOption = function setOption(name, value, ifNotSet) {
    if (!ifNotSet || !this.options || this.options[name] === undefined)
        (this.options || (this.options = {}))[name] = value;
    return this;
};

/**
 * Sets a parsed option.
 * @param {string} name parsed Option name
 * @param {*} value Option value
 * @param {string} propName dot '.' delimited full path of property within the option to set. if undefined\empty, will add a new option with that value
 * @returns {ReflectionObject} `this`
 */
ReflectionObject.prototype.setParsedOption = function setParsedOption(name, value, propName) {
    if (!this.parsedOptions) {
        this.parsedOptions = [];
    }
    var parsedOptions = this.parsedOptions;
    if (propName) {
        // If setting a sub property of an option then try to merge it
        // with an existing option
        var opt = parsedOptions.find(function (opt) {
            return Object.prototype.hasOwnProperty.call(opt, name);
        });
        if (opt) {
            // If we found an existing option - just merge the property value
            var newValue = opt[name];
            util.setProperty(newValue, propName, value);
        } else {
            // otherwise, create a new option, set it's property and add it to the list
            opt = {};
            opt[name] = util.setProperty({}, propName, value);
            parsedOptions.push(opt);
        }
    } else {
        // Always create a new option when setting the value of the option itself
        var newOpt = {};
        newOpt[name] = value;
        parsedOptions.push(newOpt);
    }
    return this;
};

/**
 * Sets multiple options.
 * @param {Object.<string,*>} options Options to set
 * @param {boolean} [ifNotSet] Sets an option only if it isn't currently set
 * @returns {ReflectionObject} `this`
 */
ReflectionObject.prototype.setOptions = function setOptions(options, ifNotSet) {
    if (options)
        for (var keys = Object.keys(options), i = 0; i < keys.length; ++i)
            this.setOption(keys[i], options[keys[i]], ifNotSet);
    return this;
};

/**
 * Converts this instance to its string representation.
 * @returns {string} Class name[, space, full name]
 */
ReflectionObject.prototype.toString = function toString() {
    var className = this.constructor.className,
        fullName  = this.fullName;
    if (fullName.length)
        return className + " " + fullName;
    return className;
};

// Sets up cyclic dependencies (called in index-light)
ReflectionObject._configure = function(Root_) {
    Root = Root_;
};


/***/ }),

/***/ "./node_modules/protobufjs/src/oneof.js":
/*!**********************************************!*\
  !*** ./node_modules/protobufjs/src/oneof.js ***!
  \**********************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = OneOf;

// extends ReflectionObject
var ReflectionObject = __webpack_require__(/*! ./object */ "./node_modules/protobufjs/src/object.js");
((OneOf.prototype = Object.create(ReflectionObject.prototype)).constructor = OneOf).className = "OneOf";

var Field = __webpack_require__(/*! ./field */ "./node_modules/protobufjs/src/field.js"),
    util  = __webpack_require__(/*! ./util */ "./node_modules/protobufjs/src/util.js");

/**
 * Constructs a new oneof instance.
 * @classdesc Reflected oneof.
 * @extends ReflectionObject
 * @constructor
 * @param {string} name Oneof name
 * @param {string[]|Object.<string,*>} [fieldNames] Field names
 * @param {Object.<string,*>} [options] Declared options
 * @param {string} [comment] Comment associated with this field
 */
function OneOf(name, fieldNames, options, comment) {
    if (!Array.isArray(fieldNames)) {
        options = fieldNames;
        fieldNames = undefined;
    }
    ReflectionObject.call(this, name, options);

    /* istanbul ignore if */
    if (!(fieldNames === undefined || Array.isArray(fieldNames)))
        throw TypeError("fieldNames must be an Array");

    /**
     * Field names that belong to this oneof.
     * @type {string[]}
     */
    this.oneof = fieldNames || []; // toJSON, marker

    /**
     * Fields that belong to this oneof as an array for iteration.
     * @type {Field[]}
     * @readonly
     */
    this.fieldsArray = []; // declared readonly for conformance, possibly not yet added to parent

    /**
     * Comment for this field.
     * @type {string|null}
     */
    this.comment = comment;
}

/**
 * Oneof descriptor.
 * @interface IOneOf
 * @property {Array.<string>} oneof Oneof field names
 * @property {Object.<string,*>} [options] Oneof options
 */

/**
 * Constructs a oneof from a oneof descriptor.
 * @param {string} name Oneof name
 * @param {IOneOf} json Oneof descriptor
 * @returns {OneOf} Created oneof
 * @throws {TypeError} If arguments are invalid
 */
OneOf.fromJSON = function fromJSON(name, json) {
    return new OneOf(name, json.oneof, json.options, json.comment);
};

/**
 * Converts this oneof to a oneof descriptor.
 * @param {IToJSONOptions} [toJSONOptions] JSON conversion options
 * @returns {IOneOf} Oneof descriptor
 */
OneOf.prototype.toJSON = function toJSON(toJSONOptions) {
    var keepComments = toJSONOptions ? Boolean(toJSONOptions.keepComments) : false;
    return util.toObject([
        "options" , this.options,
        "oneof"   , this.oneof,
        "comment" , keepComments ? this.comment : undefined
    ]);
};

/**
 * Adds the fields of the specified oneof to the parent if not already done so.
 * @param {OneOf} oneof The oneof
 * @returns {undefined}
 * @inner
 * @ignore
 */
function addFieldsToParent(oneof) {
    if (oneof.parent)
        for (var i = 0; i < oneof.fieldsArray.length; ++i)
            if (!oneof.fieldsArray[i].parent)
                oneof.parent.add(oneof.fieldsArray[i]);
}

/**
 * Adds a field to this oneof and removes it from its current parent, if any.
 * @param {Field} field Field to add
 * @returns {OneOf} `this`
 */
OneOf.prototype.add = function add(field) {

    /* istanbul ignore if */
    if (!(field instanceof Field))
        throw TypeError("field must be a Field");

    if (field.parent && field.parent !== this.parent)
        field.parent.remove(field);
    this.oneof.push(field.name);
    this.fieldsArray.push(field);
    field.partOf = this; // field.parent remains null
    addFieldsToParent(this);
    return this;
};

/**
 * Removes a field from this oneof and puts it back to the oneof's parent.
 * @param {Field} field Field to remove
 * @returns {OneOf} `this`
 */
OneOf.prototype.remove = function remove(field) {

    /* istanbul ignore if */
    if (!(field instanceof Field))
        throw TypeError("field must be a Field");

    var index = this.fieldsArray.indexOf(field);

    /* istanbul ignore if */
    if (index < 0)
        throw Error(field + " is not a member of " + this);

    this.fieldsArray.splice(index, 1);
    index = this.oneof.indexOf(field.name);

    /* istanbul ignore else */
    if (index > -1) // theoretical
        this.oneof.splice(index, 1);

    field.partOf = null;
    return this;
};

/**
 * @override
 */
OneOf.prototype.onAdd = function onAdd(parent) {
    ReflectionObject.prototype.onAdd.call(this, parent);
    var self = this;
    // Collect present fields
    for (var i = 0; i < this.oneof.length; ++i) {
        var field = parent.get(this.oneof[i]);
        if (field && !field.partOf) {
            field.partOf = self;
            self.fieldsArray.push(field);
        }
    }
    // Add not yet present fields
    addFieldsToParent(this);
};

/**
 * @override
 */
OneOf.prototype.onRemove = function onRemove(parent) {
    for (var i = 0, field; i < this.fieldsArray.length; ++i)
        if ((field = this.fieldsArray[i]).parent)
            field.parent.remove(field);
    ReflectionObject.prototype.onRemove.call(this, parent);
};

/**
 * Decorator function as returned by {@link OneOf.d} (TypeScript).
 * @typedef OneOfDecorator
 * @type {function}
 * @param {Object} prototype Target prototype
 * @param {string} oneofName OneOf name
 * @returns {undefined}
 */

/**
 * OneOf decorator (TypeScript).
 * @function
 * @param {...string} fieldNames Field names
 * @returns {OneOfDecorator} Decorator function
 * @template T extends string
 */
OneOf.d = function decorateOneOf() {
    var fieldNames = new Array(arguments.length),
        index = 0;
    while (index < arguments.length)
        fieldNames[index] = arguments[index++];
    return function oneOfDecorator(prototype, oneofName) {
        util.decorateType(prototype.constructor)
            .add(new OneOf(oneofName, fieldNames));
        Object.defineProperty(prototype, oneofName, {
            get: util.oneOfGetter(fieldNames),
            set: util.oneOfSetter(fieldNames)
        });
    };
};


/***/ }),

/***/ "./node_modules/protobufjs/src/parse.js":
/*!**********************************************!*\
  !*** ./node_modules/protobufjs/src/parse.js ***!
  \**********************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = parse;

parse.filename = null;
parse.defaults = { keepCase: false };

var tokenize  = __webpack_require__(/*! ./tokenize */ "./node_modules/protobufjs/src/tokenize.js"),
    Root      = __webpack_require__(/*! ./root */ "./node_modules/protobufjs/src/root.js"),
    Type      = __webpack_require__(/*! ./type */ "./node_modules/protobufjs/src/type.js"),
    Field     = __webpack_require__(/*! ./field */ "./node_modules/protobufjs/src/field.js"),
    MapField  = __webpack_require__(/*! ./mapfield */ "./node_modules/protobufjs/src/mapfield.js"),
    OneOf     = __webpack_require__(/*! ./oneof */ "./node_modules/protobufjs/src/oneof.js"),
    Enum      = __webpack_require__(/*! ./enum */ "./node_modules/protobufjs/src/enum.js"),
    Service   = __webpack_require__(/*! ./service */ "./node_modules/protobufjs/src/service.js"),
    Method    = __webpack_require__(/*! ./method */ "./node_modules/protobufjs/src/method.js"),
    types     = __webpack_require__(/*! ./types */ "./node_modules/protobufjs/src/types.js"),
    util      = __webpack_require__(/*! ./util */ "./node_modules/protobufjs/src/util.js");

var base10Re    = /^[1-9][0-9]*$/,
    base10NegRe = /^-?[1-9][0-9]*$/,
    base16Re    = /^0[x][0-9a-fA-F]+$/,
    base16NegRe = /^-?0[x][0-9a-fA-F]+$/,
    base8Re     = /^0[0-7]+$/,
    base8NegRe  = /^-?0[0-7]+$/,
    numberRe    = /^(?![eE])[0-9]*(?:\.[0-9]*)?(?:[eE][+-]?[0-9]+)?$/,
    nameRe      = /^[a-zA-Z_][a-zA-Z_0-9]*$/,
    typeRefRe   = /^(?:\.?[a-zA-Z_][a-zA-Z_0-9]*)(?:\.[a-zA-Z_][a-zA-Z_0-9]*)*$/,
    fqTypeRefRe = /^(?:\.[a-zA-Z_][a-zA-Z_0-9]*)+$/;

/**
 * Result object returned from {@link parse}.
 * @interface IParserResult
 * @property {string|undefined} package Package name, if declared
 * @property {string[]|undefined} imports Imports, if any
 * @property {string[]|undefined} weakImports Weak imports, if any
 * @property {string|undefined} syntax Syntax, if specified (either `"proto2"` or `"proto3"`)
 * @property {Root} root Populated root instance
 */

/**
 * Options modifying the behavior of {@link parse}.
 * @interface IParseOptions
 * @property {boolean} [keepCase=false] Keeps field casing instead of converting to camel case
 * @property {boolean} [alternateCommentMode=false] Recognize double-slash comments in addition to doc-block comments.
 * @property {boolean} [preferTrailingComment=false] Use trailing comment when both leading comment and trailing comment exist.
 */

/**
 * Options modifying the behavior of JSON serialization.
 * @interface IToJSONOptions
 * @property {boolean} [keepComments=false] Serializes comments.
 */

/**
 * Parses the given .proto source and returns an object with the parsed contents.
 * @param {string} source Source contents
 * @param {Root} root Root to populate
 * @param {IParseOptions} [options] Parse options. Defaults to {@link parse.defaults} when omitted.
 * @returns {IParserResult} Parser result
 * @property {string} filename=null Currently processing file name for error reporting, if known
 * @property {IParseOptions} defaults Default {@link IParseOptions}
 */
function parse(source, root, options) {
    /* eslint-disable callback-return */
    if (!(root instanceof Root)) {
        options = root;
        root = new Root();
    }
    if (!options)
        options = parse.defaults;

    var preferTrailingComment = options.preferTrailingComment || false;
    var tn = tokenize(source, options.alternateCommentMode || false),
        next = tn.next,
        push = tn.push,
        peek = tn.peek,
        skip = tn.skip,
        cmnt = tn.cmnt;

    var head = true,
        pkg,
        imports,
        weakImports,
        syntax,
        isProto3 = false;

    var ptr = root;

    var applyCase = options.keepCase ? function(name) { return name; } : util.camelCase;

    /* istanbul ignore next */
    function illegal(token, name, insideTryCatch) {
        var filename = parse.filename;
        if (!insideTryCatch)
            parse.filename = null;
        return Error("illegal " + (name || "token") + " '" + token + "' (" + (filename ? filename + ", " : "") + "line " + tn.line + ")");
    }

    function readString() {
        var values = [],
            token;
        do {
            /* istanbul ignore if */
            if ((token = next()) !== "\"" && token !== "'")
                throw illegal(token);

            values.push(next());
            skip(token);
            token = peek();
        } while (token === "\"" || token === "'");
        return values.join("");
    }

    function readValue(acceptTypeRef) {
        var token = next();
        switch (token) {
            case "'":
            case "\"":
                push(token);
                return readString();
            case "true": case "TRUE":
                return true;
            case "false": case "FALSE":
                return false;
        }
        try {
            return parseNumber(token, /* insideTryCatch */ true);
        } catch (e) {

            /* istanbul ignore else */
            if (acceptTypeRef && typeRefRe.test(token))
                return token;

            /* istanbul ignore next */
            throw illegal(token, "value");
        }
    }

    function readRanges(target, acceptStrings) {
        var token, start;
        do {
            if (acceptStrings && ((token = peek()) === "\"" || token === "'"))
                target.push(readString());
            else
                target.push([ start = parseId(next()), skip("to", true) ? parseId(next()) : start ]);
        } while (skip(",", true));
        var dummy = {options: undefined};
        dummy.setOption = function(name, value) {
          if (this.options === undefined) this.options = {};
          this.options[name] = value;
        };
        ifBlock(
            dummy,
            function parseRange_block(token) {
              /* istanbul ignore else */
              if (token === "option") {
                parseOption(dummy, token);  // skip
                skip(";");
              } else
                throw illegal(token);
            },
            function parseRange_line() {
              parseInlineOptions(dummy);  // skip
            });
    }

    function parseNumber(token, insideTryCatch) {
        var sign = 1;
        if (token.charAt(0) === "-") {
            sign = -1;
            token = token.substring(1);
        }
        switch (token) {
            case "inf": case "INF": case "Inf":
                return sign * Infinity;
            case "nan": case "NAN": case "Nan": case "NaN":
                return NaN;
            case "0":
                return 0;
        }
        if (base10Re.test(token))
            return sign * parseInt(token, 10);
        if (base16Re.test(token))
            return sign * parseInt(token, 16);
        if (base8Re.test(token))
            return sign * parseInt(token, 8);

        /* istanbul ignore else */
        if (numberRe.test(token))
            return sign * parseFloat(token);

        /* istanbul ignore next */
        throw illegal(token, "number", insideTryCatch);
    }

    function parseId(token, acceptNegative) {
        switch (token) {
            case "max": case "MAX": case "Max":
                return 536870911;
            case "0":
                return 0;
        }

        /* istanbul ignore if */
        if (!acceptNegative && token.charAt(0) === "-")
            throw illegal(token, "id");

        if (base10NegRe.test(token))
            return parseInt(token, 10);
        if (base16NegRe.test(token))
            return parseInt(token, 16);

        /* istanbul ignore else */
        if (base8NegRe.test(token))
            return parseInt(token, 8);

        /* istanbul ignore next */
        throw illegal(token, "id");
    }

    function parsePackage() {

        /* istanbul ignore if */
        if (pkg !== undefined)
            throw illegal("package");

        pkg = next();

        /* istanbul ignore if */
        if (!typeRefRe.test(pkg))
            throw illegal(pkg, "name");

        ptr = ptr.define(pkg);
        skip(";");
    }

    function parseImport() {
        var token = peek();
        var whichImports;
        switch (token) {
            case "weak":
                whichImports = weakImports || (weakImports = []);
                next();
                break;
            case "public":
                next();
                // eslint-disable-next-line no-fallthrough
            default:
                whichImports = imports || (imports = []);
                break;
        }
        token = readString();
        skip(";");
        whichImports.push(token);
    }

    function parseSyntax() {
        skip("=");
        syntax = readString();
        isProto3 = syntax === "proto3";

        /* istanbul ignore if */
        if (!isProto3 && syntax !== "proto2")
            throw illegal(syntax, "syntax");

        skip(";");
    }

    function parseCommon(parent, token) {
        switch (token) {

            case "option":
                parseOption(parent, token);
                skip(";");
                return true;

            case "message":
                parseType(parent, token);
                return true;

            case "enum":
                parseEnum(parent, token);
                return true;

            case "service":
                parseService(parent, token);
                return true;

            case "extend":
                parseExtension(parent, token);
                return true;
        }
        return false;
    }

    function ifBlock(obj, fnIf, fnElse) {
        var trailingLine = tn.line;
        if (obj) {
            if(typeof obj.comment !== "string") {
              obj.comment = cmnt(); // try block-type comment
            }
            obj.filename = parse.filename;
        }
        if (skip("{", true)) {
            var token;
            while ((token = next()) !== "}")
                fnIf(token);
            skip(";", true);
        } else {
            if (fnElse)
                fnElse();
            skip(";");
            if (obj && (typeof obj.comment !== "string" || preferTrailingComment))
                obj.comment = cmnt(trailingLine) || obj.comment; // try line-type comment
        }
    }

    function parseType(parent, token) {

        /* istanbul ignore if */
        if (!nameRe.test(token = next()))
            throw illegal(token, "type name");

        var type = new Type(token);
        ifBlock(type, function parseType_block(token) {
            if (parseCommon(type, token))
                return;

            switch (token) {

                case "map":
                    parseMapField(type, token);
                    break;

                case "required":
                case "repeated":
                    parseField(type, token);
                    break;

                case "optional":
                    /* istanbul ignore if */
                    if (isProto3) {
                        parseField(type, "proto3_optional");
                    } else {
                        parseField(type, "optional");
                    }
                    break;

                case "oneof":
                    parseOneOf(type, token);
                    break;

                case "extensions":
                    readRanges(type.extensions || (type.extensions = []));
                    break;

                case "reserved":
                    readRanges(type.reserved || (type.reserved = []), true);
                    break;

                default:
                    /* istanbul ignore if */
                    if (!isProto3 || !typeRefRe.test(token))
                        throw illegal(token);

                    push(token);
                    parseField(type, "optional");
                    break;
            }
        });
        parent.add(type);
    }

    function parseField(parent, rule, extend) {
        var type = next();
        if (type === "group") {
            parseGroup(parent, rule);
            return;
        }
        // Type names can consume multiple tokens, in multiple variants:
        //    package.subpackage   field       tokens: "package.subpackage" [TYPE NAME ENDS HERE] "field"
        //    package . subpackage field       tokens: "package" "." "subpackage" [TYPE NAME ENDS HERE] "field"
        //    package.  subpackage field       tokens: "package." "subpackage" [TYPE NAME ENDS HERE] "field"
        //    package  .subpackage field       tokens: "package" ".subpackage" [TYPE NAME ENDS HERE] "field"
        // Keep reading tokens until we get a type name with no period at the end,
        // and the next token does not start with a period.
        while (type.endsWith(".") || peek().startsWith(".")) {
            type += next();
        }

        /* istanbul ignore if */
        if (!typeRefRe.test(type))
            throw illegal(type, "type");

        var name = next();

        /* istanbul ignore if */
        if (!nameRe.test(name))
            throw illegal(name, "name");

        name = applyCase(name);
        skip("=");

        var field = new Field(name, parseId(next()), type, rule, extend);
        ifBlock(field, function parseField_block(token) {

            /* istanbul ignore else */
            if (token === "option") {
                parseOption(field, token);
                skip(";");
            } else
                throw illegal(token);

        }, function parseField_line() {
            parseInlineOptions(field);
        });

        if (rule === "proto3_optional") {
            // for proto3 optional fields, we create a single-member Oneof to mimic "optional" behavior
            var oneof = new OneOf("_" + name);
            field.setOption("proto3_optional", true);
            oneof.add(field);
            parent.add(oneof);
        } else {
            parent.add(field);
        }

        // JSON defaults to packed=true if not set so we have to set packed=false explicity when
        // parsing proto2 descriptors without the option, where applicable. This must be done for
        // all known packable types and anything that could be an enum (= is not a basic type).
        if (!isProto3 && field.repeated && (types.packed[type] !== undefined || types.basic[type] === undefined))
            field.setOption("packed", false, /* ifNotSet */ true);
    }

    function parseGroup(parent, rule) {
        var name = next();

        /* istanbul ignore if */
        if (!nameRe.test(name))
            throw illegal(name, "name");

        var fieldName = util.lcFirst(name);
        if (name === fieldName)
            name = util.ucFirst(name);
        skip("=");
        var id = parseId(next());
        var type = new Type(name);
        type.group = true;
        var field = new Field(fieldName, id, name, rule);
        field.filename = parse.filename;
        ifBlock(type, function parseGroup_block(token) {
            switch (token) {

                case "option":
                    parseOption(type, token);
                    skip(";");
                    break;

                case "required":
                case "repeated":
                    parseField(type, token);
                    break;

                case "optional":
                    /* istanbul ignore if */
                    if (isProto3) {
                        parseField(type, "proto3_optional");
                    } else {
                        parseField(type, "optional");
                    }
                    break;

                case "message":
                    parseType(type, token);
                    break;

                case "enum":
                    parseEnum(type, token);
                    break;

                /* istanbul ignore next */
                default:
                    throw illegal(token); // there are no groups with proto3 semantics
            }
        });
        parent.add(type)
              .add(field);
    }

    function parseMapField(parent) {
        skip("<");
        var keyType = next();

        /* istanbul ignore if */
        if (types.mapKey[keyType] === undefined)
            throw illegal(keyType, "type");

        skip(",");
        var valueType = next();

        /* istanbul ignore if */
        if (!typeRefRe.test(valueType))
            throw illegal(valueType, "type");

        skip(">");
        var name = next();

        /* istanbul ignore if */
        if (!nameRe.test(name))
            throw illegal(name, "name");

        skip("=");
        var field = new MapField(applyCase(name), parseId(next()), keyType, valueType);
        ifBlock(field, function parseMapField_block(token) {

            /* istanbul ignore else */
            if (token === "option") {
                parseOption(field, token);
                skip(";");
            } else
                throw illegal(token);

        }, function parseMapField_line() {
            parseInlineOptions(field);
        });
        parent.add(field);
    }

    function parseOneOf(parent, token) {

        /* istanbul ignore if */
        if (!nameRe.test(token = next()))
            throw illegal(token, "name");

        var oneof = new OneOf(applyCase(token));
        ifBlock(oneof, function parseOneOf_block(token) {
            if (token === "option") {
                parseOption(oneof, token);
                skip(";");
            } else {
                push(token);
                parseField(oneof, "optional");
            }
        });
        parent.add(oneof);
    }

    function parseEnum(parent, token) {

        /* istanbul ignore if */
        if (!nameRe.test(token = next()))
            throw illegal(token, "name");

        var enm = new Enum(token);
        ifBlock(enm, function parseEnum_block(token) {
          switch(token) {
            case "option":
              parseOption(enm, token);
              skip(";");
              break;

            case "reserved":
              readRanges(enm.reserved || (enm.reserved = []), true);
              break;

            default:
              parseEnumValue(enm, token);
          }
        });
        parent.add(enm);
    }

    function parseEnumValue(parent, token) {

        /* istanbul ignore if */
        if (!nameRe.test(token))
            throw illegal(token, "name");

        skip("=");
        var value = parseId(next(), true),
            dummy = {
                options: undefined
            };
        dummy.setOption = function(name, value) {
            if (this.options === undefined)
                this.options = {};
            this.options[name] = value;
        };
        ifBlock(dummy, function parseEnumValue_block(token) {

            /* istanbul ignore else */
            if (token === "option") {
                parseOption(dummy, token); // skip
                skip(";");
            } else
                throw illegal(token);

        }, function parseEnumValue_line() {
            parseInlineOptions(dummy); // skip
        });
        parent.add(token, value, dummy.comment, dummy.options);
    }

    function parseOption(parent, token) {
        var isCustom = skip("(", true);

        /* istanbul ignore if */
        if (!typeRefRe.test(token = next()))
            throw illegal(token, "name");

        var name = token;
        var option = name;
        var propName;

        if (isCustom) {
            skip(")");
            name = "(" + name + ")";
            option = name;
            token = peek();
            if (fqTypeRefRe.test(token)) {
                propName = token.slice(1); //remove '.' before property name
                name += token;
                next();
            }
        }
        skip("=");
        var optionValue = parseOptionValue(parent, name);
        setParsedOption(parent, option, optionValue, propName);
    }

    function parseOptionValue(parent, name) {
        // { a: "foo" b { c: "bar" } }
        if (skip("{", true)) {
            var objectResult = {};

            while (!skip("}", true)) {
                /* istanbul ignore if */
                if (!nameRe.test(token = next())) {
                    throw illegal(token, "name");
                }
                if (token === null) {
                  throw illegal(token, "end of input");
                }

                var value;
                var propName = token;

                skip(":", true);

                if (peek() === "{")
                    value = parseOptionValue(parent, name + "." + token);
                else if (peek() === "[") {
                    // option (my_option) = {
                    //     repeated_value: [ "foo", "bar" ]
                    // };
                    value = [];
                    var lastValue;
                    if (skip("[", true)) {
                        do {
                            lastValue = readValue(true);
                            value.push(lastValue);
                        } while (skip(",", true));
                        skip("]");
                        if (typeof lastValue !== "undefined") {
                            setOption(parent, name + "." + token, lastValue);
                        }
                    }
                } else {
                    value = readValue(true);
                    setOption(parent, name + "." + token, value);
                }

                var prevValue = objectResult[propName];

                if (prevValue)
                    value = [].concat(prevValue).concat(value);

                objectResult[propName] = value;

                // Semicolons and commas can be optional
                skip(",", true);
                skip(";", true);
            }

            return objectResult;
        }

        var simpleValue = readValue(true);
        setOption(parent, name, simpleValue);
        return simpleValue;
        // Does not enforce a delimiter to be universal
    }

    function setOption(parent, name, value) {
        if (parent.setOption)
            parent.setOption(name, value);
    }

    function setParsedOption(parent, name, value, propName) {
        if (parent.setParsedOption)
            parent.setParsedOption(name, value, propName);
    }

    function parseInlineOptions(parent) {
        if (skip("[", true)) {
            do {
                parseOption(parent, "option");
            } while (skip(",", true));
            skip("]");
        }
        return parent;
    }

    function parseService(parent, token) {

        /* istanbul ignore if */
        if (!nameRe.test(token = next()))
            throw illegal(token, "service name");

        var service = new Service(token);
        ifBlock(service, function parseService_block(token) {
            if (parseCommon(service, token))
                return;

            /* istanbul ignore else */
            if (token === "rpc")
                parseMethod(service, token);
            else
                throw illegal(token);
        });
        parent.add(service);
    }

    function parseMethod(parent, token) {
        // Get the comment of the preceding line now (if one exists) in case the
        // method is defined across multiple lines.
        var commentText = cmnt();

        var type = token;

        /* istanbul ignore if */
        if (!nameRe.test(token = next()))
            throw illegal(token, "name");

        var name = token,
            requestType, requestStream,
            responseType, responseStream;

        skip("(");
        if (skip("stream", true))
            requestStream = true;

        /* istanbul ignore if */
        if (!typeRefRe.test(token = next()))
            throw illegal(token);

        requestType = token;
        skip(")"); skip("returns"); skip("(");
        if (skip("stream", true))
            responseStream = true;

        /* istanbul ignore if */
        if (!typeRefRe.test(token = next()))
            throw illegal(token);

        responseType = token;
        skip(")");

        var method = new Method(name, type, requestType, responseType, requestStream, responseStream);
        method.comment = commentText;
        ifBlock(method, function parseMethod_block(token) {

            /* istanbul ignore else */
            if (token === "option") {
                parseOption(method, token);
                skip(";");
            } else
                throw illegal(token);

        });
        parent.add(method);
    }

    function parseExtension(parent, token) {

        /* istanbul ignore if */
        if (!typeRefRe.test(token = next()))
            throw illegal(token, "reference");

        var reference = token;
        ifBlock(null, function parseExtension_block(token) {
            switch (token) {

                case "required":
                case "repeated":
                    parseField(parent, token, reference);
                    break;

                case "optional":
                    /* istanbul ignore if */
                    if (isProto3) {
                        parseField(parent, "proto3_optional", reference);
                    } else {
                        parseField(parent, "optional", reference);
                    }
                    break;

                default:
                    /* istanbul ignore if */
                    if (!isProto3 || !typeRefRe.test(token))
                        throw illegal(token);
                    push(token);
                    parseField(parent, "optional", reference);
                    break;
            }
        });
    }

    var token;
    while ((token = next()) !== null) {
        switch (token) {

            case "package":

                /* istanbul ignore if */
                if (!head)
                    throw illegal(token);

                parsePackage();
                break;

            case "import":

                /* istanbul ignore if */
                if (!head)
                    throw illegal(token);

                parseImport();
                break;

            case "syntax":

                /* istanbul ignore if */
                if (!head)
                    throw illegal(token);

                parseSyntax();
                break;

            case "option":

                parseOption(ptr, token);
                skip(";");
                break;

            default:

                /* istanbul ignore else */
                if (parseCommon(ptr, token)) {
                    head = false;
                    continue;
                }

                /* istanbul ignore next */
                throw illegal(token);
        }
    }

    parse.filename = null;
    return {
        "package"     : pkg,
        "imports"     : imports,
         weakImports  : weakImports,
         syntax       : syntax,
         root         : root
    };
}

/**
 * Parses the given .proto source and returns an object with the parsed contents.
 * @name parse
 * @function
 * @param {string} source Source contents
 * @param {IParseOptions} [options] Parse options. Defaults to {@link parse.defaults} when omitted.
 * @returns {IParserResult} Parser result
 * @property {string} filename=null Currently processing file name for error reporting, if known
 * @property {IParseOptions} defaults Default {@link IParseOptions}
 * @variation 2
 */


/***/ }),

/***/ "./node_modules/protobufjs/src/reader.js":
/*!***********************************************!*\
  !*** ./node_modules/protobufjs/src/reader.js ***!
  \***********************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = Reader;

var util      = __webpack_require__(/*! ./util/minimal */ "./node_modules/protobufjs/src/util/minimal.js");

var BufferReader; // cyclic

var LongBits  = util.LongBits,
    utf8      = util.utf8;

/* istanbul ignore next */
function indexOutOfRange(reader, writeLength) {
    return RangeError("index out of range: " + reader.pos + " + " + (writeLength || 1) + " > " + reader.len);
}

/**
 * Constructs a new reader instance using the specified buffer.
 * @classdesc Wire format reader using `Uint8Array` if available, otherwise `Array`.
 * @constructor
 * @param {Uint8Array} buffer Buffer to read from
 */
function Reader(buffer) {

    /**
     * Read buffer.
     * @type {Uint8Array}
     */
    this.buf = buffer;

    /**
     * Read buffer position.
     * @type {number}
     */
    this.pos = 0;

    /**
     * Read buffer length.
     * @type {number}
     */
    this.len = buffer.length;
}

var create_array = typeof Uint8Array !== "undefined"
    ? function create_typed_array(buffer) {
        if (buffer instanceof Uint8Array || Array.isArray(buffer))
            return new Reader(buffer);
        throw Error("illegal buffer");
    }
    /* istanbul ignore next */
    : function create_array(buffer) {
        if (Array.isArray(buffer))
            return new Reader(buffer);
        throw Error("illegal buffer");
    };

var create = function create() {
    return util.Buffer
        ? function create_buffer_setup(buffer) {
            return (Reader.create = function create_buffer(buffer) {
                return util.Buffer.isBuffer(buffer)
                    ? new BufferReader(buffer)
                    /* istanbul ignore next */
                    : create_array(buffer);
            })(buffer);
        }
        /* istanbul ignore next */
        : create_array;
};

/**
 * Creates a new reader using the specified buffer.
 * @function
 * @param {Uint8Array|Buffer} buffer Buffer to read from
 * @returns {Reader|BufferReader} A {@link BufferReader} if `buffer` is a Buffer, otherwise a {@link Reader}
 * @throws {Error} If `buffer` is not a valid buffer
 */
Reader.create = create();

Reader.prototype._slice = util.Array.prototype.subarray || /* istanbul ignore next */ util.Array.prototype.slice;

/**
 * Reads a varint as an unsigned 32 bit value.
 * @function
 * @returns {number} Value read
 */
Reader.prototype.uint32 = (function read_uint32_setup() {
    var value = 4294967295; // optimizer type-hint, tends to deopt otherwise (?!)
    return function read_uint32() {
        value = (         this.buf[this.pos] & 127       ) >>> 0; if (this.buf[this.pos++] < 128) return value;
        value = (value | (this.buf[this.pos] & 127) <<  7) >>> 0; if (this.buf[this.pos++] < 128) return value;
        value = (value | (this.buf[this.pos] & 127) << 14) >>> 0; if (this.buf[this.pos++] < 128) return value;
        value = (value | (this.buf[this.pos] & 127) << 21) >>> 0; if (this.buf[this.pos++] < 128) return value;
        value = (value | (this.buf[this.pos] &  15) << 28) >>> 0; if (this.buf[this.pos++] < 128) return value;

        /* istanbul ignore if */
        if ((this.pos += 5) > this.len) {
            this.pos = this.len;
            throw indexOutOfRange(this, 10);
        }
        return value;
    };
})();

/**
 * Reads a varint as a signed 32 bit value.
 * @returns {number} Value read
 */
Reader.prototype.int32 = function read_int32() {
    return this.uint32() | 0;
};

/**
 * Reads a zig-zag encoded varint as a signed 32 bit value.
 * @returns {number} Value read
 */
Reader.prototype.sint32 = function read_sint32() {
    var value = this.uint32();
    return value >>> 1 ^ -(value & 1) | 0;
};

/* eslint-disable no-invalid-this */

function readLongVarint() {
    // tends to deopt with local vars for octet etc.
    var bits = new LongBits(0, 0);
    var i = 0;
    if (this.len - this.pos > 4) { // fast route (lo)
        for (; i < 4; ++i) {
            // 1st..4th
            bits.lo = (bits.lo | (this.buf[this.pos] & 127) << i * 7) >>> 0;
            if (this.buf[this.pos++] < 128)
                return bits;
        }
        // 5th
        bits.lo = (bits.lo | (this.buf[this.pos] & 127) << 28) >>> 0;
        bits.hi = (bits.hi | (this.buf[this.pos] & 127) >>  4) >>> 0;
        if (this.buf[this.pos++] < 128)
            return bits;
        i = 0;
    } else {
        for (; i < 3; ++i) {
            /* istanbul ignore if */
            if (this.pos >= this.len)
                throw indexOutOfRange(this);
            // 1st..3th
            bits.lo = (bits.lo | (this.buf[this.pos] & 127) << i * 7) >>> 0;
            if (this.buf[this.pos++] < 128)
                return bits;
        }
        // 4th
        bits.lo = (bits.lo | (this.buf[this.pos++] & 127) << i * 7) >>> 0;
        return bits;
    }
    if (this.len - this.pos > 4) { // fast route (hi)
        for (; i < 5; ++i) {
            // 6th..10th
            bits.hi = (bits.hi | (this.buf[this.pos] & 127) << i * 7 + 3) >>> 0;
            if (this.buf[this.pos++] < 128)
                return bits;
        }
    } else {
        for (; i < 5; ++i) {
            /* istanbul ignore if */
            if (this.pos >= this.len)
                throw indexOutOfRange(this);
            // 6th..10th
            bits.hi = (bits.hi | (this.buf[this.pos] & 127) << i * 7 + 3) >>> 0;
            if (this.buf[this.pos++] < 128)
                return bits;
        }
    }
    /* istanbul ignore next */
    throw Error("invalid varint encoding");
}

/* eslint-enable no-invalid-this */

/**
 * Reads a varint as a signed 64 bit value.
 * @name Reader#int64
 * @function
 * @returns {Long} Value read
 */

/**
 * Reads a varint as an unsigned 64 bit value.
 * @name Reader#uint64
 * @function
 * @returns {Long} Value read
 */

/**
 * Reads a zig-zag encoded varint as a signed 64 bit value.
 * @name Reader#sint64
 * @function
 * @returns {Long} Value read
 */

/**
 * Reads a varint as a boolean.
 * @returns {boolean} Value read
 */
Reader.prototype.bool = function read_bool() {
    return this.uint32() !== 0;
};

function readFixed32_end(buf, end) { // note that this uses `end`, not `pos`
    return (buf[end - 4]
          | buf[end - 3] << 8
          | buf[end - 2] << 16
          | buf[end - 1] << 24) >>> 0;
}

/**
 * Reads fixed 32 bits as an unsigned 32 bit integer.
 * @returns {number} Value read
 */
Reader.prototype.fixed32 = function read_fixed32() {

    /* istanbul ignore if */
    if (this.pos + 4 > this.len)
        throw indexOutOfRange(this, 4);

    return readFixed32_end(this.buf, this.pos += 4);
};

/**
 * Reads fixed 32 bits as a signed 32 bit integer.
 * @returns {number} Value read
 */
Reader.prototype.sfixed32 = function read_sfixed32() {

    /* istanbul ignore if */
    if (this.pos + 4 > this.len)
        throw indexOutOfRange(this, 4);

    return readFixed32_end(this.buf, this.pos += 4) | 0;
};

/* eslint-disable no-invalid-this */

function readFixed64(/* this: Reader */) {

    /* istanbul ignore if */
    if (this.pos + 8 > this.len)
        throw indexOutOfRange(this, 8);

    return new LongBits(readFixed32_end(this.buf, this.pos += 4), readFixed32_end(this.buf, this.pos += 4));
}

/* eslint-enable no-invalid-this */

/**
 * Reads fixed 64 bits.
 * @name Reader#fixed64
 * @function
 * @returns {Long} Value read
 */

/**
 * Reads zig-zag encoded fixed 64 bits.
 * @name Reader#sfixed64
 * @function
 * @returns {Long} Value read
 */

/**
 * Reads a float (32 bit) as a number.
 * @function
 * @returns {number} Value read
 */
Reader.prototype.float = function read_float() {

    /* istanbul ignore if */
    if (this.pos + 4 > this.len)
        throw indexOutOfRange(this, 4);

    var value = util.float.readFloatLE(this.buf, this.pos);
    this.pos += 4;
    return value;
};

/**
 * Reads a double (64 bit float) as a number.
 * @function
 * @returns {number} Value read
 */
Reader.prototype.double = function read_double() {

    /* istanbul ignore if */
    if (this.pos + 8 > this.len)
        throw indexOutOfRange(this, 4);

    var value = util.float.readDoubleLE(this.buf, this.pos);
    this.pos += 8;
    return value;
};

/**
 * Reads a sequence of bytes preceeded by its length as a varint.
 * @returns {Uint8Array} Value read
 */
Reader.prototype.bytes = function read_bytes() {
    var length = this.uint32(),
        start  = this.pos,
        end    = this.pos + length;

    /* istanbul ignore if */
    if (end > this.len)
        throw indexOutOfRange(this, length);

    this.pos += length;
    if (Array.isArray(this.buf)) // plain array
        return this.buf.slice(start, end);

    if (start === end) { // fix for IE 10/Win8 and others' subarray returning array of size 1
        var nativeBuffer = util.Buffer;
        return nativeBuffer
            ? nativeBuffer.alloc(0)
            : new this.buf.constructor(0);
    }
    return this._slice.call(this.buf, start, end);
};

/**
 * Reads a string preceeded by its byte length as a varint.
 * @returns {string} Value read
 */
Reader.prototype.string = function read_string() {
    var bytes = this.bytes();
    return utf8.read(bytes, 0, bytes.length);
};

/**
 * Skips the specified number of bytes if specified, otherwise skips a varint.
 * @param {number} [length] Length if known, otherwise a varint is assumed
 * @returns {Reader} `this`
 */
Reader.prototype.skip = function skip(length) {
    if (typeof length === "number") {
        /* istanbul ignore if */
        if (this.pos + length > this.len)
            throw indexOutOfRange(this, length);
        this.pos += length;
    } else {
        do {
            /* istanbul ignore if */
            if (this.pos >= this.len)
                throw indexOutOfRange(this);
        } while (this.buf[this.pos++] & 128);
    }
    return this;
};

/**
 * Skips the next element of the specified wire type.
 * @param {number} wireType Wire type received
 * @returns {Reader} `this`
 */
Reader.prototype.skipType = function(wireType) {
    switch (wireType) {
        case 0:
            this.skip();
            break;
        case 1:
            this.skip(8);
            break;
        case 2:
            this.skip(this.uint32());
            break;
        case 3:
            while ((wireType = this.uint32() & 7) !== 4) {
                this.skipType(wireType);
            }
            break;
        case 5:
            this.skip(4);
            break;

        /* istanbul ignore next */
        default:
            throw Error("invalid wire type " + wireType + " at offset " + this.pos);
    }
    return this;
};

Reader._configure = function(BufferReader_) {
    BufferReader = BufferReader_;
    Reader.create = create();
    BufferReader._configure();

    var fn = util.Long ? "toLong" : /* istanbul ignore next */ "toNumber";
    util.merge(Reader.prototype, {

        int64: function read_int64() {
            return readLongVarint.call(this)[fn](false);
        },

        uint64: function read_uint64() {
            return readLongVarint.call(this)[fn](true);
        },

        sint64: function read_sint64() {
            return readLongVarint.call(this).zzDecode()[fn](false);
        },

        fixed64: function read_fixed64() {
            return readFixed64.call(this)[fn](true);
        },

        sfixed64: function read_sfixed64() {
            return readFixed64.call(this)[fn](false);
        }

    });
};


/***/ }),

/***/ "./node_modules/protobufjs/src/reader_buffer.js":
/*!******************************************************!*\
  !*** ./node_modules/protobufjs/src/reader_buffer.js ***!
  \******************************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = BufferReader;

// extends Reader
var Reader = __webpack_require__(/*! ./reader */ "./node_modules/protobufjs/src/reader.js");
(BufferReader.prototype = Object.create(Reader.prototype)).constructor = BufferReader;

var util = __webpack_require__(/*! ./util/minimal */ "./node_modules/protobufjs/src/util/minimal.js");

/**
 * Constructs a new buffer reader instance.
 * @classdesc Wire format reader using node buffers.
 * @extends Reader
 * @constructor
 * @param {Buffer} buffer Buffer to read from
 */
function BufferReader(buffer) {
    Reader.call(this, buffer);

    /**
     * Read buffer.
     * @name BufferReader#buf
     * @type {Buffer}
     */
}

BufferReader._configure = function () {
    /* istanbul ignore else */
    if (util.Buffer)
        BufferReader.prototype._slice = util.Buffer.prototype.slice;
};


/**
 * @override
 */
BufferReader.prototype.string = function read_string_buffer() {
    var len = this.uint32(); // modifies pos
    return this.buf.utf8Slice
        ? this.buf.utf8Slice(this.pos, this.pos = Math.min(this.pos + len, this.len))
        : this.buf.toString("utf-8", this.pos, this.pos = Math.min(this.pos + len, this.len));
};

/**
 * Reads a sequence of bytes preceeded by its length as a varint.
 * @name BufferReader#bytes
 * @function
 * @returns {Buffer} Value read
 */

BufferReader._configure();


/***/ }),

/***/ "./node_modules/protobufjs/src/root.js":
/*!*********************************************!*\
  !*** ./node_modules/protobufjs/src/root.js ***!
  \*********************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = Root;

// extends Namespace
var Namespace = __webpack_require__(/*! ./namespace */ "./node_modules/protobufjs/src/namespace.js");
((Root.prototype = Object.create(Namespace.prototype)).constructor = Root).className = "Root";

var Field   = __webpack_require__(/*! ./field */ "./node_modules/protobufjs/src/field.js"),
    Enum    = __webpack_require__(/*! ./enum */ "./node_modules/protobufjs/src/enum.js"),
    OneOf   = __webpack_require__(/*! ./oneof */ "./node_modules/protobufjs/src/oneof.js"),
    util    = __webpack_require__(/*! ./util */ "./node_modules/protobufjs/src/util.js");

var Type,   // cyclic
    parse,  // might be excluded
    common; // "

/**
 * Constructs a new root namespace instance.
 * @classdesc Root namespace wrapping all types, enums, services, sub-namespaces etc. that belong together.
 * @extends NamespaceBase
 * @constructor
 * @param {Object.<string,*>} [options] Top level options
 */
function Root(options) {
    Namespace.call(this, "", options);

    /**
     * Deferred extension fields.
     * @type {Field[]}
     */
    this.deferred = [];

    /**
     * Resolved file names of loaded files.
     * @type {string[]}
     */
    this.files = [];
}

/**
 * Loads a namespace descriptor into a root namespace.
 * @param {INamespace} json Nameespace descriptor
 * @param {Root} [root] Root namespace, defaults to create a new one if omitted
 * @returns {Root} Root namespace
 */
Root.fromJSON = function fromJSON(json, root) {
    if (!root)
        root = new Root();
    if (json.options)
        root.setOptions(json.options);
    return root.addJSON(json.nested);
};

/**
 * Resolves the path of an imported file, relative to the importing origin.
 * This method exists so you can override it with your own logic in case your imports are scattered over multiple directories.
 * @function
 * @param {string} origin The file name of the importing file
 * @param {string} target The file name being imported
 * @returns {string|null} Resolved path to `target` or `null` to skip the file
 */
Root.prototype.resolvePath = util.path.resolve;

/**
 * Fetch content from file path or url
 * This method exists so you can override it with your own logic.
 * @function
 * @param {string} path File path or url
 * @param {FetchCallback} callback Callback function
 * @returns {undefined}
 */
Root.prototype.fetch = util.fetch;

// A symbol-like function to safely signal synchronous loading
/* istanbul ignore next */
function SYNC() {} // eslint-disable-line no-empty-function

/**
 * Loads one or multiple .proto or preprocessed .json files into this root namespace and calls the callback.
 * @param {string|string[]} filename Names of one or multiple files to load
 * @param {IParseOptions} options Parse options
 * @param {LoadCallback} callback Callback function
 * @returns {undefined}
 */
Root.prototype.load = function load(filename, options, callback) {
    if (typeof options === "function") {
        callback = options;
        options = undefined;
    }
    var self = this;
    if (!callback)
        return util.asPromise(load, self, filename, options);

    var sync = callback === SYNC; // undocumented

    // Finishes loading by calling the callback (exactly once)
    function finish(err, root) {
        /* istanbul ignore if */
        if (!callback)
            return;
        if (sync)
            throw err;
        var cb = callback;
        callback = null;
        cb(err, root);
    }

    // Bundled definition existence checking
    function getBundledFileName(filename) {
        var idx = filename.lastIndexOf("google/protobuf/");
        if (idx > -1) {
            var altname = filename.substring(idx);
            if (altname in common) return altname;
        }
        return null;
    }

    // Processes a single file
    function process(filename, source) {
        try {
            if (util.isString(source) && source.charAt(0) === "{")
                source = JSON.parse(source);
            if (!util.isString(source))
                self.setOptions(source.options).addJSON(source.nested);
            else {
                parse.filename = filename;
                var parsed = parse(source, self, options),
                    resolved,
                    i = 0;
                if (parsed.imports)
                    for (; i < parsed.imports.length; ++i)
                        if (resolved = getBundledFileName(parsed.imports[i]) || self.resolvePath(filename, parsed.imports[i]))
                            fetch(resolved);
                if (parsed.weakImports)
                    for (i = 0; i < parsed.weakImports.length; ++i)
                        if (resolved = getBundledFileName(parsed.weakImports[i]) || self.resolvePath(filename, parsed.weakImports[i]))
                            fetch(resolved, true);
            }
        } catch (err) {
            finish(err);
        }
        if (!sync && !queued)
            finish(null, self); // only once anyway
    }

    // Fetches a single file
    function fetch(filename, weak) {
        filename = getBundledFileName(filename) || filename;

        // Skip if already loaded / attempted
        if (self.files.indexOf(filename) > -1)
            return;
        self.files.push(filename);

        // Shortcut bundled definitions
        if (filename in common) {
            if (sync)
                process(filename, common[filename]);
            else {
                ++queued;
                setTimeout(function() {
                    --queued;
                    process(filename, common[filename]);
                });
            }
            return;
        }

        // Otherwise fetch from disk or network
        if (sync) {
            var source;
            try {
                source = util.fs.readFileSync(filename).toString("utf8");
            } catch (err) {
                if (!weak)
                    finish(err);
                return;
            }
            process(filename, source);
        } else {
            ++queued;
            self.fetch(filename, function(err, source) {
                --queued;
                /* istanbul ignore if */
                if (!callback)
                    return; // terminated meanwhile
                if (err) {
                    /* istanbul ignore else */
                    if (!weak)
                        finish(err);
                    else if (!queued) // can't be covered reliably
                        finish(null, self);
                    return;
                }
                process(filename, source);
            });
        }
    }
    var queued = 0;

    // Assembling the root namespace doesn't require working type
    // references anymore, so we can load everything in parallel
    if (util.isString(filename))
        filename = [ filename ];
    for (var i = 0, resolved; i < filename.length; ++i)
        if (resolved = self.resolvePath("", filename[i]))
            fetch(resolved);

    if (sync)
        return self;
    if (!queued)
        finish(null, self);
    return undefined;
};
// function load(filename:string, options:IParseOptions, callback:LoadCallback):undefined

/**
 * Loads one or multiple .proto or preprocessed .json files into this root namespace and calls the callback.
 * @function Root#load
 * @param {string|string[]} filename Names of one or multiple files to load
 * @param {LoadCallback} callback Callback function
 * @returns {undefined}
 * @variation 2
 */
// function load(filename:string, callback:LoadCallback):undefined

/**
 * Loads one or multiple .proto or preprocessed .json files into this root namespace and returns a promise.
 * @function Root#load
 * @param {string|string[]} filename Names of one or multiple files to load
 * @param {IParseOptions} [options] Parse options. Defaults to {@link parse.defaults} when omitted.
 * @returns {Promise<Root>} Promise
 * @variation 3
 */
// function load(filename:string, [options:IParseOptions]):Promise<Root>

/**
 * Synchronously loads one or multiple .proto or preprocessed .json files into this root namespace (node only).
 * @function Root#loadSync
 * @param {string|string[]} filename Names of one or multiple files to load
 * @param {IParseOptions} [options] Parse options. Defaults to {@link parse.defaults} when omitted.
 * @returns {Root} Root namespace
 * @throws {Error} If synchronous fetching is not supported (i.e. in browsers) or if a file's syntax is invalid
 */
Root.prototype.loadSync = function loadSync(filename, options) {
    if (!util.isNode)
        throw Error("not supported");
    return this.load(filename, options, SYNC);
};

/**
 * @override
 */
Root.prototype.resolveAll = function resolveAll() {
    if (this.deferred.length)
        throw Error("unresolvable extensions: " + this.deferred.map(function(field) {
            return "'extend " + field.extend + "' in " + field.parent.fullName;
        }).join(", "));
    return Namespace.prototype.resolveAll.call(this);
};

// only uppercased (and thus conflict-free) children are exposed, see below
var exposeRe = /^[A-Z]/;

/**
 * Handles a deferred declaring extension field by creating a sister field to represent it within its extended type.
 * @param {Root} root Root instance
 * @param {Field} field Declaring extension field witin the declaring type
 * @returns {boolean} `true` if successfully added to the extended type, `false` otherwise
 * @inner
 * @ignore
 */
function tryHandleExtension(root, field) {
    var extendedType = field.parent.lookup(field.extend);
    if (extendedType) {
        var sisterField = new Field(field.fullName, field.id, field.type, field.rule, undefined, field.options);
        //do not allow to extend same field twice to prevent the error
        if (extendedType.get(sisterField.name)) {
            return true;
        }
        sisterField.declaringField = field;
        field.extensionField = sisterField;
        extendedType.add(sisterField);
        return true;
    }
    return false;
}

/**
 * Called when any object is added to this root or its sub-namespaces.
 * @param {ReflectionObject} object Object added
 * @returns {undefined}
 * @private
 */
Root.prototype._handleAdd = function _handleAdd(object) {
    if (object instanceof Field) {

        if (/* an extension field (implies not part of a oneof) */ object.extend !== undefined && /* not already handled */ !object.extensionField)
            if (!tryHandleExtension(this, object))
                this.deferred.push(object);

    } else if (object instanceof Enum) {

        if (exposeRe.test(object.name))
            object.parent[object.name] = object.values; // expose enum values as property of its parent

    } else if (!(object instanceof OneOf)) /* everything else is a namespace */ {

        if (object instanceof Type) // Try to handle any deferred extensions
            for (var i = 0; i < this.deferred.length;)
                if (tryHandleExtension(this, this.deferred[i]))
                    this.deferred.splice(i, 1);
                else
                    ++i;
        for (var j = 0; j < /* initializes */ object.nestedArray.length; ++j) // recurse into the namespace
            this._handleAdd(object._nestedArray[j]);
        if (exposeRe.test(object.name))
            object.parent[object.name] = object; // expose namespace as property of its parent
    }

    // The above also adds uppercased (and thus conflict-free) nested types, services and enums as
    // properties of namespaces just like static code does. This allows using a .d.ts generated for
    // a static module with reflection-based solutions where the condition is met.
};

/**
 * Called when any object is removed from this root or its sub-namespaces.
 * @param {ReflectionObject} object Object removed
 * @returns {undefined}
 * @private
 */
Root.prototype._handleRemove = function _handleRemove(object) {
    if (object instanceof Field) {

        if (/* an extension field */ object.extend !== undefined) {
            if (/* already handled */ object.extensionField) { // remove its sister field
                object.extensionField.parent.remove(object.extensionField);
                object.extensionField = null;
            } else { // cancel the extension
                var index = this.deferred.indexOf(object);
                /* istanbul ignore else */
                if (index > -1)
                    this.deferred.splice(index, 1);
            }
        }

    } else if (object instanceof Enum) {

        if (exposeRe.test(object.name))
            delete object.parent[object.name]; // unexpose enum values

    } else if (object instanceof Namespace) {

        for (var i = 0; i < /* initializes */ object.nestedArray.length; ++i) // recurse into the namespace
            this._handleRemove(object._nestedArray[i]);

        if (exposeRe.test(object.name))
            delete object.parent[object.name]; // unexpose namespaces

    }
};

// Sets up cyclic dependencies (called in index-light)
Root._configure = function(Type_, parse_, common_) {
    Type   = Type_;
    parse  = parse_;
    common = common_;
};


/***/ }),

/***/ "./node_modules/protobufjs/src/roots.js":
/*!**********************************************!*\
  !*** ./node_modules/protobufjs/src/roots.js ***!
  \**********************************************/
/***/ ((module) => {


module.exports = {};

/**
 * Named roots.
 * This is where pbjs stores generated structures (the option `-r, --root` specifies a name).
 * Can also be used manually to make roots available across modules.
 * @name roots
 * @type {Object.<string,Root>}
 * @example
 * // pbjs -r myroot -o compiled.js ...
 *
 * // in another module:
 * require("./compiled.js");
 *
 * // in any subsequent module:
 * var root = protobuf.roots["myroot"];
 */


/***/ }),

/***/ "./node_modules/protobufjs/src/rpc.js":
/*!********************************************!*\
  !*** ./node_modules/protobufjs/src/rpc.js ***!
  \********************************************/
/***/ ((__unused_webpack_module, exports, __webpack_require__) => {



/**
 * Streaming RPC helpers.
 * @namespace
 */
var rpc = exports;

/**
 * RPC implementation passed to {@link Service#create} performing a service request on network level, i.e. by utilizing http requests or websockets.
 * @typedef RPCImpl
 * @type {function}
 * @param {Method|rpc.ServiceMethod<Message<{}>,Message<{}>>} method Reflected or static method being called
 * @param {Uint8Array} requestData Request data
 * @param {RPCImplCallback} callback Callback function
 * @returns {undefined}
 * @example
 * function rpcImpl(method, requestData, callback) {
 *     if (protobuf.util.lcFirst(method.name) !== "myMethod") // compatible with static code
 *         throw Error("no such method");
 *     asynchronouslyObtainAResponse(requestData, function(err, responseData) {
 *         callback(err, responseData);
 *     });
 * }
 */

/**
 * Node-style callback as used by {@link RPCImpl}.
 * @typedef RPCImplCallback
 * @type {function}
 * @param {Error|null} error Error, if any, otherwise `null`
 * @param {Uint8Array|null} [response] Response data or `null` to signal end of stream, if there hasn't been an error
 * @returns {undefined}
 */

rpc.Service = __webpack_require__(/*! ./rpc/service */ "./node_modules/protobufjs/src/rpc/service.js");


/***/ }),

/***/ "./node_modules/protobufjs/src/rpc/service.js":
/*!****************************************************!*\
  !*** ./node_modules/protobufjs/src/rpc/service.js ***!
  \****************************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = Service;

var util = __webpack_require__(/*! ../util/minimal */ "./node_modules/protobufjs/src/util/minimal.js");

// Extends EventEmitter
(Service.prototype = Object.create(util.EventEmitter.prototype)).constructor = Service;

/**
 * A service method callback as used by {@link rpc.ServiceMethod|ServiceMethod}.
 *
 * Differs from {@link RPCImplCallback} in that it is an actual callback of a service method which may not return `response = null`.
 * @typedef rpc.ServiceMethodCallback
 * @template TRes extends Message<TRes>
 * @type {function}
 * @param {Error|null} error Error, if any
 * @param {TRes} [response] Response message
 * @returns {undefined}
 */

/**
 * A service method part of a {@link rpc.Service} as created by {@link Service.create}.
 * @typedef rpc.ServiceMethod
 * @template TReq extends Message<TReq>
 * @template TRes extends Message<TRes>
 * @type {function}
 * @param {TReq|Properties<TReq>} request Request message or plain object
 * @param {rpc.ServiceMethodCallback<TRes>} [callback] Node-style callback called with the error, if any, and the response message
 * @returns {Promise<Message<TRes>>} Promise if `callback` has been omitted, otherwise `undefined`
 */

/**
 * Constructs a new RPC service instance.
 * @classdesc An RPC service as returned by {@link Service#create}.
 * @exports rpc.Service
 * @extends util.EventEmitter
 * @constructor
 * @param {RPCImpl} rpcImpl RPC implementation
 * @param {boolean} [requestDelimited=false] Whether requests are length-delimited
 * @param {boolean} [responseDelimited=false] Whether responses are length-delimited
 */
function Service(rpcImpl, requestDelimited, responseDelimited) {

    if (typeof rpcImpl !== "function")
        throw TypeError("rpcImpl must be a function");

    util.EventEmitter.call(this);

    /**
     * RPC implementation. Becomes `null` once the service is ended.
     * @type {RPCImpl|null}
     */
    this.rpcImpl = rpcImpl;

    /**
     * Whether requests are length-delimited.
     * @type {boolean}
     */
    this.requestDelimited = Boolean(requestDelimited);

    /**
     * Whether responses are length-delimited.
     * @type {boolean}
     */
    this.responseDelimited = Boolean(responseDelimited);
}

/**
 * Calls a service method through {@link rpc.Service#rpcImpl|rpcImpl}.
 * @param {Method|rpc.ServiceMethod<TReq,TRes>} method Reflected or static method
 * @param {Constructor<TReq>} requestCtor Request constructor
 * @param {Constructor<TRes>} responseCtor Response constructor
 * @param {TReq|Properties<TReq>} request Request message or plain object
 * @param {rpc.ServiceMethodCallback<TRes>} callback Service callback
 * @returns {undefined}
 * @template TReq extends Message<TReq>
 * @template TRes extends Message<TRes>
 */
Service.prototype.rpcCall = function rpcCall(method, requestCtor, responseCtor, request, callback) {

    if (!request)
        throw TypeError("request must be specified");

    var self = this;
    if (!callback)
        return util.asPromise(rpcCall, self, method, requestCtor, responseCtor, request);

    if (!self.rpcImpl) {
        setTimeout(function() { callback(Error("already ended")); }, 0);
        return undefined;
    }

    try {
        return self.rpcImpl(
            method,
            requestCtor[self.requestDelimited ? "encodeDelimited" : "encode"](request).finish(),
            function rpcCallback(err, response) {

                if (err) {
                    self.emit("error", err, method);
                    return callback(err);
                }

                if (response === null) {
                    self.end(/* endedByRPC */ true);
                    return undefined;
                }

                if (!(response instanceof responseCtor)) {
                    try {
                        response = responseCtor[self.responseDelimited ? "decodeDelimited" : "decode"](response);
                    } catch (err) {
                        self.emit("error", err, method);
                        return callback(err);
                    }
                }

                self.emit("data", response, method);
                return callback(null, response);
            }
        );
    } catch (err) {
        self.emit("error", err, method);
        setTimeout(function() { callback(err); }, 0);
        return undefined;
    }
};

/**
 * Ends this service and emits the `end` event.
 * @param {boolean} [endedByRPC=false] Whether the service has been ended by the RPC implementation.
 * @returns {rpc.Service} `this`
 */
Service.prototype.end = function end(endedByRPC) {
    if (this.rpcImpl) {
        if (!endedByRPC) // signal end to rpcImpl
            this.rpcImpl(null, null, null);
        this.rpcImpl = null;
        this.emit("end").off();
    }
    return this;
};


/***/ }),

/***/ "./node_modules/protobufjs/src/service.js":
/*!************************************************!*\
  !*** ./node_modules/protobufjs/src/service.js ***!
  \************************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = Service;

// extends Namespace
var Namespace = __webpack_require__(/*! ./namespace */ "./node_modules/protobufjs/src/namespace.js");
((Service.prototype = Object.create(Namespace.prototype)).constructor = Service).className = "Service";

var Method = __webpack_require__(/*! ./method */ "./node_modules/protobufjs/src/method.js"),
    util   = __webpack_require__(/*! ./util */ "./node_modules/protobufjs/src/util.js"),
    rpc    = __webpack_require__(/*! ./rpc */ "./node_modules/protobufjs/src/rpc.js");

/**
 * Constructs a new service instance.
 * @classdesc Reflected service.
 * @extends NamespaceBase
 * @constructor
 * @param {string} name Service name
 * @param {Object.<string,*>} [options] Service options
 * @throws {TypeError} If arguments are invalid
 */
function Service(name, options) {
    Namespace.call(this, name, options);

    /**
     * Service methods.
     * @type {Object.<string,Method>}
     */
    this.methods = {}; // toJSON, marker

    /**
     * Cached methods as an array.
     * @type {Method[]|null}
     * @private
     */
    this._methodsArray = null;
}

/**
 * Service descriptor.
 * @interface IService
 * @extends INamespace
 * @property {Object.<string,IMethod>} methods Method descriptors
 */

/**
 * Constructs a service from a service descriptor.
 * @param {string} name Service name
 * @param {IService} json Service descriptor
 * @returns {Service} Created service
 * @throws {TypeError} If arguments are invalid
 */
Service.fromJSON = function fromJSON(name, json) {
    var service = new Service(name, json.options);
    /* istanbul ignore else */
    if (json.methods)
        for (var names = Object.keys(json.methods), i = 0; i < names.length; ++i)
            service.add(Method.fromJSON(names[i], json.methods[names[i]]));
    if (json.nested)
        service.addJSON(json.nested);
    service.comment = json.comment;
    return service;
};

/**
 * Converts this service to a service descriptor.
 * @param {IToJSONOptions} [toJSONOptions] JSON conversion options
 * @returns {IService} Service descriptor
 */
Service.prototype.toJSON = function toJSON(toJSONOptions) {
    var inherited = Namespace.prototype.toJSON.call(this, toJSONOptions);
    var keepComments = toJSONOptions ? Boolean(toJSONOptions.keepComments) : false;
    return util.toObject([
        "options" , inherited && inherited.options || undefined,
        "methods" , Namespace.arrayToJSON(this.methodsArray, toJSONOptions) || /* istanbul ignore next */ {},
        "nested"  , inherited && inherited.nested || undefined,
        "comment" , keepComments ? this.comment : undefined
    ]);
};

/**
 * Methods of this service as an array for iteration.
 * @name Service#methodsArray
 * @type {Method[]}
 * @readonly
 */
Object.defineProperty(Service.prototype, "methodsArray", {
    get: function() {
        return this._methodsArray || (this._methodsArray = util.toArray(this.methods));
    }
});

function clearCache(service) {
    service._methodsArray = null;
    return service;
}

/**
 * @override
 */
Service.prototype.get = function get(name) {
    return this.methods[name]
        || Namespace.prototype.get.call(this, name);
};

/**
 * @override
 */
Service.prototype.resolveAll = function resolveAll() {
    var methods = this.methodsArray;
    for (var i = 0; i < methods.length; ++i)
        methods[i].resolve();
    return Namespace.prototype.resolve.call(this);
};

/**
 * @override
 */
Service.prototype.add = function add(object) {

    /* istanbul ignore if */
    if (this.get(object.name))
        throw Error("duplicate name '" + object.name + "' in " + this);

    if (object instanceof Method) {
        this.methods[object.name] = object;
        object.parent = this;
        return clearCache(this);
    }
    return Namespace.prototype.add.call(this, object);
};

/**
 * @override
 */
Service.prototype.remove = function remove(object) {
    if (object instanceof Method) {

        /* istanbul ignore if */
        if (this.methods[object.name] !== object)
            throw Error(object + " is not a member of " + this);

        delete this.methods[object.name];
        object.parent = null;
        return clearCache(this);
    }
    return Namespace.prototype.remove.call(this, object);
};

/**
 * Creates a runtime service using the specified rpc implementation.
 * @param {RPCImpl} rpcImpl RPC implementation
 * @param {boolean} [requestDelimited=false] Whether requests are length-delimited
 * @param {boolean} [responseDelimited=false] Whether responses are length-delimited
 * @returns {rpc.Service} RPC service. Useful where requests and/or responses are streamed.
 */
Service.prototype.create = function create(rpcImpl, requestDelimited, responseDelimited) {
    var rpcService = new rpc.Service(rpcImpl, requestDelimited, responseDelimited);
    for (var i = 0, method; i < /* initializes */ this.methodsArray.length; ++i) {
        var methodName = util.lcFirst((method = this._methodsArray[i]).resolve().name).replace(/[^$\w_]/g, "");
        rpcService[methodName] = util.codegen(["r","c"], util.isReserved(methodName) ? methodName + "_" : methodName)("return this.rpcCall(m,q,s,r,c)")({
            m: method,
            q: method.resolvedRequestType.ctor,
            s: method.resolvedResponseType.ctor
        });
    }
    return rpcService;
};


/***/ }),

/***/ "./node_modules/protobufjs/src/tokenize.js":
/*!*************************************************!*\
  !*** ./node_modules/protobufjs/src/tokenize.js ***!
  \*************************************************/
/***/ ((module) => {


module.exports = tokenize;

var delimRe        = /[\s{}=;:[\],'"()<>]/g,
    stringDoubleRe = /(?:"([^"\\]*(?:\\.[^"\\]*)*)")/g,
    stringSingleRe = /(?:'([^'\\]*(?:\\.[^'\\]*)*)')/g;

var setCommentRe = /^ *[*/]+ */,
    setCommentAltRe = /^\s*\*?\/*/,
    setCommentSplitRe = /\n/g,
    whitespaceRe = /\s/,
    unescapeRe = /\\(.?)/g;

var unescapeMap = {
    "0": "\0",
    "r": "\r",
    "n": "\n",
    "t": "\t"
};

/**
 * Unescapes a string.
 * @param {string} str String to unescape
 * @returns {string} Unescaped string
 * @property {Object.<string,string>} map Special characters map
 * @memberof tokenize
 */
function unescape(str) {
    return str.replace(unescapeRe, function($0, $1) {
        switch ($1) {
            case "\\":
            case "":
                return $1;
            default:
                return unescapeMap[$1] || "";
        }
    });
}

tokenize.unescape = unescape;

/**
 * Gets the next token and advances.
 * @typedef TokenizerHandleNext
 * @type {function}
 * @returns {string|null} Next token or `null` on eof
 */

/**
 * Peeks for the next token.
 * @typedef TokenizerHandlePeek
 * @type {function}
 * @returns {string|null} Next token or `null` on eof
 */

/**
 * Pushes a token back to the stack.
 * @typedef TokenizerHandlePush
 * @type {function}
 * @param {string} token Token
 * @returns {undefined}
 */

/**
 * Skips the next token.
 * @typedef TokenizerHandleSkip
 * @type {function}
 * @param {string} expected Expected token
 * @param {boolean} [optional=false] If optional
 * @returns {boolean} Whether the token matched
 * @throws {Error} If the token didn't match and is not optional
 */

/**
 * Gets the comment on the previous line or, alternatively, the line comment on the specified line.
 * @typedef TokenizerHandleCmnt
 * @type {function}
 * @param {number} [line] Line number
 * @returns {string|null} Comment text or `null` if none
 */

/**
 * Handle object returned from {@link tokenize}.
 * @interface ITokenizerHandle
 * @property {TokenizerHandleNext} next Gets the next token and advances (`null` on eof)
 * @property {TokenizerHandlePeek} peek Peeks for the next token (`null` on eof)
 * @property {TokenizerHandlePush} push Pushes a token back to the stack
 * @property {TokenizerHandleSkip} skip Skips a token, returns its presence and advances or, if non-optional and not present, throws
 * @property {TokenizerHandleCmnt} cmnt Gets the comment on the previous line or the line comment on the specified line, if any
 * @property {number} line Current line number
 */

/**
 * Tokenizes the given .proto source and returns an object with useful utility functions.
 * @param {string} source Source contents
 * @param {boolean} alternateCommentMode Whether we should activate alternate comment parsing mode.
 * @returns {ITokenizerHandle} Tokenizer handle
 */
function tokenize(source, alternateCommentMode) {
    /* eslint-disable callback-return */
    source = source.toString();

    var offset = 0,
        length = source.length,
        line = 1,
        lastCommentLine = 0,
        comments = {};

    var stack = [];

    var stringDelim = null;

    /* istanbul ignore next */
    /**
     * Creates an error for illegal syntax.
     * @param {string} subject Subject
     * @returns {Error} Error created
     * @inner
     */
    function illegal(subject) {
        return Error("illegal " + subject + " (line " + line + ")");
    }

    /**
     * Reads a string till its end.
     * @returns {string} String read
     * @inner
     */
    function readString() {
        var re = stringDelim === "'" ? stringSingleRe : stringDoubleRe;
        re.lastIndex = offset - 1;
        var match = re.exec(source);
        if (!match)
            throw illegal("string");
        offset = re.lastIndex;
        push(stringDelim);
        stringDelim = null;
        return unescape(match[1]);
    }

    /**
     * Gets the character at `pos` within the source.
     * @param {number} pos Position
     * @returns {string} Character
     * @inner
     */
    function charAt(pos) {
        return source.charAt(pos);
    }

    /**
     * Sets the current comment text.
     * @param {number} start Start offset
     * @param {number} end End offset
     * @param {boolean} isLeading set if a leading comment
     * @returns {undefined}
     * @inner
     */
    function setComment(start, end, isLeading) {
        var comment = {
            type: source.charAt(start++),
            lineEmpty: false,
            leading: isLeading,
        };
        var lookback;
        if (alternateCommentMode) {
            lookback = 2;  // alternate comment parsing: "//" or "/*"
        } else {
            lookback = 3;  // "///" or "/**"
        }
        var commentOffset = start - lookback,
            c;
        do {
            if (--commentOffset < 0 ||
                    (c = source.charAt(commentOffset)) === "\n") {
                comment.lineEmpty = true;
                break;
            }
        } while (c === " " || c === "\t");
        var lines = source
            .substring(start, end)
            .split(setCommentSplitRe);
        for (var i = 0; i < lines.length; ++i)
            lines[i] = lines[i]
                .replace(alternateCommentMode ? setCommentAltRe : setCommentRe, "")
                .trim();
        comment.text = lines
            .join("\n")
            .trim();

        comments[line] = comment;
        lastCommentLine = line;
    }

    function isDoubleSlashCommentLine(startOffset) {
        var endOffset = findEndOfLine(startOffset);

        // see if remaining line matches comment pattern
        var lineText = source.substring(startOffset, endOffset);
        var isComment = /^\s*\/\//.test(lineText);
        return isComment;
    }

    function findEndOfLine(cursor) {
        // find end of cursor's line
        var endOffset = cursor;
        while (endOffset < length && charAt(endOffset) !== "\n") {
            endOffset++;
        }
        return endOffset;
    }

    /**
     * Obtains the next token.
     * @returns {string|null} Next token or `null` on eof
     * @inner
     */
    function next() {
        if (stack.length > 0)
            return stack.shift();
        if (stringDelim)
            return readString();
        var repeat,
            prev,
            curr,
            start,
            isDoc,
            isLeadingComment = offset === 0;
        do {
            if (offset === length)
                return null;
            repeat = false;
            while (whitespaceRe.test(curr = charAt(offset))) {
                if (curr === "\n") {
                    isLeadingComment = true;
                    ++line;
                }
                if (++offset === length)
                    return null;
            }

            if (charAt(offset) === "/") {
                if (++offset === length) {
                    throw illegal("comment");
                }
                if (charAt(offset) === "/") { // Line
                    if (!alternateCommentMode) {
                        // check for triple-slash comment
                        isDoc = charAt(start = offset + 1) === "/";

                        while (charAt(++offset) !== "\n") {
                            if (offset === length) {
                                return null;
                            }
                        }
                        ++offset;
                        if (isDoc) {
                            setComment(start, offset - 1, isLeadingComment);
                            // Trailing comment cannot not be multi-line,
                            // so leading comment state should be reset to handle potential next comments
                            isLeadingComment = true;
                        }
                        ++line;
                        repeat = true;
                    } else {
                        // check for double-slash comments, consolidating consecutive lines
                        start = offset;
                        isDoc = false;
                        if (isDoubleSlashCommentLine(offset - 1)) {
                            isDoc = true;
                            do {
                                offset = findEndOfLine(offset);
                                if (offset === length) {
                                    break;
                                }
                                offset++;
                                if (!isLeadingComment) {
                                    // Trailing comment cannot not be multi-line
                                    break;
                                }
                            } while (isDoubleSlashCommentLine(offset));
                        } else {
                            offset = Math.min(length, findEndOfLine(offset) + 1);
                        }
                        if (isDoc) {
                            setComment(start, offset, isLeadingComment);
                            isLeadingComment = true;
                        }
                        line++;
                        repeat = true;
                    }
                } else if ((curr = charAt(offset)) === "*") { /* Block */
                    // check for /** (regular comment mode) or /* (alternate comment mode)
                    start = offset + 1;
                    isDoc = alternateCommentMode || charAt(start) === "*";
                    do {
                        if (curr === "\n") {
                            ++line;
                        }
                        if (++offset === length) {
                            throw illegal("comment");
                        }
                        prev = curr;
                        curr = charAt(offset);
                    } while (prev !== "*" || curr !== "/");
                    ++offset;
                    if (isDoc) {
                        setComment(start, offset - 2, isLeadingComment);
                        isLeadingComment = true;
                    }
                    repeat = true;
                } else {
                    return "/";
                }
            }
        } while (repeat);

        // offset !== length if we got here

        var end = offset;
        delimRe.lastIndex = 0;
        var delim = delimRe.test(charAt(end++));
        if (!delim)
            while (end < length && !delimRe.test(charAt(end)))
                ++end;
        var token = source.substring(offset, offset = end);
        if (token === "\"" || token === "'")
            stringDelim = token;
        return token;
    }

    /**
     * Pushes a token back to the stack.
     * @param {string} token Token
     * @returns {undefined}
     * @inner
     */
    function push(token) {
        stack.push(token);
    }

    /**
     * Peeks for the next token.
     * @returns {string|null} Token or `null` on eof
     * @inner
     */
    function peek() {
        if (!stack.length) {
            var token = next();
            if (token === null)
                return null;
            push(token);
        }
        return stack[0];
    }

    /**
     * Skips a token.
     * @param {string} expected Expected token
     * @param {boolean} [optional=false] Whether the token is optional
     * @returns {boolean} `true` when skipped, `false` if not
     * @throws {Error} When a required token is not present
     * @inner
     */
    function skip(expected, optional) {
        var actual = peek(),
            equals = actual === expected;
        if (equals) {
            next();
            return true;
        }
        if (!optional)
            throw illegal("token '" + actual + "', '" + expected + "' expected");
        return false;
    }

    /**
     * Gets a comment.
     * @param {number} [trailingLine] Line number if looking for a trailing comment
     * @returns {string|null} Comment text
     * @inner
     */
    function cmnt(trailingLine) {
        var ret = null;
        var comment;
        if (trailingLine === undefined) {
            comment = comments[line - 1];
            delete comments[line - 1];
            if (comment && (alternateCommentMode || comment.type === "*" || comment.lineEmpty)) {
                ret = comment.leading ? comment.text : null;
            }
        } else {
            /* istanbul ignore else */
            if (lastCommentLine < trailingLine) {
                peek();
            }
            comment = comments[trailingLine];
            delete comments[trailingLine];
            if (comment && !comment.lineEmpty && (alternateCommentMode || comment.type === "/")) {
                ret = comment.leading ? null : comment.text;
            }
        }
        return ret;
    }

    return Object.defineProperty({
        next: next,
        peek: peek,
        push: push,
        skip: skip,
        cmnt: cmnt
    }, "line", {
        get: function() { return line; }
    });
    /* eslint-enable callback-return */
}


/***/ }),

/***/ "./node_modules/protobufjs/src/type.js":
/*!*********************************************!*\
  !*** ./node_modules/protobufjs/src/type.js ***!
  \*********************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = Type;

// extends Namespace
var Namespace = __webpack_require__(/*! ./namespace */ "./node_modules/protobufjs/src/namespace.js");
((Type.prototype = Object.create(Namespace.prototype)).constructor = Type).className = "Type";

var Enum      = __webpack_require__(/*! ./enum */ "./node_modules/protobufjs/src/enum.js"),
    OneOf     = __webpack_require__(/*! ./oneof */ "./node_modules/protobufjs/src/oneof.js"),
    Field     = __webpack_require__(/*! ./field */ "./node_modules/protobufjs/src/field.js"),
    MapField  = __webpack_require__(/*! ./mapfield */ "./node_modules/protobufjs/src/mapfield.js"),
    Service   = __webpack_require__(/*! ./service */ "./node_modules/protobufjs/src/service.js"),
    Message   = __webpack_require__(/*! ./message */ "./node_modules/protobufjs/src/message.js"),
    Reader    = __webpack_require__(/*! ./reader */ "./node_modules/protobufjs/src/reader.js"),
    Writer    = __webpack_require__(/*! ./writer */ "./node_modules/protobufjs/src/writer.js"),
    util      = __webpack_require__(/*! ./util */ "./node_modules/protobufjs/src/util.js"),
    encoder   = __webpack_require__(/*! ./encoder */ "./node_modules/protobufjs/src/encoder.js"),
    decoder   = __webpack_require__(/*! ./decoder */ "./node_modules/protobufjs/src/decoder.js"),
    verifier  = __webpack_require__(/*! ./verifier */ "./node_modules/protobufjs/src/verifier.js"),
    converter = __webpack_require__(/*! ./converter */ "./node_modules/protobufjs/src/converter.js"),
    wrappers  = __webpack_require__(/*! ./wrappers */ "./node_modules/protobufjs/src/wrappers.js");

/**
 * Constructs a new reflected message type instance.
 * @classdesc Reflected message type.
 * @extends NamespaceBase
 * @constructor
 * @param {string} name Message name
 * @param {Object.<string,*>} [options] Declared options
 */
function Type(name, options) {
    Namespace.call(this, name, options);

    /**
     * Message fields.
     * @type {Object.<string,Field>}
     */
    this.fields = {};  // toJSON, marker

    /**
     * Oneofs declared within this namespace, if any.
     * @type {Object.<string,OneOf>}
     */
    this.oneofs = undefined; // toJSON

    /**
     * Extension ranges, if any.
     * @type {number[][]}
     */
    this.extensions = undefined; // toJSON

    /**
     * Reserved ranges, if any.
     * @type {Array.<number[]|string>}
     */
    this.reserved = undefined; // toJSON

    /*?
     * Whether this type is a legacy group.
     * @type {boolean|undefined}
     */
    this.group = undefined; // toJSON

    /**
     * Cached fields by id.
     * @type {Object.<number,Field>|null}
     * @private
     */
    this._fieldsById = null;

    /**
     * Cached fields as an array.
     * @type {Field[]|null}
     * @private
     */
    this._fieldsArray = null;

    /**
     * Cached oneofs as an array.
     * @type {OneOf[]|null}
     * @private
     */
    this._oneofsArray = null;

    /**
     * Cached constructor.
     * @type {Constructor<{}>}
     * @private
     */
    this._ctor = null;
}

Object.defineProperties(Type.prototype, {

    /**
     * Message fields by id.
     * @name Type#fieldsById
     * @type {Object.<number,Field>}
     * @readonly
     */
    fieldsById: {
        get: function() {

            /* istanbul ignore if */
            if (this._fieldsById)
                return this._fieldsById;

            this._fieldsById = {};
            for (var names = Object.keys(this.fields), i = 0; i < names.length; ++i) {
                var field = this.fields[names[i]],
                    id = field.id;

                /* istanbul ignore if */
                if (this._fieldsById[id])
                    throw Error("duplicate id " + id + " in " + this);

                this._fieldsById[id] = field;
            }
            return this._fieldsById;
        }
    },

    /**
     * Fields of this message as an array for iteration.
     * @name Type#fieldsArray
     * @type {Field[]}
     * @readonly
     */
    fieldsArray: {
        get: function() {
            return this._fieldsArray || (this._fieldsArray = util.toArray(this.fields));
        }
    },

    /**
     * Oneofs of this message as an array for iteration.
     * @name Type#oneofsArray
     * @type {OneOf[]}
     * @readonly
     */
    oneofsArray: {
        get: function() {
            return this._oneofsArray || (this._oneofsArray = util.toArray(this.oneofs));
        }
    },

    /**
     * The registered constructor, if any registered, otherwise a generic constructor.
     * Assigning a function replaces the internal constructor. If the function does not extend {@link Message} yet, its prototype will be setup accordingly and static methods will be populated. If it already extends {@link Message}, it will just replace the internal constructor.
     * @name Type#ctor
     * @type {Constructor<{}>}
     */
    ctor: {
        get: function() {
            return this._ctor || (this.ctor = Type.generateConstructor(this)());
        },
        set: function(ctor) {

            // Ensure proper prototype
            var prototype = ctor.prototype;
            if (!(prototype instanceof Message)) {
                (ctor.prototype = new Message()).constructor = ctor;
                util.merge(ctor.prototype, prototype);
            }

            // Classes and messages reference their reflected type
            ctor.$type = ctor.prototype.$type = this;

            // Mix in static methods
            util.merge(ctor, Message, true);

            this._ctor = ctor;

            // Messages have non-enumerable default values on their prototype
            var i = 0;
            for (; i < /* initializes */ this.fieldsArray.length; ++i)
                this._fieldsArray[i].resolve(); // ensures a proper value

            // Messages have non-enumerable getters and setters for each virtual oneof field
            var ctorProperties = {};
            for (i = 0; i < /* initializes */ this.oneofsArray.length; ++i)
                ctorProperties[this._oneofsArray[i].resolve().name] = {
                    get: util.oneOfGetter(this._oneofsArray[i].oneof),
                    set: util.oneOfSetter(this._oneofsArray[i].oneof)
                };
            if (i)
                Object.defineProperties(ctor.prototype, ctorProperties);
        }
    }
});

/**
 * Generates a constructor function for the specified type.
 * @param {Type} mtype Message type
 * @returns {Codegen} Codegen instance
 */
Type.generateConstructor = function generateConstructor(mtype) {
    /* eslint-disable no-unexpected-multiline */
    var gen = util.codegen(["p"], mtype.name);
    // explicitly initialize mutable object/array fields so that these aren't just inherited from the prototype
    for (var i = 0, field; i < mtype.fieldsArray.length; ++i)
        if ((field = mtype._fieldsArray[i]).map) gen
            ("this%s={}", util.safeProp(field.name));
        else if (field.repeated) gen
            ("this%s=[]", util.safeProp(field.name));
    return gen
    ("if(p)for(var ks=Object.keys(p),i=0;i<ks.length;++i)if(p[ks[i]]!=null)") // omit undefined or null
        ("this[ks[i]]=p[ks[i]]");
    /* eslint-enable no-unexpected-multiline */
};

function clearCache(type) {
    type._fieldsById = type._fieldsArray = type._oneofsArray = null;
    delete type.encode;
    delete type.decode;
    delete type.verify;
    return type;
}

/**
 * Message type descriptor.
 * @interface IType
 * @extends INamespace
 * @property {Object.<string,IOneOf>} [oneofs] Oneof descriptors
 * @property {Object.<string,IField>} fields Field descriptors
 * @property {number[][]} [extensions] Extension ranges
 * @property {Array.<number[]|string>} [reserved] Reserved ranges
 * @property {boolean} [group=false] Whether a legacy group or not
 */

/**
 * Creates a message type from a message type descriptor.
 * @param {string} name Message name
 * @param {IType} json Message type descriptor
 * @returns {Type} Created message type
 */
Type.fromJSON = function fromJSON(name, json) {
    var type = new Type(name, json.options);
    type.extensions = json.extensions;
    type.reserved = json.reserved;
    var names = Object.keys(json.fields),
        i = 0;
    for (; i < names.length; ++i)
        type.add(
            ( typeof json.fields[names[i]].keyType !== "undefined"
            ? MapField.fromJSON
            : Field.fromJSON )(names[i], json.fields[names[i]])
        );
    if (json.oneofs)
        for (names = Object.keys(json.oneofs), i = 0; i < names.length; ++i)
            type.add(OneOf.fromJSON(names[i], json.oneofs[names[i]]));
    if (json.nested)
        for (names = Object.keys(json.nested), i = 0; i < names.length; ++i) {
            var nested = json.nested[names[i]];
            type.add( // most to least likely
                ( nested.id !== undefined
                ? Field.fromJSON
                : nested.fields !== undefined
                ? Type.fromJSON
                : nested.values !== undefined
                ? Enum.fromJSON
                : nested.methods !== undefined
                ? Service.fromJSON
                : Namespace.fromJSON )(names[i], nested)
            );
        }
    if (json.extensions && json.extensions.length)
        type.extensions = json.extensions;
    if (json.reserved && json.reserved.length)
        type.reserved = json.reserved;
    if (json.group)
        type.group = true;
    if (json.comment)
        type.comment = json.comment;
    return type;
};

/**
 * Converts this message type to a message type descriptor.
 * @param {IToJSONOptions} [toJSONOptions] JSON conversion options
 * @returns {IType} Message type descriptor
 */
Type.prototype.toJSON = function toJSON(toJSONOptions) {
    var inherited = Namespace.prototype.toJSON.call(this, toJSONOptions);
    var keepComments = toJSONOptions ? Boolean(toJSONOptions.keepComments) : false;
    return util.toObject([
        "options"    , inherited && inherited.options || undefined,
        "oneofs"     , Namespace.arrayToJSON(this.oneofsArray, toJSONOptions),
        "fields"     , Namespace.arrayToJSON(this.fieldsArray.filter(function(obj) { return !obj.declaringField; }), toJSONOptions) || {},
        "extensions" , this.extensions && this.extensions.length ? this.extensions : undefined,
        "reserved"   , this.reserved && this.reserved.length ? this.reserved : undefined,
        "group"      , this.group || undefined,
        "nested"     , inherited && inherited.nested || undefined,
        "comment"    , keepComments ? this.comment : undefined
    ]);
};

/**
 * @override
 */
Type.prototype.resolveAll = function resolveAll() {
    var fields = this.fieldsArray, i = 0;
    while (i < fields.length)
        fields[i++].resolve();
    var oneofs = this.oneofsArray; i = 0;
    while (i < oneofs.length)
        oneofs[i++].resolve();
    return Namespace.prototype.resolveAll.call(this);
};

/**
 * @override
 */
Type.prototype.get = function get(name) {
    return this.fields[name]
        || this.oneofs && this.oneofs[name]
        || this.nested && this.nested[name]
        || null;
};

/**
 * Adds a nested object to this type.
 * @param {ReflectionObject} object Nested object to add
 * @returns {Type} `this`
 * @throws {TypeError} If arguments are invalid
 * @throws {Error} If there is already a nested object with this name or, if a field, when there is already a field with this id
 */
Type.prototype.add = function add(object) {

    if (this.get(object.name))
        throw Error("duplicate name '" + object.name + "' in " + this);

    if (object instanceof Field && object.extend === undefined) {
        // NOTE: Extension fields aren't actual fields on the declaring type, but nested objects.
        // The root object takes care of adding distinct sister-fields to the respective extended
        // type instead.

        // avoids calling the getter if not absolutely necessary because it's called quite frequently
        if (this._fieldsById ? /* istanbul ignore next */ this._fieldsById[object.id] : this.fieldsById[object.id])
            throw Error("duplicate id " + object.id + " in " + this);
        if (this.isReservedId(object.id))
            throw Error("id " + object.id + " is reserved in " + this);
        if (this.isReservedName(object.name))
            throw Error("name '" + object.name + "' is reserved in " + this);

        if (object.parent)
            object.parent.remove(object);
        this.fields[object.name] = object;
        object.message = this;
        object.onAdd(this);
        return clearCache(this);
    }
    if (object instanceof OneOf) {
        if (!this.oneofs)
            this.oneofs = {};
        this.oneofs[object.name] = object;
        object.onAdd(this);
        return clearCache(this);
    }
    return Namespace.prototype.add.call(this, object);
};

/**
 * Removes a nested object from this type.
 * @param {ReflectionObject} object Nested object to remove
 * @returns {Type} `this`
 * @throws {TypeError} If arguments are invalid
 * @throws {Error} If `object` is not a member of this type
 */
Type.prototype.remove = function remove(object) {
    if (object instanceof Field && object.extend === undefined) {
        // See Type#add for the reason why extension fields are excluded here.

        /* istanbul ignore if */
        if (!this.fields || this.fields[object.name] !== object)
            throw Error(object + " is not a member of " + this);

        delete this.fields[object.name];
        object.parent = null;
        object.onRemove(this);
        return clearCache(this);
    }
    if (object instanceof OneOf) {

        /* istanbul ignore if */
        if (!this.oneofs || this.oneofs[object.name] !== object)
            throw Error(object + " is not a member of " + this);

        delete this.oneofs[object.name];
        object.parent = null;
        object.onRemove(this);
        return clearCache(this);
    }
    return Namespace.prototype.remove.call(this, object);
};

/**
 * Tests if the specified id is reserved.
 * @param {number} id Id to test
 * @returns {boolean} `true` if reserved, otherwise `false`
 */
Type.prototype.isReservedId = function isReservedId(id) {
    return Namespace.isReservedId(this.reserved, id);
};

/**
 * Tests if the specified name is reserved.
 * @param {string} name Name to test
 * @returns {boolean} `true` if reserved, otherwise `false`
 */
Type.prototype.isReservedName = function isReservedName(name) {
    return Namespace.isReservedName(this.reserved, name);
};

/**
 * Creates a new message of this type using the specified properties.
 * @param {Object.<string,*>} [properties] Properties to set
 * @returns {Message<{}>} Message instance
 */
Type.prototype.create = function create(properties) {
    return new this.ctor(properties);
};

/**
 * Sets up {@link Type#encode|encode}, {@link Type#decode|decode} and {@link Type#verify|verify}.
 * @returns {Type} `this`
 */
Type.prototype.setup = function setup() {
    // Sets up everything at once so that the prototype chain does not have to be re-evaluated
    // multiple times (V8, soft-deopt prototype-check).

    var fullName = this.fullName,
        types    = [];
    for (var i = 0; i < /* initializes */ this.fieldsArray.length; ++i)
        types.push(this._fieldsArray[i].resolve().resolvedType);

    // Replace setup methods with type-specific generated functions
    this.encode = encoder(this)({
        Writer : Writer,
        types  : types,
        util   : util
    });
    this.decode = decoder(this)({
        Reader : Reader,
        types  : types,
        util   : util
    });
    this.verify = verifier(this)({
        types : types,
        util  : util
    });
    this.fromObject = converter.fromObject(this)({
        types : types,
        util  : util
    });
    this.toObject = converter.toObject(this)({
        types : types,
        util  : util
    });

    // Inject custom wrappers for common types
    var wrapper = wrappers[fullName];
    if (wrapper) {
        var originalThis = Object.create(this);
        // if (wrapper.fromObject) {
            originalThis.fromObject = this.fromObject;
            this.fromObject = wrapper.fromObject.bind(originalThis);
        // }
        // if (wrapper.toObject) {
            originalThis.toObject = this.toObject;
            this.toObject = wrapper.toObject.bind(originalThis);
        // }
    }

    return this;
};

/**
 * Encodes a message of this type. Does not implicitly {@link Type#verify|verify} messages.
 * @param {Message<{}>|Object.<string,*>} message Message instance or plain object
 * @param {Writer} [writer] Writer to encode to
 * @returns {Writer} writer
 */
Type.prototype.encode = function encode_setup(message, writer) {
    return this.setup().encode(message, writer); // overrides this method
};

/**
 * Encodes a message of this type preceeded by its byte length as a varint. Does not implicitly {@link Type#verify|verify} messages.
 * @param {Message<{}>|Object.<string,*>} message Message instance or plain object
 * @param {Writer} [writer] Writer to encode to
 * @returns {Writer} writer
 */
Type.prototype.encodeDelimited = function encodeDelimited(message, writer) {
    return this.encode(message, writer && writer.len ? writer.fork() : writer).ldelim();
};

/**
 * Decodes a message of this type.
 * @param {Reader|Uint8Array} reader Reader or buffer to decode from
 * @param {number} [length] Length of the message, if known beforehand
 * @returns {Message<{}>} Decoded message
 * @throws {Error} If the payload is not a reader or valid buffer
 * @throws {util.ProtocolError<{}>} If required fields are missing
 */
Type.prototype.decode = function decode_setup(reader, length) {
    return this.setup().decode(reader, length); // overrides this method
};

/**
 * Decodes a message of this type preceeded by its byte length as a varint.
 * @param {Reader|Uint8Array} reader Reader or buffer to decode from
 * @returns {Message<{}>} Decoded message
 * @throws {Error} If the payload is not a reader or valid buffer
 * @throws {util.ProtocolError} If required fields are missing
 */
Type.prototype.decodeDelimited = function decodeDelimited(reader) {
    if (!(reader instanceof Reader))
        reader = Reader.create(reader);
    return this.decode(reader, reader.uint32());
};

/**
 * Verifies that field values are valid and that required fields are present.
 * @param {Object.<string,*>} message Plain object to verify
 * @returns {null|string} `null` if valid, otherwise the reason why it is not
 */
Type.prototype.verify = function verify_setup(message) {
    return this.setup().verify(message); // overrides this method
};

/**
 * Creates a new message of this type from a plain object. Also converts values to their respective internal types.
 * @param {Object.<string,*>} object Plain object to convert
 * @returns {Message<{}>} Message instance
 */
Type.prototype.fromObject = function fromObject(object) {
    return this.setup().fromObject(object);
};

/**
 * Conversion options as used by {@link Type#toObject} and {@link Message.toObject}.
 * @interface IConversionOptions
 * @property {Function} [longs] Long conversion type.
 * Valid values are `String` and `Number` (the global types).
 * Defaults to copy the present value, which is a possibly unsafe number without and a {@link Long} with a long library.
 * @property {Function} [enums] Enum value conversion type.
 * Only valid value is `String` (the global type).
 * Defaults to copy the present value, which is the numeric id.
 * @property {Function} [bytes] Bytes value conversion type.
 * Valid values are `Array` and (a base64 encoded) `String` (the global types).
 * Defaults to copy the present value, which usually is a Buffer under node and an Uint8Array in the browser.
 * @property {boolean} [defaults=false] Also sets default values on the resulting object
 * @property {boolean} [arrays=false] Sets empty arrays for missing repeated fields even if `defaults=false`
 * @property {boolean} [objects=false] Sets empty objects for missing map fields even if `defaults=false`
 * @property {boolean} [oneofs=false] Includes virtual oneof properties set to the present field's name, if any
 * @property {boolean} [json=false] Performs additional JSON compatibility conversions, i.e. NaN and Infinity to strings
 */

/**
 * Creates a plain object from a message of this type. Also converts values to other types if specified.
 * @param {Message<{}>} message Message instance
 * @param {IConversionOptions} [options] Conversion options
 * @returns {Object.<string,*>} Plain object
 */
Type.prototype.toObject = function toObject(message, options) {
    return this.setup().toObject(message, options);
};

/**
 * Decorator function as returned by {@link Type.d} (TypeScript).
 * @typedef TypeDecorator
 * @type {function}
 * @param {Constructor<T>} target Target constructor
 * @returns {undefined}
 * @template T extends Message<T>
 */

/**
 * Type decorator (TypeScript).
 * @param {string} [typeName] Type name, defaults to the constructor's name
 * @returns {TypeDecorator<T>} Decorator function
 * @template T extends Message<T>
 */
Type.d = function decorateType(typeName) {
    return function typeDecorator(target) {
        util.decorateType(target, typeName);
    };
};


/***/ }),

/***/ "./node_modules/protobufjs/src/types.js":
/*!**********************************************!*\
  !*** ./node_modules/protobufjs/src/types.js ***!
  \**********************************************/
/***/ ((__unused_webpack_module, exports, __webpack_require__) => {



/**
 * Common type constants.
 * @namespace
 */
var types = exports;

var util = __webpack_require__(/*! ./util */ "./node_modules/protobufjs/src/util.js");

var s = [
    "double",   // 0
    "float",    // 1
    "int32",    // 2
    "uint32",   // 3
    "sint32",   // 4
    "fixed32",  // 5
    "sfixed32", // 6
    "int64",    // 7
    "uint64",   // 8
    "sint64",   // 9
    "fixed64",  // 10
    "sfixed64", // 11
    "bool",     // 12
    "string",   // 13
    "bytes"     // 14
];

function bake(values, offset) {
    var i = 0, o = {};
    offset |= 0;
    while (i < values.length) o[s[i + offset]] = values[i++];
    return o;
}

/**
 * Basic type wire types.
 * @type {Object.<string,number>}
 * @const
 * @property {number} double=1 Fixed64 wire type
 * @property {number} float=5 Fixed32 wire type
 * @property {number} int32=0 Varint wire type
 * @property {number} uint32=0 Varint wire type
 * @property {number} sint32=0 Varint wire type
 * @property {number} fixed32=5 Fixed32 wire type
 * @property {number} sfixed32=5 Fixed32 wire type
 * @property {number} int64=0 Varint wire type
 * @property {number} uint64=0 Varint wire type
 * @property {number} sint64=0 Varint wire type
 * @property {number} fixed64=1 Fixed64 wire type
 * @property {number} sfixed64=1 Fixed64 wire type
 * @property {number} bool=0 Varint wire type
 * @property {number} string=2 Ldelim wire type
 * @property {number} bytes=2 Ldelim wire type
 */
types.basic = bake([
    /* double   */ 1,
    /* float    */ 5,
    /* int32    */ 0,
    /* uint32   */ 0,
    /* sint32   */ 0,
    /* fixed32  */ 5,
    /* sfixed32 */ 5,
    /* int64    */ 0,
    /* uint64   */ 0,
    /* sint64   */ 0,
    /* fixed64  */ 1,
    /* sfixed64 */ 1,
    /* bool     */ 0,
    /* string   */ 2,
    /* bytes    */ 2
]);

/**
 * Basic type defaults.
 * @type {Object.<string,*>}
 * @const
 * @property {number} double=0 Double default
 * @property {number} float=0 Float default
 * @property {number} int32=0 Int32 default
 * @property {number} uint32=0 Uint32 default
 * @property {number} sint32=0 Sint32 default
 * @property {number} fixed32=0 Fixed32 default
 * @property {number} sfixed32=0 Sfixed32 default
 * @property {number} int64=0 Int64 default
 * @property {number} uint64=0 Uint64 default
 * @property {number} sint64=0 Sint32 default
 * @property {number} fixed64=0 Fixed64 default
 * @property {number} sfixed64=0 Sfixed64 default
 * @property {boolean} bool=false Bool default
 * @property {string} string="" String default
 * @property {Array.<number>} bytes=Array(0) Bytes default
 * @property {null} message=null Message default
 */
types.defaults = bake([
    /* double   */ 0,
    /* float    */ 0,
    /* int32    */ 0,
    /* uint32   */ 0,
    /* sint32   */ 0,
    /* fixed32  */ 0,
    /* sfixed32 */ 0,
    /* int64    */ 0,
    /* uint64   */ 0,
    /* sint64   */ 0,
    /* fixed64  */ 0,
    /* sfixed64 */ 0,
    /* bool     */ false,
    /* string   */ "",
    /* bytes    */ util.emptyArray,
    /* message  */ null
]);

/**
 * Basic long type wire types.
 * @type {Object.<string,number>}
 * @const
 * @property {number} int64=0 Varint wire type
 * @property {number} uint64=0 Varint wire type
 * @property {number} sint64=0 Varint wire type
 * @property {number} fixed64=1 Fixed64 wire type
 * @property {number} sfixed64=1 Fixed64 wire type
 */
types.long = bake([
    /* int64    */ 0,
    /* uint64   */ 0,
    /* sint64   */ 0,
    /* fixed64  */ 1,
    /* sfixed64 */ 1
], 7);

/**
 * Allowed types for map keys with their associated wire type.
 * @type {Object.<string,number>}
 * @const
 * @property {number} int32=0 Varint wire type
 * @property {number} uint32=0 Varint wire type
 * @property {number} sint32=0 Varint wire type
 * @property {number} fixed32=5 Fixed32 wire type
 * @property {number} sfixed32=5 Fixed32 wire type
 * @property {number} int64=0 Varint wire type
 * @property {number} uint64=0 Varint wire type
 * @property {number} sint64=0 Varint wire type
 * @property {number} fixed64=1 Fixed64 wire type
 * @property {number} sfixed64=1 Fixed64 wire type
 * @property {number} bool=0 Varint wire type
 * @property {number} string=2 Ldelim wire type
 */
types.mapKey = bake([
    /* int32    */ 0,
    /* uint32   */ 0,
    /* sint32   */ 0,
    /* fixed32  */ 5,
    /* sfixed32 */ 5,
    /* int64    */ 0,
    /* uint64   */ 0,
    /* sint64   */ 0,
    /* fixed64  */ 1,
    /* sfixed64 */ 1,
    /* bool     */ 0,
    /* string   */ 2
], 2);

/**
 * Allowed types for packed repeated fields with their associated wire type.
 * @type {Object.<string,number>}
 * @const
 * @property {number} double=1 Fixed64 wire type
 * @property {number} float=5 Fixed32 wire type
 * @property {number} int32=0 Varint wire type
 * @property {number} uint32=0 Varint wire type
 * @property {number} sint32=0 Varint wire type
 * @property {number} fixed32=5 Fixed32 wire type
 * @property {number} sfixed32=5 Fixed32 wire type
 * @property {number} int64=0 Varint wire type
 * @property {number} uint64=0 Varint wire type
 * @property {number} sint64=0 Varint wire type
 * @property {number} fixed64=1 Fixed64 wire type
 * @property {number} sfixed64=1 Fixed64 wire type
 * @property {number} bool=0 Varint wire type
 */
types.packed = bake([
    /* double   */ 1,
    /* float    */ 5,
    /* int32    */ 0,
    /* uint32   */ 0,
    /* sint32   */ 0,
    /* fixed32  */ 5,
    /* sfixed32 */ 5,
    /* int64    */ 0,
    /* uint64   */ 0,
    /* sint64   */ 0,
    /* fixed64  */ 1,
    /* sfixed64 */ 1,
    /* bool     */ 0
]);


/***/ }),

/***/ "./node_modules/protobufjs/src/util.js":
/*!*********************************************!*\
  !*** ./node_modules/protobufjs/src/util.js ***!
  \*********************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {



/**
 * Various utility functions.
 * @namespace
 */
var util = module.exports = __webpack_require__(/*! ./util/minimal */ "./node_modules/protobufjs/src/util/minimal.js");

var roots = __webpack_require__(/*! ./roots */ "./node_modules/protobufjs/src/roots.js");

var Type, // cyclic
    Enum;

util.codegen = __webpack_require__(/*! @protobufjs/codegen */ "./node_modules/@protobufjs/codegen/index.js");
util.fetch   = __webpack_require__(/*! @protobufjs/fetch */ "./node_modules/@protobufjs/fetch/index.js");
util.path    = __webpack_require__(/*! @protobufjs/path */ "./node_modules/@protobufjs/path/index.js");

/**
 * Node's fs module if available.
 * @type {Object.<string,*>}
 */
util.fs = util.inquire("fs");

/**
 * Converts an object's values to an array.
 * @param {Object.<string,*>} object Object to convert
 * @returns {Array.<*>} Converted array
 */
util.toArray = function toArray(object) {
    if (object) {
        var keys  = Object.keys(object),
            array = new Array(keys.length),
            index = 0;
        while (index < keys.length)
            array[index] = object[keys[index++]];
        return array;
    }
    return [];
};

/**
 * Converts an array of keys immediately followed by their respective value to an object, omitting undefined values.
 * @param {Array.<*>} array Array to convert
 * @returns {Object.<string,*>} Converted object
 */
util.toObject = function toObject(array) {
    var object = {},
        index  = 0;
    while (index < array.length) {
        var key = array[index++],
            val = array[index++];
        if (val !== undefined)
            object[key] = val;
    }
    return object;
};

var safePropBackslashRe = /\\/g,
    safePropQuoteRe     = /"/g;

/**
 * Tests whether the specified name is a reserved word in JS.
 * @param {string} name Name to test
 * @returns {boolean} `true` if reserved, otherwise `false`
 */
util.isReserved = function isReserved(name) {
    return /^(?:do|if|in|for|let|new|try|var|case|else|enum|eval|false|null|this|true|void|with|break|catch|class|const|super|throw|while|yield|delete|export|import|public|return|static|switch|typeof|default|extends|finally|package|private|continue|debugger|function|arguments|interface|protected|implements|instanceof)$/.test(name);
};

/**
 * Returns a safe property accessor for the specified property name.
 * @param {string} prop Property name
 * @returns {string} Safe accessor
 */
util.safeProp = function safeProp(prop) {
    if (!/^[$\w_]+$/.test(prop) || util.isReserved(prop))
        return "[\"" + prop.replace(safePropBackslashRe, "\\\\").replace(safePropQuoteRe, "\\\"") + "\"]";
    return "." + prop;
};

/**
 * Converts the first character of a string to upper case.
 * @param {string} str String to convert
 * @returns {string} Converted string
 */
util.ucFirst = function ucFirst(str) {
    return str.charAt(0).toUpperCase() + str.substring(1);
};

var camelCaseRe = /_([a-z])/g;

/**
 * Converts a string to camel case.
 * @param {string} str String to convert
 * @returns {string} Converted string
 */
util.camelCase = function camelCase(str) {
    return str.substring(0, 1)
         + str.substring(1)
               .replace(camelCaseRe, function($0, $1) { return $1.toUpperCase(); });
};

/**
 * Compares reflected fields by id.
 * @param {Field} a First field
 * @param {Field} b Second field
 * @returns {number} Comparison value
 */
util.compareFieldsById = function compareFieldsById(a, b) {
    return a.id - b.id;
};

/**
 * Decorator helper for types (TypeScript).
 * @param {Constructor<T>} ctor Constructor function
 * @param {string} [typeName] Type name, defaults to the constructor's name
 * @returns {Type} Reflected type
 * @template T extends Message<T>
 * @property {Root} root Decorators root
 */
util.decorateType = function decorateType(ctor, typeName) {

    /* istanbul ignore if */
    if (ctor.$type) {
        if (typeName && ctor.$type.name !== typeName) {
            util.decorateRoot.remove(ctor.$type);
            ctor.$type.name = typeName;
            util.decorateRoot.add(ctor.$type);
        }
        return ctor.$type;
    }

    /* istanbul ignore next */
    if (!Type)
        Type = __webpack_require__(/*! ./type */ "./node_modules/protobufjs/src/type.js");

    var type = new Type(typeName || ctor.name);
    util.decorateRoot.add(type);
    type.ctor = ctor; // sets up .encode, .decode etc.
    Object.defineProperty(ctor, "$type", { value: type, enumerable: false });
    Object.defineProperty(ctor.prototype, "$type", { value: type, enumerable: false });
    return type;
};

var decorateEnumIndex = 0;

/**
 * Decorator helper for enums (TypeScript).
 * @param {Object} object Enum object
 * @returns {Enum} Reflected enum
 */
util.decorateEnum = function decorateEnum(object) {

    /* istanbul ignore if */
    if (object.$type)
        return object.$type;

    /* istanbul ignore next */
    if (!Enum)
        Enum = __webpack_require__(/*! ./enum */ "./node_modules/protobufjs/src/enum.js");

    var enm = new Enum("Enum" + decorateEnumIndex++, object);
    util.decorateRoot.add(enm);
    Object.defineProperty(object, "$type", { value: enm, enumerable: false });
    return enm;
};


/**
 * Sets the value of a property by property path. If a value already exists, it is turned to an array
 * @param {Object.<string,*>} dst Destination object
 * @param {string} path dot '.' delimited path of the property to set
 * @param {Object} value the value to set
 * @returns {Object.<string,*>} Destination object
 */
util.setProperty = function setProperty(dst, path, value) {
    function setProp(dst, path, value) {
        var part = path.shift();
        if (part === "__proto__" || part === "prototype") {
          return dst;
        }
        if (path.length > 0) {
            dst[part] = setProp(dst[part] || {}, path, value);
        } else {
            var prevValue = dst[part];
            if (prevValue)
                value = [].concat(prevValue).concat(value);
            dst[part] = value;
        }
        return dst;
    }

    if (typeof dst !== "object")
        throw TypeError("dst must be an object");
    if (!path)
        throw TypeError("path must be specified");

    path = path.split(".");
    return setProp(dst, path, value);
};

/**
 * Decorator root (TypeScript).
 * @name util.decorateRoot
 * @type {Root}
 * @readonly
 */
Object.defineProperty(util, "decorateRoot", {
    get: function() {
        return roots["decorated"] || (roots["decorated"] = new (__webpack_require__(/*! ./root */ "./node_modules/protobufjs/src/root.js"))());
    }
});


/***/ }),

/***/ "./node_modules/protobufjs/src/util/longbits.js":
/*!******************************************************!*\
  !*** ./node_modules/protobufjs/src/util/longbits.js ***!
  \******************************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = LongBits;

var util = __webpack_require__(/*! ../util/minimal */ "./node_modules/protobufjs/src/util/minimal.js");

/**
 * Constructs new long bits.
 * @classdesc Helper class for working with the low and high bits of a 64 bit value.
 * @memberof util
 * @constructor
 * @param {number} lo Low 32 bits, unsigned
 * @param {number} hi High 32 bits, unsigned
 */
function LongBits(lo, hi) {

    // note that the casts below are theoretically unnecessary as of today, but older statically
    // generated converter code might still call the ctor with signed 32bits. kept for compat.

    /**
     * Low bits.
     * @type {number}
     */
    this.lo = lo >>> 0;

    /**
     * High bits.
     * @type {number}
     */
    this.hi = hi >>> 0;
}

/**
 * Zero bits.
 * @memberof util.LongBits
 * @type {util.LongBits}
 */
var zero = LongBits.zero = new LongBits(0, 0);

zero.toNumber = function() { return 0; };
zero.zzEncode = zero.zzDecode = function() { return this; };
zero.length = function() { return 1; };

/**
 * Zero hash.
 * @memberof util.LongBits
 * @type {string}
 */
var zeroHash = LongBits.zeroHash = "\0\0\0\0\0\0\0\0";

/**
 * Constructs new long bits from the specified number.
 * @param {number} value Value
 * @returns {util.LongBits} Instance
 */
LongBits.fromNumber = function fromNumber(value) {
    if (value === 0)
        return zero;
    var sign = value < 0;
    if (sign)
        value = -value;
    var lo = value >>> 0,
        hi = (value - lo) / 4294967296 >>> 0;
    if (sign) {
        hi = ~hi >>> 0;
        lo = ~lo >>> 0;
        if (++lo > 4294967295) {
            lo = 0;
            if (++hi > 4294967295)
                hi = 0;
        }
    }
    return new LongBits(lo, hi);
};

/**
 * Constructs new long bits from a number, long or string.
 * @param {Long|number|string} value Value
 * @returns {util.LongBits} Instance
 */
LongBits.from = function from(value) {
    if (typeof value === "number")
        return LongBits.fromNumber(value);
    if (util.isString(value)) {
        /* istanbul ignore else */
        if (util.Long)
            value = util.Long.fromString(value);
        else
            return LongBits.fromNumber(parseInt(value, 10));
    }
    return value.low || value.high ? new LongBits(value.low >>> 0, value.high >>> 0) : zero;
};

/**
 * Converts this long bits to a possibly unsafe JavaScript number.
 * @param {boolean} [unsigned=false] Whether unsigned or not
 * @returns {number} Possibly unsafe number
 */
LongBits.prototype.toNumber = function toNumber(unsigned) {
    if (!unsigned && this.hi >>> 31) {
        var lo = ~this.lo + 1 >>> 0,
            hi = ~this.hi     >>> 0;
        if (!lo)
            hi = hi + 1 >>> 0;
        return -(lo + hi * 4294967296);
    }
    return this.lo + this.hi * 4294967296;
};

/**
 * Converts this long bits to a long.
 * @param {boolean} [unsigned=false] Whether unsigned or not
 * @returns {Long} Long
 */
LongBits.prototype.toLong = function toLong(unsigned) {
    return util.Long
        ? new util.Long(this.lo | 0, this.hi | 0, Boolean(unsigned))
        /* istanbul ignore next */
        : { low: this.lo | 0, high: this.hi | 0, unsigned: Boolean(unsigned) };
};

var charCodeAt = String.prototype.charCodeAt;

/**
 * Constructs new long bits from the specified 8 characters long hash.
 * @param {string} hash Hash
 * @returns {util.LongBits} Bits
 */
LongBits.fromHash = function fromHash(hash) {
    if (hash === zeroHash)
        return zero;
    return new LongBits(
        ( charCodeAt.call(hash, 0)
        | charCodeAt.call(hash, 1) << 8
        | charCodeAt.call(hash, 2) << 16
        | charCodeAt.call(hash, 3) << 24) >>> 0
    ,
        ( charCodeAt.call(hash, 4)
        | charCodeAt.call(hash, 5) << 8
        | charCodeAt.call(hash, 6) << 16
        | charCodeAt.call(hash, 7) << 24) >>> 0
    );
};

/**
 * Converts this long bits to a 8 characters long hash.
 * @returns {string} Hash
 */
LongBits.prototype.toHash = function toHash() {
    return String.fromCharCode(
        this.lo        & 255,
        this.lo >>> 8  & 255,
        this.lo >>> 16 & 255,
        this.lo >>> 24      ,
        this.hi        & 255,
        this.hi >>> 8  & 255,
        this.hi >>> 16 & 255,
        this.hi >>> 24
    );
};

/**
 * Zig-zag encodes this long bits.
 * @returns {util.LongBits} `this`
 */
LongBits.prototype.zzEncode = function zzEncode() {
    var mask =   this.hi >> 31;
    this.hi  = ((this.hi << 1 | this.lo >>> 31) ^ mask) >>> 0;
    this.lo  = ( this.lo << 1                   ^ mask) >>> 0;
    return this;
};

/**
 * Zig-zag decodes this long bits.
 * @returns {util.LongBits} `this`
 */
LongBits.prototype.zzDecode = function zzDecode() {
    var mask = -(this.lo & 1);
    this.lo  = ((this.lo >>> 1 | this.hi << 31) ^ mask) >>> 0;
    this.hi  = ( this.hi >>> 1                  ^ mask) >>> 0;
    return this;
};

/**
 * Calculates the length of this longbits when encoded as a varint.
 * @returns {number} Length
 */
LongBits.prototype.length = function length() {
    var part0 =  this.lo,
        part1 = (this.lo >>> 28 | this.hi << 4) >>> 0,
        part2 =  this.hi >>> 24;
    return part2 === 0
         ? part1 === 0
           ? part0 < 16384
             ? part0 < 128 ? 1 : 2
             : part0 < 2097152 ? 3 : 4
           : part1 < 16384
             ? part1 < 128 ? 5 : 6
             : part1 < 2097152 ? 7 : 8
         : part2 < 128 ? 9 : 10;
};


/***/ }),

/***/ "./node_modules/protobufjs/src/util/minimal.js":
/*!*****************************************************!*\
  !*** ./node_modules/protobufjs/src/util/minimal.js ***!
  \*****************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {


var util = exports;

// used to return a Promise where callback is omitted
util.asPromise = __webpack_require__(/*! @protobufjs/aspromise */ "./node_modules/@protobufjs/aspromise/index.js");

// converts to / from base64 encoded strings
util.base64 = __webpack_require__(/*! @protobufjs/base64 */ "./node_modules/@protobufjs/base64/index.js");

// base class of rpc.Service
util.EventEmitter = __webpack_require__(/*! @protobufjs/eventemitter */ "./node_modules/@protobufjs/eventemitter/index.js");

// float handling accross browsers
util.float = __webpack_require__(/*! @protobufjs/float */ "./node_modules/@protobufjs/float/index.js");

// requires modules optionally and hides the call from bundlers
util.inquire = __webpack_require__(/*! @protobufjs/inquire */ "./node_modules/@protobufjs/inquire/index.js");

// converts to / from utf8 encoded strings
util.utf8 = __webpack_require__(/*! @protobufjs/utf8 */ "./node_modules/@protobufjs/utf8/index.js");

// provides a node-like buffer pool in the browser
util.pool = __webpack_require__(/*! @protobufjs/pool */ "./node_modules/@protobufjs/pool/index.js");

// utility to work with the low and high bits of a 64 bit value
util.LongBits = __webpack_require__(/*! ./longbits */ "./node_modules/protobufjs/src/util/longbits.js");

/**
 * Whether running within node or not.
 * @memberof util
 * @type {boolean}
 */
util.isNode = Boolean(typeof __webpack_require__.g !== "undefined"
                   && __webpack_require__.g
                   && __webpack_require__.g.process
                   && __webpack_require__.g.process.versions
                   && __webpack_require__.g.process.versions.node);

/**
 * Global object reference.
 * @memberof util
 * @type {Object}
 */
util.global = util.isNode && __webpack_require__.g
           || typeof window !== "undefined" && window
           || typeof self   !== "undefined" && self
           || this; // eslint-disable-line no-invalid-this

/**
 * An immuable empty array.
 * @memberof util
 * @type {Array.<*>}
 * @const
 */
util.emptyArray = Object.freeze ? Object.freeze([]) : /* istanbul ignore next */ []; // used on prototypes

/**
 * An immutable empty object.
 * @type {Object}
 * @const
 */
util.emptyObject = Object.freeze ? Object.freeze({}) : /* istanbul ignore next */ {}; // used on prototypes

/**
 * Tests if the specified value is an integer.
 * @function
 * @param {*} value Value to test
 * @returns {boolean} `true` if the value is an integer
 */
util.isInteger = Number.isInteger || /* istanbul ignore next */ function isInteger(value) {
    return typeof value === "number" && isFinite(value) && Math.floor(value) === value;
};

/**
 * Tests if the specified value is a string.
 * @param {*} value Value to test
 * @returns {boolean} `true` if the value is a string
 */
util.isString = function isString(value) {
    return typeof value === "string" || value instanceof String;
};

/**
 * Tests if the specified value is a non-null object.
 * @param {*} value Value to test
 * @returns {boolean} `true` if the value is a non-null object
 */
util.isObject = function isObject(value) {
    return value && typeof value === "object";
};

/**
 * Checks if a property on a message is considered to be present.
 * This is an alias of {@link util.isSet}.
 * @function
 * @param {Object} obj Plain object or message instance
 * @param {string} prop Property name
 * @returns {boolean} `true` if considered to be present, otherwise `false`
 */
util.isset =

/**
 * Checks if a property on a message is considered to be present.
 * @param {Object} obj Plain object or message instance
 * @param {string} prop Property name
 * @returns {boolean} `true` if considered to be present, otherwise `false`
 */
util.isSet = function isSet(obj, prop) {
    var value = obj[prop];
    if (value != null && obj.hasOwnProperty(prop)) // eslint-disable-line eqeqeq, no-prototype-builtins
        return typeof value !== "object" || (Array.isArray(value) ? value.length : Object.keys(value).length) > 0;
    return false;
};

/**
 * Any compatible Buffer instance.
 * This is a minimal stand-alone definition of a Buffer instance. The actual type is that exported by node's typings.
 * @interface Buffer
 * @extends Uint8Array
 */

/**
 * Node's Buffer class if available.
 * @type {Constructor<Buffer>}
 */
util.Buffer = (function() {
    try {
        var Buffer = util.inquire("buffer").Buffer;
        // refuse to use non-node buffers if not explicitly assigned (perf reasons):
        return Buffer.prototype.utf8Write ? Buffer : /* istanbul ignore next */ null;
    } catch (e) {
        /* istanbul ignore next */
        return null;
    }
})();

// Internal alias of or polyfull for Buffer.from.
util._Buffer_from = null;

// Internal alias of or polyfill for Buffer.allocUnsafe.
util._Buffer_allocUnsafe = null;

/**
 * Creates a new buffer of whatever type supported by the environment.
 * @param {number|number[]} [sizeOrArray=0] Buffer size or number array
 * @returns {Uint8Array|Buffer} Buffer
 */
util.newBuffer = function newBuffer(sizeOrArray) {
    /* istanbul ignore next */
    return typeof sizeOrArray === "number"
        ? util.Buffer
            ? util._Buffer_allocUnsafe(sizeOrArray)
            : new util.Array(sizeOrArray)
        : util.Buffer
            ? util._Buffer_from(sizeOrArray)
            : typeof Uint8Array === "undefined"
                ? sizeOrArray
                : new Uint8Array(sizeOrArray);
};

/**
 * Array implementation used in the browser. `Uint8Array` if supported, otherwise `Array`.
 * @type {Constructor<Uint8Array>}
 */
util.Array = typeof Uint8Array !== "undefined" ? Uint8Array /* istanbul ignore next */ : Array;

/**
 * Any compatible Long instance.
 * This is a minimal stand-alone definition of a Long instance. The actual type is that exported by long.js.
 * @interface Long
 * @property {number} low Low bits
 * @property {number} high High bits
 * @property {boolean} unsigned Whether unsigned or not
 */

/**
 * Long.js's Long class if available.
 * @type {Constructor<Long>}
 */
util.Long = /* istanbul ignore next */ util.global.dcodeIO && /* istanbul ignore next */ util.global.dcodeIO.Long
         || /* istanbul ignore next */ util.global.Long
         || util.inquire("long");

/**
 * Regular expression used to verify 2 bit (`bool`) map keys.
 * @type {RegExp}
 * @const
 */
util.key2Re = /^true|false|0|1$/;

/**
 * Regular expression used to verify 32 bit (`int32` etc.) map keys.
 * @type {RegExp}
 * @const
 */
util.key32Re = /^-?(?:0|[1-9][0-9]*)$/;

/**
 * Regular expression used to verify 64 bit (`int64` etc.) map keys.
 * @type {RegExp}
 * @const
 */
util.key64Re = /^(?:[\\x00-\\xff]{8}|-?(?:0|[1-9][0-9]*))$/;

/**
 * Converts a number or long to an 8 characters long hash string.
 * @param {Long|number} value Value to convert
 * @returns {string} Hash
 */
util.longToHash = function longToHash(value) {
    return value
        ? util.LongBits.from(value).toHash()
        : util.LongBits.zeroHash;
};

/**
 * Converts an 8 characters long hash string to a long or number.
 * @param {string} hash Hash
 * @param {boolean} [unsigned=false] Whether unsigned or not
 * @returns {Long|number} Original value
 */
util.longFromHash = function longFromHash(hash, unsigned) {
    var bits = util.LongBits.fromHash(hash);
    if (util.Long)
        return util.Long.fromBits(bits.lo, bits.hi, unsigned);
    return bits.toNumber(Boolean(unsigned));
};

/**
 * Merges the properties of the source object into the destination object.
 * @memberof util
 * @param {Object.<string,*>} dst Destination object
 * @param {Object.<string,*>} src Source object
 * @param {boolean} [ifNotSet=false] Merges only if the key is not already set
 * @returns {Object.<string,*>} Destination object
 */
function merge(dst, src, ifNotSet) { // used by converters
    for (var keys = Object.keys(src), i = 0; i < keys.length; ++i)
        if (dst[keys[i]] === undefined || !ifNotSet)
            dst[keys[i]] = src[keys[i]];
    return dst;
}

util.merge = merge;

/**
 * Converts the first character of a string to lower case.
 * @param {string} str String to convert
 * @returns {string} Converted string
 */
util.lcFirst = function lcFirst(str) {
    return str.charAt(0).toLowerCase() + str.substring(1);
};

/**
 * Creates a custom error constructor.
 * @memberof util
 * @param {string} name Error name
 * @returns {Constructor<Error>} Custom error constructor
 */
function newError(name) {

    function CustomError(message, properties) {

        if (!(this instanceof CustomError))
            return new CustomError(message, properties);

        // Error.call(this, message);
        // ^ just returns a new error instance because the ctor can be called as a function

        Object.defineProperty(this, "message", { get: function() { return message; } });

        /* istanbul ignore next */
        if (Error.captureStackTrace) // node
            Error.captureStackTrace(this, CustomError);
        else
            Object.defineProperty(this, "stack", { value: new Error().stack || "" });

        if (properties)
            merge(this, properties);
    }

    CustomError.prototype = Object.create(Error.prototype, {
        constructor: {
            value: CustomError,
            writable: true,
            enumerable: false,
            configurable: true,
        },
        name: {
            get: function get() { return name; },
            set: undefined,
            enumerable: false,
            // configurable: false would accurately preserve the behavior of
            // the original, but I'm guessing that was not intentional.
            // For an actual error subclass, this property would
            // be configurable.
            configurable: true,
        },
        toString: {
            value: function value() { return this.name + ": " + this.message; },
            writable: true,
            enumerable: false,
            configurable: true,
        },
    });

    return CustomError;
}

util.newError = newError;

/**
 * Constructs a new protocol error.
 * @classdesc Error subclass indicating a protocol specifc error.
 * @memberof util
 * @extends Error
 * @template T extends Message<T>
 * @constructor
 * @param {string} message Error message
 * @param {Object.<string,*>} [properties] Additional properties
 * @example
 * try {
 *     MyMessage.decode(someBuffer); // throws if required fields are missing
 * } catch (e) {
 *     if (e instanceof ProtocolError && e.instance)
 *         console.log("decoded so far: " + JSON.stringify(e.instance));
 * }
 */
util.ProtocolError = newError("ProtocolError");

/**
 * So far decoded message instance.
 * @name util.ProtocolError#instance
 * @type {Message<T>}
 */

/**
 * A OneOf getter as returned by {@link util.oneOfGetter}.
 * @typedef OneOfGetter
 * @type {function}
 * @returns {string|undefined} Set field name, if any
 */

/**
 * Builds a getter for a oneof's present field name.
 * @param {string[]} fieldNames Field names
 * @returns {OneOfGetter} Unbound getter
 */
util.oneOfGetter = function getOneOf(fieldNames) {
    var fieldMap = {};
    for (var i = 0; i < fieldNames.length; ++i)
        fieldMap[fieldNames[i]] = 1;

    /**
     * @returns {string|undefined} Set field name, if any
     * @this Object
     * @ignore
     */
    return function() { // eslint-disable-line consistent-return
        for (var keys = Object.keys(this), i = keys.length - 1; i > -1; --i)
            if (fieldMap[keys[i]] === 1 && this[keys[i]] !== undefined && this[keys[i]] !== null)
                return keys[i];
    };
};

/**
 * A OneOf setter as returned by {@link util.oneOfSetter}.
 * @typedef OneOfSetter
 * @type {function}
 * @param {string|undefined} value Field name
 * @returns {undefined}
 */

/**
 * Builds a setter for a oneof's present field name.
 * @param {string[]} fieldNames Field names
 * @returns {OneOfSetter} Unbound setter
 */
util.oneOfSetter = function setOneOf(fieldNames) {

    /**
     * @param {string} name Field name
     * @returns {undefined}
     * @this Object
     * @ignore
     */
    return function(name) {
        for (var i = 0; i < fieldNames.length; ++i)
            if (fieldNames[i] !== name)
                delete this[fieldNames[i]];
    };
};

/**
 * Default conversion options used for {@link Message#toJSON} implementations.
 *
 * These options are close to proto3's JSON mapping with the exception that internal types like Any are handled just like messages. More precisely:
 *
 * - Longs become strings
 * - Enums become string keys
 * - Bytes become base64 encoded strings
 * - (Sub-)Messages become plain objects
 * - Maps become plain objects with all string keys
 * - Repeated fields become arrays
 * - NaN and Infinity for float and double fields become strings
 *
 * @type {IConversionOptions}
 * @see https://developers.google.com/protocol-buffers/docs/proto3?hl=en#json
 */
util.toJSONOptions = {
    longs: String,
    enums: String,
    bytes: String,
    json: true
};

// Sets up buffer utility according to the environment (called in index-minimal)
util._configure = function() {
    var Buffer = util.Buffer;
    /* istanbul ignore if */
    if (!Buffer) {
        util._Buffer_from = util._Buffer_allocUnsafe = null;
        return;
    }
    // because node 4.x buffers are incompatible & immutable
    // see: https://github.com/dcodeIO/protobuf.js/pull/665
    util._Buffer_from = Buffer.from !== Uint8Array.from && Buffer.from ||
        /* istanbul ignore next */
        function Buffer_from(value, encoding) {
            return new Buffer(value, encoding);
        };
    util._Buffer_allocUnsafe = Buffer.allocUnsafe ||
        /* istanbul ignore next */
        function Buffer_allocUnsafe(size) {
            return new Buffer(size);
        };
};


/***/ }),

/***/ "./node_modules/protobufjs/src/verifier.js":
/*!*************************************************!*\
  !*** ./node_modules/protobufjs/src/verifier.js ***!
  \*************************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = verifier;

var Enum      = __webpack_require__(/*! ./enum */ "./node_modules/protobufjs/src/enum.js"),
    util      = __webpack_require__(/*! ./util */ "./node_modules/protobufjs/src/util.js");

function invalid(field, expected) {
    return field.name + ": " + expected + (field.repeated && expected !== "array" ? "[]" : field.map && expected !== "object" ? "{k:"+field.keyType+"}" : "") + " expected";
}

/**
 * Generates a partial value verifier.
 * @param {Codegen} gen Codegen instance
 * @param {Field} field Reflected field
 * @param {number} fieldIndex Field index
 * @param {string} ref Variable reference
 * @returns {Codegen} Codegen instance
 * @ignore
 */
function genVerifyValue(gen, field, fieldIndex, ref) {
    /* eslint-disable no-unexpected-multiline */
    if (field.resolvedType) {
        if (field.resolvedType instanceof Enum) { gen
            ("switch(%s){", ref)
                ("default:")
                    ("return%j", invalid(field, "enum value"));
            for (var keys = Object.keys(field.resolvedType.values), j = 0; j < keys.length; ++j) gen
                ("case %i:", field.resolvedType.values[keys[j]]);
            gen
                    ("break")
            ("}");
        } else {
            gen
            ("{")
                ("var e=types[%i].verify(%s);", fieldIndex, ref)
                ("if(e)")
                    ("return%j+e", field.name + ".")
            ("}");
        }
    } else {
        switch (field.type) {
            case "int32":
            case "uint32":
            case "sint32":
            case "fixed32":
            case "sfixed32": gen
                ("if(!util.isInteger(%s))", ref)
                    ("return%j", invalid(field, "integer"));
                break;
            case "int64":
            case "uint64":
            case "sint64":
            case "fixed64":
            case "sfixed64": gen
                ("if(!util.isInteger(%s)&&!(%s&&util.isInteger(%s.low)&&util.isInteger(%s.high)))", ref, ref, ref, ref)
                    ("return%j", invalid(field, "integer|Long"));
                break;
            case "float":
            case "double": gen
                ("if(typeof %s!==\"number\")", ref)
                    ("return%j", invalid(field, "number"));
                break;
            case "bool": gen
                ("if(typeof %s!==\"boolean\")", ref)
                    ("return%j", invalid(field, "boolean"));
                break;
            case "string": gen
                ("if(!util.isString(%s))", ref)
                    ("return%j", invalid(field, "string"));
                break;
            case "bytes": gen
                ("if(!(%s&&typeof %s.length===\"number\"||util.isString(%s)))", ref, ref, ref)
                    ("return%j", invalid(field, "buffer"));
                break;
        }
    }
    return gen;
    /* eslint-enable no-unexpected-multiline */
}

/**
 * Generates a partial key verifier.
 * @param {Codegen} gen Codegen instance
 * @param {Field} field Reflected field
 * @param {string} ref Variable reference
 * @returns {Codegen} Codegen instance
 * @ignore
 */
function genVerifyKey(gen, field, ref) {
    /* eslint-disable no-unexpected-multiline */
    switch (field.keyType) {
        case "int32":
        case "uint32":
        case "sint32":
        case "fixed32":
        case "sfixed32": gen
            ("if(!util.key32Re.test(%s))", ref)
                ("return%j", invalid(field, "integer key"));
            break;
        case "int64":
        case "uint64":
        case "sint64":
        case "fixed64":
        case "sfixed64": gen
            ("if(!util.key64Re.test(%s))", ref) // see comment above: x is ok, d is not
                ("return%j", invalid(field, "integer|Long key"));
            break;
        case "bool": gen
            ("if(!util.key2Re.test(%s))", ref)
                ("return%j", invalid(field, "boolean key"));
            break;
    }
    return gen;
    /* eslint-enable no-unexpected-multiline */
}

/**
 * Generates a verifier specific to the specified message type.
 * @param {Type} mtype Message type
 * @returns {Codegen} Codegen instance
 */
function verifier(mtype) {
    /* eslint-disable no-unexpected-multiline */

    var gen = util.codegen(["m"], mtype.name + "$verify")
    ("if(typeof m!==\"object\"||m===null)")
        ("return%j", "object expected");
    var oneofs = mtype.oneofsArray,
        seenFirstField = {};
    if (oneofs.length) gen
    ("var p={}");

    for (var i = 0; i < /* initializes */ mtype.fieldsArray.length; ++i) {
        var field = mtype._fieldsArray[i].resolve(),
            ref   = "m" + util.safeProp(field.name);

        if (field.optional) gen
        ("if(%s!=null&&m.hasOwnProperty(%j)){", ref, field.name); // !== undefined && !== null

        // map fields
        if (field.map) { gen
            ("if(!util.isObject(%s))", ref)
                ("return%j", invalid(field, "object"))
            ("var k=Object.keys(%s)", ref)
            ("for(var i=0;i<k.length;++i){");
                genVerifyKey(gen, field, "k[i]");
                genVerifyValue(gen, field, i, ref + "[k[i]]")
            ("}");

        // repeated fields
        } else if (field.repeated) { gen
            ("if(!Array.isArray(%s))", ref)
                ("return%j", invalid(field, "array"))
            ("for(var i=0;i<%s.length;++i){", ref);
                genVerifyValue(gen, field, i, ref + "[i]")
            ("}");

        // required or present fields
        } else {
            if (field.partOf) {
                var oneofProp = util.safeProp(field.partOf.name);
                if (seenFirstField[field.partOf.name] === 1) gen
            ("if(p%s===1)", oneofProp)
                ("return%j", field.partOf.name + ": multiple values");
                seenFirstField[field.partOf.name] = 1;
                gen
            ("p%s=1", oneofProp);
            }
            genVerifyValue(gen, field, i, ref);
        }
        if (field.optional) gen
        ("}");
    }
    return gen
    ("return null");
    /* eslint-enable no-unexpected-multiline */
}

/***/ }),

/***/ "./node_modules/protobufjs/src/wrappers.js":
/*!*************************************************!*\
  !*** ./node_modules/protobufjs/src/wrappers.js ***!
  \*************************************************/
/***/ ((__unused_webpack_module, exports, __webpack_require__) => {



/**
 * Wrappers for common types.
 * @type {Object.<string,IWrapper>}
 * @const
 */
var wrappers = exports;

var Message = __webpack_require__(/*! ./message */ "./node_modules/protobufjs/src/message.js");

/**
 * From object converter part of an {@link IWrapper}.
 * @typedef WrapperFromObjectConverter
 * @type {function}
 * @param {Object.<string,*>} object Plain object
 * @returns {Message<{}>} Message instance
 * @this Type
 */

/**
 * To object converter part of an {@link IWrapper}.
 * @typedef WrapperToObjectConverter
 * @type {function}
 * @param {Message<{}>} message Message instance
 * @param {IConversionOptions} [options] Conversion options
 * @returns {Object.<string,*>} Plain object
 * @this Type
 */

/**
 * Common type wrapper part of {@link wrappers}.
 * @interface IWrapper
 * @property {WrapperFromObjectConverter} [fromObject] From object converter
 * @property {WrapperToObjectConverter} [toObject] To object converter
 */

// Custom wrapper for Any
wrappers[".google.protobuf.Any"] = {

    fromObject: function(object) {

        // unwrap value type if mapped
        if (object && object["@type"]) {
             // Only use fully qualified type name after the last '/'
            var name = object["@type"].substring(object["@type"].lastIndexOf("/") + 1);
            var type = this.lookup(name);
            /* istanbul ignore else */
            if (type) {
                // type_url does not accept leading "."
                var type_url = object["@type"].charAt(0) === "." ?
                    object["@type"].slice(1) : object["@type"];
                // type_url prefix is optional, but path seperator is required
                if (type_url.indexOf("/") === -1) {
                    type_url = "/" + type_url;
                }
                return this.create({
                    type_url: type_url,
                    value: type.encode(type.fromObject(object)).finish()
                });
            }
        }

        return this.fromObject(object);
    },

    toObject: function(message, options) {

        // Default prefix
        var googleApi = "type.googleapis.com/";
        var prefix = "";
        var name = "";

        // decode value if requested and unmapped
        if (options && options.json && message.type_url && message.value) {
            // Only use fully qualified type name after the last '/'
            name = message.type_url.substring(message.type_url.lastIndexOf("/") + 1);
            // Separate the prefix used
            prefix = message.type_url.substring(0, message.type_url.lastIndexOf("/") + 1);
            var type = this.lookup(name);
            /* istanbul ignore else */
            if (type)
                message = type.decode(message.value);
        }

        // wrap value if unmapped
        if (!(message instanceof this.ctor) && message instanceof Message) {
            var object = message.$type.toObject(message, options);
            var messageName = message.$type.fullName[0] === "." ?
                message.$type.fullName.slice(1) : message.$type.fullName;
            // Default to type.googleapis.com prefix if no prefix is used
            if (prefix === "") {
                prefix = googleApi;
            }
            name = prefix + messageName;
            object["@type"] = name;
            return object;
        }

        return this.toObject(message, options);
    }
};


/***/ }),

/***/ "./node_modules/protobufjs/src/writer.js":
/*!***********************************************!*\
  !*** ./node_modules/protobufjs/src/writer.js ***!
  \***********************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = Writer;

var util      = __webpack_require__(/*! ./util/minimal */ "./node_modules/protobufjs/src/util/minimal.js");

var BufferWriter; // cyclic

var LongBits  = util.LongBits,
    base64    = util.base64,
    utf8      = util.utf8;

/**
 * Constructs a new writer operation instance.
 * @classdesc Scheduled writer operation.
 * @constructor
 * @param {function(*, Uint8Array, number)} fn Function to call
 * @param {number} len Value byte length
 * @param {*} val Value to write
 * @ignore
 */
function Op(fn, len, val) {

    /**
     * Function to call.
     * @type {function(Uint8Array, number, *)}
     */
    this.fn = fn;

    /**
     * Value byte length.
     * @type {number}
     */
    this.len = len;

    /**
     * Next operation.
     * @type {Writer.Op|undefined}
     */
    this.next = undefined;

    /**
     * Value to write.
     * @type {*}
     */
    this.val = val; // type varies
}

/* istanbul ignore next */
function noop() {} // eslint-disable-line no-empty-function

/**
 * Constructs a new writer state instance.
 * @classdesc Copied writer state.
 * @memberof Writer
 * @constructor
 * @param {Writer} writer Writer to copy state from
 * @ignore
 */
function State(writer) {

    /**
     * Current head.
     * @type {Writer.Op}
     */
    this.head = writer.head;

    /**
     * Current tail.
     * @type {Writer.Op}
     */
    this.tail = writer.tail;

    /**
     * Current buffer length.
     * @type {number}
     */
    this.len = writer.len;

    /**
     * Next state.
     * @type {State|null}
     */
    this.next = writer.states;
}

/**
 * Constructs a new writer instance.
 * @classdesc Wire format writer using `Uint8Array` if available, otherwise `Array`.
 * @constructor
 */
function Writer() {

    /**
     * Current length.
     * @type {number}
     */
    this.len = 0;

    /**
     * Operations head.
     * @type {Object}
     */
    this.head = new Op(noop, 0, 0);

    /**
     * Operations tail
     * @type {Object}
     */
    this.tail = this.head;

    /**
     * Linked forked states.
     * @type {Object|null}
     */
    this.states = null;

    // When a value is written, the writer calculates its byte length and puts it into a linked
    // list of operations to perform when finish() is called. This both allows us to allocate
    // buffers of the exact required size and reduces the amount of work we have to do compared
    // to first calculating over objects and then encoding over objects. In our case, the encoding
    // part is just a linked list walk calling operations with already prepared values.
}

var create = function create() {
    return util.Buffer
        ? function create_buffer_setup() {
            return (Writer.create = function create_buffer() {
                return new BufferWriter();
            })();
        }
        /* istanbul ignore next */
        : function create_array() {
            return new Writer();
        };
};

/**
 * Creates a new writer.
 * @function
 * @returns {BufferWriter|Writer} A {@link BufferWriter} when Buffers are supported, otherwise a {@link Writer}
 */
Writer.create = create();

/**
 * Allocates a buffer of the specified size.
 * @param {number} size Buffer size
 * @returns {Uint8Array} Buffer
 */
Writer.alloc = function alloc(size) {
    return new util.Array(size);
};

// Use Uint8Array buffer pool in the browser, just like node does with buffers
/* istanbul ignore else */
if (util.Array !== Array)
    Writer.alloc = util.pool(Writer.alloc, util.Array.prototype.subarray);

/**
 * Pushes a new operation to the queue.
 * @param {function(Uint8Array, number, *)} fn Function to call
 * @param {number} len Value byte length
 * @param {number} val Value to write
 * @returns {Writer} `this`
 * @private
 */
Writer.prototype._push = function push(fn, len, val) {
    this.tail = this.tail.next = new Op(fn, len, val);
    this.len += len;
    return this;
};

function writeByte(val, buf, pos) {
    buf[pos] = val & 255;
}

function writeVarint32(val, buf, pos) {
    while (val > 127) {
        buf[pos++] = val & 127 | 128;
        val >>>= 7;
    }
    buf[pos] = val;
}

/**
 * Constructs a new varint writer operation instance.
 * @classdesc Scheduled varint writer operation.
 * @extends Op
 * @constructor
 * @param {number} len Value byte length
 * @param {number} val Value to write
 * @ignore
 */
function VarintOp(len, val) {
    this.len = len;
    this.next = undefined;
    this.val = val;
}

VarintOp.prototype = Object.create(Op.prototype);
VarintOp.prototype.fn = writeVarint32;

/**
 * Writes an unsigned 32 bit value as a varint.
 * @param {number} value Value to write
 * @returns {Writer} `this`
 */
Writer.prototype.uint32 = function write_uint32(value) {
    // here, the call to this.push has been inlined and a varint specific Op subclass is used.
    // uint32 is by far the most frequently used operation and benefits significantly from this.
    this.len += (this.tail = this.tail.next = new VarintOp(
        (value = value >>> 0)
                < 128       ? 1
        : value < 16384     ? 2
        : value < 2097152   ? 3
        : value < 268435456 ? 4
        :                     5,
    value)).len;
    return this;
};

/**
 * Writes a signed 32 bit value as a varint.
 * @function
 * @param {number} value Value to write
 * @returns {Writer} `this`
 */
Writer.prototype.int32 = function write_int32(value) {
    return value < 0
        ? this._push(writeVarint64, 10, LongBits.fromNumber(value)) // 10 bytes per spec
        : this.uint32(value);
};

/**
 * Writes a 32 bit value as a varint, zig-zag encoded.
 * @param {number} value Value to write
 * @returns {Writer} `this`
 */
Writer.prototype.sint32 = function write_sint32(value) {
    return this.uint32((value << 1 ^ value >> 31) >>> 0);
};

function writeVarint64(val, buf, pos) {
    while (val.hi) {
        buf[pos++] = val.lo & 127 | 128;
        val.lo = (val.lo >>> 7 | val.hi << 25) >>> 0;
        val.hi >>>= 7;
    }
    while (val.lo > 127) {
        buf[pos++] = val.lo & 127 | 128;
        val.lo = val.lo >>> 7;
    }
    buf[pos++] = val.lo;
}

/**
 * Writes an unsigned 64 bit value as a varint.
 * @param {Long|number|string} value Value to write
 * @returns {Writer} `this`
 * @throws {TypeError} If `value` is a string and no long library is present.
 */
Writer.prototype.uint64 = function write_uint64(value) {
    var bits = LongBits.from(value);
    return this._push(writeVarint64, bits.length(), bits);
};

/**
 * Writes a signed 64 bit value as a varint.
 * @function
 * @param {Long|number|string} value Value to write
 * @returns {Writer} `this`
 * @throws {TypeError} If `value` is a string and no long library is present.
 */
Writer.prototype.int64 = Writer.prototype.uint64;

/**
 * Writes a signed 64 bit value as a varint, zig-zag encoded.
 * @param {Long|number|string} value Value to write
 * @returns {Writer} `this`
 * @throws {TypeError} If `value` is a string and no long library is present.
 */
Writer.prototype.sint64 = function write_sint64(value) {
    var bits = LongBits.from(value).zzEncode();
    return this._push(writeVarint64, bits.length(), bits);
};

/**
 * Writes a boolish value as a varint.
 * @param {boolean} value Value to write
 * @returns {Writer} `this`
 */
Writer.prototype.bool = function write_bool(value) {
    return this._push(writeByte, 1, value ? 1 : 0);
};

function writeFixed32(val, buf, pos) {
    buf[pos    ] =  val         & 255;
    buf[pos + 1] =  val >>> 8   & 255;
    buf[pos + 2] =  val >>> 16  & 255;
    buf[pos + 3] =  val >>> 24;
}

/**
 * Writes an unsigned 32 bit value as fixed 32 bits.
 * @param {number} value Value to write
 * @returns {Writer} `this`
 */
Writer.prototype.fixed32 = function write_fixed32(value) {
    return this._push(writeFixed32, 4, value >>> 0);
};

/**
 * Writes a signed 32 bit value as fixed 32 bits.
 * @function
 * @param {number} value Value to write
 * @returns {Writer} `this`
 */
Writer.prototype.sfixed32 = Writer.prototype.fixed32;

/**
 * Writes an unsigned 64 bit value as fixed 64 bits.
 * @param {Long|number|string} value Value to write
 * @returns {Writer} `this`
 * @throws {TypeError} If `value` is a string and no long library is present.
 */
Writer.prototype.fixed64 = function write_fixed64(value) {
    var bits = LongBits.from(value);
    return this._push(writeFixed32, 4, bits.lo)._push(writeFixed32, 4, bits.hi);
};

/**
 * Writes a signed 64 bit value as fixed 64 bits.
 * @function
 * @param {Long|number|string} value Value to write
 * @returns {Writer} `this`
 * @throws {TypeError} If `value` is a string and no long library is present.
 */
Writer.prototype.sfixed64 = Writer.prototype.fixed64;

/**
 * Writes a float (32 bit).
 * @function
 * @param {number} value Value to write
 * @returns {Writer} `this`
 */
Writer.prototype.float = function write_float(value) {
    return this._push(util.float.writeFloatLE, 4, value);
};

/**
 * Writes a double (64 bit float).
 * @function
 * @param {number} value Value to write
 * @returns {Writer} `this`
 */
Writer.prototype.double = function write_double(value) {
    return this._push(util.float.writeDoubleLE, 8, value);
};

var writeBytes = util.Array.prototype.set
    ? function writeBytes_set(val, buf, pos) {
        buf.set(val, pos); // also works for plain array values
    }
    /* istanbul ignore next */
    : function writeBytes_for(val, buf, pos) {
        for (var i = 0; i < val.length; ++i)
            buf[pos + i] = val[i];
    };

/**
 * Writes a sequence of bytes.
 * @param {Uint8Array|string} value Buffer or base64 encoded string to write
 * @returns {Writer} `this`
 */
Writer.prototype.bytes = function write_bytes(value) {
    var len = value.length >>> 0;
    if (!len)
        return this._push(writeByte, 1, 0);
    if (util.isString(value)) {
        var buf = Writer.alloc(len = base64.length(value));
        base64.decode(value, buf, 0);
        value = buf;
    }
    return this.uint32(len)._push(writeBytes, len, value);
};

/**
 * Writes a string.
 * @param {string} value Value to write
 * @returns {Writer} `this`
 */
Writer.prototype.string = function write_string(value) {
    var len = utf8.length(value);
    return len
        ? this.uint32(len)._push(utf8.write, len, value)
        : this._push(writeByte, 1, 0);
};

/**
 * Forks this writer's state by pushing it to a stack.
 * Calling {@link Writer#reset|reset} or {@link Writer#ldelim|ldelim} resets the writer to the previous state.
 * @returns {Writer} `this`
 */
Writer.prototype.fork = function fork() {
    this.states = new State(this);
    this.head = this.tail = new Op(noop, 0, 0);
    this.len = 0;
    return this;
};

/**
 * Resets this instance to the last state.
 * @returns {Writer} `this`
 */
Writer.prototype.reset = function reset() {
    if (this.states) {
        this.head   = this.states.head;
        this.tail   = this.states.tail;
        this.len    = this.states.len;
        this.states = this.states.next;
    } else {
        this.head = this.tail = new Op(noop, 0, 0);
        this.len  = 0;
    }
    return this;
};

/**
 * Resets to the last state and appends the fork state's current write length as a varint followed by its operations.
 * @returns {Writer} `this`
 */
Writer.prototype.ldelim = function ldelim() {
    var head = this.head,
        tail = this.tail,
        len  = this.len;
    this.reset().uint32(len);
    if (len) {
        this.tail.next = head.next; // skip noop
        this.tail = tail;
        this.len += len;
    }
    return this;
};

/**
 * Finishes the write operation.
 * @returns {Uint8Array} Finished buffer
 */
Writer.prototype.finish = function finish() {
    var head = this.head.next, // skip noop
        buf  = this.constructor.alloc(this.len),
        pos  = 0;
    while (head) {
        head.fn(head.val, buf, pos);
        pos += head.len;
        head = head.next;
    }
    // this.head = this.tail = null;
    return buf;
};

Writer._configure = function(BufferWriter_) {
    BufferWriter = BufferWriter_;
    Writer.create = create();
    BufferWriter._configure();
};


/***/ }),

/***/ "./node_modules/protobufjs/src/writer_buffer.js":
/*!******************************************************!*\
  !*** ./node_modules/protobufjs/src/writer_buffer.js ***!
  \******************************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {


module.exports = BufferWriter;

// extends Writer
var Writer = __webpack_require__(/*! ./writer */ "./node_modules/protobufjs/src/writer.js");
(BufferWriter.prototype = Object.create(Writer.prototype)).constructor = BufferWriter;

var util = __webpack_require__(/*! ./util/minimal */ "./node_modules/protobufjs/src/util/minimal.js");

/**
 * Constructs a new buffer writer instance.
 * @classdesc Wire format writer using node buffers.
 * @extends Writer
 * @constructor
 */
function BufferWriter() {
    Writer.call(this);
}

BufferWriter._configure = function () {
    /**
     * Allocates a buffer of the specified size.
     * @function
     * @param {number} size Buffer size
     * @returns {Buffer} Buffer
     */
    BufferWriter.alloc = util._Buffer_allocUnsafe;

    BufferWriter.writeBytesBuffer = util.Buffer && util.Buffer.prototype instanceof Uint8Array && util.Buffer.prototype.set.name === "set"
        ? function writeBytesBuffer_set(val, buf, pos) {
          buf.set(val, pos); // faster than copy (requires node >= 4 where Buffers extend Uint8Array and set is properly inherited)
          // also works for plain array values
        }
        /* istanbul ignore next */
        : function writeBytesBuffer_copy(val, buf, pos) {
          if (val.copy) // Buffer values
            val.copy(buf, pos, 0, val.length);
          else for (var i = 0; i < val.length;) // plain array values
            buf[pos++] = val[i++];
        };
};


/**
 * @override
 */
BufferWriter.prototype.bytes = function write_bytes_buffer(value) {
    if (util.isString(value))
        value = util._Buffer_from(value, "base64");
    var len = value.length >>> 0;
    this.uint32(len);
    if (len)
        this._push(BufferWriter.writeBytesBuffer, len, value);
    return this;
};

function writeStringBuffer(val, buf, pos) {
    if (val.length < 40) // plain js is faster for short strings (probably due to redundant assertions)
        util.utf8.write(val, buf, pos);
    else if (buf.utf8Write)
        buf.utf8Write(val, pos);
    else
        buf.write(val, pos);
}

/**
 * @override
 */
BufferWriter.prototype.string = function write_string_buffer(value) {
    var len = util.Buffer.byteLength(value);
    this.uint32(len);
    if (len)
        this._push(writeStringBuffer, len, value);
    return this;
};


/**
 * Finishes the write operation.
 * @name BufferWriter#finish
 * @function
 * @returns {Buffer} Finished buffer
 */

BufferWriter._configure();


/***/ }),

/***/ "./src/config.ts":
/*!***********************!*\
  !*** ./src/config.ts ***!
  \***********************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   WEBSOCKET_SERVER_URL: () => (/* binding */ WEBSOCKET_SERVER_URL)
/* harmony export */ });
var WEBSOCKET_SERVER_URL = "wss://local.galacticodyssey.space/gaos/ws";


/***/ }),

/***/ "./src/dispatcher.ts":
/*!***************************!*\
  !*** ./src/dispatcher.ts ***!
  \***************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   CLASS_ID_Authenticate: () => (/* binding */ CLASS_ID_Authenticate),
/* harmony export */   CLASS_ID_BaseMessages: () => (/* binding */ CLASS_ID_BaseMessages),
/* harmony export */   Dispatcher: () => (/* binding */ Dispatcher),
/* harmony export */   METHOD_ID_AuthenticateRequest: () => (/* binding */ METHOD_ID_AuthenticateRequest),
/* harmony export */   METHOD_ID_AuthenticateResponse: () => (/* binding */ METHOD_ID_AuthenticateResponse),
/* harmony export */   METHOD_ID_ReceiveString: () => (/* binding */ METHOD_ID_ReceiveString),
/* harmony export */   NAMESPACE_ID__UnityBrowserChannel: () => (/* binding */ NAMESPACE_ID__UnityBrowserChannel),
/* harmony export */   NAMESPACE_ID__WebSocket: () => (/* binding */ NAMESPACE_ID__WebSocket)
/* harmony export */ });
/* harmony import */ var _messages_unityBrowserMessaging_BaseMessages__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./messages/unityBrowserMessaging/BaseMessages */ "./src/messages/unityBrowserMessaging/BaseMessages.ts");
/* harmony import */ var _messages_WsAuthentication__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./messages/WsAuthentication */ "./src/messages/WsAuthentication.ts");


var FILE = 'dispatcher.ts';
var NAMESPACE_ID__WebSocket = 1;
var CLASS_ID_Authenticate = 2;
var METHOD_ID_AuthenticateRequest = 1;
var METHOD_ID_AuthenticateResponse = 2;
var NAMESPACE_ID__UnityBrowserChannel = 2;
var CLASS_ID_BaseMessages = 1;
var METHOD_ID_ReceiveString = 1;
var Dispatcher = /** @class */ (function () {
    function Dispatcher(pbRoot, wsClient) {
        this.pbRoot = pbRoot;
        this.wsClient = wsClient;
        this.pbMessageHeader = pbRoot.root.lookupType('GaoProtobuf.MessageHeader');
    }
    Dispatcher.readMessageObjectSize = function (message, offset) {
        var view = new DataView(message);
        return view.getUint32(offset, false);
    };
    Dispatcher.encodeMessageObjectSize = function (size) {
        var data = new ArrayBuffer(4);
        var view = new DataView(data);
        view.setUint32(0, size, false);
        return data;
    };
    Dispatcher.readMessageObject = function (message, offset, pbMessageObject) {
        var FUNC = 'readMessageObject()';
        try {
            var headerSize = Dispatcher.readMessageObjectSize(message, offset);
            var data = new Uint8Array(message, offset + 4, headerSize);
            var moMessageObject = pbMessageObject.decode(data);
            return { moMessageObject: moMessageObject, size: 4 + headerSize };
        }
        catch (err) {
            console.error("".concat(FILE, ":").concat(FUNC, ": error"), err);
            throw new Error('readMessageHeader failed');
        }
    };
    Dispatcher.encodeMessageObject = function (pbMessageObject, moMessageObject) {
        var FUNC = 'encodeMessageObject()';
        try {
            var dataMo = pbMessageObject.encode(moMessageObject).finish();
            var dataMoSize = Dispatcher.encodeMessageObjectSize(dataMo.length);
            var data = new Uint8Array(dataMoSize.byteLength + dataMo.length);
            // serialize message object size to view
            data.set(new Uint8Array(dataMoSize), 0);
            // serialize message object to view
            data.set(dataMo, dataMoSize.byteLength);
            return data.buffer;
        }
        catch (err) {
            console.error(FILE, FUNC, err);
            throw new Error('encodeMessageObject() failed');
        }
    };
    Dispatcher.prototype._dispatch = function (message, offset) {
        var _a = Dispatcher.readMessageObject(message, offset, this.pbMessageHeader), moMessageHeader = _a.moMessageObject, size = _a.size;
        offset += size;
        this.__dispatch(message, offset, moMessageHeader);
    };
    Dispatcher.prototype.__dispatch = function (message, offset, moMessageHeader) {
        var FUNC = '__dispatch()';
        if (moMessageHeader.namespaceId === NAMESPACE_ID__UnityBrowserChannel) {
            if (moMessageHeader.classId === CLASS_ID_BaseMessages) {
                if (moMessageHeader.methodId === METHOD_ID_ReceiveString) {
                    var pbMessage = this.pbRoot.root.lookupType('GaoProtobuf.StringMessage');
                    var _a = Dispatcher.readMessageObject(message, offset, pbMessage), moMessage = _a.moMessageObject, size = _a.size;
                    _messages_unityBrowserMessaging_BaseMessages__WEBPACK_IMPORTED_MODULE_0__.BaseMessages.receiveString(moMessage.str);
                    offset += size;
                }
                else {
                    console.warn("".concat(FILE, ":").concat(FUNC, ": unknown methodId: ").concat(moMessageHeader.methodId));
                }
            }
            else {
                console.warn("".concat(FILE, ":").concat(FUNC, ": unknown classId: ").concat(moMessageHeader.classId));
            }
        }
        else if (moMessageHeader.namespaceId === NAMESPACE_ID__WebSocket) {
            if (moMessageHeader.classId === CLASS_ID_Authenticate) {
                if (moMessageHeader.methodId === METHOD_ID_AuthenticateResponse) {
                    var pbAuthenticateResponse = this.pbRoot.root.lookupType('GaoProtobuf.AuthenticateResponse');
                    var _b = Dispatcher.readMessageObject(message, offset, pbAuthenticateResponse), moMessage = _b.moMessageObject, size = _b.size;
                    _messages_WsAuthentication__WEBPACK_IMPORTED_MODULE_1__.WsAuthentication.receiveAuthenticateResponse(moMessage, this.pbRoot.root.lookupEnum('GaoProtobuf.AuthenticationResult'));
                }
                else {
                    console.warn("".concat(FILE, ":").concat(FUNC, ": unknown methodId: ").concat(moMessageHeader.methodId));
                }
            }
            else {
                console.warn("".concat(FILE, ":").concat(FUNC, ": unknown classId: ").concat(moMessageHeader.classId));
            }
        }
        else {
            console.warn("".concat(FILE, ":").concat(FUNC, ": unknown namespaceId: ").concat(moMessageHeader.namespaceId));
        }
        this.disposeRequests();
    };
    Dispatcher.prototype.dispatch = function (message) {
        try {
            this._dispatch(message, 0);
        }
        catch (err) {
            console.error("".concat(FILE, ":dispatch(): ").concat(err), err);
            throw new Error('dispatch() failed');
        }
    };
    Dispatcher.prototype.disposeRequests = function () {
        // dispose requests
        _messages_WsAuthentication__WEBPACK_IMPORTED_MODULE_1__.WsAuthentication.diposeRequests();
    };
    return Dispatcher;
}());



/***/ }),

/***/ "./src/events.ts":
/*!***********************!*\
  !*** ./src/events.ts ***!
  \***********************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   emitUnityMessageEvent: () => (/* binding */ emitUnityMessageEvent)
/* harmony export */ });
function emitUnityMessageEvent(json) {
    var event = new CustomEvent("unity_message", { detail: json });
    document.dispatchEvent(event);
}


/***/ }),

/***/ "./src/messages/WsAuthentication.ts":
/*!******************************************!*\
  !*** ./src/messages/WsAuthentication.ts ***!
  \******************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   WsAuthentication: () => (/* binding */ WsAuthentication)
/* harmony export */ });
/* harmony import */ var _wsClient__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../wsClient */ "./src/wsClient.ts");
/* harmony import */ var _dispatcher__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../dispatcher */ "./src/dispatcher.ts");
/* harmony import */ var uuid__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! uuid */ "./node_modules/uuid/dist/esm-browser/v4.js");




var AuthenticationStatus;
(function (AuthenticationStatus) {
    AuthenticationStatus[AuthenticationStatus["UNAUTHENTICATED"] = 0] = "UNAUTHENTICATED";
    AuthenticationStatus[AuthenticationStatus["AUTHENTICATING"] = 1] = "AUTHENTICATING";
    AuthenticationStatus[AuthenticationStatus["AUTHENTICATED"] = 2] = "AUTHENTICATED";
    AuthenticationStatus[AuthenticationStatus["ERROR"] = 3] = "ERROR";
})(AuthenticationStatus || (AuthenticationStatus = {}));
var WsAuthentication = /** @class */ (function () {
    function WsAuthentication(wsClient) {
        this.authenticationStatus = AuthenticationStatus.UNAUTHENTICATED;
        this.wsClient = wsClient;
    }
    WsAuthentication.authenticate = function (wsClient, token) {
        var FUNC = 'authenticate()';
        console.log("".concat(WsAuthentication.CLASS, ":").concat(FUNC, ": authenticating..."));
        var wsAuthentication = new WsAuthentication(wsClient);
        try {
            wsAuthentication.authenticationStatus = AuthenticationStatus.AUTHENTICATING;
            var pbMessageHeader = _wsClient__WEBPACK_IMPORTED_MODULE_0__.WebSocketClient.gPbRoot.root.lookupType('GaoProtobuf.MessageHeader');
            var pbAuthenticateRequest = _wsClient__WEBPACK_IMPORTED_MODULE_0__.WebSocketClient.gPbRoot.root.lookupType('GaoProtobuf.AuthenticateRequest');
            var moMessageHeader = pbMessageHeader.create({ namespaceId: _dispatcher__WEBPACK_IMPORTED_MODULE_1__.NAMESPACE_ID__WebSocket, classId: _dispatcher__WEBPACK_IMPORTED_MODULE_1__.CLASS_ID_Authenticate, methodId: _dispatcher__WEBPACK_IMPORTED_MODULE_1__.METHOD_ID_AuthenticateRequest });
            var moAuthenticateRequest = pbAuthenticateRequest.create({
                token: token,
                requestId: (0,uuid__WEBPACK_IMPORTED_MODULE_2__["default"])()
            });
            wsAuthentication.requestStartAt = new Date();
            WsAuthentication.requests[moAuthenticateRequest.requestId] = wsAuthentication;
            // encode message header
            var dataMessageHeader = _dispatcher__WEBPACK_IMPORTED_MODULE_1__.Dispatcher.encodeMessageObject(pbMessageHeader, moMessageHeader);
            var dataAuthenticateRequest = _dispatcher__WEBPACK_IMPORTED_MODULE_1__.Dispatcher.encodeMessageObject(pbAuthenticateRequest, moAuthenticateRequest);
            // concatenate message header and message string
            var data = new Uint8Array(dataMessageHeader.byteLength + dataAuthenticateRequest.byteLength);
            data.set(new Uint8Array(dataMessageHeader), 0);
            data.set(new Uint8Array(dataAuthenticateRequest), dataMessageHeader.byteLength);
            wsClient.send(moMessageHeader, data.buffer);
        }
        catch (err) {
            wsAuthentication.authenticationStatus = AuthenticationStatus.ERROR;
            console.error("".concat(WsAuthentication.CLASS, ":").concat(FUNC, ": ").concat(err));
        }
    };
    WsAuthentication.receiveAuthenticateResponse = function (moAuthenticateReqponse, pbAuthenticationResultEnum) {
        var FUNC = 'receiveAuthenticateResponse()';
        var requestId = moAuthenticateReqponse.requestId;
        // find the request
        var request = WsAuthentication.requests[requestId];
        if (request) {
            if (moAuthenticateReqponse.result == pbAuthenticationResultEnum.values.success) {
                request.authenticationStatus = AuthenticationStatus.AUTHENTICATED;
                console.log("".concat(WsAuthentication.CLASS, ":").concat(FUNC, ": athenticated"));
                request.wsClient.setAuthenticated();
            }
            else if (moAuthenticateReqponse.result == pbAuthenticationResultEnum.values.unauthorized) {
                request.authenticationStatus = AuthenticationStatus.UNAUTHENTICATED;
            }
            else if (moAuthenticateReqponse.result == pbAuthenticationResultEnum.values.error) {
                request.authenticationStatus = AuthenticationStatus.ERROR;
            }
            else {
                console.warn("".concat(WsAuthentication.CLASS, ":").concat(FUNC, ": unknown result: ").concat(moAuthenticateReqponse.result));
            }
            delete WsAuthentication.requests[requestId];
        }
        else {
            console.warn("".concat(WsAuthentication.CLASS, ":").concat(FUNC, ": request not found"));
        }
    };
    WsAuthentication.diposeRequests = function () {
        var FUNC = 'diposeRequests()';
        // dispose requests that are older than 10 seconds or have status other than AUTHENTICATING
        var now = new Date();
        for (var key in WsAuthentication.requests) {
            var request = WsAuthentication.requests[key];
            if (now.getTime() - request.requestStartAt.getTime() > 10000) {
                if (request.authenticationStatus != AuthenticationStatus.AUTHENTICATING) {
                    console.warn("".concat(WsAuthentication.CLASS, ":").concat(FUNC, ": request timed out: ").concat(key));
                }
                delete WsAuthentication.requests[key];
            }
        }
    };
    WsAuthentication.CLASS = 'WsAuthentication';
    WsAuthentication.requests = {};
    return WsAuthentication;
}());



/***/ }),

/***/ "./src/messages/unityBrowserMessaging/BaseMessages.ts":
/*!************************************************************!*\
  !*** ./src/messages/unityBrowserMessaging/BaseMessages.ts ***!
  \************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   BaseMessages: () => (/* binding */ BaseMessages)
/* harmony export */ });
/* harmony import */ var _wsClient__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../wsClient */ "./src/wsClient.ts");
/* harmony import */ var _dispatcher__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../dispatcher */ "./src/dispatcher.ts");
/* harmony import */ var _events__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../../events */ "./src/events.ts");




var BaseMessages = /** @class */ (function () {
    function BaseMessages() {
    }
    BaseMessages.receiveString = function (str) {
        var FUNC = 'receiveString()';
        console.log("".concat(BaseMessages.CLASS_NAME, ":").concat(FUNC, ": in browser: ").concat(str));
        try {
            var json = JSON.parse(str);
            (0,_events__WEBPACK_IMPORTED_MODULE_2__.emitUnityMessageEvent)(json);
        }
        catch (err) {
            console.error("".concat(BaseMessages.CLASS_NAME, ":").concat(FUNC, ": in browser: ").concat(err));
        }
    };
    BaseMessages.sendString = function (str) {
        var FUNC = 'sendString()';
        try {
            if (window.GAO_UnityBrowserChannel) {
                // unity is running in browser,call unity directly
                try {
                    if (!window.GAO_UnityBrowserChannel) {
                        throw new Error('window.GAO_UnityBrowserChannel is not defined');
                    }
                    if (!window.GAO_UnityBrowserChannel.BaseMessages) {
                        throw new Error('window.GAO_UnityBrowserChannel.BaseMessages is not defined');
                    }
                    if (!window.GAO_UnityBrowserChannel.BaseMessages.sendString) {
                        throw new Error('window.GAO_UnityBrowserChannel.BaseMessages.sendString is not defined');
                    }
                    window.GAO_UnityBrowserChannel.BaseMessages.sendString(str);
                }
                catch (err) {
                    console.error("".concat(BaseMessages.CLASS_NAME, ":").concat(FUNC, ": in browser: error sending directly, ").concat(err));
                }
            }
            else {
                try {
                    // unity is running in editor, call wsClient
                    var dispatcher = _wsClient__WEBPACK_IMPORTED_MODULE_0__.WebSocketClient.gWsClient.dispatcher;
                    var pbMessageHeader = _wsClient__WEBPACK_IMPORTED_MODULE_0__.WebSocketClient.gPbRoot.root.lookupType('GaoProtobuf.MessageHeader');
                    var pbStringMessage = _wsClient__WEBPACK_IMPORTED_MODULE_0__.WebSocketClient.gPbRoot.root.lookupType('GaoProtobuf.StringMessage');
                    var moMessageHeader = pbMessageHeader.create({ namespaceId: _dispatcher__WEBPACK_IMPORTED_MODULE_1__.NAMESPACE_ID__UnityBrowserChannel, classId: _dispatcher__WEBPACK_IMPORTED_MODULE_1__.CLASS_ID_BaseMessages, methodId: _dispatcher__WEBPACK_IMPORTED_MODULE_1__.METHOD_ID_ReceiveString });
                    var moStringMessage = pbStringMessage.create({ str: str });
                    // encode message header
                    var dataMessageHeader = _dispatcher__WEBPACK_IMPORTED_MODULE_1__.Dispatcher.encodeMessageObject(pbMessageHeader, moMessageHeader);
                    var dataStringMessage = _dispatcher__WEBPACK_IMPORTED_MODULE_1__.Dispatcher.encodeMessageObject(pbStringMessage, moStringMessage);
                    // concatenate message header and message string
                    var data = new Uint8Array(dataMessageHeader.byteLength + dataStringMessage.byteLength);
                    data.set(new Uint8Array(dataMessageHeader), 0);
                    data.set(new Uint8Array(dataStringMessage), dataMessageHeader.byteLength);
                    _wsClient__WEBPACK_IMPORTED_MODULE_0__.WebSocketClient.gWsClient.send(moMessageHeader, data.buffer);
                }
                catch (err) {
                    console.error("".concat(BaseMessages.CLASS_NAME, ":").concat(FUNC, ": in browser: error sending via wsClient, ").concat(err));
                }
            }
        }
        catch (err) {
            console.error("".concat(BaseMessages.CLASS_NAME, ":").concat(FUNC, ": in browser: ").concat(err));
        }
    };
    BaseMessages.CLASS_NAME = 'BaseMessages';
    return BaseMessages;
}());



/***/ }),

/***/ "./src/proto.ts":
/*!**********************!*\
  !*** ./src/proto.ts ***!
  \**********************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   ProtobufRoot: () => (/* binding */ ProtobufRoot)
/* harmony export */ });
/* harmony import */ var _bundle_json__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./bundle.json */ "./src/bundle.json");
/* harmony import */ var protobufjs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! protobufjs */ "./node_modules/protobufjs/index.js");
/* harmony import */ var protobufjs__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(protobufjs__WEBPACK_IMPORTED_MODULE_1__);


/*
export interface IProtobufRoot {
    root: any;
}


export function getProtobufRoot(): IProtobufRoot {
    return{
        root: protobuf.Root.fromJSON(ptotobufJsonDescriptor)
    }
}
    */
var ProtobufRoot = /** @class */ (function () {
    function ProtobufRoot() {
        this.root = protobufjs__WEBPACK_IMPORTED_MODULE_1__.Root.fromJSON(_bundle_json__WEBPACK_IMPORTED_MODULE_0__);
    }
    return ProtobufRoot;
}());



/***/ }),

/***/ "./src/wsClient.ts":
/*!*************************!*\
  !*** ./src/wsClient.ts ***!
  \*************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   WebSocketClient: () => (/* binding */ WebSocketClient)
/* harmony export */ });
/* harmony import */ var _proto__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./proto */ "./src/proto.ts");
/* harmony import */ var _dispatcher__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./dispatcher */ "./src/dispatcher.ts");
/* harmony import */ var _messages_WsAuthentication__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./messages/WsAuthentication */ "./src/messages/WsAuthentication.ts");
/* harmony import */ var _config__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./config */ "./src/config.ts");





var WebSocketClient = /** @class */ (function () {
    function WebSocketClient() {
        this.dispatcher = null;
        this.isAuthenticated = false;
    }
    WebSocketClient.setJwtToken = function (token) {
        WebSocketClient.jwtToken = token;
        if (WebSocketClient.gWsClient) {
            _messages_WsAuthentication__WEBPACK_IMPORTED_MODULE_2__.WsAuthentication.authenticate(WebSocketClient.gWsClient, WebSocketClient.jwtToken);
        }
    };
    WebSocketClient.prototype.start = function () {
        var _this = this;
        this.ws = new WebSocket(_config__WEBPACK_IMPORTED_MODULE_3__.WEBSOCKET_SERVER_URL);
        this.ws.binaryType = 'arraybuffer';
        this.isAuthenticated = false;
        this.ws.onopen = function () {
            console.log('connected');
            _this.dispatcher = new _dispatcher__WEBPACK_IMPORTED_MODULE_1__.Dispatcher(WebSocketClient.gPbRoot, WebSocketClient.gWsClient);
            if (WebSocketClient.jwtToken) {
                _messages_WsAuthentication__WEBPACK_IMPORTED_MODULE_2__.WsAuthentication.authenticate(_this, WebSocketClient.jwtToken);
            }
        };
        this.ws.onmessage = function (e) {
            var FUNC = 'onmessage()';
            if (e.data instanceof ArrayBuffer) {
                if (_this.dispatcher) {
                    _this.dispatcher.dispatch(e.data);
                }
                else {
                    console.warn("".concat(WebSocketClient.CLASS, ":").concat(FUNC, ": dispatcher not ready, message ignored"));
                }
            }
            else {
                console.error("".concat(WebSocketClient.CLASS, ":").concat(FUNC, ": received a non-arraybuffer message, ignored"));
            }
        };
        this.ws.onclose = function () {
            console.log('disconnected');
            setTimeout(function () {
                WebSocketClient.gWsClient = new WebSocketClient();
                WebSocketClient.gWsClient.start();
            }, 9000);
        };
    };
    ;
    WebSocketClient.prototype.checkIfAuthenticatedToSend = function (moMessageHeader) {
        if (moMessageHeader.namespaceId === _dispatcher__WEBPACK_IMPORTED_MODULE_1__.NAMESPACE_ID__UnityBrowserChannel) {
            return this.isAuthenticated;
        }
        else {
            return true;
        }
    };
    WebSocketClient.prototype.send = function (moMessageHeader, message) {
        var FUNC = 'send()';
        if (!this.checkIfAuthenticatedToSend(moMessageHeader)) {
            console.warn("".concat(WebSocketClient.CLASS, ":").concat(FUNC, ": cannot send a message, websocket is not authenticated, message ignored"));
            return;
        }
        if (this.ws.readyState !== WebSocket.OPEN) {
            console.warn("".concat(WebSocketClient.CLASS, ":").concat(FUNC, ": ws is not open, message ignored"));
            return;
        }
        this.ws.send(message);
    };
    WebSocketClient.prototype.setAuthenticated = function () {
        this.isAuthenticated = true;
    };
    WebSocketClient.prototype.getIsAuthenticated = function () {
        return this.isAuthenticated;
    };
    WebSocketClient.CLASS = 'WebSocketClient';
    WebSocketClient.gPbRoot = new _proto__WEBPACK_IMPORTED_MODULE_0__.ProtobufRoot();
    WebSocketClient.gWsClient = new WebSocketClient();
    WebSocketClient.jwtToken = null;
    return WebSocketClient;
}());



/***/ }),

/***/ "./node_modules/uuid/dist/esm-browser/native.js":
/*!******************************************************!*\
  !*** ./node_modules/uuid/dist/esm-browser/native.js ***!
  \******************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
var randomUUID = typeof crypto !== 'undefined' && crypto.randomUUID && crypto.randomUUID.bind(crypto);
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({
  randomUUID
});

/***/ }),

/***/ "./node_modules/uuid/dist/esm-browser/regex.js":
/*!*****************************************************!*\
  !*** ./node_modules/uuid/dist/esm-browser/regex.js ***!
  \*****************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (/^(?:[0-9a-f]{8}-[0-9a-f]{4}-[1-8][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}|00000000-0000-0000-0000-000000000000|ffffffff-ffff-ffff-ffff-ffffffffffff)$/i);

/***/ }),

/***/ "./node_modules/uuid/dist/esm-browser/rng.js":
/*!***************************************************!*\
  !*** ./node_modules/uuid/dist/esm-browser/rng.js ***!
  \***************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (/* binding */ rng)
/* harmony export */ });
// Unique ID creation requires a high quality random # generator. In the browser we therefore
// require the crypto API and do not support built-in fallback to lower quality random number
// generators (like Math.random()).

var getRandomValues;
var rnds8 = new Uint8Array(16);
function rng() {
  // lazy load so that environments that need to polyfill have a chance to do so
  if (!getRandomValues) {
    // getRandomValues needs to be invoked in a context where "this" is a Crypto implementation.
    getRandomValues = typeof crypto !== 'undefined' && crypto.getRandomValues && crypto.getRandomValues.bind(crypto);
    if (!getRandomValues) {
      throw new Error('crypto.getRandomValues() not supported. See https://github.com/uuidjs/uuid#getrandomvalues-not-supported');
    }
  }
  return getRandomValues(rnds8);
}

/***/ }),

/***/ "./node_modules/uuid/dist/esm-browser/stringify.js":
/*!*********************************************************!*\
  !*** ./node_modules/uuid/dist/esm-browser/stringify.js ***!
  \*********************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__),
/* harmony export */   unsafeStringify: () => (/* binding */ unsafeStringify)
/* harmony export */ });
/* harmony import */ var _validate_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./validate.js */ "./node_modules/uuid/dist/esm-browser/validate.js");


/**
 * Convert array of 16 byte values to UUID string format of the form:
 * XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX
 */
var byteToHex = [];
for (var i = 0; i < 256; ++i) {
  byteToHex.push((i + 0x100).toString(16).slice(1));
}
function unsafeStringify(arr, offset = 0) {
  // Note: Be careful editing this code!  It's been tuned for performance
  // and works in ways you may not expect. See https://github.com/uuidjs/uuid/pull/434
  //
  // Note to future-self: No, you can't remove the `toLowerCase()` call.
  // REF: https://github.com/uuidjs/uuid/pull/677#issuecomment-1757351351
  return (byteToHex[arr[offset + 0]] + byteToHex[arr[offset + 1]] + byteToHex[arr[offset + 2]] + byteToHex[arr[offset + 3]] + '-' + byteToHex[arr[offset + 4]] + byteToHex[arr[offset + 5]] + '-' + byteToHex[arr[offset + 6]] + byteToHex[arr[offset + 7]] + '-' + byteToHex[arr[offset + 8]] + byteToHex[arr[offset + 9]] + '-' + byteToHex[arr[offset + 10]] + byteToHex[arr[offset + 11]] + byteToHex[arr[offset + 12]] + byteToHex[arr[offset + 13]] + byteToHex[arr[offset + 14]] + byteToHex[arr[offset + 15]]).toLowerCase();
}
function stringify(arr, offset = 0) {
  var uuid = unsafeStringify(arr, offset);
  // Consistency check for valid UUID.  If this throws, it's likely due to one
  // of the following:
  // - One or more input array values don't map to a hex octet (leading to
  // "undefined" in the uuid)
  // - Invalid input values for the RFC `version` or `variant` fields
  if (!(0,_validate_js__WEBPACK_IMPORTED_MODULE_0__["default"])(uuid)) {
    throw TypeError('Stringified UUID is invalid');
  }
  return uuid;
}
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (stringify);

/***/ }),

/***/ "./node_modules/uuid/dist/esm-browser/v4.js":
/*!**************************************************!*\
  !*** ./node_modules/uuid/dist/esm-browser/v4.js ***!
  \**************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _native_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./native.js */ "./node_modules/uuid/dist/esm-browser/native.js");
/* harmony import */ var _rng_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./rng.js */ "./node_modules/uuid/dist/esm-browser/rng.js");
/* harmony import */ var _stringify_js__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./stringify.js */ "./node_modules/uuid/dist/esm-browser/stringify.js");



function v4(options, buf, offset) {
  if (_native_js__WEBPACK_IMPORTED_MODULE_0__["default"].randomUUID && !buf && !options) {
    return _native_js__WEBPACK_IMPORTED_MODULE_0__["default"].randomUUID();
  }
  options = options || {};
  var rnds = options.random || (options.rng || _rng_js__WEBPACK_IMPORTED_MODULE_1__["default"])();

  // Per 4.4, set bits for version and `clock_seq_hi_and_reserved`
  rnds[6] = rnds[6] & 0x0f | 0x40;
  rnds[8] = rnds[8] & 0x3f | 0x80;

  // Copy bytes to buffer, if provided
  if (buf) {
    offset = offset || 0;
    for (var i = 0; i < 16; ++i) {
      buf[offset + i] = rnds[i];
    }
    return buf;
  }
  return (0,_stringify_js__WEBPACK_IMPORTED_MODULE_2__.unsafeStringify)(rnds);
}
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (v4);

/***/ }),

/***/ "./node_modules/uuid/dist/esm-browser/validate.js":
/*!********************************************************!*\
  !*** ./node_modules/uuid/dist/esm-browser/validate.js ***!
  \********************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _regex_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./regex.js */ "./node_modules/uuid/dist/esm-browser/regex.js");

function validate(uuid) {
  return typeof uuid === 'string' && _regex_js__WEBPACK_IMPORTED_MODULE_0__["default"].test(uuid);
}
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (validate);

/***/ }),

/***/ "./src/bundle.json":
/*!*************************!*\
  !*** ./src/bundle.json ***!
  \*************************/
/***/ ((module) => {

module.exports = /*#__PURE__*/JSON.parse('{"nested":{"GaoProtobuf":{"options":{"csharp_namespace":"GaoProtobuf"},"nested":{"MessageHeader":{"fields":{"fromId":{"type":"int64","id":1},"toId":{"type":"int64","id":2},"groupId":{"type":"int64","id":3},"typeId":{"type":"int32","id":4},"namespaceId":{"type":"int32","id":5},"classId":{"type":"int32","id":6},"methodId":{"type":"int32","id":7}}},"StringMessage":{"fields":{"str":{"type":"string","id":1}}},"AuthenticateRequest":{"fields":{"token":{"type":"string","id":1},"requestId":{"type":"string","id":2}}},"AuthenticationResult":{"values":{"success":0,"unauthorized":1,"error":2}},"AuthenticateResponse":{"fields":{"result":{"type":"AuthenticationResult","id":1},"requestId":{"type":"string","id":2}}}}}}}');

/***/ })

/******/ 	});
/************************************************************************/
/******/ 	// The module cache
/******/ 	var __webpack_module_cache__ = {};
/******/ 	
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/ 		// Check if module is in cache
/******/ 		var cachedModule = __webpack_module_cache__[moduleId];
/******/ 		if (cachedModule !== undefined) {
/******/ 			return cachedModule.exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = __webpack_module_cache__[moduleId] = {
/******/ 			// no module.id needed
/******/ 			// no module.loaded needed
/******/ 			exports: {}
/******/ 		};
/******/ 	
/******/ 		// Execute the module function
/******/ 		__webpack_modules__[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/ 	
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/compat get default export */
/******/ 	(() => {
/******/ 		// getDefaultExport function for compatibility with non-harmony modules
/******/ 		__webpack_require__.n = (module) => {
/******/ 			var getter = module && module.__esModule ?
/******/ 				() => (module['default']) :
/******/ 				() => (module);
/******/ 			__webpack_require__.d(getter, { a: getter });
/******/ 			return getter;
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/global */
/******/ 	(() => {
/******/ 		__webpack_require__.g = (function() {
/******/ 			if (typeof globalThis === 'object') return globalThis;
/******/ 			try {
/******/ 				return this || new Function('return this')();
/******/ 			} catch (e) {
/******/ 				if (typeof window === 'object') return window;
/******/ 			}
/******/ 		})();
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	})();
/******/ 	
/************************************************************************/
var __webpack_exports__ = {};
/*!***************************!*\
  !*** ./src/indexJsLib.ts ***!
  \***************************/
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   sendStringToUnity: () => (/* binding */ sendStringToUnity)
/* harmony export */ });
/* harmony import */ var _messages_unityBrowserMessaging_BaseMessages__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./messages/unityBrowserMessaging/BaseMessages */ "./src/messages/unityBrowserMessaging/BaseMessages.ts");

var FILE = 'indexJsLib.ts';
if (!window.GAO_UnityBrowserChannel) {
    window.GAO_UnityBrowserChannel = {};
}
if (!window.GAO_UnityBrowserChannel.BaseMessages) {
    window.GAO_UnityBrowserChannel.BaseMessages = {};
}
window.GAO_UnityBrowserChannel.BaseMessages.receiveString = function (str) {
    console.log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 2000: GAO_UnityBrowserChannel.BaseMessages.receiveString(): ".concat(str));
    _messages_unityBrowserMessaging_BaseMessages__WEBPACK_IMPORTED_MODULE_0__.BaseMessages.receiveString(str);
};
function sendStringToUnity(str) {
    var FUNC = 'sendStringToUnity()';
    console.log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 2100: sendStringToUnity(): ".concat(str));
    try {
        _messages_unityBrowserMessaging_BaseMessages__WEBPACK_IMPORTED_MODULE_0__.BaseMessages.sendString(str);
    }
    catch (err) {
        console.error("".concat(FILE, ":").concat(FUNC, ": ").concat(err));
    }
}
function keepPinging() {
    setInterval(function () {
        var msg = JSON.stringify({
            message: "Hello from browser!"
        });
        sendStringToUnity(msg);
    }, 5000);
}
// @@@@@@@@@@@@@@@@@@@@@@@@@@@
keepPinging();
// @@@@@@@@@@@@@@@@@@@@@@@@@@@

/******/ })()
;
//# sourceMappingURL=data:application/json;charset=utf-8;base64,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QTtBQUNBO0FBQ0EsV0FBVyxRQUFRO0FBQ25CLFdBQVcsWUFBWTtBQUN2QixXQUFXLFFBQVE7QUFDbkIsYUFBYTtBQUNiO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLFdBQVcsWUFBWTtBQUN2QixXQUFXLFFBQVE7QUFDbkIsYUFBYSxRQUFRO0FBQ3JCO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLFdBQVcsWUFBWTtBQUN2QixXQUFXLFFBQVE7QUFDbkIsYUFBYSxRQUFRO0FBQ3JCO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLFdBQVcsUUFBUTtBQUNuQixXQUFXLFlBQVk7QUFDdkIsV0FBVyxRQUFRO0FBQ25CLGFBQWE7QUFDYjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxXQUFXLFFBQVE7QUFDbkIsV0FBVyxZQUFZO0FBQ3ZCLFdBQVcsUUFBUTtBQUNuQixhQUFhO0FBQ2I7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsV0FBVyxZQUFZO0FBQ3ZCLFdBQVcsUUFBUTtBQUNuQixhQUFhLFFBQVE7QUFDckI7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsV0FBVyxZQUFZO0FBQ3ZCLFdBQVcsUUFBUTtBQUNuQixhQUFhLFFBQVE7QUFDckI7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxLQUFLLEtBQUs7QUFDVjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLEtBQUs7QUFDTDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLEtBQUssS0FBSztBQUNWO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxjQUFjO0FBQ2Q7QUFDQTtBQUNBLGNBQWMsMENBQTBDO0FBQ3hEO0FBQ0E7QUFDQSxjQUFjO0FBQ2Q7QUFDQSxxREFBcUQ7QUFDckQ7QUFDQTtBQUNBO0FBQ0Esa0JBQWtCO0FBQ2xCO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsS0FBSztBQUNMO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFD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Q0E7QUFDQSxjQUFjO0FBQ2Q7QUFDQTtBQUNBO0FBQ0E7QUFDQSxnRUFBZ0U7QUFDaEU7QUFDQTtBQUNBO0FBQ0EsY0FBYztBQUNkO0FBQ0Esc0JBQXNCO0FBQ3RCO0FBQ0E7QUFDQTtBQUNBLGNBQWM7QUFDZDtBQUNBLGtCQUFrQjtBQUNsQjtBQUNBO0FBQ0E7QUFDQSxjQUFjO0FBQ2Q7QUFDQSx1Q0FBdUM7QUFDdkM7QUFDQTtBQUNBO0FBQ0EsY0FBYztBQUNkO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxjQUFjO0FBQ2Q7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLGNBQWM7QUFDZDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsY0FBYztBQUNkO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxjQUFjO0FBQ2Q7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLGNBQWM7QUFDZDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsY0FBYztBQUNkO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxjQUFjO0FBQ2Q7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLGNBQWM7QUFDZDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsY0FBYztBQUNkO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxjQUFjO0FBQ2Q7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLGNBQWM7QUFDZDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsY0FBYztBQUNkO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxjQUFjO0FBQ2Q7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsY0FBYztBQUNkO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsVUFBVTtBQUNWO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLENBQUM7QUFDRDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLGNBQWMsUUFBUTtBQUN0QixjQUFjLFFBQVE7QUFDdEIsY0FB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Q0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLG9DQUFvQztBQUNwQztBQUNBO0FBQ0E7QUFDQSxtQkFBbUI7QUFDbkI7QUFDQSwwQ0FBMEM7QUFDMUM7QUFDQSxtQkFBbUI7QUFDbkIsVUFBVTtBQUNWO0FBQ0E7QUFDQSxtQkFBbUI7QUFDbkI7QUFDQSxpRUFBaUU7QUFDakU7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLHNCQUFzQjtBQUN0QjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLFNBQVM7QUFDVDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSx1QkFBdUI7QUFDdkIsY0FBYztBQUNkO0FBQ0E7QUFDQSxTQUFTO0FBQ1Q7QUFDQSxTQUFTO0FBQ1Q7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxVQUFVO0FBQ1Y7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSwyQkFBMkI7QUFDM0I7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLHNCQUFzQjtBQUN0QjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsMENBQTBDO0FBQzFDO0FBQ0EsU0FBUztBQUNUO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLHVCQUF1QjtBQUN2QixjQUFjO0FBQ2Q7QUFDQTtBQUNBLFNBQVM7QUFDVDtBQUNBLFNBQVM7QUFDVDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLHVCQUF1QjtBQUN2QixjQUFjO0FBQ2Q7QUFDQTtBQUNBO0FBQ0EsU0FBUztBQUNUO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxxQkFBcUI7QUFDckI7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsU0FBUztBQUNUO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSwyQ0FBMkM7QUFDM0MsdUJBQXVCO0FBQ3ZCLGNBQWM7QUFDZDtBQUNBO0FBQ0EsU0FBUztBQUNULHVDQUF1QztBQUN2QyxTQUFTO0FBQ1Q7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLDJDQUEyQztBQUMzQztBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLGFBQWEsYUFBYTtBQUMxQixtQkFBbUI7QUFDbkI7QUFDQTtBQUNBLDJCQUEyQjtBQUMzQjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLGlDQUFpQztBQUNqQztBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsMEJBQTBCO0FBQzFCO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxrQkFBa0I7QUFDbEI7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSx1QkFBdUI7QUFDdkI7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxjQUFjO0FBQ2Q7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsU0FBUztBQUNUO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLG1CQUFtQixpQkFBaUI7QUFDcEM7QUFDQTtBQUNB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QUFjLFFBQVE7QUFDdEIsY0FBYyxRQUFRO0FBQ3RCLGNBQWMsUUFBUTtBQUN0QixjQUFjLFNBQVM7QUFDdkIsY0FBYyxRQUFRO0FBQ3RCLGNBQWMsZ0JBQWdCO0FBQzlCLGNBQWMsTUFBTTtBQUNwQjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLFVBQVU7QUFDVjtBQUNBLGNBQWMsUUFBUTtBQUN0QixjQUFjLFFBQVE7QUFDdEIsY0FBYyxRQUFRO0FBQ3RCLGNBQWMsUUFBUTtBQUN0QixjQUFjLFFBQVE7QUFDdEI7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLFVBQVU7QUFDVjtBQUNBLGNBQWMsUUFBUTtBQUN0QixjQUFjLFFBQVE7QUFDdEIsY0FBYyxRQUFRO0FBQ3RCLGNBQWMsUUFBUTtBQUN0QixjQUFjLFFBQVE7QUFDdEIsY0FBYyxRQUFRO0FBQ3RCLGNBQWMsUUFBUTtBQUN0QixjQUFjLFFBQVE7QUFDdEIsY0FBYyxRQUFRO0FBQ3RCLGNBQWMsUUFBUTtBQUN0QixjQUFjLFFBQVE7QUFDdEIsY0FBYyxRQUFRO0FBQ3RCO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLFVBQVU7QUFDVjtBQUNBLGNBQWMsUUFBUTtBQUN0QixjQUFjLFFBQVE7QUFDdEIsY0FBYyxRQUFRO0FBQ3RCLGNBQWMsUUFBUTtBQUN0QixjQUFjLFFBQVE7QUFDdEIsY0FBYyxRQUFRO0FBQ3RCLGNBQWMsUUFBUTtBQUN0QixjQUFjLFFBQVE7QUFDdEIsY0FBYyxRQUFRO0FBQ3RCLGNBQWMsUUFBUTtBQUN0QixjQUFjLFFBQVE7QUFDdEIsY0FBYyxRQUFRO0FBQ3RCLGNBQWMsUUFBUTtBQUN0QjtBQUNBO0FB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QU0sSUFBSSxHQUFHLHFCQUFxQixDQUFDO0lBQ25DLE9BQU8sQ0FBQyxHQUFHLENBQUMsNEVBQXFFLEdBQUcsQ0FBRSxDQUFDLENBQUM7SUFDeEYsSUFDQSxDQUFDO1FBQ0csc0ZBQVksQ0FBQyxVQUFVLENBQUMsR0FBRyxDQUFDLENBQUM7SUFDakMsQ0FBQztJQUFDLE9BQU8sR0FBRyxFQUFFLENBQUM7UUFDWCxPQUFPLENBQUMsS0FBSyxDQUFDLFVBQUcsSUFBSSxjQUFJLElBQUksZUFBSyxHQUFHLENBQUUsQ0FBQyxDQUFDO0lBQzdDLENBQUM7QUFDTCxDQUFDO0FBRUQsU0FBUyxXQUFXO0lBQ2hCLFdBQVcsQ0FBQztRQUNSLElBQUksR0FBRyxHQUFHLElBQUksQ0FBQyxTQUFTLENBQ3BCO1lBQ0ksT0FBTyxFQUFFLHFCQUFxQjtTQUNqQyxDQUNKLENBQUM7UUFDRixpQkFBaUIsQ0FBQyxHQUFHLENBQUMsQ0FBQztJQUMzQixDQUFDLEVBQUUsSUFBSSxDQUFDO0FBRVosQ0FBQztBQUVELDhCQUE4QjtBQUM5QixXQUFXLEVBQUUsQ0FBQztBQUNkLDhCQUE4QiIsInNvdXJjZXMiOlsid2VicGFjazovL2pzLy4vbm9kZV9tb2R1bGVzL0Bwcm90b2J1ZmpzL2FzcHJvbWlzZS9pbmRleC5qcyIsIndlYnBhY2s6Ly9qcy8uL25vZGVfbW9kdWxlcy9AcHJvdG9idWZqcy9iYXNlNjQvaW5kZXguanMiLCJ3ZWJwYWNrOi8vanMvLi9ub2RlX21vZHVsZXMvQHByb3RvYnVmanMvY29kZWdlbi9pbmRleC5qcyIsIndlYnBhY2s6Ly9qcy8uL25vZGVfbW9kdWxlcy9AcHJvdG9idWZqcy9ldmVudGVtaXR0ZXIvaW5kZXguanMiLCJ3ZWJwYWNrOi8vanMvLi9ub2RlX21vZHVsZXMvQHByb3RvYnVmanMvZmV0Y2gvaW5kZXguanMiLCJ3ZWJwYWNrOi8vanMvLi9ub2RlX21vZHVsZXMvQHByb3RvYnVmanMvZmxvYXQvaW5kZXguanMiLCJ3ZWJwYWNrOi8vanMvLi9ub2RlX21vZHVsZXMvQHByb3RvYnVmanMvaW5xdWlyZS9pbmRleC5qcyIsIndlYnBhY2s6Ly9qcy8uL25vZGVfbW9kdWxlcy9AcHJvdG9idWZqcy9wYXRoL2luZGV4LmpzIiwid2VicGFjazovL2pzLy4vbm9kZV9tb2R1bGVzL0Bwcm90b2J1ZmpzL3Bvb2wvaW5kZXguanMiLCJ3ZWJwYWNrOi8vanMvLi9ub2RlX21vZHVsZXMvQHByb3RvYnVmanMvdXRmOC9pbmRleC5qcyIsIndlYnBhY2s6Ly9qcy8uL25vZGVfbW9kdWxlcy9wcm90b2J1ZmpzL2luZGV4LmpzIiwid2VicGFjazovL2pzLy4vbm9kZV9tb2R1bGVzL3Byb3RvYnVmanMvc3JjL2NvbW1vbi5qcyIsIndlYnBhY2s6Ly9qcy8uL25vZGVfbW9kdWxlcy9wcm90b2J1ZmpzL3NyYy9jb252ZXJ0ZXIuanMiLCJ3ZWJwYWNrOi8vanMvLi9ub2RlX21vZHVsZXMvcHJvdG9idWZqcy9zcmMvZGVjb2Rlci5qcyIsIndlYnBhY2s6Ly9qcy8uL25vZGVfbW9kdWxlcy9wcm90b2J1ZmpzL3NyYy9lbmNvZGVyLmpzIiwid2VicGFjazovL2pzLy4vbm9kZV9tb2R1bGVzL3Byb3RvYnVmanMvc3JjL2VudW0uanMiLCJ3ZWJwYWNrOi8vanMvLi9ub2RlX21vZHVsZXMvcHJvdG9idWZqcy9zcmMvZmllbGQuanMiLCJ3ZWJwYWNrOi8vanMvLi9ub2RlX21vZHVsZXMvcHJvdG9idWZqcy9zcmMvaW5kZXgtbGlnaHQuanMiLCJ3ZWJwYWNrOi8vanMvLi9ub2RlX21vZHVsZXMvcHJvdG9idWZqcy9zcmMvaW5kZXgtbWluaW1hbC5qcyIsIndlYnBhY2s6Ly9qcy8uL25vZGVfbW9kdWxlcy9wcm90b2J1ZmpzL3NyYy9pbmRleC5qcyIsIndlYnBhY2s6Ly9qcy8uL25vZGVfbW9kdWxlcy9wcm90b2J1ZmpzL3NyYy9tYXBmaWVsZC5qcyIsIndlYnBhY2s6Ly9qcy8uL25vZGVfbW9kdWxlcy9wcm90b2J1ZmpzL3NyYy9tZXNzYWdlLmpzIiwid2VicGFjazovL2pzLy4vbm9kZV9tb2R1bGVzL3Byb3RvYnVmanMvc3JjL21ldGhvZC5qcyIsIndlYnBhY2s6Ly9qcy8uL25vZGVfbW9kdWxlcy9wcm90b2J1ZmpzL3NyYy9uYW1lc3BhY2UuanMiLCJ3ZWJwYWNrOi8vanMvLi9ub2RlX21vZHVsZXMvcHJvdG9idWZqcy9zcmMvb2JqZWN0LmpzIiwid2VicGFjazovL2pzLy4vbm9kZV9tb2R1bGVzL3Byb3RvYnVmanMvc3JjL29uZW9mLmpzIiwid2VicGFjazovL2pzLy4vbm9kZV9tb2R1bGVzL3Byb3RvYnVmanMvc3JjL3BhcnNlLmpzIiwid2VicGFjazovL2pzLy4vbm9kZV9tb2R1bGVzL3Byb3RvYnVmanMvc3JjL3JlYWRlci5qcyIsIndlYnBhY2s6Ly9qcy8uL25vZGVfbW9kdWxlcy9wcm90b2J1ZmpzL3NyYy9yZWFkZXJfYnVmZmVyLmpzIiwid2VicGFjazovL2pzLy4vbm9kZV9tb2R1bGVzL3Byb3RvYnVmanMvc3JjL3Jvb3QuanMiLCJ3ZWJwYWNrOi8vanMvLi9ub2RlX21vZHVsZXMvcHJvdG9idWZqcy9zcmMvcm9vdHMuanMiLCJ3ZWJwYWNrOi8vanMvLi9ub2RlX21vZHVsZXMvcHJvdG9idWZqcy9zcmMvcnBjLmpzIiwid2VicGFjazovL2pzLy4vbm9kZV9tb2R1bGVzL3Byb3RvYnVmanMvc3JjL3JwYy9zZXJ2aWNlLmpzIiwid2VicGFjazovL2pzLy4vbm9kZV9tb2R1bGVzL3Byb3RvYnVmanMvc3JjL3NlcnZpY2UuanMiLCJ3ZWJwYWNrOi8vanMvLi9ub2RlX21vZHVsZXMvcHJvdG9idWZqcy9zcmMvdG9rZW5pemUuanMiLCJ3ZWJwYWNrOi8vanMvLi9ub2RlX21vZHVsZXMvcHJvdG9idWZqcy9zcmMvdHlwZS5qcyIsIndlYnBhY2s6Ly9qcy8uL25vZGVfbW9kdWxlcy9wcm90b2J1ZmpzL3NyYy90eXBlcy5qcyIsIndlYnBhY2s6Ly9qcy8uL25vZGVfbW9kdWxlcy9wcm90b2J1ZmpzL3NyYy91dGlsLmpzIiwid2VicGFjazovL2pzLy4vbm9kZV9tb2R1bGVzL3Byb3RvYnVmanMvc3JjL3V0aWwvbG9uZ2JpdHMuanMiLCJ3ZWJwYWNrOi8vanMvLi9ub2RlX21vZHVsZXMvcHJvdG9idWZqcy9zcmMvdXRpbC9taW5pbWFsLmpzIiwid2VicGFjazovL2pzLy4vbm9kZV9tb2R1bGVzL3Byb3RvYnVmanMvc3JjL3ZlcmlmaWVyLmpzIiwid2VicGFjazovL2pzLy4vbm9kZV9tb2R1bGVzL3Byb3RvYnVmanMvc3JjL3dyYXBwZXJzLmpzIiwid2VicGFjazovL2pzLy4vbm9kZV9tb2R1bGVzL3Byb3RvYnVmanMvc3JjL3dyaXRlci5qcyIsIndlYnBhY2s6Ly9qcy8uL25vZGVfbW9kdWxlcy9wcm90b2J1ZmpzL3NyYy93cml0ZXJfYnVmZmVyLmpzIiwid2VicGFjazovL2pzLy4vc3JjL2NvbmZpZy50cyIsIndlYnBhY2s6Ly9qcy8uL3NyYy9kaXNwYXRjaGVyLnRzIiwid2VicGFjazovL2pzLy4vc3JjL2V2ZW50cy50cyIsIndlYnBhY2s6Ly9qcy8uL3NyYy9tZXNzYWdlcy9Xc0F1dGhlbnRpY2F0aW9uLnRzIiwid2VicGFjazovL2pzLy4vc3JjL21lc3NhZ2VzL3VuaXR5QnJvd3Nlck1lc3NhZ2luZy9CYXNlTWVzc2FnZXMudHMiLCJ3ZWJwYWNrOi8vanMvLi9zcmMvcHJvdG8udHMiLCJ3ZWJwYWNrOi8vanMvLi9zcmMvd3NDbGllbnQudHMiLCJ3ZWJwYWNrOi8vanMvLi9ub2RlX21vZHVsZXMvdXVpZC9kaXN0L2VzbS1icm93c2VyL25hdGl2ZS5qcyIsIndlYnBhY2s6Ly9qcy8uL25vZGVfbW9kdWxlcy91dWlkL2Rpc3QvZXNtLWJyb3dzZXIvcmVnZXguanMiLCJ3ZWJwYWNrOi8vanMvLi9ub2RlX21vZHVsZXMvdXVpZC9kaXN0L2VzbS1icm93c2VyL3JuZy5qcyIsIndlYnBhY2s6Ly9qcy8uL25vZGVfbW9kdWxlcy91dWlkL2Rpc3QvZXNtLWJyb3dzZXIvc3RyaW5naWZ5LmpzIiwid2VicGFjazovL2pzLy4vbm9kZV9tb2R1bGVzL3V1aWQvZGlzdC9lc20tYnJvd3Nlci92NC5qcyIsIndlYnBhY2s6Ly9qcy8uL25vZGVfbW9kdWxlcy91dWlkL2Rpc3QvZXNtLWJyb3dzZXIvdmFsaWRhdGUuanMiLCJ3ZWJwYWNrOi8vanMvd2VicGFjay9ib290c3RyYXAiLCJ3ZWJwYWNrOi8vanMvd2VicGFjay9ydW50aW1lL2NvbXBhdCBnZXQgZGVmYXVsdCBleHBvcnQiLCJ3ZWJwYWNrOi8vanMvd2VicGFjay9ydW50aW1lL2RlZmluZSBwcm9wZXJ0eSBnZXR0ZXJzIiwid2VicGFjazovL2pzL3dlYnBhY2svcnVudGltZS9nbG9iYWwiLCJ3ZWJwYWNrOi8vanMvd2VicGFjay9ydW50aW1lL2hhc093blByb3BlcnR5IHNob3J0aGFuZCIsIndlYnBhY2s6Ly9qcy93ZWJwYWNrL3J1bnRpbWUvbWFrZSBuYW1lc3BhY2Ugb2JqZWN0Iiwid2VicGFjazovL2pzLy4vc3JjL2luZGV4SnNMaWIudHMiXSwic291cmNlc0NvbnRlbnQiOlsiXCJ1c2Ugc3RyaWN0XCI7XHJcbm1vZHVsZS5leHBvcnRzID0gYXNQcm9taXNlO1xyXG5cclxuLyoqXHJcbiAqIENhbGxiYWNrIGFzIHVzZWQgYnkge0BsaW5rIHV0aWwuYXNQcm9taXNlfS5cclxuICogQHR5cGVkZWYgYXNQcm9taXNlQ2FsbGJhY2tcclxuICogQHR5cGUge2Z1bmN0aW9ufVxyXG4gKiBAcGFyYW0ge0Vycm9yfG51bGx9IGVycm9yIEVycm9yLCBpZiBhbnlcclxuICogQHBhcmFtIHsuLi4qfSBwYXJhbXMgQWRkaXRpb25hbCBhcmd1bWVudHNcclxuICogQHJldHVybnMge3VuZGVmaW5lZH1cclxuICovXHJcblxyXG4vKipcclxuICogUmV0dXJucyBhIHByb21pc2UgZnJvbSBhIG5vZGUtc3R5bGUgY2FsbGJhY2sgZnVuY3Rpb24uXHJcbiAqIEBtZW1iZXJvZiB1dGlsXHJcbiAqIEBwYXJhbSB7YXNQcm9taXNlQ2FsbGJhY2t9IGZuIEZ1bmN0aW9uIHRvIGNhbGxcclxuICogQHBhcmFtIHsqfSBjdHggRnVuY3Rpb24gY29udGV4dFxyXG4gKiBAcGFyYW0gey4uLip9IHBhcmFtcyBGdW5jdGlvbiBhcmd1bWVudHNcclxuICogQHJldHVybnMge1Byb21pc2U8Kj59IFByb21pc2lmaWVkIGZ1bmN0aW9uXHJcbiAqL1xyXG5mdW5jdGlvbiBhc1Byb21pc2UoZm4sIGN0eC8qLCB2YXJhcmdzICovKSB7XHJcbiAgICB2YXIgcGFyYW1zICA9IG5ldyBBcnJheShhcmd1bWVudHMubGVuZ3RoIC0gMSksXHJcbiAgICAgICAgb2Zmc2V0ICA9IDAsXHJcbiAgICAgICAgaW5kZXggICA9IDIsXHJcbiAgICAgICAgcGVuZGluZyA9IHRydWU7XHJcbiAgICB3aGlsZSAoaW5kZXggPCBhcmd1bWVudHMubGVuZ3RoKVxyXG4gICAgICAgIHBhcmFtc1tvZmZzZXQrK10gPSBhcmd1bWVudHNbaW5kZXgrK107XHJcbiAgICByZXR1cm4gbmV3IFByb21pc2UoZnVuY3Rpb24gZXhlY3V0b3IocmVzb2x2ZSwgcmVqZWN0KSB7XHJcbiAgICAgICAgcGFyYW1zW29mZnNldF0gPSBmdW5jdGlvbiBjYWxsYmFjayhlcnIvKiwgdmFyYXJncyAqLykge1xyXG4gICAgICAgICAgICBpZiAocGVuZGluZykge1xyXG4gICAgICAgICAgICAgICAgcGVuZGluZyA9IGZhbHNlO1xyXG4gICAgICAgICAgICAgICAgaWYgKGVycilcclxuICAgICAgICAgICAgICAgICAgICByZWplY3QoZXJyKTtcclxuICAgICAgICAgICAgICAgIGVsc2Uge1xyXG4gICAgICAgICAgICAgICAgICAgIHZhciBwYXJhbXMgPSBuZXcgQXJyYXkoYXJndW1lbnRzLmxlbmd0aCAtIDEpLFxyXG4gICAgICAgICAgICAgICAgICAgICAgICBvZmZzZXQgPSAwO1xyXG4gICAgICAgICAgICAgICAgICAgIHdoaWxlIChvZmZzZXQgPCBwYXJhbXMubGVuZ3RoKVxyXG4gICAgICAgICAgICAgICAgICAgICAgICBwYXJhbXNbb2Zmc2V0KytdID0gYXJndW1lbnRzW29mZnNldF07XHJcbiAgICAgICAgICAgICAgICAgICAgcmVzb2x2ZS5hcHBseShudWxsLCBwYXJhbXMpO1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfTtcclxuICAgICAgICB0cnkge1xyXG4gICAgICAgICAgICBmbi5hcHBseShjdHggfHwgbnVsbCwgcGFyYW1zKTtcclxuICAgICAgICB9IGNhdGNoIChlcnIpIHtcclxuICAgICAgICAgICAgaWYgKHBlbmRpbmcpIHtcclxuICAgICAgICAgICAgICAgIHBlbmRpbmcgPSBmYWxzZTtcclxuICAgICAgICAgICAgICAgIHJlamVjdChlcnIpO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG4gICAgfSk7XHJcbn1cclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcblxyXG4vKipcclxuICogQSBtaW5pbWFsIGJhc2U2NCBpbXBsZW1lbnRhdGlvbiBmb3IgbnVtYmVyIGFycmF5cy5cclxuICogQG1lbWJlcm9mIHV0aWxcclxuICogQG5hbWVzcGFjZVxyXG4gKi9cclxudmFyIGJhc2U2NCA9IGV4cG9ydHM7XHJcblxyXG4vKipcclxuICogQ2FsY3VsYXRlcyB0aGUgYnl0ZSBsZW5ndGggb2YgYSBiYXNlNjQgZW5jb2RlZCBzdHJpbmcuXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBzdHJpbmcgQmFzZTY0IGVuY29kZWQgc3RyaW5nXHJcbiAqIEByZXR1cm5zIHtudW1iZXJ9IEJ5dGUgbGVuZ3RoXHJcbiAqL1xyXG5iYXNlNjQubGVuZ3RoID0gZnVuY3Rpb24gbGVuZ3RoKHN0cmluZykge1xyXG4gICAgdmFyIHAgPSBzdHJpbmcubGVuZ3RoO1xyXG4gICAgaWYgKCFwKVxyXG4gICAgICAgIHJldHVybiAwO1xyXG4gICAgdmFyIG4gPSAwO1xyXG4gICAgd2hpbGUgKC0tcCAlIDQgPiAxICYmIHN0cmluZy5jaGFyQXQocCkgPT09IFwiPVwiKVxyXG4gICAgICAgICsrbjtcclxuICAgIHJldHVybiBNYXRoLmNlaWwoc3RyaW5nLmxlbmd0aCAqIDMpIC8gNCAtIG47XHJcbn07XHJcblxyXG4vLyBCYXNlNjQgZW5jb2RpbmcgdGFibGVcclxudmFyIGI2NCA9IG5ldyBBcnJheSg2NCk7XHJcblxyXG4vLyBCYXNlNjQgZGVjb2RpbmcgdGFibGVcclxudmFyIHM2NCA9IG5ldyBBcnJheSgxMjMpO1xyXG5cclxuLy8gNjUuLjkwLCA5Ny4uMTIyLCA0OC4uNTcsIDQzLCA0N1xyXG5mb3IgKHZhciBpID0gMDsgaSA8IDY0OylcclxuICAgIHM2NFtiNjRbaV0gPSBpIDwgMjYgPyBpICsgNjUgOiBpIDwgNTIgPyBpICsgNzEgOiBpIDwgNjIgPyBpIC0gNCA6IGkgLSA1OSB8IDQzXSA9IGkrKztcclxuXHJcbi8qKlxyXG4gKiBFbmNvZGVzIGEgYnVmZmVyIHRvIGEgYmFzZTY0IGVuY29kZWQgc3RyaW5nLlxyXG4gKiBAcGFyYW0ge1VpbnQ4QXJyYXl9IGJ1ZmZlciBTb3VyY2UgYnVmZmVyXHJcbiAqIEBwYXJhbSB7bnVtYmVyfSBzdGFydCBTb3VyY2Ugc3RhcnRcclxuICogQHBhcmFtIHtudW1iZXJ9IGVuZCBTb3VyY2UgZW5kXHJcbiAqIEByZXR1cm5zIHtzdHJpbmd9IEJhc2U2NCBlbmNvZGVkIHN0cmluZ1xyXG4gKi9cclxuYmFzZTY0LmVuY29kZSA9IGZ1bmN0aW9uIGVuY29kZShidWZmZXIsIHN0YXJ0LCBlbmQpIHtcclxuICAgIHZhciBwYXJ0cyA9IG51bGwsXHJcbiAgICAgICAgY2h1bmsgPSBbXTtcclxuICAgIHZhciBpID0gMCwgLy8gb3V0cHV0IGluZGV4XHJcbiAgICAgICAgaiA9IDAsIC8vIGdvdG8gaW5kZXhcclxuICAgICAgICB0OyAgICAgLy8gdGVtcG9yYXJ5XHJcbiAgICB3aGlsZSAoc3RhcnQgPCBlbmQpIHtcclxuICAgICAgICB2YXIgYiA9IGJ1ZmZlcltzdGFydCsrXTtcclxuICAgICAgICBzd2l0Y2ggKGopIHtcclxuICAgICAgICAgICAgY2FzZSAwOlxyXG4gICAgICAgICAgICAgICAgY2h1bmtbaSsrXSA9IGI2NFtiID4+IDJdO1xyXG4gICAgICAgICAgICAgICAgdCA9IChiICYgMykgPDwgNDtcclxuICAgICAgICAgICAgICAgIGogPSAxO1xyXG4gICAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgICAgIGNhc2UgMTpcclxuICAgICAgICAgICAgICAgIGNodW5rW2krK10gPSBiNjRbdCB8IGIgPj4gNF07XHJcbiAgICAgICAgICAgICAgICB0ID0gKGIgJiAxNSkgPDwgMjtcclxuICAgICAgICAgICAgICAgIGogPSAyO1xyXG4gICAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgICAgIGNhc2UgMjpcclxuICAgICAgICAgICAgICAgIGNodW5rW2krK10gPSBiNjRbdCB8IGIgPj4gNl07XHJcbiAgICAgICAgICAgICAgICBjaHVua1tpKytdID0gYjY0W2IgJiA2M107XHJcbiAgICAgICAgICAgICAgICBqID0gMDtcclxuICAgICAgICAgICAgICAgIGJyZWFrO1xyXG4gICAgICAgIH1cclxuICAgICAgICBpZiAoaSA+IDgxOTEpIHtcclxuICAgICAgICAgICAgKHBhcnRzIHx8IChwYXJ0cyA9IFtdKSkucHVzaChTdHJpbmcuZnJvbUNoYXJDb2RlLmFwcGx5KFN0cmluZywgY2h1bmspKTtcclxuICAgICAgICAgICAgaSA9IDA7XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG4gICAgaWYgKGopIHtcclxuICAgICAgICBjaHVua1tpKytdID0gYjY0W3RdO1xyXG4gICAgICAgIGNodW5rW2krK10gPSA2MTtcclxuICAgICAgICBpZiAoaiA9PT0gMSlcclxuICAgICAgICAgICAgY2h1bmtbaSsrXSA9IDYxO1xyXG4gICAgfVxyXG4gICAgaWYgKHBhcnRzKSB7XHJcbiAgICAgICAgaWYgKGkpXHJcbiAgICAgICAgICAgIHBhcnRzLnB1c2goU3RyaW5nLmZyb21DaGFyQ29kZS5hcHBseShTdHJpbmcsIGNodW5rLnNsaWNlKDAsIGkpKSk7XHJcbiAgICAgICAgcmV0dXJuIHBhcnRzLmpvaW4oXCJcIik7XHJcbiAgICB9XHJcbiAgICByZXR1cm4gU3RyaW5nLmZyb21DaGFyQ29kZS5hcHBseShTdHJpbmcsIGNodW5rLnNsaWNlKDAsIGkpKTtcclxufTtcclxuXHJcbnZhciBpbnZhbGlkRW5jb2RpbmcgPSBcImludmFsaWQgZW5jb2RpbmdcIjtcclxuXHJcbi8qKlxyXG4gKiBEZWNvZGVzIGEgYmFzZTY0IGVuY29kZWQgc3RyaW5nIHRvIGEgYnVmZmVyLlxyXG4gKiBAcGFyYW0ge3N0cmluZ30gc3RyaW5nIFNvdXJjZSBzdHJpbmdcclxuICogQHBhcmFtIHtVaW50OEFycmF5fSBidWZmZXIgRGVzdGluYXRpb24gYnVmZmVyXHJcbiAqIEBwYXJhbSB7bnVtYmVyfSBvZmZzZXQgRGVzdGluYXRpb24gb2Zmc2V0XHJcbiAqIEByZXR1cm5zIHtudW1iZXJ9IE51bWJlciBvZiBieXRlcyB3cml0dGVuXHJcbiAqIEB0aHJvd3Mge0Vycm9yfSBJZiBlbmNvZGluZyBpcyBpbnZhbGlkXHJcbiAqL1xyXG5iYXNlNjQuZGVjb2RlID0gZnVuY3Rpb24gZGVjb2RlKHN0cmluZywgYnVmZmVyLCBvZmZzZXQpIHtcclxuICAgIHZhciBzdGFydCA9IG9mZnNldDtcclxuICAgIHZhciBqID0gMCwgLy8gZ290byBpbmRleFxyXG4gICAgICAgIHQ7ICAgICAvLyB0ZW1wb3JhcnlcclxuICAgIGZvciAodmFyIGkgPSAwOyBpIDwgc3RyaW5nLmxlbmd0aDspIHtcclxuICAgICAgICB2YXIgYyA9IHN0cmluZy5jaGFyQ29kZUF0KGkrKyk7XHJcbiAgICAgICAgaWYgKGMgPT09IDYxICYmIGogPiAxKVxyXG4gICAgICAgICAgICBicmVhaztcclxuICAgICAgICBpZiAoKGMgPSBzNjRbY10pID09PSB1bmRlZmluZWQpXHJcbiAgICAgICAgICAgIHRocm93IEVycm9yKGludmFsaWRFbmNvZGluZyk7XHJcbiAgICAgICAgc3dpdGNoIChqKSB7XHJcbiAgICAgICAgICAgIGNhc2UgMDpcclxuICAgICAgICAgICAgICAgIHQgPSBjO1xyXG4gICAgICAgICAgICAgICAgaiA9IDE7XHJcbiAgICAgICAgICAgICAgICBicmVhaztcclxuICAgICAgICAgICAgY2FzZSAxOlxyXG4gICAgICAgICAgICAgICAgYnVmZmVyW29mZnNldCsrXSA9IHQgPDwgMiB8IChjICYgNDgpID4+IDQ7XHJcbiAgICAgICAgICAgICAgICB0ID0gYztcclxuICAgICAgICAgICAgICAgIGogPSAyO1xyXG4gICAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgICAgIGNhc2UgMjpcclxuICAgICAgICAgICAgICAgIGJ1ZmZlcltvZmZzZXQrK10gPSAodCAmIDE1KSA8PCA0IHwgKGMgJiA2MCkgPj4gMjtcclxuICAgICAgICAgICAgICAgIHQgPSBjO1xyXG4gICAgICAgICAgICAgICAgaiA9IDM7XHJcbiAgICAgICAgICAgICAgICBicmVhaztcclxuICAgICAgICAgICAgY2FzZSAzOlxyXG4gICAgICAgICAgICAgICAgYnVmZmVyW29mZnNldCsrXSA9ICh0ICYgMykgPDwgNiB8IGM7XHJcbiAgICAgICAgICAgICAgICBqID0gMDtcclxuICAgICAgICAgICAgICAgIGJyZWFrO1xyXG4gICAgICAgIH1cclxuICAgIH1cclxuICAgIGlmIChqID09PSAxKVxyXG4gICAgICAgIHRocm93IEVycm9yKGludmFsaWRFbmNvZGluZyk7XHJcbiAgICByZXR1cm4gb2Zmc2V0IC0gc3RhcnQ7XHJcbn07XHJcblxyXG4vKipcclxuICogVGVzdHMgaWYgdGhlIHNwZWNpZmllZCBzdHJpbmcgYXBwZWFycyB0byBiZSBiYXNlNjQgZW5jb2RlZC5cclxuICogQHBhcmFtIHtzdHJpbmd9IHN0cmluZyBTdHJpbmcgdG8gdGVzdFxyXG4gKiBAcmV0dXJucyB7Ym9vbGVhbn0gYHRydWVgIGlmIHByb2JhYmx5IGJhc2U2NCBlbmNvZGVkLCBvdGhlcndpc2UgZmFsc2VcclxuICovXHJcbmJhc2U2NC50ZXN0ID0gZnVuY3Rpb24gdGVzdChzdHJpbmcpIHtcclxuICAgIHJldHVybiAvXig/OltBLVphLXowLTkrL117NH0pKig/OltBLVphLXowLTkrL117Mn09PXxbQS1aYS16MC05Ky9dezN9PSk/JC8udGVzdChzdHJpbmcpO1xyXG59O1xyXG4iLCJcInVzZSBzdHJpY3RcIjtcclxubW9kdWxlLmV4cG9ydHMgPSBjb2RlZ2VuO1xyXG5cclxuLyoqXHJcbiAqIEJlZ2lucyBnZW5lcmF0aW5nIGEgZnVuY3Rpb24uXHJcbiAqIEBtZW1iZXJvZiB1dGlsXHJcbiAqIEBwYXJhbSB7c3RyaW5nW119IGZ1bmN0aW9uUGFyYW1zIEZ1bmN0aW9uIHBhcmFtZXRlciBuYW1lc1xyXG4gKiBAcGFyYW0ge3N0cmluZ30gW2Z1bmN0aW9uTmFtZV0gRnVuY3Rpb24gbmFtZSBpZiBub3QgYW5vbnltb3VzXHJcbiAqIEByZXR1cm5zIHtDb2RlZ2VufSBBcHBlbmRlciB0aGF0IGFwcGVuZHMgY29kZSB0byB0aGUgZnVuY3Rpb24ncyBib2R5XHJcbiAqL1xyXG5mdW5jdGlvbiBjb2RlZ2VuKGZ1bmN0aW9uUGFyYW1zLCBmdW5jdGlvbk5hbWUpIHtcclxuXHJcbiAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgaWYgKi9cclxuICAgIGlmICh0eXBlb2YgZnVuY3Rpb25QYXJhbXMgPT09IFwic3RyaW5nXCIpIHtcclxuICAgICAgICBmdW5jdGlvbk5hbWUgPSBmdW5jdGlvblBhcmFtcztcclxuICAgICAgICBmdW5jdGlvblBhcmFtcyA9IHVuZGVmaW5lZDtcclxuICAgIH1cclxuXHJcbiAgICB2YXIgYm9keSA9IFtdO1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogQXBwZW5kcyBjb2RlIHRvIHRoZSBmdW5jdGlvbidzIGJvZHkgb3IgZmluaXNoZXMgZ2VuZXJhdGlvbi5cclxuICAgICAqIEB0eXBlZGVmIENvZGVnZW5cclxuICAgICAqIEB0eXBlIHtmdW5jdGlvbn1cclxuICAgICAqIEBwYXJhbSB7c3RyaW5nfE9iamVjdC48c3RyaW5nLCo+fSBbZm9ybWF0U3RyaW5nT3JTY29wZV0gRm9ybWF0IHN0cmluZyBvciwgdG8gZmluaXNoIHRoZSBmdW5jdGlvbiwgYW4gb2JqZWN0IG9mIGFkZGl0aW9uYWwgc2NvcGUgdmFyaWFibGVzLCBpZiBhbnlcclxuICAgICAqIEBwYXJhbSB7Li4uKn0gW2Zvcm1hdFBhcmFtc10gRm9ybWF0IHBhcmFtZXRlcnNcclxuICAgICAqIEByZXR1cm5zIHtDb2RlZ2VufEZ1bmN0aW9ufSBJdHNlbGYgb3IgdGhlIGdlbmVyYXRlZCBmdW5jdGlvbiBpZiBmaW5pc2hlZFxyXG4gICAgICogQHRocm93cyB7RXJyb3J9IElmIGZvcm1hdCBwYXJhbWV0ZXIgY291bnRzIGRvIG5vdCBtYXRjaFxyXG4gICAgICovXHJcblxyXG4gICAgZnVuY3Rpb24gQ29kZWdlbihmb3JtYXRTdHJpbmdPclNjb3BlKSB7XHJcbiAgICAgICAgLy8gbm90ZSB0aGF0IGV4cGxpY2l0IGFycmF5IGhhbmRsaW5nIGJlbG93IG1ha2VzIHRoaXMgfjUwJSBmYXN0ZXJcclxuXHJcbiAgICAgICAgLy8gZmluaXNoIHRoZSBmdW5jdGlvblxyXG4gICAgICAgIGlmICh0eXBlb2YgZm9ybWF0U3RyaW5nT3JTY29wZSAhPT0gXCJzdHJpbmdcIikge1xyXG4gICAgICAgICAgICB2YXIgc291cmNlID0gdG9TdHJpbmcoKTtcclxuICAgICAgICAgICAgaWYgKGNvZGVnZW4udmVyYm9zZSlcclxuICAgICAgICAgICAgICAgIGNvbnNvbGUubG9nKFwiY29kZWdlbjogXCIgKyBzb3VyY2UpOyAvLyBlc2xpbnQtZGlzYWJsZS1saW5lIG5vLWNvbnNvbGVcclxuICAgICAgICAgICAgc291cmNlID0gXCJyZXR1cm4gXCIgKyBzb3VyY2U7XHJcbiAgICAgICAgICAgIGlmIChmb3JtYXRTdHJpbmdPclNjb3BlKSB7XHJcbiAgICAgICAgICAgICAgICB2YXIgc2NvcGVLZXlzICAgPSBPYmplY3Qua2V5cyhmb3JtYXRTdHJpbmdPclNjb3BlKSxcclxuICAgICAgICAgICAgICAgICAgICBzY29wZVBhcmFtcyA9IG5ldyBBcnJheShzY29wZUtleXMubGVuZ3RoICsgMSksXHJcbiAgICAgICAgICAgICAgICAgICAgc2NvcGVWYWx1ZXMgPSBuZXcgQXJyYXkoc2NvcGVLZXlzLmxlbmd0aCksXHJcbiAgICAgICAgICAgICAgICAgICAgc2NvcGVPZmZzZXQgPSAwO1xyXG4gICAgICAgICAgICAgICAgd2hpbGUgKHNjb3BlT2Zmc2V0IDwgc2NvcGVLZXlzLmxlbmd0aCkge1xyXG4gICAgICAgICAgICAgICAgICAgIHNjb3BlUGFyYW1zW3Njb3BlT2Zmc2V0XSA9IHNjb3BlS2V5c1tzY29wZU9mZnNldF07XHJcbiAgICAgICAgICAgICAgICAgICAgc2NvcGVWYWx1ZXNbc2NvcGVPZmZzZXRdID0gZm9ybWF0U3RyaW5nT3JTY29wZVtzY29wZUtleXNbc2NvcGVPZmZzZXQrK11dO1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgc2NvcGVQYXJhbXNbc2NvcGVPZmZzZXRdID0gc291cmNlO1xyXG4gICAgICAgICAgICAgICAgcmV0dXJuIEZ1bmN0aW9uLmFwcGx5KG51bGwsIHNjb3BlUGFyYW1zKS5hcHBseShudWxsLCBzY29wZVZhbHVlcyk7IC8vIGVzbGludC1kaXNhYmxlLWxpbmUgbm8tbmV3LWZ1bmNcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICByZXR1cm4gRnVuY3Rpb24oc291cmNlKSgpOyAvLyBlc2xpbnQtZGlzYWJsZS1saW5lIG5vLW5ldy1mdW5jXHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICAvLyBvdGhlcndpc2UgYXBwZW5kIHRvIGJvZHlcclxuICAgICAgICB2YXIgZm9ybWF0UGFyYW1zID0gbmV3IEFycmF5KGFyZ3VtZW50cy5sZW5ndGggLSAxKSxcclxuICAgICAgICAgICAgZm9ybWF0T2Zmc2V0ID0gMDtcclxuICAgICAgICB3aGlsZSAoZm9ybWF0T2Zmc2V0IDwgZm9ybWF0UGFyYW1zLmxlbmd0aClcclxuICAgICAgICAgICAgZm9ybWF0UGFyYW1zW2Zvcm1hdE9mZnNldF0gPSBhcmd1bWVudHNbKytmb3JtYXRPZmZzZXRdO1xyXG4gICAgICAgIGZvcm1hdE9mZnNldCA9IDA7XHJcbiAgICAgICAgZm9ybWF0U3RyaW5nT3JTY29wZSA9IGZvcm1hdFN0cmluZ09yU2NvcGUucmVwbGFjZSgvJShbJWRmaWpzXSkvZywgZnVuY3Rpb24gcmVwbGFjZSgkMCwgJDEpIHtcclxuICAgICAgICAgICAgdmFyIHZhbHVlID0gZm9ybWF0UGFyYW1zW2Zvcm1hdE9mZnNldCsrXTtcclxuICAgICAgICAgICAgc3dpdGNoICgkMSkge1xyXG4gICAgICAgICAgICAgICAgY2FzZSBcImRcIjogY2FzZSBcImZcIjogcmV0dXJuIFN0cmluZyhOdW1iZXIodmFsdWUpKTtcclxuICAgICAgICAgICAgICAgIGNhc2UgXCJpXCI6IHJldHVybiBTdHJpbmcoTWF0aC5mbG9vcih2YWx1ZSkpO1xyXG4gICAgICAgICAgICAgICAgY2FzZSBcImpcIjogcmV0dXJuIEpTT04uc3RyaW5naWZ5KHZhbHVlKTtcclxuICAgICAgICAgICAgICAgIGNhc2UgXCJzXCI6IHJldHVybiBTdHJpbmcodmFsdWUpO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIHJldHVybiBcIiVcIjtcclxuICAgICAgICB9KTtcclxuICAgICAgICBpZiAoZm9ybWF0T2Zmc2V0ICE9PSBmb3JtYXRQYXJhbXMubGVuZ3RoKVxyXG4gICAgICAgICAgICB0aHJvdyBFcnJvcihcInBhcmFtZXRlciBjb3VudCBtaXNtYXRjaFwiKTtcclxuICAgICAgICBib2R5LnB1c2goZm9ybWF0U3RyaW5nT3JTY29wZSk7XHJcbiAgICAgICAgcmV0dXJuIENvZGVnZW47XHJcbiAgICB9XHJcblxyXG4gICAgZnVuY3Rpb24gdG9TdHJpbmcoZnVuY3Rpb25OYW1lT3ZlcnJpZGUpIHtcclxuICAgICAgICByZXR1cm4gXCJmdW5jdGlvbiBcIiArIChmdW5jdGlvbk5hbWVPdmVycmlkZSB8fCBmdW5jdGlvbk5hbWUgfHwgXCJcIikgKyBcIihcIiArIChmdW5jdGlvblBhcmFtcyAmJiBmdW5jdGlvblBhcmFtcy5qb2luKFwiLFwiKSB8fCBcIlwiKSArIFwiKXtcXG4gIFwiICsgYm9keS5qb2luKFwiXFxuICBcIikgKyBcIlxcbn1cIjtcclxuICAgIH1cclxuXHJcbiAgICBDb2RlZ2VuLnRvU3RyaW5nID0gdG9TdHJpbmc7XHJcbiAgICByZXR1cm4gQ29kZWdlbjtcclxufVxyXG5cclxuLyoqXHJcbiAqIEJlZ2lucyBnZW5lcmF0aW5nIGEgZnVuY3Rpb24uXHJcbiAqIEBtZW1iZXJvZiB1dGlsXHJcbiAqIEBmdW5jdGlvbiBjb2RlZ2VuXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBbZnVuY3Rpb25OYW1lXSBGdW5jdGlvbiBuYW1lIGlmIG5vdCBhbm9ueW1vdXNcclxuICogQHJldHVybnMge0NvZGVnZW59IEFwcGVuZGVyIHRoYXQgYXBwZW5kcyBjb2RlIHRvIHRoZSBmdW5jdGlvbidzIGJvZHlcclxuICogQHZhcmlhdGlvbiAyXHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIFdoZW4gc2V0IHRvIGB0cnVlYCwgY29kZWdlbiB3aWxsIGxvZyBnZW5lcmF0ZWQgY29kZSB0byBjb25zb2xlLiBVc2VmdWwgZm9yIGRlYnVnZ2luZy5cclxuICogQG5hbWUgdXRpbC5jb2RlZ2VuLnZlcmJvc2VcclxuICogQHR5cGUge2Jvb2xlYW59XHJcbiAqL1xyXG5jb2RlZ2VuLnZlcmJvc2UgPSBmYWxzZTtcclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbm1vZHVsZS5leHBvcnRzID0gRXZlbnRFbWl0dGVyO1xyXG5cclxuLyoqXHJcbiAqIENvbnN0cnVjdHMgYSBuZXcgZXZlbnQgZW1pdHRlciBpbnN0YW5jZS5cclxuICogQGNsYXNzZGVzYyBBIG1pbmltYWwgZXZlbnQgZW1pdHRlci5cclxuICogQG1lbWJlcm9mIHV0aWxcclxuICogQGNvbnN0cnVjdG9yXHJcbiAqL1xyXG5mdW5jdGlvbiBFdmVudEVtaXR0ZXIoKSB7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBSZWdpc3RlcmVkIGxpc3RlbmVycy5cclxuICAgICAqIEB0eXBlIHtPYmplY3QuPHN0cmluZywqPn1cclxuICAgICAqIEBwcml2YXRlXHJcbiAgICAgKi9cclxuICAgIHRoaXMuX2xpc3RlbmVycyA9IHt9O1xyXG59XHJcblxyXG4vKipcclxuICogUmVnaXN0ZXJzIGFuIGV2ZW50IGxpc3RlbmVyLlxyXG4gKiBAcGFyYW0ge3N0cmluZ30gZXZ0IEV2ZW50IG5hbWVcclxuICogQHBhcmFtIHtmdW5jdGlvbn0gZm4gTGlzdGVuZXJcclxuICogQHBhcmFtIHsqfSBbY3R4XSBMaXN0ZW5lciBjb250ZXh0XHJcbiAqIEByZXR1cm5zIHt1dGlsLkV2ZW50RW1pdHRlcn0gYHRoaXNgXHJcbiAqL1xyXG5FdmVudEVtaXR0ZXIucHJvdG90eXBlLm9uID0gZnVuY3Rpb24gb24oZXZ0LCBmbiwgY3R4KSB7XHJcbiAgICAodGhpcy5fbGlzdGVuZXJzW2V2dF0gfHwgKHRoaXMuX2xpc3RlbmVyc1tldnRdID0gW10pKS5wdXNoKHtcclxuICAgICAgICBmbiAgOiBmbixcclxuICAgICAgICBjdHggOiBjdHggfHwgdGhpc1xyXG4gICAgfSk7XHJcbiAgICByZXR1cm4gdGhpcztcclxufTtcclxuXHJcbi8qKlxyXG4gKiBSZW1vdmVzIGFuIGV2ZW50IGxpc3RlbmVyIG9yIGFueSBtYXRjaGluZyBsaXN0ZW5lcnMgaWYgYXJndW1lbnRzIGFyZSBvbWl0dGVkLlxyXG4gKiBAcGFyYW0ge3N0cmluZ30gW2V2dF0gRXZlbnQgbmFtZS4gUmVtb3ZlcyBhbGwgbGlzdGVuZXJzIGlmIG9taXR0ZWQuXHJcbiAqIEBwYXJhbSB7ZnVuY3Rpb259IFtmbl0gTGlzdGVuZXIgdG8gcmVtb3ZlLiBSZW1vdmVzIGFsbCBsaXN0ZW5lcnMgb2YgYGV2dGAgaWYgb21pdHRlZC5cclxuICogQHJldHVybnMge3V0aWwuRXZlbnRFbWl0dGVyfSBgdGhpc2BcclxuICovXHJcbkV2ZW50RW1pdHRlci5wcm90b3R5cGUub2ZmID0gZnVuY3Rpb24gb2ZmKGV2dCwgZm4pIHtcclxuICAgIGlmIChldnQgPT09IHVuZGVmaW5lZClcclxuICAgICAgICB0aGlzLl9saXN0ZW5lcnMgPSB7fTtcclxuICAgIGVsc2Uge1xyXG4gICAgICAgIGlmIChmbiA9PT0gdW5kZWZpbmVkKVxyXG4gICAgICAgICAgICB0aGlzLl9saXN0ZW5lcnNbZXZ0XSA9IFtdO1xyXG4gICAgICAgIGVsc2Uge1xyXG4gICAgICAgICAgICB2YXIgbGlzdGVuZXJzID0gdGhpcy5fbGlzdGVuZXJzW2V2dF07XHJcbiAgICAgICAgICAgIGZvciAodmFyIGkgPSAwOyBpIDwgbGlzdGVuZXJzLmxlbmd0aDspXHJcbiAgICAgICAgICAgICAgICBpZiAobGlzdGVuZXJzW2ldLmZuID09PSBmbilcclxuICAgICAgICAgICAgICAgICAgICBsaXN0ZW5lcnMuc3BsaWNlKGksIDEpO1xyXG4gICAgICAgICAgICAgICAgZWxzZVxyXG4gICAgICAgICAgICAgICAgICAgICsraTtcclxuICAgICAgICB9XHJcbiAgICB9XHJcbiAgICByZXR1cm4gdGhpcztcclxufTtcclxuXHJcbi8qKlxyXG4gKiBFbWl0cyBhbiBldmVudCBieSBjYWxsaW5nIGl0cyBsaXN0ZW5lcnMgd2l0aCB0aGUgc3BlY2lmaWVkIGFyZ3VtZW50cy5cclxuICogQHBhcmFtIHtzdHJpbmd9IGV2dCBFdmVudCBuYW1lXHJcbiAqIEBwYXJhbSB7Li4uKn0gYXJncyBBcmd1bWVudHNcclxuICogQHJldHVybnMge3V0aWwuRXZlbnRFbWl0dGVyfSBgdGhpc2BcclxuICovXHJcbkV2ZW50RW1pdHRlci5wcm90b3R5cGUuZW1pdCA9IGZ1bmN0aW9uIGVtaXQoZXZ0KSB7XHJcbiAgICB2YXIgbGlzdGVuZXJzID0gdGhpcy5fbGlzdGVuZXJzW2V2dF07XHJcbiAgICBpZiAobGlzdGVuZXJzKSB7XHJcbiAgICAgICAgdmFyIGFyZ3MgPSBbXSxcclxuICAgICAgICAgICAgaSA9IDE7XHJcbiAgICAgICAgZm9yICg7IGkgPCBhcmd1bWVudHMubGVuZ3RoOylcclxuICAgICAgICAgICAgYXJncy5wdXNoKGFyZ3VtZW50c1tpKytdKTtcclxuICAgICAgICBmb3IgKGkgPSAwOyBpIDwgbGlzdGVuZXJzLmxlbmd0aDspXHJcbiAgICAgICAgICAgIGxpc3RlbmVyc1tpXS5mbi5hcHBseShsaXN0ZW5lcnNbaSsrXS5jdHgsIGFyZ3MpO1xyXG4gICAgfVxyXG4gICAgcmV0dXJuIHRoaXM7XHJcbn07XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG5tb2R1bGUuZXhwb3J0cyA9IGZldGNoO1xyXG5cclxudmFyIGFzUHJvbWlzZSA9IHJlcXVpcmUoXCJAcHJvdG9idWZqcy9hc3Byb21pc2VcIiksXHJcbiAgICBpbnF1aXJlICAgPSByZXF1aXJlKFwiQHByb3RvYnVmanMvaW5xdWlyZVwiKTtcclxuXHJcbnZhciBmcyA9IGlucXVpcmUoXCJmc1wiKTtcclxuXHJcbi8qKlxyXG4gKiBOb2RlLXN0eWxlIGNhbGxiYWNrIGFzIHVzZWQgYnkge0BsaW5rIHV0aWwuZmV0Y2h9LlxyXG4gKiBAdHlwZWRlZiBGZXRjaENhbGxiYWNrXHJcbiAqIEB0eXBlIHtmdW5jdGlvbn1cclxuICogQHBhcmFtIHs/RXJyb3J9IGVycm9yIEVycm9yLCBpZiBhbnksIG90aGVyd2lzZSBgbnVsbGBcclxuICogQHBhcmFtIHtzdHJpbmd9IFtjb250ZW50c10gRmlsZSBjb250ZW50cywgaWYgdGhlcmUgaGFzbid0IGJlZW4gYW4gZXJyb3JcclxuICogQHJldHVybnMge3VuZGVmaW5lZH1cclxuICovXHJcblxyXG4vKipcclxuICogT3B0aW9ucyBhcyB1c2VkIGJ5IHtAbGluayB1dGlsLmZldGNofS5cclxuICogQHR5cGVkZWYgRmV0Y2hPcHRpb25zXHJcbiAqIEB0eXBlIHtPYmplY3R9XHJcbiAqIEBwcm9wZXJ0eSB7Ym9vbGVhbn0gW2JpbmFyeT1mYWxzZV0gV2hldGhlciBleHBlY3RpbmcgYSBiaW5hcnkgcmVzcG9uc2VcclxuICogQHByb3BlcnR5IHtib29sZWFufSBbeGhyPWZhbHNlXSBJZiBgdHJ1ZWAsIGZvcmNlcyB0aGUgdXNlIG9mIFhNTEh0dHBSZXF1ZXN0XHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIEZldGNoZXMgdGhlIGNvbnRlbnRzIG9mIGEgZmlsZS5cclxuICogQG1lbWJlcm9mIHV0aWxcclxuICogQHBhcmFtIHtzdHJpbmd9IGZpbGVuYW1lIEZpbGUgcGF0aCBvciB1cmxcclxuICogQHBhcmFtIHtGZXRjaE9wdGlvbnN9IG9wdGlvbnMgRmV0Y2ggb3B0aW9uc1xyXG4gKiBAcGFyYW0ge0ZldGNoQ2FsbGJhY2t9IGNhbGxiYWNrIENhbGxiYWNrIGZ1bmN0aW9uXHJcbiAqIEByZXR1cm5zIHt1bmRlZmluZWR9XHJcbiAqL1xyXG5mdW5jdGlvbiBmZXRjaChmaWxlbmFtZSwgb3B0aW9ucywgY2FsbGJhY2spIHtcclxuICAgIGlmICh0eXBlb2Ygb3B0aW9ucyA9PT0gXCJmdW5jdGlvblwiKSB7XHJcbiAgICAgICAgY2FsbGJhY2sgPSBvcHRpb25zO1xyXG4gICAgICAgIG9wdGlvbnMgPSB7fTtcclxuICAgIH0gZWxzZSBpZiAoIW9wdGlvbnMpXHJcbiAgICAgICAgb3B0aW9ucyA9IHt9O1xyXG5cclxuICAgIGlmICghY2FsbGJhY2spXHJcbiAgICAgICAgcmV0dXJuIGFzUHJvbWlzZShmZXRjaCwgdGhpcywgZmlsZW5hbWUsIG9wdGlvbnMpOyAvLyBlc2xpbnQtZGlzYWJsZS1saW5lIG5vLWludmFsaWQtdGhpc1xyXG5cclxuICAgIC8vIGlmIGEgbm9kZS1saWtlIGZpbGVzeXN0ZW0gaXMgcHJlc2VudCwgdHJ5IGl0IGZpcnN0IGJ1dCBmYWxsIGJhY2sgdG8gWEhSIGlmIG5vdGhpbmcgaXMgZm91bmQuXHJcbiAgICBpZiAoIW9wdGlvbnMueGhyICYmIGZzICYmIGZzLnJlYWRGaWxlKVxyXG4gICAgICAgIHJldHVybiBmcy5yZWFkRmlsZShmaWxlbmFtZSwgZnVuY3Rpb24gZmV0Y2hSZWFkRmlsZUNhbGxiYWNrKGVyciwgY29udGVudHMpIHtcclxuICAgICAgICAgICAgcmV0dXJuIGVyciAmJiB0eXBlb2YgWE1MSHR0cFJlcXVlc3QgIT09IFwidW5kZWZpbmVkXCJcclxuICAgICAgICAgICAgICAgID8gZmV0Y2gueGhyKGZpbGVuYW1lLCBvcHRpb25zLCBjYWxsYmFjaylcclxuICAgICAgICAgICAgICAgIDogZXJyXHJcbiAgICAgICAgICAgICAgICA/IGNhbGxiYWNrKGVycilcclxuICAgICAgICAgICAgICAgIDogY2FsbGJhY2sobnVsbCwgb3B0aW9ucy5iaW5hcnkgPyBjb250ZW50cyA6IGNvbnRlbnRzLnRvU3RyaW5nKFwidXRmOFwiKSk7XHJcbiAgICAgICAgfSk7XHJcblxyXG4gICAgLy8gdXNlIHRoZSBYSFIgdmVyc2lvbiBvdGhlcndpc2UuXHJcbiAgICByZXR1cm4gZmV0Y2gueGhyKGZpbGVuYW1lLCBvcHRpb25zLCBjYWxsYmFjayk7XHJcbn1cclxuXHJcbi8qKlxyXG4gKiBGZXRjaGVzIHRoZSBjb250ZW50cyBvZiBhIGZpbGUuXHJcbiAqIEBuYW1lIHV0aWwuZmV0Y2hcclxuICogQGZ1bmN0aW9uXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBwYXRoIEZpbGUgcGF0aCBvciB1cmxcclxuICogQHBhcmFtIHtGZXRjaENhbGxiYWNrfSBjYWxsYmFjayBDYWxsYmFjayBmdW5jdGlvblxyXG4gKiBAcmV0dXJucyB7dW5kZWZpbmVkfVxyXG4gKiBAdmFyaWF0aW9uIDJcclxuICovXHJcblxyXG4vKipcclxuICogRmV0Y2hlcyB0aGUgY29udGVudHMgb2YgYSBmaWxlLlxyXG4gKiBAbmFtZSB1dGlsLmZldGNoXHJcbiAqIEBmdW5jdGlvblxyXG4gKiBAcGFyYW0ge3N0cmluZ30gcGF0aCBGaWxlIHBhdGggb3IgdXJsXHJcbiAqIEBwYXJhbSB7RmV0Y2hPcHRpb25zfSBbb3B0aW9uc10gRmV0Y2ggb3B0aW9uc1xyXG4gKiBAcmV0dXJucyB7UHJvbWlzZTxzdHJpbmd8VWludDhBcnJheT59IFByb21pc2VcclxuICogQHZhcmlhdGlvbiAzXHJcbiAqL1xyXG5cclxuLyoqL1xyXG5mZXRjaC54aHIgPSBmdW5jdGlvbiBmZXRjaF94aHIoZmlsZW5hbWUsIG9wdGlvbnMsIGNhbGxiYWNrKSB7XHJcbiAgICB2YXIgeGhyID0gbmV3IFhNTEh0dHBSZXF1ZXN0KCk7XHJcbiAgICB4aHIub25yZWFkeXN0YXRlY2hhbmdlIC8qIHdvcmtzIGV2ZXJ5d2hlcmUgKi8gPSBmdW5jdGlvbiBmZXRjaE9uUmVhZHlTdGF0ZUNoYW5nZSgpIHtcclxuXHJcbiAgICAgICAgaWYgKHhoci5yZWFkeVN0YXRlICE9PSA0KVxyXG4gICAgICAgICAgICByZXR1cm4gdW5kZWZpbmVkO1xyXG5cclxuICAgICAgICAvLyBsb2NhbCBjb3JzIHNlY3VyaXR5IGVycm9ycyByZXR1cm4gc3RhdHVzIDAgLyBlbXB0eSBzdHJpbmcsIHRvby4gYWZhaWsgdGhpcyBjYW5ub3QgYmVcclxuICAgICAgICAvLyByZWxpYWJseSBkaXN0aW5ndWlzaGVkIGZyb20gYW4gYWN0dWFsbHkgZW1wdHkgZmlsZSBmb3Igc2VjdXJpdHkgcmVhc29ucy4gZmVlbCBmcmVlXHJcbiAgICAgICAgLy8gdG8gc2VuZCBhIHB1bGwgcmVxdWVzdCBpZiB5b3UgYXJlIGF3YXJlIG9mIGEgc29sdXRpb24uXHJcbiAgICAgICAgaWYgKHhoci5zdGF0dXMgIT09IDAgJiYgeGhyLnN0YXR1cyAhPT0gMjAwKVxyXG4gICAgICAgICAgICByZXR1cm4gY2FsbGJhY2soRXJyb3IoXCJzdGF0dXMgXCIgKyB4aHIuc3RhdHVzKSk7XHJcblxyXG4gICAgICAgIC8vIGlmIGJpbmFyeSBkYXRhIGlzIGV4cGVjdGVkLCBtYWtlIHN1cmUgdGhhdCBzb21lIHNvcnQgb2YgYXJyYXkgaXMgcmV0dXJuZWQsIGV2ZW4gaWZcclxuICAgICAgICAvLyBBcnJheUJ1ZmZlcnMgYXJlIG5vdCBzdXBwb3J0ZWQuIHRoZSBiaW5hcnkgc3RyaW5nIGZhbGxiYWNrLCBob3dldmVyLCBpcyB1bnNhZmUuXHJcbiAgICAgICAgaWYgKG9wdGlvbnMuYmluYXJ5KSB7XHJcbiAgICAgICAgICAgIHZhciBidWZmZXIgPSB4aHIucmVzcG9uc2U7XHJcbiAgICAgICAgICAgIGlmICghYnVmZmVyKSB7XHJcbiAgICAgICAgICAgICAgICBidWZmZXIgPSBbXTtcclxuICAgICAgICAgICAgICAgIGZvciAodmFyIGkgPSAwOyBpIDwgeGhyLnJlc3BvbnNlVGV4dC5sZW5ndGg7ICsraSlcclxuICAgICAgICAgICAgICAgICAgICBidWZmZXIucHVzaCh4aHIucmVzcG9uc2VUZXh0LmNoYXJDb2RlQXQoaSkgJiAyNTUpO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIHJldHVybiBjYWxsYmFjayhudWxsLCB0eXBlb2YgVWludDhBcnJheSAhPT0gXCJ1bmRlZmluZWRcIiA/IG5ldyBVaW50OEFycmF5KGJ1ZmZlcikgOiBidWZmZXIpO1xyXG4gICAgICAgIH1cclxuICAgICAgICByZXR1cm4gY2FsbGJhY2sobnVsbCwgeGhyLnJlc3BvbnNlVGV4dCk7XHJcbiAgICB9O1xyXG5cclxuICAgIGlmIChvcHRpb25zLmJpbmFyeSkge1xyXG4gICAgICAgIC8vIHJlZjogaHR0cHM6Ly9kZXZlbG9wZXIubW96aWxsYS5vcmcvZW4tVVMvZG9jcy9XZWIvQVBJL1hNTEh0dHBSZXF1ZXN0L1NlbmRpbmdfYW5kX1JlY2VpdmluZ19CaW5hcnlfRGF0YSNSZWNlaXZpbmdfYmluYXJ5X2RhdGFfaW5fb2xkZXJfYnJvd3NlcnNcclxuICAgICAgICBpZiAoXCJvdmVycmlkZU1pbWVUeXBlXCIgaW4geGhyKVxyXG4gICAgICAgICAgICB4aHIub3ZlcnJpZGVNaW1lVHlwZShcInRleHQvcGxhaW47IGNoYXJzZXQ9eC11c2VyLWRlZmluZWRcIik7XHJcbiAgICAgICAgeGhyLnJlc3BvbnNlVHlwZSA9IFwiYXJyYXlidWZmZXJcIjtcclxuICAgIH1cclxuXHJcbiAgICB4aHIub3BlbihcIkdFVFwiLCBmaWxlbmFtZSk7XHJcbiAgICB4aHIuc2VuZCgpO1xyXG59O1xyXG4iLCJcInVzZSBzdHJpY3RcIjtcclxuXHJcbm1vZHVsZS5leHBvcnRzID0gZmFjdG9yeShmYWN0b3J5KTtcclxuXHJcbi8qKlxyXG4gKiBSZWFkcyAvIHdyaXRlcyBmbG9hdHMgLyBkb3VibGVzIGZyb20gLyB0byBidWZmZXJzLlxyXG4gKiBAbmFtZSB1dGlsLmZsb2F0XHJcbiAqIEBuYW1lc3BhY2VcclxuICovXHJcblxyXG4vKipcclxuICogV3JpdGVzIGEgMzIgYml0IGZsb2F0IHRvIGEgYnVmZmVyIHVzaW5nIGxpdHRsZSBlbmRpYW4gYnl0ZSBvcmRlci5cclxuICogQG5hbWUgdXRpbC5mbG9hdC53cml0ZUZsb2F0TEVcclxuICogQGZ1bmN0aW9uXHJcbiAqIEBwYXJhbSB7bnVtYmVyfSB2YWwgVmFsdWUgdG8gd3JpdGVcclxuICogQHBhcmFtIHtVaW50OEFycmF5fSBidWYgVGFyZ2V0IGJ1ZmZlclxyXG4gKiBAcGFyYW0ge251bWJlcn0gcG9zIFRhcmdldCBidWZmZXIgb2Zmc2V0XHJcbiAqIEByZXR1cm5zIHt1bmRlZmluZWR9XHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIFdyaXRlcyBhIDMyIGJpdCBmbG9hdCB0byBhIGJ1ZmZlciB1c2luZyBiaWcgZW5kaWFuIGJ5dGUgb3JkZXIuXHJcbiAqIEBuYW1lIHV0aWwuZmxvYXQud3JpdGVGbG9hdEJFXHJcbiAqIEBmdW5jdGlvblxyXG4gKiBAcGFyYW0ge251bWJlcn0gdmFsIFZhbHVlIHRvIHdyaXRlXHJcbiAqIEBwYXJhbSB7VWludDhBcnJheX0gYnVmIFRhcmdldCBidWZmZXJcclxuICogQHBhcmFtIHtudW1iZXJ9IHBvcyBUYXJnZXQgYnVmZmVyIG9mZnNldFxyXG4gKiBAcmV0dXJucyB7dW5kZWZpbmVkfVxyXG4gKi9cclxuXHJcbi8qKlxyXG4gKiBSZWFkcyBhIDMyIGJpdCBmbG9hdCBmcm9tIGEgYnVmZmVyIHVzaW5nIGxpdHRsZSBlbmRpYW4gYnl0ZSBvcmRlci5cclxuICogQG5hbWUgdXRpbC5mbG9hdC5yZWFkRmxvYXRMRVxyXG4gKiBAZnVuY3Rpb25cclxuICogQHBhcmFtIHtVaW50OEFycmF5fSBidWYgU291cmNlIGJ1ZmZlclxyXG4gKiBAcGFyYW0ge251bWJlcn0gcG9zIFNvdXJjZSBidWZmZXIgb2Zmc2V0XHJcbiAqIEByZXR1cm5zIHtudW1iZXJ9IFZhbHVlIHJlYWRcclxuICovXHJcblxyXG4vKipcclxuICogUmVhZHMgYSAzMiBiaXQgZmxvYXQgZnJvbSBhIGJ1ZmZlciB1c2luZyBiaWcgZW5kaWFuIGJ5dGUgb3JkZXIuXHJcbiAqIEBuYW1lIHV0aWwuZmxvYXQucmVhZEZsb2F0QkVcclxuICogQGZ1bmN0aW9uXHJcbiAqIEBwYXJhbSB7VWludDhBcnJheX0gYnVmIFNvdXJjZSBidWZmZXJcclxuICogQHBhcmFtIHtudW1iZXJ9IHBvcyBTb3VyY2UgYnVmZmVyIG9mZnNldFxyXG4gKiBAcmV0dXJucyB7bnVtYmVyfSBWYWx1ZSByZWFkXHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIFdyaXRlcyBhIDY0IGJpdCBkb3VibGUgdG8gYSBidWZmZXIgdXNpbmcgbGl0dGxlIGVuZGlhbiBieXRlIG9yZGVyLlxyXG4gKiBAbmFtZSB1dGlsLmZsb2F0LndyaXRlRG91YmxlTEVcclxuICogQGZ1bmN0aW9uXHJcbiAqIEBwYXJhbSB7bnVtYmVyfSB2YWwgVmFsdWUgdG8gd3JpdGVcclxuICogQHBhcmFtIHtVaW50OEFycmF5fSBidWYgVGFyZ2V0IGJ1ZmZlclxyXG4gKiBAcGFyYW0ge251bWJlcn0gcG9zIFRhcmdldCBidWZmZXIgb2Zmc2V0XHJcbiAqIEByZXR1cm5zIHt1bmRlZmluZWR9XHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIFdyaXRlcyBhIDY0IGJpdCBkb3VibGUgdG8gYSBidWZmZXIgdXNpbmcgYmlnIGVuZGlhbiBieXRlIG9yZGVyLlxyXG4gKiBAbmFtZSB1dGlsLmZsb2F0LndyaXRlRG91YmxlQkVcclxuICogQGZ1bmN0aW9uXHJcbiAqIEBwYXJhbSB7bnVtYmVyfSB2YWwgVmFsdWUgdG8gd3JpdGVcclxuICogQHBhcmFtIHtVaW50OEFycmF5fSBidWYgVGFyZ2V0IGJ1ZmZlclxyXG4gKiBAcGFyYW0ge251bWJlcn0gcG9zIFRhcmdldCBidWZmZXIgb2Zmc2V0XHJcbiAqIEByZXR1cm5zIHt1bmRlZmluZWR9XHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIFJlYWRzIGEgNjQgYml0IGRvdWJsZSBmcm9tIGEgYnVmZmVyIHVzaW5nIGxpdHRsZSBlbmRpYW4gYnl0ZSBvcmRlci5cclxuICogQG5hbWUgdXRpbC5mbG9hdC5yZWFkRG91YmxlTEVcclxuICogQGZ1bmN0aW9uXHJcbiAqIEBwYXJhbSB7VWludDhBcnJheX0gYnVmIFNvdXJjZSBidWZmZXJcclxuICogQHBhcmFtIHtudW1iZXJ9IHBvcyBTb3VyY2UgYnVmZmVyIG9mZnNldFxyXG4gKiBAcmV0dXJucyB7bnVtYmVyfSBWYWx1ZSByZWFkXHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIFJlYWRzIGEgNjQgYml0IGRvdWJsZSBmcm9tIGEgYnVmZmVyIHVzaW5nIGJpZyBlbmRpYW4gYnl0ZSBvcmRlci5cclxuICogQG5hbWUgdXRpbC5mbG9hdC5yZWFkRG91YmxlQkVcclxuICogQGZ1bmN0aW9uXHJcbiAqIEBwYXJhbSB7VWludDhBcnJheX0gYnVmIFNvdXJjZSBidWZmZXJcclxuICogQHBhcmFtIHtudW1iZXJ9IHBvcyBTb3VyY2UgYnVmZmVyIG9mZnNldFxyXG4gKiBAcmV0dXJucyB7bnVtYmVyfSBWYWx1ZSByZWFkXHJcbiAqL1xyXG5cclxuLy8gRmFjdG9yeSBmdW5jdGlvbiBmb3IgdGhlIHB1cnBvc2Ugb2Ygbm9kZS1iYXNlZCB0ZXN0aW5nIGluIG1vZGlmaWVkIGdsb2JhbCBlbnZpcm9ubWVudHNcclxuZnVuY3Rpb24gZmFjdG9yeShleHBvcnRzKSB7XHJcblxyXG4gICAgLy8gZmxvYXQ6IHR5cGVkIGFycmF5XHJcbiAgICBpZiAodHlwZW9mIEZsb2F0MzJBcnJheSAhPT0gXCJ1bmRlZmluZWRcIikgKGZ1bmN0aW9uKCkge1xyXG5cclxuICAgICAgICB2YXIgZjMyID0gbmV3IEZsb2F0MzJBcnJheShbIC0wIF0pLFxyXG4gICAgICAgICAgICBmOGIgPSBuZXcgVWludDhBcnJheShmMzIuYnVmZmVyKSxcclxuICAgICAgICAgICAgbGUgID0gZjhiWzNdID09PSAxMjg7XHJcblxyXG4gICAgICAgIGZ1bmN0aW9uIHdyaXRlRmxvYXRfZjMyX2NweSh2YWwsIGJ1ZiwgcG9zKSB7XHJcbiAgICAgICAgICAgIGYzMlswXSA9IHZhbDtcclxuICAgICAgICAgICAgYnVmW3BvcyAgICBdID0gZjhiWzBdO1xyXG4gICAgICAgICAgICBidWZbcG9zICsgMV0gPSBmOGJbMV07XHJcbiAgICAgICAgICAgIGJ1Zltwb3MgKyAyXSA9IGY4YlsyXTtcclxuICAgICAgICAgICAgYnVmW3BvcyArIDNdID0gZjhiWzNdO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgZnVuY3Rpb24gd3JpdGVGbG9hdF9mMzJfcmV2KHZhbCwgYnVmLCBwb3MpIHtcclxuICAgICAgICAgICAgZjMyWzBdID0gdmFsO1xyXG4gICAgICAgICAgICBidWZbcG9zICAgIF0gPSBmOGJbM107XHJcbiAgICAgICAgICAgIGJ1Zltwb3MgKyAxXSA9IGY4YlsyXTtcclxuICAgICAgICAgICAgYnVmW3BvcyArIDJdID0gZjhiWzFdO1xyXG4gICAgICAgICAgICBidWZbcG9zICsgM10gPSBmOGJbMF07XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgbmV4dCAqL1xyXG4gICAgICAgIGV4cG9ydHMud3JpdGVGbG9hdExFID0gbGUgPyB3cml0ZUZsb2F0X2YzMl9jcHkgOiB3cml0ZUZsb2F0X2YzMl9yZXY7XHJcbiAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIG5leHQgKi9cclxuICAgICAgICBleHBvcnRzLndyaXRlRmxvYXRCRSA9IGxlID8gd3JpdGVGbG9hdF9mMzJfcmV2IDogd3JpdGVGbG9hdF9mMzJfY3B5O1xyXG5cclxuICAgICAgICBmdW5jdGlvbiByZWFkRmxvYXRfZjMyX2NweShidWYsIHBvcykge1xyXG4gICAgICAgICAgICBmOGJbMF0gPSBidWZbcG9zICAgIF07XHJcbiAgICAgICAgICAgIGY4YlsxXSA9IGJ1Zltwb3MgKyAxXTtcclxuICAgICAgICAgICAgZjhiWzJdID0gYnVmW3BvcyArIDJdO1xyXG4gICAgICAgICAgICBmOGJbM10gPSBidWZbcG9zICsgM107XHJcbiAgICAgICAgICAgIHJldHVybiBmMzJbMF07XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICBmdW5jdGlvbiByZWFkRmxvYXRfZjMyX3JldihidWYsIHBvcykge1xyXG4gICAgICAgICAgICBmOGJbM10gPSBidWZbcG9zICAgIF07XHJcbiAgICAgICAgICAgIGY4YlsyXSA9IGJ1Zltwb3MgKyAxXTtcclxuICAgICAgICAgICAgZjhiWzFdID0gYnVmW3BvcyArIDJdO1xyXG4gICAgICAgICAgICBmOGJbMF0gPSBidWZbcG9zICsgM107XHJcbiAgICAgICAgICAgIHJldHVybiBmMzJbMF07XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgbmV4dCAqL1xyXG4gICAgICAgIGV4cG9ydHMucmVhZEZsb2F0TEUgPSBsZSA/IHJlYWRGbG9hdF9mMzJfY3B5IDogcmVhZEZsb2F0X2YzMl9yZXY7XHJcbiAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIG5leHQgKi9cclxuICAgICAgICBleHBvcnRzLnJlYWRGbG9hdEJFID0gbGUgPyByZWFkRmxvYXRfZjMyX3JldiA6IHJlYWRGbG9hdF9mMzJfY3B5O1xyXG5cclxuICAgIC8vIGZsb2F0OiBpZWVlNzU0XHJcbiAgICB9KSgpOyBlbHNlIChmdW5jdGlvbigpIHtcclxuXHJcbiAgICAgICAgZnVuY3Rpb24gd3JpdGVGbG9hdF9pZWVlNzU0KHdyaXRlVWludCwgdmFsLCBidWYsIHBvcykge1xyXG4gICAgICAgICAgICB2YXIgc2lnbiA9IHZhbCA8IDAgPyAxIDogMDtcclxuICAgICAgICAgICAgaWYgKHNpZ24pXHJcbiAgICAgICAgICAgICAgICB2YWwgPSAtdmFsO1xyXG4gICAgICAgICAgICBpZiAodmFsID09PSAwKVxyXG4gICAgICAgICAgICAgICAgd3JpdGVVaW50KDEgLyB2YWwgPiAwID8gLyogcG9zaXRpdmUgKi8gMCA6IC8qIG5lZ2F0aXZlIDAgKi8gMjE0NzQ4MzY0OCwgYnVmLCBwb3MpO1xyXG4gICAgICAgICAgICBlbHNlIGlmIChpc05hTih2YWwpKVxyXG4gICAgICAgICAgICAgICAgd3JpdGVVaW50KDIxNDMyODkzNDQsIGJ1ZiwgcG9zKTtcclxuICAgICAgICAgICAgZWxzZSBpZiAodmFsID4gMy40MDI4MjM0NjYzODUyODg2ZSszOCkgLy8gKy1JbmZpbml0eVxyXG4gICAgICAgICAgICAgICAgd3JpdGVVaW50KChzaWduIDw8IDMxIHwgMjEzOTA5NTA0MCkgPj4+IDAsIGJ1ZiwgcG9zKTtcclxuICAgICAgICAgICAgZWxzZSBpZiAodmFsIDwgMS4xNzU0OTQzNTA4MjIyODc1ZS0zOCkgLy8gZGVub3JtYWxcclxuICAgICAgICAgICAgICAgIHdyaXRlVWludCgoc2lnbiA8PCAzMSB8IE1hdGgucm91bmQodmFsIC8gMS40MDEyOTg0NjQzMjQ4MTdlLTQ1KSkgPj4+IDAsIGJ1ZiwgcG9zKTtcclxuICAgICAgICAgICAgZWxzZSB7XHJcbiAgICAgICAgICAgICAgICB2YXIgZXhwb25lbnQgPSBNYXRoLmZsb29yKE1hdGgubG9nKHZhbCkgLyBNYXRoLkxOMiksXHJcbiAgICAgICAgICAgICAgICAgICAgbWFudGlzc2EgPSBNYXRoLnJvdW5kKHZhbCAqIE1hdGgucG93KDIsIC1leHBvbmVudCkgKiA4Mzg4NjA4KSAmIDgzODg2MDc7XHJcbiAgICAgICAgICAgICAgICB3cml0ZVVpbnQoKHNpZ24gPDwgMzEgfCBleHBvbmVudCArIDEyNyA8PCAyMyB8IG1hbnRpc3NhKSA+Pj4gMCwgYnVmLCBwb3MpO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICBleHBvcnRzLndyaXRlRmxvYXRMRSA9IHdyaXRlRmxvYXRfaWVlZTc1NC5iaW5kKG51bGwsIHdyaXRlVWludExFKTtcclxuICAgICAgICBleHBvcnRzLndyaXRlRmxvYXRCRSA9IHdyaXRlRmxvYXRfaWVlZTc1NC5iaW5kKG51bGwsIHdyaXRlVWludEJFKTtcclxuXHJcbiAgICAgICAgZnVuY3Rpb24gcmVhZEZsb2F0X2llZWU3NTQocmVhZFVpbnQsIGJ1ZiwgcG9zKSB7XHJcbiAgICAgICAgICAgIHZhciB1aW50ID0gcmVhZFVpbnQoYnVmLCBwb3MpLFxyXG4gICAgICAgICAgICAgICAgc2lnbiA9ICh1aW50ID4+IDMxKSAqIDIgKyAxLFxyXG4gICAgICAgICAgICAgICAgZXhwb25lbnQgPSB1aW50ID4+PiAyMyAmIDI1NSxcclxuICAgICAgICAgICAgICAgIG1hbnRpc3NhID0gdWludCAmIDgzODg2MDc7XHJcbiAgICAgICAgICAgIHJldHVybiBleHBvbmVudCA9PT0gMjU1XHJcbiAgICAgICAgICAgICAgICA/IG1hbnRpc3NhXHJcbiAgICAgICAgICAgICAgICA/IE5hTlxyXG4gICAgICAgICAgICAgICAgOiBzaWduICogSW5maW5pdHlcclxuICAgICAgICAgICAgICAgIDogZXhwb25lbnQgPT09IDAgLy8gZGVub3JtYWxcclxuICAgICAgICAgICAgICAgID8gc2lnbiAqIDEuNDAxMjk4NDY0MzI0ODE3ZS00NSAqIG1hbnRpc3NhXHJcbiAgICAgICAgICAgICAgICA6IHNpZ24gKiBNYXRoLnBvdygyLCBleHBvbmVudCAtIDE1MCkgKiAobWFudGlzc2EgKyA4Mzg4NjA4KTtcclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIGV4cG9ydHMucmVhZEZsb2F0TEUgPSByZWFkRmxvYXRfaWVlZTc1NC5iaW5kKG51bGwsIHJlYWRVaW50TEUpO1xyXG4gICAgICAgIGV4cG9ydHMucmVhZEZsb2F0QkUgPSByZWFkRmxvYXRfaWVlZTc1NC5iaW5kKG51bGwsIHJlYWRVaW50QkUpO1xyXG5cclxuICAgIH0pKCk7XHJcblxyXG4gICAgLy8gZG91YmxlOiB0eXBlZCBhcnJheVxyXG4gICAgaWYgKHR5cGVvZiBGbG9hdDY0QXJyYXkgIT09IFwidW5kZWZpbmVkXCIpIChmdW5jdGlvbigpIHtcclxuXHJcbiAgICAgICAgdmFyIGY2NCA9IG5ldyBGbG9hdDY0QXJyYXkoWy0wXSksXHJcbiAgICAgICAgICAgIGY4YiA9IG5ldyBVaW50OEFycmF5KGY2NC5idWZmZXIpLFxyXG4gICAgICAgICAgICBsZSAgPSBmOGJbN10gPT09IDEyODtcclxuXHJcbiAgICAgICAgZnVuY3Rpb24gd3JpdGVEb3VibGVfZjY0X2NweSh2YWwsIGJ1ZiwgcG9zKSB7XHJcbiAgICAgICAgICAgIGY2NFswXSA9IHZhbDtcclxuICAgICAgICAgICAgYnVmW3BvcyAgICBdID0gZjhiWzBdO1xyXG4gICAgICAgICAgICBidWZbcG9zICsgMV0gPSBmOGJbMV07XHJcbiAgICAgICAgICAgIGJ1Zltwb3MgKyAyXSA9IGY4YlsyXTtcclxuICAgICAgICAgICAgYnVmW3BvcyArIDNdID0gZjhiWzNdO1xyXG4gICAgICAgICAgICBidWZbcG9zICsgNF0gPSBmOGJbNF07XHJcbiAgICAgICAgICAgIGJ1Zltwb3MgKyA1XSA9IGY4Yls1XTtcclxuICAgICAgICAgICAgYnVmW3BvcyArIDZdID0gZjhiWzZdO1xyXG4gICAgICAgICAgICBidWZbcG9zICsgN10gPSBmOGJbN107XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICBmdW5jdGlvbiB3cml0ZURvdWJsZV9mNjRfcmV2KHZhbCwgYnVmLCBwb3MpIHtcclxuICAgICAgICAgICAgZjY0WzBdID0gdmFsO1xyXG4gICAgICAgICAgICBidWZbcG9zICAgIF0gPSBmOGJbN107XHJcbiAgICAgICAgICAgIGJ1Zltwb3MgKyAxXSA9IGY4Yls2XTtcclxuICAgICAgICAgICAgYnVmW3BvcyArIDJdID0gZjhiWzVdO1xyXG4gICAgICAgICAgICBidWZbcG9zICsgM10gPSBmOGJbNF07XHJcbiAgICAgICAgICAgIGJ1Zltwb3MgKyA0XSA9IGY4YlszXTtcclxuICAgICAgICAgICAgYnVmW3BvcyArIDVdID0gZjhiWzJdO1xyXG4gICAgICAgICAgICBidWZbcG9zICsgNl0gPSBmOGJbMV07XHJcbiAgICAgICAgICAgIGJ1Zltwb3MgKyA3XSA9IGY4YlswXTtcclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0ICovXHJcbiAgICAgICAgZXhwb3J0cy53cml0ZURvdWJsZUxFID0gbGUgPyB3cml0ZURvdWJsZV9mNjRfY3B5IDogd3JpdGVEb3VibGVfZjY0X3JldjtcclxuICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgbmV4dCAqL1xyXG4gICAgICAgIGV4cG9ydHMud3JpdGVEb3VibGVCRSA9IGxlID8gd3JpdGVEb3VibGVfZjY0X3JldiA6IHdyaXRlRG91YmxlX2Y2NF9jcHk7XHJcblxyXG4gICAgICAgIGZ1bmN0aW9uIHJlYWREb3VibGVfZjY0X2NweShidWYsIHBvcykge1xyXG4gICAgICAgICAgICBmOGJbMF0gPSBidWZbcG9zICAgIF07XHJcbiAgICAgICAgICAgIGY4YlsxXSA9IGJ1Zltwb3MgKyAxXTtcclxuICAgICAgICAgICAgZjhiWzJdID0gYnVmW3BvcyArIDJdO1xyXG4gICAgICAgICAgICBmOGJbM10gPSBidWZbcG9zICsgM107XHJcbiAgICAgICAgICAgIGY4Yls0XSA9IGJ1Zltwb3MgKyA0XTtcclxuICAgICAgICAgICAgZjhiWzVdID0gYnVmW3BvcyArIDVdO1xyXG4gICAgICAgICAgICBmOGJbNl0gPSBidWZbcG9zICsgNl07XHJcbiAgICAgICAgICAgIGY4Yls3XSA9IGJ1Zltwb3MgKyA3XTtcclxuICAgICAgICAgICAgcmV0dXJuIGY2NFswXTtcclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIGZ1bmN0aW9uIHJlYWREb3VibGVfZjY0X3JldihidWYsIHBvcykge1xyXG4gICAgICAgICAgICBmOGJbN10gPSBidWZbcG9zICAgIF07XHJcbiAgICAgICAgICAgIGY4Yls2XSA9IGJ1Zltwb3MgKyAxXTtcclxuICAgICAgICAgICAgZjhiWzVdID0gYnVmW3BvcyArIDJdO1xyXG4gICAgICAgICAgICBmOGJbNF0gPSBidWZbcG9zICsgM107XHJcbiAgICAgICAgICAgIGY4YlszXSA9IGJ1Zltwb3MgKyA0XTtcclxuICAgICAgICAgICAgZjhiWzJdID0gYnVmW3BvcyArIDVdO1xyXG4gICAgICAgICAgICBmOGJbMV0gPSBidWZbcG9zICsgNl07XHJcbiAgICAgICAgICAgIGY4YlswXSA9IGJ1Zltwb3MgKyA3XTtcclxuICAgICAgICAgICAgcmV0dXJuIGY2NFswXTtcclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0ICovXHJcbiAgICAgICAgZXhwb3J0cy5yZWFkRG91YmxlTEUgPSBsZSA/IHJlYWREb3VibGVfZjY0X2NweSA6IHJlYWREb3VibGVfZjY0X3JldjtcclxuICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgbmV4dCAqL1xyXG4gICAgICAgIGV4cG9ydHMucmVhZERvdWJsZUJFID0gbGUgPyByZWFkRG91YmxlX2Y2NF9yZXYgOiByZWFkRG91YmxlX2Y2NF9jcHk7XHJcblxyXG4gICAgLy8gZG91YmxlOiBpZWVlNzU0XHJcbiAgICB9KSgpOyBlbHNlIChmdW5jdGlvbigpIHtcclxuXHJcbiAgICAgICAgZnVuY3Rpb24gd3JpdGVEb3VibGVfaWVlZTc1NCh3cml0ZVVpbnQsIG9mZjAsIG9mZjEsIHZhbCwgYnVmLCBwb3MpIHtcclxuICAgICAgICAgICAgdmFyIHNpZ24gPSB2YWwgPCAwID8gMSA6IDA7XHJcbiAgICAgICAgICAgIGlmIChzaWduKVxyXG4gICAgICAgICAgICAgICAgdmFsID0gLXZhbDtcclxuICAgICAgICAgICAgaWYgKHZhbCA9PT0gMCkge1xyXG4gICAgICAgICAgICAgICAgd3JpdGVVaW50KDAsIGJ1ZiwgcG9zICsgb2ZmMCk7XHJcbiAgICAgICAgICAgICAgICB3cml0ZVVpbnQoMSAvIHZhbCA+IDAgPyAvKiBwb3NpdGl2ZSAqLyAwIDogLyogbmVnYXRpdmUgMCAqLyAyMTQ3NDgzNjQ4LCBidWYsIHBvcyArIG9mZjEpO1xyXG4gICAgICAgICAgICB9IGVsc2UgaWYgKGlzTmFOKHZhbCkpIHtcclxuICAgICAgICAgICAgICAgIHdyaXRlVWludCgwLCBidWYsIHBvcyArIG9mZjApO1xyXG4gICAgICAgICAgICAgICAgd3JpdGVVaW50KDIxNDY5NTkzNjAsIGJ1ZiwgcG9zICsgb2ZmMSk7XHJcbiAgICAgICAgICAgIH0gZWxzZSBpZiAodmFsID4gMS43OTc2OTMxMzQ4NjIzMTU3ZSszMDgpIHsgLy8gKy1JbmZpbml0eVxyXG4gICAgICAgICAgICAgICAgd3JpdGVVaW50KDAsIGJ1ZiwgcG9zICsgb2ZmMCk7XHJcbiAgICAgICAgICAgICAgICB3cml0ZVVpbnQoKHNpZ24gPDwgMzEgfCAyMTQ2NDM1MDcyKSA+Pj4gMCwgYnVmLCBwb3MgKyBvZmYxKTtcclxuICAgICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgICAgIHZhciBtYW50aXNzYTtcclxuICAgICAgICAgICAgICAgIGlmICh2YWwgPCAyLjIyNTA3Mzg1ODUwNzIwMTRlLTMwOCkgeyAvLyBkZW5vcm1hbFxyXG4gICAgICAgICAgICAgICAgICAgIG1hbnRpc3NhID0gdmFsIC8gNWUtMzI0O1xyXG4gICAgICAgICAgICAgICAgICAgIHdyaXRlVWludChtYW50aXNzYSA+Pj4gMCwgYnVmLCBwb3MgKyBvZmYwKTtcclxuICAgICAgICAgICAgICAgICAgICB3cml0ZVVpbnQoKHNpZ24gPDwgMzEgfCBtYW50aXNzYSAvIDQyOTQ5NjcyOTYpID4+PiAwLCBidWYsIHBvcyArIG9mZjEpO1xyXG4gICAgICAgICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgICAgICAgICB2YXIgZXhwb25lbnQgPSBNYXRoLmZsb29yKE1hdGgubG9nKHZhbCkgLyBNYXRoLkxOMik7XHJcbiAgICAgICAgICAgICAgICAgICAgaWYgKGV4cG9uZW50ID09PSAxMDI0KVxyXG4gICAgICAgICAgICAgICAgICAgICAgICBleHBvbmVudCA9IDEwMjM7XHJcbiAgICAgICAgICAgICAgICAgICAgbWFudGlzc2EgPSB2YWwgKiBNYXRoLnBvdygyLCAtZXhwb25lbnQpO1xyXG4gICAgICAgICAgICAgICAgICAgIHdyaXRlVWludChtYW50aXNzYSAqIDQ1MDM1OTk2MjczNzA0OTYgPj4+IDAsIGJ1ZiwgcG9zICsgb2ZmMCk7XHJcbiAgICAgICAgICAgICAgICAgICAgd3JpdGVVaW50KChzaWduIDw8IDMxIHwgZXhwb25lbnQgKyAxMDIzIDw8IDIwIHwgbWFudGlzc2EgKiAxMDQ4NTc2ICYgMTA0ODU3NSkgPj4+IDAsIGJ1ZiwgcG9zICsgb2ZmMSk7XHJcbiAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIGV4cG9ydHMud3JpdGVEb3VibGVMRSA9IHdyaXRlRG91YmxlX2llZWU3NTQuYmluZChudWxsLCB3cml0ZVVpbnRMRSwgMCwgNCk7XHJcbiAgICAgICAgZXhwb3J0cy53cml0ZURvdWJsZUJFID0gd3JpdGVEb3VibGVfaWVlZTc1NC5iaW5kKG51bGwsIHdyaXRlVWludEJFLCA0LCAwKTtcclxuXHJcbiAgICAgICAgZnVuY3Rpb24gcmVhZERvdWJsZV9pZWVlNzU0KHJlYWRVaW50LCBvZmYwLCBvZmYxLCBidWYsIHBvcykge1xyXG4gICAgICAgICAgICB2YXIgbG8gPSByZWFkVWludChidWYsIHBvcyArIG9mZjApLFxyXG4gICAgICAgICAgICAgICAgaGkgPSByZWFkVWludChidWYsIHBvcyArIG9mZjEpO1xyXG4gICAgICAgICAgICB2YXIgc2lnbiA9IChoaSA+PiAzMSkgKiAyICsgMSxcclxuICAgICAgICAgICAgICAgIGV4cG9uZW50ID0gaGkgPj4+IDIwICYgMjA0NyxcclxuICAgICAgICAgICAgICAgIG1hbnRpc3NhID0gNDI5NDk2NzI5NiAqIChoaSAmIDEwNDg1NzUpICsgbG87XHJcbiAgICAgICAgICAgIHJldHVybiBleHBvbmVudCA9PT0gMjA0N1xyXG4gICAgICAgICAgICAgICAgPyBtYW50aXNzYVxyXG4gICAgICAgICAgICAgICAgPyBOYU5cclxuICAgICAgICAgICAgICAgIDogc2lnbiAqIEluZmluaXR5XHJcbiAgICAgICAgICAgICAgICA6IGV4cG9uZW50ID09PSAwIC8vIGRlbm9ybWFsXHJcbiAgICAgICAgICAgICAgICA/IHNpZ24gKiA1ZS0zMjQgKiBtYW50aXNzYVxyXG4gICAgICAgICAgICAgICAgOiBzaWduICogTWF0aC5wb3coMiwgZXhwb25lbnQgLSAxMDc1KSAqIChtYW50aXNzYSArIDQ1MDM1OTk2MjczNzA0OTYpO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgZXhwb3J0cy5yZWFkRG91YmxlTEUgPSByZWFkRG91YmxlX2llZWU3NTQuYmluZChudWxsLCByZWFkVWludExFLCAwLCA0KTtcclxuICAgICAgICBleHBvcnRzLnJlYWREb3VibGVCRSA9IHJlYWREb3VibGVfaWVlZTc1NC5iaW5kKG51bGwsIHJlYWRVaW50QkUsIDQsIDApO1xyXG5cclxuICAgIH0pKCk7XHJcblxyXG4gICAgcmV0dXJuIGV4cG9ydHM7XHJcbn1cclxuXHJcbi8vIHVpbnQgaGVscGVyc1xyXG5cclxuZnVuY3Rpb24gd3JpdGVVaW50TEUodmFsLCBidWYsIHBvcykge1xyXG4gICAgYnVmW3BvcyAgICBdID0gIHZhbCAgICAgICAgJiAyNTU7XHJcbiAgICBidWZbcG9zICsgMV0gPSAgdmFsID4+PiA4ICAmIDI1NTtcclxuICAgIGJ1Zltwb3MgKyAyXSA9ICB2YWwgPj4+IDE2ICYgMjU1O1xyXG4gICAgYnVmW3BvcyArIDNdID0gIHZhbCA+Pj4gMjQ7XHJcbn1cclxuXHJcbmZ1bmN0aW9uIHdyaXRlVWludEJFKHZhbCwgYnVmLCBwb3MpIHtcclxuICAgIGJ1Zltwb3MgICAgXSA9ICB2YWwgPj4+IDI0O1xyXG4gICAgYnVmW3BvcyArIDFdID0gIHZhbCA+Pj4gMTYgJiAyNTU7XHJcbiAgICBidWZbcG9zICsgMl0gPSAgdmFsID4+PiA4ICAmIDI1NTtcclxuICAgIGJ1Zltwb3MgKyAzXSA9ICB2YWwgICAgICAgICYgMjU1O1xyXG59XHJcblxyXG5mdW5jdGlvbiByZWFkVWludExFKGJ1ZiwgcG9zKSB7XHJcbiAgICByZXR1cm4gKGJ1Zltwb3MgICAgXVxyXG4gICAgICAgICAgfCBidWZbcG9zICsgMV0gPDwgOFxyXG4gICAgICAgICAgfCBidWZbcG9zICsgMl0gPDwgMTZcclxuICAgICAgICAgIHwgYnVmW3BvcyArIDNdIDw8IDI0KSA+Pj4gMDtcclxufVxyXG5cclxuZnVuY3Rpb24gcmVhZFVpbnRCRShidWYsIHBvcykge1xyXG4gICAgcmV0dXJuIChidWZbcG9zICAgIF0gPDwgMjRcclxuICAgICAgICAgIHwgYnVmW3BvcyArIDFdIDw8IDE2XHJcbiAgICAgICAgICB8IGJ1Zltwb3MgKyAyXSA8PCA4XHJcbiAgICAgICAgICB8IGJ1Zltwb3MgKyAzXSkgPj4+IDA7XHJcbn1cclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbm1vZHVsZS5leHBvcnRzID0gaW5xdWlyZTtcclxuXHJcbi8qKlxyXG4gKiBSZXF1aXJlcyBhIG1vZHVsZSBvbmx5IGlmIGF2YWlsYWJsZS5cclxuICogQG1lbWJlcm9mIHV0aWxcclxuICogQHBhcmFtIHtzdHJpbmd9IG1vZHVsZU5hbWUgTW9kdWxlIHRvIHJlcXVpcmVcclxuICogQHJldHVybnMgez9PYmplY3R9IFJlcXVpcmVkIG1vZHVsZSBpZiBhdmFpbGFibGUgYW5kIG5vdCBlbXB0eSwgb3RoZXJ3aXNlIGBudWxsYFxyXG4gKi9cclxuZnVuY3Rpb24gaW5xdWlyZShtb2R1bGVOYW1lKSB7XHJcbiAgICB0cnkge1xyXG4gICAgICAgIHZhciBtb2QgPSBldmFsKFwicXVpcmVcIi5yZXBsYWNlKC9eLyxcInJlXCIpKShtb2R1bGVOYW1lKTsgLy8gZXNsaW50LWRpc2FibGUtbGluZSBuby1ldmFsXHJcbiAgICAgICAgaWYgKG1vZCAmJiAobW9kLmxlbmd0aCB8fCBPYmplY3Qua2V5cyhtb2QpLmxlbmd0aCkpXHJcbiAgICAgICAgICAgIHJldHVybiBtb2Q7XHJcbiAgICB9IGNhdGNoIChlKSB7fSAvLyBlc2xpbnQtZGlzYWJsZS1saW5lIG5vLWVtcHR5XHJcbiAgICByZXR1cm4gbnVsbDtcclxufVxyXG4iLCJcInVzZSBzdHJpY3RcIjtcclxuXHJcbi8qKlxyXG4gKiBBIG1pbmltYWwgcGF0aCBtb2R1bGUgdG8gcmVzb2x2ZSBVbml4LCBXaW5kb3dzIGFuZCBVUkwgcGF0aHMgYWxpa2UuXHJcbiAqIEBtZW1iZXJvZiB1dGlsXHJcbiAqIEBuYW1lc3BhY2VcclxuICovXHJcbnZhciBwYXRoID0gZXhwb3J0cztcclxuXHJcbnZhciBpc0Fic29sdXRlID1cclxuLyoqXHJcbiAqIFRlc3RzIGlmIHRoZSBzcGVjaWZpZWQgcGF0aCBpcyBhYnNvbHV0ZS5cclxuICogQHBhcmFtIHtzdHJpbmd9IHBhdGggUGF0aCB0byB0ZXN0XHJcbiAqIEByZXR1cm5zIHtib29sZWFufSBgdHJ1ZWAgaWYgcGF0aCBpcyBhYnNvbHV0ZVxyXG4gKi9cclxucGF0aC5pc0Fic29sdXRlID0gZnVuY3Rpb24gaXNBYnNvbHV0ZShwYXRoKSB7XHJcbiAgICByZXR1cm4gL14oPzpcXC98XFx3KzopLy50ZXN0KHBhdGgpO1xyXG59O1xyXG5cclxudmFyIG5vcm1hbGl6ZSA9XHJcbi8qKlxyXG4gKiBOb3JtYWxpemVzIHRoZSBzcGVjaWZpZWQgcGF0aC5cclxuICogQHBhcmFtIHtzdHJpbmd9IHBhdGggUGF0aCB0byBub3JtYWxpemVcclxuICogQHJldHVybnMge3N0cmluZ30gTm9ybWFsaXplZCBwYXRoXHJcbiAqL1xyXG5wYXRoLm5vcm1hbGl6ZSA9IGZ1bmN0aW9uIG5vcm1hbGl6ZShwYXRoKSB7XHJcbiAgICBwYXRoID0gcGF0aC5yZXBsYWNlKC9cXFxcL2csIFwiL1wiKVxyXG4gICAgICAgICAgICAgICAucmVwbGFjZSgvXFwvezIsfS9nLCBcIi9cIik7XHJcbiAgICB2YXIgcGFydHMgICAgPSBwYXRoLnNwbGl0KFwiL1wiKSxcclxuICAgICAgICBhYnNvbHV0ZSA9IGlzQWJzb2x1dGUocGF0aCksXHJcbiAgICAgICAgcHJlZml4ICAgPSBcIlwiO1xyXG4gICAgaWYgKGFic29sdXRlKVxyXG4gICAgICAgIHByZWZpeCA9IHBhcnRzLnNoaWZ0KCkgKyBcIi9cIjtcclxuICAgIGZvciAodmFyIGkgPSAwOyBpIDwgcGFydHMubGVuZ3RoOykge1xyXG4gICAgICAgIGlmIChwYXJ0c1tpXSA9PT0gXCIuLlwiKSB7XHJcbiAgICAgICAgICAgIGlmIChpID4gMCAmJiBwYXJ0c1tpIC0gMV0gIT09IFwiLi5cIilcclxuICAgICAgICAgICAgICAgIHBhcnRzLnNwbGljZSgtLWksIDIpO1xyXG4gICAgICAgICAgICBlbHNlIGlmIChhYnNvbHV0ZSlcclxuICAgICAgICAgICAgICAgIHBhcnRzLnNwbGljZShpLCAxKTtcclxuICAgICAgICAgICAgZWxzZVxyXG4gICAgICAgICAgICAgICAgKytpO1xyXG4gICAgICAgIH0gZWxzZSBpZiAocGFydHNbaV0gPT09IFwiLlwiKVxyXG4gICAgICAgICAgICBwYXJ0cy5zcGxpY2UoaSwgMSk7XHJcbiAgICAgICAgZWxzZVxyXG4gICAgICAgICAgICArK2k7XHJcbiAgICB9XHJcbiAgICByZXR1cm4gcHJlZml4ICsgcGFydHMuam9pbihcIi9cIik7XHJcbn07XHJcblxyXG4vKipcclxuICogUmVzb2x2ZXMgdGhlIHNwZWNpZmllZCBpbmNsdWRlIHBhdGggYWdhaW5zdCB0aGUgc3BlY2lmaWVkIG9yaWdpbiBwYXRoLlxyXG4gKiBAcGFyYW0ge3N0cmluZ30gb3JpZ2luUGF0aCBQYXRoIHRvIHRoZSBvcmlnaW4gZmlsZVxyXG4gKiBAcGFyYW0ge3N0cmluZ30gaW5jbHVkZVBhdGggSW5jbHVkZSBwYXRoIHJlbGF0aXZlIHRvIG9yaWdpbiBwYXRoXHJcbiAqIEBwYXJhbSB7Ym9vbGVhbn0gW2FscmVhZHlOb3JtYWxpemVkPWZhbHNlXSBgdHJ1ZWAgaWYgYm90aCBwYXRocyBhcmUgYWxyZWFkeSBrbm93biB0byBiZSBub3JtYWxpemVkXHJcbiAqIEByZXR1cm5zIHtzdHJpbmd9IFBhdGggdG8gdGhlIGluY2x1ZGUgZmlsZVxyXG4gKi9cclxucGF0aC5yZXNvbHZlID0gZnVuY3Rpb24gcmVzb2x2ZShvcmlnaW5QYXRoLCBpbmNsdWRlUGF0aCwgYWxyZWFkeU5vcm1hbGl6ZWQpIHtcclxuICAgIGlmICghYWxyZWFkeU5vcm1hbGl6ZWQpXHJcbiAgICAgICAgaW5jbHVkZVBhdGggPSBub3JtYWxpemUoaW5jbHVkZVBhdGgpO1xyXG4gICAgaWYgKGlzQWJzb2x1dGUoaW5jbHVkZVBhdGgpKVxyXG4gICAgICAgIHJldHVybiBpbmNsdWRlUGF0aDtcclxuICAgIGlmICghYWxyZWFkeU5vcm1hbGl6ZWQpXHJcbiAgICAgICAgb3JpZ2luUGF0aCA9IG5vcm1hbGl6ZShvcmlnaW5QYXRoKTtcclxuICAgIHJldHVybiAob3JpZ2luUGF0aCA9IG9yaWdpblBhdGgucmVwbGFjZSgvKD86XFwvfF4pW14vXSskLywgXCJcIikpLmxlbmd0aCA/IG5vcm1hbGl6ZShvcmlnaW5QYXRoICsgXCIvXCIgKyBpbmNsdWRlUGF0aCkgOiBpbmNsdWRlUGF0aDtcclxufTtcclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbm1vZHVsZS5leHBvcnRzID0gcG9vbDtcclxuXHJcbi8qKlxyXG4gKiBBbiBhbGxvY2F0b3IgYXMgdXNlZCBieSB7QGxpbmsgdXRpbC5wb29sfS5cclxuICogQHR5cGVkZWYgUG9vbEFsbG9jYXRvclxyXG4gKiBAdHlwZSB7ZnVuY3Rpb259XHJcbiAqIEBwYXJhbSB7bnVtYmVyfSBzaXplIEJ1ZmZlciBzaXplXHJcbiAqIEByZXR1cm5zIHtVaW50OEFycmF5fSBCdWZmZXJcclxuICovXHJcblxyXG4vKipcclxuICogQSBzbGljZXIgYXMgdXNlZCBieSB7QGxpbmsgdXRpbC5wb29sfS5cclxuICogQHR5cGVkZWYgUG9vbFNsaWNlclxyXG4gKiBAdHlwZSB7ZnVuY3Rpb259XHJcbiAqIEBwYXJhbSB7bnVtYmVyfSBzdGFydCBTdGFydCBvZmZzZXRcclxuICogQHBhcmFtIHtudW1iZXJ9IGVuZCBFbmQgb2Zmc2V0XHJcbiAqIEByZXR1cm5zIHtVaW50OEFycmF5fSBCdWZmZXIgc2xpY2VcclxuICogQHRoaXMge1VpbnQ4QXJyYXl9XHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIEEgZ2VuZXJhbCBwdXJwb3NlIGJ1ZmZlciBwb29sLlxyXG4gKiBAbWVtYmVyb2YgdXRpbFxyXG4gKiBAZnVuY3Rpb25cclxuICogQHBhcmFtIHtQb29sQWxsb2NhdG9yfSBhbGxvYyBBbGxvY2F0b3JcclxuICogQHBhcmFtIHtQb29sU2xpY2VyfSBzbGljZSBTbGljZXJcclxuICogQHBhcmFtIHtudW1iZXJ9IFtzaXplPTgxOTJdIFNsYWIgc2l6ZVxyXG4gKiBAcmV0dXJucyB7UG9vbEFsbG9jYXRvcn0gUG9vbGVkIGFsbG9jYXRvclxyXG4gKi9cclxuZnVuY3Rpb24gcG9vbChhbGxvYywgc2xpY2UsIHNpemUpIHtcclxuICAgIHZhciBTSVpFICAgPSBzaXplIHx8IDgxOTI7XHJcbiAgICB2YXIgTUFYICAgID0gU0laRSA+Pj4gMTtcclxuICAgIHZhciBzbGFiICAgPSBudWxsO1xyXG4gICAgdmFyIG9mZnNldCA9IFNJWkU7XHJcbiAgICByZXR1cm4gZnVuY3Rpb24gcG9vbF9hbGxvYyhzaXplKSB7XHJcbiAgICAgICAgaWYgKHNpemUgPCAxIHx8IHNpemUgPiBNQVgpXHJcbiAgICAgICAgICAgIHJldHVybiBhbGxvYyhzaXplKTtcclxuICAgICAgICBpZiAob2Zmc2V0ICsgc2l6ZSA+IFNJWkUpIHtcclxuICAgICAgICAgICAgc2xhYiA9IGFsbG9jKFNJWkUpO1xyXG4gICAgICAgICAgICBvZmZzZXQgPSAwO1xyXG4gICAgICAgIH1cclxuICAgICAgICB2YXIgYnVmID0gc2xpY2UuY2FsbChzbGFiLCBvZmZzZXQsIG9mZnNldCArPSBzaXplKTtcclxuICAgICAgICBpZiAob2Zmc2V0ICYgNykgLy8gYWxpZ24gdG8gMzIgYml0XHJcbiAgICAgICAgICAgIG9mZnNldCA9IChvZmZzZXQgfCA3KSArIDE7XHJcbiAgICAgICAgcmV0dXJuIGJ1ZjtcclxuICAgIH07XHJcbn1cclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcblxyXG4vKipcclxuICogQSBtaW5pbWFsIFVURjggaW1wbGVtZW50YXRpb24gZm9yIG51bWJlciBhcnJheXMuXHJcbiAqIEBtZW1iZXJvZiB1dGlsXHJcbiAqIEBuYW1lc3BhY2VcclxuICovXHJcbnZhciB1dGY4ID0gZXhwb3J0cztcclxuXHJcbi8qKlxyXG4gKiBDYWxjdWxhdGVzIHRoZSBVVEY4IGJ5dGUgbGVuZ3RoIG9mIGEgc3RyaW5nLlxyXG4gKiBAcGFyYW0ge3N0cmluZ30gc3RyaW5nIFN0cmluZ1xyXG4gKiBAcmV0dXJucyB7bnVtYmVyfSBCeXRlIGxlbmd0aFxyXG4gKi9cclxudXRmOC5sZW5ndGggPSBmdW5jdGlvbiB1dGY4X2xlbmd0aChzdHJpbmcpIHtcclxuICAgIHZhciBsZW4gPSAwLFxyXG4gICAgICAgIGMgPSAwO1xyXG4gICAgZm9yICh2YXIgaSA9IDA7IGkgPCBzdHJpbmcubGVuZ3RoOyArK2kpIHtcclxuICAgICAgICBjID0gc3RyaW5nLmNoYXJDb2RlQXQoaSk7XHJcbiAgICAgICAgaWYgKGMgPCAxMjgpXHJcbiAgICAgICAgICAgIGxlbiArPSAxO1xyXG4gICAgICAgIGVsc2UgaWYgKGMgPCAyMDQ4KVxyXG4gICAgICAgICAgICBsZW4gKz0gMjtcclxuICAgICAgICBlbHNlIGlmICgoYyAmIDB4RkMwMCkgPT09IDB4RDgwMCAmJiAoc3RyaW5nLmNoYXJDb2RlQXQoaSArIDEpICYgMHhGQzAwKSA9PT0gMHhEQzAwKSB7XHJcbiAgICAgICAgICAgICsraTtcclxuICAgICAgICAgICAgbGVuICs9IDQ7XHJcbiAgICAgICAgfSBlbHNlXHJcbiAgICAgICAgICAgIGxlbiArPSAzO1xyXG4gICAgfVxyXG4gICAgcmV0dXJuIGxlbjtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBSZWFkcyBVVEY4IGJ5dGVzIGFzIGEgc3RyaW5nLlxyXG4gKiBAcGFyYW0ge1VpbnQ4QXJyYXl9IGJ1ZmZlciBTb3VyY2UgYnVmZmVyXHJcbiAqIEBwYXJhbSB7bnVtYmVyfSBzdGFydCBTb3VyY2Ugc3RhcnRcclxuICogQHBhcmFtIHtudW1iZXJ9IGVuZCBTb3VyY2UgZW5kXHJcbiAqIEByZXR1cm5zIHtzdHJpbmd9IFN0cmluZyByZWFkXHJcbiAqL1xyXG51dGY4LnJlYWQgPSBmdW5jdGlvbiB1dGY4X3JlYWQoYnVmZmVyLCBzdGFydCwgZW5kKSB7XHJcbiAgICB2YXIgbGVuID0gZW5kIC0gc3RhcnQ7XHJcbiAgICBpZiAobGVuIDwgMSlcclxuICAgICAgICByZXR1cm4gXCJcIjtcclxuICAgIHZhciBwYXJ0cyA9IG51bGwsXHJcbiAgICAgICAgY2h1bmsgPSBbXSxcclxuICAgICAgICBpID0gMCwgLy8gY2hhciBvZmZzZXRcclxuICAgICAgICB0OyAgICAgLy8gdGVtcG9yYXJ5XHJcbiAgICB3aGlsZSAoc3RhcnQgPCBlbmQpIHtcclxuICAgICAgICB0ID0gYnVmZmVyW3N0YXJ0KytdO1xyXG4gICAgICAgIGlmICh0IDwgMTI4KVxyXG4gICAgICAgICAgICBjaHVua1tpKytdID0gdDtcclxuICAgICAgICBlbHNlIGlmICh0ID4gMTkxICYmIHQgPCAyMjQpXHJcbiAgICAgICAgICAgIGNodW5rW2krK10gPSAodCAmIDMxKSA8PCA2IHwgYnVmZmVyW3N0YXJ0KytdICYgNjM7XHJcbiAgICAgICAgZWxzZSBpZiAodCA+IDIzOSAmJiB0IDwgMzY1KSB7XHJcbiAgICAgICAgICAgIHQgPSAoKHQgJiA3KSA8PCAxOCB8IChidWZmZXJbc3RhcnQrK10gJiA2MykgPDwgMTIgfCAoYnVmZmVyW3N0YXJ0KytdICYgNjMpIDw8IDYgfCBidWZmZXJbc3RhcnQrK10gJiA2MykgLSAweDEwMDAwO1xyXG4gICAgICAgICAgICBjaHVua1tpKytdID0gMHhEODAwICsgKHQgPj4gMTApO1xyXG4gICAgICAgICAgICBjaHVua1tpKytdID0gMHhEQzAwICsgKHQgJiAxMDIzKTtcclxuICAgICAgICB9IGVsc2VcclxuICAgICAgICAgICAgY2h1bmtbaSsrXSA9ICh0ICYgMTUpIDw8IDEyIHwgKGJ1ZmZlcltzdGFydCsrXSAmIDYzKSA8PCA2IHwgYnVmZmVyW3N0YXJ0KytdICYgNjM7XHJcbiAgICAgICAgaWYgKGkgPiA4MTkxKSB7XHJcbiAgICAgICAgICAgIChwYXJ0cyB8fCAocGFydHMgPSBbXSkpLnB1c2goU3RyaW5nLmZyb21DaGFyQ29kZS5hcHBseShTdHJpbmcsIGNodW5rKSk7XHJcbiAgICAgICAgICAgIGkgPSAwO1xyXG4gICAgICAgIH1cclxuICAgIH1cclxuICAgIGlmIChwYXJ0cykge1xyXG4gICAgICAgIGlmIChpKVxyXG4gICAgICAgICAgICBwYXJ0cy5wdXNoKFN0cmluZy5mcm9tQ2hhckNvZGUuYXBwbHkoU3RyaW5nLCBjaHVuay5zbGljZSgwLCBpKSkpO1xyXG4gICAgICAgIHJldHVybiBwYXJ0cy5qb2luKFwiXCIpO1xyXG4gICAgfVxyXG4gICAgcmV0dXJuIFN0cmluZy5mcm9tQ2hhckNvZGUuYXBwbHkoU3RyaW5nLCBjaHVuay5zbGljZSgwLCBpKSk7XHJcbn07XHJcblxyXG4vKipcclxuICogV3JpdGVzIGEgc3RyaW5nIGFzIFVURjggYnl0ZXMuXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBzdHJpbmcgU291cmNlIHN0cmluZ1xyXG4gKiBAcGFyYW0ge1VpbnQ4QXJyYXl9IGJ1ZmZlciBEZXN0aW5hdGlvbiBidWZmZXJcclxuICogQHBhcmFtIHtudW1iZXJ9IG9mZnNldCBEZXN0aW5hdGlvbiBvZmZzZXRcclxuICogQHJldHVybnMge251bWJlcn0gQnl0ZXMgd3JpdHRlblxyXG4gKi9cclxudXRmOC53cml0ZSA9IGZ1bmN0aW9uIHV0Zjhfd3JpdGUoc3RyaW5nLCBidWZmZXIsIG9mZnNldCkge1xyXG4gICAgdmFyIHN0YXJ0ID0gb2Zmc2V0LFxyXG4gICAgICAgIGMxLCAvLyBjaGFyYWN0ZXIgMVxyXG4gICAgICAgIGMyOyAvLyBjaGFyYWN0ZXIgMlxyXG4gICAgZm9yICh2YXIgaSA9IDA7IGkgPCBzdHJpbmcubGVuZ3RoOyArK2kpIHtcclxuICAgICAgICBjMSA9IHN0cmluZy5jaGFyQ29kZUF0KGkpO1xyXG4gICAgICAgIGlmIChjMSA8IDEyOCkge1xyXG4gICAgICAgICAgICBidWZmZXJbb2Zmc2V0KytdID0gYzE7XHJcbiAgICAgICAgfSBlbHNlIGlmIChjMSA8IDIwNDgpIHtcclxuICAgICAgICAgICAgYnVmZmVyW29mZnNldCsrXSA9IGMxID4+IDYgICAgICAgfCAxOTI7XHJcbiAgICAgICAgICAgIGJ1ZmZlcltvZmZzZXQrK10gPSBjMSAgICAgICAmIDYzIHwgMTI4O1xyXG4gICAgICAgIH0gZWxzZSBpZiAoKGMxICYgMHhGQzAwKSA9PT0gMHhEODAwICYmICgoYzIgPSBzdHJpbmcuY2hhckNvZGVBdChpICsgMSkpICYgMHhGQzAwKSA9PT0gMHhEQzAwKSB7XHJcbiAgICAgICAgICAgIGMxID0gMHgxMDAwMCArICgoYzEgJiAweDAzRkYpIDw8IDEwKSArIChjMiAmIDB4MDNGRik7XHJcbiAgICAgICAgICAgICsraTtcclxuICAgICAgICAgICAgYnVmZmVyW29mZnNldCsrXSA9IGMxID4+IDE4ICAgICAgfCAyNDA7XHJcbiAgICAgICAgICAgIGJ1ZmZlcltvZmZzZXQrK10gPSBjMSA+PiAxMiAmIDYzIHwgMTI4O1xyXG4gICAgICAgICAgICBidWZmZXJbb2Zmc2V0KytdID0gYzEgPj4gNiAgJiA2MyB8IDEyODtcclxuICAgICAgICAgICAgYnVmZmVyW29mZnNldCsrXSA9IGMxICAgICAgICYgNjMgfCAxMjg7XHJcbiAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgYnVmZmVyW29mZnNldCsrXSA9IGMxID4+IDEyICAgICAgfCAyMjQ7XHJcbiAgICAgICAgICAgIGJ1ZmZlcltvZmZzZXQrK10gPSBjMSA+PiA2ICAmIDYzIHwgMTI4O1xyXG4gICAgICAgICAgICBidWZmZXJbb2Zmc2V0KytdID0gYzEgICAgICAgJiA2MyB8IDEyODtcclxuICAgICAgICB9XHJcbiAgICB9XHJcbiAgICByZXR1cm4gb2Zmc2V0IC0gc3RhcnQ7XHJcbn07XHJcbiIsIi8vIGZ1bGwgbGlicmFyeSBlbnRyeSBwb2ludC5cclxuXHJcblwidXNlIHN0cmljdFwiO1xyXG5tb2R1bGUuZXhwb3J0cyA9IHJlcXVpcmUoXCIuL3NyYy9pbmRleFwiKTtcclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbm1vZHVsZS5leHBvcnRzID0gY29tbW9uO1xyXG5cclxudmFyIGNvbW1vblJlID0gL1xcL3xcXC4vO1xyXG5cclxuLyoqXHJcbiAqIFByb3ZpZGVzIGNvbW1vbiB0eXBlIGRlZmluaXRpb25zLlxyXG4gKiBDYW4gYWxzbyBiZSB1c2VkIHRvIHByb3ZpZGUgYWRkaXRpb25hbCBnb29nbGUgdHlwZXMgb3IgeW91ciBvd24gY3VzdG9tIHR5cGVzLlxyXG4gKiBAcGFyYW0ge3N0cmluZ30gbmFtZSBTaG9ydCBuYW1lIGFzIGluIGBnb29nbGUvcHJvdG9idWYvW25hbWVdLnByb3RvYCBvciBmdWxsIGZpbGUgbmFtZVxyXG4gKiBAcGFyYW0ge09iamVjdC48c3RyaW5nLCo+fSBqc29uIEpTT04gZGVmaW5pdGlvbiB3aXRoaW4gYGdvb2dsZS5wcm90b2J1ZmAgaWYgYSBzaG9ydCBuYW1lLCBvdGhlcndpc2UgdGhlIGZpbGUncyByb290IGRlZmluaXRpb25cclxuICogQHJldHVybnMge3VuZGVmaW5lZH1cclxuICogQHByb3BlcnR5IHtJTmFtZXNwYWNlfSBnb29nbGUvcHJvdG9idWYvYW55LnByb3RvIEFueVxyXG4gKiBAcHJvcGVydHkge0lOYW1lc3BhY2V9IGdvb2dsZS9wcm90b2J1Zi9kdXJhdGlvbi5wcm90byBEdXJhdGlvblxyXG4gKiBAcHJvcGVydHkge0lOYW1lc3BhY2V9IGdvb2dsZS9wcm90b2J1Zi9lbXB0eS5wcm90byBFbXB0eVxyXG4gKiBAcHJvcGVydHkge0lOYW1lc3BhY2V9IGdvb2dsZS9wcm90b2J1Zi9maWVsZF9tYXNrLnByb3RvIEZpZWxkTWFza1xyXG4gKiBAcHJvcGVydHkge0lOYW1lc3BhY2V9IGdvb2dsZS9wcm90b2J1Zi9zdHJ1Y3QucHJvdG8gU3RydWN0LCBWYWx1ZSwgTnVsbFZhbHVlIGFuZCBMaXN0VmFsdWVcclxuICogQHByb3BlcnR5IHtJTmFtZXNwYWNlfSBnb29nbGUvcHJvdG9idWYvdGltZXN0YW1wLnByb3RvIFRpbWVzdGFtcFxyXG4gKiBAcHJvcGVydHkge0lOYW1lc3BhY2V9IGdvb2dsZS9wcm90b2J1Zi93cmFwcGVycy5wcm90byBXcmFwcGVyc1xyXG4gKiBAZXhhbXBsZVxyXG4gKiAvLyBtYW51YWxseSBwcm92aWRlcyBkZXNjcmlwdG9yLnByb3RvIChhc3N1bWVzIGdvb2dsZS9wcm90b2J1Zi8gbmFtZXNwYWNlIGFuZCAucHJvdG8gZXh0ZW5zaW9uKVxyXG4gKiBwcm90b2J1Zi5jb21tb24oXCJkZXNjcmlwdG9yXCIsIGRlc2NyaXB0b3JKc29uKTtcclxuICpcclxuICogLy8gbWFudWFsbHkgcHJvdmlkZXMgYSBjdXN0b20gZGVmaW5pdGlvbiAodXNlcyBteS5mb28gbmFtZXNwYWNlKVxyXG4gKiBwcm90b2J1Zi5jb21tb24oXCJteS9mb28vYmFyLnByb3RvXCIsIG15Rm9vQmFySnNvbik7XHJcbiAqL1xyXG5mdW5jdGlvbiBjb21tb24obmFtZSwganNvbikge1xyXG4gICAgaWYgKCFjb21tb25SZS50ZXN0KG5hbWUpKSB7XHJcbiAgICAgICAgbmFtZSA9IFwiZ29vZ2xlL3Byb3RvYnVmL1wiICsgbmFtZSArIFwiLnByb3RvXCI7XHJcbiAgICAgICAganNvbiA9IHsgbmVzdGVkOiB7IGdvb2dsZTogeyBuZXN0ZWQ6IHsgcHJvdG9idWY6IHsgbmVzdGVkOiBqc29uIH0gfSB9IH0gfTtcclxuICAgIH1cclxuICAgIGNvbW1vbltuYW1lXSA9IGpzb247XHJcbn1cclxuXHJcbi8vIE5vdCBwcm92aWRlZCBiZWNhdXNlIG9mIGxpbWl0ZWQgdXNlIChmZWVsIGZyZWUgdG8gZGlzY3VzcyBvciB0byBwcm92aWRlIHlvdXJzZWxmKTpcclxuLy9cclxuLy8gZ29vZ2xlL3Byb3RvYnVmL2Rlc2NyaXB0b3IucHJvdG9cclxuLy8gZ29vZ2xlL3Byb3RvYnVmL3NvdXJjZV9jb250ZXh0LnByb3RvXHJcbi8vIGdvb2dsZS9wcm90b2J1Zi90eXBlLnByb3RvXHJcbi8vXHJcbi8vIFN0cmlwcGVkIGFuZCBwcmUtcGFyc2VkIHZlcnNpb25zIG9mIHRoZXNlIG5vbi1idW5kbGVkIGZpbGVzIGFyZSBpbnN0ZWFkIGF2YWlsYWJsZSBhcyBwYXJ0IG9mXHJcbi8vIHRoZSByZXBvc2l0b3J5IG9yIHBhY2thZ2Ugd2l0aGluIHRoZSBnb29nbGUvcHJvdG9idWYgZGlyZWN0b3J5LlxyXG5cclxuY29tbW9uKFwiYW55XCIsIHtcclxuXHJcbiAgICAvKipcclxuICAgICAqIFByb3BlcnRpZXMgb2YgYSBnb29nbGUucHJvdG9idWYuQW55IG1lc3NhZ2UuXHJcbiAgICAgKiBAaW50ZXJmYWNlIElBbnlcclxuICAgICAqIEB0eXBlIHtPYmplY3R9XHJcbiAgICAgKiBAcHJvcGVydHkge3N0cmluZ30gW3R5cGVVcmxdXHJcbiAgICAgKiBAcHJvcGVydHkge1VpbnQ4QXJyYXl9IFtieXRlc11cclxuICAgICAqIEBtZW1iZXJvZiBjb21tb25cclxuICAgICAqL1xyXG4gICAgQW55OiB7XHJcbiAgICAgICAgZmllbGRzOiB7XHJcbiAgICAgICAgICAgIHR5cGVfdXJsOiB7XHJcbiAgICAgICAgICAgICAgICB0eXBlOiBcInN0cmluZ1wiLFxyXG4gICAgICAgICAgICAgICAgaWQ6IDFcclxuICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgdmFsdWU6IHtcclxuICAgICAgICAgICAgICAgIHR5cGU6IFwiYnl0ZXNcIixcclxuICAgICAgICAgICAgICAgIGlkOiAyXHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICB9XHJcbn0pO1xyXG5cclxudmFyIHRpbWVUeXBlO1xyXG5cclxuY29tbW9uKFwiZHVyYXRpb25cIiwge1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogUHJvcGVydGllcyBvZiBhIGdvb2dsZS5wcm90b2J1Zi5EdXJhdGlvbiBtZXNzYWdlLlxyXG4gICAgICogQGludGVyZmFjZSBJRHVyYXRpb25cclxuICAgICAqIEB0eXBlIHtPYmplY3R9XHJcbiAgICAgKiBAcHJvcGVydHkge251bWJlcnxMb25nfSBbc2Vjb25kc11cclxuICAgICAqIEBwcm9wZXJ0eSB7bnVtYmVyfSBbbmFub3NdXHJcbiAgICAgKiBAbWVtYmVyb2YgY29tbW9uXHJcbiAgICAgKi9cclxuICAgIER1cmF0aW9uOiB0aW1lVHlwZSA9IHtcclxuICAgICAgICBmaWVsZHM6IHtcclxuICAgICAgICAgICAgc2Vjb25kczoge1xyXG4gICAgICAgICAgICAgICAgdHlwZTogXCJpbnQ2NFwiLFxyXG4gICAgICAgICAgICAgICAgaWQ6IDFcclxuICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgbmFub3M6IHtcclxuICAgICAgICAgICAgICAgIHR5cGU6IFwiaW50MzJcIixcclxuICAgICAgICAgICAgICAgIGlkOiAyXHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICB9XHJcbn0pO1xyXG5cclxuY29tbW9uKFwidGltZXN0YW1wXCIsIHtcclxuXHJcbiAgICAvKipcclxuICAgICAqIFByb3BlcnRpZXMgb2YgYSBnb29nbGUucHJvdG9idWYuVGltZXN0YW1wIG1lc3NhZ2UuXHJcbiAgICAgKiBAaW50ZXJmYWNlIElUaW1lc3RhbXBcclxuICAgICAqIEB0eXBlIHtPYmplY3R9XHJcbiAgICAgKiBAcHJvcGVydHkge251bWJlcnxMb25nfSBbc2Vjb25kc11cclxuICAgICAqIEBwcm9wZXJ0eSB7bnVtYmVyfSBbbmFub3NdXHJcbiAgICAgKiBAbWVtYmVyb2YgY29tbW9uXHJcbiAgICAgKi9cclxuICAgIFRpbWVzdGFtcDogdGltZVR5cGVcclxufSk7XHJcblxyXG5jb21tb24oXCJlbXB0eVwiLCB7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBQcm9wZXJ0aWVzIG9mIGEgZ29vZ2xlLnByb3RvYnVmLkVtcHR5IG1lc3NhZ2UuXHJcbiAgICAgKiBAaW50ZXJmYWNlIElFbXB0eVxyXG4gICAgICogQG1lbWJlcm9mIGNvbW1vblxyXG4gICAgICovXHJcbiAgICBFbXB0eToge1xyXG4gICAgICAgIGZpZWxkczoge31cclxuICAgIH1cclxufSk7XHJcblxyXG5jb21tb24oXCJzdHJ1Y3RcIiwge1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogUHJvcGVydGllcyBvZiBhIGdvb2dsZS5wcm90b2J1Zi5TdHJ1Y3QgbWVzc2FnZS5cclxuICAgICAqIEBpbnRlcmZhY2UgSVN0cnVjdFxyXG4gICAgICogQHR5cGUge09iamVjdH1cclxuICAgICAqIEBwcm9wZXJ0eSB7T2JqZWN0LjxzdHJpbmcsSVZhbHVlPn0gW2ZpZWxkc11cclxuICAgICAqIEBtZW1iZXJvZiBjb21tb25cclxuICAgICAqL1xyXG4gICAgU3RydWN0OiB7XHJcbiAgICAgICAgZmllbGRzOiB7XHJcbiAgICAgICAgICAgIGZpZWxkczoge1xyXG4gICAgICAgICAgICAgICAga2V5VHlwZTogXCJzdHJpbmdcIixcclxuICAgICAgICAgICAgICAgIHR5cGU6IFwiVmFsdWVcIixcclxuICAgICAgICAgICAgICAgIGlkOiAxXHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogUHJvcGVydGllcyBvZiBhIGdvb2dsZS5wcm90b2J1Zi5WYWx1ZSBtZXNzYWdlLlxyXG4gICAgICogQGludGVyZmFjZSBJVmFsdWVcclxuICAgICAqIEB0eXBlIHtPYmplY3R9XHJcbiAgICAgKiBAcHJvcGVydHkge3N0cmluZ30gW2tpbmRdXHJcbiAgICAgKiBAcHJvcGVydHkgezB9IFtudWxsVmFsdWVdXHJcbiAgICAgKiBAcHJvcGVydHkge251bWJlcn0gW251bWJlclZhbHVlXVxyXG4gICAgICogQHByb3BlcnR5IHtzdHJpbmd9IFtzdHJpbmdWYWx1ZV1cclxuICAgICAqIEBwcm9wZXJ0eSB7Ym9vbGVhbn0gW2Jvb2xWYWx1ZV1cclxuICAgICAqIEBwcm9wZXJ0eSB7SVN0cnVjdH0gW3N0cnVjdFZhbHVlXVxyXG4gICAgICogQHByb3BlcnR5IHtJTGlzdFZhbHVlfSBbbGlzdFZhbHVlXVxyXG4gICAgICogQG1lbWJlcm9mIGNvbW1vblxyXG4gICAgICovXHJcbiAgICBWYWx1ZToge1xyXG4gICAgICAgIG9uZW9mczoge1xyXG4gICAgICAgICAgICBraW5kOiB7XHJcbiAgICAgICAgICAgICAgICBvbmVvZjogW1xyXG4gICAgICAgICAgICAgICAgICAgIFwibnVsbFZhbHVlXCIsXHJcbiAgICAgICAgICAgICAgICAgICAgXCJudW1iZXJWYWx1ZVwiLFxyXG4gICAgICAgICAgICAgICAgICAgIFwic3RyaW5nVmFsdWVcIixcclxuICAgICAgICAgICAgICAgICAgICBcImJvb2xWYWx1ZVwiLFxyXG4gICAgICAgICAgICAgICAgICAgIFwic3RydWN0VmFsdWVcIixcclxuICAgICAgICAgICAgICAgICAgICBcImxpc3RWYWx1ZVwiXHJcbiAgICAgICAgICAgICAgICBdXHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9LFxyXG4gICAgICAgIGZpZWxkczoge1xyXG4gICAgICAgICAgICBudWxsVmFsdWU6IHtcclxuICAgICAgICAgICAgICAgIHR5cGU6IFwiTnVsbFZhbHVlXCIsXHJcbiAgICAgICAgICAgICAgICBpZDogMVxyXG4gICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICBudW1iZXJWYWx1ZToge1xyXG4gICAgICAgICAgICAgICAgdHlwZTogXCJkb3VibGVcIixcclxuICAgICAgICAgICAgICAgIGlkOiAyXHJcbiAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgIHN0cmluZ1ZhbHVlOiB7XHJcbiAgICAgICAgICAgICAgICB0eXBlOiBcInN0cmluZ1wiLFxyXG4gICAgICAgICAgICAgICAgaWQ6IDNcclxuICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgYm9vbFZhbHVlOiB7XHJcbiAgICAgICAgICAgICAgICB0eXBlOiBcImJvb2xcIixcclxuICAgICAgICAgICAgICAgIGlkOiA0XHJcbiAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgIHN0cnVjdFZhbHVlOiB7XHJcbiAgICAgICAgICAgICAgICB0eXBlOiBcIlN0cnVjdFwiLFxyXG4gICAgICAgICAgICAgICAgaWQ6IDVcclxuICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgbGlzdFZhbHVlOiB7XHJcbiAgICAgICAgICAgICAgICB0eXBlOiBcIkxpc3RWYWx1ZVwiLFxyXG4gICAgICAgICAgICAgICAgaWQ6IDZcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuICAgIH0sXHJcblxyXG4gICAgTnVsbFZhbHVlOiB7XHJcbiAgICAgICAgdmFsdWVzOiB7XHJcbiAgICAgICAgICAgIE5VTExfVkFMVUU6IDBcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogUHJvcGVydGllcyBvZiBhIGdvb2dsZS5wcm90b2J1Zi5MaXN0VmFsdWUgbWVzc2FnZS5cclxuICAgICAqIEBpbnRlcmZhY2UgSUxpc3RWYWx1ZVxyXG4gICAgICogQHR5cGUge09iamVjdH1cclxuICAgICAqIEBwcm9wZXJ0eSB7QXJyYXkuPElWYWx1ZT59IFt2YWx1ZXNdXHJcbiAgICAgKiBAbWVtYmVyb2YgY29tbW9uXHJcbiAgICAgKi9cclxuICAgIExpc3RWYWx1ZToge1xyXG4gICAgICAgIGZpZWxkczoge1xyXG4gICAgICAgICAgICB2YWx1ZXM6IHtcclxuICAgICAgICAgICAgICAgIHJ1bGU6IFwicmVwZWF0ZWRcIixcclxuICAgICAgICAgICAgICAgIHR5cGU6IFwiVmFsdWVcIixcclxuICAgICAgICAgICAgICAgIGlkOiAxXHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICB9XHJcbn0pO1xyXG5cclxuY29tbW9uKFwid3JhcHBlcnNcIiwge1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogUHJvcGVydGllcyBvZiBhIGdvb2dsZS5wcm90b2J1Zi5Eb3VibGVWYWx1ZSBtZXNzYWdlLlxyXG4gICAgICogQGludGVyZmFjZSBJRG91YmxlVmFsdWVcclxuICAgICAqIEB0eXBlIHtPYmplY3R9XHJcbiAgICAgKiBAcHJvcGVydHkge251bWJlcn0gW3ZhbHVlXVxyXG4gICAgICogQG1lbWJlcm9mIGNvbW1vblxyXG4gICAgICovXHJcbiAgICBEb3VibGVWYWx1ZToge1xyXG4gICAgICAgIGZpZWxkczoge1xyXG4gICAgICAgICAgICB2YWx1ZToge1xyXG4gICAgICAgICAgICAgICAgdHlwZTogXCJkb3VibGVcIixcclxuICAgICAgICAgICAgICAgIGlkOiAxXHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogUHJvcGVydGllcyBvZiBhIGdvb2dsZS5wcm90b2J1Zi5GbG9hdFZhbHVlIG1lc3NhZ2UuXHJcbiAgICAgKiBAaW50ZXJmYWNlIElGbG9hdFZhbHVlXHJcbiAgICAgKiBAdHlwZSB7T2JqZWN0fVxyXG4gICAgICogQHByb3BlcnR5IHtudW1iZXJ9IFt2YWx1ZV1cclxuICAgICAqIEBtZW1iZXJvZiBjb21tb25cclxuICAgICAqL1xyXG4gICAgRmxvYXRWYWx1ZToge1xyXG4gICAgICAgIGZpZWxkczoge1xyXG4gICAgICAgICAgICB2YWx1ZToge1xyXG4gICAgICAgICAgICAgICAgdHlwZTogXCJmbG9hdFwiLFxyXG4gICAgICAgICAgICAgICAgaWQ6IDFcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBQcm9wZXJ0aWVzIG9mIGEgZ29vZ2xlLnByb3RvYnVmLkludDY0VmFsdWUgbWVzc2FnZS5cclxuICAgICAqIEBpbnRlcmZhY2UgSUludDY0VmFsdWVcclxuICAgICAqIEB0eXBlIHtPYmplY3R9XHJcbiAgICAgKiBAcHJvcGVydHkge251bWJlcnxMb25nfSBbdmFsdWVdXHJcbiAgICAgKiBAbWVtYmVyb2YgY29tbW9uXHJcbiAgICAgKi9cclxuICAgIEludDY0VmFsdWU6IHtcclxuICAgICAgICBmaWVsZHM6IHtcclxuICAgICAgICAgICAgdmFsdWU6IHtcclxuICAgICAgICAgICAgICAgIHR5cGU6IFwiaW50NjRcIixcclxuICAgICAgICAgICAgICAgIGlkOiAxXHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogUHJvcGVydGllcyBvZiBhIGdvb2dsZS5wcm90b2J1Zi5VSW50NjRWYWx1ZSBtZXNzYWdlLlxyXG4gICAgICogQGludGVyZmFjZSBJVUludDY0VmFsdWVcclxuICAgICAqIEB0eXBlIHtPYmplY3R9XHJcbiAgICAgKiBAcHJvcGVydHkge251bWJlcnxMb25nfSBbdmFsdWVdXHJcbiAgICAgKiBAbWVtYmVyb2YgY29tbW9uXHJcbiAgICAgKi9cclxuICAgIFVJbnQ2NFZhbHVlOiB7XHJcbiAgICAgICAgZmllbGRzOiB7XHJcbiAgICAgICAgICAgIHZhbHVlOiB7XHJcbiAgICAgICAgICAgICAgICB0eXBlOiBcInVpbnQ2NFwiLFxyXG4gICAgICAgICAgICAgICAgaWQ6IDFcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBQcm9wZXJ0aWVzIG9mIGEgZ29vZ2xlLnByb3RvYnVmLkludDMyVmFsdWUgbWVzc2FnZS5cclxuICAgICAqIEBpbnRlcmZhY2UgSUludDMyVmFsdWVcclxuICAgICAqIEB0eXBlIHtPYmplY3R9XHJcbiAgICAgKiBAcHJvcGVydHkge251bWJlcn0gW3ZhbHVlXVxyXG4gICAgICogQG1lbWJlcm9mIGNvbW1vblxyXG4gICAgICovXHJcbiAgICBJbnQzMlZhbHVlOiB7XHJcbiAgICAgICAgZmllbGRzOiB7XHJcbiAgICAgICAgICAgIHZhbHVlOiB7XHJcbiAgICAgICAgICAgICAgICB0eXBlOiBcImludDMyXCIsXHJcbiAgICAgICAgICAgICAgICBpZDogMVxyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFByb3BlcnRpZXMgb2YgYSBnb29nbGUucHJvdG9idWYuVUludDMyVmFsdWUgbWVzc2FnZS5cclxuICAgICAqIEBpbnRlcmZhY2UgSVVJbnQzMlZhbHVlXHJcbiAgICAgKiBAdHlwZSB7T2JqZWN0fVxyXG4gICAgICogQHByb3BlcnR5IHtudW1iZXJ9IFt2YWx1ZV1cclxuICAgICAqIEBtZW1iZXJvZiBjb21tb25cclxuICAgICAqL1xyXG4gICAgVUludDMyVmFsdWU6IHtcclxuICAgICAgICBmaWVsZHM6IHtcclxuICAgICAgICAgICAgdmFsdWU6IHtcclxuICAgICAgICAgICAgICAgIHR5cGU6IFwidWludDMyXCIsXHJcbiAgICAgICAgICAgICAgICBpZDogMVxyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFByb3BlcnRpZXMgb2YgYSBnb29nbGUucHJvdG9idWYuQm9vbFZhbHVlIG1lc3NhZ2UuXHJcbiAgICAgKiBAaW50ZXJmYWNlIElCb29sVmFsdWVcclxuICAgICAqIEB0eXBlIHtPYmplY3R9XHJcbiAgICAgKiBAcHJvcGVydHkge2Jvb2xlYW59IFt2YWx1ZV1cclxuICAgICAqIEBtZW1iZXJvZiBjb21tb25cclxuICAgICAqL1xyXG4gICAgQm9vbFZhbHVlOiB7XHJcbiAgICAgICAgZmllbGRzOiB7XHJcbiAgICAgICAgICAgIHZhbHVlOiB7XHJcbiAgICAgICAgICAgICAgICB0eXBlOiBcImJvb2xcIixcclxuICAgICAgICAgICAgICAgIGlkOiAxXHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogUHJvcGVydGllcyBvZiBhIGdvb2dsZS5wcm90b2J1Zi5TdHJpbmdWYWx1ZSBtZXNzYWdlLlxyXG4gICAgICogQGludGVyZmFjZSBJU3RyaW5nVmFsdWVcclxuICAgICAqIEB0eXBlIHtPYmplY3R9XHJcbiAgICAgKiBAcHJvcGVydHkge3N0cmluZ30gW3ZhbHVlXVxyXG4gICAgICogQG1lbWJlcm9mIGNvbW1vblxyXG4gICAgICovXHJcbiAgICBTdHJpbmdWYWx1ZToge1xyXG4gICAgICAgIGZpZWxkczoge1xyXG4gICAgICAgICAgICB2YWx1ZToge1xyXG4gICAgICAgICAgICAgICAgdHlwZTogXCJzdHJpbmdcIixcclxuICAgICAgICAgICAgICAgIGlkOiAxXHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogUHJvcGVydGllcyBvZiBhIGdvb2dsZS5wcm90b2J1Zi5CeXRlc1ZhbHVlIG1lc3NhZ2UuXHJcbiAgICAgKiBAaW50ZXJmYWNlIElCeXRlc1ZhbHVlXHJcbiAgICAgKiBAdHlwZSB7T2JqZWN0fVxyXG4gICAgICogQHByb3BlcnR5IHtVaW50OEFycmF5fSBbdmFsdWVdXHJcbiAgICAgKiBAbWVtYmVyb2YgY29tbW9uXHJcbiAgICAgKi9cclxuICAgIEJ5dGVzVmFsdWU6IHtcclxuICAgICAgICBmaWVsZHM6IHtcclxuICAgICAgICAgICAgdmFsdWU6IHtcclxuICAgICAgICAgICAgICAgIHR5cGU6IFwiYnl0ZXNcIixcclxuICAgICAgICAgICAgICAgIGlkOiAxXHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICB9XHJcbn0pO1xyXG5cclxuY29tbW9uKFwiZmllbGRfbWFza1wiLCB7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBQcm9wZXJ0aWVzIG9mIGEgZ29vZ2xlLnByb3RvYnVmLkZpZWxkTWFzayBtZXNzYWdlLlxyXG4gICAgICogQGludGVyZmFjZSBJRG91YmxlVmFsdWVcclxuICAgICAqIEB0eXBlIHtPYmplY3R9XHJcbiAgICAgKiBAcHJvcGVydHkge251bWJlcn0gW3ZhbHVlXVxyXG4gICAgICogQG1lbWJlcm9mIGNvbW1vblxyXG4gICAgICovXHJcbiAgICBGaWVsZE1hc2s6IHtcclxuICAgICAgICBmaWVsZHM6IHtcclxuICAgICAgICAgICAgcGF0aHM6IHtcclxuICAgICAgICAgICAgICAgIHJ1bGU6IFwicmVwZWF0ZWRcIixcclxuICAgICAgICAgICAgICAgIHR5cGU6IFwic3RyaW5nXCIsXHJcbiAgICAgICAgICAgICAgICBpZDogMVxyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG59KTtcclxuXHJcbi8qKlxyXG4gKiBHZXRzIHRoZSByb290IGRlZmluaXRpb24gb2YgdGhlIHNwZWNpZmllZCBjb21tb24gcHJvdG8gZmlsZS5cclxuICpcclxuICogQnVuZGxlZCBkZWZpbml0aW9ucyBhcmU6XHJcbiAqIC0gZ29vZ2xlL3Byb3RvYnVmL2FueS5wcm90b1xyXG4gKiAtIGdvb2dsZS9wcm90b2J1Zi9kdXJhdGlvbi5wcm90b1xyXG4gKiAtIGdvb2dsZS9wcm90b2J1Zi9lbXB0eS5wcm90b1xyXG4gKiAtIGdvb2dsZS9wcm90b2J1Zi9maWVsZF9tYXNrLnByb3RvXHJcbiAqIC0gZ29vZ2xlL3Byb3RvYnVmL3N0cnVjdC5wcm90b1xyXG4gKiAtIGdvb2dsZS9wcm90b2J1Zi90aW1lc3RhbXAucHJvdG9cclxuICogLSBnb29nbGUvcHJvdG9idWYvd3JhcHBlcnMucHJvdG9cclxuICpcclxuICogQHBhcmFtIHtzdHJpbmd9IGZpbGUgUHJvdG8gZmlsZSBuYW1lXHJcbiAqIEByZXR1cm5zIHtJTmFtZXNwYWNlfG51bGx9IFJvb3QgZGVmaW5pdGlvbiBvciBgbnVsbGAgaWYgbm90IGRlZmluZWRcclxuICovXHJcbmNvbW1vbi5nZXQgPSBmdW5jdGlvbiBnZXQoZmlsZSkge1xyXG4gICAgcmV0dXJuIGNvbW1vbltmaWxlXSB8fCBudWxsO1xyXG59O1xyXG4iLCJcInVzZSBzdHJpY3RcIjtcclxuLyoqXHJcbiAqIFJ1bnRpbWUgbWVzc2FnZSBmcm9tL3RvIHBsYWluIG9iamVjdCBjb252ZXJ0ZXJzLlxyXG4gKiBAbmFtZXNwYWNlXHJcbiAqL1xyXG52YXIgY29udmVydGVyID0gZXhwb3J0cztcclxuXHJcbnZhciBFbnVtID0gcmVxdWlyZShcIi4vZW51bVwiKSxcclxuICAgIHV0aWwgPSByZXF1aXJlKFwiLi91dGlsXCIpO1xyXG5cclxuLyoqXHJcbiAqIEdlbmVyYXRlcyBhIHBhcnRpYWwgdmFsdWUgZnJvbU9iamVjdCBjb252ZXRlci5cclxuICogQHBhcmFtIHtDb2RlZ2VufSBnZW4gQ29kZWdlbiBpbnN0YW5jZVxyXG4gKiBAcGFyYW0ge0ZpZWxkfSBmaWVsZCBSZWZsZWN0ZWQgZmllbGRcclxuICogQHBhcmFtIHtudW1iZXJ9IGZpZWxkSW5kZXggRmllbGQgaW5kZXhcclxuICogQHBhcmFtIHtzdHJpbmd9IHByb3AgUHJvcGVydHkgcmVmZXJlbmNlXHJcbiAqIEByZXR1cm5zIHtDb2RlZ2VufSBDb2RlZ2VuIGluc3RhbmNlXHJcbiAqIEBpZ25vcmVcclxuICovXHJcbmZ1bmN0aW9uIGdlblZhbHVlUGFydGlhbF9mcm9tT2JqZWN0KGdlbiwgZmllbGQsIGZpZWxkSW5kZXgsIHByb3ApIHtcclxuICAgIHZhciBkZWZhdWx0QWxyZWFkeUVtaXR0ZWQgPSBmYWxzZTtcclxuICAgIC8qIGVzbGludC1kaXNhYmxlIG5vLXVuZXhwZWN0ZWQtbXVsdGlsaW5lLCBibG9jay1zY29wZWQtdmFyLCBuby1yZWRlY2xhcmUgKi9cclxuICAgIGlmIChmaWVsZC5yZXNvbHZlZFR5cGUpIHtcclxuICAgICAgICBpZiAoZmllbGQucmVzb2x2ZWRUeXBlIGluc3RhbmNlb2YgRW51bSkgeyBnZW5cclxuICAgICAgICAgICAgKFwic3dpdGNoKGQlcyl7XCIsIHByb3ApO1xyXG4gICAgICAgICAgICBmb3IgKHZhciB2YWx1ZXMgPSBmaWVsZC5yZXNvbHZlZFR5cGUudmFsdWVzLCBrZXlzID0gT2JqZWN0LmtleXModmFsdWVzKSwgaSA9IDA7IGkgPCBrZXlzLmxlbmd0aDsgKytpKSB7XHJcbiAgICAgICAgICAgICAgICAvLyBlbnVtIHVua25vd24gdmFsdWVzIHBhc3N0aHJvdWdoXHJcbiAgICAgICAgICAgICAgICBpZiAodmFsdWVzW2tleXNbaV1dID09PSBmaWVsZC50eXBlRGVmYXVsdCAmJiAhZGVmYXVsdEFscmVhZHlFbWl0dGVkKSB7IGdlblxyXG4gICAgICAgICAgICAgICAgICAgIChcImRlZmF1bHQ6XCIpXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIChcImlmKHR5cGVvZihkJXMpPT09XFxcIm51bWJlclxcXCIpe20lcz1kJXM7YnJlYWt9XCIsIHByb3AsIHByb3AsIHByb3ApO1xyXG4gICAgICAgICAgICAgICAgICAgIGlmICghZmllbGQucmVwZWF0ZWQpIGdlbiAvLyBmYWxsYmFjayB0byBkZWZhdWx0IHZhbHVlIG9ubHkgZm9yXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC8vIGFycmF5cywgdG8gYXZvaWQgbGVhdmluZyBob2xlcy5cclxuICAgICAgICAgICAgICAgICAgICAgICAgKFwiYnJlYWtcIik7ICAgICAgICAgICAvLyBmb3Igbm9uLXJlcGVhdGVkIGZpZWxkcywganVzdCBpZ25vcmVcclxuICAgICAgICAgICAgICAgICAgICBkZWZhdWx0QWxyZWFkeUVtaXR0ZWQgPSB0cnVlO1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgZ2VuXHJcbiAgICAgICAgICAgICAgICAoXCJjYXNlJWo6XCIsIGtleXNbaV0pXHJcbiAgICAgICAgICAgICAgICAoXCJjYXNlICVpOlwiLCB2YWx1ZXNba2V5c1tpXV0pXHJcbiAgICAgICAgICAgICAgICAgICAgKFwibSVzPSVqXCIsIHByb3AsIHZhbHVlc1trZXlzW2ldXSlcclxuICAgICAgICAgICAgICAgICAgICAoXCJicmVha1wiKTtcclxuICAgICAgICAgICAgfSBnZW5cclxuICAgICAgICAgICAgKFwifVwiKTtcclxuICAgICAgICB9IGVsc2UgZ2VuXHJcbiAgICAgICAgICAgIChcImlmKHR5cGVvZiBkJXMhPT1cXFwib2JqZWN0XFxcIilcIiwgcHJvcClcclxuICAgICAgICAgICAgICAgIChcInRocm93IFR5cGVFcnJvciglailcIiwgZmllbGQuZnVsbE5hbWUgKyBcIjogb2JqZWN0IGV4cGVjdGVkXCIpXHJcbiAgICAgICAgICAgIChcIm0lcz10eXBlc1slaV0uZnJvbU9iamVjdChkJXMpXCIsIHByb3AsIGZpZWxkSW5kZXgsIHByb3ApO1xyXG4gICAgfSBlbHNlIHtcclxuICAgICAgICB2YXIgaXNVbnNpZ25lZCA9IGZhbHNlO1xyXG4gICAgICAgIHN3aXRjaCAoZmllbGQudHlwZSkge1xyXG4gICAgICAgICAgICBjYXNlIFwiZG91YmxlXCI6XHJcbiAgICAgICAgICAgIGNhc2UgXCJmbG9hdFwiOiBnZW5cclxuICAgICAgICAgICAgICAgIChcIm0lcz1OdW1iZXIoZCVzKVwiLCBwcm9wLCBwcm9wKTsgLy8gYWxzbyBjYXRjaGVzIFwiTmFOXCIsIFwiSW5maW5pdHlcIlxyXG4gICAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgICAgIGNhc2UgXCJ1aW50MzJcIjpcclxuICAgICAgICAgICAgY2FzZSBcImZpeGVkMzJcIjogZ2VuXHJcbiAgICAgICAgICAgICAgICAoXCJtJXM9ZCVzPj4+MFwiLCBwcm9wLCBwcm9wKTtcclxuICAgICAgICAgICAgICAgIGJyZWFrO1xyXG4gICAgICAgICAgICBjYXNlIFwiaW50MzJcIjpcclxuICAgICAgICAgICAgY2FzZSBcInNpbnQzMlwiOlxyXG4gICAgICAgICAgICBjYXNlIFwic2ZpeGVkMzJcIjogZ2VuXHJcbiAgICAgICAgICAgICAgICAoXCJtJXM9ZCVzfDBcIiwgcHJvcCwgcHJvcCk7XHJcbiAgICAgICAgICAgICAgICBicmVhaztcclxuICAgICAgICAgICAgY2FzZSBcInVpbnQ2NFwiOlxyXG4gICAgICAgICAgICAgICAgaXNVbnNpZ25lZCA9IHRydWU7XHJcbiAgICAgICAgICAgICAgICAvLyBlc2xpbnQtZGlzYWJsZS1uZXh0LWxpbmUgbm8tZmFsbHRocm91Z2hcclxuICAgICAgICAgICAgY2FzZSBcImludDY0XCI6XHJcbiAgICAgICAgICAgIGNhc2UgXCJzaW50NjRcIjpcclxuICAgICAgICAgICAgY2FzZSBcImZpeGVkNjRcIjpcclxuICAgICAgICAgICAgY2FzZSBcInNmaXhlZDY0XCI6IGdlblxyXG4gICAgICAgICAgICAgICAgKFwiaWYodXRpbC5Mb25nKVwiKVxyXG4gICAgICAgICAgICAgICAgICAgIChcIihtJXM9dXRpbC5Mb25nLmZyb21WYWx1ZShkJXMpKS51bnNpZ25lZD0lalwiLCBwcm9wLCBwcm9wLCBpc1Vuc2lnbmVkKVxyXG4gICAgICAgICAgICAgICAgKFwiZWxzZSBpZih0eXBlb2YgZCVzPT09XFxcInN0cmluZ1xcXCIpXCIsIHByb3ApXHJcbiAgICAgICAgICAgICAgICAgICAgKFwibSVzPXBhcnNlSW50KGQlcywxMClcIiwgcHJvcCwgcHJvcClcclxuICAgICAgICAgICAgICAgIChcImVsc2UgaWYodHlwZW9mIGQlcz09PVxcXCJudW1iZXJcXFwiKVwiLCBwcm9wKVxyXG4gICAgICAgICAgICAgICAgICAgIChcIm0lcz1kJXNcIiwgcHJvcCwgcHJvcClcclxuICAgICAgICAgICAgICAgIChcImVsc2UgaWYodHlwZW9mIGQlcz09PVxcXCJvYmplY3RcXFwiKVwiLCBwcm9wKVxyXG4gICAgICAgICAgICAgICAgICAgIChcIm0lcz1uZXcgdXRpbC5Mb25nQml0cyhkJXMubG93Pj4+MCxkJXMuaGlnaD4+PjApLnRvTnVtYmVyKCVzKVwiLCBwcm9wLCBwcm9wLCBwcm9wLCBpc1Vuc2lnbmVkID8gXCJ0cnVlXCIgOiBcIlwiKTtcclxuICAgICAgICAgICAgICAgIGJyZWFrO1xyXG4gICAgICAgICAgICBjYXNlIFwiYnl0ZXNcIjogZ2VuXHJcbiAgICAgICAgICAgICAgICAoXCJpZih0eXBlb2YgZCVzPT09XFxcInN0cmluZ1xcXCIpXCIsIHByb3ApXHJcbiAgICAgICAgICAgICAgICAgICAgKFwidXRpbC5iYXNlNjQuZGVjb2RlKGQlcyxtJXM9dXRpbC5uZXdCdWZmZXIodXRpbC5iYXNlNjQubGVuZ3RoKGQlcykpLDApXCIsIHByb3AsIHByb3AsIHByb3ApXHJcbiAgICAgICAgICAgICAgICAoXCJlbHNlIGlmKGQlcy5sZW5ndGggPj0gMClcIiwgcHJvcClcclxuICAgICAgICAgICAgICAgICAgICAoXCJtJXM9ZCVzXCIsIHByb3AsIHByb3ApO1xyXG4gICAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgICAgIGNhc2UgXCJzdHJpbmdcIjogZ2VuXHJcbiAgICAgICAgICAgICAgICAoXCJtJXM9U3RyaW5nKGQlcylcIiwgcHJvcCwgcHJvcCk7XHJcbiAgICAgICAgICAgICAgICBicmVhaztcclxuICAgICAgICAgICAgY2FzZSBcImJvb2xcIjogZ2VuXHJcbiAgICAgICAgICAgICAgICAoXCJtJXM9Qm9vbGVhbihkJXMpXCIsIHByb3AsIHByb3ApO1xyXG4gICAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgICAgIC8qIGRlZmF1bHQ6IGdlblxyXG4gICAgICAgICAgICAgICAgKFwibSVzPWQlc1wiLCBwcm9wLCBwcm9wKTtcclxuICAgICAgICAgICAgICAgIGJyZWFrOyAqL1xyXG4gICAgICAgIH1cclxuICAgIH1cclxuICAgIHJldHVybiBnZW47XHJcbiAgICAvKiBlc2xpbnQtZW5hYmxlIG5vLXVuZXhwZWN0ZWQtbXVsdGlsaW5lLCBibG9jay1zY29wZWQtdmFyLCBuby1yZWRlY2xhcmUgKi9cclxufVxyXG5cclxuLyoqXHJcbiAqIEdlbmVyYXRlcyBhIHBsYWluIG9iamVjdCB0byBydW50aW1lIG1lc3NhZ2UgY29udmVydGVyIHNwZWNpZmljIHRvIHRoZSBzcGVjaWZpZWQgbWVzc2FnZSB0eXBlLlxyXG4gKiBAcGFyYW0ge1R5cGV9IG10eXBlIE1lc3NhZ2UgdHlwZVxyXG4gKiBAcmV0dXJucyB7Q29kZWdlbn0gQ29kZWdlbiBpbnN0YW5jZVxyXG4gKi9cclxuY29udmVydGVyLmZyb21PYmplY3QgPSBmdW5jdGlvbiBmcm9tT2JqZWN0KG10eXBlKSB7XHJcbiAgICAvKiBlc2xpbnQtZGlzYWJsZSBuby11bmV4cGVjdGVkLW11bHRpbGluZSwgYmxvY2stc2NvcGVkLXZhciwgbm8tcmVkZWNsYXJlICovXHJcbiAgICB2YXIgZmllbGRzID0gbXR5cGUuZmllbGRzQXJyYXk7XHJcbiAgICB2YXIgZ2VuID0gdXRpbC5jb2RlZ2VuKFtcImRcIl0sIG10eXBlLm5hbWUgKyBcIiRmcm9tT2JqZWN0XCIpXHJcbiAgICAoXCJpZihkIGluc3RhbmNlb2YgdGhpcy5jdG9yKVwiKVxyXG4gICAgICAgIChcInJldHVybiBkXCIpO1xyXG4gICAgaWYgKCFmaWVsZHMubGVuZ3RoKSByZXR1cm4gZ2VuXHJcbiAgICAoXCJyZXR1cm4gbmV3IHRoaXMuY3RvclwiKTtcclxuICAgIGdlblxyXG4gICAgKFwidmFyIG09bmV3IHRoaXMuY3RvclwiKTtcclxuICAgIGZvciAodmFyIGkgPSAwOyBpIDwgZmllbGRzLmxlbmd0aDsgKytpKSB7XHJcbiAgICAgICAgdmFyIGZpZWxkICA9IGZpZWxkc1tpXS5yZXNvbHZlKCksXHJcbiAgICAgICAgICAgIHByb3AgICA9IHV0aWwuc2FmZVByb3AoZmllbGQubmFtZSk7XHJcblxyXG4gICAgICAgIC8vIE1hcCBmaWVsZHNcclxuICAgICAgICBpZiAoZmllbGQubWFwKSB7IGdlblxyXG4gICAgKFwiaWYoZCVzKXtcIiwgcHJvcClcclxuICAgICAgICAoXCJpZih0eXBlb2YgZCVzIT09XFxcIm9iamVjdFxcXCIpXCIsIHByb3ApXHJcbiAgICAgICAgICAgIChcInRocm93IFR5cGVFcnJvciglailcIiwgZmllbGQuZnVsbE5hbWUgKyBcIjogb2JqZWN0IGV4cGVjdGVkXCIpXHJcbiAgICAgICAgKFwibSVzPXt9XCIsIHByb3ApXHJcbiAgICAgICAgKFwiZm9yKHZhciBrcz1PYmplY3Qua2V5cyhkJXMpLGk9MDtpPGtzLmxlbmd0aDsrK2kpe1wiLCBwcm9wKTtcclxuICAgICAgICAgICAgZ2VuVmFsdWVQYXJ0aWFsX2Zyb21PYmplY3QoZ2VuLCBmaWVsZCwgLyogbm90IHNvcnRlZCAqLyBpLCBwcm9wICsgXCJba3NbaV1dXCIpXHJcbiAgICAgICAgKFwifVwiKVxyXG4gICAgKFwifVwiKTtcclxuXHJcbiAgICAgICAgLy8gUmVwZWF0ZWQgZmllbGRzXHJcbiAgICAgICAgfSBlbHNlIGlmIChmaWVsZC5yZXBlYXRlZCkgeyBnZW5cclxuICAgIChcImlmKGQlcyl7XCIsIHByb3ApXHJcbiAgICAgICAgKFwiaWYoIUFycmF5LmlzQXJyYXkoZCVzKSlcIiwgcHJvcClcclxuICAgICAgICAgICAgKFwidGhyb3cgVHlwZUVycm9yKCVqKVwiLCBmaWVsZC5mdWxsTmFtZSArIFwiOiBhcnJheSBleHBlY3RlZFwiKVxyXG4gICAgICAgIChcIm0lcz1bXVwiLCBwcm9wKVxyXG4gICAgICAgIChcImZvcih2YXIgaT0wO2k8ZCVzLmxlbmd0aDsrK2kpe1wiLCBwcm9wKTtcclxuICAgICAgICAgICAgZ2VuVmFsdWVQYXJ0aWFsX2Zyb21PYmplY3QoZ2VuLCBmaWVsZCwgLyogbm90IHNvcnRlZCAqLyBpLCBwcm9wICsgXCJbaV1cIilcclxuICAgICAgICAoXCJ9XCIpXHJcbiAgICAoXCJ9XCIpO1xyXG5cclxuICAgICAgICAvLyBOb24tcmVwZWF0ZWQgZmllbGRzXHJcbiAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgaWYgKCEoZmllbGQucmVzb2x2ZWRUeXBlIGluc3RhbmNlb2YgRW51bSkpIGdlbiAvLyBubyBuZWVkIHRvIHRlc3QgZm9yIG51bGwvdW5kZWZpbmVkIGlmIGFuIGVudW0gKHVzZXMgc3dpdGNoKVxyXG4gICAgKFwiaWYoZCVzIT1udWxsKXtcIiwgcHJvcCk7IC8vICE9PSB1bmRlZmluZWQgJiYgIT09IG51bGxcclxuICAgICAgICBnZW5WYWx1ZVBhcnRpYWxfZnJvbU9iamVjdChnZW4sIGZpZWxkLCAvKiBub3Qgc29ydGVkICovIGksIHByb3ApO1xyXG4gICAgICAgICAgICBpZiAoIShmaWVsZC5yZXNvbHZlZFR5cGUgaW5zdGFuY2VvZiBFbnVtKSkgZ2VuXHJcbiAgICAoXCJ9XCIpO1xyXG4gICAgICAgIH1cclxuICAgIH0gcmV0dXJuIGdlblxyXG4gICAgKFwicmV0dXJuIG1cIik7XHJcbiAgICAvKiBlc2xpbnQtZW5hYmxlIG5vLXVuZXhwZWN0ZWQtbXVsdGlsaW5lLCBibG9jay1zY29wZWQtdmFyLCBuby1yZWRlY2xhcmUgKi9cclxufTtcclxuXHJcbi8qKlxyXG4gKiBHZW5lcmF0ZXMgYSBwYXJ0aWFsIHZhbHVlIHRvT2JqZWN0IGNvbnZlcnRlci5cclxuICogQHBhcmFtIHtDb2RlZ2VufSBnZW4gQ29kZWdlbiBpbnN0YW5jZVxyXG4gKiBAcGFyYW0ge0ZpZWxkfSBmaWVsZCBSZWZsZWN0ZWQgZmllbGRcclxuICogQHBhcmFtIHtudW1iZXJ9IGZpZWxkSW5kZXggRmllbGQgaW5kZXhcclxuICogQHBhcmFtIHtzdHJpbmd9IHByb3AgUHJvcGVydHkgcmVmZXJlbmNlXHJcbiAqIEByZXR1cm5zIHtDb2RlZ2VufSBDb2RlZ2VuIGluc3RhbmNlXHJcbiAqIEBpZ25vcmVcclxuICovXHJcbmZ1bmN0aW9uIGdlblZhbHVlUGFydGlhbF90b09iamVjdChnZW4sIGZpZWxkLCBmaWVsZEluZGV4LCBwcm9wKSB7XHJcbiAgICAvKiBlc2xpbnQtZGlzYWJsZSBuby11bmV4cGVjdGVkLW11bHRpbGluZSwgYmxvY2stc2NvcGVkLXZhciwgbm8tcmVkZWNsYXJlICovXHJcbiAgICBpZiAoZmllbGQucmVzb2x2ZWRUeXBlKSB7XHJcbiAgICAgICAgaWYgKGZpZWxkLnJlc29sdmVkVHlwZSBpbnN0YW5jZW9mIEVudW0pIGdlblxyXG4gICAgICAgICAgICAoXCJkJXM9by5lbnVtcz09PVN0cmluZz8odHlwZXNbJWldLnZhbHVlc1ttJXNdPT09dW5kZWZpbmVkP20lczp0eXBlc1slaV0udmFsdWVzW20lc10pOm0lc1wiLCBwcm9wLCBmaWVsZEluZGV4LCBwcm9wLCBwcm9wLCBmaWVsZEluZGV4LCBwcm9wLCBwcm9wKTtcclxuICAgICAgICBlbHNlIGdlblxyXG4gICAgICAgICAgICAoXCJkJXM9dHlwZXNbJWldLnRvT2JqZWN0KG0lcyxvKVwiLCBwcm9wLCBmaWVsZEluZGV4LCBwcm9wKTtcclxuICAgIH0gZWxzZSB7XHJcbiAgICAgICAgdmFyIGlzVW5zaWduZWQgPSBmYWxzZTtcclxuICAgICAgICBzd2l0Y2ggKGZpZWxkLnR5cGUpIHtcclxuICAgICAgICAgICAgY2FzZSBcImRvdWJsZVwiOlxyXG4gICAgICAgICAgICBjYXNlIFwiZmxvYXRcIjogZ2VuXHJcbiAgICAgICAgICAgIChcImQlcz1vLmpzb24mJiFpc0Zpbml0ZShtJXMpP1N0cmluZyhtJXMpOm0lc1wiLCBwcm9wLCBwcm9wLCBwcm9wLCBwcm9wKTtcclxuICAgICAgICAgICAgICAgIGJyZWFrO1xyXG4gICAgICAgICAgICBjYXNlIFwidWludDY0XCI6XHJcbiAgICAgICAgICAgICAgICBpc1Vuc2lnbmVkID0gdHJ1ZTtcclxuICAgICAgICAgICAgICAgIC8vIGVzbGludC1kaXNhYmxlLW5leHQtbGluZSBuby1mYWxsdGhyb3VnaFxyXG4gICAgICAgICAgICBjYXNlIFwiaW50NjRcIjpcclxuICAgICAgICAgICAgY2FzZSBcInNpbnQ2NFwiOlxyXG4gICAgICAgICAgICBjYXNlIFwiZml4ZWQ2NFwiOlxyXG4gICAgICAgICAgICBjYXNlIFwic2ZpeGVkNjRcIjogZ2VuXHJcbiAgICAgICAgICAgIChcImlmKHR5cGVvZiBtJXM9PT1cXFwibnVtYmVyXFxcIilcIiwgcHJvcClcclxuICAgICAgICAgICAgICAgIChcImQlcz1vLmxvbmdzPT09U3RyaW5nP1N0cmluZyhtJXMpOm0lc1wiLCBwcm9wLCBwcm9wLCBwcm9wKVxyXG4gICAgICAgICAgICAoXCJlbHNlXCIpIC8vIExvbmctbGlrZVxyXG4gICAgICAgICAgICAgICAgKFwiZCVzPW8ubG9uZ3M9PT1TdHJpbmc/dXRpbC5Mb25nLnByb3RvdHlwZS50b1N0cmluZy5jYWxsKG0lcyk6by5sb25ncz09PU51bWJlcj9uZXcgdXRpbC5Mb25nQml0cyhtJXMubG93Pj4+MCxtJXMuaGlnaD4+PjApLnRvTnVtYmVyKCVzKTptJXNcIiwgcHJvcCwgcHJvcCwgcHJvcCwgcHJvcCwgaXNVbnNpZ25lZCA/IFwidHJ1ZVwiOiBcIlwiLCBwcm9wKTtcclxuICAgICAgICAgICAgICAgIGJyZWFrO1xyXG4gICAgICAgICAgICBjYXNlIFwiYnl0ZXNcIjogZ2VuXHJcbiAgICAgICAgICAgIChcImQlcz1vLmJ5dGVzPT09U3RyaW5nP3V0aWwuYmFzZTY0LmVuY29kZShtJXMsMCxtJXMubGVuZ3RoKTpvLmJ5dGVzPT09QXJyYXk/QXJyYXkucHJvdG90eXBlLnNsaWNlLmNhbGwobSVzKTptJXNcIiwgcHJvcCwgcHJvcCwgcHJvcCwgcHJvcCwgcHJvcCk7XHJcbiAgICAgICAgICAgICAgICBicmVhaztcclxuICAgICAgICAgICAgZGVmYXVsdDogZ2VuXHJcbiAgICAgICAgICAgIChcImQlcz1tJXNcIiwgcHJvcCwgcHJvcCk7XHJcbiAgICAgICAgICAgICAgICBicmVhaztcclxuICAgICAgICB9XHJcbiAgICB9XHJcbiAgICByZXR1cm4gZ2VuO1xyXG4gICAgLyogZXNsaW50LWVuYWJsZSBuby11bmV4cGVjdGVkLW11bHRpbGluZSwgYmxvY2stc2NvcGVkLXZhciwgbm8tcmVkZWNsYXJlICovXHJcbn1cclxuXHJcbi8qKlxyXG4gKiBHZW5lcmF0ZXMgYSBydW50aW1lIG1lc3NhZ2UgdG8gcGxhaW4gb2JqZWN0IGNvbnZlcnRlciBzcGVjaWZpYyB0byB0aGUgc3BlY2lmaWVkIG1lc3NhZ2UgdHlwZS5cclxuICogQHBhcmFtIHtUeXBlfSBtdHlwZSBNZXNzYWdlIHR5cGVcclxuICogQHJldHVybnMge0NvZGVnZW59IENvZGVnZW4gaW5zdGFuY2VcclxuICovXHJcbmNvbnZlcnRlci50b09iamVjdCA9IGZ1bmN0aW9uIHRvT2JqZWN0KG10eXBlKSB7XHJcbiAgICAvKiBlc2xpbnQtZGlzYWJsZSBuby11bmV4cGVjdGVkLW11bHRpbGluZSwgYmxvY2stc2NvcGVkLXZhciwgbm8tcmVkZWNsYXJlICovXHJcbiAgICB2YXIgZmllbGRzID0gbXR5cGUuZmllbGRzQXJyYXkuc2xpY2UoKS5zb3J0KHV0aWwuY29tcGFyZUZpZWxkc0J5SWQpO1xyXG4gICAgaWYgKCFmaWVsZHMubGVuZ3RoKVxyXG4gICAgICAgIHJldHVybiB1dGlsLmNvZGVnZW4oKShcInJldHVybiB7fVwiKTtcclxuICAgIHZhciBnZW4gPSB1dGlsLmNvZGVnZW4oW1wibVwiLCBcIm9cIl0sIG10eXBlLm5hbWUgKyBcIiR0b09iamVjdFwiKVxyXG4gICAgKFwiaWYoIW8pXCIpXHJcbiAgICAgICAgKFwibz17fVwiKVxyXG4gICAgKFwidmFyIGQ9e31cIik7XHJcblxyXG4gICAgdmFyIHJlcGVhdGVkRmllbGRzID0gW10sXHJcbiAgICAgICAgbWFwRmllbGRzID0gW10sXHJcbiAgICAgICAgbm9ybWFsRmllbGRzID0gW10sXHJcbiAgICAgICAgaSA9IDA7XHJcbiAgICBmb3IgKDsgaSA8IGZpZWxkcy5sZW5ndGg7ICsraSlcclxuICAgICAgICBpZiAoIWZpZWxkc1tpXS5wYXJ0T2YpXHJcbiAgICAgICAgICAgICggZmllbGRzW2ldLnJlc29sdmUoKS5yZXBlYXRlZCA/IHJlcGVhdGVkRmllbGRzXHJcbiAgICAgICAgICAgIDogZmllbGRzW2ldLm1hcCA/IG1hcEZpZWxkc1xyXG4gICAgICAgICAgICA6IG5vcm1hbEZpZWxkcykucHVzaChmaWVsZHNbaV0pO1xyXG5cclxuICAgIGlmIChyZXBlYXRlZEZpZWxkcy5sZW5ndGgpIHsgZ2VuXHJcbiAgICAoXCJpZihvLmFycmF5c3x8by5kZWZhdWx0cyl7XCIpO1xyXG4gICAgICAgIGZvciAoaSA9IDA7IGkgPCByZXBlYXRlZEZpZWxkcy5sZW5ndGg7ICsraSkgZ2VuXHJcbiAgICAgICAgKFwiZCVzPVtdXCIsIHV0aWwuc2FmZVByb3AocmVwZWF0ZWRGaWVsZHNbaV0ubmFtZSkpO1xyXG4gICAgICAgIGdlblxyXG4gICAgKFwifVwiKTtcclxuICAgIH1cclxuXHJcbiAgICBpZiAobWFwRmllbGRzLmxlbmd0aCkgeyBnZW5cclxuICAgIChcImlmKG8ub2JqZWN0c3x8by5kZWZhdWx0cyl7XCIpO1xyXG4gICAgICAgIGZvciAoaSA9IDA7IGkgPCBtYXBGaWVsZHMubGVuZ3RoOyArK2kpIGdlblxyXG4gICAgICAgIChcImQlcz17fVwiLCB1dGlsLnNhZmVQcm9wKG1hcEZpZWxkc1tpXS5uYW1lKSk7XHJcbiAgICAgICAgZ2VuXHJcbiAgICAoXCJ9XCIpO1xyXG4gICAgfVxyXG5cclxuICAgIGlmIChub3JtYWxGaWVsZHMubGVuZ3RoKSB7IGdlblxyXG4gICAgKFwiaWYoby5kZWZhdWx0cyl7XCIpO1xyXG4gICAgICAgIGZvciAoaSA9IDA7IGkgPCBub3JtYWxGaWVsZHMubGVuZ3RoOyArK2kpIHtcclxuICAgICAgICAgICAgdmFyIGZpZWxkID0gbm9ybWFsRmllbGRzW2ldLFxyXG4gICAgICAgICAgICAgICAgcHJvcCAgPSB1dGlsLnNhZmVQcm9wKGZpZWxkLm5hbWUpO1xyXG4gICAgICAgICAgICBpZiAoZmllbGQucmVzb2x2ZWRUeXBlIGluc3RhbmNlb2YgRW51bSkgZ2VuXHJcbiAgICAgICAgKFwiZCVzPW8uZW51bXM9PT1TdHJpbmc/JWo6JWpcIiwgcHJvcCwgZmllbGQucmVzb2x2ZWRUeXBlLnZhbHVlc0J5SWRbZmllbGQudHlwZURlZmF1bHRdLCBmaWVsZC50eXBlRGVmYXVsdCk7XHJcbiAgICAgICAgICAgIGVsc2UgaWYgKGZpZWxkLmxvbmcpIGdlblxyXG4gICAgICAgIChcImlmKHV0aWwuTG9uZyl7XCIpXHJcbiAgICAgICAgICAgIChcInZhciBuPW5ldyB1dGlsLkxvbmcoJWksJWksJWopXCIsIGZpZWxkLnR5cGVEZWZhdWx0LmxvdywgZmllbGQudHlwZURlZmF1bHQuaGlnaCwgZmllbGQudHlwZURlZmF1bHQudW5zaWduZWQpXHJcbiAgICAgICAgICAgIChcImQlcz1vLmxvbmdzPT09U3RyaW5nP24udG9TdHJpbmcoKTpvLmxvbmdzPT09TnVtYmVyP24udG9OdW1iZXIoKTpuXCIsIHByb3ApXHJcbiAgICAgICAgKFwifWVsc2VcIilcclxuICAgICAgICAgICAgKFwiZCVzPW8ubG9uZ3M9PT1TdHJpbmc/JWo6JWlcIiwgcHJvcCwgZmllbGQudHlwZURlZmF1bHQudG9TdHJpbmcoKSwgZmllbGQudHlwZURlZmF1bHQudG9OdW1iZXIoKSk7XHJcbiAgICAgICAgICAgIGVsc2UgaWYgKGZpZWxkLmJ5dGVzKSB7XHJcbiAgICAgICAgICAgICAgICB2YXIgYXJyYXlEZWZhdWx0ID0gXCJbXCIgKyBBcnJheS5wcm90b3R5cGUuc2xpY2UuY2FsbChmaWVsZC50eXBlRGVmYXVsdCkuam9pbihcIixcIikgKyBcIl1cIjtcclxuICAgICAgICAgICAgICAgIGdlblxyXG4gICAgICAgIChcImlmKG8uYnl0ZXM9PT1TdHJpbmcpZCVzPSVqXCIsIHByb3AsIFN0cmluZy5mcm9tQ2hhckNvZGUuYXBwbHkoU3RyaW5nLCBmaWVsZC50eXBlRGVmYXVsdCkpXHJcbiAgICAgICAgKFwiZWxzZXtcIilcclxuICAgICAgICAgICAgKFwiZCVzPSVzXCIsIHByb3AsIGFycmF5RGVmYXVsdClcclxuICAgICAgICAgICAgKFwiaWYoby5ieXRlcyE9PUFycmF5KWQlcz11dGlsLm5ld0J1ZmZlcihkJXMpXCIsIHByb3AsIHByb3ApXHJcbiAgICAgICAgKFwifVwiKTtcclxuICAgICAgICAgICAgfSBlbHNlIGdlblxyXG4gICAgICAgIChcImQlcz0lalwiLCBwcm9wLCBmaWVsZC50eXBlRGVmYXVsdCk7IC8vIGFsc28gbWVzc2FnZXMgKD1udWxsKVxyXG4gICAgICAgIH0gZ2VuXHJcbiAgICAoXCJ9XCIpO1xyXG4gICAgfVxyXG4gICAgdmFyIGhhc0tzMiA9IGZhbHNlO1xyXG4gICAgZm9yIChpID0gMDsgaSA8IGZpZWxkcy5sZW5ndGg7ICsraSkge1xyXG4gICAgICAgIHZhciBmaWVsZCA9IGZpZWxkc1tpXSxcclxuICAgICAgICAgICAgaW5kZXggPSBtdHlwZS5fZmllbGRzQXJyYXkuaW5kZXhPZihmaWVsZCksXHJcbiAgICAgICAgICAgIHByb3AgID0gdXRpbC5zYWZlUHJvcChmaWVsZC5uYW1lKTtcclxuICAgICAgICBpZiAoZmllbGQubWFwKSB7XHJcbiAgICAgICAgICAgIGlmICghaGFzS3MyKSB7IGhhc0tzMiA9IHRydWU7IGdlblxyXG4gICAgKFwidmFyIGtzMlwiKTtcclxuICAgICAgICAgICAgfSBnZW5cclxuICAgIChcImlmKG0lcyYmKGtzMj1PYmplY3Qua2V5cyhtJXMpKS5sZW5ndGgpe1wiLCBwcm9wLCBwcm9wKVxyXG4gICAgICAgIChcImQlcz17fVwiLCBwcm9wKVxyXG4gICAgICAgIChcImZvcih2YXIgaj0wO2o8a3MyLmxlbmd0aDsrK2ope1wiKTtcclxuICAgICAgICAgICAgZ2VuVmFsdWVQYXJ0aWFsX3RvT2JqZWN0KGdlbiwgZmllbGQsIC8qIHNvcnRlZCAqLyBpbmRleCwgcHJvcCArIFwiW2tzMltqXV1cIilcclxuICAgICAgICAoXCJ9XCIpO1xyXG4gICAgICAgIH0gZWxzZSBpZiAoZmllbGQucmVwZWF0ZWQpIHsgZ2VuXHJcbiAgICAoXCJpZihtJXMmJm0lcy5sZW5ndGgpe1wiLCBwcm9wLCBwcm9wKVxyXG4gICAgICAgIChcImQlcz1bXVwiLCBwcm9wKVxyXG4gICAgICAgIChcImZvcih2YXIgaj0wO2o8bSVzLmxlbmd0aDsrK2ope1wiLCBwcm9wKTtcclxuICAgICAgICAgICAgZ2VuVmFsdWVQYXJ0aWFsX3RvT2JqZWN0KGdlbiwgZmllbGQsIC8qIHNvcnRlZCAqLyBpbmRleCwgcHJvcCArIFwiW2pdXCIpXHJcbiAgICAgICAgKFwifVwiKTtcclxuICAgICAgICB9IGVsc2UgeyBnZW5cclxuICAgIChcImlmKG0lcyE9bnVsbCYmbS5oYXNPd25Qcm9wZXJ0eSglaikpe1wiLCBwcm9wLCBmaWVsZC5uYW1lKTsgLy8gIT09IHVuZGVmaW5lZCAmJiAhPT0gbnVsbFxyXG4gICAgICAgIGdlblZhbHVlUGFydGlhbF90b09iamVjdChnZW4sIGZpZWxkLCAvKiBzb3J0ZWQgKi8gaW5kZXgsIHByb3ApO1xyXG4gICAgICAgIGlmIChmaWVsZC5wYXJ0T2YpIGdlblxyXG4gICAgICAgIChcImlmKG8ub25lb2ZzKVwiKVxyXG4gICAgICAgICAgICAoXCJkJXM9JWpcIiwgdXRpbC5zYWZlUHJvcChmaWVsZC5wYXJ0T2YubmFtZSksIGZpZWxkLm5hbWUpO1xyXG4gICAgICAgIH1cclxuICAgICAgICBnZW5cclxuICAgIChcIn1cIik7XHJcbiAgICB9XHJcbiAgICByZXR1cm4gZ2VuXHJcbiAgICAoXCJyZXR1cm4gZFwiKTtcclxuICAgIC8qIGVzbGludC1lbmFibGUgbm8tdW5leHBlY3RlZC1tdWx0aWxpbmUsIGJsb2NrLXNjb3BlZC12YXIsIG5vLXJlZGVjbGFyZSAqL1xyXG59O1xyXG4iLCJcInVzZSBzdHJpY3RcIjtcclxubW9kdWxlLmV4cG9ydHMgPSBkZWNvZGVyO1xyXG5cclxudmFyIEVudW0gICAgPSByZXF1aXJlKFwiLi9lbnVtXCIpLFxyXG4gICAgdHlwZXMgICA9IHJlcXVpcmUoXCIuL3R5cGVzXCIpLFxyXG4gICAgdXRpbCAgICA9IHJlcXVpcmUoXCIuL3V0aWxcIik7XHJcblxyXG5mdW5jdGlvbiBtaXNzaW5nKGZpZWxkKSB7XHJcbiAgICByZXR1cm4gXCJtaXNzaW5nIHJlcXVpcmVkICdcIiArIGZpZWxkLm5hbWUgKyBcIidcIjtcclxufVxyXG5cclxuLyoqXHJcbiAqIEdlbmVyYXRlcyBhIGRlY29kZXIgc3BlY2lmaWMgdG8gdGhlIHNwZWNpZmllZCBtZXNzYWdlIHR5cGUuXHJcbiAqIEBwYXJhbSB7VHlwZX0gbXR5cGUgTWVzc2FnZSB0eXBlXHJcbiAqIEByZXR1cm5zIHtDb2RlZ2VufSBDb2RlZ2VuIGluc3RhbmNlXHJcbiAqL1xyXG5mdW5jdGlvbiBkZWNvZGVyKG10eXBlKSB7XHJcbiAgICAvKiBlc2xpbnQtZGlzYWJsZSBuby11bmV4cGVjdGVkLW11bHRpbGluZSAqL1xyXG4gICAgdmFyIGdlbiA9IHV0aWwuY29kZWdlbihbXCJyXCIsIFwibFwiXSwgbXR5cGUubmFtZSArIFwiJGRlY29kZVwiKVxyXG4gICAgKFwiaWYoIShyIGluc3RhbmNlb2YgUmVhZGVyKSlcIilcclxuICAgICAgICAoXCJyPVJlYWRlci5jcmVhdGUocilcIilcclxuICAgIChcInZhciBjPWw9PT11bmRlZmluZWQ/ci5sZW46ci5wb3MrbCxtPW5ldyB0aGlzLmN0b3JcIiArIChtdHlwZS5maWVsZHNBcnJheS5maWx0ZXIoZnVuY3Rpb24oZmllbGQpIHsgcmV0dXJuIGZpZWxkLm1hcDsgfSkubGVuZ3RoID8gXCIsayx2YWx1ZVwiIDogXCJcIikpXHJcbiAgICAoXCJ3aGlsZShyLnBvczxjKXtcIilcclxuICAgICAgICAoXCJ2YXIgdD1yLnVpbnQzMigpXCIpO1xyXG4gICAgaWYgKG10eXBlLmdyb3VwKSBnZW5cclxuICAgICAgICAoXCJpZigodCY3KT09PTQpXCIpXHJcbiAgICAgICAgICAgIChcImJyZWFrXCIpO1xyXG4gICAgZ2VuXHJcbiAgICAgICAgKFwic3dpdGNoKHQ+Pj4zKXtcIik7XHJcblxyXG4gICAgdmFyIGkgPSAwO1xyXG4gICAgZm9yICg7IGkgPCAvKiBpbml0aWFsaXplcyAqLyBtdHlwZS5maWVsZHNBcnJheS5sZW5ndGg7ICsraSkge1xyXG4gICAgICAgIHZhciBmaWVsZCA9IG10eXBlLl9maWVsZHNBcnJheVtpXS5yZXNvbHZlKCksXHJcbiAgICAgICAgICAgIHR5cGUgID0gZmllbGQucmVzb2x2ZWRUeXBlIGluc3RhbmNlb2YgRW51bSA/IFwiaW50MzJcIiA6IGZpZWxkLnR5cGUsXHJcbiAgICAgICAgICAgIHJlZiAgID0gXCJtXCIgKyB1dGlsLnNhZmVQcm9wKGZpZWxkLm5hbWUpOyBnZW5cclxuICAgICAgICAgICAgKFwiY2FzZSAlaToge1wiLCBmaWVsZC5pZCk7XHJcblxyXG4gICAgICAgIC8vIE1hcCBmaWVsZHNcclxuICAgICAgICBpZiAoZmllbGQubWFwKSB7IGdlblxyXG4gICAgICAgICAgICAgICAgKFwiaWYoJXM9PT11dGlsLmVtcHR5T2JqZWN0KVwiLCByZWYpXHJcbiAgICAgICAgICAgICAgICAgICAgKFwiJXM9e31cIiwgcmVmKVxyXG4gICAgICAgICAgICAgICAgKFwidmFyIGMyID0gci51aW50MzIoKStyLnBvc1wiKTtcclxuXHJcbiAgICAgICAgICAgIGlmICh0eXBlcy5kZWZhdWx0c1tmaWVsZC5rZXlUeXBlXSAhPT0gdW5kZWZpbmVkKSBnZW5cclxuICAgICAgICAgICAgICAgIChcIms9JWpcIiwgdHlwZXMuZGVmYXVsdHNbZmllbGQua2V5VHlwZV0pO1xyXG4gICAgICAgICAgICBlbHNlIGdlblxyXG4gICAgICAgICAgICAgICAgKFwiaz1udWxsXCIpO1xyXG5cclxuICAgICAgICAgICAgaWYgKHR5cGVzLmRlZmF1bHRzW3R5cGVdICE9PSB1bmRlZmluZWQpIGdlblxyXG4gICAgICAgICAgICAgICAgKFwidmFsdWU9JWpcIiwgdHlwZXMuZGVmYXVsdHNbdHlwZV0pO1xyXG4gICAgICAgICAgICBlbHNlIGdlblxyXG4gICAgICAgICAgICAgICAgKFwidmFsdWU9bnVsbFwiKTtcclxuXHJcbiAgICAgICAgICAgIGdlblxyXG4gICAgICAgICAgICAgICAgKFwid2hpbGUoci5wb3M8YzIpe1wiKVxyXG4gICAgICAgICAgICAgICAgICAgIChcInZhciB0YWcyPXIudWludDMyKClcIilcclxuICAgICAgICAgICAgICAgICAgICAoXCJzd2l0Y2godGFnMj4+PjMpe1wiKVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAoXCJjYXNlIDE6IGs9ci4lcygpOyBicmVha1wiLCBmaWVsZC5rZXlUeXBlKVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAoXCJjYXNlIDI6XCIpO1xyXG5cclxuICAgICAgICAgICAgaWYgKHR5cGVzLmJhc2ljW3R5cGVdID09PSB1bmRlZmluZWQpIGdlblxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgKFwidmFsdWU9dHlwZXNbJWldLmRlY29kZShyLHIudWludDMyKCkpXCIsIGkpOyAvLyBjYW4ndCBiZSBncm91cHNcclxuICAgICAgICAgICAgZWxzZSBnZW5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIChcInZhbHVlPXIuJXMoKVwiLCB0eXBlKTtcclxuXHJcbiAgICAgICAgICAgIGdlblxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgKFwiYnJlYWtcIilcclxuICAgICAgICAgICAgICAgICAgICAgICAgKFwiZGVmYXVsdDpcIilcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIChcInIuc2tpcFR5cGUodGFnMiY3KVwiKVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgKFwiYnJlYWtcIilcclxuICAgICAgICAgICAgICAgICAgICAoXCJ9XCIpXHJcbiAgICAgICAgICAgICAgICAoXCJ9XCIpO1xyXG5cclxuICAgICAgICAgICAgaWYgKHR5cGVzLmxvbmdbZmllbGQua2V5VHlwZV0gIT09IHVuZGVmaW5lZCkgZ2VuXHJcbiAgICAgICAgICAgICAgICAoXCIlc1t0eXBlb2Ygaz09PVxcXCJvYmplY3RcXFwiP3V0aWwubG9uZ1RvSGFzaChrKTprXT12YWx1ZVwiLCByZWYpO1xyXG4gICAgICAgICAgICBlbHNlIGdlblxyXG4gICAgICAgICAgICAgICAgKFwiJXNba109dmFsdWVcIiwgcmVmKTtcclxuXHJcbiAgICAgICAgLy8gUmVwZWF0ZWQgZmllbGRzXHJcbiAgICAgICAgfSBlbHNlIGlmIChmaWVsZC5yZXBlYXRlZCkgeyBnZW5cclxuXHJcbiAgICAgICAgICAgICAgICAoXCJpZighKCVzJiYlcy5sZW5ndGgpKVwiLCByZWYsIHJlZilcclxuICAgICAgICAgICAgICAgICAgICAoXCIlcz1bXVwiLCByZWYpO1xyXG5cclxuICAgICAgICAgICAgLy8gUGFja2FibGUgKGFsd2F5cyBjaGVjayBmb3IgZm9yd2FyZCBhbmQgYmFja3dhcmQgY29tcGF0aWJsaXR5KVxyXG4gICAgICAgICAgICBpZiAodHlwZXMucGFja2VkW3R5cGVdICE9PSB1bmRlZmluZWQpIGdlblxyXG4gICAgICAgICAgICAgICAgKFwiaWYoKHQmNyk9PT0yKXtcIilcclxuICAgICAgICAgICAgICAgICAgICAoXCJ2YXIgYzI9ci51aW50MzIoKStyLnBvc1wiKVxyXG4gICAgICAgICAgICAgICAgICAgIChcIndoaWxlKHIucG9zPGMyKVwiKVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAoXCIlcy5wdXNoKHIuJXMoKSlcIiwgcmVmLCB0eXBlKVxyXG4gICAgICAgICAgICAgICAgKFwifWVsc2VcIik7XHJcblxyXG4gICAgICAgICAgICAvLyBOb24tcGFja2VkXHJcbiAgICAgICAgICAgIGlmICh0eXBlcy5iYXNpY1t0eXBlXSA9PT0gdW5kZWZpbmVkKSBnZW4oZmllbGQucmVzb2x2ZWRUeXBlLmdyb3VwXHJcbiAgICAgICAgICAgICAgICAgICAgPyBcIiVzLnB1c2godHlwZXNbJWldLmRlY29kZShyKSlcIlxyXG4gICAgICAgICAgICAgICAgICAgIDogXCIlcy5wdXNoKHR5cGVzWyVpXS5kZWNvZGUocixyLnVpbnQzMigpKSlcIiwgcmVmLCBpKTtcclxuICAgICAgICAgICAgZWxzZSBnZW5cclxuICAgICAgICAgICAgICAgICAgICAoXCIlcy5wdXNoKHIuJXMoKSlcIiwgcmVmLCB0eXBlKTtcclxuXHJcbiAgICAgICAgLy8gTm9uLXJlcGVhdGVkXHJcbiAgICAgICAgfSBlbHNlIGlmICh0eXBlcy5iYXNpY1t0eXBlXSA9PT0gdW5kZWZpbmVkKSBnZW4oZmllbGQucmVzb2x2ZWRUeXBlLmdyb3VwXHJcbiAgICAgICAgICAgICAgICA/IFwiJXM9dHlwZXNbJWldLmRlY29kZShyKVwiXHJcbiAgICAgICAgICAgICAgICA6IFwiJXM9dHlwZXNbJWldLmRlY29kZShyLHIudWludDMyKCkpXCIsIHJlZiwgaSk7XHJcbiAgICAgICAgZWxzZSBnZW5cclxuICAgICAgICAgICAgICAgIChcIiVzPXIuJXMoKVwiLCByZWYsIHR5cGUpO1xyXG4gICAgICAgIGdlblxyXG4gICAgICAgICAgICAgICAgKFwiYnJlYWtcIilcclxuICAgICAgICAgICAgKFwifVwiKTtcclxuICAgICAgICAvLyBVbmtub3duIGZpZWxkc1xyXG4gICAgfSBnZW5cclxuICAgICAgICAgICAgKFwiZGVmYXVsdDpcIilcclxuICAgICAgICAgICAgICAgIChcInIuc2tpcFR5cGUodCY3KVwiKVxyXG4gICAgICAgICAgICAgICAgKFwiYnJlYWtcIilcclxuXHJcbiAgICAgICAgKFwifVwiKVxyXG4gICAgKFwifVwiKTtcclxuXHJcbiAgICAvLyBGaWVsZCBwcmVzZW5jZVxyXG4gICAgZm9yIChpID0gMDsgaSA8IG10eXBlLl9maWVsZHNBcnJheS5sZW5ndGg7ICsraSkge1xyXG4gICAgICAgIHZhciByZmllbGQgPSBtdHlwZS5fZmllbGRzQXJyYXlbaV07XHJcbiAgICAgICAgaWYgKHJmaWVsZC5yZXF1aXJlZCkgZ2VuXHJcbiAgICAoXCJpZighbS5oYXNPd25Qcm9wZXJ0eSglaikpXCIsIHJmaWVsZC5uYW1lKVxyXG4gICAgICAgIChcInRocm93IHV0aWwuUHJvdG9jb2xFcnJvciglaix7aW5zdGFuY2U6bX0pXCIsIG1pc3NpbmcocmZpZWxkKSk7XHJcbiAgICB9XHJcblxyXG4gICAgcmV0dXJuIGdlblxyXG4gICAgKFwicmV0dXJuIG1cIik7XHJcbiAgICAvKiBlc2xpbnQtZW5hYmxlIG5vLXVuZXhwZWN0ZWQtbXVsdGlsaW5lICovXHJcbn1cclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbm1vZHVsZS5leHBvcnRzID0gZW5jb2RlcjtcclxuXHJcbnZhciBFbnVtICAgICA9IHJlcXVpcmUoXCIuL2VudW1cIiksXHJcbiAgICB0eXBlcyAgICA9IHJlcXVpcmUoXCIuL3R5cGVzXCIpLFxyXG4gICAgdXRpbCAgICAgPSByZXF1aXJlKFwiLi91dGlsXCIpO1xyXG5cclxuLyoqXHJcbiAqIEdlbmVyYXRlcyBhIHBhcnRpYWwgbWVzc2FnZSB0eXBlIGVuY29kZXIuXHJcbiAqIEBwYXJhbSB7Q29kZWdlbn0gZ2VuIENvZGVnZW4gaW5zdGFuY2VcclxuICogQHBhcmFtIHtGaWVsZH0gZmllbGQgUmVmbGVjdGVkIGZpZWxkXHJcbiAqIEBwYXJhbSB7bnVtYmVyfSBmaWVsZEluZGV4IEZpZWxkIGluZGV4XHJcbiAqIEBwYXJhbSB7c3RyaW5nfSByZWYgVmFyaWFibGUgcmVmZXJlbmNlXHJcbiAqIEByZXR1cm5zIHtDb2RlZ2VufSBDb2RlZ2VuIGluc3RhbmNlXHJcbiAqIEBpZ25vcmVcclxuICovXHJcbmZ1bmN0aW9uIGdlblR5cGVQYXJ0aWFsKGdlbiwgZmllbGQsIGZpZWxkSW5kZXgsIHJlZikge1xyXG4gICAgcmV0dXJuIGZpZWxkLnJlc29sdmVkVHlwZS5ncm91cFxyXG4gICAgICAgID8gZ2VuKFwidHlwZXNbJWldLmVuY29kZSglcyx3LnVpbnQzMiglaSkpLnVpbnQzMiglaSlcIiwgZmllbGRJbmRleCwgcmVmLCAoZmllbGQuaWQgPDwgMyB8IDMpID4+PiAwLCAoZmllbGQuaWQgPDwgMyB8IDQpID4+PiAwKVxyXG4gICAgICAgIDogZ2VuKFwidHlwZXNbJWldLmVuY29kZSglcyx3LnVpbnQzMiglaSkuZm9yaygpKS5sZGVsaW0oKVwiLCBmaWVsZEluZGV4LCByZWYsIChmaWVsZC5pZCA8PCAzIHwgMikgPj4+IDApO1xyXG59XHJcblxyXG4vKipcclxuICogR2VuZXJhdGVzIGFuIGVuY29kZXIgc3BlY2lmaWMgdG8gdGhlIHNwZWNpZmllZCBtZXNzYWdlIHR5cGUuXHJcbiAqIEBwYXJhbSB7VHlwZX0gbXR5cGUgTWVzc2FnZSB0eXBlXHJcbiAqIEByZXR1cm5zIHtDb2RlZ2VufSBDb2RlZ2VuIGluc3RhbmNlXHJcbiAqL1xyXG5mdW5jdGlvbiBlbmNvZGVyKG10eXBlKSB7XHJcbiAgICAvKiBlc2xpbnQtZGlzYWJsZSBuby11bmV4cGVjdGVkLW11bHRpbGluZSwgYmxvY2stc2NvcGVkLXZhciwgbm8tcmVkZWNsYXJlICovXHJcbiAgICB2YXIgZ2VuID0gdXRpbC5jb2RlZ2VuKFtcIm1cIiwgXCJ3XCJdLCBtdHlwZS5uYW1lICsgXCIkZW5jb2RlXCIpXHJcbiAgICAoXCJpZighdylcIilcclxuICAgICAgICAoXCJ3PVdyaXRlci5jcmVhdGUoKVwiKTtcclxuXHJcbiAgICB2YXIgaSwgcmVmO1xyXG5cclxuICAgIC8vIFwid2hlbiBhIG1lc3NhZ2UgaXMgc2VyaWFsaXplZCBpdHMga25vd24gZmllbGRzIHNob3VsZCBiZSB3cml0dGVuIHNlcXVlbnRpYWxseSBieSBmaWVsZCBudW1iZXJcIlxyXG4gICAgdmFyIGZpZWxkcyA9IC8qIGluaXRpYWxpemVzICovIG10eXBlLmZpZWxkc0FycmF5LnNsaWNlKCkuc29ydCh1dGlsLmNvbXBhcmVGaWVsZHNCeUlkKTtcclxuXHJcbiAgICBmb3IgKHZhciBpID0gMDsgaSA8IGZpZWxkcy5sZW5ndGg7ICsraSkge1xyXG4gICAgICAgIHZhciBmaWVsZCAgICA9IGZpZWxkc1tpXS5yZXNvbHZlKCksXHJcbiAgICAgICAgICAgIGluZGV4ICAgID0gbXR5cGUuX2ZpZWxkc0FycmF5LmluZGV4T2YoZmllbGQpLFxyXG4gICAgICAgICAgICB0eXBlICAgICA9IGZpZWxkLnJlc29sdmVkVHlwZSBpbnN0YW5jZW9mIEVudW0gPyBcImludDMyXCIgOiBmaWVsZC50eXBlLFxyXG4gICAgICAgICAgICB3aXJlVHlwZSA9IHR5cGVzLmJhc2ljW3R5cGVdO1xyXG4gICAgICAgICAgICByZWYgICAgICA9IFwibVwiICsgdXRpbC5zYWZlUHJvcChmaWVsZC5uYW1lKTtcclxuXHJcbiAgICAgICAgLy8gTWFwIGZpZWxkc1xyXG4gICAgICAgIGlmIChmaWVsZC5tYXApIHtcclxuICAgICAgICAgICAgZ2VuXHJcbiAgICAoXCJpZiglcyE9bnVsbCYmT2JqZWN0Lmhhc093blByb3BlcnR5LmNhbGwobSwlaikpe1wiLCByZWYsIGZpZWxkLm5hbWUpIC8vICE9PSB1bmRlZmluZWQgJiYgIT09IG51bGxcclxuICAgICAgICAoXCJmb3IodmFyIGtzPU9iamVjdC5rZXlzKCVzKSxpPTA7aTxrcy5sZW5ndGg7KytpKXtcIiwgcmVmKVxyXG4gICAgICAgICAgICAoXCJ3LnVpbnQzMiglaSkuZm9yaygpLnVpbnQzMiglaSkuJXMoa3NbaV0pXCIsIChmaWVsZC5pZCA8PCAzIHwgMikgPj4+IDAsIDggfCB0eXBlcy5tYXBLZXlbZmllbGQua2V5VHlwZV0sIGZpZWxkLmtleVR5cGUpO1xyXG4gICAgICAgICAgICBpZiAod2lyZVR5cGUgPT09IHVuZGVmaW5lZCkgZ2VuXHJcbiAgICAgICAgICAgIChcInR5cGVzWyVpXS5lbmNvZGUoJXNba3NbaV1dLHcudWludDMyKDE4KS5mb3JrKCkpLmxkZWxpbSgpLmxkZWxpbSgpXCIsIGluZGV4LCByZWYpOyAvLyBjYW4ndCBiZSBncm91cHNcclxuICAgICAgICAgICAgZWxzZSBnZW5cclxuICAgICAgICAgICAgKFwiLnVpbnQzMiglaSkuJXMoJXNba3NbaV1dKS5sZGVsaW0oKVwiLCAxNiB8IHdpcmVUeXBlLCB0eXBlLCByZWYpO1xyXG4gICAgICAgICAgICBnZW5cclxuICAgICAgICAoXCJ9XCIpXHJcbiAgICAoXCJ9XCIpO1xyXG5cclxuICAgICAgICAgICAgLy8gUmVwZWF0ZWQgZmllbGRzXHJcbiAgICAgICAgfSBlbHNlIGlmIChmaWVsZC5yZXBlYXRlZCkgeyBnZW5cclxuICAgIChcImlmKCVzIT1udWxsJiYlcy5sZW5ndGgpe1wiLCByZWYsIHJlZik7IC8vICE9PSB1bmRlZmluZWQgJiYgIT09IG51bGxcclxuXHJcbiAgICAgICAgICAgIC8vIFBhY2tlZCByZXBlYXRlZFxyXG4gICAgICAgICAgICBpZiAoZmllbGQucGFja2VkICYmIHR5cGVzLnBhY2tlZFt0eXBlXSAhPT0gdW5kZWZpbmVkKSB7IGdlblxyXG5cclxuICAgICAgICAoXCJ3LnVpbnQzMiglaSkuZm9yaygpXCIsIChmaWVsZC5pZCA8PCAzIHwgMikgPj4+IDApXHJcbiAgICAgICAgKFwiZm9yKHZhciBpPTA7aTwlcy5sZW5ndGg7KytpKVwiLCByZWYpXHJcbiAgICAgICAgICAgIChcIncuJXMoJXNbaV0pXCIsIHR5cGUsIHJlZilcclxuICAgICAgICAoXCJ3LmxkZWxpbSgpXCIpO1xyXG5cclxuICAgICAgICAgICAgLy8gTm9uLXBhY2tlZFxyXG4gICAgICAgICAgICB9IGVsc2UgeyBnZW5cclxuXHJcbiAgICAgICAgKFwiZm9yKHZhciBpPTA7aTwlcy5sZW5ndGg7KytpKVwiLCByZWYpO1xyXG4gICAgICAgICAgICAgICAgaWYgKHdpcmVUeXBlID09PSB1bmRlZmluZWQpXHJcbiAgICAgICAgICAgIGdlblR5cGVQYXJ0aWFsKGdlbiwgZmllbGQsIGluZGV4LCByZWYgKyBcIltpXVwiKTtcclxuICAgICAgICAgICAgICAgIGVsc2UgZ2VuXHJcbiAgICAgICAgICAgIChcIncudWludDMyKCVpKS4lcyglc1tpXSlcIiwgKGZpZWxkLmlkIDw8IDMgfCB3aXJlVHlwZSkgPj4+IDAsIHR5cGUsIHJlZik7XHJcblxyXG4gICAgICAgICAgICB9IGdlblxyXG4gICAgKFwifVwiKTtcclxuXHJcbiAgICAgICAgLy8gTm9uLXJlcGVhdGVkXHJcbiAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgaWYgKGZpZWxkLm9wdGlvbmFsKSBnZW5cclxuICAgIChcImlmKCVzIT1udWxsJiZPYmplY3QuaGFzT3duUHJvcGVydHkuY2FsbChtLCVqKSlcIiwgcmVmLCBmaWVsZC5uYW1lKTsgLy8gIT09IHVuZGVmaW5lZCAmJiAhPT0gbnVsbFxyXG5cclxuICAgICAgICAgICAgaWYgKHdpcmVUeXBlID09PSB1bmRlZmluZWQpXHJcbiAgICAgICAgZ2VuVHlwZVBhcnRpYWwoZ2VuLCBmaWVsZCwgaW5kZXgsIHJlZik7XHJcbiAgICAgICAgICAgIGVsc2UgZ2VuXHJcbiAgICAgICAgKFwidy51aW50MzIoJWkpLiVzKCVzKVwiLCAoZmllbGQuaWQgPDwgMyB8IHdpcmVUeXBlKSA+Pj4gMCwgdHlwZSwgcmVmKTtcclxuXHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiBnZW5cclxuICAgIChcInJldHVybiB3XCIpO1xyXG4gICAgLyogZXNsaW50LWVuYWJsZSBuby11bmV4cGVjdGVkLW11bHRpbGluZSwgYmxvY2stc2NvcGVkLXZhciwgbm8tcmVkZWNsYXJlICovXHJcbn1cclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbm1vZHVsZS5leHBvcnRzID0gRW51bTtcclxuXHJcbi8vIGV4dGVuZHMgUmVmbGVjdGlvbk9iamVjdFxyXG52YXIgUmVmbGVjdGlvbk9iamVjdCA9IHJlcXVpcmUoXCIuL29iamVjdFwiKTtcclxuKChFbnVtLnByb3RvdHlwZSA9IE9iamVjdC5jcmVhdGUoUmVmbGVjdGlvbk9iamVjdC5wcm90b3R5cGUpKS5jb25zdHJ1Y3RvciA9IEVudW0pLmNsYXNzTmFtZSA9IFwiRW51bVwiO1xyXG5cclxudmFyIE5hbWVzcGFjZSA9IHJlcXVpcmUoXCIuL25hbWVzcGFjZVwiKSxcclxuICAgIHV0aWwgPSByZXF1aXJlKFwiLi91dGlsXCIpO1xyXG5cclxuLyoqXHJcbiAqIENvbnN0cnVjdHMgYSBuZXcgZW51bSBpbnN0YW5jZS5cclxuICogQGNsYXNzZGVzYyBSZWZsZWN0ZWQgZW51bS5cclxuICogQGV4dGVuZHMgUmVmbGVjdGlvbk9iamVjdFxyXG4gKiBAY29uc3RydWN0b3JcclxuICogQHBhcmFtIHtzdHJpbmd9IG5hbWUgVW5pcXVlIG5hbWUgd2l0aGluIGl0cyBuYW1lc3BhY2VcclxuICogQHBhcmFtIHtPYmplY3QuPHN0cmluZyxudW1iZXI+fSBbdmFsdWVzXSBFbnVtIHZhbHVlcyBhcyBhbiBvYmplY3QsIGJ5IG5hbWVcclxuICogQHBhcmFtIHtPYmplY3QuPHN0cmluZywqPn0gW29wdGlvbnNdIERlY2xhcmVkIG9wdGlvbnNcclxuICogQHBhcmFtIHtzdHJpbmd9IFtjb21tZW50XSBUaGUgY29tbWVudCBmb3IgdGhpcyBlbnVtXHJcbiAqIEBwYXJhbSB7T2JqZWN0LjxzdHJpbmcsc3RyaW5nPn0gW2NvbW1lbnRzXSBUaGUgdmFsdWUgY29tbWVudHMgZm9yIHRoaXMgZW51bVxyXG4gKiBAcGFyYW0ge09iamVjdC48c3RyaW5nLE9iamVjdDxzdHJpbmcsKj4+fHVuZGVmaW5lZH0gW3ZhbHVlc09wdGlvbnNdIFRoZSB2YWx1ZSBvcHRpb25zIGZvciB0aGlzIGVudW1cclxuICovXHJcbmZ1bmN0aW9uIEVudW0obmFtZSwgdmFsdWVzLCBvcHRpb25zLCBjb21tZW50LCBjb21tZW50cywgdmFsdWVzT3B0aW9ucykge1xyXG4gICAgUmVmbGVjdGlvbk9iamVjdC5jYWxsKHRoaXMsIG5hbWUsIG9wdGlvbnMpO1xyXG5cclxuICAgIGlmICh2YWx1ZXMgJiYgdHlwZW9mIHZhbHVlcyAhPT0gXCJvYmplY3RcIilcclxuICAgICAgICB0aHJvdyBUeXBlRXJyb3IoXCJ2YWx1ZXMgbXVzdCBiZSBhbiBvYmplY3RcIik7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBFbnVtIHZhbHVlcyBieSBpZC5cclxuICAgICAqIEB0eXBlIHtPYmplY3QuPG51bWJlcixzdHJpbmc+fVxyXG4gICAgICovXHJcbiAgICB0aGlzLnZhbHVlc0J5SWQgPSB7fTtcclxuXHJcbiAgICAvKipcclxuICAgICAqIEVudW0gdmFsdWVzIGJ5IG5hbWUuXHJcbiAgICAgKiBAdHlwZSB7T2JqZWN0LjxzdHJpbmcsbnVtYmVyPn1cclxuICAgICAqL1xyXG4gICAgdGhpcy52YWx1ZXMgPSBPYmplY3QuY3JlYXRlKHRoaXMudmFsdWVzQnlJZCk7IC8vIHRvSlNPTiwgbWFya2VyXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBFbnVtIGNvbW1lbnQgdGV4dC5cclxuICAgICAqIEB0eXBlIHtzdHJpbmd8bnVsbH1cclxuICAgICAqL1xyXG4gICAgdGhpcy5jb21tZW50ID0gY29tbWVudDtcclxuXHJcbiAgICAvKipcclxuICAgICAqIFZhbHVlIGNvbW1lbnQgdGV4dHMsIGlmIGFueS5cclxuICAgICAqIEB0eXBlIHtPYmplY3QuPHN0cmluZyxzdHJpbmc+fVxyXG4gICAgICovXHJcbiAgICB0aGlzLmNvbW1lbnRzID0gY29tbWVudHMgfHwge307XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBWYWx1ZXMgb3B0aW9ucywgaWYgYW55XHJcbiAgICAgKiBAdHlwZSB7T2JqZWN0PHN0cmluZywgT2JqZWN0PHN0cmluZywgKj4+fHVuZGVmaW5lZH1cclxuICAgICAqL1xyXG4gICAgdGhpcy52YWx1ZXNPcHRpb25zID0gdmFsdWVzT3B0aW9ucztcclxuXHJcbiAgICAvKipcclxuICAgICAqIFJlc2VydmVkIHJhbmdlcywgaWYgYW55LlxyXG4gICAgICogQHR5cGUge0FycmF5LjxudW1iZXJbXXxzdHJpbmc+fVxyXG4gICAgICovXHJcbiAgICB0aGlzLnJlc2VydmVkID0gdW5kZWZpbmVkOyAvLyB0b0pTT05cclxuXHJcbiAgICAvLyBOb3RlIHRoYXQgdmFsdWVzIGluaGVyaXQgdmFsdWVzQnlJZCBvbiB0aGVpciBwcm90b3R5cGUgd2hpY2ggbWFrZXMgdGhlbSBhIFR5cGVTY3JpcHQtXHJcbiAgICAvLyBjb21wYXRpYmxlIGVudW0uIFRoaXMgaXMgdXNlZCBieSBwYnRzIHRvIHdyaXRlIGFjdHVhbCBlbnVtIGRlZmluaXRpb25zIHRoYXQgd29yayBmb3JcclxuICAgIC8vIHN0YXRpYyBhbmQgcmVmbGVjdGlvbiBjb2RlIGFsaWtlIGluc3RlYWQgb2YgZW1pdHRpbmcgZ2VuZXJpYyBvYmplY3QgZGVmaW5pdGlvbnMuXHJcblxyXG4gICAgaWYgKHZhbHVlcylcclxuICAgICAgICBmb3IgKHZhciBrZXlzID0gT2JqZWN0LmtleXModmFsdWVzKSwgaSA9IDA7IGkgPCBrZXlzLmxlbmd0aDsgKytpKVxyXG4gICAgICAgICAgICBpZiAodHlwZW9mIHZhbHVlc1trZXlzW2ldXSA9PT0gXCJudW1iZXJcIikgLy8gdXNlIGZvcndhcmQgZW50cmllcyBvbmx5XHJcbiAgICAgICAgICAgICAgICB0aGlzLnZhbHVlc0J5SWRbIHRoaXMudmFsdWVzW2tleXNbaV1dID0gdmFsdWVzW2tleXNbaV1dIF0gPSBrZXlzW2ldO1xyXG59XHJcblxyXG4vKipcclxuICogRW51bSBkZXNjcmlwdG9yLlxyXG4gKiBAaW50ZXJmYWNlIElFbnVtXHJcbiAqIEBwcm9wZXJ0eSB7T2JqZWN0LjxzdHJpbmcsbnVtYmVyPn0gdmFsdWVzIEVudW0gdmFsdWVzXHJcbiAqIEBwcm9wZXJ0eSB7T2JqZWN0LjxzdHJpbmcsKj59IFtvcHRpb25zXSBFbnVtIG9wdGlvbnNcclxuICovXHJcblxyXG4vKipcclxuICogQ29uc3RydWN0cyBhbiBlbnVtIGZyb20gYW4gZW51bSBkZXNjcmlwdG9yLlxyXG4gKiBAcGFyYW0ge3N0cmluZ30gbmFtZSBFbnVtIG5hbWVcclxuICogQHBhcmFtIHtJRW51bX0ganNvbiBFbnVtIGRlc2NyaXB0b3JcclxuICogQHJldHVybnMge0VudW19IENyZWF0ZWQgZW51bVxyXG4gKiBAdGhyb3dzIHtUeXBlRXJyb3J9IElmIGFyZ3VtZW50cyBhcmUgaW52YWxpZFxyXG4gKi9cclxuRW51bS5mcm9tSlNPTiA9IGZ1bmN0aW9uIGZyb21KU09OKG5hbWUsIGpzb24pIHtcclxuICAgIHZhciBlbm0gPSBuZXcgRW51bShuYW1lLCBqc29uLnZhbHVlcywganNvbi5vcHRpb25zLCBqc29uLmNvbW1lbnQsIGpzb24uY29tbWVudHMpO1xyXG4gICAgZW5tLnJlc2VydmVkID0ganNvbi5yZXNlcnZlZDtcclxuICAgIHJldHVybiBlbm07XHJcbn07XHJcblxyXG4vKipcclxuICogQ29udmVydHMgdGhpcyBlbnVtIHRvIGFuIGVudW0gZGVzY3JpcHRvci5cclxuICogQHBhcmFtIHtJVG9KU09OT3B0aW9uc30gW3RvSlNPTk9wdGlvbnNdIEpTT04gY29udmVyc2lvbiBvcHRpb25zXHJcbiAqIEByZXR1cm5zIHtJRW51bX0gRW51bSBkZXNjcmlwdG9yXHJcbiAqL1xyXG5FbnVtLnByb3RvdHlwZS50b0pTT04gPSBmdW5jdGlvbiB0b0pTT04odG9KU09OT3B0aW9ucykge1xyXG4gICAgdmFyIGtlZXBDb21tZW50cyA9IHRvSlNPTk9wdGlvbnMgPyBCb29sZWFuKHRvSlNPTk9wdGlvbnMua2VlcENvbW1lbnRzKSA6IGZhbHNlO1xyXG4gICAgcmV0dXJuIHV0aWwudG9PYmplY3QoW1xyXG4gICAgICAgIFwib3B0aW9uc1wiICAgICAgICwgdGhpcy5vcHRpb25zLFxyXG4gICAgICAgIFwidmFsdWVzT3B0aW9uc1wiICwgdGhpcy52YWx1ZXNPcHRpb25zLFxyXG4gICAgICAgIFwidmFsdWVzXCIgICAgICAgICwgdGhpcy52YWx1ZXMsXHJcbiAgICAgICAgXCJyZXNlcnZlZFwiICAgICAgLCB0aGlzLnJlc2VydmVkICYmIHRoaXMucmVzZXJ2ZWQubGVuZ3RoID8gdGhpcy5yZXNlcnZlZCA6IHVuZGVmaW5lZCxcclxuICAgICAgICBcImNvbW1lbnRcIiAgICAgICAsIGtlZXBDb21tZW50cyA/IHRoaXMuY29tbWVudCA6IHVuZGVmaW5lZCxcclxuICAgICAgICBcImNvbW1lbnRzXCIgICAgICAsIGtlZXBDb21tZW50cyA/IHRoaXMuY29tbWVudHMgOiB1bmRlZmluZWRcclxuICAgIF0pO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIEFkZHMgYSB2YWx1ZSB0byB0aGlzIGVudW0uXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBuYW1lIFZhbHVlIG5hbWVcclxuICogQHBhcmFtIHtudW1iZXJ9IGlkIFZhbHVlIGlkXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBbY29tbWVudF0gQ29tbWVudCwgaWYgYW55XHJcbiAqIEBwYXJhbSB7T2JqZWN0LjxzdHJpbmcsICo+fHVuZGVmaW5lZH0gW29wdGlvbnNdIE9wdGlvbnMsIGlmIGFueVxyXG4gKiBAcmV0dXJucyB7RW51bX0gYHRoaXNgXHJcbiAqIEB0aHJvd3Mge1R5cGVFcnJvcn0gSWYgYXJndW1lbnRzIGFyZSBpbnZhbGlkXHJcbiAqIEB0aHJvd3Mge0Vycm9yfSBJZiB0aGVyZSBpcyBhbHJlYWR5IGEgdmFsdWUgd2l0aCB0aGlzIG5hbWUgb3IgaWRcclxuICovXHJcbkVudW0ucHJvdG90eXBlLmFkZCA9IGZ1bmN0aW9uIGFkZChuYW1lLCBpZCwgY29tbWVudCwgb3B0aW9ucykge1xyXG4gICAgLy8gdXRpbGl6ZWQgYnkgdGhlIHBhcnNlciBidXQgbm90IGJ5IC5mcm9tSlNPTlxyXG5cclxuICAgIGlmICghdXRpbC5pc1N0cmluZyhuYW1lKSlcclxuICAgICAgICB0aHJvdyBUeXBlRXJyb3IoXCJuYW1lIG11c3QgYmUgYSBzdHJpbmdcIik7XHJcblxyXG4gICAgaWYgKCF1dGlsLmlzSW50ZWdlcihpZCkpXHJcbiAgICAgICAgdGhyb3cgVHlwZUVycm9yKFwiaWQgbXVzdCBiZSBhbiBpbnRlZ2VyXCIpO1xyXG5cclxuICAgIGlmICh0aGlzLnZhbHVlc1tuYW1lXSAhPT0gdW5kZWZpbmVkKVxyXG4gICAgICAgIHRocm93IEVycm9yKFwiZHVwbGljYXRlIG5hbWUgJ1wiICsgbmFtZSArIFwiJyBpbiBcIiArIHRoaXMpO1xyXG5cclxuICAgIGlmICh0aGlzLmlzUmVzZXJ2ZWRJZChpZCkpXHJcbiAgICAgICAgdGhyb3cgRXJyb3IoXCJpZCBcIiArIGlkICsgXCIgaXMgcmVzZXJ2ZWQgaW4gXCIgKyB0aGlzKTtcclxuXHJcbiAgICBpZiAodGhpcy5pc1Jlc2VydmVkTmFtZShuYW1lKSlcclxuICAgICAgICB0aHJvdyBFcnJvcihcIm5hbWUgJ1wiICsgbmFtZSArIFwiJyBpcyByZXNlcnZlZCBpbiBcIiArIHRoaXMpO1xyXG5cclxuICAgIGlmICh0aGlzLnZhbHVlc0J5SWRbaWRdICE9PSB1bmRlZmluZWQpIHtcclxuICAgICAgICBpZiAoISh0aGlzLm9wdGlvbnMgJiYgdGhpcy5vcHRpb25zLmFsbG93X2FsaWFzKSlcclxuICAgICAgICAgICAgdGhyb3cgRXJyb3IoXCJkdXBsaWNhdGUgaWQgXCIgKyBpZCArIFwiIGluIFwiICsgdGhpcyk7XHJcbiAgICAgICAgdGhpcy52YWx1ZXNbbmFtZV0gPSBpZDtcclxuICAgIH0gZWxzZVxyXG4gICAgICAgIHRoaXMudmFsdWVzQnlJZFt0aGlzLnZhbHVlc1tuYW1lXSA9IGlkXSA9IG5hbWU7XHJcblxyXG4gICAgaWYgKG9wdGlvbnMpIHtcclxuICAgICAgICBpZiAodGhpcy52YWx1ZXNPcHRpb25zID09PSB1bmRlZmluZWQpXHJcbiAgICAgICAgICAgIHRoaXMudmFsdWVzT3B0aW9ucyA9IHt9O1xyXG4gICAgICAgIHRoaXMudmFsdWVzT3B0aW9uc1tuYW1lXSA9IG9wdGlvbnMgfHwgbnVsbDtcclxuICAgIH1cclxuXHJcbiAgICB0aGlzLmNvbW1lbnRzW25hbWVdID0gY29tbWVudCB8fCBudWxsO1xyXG4gICAgcmV0dXJuIHRoaXM7XHJcbn07XHJcblxyXG4vKipcclxuICogUmVtb3ZlcyBhIHZhbHVlIGZyb20gdGhpcyBlbnVtXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBuYW1lIFZhbHVlIG5hbWVcclxuICogQHJldHVybnMge0VudW19IGB0aGlzYFxyXG4gKiBAdGhyb3dzIHtUeXBlRXJyb3J9IElmIGFyZ3VtZW50cyBhcmUgaW52YWxpZFxyXG4gKiBAdGhyb3dzIHtFcnJvcn0gSWYgYG5hbWVgIGlzIG5vdCBhIG5hbWUgb2YgdGhpcyBlbnVtXHJcbiAqL1xyXG5FbnVtLnByb3RvdHlwZS5yZW1vdmUgPSBmdW5jdGlvbiByZW1vdmUobmFtZSkge1xyXG5cclxuICAgIGlmICghdXRpbC5pc1N0cmluZyhuYW1lKSlcclxuICAgICAgICB0aHJvdyBUeXBlRXJyb3IoXCJuYW1lIG11c3QgYmUgYSBzdHJpbmdcIik7XHJcblxyXG4gICAgdmFyIHZhbCA9IHRoaXMudmFsdWVzW25hbWVdO1xyXG4gICAgaWYgKHZhbCA9PSBudWxsKVxyXG4gICAgICAgIHRocm93IEVycm9yKFwibmFtZSAnXCIgKyBuYW1lICsgXCInIGRvZXMgbm90IGV4aXN0IGluIFwiICsgdGhpcyk7XHJcblxyXG4gICAgZGVsZXRlIHRoaXMudmFsdWVzQnlJZFt2YWxdO1xyXG4gICAgZGVsZXRlIHRoaXMudmFsdWVzW25hbWVdO1xyXG4gICAgZGVsZXRlIHRoaXMuY29tbWVudHNbbmFtZV07XHJcbiAgICBpZiAodGhpcy52YWx1ZXNPcHRpb25zKVxyXG4gICAgICAgIGRlbGV0ZSB0aGlzLnZhbHVlc09wdGlvbnNbbmFtZV07XHJcblxyXG4gICAgcmV0dXJuIHRoaXM7XHJcbn07XHJcblxyXG4vKipcclxuICogVGVzdHMgaWYgdGhlIHNwZWNpZmllZCBpZCBpcyByZXNlcnZlZC5cclxuICogQHBhcmFtIHtudW1iZXJ9IGlkIElkIHRvIHRlc3RcclxuICogQHJldHVybnMge2Jvb2xlYW59IGB0cnVlYCBpZiByZXNlcnZlZCwgb3RoZXJ3aXNlIGBmYWxzZWBcclxuICovXHJcbkVudW0ucHJvdG90eXBlLmlzUmVzZXJ2ZWRJZCA9IGZ1bmN0aW9uIGlzUmVzZXJ2ZWRJZChpZCkge1xyXG4gICAgcmV0dXJuIE5hbWVzcGFjZS5pc1Jlc2VydmVkSWQodGhpcy5yZXNlcnZlZCwgaWQpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIFRlc3RzIGlmIHRoZSBzcGVjaWZpZWQgbmFtZSBpcyByZXNlcnZlZC5cclxuICogQHBhcmFtIHtzdHJpbmd9IG5hbWUgTmFtZSB0byB0ZXN0XHJcbiAqIEByZXR1cm5zIHtib29sZWFufSBgdHJ1ZWAgaWYgcmVzZXJ2ZWQsIG90aGVyd2lzZSBgZmFsc2VgXHJcbiAqL1xyXG5FbnVtLnByb3RvdHlwZS5pc1Jlc2VydmVkTmFtZSA9IGZ1bmN0aW9uIGlzUmVzZXJ2ZWROYW1lKG5hbWUpIHtcclxuICAgIHJldHVybiBOYW1lc3BhY2UuaXNSZXNlcnZlZE5hbWUodGhpcy5yZXNlcnZlZCwgbmFtZSk7XHJcbn07XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG5tb2R1bGUuZXhwb3J0cyA9IEZpZWxkO1xyXG5cclxuLy8gZXh0ZW5kcyBSZWZsZWN0aW9uT2JqZWN0XHJcbnZhciBSZWZsZWN0aW9uT2JqZWN0ID0gcmVxdWlyZShcIi4vb2JqZWN0XCIpO1xyXG4oKEZpZWxkLnByb3RvdHlwZSA9IE9iamVjdC5jcmVhdGUoUmVmbGVjdGlvbk9iamVjdC5wcm90b3R5cGUpKS5jb25zdHJ1Y3RvciA9IEZpZWxkKS5jbGFzc05hbWUgPSBcIkZpZWxkXCI7XHJcblxyXG52YXIgRW51bSAgPSByZXF1aXJlKFwiLi9lbnVtXCIpLFxyXG4gICAgdHlwZXMgPSByZXF1aXJlKFwiLi90eXBlc1wiKSxcclxuICAgIHV0aWwgID0gcmVxdWlyZShcIi4vdXRpbFwiKTtcclxuXHJcbnZhciBUeXBlOyAvLyBjeWNsaWNcclxuXHJcbnZhciBydWxlUmUgPSAvXnJlcXVpcmVkfG9wdGlvbmFsfHJlcGVhdGVkJC87XHJcblxyXG4vKipcclxuICogQ29uc3RydWN0cyBhIG5ldyBtZXNzYWdlIGZpZWxkIGluc3RhbmNlLiBOb3RlIHRoYXQge0BsaW5rIE1hcEZpZWxkfG1hcCBmaWVsZHN9IGhhdmUgdGhlaXIgb3duIGNsYXNzLlxyXG4gKiBAbmFtZSBGaWVsZFxyXG4gKiBAY2xhc3NkZXNjIFJlZmxlY3RlZCBtZXNzYWdlIGZpZWxkLlxyXG4gKiBAZXh0ZW5kcyBGaWVsZEJhc2VcclxuICogQGNvbnN0cnVjdG9yXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBuYW1lIFVuaXF1ZSBuYW1lIHdpdGhpbiBpdHMgbmFtZXNwYWNlXHJcbiAqIEBwYXJhbSB7bnVtYmVyfSBpZCBVbmlxdWUgaWQgd2l0aGluIGl0cyBuYW1lc3BhY2VcclxuICogQHBhcmFtIHtzdHJpbmd9IHR5cGUgVmFsdWUgdHlwZVxyXG4gKiBAcGFyYW0ge3N0cmluZ3xPYmplY3QuPHN0cmluZywqPn0gW3J1bGU9XCJvcHRpb25hbFwiXSBGaWVsZCBydWxlXHJcbiAqIEBwYXJhbSB7c3RyaW5nfE9iamVjdC48c3RyaW5nLCo+fSBbZXh0ZW5kXSBFeHRlbmRlZCB0eXBlIGlmIGRpZmZlcmVudCBmcm9tIHBhcmVudFxyXG4gKiBAcGFyYW0ge09iamVjdC48c3RyaW5nLCo+fSBbb3B0aW9uc10gRGVjbGFyZWQgb3B0aW9uc1xyXG4gKi9cclxuXHJcbi8qKlxyXG4gKiBDb25zdHJ1Y3RzIGEgZmllbGQgZnJvbSBhIGZpZWxkIGRlc2NyaXB0b3IuXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBuYW1lIEZpZWxkIG5hbWVcclxuICogQHBhcmFtIHtJRmllbGR9IGpzb24gRmllbGQgZGVzY3JpcHRvclxyXG4gKiBAcmV0dXJucyB7RmllbGR9IENyZWF0ZWQgZmllbGRcclxuICogQHRocm93cyB7VHlwZUVycm9yfSBJZiBhcmd1bWVudHMgYXJlIGludmFsaWRcclxuICovXHJcbkZpZWxkLmZyb21KU09OID0gZnVuY3Rpb24gZnJvbUpTT04obmFtZSwganNvbikge1xyXG4gICAgcmV0dXJuIG5ldyBGaWVsZChuYW1lLCBqc29uLmlkLCBqc29uLnR5cGUsIGpzb24ucnVsZSwganNvbi5leHRlbmQsIGpzb24ub3B0aW9ucywganNvbi5jb21tZW50KTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBOb3QgYW4gYWN0dWFsIGNvbnN0cnVjdG9yLiBVc2Uge0BsaW5rIEZpZWxkfSBpbnN0ZWFkLlxyXG4gKiBAY2xhc3NkZXNjIEJhc2UgY2xhc3Mgb2YgYWxsIHJlZmxlY3RlZCBtZXNzYWdlIGZpZWxkcy4gVGhpcyBpcyBub3QgYW4gYWN0dWFsIGNsYXNzIGJ1dCBoZXJlIGZvciB0aGUgc2FrZSBvZiBoYXZpbmcgY29uc2lzdGVudCB0eXBlIGRlZmluaXRpb25zLlxyXG4gKiBAZXhwb3J0cyBGaWVsZEJhc2VcclxuICogQGV4dGVuZHMgUmVmbGVjdGlvbk9iamVjdFxyXG4gKiBAY29uc3RydWN0b3JcclxuICogQHBhcmFtIHtzdHJpbmd9IG5hbWUgVW5pcXVlIG5hbWUgd2l0aGluIGl0cyBuYW1lc3BhY2VcclxuICogQHBhcmFtIHtudW1iZXJ9IGlkIFVuaXF1ZSBpZCB3aXRoaW4gaXRzIG5hbWVzcGFjZVxyXG4gKiBAcGFyYW0ge3N0cmluZ30gdHlwZSBWYWx1ZSB0eXBlXHJcbiAqIEBwYXJhbSB7c3RyaW5nfE9iamVjdC48c3RyaW5nLCo+fSBbcnVsZT1cIm9wdGlvbmFsXCJdIEZpZWxkIHJ1bGVcclxuICogQHBhcmFtIHtzdHJpbmd8T2JqZWN0LjxzdHJpbmcsKj59IFtleHRlbmRdIEV4dGVuZGVkIHR5cGUgaWYgZGlmZmVyZW50IGZyb20gcGFyZW50XHJcbiAqIEBwYXJhbSB7T2JqZWN0LjxzdHJpbmcsKj59IFtvcHRpb25zXSBEZWNsYXJlZCBvcHRpb25zXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBbY29tbWVudF0gQ29tbWVudCBhc3NvY2lhdGVkIHdpdGggdGhpcyBmaWVsZFxyXG4gKi9cclxuZnVuY3Rpb24gRmllbGQobmFtZSwgaWQsIHR5cGUsIHJ1bGUsIGV4dGVuZCwgb3B0aW9ucywgY29tbWVudCkge1xyXG5cclxuICAgIGlmICh1dGlsLmlzT2JqZWN0KHJ1bGUpKSB7XHJcbiAgICAgICAgY29tbWVudCA9IGV4dGVuZDtcclxuICAgICAgICBvcHRpb25zID0gcnVsZTtcclxuICAgICAgICBydWxlID0gZXh0ZW5kID0gdW5kZWZpbmVkO1xyXG4gICAgfSBlbHNlIGlmICh1dGlsLmlzT2JqZWN0KGV4dGVuZCkpIHtcclxuICAgICAgICBjb21tZW50ID0gb3B0aW9ucztcclxuICAgICAgICBvcHRpb25zID0gZXh0ZW5kO1xyXG4gICAgICAgIGV4dGVuZCA9IHVuZGVmaW5lZDtcclxuICAgIH1cclxuXHJcbiAgICBSZWZsZWN0aW9uT2JqZWN0LmNhbGwodGhpcywgbmFtZSwgb3B0aW9ucyk7XHJcblxyXG4gICAgaWYgKCF1dGlsLmlzSW50ZWdlcihpZCkgfHwgaWQgPCAwKVxyXG4gICAgICAgIHRocm93IFR5cGVFcnJvcihcImlkIG11c3QgYmUgYSBub24tbmVnYXRpdmUgaW50ZWdlclwiKTtcclxuXHJcbiAgICBpZiAoIXV0aWwuaXNTdHJpbmcodHlwZSkpXHJcbiAgICAgICAgdGhyb3cgVHlwZUVycm9yKFwidHlwZSBtdXN0IGJlIGEgc3RyaW5nXCIpO1xyXG5cclxuICAgIGlmIChydWxlICE9PSB1bmRlZmluZWQgJiYgIXJ1bGVSZS50ZXN0KHJ1bGUgPSBydWxlLnRvU3RyaW5nKCkudG9Mb3dlckNhc2UoKSkpXHJcbiAgICAgICAgdGhyb3cgVHlwZUVycm9yKFwicnVsZSBtdXN0IGJlIGEgc3RyaW5nIHJ1bGVcIik7XHJcblxyXG4gICAgaWYgKGV4dGVuZCAhPT0gdW5kZWZpbmVkICYmICF1dGlsLmlzU3RyaW5nKGV4dGVuZCkpXHJcbiAgICAgICAgdGhyb3cgVHlwZUVycm9yKFwiZXh0ZW5kIG11c3QgYmUgYSBzdHJpbmdcIik7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBGaWVsZCBydWxlLCBpZiBhbnkuXHJcbiAgICAgKiBAdHlwZSB7c3RyaW5nfHVuZGVmaW5lZH1cclxuICAgICAqL1xyXG4gICAgaWYgKHJ1bGUgPT09IFwicHJvdG8zX29wdGlvbmFsXCIpIHtcclxuICAgICAgICBydWxlID0gXCJvcHRpb25hbFwiO1xyXG4gICAgfVxyXG4gICAgdGhpcy5ydWxlID0gcnVsZSAmJiBydWxlICE9PSBcIm9wdGlvbmFsXCIgPyBydWxlIDogdW5kZWZpbmVkOyAvLyB0b0pTT05cclxuXHJcbiAgICAvKipcclxuICAgICAqIEZpZWxkIHR5cGUuXHJcbiAgICAgKiBAdHlwZSB7c3RyaW5nfVxyXG4gICAgICovXHJcbiAgICB0aGlzLnR5cGUgPSB0eXBlOyAvLyB0b0pTT05cclxuXHJcbiAgICAvKipcclxuICAgICAqIFVuaXF1ZSBmaWVsZCBpZC5cclxuICAgICAqIEB0eXBlIHtudW1iZXJ9XHJcbiAgICAgKi9cclxuICAgIHRoaXMuaWQgPSBpZDsgLy8gdG9KU09OLCBtYXJrZXJcclxuXHJcbiAgICAvKipcclxuICAgICAqIEV4dGVuZGVkIHR5cGUgaWYgZGlmZmVyZW50IGZyb20gcGFyZW50LlxyXG4gICAgICogQHR5cGUge3N0cmluZ3x1bmRlZmluZWR9XHJcbiAgICAgKi9cclxuICAgIHRoaXMuZXh0ZW5kID0gZXh0ZW5kIHx8IHVuZGVmaW5lZDsgLy8gdG9KU09OXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBXaGV0aGVyIHRoaXMgZmllbGQgaXMgcmVxdWlyZWQuXHJcbiAgICAgKiBAdHlwZSB7Ym9vbGVhbn1cclxuICAgICAqL1xyXG4gICAgdGhpcy5yZXF1aXJlZCA9IHJ1bGUgPT09IFwicmVxdWlyZWRcIjtcclxuXHJcbiAgICAvKipcclxuICAgICAqIFdoZXRoZXIgdGhpcyBmaWVsZCBpcyBvcHRpb25hbC5cclxuICAgICAqIEB0eXBlIHtib29sZWFufVxyXG4gICAgICovXHJcbiAgICB0aGlzLm9wdGlvbmFsID0gIXRoaXMucmVxdWlyZWQ7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBXaGV0aGVyIHRoaXMgZmllbGQgaXMgcmVwZWF0ZWQuXHJcbiAgICAgKiBAdHlwZSB7Ym9vbGVhbn1cclxuICAgICAqL1xyXG4gICAgdGhpcy5yZXBlYXRlZCA9IHJ1bGUgPT09IFwicmVwZWF0ZWRcIjtcclxuXHJcbiAgICAvKipcclxuICAgICAqIFdoZXRoZXIgdGhpcyBmaWVsZCBpcyBhIG1hcCBvciBub3QuXHJcbiAgICAgKiBAdHlwZSB7Ym9vbGVhbn1cclxuICAgICAqL1xyXG4gICAgdGhpcy5tYXAgPSBmYWxzZTtcclxuXHJcbiAgICAvKipcclxuICAgICAqIE1lc3NhZ2UgdGhpcyBmaWVsZCBiZWxvbmdzIHRvLlxyXG4gICAgICogQHR5cGUge1R5cGV8bnVsbH1cclxuICAgICAqL1xyXG4gICAgdGhpcy5tZXNzYWdlID0gbnVsbDtcclxuXHJcbiAgICAvKipcclxuICAgICAqIE9uZU9mIHRoaXMgZmllbGQgYmVsb25ncyB0bywgaWYgYW55LFxyXG4gICAgICogQHR5cGUge09uZU9mfG51bGx9XHJcbiAgICAgKi9cclxuICAgIHRoaXMucGFydE9mID0gbnVsbDtcclxuXHJcbiAgICAvKipcclxuICAgICAqIFRoZSBmaWVsZCB0eXBlJ3MgZGVmYXVsdCB2YWx1ZS5cclxuICAgICAqIEB0eXBlIHsqfVxyXG4gICAgICovXHJcbiAgICB0aGlzLnR5cGVEZWZhdWx0ID0gbnVsbDtcclxuXHJcbiAgICAvKipcclxuICAgICAqIFRoZSBmaWVsZCdzIGRlZmF1bHQgdmFsdWUgb24gcHJvdG90eXBlcy5cclxuICAgICAqIEB0eXBlIHsqfVxyXG4gICAgICovXHJcbiAgICB0aGlzLmRlZmF1bHRWYWx1ZSA9IG51bGw7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBXaGV0aGVyIHRoaXMgZmllbGQncyB2YWx1ZSBzaG91bGQgYmUgdHJlYXRlZCBhcyBhIGxvbmcuXHJcbiAgICAgKiBAdHlwZSB7Ym9vbGVhbn1cclxuICAgICAqL1xyXG4gICAgdGhpcy5sb25nID0gdXRpbC5Mb25nID8gdHlwZXMubG9uZ1t0eXBlXSAhPT0gdW5kZWZpbmVkIDogLyogaXN0YW5idWwgaWdub3JlIG5leHQgKi8gZmFsc2U7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBXaGV0aGVyIHRoaXMgZmllbGQncyB2YWx1ZSBpcyBhIGJ1ZmZlci5cclxuICAgICAqIEB0eXBlIHtib29sZWFufVxyXG4gICAgICovXHJcbiAgICB0aGlzLmJ5dGVzID0gdHlwZSA9PT0gXCJieXRlc1wiO1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmVzb2x2ZWQgdHlwZSBpZiBub3QgYSBiYXNpYyB0eXBlLlxyXG4gICAgICogQHR5cGUge1R5cGV8RW51bXxudWxsfVxyXG4gICAgICovXHJcbiAgICB0aGlzLnJlc29sdmVkVHlwZSA9IG51bGw7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBTaXN0ZXItZmllbGQgd2l0aGluIHRoZSBleHRlbmRlZCB0eXBlIGlmIGEgZGVjbGFyaW5nIGV4dGVuc2lvbiBmaWVsZC5cclxuICAgICAqIEB0eXBlIHtGaWVsZHxudWxsfVxyXG4gICAgICovXHJcbiAgICB0aGlzLmV4dGVuc2lvbkZpZWxkID0gbnVsbDtcclxuXHJcbiAgICAvKipcclxuICAgICAqIFNpc3Rlci1maWVsZCB3aXRoaW4gdGhlIGRlY2xhcmluZyBuYW1lc3BhY2UgaWYgYW4gZXh0ZW5kZWQgZmllbGQuXHJcbiAgICAgKiBAdHlwZSB7RmllbGR8bnVsbH1cclxuICAgICAqL1xyXG4gICAgdGhpcy5kZWNsYXJpbmdGaWVsZCA9IG51bGw7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBJbnRlcm5hbGx5IHJlbWVtYmVycyB3aGV0aGVyIHRoaXMgZmllbGQgaXMgcGFja2VkLlxyXG4gICAgICogQHR5cGUge2Jvb2xlYW58bnVsbH1cclxuICAgICAqIEBwcml2YXRlXHJcbiAgICAgKi9cclxuICAgIHRoaXMuX3BhY2tlZCA9IG51bGw7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBDb21tZW50IGZvciB0aGlzIGZpZWxkLlxyXG4gICAgICogQHR5cGUge3N0cmluZ3xudWxsfVxyXG4gICAgICovXHJcbiAgICB0aGlzLmNvbW1lbnQgPSBjb21tZW50O1xyXG59XHJcblxyXG4vKipcclxuICogRGV0ZXJtaW5lcyB3aGV0aGVyIHRoaXMgZmllbGQgaXMgcGFja2VkLiBPbmx5IHJlbGV2YW50IHdoZW4gcmVwZWF0ZWQgYW5kIHdvcmtpbmcgd2l0aCBwcm90bzIuXHJcbiAqIEBuYW1lIEZpZWxkI3BhY2tlZFxyXG4gKiBAdHlwZSB7Ym9vbGVhbn1cclxuICogQHJlYWRvbmx5XHJcbiAqL1xyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoRmllbGQucHJvdG90eXBlLCBcInBhY2tlZFwiLCB7XHJcbiAgICBnZXQ6IGZ1bmN0aW9uKCkge1xyXG4gICAgICAgIC8vIGRlZmF1bHRzIHRvIHBhY2tlZD10cnVlIGlmIG5vdCBleHBsaWNpdHkgc2V0IHRvIGZhbHNlXHJcbiAgICAgICAgaWYgKHRoaXMuX3BhY2tlZCA9PT0gbnVsbClcclxuICAgICAgICAgICAgdGhpcy5fcGFja2VkID0gdGhpcy5nZXRPcHRpb24oXCJwYWNrZWRcIikgIT09IGZhbHNlO1xyXG4gICAgICAgIHJldHVybiB0aGlzLl9wYWNrZWQ7XHJcbiAgICB9XHJcbn0pO1xyXG5cclxuLyoqXHJcbiAqIEBvdmVycmlkZVxyXG4gKi9cclxuRmllbGQucHJvdG90eXBlLnNldE9wdGlvbiA9IGZ1bmN0aW9uIHNldE9wdGlvbihuYW1lLCB2YWx1ZSwgaWZOb3RTZXQpIHtcclxuICAgIGlmIChuYW1lID09PSBcInBhY2tlZFwiKSAvLyBjbGVhciBjYWNoZWQgYmVmb3JlIHNldHRpbmdcclxuICAgICAgICB0aGlzLl9wYWNrZWQgPSBudWxsO1xyXG4gICAgcmV0dXJuIFJlZmxlY3Rpb25PYmplY3QucHJvdG90eXBlLnNldE9wdGlvbi5jYWxsKHRoaXMsIG5hbWUsIHZhbHVlLCBpZk5vdFNldCk7XHJcbn07XHJcblxyXG4vKipcclxuICogRmllbGQgZGVzY3JpcHRvci5cclxuICogQGludGVyZmFjZSBJRmllbGRcclxuICogQHByb3BlcnR5IHtzdHJpbmd9IFtydWxlPVwib3B0aW9uYWxcIl0gRmllbGQgcnVsZVxyXG4gKiBAcHJvcGVydHkge3N0cmluZ30gdHlwZSBGaWVsZCB0eXBlXHJcbiAqIEBwcm9wZXJ0eSB7bnVtYmVyfSBpZCBGaWVsZCBpZFxyXG4gKiBAcHJvcGVydHkge09iamVjdC48c3RyaW5nLCo+fSBbb3B0aW9uc10gRmllbGQgb3B0aW9uc1xyXG4gKi9cclxuXHJcbi8qKlxyXG4gKiBFeHRlbnNpb24gZmllbGQgZGVzY3JpcHRvci5cclxuICogQGludGVyZmFjZSBJRXh0ZW5zaW9uRmllbGRcclxuICogQGV4dGVuZHMgSUZpZWxkXHJcbiAqIEBwcm9wZXJ0eSB7c3RyaW5nfSBleHRlbmQgRXh0ZW5kZWQgdHlwZVxyXG4gKi9cclxuXHJcbi8qKlxyXG4gKiBDb252ZXJ0cyB0aGlzIGZpZWxkIHRvIGEgZmllbGQgZGVzY3JpcHRvci5cclxuICogQHBhcmFtIHtJVG9KU09OT3B0aW9uc30gW3RvSlNPTk9wdGlvbnNdIEpTT04gY29udmVyc2lvbiBvcHRpb25zXHJcbiAqIEByZXR1cm5zIHtJRmllbGR9IEZpZWxkIGRlc2NyaXB0b3JcclxuICovXHJcbkZpZWxkLnByb3RvdHlwZS50b0pTT04gPSBmdW5jdGlvbiB0b0pTT04odG9KU09OT3B0aW9ucykge1xyXG4gICAgdmFyIGtlZXBDb21tZW50cyA9IHRvSlNPTk9wdGlvbnMgPyBCb29sZWFuKHRvSlNPTk9wdGlvbnMua2VlcENvbW1lbnRzKSA6IGZhbHNlO1xyXG4gICAgcmV0dXJuIHV0aWwudG9PYmplY3QoW1xyXG4gICAgICAgIFwicnVsZVwiICAgICwgdGhpcy5ydWxlICE9PSBcIm9wdGlvbmFsXCIgJiYgdGhpcy5ydWxlIHx8IHVuZGVmaW5lZCxcclxuICAgICAgICBcInR5cGVcIiAgICAsIHRoaXMudHlwZSxcclxuICAgICAgICBcImlkXCIgICAgICAsIHRoaXMuaWQsXHJcbiAgICAgICAgXCJleHRlbmRcIiAgLCB0aGlzLmV4dGVuZCxcclxuICAgICAgICBcIm9wdGlvbnNcIiAsIHRoaXMub3B0aW9ucyxcclxuICAgICAgICBcImNvbW1lbnRcIiAsIGtlZXBDb21tZW50cyA/IHRoaXMuY29tbWVudCA6IHVuZGVmaW5lZFxyXG4gICAgXSk7XHJcbn07XHJcblxyXG4vKipcclxuICogUmVzb2x2ZXMgdGhpcyBmaWVsZCdzIHR5cGUgcmVmZXJlbmNlcy5cclxuICogQHJldHVybnMge0ZpZWxkfSBgdGhpc2BcclxuICogQHRocm93cyB7RXJyb3J9IElmIGFueSByZWZlcmVuY2UgY2Fubm90IGJlIHJlc29sdmVkXHJcbiAqL1xyXG5GaWVsZC5wcm90b3R5cGUucmVzb2x2ZSA9IGZ1bmN0aW9uIHJlc29sdmUoKSB7XHJcblxyXG4gICAgaWYgKHRoaXMucmVzb2x2ZWQpXHJcbiAgICAgICAgcmV0dXJuIHRoaXM7XHJcblxyXG4gICAgaWYgKCh0aGlzLnR5cGVEZWZhdWx0ID0gdHlwZXMuZGVmYXVsdHNbdGhpcy50eXBlXSkgPT09IHVuZGVmaW5lZCkgeyAvLyBpZiBub3QgYSBiYXNpYyB0eXBlLCByZXNvbHZlIGl0XHJcbiAgICAgICAgdGhpcy5yZXNvbHZlZFR5cGUgPSAodGhpcy5kZWNsYXJpbmdGaWVsZCA/IHRoaXMuZGVjbGFyaW5nRmllbGQucGFyZW50IDogdGhpcy5wYXJlbnQpLmxvb2t1cFR5cGVPckVudW0odGhpcy50eXBlKTtcclxuICAgICAgICBpZiAodGhpcy5yZXNvbHZlZFR5cGUgaW5zdGFuY2VvZiBUeXBlKVxyXG4gICAgICAgICAgICB0aGlzLnR5cGVEZWZhdWx0ID0gbnVsbDtcclxuICAgICAgICBlbHNlIC8vIGluc3RhbmNlb2YgRW51bVxyXG4gICAgICAgICAgICB0aGlzLnR5cGVEZWZhdWx0ID0gdGhpcy5yZXNvbHZlZFR5cGUudmFsdWVzW09iamVjdC5rZXlzKHRoaXMucmVzb2x2ZWRUeXBlLnZhbHVlcylbMF1dOyAvLyBmaXJzdCBkZWZpbmVkXHJcbiAgICB9IGVsc2UgaWYgKHRoaXMub3B0aW9ucyAmJiB0aGlzLm9wdGlvbnMucHJvdG8zX29wdGlvbmFsKSB7XHJcbiAgICAgICAgLy8gcHJvdG8zIHNjYWxhciB2YWx1ZSBtYXJrZWQgb3B0aW9uYWw7IHNob3VsZCBkZWZhdWx0IHRvIG51bGxcclxuICAgICAgICB0aGlzLnR5cGVEZWZhdWx0ID0gbnVsbDtcclxuICAgIH1cclxuXHJcbiAgICAvLyB1c2UgZXhwbGljaXRseSBzZXQgZGVmYXVsdCB2YWx1ZSBpZiBwcmVzZW50XHJcbiAgICBpZiAodGhpcy5vcHRpb25zICYmIHRoaXMub3B0aW9uc1tcImRlZmF1bHRcIl0gIT0gbnVsbCkge1xyXG4gICAgICAgIHRoaXMudHlwZURlZmF1bHQgPSB0aGlzLm9wdGlvbnNbXCJkZWZhdWx0XCJdO1xyXG4gICAgICAgIGlmICh0aGlzLnJlc29sdmVkVHlwZSBpbnN0YW5jZW9mIEVudW0gJiYgdHlwZW9mIHRoaXMudHlwZURlZmF1bHQgPT09IFwic3RyaW5nXCIpXHJcbiAgICAgICAgICAgIHRoaXMudHlwZURlZmF1bHQgPSB0aGlzLnJlc29sdmVkVHlwZS52YWx1ZXNbdGhpcy50eXBlRGVmYXVsdF07XHJcbiAgICB9XHJcblxyXG4gICAgLy8gcmVtb3ZlIHVubmVjZXNzYXJ5IG9wdGlvbnNcclxuICAgIGlmICh0aGlzLm9wdGlvbnMpIHtcclxuICAgICAgICBpZiAodGhpcy5vcHRpb25zLnBhY2tlZCA9PT0gdHJ1ZSB8fCB0aGlzLm9wdGlvbnMucGFja2VkICE9PSB1bmRlZmluZWQgJiYgdGhpcy5yZXNvbHZlZFR5cGUgJiYgISh0aGlzLnJlc29sdmVkVHlwZSBpbnN0YW5jZW9mIEVudW0pKVxyXG4gICAgICAgICAgICBkZWxldGUgdGhpcy5vcHRpb25zLnBhY2tlZDtcclxuICAgICAgICBpZiAoIU9iamVjdC5rZXlzKHRoaXMub3B0aW9ucykubGVuZ3RoKVxyXG4gICAgICAgICAgICB0aGlzLm9wdGlvbnMgPSB1bmRlZmluZWQ7XHJcbiAgICB9XHJcblxyXG4gICAgLy8gY29udmVydCB0byBpbnRlcm5hbCBkYXRhIHR5cGUgaWYgbmVjZXNzc2FyeVxyXG4gICAgaWYgKHRoaXMubG9uZykge1xyXG4gICAgICAgIHRoaXMudHlwZURlZmF1bHQgPSB1dGlsLkxvbmcuZnJvbU51bWJlcih0aGlzLnR5cGVEZWZhdWx0LCB0aGlzLnR5cGUuY2hhckF0KDApID09PSBcInVcIik7XHJcblxyXG4gICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBlbHNlICovXHJcbiAgICAgICAgaWYgKE9iamVjdC5mcmVlemUpXHJcbiAgICAgICAgICAgIE9iamVjdC5mcmVlemUodGhpcy50eXBlRGVmYXVsdCk7IC8vIGxvbmcgaW5zdGFuY2VzIGFyZSBtZWFudCB0byBiZSBpbW11dGFibGUgYW55d2F5IChpLmUuIHVzZSBzbWFsbCBpbnQgY2FjaGUgdGhhdCBldmVuIHJlcXVpcmVzIGl0KVxyXG5cclxuICAgIH0gZWxzZSBpZiAodGhpcy5ieXRlcyAmJiB0eXBlb2YgdGhpcy50eXBlRGVmYXVsdCA9PT0gXCJzdHJpbmdcIikge1xyXG4gICAgICAgIHZhciBidWY7XHJcbiAgICAgICAgaWYgKHV0aWwuYmFzZTY0LnRlc3QodGhpcy50eXBlRGVmYXVsdCkpXHJcbiAgICAgICAgICAgIHV0aWwuYmFzZTY0LmRlY29kZSh0aGlzLnR5cGVEZWZhdWx0LCBidWYgPSB1dGlsLm5ld0J1ZmZlcih1dGlsLmJhc2U2NC5sZW5ndGgodGhpcy50eXBlRGVmYXVsdCkpLCAwKTtcclxuICAgICAgICBlbHNlXHJcbiAgICAgICAgICAgIHV0aWwudXRmOC53cml0ZSh0aGlzLnR5cGVEZWZhdWx0LCBidWYgPSB1dGlsLm5ld0J1ZmZlcih1dGlsLnV0ZjgubGVuZ3RoKHRoaXMudHlwZURlZmF1bHQpKSwgMCk7XHJcbiAgICAgICAgdGhpcy50eXBlRGVmYXVsdCA9IGJ1ZjtcclxuICAgIH1cclxuXHJcbiAgICAvLyB0YWtlIHNwZWNpYWwgY2FyZSBvZiBtYXBzIGFuZCByZXBlYXRlZCBmaWVsZHNcclxuICAgIGlmICh0aGlzLm1hcClcclxuICAgICAgICB0aGlzLmRlZmF1bHRWYWx1ZSA9IHV0aWwuZW1wdHlPYmplY3Q7XHJcbiAgICBlbHNlIGlmICh0aGlzLnJlcGVhdGVkKVxyXG4gICAgICAgIHRoaXMuZGVmYXVsdFZhbHVlID0gdXRpbC5lbXB0eUFycmF5O1xyXG4gICAgZWxzZVxyXG4gICAgICAgIHRoaXMuZGVmYXVsdFZhbHVlID0gdGhpcy50eXBlRGVmYXVsdDtcclxuXHJcbiAgICAvLyBlbnN1cmUgcHJvcGVyIHZhbHVlIG9uIHByb3RvdHlwZVxyXG4gICAgaWYgKHRoaXMucGFyZW50IGluc3RhbmNlb2YgVHlwZSlcclxuICAgICAgICB0aGlzLnBhcmVudC5jdG9yLnByb3RvdHlwZVt0aGlzLm5hbWVdID0gdGhpcy5kZWZhdWx0VmFsdWU7XHJcblxyXG4gICAgcmV0dXJuIFJlZmxlY3Rpb25PYmplY3QucHJvdG90eXBlLnJlc29sdmUuY2FsbCh0aGlzKTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBEZWNvcmF0b3IgZnVuY3Rpb24gYXMgcmV0dXJuZWQgYnkge0BsaW5rIEZpZWxkLmR9IGFuZCB7QGxpbmsgTWFwRmllbGQuZH0gKFR5cGVTY3JpcHQpLlxyXG4gKiBAdHlwZWRlZiBGaWVsZERlY29yYXRvclxyXG4gKiBAdHlwZSB7ZnVuY3Rpb259XHJcbiAqIEBwYXJhbSB7T2JqZWN0fSBwcm90b3R5cGUgVGFyZ2V0IHByb3RvdHlwZVxyXG4gKiBAcGFyYW0ge3N0cmluZ30gZmllbGROYW1lIEZpZWxkIG5hbWVcclxuICogQHJldHVybnMge3VuZGVmaW5lZH1cclxuICovXHJcblxyXG4vKipcclxuICogRmllbGQgZGVjb3JhdG9yIChUeXBlU2NyaXB0KS5cclxuICogQG5hbWUgRmllbGQuZFxyXG4gKiBAZnVuY3Rpb25cclxuICogQHBhcmFtIHtudW1iZXJ9IGZpZWxkSWQgRmllbGQgaWRcclxuICogQHBhcmFtIHtcImRvdWJsZVwifFwiZmxvYXRcInxcImludDMyXCJ8XCJ1aW50MzJcInxcInNpbnQzMlwifFwiZml4ZWQzMlwifFwic2ZpeGVkMzJcInxcImludDY0XCJ8XCJ1aW50NjRcInxcInNpbnQ2NFwifFwiZml4ZWQ2NFwifFwic2ZpeGVkNjRcInxcInN0cmluZ1wifFwiYm9vbFwifFwiYnl0ZXNcInxPYmplY3R9IGZpZWxkVHlwZSBGaWVsZCB0eXBlXHJcbiAqIEBwYXJhbSB7XCJvcHRpb25hbFwifFwicmVxdWlyZWRcInxcInJlcGVhdGVkXCJ9IFtmaWVsZFJ1bGU9XCJvcHRpb25hbFwiXSBGaWVsZCBydWxlXHJcbiAqIEBwYXJhbSB7VH0gW2RlZmF1bHRWYWx1ZV0gRGVmYXVsdCB2YWx1ZVxyXG4gKiBAcmV0dXJucyB7RmllbGREZWNvcmF0b3J9IERlY29yYXRvciBmdW5jdGlvblxyXG4gKiBAdGVtcGxhdGUgVCBleHRlbmRzIG51bWJlciB8IG51bWJlcltdIHwgTG9uZyB8IExvbmdbXSB8IHN0cmluZyB8IHN0cmluZ1tdIHwgYm9vbGVhbiB8IGJvb2xlYW5bXSB8IFVpbnQ4QXJyYXkgfCBVaW50OEFycmF5W10gfCBCdWZmZXIgfCBCdWZmZXJbXVxyXG4gKi9cclxuRmllbGQuZCA9IGZ1bmN0aW9uIGRlY29yYXRlRmllbGQoZmllbGRJZCwgZmllbGRUeXBlLCBmaWVsZFJ1bGUsIGRlZmF1bHRWYWx1ZSkge1xyXG5cclxuICAgIC8vIHN1Ym1lc3NhZ2U6IGRlY29yYXRlIHRoZSBzdWJtZXNzYWdlIGFuZCB1c2UgaXRzIG5hbWUgYXMgdGhlIHR5cGVcclxuICAgIGlmICh0eXBlb2YgZmllbGRUeXBlID09PSBcImZ1bmN0aW9uXCIpXHJcbiAgICAgICAgZmllbGRUeXBlID0gdXRpbC5kZWNvcmF0ZVR5cGUoZmllbGRUeXBlKS5uYW1lO1xyXG5cclxuICAgIC8vIGVudW0gcmVmZXJlbmNlOiBjcmVhdGUgYSByZWZsZWN0ZWQgY29weSBvZiB0aGUgZW51bSBhbmQga2VlcCByZXVzZWluZyBpdFxyXG4gICAgZWxzZSBpZiAoZmllbGRUeXBlICYmIHR5cGVvZiBmaWVsZFR5cGUgPT09IFwib2JqZWN0XCIpXHJcbiAgICAgICAgZmllbGRUeXBlID0gdXRpbC5kZWNvcmF0ZUVudW0oZmllbGRUeXBlKS5uYW1lO1xyXG5cclxuICAgIHJldHVybiBmdW5jdGlvbiBmaWVsZERlY29yYXRvcihwcm90b3R5cGUsIGZpZWxkTmFtZSkge1xyXG4gICAgICAgIHV0aWwuZGVjb3JhdGVUeXBlKHByb3RvdHlwZS5jb25zdHJ1Y3RvcilcclxuICAgICAgICAgICAgLmFkZChuZXcgRmllbGQoZmllbGROYW1lLCBmaWVsZElkLCBmaWVsZFR5cGUsIGZpZWxkUnVsZSwgeyBcImRlZmF1bHRcIjogZGVmYXVsdFZhbHVlIH0pKTtcclxuICAgIH07XHJcbn07XHJcblxyXG4vKipcclxuICogRmllbGQgZGVjb3JhdG9yIChUeXBlU2NyaXB0KS5cclxuICogQG5hbWUgRmllbGQuZFxyXG4gKiBAZnVuY3Rpb25cclxuICogQHBhcmFtIHtudW1iZXJ9IGZpZWxkSWQgRmllbGQgaWRcclxuICogQHBhcmFtIHtDb25zdHJ1Y3RvcjxUPnxzdHJpbmd9IGZpZWxkVHlwZSBGaWVsZCB0eXBlXHJcbiAqIEBwYXJhbSB7XCJvcHRpb25hbFwifFwicmVxdWlyZWRcInxcInJlcGVhdGVkXCJ9IFtmaWVsZFJ1bGU9XCJvcHRpb25hbFwiXSBGaWVsZCBydWxlXHJcbiAqIEByZXR1cm5zIHtGaWVsZERlY29yYXRvcn0gRGVjb3JhdG9yIGZ1bmN0aW9uXHJcbiAqIEB0ZW1wbGF0ZSBUIGV4dGVuZHMgTWVzc2FnZTxUPlxyXG4gKiBAdmFyaWF0aW9uIDJcclxuICovXHJcbi8vIGxpa2UgRmllbGQuZCBidXQgd2l0aG91dCBhIGRlZmF1bHQgdmFsdWVcclxuXHJcbi8vIFNldHMgdXAgY3ljbGljIGRlcGVuZGVuY2llcyAoY2FsbGVkIGluIGluZGV4LWxpZ2h0KVxyXG5GaWVsZC5fY29uZmlndXJlID0gZnVuY3Rpb24gY29uZmlndXJlKFR5cGVfKSB7XHJcbiAgICBUeXBlID0gVHlwZV87XHJcbn07XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG52YXIgcHJvdG9idWYgPSBtb2R1bGUuZXhwb3J0cyA9IHJlcXVpcmUoXCIuL2luZGV4LW1pbmltYWxcIik7XHJcblxyXG5wcm90b2J1Zi5idWlsZCA9IFwibGlnaHRcIjtcclxuXHJcbi8qKlxyXG4gKiBBIG5vZGUtc3R5bGUgY2FsbGJhY2sgYXMgdXNlZCBieSB7QGxpbmsgbG9hZH0gYW5kIHtAbGluayBSb290I2xvYWR9LlxyXG4gKiBAdHlwZWRlZiBMb2FkQ2FsbGJhY2tcclxuICogQHR5cGUge2Z1bmN0aW9ufVxyXG4gKiBAcGFyYW0ge0Vycm9yfG51bGx9IGVycm9yIEVycm9yLCBpZiBhbnksIG90aGVyd2lzZSBgbnVsbGBcclxuICogQHBhcmFtIHtSb290fSBbcm9vdF0gUm9vdCwgaWYgdGhlcmUgaGFzbid0IGJlZW4gYW4gZXJyb3JcclxuICogQHJldHVybnMge3VuZGVmaW5lZH1cclxuICovXHJcblxyXG4vKipcclxuICogTG9hZHMgb25lIG9yIG11bHRpcGxlIC5wcm90byBvciBwcmVwcm9jZXNzZWQgLmpzb24gZmlsZXMgaW50byBhIGNvbW1vbiByb290IG5hbWVzcGFjZSBhbmQgY2FsbHMgdGhlIGNhbGxiYWNrLlxyXG4gKiBAcGFyYW0ge3N0cmluZ3xzdHJpbmdbXX0gZmlsZW5hbWUgT25lIG9yIG11bHRpcGxlIGZpbGVzIHRvIGxvYWRcclxuICogQHBhcmFtIHtSb290fSByb290IFJvb3QgbmFtZXNwYWNlLCBkZWZhdWx0cyB0byBjcmVhdGUgYSBuZXcgb25lIGlmIG9taXR0ZWQuXHJcbiAqIEBwYXJhbSB7TG9hZENhbGxiYWNrfSBjYWxsYmFjayBDYWxsYmFjayBmdW5jdGlvblxyXG4gKiBAcmV0dXJucyB7dW5kZWZpbmVkfVxyXG4gKiBAc2VlIHtAbGluayBSb290I2xvYWR9XHJcbiAqL1xyXG5mdW5jdGlvbiBsb2FkKGZpbGVuYW1lLCByb290LCBjYWxsYmFjaykge1xyXG4gICAgaWYgKHR5cGVvZiByb290ID09PSBcImZ1bmN0aW9uXCIpIHtcclxuICAgICAgICBjYWxsYmFjayA9IHJvb3Q7XHJcbiAgICAgICAgcm9vdCA9IG5ldyBwcm90b2J1Zi5Sb290KCk7XHJcbiAgICB9IGVsc2UgaWYgKCFyb290KVxyXG4gICAgICAgIHJvb3QgPSBuZXcgcHJvdG9idWYuUm9vdCgpO1xyXG4gICAgcmV0dXJuIHJvb3QubG9hZChmaWxlbmFtZSwgY2FsbGJhY2spO1xyXG59XHJcblxyXG4vKipcclxuICogTG9hZHMgb25lIG9yIG11bHRpcGxlIC5wcm90byBvciBwcmVwcm9jZXNzZWQgLmpzb24gZmlsZXMgaW50byBhIGNvbW1vbiByb290IG5hbWVzcGFjZSBhbmQgY2FsbHMgdGhlIGNhbGxiYWNrLlxyXG4gKiBAbmFtZSBsb2FkXHJcbiAqIEBmdW5jdGlvblxyXG4gKiBAcGFyYW0ge3N0cmluZ3xzdHJpbmdbXX0gZmlsZW5hbWUgT25lIG9yIG11bHRpcGxlIGZpbGVzIHRvIGxvYWRcclxuICogQHBhcmFtIHtMb2FkQ2FsbGJhY2t9IGNhbGxiYWNrIENhbGxiYWNrIGZ1bmN0aW9uXHJcbiAqIEByZXR1cm5zIHt1bmRlZmluZWR9XHJcbiAqIEBzZWUge0BsaW5rIFJvb3QjbG9hZH1cclxuICogQHZhcmlhdGlvbiAyXHJcbiAqL1xyXG4vLyBmdW5jdGlvbiBsb2FkKGZpbGVuYW1lOnN0cmluZywgY2FsbGJhY2s6TG9hZENhbGxiYWNrKTp1bmRlZmluZWRcclxuXHJcbi8qKlxyXG4gKiBMb2FkcyBvbmUgb3IgbXVsdGlwbGUgLnByb3RvIG9yIHByZXByb2Nlc3NlZCAuanNvbiBmaWxlcyBpbnRvIGEgY29tbW9uIHJvb3QgbmFtZXNwYWNlIGFuZCByZXR1cm5zIGEgcHJvbWlzZS5cclxuICogQG5hbWUgbG9hZFxyXG4gKiBAZnVuY3Rpb25cclxuICogQHBhcmFtIHtzdHJpbmd8c3RyaW5nW119IGZpbGVuYW1lIE9uZSBvciBtdWx0aXBsZSBmaWxlcyB0byBsb2FkXHJcbiAqIEBwYXJhbSB7Um9vdH0gW3Jvb3RdIFJvb3QgbmFtZXNwYWNlLCBkZWZhdWx0cyB0byBjcmVhdGUgYSBuZXcgb25lIGlmIG9taXR0ZWQuXHJcbiAqIEByZXR1cm5zIHtQcm9taXNlPFJvb3Q+fSBQcm9taXNlXHJcbiAqIEBzZWUge0BsaW5rIFJvb3QjbG9hZH1cclxuICogQHZhcmlhdGlvbiAzXHJcbiAqL1xyXG4vLyBmdW5jdGlvbiBsb2FkKGZpbGVuYW1lOnN0cmluZywgW3Jvb3Q6Um9vdF0pOlByb21pc2U8Um9vdD5cclxuXHJcbnByb3RvYnVmLmxvYWQgPSBsb2FkO1xyXG5cclxuLyoqXHJcbiAqIFN5bmNocm9ub3VzbHkgbG9hZHMgb25lIG9yIG11bHRpcGxlIC5wcm90byBvciBwcmVwcm9jZXNzZWQgLmpzb24gZmlsZXMgaW50byBhIGNvbW1vbiByb290IG5hbWVzcGFjZSAobm9kZSBvbmx5KS5cclxuICogQHBhcmFtIHtzdHJpbmd8c3RyaW5nW119IGZpbGVuYW1lIE9uZSBvciBtdWx0aXBsZSBmaWxlcyB0byBsb2FkXHJcbiAqIEBwYXJhbSB7Um9vdH0gW3Jvb3RdIFJvb3QgbmFtZXNwYWNlLCBkZWZhdWx0cyB0byBjcmVhdGUgYSBuZXcgb25lIGlmIG9taXR0ZWQuXHJcbiAqIEByZXR1cm5zIHtSb290fSBSb290IG5hbWVzcGFjZVxyXG4gKiBAdGhyb3dzIHtFcnJvcn0gSWYgc3luY2hyb25vdXMgZmV0Y2hpbmcgaXMgbm90IHN1cHBvcnRlZCAoaS5lLiBpbiBicm93c2Vycykgb3IgaWYgYSBmaWxlJ3Mgc3ludGF4IGlzIGludmFsaWRcclxuICogQHNlZSB7QGxpbmsgUm9vdCNsb2FkU3luY31cclxuICovXHJcbmZ1bmN0aW9uIGxvYWRTeW5jKGZpbGVuYW1lLCByb290KSB7XHJcbiAgICBpZiAoIXJvb3QpXHJcbiAgICAgICAgcm9vdCA9IG5ldyBwcm90b2J1Zi5Sb290KCk7XHJcbiAgICByZXR1cm4gcm9vdC5sb2FkU3luYyhmaWxlbmFtZSk7XHJcbn1cclxuXHJcbnByb3RvYnVmLmxvYWRTeW5jID0gbG9hZFN5bmM7XHJcblxyXG4vLyBTZXJpYWxpemF0aW9uXHJcbnByb3RvYnVmLmVuY29kZXIgICAgICAgICAgPSByZXF1aXJlKFwiLi9lbmNvZGVyXCIpO1xyXG5wcm90b2J1Zi5kZWNvZGVyICAgICAgICAgID0gcmVxdWlyZShcIi4vZGVjb2RlclwiKTtcclxucHJvdG9idWYudmVyaWZpZXIgICAgICAgICA9IHJlcXVpcmUoXCIuL3ZlcmlmaWVyXCIpO1xyXG5wcm90b2J1Zi5jb252ZXJ0ZXIgICAgICAgID0gcmVxdWlyZShcIi4vY29udmVydGVyXCIpO1xyXG5cclxuLy8gUmVmbGVjdGlvblxyXG5wcm90b2J1Zi5SZWZsZWN0aW9uT2JqZWN0ID0gcmVxdWlyZShcIi4vb2JqZWN0XCIpO1xyXG5wcm90b2J1Zi5OYW1lc3BhY2UgICAgICAgID0gcmVxdWlyZShcIi4vbmFtZXNwYWNlXCIpO1xyXG5wcm90b2J1Zi5Sb290ICAgICAgICAgICAgID0gcmVxdWlyZShcIi4vcm9vdFwiKTtcclxucHJvdG9idWYuRW51bSAgICAgICAgICAgICA9IHJlcXVpcmUoXCIuL2VudW1cIik7XHJcbnByb3RvYnVmLlR5cGUgICAgICAgICAgICAgPSByZXF1aXJlKFwiLi90eXBlXCIpO1xyXG5wcm90b2J1Zi5GaWVsZCAgICAgICAgICAgID0gcmVxdWlyZShcIi4vZmllbGRcIik7XHJcbnByb3RvYnVmLk9uZU9mICAgICAgICAgICAgPSByZXF1aXJlKFwiLi9vbmVvZlwiKTtcclxucHJvdG9idWYuTWFwRmllbGQgICAgICAgICA9IHJlcXVpcmUoXCIuL21hcGZpZWxkXCIpO1xyXG5wcm90b2J1Zi5TZXJ2aWNlICAgICAgICAgID0gcmVxdWlyZShcIi4vc2VydmljZVwiKTtcclxucHJvdG9idWYuTWV0aG9kICAgICAgICAgICA9IHJlcXVpcmUoXCIuL21ldGhvZFwiKTtcclxuXHJcbi8vIFJ1bnRpbWVcclxucHJvdG9idWYuTWVzc2FnZSAgICAgICAgICA9IHJlcXVpcmUoXCIuL21lc3NhZ2VcIik7XHJcbnByb3RvYnVmLndyYXBwZXJzICAgICAgICAgPSByZXF1aXJlKFwiLi93cmFwcGVyc1wiKTtcclxuXHJcbi8vIFV0aWxpdHlcclxucHJvdG9idWYudHlwZXMgICAgICAgICAgICA9IHJlcXVpcmUoXCIuL3R5cGVzXCIpO1xyXG5wcm90b2J1Zi51dGlsICAgICAgICAgICAgID0gcmVxdWlyZShcIi4vdXRpbFwiKTtcclxuXHJcbi8vIFNldCB1cCBwb3NzaWJseSBjeWNsaWMgcmVmbGVjdGlvbiBkZXBlbmRlbmNpZXNcclxucHJvdG9idWYuUmVmbGVjdGlvbk9iamVjdC5fY29uZmlndXJlKHByb3RvYnVmLlJvb3QpO1xyXG5wcm90b2J1Zi5OYW1lc3BhY2UuX2NvbmZpZ3VyZShwcm90b2J1Zi5UeXBlLCBwcm90b2J1Zi5TZXJ2aWNlLCBwcm90b2J1Zi5FbnVtKTtcclxucHJvdG9idWYuUm9vdC5fY29uZmlndXJlKHByb3RvYnVmLlR5cGUpO1xyXG5wcm90b2J1Zi5GaWVsZC5fY29uZmlndXJlKHByb3RvYnVmLlR5cGUpO1xyXG4iLCJcInVzZSBzdHJpY3RcIjtcclxudmFyIHByb3RvYnVmID0gZXhwb3J0cztcclxuXHJcbi8qKlxyXG4gKiBCdWlsZCB0eXBlLCBvbmUgb2YgYFwiZnVsbFwiYCwgYFwibGlnaHRcImAgb3IgYFwibWluaW1hbFwiYC5cclxuICogQG5hbWUgYnVpbGRcclxuICogQHR5cGUge3N0cmluZ31cclxuICogQGNvbnN0XHJcbiAqL1xyXG5wcm90b2J1Zi5idWlsZCA9IFwibWluaW1hbFwiO1xyXG5cclxuLy8gU2VyaWFsaXphdGlvblxyXG5wcm90b2J1Zi5Xcml0ZXIgICAgICAgPSByZXF1aXJlKFwiLi93cml0ZXJcIik7XHJcbnByb3RvYnVmLkJ1ZmZlcldyaXRlciA9IHJlcXVpcmUoXCIuL3dyaXRlcl9idWZmZXJcIik7XHJcbnByb3RvYnVmLlJlYWRlciAgICAgICA9IHJlcXVpcmUoXCIuL3JlYWRlclwiKTtcclxucHJvdG9idWYuQnVmZmVyUmVhZGVyID0gcmVxdWlyZShcIi4vcmVhZGVyX2J1ZmZlclwiKTtcclxuXHJcbi8vIFV0aWxpdHlcclxucHJvdG9idWYudXRpbCAgICAgICAgID0gcmVxdWlyZShcIi4vdXRpbC9taW5pbWFsXCIpO1xyXG5wcm90b2J1Zi5ycGMgICAgICAgICAgPSByZXF1aXJlKFwiLi9ycGNcIik7XHJcbnByb3RvYnVmLnJvb3RzICAgICAgICA9IHJlcXVpcmUoXCIuL3Jvb3RzXCIpO1xyXG5wcm90b2J1Zi5jb25maWd1cmUgICAgPSBjb25maWd1cmU7XHJcblxyXG4vKiBpc3RhbmJ1bCBpZ25vcmUgbmV4dCAqL1xyXG4vKipcclxuICogUmVjb25maWd1cmVzIHRoZSBsaWJyYXJ5IGFjY29yZGluZyB0byB0aGUgZW52aXJvbm1lbnQuXHJcbiAqIEByZXR1cm5zIHt1bmRlZmluZWR9XHJcbiAqL1xyXG5mdW5jdGlvbiBjb25maWd1cmUoKSB7XHJcbiAgICBwcm90b2J1Zi51dGlsLl9jb25maWd1cmUoKTtcclxuICAgIHByb3RvYnVmLldyaXRlci5fY29uZmlndXJlKHByb3RvYnVmLkJ1ZmZlcldyaXRlcik7XHJcbiAgICBwcm90b2J1Zi5SZWFkZXIuX2NvbmZpZ3VyZShwcm90b2J1Zi5CdWZmZXJSZWFkZXIpO1xyXG59XHJcblxyXG4vLyBTZXQgdXAgYnVmZmVyIHV0aWxpdHkgYWNjb3JkaW5nIHRvIHRoZSBlbnZpcm9ubWVudFxyXG5jb25maWd1cmUoKTtcclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbnZhciBwcm90b2J1ZiA9IG1vZHVsZS5leHBvcnRzID0gcmVxdWlyZShcIi4vaW5kZXgtbGlnaHRcIik7XHJcblxyXG5wcm90b2J1Zi5idWlsZCA9IFwiZnVsbFwiO1xyXG5cclxuLy8gUGFyc2VyXHJcbnByb3RvYnVmLnRva2VuaXplICAgICAgICAgPSByZXF1aXJlKFwiLi90b2tlbml6ZVwiKTtcclxucHJvdG9idWYucGFyc2UgICAgICAgICAgICA9IHJlcXVpcmUoXCIuL3BhcnNlXCIpO1xyXG5wcm90b2J1Zi5jb21tb24gICAgICAgICAgID0gcmVxdWlyZShcIi4vY29tbW9uXCIpO1xyXG5cclxuLy8gQ29uZmlndXJlIHBhcnNlclxyXG5wcm90b2J1Zi5Sb290Ll9jb25maWd1cmUocHJvdG9idWYuVHlwZSwgcHJvdG9idWYucGFyc2UsIHByb3RvYnVmLmNvbW1vbik7XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG5tb2R1bGUuZXhwb3J0cyA9IE1hcEZpZWxkO1xyXG5cclxuLy8gZXh0ZW5kcyBGaWVsZFxyXG52YXIgRmllbGQgPSByZXF1aXJlKFwiLi9maWVsZFwiKTtcclxuKChNYXBGaWVsZC5wcm90b3R5cGUgPSBPYmplY3QuY3JlYXRlKEZpZWxkLnByb3RvdHlwZSkpLmNvbnN0cnVjdG9yID0gTWFwRmllbGQpLmNsYXNzTmFtZSA9IFwiTWFwRmllbGRcIjtcclxuXHJcbnZhciB0eXBlcyAgID0gcmVxdWlyZShcIi4vdHlwZXNcIiksXHJcbiAgICB1dGlsICAgID0gcmVxdWlyZShcIi4vdXRpbFwiKTtcclxuXHJcbi8qKlxyXG4gKiBDb25zdHJ1Y3RzIGEgbmV3IG1hcCBmaWVsZCBpbnN0YW5jZS5cclxuICogQGNsYXNzZGVzYyBSZWZsZWN0ZWQgbWFwIGZpZWxkLlxyXG4gKiBAZXh0ZW5kcyBGaWVsZEJhc2VcclxuICogQGNvbnN0cnVjdG9yXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBuYW1lIFVuaXF1ZSBuYW1lIHdpdGhpbiBpdHMgbmFtZXNwYWNlXHJcbiAqIEBwYXJhbSB7bnVtYmVyfSBpZCBVbmlxdWUgaWQgd2l0aGluIGl0cyBuYW1lc3BhY2VcclxuICogQHBhcmFtIHtzdHJpbmd9IGtleVR5cGUgS2V5IHR5cGVcclxuICogQHBhcmFtIHtzdHJpbmd9IHR5cGUgVmFsdWUgdHlwZVxyXG4gKiBAcGFyYW0ge09iamVjdC48c3RyaW5nLCo+fSBbb3B0aW9uc10gRGVjbGFyZWQgb3B0aW9uc1xyXG4gKiBAcGFyYW0ge3N0cmluZ30gW2NvbW1lbnRdIENvbW1lbnQgYXNzb2NpYXRlZCB3aXRoIHRoaXMgZmllbGRcclxuICovXHJcbmZ1bmN0aW9uIE1hcEZpZWxkKG5hbWUsIGlkLCBrZXlUeXBlLCB0eXBlLCBvcHRpb25zLCBjb21tZW50KSB7XHJcbiAgICBGaWVsZC5jYWxsKHRoaXMsIG5hbWUsIGlkLCB0eXBlLCB1bmRlZmluZWQsIHVuZGVmaW5lZCwgb3B0aW9ucywgY29tbWVudCk7XHJcblxyXG4gICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICBpZiAoIXV0aWwuaXNTdHJpbmcoa2V5VHlwZSkpXHJcbiAgICAgICAgdGhyb3cgVHlwZUVycm9yKFwia2V5VHlwZSBtdXN0IGJlIGEgc3RyaW5nXCIpO1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogS2V5IHR5cGUuXHJcbiAgICAgKiBAdHlwZSB7c3RyaW5nfVxyXG4gICAgICovXHJcbiAgICB0aGlzLmtleVR5cGUgPSBrZXlUeXBlOyAvLyB0b0pTT04sIG1hcmtlclxyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmVzb2x2ZWQga2V5IHR5cGUgaWYgbm90IGEgYmFzaWMgdHlwZS5cclxuICAgICAqIEB0eXBlIHtSZWZsZWN0aW9uT2JqZWN0fG51bGx9XHJcbiAgICAgKi9cclxuICAgIHRoaXMucmVzb2x2ZWRLZXlUeXBlID0gbnVsbDtcclxuXHJcbiAgICAvLyBPdmVycmlkZXMgRmllbGQjbWFwXHJcbiAgICB0aGlzLm1hcCA9IHRydWU7XHJcbn1cclxuXHJcbi8qKlxyXG4gKiBNYXAgZmllbGQgZGVzY3JpcHRvci5cclxuICogQGludGVyZmFjZSBJTWFwRmllbGRcclxuICogQGV4dGVuZHMge0lGaWVsZH1cclxuICogQHByb3BlcnR5IHtzdHJpbmd9IGtleVR5cGUgS2V5IHR5cGVcclxuICovXHJcblxyXG4vKipcclxuICogRXh0ZW5zaW9uIG1hcCBmaWVsZCBkZXNjcmlwdG9yLlxyXG4gKiBAaW50ZXJmYWNlIElFeHRlbnNpb25NYXBGaWVsZFxyXG4gKiBAZXh0ZW5kcyBJTWFwRmllbGRcclxuICogQHByb3BlcnR5IHtzdHJpbmd9IGV4dGVuZCBFeHRlbmRlZCB0eXBlXHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIENvbnN0cnVjdHMgYSBtYXAgZmllbGQgZnJvbSBhIG1hcCBmaWVsZCBkZXNjcmlwdG9yLlxyXG4gKiBAcGFyYW0ge3N0cmluZ30gbmFtZSBGaWVsZCBuYW1lXHJcbiAqIEBwYXJhbSB7SU1hcEZpZWxkfSBqc29uIE1hcCBmaWVsZCBkZXNjcmlwdG9yXHJcbiAqIEByZXR1cm5zIHtNYXBGaWVsZH0gQ3JlYXRlZCBtYXAgZmllbGRcclxuICogQHRocm93cyB7VHlwZUVycm9yfSBJZiBhcmd1bWVudHMgYXJlIGludmFsaWRcclxuICovXHJcbk1hcEZpZWxkLmZyb21KU09OID0gZnVuY3Rpb24gZnJvbUpTT04obmFtZSwganNvbikge1xyXG4gICAgcmV0dXJuIG5ldyBNYXBGaWVsZChuYW1lLCBqc29uLmlkLCBqc29uLmtleVR5cGUsIGpzb24udHlwZSwganNvbi5vcHRpb25zLCBqc29uLmNvbW1lbnQpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIENvbnZlcnRzIHRoaXMgbWFwIGZpZWxkIHRvIGEgbWFwIGZpZWxkIGRlc2NyaXB0b3IuXHJcbiAqIEBwYXJhbSB7SVRvSlNPTk9wdGlvbnN9IFt0b0pTT05PcHRpb25zXSBKU09OIGNvbnZlcnNpb24gb3B0aW9uc1xyXG4gKiBAcmV0dXJucyB7SU1hcEZpZWxkfSBNYXAgZmllbGQgZGVzY3JpcHRvclxyXG4gKi9cclxuTWFwRmllbGQucHJvdG90eXBlLnRvSlNPTiA9IGZ1bmN0aW9uIHRvSlNPTih0b0pTT05PcHRpb25zKSB7XHJcbiAgICB2YXIga2VlcENvbW1lbnRzID0gdG9KU09OT3B0aW9ucyA/IEJvb2xlYW4odG9KU09OT3B0aW9ucy5rZWVwQ29tbWVudHMpIDogZmFsc2U7XHJcbiAgICByZXR1cm4gdXRpbC50b09iamVjdChbXHJcbiAgICAgICAgXCJrZXlUeXBlXCIgLCB0aGlzLmtleVR5cGUsXHJcbiAgICAgICAgXCJ0eXBlXCIgICAgLCB0aGlzLnR5cGUsXHJcbiAgICAgICAgXCJpZFwiICAgICAgLCB0aGlzLmlkLFxyXG4gICAgICAgIFwiZXh0ZW5kXCIgICwgdGhpcy5leHRlbmQsXHJcbiAgICAgICAgXCJvcHRpb25zXCIgLCB0aGlzLm9wdGlvbnMsXHJcbiAgICAgICAgXCJjb21tZW50XCIgLCBrZWVwQ29tbWVudHMgPyB0aGlzLmNvbW1lbnQgOiB1bmRlZmluZWRcclxuICAgIF0pO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIEBvdmVycmlkZVxyXG4gKi9cclxuTWFwRmllbGQucHJvdG90eXBlLnJlc29sdmUgPSBmdW5jdGlvbiByZXNvbHZlKCkge1xyXG4gICAgaWYgKHRoaXMucmVzb2x2ZWQpXHJcbiAgICAgICAgcmV0dXJuIHRoaXM7XHJcblxyXG4gICAgLy8gQmVzaWRlcyBhIHZhbHVlIHR5cGUsIG1hcCBmaWVsZHMgaGF2ZSBhIGtleSB0eXBlIHRoYXQgbWF5IGJlIFwiYW55IHNjYWxhciB0eXBlIGV4Y2VwdCBmb3IgZmxvYXRpbmcgcG9pbnQgdHlwZXMgYW5kIGJ5dGVzXCJcclxuICAgIGlmICh0eXBlcy5tYXBLZXlbdGhpcy5rZXlUeXBlXSA9PT0gdW5kZWZpbmVkKVxyXG4gICAgICAgIHRocm93IEVycm9yKFwiaW52YWxpZCBrZXkgdHlwZTogXCIgKyB0aGlzLmtleVR5cGUpO1xyXG5cclxuICAgIHJldHVybiBGaWVsZC5wcm90b3R5cGUucmVzb2x2ZS5jYWxsKHRoaXMpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIE1hcCBmaWVsZCBkZWNvcmF0b3IgKFR5cGVTY3JpcHQpLlxyXG4gKiBAbmFtZSBNYXBGaWVsZC5kXHJcbiAqIEBmdW5jdGlvblxyXG4gKiBAcGFyYW0ge251bWJlcn0gZmllbGRJZCBGaWVsZCBpZFxyXG4gKiBAcGFyYW0ge1wiaW50MzJcInxcInVpbnQzMlwifFwic2ludDMyXCJ8XCJmaXhlZDMyXCJ8XCJzZml4ZWQzMlwifFwiaW50NjRcInxcInVpbnQ2NFwifFwic2ludDY0XCJ8XCJmaXhlZDY0XCJ8XCJzZml4ZWQ2NFwifFwiYm9vbFwifFwic3RyaW5nXCJ9IGZpZWxkS2V5VHlwZSBGaWVsZCBrZXkgdHlwZVxyXG4gKiBAcGFyYW0ge1wiZG91YmxlXCJ8XCJmbG9hdFwifFwiaW50MzJcInxcInVpbnQzMlwifFwic2ludDMyXCJ8XCJmaXhlZDMyXCJ8XCJzZml4ZWQzMlwifFwiaW50NjRcInxcInVpbnQ2NFwifFwic2ludDY0XCJ8XCJmaXhlZDY0XCJ8XCJzZml4ZWQ2NFwifFwiYm9vbFwifFwic3RyaW5nXCJ8XCJieXRlc1wifE9iamVjdHxDb25zdHJ1Y3Rvcjx7fT59IGZpZWxkVmFsdWVUeXBlIEZpZWxkIHZhbHVlIHR5cGVcclxuICogQHJldHVybnMge0ZpZWxkRGVjb3JhdG9yfSBEZWNvcmF0b3IgZnVuY3Rpb25cclxuICogQHRlbXBsYXRlIFQgZXh0ZW5kcyB7IFtrZXk6IHN0cmluZ106IG51bWJlciB8IExvbmcgfCBzdHJpbmcgfCBib29sZWFuIHwgVWludDhBcnJheSB8IEJ1ZmZlciB8IG51bWJlcltdIHwgTWVzc2FnZTx7fT4gfVxyXG4gKi9cclxuTWFwRmllbGQuZCA9IGZ1bmN0aW9uIGRlY29yYXRlTWFwRmllbGQoZmllbGRJZCwgZmllbGRLZXlUeXBlLCBmaWVsZFZhbHVlVHlwZSkge1xyXG5cclxuICAgIC8vIHN1Ym1lc3NhZ2UgdmFsdWU6IGRlY29yYXRlIHRoZSBzdWJtZXNzYWdlIGFuZCB1c2UgaXRzIG5hbWUgYXMgdGhlIHR5cGVcclxuICAgIGlmICh0eXBlb2YgZmllbGRWYWx1ZVR5cGUgPT09IFwiZnVuY3Rpb25cIilcclxuICAgICAgICBmaWVsZFZhbHVlVHlwZSA9IHV0aWwuZGVjb3JhdGVUeXBlKGZpZWxkVmFsdWVUeXBlKS5uYW1lO1xyXG5cclxuICAgIC8vIGVudW0gcmVmZXJlbmNlIHZhbHVlOiBjcmVhdGUgYSByZWZsZWN0ZWQgY29weSBvZiB0aGUgZW51bSBhbmQga2VlcCByZXVzZWluZyBpdFxyXG4gICAgZWxzZSBpZiAoZmllbGRWYWx1ZVR5cGUgJiYgdHlwZW9mIGZpZWxkVmFsdWVUeXBlID09PSBcIm9iamVjdFwiKVxyXG4gICAgICAgIGZpZWxkVmFsdWVUeXBlID0gdXRpbC5kZWNvcmF0ZUVudW0oZmllbGRWYWx1ZVR5cGUpLm5hbWU7XHJcblxyXG4gICAgcmV0dXJuIGZ1bmN0aW9uIG1hcEZpZWxkRGVjb3JhdG9yKHByb3RvdHlwZSwgZmllbGROYW1lKSB7XHJcbiAgICAgICAgdXRpbC5kZWNvcmF0ZVR5cGUocHJvdG90eXBlLmNvbnN0cnVjdG9yKVxyXG4gICAgICAgICAgICAuYWRkKG5ldyBNYXBGaWVsZChmaWVsZE5hbWUsIGZpZWxkSWQsIGZpZWxkS2V5VHlwZSwgZmllbGRWYWx1ZVR5cGUpKTtcclxuICAgIH07XHJcbn07XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG5tb2R1bGUuZXhwb3J0cyA9IE1lc3NhZ2U7XHJcblxyXG52YXIgdXRpbCA9IHJlcXVpcmUoXCIuL3V0aWwvbWluaW1hbFwiKTtcclxuXHJcbi8qKlxyXG4gKiBDb25zdHJ1Y3RzIGEgbmV3IG1lc3NhZ2UgaW5zdGFuY2UuXHJcbiAqIEBjbGFzc2Rlc2MgQWJzdHJhY3QgcnVudGltZSBtZXNzYWdlLlxyXG4gKiBAY29uc3RydWN0b3JcclxuICogQHBhcmFtIHtQcm9wZXJ0aWVzPFQ+fSBbcHJvcGVydGllc10gUHJvcGVydGllcyB0byBzZXRcclxuICogQHRlbXBsYXRlIFQgZXh0ZW5kcyBvYmplY3QgPSBvYmplY3RcclxuICovXHJcbmZ1bmN0aW9uIE1lc3NhZ2UocHJvcGVydGllcykge1xyXG4gICAgLy8gbm90IHVzZWQgaW50ZXJuYWxseVxyXG4gICAgaWYgKHByb3BlcnRpZXMpXHJcbiAgICAgICAgZm9yICh2YXIga2V5cyA9IE9iamVjdC5rZXlzKHByb3BlcnRpZXMpLCBpID0gMDsgaSA8IGtleXMubGVuZ3RoOyArK2kpXHJcbiAgICAgICAgICAgIHRoaXNba2V5c1tpXV0gPSBwcm9wZXJ0aWVzW2tleXNbaV1dO1xyXG59XHJcblxyXG4vKipcclxuICogUmVmZXJlbmNlIHRvIHRoZSByZWZsZWN0ZWQgdHlwZS5cclxuICogQG5hbWUgTWVzc2FnZS4kdHlwZVxyXG4gKiBAdHlwZSB7VHlwZX1cclxuICogQHJlYWRvbmx5XHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIFJlZmVyZW5jZSB0byB0aGUgcmVmbGVjdGVkIHR5cGUuXHJcbiAqIEBuYW1lIE1lc3NhZ2UjJHR5cGVcclxuICogQHR5cGUge1R5cGV9XHJcbiAqIEByZWFkb25seVxyXG4gKi9cclxuXHJcbi8qZXNsaW50LWRpc2FibGUgdmFsaWQtanNkb2MqL1xyXG5cclxuLyoqXHJcbiAqIENyZWF0ZXMgYSBuZXcgbWVzc2FnZSBvZiB0aGlzIHR5cGUgdXNpbmcgdGhlIHNwZWNpZmllZCBwcm9wZXJ0aWVzLlxyXG4gKiBAcGFyYW0ge09iamVjdC48c3RyaW5nLCo+fSBbcHJvcGVydGllc10gUHJvcGVydGllcyB0byBzZXRcclxuICogQHJldHVybnMge01lc3NhZ2U8VD59IE1lc3NhZ2UgaW5zdGFuY2VcclxuICogQHRlbXBsYXRlIFQgZXh0ZW5kcyBNZXNzYWdlPFQ+XHJcbiAqIEB0aGlzIENvbnN0cnVjdG9yPFQ+XHJcbiAqL1xyXG5NZXNzYWdlLmNyZWF0ZSA9IGZ1bmN0aW9uIGNyZWF0ZShwcm9wZXJ0aWVzKSB7XHJcbiAgICByZXR1cm4gdGhpcy4kdHlwZS5jcmVhdGUocHJvcGVydGllcyk7XHJcbn07XHJcblxyXG4vKipcclxuICogRW5jb2RlcyBhIG1lc3NhZ2Ugb2YgdGhpcyB0eXBlLlxyXG4gKiBAcGFyYW0ge1R8T2JqZWN0LjxzdHJpbmcsKj59IG1lc3NhZ2UgTWVzc2FnZSB0byBlbmNvZGVcclxuICogQHBhcmFtIHtXcml0ZXJ9IFt3cml0ZXJdIFdyaXRlciB0byB1c2VcclxuICogQHJldHVybnMge1dyaXRlcn0gV3JpdGVyXHJcbiAqIEB0ZW1wbGF0ZSBUIGV4dGVuZHMgTWVzc2FnZTxUPlxyXG4gKiBAdGhpcyBDb25zdHJ1Y3RvcjxUPlxyXG4gKi9cclxuTWVzc2FnZS5lbmNvZGUgPSBmdW5jdGlvbiBlbmNvZGUobWVzc2FnZSwgd3JpdGVyKSB7XHJcbiAgICByZXR1cm4gdGhpcy4kdHlwZS5lbmNvZGUobWVzc2FnZSwgd3JpdGVyKTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBFbmNvZGVzIGEgbWVzc2FnZSBvZiB0aGlzIHR5cGUgcHJlY2VlZGVkIGJ5IGl0cyBsZW5ndGggYXMgYSB2YXJpbnQuXHJcbiAqIEBwYXJhbSB7VHxPYmplY3QuPHN0cmluZywqPn0gbWVzc2FnZSBNZXNzYWdlIHRvIGVuY29kZVxyXG4gKiBAcGFyYW0ge1dyaXRlcn0gW3dyaXRlcl0gV3JpdGVyIHRvIHVzZVxyXG4gKiBAcmV0dXJucyB7V3JpdGVyfSBXcml0ZXJcclxuICogQHRlbXBsYXRlIFQgZXh0ZW5kcyBNZXNzYWdlPFQ+XHJcbiAqIEB0aGlzIENvbnN0cnVjdG9yPFQ+XHJcbiAqL1xyXG5NZXNzYWdlLmVuY29kZURlbGltaXRlZCA9IGZ1bmN0aW9uIGVuY29kZURlbGltaXRlZChtZXNzYWdlLCB3cml0ZXIpIHtcclxuICAgIHJldHVybiB0aGlzLiR0eXBlLmVuY29kZURlbGltaXRlZChtZXNzYWdlLCB3cml0ZXIpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIERlY29kZXMgYSBtZXNzYWdlIG9mIHRoaXMgdHlwZS5cclxuICogQG5hbWUgTWVzc2FnZS5kZWNvZGVcclxuICogQGZ1bmN0aW9uXHJcbiAqIEBwYXJhbSB7UmVhZGVyfFVpbnQ4QXJyYXl9IHJlYWRlciBSZWFkZXIgb3IgYnVmZmVyIHRvIGRlY29kZVxyXG4gKiBAcmV0dXJucyB7VH0gRGVjb2RlZCBtZXNzYWdlXHJcbiAqIEB0ZW1wbGF0ZSBUIGV4dGVuZHMgTWVzc2FnZTxUPlxyXG4gKiBAdGhpcyBDb25zdHJ1Y3RvcjxUPlxyXG4gKi9cclxuTWVzc2FnZS5kZWNvZGUgPSBmdW5jdGlvbiBkZWNvZGUocmVhZGVyKSB7XHJcbiAgICByZXR1cm4gdGhpcy4kdHlwZS5kZWNvZGUocmVhZGVyKTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBEZWNvZGVzIGEgbWVzc2FnZSBvZiB0aGlzIHR5cGUgcHJlY2VlZGVkIGJ5IGl0cyBsZW5ndGggYXMgYSB2YXJpbnQuXHJcbiAqIEBuYW1lIE1lc3NhZ2UuZGVjb2RlRGVsaW1pdGVkXHJcbiAqIEBmdW5jdGlvblxyXG4gKiBAcGFyYW0ge1JlYWRlcnxVaW50OEFycmF5fSByZWFkZXIgUmVhZGVyIG9yIGJ1ZmZlciB0byBkZWNvZGVcclxuICogQHJldHVybnMge1R9IERlY29kZWQgbWVzc2FnZVxyXG4gKiBAdGVtcGxhdGUgVCBleHRlbmRzIE1lc3NhZ2U8VD5cclxuICogQHRoaXMgQ29uc3RydWN0b3I8VD5cclxuICovXHJcbk1lc3NhZ2UuZGVjb2RlRGVsaW1pdGVkID0gZnVuY3Rpb24gZGVjb2RlRGVsaW1pdGVkKHJlYWRlcikge1xyXG4gICAgcmV0dXJuIHRoaXMuJHR5cGUuZGVjb2RlRGVsaW1pdGVkKHJlYWRlcik7XHJcbn07XHJcblxyXG4vKipcclxuICogVmVyaWZpZXMgYSBtZXNzYWdlIG9mIHRoaXMgdHlwZS5cclxuICogQG5hbWUgTWVzc2FnZS52ZXJpZnlcclxuICogQGZ1bmN0aW9uXHJcbiAqIEBwYXJhbSB7T2JqZWN0LjxzdHJpbmcsKj59IG1lc3NhZ2UgUGxhaW4gb2JqZWN0IHRvIHZlcmlmeVxyXG4gKiBAcmV0dXJucyB7c3RyaW5nfG51bGx9IGBudWxsYCBpZiB2YWxpZCwgb3RoZXJ3aXNlIHRoZSByZWFzb24gd2h5IGl0IGlzIG5vdFxyXG4gKi9cclxuTWVzc2FnZS52ZXJpZnkgPSBmdW5jdGlvbiB2ZXJpZnkobWVzc2FnZSkge1xyXG4gICAgcmV0dXJuIHRoaXMuJHR5cGUudmVyaWZ5KG1lc3NhZ2UpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIENyZWF0ZXMgYSBuZXcgbWVzc2FnZSBvZiB0aGlzIHR5cGUgZnJvbSBhIHBsYWluIG9iamVjdC4gQWxzbyBjb252ZXJ0cyB2YWx1ZXMgdG8gdGhlaXIgcmVzcGVjdGl2ZSBpbnRlcm5hbCB0eXBlcy5cclxuICogQHBhcmFtIHtPYmplY3QuPHN0cmluZywqPn0gb2JqZWN0IFBsYWluIG9iamVjdFxyXG4gKiBAcmV0dXJucyB7VH0gTWVzc2FnZSBpbnN0YW5jZVxyXG4gKiBAdGVtcGxhdGUgVCBleHRlbmRzIE1lc3NhZ2U8VD5cclxuICogQHRoaXMgQ29uc3RydWN0b3I8VD5cclxuICovXHJcbk1lc3NhZ2UuZnJvbU9iamVjdCA9IGZ1bmN0aW9uIGZyb21PYmplY3Qob2JqZWN0KSB7XHJcbiAgICByZXR1cm4gdGhpcy4kdHlwZS5mcm9tT2JqZWN0KG9iamVjdCk7XHJcbn07XHJcblxyXG4vKipcclxuICogQ3JlYXRlcyBhIHBsYWluIG9iamVjdCBmcm9tIGEgbWVzc2FnZSBvZiB0aGlzIHR5cGUuIEFsc28gY29udmVydHMgdmFsdWVzIHRvIG90aGVyIHR5cGVzIGlmIHNwZWNpZmllZC5cclxuICogQHBhcmFtIHtUfSBtZXNzYWdlIE1lc3NhZ2UgaW5zdGFuY2VcclxuICogQHBhcmFtIHtJQ29udmVyc2lvbk9wdGlvbnN9IFtvcHRpb25zXSBDb252ZXJzaW9uIG9wdGlvbnNcclxuICogQHJldHVybnMge09iamVjdC48c3RyaW5nLCo+fSBQbGFpbiBvYmplY3RcclxuICogQHRlbXBsYXRlIFQgZXh0ZW5kcyBNZXNzYWdlPFQ+XHJcbiAqIEB0aGlzIENvbnN0cnVjdG9yPFQ+XHJcbiAqL1xyXG5NZXNzYWdlLnRvT2JqZWN0ID0gZnVuY3Rpb24gdG9PYmplY3QobWVzc2FnZSwgb3B0aW9ucykge1xyXG4gICAgcmV0dXJuIHRoaXMuJHR5cGUudG9PYmplY3QobWVzc2FnZSwgb3B0aW9ucyk7XHJcbn07XHJcblxyXG4vKipcclxuICogQ29udmVydHMgdGhpcyBtZXNzYWdlIHRvIEpTT04uXHJcbiAqIEByZXR1cm5zIHtPYmplY3QuPHN0cmluZywqPn0gSlNPTiBvYmplY3RcclxuICovXHJcbk1lc3NhZ2UucHJvdG90eXBlLnRvSlNPTiA9IGZ1bmN0aW9uIHRvSlNPTigpIHtcclxuICAgIHJldHVybiB0aGlzLiR0eXBlLnRvT2JqZWN0KHRoaXMsIHV0aWwudG9KU09OT3B0aW9ucyk7XHJcbn07XHJcblxyXG4vKmVzbGludC1lbmFibGUgdmFsaWQtanNkb2MqLyIsIlwidXNlIHN0cmljdFwiO1xyXG5tb2R1bGUuZXhwb3J0cyA9IE1ldGhvZDtcclxuXHJcbi8vIGV4dGVuZHMgUmVmbGVjdGlvbk9iamVjdFxyXG52YXIgUmVmbGVjdGlvbk9iamVjdCA9IHJlcXVpcmUoXCIuL29iamVjdFwiKTtcclxuKChNZXRob2QucHJvdG90eXBlID0gT2JqZWN0LmNyZWF0ZShSZWZsZWN0aW9uT2JqZWN0LnByb3RvdHlwZSkpLmNvbnN0cnVjdG9yID0gTWV0aG9kKS5jbGFzc05hbWUgPSBcIk1ldGhvZFwiO1xyXG5cclxudmFyIHV0aWwgPSByZXF1aXJlKFwiLi91dGlsXCIpO1xyXG5cclxuLyoqXHJcbiAqIENvbnN0cnVjdHMgYSBuZXcgc2VydmljZSBtZXRob2QgaW5zdGFuY2UuXHJcbiAqIEBjbGFzc2Rlc2MgUmVmbGVjdGVkIHNlcnZpY2UgbWV0aG9kLlxyXG4gKiBAZXh0ZW5kcyBSZWZsZWN0aW9uT2JqZWN0XHJcbiAqIEBjb25zdHJ1Y3RvclxyXG4gKiBAcGFyYW0ge3N0cmluZ30gbmFtZSBNZXRob2QgbmFtZVxyXG4gKiBAcGFyYW0ge3N0cmluZ3x1bmRlZmluZWR9IHR5cGUgTWV0aG9kIHR5cGUsIHVzdWFsbHkgYFwicnBjXCJgXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSByZXF1ZXN0VHlwZSBSZXF1ZXN0IG1lc3NhZ2UgdHlwZVxyXG4gKiBAcGFyYW0ge3N0cmluZ30gcmVzcG9uc2VUeXBlIFJlc3BvbnNlIG1lc3NhZ2UgdHlwZVxyXG4gKiBAcGFyYW0ge2Jvb2xlYW58T2JqZWN0LjxzdHJpbmcsKj59IFtyZXF1ZXN0U3RyZWFtXSBXaGV0aGVyIHRoZSByZXF1ZXN0IGlzIHN0cmVhbWVkXHJcbiAqIEBwYXJhbSB7Ym9vbGVhbnxPYmplY3QuPHN0cmluZywqPn0gW3Jlc3BvbnNlU3RyZWFtXSBXaGV0aGVyIHRoZSByZXNwb25zZSBpcyBzdHJlYW1lZFxyXG4gKiBAcGFyYW0ge09iamVjdC48c3RyaW5nLCo+fSBbb3B0aW9uc10gRGVjbGFyZWQgb3B0aW9uc1xyXG4gKiBAcGFyYW0ge3N0cmluZ30gW2NvbW1lbnRdIFRoZSBjb21tZW50IGZvciB0aGlzIG1ldGhvZFxyXG4gKiBAcGFyYW0ge09iamVjdC48c3RyaW5nLCo+fSBbcGFyc2VkT3B0aW9uc10gRGVjbGFyZWQgb3B0aW9ucywgcHJvcGVybHkgcGFyc2VkIGludG8gYW4gb2JqZWN0XHJcbiAqL1xyXG5mdW5jdGlvbiBNZXRob2QobmFtZSwgdHlwZSwgcmVxdWVzdFR5cGUsIHJlc3BvbnNlVHlwZSwgcmVxdWVzdFN0cmVhbSwgcmVzcG9uc2VTdHJlYW0sIG9wdGlvbnMsIGNvbW1lbnQsIHBhcnNlZE9wdGlvbnMpIHtcclxuXHJcbiAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgbmV4dCAqL1xyXG4gICAgaWYgKHV0aWwuaXNPYmplY3QocmVxdWVzdFN0cmVhbSkpIHtcclxuICAgICAgICBvcHRpb25zID0gcmVxdWVzdFN0cmVhbTtcclxuICAgICAgICByZXF1ZXN0U3RyZWFtID0gcmVzcG9uc2VTdHJlYW0gPSB1bmRlZmluZWQ7XHJcbiAgICB9IGVsc2UgaWYgKHV0aWwuaXNPYmplY3QocmVzcG9uc2VTdHJlYW0pKSB7XHJcbiAgICAgICAgb3B0aW9ucyA9IHJlc3BvbnNlU3RyZWFtO1xyXG4gICAgICAgIHJlc3BvbnNlU3RyZWFtID0gdW5kZWZpbmVkO1xyXG4gICAgfVxyXG5cclxuICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgaWYgKCEodHlwZSA9PT0gdW5kZWZpbmVkIHx8IHV0aWwuaXNTdHJpbmcodHlwZSkpKVxyXG4gICAgICAgIHRocm93IFR5cGVFcnJvcihcInR5cGUgbXVzdCBiZSBhIHN0cmluZ1wiKTtcclxuXHJcbiAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgaWYgKi9cclxuICAgIGlmICghdXRpbC5pc1N0cmluZyhyZXF1ZXN0VHlwZSkpXHJcbiAgICAgICAgdGhyb3cgVHlwZUVycm9yKFwicmVxdWVzdFR5cGUgbXVzdCBiZSBhIHN0cmluZ1wiKTtcclxuXHJcbiAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgaWYgKi9cclxuICAgIGlmICghdXRpbC5pc1N0cmluZyhyZXNwb25zZVR5cGUpKVxyXG4gICAgICAgIHRocm93IFR5cGVFcnJvcihcInJlc3BvbnNlVHlwZSBtdXN0IGJlIGEgc3RyaW5nXCIpO1xyXG5cclxuICAgIFJlZmxlY3Rpb25PYmplY3QuY2FsbCh0aGlzLCBuYW1lLCBvcHRpb25zKTtcclxuXHJcbiAgICAvKipcclxuICAgICAqIE1ldGhvZCB0eXBlLlxyXG4gICAgICogQHR5cGUge3N0cmluZ31cclxuICAgICAqL1xyXG4gICAgdGhpcy50eXBlID0gdHlwZSB8fCBcInJwY1wiOyAvLyB0b0pTT05cclxuXHJcbiAgICAvKipcclxuICAgICAqIFJlcXVlc3QgdHlwZS5cclxuICAgICAqIEB0eXBlIHtzdHJpbmd9XHJcbiAgICAgKi9cclxuICAgIHRoaXMucmVxdWVzdFR5cGUgPSByZXF1ZXN0VHlwZTsgLy8gdG9KU09OLCBtYXJrZXJcclxuXHJcbiAgICAvKipcclxuICAgICAqIFdoZXRoZXIgcmVxdWVzdHMgYXJlIHN0cmVhbWVkIG9yIG5vdC5cclxuICAgICAqIEB0eXBlIHtib29sZWFufHVuZGVmaW5lZH1cclxuICAgICAqL1xyXG4gICAgdGhpcy5yZXF1ZXN0U3RyZWFtID0gcmVxdWVzdFN0cmVhbSA/IHRydWUgOiB1bmRlZmluZWQ7IC8vIHRvSlNPTlxyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmVzcG9uc2UgdHlwZS5cclxuICAgICAqIEB0eXBlIHtzdHJpbmd9XHJcbiAgICAgKi9cclxuICAgIHRoaXMucmVzcG9uc2VUeXBlID0gcmVzcG9uc2VUeXBlOyAvLyB0b0pTT05cclxuXHJcbiAgICAvKipcclxuICAgICAqIFdoZXRoZXIgcmVzcG9uc2VzIGFyZSBzdHJlYW1lZCBvciBub3QuXHJcbiAgICAgKiBAdHlwZSB7Ym9vbGVhbnx1bmRlZmluZWR9XHJcbiAgICAgKi9cclxuICAgIHRoaXMucmVzcG9uc2VTdHJlYW0gPSByZXNwb25zZVN0cmVhbSA/IHRydWUgOiB1bmRlZmluZWQ7IC8vIHRvSlNPTlxyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmVzb2x2ZWQgcmVxdWVzdCB0eXBlLlxyXG4gICAgICogQHR5cGUge1R5cGV8bnVsbH1cclxuICAgICAqL1xyXG4gICAgdGhpcy5yZXNvbHZlZFJlcXVlc3RUeXBlID0gbnVsbDtcclxuXHJcbiAgICAvKipcclxuICAgICAqIFJlc29sdmVkIHJlc3BvbnNlIHR5cGUuXHJcbiAgICAgKiBAdHlwZSB7VHlwZXxudWxsfVxyXG4gICAgICovXHJcbiAgICB0aGlzLnJlc29sdmVkUmVzcG9uc2VUeXBlID0gbnVsbDtcclxuXHJcbiAgICAvKipcclxuICAgICAqIENvbW1lbnQgZm9yIHRoaXMgbWV0aG9kXHJcbiAgICAgKiBAdHlwZSB7c3RyaW5nfG51bGx9XHJcbiAgICAgKi9cclxuICAgIHRoaXMuY29tbWVudCA9IGNvbW1lbnQ7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBPcHRpb25zIHByb3Blcmx5IHBhcnNlZCBpbnRvIGFuIG9iamVjdFxyXG4gICAgICovXHJcbiAgICB0aGlzLnBhcnNlZE9wdGlvbnMgPSBwYXJzZWRPcHRpb25zO1xyXG59XHJcblxyXG4vKipcclxuICogTWV0aG9kIGRlc2NyaXB0b3IuXHJcbiAqIEBpbnRlcmZhY2UgSU1ldGhvZFxyXG4gKiBAcHJvcGVydHkge3N0cmluZ30gW3R5cGU9XCJycGNcIl0gTWV0aG9kIHR5cGVcclxuICogQHByb3BlcnR5IHtzdHJpbmd9IHJlcXVlc3RUeXBlIFJlcXVlc3QgdHlwZVxyXG4gKiBAcHJvcGVydHkge3N0cmluZ30gcmVzcG9uc2VUeXBlIFJlc3BvbnNlIHR5cGVcclxuICogQHByb3BlcnR5IHtib29sZWFufSBbcmVxdWVzdFN0cmVhbT1mYWxzZV0gV2hldGhlciByZXF1ZXN0cyBhcmUgc3RyZWFtZWRcclxuICogQHByb3BlcnR5IHtib29sZWFufSBbcmVzcG9uc2VTdHJlYW09ZmFsc2VdIFdoZXRoZXIgcmVzcG9uc2VzIGFyZSBzdHJlYW1lZFxyXG4gKiBAcHJvcGVydHkge09iamVjdC48c3RyaW5nLCo+fSBbb3B0aW9uc10gTWV0aG9kIG9wdGlvbnNcclxuICogQHByb3BlcnR5IHtzdHJpbmd9IGNvbW1lbnQgTWV0aG9kIGNvbW1lbnRzXHJcbiAqIEBwcm9wZXJ0eSB7T2JqZWN0LjxzdHJpbmcsKj59IFtwYXJzZWRPcHRpb25zXSBNZXRob2Qgb3B0aW9ucyBwcm9wZXJseSBwYXJzZWQgaW50byBhbiBvYmplY3RcclxuICovXHJcblxyXG4vKipcclxuICogQ29uc3RydWN0cyBhIG1ldGhvZCBmcm9tIGEgbWV0aG9kIGRlc2NyaXB0b3IuXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBuYW1lIE1ldGhvZCBuYW1lXHJcbiAqIEBwYXJhbSB7SU1ldGhvZH0ganNvbiBNZXRob2QgZGVzY3JpcHRvclxyXG4gKiBAcmV0dXJucyB7TWV0aG9kfSBDcmVhdGVkIG1ldGhvZFxyXG4gKiBAdGhyb3dzIHtUeXBlRXJyb3J9IElmIGFyZ3VtZW50cyBhcmUgaW52YWxpZFxyXG4gKi9cclxuTWV0aG9kLmZyb21KU09OID0gZnVuY3Rpb24gZnJvbUpTT04obmFtZSwganNvbikge1xyXG4gICAgcmV0dXJuIG5ldyBNZXRob2QobmFtZSwganNvbi50eXBlLCBqc29uLnJlcXVlc3RUeXBlLCBqc29uLnJlc3BvbnNlVHlwZSwganNvbi5yZXF1ZXN0U3RyZWFtLCBqc29uLnJlc3BvbnNlU3RyZWFtLCBqc29uLm9wdGlvbnMsIGpzb24uY29tbWVudCwganNvbi5wYXJzZWRPcHRpb25zKTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBDb252ZXJ0cyB0aGlzIG1ldGhvZCB0byBhIG1ldGhvZCBkZXNjcmlwdG9yLlxyXG4gKiBAcGFyYW0ge0lUb0pTT05PcHRpb25zfSBbdG9KU09OT3B0aW9uc10gSlNPTiBjb252ZXJzaW9uIG9wdGlvbnNcclxuICogQHJldHVybnMge0lNZXRob2R9IE1ldGhvZCBkZXNjcmlwdG9yXHJcbiAqL1xyXG5NZXRob2QucHJvdG90eXBlLnRvSlNPTiA9IGZ1bmN0aW9uIHRvSlNPTih0b0pTT05PcHRpb25zKSB7XHJcbiAgICB2YXIga2VlcENvbW1lbnRzID0gdG9KU09OT3B0aW9ucyA/IEJvb2xlYW4odG9KU09OT3B0aW9ucy5rZWVwQ29tbWVudHMpIDogZmFsc2U7XHJcbiAgICByZXR1cm4gdXRpbC50b09iamVjdChbXHJcbiAgICAgICAgXCJ0eXBlXCIgICAgICAgICAgICwgdGhpcy50eXBlICE9PSBcInJwY1wiICYmIC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0ICovIHRoaXMudHlwZSB8fCB1bmRlZmluZWQsXHJcbiAgICAgICAgXCJyZXF1ZXN0VHlwZVwiICAgICwgdGhpcy5yZXF1ZXN0VHlwZSxcclxuICAgICAgICBcInJlcXVlc3RTdHJlYW1cIiAgLCB0aGlzLnJlcXVlc3RTdHJlYW0sXHJcbiAgICAgICAgXCJyZXNwb25zZVR5cGVcIiAgICwgdGhpcy5yZXNwb25zZVR5cGUsXHJcbiAgICAgICAgXCJyZXNwb25zZVN0cmVhbVwiICwgdGhpcy5yZXNwb25zZVN0cmVhbSxcclxuICAgICAgICBcIm9wdGlvbnNcIiAgICAgICAgLCB0aGlzLm9wdGlvbnMsXHJcbiAgICAgICAgXCJjb21tZW50XCIgICAgICAgICwga2VlcENvbW1lbnRzID8gdGhpcy5jb21tZW50IDogdW5kZWZpbmVkLFxyXG4gICAgICAgIFwicGFyc2VkT3B0aW9uc1wiICAsIHRoaXMucGFyc2VkT3B0aW9ucyxcclxuICAgIF0pO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIEBvdmVycmlkZVxyXG4gKi9cclxuTWV0aG9kLnByb3RvdHlwZS5yZXNvbHZlID0gZnVuY3Rpb24gcmVzb2x2ZSgpIHtcclxuXHJcbiAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgaWYgKi9cclxuICAgIGlmICh0aGlzLnJlc29sdmVkKVxyXG4gICAgICAgIHJldHVybiB0aGlzO1xyXG5cclxuICAgIHRoaXMucmVzb2x2ZWRSZXF1ZXN0VHlwZSA9IHRoaXMucGFyZW50Lmxvb2t1cFR5cGUodGhpcy5yZXF1ZXN0VHlwZSk7XHJcbiAgICB0aGlzLnJlc29sdmVkUmVzcG9uc2VUeXBlID0gdGhpcy5wYXJlbnQubG9va3VwVHlwZSh0aGlzLnJlc3BvbnNlVHlwZSk7XHJcblxyXG4gICAgcmV0dXJuIFJlZmxlY3Rpb25PYmplY3QucHJvdG90eXBlLnJlc29sdmUuY2FsbCh0aGlzKTtcclxufTtcclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbm1vZHVsZS5leHBvcnRzID0gTmFtZXNwYWNlO1xyXG5cclxuLy8gZXh0ZW5kcyBSZWZsZWN0aW9uT2JqZWN0XHJcbnZhciBSZWZsZWN0aW9uT2JqZWN0ID0gcmVxdWlyZShcIi4vb2JqZWN0XCIpO1xyXG4oKE5hbWVzcGFjZS5wcm90b3R5cGUgPSBPYmplY3QuY3JlYXRlKFJlZmxlY3Rpb25PYmplY3QucHJvdG90eXBlKSkuY29uc3RydWN0b3IgPSBOYW1lc3BhY2UpLmNsYXNzTmFtZSA9IFwiTmFtZXNwYWNlXCI7XHJcblxyXG52YXIgRmllbGQgICAgPSByZXF1aXJlKFwiLi9maWVsZFwiKSxcclxuICAgIHV0aWwgICAgID0gcmVxdWlyZShcIi4vdXRpbFwiKSxcclxuICAgIE9uZU9mICAgID0gcmVxdWlyZShcIi4vb25lb2ZcIik7XHJcblxyXG52YXIgVHlwZSwgICAgLy8gY3ljbGljXHJcbiAgICBTZXJ2aWNlLFxyXG4gICAgRW51bTtcclxuXHJcbi8qKlxyXG4gKiBDb25zdHJ1Y3RzIGEgbmV3IG5hbWVzcGFjZSBpbnN0YW5jZS5cclxuICogQG5hbWUgTmFtZXNwYWNlXHJcbiAqIEBjbGFzc2Rlc2MgUmVmbGVjdGVkIG5hbWVzcGFjZS5cclxuICogQGV4dGVuZHMgTmFtZXNwYWNlQmFzZVxyXG4gKiBAY29uc3RydWN0b3JcclxuICogQHBhcmFtIHtzdHJpbmd9IG5hbWUgTmFtZXNwYWNlIG5hbWVcclxuICogQHBhcmFtIHtPYmplY3QuPHN0cmluZywqPn0gW29wdGlvbnNdIERlY2xhcmVkIG9wdGlvbnNcclxuICovXHJcblxyXG4vKipcclxuICogQ29uc3RydWN0cyBhIG5hbWVzcGFjZSBmcm9tIEpTT04uXHJcbiAqIEBtZW1iZXJvZiBOYW1lc3BhY2VcclxuICogQGZ1bmN0aW9uXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBuYW1lIE5hbWVzcGFjZSBuYW1lXHJcbiAqIEBwYXJhbSB7T2JqZWN0LjxzdHJpbmcsKj59IGpzb24gSlNPTiBvYmplY3RcclxuICogQHJldHVybnMge05hbWVzcGFjZX0gQ3JlYXRlZCBuYW1lc3BhY2VcclxuICogQHRocm93cyB7VHlwZUVycm9yfSBJZiBhcmd1bWVudHMgYXJlIGludmFsaWRcclxuICovXHJcbk5hbWVzcGFjZS5mcm9tSlNPTiA9IGZ1bmN0aW9uIGZyb21KU09OKG5hbWUsIGpzb24pIHtcclxuICAgIHJldHVybiBuZXcgTmFtZXNwYWNlKG5hbWUsIGpzb24ub3B0aW9ucykuYWRkSlNPTihqc29uLm5lc3RlZCk7XHJcbn07XHJcblxyXG4vKipcclxuICogQ29udmVydHMgYW4gYXJyYXkgb2YgcmVmbGVjdGlvbiBvYmplY3RzIHRvIEpTT04uXHJcbiAqIEBtZW1iZXJvZiBOYW1lc3BhY2VcclxuICogQHBhcmFtIHtSZWZsZWN0aW9uT2JqZWN0W119IGFycmF5IE9iamVjdCBhcnJheVxyXG4gKiBAcGFyYW0ge0lUb0pTT05PcHRpb25zfSBbdG9KU09OT3B0aW9uc10gSlNPTiBjb252ZXJzaW9uIG9wdGlvbnNcclxuICogQHJldHVybnMge09iamVjdC48c3RyaW5nLCo+fHVuZGVmaW5lZH0gSlNPTiBvYmplY3Qgb3IgYHVuZGVmaW5lZGAgd2hlbiBhcnJheSBpcyBlbXB0eVxyXG4gKi9cclxuZnVuY3Rpb24gYXJyYXlUb0pTT04oYXJyYXksIHRvSlNPTk9wdGlvbnMpIHtcclxuICAgIGlmICghKGFycmF5ICYmIGFycmF5Lmxlbmd0aCkpXHJcbiAgICAgICAgcmV0dXJuIHVuZGVmaW5lZDtcclxuICAgIHZhciBvYmogPSB7fTtcclxuICAgIGZvciAodmFyIGkgPSAwOyBpIDwgYXJyYXkubGVuZ3RoOyArK2kpXHJcbiAgICAgICAgb2JqW2FycmF5W2ldLm5hbWVdID0gYXJyYXlbaV0udG9KU09OKHRvSlNPTk9wdGlvbnMpO1xyXG4gICAgcmV0dXJuIG9iajtcclxufVxyXG5cclxuTmFtZXNwYWNlLmFycmF5VG9KU09OID0gYXJyYXlUb0pTT047XHJcblxyXG4vKipcclxuICogVGVzdHMgaWYgdGhlIHNwZWNpZmllZCBpZCBpcyByZXNlcnZlZC5cclxuICogQHBhcmFtIHtBcnJheS48bnVtYmVyW118c3RyaW5nPnx1bmRlZmluZWR9IHJlc2VydmVkIEFycmF5IG9mIHJlc2VydmVkIHJhbmdlcyBhbmQgbmFtZXNcclxuICogQHBhcmFtIHtudW1iZXJ9IGlkIElkIHRvIHRlc3RcclxuICogQHJldHVybnMge2Jvb2xlYW59IGB0cnVlYCBpZiByZXNlcnZlZCwgb3RoZXJ3aXNlIGBmYWxzZWBcclxuICovXHJcbk5hbWVzcGFjZS5pc1Jlc2VydmVkSWQgPSBmdW5jdGlvbiBpc1Jlc2VydmVkSWQocmVzZXJ2ZWQsIGlkKSB7XHJcbiAgICBpZiAocmVzZXJ2ZWQpXHJcbiAgICAgICAgZm9yICh2YXIgaSA9IDA7IGkgPCByZXNlcnZlZC5sZW5ndGg7ICsraSlcclxuICAgICAgICAgICAgaWYgKHR5cGVvZiByZXNlcnZlZFtpXSAhPT0gXCJzdHJpbmdcIiAmJiByZXNlcnZlZFtpXVswXSA8PSBpZCAmJiByZXNlcnZlZFtpXVsxXSA+IGlkKVxyXG4gICAgICAgICAgICAgICAgcmV0dXJuIHRydWU7XHJcbiAgICByZXR1cm4gZmFsc2U7XHJcbn07XHJcblxyXG4vKipcclxuICogVGVzdHMgaWYgdGhlIHNwZWNpZmllZCBuYW1lIGlzIHJlc2VydmVkLlxyXG4gKiBAcGFyYW0ge0FycmF5LjxudW1iZXJbXXxzdHJpbmc+fHVuZGVmaW5lZH0gcmVzZXJ2ZWQgQXJyYXkgb2YgcmVzZXJ2ZWQgcmFuZ2VzIGFuZCBuYW1lc1xyXG4gKiBAcGFyYW0ge3N0cmluZ30gbmFtZSBOYW1lIHRvIHRlc3RcclxuICogQHJldHVybnMge2Jvb2xlYW59IGB0cnVlYCBpZiByZXNlcnZlZCwgb3RoZXJ3aXNlIGBmYWxzZWBcclxuICovXHJcbk5hbWVzcGFjZS5pc1Jlc2VydmVkTmFtZSA9IGZ1bmN0aW9uIGlzUmVzZXJ2ZWROYW1lKHJlc2VydmVkLCBuYW1lKSB7XHJcbiAgICBpZiAocmVzZXJ2ZWQpXHJcbiAgICAgICAgZm9yICh2YXIgaSA9IDA7IGkgPCByZXNlcnZlZC5sZW5ndGg7ICsraSlcclxuICAgICAgICAgICAgaWYgKHJlc2VydmVkW2ldID09PSBuYW1lKVxyXG4gICAgICAgICAgICAgICAgcmV0dXJuIHRydWU7XHJcbiAgICByZXR1cm4gZmFsc2U7XHJcbn07XHJcblxyXG4vKipcclxuICogTm90IGFuIGFjdHVhbCBjb25zdHJ1Y3Rvci4gVXNlIHtAbGluayBOYW1lc3BhY2V9IGluc3RlYWQuXHJcbiAqIEBjbGFzc2Rlc2MgQmFzZSBjbGFzcyBvZiBhbGwgcmVmbGVjdGlvbiBvYmplY3RzIGNvbnRhaW5pbmcgbmVzdGVkIG9iamVjdHMuIFRoaXMgaXMgbm90IGFuIGFjdHVhbCBjbGFzcyBidXQgaGVyZSBmb3IgdGhlIHNha2Ugb2YgaGF2aW5nIGNvbnNpc3RlbnQgdHlwZSBkZWZpbml0aW9ucy5cclxuICogQGV4cG9ydHMgTmFtZXNwYWNlQmFzZVxyXG4gKiBAZXh0ZW5kcyBSZWZsZWN0aW9uT2JqZWN0XHJcbiAqIEBhYnN0cmFjdFxyXG4gKiBAY29uc3RydWN0b3JcclxuICogQHBhcmFtIHtzdHJpbmd9IG5hbWUgTmFtZXNwYWNlIG5hbWVcclxuICogQHBhcmFtIHtPYmplY3QuPHN0cmluZywqPn0gW29wdGlvbnNdIERlY2xhcmVkIG9wdGlvbnNcclxuICogQHNlZSB7QGxpbmsgTmFtZXNwYWNlfVxyXG4gKi9cclxuZnVuY3Rpb24gTmFtZXNwYWNlKG5hbWUsIG9wdGlvbnMpIHtcclxuICAgIFJlZmxlY3Rpb25PYmplY3QuY2FsbCh0aGlzLCBuYW1lLCBvcHRpb25zKTtcclxuXHJcbiAgICAvKipcclxuICAgICAqIE5lc3RlZCBvYmplY3RzIGJ5IG5hbWUuXHJcbiAgICAgKiBAdHlwZSB7T2JqZWN0LjxzdHJpbmcsUmVmbGVjdGlvbk9iamVjdD58dW5kZWZpbmVkfVxyXG4gICAgICovXHJcbiAgICB0aGlzLm5lc3RlZCA9IHVuZGVmaW5lZDsgLy8gdG9KU09OXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBDYWNoZWQgbmVzdGVkIG9iamVjdHMgYXMgYW4gYXJyYXkuXHJcbiAgICAgKiBAdHlwZSB7UmVmbGVjdGlvbk9iamVjdFtdfG51bGx9XHJcbiAgICAgKiBAcHJpdmF0ZVxyXG4gICAgICovXHJcbiAgICB0aGlzLl9uZXN0ZWRBcnJheSA9IG51bGw7XHJcbn1cclxuXHJcbmZ1bmN0aW9uIGNsZWFyQ2FjaGUobmFtZXNwYWNlKSB7XHJcbiAgICBuYW1lc3BhY2UuX25lc3RlZEFycmF5ID0gbnVsbDtcclxuICAgIHJldHVybiBuYW1lc3BhY2U7XHJcbn1cclxuXHJcbi8qKlxyXG4gKiBOZXN0ZWQgb2JqZWN0cyBvZiB0aGlzIG5hbWVzcGFjZSBhcyBhbiBhcnJheSBmb3IgaXRlcmF0aW9uLlxyXG4gKiBAbmFtZSBOYW1lc3BhY2VCYXNlI25lc3RlZEFycmF5XHJcbiAqIEB0eXBlIHtSZWZsZWN0aW9uT2JqZWN0W119XHJcbiAqIEByZWFkb25seVxyXG4gKi9cclxuT2JqZWN0LmRlZmluZVByb3BlcnR5KE5hbWVzcGFjZS5wcm90b3R5cGUsIFwibmVzdGVkQXJyYXlcIiwge1xyXG4gICAgZ2V0OiBmdW5jdGlvbigpIHtcclxuICAgICAgICByZXR1cm4gdGhpcy5fbmVzdGVkQXJyYXkgfHwgKHRoaXMuX25lc3RlZEFycmF5ID0gdXRpbC50b0FycmF5KHRoaXMubmVzdGVkKSk7XHJcbiAgICB9XHJcbn0pO1xyXG5cclxuLyoqXHJcbiAqIE5hbWVzcGFjZSBkZXNjcmlwdG9yLlxyXG4gKiBAaW50ZXJmYWNlIElOYW1lc3BhY2VcclxuICogQHByb3BlcnR5IHtPYmplY3QuPHN0cmluZywqPn0gW29wdGlvbnNdIE5hbWVzcGFjZSBvcHRpb25zXHJcbiAqIEBwcm9wZXJ0eSB7T2JqZWN0LjxzdHJpbmcsQW55TmVzdGVkT2JqZWN0Pn0gW25lc3RlZF0gTmVzdGVkIG9iamVjdCBkZXNjcmlwdG9yc1xyXG4gKi9cclxuXHJcbi8qKlxyXG4gKiBBbnkgZXh0ZW5zaW9uIGZpZWxkIGRlc2NyaXB0b3IuXHJcbiAqIEB0eXBlZGVmIEFueUV4dGVuc2lvbkZpZWxkXHJcbiAqIEB0eXBlIHtJRXh0ZW5zaW9uRmllbGR8SUV4dGVuc2lvbk1hcEZpZWxkfVxyXG4gKi9cclxuXHJcbi8qKlxyXG4gKiBBbnkgbmVzdGVkIG9iamVjdCBkZXNjcmlwdG9yLlxyXG4gKiBAdHlwZWRlZiBBbnlOZXN0ZWRPYmplY3RcclxuICogQHR5cGUge0lFbnVtfElUeXBlfElTZXJ2aWNlfEFueUV4dGVuc2lvbkZpZWxkfElOYW1lc3BhY2V8SU9uZU9mfVxyXG4gKi9cclxuXHJcbi8qKlxyXG4gKiBDb252ZXJ0cyB0aGlzIG5hbWVzcGFjZSB0byBhIG5hbWVzcGFjZSBkZXNjcmlwdG9yLlxyXG4gKiBAcGFyYW0ge0lUb0pTT05PcHRpb25zfSBbdG9KU09OT3B0aW9uc10gSlNPTiBjb252ZXJzaW9uIG9wdGlvbnNcclxuICogQHJldHVybnMge0lOYW1lc3BhY2V9IE5hbWVzcGFjZSBkZXNjcmlwdG9yXHJcbiAqL1xyXG5OYW1lc3BhY2UucHJvdG90eXBlLnRvSlNPTiA9IGZ1bmN0aW9uIHRvSlNPTih0b0pTT05PcHRpb25zKSB7XHJcbiAgICByZXR1cm4gdXRpbC50b09iamVjdChbXHJcbiAgICAgICAgXCJvcHRpb25zXCIgLCB0aGlzLm9wdGlvbnMsXHJcbiAgICAgICAgXCJuZXN0ZWRcIiAgLCBhcnJheVRvSlNPTih0aGlzLm5lc3RlZEFycmF5LCB0b0pTT05PcHRpb25zKVxyXG4gICAgXSk7XHJcbn07XHJcblxyXG4vKipcclxuICogQWRkcyBuZXN0ZWQgb2JqZWN0cyB0byB0aGlzIG5hbWVzcGFjZSBmcm9tIG5lc3RlZCBvYmplY3QgZGVzY3JpcHRvcnMuXHJcbiAqIEBwYXJhbSB7T2JqZWN0LjxzdHJpbmcsQW55TmVzdGVkT2JqZWN0Pn0gbmVzdGVkSnNvbiBBbnkgbmVzdGVkIG9iamVjdCBkZXNjcmlwdG9yc1xyXG4gKiBAcmV0dXJucyB7TmFtZXNwYWNlfSBgdGhpc2BcclxuICovXHJcbk5hbWVzcGFjZS5wcm90b3R5cGUuYWRkSlNPTiA9IGZ1bmN0aW9uIGFkZEpTT04obmVzdGVkSnNvbikge1xyXG4gICAgdmFyIG5zID0gdGhpcztcclxuICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBlbHNlICovXHJcbiAgICBpZiAobmVzdGVkSnNvbikge1xyXG4gICAgICAgIGZvciAodmFyIG5hbWVzID0gT2JqZWN0LmtleXMobmVzdGVkSnNvbiksIGkgPSAwLCBuZXN0ZWQ7IGkgPCBuYW1lcy5sZW5ndGg7ICsraSkge1xyXG4gICAgICAgICAgICBuZXN0ZWQgPSBuZXN0ZWRKc29uW25hbWVzW2ldXTtcclxuICAgICAgICAgICAgbnMuYWRkKCAvLyBtb3N0IHRvIGxlYXN0IGxpa2VseVxyXG4gICAgICAgICAgICAgICAgKCBuZXN0ZWQuZmllbGRzICE9PSB1bmRlZmluZWRcclxuICAgICAgICAgICAgICAgID8gVHlwZS5mcm9tSlNPTlxyXG4gICAgICAgICAgICAgICAgOiBuZXN0ZWQudmFsdWVzICE9PSB1bmRlZmluZWRcclxuICAgICAgICAgICAgICAgID8gRW51bS5mcm9tSlNPTlxyXG4gICAgICAgICAgICAgICAgOiBuZXN0ZWQubWV0aG9kcyAhPT0gdW5kZWZpbmVkXHJcbiAgICAgICAgICAgICAgICA/IFNlcnZpY2UuZnJvbUpTT05cclxuICAgICAgICAgICAgICAgIDogbmVzdGVkLmlkICE9PSB1bmRlZmluZWRcclxuICAgICAgICAgICAgICAgID8gRmllbGQuZnJvbUpTT05cclxuICAgICAgICAgICAgICAgIDogTmFtZXNwYWNlLmZyb21KU09OICkobmFtZXNbaV0sIG5lc3RlZClcclxuICAgICAgICAgICAgKTtcclxuICAgICAgICB9XHJcbiAgICB9XHJcbiAgICByZXR1cm4gdGhpcztcclxufTtcclxuXHJcbi8qKlxyXG4gKiBHZXRzIHRoZSBuZXN0ZWQgb2JqZWN0IG9mIHRoZSBzcGVjaWZpZWQgbmFtZS5cclxuICogQHBhcmFtIHtzdHJpbmd9IG5hbWUgTmVzdGVkIG9iamVjdCBuYW1lXHJcbiAqIEByZXR1cm5zIHtSZWZsZWN0aW9uT2JqZWN0fG51bGx9IFRoZSByZWZsZWN0aW9uIG9iamVjdCBvciBgbnVsbGAgaWYgaXQgZG9lc24ndCBleGlzdFxyXG4gKi9cclxuTmFtZXNwYWNlLnByb3RvdHlwZS5nZXQgPSBmdW5jdGlvbiBnZXQobmFtZSkge1xyXG4gICAgcmV0dXJuIHRoaXMubmVzdGVkICYmIHRoaXMubmVzdGVkW25hbWVdXHJcbiAgICAgICAgfHwgbnVsbDtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBHZXRzIHRoZSB2YWx1ZXMgb2YgdGhlIG5lc3RlZCB7QGxpbmsgRW51bXxlbnVtfSBvZiB0aGUgc3BlY2lmaWVkIG5hbWUuXHJcbiAqIFRoaXMgbWV0aG9kcyBkaWZmZXJzIGZyb20ge0BsaW5rIE5hbWVzcGFjZSNnZXR8Z2V0fSBpbiB0aGF0IGl0IHJldHVybnMgYW4gZW51bSdzIHZhbHVlcyBkaXJlY3RseSBhbmQgdGhyb3dzIGluc3RlYWQgb2YgcmV0dXJuaW5nIGBudWxsYC5cclxuICogQHBhcmFtIHtzdHJpbmd9IG5hbWUgTmVzdGVkIGVudW0gbmFtZVxyXG4gKiBAcmV0dXJucyB7T2JqZWN0LjxzdHJpbmcsbnVtYmVyPn0gRW51bSB2YWx1ZXNcclxuICogQHRocm93cyB7RXJyb3J9IElmIHRoZXJlIGlzIG5vIHN1Y2ggZW51bVxyXG4gKi9cclxuTmFtZXNwYWNlLnByb3RvdHlwZS5nZXRFbnVtID0gZnVuY3Rpb24gZ2V0RW51bShuYW1lKSB7XHJcbiAgICBpZiAodGhpcy5uZXN0ZWQgJiYgdGhpcy5uZXN0ZWRbbmFtZV0gaW5zdGFuY2VvZiBFbnVtKVxyXG4gICAgICAgIHJldHVybiB0aGlzLm5lc3RlZFtuYW1lXS52YWx1ZXM7XHJcbiAgICB0aHJvdyBFcnJvcihcIm5vIHN1Y2ggZW51bTogXCIgKyBuYW1lKTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBBZGRzIGEgbmVzdGVkIG9iamVjdCB0byB0aGlzIG5hbWVzcGFjZS5cclxuICogQHBhcmFtIHtSZWZsZWN0aW9uT2JqZWN0fSBvYmplY3QgTmVzdGVkIG9iamVjdCB0byBhZGRcclxuICogQHJldHVybnMge05hbWVzcGFjZX0gYHRoaXNgXHJcbiAqIEB0aHJvd3Mge1R5cGVFcnJvcn0gSWYgYXJndW1lbnRzIGFyZSBpbnZhbGlkXHJcbiAqIEB0aHJvd3Mge0Vycm9yfSBJZiB0aGVyZSBpcyBhbHJlYWR5IGEgbmVzdGVkIG9iamVjdCB3aXRoIHRoaXMgbmFtZVxyXG4gKi9cclxuTmFtZXNwYWNlLnByb3RvdHlwZS5hZGQgPSBmdW5jdGlvbiBhZGQob2JqZWN0KSB7XHJcblxyXG4gICAgaWYgKCEob2JqZWN0IGluc3RhbmNlb2YgRmllbGQgJiYgb2JqZWN0LmV4dGVuZCAhPT0gdW5kZWZpbmVkIHx8IG9iamVjdCBpbnN0YW5jZW9mIFR5cGUgIHx8IG9iamVjdCBpbnN0YW5jZW9mIE9uZU9mIHx8IG9iamVjdCBpbnN0YW5jZW9mIEVudW0gfHwgb2JqZWN0IGluc3RhbmNlb2YgU2VydmljZSB8fCBvYmplY3QgaW5zdGFuY2VvZiBOYW1lc3BhY2UpKVxyXG4gICAgICAgIHRocm93IFR5cGVFcnJvcihcIm9iamVjdCBtdXN0IGJlIGEgdmFsaWQgbmVzdGVkIG9iamVjdFwiKTtcclxuXHJcbiAgICBpZiAoIXRoaXMubmVzdGVkKVxyXG4gICAgICAgIHRoaXMubmVzdGVkID0ge307XHJcbiAgICBlbHNlIHtcclxuICAgICAgICB2YXIgcHJldiA9IHRoaXMuZ2V0KG9iamVjdC5uYW1lKTtcclxuICAgICAgICBpZiAocHJldikge1xyXG4gICAgICAgICAgICBpZiAocHJldiBpbnN0YW5jZW9mIE5hbWVzcGFjZSAmJiBvYmplY3QgaW5zdGFuY2VvZiBOYW1lc3BhY2UgJiYgIShwcmV2IGluc3RhbmNlb2YgVHlwZSB8fCBwcmV2IGluc3RhbmNlb2YgU2VydmljZSkpIHtcclxuICAgICAgICAgICAgICAgIC8vIHJlcGxhY2UgcGxhaW4gbmFtZXNwYWNlIGJ1dCBrZWVwIGV4aXN0aW5nIG5lc3RlZCBlbGVtZW50cyBhbmQgb3B0aW9uc1xyXG4gICAgICAgICAgICAgICAgdmFyIG5lc3RlZCA9IHByZXYubmVzdGVkQXJyYXk7XHJcbiAgICAgICAgICAgICAgICBmb3IgKHZhciBpID0gMDsgaSA8IG5lc3RlZC5sZW5ndGg7ICsraSlcclxuICAgICAgICAgICAgICAgICAgICBvYmplY3QuYWRkKG5lc3RlZFtpXSk7XHJcbiAgICAgICAgICAgICAgICB0aGlzLnJlbW92ZShwcmV2KTtcclxuICAgICAgICAgICAgICAgIGlmICghdGhpcy5uZXN0ZWQpXHJcbiAgICAgICAgICAgICAgICAgICAgdGhpcy5uZXN0ZWQgPSB7fTtcclxuICAgICAgICAgICAgICAgIG9iamVjdC5zZXRPcHRpb25zKHByZXYub3B0aW9ucywgdHJ1ZSk7XHJcblxyXG4gICAgICAgICAgICB9IGVsc2VcclxuICAgICAgICAgICAgICAgIHRocm93IEVycm9yKFwiZHVwbGljYXRlIG5hbWUgJ1wiICsgb2JqZWN0Lm5hbWUgKyBcIicgaW4gXCIgKyB0aGlzKTtcclxuICAgICAgICB9XHJcbiAgICB9XHJcbiAgICB0aGlzLm5lc3RlZFtvYmplY3QubmFtZV0gPSBvYmplY3Q7XHJcbiAgICBvYmplY3Qub25BZGQodGhpcyk7XHJcbiAgICByZXR1cm4gY2xlYXJDYWNoZSh0aGlzKTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBSZW1vdmVzIGEgbmVzdGVkIG9iamVjdCBmcm9tIHRoaXMgbmFtZXNwYWNlLlxyXG4gKiBAcGFyYW0ge1JlZmxlY3Rpb25PYmplY3R9IG9iamVjdCBOZXN0ZWQgb2JqZWN0IHRvIHJlbW92ZVxyXG4gKiBAcmV0dXJucyB7TmFtZXNwYWNlfSBgdGhpc2BcclxuICogQHRocm93cyB7VHlwZUVycm9yfSBJZiBhcmd1bWVudHMgYXJlIGludmFsaWRcclxuICogQHRocm93cyB7RXJyb3J9IElmIGBvYmplY3RgIGlzIG5vdCBhIG1lbWJlciBvZiB0aGlzIG5hbWVzcGFjZVxyXG4gKi9cclxuTmFtZXNwYWNlLnByb3RvdHlwZS5yZW1vdmUgPSBmdW5jdGlvbiByZW1vdmUob2JqZWN0KSB7XHJcblxyXG4gICAgaWYgKCEob2JqZWN0IGluc3RhbmNlb2YgUmVmbGVjdGlvbk9iamVjdCkpXHJcbiAgICAgICAgdGhyb3cgVHlwZUVycm9yKFwib2JqZWN0IG11c3QgYmUgYSBSZWZsZWN0aW9uT2JqZWN0XCIpO1xyXG4gICAgaWYgKG9iamVjdC5wYXJlbnQgIT09IHRoaXMpXHJcbiAgICAgICAgdGhyb3cgRXJyb3Iob2JqZWN0ICsgXCIgaXMgbm90IGEgbWVtYmVyIG9mIFwiICsgdGhpcyk7XHJcblxyXG4gICAgZGVsZXRlIHRoaXMubmVzdGVkW29iamVjdC5uYW1lXTtcclxuICAgIGlmICghT2JqZWN0LmtleXModGhpcy5uZXN0ZWQpLmxlbmd0aClcclxuICAgICAgICB0aGlzLm5lc3RlZCA9IHVuZGVmaW5lZDtcclxuXHJcbiAgICBvYmplY3Qub25SZW1vdmUodGhpcyk7XHJcbiAgICByZXR1cm4gY2xlYXJDYWNoZSh0aGlzKTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBEZWZpbmVzIGFkZGl0aWFsIG5hbWVzcGFjZXMgd2l0aGluIHRoaXMgb25lIGlmIG5vdCB5ZXQgZXhpc3RpbmcuXHJcbiAqIEBwYXJhbSB7c3RyaW5nfHN0cmluZ1tdfSBwYXRoIFBhdGggdG8gY3JlYXRlXHJcbiAqIEBwYXJhbSB7Kn0gW2pzb25dIE5lc3RlZCB0eXBlcyB0byBjcmVhdGUgZnJvbSBKU09OXHJcbiAqIEByZXR1cm5zIHtOYW1lc3BhY2V9IFBvaW50ZXIgdG8gdGhlIGxhc3QgbmFtZXNwYWNlIGNyZWF0ZWQgb3IgYHRoaXNgIGlmIHBhdGggaXMgZW1wdHlcclxuICovXHJcbk5hbWVzcGFjZS5wcm90b3R5cGUuZGVmaW5lID0gZnVuY3Rpb24gZGVmaW5lKHBhdGgsIGpzb24pIHtcclxuXHJcbiAgICBpZiAodXRpbC5pc1N0cmluZyhwYXRoKSlcclxuICAgICAgICBwYXRoID0gcGF0aC5zcGxpdChcIi5cIik7XHJcbiAgICBlbHNlIGlmICghQXJyYXkuaXNBcnJheShwYXRoKSlcclxuICAgICAgICB0aHJvdyBUeXBlRXJyb3IoXCJpbGxlZ2FsIHBhdGhcIik7XHJcbiAgICBpZiAocGF0aCAmJiBwYXRoLmxlbmd0aCAmJiBwYXRoWzBdID09PSBcIlwiKVxyXG4gICAgICAgIHRocm93IEVycm9yKFwicGF0aCBtdXN0IGJlIHJlbGF0aXZlXCIpO1xyXG5cclxuICAgIHZhciBwdHIgPSB0aGlzO1xyXG4gICAgd2hpbGUgKHBhdGgubGVuZ3RoID4gMCkge1xyXG4gICAgICAgIHZhciBwYXJ0ID0gcGF0aC5zaGlmdCgpO1xyXG4gICAgICAgIGlmIChwdHIubmVzdGVkICYmIHB0ci5uZXN0ZWRbcGFydF0pIHtcclxuICAgICAgICAgICAgcHRyID0gcHRyLm5lc3RlZFtwYXJ0XTtcclxuICAgICAgICAgICAgaWYgKCEocHRyIGluc3RhbmNlb2YgTmFtZXNwYWNlKSlcclxuICAgICAgICAgICAgICAgIHRocm93IEVycm9yKFwicGF0aCBjb25mbGljdHMgd2l0aCBub24tbmFtZXNwYWNlIG9iamVjdHNcIik7XHJcbiAgICAgICAgfSBlbHNlXHJcbiAgICAgICAgICAgIHB0ci5hZGQocHRyID0gbmV3IE5hbWVzcGFjZShwYXJ0KSk7XHJcbiAgICB9XHJcbiAgICBpZiAoanNvbilcclxuICAgICAgICBwdHIuYWRkSlNPTihqc29uKTtcclxuICAgIHJldHVybiBwdHI7XHJcbn07XHJcblxyXG4vKipcclxuICogUmVzb2x2ZXMgdGhpcyBuYW1lc3BhY2UncyBhbmQgYWxsIGl0cyBuZXN0ZWQgb2JqZWN0cycgdHlwZSByZWZlcmVuY2VzLiBVc2VmdWwgdG8gdmFsaWRhdGUgYSByZWZsZWN0aW9uIHRyZWUsIGJ1dCBjb21lcyBhdCBhIGNvc3QuXHJcbiAqIEByZXR1cm5zIHtOYW1lc3BhY2V9IGB0aGlzYFxyXG4gKi9cclxuTmFtZXNwYWNlLnByb3RvdHlwZS5yZXNvbHZlQWxsID0gZnVuY3Rpb24gcmVzb2x2ZUFsbCgpIHtcclxuICAgIHZhciBuZXN0ZWQgPSB0aGlzLm5lc3RlZEFycmF5LCBpID0gMDtcclxuICAgIHdoaWxlIChpIDwgbmVzdGVkLmxlbmd0aClcclxuICAgICAgICBpZiAobmVzdGVkW2ldIGluc3RhbmNlb2YgTmFtZXNwYWNlKVxyXG4gICAgICAgICAgICBuZXN0ZWRbaSsrXS5yZXNvbHZlQWxsKCk7XHJcbiAgICAgICAgZWxzZVxyXG4gICAgICAgICAgICBuZXN0ZWRbaSsrXS5yZXNvbHZlKCk7XHJcbiAgICByZXR1cm4gdGhpcy5yZXNvbHZlKCk7XHJcbn07XHJcblxyXG4vKipcclxuICogUmVjdXJzaXZlbHkgbG9va3MgdXAgdGhlIHJlZmxlY3Rpb24gb2JqZWN0IG1hdGNoaW5nIHRoZSBzcGVjaWZpZWQgcGF0aCBpbiB0aGUgc2NvcGUgb2YgdGhpcyBuYW1lc3BhY2UuXHJcbiAqIEBwYXJhbSB7c3RyaW5nfHN0cmluZ1tdfSBwYXRoIFBhdGggdG8gbG9vayB1cFxyXG4gKiBAcGFyYW0geyp8QXJyYXkuPCo+fSBmaWx0ZXJUeXBlcyBGaWx0ZXIgdHlwZXMsIGFueSBjb21iaW5hdGlvbiBvZiB0aGUgY29uc3RydWN0b3JzIG9mIGBwcm90b2J1Zi5UeXBlYCwgYHByb3RvYnVmLkVudW1gLCBgcHJvdG9idWYuU2VydmljZWAgZXRjLlxyXG4gKiBAcGFyYW0ge2Jvb2xlYW59IFtwYXJlbnRBbHJlYWR5Q2hlY2tlZD1mYWxzZV0gSWYga25vd24sIHdoZXRoZXIgdGhlIHBhcmVudCBoYXMgYWxyZWFkeSBiZWVuIGNoZWNrZWRcclxuICogQHJldHVybnMge1JlZmxlY3Rpb25PYmplY3R8bnVsbH0gTG9va2VkIHVwIG9iamVjdCBvciBgbnVsbGAgaWYgbm9uZSBjb3VsZCBiZSBmb3VuZFxyXG4gKi9cclxuTmFtZXNwYWNlLnByb3RvdHlwZS5sb29rdXAgPSBmdW5jdGlvbiBsb29rdXAocGF0aCwgZmlsdGVyVHlwZXMsIHBhcmVudEFscmVhZHlDaGVja2VkKSB7XHJcblxyXG4gICAgLyogaXN0YW5idWwgaWdub3JlIG5leHQgKi9cclxuICAgIGlmICh0eXBlb2YgZmlsdGVyVHlwZXMgPT09IFwiYm9vbGVhblwiKSB7XHJcbiAgICAgICAgcGFyZW50QWxyZWFkeUNoZWNrZWQgPSBmaWx0ZXJUeXBlcztcclxuICAgICAgICBmaWx0ZXJUeXBlcyA9IHVuZGVmaW5lZDtcclxuICAgIH0gZWxzZSBpZiAoZmlsdGVyVHlwZXMgJiYgIUFycmF5LmlzQXJyYXkoZmlsdGVyVHlwZXMpKVxyXG4gICAgICAgIGZpbHRlclR5cGVzID0gWyBmaWx0ZXJUeXBlcyBdO1xyXG5cclxuICAgIGlmICh1dGlsLmlzU3RyaW5nKHBhdGgpICYmIHBhdGgubGVuZ3RoKSB7XHJcbiAgICAgICAgaWYgKHBhdGggPT09IFwiLlwiKVxyXG4gICAgICAgICAgICByZXR1cm4gdGhpcy5yb290O1xyXG4gICAgICAgIHBhdGggPSBwYXRoLnNwbGl0KFwiLlwiKTtcclxuICAgIH0gZWxzZSBpZiAoIXBhdGgubGVuZ3RoKVxyXG4gICAgICAgIHJldHVybiB0aGlzO1xyXG5cclxuICAgIC8vIFN0YXJ0IGF0IHJvb3QgaWYgcGF0aCBpcyBhYnNvbHV0ZVxyXG4gICAgaWYgKHBhdGhbMF0gPT09IFwiXCIpXHJcbiAgICAgICAgcmV0dXJuIHRoaXMucm9vdC5sb29rdXAocGF0aC5zbGljZSgxKSwgZmlsdGVyVHlwZXMpO1xyXG5cclxuICAgIC8vIFRlc3QgaWYgdGhlIGZpcnN0IHBhcnQgbWF0Y2hlcyBhbnkgbmVzdGVkIG9iamVjdCwgYW5kIGlmIHNvLCB0cmF2ZXJzZSBpZiBwYXRoIGNvbnRhaW5zIG1vcmVcclxuICAgIHZhciBmb3VuZCA9IHRoaXMuZ2V0KHBhdGhbMF0pO1xyXG4gICAgaWYgKGZvdW5kKSB7XHJcbiAgICAgICAgaWYgKHBhdGgubGVuZ3RoID09PSAxKSB7XHJcbiAgICAgICAgICAgIGlmICghZmlsdGVyVHlwZXMgfHwgZmlsdGVyVHlwZXMuaW5kZXhPZihmb3VuZC5jb25zdHJ1Y3RvcikgPiAtMSlcclxuICAgICAgICAgICAgICAgIHJldHVybiBmb3VuZDtcclxuICAgICAgICB9IGVsc2UgaWYgKGZvdW5kIGluc3RhbmNlb2YgTmFtZXNwYWNlICYmIChmb3VuZCA9IGZvdW5kLmxvb2t1cChwYXRoLnNsaWNlKDEpLCBmaWx0ZXJUeXBlcywgdHJ1ZSkpKVxyXG4gICAgICAgICAgICByZXR1cm4gZm91bmQ7XHJcblxyXG4gICAgLy8gT3RoZXJ3aXNlIHRyeSBlYWNoIG5lc3RlZCBuYW1lc3BhY2VcclxuICAgIH0gZWxzZVxyXG4gICAgICAgIGZvciAodmFyIGkgPSAwOyBpIDwgdGhpcy5uZXN0ZWRBcnJheS5sZW5ndGg7ICsraSlcclxuICAgICAgICAgICAgaWYgKHRoaXMuX25lc3RlZEFycmF5W2ldIGluc3RhbmNlb2YgTmFtZXNwYWNlICYmIChmb3VuZCA9IHRoaXMuX25lc3RlZEFycmF5W2ldLmxvb2t1cChwYXRoLCBmaWx0ZXJUeXBlcywgdHJ1ZSkpKVxyXG4gICAgICAgICAgICAgICAgcmV0dXJuIGZvdW5kO1xyXG5cclxuICAgIC8vIElmIHRoZXJlIGhhc24ndCBiZWVuIGEgbWF0Y2gsIHRyeSBhZ2FpbiBhdCB0aGUgcGFyZW50XHJcbiAgICBpZiAodGhpcy5wYXJlbnQgPT09IG51bGwgfHwgcGFyZW50QWxyZWFkeUNoZWNrZWQpXHJcbiAgICAgICAgcmV0dXJuIG51bGw7XHJcbiAgICByZXR1cm4gdGhpcy5wYXJlbnQubG9va3VwKHBhdGgsIGZpbHRlclR5cGVzKTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBMb29rcyB1cCB0aGUgcmVmbGVjdGlvbiBvYmplY3QgYXQgdGhlIHNwZWNpZmllZCBwYXRoLCByZWxhdGl2ZSB0byB0aGlzIG5hbWVzcGFjZS5cclxuICogQG5hbWUgTmFtZXNwYWNlQmFzZSNsb29rdXBcclxuICogQGZ1bmN0aW9uXHJcbiAqIEBwYXJhbSB7c3RyaW5nfHN0cmluZ1tdfSBwYXRoIFBhdGggdG8gbG9vayB1cFxyXG4gKiBAcGFyYW0ge2Jvb2xlYW59IFtwYXJlbnRBbHJlYWR5Q2hlY2tlZD1mYWxzZV0gV2hldGhlciB0aGUgcGFyZW50IGhhcyBhbHJlYWR5IGJlZW4gY2hlY2tlZFxyXG4gKiBAcmV0dXJucyB7UmVmbGVjdGlvbk9iamVjdHxudWxsfSBMb29rZWQgdXAgb2JqZWN0IG9yIGBudWxsYCBpZiBub25lIGNvdWxkIGJlIGZvdW5kXHJcbiAqIEB2YXJpYXRpb24gMlxyXG4gKi9cclxuLy8gbG9va3VwKHBhdGg6IHN0cmluZywgW3BhcmVudEFscmVhZHlDaGVja2VkOiBib29sZWFuXSlcclxuXHJcbi8qKlxyXG4gKiBMb29rcyB1cCB0aGUge0BsaW5rIFR5cGV8dHlwZX0gYXQgdGhlIHNwZWNpZmllZCBwYXRoLCByZWxhdGl2ZSB0byB0aGlzIG5hbWVzcGFjZS5cclxuICogQmVzaWRlcyBpdHMgc2lnbmF0dXJlLCB0aGlzIG1ldGhvZHMgZGlmZmVycyBmcm9tIHtAbGluayBOYW1lc3BhY2UjbG9va3VwfGxvb2t1cH0gaW4gdGhhdCBpdCB0aHJvd3MgaW5zdGVhZCBvZiByZXR1cm5pbmcgYG51bGxgLlxyXG4gKiBAcGFyYW0ge3N0cmluZ3xzdHJpbmdbXX0gcGF0aCBQYXRoIHRvIGxvb2sgdXBcclxuICogQHJldHVybnMge1R5cGV9IExvb2tlZCB1cCB0eXBlXHJcbiAqIEB0aHJvd3Mge0Vycm9yfSBJZiBgcGF0aGAgZG9lcyBub3QgcG9pbnQgdG8gYSB0eXBlXHJcbiAqL1xyXG5OYW1lc3BhY2UucHJvdG90eXBlLmxvb2t1cFR5cGUgPSBmdW5jdGlvbiBsb29rdXBUeXBlKHBhdGgpIHtcclxuICAgIHZhciBmb3VuZCA9IHRoaXMubG9va3VwKHBhdGgsIFsgVHlwZSBdKTtcclxuICAgIGlmICghZm91bmQpXHJcbiAgICAgICAgdGhyb3cgRXJyb3IoXCJubyBzdWNoIHR5cGU6IFwiICsgcGF0aCk7XHJcbiAgICByZXR1cm4gZm91bmQ7XHJcbn07XHJcblxyXG4vKipcclxuICogTG9va3MgdXAgdGhlIHZhbHVlcyBvZiB0aGUge0BsaW5rIEVudW18ZW51bX0gYXQgdGhlIHNwZWNpZmllZCBwYXRoLCByZWxhdGl2ZSB0byB0aGlzIG5hbWVzcGFjZS5cclxuICogQmVzaWRlcyBpdHMgc2lnbmF0dXJlLCB0aGlzIG1ldGhvZHMgZGlmZmVycyBmcm9tIHtAbGluayBOYW1lc3BhY2UjbG9va3VwfGxvb2t1cH0gaW4gdGhhdCBpdCB0aHJvd3MgaW5zdGVhZCBvZiByZXR1cm5pbmcgYG51bGxgLlxyXG4gKiBAcGFyYW0ge3N0cmluZ3xzdHJpbmdbXX0gcGF0aCBQYXRoIHRvIGxvb2sgdXBcclxuICogQHJldHVybnMge0VudW19IExvb2tlZCB1cCBlbnVtXHJcbiAqIEB0aHJvd3Mge0Vycm9yfSBJZiBgcGF0aGAgZG9lcyBub3QgcG9pbnQgdG8gYW4gZW51bVxyXG4gKi9cclxuTmFtZXNwYWNlLnByb3RvdHlwZS5sb29rdXBFbnVtID0gZnVuY3Rpb24gbG9va3VwRW51bShwYXRoKSB7XHJcbiAgICB2YXIgZm91bmQgPSB0aGlzLmxvb2t1cChwYXRoLCBbIEVudW0gXSk7XHJcbiAgICBpZiAoIWZvdW5kKVxyXG4gICAgICAgIHRocm93IEVycm9yKFwibm8gc3VjaCBFbnVtICdcIiArIHBhdGggKyBcIicgaW4gXCIgKyB0aGlzKTtcclxuICAgIHJldHVybiBmb3VuZDtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBMb29rcyB1cCB0aGUge0BsaW5rIFR5cGV8dHlwZX0gb3Ige0BsaW5rIEVudW18ZW51bX0gYXQgdGhlIHNwZWNpZmllZCBwYXRoLCByZWxhdGl2ZSB0byB0aGlzIG5hbWVzcGFjZS5cclxuICogQmVzaWRlcyBpdHMgc2lnbmF0dXJlLCB0aGlzIG1ldGhvZHMgZGlmZmVycyBmcm9tIHtAbGluayBOYW1lc3BhY2UjbG9va3VwfGxvb2t1cH0gaW4gdGhhdCBpdCB0aHJvd3MgaW5zdGVhZCBvZiByZXR1cm5pbmcgYG51bGxgLlxyXG4gKiBAcGFyYW0ge3N0cmluZ3xzdHJpbmdbXX0gcGF0aCBQYXRoIHRvIGxvb2sgdXBcclxuICogQHJldHVybnMge1R5cGV9IExvb2tlZCB1cCB0eXBlIG9yIGVudW1cclxuICogQHRocm93cyB7RXJyb3J9IElmIGBwYXRoYCBkb2VzIG5vdCBwb2ludCB0byBhIHR5cGUgb3IgZW51bVxyXG4gKi9cclxuTmFtZXNwYWNlLnByb3RvdHlwZS5sb29rdXBUeXBlT3JFbnVtID0gZnVuY3Rpb24gbG9va3VwVHlwZU9yRW51bShwYXRoKSB7XHJcbiAgICB2YXIgZm91bmQgPSB0aGlzLmxvb2t1cChwYXRoLCBbIFR5cGUsIEVudW0gXSk7XHJcbiAgICBpZiAoIWZvdW5kKVxyXG4gICAgICAgIHRocm93IEVycm9yKFwibm8gc3VjaCBUeXBlIG9yIEVudW0gJ1wiICsgcGF0aCArIFwiJyBpbiBcIiArIHRoaXMpO1xyXG4gICAgcmV0dXJuIGZvdW5kO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIExvb2tzIHVwIHRoZSB7QGxpbmsgU2VydmljZXxzZXJ2aWNlfSBhdCB0aGUgc3BlY2lmaWVkIHBhdGgsIHJlbGF0aXZlIHRvIHRoaXMgbmFtZXNwYWNlLlxyXG4gKiBCZXNpZGVzIGl0cyBzaWduYXR1cmUsIHRoaXMgbWV0aG9kcyBkaWZmZXJzIGZyb20ge0BsaW5rIE5hbWVzcGFjZSNsb29rdXB8bG9va3VwfSBpbiB0aGF0IGl0IHRocm93cyBpbnN0ZWFkIG9mIHJldHVybmluZyBgbnVsbGAuXHJcbiAqIEBwYXJhbSB7c3RyaW5nfHN0cmluZ1tdfSBwYXRoIFBhdGggdG8gbG9vayB1cFxyXG4gKiBAcmV0dXJucyB7U2VydmljZX0gTG9va2VkIHVwIHNlcnZpY2VcclxuICogQHRocm93cyB7RXJyb3J9IElmIGBwYXRoYCBkb2VzIG5vdCBwb2ludCB0byBhIHNlcnZpY2VcclxuICovXHJcbk5hbWVzcGFjZS5wcm90b3R5cGUubG9va3VwU2VydmljZSA9IGZ1bmN0aW9uIGxvb2t1cFNlcnZpY2UocGF0aCkge1xyXG4gICAgdmFyIGZvdW5kID0gdGhpcy5sb29rdXAocGF0aCwgWyBTZXJ2aWNlIF0pO1xyXG4gICAgaWYgKCFmb3VuZClcclxuICAgICAgICB0aHJvdyBFcnJvcihcIm5vIHN1Y2ggU2VydmljZSAnXCIgKyBwYXRoICsgXCInIGluIFwiICsgdGhpcyk7XHJcbiAgICByZXR1cm4gZm91bmQ7XHJcbn07XHJcblxyXG4vLyBTZXRzIHVwIGN5Y2xpYyBkZXBlbmRlbmNpZXMgKGNhbGxlZCBpbiBpbmRleC1saWdodClcclxuTmFtZXNwYWNlLl9jb25maWd1cmUgPSBmdW5jdGlvbihUeXBlXywgU2VydmljZV8sIEVudW1fKSB7XHJcbiAgICBUeXBlICAgID0gVHlwZV87XHJcbiAgICBTZXJ2aWNlID0gU2VydmljZV87XHJcbiAgICBFbnVtICAgID0gRW51bV87XHJcbn07XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG5tb2R1bGUuZXhwb3J0cyA9IFJlZmxlY3Rpb25PYmplY3Q7XHJcblxyXG5SZWZsZWN0aW9uT2JqZWN0LmNsYXNzTmFtZSA9IFwiUmVmbGVjdGlvbk9iamVjdFwiO1xyXG5cclxudmFyIHV0aWwgPSByZXF1aXJlKFwiLi91dGlsXCIpO1xyXG5cclxudmFyIFJvb3Q7IC8vIGN5Y2xpY1xyXG5cclxuLyoqXHJcbiAqIENvbnN0cnVjdHMgYSBuZXcgcmVmbGVjdGlvbiBvYmplY3QgaW5zdGFuY2UuXHJcbiAqIEBjbGFzc2Rlc2MgQmFzZSBjbGFzcyBvZiBhbGwgcmVmbGVjdGlvbiBvYmplY3RzLlxyXG4gKiBAY29uc3RydWN0b3JcclxuICogQHBhcmFtIHtzdHJpbmd9IG5hbWUgT2JqZWN0IG5hbWVcclxuICogQHBhcmFtIHtPYmplY3QuPHN0cmluZywqPn0gW29wdGlvbnNdIERlY2xhcmVkIG9wdGlvbnNcclxuICogQGFic3RyYWN0XHJcbiAqL1xyXG5mdW5jdGlvbiBSZWZsZWN0aW9uT2JqZWN0KG5hbWUsIG9wdGlvbnMpIHtcclxuXHJcbiAgICBpZiAoIXV0aWwuaXNTdHJpbmcobmFtZSkpXHJcbiAgICAgICAgdGhyb3cgVHlwZUVycm9yKFwibmFtZSBtdXN0IGJlIGEgc3RyaW5nXCIpO1xyXG5cclxuICAgIGlmIChvcHRpb25zICYmICF1dGlsLmlzT2JqZWN0KG9wdGlvbnMpKVxyXG4gICAgICAgIHRocm93IFR5cGVFcnJvcihcIm9wdGlvbnMgbXVzdCBiZSBhbiBvYmplY3RcIik7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBPcHRpb25zLlxyXG4gICAgICogQHR5cGUge09iamVjdC48c3RyaW5nLCo+fHVuZGVmaW5lZH1cclxuICAgICAqL1xyXG4gICAgdGhpcy5vcHRpb25zID0gb3B0aW9uczsgLy8gdG9KU09OXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBQYXJzZWQgT3B0aW9ucy5cclxuICAgICAqIEB0eXBlIHtBcnJheS48T2JqZWN0LjxzdHJpbmcsKj4+fHVuZGVmaW5lZH1cclxuICAgICAqL1xyXG4gICAgdGhpcy5wYXJzZWRPcHRpb25zID0gbnVsbDtcclxuXHJcbiAgICAvKipcclxuICAgICAqIFVuaXF1ZSBuYW1lIHdpdGhpbiBpdHMgbmFtZXNwYWNlLlxyXG4gICAgICogQHR5cGUge3N0cmluZ31cclxuICAgICAqL1xyXG4gICAgdGhpcy5uYW1lID0gbmFtZTtcclxuXHJcbiAgICAvKipcclxuICAgICAqIFBhcmVudCBuYW1lc3BhY2UuXHJcbiAgICAgKiBAdHlwZSB7TmFtZXNwYWNlfG51bGx9XHJcbiAgICAgKi9cclxuICAgIHRoaXMucGFyZW50ID0gbnVsbDtcclxuXHJcbiAgICAvKipcclxuICAgICAqIFdoZXRoZXIgYWxyZWFkeSByZXNvbHZlZCBvciBub3QuXHJcbiAgICAgKiBAdHlwZSB7Ym9vbGVhbn1cclxuICAgICAqL1xyXG4gICAgdGhpcy5yZXNvbHZlZCA9IGZhbHNlO1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogQ29tbWVudCB0ZXh0LCBpZiBhbnkuXHJcbiAgICAgKiBAdHlwZSB7c3RyaW5nfG51bGx9XHJcbiAgICAgKi9cclxuICAgIHRoaXMuY29tbWVudCA9IG51bGw7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBEZWZpbmluZyBmaWxlIG5hbWUuXHJcbiAgICAgKiBAdHlwZSB7c3RyaW5nfG51bGx9XHJcbiAgICAgKi9cclxuICAgIHRoaXMuZmlsZW5hbWUgPSBudWxsO1xyXG59XHJcblxyXG5PYmplY3QuZGVmaW5lUHJvcGVydGllcyhSZWZsZWN0aW9uT2JqZWN0LnByb3RvdHlwZSwge1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmVmZXJlbmNlIHRvIHRoZSByb290IG5hbWVzcGFjZS5cclxuICAgICAqIEBuYW1lIFJlZmxlY3Rpb25PYmplY3Qjcm9vdFxyXG4gICAgICogQHR5cGUge1Jvb3R9XHJcbiAgICAgKiBAcmVhZG9ubHlcclxuICAgICAqL1xyXG4gICAgcm9vdDoge1xyXG4gICAgICAgIGdldDogZnVuY3Rpb24oKSB7XHJcbiAgICAgICAgICAgIHZhciBwdHIgPSB0aGlzO1xyXG4gICAgICAgICAgICB3aGlsZSAocHRyLnBhcmVudCAhPT0gbnVsbClcclxuICAgICAgICAgICAgICAgIHB0ciA9IHB0ci5wYXJlbnQ7XHJcbiAgICAgICAgICAgIHJldHVybiBwdHI7XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIEZ1bGwgbmFtZSBpbmNsdWRpbmcgbGVhZGluZyBkb3QuXHJcbiAgICAgKiBAbmFtZSBSZWZsZWN0aW9uT2JqZWN0I2Z1bGxOYW1lXHJcbiAgICAgKiBAdHlwZSB7c3RyaW5nfVxyXG4gICAgICogQHJlYWRvbmx5XHJcbiAgICAgKi9cclxuICAgIGZ1bGxOYW1lOiB7XHJcbiAgICAgICAgZ2V0OiBmdW5jdGlvbigpIHtcclxuICAgICAgICAgICAgdmFyIHBhdGggPSBbIHRoaXMubmFtZSBdLFxyXG4gICAgICAgICAgICAgICAgcHRyID0gdGhpcy5wYXJlbnQ7XHJcbiAgICAgICAgICAgIHdoaWxlIChwdHIpIHtcclxuICAgICAgICAgICAgICAgIHBhdGgudW5zaGlmdChwdHIubmFtZSk7XHJcbiAgICAgICAgICAgICAgICBwdHIgPSBwdHIucGFyZW50O1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIHJldHVybiBwYXRoLmpvaW4oXCIuXCIpO1xyXG4gICAgICAgIH1cclxuICAgIH1cclxufSk7XHJcblxyXG4vKipcclxuICogQ29udmVydHMgdGhpcyByZWZsZWN0aW9uIG9iamVjdCB0byBpdHMgZGVzY3JpcHRvciByZXByZXNlbnRhdGlvbi5cclxuICogQHJldHVybnMge09iamVjdC48c3RyaW5nLCo+fSBEZXNjcmlwdG9yXHJcbiAqIEBhYnN0cmFjdFxyXG4gKi9cclxuUmVmbGVjdGlvbk9iamVjdC5wcm90b3R5cGUudG9KU09OID0gLyogaXN0YW5idWwgaWdub3JlIG5leHQgKi8gZnVuY3Rpb24gdG9KU09OKCkge1xyXG4gICAgdGhyb3cgRXJyb3IoKTsgLy8gbm90IGltcGxlbWVudGVkLCBzaG91bGRuJ3QgaGFwcGVuXHJcbn07XHJcblxyXG4vKipcclxuICogQ2FsbGVkIHdoZW4gdGhpcyBvYmplY3QgaXMgYWRkZWQgdG8gYSBwYXJlbnQuXHJcbiAqIEBwYXJhbSB7UmVmbGVjdGlvbk9iamVjdH0gcGFyZW50IFBhcmVudCBhZGRlZCB0b1xyXG4gKiBAcmV0dXJucyB7dW5kZWZpbmVkfVxyXG4gKi9cclxuUmVmbGVjdGlvbk9iamVjdC5wcm90b3R5cGUub25BZGQgPSBmdW5jdGlvbiBvbkFkZChwYXJlbnQpIHtcclxuICAgIGlmICh0aGlzLnBhcmVudCAmJiB0aGlzLnBhcmVudCAhPT0gcGFyZW50KVxyXG4gICAgICAgIHRoaXMucGFyZW50LnJlbW92ZSh0aGlzKTtcclxuICAgIHRoaXMucGFyZW50ID0gcGFyZW50O1xyXG4gICAgdGhpcy5yZXNvbHZlZCA9IGZhbHNlO1xyXG4gICAgdmFyIHJvb3QgPSBwYXJlbnQucm9vdDtcclxuICAgIGlmIChyb290IGluc3RhbmNlb2YgUm9vdClcclxuICAgICAgICByb290Ll9oYW5kbGVBZGQodGhpcyk7XHJcbn07XHJcblxyXG4vKipcclxuICogQ2FsbGVkIHdoZW4gdGhpcyBvYmplY3QgaXMgcmVtb3ZlZCBmcm9tIGEgcGFyZW50LlxyXG4gKiBAcGFyYW0ge1JlZmxlY3Rpb25PYmplY3R9IHBhcmVudCBQYXJlbnQgcmVtb3ZlZCBmcm9tXHJcbiAqIEByZXR1cm5zIHt1bmRlZmluZWR9XHJcbiAqL1xyXG5SZWZsZWN0aW9uT2JqZWN0LnByb3RvdHlwZS5vblJlbW92ZSA9IGZ1bmN0aW9uIG9uUmVtb3ZlKHBhcmVudCkge1xyXG4gICAgdmFyIHJvb3QgPSBwYXJlbnQucm9vdDtcclxuICAgIGlmIChyb290IGluc3RhbmNlb2YgUm9vdClcclxuICAgICAgICByb290Ll9oYW5kbGVSZW1vdmUodGhpcyk7XHJcbiAgICB0aGlzLnBhcmVudCA9IG51bGw7XHJcbiAgICB0aGlzLnJlc29sdmVkID0gZmFsc2U7XHJcbn07XHJcblxyXG4vKipcclxuICogUmVzb2x2ZXMgdGhpcyBvYmplY3RzIHR5cGUgcmVmZXJlbmNlcy5cclxuICogQHJldHVybnMge1JlZmxlY3Rpb25PYmplY3R9IGB0aGlzYFxyXG4gKi9cclxuUmVmbGVjdGlvbk9iamVjdC5wcm90b3R5cGUucmVzb2x2ZSA9IGZ1bmN0aW9uIHJlc29sdmUoKSB7XHJcbiAgICBpZiAodGhpcy5yZXNvbHZlZClcclxuICAgICAgICByZXR1cm4gdGhpcztcclxuICAgIGlmICh0aGlzLnJvb3QgaW5zdGFuY2VvZiBSb290KVxyXG4gICAgICAgIHRoaXMucmVzb2x2ZWQgPSB0cnVlOyAvLyBvbmx5IGlmIHBhcnQgb2YgYSByb290XHJcbiAgICByZXR1cm4gdGhpcztcclxufTtcclxuXHJcbi8qKlxyXG4gKiBHZXRzIGFuIG9wdGlvbiB2YWx1ZS5cclxuICogQHBhcmFtIHtzdHJpbmd9IG5hbWUgT3B0aW9uIG5hbWVcclxuICogQHJldHVybnMgeyp9IE9wdGlvbiB2YWx1ZSBvciBgdW5kZWZpbmVkYCBpZiBub3Qgc2V0XHJcbiAqL1xyXG5SZWZsZWN0aW9uT2JqZWN0LnByb3RvdHlwZS5nZXRPcHRpb24gPSBmdW5jdGlvbiBnZXRPcHRpb24obmFtZSkge1xyXG4gICAgaWYgKHRoaXMub3B0aW9ucylcclxuICAgICAgICByZXR1cm4gdGhpcy5vcHRpb25zW25hbWVdO1xyXG4gICAgcmV0dXJuIHVuZGVmaW5lZDtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBTZXRzIGFuIG9wdGlvbi5cclxuICogQHBhcmFtIHtzdHJpbmd9IG5hbWUgT3B0aW9uIG5hbWVcclxuICogQHBhcmFtIHsqfSB2YWx1ZSBPcHRpb24gdmFsdWVcclxuICogQHBhcmFtIHtib29sZWFufSBbaWZOb3RTZXRdIFNldHMgdGhlIG9wdGlvbiBvbmx5IGlmIGl0IGlzbid0IGN1cnJlbnRseSBzZXRcclxuICogQHJldHVybnMge1JlZmxlY3Rpb25PYmplY3R9IGB0aGlzYFxyXG4gKi9cclxuUmVmbGVjdGlvbk9iamVjdC5wcm90b3R5cGUuc2V0T3B0aW9uID0gZnVuY3Rpb24gc2V0T3B0aW9uKG5hbWUsIHZhbHVlLCBpZk5vdFNldCkge1xyXG4gICAgaWYgKCFpZk5vdFNldCB8fCAhdGhpcy5vcHRpb25zIHx8IHRoaXMub3B0aW9uc1tuYW1lXSA9PT0gdW5kZWZpbmVkKVxyXG4gICAgICAgICh0aGlzLm9wdGlvbnMgfHwgKHRoaXMub3B0aW9ucyA9IHt9KSlbbmFtZV0gPSB2YWx1ZTtcclxuICAgIHJldHVybiB0aGlzO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIFNldHMgYSBwYXJzZWQgb3B0aW9uLlxyXG4gKiBAcGFyYW0ge3N0cmluZ30gbmFtZSBwYXJzZWQgT3B0aW9uIG5hbWVcclxuICogQHBhcmFtIHsqfSB2YWx1ZSBPcHRpb24gdmFsdWVcclxuICogQHBhcmFtIHtzdHJpbmd9IHByb3BOYW1lIGRvdCAnLicgZGVsaW1pdGVkIGZ1bGwgcGF0aCBvZiBwcm9wZXJ0eSB3aXRoaW4gdGhlIG9wdGlvbiB0byBzZXQuIGlmIHVuZGVmaW5lZFxcZW1wdHksIHdpbGwgYWRkIGEgbmV3IG9wdGlvbiB3aXRoIHRoYXQgdmFsdWVcclxuICogQHJldHVybnMge1JlZmxlY3Rpb25PYmplY3R9IGB0aGlzYFxyXG4gKi9cclxuUmVmbGVjdGlvbk9iamVjdC5wcm90b3R5cGUuc2V0UGFyc2VkT3B0aW9uID0gZnVuY3Rpb24gc2V0UGFyc2VkT3B0aW9uKG5hbWUsIHZhbHVlLCBwcm9wTmFtZSkge1xyXG4gICAgaWYgKCF0aGlzLnBhcnNlZE9wdGlvbnMpIHtcclxuICAgICAgICB0aGlzLnBhcnNlZE9wdGlvbnMgPSBbXTtcclxuICAgIH1cclxuICAgIHZhciBwYXJzZWRPcHRpb25zID0gdGhpcy5wYXJzZWRPcHRpb25zO1xyXG4gICAgaWYgKHByb3BOYW1lKSB7XHJcbiAgICAgICAgLy8gSWYgc2V0dGluZyBhIHN1YiBwcm9wZXJ0eSBvZiBhbiBvcHRpb24gdGhlbiB0cnkgdG8gbWVyZ2UgaXRcclxuICAgICAgICAvLyB3aXRoIGFuIGV4aXN0aW5nIG9wdGlvblxyXG4gICAgICAgIHZhciBvcHQgPSBwYXJzZWRPcHRpb25zLmZpbmQoZnVuY3Rpb24gKG9wdCkge1xyXG4gICAgICAgICAgICByZXR1cm4gT2JqZWN0LnByb3RvdHlwZS5oYXNPd25Qcm9wZXJ0eS5jYWxsKG9wdCwgbmFtZSk7XHJcbiAgICAgICAgfSk7XHJcbiAgICAgICAgaWYgKG9wdCkge1xyXG4gICAgICAgICAgICAvLyBJZiB3ZSBmb3VuZCBhbiBleGlzdGluZyBvcHRpb24gLSBqdXN0IG1lcmdlIHRoZSBwcm9wZXJ0eSB2YWx1ZVxyXG4gICAgICAgICAgICB2YXIgbmV3VmFsdWUgPSBvcHRbbmFtZV07XHJcbiAgICAgICAgICAgIHV0aWwuc2V0UHJvcGVydHkobmV3VmFsdWUsIHByb3BOYW1lLCB2YWx1ZSk7XHJcbiAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgLy8gb3RoZXJ3aXNlLCBjcmVhdGUgYSBuZXcgb3B0aW9uLCBzZXQgaXQncyBwcm9wZXJ0eSBhbmQgYWRkIGl0IHRvIHRoZSBsaXN0XHJcbiAgICAgICAgICAgIG9wdCA9IHt9O1xyXG4gICAgICAgICAgICBvcHRbbmFtZV0gPSB1dGlsLnNldFByb3BlcnR5KHt9LCBwcm9wTmFtZSwgdmFsdWUpO1xyXG4gICAgICAgICAgICBwYXJzZWRPcHRpb25zLnB1c2gob3B0KTtcclxuICAgICAgICB9XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICAgIC8vIEFsd2F5cyBjcmVhdGUgYSBuZXcgb3B0aW9uIHdoZW4gc2V0dGluZyB0aGUgdmFsdWUgb2YgdGhlIG9wdGlvbiBpdHNlbGZcclxuICAgICAgICB2YXIgbmV3T3B0ID0ge307XHJcbiAgICAgICAgbmV3T3B0W25hbWVdID0gdmFsdWU7XHJcbiAgICAgICAgcGFyc2VkT3B0aW9ucy5wdXNoKG5ld09wdCk7XHJcbiAgICB9XHJcbiAgICByZXR1cm4gdGhpcztcclxufTtcclxuXHJcbi8qKlxyXG4gKiBTZXRzIG11bHRpcGxlIG9wdGlvbnMuXHJcbiAqIEBwYXJhbSB7T2JqZWN0LjxzdHJpbmcsKj59IG9wdGlvbnMgT3B0aW9ucyB0byBzZXRcclxuICogQHBhcmFtIHtib29sZWFufSBbaWZOb3RTZXRdIFNldHMgYW4gb3B0aW9uIG9ubHkgaWYgaXQgaXNuJ3QgY3VycmVudGx5IHNldFxyXG4gKiBAcmV0dXJucyB7UmVmbGVjdGlvbk9iamVjdH0gYHRoaXNgXHJcbiAqL1xyXG5SZWZsZWN0aW9uT2JqZWN0LnByb3RvdHlwZS5zZXRPcHRpb25zID0gZnVuY3Rpb24gc2V0T3B0aW9ucyhvcHRpb25zLCBpZk5vdFNldCkge1xyXG4gICAgaWYgKG9wdGlvbnMpXHJcbiAgICAgICAgZm9yICh2YXIga2V5cyA9IE9iamVjdC5rZXlzKG9wdGlvbnMpLCBpID0gMDsgaSA8IGtleXMubGVuZ3RoOyArK2kpXHJcbiAgICAgICAgICAgIHRoaXMuc2V0T3B0aW9uKGtleXNbaV0sIG9wdGlvbnNba2V5c1tpXV0sIGlmTm90U2V0KTtcclxuICAgIHJldHVybiB0aGlzO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIENvbnZlcnRzIHRoaXMgaW5zdGFuY2UgdG8gaXRzIHN0cmluZyByZXByZXNlbnRhdGlvbi5cclxuICogQHJldHVybnMge3N0cmluZ30gQ2xhc3MgbmFtZVssIHNwYWNlLCBmdWxsIG5hbWVdXHJcbiAqL1xyXG5SZWZsZWN0aW9uT2JqZWN0LnByb3RvdHlwZS50b1N0cmluZyA9IGZ1bmN0aW9uIHRvU3RyaW5nKCkge1xyXG4gICAgdmFyIGNsYXNzTmFtZSA9IHRoaXMuY29uc3RydWN0b3IuY2xhc3NOYW1lLFxyXG4gICAgICAgIGZ1bGxOYW1lICA9IHRoaXMuZnVsbE5hbWU7XHJcbiAgICBpZiAoZnVsbE5hbWUubGVuZ3RoKVxyXG4gICAgICAgIHJldHVybiBjbGFzc05hbWUgKyBcIiBcIiArIGZ1bGxOYW1lO1xyXG4gICAgcmV0dXJuIGNsYXNzTmFtZTtcclxufTtcclxuXHJcbi8vIFNldHMgdXAgY3ljbGljIGRlcGVuZGVuY2llcyAoY2FsbGVkIGluIGluZGV4LWxpZ2h0KVxyXG5SZWZsZWN0aW9uT2JqZWN0Ll9jb25maWd1cmUgPSBmdW5jdGlvbihSb290Xykge1xyXG4gICAgUm9vdCA9IFJvb3RfO1xyXG59O1xyXG4iLCJcInVzZSBzdHJpY3RcIjtcclxubW9kdWxlLmV4cG9ydHMgPSBPbmVPZjtcclxuXHJcbi8vIGV4dGVuZHMgUmVmbGVjdGlvbk9iamVjdFxyXG52YXIgUmVmbGVjdGlvbk9iamVjdCA9IHJlcXVpcmUoXCIuL29iamVjdFwiKTtcclxuKChPbmVPZi5wcm90b3R5cGUgPSBPYmplY3QuY3JlYXRlKFJlZmxlY3Rpb25PYmplY3QucHJvdG90eXBlKSkuY29uc3RydWN0b3IgPSBPbmVPZikuY2xhc3NOYW1lID0gXCJPbmVPZlwiO1xyXG5cclxudmFyIEZpZWxkID0gcmVxdWlyZShcIi4vZmllbGRcIiksXHJcbiAgICB1dGlsICA9IHJlcXVpcmUoXCIuL3V0aWxcIik7XHJcblxyXG4vKipcclxuICogQ29uc3RydWN0cyBhIG5ldyBvbmVvZiBpbnN0YW5jZS5cclxuICogQGNsYXNzZGVzYyBSZWZsZWN0ZWQgb25lb2YuXHJcbiAqIEBleHRlbmRzIFJlZmxlY3Rpb25PYmplY3RcclxuICogQGNvbnN0cnVjdG9yXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBuYW1lIE9uZW9mIG5hbWVcclxuICogQHBhcmFtIHtzdHJpbmdbXXxPYmplY3QuPHN0cmluZywqPn0gW2ZpZWxkTmFtZXNdIEZpZWxkIG5hbWVzXHJcbiAqIEBwYXJhbSB7T2JqZWN0LjxzdHJpbmcsKj59IFtvcHRpb25zXSBEZWNsYXJlZCBvcHRpb25zXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBbY29tbWVudF0gQ29tbWVudCBhc3NvY2lhdGVkIHdpdGggdGhpcyBmaWVsZFxyXG4gKi9cclxuZnVuY3Rpb24gT25lT2YobmFtZSwgZmllbGROYW1lcywgb3B0aW9ucywgY29tbWVudCkge1xyXG4gICAgaWYgKCFBcnJheS5pc0FycmF5KGZpZWxkTmFtZXMpKSB7XHJcbiAgICAgICAgb3B0aW9ucyA9IGZpZWxkTmFtZXM7XHJcbiAgICAgICAgZmllbGROYW1lcyA9IHVuZGVmaW5lZDtcclxuICAgIH1cclxuICAgIFJlZmxlY3Rpb25PYmplY3QuY2FsbCh0aGlzLCBuYW1lLCBvcHRpb25zKTtcclxuXHJcbiAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgaWYgKi9cclxuICAgIGlmICghKGZpZWxkTmFtZXMgPT09IHVuZGVmaW5lZCB8fCBBcnJheS5pc0FycmF5KGZpZWxkTmFtZXMpKSlcclxuICAgICAgICB0aHJvdyBUeXBlRXJyb3IoXCJmaWVsZE5hbWVzIG11c3QgYmUgYW4gQXJyYXlcIik7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBGaWVsZCBuYW1lcyB0aGF0IGJlbG9uZyB0byB0aGlzIG9uZW9mLlxyXG4gICAgICogQHR5cGUge3N0cmluZ1tdfVxyXG4gICAgICovXHJcbiAgICB0aGlzLm9uZW9mID0gZmllbGROYW1lcyB8fCBbXTsgLy8gdG9KU09OLCBtYXJrZXJcclxuXHJcbiAgICAvKipcclxuICAgICAqIEZpZWxkcyB0aGF0IGJlbG9uZyB0byB0aGlzIG9uZW9mIGFzIGFuIGFycmF5IGZvciBpdGVyYXRpb24uXHJcbiAgICAgKiBAdHlwZSB7RmllbGRbXX1cclxuICAgICAqIEByZWFkb25seVxyXG4gICAgICovXHJcbiAgICB0aGlzLmZpZWxkc0FycmF5ID0gW107IC8vIGRlY2xhcmVkIHJlYWRvbmx5IGZvciBjb25mb3JtYW5jZSwgcG9zc2libHkgbm90IHlldCBhZGRlZCB0byBwYXJlbnRcclxuXHJcbiAgICAvKipcclxuICAgICAqIENvbW1lbnQgZm9yIHRoaXMgZmllbGQuXHJcbiAgICAgKiBAdHlwZSB7c3RyaW5nfG51bGx9XHJcbiAgICAgKi9cclxuICAgIHRoaXMuY29tbWVudCA9IGNvbW1lbnQ7XHJcbn1cclxuXHJcbi8qKlxyXG4gKiBPbmVvZiBkZXNjcmlwdG9yLlxyXG4gKiBAaW50ZXJmYWNlIElPbmVPZlxyXG4gKiBAcHJvcGVydHkge0FycmF5LjxzdHJpbmc+fSBvbmVvZiBPbmVvZiBmaWVsZCBuYW1lc1xyXG4gKiBAcHJvcGVydHkge09iamVjdC48c3RyaW5nLCo+fSBbb3B0aW9uc10gT25lb2Ygb3B0aW9uc1xyXG4gKi9cclxuXHJcbi8qKlxyXG4gKiBDb25zdHJ1Y3RzIGEgb25lb2YgZnJvbSBhIG9uZW9mIGRlc2NyaXB0b3IuXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBuYW1lIE9uZW9mIG5hbWVcclxuICogQHBhcmFtIHtJT25lT2Z9IGpzb24gT25lb2YgZGVzY3JpcHRvclxyXG4gKiBAcmV0dXJucyB7T25lT2Z9IENyZWF0ZWQgb25lb2ZcclxuICogQHRocm93cyB7VHlwZUVycm9yfSBJZiBhcmd1bWVudHMgYXJlIGludmFsaWRcclxuICovXHJcbk9uZU9mLmZyb21KU09OID0gZnVuY3Rpb24gZnJvbUpTT04obmFtZSwganNvbikge1xyXG4gICAgcmV0dXJuIG5ldyBPbmVPZihuYW1lLCBqc29uLm9uZW9mLCBqc29uLm9wdGlvbnMsIGpzb24uY29tbWVudCk7XHJcbn07XHJcblxyXG4vKipcclxuICogQ29udmVydHMgdGhpcyBvbmVvZiB0byBhIG9uZW9mIGRlc2NyaXB0b3IuXHJcbiAqIEBwYXJhbSB7SVRvSlNPTk9wdGlvbnN9IFt0b0pTT05PcHRpb25zXSBKU09OIGNvbnZlcnNpb24gb3B0aW9uc1xyXG4gKiBAcmV0dXJucyB7SU9uZU9mfSBPbmVvZiBkZXNjcmlwdG9yXHJcbiAqL1xyXG5PbmVPZi5wcm90b3R5cGUudG9KU09OID0gZnVuY3Rpb24gdG9KU09OKHRvSlNPTk9wdGlvbnMpIHtcclxuICAgIHZhciBrZWVwQ29tbWVudHMgPSB0b0pTT05PcHRpb25zID8gQm9vbGVhbih0b0pTT05PcHRpb25zLmtlZXBDb21tZW50cykgOiBmYWxzZTtcclxuICAgIHJldHVybiB1dGlsLnRvT2JqZWN0KFtcclxuICAgICAgICBcIm9wdGlvbnNcIiAsIHRoaXMub3B0aW9ucyxcclxuICAgICAgICBcIm9uZW9mXCIgICAsIHRoaXMub25lb2YsXHJcbiAgICAgICAgXCJjb21tZW50XCIgLCBrZWVwQ29tbWVudHMgPyB0aGlzLmNvbW1lbnQgOiB1bmRlZmluZWRcclxuICAgIF0pO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIEFkZHMgdGhlIGZpZWxkcyBvZiB0aGUgc3BlY2lmaWVkIG9uZW9mIHRvIHRoZSBwYXJlbnQgaWYgbm90IGFscmVhZHkgZG9uZSBzby5cclxuICogQHBhcmFtIHtPbmVPZn0gb25lb2YgVGhlIG9uZW9mXHJcbiAqIEByZXR1cm5zIHt1bmRlZmluZWR9XHJcbiAqIEBpbm5lclxyXG4gKiBAaWdub3JlXHJcbiAqL1xyXG5mdW5jdGlvbiBhZGRGaWVsZHNUb1BhcmVudChvbmVvZikge1xyXG4gICAgaWYgKG9uZW9mLnBhcmVudClcclxuICAgICAgICBmb3IgKHZhciBpID0gMDsgaSA8IG9uZW9mLmZpZWxkc0FycmF5Lmxlbmd0aDsgKytpKVxyXG4gICAgICAgICAgICBpZiAoIW9uZW9mLmZpZWxkc0FycmF5W2ldLnBhcmVudClcclxuICAgICAgICAgICAgICAgIG9uZW9mLnBhcmVudC5hZGQob25lb2YuZmllbGRzQXJyYXlbaV0pO1xyXG59XHJcblxyXG4vKipcclxuICogQWRkcyBhIGZpZWxkIHRvIHRoaXMgb25lb2YgYW5kIHJlbW92ZXMgaXQgZnJvbSBpdHMgY3VycmVudCBwYXJlbnQsIGlmIGFueS5cclxuICogQHBhcmFtIHtGaWVsZH0gZmllbGQgRmllbGQgdG8gYWRkXHJcbiAqIEByZXR1cm5zIHtPbmVPZn0gYHRoaXNgXHJcbiAqL1xyXG5PbmVPZi5wcm90b3R5cGUuYWRkID0gZnVuY3Rpb24gYWRkKGZpZWxkKSB7XHJcblxyXG4gICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICBpZiAoIShmaWVsZCBpbnN0YW5jZW9mIEZpZWxkKSlcclxuICAgICAgICB0aHJvdyBUeXBlRXJyb3IoXCJmaWVsZCBtdXN0IGJlIGEgRmllbGRcIik7XHJcblxyXG4gICAgaWYgKGZpZWxkLnBhcmVudCAmJiBmaWVsZC5wYXJlbnQgIT09IHRoaXMucGFyZW50KVxyXG4gICAgICAgIGZpZWxkLnBhcmVudC5yZW1vdmUoZmllbGQpO1xyXG4gICAgdGhpcy5vbmVvZi5wdXNoKGZpZWxkLm5hbWUpO1xyXG4gICAgdGhpcy5maWVsZHNBcnJheS5wdXNoKGZpZWxkKTtcclxuICAgIGZpZWxkLnBhcnRPZiA9IHRoaXM7IC8vIGZpZWxkLnBhcmVudCByZW1haW5zIG51bGxcclxuICAgIGFkZEZpZWxkc1RvUGFyZW50KHRoaXMpO1xyXG4gICAgcmV0dXJuIHRoaXM7XHJcbn07XHJcblxyXG4vKipcclxuICogUmVtb3ZlcyBhIGZpZWxkIGZyb20gdGhpcyBvbmVvZiBhbmQgcHV0cyBpdCBiYWNrIHRvIHRoZSBvbmVvZidzIHBhcmVudC5cclxuICogQHBhcmFtIHtGaWVsZH0gZmllbGQgRmllbGQgdG8gcmVtb3ZlXHJcbiAqIEByZXR1cm5zIHtPbmVPZn0gYHRoaXNgXHJcbiAqL1xyXG5PbmVPZi5wcm90b3R5cGUucmVtb3ZlID0gZnVuY3Rpb24gcmVtb3ZlKGZpZWxkKSB7XHJcblxyXG4gICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICBpZiAoIShmaWVsZCBpbnN0YW5jZW9mIEZpZWxkKSlcclxuICAgICAgICB0aHJvdyBUeXBlRXJyb3IoXCJmaWVsZCBtdXN0IGJlIGEgRmllbGRcIik7XHJcblxyXG4gICAgdmFyIGluZGV4ID0gdGhpcy5maWVsZHNBcnJheS5pbmRleE9mKGZpZWxkKTtcclxuXHJcbiAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgaWYgKi9cclxuICAgIGlmIChpbmRleCA8IDApXHJcbiAgICAgICAgdGhyb3cgRXJyb3IoZmllbGQgKyBcIiBpcyBub3QgYSBtZW1iZXIgb2YgXCIgKyB0aGlzKTtcclxuXHJcbiAgICB0aGlzLmZpZWxkc0FycmF5LnNwbGljZShpbmRleCwgMSk7XHJcbiAgICBpbmRleCA9IHRoaXMub25lb2YuaW5kZXhPZihmaWVsZC5uYW1lKTtcclxuXHJcbiAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgZWxzZSAqL1xyXG4gICAgaWYgKGluZGV4ID4gLTEpIC8vIHRoZW9yZXRpY2FsXHJcbiAgICAgICAgdGhpcy5vbmVvZi5zcGxpY2UoaW5kZXgsIDEpO1xyXG5cclxuICAgIGZpZWxkLnBhcnRPZiA9IG51bGw7XHJcbiAgICByZXR1cm4gdGhpcztcclxufTtcclxuXHJcbi8qKlxyXG4gKiBAb3ZlcnJpZGVcclxuICovXHJcbk9uZU9mLnByb3RvdHlwZS5vbkFkZCA9IGZ1bmN0aW9uIG9uQWRkKHBhcmVudCkge1xyXG4gICAgUmVmbGVjdGlvbk9iamVjdC5wcm90b3R5cGUub25BZGQuY2FsbCh0aGlzLCBwYXJlbnQpO1xyXG4gICAgdmFyIHNlbGYgPSB0aGlzO1xyXG4gICAgLy8gQ29sbGVjdCBwcmVzZW50IGZpZWxkc1xyXG4gICAgZm9yICh2YXIgaSA9IDA7IGkgPCB0aGlzLm9uZW9mLmxlbmd0aDsgKytpKSB7XHJcbiAgICAgICAgdmFyIGZpZWxkID0gcGFyZW50LmdldCh0aGlzLm9uZW9mW2ldKTtcclxuICAgICAgICBpZiAoZmllbGQgJiYgIWZpZWxkLnBhcnRPZikge1xyXG4gICAgICAgICAgICBmaWVsZC5wYXJ0T2YgPSBzZWxmO1xyXG4gICAgICAgICAgICBzZWxmLmZpZWxkc0FycmF5LnB1c2goZmllbGQpO1xyXG4gICAgICAgIH1cclxuICAgIH1cclxuICAgIC8vIEFkZCBub3QgeWV0IHByZXNlbnQgZmllbGRzXHJcbiAgICBhZGRGaWVsZHNUb1BhcmVudCh0aGlzKTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBAb3ZlcnJpZGVcclxuICovXHJcbk9uZU9mLnByb3RvdHlwZS5vblJlbW92ZSA9IGZ1bmN0aW9uIG9uUmVtb3ZlKHBhcmVudCkge1xyXG4gICAgZm9yICh2YXIgaSA9IDAsIGZpZWxkOyBpIDwgdGhpcy5maWVsZHNBcnJheS5sZW5ndGg7ICsraSlcclxuICAgICAgICBpZiAoKGZpZWxkID0gdGhpcy5maWVsZHNBcnJheVtpXSkucGFyZW50KVxyXG4gICAgICAgICAgICBmaWVsZC5wYXJlbnQucmVtb3ZlKGZpZWxkKTtcclxuICAgIFJlZmxlY3Rpb25PYmplY3QucHJvdG90eXBlLm9uUmVtb3ZlLmNhbGwodGhpcywgcGFyZW50KTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBEZWNvcmF0b3IgZnVuY3Rpb24gYXMgcmV0dXJuZWQgYnkge0BsaW5rIE9uZU9mLmR9IChUeXBlU2NyaXB0KS5cclxuICogQHR5cGVkZWYgT25lT2ZEZWNvcmF0b3JcclxuICogQHR5cGUge2Z1bmN0aW9ufVxyXG4gKiBAcGFyYW0ge09iamVjdH0gcHJvdG90eXBlIFRhcmdldCBwcm90b3R5cGVcclxuICogQHBhcmFtIHtzdHJpbmd9IG9uZW9mTmFtZSBPbmVPZiBuYW1lXHJcbiAqIEByZXR1cm5zIHt1bmRlZmluZWR9XHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIE9uZU9mIGRlY29yYXRvciAoVHlwZVNjcmlwdCkuXHJcbiAqIEBmdW5jdGlvblxyXG4gKiBAcGFyYW0gey4uLnN0cmluZ30gZmllbGROYW1lcyBGaWVsZCBuYW1lc1xyXG4gKiBAcmV0dXJucyB7T25lT2ZEZWNvcmF0b3J9IERlY29yYXRvciBmdW5jdGlvblxyXG4gKiBAdGVtcGxhdGUgVCBleHRlbmRzIHN0cmluZ1xyXG4gKi9cclxuT25lT2YuZCA9IGZ1bmN0aW9uIGRlY29yYXRlT25lT2YoKSB7XHJcbiAgICB2YXIgZmllbGROYW1lcyA9IG5ldyBBcnJheShhcmd1bWVudHMubGVuZ3RoKSxcclxuICAgICAgICBpbmRleCA9IDA7XHJcbiAgICB3aGlsZSAoaW5kZXggPCBhcmd1bWVudHMubGVuZ3RoKVxyXG4gICAgICAgIGZpZWxkTmFtZXNbaW5kZXhdID0gYXJndW1lbnRzW2luZGV4KytdO1xyXG4gICAgcmV0dXJuIGZ1bmN0aW9uIG9uZU9mRGVjb3JhdG9yKHByb3RvdHlwZSwgb25lb2ZOYW1lKSB7XHJcbiAgICAgICAgdXRpbC5kZWNvcmF0ZVR5cGUocHJvdG90eXBlLmNvbnN0cnVjdG9yKVxyXG4gICAgICAgICAgICAuYWRkKG5ldyBPbmVPZihvbmVvZk5hbWUsIGZpZWxkTmFtZXMpKTtcclxuICAgICAgICBPYmplY3QuZGVmaW5lUHJvcGVydHkocHJvdG90eXBlLCBvbmVvZk5hbWUsIHtcclxuICAgICAgICAgICAgZ2V0OiB1dGlsLm9uZU9mR2V0dGVyKGZpZWxkTmFtZXMpLFxyXG4gICAgICAgICAgICBzZXQ6IHV0aWwub25lT2ZTZXR0ZXIoZmllbGROYW1lcylcclxuICAgICAgICB9KTtcclxuICAgIH07XHJcbn07XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG5tb2R1bGUuZXhwb3J0cyA9IHBhcnNlO1xyXG5cclxucGFyc2UuZmlsZW5hbWUgPSBudWxsO1xyXG5wYXJzZS5kZWZhdWx0cyA9IHsga2VlcENhc2U6IGZhbHNlIH07XHJcblxyXG52YXIgdG9rZW5pemUgID0gcmVxdWlyZShcIi4vdG9rZW5pemVcIiksXHJcbiAgICBSb290ICAgICAgPSByZXF1aXJlKFwiLi9yb290XCIpLFxyXG4gICAgVHlwZSAgICAgID0gcmVxdWlyZShcIi4vdHlwZVwiKSxcclxuICAgIEZpZWxkICAgICA9IHJlcXVpcmUoXCIuL2ZpZWxkXCIpLFxyXG4gICAgTWFwRmllbGQgID0gcmVxdWlyZShcIi4vbWFwZmllbGRcIiksXHJcbiAgICBPbmVPZiAgICAgPSByZXF1aXJlKFwiLi9vbmVvZlwiKSxcclxuICAgIEVudW0gICAgICA9IHJlcXVpcmUoXCIuL2VudW1cIiksXHJcbiAgICBTZXJ2aWNlICAgPSByZXF1aXJlKFwiLi9zZXJ2aWNlXCIpLFxyXG4gICAgTWV0aG9kICAgID0gcmVxdWlyZShcIi4vbWV0aG9kXCIpLFxyXG4gICAgdHlwZXMgICAgID0gcmVxdWlyZShcIi4vdHlwZXNcIiksXHJcbiAgICB1dGlsICAgICAgPSByZXF1aXJlKFwiLi91dGlsXCIpO1xyXG5cclxudmFyIGJhc2UxMFJlICAgID0gL15bMS05XVswLTldKiQvLFxyXG4gICAgYmFzZTEwTmVnUmUgPSAvXi0/WzEtOV1bMC05XSokLyxcclxuICAgIGJhc2UxNlJlICAgID0gL14wW3hdWzAtOWEtZkEtRl0rJC8sXHJcbiAgICBiYXNlMTZOZWdSZSA9IC9eLT8wW3hdWzAtOWEtZkEtRl0rJC8sXHJcbiAgICBiYXNlOFJlICAgICA9IC9eMFswLTddKyQvLFxyXG4gICAgYmFzZThOZWdSZSAgPSAvXi0/MFswLTddKyQvLFxyXG4gICAgbnVtYmVyUmUgICAgPSAvXig/IVtlRV0pWzAtOV0qKD86XFwuWzAtOV0qKT8oPzpbZUVdWystXT9bMC05XSspPyQvLFxyXG4gICAgbmFtZVJlICAgICAgPSAvXlthLXpBLVpfXVthLXpBLVpfMC05XSokLyxcclxuICAgIHR5cGVSZWZSZSAgID0gL14oPzpcXC4/W2EtekEtWl9dW2EtekEtWl8wLTldKikoPzpcXC5bYS16QS1aX11bYS16QS1aXzAtOV0qKSokLyxcclxuICAgIGZxVHlwZVJlZlJlID0gL14oPzpcXC5bYS16QS1aX11bYS16QS1aXzAtOV0qKSskLztcclxuXHJcbi8qKlxyXG4gKiBSZXN1bHQgb2JqZWN0IHJldHVybmVkIGZyb20ge0BsaW5rIHBhcnNlfS5cclxuICogQGludGVyZmFjZSBJUGFyc2VyUmVzdWx0XHJcbiAqIEBwcm9wZXJ0eSB7c3RyaW5nfHVuZGVmaW5lZH0gcGFja2FnZSBQYWNrYWdlIG5hbWUsIGlmIGRlY2xhcmVkXHJcbiAqIEBwcm9wZXJ0eSB7c3RyaW5nW118dW5kZWZpbmVkfSBpbXBvcnRzIEltcG9ydHMsIGlmIGFueVxyXG4gKiBAcHJvcGVydHkge3N0cmluZ1tdfHVuZGVmaW5lZH0gd2Vha0ltcG9ydHMgV2VhayBpbXBvcnRzLCBpZiBhbnlcclxuICogQHByb3BlcnR5IHtzdHJpbmd8dW5kZWZpbmVkfSBzeW50YXggU3ludGF4LCBpZiBzcGVjaWZpZWQgKGVpdGhlciBgXCJwcm90bzJcImAgb3IgYFwicHJvdG8zXCJgKVxyXG4gKiBAcHJvcGVydHkge1Jvb3R9IHJvb3QgUG9wdWxhdGVkIHJvb3QgaW5zdGFuY2VcclxuICovXHJcblxyXG4vKipcclxuICogT3B0aW9ucyBtb2RpZnlpbmcgdGhlIGJlaGF2aW9yIG9mIHtAbGluayBwYXJzZX0uXHJcbiAqIEBpbnRlcmZhY2UgSVBhcnNlT3B0aW9uc1xyXG4gKiBAcHJvcGVydHkge2Jvb2xlYW59IFtrZWVwQ2FzZT1mYWxzZV0gS2VlcHMgZmllbGQgY2FzaW5nIGluc3RlYWQgb2YgY29udmVydGluZyB0byBjYW1lbCBjYXNlXHJcbiAqIEBwcm9wZXJ0eSB7Ym9vbGVhbn0gW2FsdGVybmF0ZUNvbW1lbnRNb2RlPWZhbHNlXSBSZWNvZ25pemUgZG91YmxlLXNsYXNoIGNvbW1lbnRzIGluIGFkZGl0aW9uIHRvIGRvYy1ibG9jayBjb21tZW50cy5cclxuICogQHByb3BlcnR5IHtib29sZWFufSBbcHJlZmVyVHJhaWxpbmdDb21tZW50PWZhbHNlXSBVc2UgdHJhaWxpbmcgY29tbWVudCB3aGVuIGJvdGggbGVhZGluZyBjb21tZW50IGFuZCB0cmFpbGluZyBjb21tZW50IGV4aXN0LlxyXG4gKi9cclxuXHJcbi8qKlxyXG4gKiBPcHRpb25zIG1vZGlmeWluZyB0aGUgYmVoYXZpb3Igb2YgSlNPTiBzZXJpYWxpemF0aW9uLlxyXG4gKiBAaW50ZXJmYWNlIElUb0pTT05PcHRpb25zXHJcbiAqIEBwcm9wZXJ0eSB7Ym9vbGVhbn0gW2tlZXBDb21tZW50cz1mYWxzZV0gU2VyaWFsaXplcyBjb21tZW50cy5cclxuICovXHJcblxyXG4vKipcclxuICogUGFyc2VzIHRoZSBnaXZlbiAucHJvdG8gc291cmNlIGFuZCByZXR1cm5zIGFuIG9iamVjdCB3aXRoIHRoZSBwYXJzZWQgY29udGVudHMuXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBzb3VyY2UgU291cmNlIGNvbnRlbnRzXHJcbiAqIEBwYXJhbSB7Um9vdH0gcm9vdCBSb290IHRvIHBvcHVsYXRlXHJcbiAqIEBwYXJhbSB7SVBhcnNlT3B0aW9uc30gW29wdGlvbnNdIFBhcnNlIG9wdGlvbnMuIERlZmF1bHRzIHRvIHtAbGluayBwYXJzZS5kZWZhdWx0c30gd2hlbiBvbWl0dGVkLlxyXG4gKiBAcmV0dXJucyB7SVBhcnNlclJlc3VsdH0gUGFyc2VyIHJlc3VsdFxyXG4gKiBAcHJvcGVydHkge3N0cmluZ30gZmlsZW5hbWU9bnVsbCBDdXJyZW50bHkgcHJvY2Vzc2luZyBmaWxlIG5hbWUgZm9yIGVycm9yIHJlcG9ydGluZywgaWYga25vd25cclxuICogQHByb3BlcnR5IHtJUGFyc2VPcHRpb25zfSBkZWZhdWx0cyBEZWZhdWx0IHtAbGluayBJUGFyc2VPcHRpb25zfVxyXG4gKi9cclxuZnVuY3Rpb24gcGFyc2Uoc291cmNlLCByb290LCBvcHRpb25zKSB7XHJcbiAgICAvKiBlc2xpbnQtZGlzYWJsZSBjYWxsYmFjay1yZXR1cm4gKi9cclxuICAgIGlmICghKHJvb3QgaW5zdGFuY2VvZiBSb290KSkge1xyXG4gICAgICAgIG9wdGlvbnMgPSByb290O1xyXG4gICAgICAgIHJvb3QgPSBuZXcgUm9vdCgpO1xyXG4gICAgfVxyXG4gICAgaWYgKCFvcHRpb25zKVxyXG4gICAgICAgIG9wdGlvbnMgPSBwYXJzZS5kZWZhdWx0cztcclxuXHJcbiAgICB2YXIgcHJlZmVyVHJhaWxpbmdDb21tZW50ID0gb3B0aW9ucy5wcmVmZXJUcmFpbGluZ0NvbW1lbnQgfHwgZmFsc2U7XHJcbiAgICB2YXIgdG4gPSB0b2tlbml6ZShzb3VyY2UsIG9wdGlvbnMuYWx0ZXJuYXRlQ29tbWVudE1vZGUgfHwgZmFsc2UpLFxyXG4gICAgICAgIG5leHQgPSB0bi5uZXh0LFxyXG4gICAgICAgIHB1c2ggPSB0bi5wdXNoLFxyXG4gICAgICAgIHBlZWsgPSB0bi5wZWVrLFxyXG4gICAgICAgIHNraXAgPSB0bi5za2lwLFxyXG4gICAgICAgIGNtbnQgPSB0bi5jbW50O1xyXG5cclxuICAgIHZhciBoZWFkID0gdHJ1ZSxcclxuICAgICAgICBwa2csXHJcbiAgICAgICAgaW1wb3J0cyxcclxuICAgICAgICB3ZWFrSW1wb3J0cyxcclxuICAgICAgICBzeW50YXgsXHJcbiAgICAgICAgaXNQcm90bzMgPSBmYWxzZTtcclxuXHJcbiAgICB2YXIgcHRyID0gcm9vdDtcclxuXHJcbiAgICB2YXIgYXBwbHlDYXNlID0gb3B0aW9ucy5rZWVwQ2FzZSA/IGZ1bmN0aW9uKG5hbWUpIHsgcmV0dXJuIG5hbWU7IH0gOiB1dGlsLmNhbWVsQ2FzZTtcclxuXHJcbiAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgbmV4dCAqL1xyXG4gICAgZnVuY3Rpb24gaWxsZWdhbCh0b2tlbiwgbmFtZSwgaW5zaWRlVHJ5Q2F0Y2gpIHtcclxuICAgICAgICB2YXIgZmlsZW5hbWUgPSBwYXJzZS5maWxlbmFtZTtcclxuICAgICAgICBpZiAoIWluc2lkZVRyeUNhdGNoKVxyXG4gICAgICAgICAgICBwYXJzZS5maWxlbmFtZSA9IG51bGw7XHJcbiAgICAgICAgcmV0dXJuIEVycm9yKFwiaWxsZWdhbCBcIiArIChuYW1lIHx8IFwidG9rZW5cIikgKyBcIiAnXCIgKyB0b2tlbiArIFwiJyAoXCIgKyAoZmlsZW5hbWUgPyBmaWxlbmFtZSArIFwiLCBcIiA6IFwiXCIpICsgXCJsaW5lIFwiICsgdG4ubGluZSArIFwiKVwiKTtcclxuICAgIH1cclxuXHJcbiAgICBmdW5jdGlvbiByZWFkU3RyaW5nKCkge1xyXG4gICAgICAgIHZhciB2YWx1ZXMgPSBbXSxcclxuICAgICAgICAgICAgdG9rZW47XHJcbiAgICAgICAgZG8ge1xyXG4gICAgICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgaWYgKi9cclxuICAgICAgICAgICAgaWYgKCh0b2tlbiA9IG5leHQoKSkgIT09IFwiXFxcIlwiICYmIHRva2VuICE9PSBcIidcIilcclxuICAgICAgICAgICAgICAgIHRocm93IGlsbGVnYWwodG9rZW4pO1xyXG5cclxuICAgICAgICAgICAgdmFsdWVzLnB1c2gobmV4dCgpKTtcclxuICAgICAgICAgICAgc2tpcCh0b2tlbik7XHJcbiAgICAgICAgICAgIHRva2VuID0gcGVlaygpO1xyXG4gICAgICAgIH0gd2hpbGUgKHRva2VuID09PSBcIlxcXCJcIiB8fCB0b2tlbiA9PT0gXCInXCIpO1xyXG4gICAgICAgIHJldHVybiB2YWx1ZXMuam9pbihcIlwiKTtcclxuICAgIH1cclxuXHJcbiAgICBmdW5jdGlvbiByZWFkVmFsdWUoYWNjZXB0VHlwZVJlZikge1xyXG4gICAgICAgIHZhciB0b2tlbiA9IG5leHQoKTtcclxuICAgICAgICBzd2l0Y2ggKHRva2VuKSB7XHJcbiAgICAgICAgICAgIGNhc2UgXCInXCI6XHJcbiAgICAgICAgICAgIGNhc2UgXCJcXFwiXCI6XHJcbiAgICAgICAgICAgICAgICBwdXNoKHRva2VuKTtcclxuICAgICAgICAgICAgICAgIHJldHVybiByZWFkU3RyaW5nKCk7XHJcbiAgICAgICAgICAgIGNhc2UgXCJ0cnVlXCI6IGNhc2UgXCJUUlVFXCI6XHJcbiAgICAgICAgICAgICAgICByZXR1cm4gdHJ1ZTtcclxuICAgICAgICAgICAgY2FzZSBcImZhbHNlXCI6IGNhc2UgXCJGQUxTRVwiOlxyXG4gICAgICAgICAgICAgICAgcmV0dXJuIGZhbHNlO1xyXG4gICAgICAgIH1cclxuICAgICAgICB0cnkge1xyXG4gICAgICAgICAgICByZXR1cm4gcGFyc2VOdW1iZXIodG9rZW4sIC8qIGluc2lkZVRyeUNhdGNoICovIHRydWUpO1xyXG4gICAgICAgIH0gY2F0Y2ggKGUpIHtcclxuXHJcbiAgICAgICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBlbHNlICovXHJcbiAgICAgICAgICAgIGlmIChhY2NlcHRUeXBlUmVmICYmIHR5cGVSZWZSZS50ZXN0KHRva2VuKSlcclxuICAgICAgICAgICAgICAgIHJldHVybiB0b2tlbjtcclxuXHJcbiAgICAgICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0ICovXHJcbiAgICAgICAgICAgIHRocm93IGlsbGVnYWwodG9rZW4sIFwidmFsdWVcIik7XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG5cclxuICAgIGZ1bmN0aW9uIHJlYWRSYW5nZXModGFyZ2V0LCBhY2NlcHRTdHJpbmdzKSB7XHJcbiAgICAgICAgdmFyIHRva2VuLCBzdGFydDtcclxuICAgICAgICBkbyB7XHJcbiAgICAgICAgICAgIGlmIChhY2NlcHRTdHJpbmdzICYmICgodG9rZW4gPSBwZWVrKCkpID09PSBcIlxcXCJcIiB8fCB0b2tlbiA9PT0gXCInXCIpKVxyXG4gICAgICAgICAgICAgICAgdGFyZ2V0LnB1c2gocmVhZFN0cmluZygpKTtcclxuICAgICAgICAgICAgZWxzZVxyXG4gICAgICAgICAgICAgICAgdGFyZ2V0LnB1c2goWyBzdGFydCA9IHBhcnNlSWQobmV4dCgpKSwgc2tpcChcInRvXCIsIHRydWUpID8gcGFyc2VJZChuZXh0KCkpIDogc3RhcnQgXSk7XHJcbiAgICAgICAgfSB3aGlsZSAoc2tpcChcIixcIiwgdHJ1ZSkpO1xyXG4gICAgICAgIHZhciBkdW1teSA9IHtvcHRpb25zOiB1bmRlZmluZWR9O1xyXG4gICAgICAgIGR1bW15LnNldE9wdGlvbiA9IGZ1bmN0aW9uKG5hbWUsIHZhbHVlKSB7XHJcbiAgICAgICAgICBpZiAodGhpcy5vcHRpb25zID09PSB1bmRlZmluZWQpIHRoaXMub3B0aW9ucyA9IHt9O1xyXG4gICAgICAgICAgdGhpcy5vcHRpb25zW25hbWVdID0gdmFsdWU7XHJcbiAgICAgICAgfTtcclxuICAgICAgICBpZkJsb2NrKFxyXG4gICAgICAgICAgICBkdW1teSxcclxuICAgICAgICAgICAgZnVuY3Rpb24gcGFyc2VSYW5nZV9ibG9jayh0b2tlbikge1xyXG4gICAgICAgICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBlbHNlICovXHJcbiAgICAgICAgICAgICAgaWYgKHRva2VuID09PSBcIm9wdGlvblwiKSB7XHJcbiAgICAgICAgICAgICAgICBwYXJzZU9wdGlvbihkdW1teSwgdG9rZW4pOyAgLy8gc2tpcFxyXG4gICAgICAgICAgICAgICAgc2tpcChcIjtcIik7XHJcbiAgICAgICAgICAgICAgfSBlbHNlXHJcbiAgICAgICAgICAgICAgICB0aHJvdyBpbGxlZ2FsKHRva2VuKTtcclxuICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgZnVuY3Rpb24gcGFyc2VSYW5nZV9saW5lKCkge1xyXG4gICAgICAgICAgICAgIHBhcnNlSW5saW5lT3B0aW9ucyhkdW1teSk7ICAvLyBza2lwXHJcbiAgICAgICAgICAgIH0pO1xyXG4gICAgfVxyXG5cclxuICAgIGZ1bmN0aW9uIHBhcnNlTnVtYmVyKHRva2VuLCBpbnNpZGVUcnlDYXRjaCkge1xyXG4gICAgICAgIHZhciBzaWduID0gMTtcclxuICAgICAgICBpZiAodG9rZW4uY2hhckF0KDApID09PSBcIi1cIikge1xyXG4gICAgICAgICAgICBzaWduID0gLTE7XHJcbiAgICAgICAgICAgIHRva2VuID0gdG9rZW4uc3Vic3RyaW5nKDEpO1xyXG4gICAgICAgIH1cclxuICAgICAgICBzd2l0Y2ggKHRva2VuKSB7XHJcbiAgICAgICAgICAgIGNhc2UgXCJpbmZcIjogY2FzZSBcIklORlwiOiBjYXNlIFwiSW5mXCI6XHJcbiAgICAgICAgICAgICAgICByZXR1cm4gc2lnbiAqIEluZmluaXR5O1xyXG4gICAgICAgICAgICBjYXNlIFwibmFuXCI6IGNhc2UgXCJOQU5cIjogY2FzZSBcIk5hblwiOiBjYXNlIFwiTmFOXCI6XHJcbiAgICAgICAgICAgICAgICByZXR1cm4gTmFOO1xyXG4gICAgICAgICAgICBjYXNlIFwiMFwiOlxyXG4gICAgICAgICAgICAgICAgcmV0dXJuIDA7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIGlmIChiYXNlMTBSZS50ZXN0KHRva2VuKSlcclxuICAgICAgICAgICAgcmV0dXJuIHNpZ24gKiBwYXJzZUludCh0b2tlbiwgMTApO1xyXG4gICAgICAgIGlmIChiYXNlMTZSZS50ZXN0KHRva2VuKSlcclxuICAgICAgICAgICAgcmV0dXJuIHNpZ24gKiBwYXJzZUludCh0b2tlbiwgMTYpO1xyXG4gICAgICAgIGlmIChiYXNlOFJlLnRlc3QodG9rZW4pKVxyXG4gICAgICAgICAgICByZXR1cm4gc2lnbiAqIHBhcnNlSW50KHRva2VuLCA4KTtcclxuXHJcbiAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGVsc2UgKi9cclxuICAgICAgICBpZiAobnVtYmVyUmUudGVzdCh0b2tlbikpXHJcbiAgICAgICAgICAgIHJldHVybiBzaWduICogcGFyc2VGbG9hdCh0b2tlbik7XHJcblxyXG4gICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0ICovXHJcbiAgICAgICAgdGhyb3cgaWxsZWdhbCh0b2tlbiwgXCJudW1iZXJcIiwgaW5zaWRlVHJ5Q2F0Y2gpO1xyXG4gICAgfVxyXG5cclxuICAgIGZ1bmN0aW9uIHBhcnNlSWQodG9rZW4sIGFjY2VwdE5lZ2F0aXZlKSB7XHJcbiAgICAgICAgc3dpdGNoICh0b2tlbikge1xyXG4gICAgICAgICAgICBjYXNlIFwibWF4XCI6IGNhc2UgXCJNQVhcIjogY2FzZSBcIk1heFwiOlxyXG4gICAgICAgICAgICAgICAgcmV0dXJuIDUzNjg3MDkxMTtcclxuICAgICAgICAgICAgY2FzZSBcIjBcIjpcclxuICAgICAgICAgICAgICAgIHJldHVybiAwO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICAgICAgaWYgKCFhY2NlcHROZWdhdGl2ZSAmJiB0b2tlbi5jaGFyQXQoMCkgPT09IFwiLVwiKVxyXG4gICAgICAgICAgICB0aHJvdyBpbGxlZ2FsKHRva2VuLCBcImlkXCIpO1xyXG5cclxuICAgICAgICBpZiAoYmFzZTEwTmVnUmUudGVzdCh0b2tlbikpXHJcbiAgICAgICAgICAgIHJldHVybiBwYXJzZUludCh0b2tlbiwgMTApO1xyXG4gICAgICAgIGlmIChiYXNlMTZOZWdSZS50ZXN0KHRva2VuKSlcclxuICAgICAgICAgICAgcmV0dXJuIHBhcnNlSW50KHRva2VuLCAxNik7XHJcblxyXG4gICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBlbHNlICovXHJcbiAgICAgICAgaWYgKGJhc2U4TmVnUmUudGVzdCh0b2tlbikpXHJcbiAgICAgICAgICAgIHJldHVybiBwYXJzZUludCh0b2tlbiwgOCk7XHJcblxyXG4gICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0ICovXHJcbiAgICAgICAgdGhyb3cgaWxsZWdhbCh0b2tlbiwgXCJpZFwiKTtcclxuICAgIH1cclxuXHJcbiAgICBmdW5jdGlvbiBwYXJzZVBhY2thZ2UoKSB7XHJcblxyXG4gICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgICAgIGlmIChwa2cgIT09IHVuZGVmaW5lZClcclxuICAgICAgICAgICAgdGhyb3cgaWxsZWdhbChcInBhY2thZ2VcIik7XHJcblxyXG4gICAgICAgIHBrZyA9IG5leHQoKTtcclxuXHJcbiAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICAgICAgaWYgKCF0eXBlUmVmUmUudGVzdChwa2cpKVxyXG4gICAgICAgICAgICB0aHJvdyBpbGxlZ2FsKHBrZywgXCJuYW1lXCIpO1xyXG5cclxuICAgICAgICBwdHIgPSBwdHIuZGVmaW5lKHBrZyk7XHJcbiAgICAgICAgc2tpcChcIjtcIik7XHJcbiAgICB9XHJcblxyXG4gICAgZnVuY3Rpb24gcGFyc2VJbXBvcnQoKSB7XHJcbiAgICAgICAgdmFyIHRva2VuID0gcGVlaygpO1xyXG4gICAgICAgIHZhciB3aGljaEltcG9ydHM7XHJcbiAgICAgICAgc3dpdGNoICh0b2tlbikge1xyXG4gICAgICAgICAgICBjYXNlIFwid2Vha1wiOlxyXG4gICAgICAgICAgICAgICAgd2hpY2hJbXBvcnRzID0gd2Vha0ltcG9ydHMgfHwgKHdlYWtJbXBvcnRzID0gW10pO1xyXG4gICAgICAgICAgICAgICAgbmV4dCgpO1xyXG4gICAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgICAgIGNhc2UgXCJwdWJsaWNcIjpcclxuICAgICAgICAgICAgICAgIG5leHQoKTtcclxuICAgICAgICAgICAgICAgIC8vIGVzbGludC1kaXNhYmxlLW5leHQtbGluZSBuby1mYWxsdGhyb3VnaFxyXG4gICAgICAgICAgICBkZWZhdWx0OlxyXG4gICAgICAgICAgICAgICAgd2hpY2hJbXBvcnRzID0gaW1wb3J0cyB8fCAoaW1wb3J0cyA9IFtdKTtcclxuICAgICAgICAgICAgICAgIGJyZWFrO1xyXG4gICAgICAgIH1cclxuICAgICAgICB0b2tlbiA9IHJlYWRTdHJpbmcoKTtcclxuICAgICAgICBza2lwKFwiO1wiKTtcclxuICAgICAgICB3aGljaEltcG9ydHMucHVzaCh0b2tlbik7XHJcbiAgICB9XHJcblxyXG4gICAgZnVuY3Rpb24gcGFyc2VTeW50YXgoKSB7XHJcbiAgICAgICAgc2tpcChcIj1cIik7XHJcbiAgICAgICAgc3ludGF4ID0gcmVhZFN0cmluZygpO1xyXG4gICAgICAgIGlzUHJvdG8zID0gc3ludGF4ID09PSBcInByb3RvM1wiO1xyXG5cclxuICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgaWYgKi9cclxuICAgICAgICBpZiAoIWlzUHJvdG8zICYmIHN5bnRheCAhPT0gXCJwcm90bzJcIilcclxuICAgICAgICAgICAgdGhyb3cgaWxsZWdhbChzeW50YXgsIFwic3ludGF4XCIpO1xyXG5cclxuICAgICAgICBza2lwKFwiO1wiKTtcclxuICAgIH1cclxuXHJcbiAgICBmdW5jdGlvbiBwYXJzZUNvbW1vbihwYXJlbnQsIHRva2VuKSB7XHJcbiAgICAgICAgc3dpdGNoICh0b2tlbikge1xyXG5cclxuICAgICAgICAgICAgY2FzZSBcIm9wdGlvblwiOlxyXG4gICAgICAgICAgICAgICAgcGFyc2VPcHRpb24ocGFyZW50LCB0b2tlbik7XHJcbiAgICAgICAgICAgICAgICBza2lwKFwiO1wiKTtcclxuICAgICAgICAgICAgICAgIHJldHVybiB0cnVlO1xyXG5cclxuICAgICAgICAgICAgY2FzZSBcIm1lc3NhZ2VcIjpcclxuICAgICAgICAgICAgICAgIHBhcnNlVHlwZShwYXJlbnQsIHRva2VuKTtcclxuICAgICAgICAgICAgICAgIHJldHVybiB0cnVlO1xyXG5cclxuICAgICAgICAgICAgY2FzZSBcImVudW1cIjpcclxuICAgICAgICAgICAgICAgIHBhcnNlRW51bShwYXJlbnQsIHRva2VuKTtcclxuICAgICAgICAgICAgICAgIHJldHVybiB0cnVlO1xyXG5cclxuICAgICAgICAgICAgY2FzZSBcInNlcnZpY2VcIjpcclxuICAgICAgICAgICAgICAgIHBhcnNlU2VydmljZShwYXJlbnQsIHRva2VuKTtcclxuICAgICAgICAgICAgICAgIHJldHVybiB0cnVlO1xyXG5cclxuICAgICAgICAgICAgY2FzZSBcImV4dGVuZFwiOlxyXG4gICAgICAgICAgICAgICAgcGFyc2VFeHRlbnNpb24ocGFyZW50LCB0b2tlbik7XHJcbiAgICAgICAgICAgICAgICByZXR1cm4gdHJ1ZTtcclxuICAgICAgICB9XHJcbiAgICAgICAgcmV0dXJuIGZhbHNlO1xyXG4gICAgfVxyXG5cclxuICAgIGZ1bmN0aW9uIGlmQmxvY2sob2JqLCBmbklmLCBmbkVsc2UpIHtcclxuICAgICAgICB2YXIgdHJhaWxpbmdMaW5lID0gdG4ubGluZTtcclxuICAgICAgICBpZiAob2JqKSB7XHJcbiAgICAgICAgICAgIGlmKHR5cGVvZiBvYmouY29tbWVudCAhPT0gXCJzdHJpbmdcIikge1xyXG4gICAgICAgICAgICAgIG9iai5jb21tZW50ID0gY21udCgpOyAvLyB0cnkgYmxvY2stdHlwZSBjb21tZW50XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgb2JqLmZpbGVuYW1lID0gcGFyc2UuZmlsZW5hbWU7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIGlmIChza2lwKFwie1wiLCB0cnVlKSkge1xyXG4gICAgICAgICAgICB2YXIgdG9rZW47XHJcbiAgICAgICAgICAgIHdoaWxlICgodG9rZW4gPSBuZXh0KCkpICE9PSBcIn1cIilcclxuICAgICAgICAgICAgICAgIGZuSWYodG9rZW4pO1xyXG4gICAgICAgICAgICBza2lwKFwiO1wiLCB0cnVlKTtcclxuICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICBpZiAoZm5FbHNlKVxyXG4gICAgICAgICAgICAgICAgZm5FbHNlKCk7XHJcbiAgICAgICAgICAgIHNraXAoXCI7XCIpO1xyXG4gICAgICAgICAgICBpZiAob2JqICYmICh0eXBlb2Ygb2JqLmNvbW1lbnQgIT09IFwic3RyaW5nXCIgfHwgcHJlZmVyVHJhaWxpbmdDb21tZW50KSlcclxuICAgICAgICAgICAgICAgIG9iai5jb21tZW50ID0gY21udCh0cmFpbGluZ0xpbmUpIHx8IG9iai5jb21tZW50OyAvLyB0cnkgbGluZS10eXBlIGNvbW1lbnRcclxuICAgICAgICB9XHJcbiAgICB9XHJcblxyXG4gICAgZnVuY3Rpb24gcGFyc2VUeXBlKHBhcmVudCwgdG9rZW4pIHtcclxuXHJcbiAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICAgICAgaWYgKCFuYW1lUmUudGVzdCh0b2tlbiA9IG5leHQoKSkpXHJcbiAgICAgICAgICAgIHRocm93IGlsbGVnYWwodG9rZW4sIFwidHlwZSBuYW1lXCIpO1xyXG5cclxuICAgICAgICB2YXIgdHlwZSA9IG5ldyBUeXBlKHRva2VuKTtcclxuICAgICAgICBpZkJsb2NrKHR5cGUsIGZ1bmN0aW9uIHBhcnNlVHlwZV9ibG9jayh0b2tlbikge1xyXG4gICAgICAgICAgICBpZiAocGFyc2VDb21tb24odHlwZSwgdG9rZW4pKVxyXG4gICAgICAgICAgICAgICAgcmV0dXJuO1xyXG5cclxuICAgICAgICAgICAgc3dpdGNoICh0b2tlbikge1xyXG5cclxuICAgICAgICAgICAgICAgIGNhc2UgXCJtYXBcIjpcclxuICAgICAgICAgICAgICAgICAgICBwYXJzZU1hcEZpZWxkKHR5cGUsIHRva2VuKTtcclxuICAgICAgICAgICAgICAgICAgICBicmVhaztcclxuXHJcbiAgICAgICAgICAgICAgICBjYXNlIFwicmVxdWlyZWRcIjpcclxuICAgICAgICAgICAgICAgIGNhc2UgXCJyZXBlYXRlZFwiOlxyXG4gICAgICAgICAgICAgICAgICAgIHBhcnNlRmllbGQodHlwZSwgdG9rZW4pO1xyXG4gICAgICAgICAgICAgICAgICAgIGJyZWFrO1xyXG5cclxuICAgICAgICAgICAgICAgIGNhc2UgXCJvcHRpb25hbFwiOlxyXG4gICAgICAgICAgICAgICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgICAgICAgICAgICAgICAgIGlmIChpc1Byb3RvMykge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICBwYXJzZUZpZWxkKHR5cGUsIFwicHJvdG8zX29wdGlvbmFsXCIpO1xyXG4gICAgICAgICAgICAgICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHBhcnNlRmllbGQodHlwZSwgXCJvcHRpb25hbFwiKTtcclxuICAgICAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICAgICAgYnJlYWs7XHJcblxyXG4gICAgICAgICAgICAgICAgY2FzZSBcIm9uZW9mXCI6XHJcbiAgICAgICAgICAgICAgICAgICAgcGFyc2VPbmVPZih0eXBlLCB0b2tlbik7XHJcbiAgICAgICAgICAgICAgICAgICAgYnJlYWs7XHJcblxyXG4gICAgICAgICAgICAgICAgY2FzZSBcImV4dGVuc2lvbnNcIjpcclxuICAgICAgICAgICAgICAgICAgICByZWFkUmFuZ2VzKHR5cGUuZXh0ZW5zaW9ucyB8fCAodHlwZS5leHRlbnNpb25zID0gW10pKTtcclxuICAgICAgICAgICAgICAgICAgICBicmVhaztcclxuXHJcbiAgICAgICAgICAgICAgICBjYXNlIFwicmVzZXJ2ZWRcIjpcclxuICAgICAgICAgICAgICAgICAgICByZWFkUmFuZ2VzKHR5cGUucmVzZXJ2ZWQgfHwgKHR5cGUucmVzZXJ2ZWQgPSBbXSksIHRydWUpO1xyXG4gICAgICAgICAgICAgICAgICAgIGJyZWFrO1xyXG5cclxuICAgICAgICAgICAgICAgIGRlZmF1bHQ6XHJcbiAgICAgICAgICAgICAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICAgICAgICAgICAgICAgICAgaWYgKCFpc1Byb3RvMyB8fCAhdHlwZVJlZlJlLnRlc3QodG9rZW4pKVxyXG4gICAgICAgICAgICAgICAgICAgICAgICB0aHJvdyBpbGxlZ2FsKHRva2VuKTtcclxuXHJcbiAgICAgICAgICAgICAgICAgICAgcHVzaCh0b2tlbik7XHJcbiAgICAgICAgICAgICAgICAgICAgcGFyc2VGaWVsZCh0eXBlLCBcIm9wdGlvbmFsXCIpO1xyXG4gICAgICAgICAgICAgICAgICAgIGJyZWFrO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfSk7XHJcbiAgICAgICAgcGFyZW50LmFkZCh0eXBlKTtcclxuICAgIH1cclxuXHJcbiAgICBmdW5jdGlvbiBwYXJzZUZpZWxkKHBhcmVudCwgcnVsZSwgZXh0ZW5kKSB7XHJcbiAgICAgICAgdmFyIHR5cGUgPSBuZXh0KCk7XHJcbiAgICAgICAgaWYgKHR5cGUgPT09IFwiZ3JvdXBcIikge1xyXG4gICAgICAgICAgICBwYXJzZUdyb3VwKHBhcmVudCwgcnVsZSk7XHJcbiAgICAgICAgICAgIHJldHVybjtcclxuICAgICAgICB9XHJcbiAgICAgICAgLy8gVHlwZSBuYW1lcyBjYW4gY29uc3VtZSBtdWx0aXBsZSB0b2tlbnMsIGluIG11bHRpcGxlIHZhcmlhbnRzOlxyXG4gICAgICAgIC8vICAgIHBhY2thZ2Uuc3VicGFja2FnZSAgIGZpZWxkICAgICAgIHRva2VuczogXCJwYWNrYWdlLnN1YnBhY2thZ2VcIiBbVFlQRSBOQU1FIEVORFMgSEVSRV0gXCJmaWVsZFwiXHJcbiAgICAgICAgLy8gICAgcGFja2FnZSAuIHN1YnBhY2thZ2UgZmllbGQgICAgICAgdG9rZW5zOiBcInBhY2thZ2VcIiBcIi5cIiBcInN1YnBhY2thZ2VcIiBbVFlQRSBOQU1FIEVORFMgSEVSRV0gXCJmaWVsZFwiXHJcbiAgICAgICAgLy8gICAgcGFja2FnZS4gIHN1YnBhY2thZ2UgZmllbGQgICAgICAgdG9rZW5zOiBcInBhY2thZ2UuXCIgXCJzdWJwYWNrYWdlXCIgW1RZUEUgTkFNRSBFTkRTIEhFUkVdIFwiZmllbGRcIlxyXG4gICAgICAgIC8vICAgIHBhY2thZ2UgIC5zdWJwYWNrYWdlIGZpZWxkICAgICAgIHRva2VuczogXCJwYWNrYWdlXCIgXCIuc3VicGFja2FnZVwiIFtUWVBFIE5BTUUgRU5EUyBIRVJFXSBcImZpZWxkXCJcclxuICAgICAgICAvLyBLZWVwIHJlYWRpbmcgdG9rZW5zIHVudGlsIHdlIGdldCBhIHR5cGUgbmFtZSB3aXRoIG5vIHBlcmlvZCBhdCB0aGUgZW5kLFxyXG4gICAgICAgIC8vIGFuZCB0aGUgbmV4dCB0b2tlbiBkb2VzIG5vdCBzdGFydCB3aXRoIGEgcGVyaW9kLlxyXG4gICAgICAgIHdoaWxlICh0eXBlLmVuZHNXaXRoKFwiLlwiKSB8fCBwZWVrKCkuc3RhcnRzV2l0aChcIi5cIikpIHtcclxuICAgICAgICAgICAgdHlwZSArPSBuZXh0KCk7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgaWYgKi9cclxuICAgICAgICBpZiAoIXR5cGVSZWZSZS50ZXN0KHR5cGUpKVxyXG4gICAgICAgICAgICB0aHJvdyBpbGxlZ2FsKHR5cGUsIFwidHlwZVwiKTtcclxuXHJcbiAgICAgICAgdmFyIG5hbWUgPSBuZXh0KCk7XHJcblxyXG4gICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgICAgIGlmICghbmFtZVJlLnRlc3QobmFtZSkpXHJcbiAgICAgICAgICAgIHRocm93IGlsbGVnYWwobmFtZSwgXCJuYW1lXCIpO1xyXG5cclxuICAgICAgICBuYW1lID0gYXBwbHlDYXNlKG5hbWUpO1xyXG4gICAgICAgIHNraXAoXCI9XCIpO1xyXG5cclxuICAgICAgICB2YXIgZmllbGQgPSBuZXcgRmllbGQobmFtZSwgcGFyc2VJZChuZXh0KCkpLCB0eXBlLCBydWxlLCBleHRlbmQpO1xyXG4gICAgICAgIGlmQmxvY2soZmllbGQsIGZ1bmN0aW9uIHBhcnNlRmllbGRfYmxvY2sodG9rZW4pIHtcclxuXHJcbiAgICAgICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBlbHNlICovXHJcbiAgICAgICAgICAgIGlmICh0b2tlbiA9PT0gXCJvcHRpb25cIikge1xyXG4gICAgICAgICAgICAgICAgcGFyc2VPcHRpb24oZmllbGQsIHRva2VuKTtcclxuICAgICAgICAgICAgICAgIHNraXAoXCI7XCIpO1xyXG4gICAgICAgICAgICB9IGVsc2VcclxuICAgICAgICAgICAgICAgIHRocm93IGlsbGVnYWwodG9rZW4pO1xyXG5cclxuICAgICAgICB9LCBmdW5jdGlvbiBwYXJzZUZpZWxkX2xpbmUoKSB7XHJcbiAgICAgICAgICAgIHBhcnNlSW5saW5lT3B0aW9ucyhmaWVsZCk7XHJcbiAgICAgICAgfSk7XHJcblxyXG4gICAgICAgIGlmIChydWxlID09PSBcInByb3RvM19vcHRpb25hbFwiKSB7XHJcbiAgICAgICAgICAgIC8vIGZvciBwcm90bzMgb3B0aW9uYWwgZmllbGRzLCB3ZSBjcmVhdGUgYSBzaW5nbGUtbWVtYmVyIE9uZW9mIHRvIG1pbWljIFwib3B0aW9uYWxcIiBiZWhhdmlvclxyXG4gICAgICAgICAgICB2YXIgb25lb2YgPSBuZXcgT25lT2YoXCJfXCIgKyBuYW1lKTtcclxuICAgICAgICAgICAgZmllbGQuc2V0T3B0aW9uKFwicHJvdG8zX29wdGlvbmFsXCIsIHRydWUpO1xyXG4gICAgICAgICAgICBvbmVvZi5hZGQoZmllbGQpO1xyXG4gICAgICAgICAgICBwYXJlbnQuYWRkKG9uZW9mKTtcclxuICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICBwYXJlbnQuYWRkKGZpZWxkKTtcclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIC8vIEpTT04gZGVmYXVsdHMgdG8gcGFja2VkPXRydWUgaWYgbm90IHNldCBzbyB3ZSBoYXZlIHRvIHNldCBwYWNrZWQ9ZmFsc2UgZXhwbGljaXR5IHdoZW5cclxuICAgICAgICAvLyBwYXJzaW5nIHByb3RvMiBkZXNjcmlwdG9ycyB3aXRob3V0IHRoZSBvcHRpb24sIHdoZXJlIGFwcGxpY2FibGUuIFRoaXMgbXVzdCBiZSBkb25lIGZvclxyXG4gICAgICAgIC8vIGFsbCBrbm93biBwYWNrYWJsZSB0eXBlcyBhbmQgYW55dGhpbmcgdGhhdCBjb3VsZCBiZSBhbiBlbnVtICg9IGlzIG5vdCBhIGJhc2ljIHR5cGUpLlxyXG4gICAgICAgIGlmICghaXNQcm90bzMgJiYgZmllbGQucmVwZWF0ZWQgJiYgKHR5cGVzLnBhY2tlZFt0eXBlXSAhPT0gdW5kZWZpbmVkIHx8IHR5cGVzLmJhc2ljW3R5cGVdID09PSB1bmRlZmluZWQpKVxyXG4gICAgICAgICAgICBmaWVsZC5zZXRPcHRpb24oXCJwYWNrZWRcIiwgZmFsc2UsIC8qIGlmTm90U2V0ICovIHRydWUpO1xyXG4gICAgfVxyXG5cclxuICAgIGZ1bmN0aW9uIHBhcnNlR3JvdXAocGFyZW50LCBydWxlKSB7XHJcbiAgICAgICAgdmFyIG5hbWUgPSBuZXh0KCk7XHJcblxyXG4gICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgICAgIGlmICghbmFtZVJlLnRlc3QobmFtZSkpXHJcbiAgICAgICAgICAgIHRocm93IGlsbGVnYWwobmFtZSwgXCJuYW1lXCIpO1xyXG5cclxuICAgICAgICB2YXIgZmllbGROYW1lID0gdXRpbC5sY0ZpcnN0KG5hbWUpO1xyXG4gICAgICAgIGlmIChuYW1lID09PSBmaWVsZE5hbWUpXHJcbiAgICAgICAgICAgIG5hbWUgPSB1dGlsLnVjRmlyc3QobmFtZSk7XHJcbiAgICAgICAgc2tpcChcIj1cIik7XHJcbiAgICAgICAgdmFyIGlkID0gcGFyc2VJZChuZXh0KCkpO1xyXG4gICAgICAgIHZhciB0eXBlID0gbmV3IFR5cGUobmFtZSk7XHJcbiAgICAgICAgdHlwZS5ncm91cCA9IHRydWU7XHJcbiAgICAgICAgdmFyIGZpZWxkID0gbmV3IEZpZWxkKGZpZWxkTmFtZSwgaWQsIG5hbWUsIHJ1bGUpO1xyXG4gICAgICAgIGZpZWxkLmZpbGVuYW1lID0gcGFyc2UuZmlsZW5hbWU7XHJcbiAgICAgICAgaWZCbG9jayh0eXBlLCBmdW5jdGlvbiBwYXJzZUdyb3VwX2Jsb2NrKHRva2VuKSB7XHJcbiAgICAgICAgICAgIHN3aXRjaCAodG9rZW4pIHtcclxuXHJcbiAgICAgICAgICAgICAgICBjYXNlIFwib3B0aW9uXCI6XHJcbiAgICAgICAgICAgICAgICAgICAgcGFyc2VPcHRpb24odHlwZSwgdG9rZW4pO1xyXG4gICAgICAgICAgICAgICAgICAgIHNraXAoXCI7XCIpO1xyXG4gICAgICAgICAgICAgICAgICAgIGJyZWFrO1xyXG5cclxuICAgICAgICAgICAgICAgIGNhc2UgXCJyZXF1aXJlZFwiOlxyXG4gICAgICAgICAgICAgICAgY2FzZSBcInJlcGVhdGVkXCI6XHJcbiAgICAgICAgICAgICAgICAgICAgcGFyc2VGaWVsZCh0eXBlLCB0b2tlbik7XHJcbiAgICAgICAgICAgICAgICAgICAgYnJlYWs7XHJcblxyXG4gICAgICAgICAgICAgICAgY2FzZSBcIm9wdGlvbmFsXCI6XHJcbiAgICAgICAgICAgICAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICAgICAgICAgICAgICAgICAgaWYgKGlzUHJvdG8zKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHBhcnNlRmllbGQodHlwZSwgXCJwcm90bzNfb3B0aW9uYWxcIik7XHJcbiAgICAgICAgICAgICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgcGFyc2VGaWVsZCh0eXBlLCBcIm9wdGlvbmFsXCIpO1xyXG4gICAgICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgICAgICBicmVhaztcclxuXHJcbiAgICAgICAgICAgICAgICBjYXNlIFwibWVzc2FnZVwiOlxyXG4gICAgICAgICAgICAgICAgICAgIHBhcnNlVHlwZSh0eXBlLCB0b2tlbik7XHJcbiAgICAgICAgICAgICAgICAgICAgYnJlYWs7XHJcblxyXG4gICAgICAgICAgICAgICAgY2FzZSBcImVudW1cIjpcclxuICAgICAgICAgICAgICAgICAgICBwYXJzZUVudW0odHlwZSwgdG9rZW4pO1xyXG4gICAgICAgICAgICAgICAgICAgIGJyZWFrO1xyXG5cclxuICAgICAgICAgICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0ICovXHJcbiAgICAgICAgICAgICAgICBkZWZhdWx0OlxyXG4gICAgICAgICAgICAgICAgICAgIHRocm93IGlsbGVnYWwodG9rZW4pOyAvLyB0aGVyZSBhcmUgbm8gZ3JvdXBzIHdpdGggcHJvdG8zIHNlbWFudGljc1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfSk7XHJcbiAgICAgICAgcGFyZW50LmFkZCh0eXBlKVxyXG4gICAgICAgICAgICAgIC5hZGQoZmllbGQpO1xyXG4gICAgfVxyXG5cclxuICAgIGZ1bmN0aW9uIHBhcnNlTWFwRmllbGQocGFyZW50KSB7XHJcbiAgICAgICAgc2tpcChcIjxcIik7XHJcbiAgICAgICAgdmFyIGtleVR5cGUgPSBuZXh0KCk7XHJcblxyXG4gICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgICAgIGlmICh0eXBlcy5tYXBLZXlba2V5VHlwZV0gPT09IHVuZGVmaW5lZClcclxuICAgICAgICAgICAgdGhyb3cgaWxsZWdhbChrZXlUeXBlLCBcInR5cGVcIik7XHJcblxyXG4gICAgICAgIHNraXAoXCIsXCIpO1xyXG4gICAgICAgIHZhciB2YWx1ZVR5cGUgPSBuZXh0KCk7XHJcblxyXG4gICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgICAgIGlmICghdHlwZVJlZlJlLnRlc3QodmFsdWVUeXBlKSlcclxuICAgICAgICAgICAgdGhyb3cgaWxsZWdhbCh2YWx1ZVR5cGUsIFwidHlwZVwiKTtcclxuXHJcbiAgICAgICAgc2tpcChcIj5cIik7XHJcbiAgICAgICAgdmFyIG5hbWUgPSBuZXh0KCk7XHJcblxyXG4gICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgICAgIGlmICghbmFtZVJlLnRlc3QobmFtZSkpXHJcbiAgICAgICAgICAgIHRocm93IGlsbGVnYWwobmFtZSwgXCJuYW1lXCIpO1xyXG5cclxuICAgICAgICBza2lwKFwiPVwiKTtcclxuICAgICAgICB2YXIgZmllbGQgPSBuZXcgTWFwRmllbGQoYXBwbHlDYXNlKG5hbWUpLCBwYXJzZUlkKG5leHQoKSksIGtleVR5cGUsIHZhbHVlVHlwZSk7XHJcbiAgICAgICAgaWZCbG9jayhmaWVsZCwgZnVuY3Rpb24gcGFyc2VNYXBGaWVsZF9ibG9jayh0b2tlbikge1xyXG5cclxuICAgICAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGVsc2UgKi9cclxuICAgICAgICAgICAgaWYgKHRva2VuID09PSBcIm9wdGlvblwiKSB7XHJcbiAgICAgICAgICAgICAgICBwYXJzZU9wdGlvbihmaWVsZCwgdG9rZW4pO1xyXG4gICAgICAgICAgICAgICAgc2tpcChcIjtcIik7XHJcbiAgICAgICAgICAgIH0gZWxzZVxyXG4gICAgICAgICAgICAgICAgdGhyb3cgaWxsZWdhbCh0b2tlbik7XHJcblxyXG4gICAgICAgIH0sIGZ1bmN0aW9uIHBhcnNlTWFwRmllbGRfbGluZSgpIHtcclxuICAgICAgICAgICAgcGFyc2VJbmxpbmVPcHRpb25zKGZpZWxkKTtcclxuICAgICAgICB9KTtcclxuICAgICAgICBwYXJlbnQuYWRkKGZpZWxkKTtcclxuICAgIH1cclxuXHJcbiAgICBmdW5jdGlvbiBwYXJzZU9uZU9mKHBhcmVudCwgdG9rZW4pIHtcclxuXHJcbiAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICAgICAgaWYgKCFuYW1lUmUudGVzdCh0b2tlbiA9IG5leHQoKSkpXHJcbiAgICAgICAgICAgIHRocm93IGlsbGVnYWwodG9rZW4sIFwibmFtZVwiKTtcclxuXHJcbiAgICAgICAgdmFyIG9uZW9mID0gbmV3IE9uZU9mKGFwcGx5Q2FzZSh0b2tlbikpO1xyXG4gICAgICAgIGlmQmxvY2sob25lb2YsIGZ1bmN0aW9uIHBhcnNlT25lT2ZfYmxvY2sodG9rZW4pIHtcclxuICAgICAgICAgICAgaWYgKHRva2VuID09PSBcIm9wdGlvblwiKSB7XHJcbiAgICAgICAgICAgICAgICBwYXJzZU9wdGlvbihvbmVvZiwgdG9rZW4pO1xyXG4gICAgICAgICAgICAgICAgc2tpcChcIjtcIik7XHJcbiAgICAgICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgICAgICBwdXNoKHRva2VuKTtcclxuICAgICAgICAgICAgICAgIHBhcnNlRmllbGQob25lb2YsIFwib3B0aW9uYWxcIik7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9KTtcclxuICAgICAgICBwYXJlbnQuYWRkKG9uZW9mKTtcclxuICAgIH1cclxuXHJcbiAgICBmdW5jdGlvbiBwYXJzZUVudW0ocGFyZW50LCB0b2tlbikge1xyXG5cclxuICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgaWYgKi9cclxuICAgICAgICBpZiAoIW5hbWVSZS50ZXN0KHRva2VuID0gbmV4dCgpKSlcclxuICAgICAgICAgICAgdGhyb3cgaWxsZWdhbCh0b2tlbiwgXCJuYW1lXCIpO1xyXG5cclxuICAgICAgICB2YXIgZW5tID0gbmV3IEVudW0odG9rZW4pO1xyXG4gICAgICAgIGlmQmxvY2soZW5tLCBmdW5jdGlvbiBwYXJzZUVudW1fYmxvY2sodG9rZW4pIHtcclxuICAgICAgICAgIHN3aXRjaCh0b2tlbikge1xyXG4gICAgICAgICAgICBjYXNlIFwib3B0aW9uXCI6XHJcbiAgICAgICAgICAgICAgcGFyc2VPcHRpb24oZW5tLCB0b2tlbik7XHJcbiAgICAgICAgICAgICAgc2tpcChcIjtcIik7XHJcbiAgICAgICAgICAgICAgYnJlYWs7XHJcblxyXG4gICAgICAgICAgICBjYXNlIFwicmVzZXJ2ZWRcIjpcclxuICAgICAgICAgICAgICByZWFkUmFuZ2VzKGVubS5yZXNlcnZlZCB8fCAoZW5tLnJlc2VydmVkID0gW10pLCB0cnVlKTtcclxuICAgICAgICAgICAgICBicmVhaztcclxuXHJcbiAgICAgICAgICAgIGRlZmF1bHQ6XHJcbiAgICAgICAgICAgICAgcGFyc2VFbnVtVmFsdWUoZW5tLCB0b2tlbik7XHJcbiAgICAgICAgICB9XHJcbiAgICAgICAgfSk7XHJcbiAgICAgICAgcGFyZW50LmFkZChlbm0pO1xyXG4gICAgfVxyXG5cclxuICAgIGZ1bmN0aW9uIHBhcnNlRW51bVZhbHVlKHBhcmVudCwgdG9rZW4pIHtcclxuXHJcbiAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICAgICAgaWYgKCFuYW1lUmUudGVzdCh0b2tlbikpXHJcbiAgICAgICAgICAgIHRocm93IGlsbGVnYWwodG9rZW4sIFwibmFtZVwiKTtcclxuXHJcbiAgICAgICAgc2tpcChcIj1cIik7XHJcbiAgICAgICAgdmFyIHZhbHVlID0gcGFyc2VJZChuZXh0KCksIHRydWUpLFxyXG4gICAgICAgICAgICBkdW1teSA9IHtcclxuICAgICAgICAgICAgICAgIG9wdGlvbnM6IHVuZGVmaW5lZFxyXG4gICAgICAgICAgICB9O1xyXG4gICAgICAgIGR1bW15LnNldE9wdGlvbiA9IGZ1bmN0aW9uKG5hbWUsIHZhbHVlKSB7XHJcbiAgICAgICAgICAgIGlmICh0aGlzLm9wdGlvbnMgPT09IHVuZGVmaW5lZClcclxuICAgICAgICAgICAgICAgIHRoaXMub3B0aW9ucyA9IHt9O1xyXG4gICAgICAgICAgICB0aGlzLm9wdGlvbnNbbmFtZV0gPSB2YWx1ZTtcclxuICAgICAgICB9O1xyXG4gICAgICAgIGlmQmxvY2soZHVtbXksIGZ1bmN0aW9uIHBhcnNlRW51bVZhbHVlX2Jsb2NrKHRva2VuKSB7XHJcblxyXG4gICAgICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgZWxzZSAqL1xyXG4gICAgICAgICAgICBpZiAodG9rZW4gPT09IFwib3B0aW9uXCIpIHtcclxuICAgICAgICAgICAgICAgIHBhcnNlT3B0aW9uKGR1bW15LCB0b2tlbik7IC8vIHNraXBcclxuICAgICAgICAgICAgICAgIHNraXAoXCI7XCIpO1xyXG4gICAgICAgICAgICB9IGVsc2VcclxuICAgICAgICAgICAgICAgIHRocm93IGlsbGVnYWwodG9rZW4pO1xyXG5cclxuICAgICAgICB9LCBmdW5jdGlvbiBwYXJzZUVudW1WYWx1ZV9saW5lKCkge1xyXG4gICAgICAgICAgICBwYXJzZUlubGluZU9wdGlvbnMoZHVtbXkpOyAvLyBza2lwXHJcbiAgICAgICAgfSk7XHJcbiAgICAgICAgcGFyZW50LmFkZCh0b2tlbiwgdmFsdWUsIGR1bW15LmNvbW1lbnQsIGR1bW15Lm9wdGlvbnMpO1xyXG4gICAgfVxyXG5cclxuICAgIGZ1bmN0aW9uIHBhcnNlT3B0aW9uKHBhcmVudCwgdG9rZW4pIHtcclxuICAgICAgICB2YXIgaXNDdXN0b20gPSBza2lwKFwiKFwiLCB0cnVlKTtcclxuXHJcbiAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICAgICAgaWYgKCF0eXBlUmVmUmUudGVzdCh0b2tlbiA9IG5leHQoKSkpXHJcbiAgICAgICAgICAgIHRocm93IGlsbGVnYWwodG9rZW4sIFwibmFtZVwiKTtcclxuXHJcbiAgICAgICAgdmFyIG5hbWUgPSB0b2tlbjtcclxuICAgICAgICB2YXIgb3B0aW9uID0gbmFtZTtcclxuICAgICAgICB2YXIgcHJvcE5hbWU7XHJcblxyXG4gICAgICAgIGlmIChpc0N1c3RvbSkge1xyXG4gICAgICAgICAgICBza2lwKFwiKVwiKTtcclxuICAgICAgICAgICAgbmFtZSA9IFwiKFwiICsgbmFtZSArIFwiKVwiO1xyXG4gICAgICAgICAgICBvcHRpb24gPSBuYW1lO1xyXG4gICAgICAgICAgICB0b2tlbiA9IHBlZWsoKTtcclxuICAgICAgICAgICAgaWYgKGZxVHlwZVJlZlJlLnRlc3QodG9rZW4pKSB7XHJcbiAgICAgICAgICAgICAgICBwcm9wTmFtZSA9IHRva2VuLnNsaWNlKDEpOyAvL3JlbW92ZSAnLicgYmVmb3JlIHByb3BlcnR5IG5hbWVcclxuICAgICAgICAgICAgICAgIG5hbWUgKz0gdG9rZW47XHJcbiAgICAgICAgICAgICAgICBuZXh0KCk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICAgICAgc2tpcChcIj1cIik7XHJcbiAgICAgICAgdmFyIG9wdGlvblZhbHVlID0gcGFyc2VPcHRpb25WYWx1ZShwYXJlbnQsIG5hbWUpO1xyXG4gICAgICAgIHNldFBhcnNlZE9wdGlvbihwYXJlbnQsIG9wdGlvbiwgb3B0aW9uVmFsdWUsIHByb3BOYW1lKTtcclxuICAgIH1cclxuXHJcbiAgICBmdW5jdGlvbiBwYXJzZU9wdGlvblZhbHVlKHBhcmVudCwgbmFtZSkge1xyXG4gICAgICAgIC8vIHsgYTogXCJmb29cIiBiIHsgYzogXCJiYXJcIiB9IH1cclxuICAgICAgICBpZiAoc2tpcChcIntcIiwgdHJ1ZSkpIHtcclxuICAgICAgICAgICAgdmFyIG9iamVjdFJlc3VsdCA9IHt9O1xyXG5cclxuICAgICAgICAgICAgd2hpbGUgKCFza2lwKFwifVwiLCB0cnVlKSkge1xyXG4gICAgICAgICAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICAgICAgICAgICAgICBpZiAoIW5hbWVSZS50ZXN0KHRva2VuID0gbmV4dCgpKSkge1xyXG4gICAgICAgICAgICAgICAgICAgIHRocm93IGlsbGVnYWwodG9rZW4sIFwibmFtZVwiKTtcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgIGlmICh0b2tlbiA9PT0gbnVsbCkge1xyXG4gICAgICAgICAgICAgICAgICB0aHJvdyBpbGxlZ2FsKHRva2VuLCBcImVuZCBvZiBpbnB1dFwiKTtcclxuICAgICAgICAgICAgICAgIH1cclxuXHJcbiAgICAgICAgICAgICAgICB2YXIgdmFsdWU7XHJcbiAgICAgICAgICAgICAgICB2YXIgcHJvcE5hbWUgPSB0b2tlbjtcclxuXHJcbiAgICAgICAgICAgICAgICBza2lwKFwiOlwiLCB0cnVlKTtcclxuXHJcbiAgICAgICAgICAgICAgICBpZiAocGVlaygpID09PSBcIntcIilcclxuICAgICAgICAgICAgICAgICAgICB2YWx1ZSA9IHBhcnNlT3B0aW9uVmFsdWUocGFyZW50LCBuYW1lICsgXCIuXCIgKyB0b2tlbik7XHJcbiAgICAgICAgICAgICAgICBlbHNlIGlmIChwZWVrKCkgPT09IFwiW1wiKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgLy8gb3B0aW9uIChteV9vcHRpb24pID0ge1xyXG4gICAgICAgICAgICAgICAgICAgIC8vICAgICByZXBlYXRlZF92YWx1ZTogWyBcImZvb1wiLCBcImJhclwiIF1cclxuICAgICAgICAgICAgICAgICAgICAvLyB9O1xyXG4gICAgICAgICAgICAgICAgICAgIHZhbHVlID0gW107XHJcbiAgICAgICAgICAgICAgICAgICAgdmFyIGxhc3RWYWx1ZTtcclxuICAgICAgICAgICAgICAgICAgICBpZiAoc2tpcChcIltcIiwgdHJ1ZSkpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgZG8ge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgbGFzdFZhbHVlID0gcmVhZFZhbHVlKHRydWUpO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgdmFsdWUucHVzaChsYXN0VmFsdWUpO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICB9IHdoaWxlIChza2lwKFwiLFwiLCB0cnVlKSk7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHNraXAoXCJdXCIpO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICBpZiAodHlwZW9mIGxhc3RWYWx1ZSAhPT0gXCJ1bmRlZmluZWRcIikge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgc2V0T3B0aW9uKHBhcmVudCwgbmFtZSArIFwiLlwiICsgdG9rZW4sIGxhc3RWYWx1ZSk7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICAgICAgICAgIHZhbHVlID0gcmVhZFZhbHVlKHRydWUpO1xyXG4gICAgICAgICAgICAgICAgICAgIHNldE9wdGlvbihwYXJlbnQsIG5hbWUgKyBcIi5cIiArIHRva2VuLCB2YWx1ZSk7XHJcbiAgICAgICAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgICAgICAgdmFyIHByZXZWYWx1ZSA9IG9iamVjdFJlc3VsdFtwcm9wTmFtZV07XHJcblxyXG4gICAgICAgICAgICAgICAgaWYgKHByZXZWYWx1ZSlcclxuICAgICAgICAgICAgICAgICAgICB2YWx1ZSA9IFtdLmNvbmNhdChwcmV2VmFsdWUpLmNvbmNhdCh2YWx1ZSk7XHJcblxyXG4gICAgICAgICAgICAgICAgb2JqZWN0UmVzdWx0W3Byb3BOYW1lXSA9IHZhbHVlO1xyXG5cclxuICAgICAgICAgICAgICAgIC8vIFNlbWljb2xvbnMgYW5kIGNvbW1hcyBjYW4gYmUgb3B0aW9uYWxcclxuICAgICAgICAgICAgICAgIHNraXAoXCIsXCIsIHRydWUpO1xyXG4gICAgICAgICAgICAgICAgc2tpcChcIjtcIiwgdHJ1ZSk7XHJcbiAgICAgICAgICAgIH1cclxuXHJcbiAgICAgICAgICAgIHJldHVybiBvYmplY3RSZXN1bHQ7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICB2YXIgc2ltcGxlVmFsdWUgPSByZWFkVmFsdWUodHJ1ZSk7XHJcbiAgICAgICAgc2V0T3B0aW9uKHBhcmVudCwgbmFtZSwgc2ltcGxlVmFsdWUpO1xyXG4gICAgICAgIHJldHVybiBzaW1wbGVWYWx1ZTtcclxuICAgICAgICAvLyBEb2VzIG5vdCBlbmZvcmNlIGEgZGVsaW1pdGVyIHRvIGJlIHVuaXZlcnNhbFxyXG4gICAgfVxyXG5cclxuICAgIGZ1bmN0aW9uIHNldE9wdGlvbihwYXJlbnQsIG5hbWUsIHZhbHVlKSB7XHJcbiAgICAgICAgaWYgKHBhcmVudC5zZXRPcHRpb24pXHJcbiAgICAgICAgICAgIHBhcmVudC5zZXRPcHRpb24obmFtZSwgdmFsdWUpO1xyXG4gICAgfVxyXG5cclxuICAgIGZ1bmN0aW9uIHNldFBhcnNlZE9wdGlvbihwYXJlbnQsIG5hbWUsIHZhbHVlLCBwcm9wTmFtZSkge1xyXG4gICAgICAgIGlmIChwYXJlbnQuc2V0UGFyc2VkT3B0aW9uKVxyXG4gICAgICAgICAgICBwYXJlbnQuc2V0UGFyc2VkT3B0aW9uKG5hbWUsIHZhbHVlLCBwcm9wTmFtZSk7XHJcbiAgICB9XHJcblxyXG4gICAgZnVuY3Rpb24gcGFyc2VJbmxpbmVPcHRpb25zKHBhcmVudCkge1xyXG4gICAgICAgIGlmIChza2lwKFwiW1wiLCB0cnVlKSkge1xyXG4gICAgICAgICAgICBkbyB7XHJcbiAgICAgICAgICAgICAgICBwYXJzZU9wdGlvbihwYXJlbnQsIFwib3B0aW9uXCIpO1xyXG4gICAgICAgICAgICB9IHdoaWxlIChza2lwKFwiLFwiLCB0cnVlKSk7XHJcbiAgICAgICAgICAgIHNraXAoXCJdXCIpO1xyXG4gICAgICAgIH1cclxuICAgICAgICByZXR1cm4gcGFyZW50O1xyXG4gICAgfVxyXG5cclxuICAgIGZ1bmN0aW9uIHBhcnNlU2VydmljZShwYXJlbnQsIHRva2VuKSB7XHJcblxyXG4gICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgICAgIGlmICghbmFtZVJlLnRlc3QodG9rZW4gPSBuZXh0KCkpKVxyXG4gICAgICAgICAgICB0aHJvdyBpbGxlZ2FsKHRva2VuLCBcInNlcnZpY2UgbmFtZVwiKTtcclxuXHJcbiAgICAgICAgdmFyIHNlcnZpY2UgPSBuZXcgU2VydmljZSh0b2tlbik7XHJcbiAgICAgICAgaWZCbG9jayhzZXJ2aWNlLCBmdW5jdGlvbiBwYXJzZVNlcnZpY2VfYmxvY2sodG9rZW4pIHtcclxuICAgICAgICAgICAgaWYgKHBhcnNlQ29tbW9uKHNlcnZpY2UsIHRva2VuKSlcclxuICAgICAgICAgICAgICAgIHJldHVybjtcclxuXHJcbiAgICAgICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBlbHNlICovXHJcbiAgICAgICAgICAgIGlmICh0b2tlbiA9PT0gXCJycGNcIilcclxuICAgICAgICAgICAgICAgIHBhcnNlTWV0aG9kKHNlcnZpY2UsIHRva2VuKTtcclxuICAgICAgICAgICAgZWxzZVxyXG4gICAgICAgICAgICAgICAgdGhyb3cgaWxsZWdhbCh0b2tlbik7XHJcbiAgICAgICAgfSk7XHJcbiAgICAgICAgcGFyZW50LmFkZChzZXJ2aWNlKTtcclxuICAgIH1cclxuXHJcbiAgICBmdW5jdGlvbiBwYXJzZU1ldGhvZChwYXJlbnQsIHRva2VuKSB7XHJcbiAgICAgICAgLy8gR2V0IHRoZSBjb21tZW50IG9mIHRoZSBwcmVjZWRpbmcgbGluZSBub3cgKGlmIG9uZSBleGlzdHMpIGluIGNhc2UgdGhlXHJcbiAgICAgICAgLy8gbWV0aG9kIGlzIGRlZmluZWQgYWNyb3NzIG11bHRpcGxlIGxpbmVzLlxyXG4gICAgICAgIHZhciBjb21tZW50VGV4dCA9IGNtbnQoKTtcclxuXHJcbiAgICAgICAgdmFyIHR5cGUgPSB0b2tlbjtcclxuXHJcbiAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICAgICAgaWYgKCFuYW1lUmUudGVzdCh0b2tlbiA9IG5leHQoKSkpXHJcbiAgICAgICAgICAgIHRocm93IGlsbGVnYWwodG9rZW4sIFwibmFtZVwiKTtcclxuXHJcbiAgICAgICAgdmFyIG5hbWUgPSB0b2tlbixcclxuICAgICAgICAgICAgcmVxdWVzdFR5cGUsIHJlcXVlc3RTdHJlYW0sXHJcbiAgICAgICAgICAgIHJlc3BvbnNlVHlwZSwgcmVzcG9uc2VTdHJlYW07XHJcblxyXG4gICAgICAgIHNraXAoXCIoXCIpO1xyXG4gICAgICAgIGlmIChza2lwKFwic3RyZWFtXCIsIHRydWUpKVxyXG4gICAgICAgICAgICByZXF1ZXN0U3RyZWFtID0gdHJ1ZTtcclxuXHJcbiAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICAgICAgaWYgKCF0eXBlUmVmUmUudGVzdCh0b2tlbiA9IG5leHQoKSkpXHJcbiAgICAgICAgICAgIHRocm93IGlsbGVnYWwodG9rZW4pO1xyXG5cclxuICAgICAgICByZXF1ZXN0VHlwZSA9IHRva2VuO1xyXG4gICAgICAgIHNraXAoXCIpXCIpOyBza2lwKFwicmV0dXJuc1wiKTsgc2tpcChcIihcIik7XHJcbiAgICAgICAgaWYgKHNraXAoXCJzdHJlYW1cIiwgdHJ1ZSkpXHJcbiAgICAgICAgICAgIHJlc3BvbnNlU3RyZWFtID0gdHJ1ZTtcclxuXHJcbiAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICAgICAgaWYgKCF0eXBlUmVmUmUudGVzdCh0b2tlbiA9IG5leHQoKSkpXHJcbiAgICAgICAgICAgIHRocm93IGlsbGVnYWwodG9rZW4pO1xyXG5cclxuICAgICAgICByZXNwb25zZVR5cGUgPSB0b2tlbjtcclxuICAgICAgICBza2lwKFwiKVwiKTtcclxuXHJcbiAgICAgICAgdmFyIG1ldGhvZCA9IG5ldyBNZXRob2QobmFtZSwgdHlwZSwgcmVxdWVzdFR5cGUsIHJlc3BvbnNlVHlwZSwgcmVxdWVzdFN0cmVhbSwgcmVzcG9uc2VTdHJlYW0pO1xyXG4gICAgICAgIG1ldGhvZC5jb21tZW50ID0gY29tbWVudFRleHQ7XHJcbiAgICAgICAgaWZCbG9jayhtZXRob2QsIGZ1bmN0aW9uIHBhcnNlTWV0aG9kX2Jsb2NrKHRva2VuKSB7XHJcblxyXG4gICAgICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgZWxzZSAqL1xyXG4gICAgICAgICAgICBpZiAodG9rZW4gPT09IFwib3B0aW9uXCIpIHtcclxuICAgICAgICAgICAgICAgIHBhcnNlT3B0aW9uKG1ldGhvZCwgdG9rZW4pO1xyXG4gICAgICAgICAgICAgICAgc2tpcChcIjtcIik7XHJcbiAgICAgICAgICAgIH0gZWxzZVxyXG4gICAgICAgICAgICAgICAgdGhyb3cgaWxsZWdhbCh0b2tlbik7XHJcblxyXG4gICAgICAgIH0pO1xyXG4gICAgICAgIHBhcmVudC5hZGQobWV0aG9kKTtcclxuICAgIH1cclxuXHJcbiAgICBmdW5jdGlvbiBwYXJzZUV4dGVuc2lvbihwYXJlbnQsIHRva2VuKSB7XHJcblxyXG4gICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgICAgIGlmICghdHlwZVJlZlJlLnRlc3QodG9rZW4gPSBuZXh0KCkpKVxyXG4gICAgICAgICAgICB0aHJvdyBpbGxlZ2FsKHRva2VuLCBcInJlZmVyZW5jZVwiKTtcclxuXHJcbiAgICAgICAgdmFyIHJlZmVyZW5jZSA9IHRva2VuO1xyXG4gICAgICAgIGlmQmxvY2sobnVsbCwgZnVuY3Rpb24gcGFyc2VFeHRlbnNpb25fYmxvY2sodG9rZW4pIHtcclxuICAgICAgICAgICAgc3dpdGNoICh0b2tlbikge1xyXG5cclxuICAgICAgICAgICAgICAgIGNhc2UgXCJyZXF1aXJlZFwiOlxyXG4gICAgICAgICAgICAgICAgY2FzZSBcInJlcGVhdGVkXCI6XHJcbiAgICAgICAgICAgICAgICAgICAgcGFyc2VGaWVsZChwYXJlbnQsIHRva2VuLCByZWZlcmVuY2UpO1xyXG4gICAgICAgICAgICAgICAgICAgIGJyZWFrO1xyXG5cclxuICAgICAgICAgICAgICAgIGNhc2UgXCJvcHRpb25hbFwiOlxyXG4gICAgICAgICAgICAgICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgICAgICAgICAgICAgICAgIGlmIChpc1Byb3RvMykge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICBwYXJzZUZpZWxkKHBhcmVudCwgXCJwcm90bzNfb3B0aW9uYWxcIiwgcmVmZXJlbmNlKTtcclxuICAgICAgICAgICAgICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICBwYXJzZUZpZWxkKHBhcmVudCwgXCJvcHRpb25hbFwiLCByZWZlcmVuY2UpO1xyXG4gICAgICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgICAgICBicmVhaztcclxuXHJcbiAgICAgICAgICAgICAgICBkZWZhdWx0OlxyXG4gICAgICAgICAgICAgICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgICAgICAgICAgICAgICAgIGlmICghaXNQcm90bzMgfHwgIXR5cGVSZWZSZS50ZXN0KHRva2VuKSlcclxuICAgICAgICAgICAgICAgICAgICAgICAgdGhyb3cgaWxsZWdhbCh0b2tlbik7XHJcbiAgICAgICAgICAgICAgICAgICAgcHVzaCh0b2tlbik7XHJcbiAgICAgICAgICAgICAgICAgICAgcGFyc2VGaWVsZChwYXJlbnQsIFwib3B0aW9uYWxcIiwgcmVmZXJlbmNlKTtcclxuICAgICAgICAgICAgICAgICAgICBicmVhaztcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH0pO1xyXG4gICAgfVxyXG5cclxuICAgIHZhciB0b2tlbjtcclxuICAgIHdoaWxlICgodG9rZW4gPSBuZXh0KCkpICE9PSBudWxsKSB7XHJcbiAgICAgICAgc3dpdGNoICh0b2tlbikge1xyXG5cclxuICAgICAgICAgICAgY2FzZSBcInBhY2thZ2VcIjpcclxuXHJcbiAgICAgICAgICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgaWYgKi9cclxuICAgICAgICAgICAgICAgIGlmICghaGVhZClcclxuICAgICAgICAgICAgICAgICAgICB0aHJvdyBpbGxlZ2FsKHRva2VuKTtcclxuXHJcbiAgICAgICAgICAgICAgICBwYXJzZVBhY2thZ2UoKTtcclxuICAgICAgICAgICAgICAgIGJyZWFrO1xyXG5cclxuICAgICAgICAgICAgY2FzZSBcImltcG9ydFwiOlxyXG5cclxuICAgICAgICAgICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgICAgICAgICAgICAgaWYgKCFoZWFkKVxyXG4gICAgICAgICAgICAgICAgICAgIHRocm93IGlsbGVnYWwodG9rZW4pO1xyXG5cclxuICAgICAgICAgICAgICAgIHBhcnNlSW1wb3J0KCk7XHJcbiAgICAgICAgICAgICAgICBicmVhaztcclxuXHJcbiAgICAgICAgICAgIGNhc2UgXCJzeW50YXhcIjpcclxuXHJcbiAgICAgICAgICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgaWYgKi9cclxuICAgICAgICAgICAgICAgIGlmICghaGVhZClcclxuICAgICAgICAgICAgICAgICAgICB0aHJvdyBpbGxlZ2FsKHRva2VuKTtcclxuXHJcbiAgICAgICAgICAgICAgICBwYXJzZVN5bnRheCgpO1xyXG4gICAgICAgICAgICAgICAgYnJlYWs7XHJcblxyXG4gICAgICAgICAgICBjYXNlIFwib3B0aW9uXCI6XHJcblxyXG4gICAgICAgICAgICAgICAgcGFyc2VPcHRpb24ocHRyLCB0b2tlbik7XHJcbiAgICAgICAgICAgICAgICBza2lwKFwiO1wiKTtcclxuICAgICAgICAgICAgICAgIGJyZWFrO1xyXG5cclxuICAgICAgICAgICAgZGVmYXVsdDpcclxuXHJcbiAgICAgICAgICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgZWxzZSAqL1xyXG4gICAgICAgICAgICAgICAgaWYgKHBhcnNlQ29tbW9uKHB0ciwgdG9rZW4pKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgaGVhZCA9IGZhbHNlO1xyXG4gICAgICAgICAgICAgICAgICAgIGNvbnRpbnVlO1xyXG4gICAgICAgICAgICAgICAgfVxyXG5cclxuICAgICAgICAgICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0ICovXHJcbiAgICAgICAgICAgICAgICB0aHJvdyBpbGxlZ2FsKHRva2VuKTtcclxuICAgICAgICB9XHJcbiAgICB9XHJcblxyXG4gICAgcGFyc2UuZmlsZW5hbWUgPSBudWxsO1xyXG4gICAgcmV0dXJuIHtcclxuICAgICAgICBcInBhY2thZ2VcIiAgICAgOiBwa2csXHJcbiAgICAgICAgXCJpbXBvcnRzXCIgICAgIDogaW1wb3J0cyxcclxuICAgICAgICAgd2Vha0ltcG9ydHMgIDogd2Vha0ltcG9ydHMsXHJcbiAgICAgICAgIHN5bnRheCAgICAgICA6IHN5bnRheCxcclxuICAgICAgICAgcm9vdCAgICAgICAgIDogcm9vdFxyXG4gICAgfTtcclxufVxyXG5cclxuLyoqXHJcbiAqIFBhcnNlcyB0aGUgZ2l2ZW4gLnByb3RvIHNvdXJjZSBhbmQgcmV0dXJucyBhbiBvYmplY3Qgd2l0aCB0aGUgcGFyc2VkIGNvbnRlbnRzLlxyXG4gKiBAbmFtZSBwYXJzZVxyXG4gKiBAZnVuY3Rpb25cclxuICogQHBhcmFtIHtzdHJpbmd9IHNvdXJjZSBTb3VyY2UgY29udGVudHNcclxuICogQHBhcmFtIHtJUGFyc2VPcHRpb25zfSBbb3B0aW9uc10gUGFyc2Ugb3B0aW9ucy4gRGVmYXVsdHMgdG8ge0BsaW5rIHBhcnNlLmRlZmF1bHRzfSB3aGVuIG9taXR0ZWQuXHJcbiAqIEByZXR1cm5zIHtJUGFyc2VyUmVzdWx0fSBQYXJzZXIgcmVzdWx0XHJcbiAqIEBwcm9wZXJ0eSB7c3RyaW5nfSBmaWxlbmFtZT1udWxsIEN1cnJlbnRseSBwcm9jZXNzaW5nIGZpbGUgbmFtZSBmb3IgZXJyb3IgcmVwb3J0aW5nLCBpZiBrbm93blxyXG4gKiBAcHJvcGVydHkge0lQYXJzZU9wdGlvbnN9IGRlZmF1bHRzIERlZmF1bHQge0BsaW5rIElQYXJzZU9wdGlvbnN9XHJcbiAqIEB2YXJpYXRpb24gMlxyXG4gKi9cclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbm1vZHVsZS5leHBvcnRzID0gUmVhZGVyO1xyXG5cclxudmFyIHV0aWwgICAgICA9IHJlcXVpcmUoXCIuL3V0aWwvbWluaW1hbFwiKTtcclxuXHJcbnZhciBCdWZmZXJSZWFkZXI7IC8vIGN5Y2xpY1xyXG5cclxudmFyIExvbmdCaXRzICA9IHV0aWwuTG9uZ0JpdHMsXHJcbiAgICB1dGY4ICAgICAgPSB1dGlsLnV0Zjg7XHJcblxyXG4vKiBpc3RhbmJ1bCBpZ25vcmUgbmV4dCAqL1xyXG5mdW5jdGlvbiBpbmRleE91dE9mUmFuZ2UocmVhZGVyLCB3cml0ZUxlbmd0aCkge1xyXG4gICAgcmV0dXJuIFJhbmdlRXJyb3IoXCJpbmRleCBvdXQgb2YgcmFuZ2U6IFwiICsgcmVhZGVyLnBvcyArIFwiICsgXCIgKyAod3JpdGVMZW5ndGggfHwgMSkgKyBcIiA+IFwiICsgcmVhZGVyLmxlbik7XHJcbn1cclxuXHJcbi8qKlxyXG4gKiBDb25zdHJ1Y3RzIGEgbmV3IHJlYWRlciBpbnN0YW5jZSB1c2luZyB0aGUgc3BlY2lmaWVkIGJ1ZmZlci5cclxuICogQGNsYXNzZGVzYyBXaXJlIGZvcm1hdCByZWFkZXIgdXNpbmcgYFVpbnQ4QXJyYXlgIGlmIGF2YWlsYWJsZSwgb3RoZXJ3aXNlIGBBcnJheWAuXHJcbiAqIEBjb25zdHJ1Y3RvclxyXG4gKiBAcGFyYW0ge1VpbnQ4QXJyYXl9IGJ1ZmZlciBCdWZmZXIgdG8gcmVhZCBmcm9tXHJcbiAqL1xyXG5mdW5jdGlvbiBSZWFkZXIoYnVmZmVyKSB7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBSZWFkIGJ1ZmZlci5cclxuICAgICAqIEB0eXBlIHtVaW50OEFycmF5fVxyXG4gICAgICovXHJcbiAgICB0aGlzLmJ1ZiA9IGJ1ZmZlcjtcclxuXHJcbiAgICAvKipcclxuICAgICAqIFJlYWQgYnVmZmVyIHBvc2l0aW9uLlxyXG4gICAgICogQHR5cGUge251bWJlcn1cclxuICAgICAqL1xyXG4gICAgdGhpcy5wb3MgPSAwO1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmVhZCBidWZmZXIgbGVuZ3RoLlxyXG4gICAgICogQHR5cGUge251bWJlcn1cclxuICAgICAqL1xyXG4gICAgdGhpcy5sZW4gPSBidWZmZXIubGVuZ3RoO1xyXG59XHJcblxyXG52YXIgY3JlYXRlX2FycmF5ID0gdHlwZW9mIFVpbnQ4QXJyYXkgIT09IFwidW5kZWZpbmVkXCJcclxuICAgID8gZnVuY3Rpb24gY3JlYXRlX3R5cGVkX2FycmF5KGJ1ZmZlcikge1xyXG4gICAgICAgIGlmIChidWZmZXIgaW5zdGFuY2VvZiBVaW50OEFycmF5IHx8IEFycmF5LmlzQXJyYXkoYnVmZmVyKSlcclxuICAgICAgICAgICAgcmV0dXJuIG5ldyBSZWFkZXIoYnVmZmVyKTtcclxuICAgICAgICB0aHJvdyBFcnJvcihcImlsbGVnYWwgYnVmZmVyXCIpO1xyXG4gICAgfVxyXG4gICAgLyogaXN0YW5idWwgaWdub3JlIG5leHQgKi9cclxuICAgIDogZnVuY3Rpb24gY3JlYXRlX2FycmF5KGJ1ZmZlcikge1xyXG4gICAgICAgIGlmIChBcnJheS5pc0FycmF5KGJ1ZmZlcikpXHJcbiAgICAgICAgICAgIHJldHVybiBuZXcgUmVhZGVyKGJ1ZmZlcik7XHJcbiAgICAgICAgdGhyb3cgRXJyb3IoXCJpbGxlZ2FsIGJ1ZmZlclwiKTtcclxuICAgIH07XHJcblxyXG52YXIgY3JlYXRlID0gZnVuY3Rpb24gY3JlYXRlKCkge1xyXG4gICAgcmV0dXJuIHV0aWwuQnVmZmVyXHJcbiAgICAgICAgPyBmdW5jdGlvbiBjcmVhdGVfYnVmZmVyX3NldHVwKGJ1ZmZlcikge1xyXG4gICAgICAgICAgICByZXR1cm4gKFJlYWRlci5jcmVhdGUgPSBmdW5jdGlvbiBjcmVhdGVfYnVmZmVyKGJ1ZmZlcikge1xyXG4gICAgICAgICAgICAgICAgcmV0dXJuIHV0aWwuQnVmZmVyLmlzQnVmZmVyKGJ1ZmZlcilcclxuICAgICAgICAgICAgICAgICAgICA/IG5ldyBCdWZmZXJSZWFkZXIoYnVmZmVyKVxyXG4gICAgICAgICAgICAgICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0ICovXHJcbiAgICAgICAgICAgICAgICAgICAgOiBjcmVhdGVfYXJyYXkoYnVmZmVyKTtcclxuICAgICAgICAgICAgfSkoYnVmZmVyKTtcclxuICAgICAgICB9XHJcbiAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIG5leHQgKi9cclxuICAgICAgICA6IGNyZWF0ZV9hcnJheTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBDcmVhdGVzIGEgbmV3IHJlYWRlciB1c2luZyB0aGUgc3BlY2lmaWVkIGJ1ZmZlci5cclxuICogQGZ1bmN0aW9uXHJcbiAqIEBwYXJhbSB7VWludDhBcnJheXxCdWZmZXJ9IGJ1ZmZlciBCdWZmZXIgdG8gcmVhZCBmcm9tXHJcbiAqIEByZXR1cm5zIHtSZWFkZXJ8QnVmZmVyUmVhZGVyfSBBIHtAbGluayBCdWZmZXJSZWFkZXJ9IGlmIGBidWZmZXJgIGlzIGEgQnVmZmVyLCBvdGhlcndpc2UgYSB7QGxpbmsgUmVhZGVyfVxyXG4gKiBAdGhyb3dzIHtFcnJvcn0gSWYgYGJ1ZmZlcmAgaXMgbm90IGEgdmFsaWQgYnVmZmVyXHJcbiAqL1xyXG5SZWFkZXIuY3JlYXRlID0gY3JlYXRlKCk7XHJcblxyXG5SZWFkZXIucHJvdG90eXBlLl9zbGljZSA9IHV0aWwuQXJyYXkucHJvdG90eXBlLnN1YmFycmF5IHx8IC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0ICovIHV0aWwuQXJyYXkucHJvdG90eXBlLnNsaWNlO1xyXG5cclxuLyoqXHJcbiAqIFJlYWRzIGEgdmFyaW50IGFzIGFuIHVuc2lnbmVkIDMyIGJpdCB2YWx1ZS5cclxuICogQGZ1bmN0aW9uXHJcbiAqIEByZXR1cm5zIHtudW1iZXJ9IFZhbHVlIHJlYWRcclxuICovXHJcblJlYWRlci5wcm90b3R5cGUudWludDMyID0gKGZ1bmN0aW9uIHJlYWRfdWludDMyX3NldHVwKCkge1xyXG4gICAgdmFyIHZhbHVlID0gNDI5NDk2NzI5NTsgLy8gb3B0aW1pemVyIHR5cGUtaGludCwgdGVuZHMgdG8gZGVvcHQgb3RoZXJ3aXNlICg/ISlcclxuICAgIHJldHVybiBmdW5jdGlvbiByZWFkX3VpbnQzMigpIHtcclxuICAgICAgICB2YWx1ZSA9ICggICAgICAgICB0aGlzLmJ1Zlt0aGlzLnBvc10gJiAxMjcgICAgICAgKSA+Pj4gMDsgaWYgKHRoaXMuYnVmW3RoaXMucG9zKytdIDwgMTI4KSByZXR1cm4gdmFsdWU7XHJcbiAgICAgICAgdmFsdWUgPSAodmFsdWUgfCAodGhpcy5idWZbdGhpcy5wb3NdICYgMTI3KSA8PCAgNykgPj4+IDA7IGlmICh0aGlzLmJ1Zlt0aGlzLnBvcysrXSA8IDEyOCkgcmV0dXJuIHZhbHVlO1xyXG4gICAgICAgIHZhbHVlID0gKHZhbHVlIHwgKHRoaXMuYnVmW3RoaXMucG9zXSAmIDEyNykgPDwgMTQpID4+PiAwOyBpZiAodGhpcy5idWZbdGhpcy5wb3MrK10gPCAxMjgpIHJldHVybiB2YWx1ZTtcclxuICAgICAgICB2YWx1ZSA9ICh2YWx1ZSB8ICh0aGlzLmJ1Zlt0aGlzLnBvc10gJiAxMjcpIDw8IDIxKSA+Pj4gMDsgaWYgKHRoaXMuYnVmW3RoaXMucG9zKytdIDwgMTI4KSByZXR1cm4gdmFsdWU7XHJcbiAgICAgICAgdmFsdWUgPSAodmFsdWUgfCAodGhpcy5idWZbdGhpcy5wb3NdICYgIDE1KSA8PCAyOCkgPj4+IDA7IGlmICh0aGlzLmJ1Zlt0aGlzLnBvcysrXSA8IDEyOCkgcmV0dXJuIHZhbHVlO1xyXG5cclxuICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgaWYgKi9cclxuICAgICAgICBpZiAoKHRoaXMucG9zICs9IDUpID4gdGhpcy5sZW4pIHtcclxuICAgICAgICAgICAgdGhpcy5wb3MgPSB0aGlzLmxlbjtcclxuICAgICAgICAgICAgdGhyb3cgaW5kZXhPdXRPZlJhbmdlKHRoaXMsIDEwKTtcclxuICAgICAgICB9XHJcbiAgICAgICAgcmV0dXJuIHZhbHVlO1xyXG4gICAgfTtcclxufSkoKTtcclxuXHJcbi8qKlxyXG4gKiBSZWFkcyBhIHZhcmludCBhcyBhIHNpZ25lZCAzMiBiaXQgdmFsdWUuXHJcbiAqIEByZXR1cm5zIHtudW1iZXJ9IFZhbHVlIHJlYWRcclxuICovXHJcblJlYWRlci5wcm90b3R5cGUuaW50MzIgPSBmdW5jdGlvbiByZWFkX2ludDMyKCkge1xyXG4gICAgcmV0dXJuIHRoaXMudWludDMyKCkgfCAwO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIFJlYWRzIGEgemlnLXphZyBlbmNvZGVkIHZhcmludCBhcyBhIHNpZ25lZCAzMiBiaXQgdmFsdWUuXHJcbiAqIEByZXR1cm5zIHtudW1iZXJ9IFZhbHVlIHJlYWRcclxuICovXHJcblJlYWRlci5wcm90b3R5cGUuc2ludDMyID0gZnVuY3Rpb24gcmVhZF9zaW50MzIoKSB7XHJcbiAgICB2YXIgdmFsdWUgPSB0aGlzLnVpbnQzMigpO1xyXG4gICAgcmV0dXJuIHZhbHVlID4+PiAxIF4gLSh2YWx1ZSAmIDEpIHwgMDtcclxufTtcclxuXHJcbi8qIGVzbGludC1kaXNhYmxlIG5vLWludmFsaWQtdGhpcyAqL1xyXG5cclxuZnVuY3Rpb24gcmVhZExvbmdWYXJpbnQoKSB7XHJcbiAgICAvLyB0ZW5kcyB0byBkZW9wdCB3aXRoIGxvY2FsIHZhcnMgZm9yIG9jdGV0IGV0Yy5cclxuICAgIHZhciBiaXRzID0gbmV3IExvbmdCaXRzKDAsIDApO1xyXG4gICAgdmFyIGkgPSAwO1xyXG4gICAgaWYgKHRoaXMubGVuIC0gdGhpcy5wb3MgPiA0KSB7IC8vIGZhc3Qgcm91dGUgKGxvKVxyXG4gICAgICAgIGZvciAoOyBpIDwgNDsgKytpKSB7XHJcbiAgICAgICAgICAgIC8vIDFzdC4uNHRoXHJcbiAgICAgICAgICAgIGJpdHMubG8gPSAoYml0cy5sbyB8ICh0aGlzLmJ1Zlt0aGlzLnBvc10gJiAxMjcpIDw8IGkgKiA3KSA+Pj4gMDtcclxuICAgICAgICAgICAgaWYgKHRoaXMuYnVmW3RoaXMucG9zKytdIDwgMTI4KVxyXG4gICAgICAgICAgICAgICAgcmV0dXJuIGJpdHM7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIC8vIDV0aFxyXG4gICAgICAgIGJpdHMubG8gPSAoYml0cy5sbyB8ICh0aGlzLmJ1Zlt0aGlzLnBvc10gJiAxMjcpIDw8IDI4KSA+Pj4gMDtcclxuICAgICAgICBiaXRzLmhpID0gKGJpdHMuaGkgfCAodGhpcy5idWZbdGhpcy5wb3NdICYgMTI3KSA+PiAgNCkgPj4+IDA7XHJcbiAgICAgICAgaWYgKHRoaXMuYnVmW3RoaXMucG9zKytdIDwgMTI4KVxyXG4gICAgICAgICAgICByZXR1cm4gYml0cztcclxuICAgICAgICBpID0gMDtcclxuICAgIH0gZWxzZSB7XHJcbiAgICAgICAgZm9yICg7IGkgPCAzOyArK2kpIHtcclxuICAgICAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICAgICAgICAgIGlmICh0aGlzLnBvcyA+PSB0aGlzLmxlbilcclxuICAgICAgICAgICAgICAgIHRocm93IGluZGV4T3V0T2ZSYW5nZSh0aGlzKTtcclxuICAgICAgICAgICAgLy8gMXN0Li4zdGhcclxuICAgICAgICAgICAgYml0cy5sbyA9IChiaXRzLmxvIHwgKHRoaXMuYnVmW3RoaXMucG9zXSAmIDEyNykgPDwgaSAqIDcpID4+PiAwO1xyXG4gICAgICAgICAgICBpZiAodGhpcy5idWZbdGhpcy5wb3MrK10gPCAxMjgpXHJcbiAgICAgICAgICAgICAgICByZXR1cm4gYml0cztcclxuICAgICAgICB9XHJcbiAgICAgICAgLy8gNHRoXHJcbiAgICAgICAgYml0cy5sbyA9IChiaXRzLmxvIHwgKHRoaXMuYnVmW3RoaXMucG9zKytdICYgMTI3KSA8PCBpICogNykgPj4+IDA7XHJcbiAgICAgICAgcmV0dXJuIGJpdHM7XHJcbiAgICB9XHJcbiAgICBpZiAodGhpcy5sZW4gLSB0aGlzLnBvcyA+IDQpIHsgLy8gZmFzdCByb3V0ZSAoaGkpXHJcbiAgICAgICAgZm9yICg7IGkgPCA1OyArK2kpIHtcclxuICAgICAgICAgICAgLy8gNnRoLi4xMHRoXHJcbiAgICAgICAgICAgIGJpdHMuaGkgPSAoYml0cy5oaSB8ICh0aGlzLmJ1Zlt0aGlzLnBvc10gJiAxMjcpIDw8IGkgKiA3ICsgMykgPj4+IDA7XHJcbiAgICAgICAgICAgIGlmICh0aGlzLmJ1Zlt0aGlzLnBvcysrXSA8IDEyOClcclxuICAgICAgICAgICAgICAgIHJldHVybiBiaXRzO1xyXG4gICAgICAgIH1cclxuICAgIH0gZWxzZSB7XHJcbiAgICAgICAgZm9yICg7IGkgPCA1OyArK2kpIHtcclxuICAgICAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICAgICAgICAgIGlmICh0aGlzLnBvcyA+PSB0aGlzLmxlbilcclxuICAgICAgICAgICAgICAgIHRocm93IGluZGV4T3V0T2ZSYW5nZSh0aGlzKTtcclxuICAgICAgICAgICAgLy8gNnRoLi4xMHRoXHJcbiAgICAgICAgICAgIGJpdHMuaGkgPSAoYml0cy5oaSB8ICh0aGlzLmJ1Zlt0aGlzLnBvc10gJiAxMjcpIDw8IGkgKiA3ICsgMykgPj4+IDA7XHJcbiAgICAgICAgICAgIGlmICh0aGlzLmJ1Zlt0aGlzLnBvcysrXSA8IDEyOClcclxuICAgICAgICAgICAgICAgIHJldHVybiBiaXRzO1xyXG4gICAgICAgIH1cclxuICAgIH1cclxuICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0ICovXHJcbiAgICB0aHJvdyBFcnJvcihcImludmFsaWQgdmFyaW50IGVuY29kaW5nXCIpO1xyXG59XHJcblxyXG4vKiBlc2xpbnQtZW5hYmxlIG5vLWludmFsaWQtdGhpcyAqL1xyXG5cclxuLyoqXHJcbiAqIFJlYWRzIGEgdmFyaW50IGFzIGEgc2lnbmVkIDY0IGJpdCB2YWx1ZS5cclxuICogQG5hbWUgUmVhZGVyI2ludDY0XHJcbiAqIEBmdW5jdGlvblxyXG4gKiBAcmV0dXJucyB7TG9uZ30gVmFsdWUgcmVhZFxyXG4gKi9cclxuXHJcbi8qKlxyXG4gKiBSZWFkcyBhIHZhcmludCBhcyBhbiB1bnNpZ25lZCA2NCBiaXQgdmFsdWUuXHJcbiAqIEBuYW1lIFJlYWRlciN1aW50NjRcclxuICogQGZ1bmN0aW9uXHJcbiAqIEByZXR1cm5zIHtMb25nfSBWYWx1ZSByZWFkXHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIFJlYWRzIGEgemlnLXphZyBlbmNvZGVkIHZhcmludCBhcyBhIHNpZ25lZCA2NCBiaXQgdmFsdWUuXHJcbiAqIEBuYW1lIFJlYWRlciNzaW50NjRcclxuICogQGZ1bmN0aW9uXHJcbiAqIEByZXR1cm5zIHtMb25nfSBWYWx1ZSByZWFkXHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIFJlYWRzIGEgdmFyaW50IGFzIGEgYm9vbGVhbi5cclxuICogQHJldHVybnMge2Jvb2xlYW59IFZhbHVlIHJlYWRcclxuICovXHJcblJlYWRlci5wcm90b3R5cGUuYm9vbCA9IGZ1bmN0aW9uIHJlYWRfYm9vbCgpIHtcclxuICAgIHJldHVybiB0aGlzLnVpbnQzMigpICE9PSAwO1xyXG59O1xyXG5cclxuZnVuY3Rpb24gcmVhZEZpeGVkMzJfZW5kKGJ1ZiwgZW5kKSB7IC8vIG5vdGUgdGhhdCB0aGlzIHVzZXMgYGVuZGAsIG5vdCBgcG9zYFxyXG4gICAgcmV0dXJuIChidWZbZW5kIC0gNF1cclxuICAgICAgICAgIHwgYnVmW2VuZCAtIDNdIDw8IDhcclxuICAgICAgICAgIHwgYnVmW2VuZCAtIDJdIDw8IDE2XHJcbiAgICAgICAgICB8IGJ1ZltlbmQgLSAxXSA8PCAyNCkgPj4+IDA7XHJcbn1cclxuXHJcbi8qKlxyXG4gKiBSZWFkcyBmaXhlZCAzMiBiaXRzIGFzIGFuIHVuc2lnbmVkIDMyIGJpdCBpbnRlZ2VyLlxyXG4gKiBAcmV0dXJucyB7bnVtYmVyfSBWYWx1ZSByZWFkXHJcbiAqL1xyXG5SZWFkZXIucHJvdG90eXBlLmZpeGVkMzIgPSBmdW5jdGlvbiByZWFkX2ZpeGVkMzIoKSB7XHJcblxyXG4gICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICBpZiAodGhpcy5wb3MgKyA0ID4gdGhpcy5sZW4pXHJcbiAgICAgICAgdGhyb3cgaW5kZXhPdXRPZlJhbmdlKHRoaXMsIDQpO1xyXG5cclxuICAgIHJldHVybiByZWFkRml4ZWQzMl9lbmQodGhpcy5idWYsIHRoaXMucG9zICs9IDQpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIFJlYWRzIGZpeGVkIDMyIGJpdHMgYXMgYSBzaWduZWQgMzIgYml0IGludGVnZXIuXHJcbiAqIEByZXR1cm5zIHtudW1iZXJ9IFZhbHVlIHJlYWRcclxuICovXHJcblJlYWRlci5wcm90b3R5cGUuc2ZpeGVkMzIgPSBmdW5jdGlvbiByZWFkX3NmaXhlZDMyKCkge1xyXG5cclxuICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgaWYgKHRoaXMucG9zICsgNCA+IHRoaXMubGVuKVxyXG4gICAgICAgIHRocm93IGluZGV4T3V0T2ZSYW5nZSh0aGlzLCA0KTtcclxuXHJcbiAgICByZXR1cm4gcmVhZEZpeGVkMzJfZW5kKHRoaXMuYnVmLCB0aGlzLnBvcyArPSA0KSB8IDA7XHJcbn07XHJcblxyXG4vKiBlc2xpbnQtZGlzYWJsZSBuby1pbnZhbGlkLXRoaXMgKi9cclxuXHJcbmZ1bmN0aW9uIHJlYWRGaXhlZDY0KC8qIHRoaXM6IFJlYWRlciAqLykge1xyXG5cclxuICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgaWYgKHRoaXMucG9zICsgOCA+IHRoaXMubGVuKVxyXG4gICAgICAgIHRocm93IGluZGV4T3V0T2ZSYW5nZSh0aGlzLCA4KTtcclxuXHJcbiAgICByZXR1cm4gbmV3IExvbmdCaXRzKHJlYWRGaXhlZDMyX2VuZCh0aGlzLmJ1ZiwgdGhpcy5wb3MgKz0gNCksIHJlYWRGaXhlZDMyX2VuZCh0aGlzLmJ1ZiwgdGhpcy5wb3MgKz0gNCkpO1xyXG59XHJcblxyXG4vKiBlc2xpbnQtZW5hYmxlIG5vLWludmFsaWQtdGhpcyAqL1xyXG5cclxuLyoqXHJcbiAqIFJlYWRzIGZpeGVkIDY0IGJpdHMuXHJcbiAqIEBuYW1lIFJlYWRlciNmaXhlZDY0XHJcbiAqIEBmdW5jdGlvblxyXG4gKiBAcmV0dXJucyB7TG9uZ30gVmFsdWUgcmVhZFxyXG4gKi9cclxuXHJcbi8qKlxyXG4gKiBSZWFkcyB6aWctemFnIGVuY29kZWQgZml4ZWQgNjQgYml0cy5cclxuICogQG5hbWUgUmVhZGVyI3NmaXhlZDY0XHJcbiAqIEBmdW5jdGlvblxyXG4gKiBAcmV0dXJucyB7TG9uZ30gVmFsdWUgcmVhZFxyXG4gKi9cclxuXHJcbi8qKlxyXG4gKiBSZWFkcyBhIGZsb2F0ICgzMiBiaXQpIGFzIGEgbnVtYmVyLlxyXG4gKiBAZnVuY3Rpb25cclxuICogQHJldHVybnMge251bWJlcn0gVmFsdWUgcmVhZFxyXG4gKi9cclxuUmVhZGVyLnByb3RvdHlwZS5mbG9hdCA9IGZ1bmN0aW9uIHJlYWRfZmxvYXQoKSB7XHJcblxyXG4gICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICBpZiAodGhpcy5wb3MgKyA0ID4gdGhpcy5sZW4pXHJcbiAgICAgICAgdGhyb3cgaW5kZXhPdXRPZlJhbmdlKHRoaXMsIDQpO1xyXG5cclxuICAgIHZhciB2YWx1ZSA9IHV0aWwuZmxvYXQucmVhZEZsb2F0TEUodGhpcy5idWYsIHRoaXMucG9zKTtcclxuICAgIHRoaXMucG9zICs9IDQ7XHJcbiAgICByZXR1cm4gdmFsdWU7XHJcbn07XHJcblxyXG4vKipcclxuICogUmVhZHMgYSBkb3VibGUgKDY0IGJpdCBmbG9hdCkgYXMgYSBudW1iZXIuXHJcbiAqIEBmdW5jdGlvblxyXG4gKiBAcmV0dXJucyB7bnVtYmVyfSBWYWx1ZSByZWFkXHJcbiAqL1xyXG5SZWFkZXIucHJvdG90eXBlLmRvdWJsZSA9IGZ1bmN0aW9uIHJlYWRfZG91YmxlKCkge1xyXG5cclxuICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgaWYgKHRoaXMucG9zICsgOCA+IHRoaXMubGVuKVxyXG4gICAgICAgIHRocm93IGluZGV4T3V0T2ZSYW5nZSh0aGlzLCA0KTtcclxuXHJcbiAgICB2YXIgdmFsdWUgPSB1dGlsLmZsb2F0LnJlYWREb3VibGVMRSh0aGlzLmJ1ZiwgdGhpcy5wb3MpO1xyXG4gICAgdGhpcy5wb3MgKz0gODtcclxuICAgIHJldHVybiB2YWx1ZTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBSZWFkcyBhIHNlcXVlbmNlIG9mIGJ5dGVzIHByZWNlZWRlZCBieSBpdHMgbGVuZ3RoIGFzIGEgdmFyaW50LlxyXG4gKiBAcmV0dXJucyB7VWludDhBcnJheX0gVmFsdWUgcmVhZFxyXG4gKi9cclxuUmVhZGVyLnByb3RvdHlwZS5ieXRlcyA9IGZ1bmN0aW9uIHJlYWRfYnl0ZXMoKSB7XHJcbiAgICB2YXIgbGVuZ3RoID0gdGhpcy51aW50MzIoKSxcclxuICAgICAgICBzdGFydCAgPSB0aGlzLnBvcyxcclxuICAgICAgICBlbmQgICAgPSB0aGlzLnBvcyArIGxlbmd0aDtcclxuXHJcbiAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgaWYgKi9cclxuICAgIGlmIChlbmQgPiB0aGlzLmxlbilcclxuICAgICAgICB0aHJvdyBpbmRleE91dE9mUmFuZ2UodGhpcywgbGVuZ3RoKTtcclxuXHJcbiAgICB0aGlzLnBvcyArPSBsZW5ndGg7XHJcbiAgICBpZiAoQXJyYXkuaXNBcnJheSh0aGlzLmJ1ZikpIC8vIHBsYWluIGFycmF5XHJcbiAgICAgICAgcmV0dXJuIHRoaXMuYnVmLnNsaWNlKHN0YXJ0LCBlbmQpO1xyXG5cclxuICAgIGlmIChzdGFydCA9PT0gZW5kKSB7IC8vIGZpeCBmb3IgSUUgMTAvV2luOCBhbmQgb3RoZXJzJyBzdWJhcnJheSByZXR1cm5pbmcgYXJyYXkgb2Ygc2l6ZSAxXHJcbiAgICAgICAgdmFyIG5hdGl2ZUJ1ZmZlciA9IHV0aWwuQnVmZmVyO1xyXG4gICAgICAgIHJldHVybiBuYXRpdmVCdWZmZXJcclxuICAgICAgICAgICAgPyBuYXRpdmVCdWZmZXIuYWxsb2MoMClcclxuICAgICAgICAgICAgOiBuZXcgdGhpcy5idWYuY29uc3RydWN0b3IoMCk7XHJcbiAgICB9XHJcbiAgICByZXR1cm4gdGhpcy5fc2xpY2UuY2FsbCh0aGlzLmJ1Ziwgc3RhcnQsIGVuZCk7XHJcbn07XHJcblxyXG4vKipcclxuICogUmVhZHMgYSBzdHJpbmcgcHJlY2VlZGVkIGJ5IGl0cyBieXRlIGxlbmd0aCBhcyBhIHZhcmludC5cclxuICogQHJldHVybnMge3N0cmluZ30gVmFsdWUgcmVhZFxyXG4gKi9cclxuUmVhZGVyLnByb3RvdHlwZS5zdHJpbmcgPSBmdW5jdGlvbiByZWFkX3N0cmluZygpIHtcclxuICAgIHZhciBieXRlcyA9IHRoaXMuYnl0ZXMoKTtcclxuICAgIHJldHVybiB1dGY4LnJlYWQoYnl0ZXMsIDAsIGJ5dGVzLmxlbmd0aCk7XHJcbn07XHJcblxyXG4vKipcclxuICogU2tpcHMgdGhlIHNwZWNpZmllZCBudW1iZXIgb2YgYnl0ZXMgaWYgc3BlY2lmaWVkLCBvdGhlcndpc2Ugc2tpcHMgYSB2YXJpbnQuXHJcbiAqIEBwYXJhbSB7bnVtYmVyfSBbbGVuZ3RoXSBMZW5ndGggaWYga25vd24sIG90aGVyd2lzZSBhIHZhcmludCBpcyBhc3N1bWVkXHJcbiAqIEByZXR1cm5zIHtSZWFkZXJ9IGB0aGlzYFxyXG4gKi9cclxuUmVhZGVyLnByb3RvdHlwZS5za2lwID0gZnVuY3Rpb24gc2tpcChsZW5ndGgpIHtcclxuICAgIGlmICh0eXBlb2YgbGVuZ3RoID09PSBcIm51bWJlclwiKSB7XHJcbiAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICAgICAgaWYgKHRoaXMucG9zICsgbGVuZ3RoID4gdGhpcy5sZW4pXHJcbiAgICAgICAgICAgIHRocm93IGluZGV4T3V0T2ZSYW5nZSh0aGlzLCBsZW5ndGgpO1xyXG4gICAgICAgIHRoaXMucG9zICs9IGxlbmd0aDtcclxuICAgIH0gZWxzZSB7XHJcbiAgICAgICAgZG8ge1xyXG4gICAgICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgaWYgKi9cclxuICAgICAgICAgICAgaWYgKHRoaXMucG9zID49IHRoaXMubGVuKVxyXG4gICAgICAgICAgICAgICAgdGhyb3cgaW5kZXhPdXRPZlJhbmdlKHRoaXMpO1xyXG4gICAgICAgIH0gd2hpbGUgKHRoaXMuYnVmW3RoaXMucG9zKytdICYgMTI4KTtcclxuICAgIH1cclxuICAgIHJldHVybiB0aGlzO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIFNraXBzIHRoZSBuZXh0IGVsZW1lbnQgb2YgdGhlIHNwZWNpZmllZCB3aXJlIHR5cGUuXHJcbiAqIEBwYXJhbSB7bnVtYmVyfSB3aXJlVHlwZSBXaXJlIHR5cGUgcmVjZWl2ZWRcclxuICogQHJldHVybnMge1JlYWRlcn0gYHRoaXNgXHJcbiAqL1xyXG5SZWFkZXIucHJvdG90eXBlLnNraXBUeXBlID0gZnVuY3Rpb24od2lyZVR5cGUpIHtcclxuICAgIHN3aXRjaCAod2lyZVR5cGUpIHtcclxuICAgICAgICBjYXNlIDA6XHJcbiAgICAgICAgICAgIHRoaXMuc2tpcCgpO1xyXG4gICAgICAgICAgICBicmVhaztcclxuICAgICAgICBjYXNlIDE6XHJcbiAgICAgICAgICAgIHRoaXMuc2tpcCg4KTtcclxuICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgY2FzZSAyOlxyXG4gICAgICAgICAgICB0aGlzLnNraXAodGhpcy51aW50MzIoKSk7XHJcbiAgICAgICAgICAgIGJyZWFrO1xyXG4gICAgICAgIGNhc2UgMzpcclxuICAgICAgICAgICAgd2hpbGUgKCh3aXJlVHlwZSA9IHRoaXMudWludDMyKCkgJiA3KSAhPT0gNCkge1xyXG4gICAgICAgICAgICAgICAgdGhpcy5za2lwVHlwZSh3aXJlVHlwZSk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgY2FzZSA1OlxyXG4gICAgICAgICAgICB0aGlzLnNraXAoNCk7XHJcbiAgICAgICAgICAgIGJyZWFrO1xyXG5cclxuICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgbmV4dCAqL1xyXG4gICAgICAgIGRlZmF1bHQ6XHJcbiAgICAgICAgICAgIHRocm93IEVycm9yKFwiaW52YWxpZCB3aXJlIHR5cGUgXCIgKyB3aXJlVHlwZSArIFwiIGF0IG9mZnNldCBcIiArIHRoaXMucG9zKTtcclxuICAgIH1cclxuICAgIHJldHVybiB0aGlzO1xyXG59O1xyXG5cclxuUmVhZGVyLl9jb25maWd1cmUgPSBmdW5jdGlvbihCdWZmZXJSZWFkZXJfKSB7XHJcbiAgICBCdWZmZXJSZWFkZXIgPSBCdWZmZXJSZWFkZXJfO1xyXG4gICAgUmVhZGVyLmNyZWF0ZSA9IGNyZWF0ZSgpO1xyXG4gICAgQnVmZmVyUmVhZGVyLl9jb25maWd1cmUoKTtcclxuXHJcbiAgICB2YXIgZm4gPSB1dGlsLkxvbmcgPyBcInRvTG9uZ1wiIDogLyogaXN0YW5idWwgaWdub3JlIG5leHQgKi8gXCJ0b051bWJlclwiO1xyXG4gICAgdXRpbC5tZXJnZShSZWFkZXIucHJvdG90eXBlLCB7XHJcblxyXG4gICAgICAgIGludDY0OiBmdW5jdGlvbiByZWFkX2ludDY0KCkge1xyXG4gICAgICAgICAgICByZXR1cm4gcmVhZExvbmdWYXJpbnQuY2FsbCh0aGlzKVtmbl0oZmFsc2UpO1xyXG4gICAgICAgIH0sXHJcblxyXG4gICAgICAgIHVpbnQ2NDogZnVuY3Rpb24gcmVhZF91aW50NjQoKSB7XHJcbiAgICAgICAgICAgIHJldHVybiByZWFkTG9uZ1ZhcmludC5jYWxsKHRoaXMpW2ZuXSh0cnVlKTtcclxuICAgICAgICB9LFxyXG5cclxuICAgICAgICBzaW50NjQ6IGZ1bmN0aW9uIHJlYWRfc2ludDY0KCkge1xyXG4gICAgICAgICAgICByZXR1cm4gcmVhZExvbmdWYXJpbnQuY2FsbCh0aGlzKS56ekRlY29kZSgpW2ZuXShmYWxzZSk7XHJcbiAgICAgICAgfSxcclxuXHJcbiAgICAgICAgZml4ZWQ2NDogZnVuY3Rpb24gcmVhZF9maXhlZDY0KCkge1xyXG4gICAgICAgICAgICByZXR1cm4gcmVhZEZpeGVkNjQuY2FsbCh0aGlzKVtmbl0odHJ1ZSk7XHJcbiAgICAgICAgfSxcclxuXHJcbiAgICAgICAgc2ZpeGVkNjQ6IGZ1bmN0aW9uIHJlYWRfc2ZpeGVkNjQoKSB7XHJcbiAgICAgICAgICAgIHJldHVybiByZWFkRml4ZWQ2NC5jYWxsKHRoaXMpW2ZuXShmYWxzZSk7XHJcbiAgICAgICAgfVxyXG5cclxuICAgIH0pO1xyXG59O1xyXG4iLCJcInVzZSBzdHJpY3RcIjtcclxubW9kdWxlLmV4cG9ydHMgPSBCdWZmZXJSZWFkZXI7XHJcblxyXG4vLyBleHRlbmRzIFJlYWRlclxyXG52YXIgUmVhZGVyID0gcmVxdWlyZShcIi4vcmVhZGVyXCIpO1xyXG4oQnVmZmVyUmVhZGVyLnByb3RvdHlwZSA9IE9iamVjdC5jcmVhdGUoUmVhZGVyLnByb3RvdHlwZSkpLmNvbnN0cnVjdG9yID0gQnVmZmVyUmVhZGVyO1xyXG5cclxudmFyIHV0aWwgPSByZXF1aXJlKFwiLi91dGlsL21pbmltYWxcIik7XHJcblxyXG4vKipcclxuICogQ29uc3RydWN0cyBhIG5ldyBidWZmZXIgcmVhZGVyIGluc3RhbmNlLlxyXG4gKiBAY2xhc3NkZXNjIFdpcmUgZm9ybWF0IHJlYWRlciB1c2luZyBub2RlIGJ1ZmZlcnMuXHJcbiAqIEBleHRlbmRzIFJlYWRlclxyXG4gKiBAY29uc3RydWN0b3JcclxuICogQHBhcmFtIHtCdWZmZXJ9IGJ1ZmZlciBCdWZmZXIgdG8gcmVhZCBmcm9tXHJcbiAqL1xyXG5mdW5jdGlvbiBCdWZmZXJSZWFkZXIoYnVmZmVyKSB7XHJcbiAgICBSZWFkZXIuY2FsbCh0aGlzLCBidWZmZXIpO1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmVhZCBidWZmZXIuXHJcbiAgICAgKiBAbmFtZSBCdWZmZXJSZWFkZXIjYnVmXHJcbiAgICAgKiBAdHlwZSB7QnVmZmVyfVxyXG4gICAgICovXHJcbn1cclxuXHJcbkJ1ZmZlclJlYWRlci5fY29uZmlndXJlID0gZnVuY3Rpb24gKCkge1xyXG4gICAgLyogaXN0YW5idWwgaWdub3JlIGVsc2UgKi9cclxuICAgIGlmICh1dGlsLkJ1ZmZlcilcclxuICAgICAgICBCdWZmZXJSZWFkZXIucHJvdG90eXBlLl9zbGljZSA9IHV0aWwuQnVmZmVyLnByb3RvdHlwZS5zbGljZTtcclxufTtcclxuXHJcblxyXG4vKipcclxuICogQG92ZXJyaWRlXHJcbiAqL1xyXG5CdWZmZXJSZWFkZXIucHJvdG90eXBlLnN0cmluZyA9IGZ1bmN0aW9uIHJlYWRfc3RyaW5nX2J1ZmZlcigpIHtcclxuICAgIHZhciBsZW4gPSB0aGlzLnVpbnQzMigpOyAvLyBtb2RpZmllcyBwb3NcclxuICAgIHJldHVybiB0aGlzLmJ1Zi51dGY4U2xpY2VcclxuICAgICAgICA/IHRoaXMuYnVmLnV0ZjhTbGljZSh0aGlzLnBvcywgdGhpcy5wb3MgPSBNYXRoLm1pbih0aGlzLnBvcyArIGxlbiwgdGhpcy5sZW4pKVxyXG4gICAgICAgIDogdGhpcy5idWYudG9TdHJpbmcoXCJ1dGYtOFwiLCB0aGlzLnBvcywgdGhpcy5wb3MgPSBNYXRoLm1pbih0aGlzLnBvcyArIGxlbiwgdGhpcy5sZW4pKTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBSZWFkcyBhIHNlcXVlbmNlIG9mIGJ5dGVzIHByZWNlZWRlZCBieSBpdHMgbGVuZ3RoIGFzIGEgdmFyaW50LlxyXG4gKiBAbmFtZSBCdWZmZXJSZWFkZXIjYnl0ZXNcclxuICogQGZ1bmN0aW9uXHJcbiAqIEByZXR1cm5zIHtCdWZmZXJ9IFZhbHVlIHJlYWRcclxuICovXHJcblxyXG5CdWZmZXJSZWFkZXIuX2NvbmZpZ3VyZSgpO1xyXG4iLCJcInVzZSBzdHJpY3RcIjtcclxubW9kdWxlLmV4cG9ydHMgPSBSb290O1xyXG5cclxuLy8gZXh0ZW5kcyBOYW1lc3BhY2VcclxudmFyIE5hbWVzcGFjZSA9IHJlcXVpcmUoXCIuL25hbWVzcGFjZVwiKTtcclxuKChSb290LnByb3RvdHlwZSA9IE9iamVjdC5jcmVhdGUoTmFtZXNwYWNlLnByb3RvdHlwZSkpLmNvbnN0cnVjdG9yID0gUm9vdCkuY2xhc3NOYW1lID0gXCJSb290XCI7XHJcblxyXG52YXIgRmllbGQgICA9IHJlcXVpcmUoXCIuL2ZpZWxkXCIpLFxyXG4gICAgRW51bSAgICA9IHJlcXVpcmUoXCIuL2VudW1cIiksXHJcbiAgICBPbmVPZiAgID0gcmVxdWlyZShcIi4vb25lb2ZcIiksXHJcbiAgICB1dGlsICAgID0gcmVxdWlyZShcIi4vdXRpbFwiKTtcclxuXHJcbnZhciBUeXBlLCAgIC8vIGN5Y2xpY1xyXG4gICAgcGFyc2UsICAvLyBtaWdodCBiZSBleGNsdWRlZFxyXG4gICAgY29tbW9uOyAvLyBcIlxyXG5cclxuLyoqXHJcbiAqIENvbnN0cnVjdHMgYSBuZXcgcm9vdCBuYW1lc3BhY2UgaW5zdGFuY2UuXHJcbiAqIEBjbGFzc2Rlc2MgUm9vdCBuYW1lc3BhY2Ugd3JhcHBpbmcgYWxsIHR5cGVzLCBlbnVtcywgc2VydmljZXMsIHN1Yi1uYW1lc3BhY2VzIGV0Yy4gdGhhdCBiZWxvbmcgdG9nZXRoZXIuXHJcbiAqIEBleHRlbmRzIE5hbWVzcGFjZUJhc2VcclxuICogQGNvbnN0cnVjdG9yXHJcbiAqIEBwYXJhbSB7T2JqZWN0LjxzdHJpbmcsKj59IFtvcHRpb25zXSBUb3AgbGV2ZWwgb3B0aW9uc1xyXG4gKi9cclxuZnVuY3Rpb24gUm9vdChvcHRpb25zKSB7XHJcbiAgICBOYW1lc3BhY2UuY2FsbCh0aGlzLCBcIlwiLCBvcHRpb25zKTtcclxuXHJcbiAgICAvKipcclxuICAgICAqIERlZmVycmVkIGV4dGVuc2lvbiBmaWVsZHMuXHJcbiAgICAgKiBAdHlwZSB7RmllbGRbXX1cclxuICAgICAqL1xyXG4gICAgdGhpcy5kZWZlcnJlZCA9IFtdO1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmVzb2x2ZWQgZmlsZSBuYW1lcyBvZiBsb2FkZWQgZmlsZXMuXHJcbiAgICAgKiBAdHlwZSB7c3RyaW5nW119XHJcbiAgICAgKi9cclxuICAgIHRoaXMuZmlsZXMgPSBbXTtcclxufVxyXG5cclxuLyoqXHJcbiAqIExvYWRzIGEgbmFtZXNwYWNlIGRlc2NyaXB0b3IgaW50byBhIHJvb3QgbmFtZXNwYWNlLlxyXG4gKiBAcGFyYW0ge0lOYW1lc3BhY2V9IGpzb24gTmFtZWVzcGFjZSBkZXNjcmlwdG9yXHJcbiAqIEBwYXJhbSB7Um9vdH0gW3Jvb3RdIFJvb3QgbmFtZXNwYWNlLCBkZWZhdWx0cyB0byBjcmVhdGUgYSBuZXcgb25lIGlmIG9taXR0ZWRcclxuICogQHJldHVybnMge1Jvb3R9IFJvb3QgbmFtZXNwYWNlXHJcbiAqL1xyXG5Sb290LmZyb21KU09OID0gZnVuY3Rpb24gZnJvbUpTT04oanNvbiwgcm9vdCkge1xyXG4gICAgaWYgKCFyb290KVxyXG4gICAgICAgIHJvb3QgPSBuZXcgUm9vdCgpO1xyXG4gICAgaWYgKGpzb24ub3B0aW9ucylcclxuICAgICAgICByb290LnNldE9wdGlvbnMoanNvbi5vcHRpb25zKTtcclxuICAgIHJldHVybiByb290LmFkZEpTT04oanNvbi5uZXN0ZWQpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIFJlc29sdmVzIHRoZSBwYXRoIG9mIGFuIGltcG9ydGVkIGZpbGUsIHJlbGF0aXZlIHRvIHRoZSBpbXBvcnRpbmcgb3JpZ2luLlxyXG4gKiBUaGlzIG1ldGhvZCBleGlzdHMgc28geW91IGNhbiBvdmVycmlkZSBpdCB3aXRoIHlvdXIgb3duIGxvZ2ljIGluIGNhc2UgeW91ciBpbXBvcnRzIGFyZSBzY2F0dGVyZWQgb3ZlciBtdWx0aXBsZSBkaXJlY3Rvcmllcy5cclxuICogQGZ1bmN0aW9uXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBvcmlnaW4gVGhlIGZpbGUgbmFtZSBvZiB0aGUgaW1wb3J0aW5nIGZpbGVcclxuICogQHBhcmFtIHtzdHJpbmd9IHRhcmdldCBUaGUgZmlsZSBuYW1lIGJlaW5nIGltcG9ydGVkXHJcbiAqIEByZXR1cm5zIHtzdHJpbmd8bnVsbH0gUmVzb2x2ZWQgcGF0aCB0byBgdGFyZ2V0YCBvciBgbnVsbGAgdG8gc2tpcCB0aGUgZmlsZVxyXG4gKi9cclxuUm9vdC5wcm90b3R5cGUucmVzb2x2ZVBhdGggPSB1dGlsLnBhdGgucmVzb2x2ZTtcclxuXHJcbi8qKlxyXG4gKiBGZXRjaCBjb250ZW50IGZyb20gZmlsZSBwYXRoIG9yIHVybFxyXG4gKiBUaGlzIG1ldGhvZCBleGlzdHMgc28geW91IGNhbiBvdmVycmlkZSBpdCB3aXRoIHlvdXIgb3duIGxvZ2ljLlxyXG4gKiBAZnVuY3Rpb25cclxuICogQHBhcmFtIHtzdHJpbmd9IHBhdGggRmlsZSBwYXRoIG9yIHVybFxyXG4gKiBAcGFyYW0ge0ZldGNoQ2FsbGJhY2t9IGNhbGxiYWNrIENhbGxiYWNrIGZ1bmN0aW9uXHJcbiAqIEByZXR1cm5zIHt1bmRlZmluZWR9XHJcbiAqL1xyXG5Sb290LnByb3RvdHlwZS5mZXRjaCA9IHV0aWwuZmV0Y2g7XHJcblxyXG4vLyBBIHN5bWJvbC1saWtlIGZ1bmN0aW9uIHRvIHNhZmVseSBzaWduYWwgc3luY2hyb25vdXMgbG9hZGluZ1xyXG4vKiBpc3RhbmJ1bCBpZ25vcmUgbmV4dCAqL1xyXG5mdW5jdGlvbiBTWU5DKCkge30gLy8gZXNsaW50LWRpc2FibGUtbGluZSBuby1lbXB0eS1mdW5jdGlvblxyXG5cclxuLyoqXHJcbiAqIExvYWRzIG9uZSBvciBtdWx0aXBsZSAucHJvdG8gb3IgcHJlcHJvY2Vzc2VkIC5qc29uIGZpbGVzIGludG8gdGhpcyByb290IG5hbWVzcGFjZSBhbmQgY2FsbHMgdGhlIGNhbGxiYWNrLlxyXG4gKiBAcGFyYW0ge3N0cmluZ3xzdHJpbmdbXX0gZmlsZW5hbWUgTmFtZXMgb2Ygb25lIG9yIG11bHRpcGxlIGZpbGVzIHRvIGxvYWRcclxuICogQHBhcmFtIHtJUGFyc2VPcHRpb25zfSBvcHRpb25zIFBhcnNlIG9wdGlvbnNcclxuICogQHBhcmFtIHtMb2FkQ2FsbGJhY2t9IGNhbGxiYWNrIENhbGxiYWNrIGZ1bmN0aW9uXHJcbiAqIEByZXR1cm5zIHt1bmRlZmluZWR9XHJcbiAqL1xyXG5Sb290LnByb3RvdHlwZS5sb2FkID0gZnVuY3Rpb24gbG9hZChmaWxlbmFtZSwgb3B0aW9ucywgY2FsbGJhY2spIHtcclxuICAgIGlmICh0eXBlb2Ygb3B0aW9ucyA9PT0gXCJmdW5jdGlvblwiKSB7XHJcbiAgICAgICAgY2FsbGJhY2sgPSBvcHRpb25zO1xyXG4gICAgICAgIG9wdGlvbnMgPSB1bmRlZmluZWQ7XHJcbiAgICB9XHJcbiAgICB2YXIgc2VsZiA9IHRoaXM7XHJcbiAgICBpZiAoIWNhbGxiYWNrKVxyXG4gICAgICAgIHJldHVybiB1dGlsLmFzUHJvbWlzZShsb2FkLCBzZWxmLCBmaWxlbmFtZSwgb3B0aW9ucyk7XHJcblxyXG4gICAgdmFyIHN5bmMgPSBjYWxsYmFjayA9PT0gU1lOQzsgLy8gdW5kb2N1bWVudGVkXHJcblxyXG4gICAgLy8gRmluaXNoZXMgbG9hZGluZyBieSBjYWxsaW5nIHRoZSBjYWxsYmFjayAoZXhhY3RseSBvbmNlKVxyXG4gICAgZnVuY3Rpb24gZmluaXNoKGVyciwgcm9vdCkge1xyXG4gICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgICAgIGlmICghY2FsbGJhY2spXHJcbiAgICAgICAgICAgIHJldHVybjtcclxuICAgICAgICBpZiAoc3luYylcclxuICAgICAgICAgICAgdGhyb3cgZXJyO1xyXG4gICAgICAgIHZhciBjYiA9IGNhbGxiYWNrO1xyXG4gICAgICAgIGNhbGxiYWNrID0gbnVsbDtcclxuICAgICAgICBjYihlcnIsIHJvb3QpO1xyXG4gICAgfVxyXG5cclxuICAgIC8vIEJ1bmRsZWQgZGVmaW5pdGlvbiBleGlzdGVuY2UgY2hlY2tpbmdcclxuICAgIGZ1bmN0aW9uIGdldEJ1bmRsZWRGaWxlTmFtZShmaWxlbmFtZSkge1xyXG4gICAgICAgIHZhciBpZHggPSBmaWxlbmFtZS5sYXN0SW5kZXhPZihcImdvb2dsZS9wcm90b2J1Zi9cIik7XHJcbiAgICAgICAgaWYgKGlkeCA+IC0xKSB7XHJcbiAgICAgICAgICAgIHZhciBhbHRuYW1lID0gZmlsZW5hbWUuc3Vic3RyaW5nKGlkeCk7XHJcbiAgICAgICAgICAgIGlmIChhbHRuYW1lIGluIGNvbW1vbikgcmV0dXJuIGFsdG5hbWU7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIHJldHVybiBudWxsO1xyXG4gICAgfVxyXG5cclxuICAgIC8vIFByb2Nlc3NlcyBhIHNpbmdsZSBmaWxlXHJcbiAgICBmdW5jdGlvbiBwcm9jZXNzKGZpbGVuYW1lLCBzb3VyY2UpIHtcclxuICAgICAgICB0cnkge1xyXG4gICAgICAgICAgICBpZiAodXRpbC5pc1N0cmluZyhzb3VyY2UpICYmIHNvdXJjZS5jaGFyQXQoMCkgPT09IFwie1wiKVxyXG4gICAgICAgICAgICAgICAgc291cmNlID0gSlNPTi5wYXJzZShzb3VyY2UpO1xyXG4gICAgICAgICAgICBpZiAoIXV0aWwuaXNTdHJpbmcoc291cmNlKSlcclxuICAgICAgICAgICAgICAgIHNlbGYuc2V0T3B0aW9ucyhzb3VyY2Uub3B0aW9ucykuYWRkSlNPTihzb3VyY2UubmVzdGVkKTtcclxuICAgICAgICAgICAgZWxzZSB7XHJcbiAgICAgICAgICAgICAgICBwYXJzZS5maWxlbmFtZSA9IGZpbGVuYW1lO1xyXG4gICAgICAgICAgICAgICAgdmFyIHBhcnNlZCA9IHBhcnNlKHNvdXJjZSwgc2VsZiwgb3B0aW9ucyksXHJcbiAgICAgICAgICAgICAgICAgICAgcmVzb2x2ZWQsXHJcbiAgICAgICAgICAgICAgICAgICAgaSA9IDA7XHJcbiAgICAgICAgICAgICAgICBpZiAocGFyc2VkLmltcG9ydHMpXHJcbiAgICAgICAgICAgICAgICAgICAgZm9yICg7IGkgPCBwYXJzZWQuaW1wb3J0cy5sZW5ndGg7ICsraSlcclxuICAgICAgICAgICAgICAgICAgICAgICAgaWYgKHJlc29sdmVkID0gZ2V0QnVuZGxlZEZpbGVOYW1lKHBhcnNlZC5pbXBvcnRzW2ldKSB8fCBzZWxmLnJlc29sdmVQYXRoKGZpbGVuYW1lLCBwYXJzZWQuaW1wb3J0c1tpXSkpXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBmZXRjaChyZXNvbHZlZCk7XHJcbiAgICAgICAgICAgICAgICBpZiAocGFyc2VkLndlYWtJbXBvcnRzKVxyXG4gICAgICAgICAgICAgICAgICAgIGZvciAoaSA9IDA7IGkgPCBwYXJzZWQud2Vha0ltcG9ydHMubGVuZ3RoOyArK2kpXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGlmIChyZXNvbHZlZCA9IGdldEJ1bmRsZWRGaWxlTmFtZShwYXJzZWQud2Vha0ltcG9ydHNbaV0pIHx8IHNlbGYucmVzb2x2ZVBhdGgoZmlsZW5hbWUsIHBhcnNlZC53ZWFrSW1wb3J0c1tpXSkpXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBmZXRjaChyZXNvbHZlZCwgdHJ1ZSk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9IGNhdGNoIChlcnIpIHtcclxuICAgICAgICAgICAgZmluaXNoKGVycik7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIGlmICghc3luYyAmJiAhcXVldWVkKVxyXG4gICAgICAgICAgICBmaW5pc2gobnVsbCwgc2VsZik7IC8vIG9ubHkgb25jZSBhbnl3YXlcclxuICAgIH1cclxuXHJcbiAgICAvLyBGZXRjaGVzIGEgc2luZ2xlIGZpbGVcclxuICAgIGZ1bmN0aW9uIGZldGNoKGZpbGVuYW1lLCB3ZWFrKSB7XHJcbiAgICAgICAgZmlsZW5hbWUgPSBnZXRCdW5kbGVkRmlsZU5hbWUoZmlsZW5hbWUpIHx8IGZpbGVuYW1lO1xyXG5cclxuICAgICAgICAvLyBTa2lwIGlmIGFscmVhZHkgbG9hZGVkIC8gYXR0ZW1wdGVkXHJcbiAgICAgICAgaWYgKHNlbGYuZmlsZXMuaW5kZXhPZihmaWxlbmFtZSkgPiAtMSlcclxuICAgICAgICAgICAgcmV0dXJuO1xyXG4gICAgICAgIHNlbGYuZmlsZXMucHVzaChmaWxlbmFtZSk7XHJcblxyXG4gICAgICAgIC8vIFNob3J0Y3V0IGJ1bmRsZWQgZGVmaW5pdGlvbnNcclxuICAgICAgICBpZiAoZmlsZW5hbWUgaW4gY29tbW9uKSB7XHJcbiAgICAgICAgICAgIGlmIChzeW5jKVxyXG4gICAgICAgICAgICAgICAgcHJvY2VzcyhmaWxlbmFtZSwgY29tbW9uW2ZpbGVuYW1lXSk7XHJcbiAgICAgICAgICAgIGVsc2Uge1xyXG4gICAgICAgICAgICAgICAgKytxdWV1ZWQ7XHJcbiAgICAgICAgICAgICAgICBzZXRUaW1lb3V0KGZ1bmN0aW9uKCkge1xyXG4gICAgICAgICAgICAgICAgICAgIC0tcXVldWVkO1xyXG4gICAgICAgICAgICAgICAgICAgIHByb2Nlc3MoZmlsZW5hbWUsIGNvbW1vbltmaWxlbmFtZV0pO1xyXG4gICAgICAgICAgICAgICAgfSk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgcmV0dXJuO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgLy8gT3RoZXJ3aXNlIGZldGNoIGZyb20gZGlzayBvciBuZXR3b3JrXHJcbiAgICAgICAgaWYgKHN5bmMpIHtcclxuICAgICAgICAgICAgdmFyIHNvdXJjZTtcclxuICAgICAgICAgICAgdHJ5IHtcclxuICAgICAgICAgICAgICAgIHNvdXJjZSA9IHV0aWwuZnMucmVhZEZpbGVTeW5jKGZpbGVuYW1lKS50b1N0cmluZyhcInV0ZjhcIik7XHJcbiAgICAgICAgICAgIH0gY2F0Y2ggKGVycikge1xyXG4gICAgICAgICAgICAgICAgaWYgKCF3ZWFrKVxyXG4gICAgICAgICAgICAgICAgICAgIGZpbmlzaChlcnIpO1xyXG4gICAgICAgICAgICAgICAgcmV0dXJuO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIHByb2Nlc3MoZmlsZW5hbWUsIHNvdXJjZSk7XHJcbiAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgKytxdWV1ZWQ7XHJcbiAgICAgICAgICAgIHNlbGYuZmV0Y2goZmlsZW5hbWUsIGZ1bmN0aW9uKGVyciwgc291cmNlKSB7XHJcbiAgICAgICAgICAgICAgICAtLXF1ZXVlZDtcclxuICAgICAgICAgICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgICAgICAgICAgICAgaWYgKCFjYWxsYmFjaylcclxuICAgICAgICAgICAgICAgICAgICByZXR1cm47IC8vIHRlcm1pbmF0ZWQgbWVhbndoaWxlXHJcbiAgICAgICAgICAgICAgICBpZiAoZXJyKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGVsc2UgKi9cclxuICAgICAgICAgICAgICAgICAgICBpZiAoIXdlYWspXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGZpbmlzaChlcnIpO1xyXG4gICAgICAgICAgICAgICAgICAgIGVsc2UgaWYgKCFxdWV1ZWQpIC8vIGNhbid0IGJlIGNvdmVyZWQgcmVsaWFibHlcclxuICAgICAgICAgICAgICAgICAgICAgICAgZmluaXNoKG51bGwsIHNlbGYpO1xyXG4gICAgICAgICAgICAgICAgICAgIHJldHVybjtcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgIHByb2Nlc3MoZmlsZW5hbWUsIHNvdXJjZSk7XHJcbiAgICAgICAgICAgIH0pO1xyXG4gICAgICAgIH1cclxuICAgIH1cclxuICAgIHZhciBxdWV1ZWQgPSAwO1xyXG5cclxuICAgIC8vIEFzc2VtYmxpbmcgdGhlIHJvb3QgbmFtZXNwYWNlIGRvZXNuJ3QgcmVxdWlyZSB3b3JraW5nIHR5cGVcclxuICAgIC8vIHJlZmVyZW5jZXMgYW55bW9yZSwgc28gd2UgY2FuIGxvYWQgZXZlcnl0aGluZyBpbiBwYXJhbGxlbFxyXG4gICAgaWYgKHV0aWwuaXNTdHJpbmcoZmlsZW5hbWUpKVxyXG4gICAgICAgIGZpbGVuYW1lID0gWyBmaWxlbmFtZSBdO1xyXG4gICAgZm9yICh2YXIgaSA9IDAsIHJlc29sdmVkOyBpIDwgZmlsZW5hbWUubGVuZ3RoOyArK2kpXHJcbiAgICAgICAgaWYgKHJlc29sdmVkID0gc2VsZi5yZXNvbHZlUGF0aChcIlwiLCBmaWxlbmFtZVtpXSkpXHJcbiAgICAgICAgICAgIGZldGNoKHJlc29sdmVkKTtcclxuXHJcbiAgICBpZiAoc3luYylcclxuICAgICAgICByZXR1cm4gc2VsZjtcclxuICAgIGlmICghcXVldWVkKVxyXG4gICAgICAgIGZpbmlzaChudWxsLCBzZWxmKTtcclxuICAgIHJldHVybiB1bmRlZmluZWQ7XHJcbn07XHJcbi8vIGZ1bmN0aW9uIGxvYWQoZmlsZW5hbWU6c3RyaW5nLCBvcHRpb25zOklQYXJzZU9wdGlvbnMsIGNhbGxiYWNrOkxvYWRDYWxsYmFjayk6dW5kZWZpbmVkXHJcblxyXG4vKipcclxuICogTG9hZHMgb25lIG9yIG11bHRpcGxlIC5wcm90byBvciBwcmVwcm9jZXNzZWQgLmpzb24gZmlsZXMgaW50byB0aGlzIHJvb3QgbmFtZXNwYWNlIGFuZCBjYWxscyB0aGUgY2FsbGJhY2suXHJcbiAqIEBmdW5jdGlvbiBSb290I2xvYWRcclxuICogQHBhcmFtIHtzdHJpbmd8c3RyaW5nW119IGZpbGVuYW1lIE5hbWVzIG9mIG9uZSBvciBtdWx0aXBsZSBmaWxlcyB0byBsb2FkXHJcbiAqIEBwYXJhbSB7TG9hZENhbGxiYWNrfSBjYWxsYmFjayBDYWxsYmFjayBmdW5jdGlvblxyXG4gKiBAcmV0dXJucyB7dW5kZWZpbmVkfVxyXG4gKiBAdmFyaWF0aW9uIDJcclxuICovXHJcbi8vIGZ1bmN0aW9uIGxvYWQoZmlsZW5hbWU6c3RyaW5nLCBjYWxsYmFjazpMb2FkQ2FsbGJhY2spOnVuZGVmaW5lZFxyXG5cclxuLyoqXHJcbiAqIExvYWRzIG9uZSBvciBtdWx0aXBsZSAucHJvdG8gb3IgcHJlcHJvY2Vzc2VkIC5qc29uIGZpbGVzIGludG8gdGhpcyByb290IG5hbWVzcGFjZSBhbmQgcmV0dXJucyBhIHByb21pc2UuXHJcbiAqIEBmdW5jdGlvbiBSb290I2xvYWRcclxuICogQHBhcmFtIHtzdHJpbmd8c3RyaW5nW119IGZpbGVuYW1lIE5hbWVzIG9mIG9uZSBvciBtdWx0aXBsZSBmaWxlcyB0byBsb2FkXHJcbiAqIEBwYXJhbSB7SVBhcnNlT3B0aW9uc30gW29wdGlvbnNdIFBhcnNlIG9wdGlvbnMuIERlZmF1bHRzIHRvIHtAbGluayBwYXJzZS5kZWZhdWx0c30gd2hlbiBvbWl0dGVkLlxyXG4gKiBAcmV0dXJucyB7UHJvbWlzZTxSb290Pn0gUHJvbWlzZVxyXG4gKiBAdmFyaWF0aW9uIDNcclxuICovXHJcbi8vIGZ1bmN0aW9uIGxvYWQoZmlsZW5hbWU6c3RyaW5nLCBbb3B0aW9uczpJUGFyc2VPcHRpb25zXSk6UHJvbWlzZTxSb290PlxyXG5cclxuLyoqXHJcbiAqIFN5bmNocm9ub3VzbHkgbG9hZHMgb25lIG9yIG11bHRpcGxlIC5wcm90byBvciBwcmVwcm9jZXNzZWQgLmpzb24gZmlsZXMgaW50byB0aGlzIHJvb3QgbmFtZXNwYWNlIChub2RlIG9ubHkpLlxyXG4gKiBAZnVuY3Rpb24gUm9vdCNsb2FkU3luY1xyXG4gKiBAcGFyYW0ge3N0cmluZ3xzdHJpbmdbXX0gZmlsZW5hbWUgTmFtZXMgb2Ygb25lIG9yIG11bHRpcGxlIGZpbGVzIHRvIGxvYWRcclxuICogQHBhcmFtIHtJUGFyc2VPcHRpb25zfSBbb3B0aW9uc10gUGFyc2Ugb3B0aW9ucy4gRGVmYXVsdHMgdG8ge0BsaW5rIHBhcnNlLmRlZmF1bHRzfSB3aGVuIG9taXR0ZWQuXHJcbiAqIEByZXR1cm5zIHtSb290fSBSb290IG5hbWVzcGFjZVxyXG4gKiBAdGhyb3dzIHtFcnJvcn0gSWYgc3luY2hyb25vdXMgZmV0Y2hpbmcgaXMgbm90IHN1cHBvcnRlZCAoaS5lLiBpbiBicm93c2Vycykgb3IgaWYgYSBmaWxlJ3Mgc3ludGF4IGlzIGludmFsaWRcclxuICovXHJcblJvb3QucHJvdG90eXBlLmxvYWRTeW5jID0gZnVuY3Rpb24gbG9hZFN5bmMoZmlsZW5hbWUsIG9wdGlvbnMpIHtcclxuICAgIGlmICghdXRpbC5pc05vZGUpXHJcbiAgICAgICAgdGhyb3cgRXJyb3IoXCJub3Qgc3VwcG9ydGVkXCIpO1xyXG4gICAgcmV0dXJuIHRoaXMubG9hZChmaWxlbmFtZSwgb3B0aW9ucywgU1lOQyk7XHJcbn07XHJcblxyXG4vKipcclxuICogQG92ZXJyaWRlXHJcbiAqL1xyXG5Sb290LnByb3RvdHlwZS5yZXNvbHZlQWxsID0gZnVuY3Rpb24gcmVzb2x2ZUFsbCgpIHtcclxuICAgIGlmICh0aGlzLmRlZmVycmVkLmxlbmd0aClcclxuICAgICAgICB0aHJvdyBFcnJvcihcInVucmVzb2x2YWJsZSBleHRlbnNpb25zOiBcIiArIHRoaXMuZGVmZXJyZWQubWFwKGZ1bmN0aW9uKGZpZWxkKSB7XHJcbiAgICAgICAgICAgIHJldHVybiBcIidleHRlbmQgXCIgKyBmaWVsZC5leHRlbmQgKyBcIicgaW4gXCIgKyBmaWVsZC5wYXJlbnQuZnVsbE5hbWU7XHJcbiAgICAgICAgfSkuam9pbihcIiwgXCIpKTtcclxuICAgIHJldHVybiBOYW1lc3BhY2UucHJvdG90eXBlLnJlc29sdmVBbGwuY2FsbCh0aGlzKTtcclxufTtcclxuXHJcbi8vIG9ubHkgdXBwZXJjYXNlZCAoYW5kIHRodXMgY29uZmxpY3QtZnJlZSkgY2hpbGRyZW4gYXJlIGV4cG9zZWQsIHNlZSBiZWxvd1xyXG52YXIgZXhwb3NlUmUgPSAvXltBLVpdLztcclxuXHJcbi8qKlxyXG4gKiBIYW5kbGVzIGEgZGVmZXJyZWQgZGVjbGFyaW5nIGV4dGVuc2lvbiBmaWVsZCBieSBjcmVhdGluZyBhIHNpc3RlciBmaWVsZCB0byByZXByZXNlbnQgaXQgd2l0aGluIGl0cyBleHRlbmRlZCB0eXBlLlxyXG4gKiBAcGFyYW0ge1Jvb3R9IHJvb3QgUm9vdCBpbnN0YW5jZVxyXG4gKiBAcGFyYW0ge0ZpZWxkfSBmaWVsZCBEZWNsYXJpbmcgZXh0ZW5zaW9uIGZpZWxkIHdpdGluIHRoZSBkZWNsYXJpbmcgdHlwZVxyXG4gKiBAcmV0dXJucyB7Ym9vbGVhbn0gYHRydWVgIGlmIHN1Y2Nlc3NmdWxseSBhZGRlZCB0byB0aGUgZXh0ZW5kZWQgdHlwZSwgYGZhbHNlYCBvdGhlcndpc2VcclxuICogQGlubmVyXHJcbiAqIEBpZ25vcmVcclxuICovXHJcbmZ1bmN0aW9uIHRyeUhhbmRsZUV4dGVuc2lvbihyb290LCBmaWVsZCkge1xyXG4gICAgdmFyIGV4dGVuZGVkVHlwZSA9IGZpZWxkLnBhcmVudC5sb29rdXAoZmllbGQuZXh0ZW5kKTtcclxuICAgIGlmIChleHRlbmRlZFR5cGUpIHtcclxuICAgICAgICB2YXIgc2lzdGVyRmllbGQgPSBuZXcgRmllbGQoZmllbGQuZnVsbE5hbWUsIGZpZWxkLmlkLCBmaWVsZC50eXBlLCBmaWVsZC5ydWxlLCB1bmRlZmluZWQsIGZpZWxkLm9wdGlvbnMpO1xyXG4gICAgICAgIC8vZG8gbm90IGFsbG93IHRvIGV4dGVuZCBzYW1lIGZpZWxkIHR3aWNlIHRvIHByZXZlbnQgdGhlIGVycm9yXHJcbiAgICAgICAgaWYgKGV4dGVuZGVkVHlwZS5nZXQoc2lzdGVyRmllbGQubmFtZSkpIHtcclxuICAgICAgICAgICAgcmV0dXJuIHRydWU7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIHNpc3RlckZpZWxkLmRlY2xhcmluZ0ZpZWxkID0gZmllbGQ7XHJcbiAgICAgICAgZmllbGQuZXh0ZW5zaW9uRmllbGQgPSBzaXN0ZXJGaWVsZDtcclxuICAgICAgICBleHRlbmRlZFR5cGUuYWRkKHNpc3RlckZpZWxkKTtcclxuICAgICAgICByZXR1cm4gdHJ1ZTtcclxuICAgIH1cclxuICAgIHJldHVybiBmYWxzZTtcclxufVxyXG5cclxuLyoqXHJcbiAqIENhbGxlZCB3aGVuIGFueSBvYmplY3QgaXMgYWRkZWQgdG8gdGhpcyByb290IG9yIGl0cyBzdWItbmFtZXNwYWNlcy5cclxuICogQHBhcmFtIHtSZWZsZWN0aW9uT2JqZWN0fSBvYmplY3QgT2JqZWN0IGFkZGVkXHJcbiAqIEByZXR1cm5zIHt1bmRlZmluZWR9XHJcbiAqIEBwcml2YXRlXHJcbiAqL1xyXG5Sb290LnByb3RvdHlwZS5faGFuZGxlQWRkID0gZnVuY3Rpb24gX2hhbmRsZUFkZChvYmplY3QpIHtcclxuICAgIGlmIChvYmplY3QgaW5zdGFuY2VvZiBGaWVsZCkge1xyXG5cclxuICAgICAgICBpZiAoLyogYW4gZXh0ZW5zaW9uIGZpZWxkIChpbXBsaWVzIG5vdCBwYXJ0IG9mIGEgb25lb2YpICovIG9iamVjdC5leHRlbmQgIT09IHVuZGVmaW5lZCAmJiAvKiBub3QgYWxyZWFkeSBoYW5kbGVkICovICFvYmplY3QuZXh0ZW5zaW9uRmllbGQpXHJcbiAgICAgICAgICAgIGlmICghdHJ5SGFuZGxlRXh0ZW5zaW9uKHRoaXMsIG9iamVjdCkpXHJcbiAgICAgICAgICAgICAgICB0aGlzLmRlZmVycmVkLnB1c2gob2JqZWN0KTtcclxuXHJcbiAgICB9IGVsc2UgaWYgKG9iamVjdCBpbnN0YW5jZW9mIEVudW0pIHtcclxuXHJcbiAgICAgICAgaWYgKGV4cG9zZVJlLnRlc3Qob2JqZWN0Lm5hbWUpKVxyXG4gICAgICAgICAgICBvYmplY3QucGFyZW50W29iamVjdC5uYW1lXSA9IG9iamVjdC52YWx1ZXM7IC8vIGV4cG9zZSBlbnVtIHZhbHVlcyBhcyBwcm9wZXJ0eSBvZiBpdHMgcGFyZW50XHJcblxyXG4gICAgfSBlbHNlIGlmICghKG9iamVjdCBpbnN0YW5jZW9mIE9uZU9mKSkgLyogZXZlcnl0aGluZyBlbHNlIGlzIGEgbmFtZXNwYWNlICovIHtcclxuXHJcbiAgICAgICAgaWYgKG9iamVjdCBpbnN0YW5jZW9mIFR5cGUpIC8vIFRyeSB0byBoYW5kbGUgYW55IGRlZmVycmVkIGV4dGVuc2lvbnNcclxuICAgICAgICAgICAgZm9yICh2YXIgaSA9IDA7IGkgPCB0aGlzLmRlZmVycmVkLmxlbmd0aDspXHJcbiAgICAgICAgICAgICAgICBpZiAodHJ5SGFuZGxlRXh0ZW5zaW9uKHRoaXMsIHRoaXMuZGVmZXJyZWRbaV0pKVxyXG4gICAgICAgICAgICAgICAgICAgIHRoaXMuZGVmZXJyZWQuc3BsaWNlKGksIDEpO1xyXG4gICAgICAgICAgICAgICAgZWxzZVxyXG4gICAgICAgICAgICAgICAgICAgICsraTtcclxuICAgICAgICBmb3IgKHZhciBqID0gMDsgaiA8IC8qIGluaXRpYWxpemVzICovIG9iamVjdC5uZXN0ZWRBcnJheS5sZW5ndGg7ICsraikgLy8gcmVjdXJzZSBpbnRvIHRoZSBuYW1lc3BhY2VcclxuICAgICAgICAgICAgdGhpcy5faGFuZGxlQWRkKG9iamVjdC5fbmVzdGVkQXJyYXlbal0pO1xyXG4gICAgICAgIGlmIChleHBvc2VSZS50ZXN0KG9iamVjdC5uYW1lKSlcclxuICAgICAgICAgICAgb2JqZWN0LnBhcmVudFtvYmplY3QubmFtZV0gPSBvYmplY3Q7IC8vIGV4cG9zZSBuYW1lc3BhY2UgYXMgcHJvcGVydHkgb2YgaXRzIHBhcmVudFxyXG4gICAgfVxyXG5cclxuICAgIC8vIFRoZSBhYm92ZSBhbHNvIGFkZHMgdXBwZXJjYXNlZCAoYW5kIHRodXMgY29uZmxpY3QtZnJlZSkgbmVzdGVkIHR5cGVzLCBzZXJ2aWNlcyBhbmQgZW51bXMgYXNcclxuICAgIC8vIHByb3BlcnRpZXMgb2YgbmFtZXNwYWNlcyBqdXN0IGxpa2Ugc3RhdGljIGNvZGUgZG9lcy4gVGhpcyBhbGxvd3MgdXNpbmcgYSAuZC50cyBnZW5lcmF0ZWQgZm9yXHJcbiAgICAvLyBhIHN0YXRpYyBtb2R1bGUgd2l0aCByZWZsZWN0aW9uLWJhc2VkIHNvbHV0aW9ucyB3aGVyZSB0aGUgY29uZGl0aW9uIGlzIG1ldC5cclxufTtcclxuXHJcbi8qKlxyXG4gKiBDYWxsZWQgd2hlbiBhbnkgb2JqZWN0IGlzIHJlbW92ZWQgZnJvbSB0aGlzIHJvb3Qgb3IgaXRzIHN1Yi1uYW1lc3BhY2VzLlxyXG4gKiBAcGFyYW0ge1JlZmxlY3Rpb25PYmplY3R9IG9iamVjdCBPYmplY3QgcmVtb3ZlZFxyXG4gKiBAcmV0dXJucyB7dW5kZWZpbmVkfVxyXG4gKiBAcHJpdmF0ZVxyXG4gKi9cclxuUm9vdC5wcm90b3R5cGUuX2hhbmRsZVJlbW92ZSA9IGZ1bmN0aW9uIF9oYW5kbGVSZW1vdmUob2JqZWN0KSB7XHJcbiAgICBpZiAob2JqZWN0IGluc3RhbmNlb2YgRmllbGQpIHtcclxuXHJcbiAgICAgICAgaWYgKC8qIGFuIGV4dGVuc2lvbiBmaWVsZCAqLyBvYmplY3QuZXh0ZW5kICE9PSB1bmRlZmluZWQpIHtcclxuICAgICAgICAgICAgaWYgKC8qIGFscmVhZHkgaGFuZGxlZCAqLyBvYmplY3QuZXh0ZW5zaW9uRmllbGQpIHsgLy8gcmVtb3ZlIGl0cyBzaXN0ZXIgZmllbGRcclxuICAgICAgICAgICAgICAgIG9iamVjdC5leHRlbnNpb25GaWVsZC5wYXJlbnQucmVtb3ZlKG9iamVjdC5leHRlbnNpb25GaWVsZCk7XHJcbiAgICAgICAgICAgICAgICBvYmplY3QuZXh0ZW5zaW9uRmllbGQgPSBudWxsO1xyXG4gICAgICAgICAgICB9IGVsc2UgeyAvLyBjYW5jZWwgdGhlIGV4dGVuc2lvblxyXG4gICAgICAgICAgICAgICAgdmFyIGluZGV4ID0gdGhpcy5kZWZlcnJlZC5pbmRleE9mKG9iamVjdCk7XHJcbiAgICAgICAgICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgZWxzZSAqL1xyXG4gICAgICAgICAgICAgICAgaWYgKGluZGV4ID4gLTEpXHJcbiAgICAgICAgICAgICAgICAgICAgdGhpcy5kZWZlcnJlZC5zcGxpY2UoaW5kZXgsIDEpO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG5cclxuICAgIH0gZWxzZSBpZiAob2JqZWN0IGluc3RhbmNlb2YgRW51bSkge1xyXG5cclxuICAgICAgICBpZiAoZXhwb3NlUmUudGVzdChvYmplY3QubmFtZSkpXHJcbiAgICAgICAgICAgIGRlbGV0ZSBvYmplY3QucGFyZW50W29iamVjdC5uYW1lXTsgLy8gdW5leHBvc2UgZW51bSB2YWx1ZXNcclxuXHJcbiAgICB9IGVsc2UgaWYgKG9iamVjdCBpbnN0YW5jZW9mIE5hbWVzcGFjZSkge1xyXG5cclxuICAgICAgICBmb3IgKHZhciBpID0gMDsgaSA8IC8qIGluaXRpYWxpemVzICovIG9iamVjdC5uZXN0ZWRBcnJheS5sZW5ndGg7ICsraSkgLy8gcmVjdXJzZSBpbnRvIHRoZSBuYW1lc3BhY2VcclxuICAgICAgICAgICAgdGhpcy5faGFuZGxlUmVtb3ZlKG9iamVjdC5fbmVzdGVkQXJyYXlbaV0pO1xyXG5cclxuICAgICAgICBpZiAoZXhwb3NlUmUudGVzdChvYmplY3QubmFtZSkpXHJcbiAgICAgICAgICAgIGRlbGV0ZSBvYmplY3QucGFyZW50W29iamVjdC5uYW1lXTsgLy8gdW5leHBvc2UgbmFtZXNwYWNlc1xyXG5cclxuICAgIH1cclxufTtcclxuXHJcbi8vIFNldHMgdXAgY3ljbGljIGRlcGVuZGVuY2llcyAoY2FsbGVkIGluIGluZGV4LWxpZ2h0KVxyXG5Sb290Ll9jb25maWd1cmUgPSBmdW5jdGlvbihUeXBlXywgcGFyc2VfLCBjb21tb25fKSB7XHJcbiAgICBUeXBlICAgPSBUeXBlXztcclxuICAgIHBhcnNlICA9IHBhcnNlXztcclxuICAgIGNvbW1vbiA9IGNvbW1vbl87XHJcbn07XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG5tb2R1bGUuZXhwb3J0cyA9IHt9O1xyXG5cclxuLyoqXHJcbiAqIE5hbWVkIHJvb3RzLlxyXG4gKiBUaGlzIGlzIHdoZXJlIHBianMgc3RvcmVzIGdlbmVyYXRlZCBzdHJ1Y3R1cmVzICh0aGUgb3B0aW9uIGAtciwgLS1yb290YCBzcGVjaWZpZXMgYSBuYW1lKS5cclxuICogQ2FuIGFsc28gYmUgdXNlZCBtYW51YWxseSB0byBtYWtlIHJvb3RzIGF2YWlsYWJsZSBhY3Jvc3MgbW9kdWxlcy5cclxuICogQG5hbWUgcm9vdHNcclxuICogQHR5cGUge09iamVjdC48c3RyaW5nLFJvb3Q+fVxyXG4gKiBAZXhhbXBsZVxyXG4gKiAvLyBwYmpzIC1yIG15cm9vdCAtbyBjb21waWxlZC5qcyAuLi5cclxuICpcclxuICogLy8gaW4gYW5vdGhlciBtb2R1bGU6XHJcbiAqIHJlcXVpcmUoXCIuL2NvbXBpbGVkLmpzXCIpO1xyXG4gKlxyXG4gKiAvLyBpbiBhbnkgc3Vic2VxdWVudCBtb2R1bGU6XHJcbiAqIHZhciByb290ID0gcHJvdG9idWYucm9vdHNbXCJteXJvb3RcIl07XHJcbiAqL1xyXG4iLCJcInVzZSBzdHJpY3RcIjtcclxuXHJcbi8qKlxyXG4gKiBTdHJlYW1pbmcgUlBDIGhlbHBlcnMuXHJcbiAqIEBuYW1lc3BhY2VcclxuICovXHJcbnZhciBycGMgPSBleHBvcnRzO1xyXG5cclxuLyoqXHJcbiAqIFJQQyBpbXBsZW1lbnRhdGlvbiBwYXNzZWQgdG8ge0BsaW5rIFNlcnZpY2UjY3JlYXRlfSBwZXJmb3JtaW5nIGEgc2VydmljZSByZXF1ZXN0IG9uIG5ldHdvcmsgbGV2ZWwsIGkuZS4gYnkgdXRpbGl6aW5nIGh0dHAgcmVxdWVzdHMgb3Igd2Vic29ja2V0cy5cclxuICogQHR5cGVkZWYgUlBDSW1wbFxyXG4gKiBAdHlwZSB7ZnVuY3Rpb259XHJcbiAqIEBwYXJhbSB7TWV0aG9kfHJwYy5TZXJ2aWNlTWV0aG9kPE1lc3NhZ2U8e30+LE1lc3NhZ2U8e30+Pn0gbWV0aG9kIFJlZmxlY3RlZCBvciBzdGF0aWMgbWV0aG9kIGJlaW5nIGNhbGxlZFxyXG4gKiBAcGFyYW0ge1VpbnQ4QXJyYXl9IHJlcXVlc3REYXRhIFJlcXVlc3QgZGF0YVxyXG4gKiBAcGFyYW0ge1JQQ0ltcGxDYWxsYmFja30gY2FsbGJhY2sgQ2FsbGJhY2sgZnVuY3Rpb25cclxuICogQHJldHVybnMge3VuZGVmaW5lZH1cclxuICogQGV4YW1wbGVcclxuICogZnVuY3Rpb24gcnBjSW1wbChtZXRob2QsIHJlcXVlc3REYXRhLCBjYWxsYmFjaykge1xyXG4gKiAgICAgaWYgKHByb3RvYnVmLnV0aWwubGNGaXJzdChtZXRob2QubmFtZSkgIT09IFwibXlNZXRob2RcIikgLy8gY29tcGF0aWJsZSB3aXRoIHN0YXRpYyBjb2RlXHJcbiAqICAgICAgICAgdGhyb3cgRXJyb3IoXCJubyBzdWNoIG1ldGhvZFwiKTtcclxuICogICAgIGFzeW5jaHJvbm91c2x5T2J0YWluQVJlc3BvbnNlKHJlcXVlc3REYXRhLCBmdW5jdGlvbihlcnIsIHJlc3BvbnNlRGF0YSkge1xyXG4gKiAgICAgICAgIGNhbGxiYWNrKGVyciwgcmVzcG9uc2VEYXRhKTtcclxuICogICAgIH0pO1xyXG4gKiB9XHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIE5vZGUtc3R5bGUgY2FsbGJhY2sgYXMgdXNlZCBieSB7QGxpbmsgUlBDSW1wbH0uXHJcbiAqIEB0eXBlZGVmIFJQQ0ltcGxDYWxsYmFja1xyXG4gKiBAdHlwZSB7ZnVuY3Rpb259XHJcbiAqIEBwYXJhbSB7RXJyb3J8bnVsbH0gZXJyb3IgRXJyb3IsIGlmIGFueSwgb3RoZXJ3aXNlIGBudWxsYFxyXG4gKiBAcGFyYW0ge1VpbnQ4QXJyYXl8bnVsbH0gW3Jlc3BvbnNlXSBSZXNwb25zZSBkYXRhIG9yIGBudWxsYCB0byBzaWduYWwgZW5kIG9mIHN0cmVhbSwgaWYgdGhlcmUgaGFzbid0IGJlZW4gYW4gZXJyb3JcclxuICogQHJldHVybnMge3VuZGVmaW5lZH1cclxuICovXHJcblxyXG5ycGMuU2VydmljZSA9IHJlcXVpcmUoXCIuL3JwYy9zZXJ2aWNlXCIpO1xyXG4iLCJcInVzZSBzdHJpY3RcIjtcclxubW9kdWxlLmV4cG9ydHMgPSBTZXJ2aWNlO1xyXG5cclxudmFyIHV0aWwgPSByZXF1aXJlKFwiLi4vdXRpbC9taW5pbWFsXCIpO1xyXG5cclxuLy8gRXh0ZW5kcyBFdmVudEVtaXR0ZXJcclxuKFNlcnZpY2UucHJvdG90eXBlID0gT2JqZWN0LmNyZWF0ZSh1dGlsLkV2ZW50RW1pdHRlci5wcm90b3R5cGUpKS5jb25zdHJ1Y3RvciA9IFNlcnZpY2U7XHJcblxyXG4vKipcclxuICogQSBzZXJ2aWNlIG1ldGhvZCBjYWxsYmFjayBhcyB1c2VkIGJ5IHtAbGluayBycGMuU2VydmljZU1ldGhvZHxTZXJ2aWNlTWV0aG9kfS5cclxuICpcclxuICogRGlmZmVycyBmcm9tIHtAbGluayBSUENJbXBsQ2FsbGJhY2t9IGluIHRoYXQgaXQgaXMgYW4gYWN0dWFsIGNhbGxiYWNrIG9mIGEgc2VydmljZSBtZXRob2Qgd2hpY2ggbWF5IG5vdCByZXR1cm4gYHJlc3BvbnNlID0gbnVsbGAuXHJcbiAqIEB0eXBlZGVmIHJwYy5TZXJ2aWNlTWV0aG9kQ2FsbGJhY2tcclxuICogQHRlbXBsYXRlIFRSZXMgZXh0ZW5kcyBNZXNzYWdlPFRSZXM+XHJcbiAqIEB0eXBlIHtmdW5jdGlvbn1cclxuICogQHBhcmFtIHtFcnJvcnxudWxsfSBlcnJvciBFcnJvciwgaWYgYW55XHJcbiAqIEBwYXJhbSB7VFJlc30gW3Jlc3BvbnNlXSBSZXNwb25zZSBtZXNzYWdlXHJcbiAqIEByZXR1cm5zIHt1bmRlZmluZWR9XHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIEEgc2VydmljZSBtZXRob2QgcGFydCBvZiBhIHtAbGluayBycGMuU2VydmljZX0gYXMgY3JlYXRlZCBieSB7QGxpbmsgU2VydmljZS5jcmVhdGV9LlxyXG4gKiBAdHlwZWRlZiBycGMuU2VydmljZU1ldGhvZFxyXG4gKiBAdGVtcGxhdGUgVFJlcSBleHRlbmRzIE1lc3NhZ2U8VFJlcT5cclxuICogQHRlbXBsYXRlIFRSZXMgZXh0ZW5kcyBNZXNzYWdlPFRSZXM+XHJcbiAqIEB0eXBlIHtmdW5jdGlvbn1cclxuICogQHBhcmFtIHtUUmVxfFByb3BlcnRpZXM8VFJlcT59IHJlcXVlc3QgUmVxdWVzdCBtZXNzYWdlIG9yIHBsYWluIG9iamVjdFxyXG4gKiBAcGFyYW0ge3JwYy5TZXJ2aWNlTWV0aG9kQ2FsbGJhY2s8VFJlcz59IFtjYWxsYmFja10gTm9kZS1zdHlsZSBjYWxsYmFjayBjYWxsZWQgd2l0aCB0aGUgZXJyb3IsIGlmIGFueSwgYW5kIHRoZSByZXNwb25zZSBtZXNzYWdlXHJcbiAqIEByZXR1cm5zIHtQcm9taXNlPE1lc3NhZ2U8VFJlcz4+fSBQcm9taXNlIGlmIGBjYWxsYmFja2AgaGFzIGJlZW4gb21pdHRlZCwgb3RoZXJ3aXNlIGB1bmRlZmluZWRgXHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIENvbnN0cnVjdHMgYSBuZXcgUlBDIHNlcnZpY2UgaW5zdGFuY2UuXHJcbiAqIEBjbGFzc2Rlc2MgQW4gUlBDIHNlcnZpY2UgYXMgcmV0dXJuZWQgYnkge0BsaW5rIFNlcnZpY2UjY3JlYXRlfS5cclxuICogQGV4cG9ydHMgcnBjLlNlcnZpY2VcclxuICogQGV4dGVuZHMgdXRpbC5FdmVudEVtaXR0ZXJcclxuICogQGNvbnN0cnVjdG9yXHJcbiAqIEBwYXJhbSB7UlBDSW1wbH0gcnBjSW1wbCBSUEMgaW1wbGVtZW50YXRpb25cclxuICogQHBhcmFtIHtib29sZWFufSBbcmVxdWVzdERlbGltaXRlZD1mYWxzZV0gV2hldGhlciByZXF1ZXN0cyBhcmUgbGVuZ3RoLWRlbGltaXRlZFxyXG4gKiBAcGFyYW0ge2Jvb2xlYW59IFtyZXNwb25zZURlbGltaXRlZD1mYWxzZV0gV2hldGhlciByZXNwb25zZXMgYXJlIGxlbmd0aC1kZWxpbWl0ZWRcclxuICovXHJcbmZ1bmN0aW9uIFNlcnZpY2UocnBjSW1wbCwgcmVxdWVzdERlbGltaXRlZCwgcmVzcG9uc2VEZWxpbWl0ZWQpIHtcclxuXHJcbiAgICBpZiAodHlwZW9mIHJwY0ltcGwgIT09IFwiZnVuY3Rpb25cIilcclxuICAgICAgICB0aHJvdyBUeXBlRXJyb3IoXCJycGNJbXBsIG11c3QgYmUgYSBmdW5jdGlvblwiKTtcclxuXHJcbiAgICB1dGlsLkV2ZW50RW1pdHRlci5jYWxsKHRoaXMpO1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogUlBDIGltcGxlbWVudGF0aW9uLiBCZWNvbWVzIGBudWxsYCBvbmNlIHRoZSBzZXJ2aWNlIGlzIGVuZGVkLlxyXG4gICAgICogQHR5cGUge1JQQ0ltcGx8bnVsbH1cclxuICAgICAqL1xyXG4gICAgdGhpcy5ycGNJbXBsID0gcnBjSW1wbDtcclxuXHJcbiAgICAvKipcclxuICAgICAqIFdoZXRoZXIgcmVxdWVzdHMgYXJlIGxlbmd0aC1kZWxpbWl0ZWQuXHJcbiAgICAgKiBAdHlwZSB7Ym9vbGVhbn1cclxuICAgICAqL1xyXG4gICAgdGhpcy5yZXF1ZXN0RGVsaW1pdGVkID0gQm9vbGVhbihyZXF1ZXN0RGVsaW1pdGVkKTtcclxuXHJcbiAgICAvKipcclxuICAgICAqIFdoZXRoZXIgcmVzcG9uc2VzIGFyZSBsZW5ndGgtZGVsaW1pdGVkLlxyXG4gICAgICogQHR5cGUge2Jvb2xlYW59XHJcbiAgICAgKi9cclxuICAgIHRoaXMucmVzcG9uc2VEZWxpbWl0ZWQgPSBCb29sZWFuKHJlc3BvbnNlRGVsaW1pdGVkKTtcclxufVxyXG5cclxuLyoqXHJcbiAqIENhbGxzIGEgc2VydmljZSBtZXRob2QgdGhyb3VnaCB7QGxpbmsgcnBjLlNlcnZpY2UjcnBjSW1wbHxycGNJbXBsfS5cclxuICogQHBhcmFtIHtNZXRob2R8cnBjLlNlcnZpY2VNZXRob2Q8VFJlcSxUUmVzPn0gbWV0aG9kIFJlZmxlY3RlZCBvciBzdGF0aWMgbWV0aG9kXHJcbiAqIEBwYXJhbSB7Q29uc3RydWN0b3I8VFJlcT59IHJlcXVlc3RDdG9yIFJlcXVlc3QgY29uc3RydWN0b3JcclxuICogQHBhcmFtIHtDb25zdHJ1Y3RvcjxUUmVzPn0gcmVzcG9uc2VDdG9yIFJlc3BvbnNlIGNvbnN0cnVjdG9yXHJcbiAqIEBwYXJhbSB7VFJlcXxQcm9wZXJ0aWVzPFRSZXE+fSByZXF1ZXN0IFJlcXVlc3QgbWVzc2FnZSBvciBwbGFpbiBvYmplY3RcclxuICogQHBhcmFtIHtycGMuU2VydmljZU1ldGhvZENhbGxiYWNrPFRSZXM+fSBjYWxsYmFjayBTZXJ2aWNlIGNhbGxiYWNrXHJcbiAqIEByZXR1cm5zIHt1bmRlZmluZWR9XHJcbiAqIEB0ZW1wbGF0ZSBUUmVxIGV4dGVuZHMgTWVzc2FnZTxUUmVxPlxyXG4gKiBAdGVtcGxhdGUgVFJlcyBleHRlbmRzIE1lc3NhZ2U8VFJlcz5cclxuICovXHJcblNlcnZpY2UucHJvdG90eXBlLnJwY0NhbGwgPSBmdW5jdGlvbiBycGNDYWxsKG1ldGhvZCwgcmVxdWVzdEN0b3IsIHJlc3BvbnNlQ3RvciwgcmVxdWVzdCwgY2FsbGJhY2spIHtcclxuXHJcbiAgICBpZiAoIXJlcXVlc3QpXHJcbiAgICAgICAgdGhyb3cgVHlwZUVycm9yKFwicmVxdWVzdCBtdXN0IGJlIHNwZWNpZmllZFwiKTtcclxuXHJcbiAgICB2YXIgc2VsZiA9IHRoaXM7XHJcbiAgICBpZiAoIWNhbGxiYWNrKVxyXG4gICAgICAgIHJldHVybiB1dGlsLmFzUHJvbWlzZShycGNDYWxsLCBzZWxmLCBtZXRob2QsIHJlcXVlc3RDdG9yLCByZXNwb25zZUN0b3IsIHJlcXVlc3QpO1xyXG5cclxuICAgIGlmICghc2VsZi5ycGNJbXBsKSB7XHJcbiAgICAgICAgc2V0VGltZW91dChmdW5jdGlvbigpIHsgY2FsbGJhY2soRXJyb3IoXCJhbHJlYWR5IGVuZGVkXCIpKTsgfSwgMCk7XHJcbiAgICAgICAgcmV0dXJuIHVuZGVmaW5lZDtcclxuICAgIH1cclxuXHJcbiAgICB0cnkge1xyXG4gICAgICAgIHJldHVybiBzZWxmLnJwY0ltcGwoXHJcbiAgICAgICAgICAgIG1ldGhvZCxcclxuICAgICAgICAgICAgcmVxdWVzdEN0b3Jbc2VsZi5yZXF1ZXN0RGVsaW1pdGVkID8gXCJlbmNvZGVEZWxpbWl0ZWRcIiA6IFwiZW5jb2RlXCJdKHJlcXVlc3QpLmZpbmlzaCgpLFxyXG4gICAgICAgICAgICBmdW5jdGlvbiBycGNDYWxsYmFjayhlcnIsIHJlc3BvbnNlKSB7XHJcblxyXG4gICAgICAgICAgICAgICAgaWYgKGVycikge1xyXG4gICAgICAgICAgICAgICAgICAgIHNlbGYuZW1pdChcImVycm9yXCIsIGVyciwgbWV0aG9kKTtcclxuICAgICAgICAgICAgICAgICAgICByZXR1cm4gY2FsbGJhY2soZXJyKTtcclxuICAgICAgICAgICAgICAgIH1cclxuXHJcbiAgICAgICAgICAgICAgICBpZiAocmVzcG9uc2UgPT09IG51bGwpIHtcclxuICAgICAgICAgICAgICAgICAgICBzZWxmLmVuZCgvKiBlbmRlZEJ5UlBDICovIHRydWUpO1xyXG4gICAgICAgICAgICAgICAgICAgIHJldHVybiB1bmRlZmluZWQ7XHJcbiAgICAgICAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgICAgICAgaWYgKCEocmVzcG9uc2UgaW5zdGFuY2VvZiByZXNwb25zZUN0b3IpKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgdHJ5IHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgcmVzcG9uc2UgPSByZXNwb25zZUN0b3Jbc2VsZi5yZXNwb25zZURlbGltaXRlZCA/IFwiZGVjb2RlRGVsaW1pdGVkXCIgOiBcImRlY29kZVwiXShyZXNwb25zZSk7XHJcbiAgICAgICAgICAgICAgICAgICAgfSBjYXRjaCAoZXJyKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHNlbGYuZW1pdChcImVycm9yXCIsIGVyciwgbWV0aG9kKTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgcmV0dXJuIGNhbGxiYWNrKGVycik7XHJcbiAgICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgfVxyXG5cclxuICAgICAgICAgICAgICAgIHNlbGYuZW1pdChcImRhdGFcIiwgcmVzcG9uc2UsIG1ldGhvZCk7XHJcbiAgICAgICAgICAgICAgICByZXR1cm4gY2FsbGJhY2sobnVsbCwgcmVzcG9uc2UpO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgKTtcclxuICAgIH0gY2F0Y2ggKGVycikge1xyXG4gICAgICAgIHNlbGYuZW1pdChcImVycm9yXCIsIGVyciwgbWV0aG9kKTtcclxuICAgICAgICBzZXRUaW1lb3V0KGZ1bmN0aW9uKCkgeyBjYWxsYmFjayhlcnIpOyB9LCAwKTtcclxuICAgICAgICByZXR1cm4gdW5kZWZpbmVkO1xyXG4gICAgfVxyXG59O1xyXG5cclxuLyoqXHJcbiAqIEVuZHMgdGhpcyBzZXJ2aWNlIGFuZCBlbWl0cyB0aGUgYGVuZGAgZXZlbnQuXHJcbiAqIEBwYXJhbSB7Ym9vbGVhbn0gW2VuZGVkQnlSUEM9ZmFsc2VdIFdoZXRoZXIgdGhlIHNlcnZpY2UgaGFzIGJlZW4gZW5kZWQgYnkgdGhlIFJQQyBpbXBsZW1lbnRhdGlvbi5cclxuICogQHJldHVybnMge3JwYy5TZXJ2aWNlfSBgdGhpc2BcclxuICovXHJcblNlcnZpY2UucHJvdG90eXBlLmVuZCA9IGZ1bmN0aW9uIGVuZChlbmRlZEJ5UlBDKSB7XHJcbiAgICBpZiAodGhpcy5ycGNJbXBsKSB7XHJcbiAgICAgICAgaWYgKCFlbmRlZEJ5UlBDKSAvLyBzaWduYWwgZW5kIHRvIHJwY0ltcGxcclxuICAgICAgICAgICAgdGhpcy5ycGNJbXBsKG51bGwsIG51bGwsIG51bGwpO1xyXG4gICAgICAgIHRoaXMucnBjSW1wbCA9IG51bGw7XHJcbiAgICAgICAgdGhpcy5lbWl0KFwiZW5kXCIpLm9mZigpO1xyXG4gICAgfVxyXG4gICAgcmV0dXJuIHRoaXM7XHJcbn07XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG5tb2R1bGUuZXhwb3J0cyA9IFNlcnZpY2U7XHJcblxyXG4vLyBleHRlbmRzIE5hbWVzcGFjZVxyXG52YXIgTmFtZXNwYWNlID0gcmVxdWlyZShcIi4vbmFtZXNwYWNlXCIpO1xyXG4oKFNlcnZpY2UucHJvdG90eXBlID0gT2JqZWN0LmNyZWF0ZShOYW1lc3BhY2UucHJvdG90eXBlKSkuY29uc3RydWN0b3IgPSBTZXJ2aWNlKS5jbGFzc05hbWUgPSBcIlNlcnZpY2VcIjtcclxuXHJcbnZhciBNZXRob2QgPSByZXF1aXJlKFwiLi9tZXRob2RcIiksXHJcbiAgICB1dGlsICAgPSByZXF1aXJlKFwiLi91dGlsXCIpLFxyXG4gICAgcnBjICAgID0gcmVxdWlyZShcIi4vcnBjXCIpO1xyXG5cclxuLyoqXHJcbiAqIENvbnN0cnVjdHMgYSBuZXcgc2VydmljZSBpbnN0YW5jZS5cclxuICogQGNsYXNzZGVzYyBSZWZsZWN0ZWQgc2VydmljZS5cclxuICogQGV4dGVuZHMgTmFtZXNwYWNlQmFzZVxyXG4gKiBAY29uc3RydWN0b3JcclxuICogQHBhcmFtIHtzdHJpbmd9IG5hbWUgU2VydmljZSBuYW1lXHJcbiAqIEBwYXJhbSB7T2JqZWN0LjxzdHJpbmcsKj59IFtvcHRpb25zXSBTZXJ2aWNlIG9wdGlvbnNcclxuICogQHRocm93cyB7VHlwZUVycm9yfSBJZiBhcmd1bWVudHMgYXJlIGludmFsaWRcclxuICovXHJcbmZ1bmN0aW9uIFNlcnZpY2UobmFtZSwgb3B0aW9ucykge1xyXG4gICAgTmFtZXNwYWNlLmNhbGwodGhpcywgbmFtZSwgb3B0aW9ucyk7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBTZXJ2aWNlIG1ldGhvZHMuXHJcbiAgICAgKiBAdHlwZSB7T2JqZWN0LjxzdHJpbmcsTWV0aG9kPn1cclxuICAgICAqL1xyXG4gICAgdGhpcy5tZXRob2RzID0ge307IC8vIHRvSlNPTiwgbWFya2VyXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBDYWNoZWQgbWV0aG9kcyBhcyBhbiBhcnJheS5cclxuICAgICAqIEB0eXBlIHtNZXRob2RbXXxudWxsfVxyXG4gICAgICogQHByaXZhdGVcclxuICAgICAqL1xyXG4gICAgdGhpcy5fbWV0aG9kc0FycmF5ID0gbnVsbDtcclxufVxyXG5cclxuLyoqXHJcbiAqIFNlcnZpY2UgZGVzY3JpcHRvci5cclxuICogQGludGVyZmFjZSBJU2VydmljZVxyXG4gKiBAZXh0ZW5kcyBJTmFtZXNwYWNlXHJcbiAqIEBwcm9wZXJ0eSB7T2JqZWN0LjxzdHJpbmcsSU1ldGhvZD59IG1ldGhvZHMgTWV0aG9kIGRlc2NyaXB0b3JzXHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIENvbnN0cnVjdHMgYSBzZXJ2aWNlIGZyb20gYSBzZXJ2aWNlIGRlc2NyaXB0b3IuXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBuYW1lIFNlcnZpY2UgbmFtZVxyXG4gKiBAcGFyYW0ge0lTZXJ2aWNlfSBqc29uIFNlcnZpY2UgZGVzY3JpcHRvclxyXG4gKiBAcmV0dXJucyB7U2VydmljZX0gQ3JlYXRlZCBzZXJ2aWNlXHJcbiAqIEB0aHJvd3Mge1R5cGVFcnJvcn0gSWYgYXJndW1lbnRzIGFyZSBpbnZhbGlkXHJcbiAqL1xyXG5TZXJ2aWNlLmZyb21KU09OID0gZnVuY3Rpb24gZnJvbUpTT04obmFtZSwganNvbikge1xyXG4gICAgdmFyIHNlcnZpY2UgPSBuZXcgU2VydmljZShuYW1lLCBqc29uLm9wdGlvbnMpO1xyXG4gICAgLyogaXN0YW5idWwgaWdub3JlIGVsc2UgKi9cclxuICAgIGlmIChqc29uLm1ldGhvZHMpXHJcbiAgICAgICAgZm9yICh2YXIgbmFtZXMgPSBPYmplY3Qua2V5cyhqc29uLm1ldGhvZHMpLCBpID0gMDsgaSA8IG5hbWVzLmxlbmd0aDsgKytpKVxyXG4gICAgICAgICAgICBzZXJ2aWNlLmFkZChNZXRob2QuZnJvbUpTT04obmFtZXNbaV0sIGpzb24ubWV0aG9kc1tuYW1lc1tpXV0pKTtcclxuICAgIGlmIChqc29uLm5lc3RlZClcclxuICAgICAgICBzZXJ2aWNlLmFkZEpTT04oanNvbi5uZXN0ZWQpO1xyXG4gICAgc2VydmljZS5jb21tZW50ID0ganNvbi5jb21tZW50O1xyXG4gICAgcmV0dXJuIHNlcnZpY2U7XHJcbn07XHJcblxyXG4vKipcclxuICogQ29udmVydHMgdGhpcyBzZXJ2aWNlIHRvIGEgc2VydmljZSBkZXNjcmlwdG9yLlxyXG4gKiBAcGFyYW0ge0lUb0pTT05PcHRpb25zfSBbdG9KU09OT3B0aW9uc10gSlNPTiBjb252ZXJzaW9uIG9wdGlvbnNcclxuICogQHJldHVybnMge0lTZXJ2aWNlfSBTZXJ2aWNlIGRlc2NyaXB0b3JcclxuICovXHJcblNlcnZpY2UucHJvdG90eXBlLnRvSlNPTiA9IGZ1bmN0aW9uIHRvSlNPTih0b0pTT05PcHRpb25zKSB7XHJcbiAgICB2YXIgaW5oZXJpdGVkID0gTmFtZXNwYWNlLnByb3RvdHlwZS50b0pTT04uY2FsbCh0aGlzLCB0b0pTT05PcHRpb25zKTtcclxuICAgIHZhciBrZWVwQ29tbWVudHMgPSB0b0pTT05PcHRpb25zID8gQm9vbGVhbih0b0pTT05PcHRpb25zLmtlZXBDb21tZW50cykgOiBmYWxzZTtcclxuICAgIHJldHVybiB1dGlsLnRvT2JqZWN0KFtcclxuICAgICAgICBcIm9wdGlvbnNcIiAsIGluaGVyaXRlZCAmJiBpbmhlcml0ZWQub3B0aW9ucyB8fCB1bmRlZmluZWQsXHJcbiAgICAgICAgXCJtZXRob2RzXCIgLCBOYW1lc3BhY2UuYXJyYXlUb0pTT04odGhpcy5tZXRob2RzQXJyYXksIHRvSlNPTk9wdGlvbnMpIHx8IC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0ICovIHt9LFxyXG4gICAgICAgIFwibmVzdGVkXCIgICwgaW5oZXJpdGVkICYmIGluaGVyaXRlZC5uZXN0ZWQgfHwgdW5kZWZpbmVkLFxyXG4gICAgICAgIFwiY29tbWVudFwiICwga2VlcENvbW1lbnRzID8gdGhpcy5jb21tZW50IDogdW5kZWZpbmVkXHJcbiAgICBdKTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBNZXRob2RzIG9mIHRoaXMgc2VydmljZSBhcyBhbiBhcnJheSBmb3IgaXRlcmF0aW9uLlxyXG4gKiBAbmFtZSBTZXJ2aWNlI21ldGhvZHNBcnJheVxyXG4gKiBAdHlwZSB7TWV0aG9kW119XHJcbiAqIEByZWFkb25seVxyXG4gKi9cclxuT2JqZWN0LmRlZmluZVByb3BlcnR5KFNlcnZpY2UucHJvdG90eXBlLCBcIm1ldGhvZHNBcnJheVwiLCB7XHJcbiAgICBnZXQ6IGZ1bmN0aW9uKCkge1xyXG4gICAgICAgIHJldHVybiB0aGlzLl9tZXRob2RzQXJyYXkgfHwgKHRoaXMuX21ldGhvZHNBcnJheSA9IHV0aWwudG9BcnJheSh0aGlzLm1ldGhvZHMpKTtcclxuICAgIH1cclxufSk7XHJcblxyXG5mdW5jdGlvbiBjbGVhckNhY2hlKHNlcnZpY2UpIHtcclxuICAgIHNlcnZpY2UuX21ldGhvZHNBcnJheSA9IG51bGw7XHJcbiAgICByZXR1cm4gc2VydmljZTtcclxufVxyXG5cclxuLyoqXHJcbiAqIEBvdmVycmlkZVxyXG4gKi9cclxuU2VydmljZS5wcm90b3R5cGUuZ2V0ID0gZnVuY3Rpb24gZ2V0KG5hbWUpIHtcclxuICAgIHJldHVybiB0aGlzLm1ldGhvZHNbbmFtZV1cclxuICAgICAgICB8fCBOYW1lc3BhY2UucHJvdG90eXBlLmdldC5jYWxsKHRoaXMsIG5hbWUpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIEBvdmVycmlkZVxyXG4gKi9cclxuU2VydmljZS5wcm90b3R5cGUucmVzb2x2ZUFsbCA9IGZ1bmN0aW9uIHJlc29sdmVBbGwoKSB7XHJcbiAgICB2YXIgbWV0aG9kcyA9IHRoaXMubWV0aG9kc0FycmF5O1xyXG4gICAgZm9yICh2YXIgaSA9IDA7IGkgPCBtZXRob2RzLmxlbmd0aDsgKytpKVxyXG4gICAgICAgIG1ldGhvZHNbaV0ucmVzb2x2ZSgpO1xyXG4gICAgcmV0dXJuIE5hbWVzcGFjZS5wcm90b3R5cGUucmVzb2x2ZS5jYWxsKHRoaXMpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIEBvdmVycmlkZVxyXG4gKi9cclxuU2VydmljZS5wcm90b3R5cGUuYWRkID0gZnVuY3Rpb24gYWRkKG9iamVjdCkge1xyXG5cclxuICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgaWYgKHRoaXMuZ2V0KG9iamVjdC5uYW1lKSlcclxuICAgICAgICB0aHJvdyBFcnJvcihcImR1cGxpY2F0ZSBuYW1lICdcIiArIG9iamVjdC5uYW1lICsgXCInIGluIFwiICsgdGhpcyk7XHJcblxyXG4gICAgaWYgKG9iamVjdCBpbnN0YW5jZW9mIE1ldGhvZCkge1xyXG4gICAgICAgIHRoaXMubWV0aG9kc1tvYmplY3QubmFtZV0gPSBvYmplY3Q7XHJcbiAgICAgICAgb2JqZWN0LnBhcmVudCA9IHRoaXM7XHJcbiAgICAgICAgcmV0dXJuIGNsZWFyQ2FjaGUodGhpcyk7XHJcbiAgICB9XHJcbiAgICByZXR1cm4gTmFtZXNwYWNlLnByb3RvdHlwZS5hZGQuY2FsbCh0aGlzLCBvYmplY3QpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIEBvdmVycmlkZVxyXG4gKi9cclxuU2VydmljZS5wcm90b3R5cGUucmVtb3ZlID0gZnVuY3Rpb24gcmVtb3ZlKG9iamVjdCkge1xyXG4gICAgaWYgKG9iamVjdCBpbnN0YW5jZW9mIE1ldGhvZCkge1xyXG5cclxuICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgaWYgKi9cclxuICAgICAgICBpZiAodGhpcy5tZXRob2RzW29iamVjdC5uYW1lXSAhPT0gb2JqZWN0KVxyXG4gICAgICAgICAgICB0aHJvdyBFcnJvcihvYmplY3QgKyBcIiBpcyBub3QgYSBtZW1iZXIgb2YgXCIgKyB0aGlzKTtcclxuXHJcbiAgICAgICAgZGVsZXRlIHRoaXMubWV0aG9kc1tvYmplY3QubmFtZV07XHJcbiAgICAgICAgb2JqZWN0LnBhcmVudCA9IG51bGw7XHJcbiAgICAgICAgcmV0dXJuIGNsZWFyQ2FjaGUodGhpcyk7XHJcbiAgICB9XHJcbiAgICByZXR1cm4gTmFtZXNwYWNlLnByb3RvdHlwZS5yZW1vdmUuY2FsbCh0aGlzLCBvYmplY3QpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIENyZWF0ZXMgYSBydW50aW1lIHNlcnZpY2UgdXNpbmcgdGhlIHNwZWNpZmllZCBycGMgaW1wbGVtZW50YXRpb24uXHJcbiAqIEBwYXJhbSB7UlBDSW1wbH0gcnBjSW1wbCBSUEMgaW1wbGVtZW50YXRpb25cclxuICogQHBhcmFtIHtib29sZWFufSBbcmVxdWVzdERlbGltaXRlZD1mYWxzZV0gV2hldGhlciByZXF1ZXN0cyBhcmUgbGVuZ3RoLWRlbGltaXRlZFxyXG4gKiBAcGFyYW0ge2Jvb2xlYW59IFtyZXNwb25zZURlbGltaXRlZD1mYWxzZV0gV2hldGhlciByZXNwb25zZXMgYXJlIGxlbmd0aC1kZWxpbWl0ZWRcclxuICogQHJldHVybnMge3JwYy5TZXJ2aWNlfSBSUEMgc2VydmljZS4gVXNlZnVsIHdoZXJlIHJlcXVlc3RzIGFuZC9vciByZXNwb25zZXMgYXJlIHN0cmVhbWVkLlxyXG4gKi9cclxuU2VydmljZS5wcm90b3R5cGUuY3JlYXRlID0gZnVuY3Rpb24gY3JlYXRlKHJwY0ltcGwsIHJlcXVlc3REZWxpbWl0ZWQsIHJlc3BvbnNlRGVsaW1pdGVkKSB7XHJcbiAgICB2YXIgcnBjU2VydmljZSA9IG5ldyBycGMuU2VydmljZShycGNJbXBsLCByZXF1ZXN0RGVsaW1pdGVkLCByZXNwb25zZURlbGltaXRlZCk7XHJcbiAgICBmb3IgKHZhciBpID0gMCwgbWV0aG9kOyBpIDwgLyogaW5pdGlhbGl6ZXMgKi8gdGhpcy5tZXRob2RzQXJyYXkubGVuZ3RoOyArK2kpIHtcclxuICAgICAgICB2YXIgbWV0aG9kTmFtZSA9IHV0aWwubGNGaXJzdCgobWV0aG9kID0gdGhpcy5fbWV0aG9kc0FycmF5W2ldKS5yZXNvbHZlKCkubmFtZSkucmVwbGFjZSgvW14kXFx3X10vZywgXCJcIik7XHJcbiAgICAgICAgcnBjU2VydmljZVttZXRob2ROYW1lXSA9IHV0aWwuY29kZWdlbihbXCJyXCIsXCJjXCJdLCB1dGlsLmlzUmVzZXJ2ZWQobWV0aG9kTmFtZSkgPyBtZXRob2ROYW1lICsgXCJfXCIgOiBtZXRob2ROYW1lKShcInJldHVybiB0aGlzLnJwY0NhbGwobSxxLHMscixjKVwiKSh7XHJcbiAgICAgICAgICAgIG06IG1ldGhvZCxcclxuICAgICAgICAgICAgcTogbWV0aG9kLnJlc29sdmVkUmVxdWVzdFR5cGUuY3RvcixcclxuICAgICAgICAgICAgczogbWV0aG9kLnJlc29sdmVkUmVzcG9uc2VUeXBlLmN0b3JcclxuICAgICAgICB9KTtcclxuICAgIH1cclxuICAgIHJldHVybiBycGNTZXJ2aWNlO1xyXG59O1xyXG4iLCJcInVzZSBzdHJpY3RcIjtcclxubW9kdWxlLmV4cG9ydHMgPSB0b2tlbml6ZTtcclxuXHJcbnZhciBkZWxpbVJlICAgICAgICA9IC9bXFxze309OzpbXFxdLCdcIigpPD5dL2csXHJcbiAgICBzdHJpbmdEb3VibGVSZSA9IC8oPzpcIihbXlwiXFxcXF0qKD86XFxcXC5bXlwiXFxcXF0qKSopXCIpL2csXHJcbiAgICBzdHJpbmdTaW5nbGVSZSA9IC8oPzonKFteJ1xcXFxdKig/OlxcXFwuW14nXFxcXF0qKSopJykvZztcclxuXHJcbnZhciBzZXRDb21tZW50UmUgPSAvXiAqWyovXSsgKi8sXHJcbiAgICBzZXRDb21tZW50QWx0UmUgPSAvXlxccypcXCo/XFwvKi8sXHJcbiAgICBzZXRDb21tZW50U3BsaXRSZSA9IC9cXG4vZyxcclxuICAgIHdoaXRlc3BhY2VSZSA9IC9cXHMvLFxyXG4gICAgdW5lc2NhcGVSZSA9IC9cXFxcKC4/KS9nO1xyXG5cclxudmFyIHVuZXNjYXBlTWFwID0ge1xyXG4gICAgXCIwXCI6IFwiXFwwXCIsXHJcbiAgICBcInJcIjogXCJcXHJcIixcclxuICAgIFwiblwiOiBcIlxcblwiLFxyXG4gICAgXCJ0XCI6IFwiXFx0XCJcclxufTtcclxuXHJcbi8qKlxyXG4gKiBVbmVzY2FwZXMgYSBzdHJpbmcuXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBzdHIgU3RyaW5nIHRvIHVuZXNjYXBlXHJcbiAqIEByZXR1cm5zIHtzdHJpbmd9IFVuZXNjYXBlZCBzdHJpbmdcclxuICogQHByb3BlcnR5IHtPYmplY3QuPHN0cmluZyxzdHJpbmc+fSBtYXAgU3BlY2lhbCBjaGFyYWN0ZXJzIG1hcFxyXG4gKiBAbWVtYmVyb2YgdG9rZW5pemVcclxuICovXHJcbmZ1bmN0aW9uIHVuZXNjYXBlKHN0cikge1xyXG4gICAgcmV0dXJuIHN0ci5yZXBsYWNlKHVuZXNjYXBlUmUsIGZ1bmN0aW9uKCQwLCAkMSkge1xyXG4gICAgICAgIHN3aXRjaCAoJDEpIHtcclxuICAgICAgICAgICAgY2FzZSBcIlxcXFxcIjpcclxuICAgICAgICAgICAgY2FzZSBcIlwiOlxyXG4gICAgICAgICAgICAgICAgcmV0dXJuICQxO1xyXG4gICAgICAgICAgICBkZWZhdWx0OlxyXG4gICAgICAgICAgICAgICAgcmV0dXJuIHVuZXNjYXBlTWFwWyQxXSB8fCBcIlwiO1xyXG4gICAgICAgIH1cclxuICAgIH0pO1xyXG59XHJcblxyXG50b2tlbml6ZS51bmVzY2FwZSA9IHVuZXNjYXBlO1xyXG5cclxuLyoqXHJcbiAqIEdldHMgdGhlIG5leHQgdG9rZW4gYW5kIGFkdmFuY2VzLlxyXG4gKiBAdHlwZWRlZiBUb2tlbml6ZXJIYW5kbGVOZXh0XHJcbiAqIEB0eXBlIHtmdW5jdGlvbn1cclxuICogQHJldHVybnMge3N0cmluZ3xudWxsfSBOZXh0IHRva2VuIG9yIGBudWxsYCBvbiBlb2ZcclxuICovXHJcblxyXG4vKipcclxuICogUGVla3MgZm9yIHRoZSBuZXh0IHRva2VuLlxyXG4gKiBAdHlwZWRlZiBUb2tlbml6ZXJIYW5kbGVQZWVrXHJcbiAqIEB0eXBlIHtmdW5jdGlvbn1cclxuICogQHJldHVybnMge3N0cmluZ3xudWxsfSBOZXh0IHRva2VuIG9yIGBudWxsYCBvbiBlb2ZcclxuICovXHJcblxyXG4vKipcclxuICogUHVzaGVzIGEgdG9rZW4gYmFjayB0byB0aGUgc3RhY2suXHJcbiAqIEB0eXBlZGVmIFRva2VuaXplckhhbmRsZVB1c2hcclxuICogQHR5cGUge2Z1bmN0aW9ufVxyXG4gKiBAcGFyYW0ge3N0cmluZ30gdG9rZW4gVG9rZW5cclxuICogQHJldHVybnMge3VuZGVmaW5lZH1cclxuICovXHJcblxyXG4vKipcclxuICogU2tpcHMgdGhlIG5leHQgdG9rZW4uXHJcbiAqIEB0eXBlZGVmIFRva2VuaXplckhhbmRsZVNraXBcclxuICogQHR5cGUge2Z1bmN0aW9ufVxyXG4gKiBAcGFyYW0ge3N0cmluZ30gZXhwZWN0ZWQgRXhwZWN0ZWQgdG9rZW5cclxuICogQHBhcmFtIHtib29sZWFufSBbb3B0aW9uYWw9ZmFsc2VdIElmIG9wdGlvbmFsXHJcbiAqIEByZXR1cm5zIHtib29sZWFufSBXaGV0aGVyIHRoZSB0b2tlbiBtYXRjaGVkXHJcbiAqIEB0aHJvd3Mge0Vycm9yfSBJZiB0aGUgdG9rZW4gZGlkbid0IG1hdGNoIGFuZCBpcyBub3Qgb3B0aW9uYWxcclxuICovXHJcblxyXG4vKipcclxuICogR2V0cyB0aGUgY29tbWVudCBvbiB0aGUgcHJldmlvdXMgbGluZSBvciwgYWx0ZXJuYXRpdmVseSwgdGhlIGxpbmUgY29tbWVudCBvbiB0aGUgc3BlY2lmaWVkIGxpbmUuXHJcbiAqIEB0eXBlZGVmIFRva2VuaXplckhhbmRsZUNtbnRcclxuICogQHR5cGUge2Z1bmN0aW9ufVxyXG4gKiBAcGFyYW0ge251bWJlcn0gW2xpbmVdIExpbmUgbnVtYmVyXHJcbiAqIEByZXR1cm5zIHtzdHJpbmd8bnVsbH0gQ29tbWVudCB0ZXh0IG9yIGBudWxsYCBpZiBub25lXHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIEhhbmRsZSBvYmplY3QgcmV0dXJuZWQgZnJvbSB7QGxpbmsgdG9rZW5pemV9LlxyXG4gKiBAaW50ZXJmYWNlIElUb2tlbml6ZXJIYW5kbGVcclxuICogQHByb3BlcnR5IHtUb2tlbml6ZXJIYW5kbGVOZXh0fSBuZXh0IEdldHMgdGhlIG5leHQgdG9rZW4gYW5kIGFkdmFuY2VzIChgbnVsbGAgb24gZW9mKVxyXG4gKiBAcHJvcGVydHkge1Rva2VuaXplckhhbmRsZVBlZWt9IHBlZWsgUGVla3MgZm9yIHRoZSBuZXh0IHRva2VuIChgbnVsbGAgb24gZW9mKVxyXG4gKiBAcHJvcGVydHkge1Rva2VuaXplckhhbmRsZVB1c2h9IHB1c2ggUHVzaGVzIGEgdG9rZW4gYmFjayB0byB0aGUgc3RhY2tcclxuICogQHByb3BlcnR5IHtUb2tlbml6ZXJIYW5kbGVTa2lwfSBza2lwIFNraXBzIGEgdG9rZW4sIHJldHVybnMgaXRzIHByZXNlbmNlIGFuZCBhZHZhbmNlcyBvciwgaWYgbm9uLW9wdGlvbmFsIGFuZCBub3QgcHJlc2VudCwgdGhyb3dzXHJcbiAqIEBwcm9wZXJ0eSB7VG9rZW5pemVySGFuZGxlQ21udH0gY21udCBHZXRzIHRoZSBjb21tZW50IG9uIHRoZSBwcmV2aW91cyBsaW5lIG9yIHRoZSBsaW5lIGNvbW1lbnQgb24gdGhlIHNwZWNpZmllZCBsaW5lLCBpZiBhbnlcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IGxpbmUgQ3VycmVudCBsaW5lIG51bWJlclxyXG4gKi9cclxuXHJcbi8qKlxyXG4gKiBUb2tlbml6ZXMgdGhlIGdpdmVuIC5wcm90byBzb3VyY2UgYW5kIHJldHVybnMgYW4gb2JqZWN0IHdpdGggdXNlZnVsIHV0aWxpdHkgZnVuY3Rpb25zLlxyXG4gKiBAcGFyYW0ge3N0cmluZ30gc291cmNlIFNvdXJjZSBjb250ZW50c1xyXG4gKiBAcGFyYW0ge2Jvb2xlYW59IGFsdGVybmF0ZUNvbW1lbnRNb2RlIFdoZXRoZXIgd2Ugc2hvdWxkIGFjdGl2YXRlIGFsdGVybmF0ZSBjb21tZW50IHBhcnNpbmcgbW9kZS5cclxuICogQHJldHVybnMge0lUb2tlbml6ZXJIYW5kbGV9IFRva2VuaXplciBoYW5kbGVcclxuICovXHJcbmZ1bmN0aW9uIHRva2VuaXplKHNvdXJjZSwgYWx0ZXJuYXRlQ29tbWVudE1vZGUpIHtcclxuICAgIC8qIGVzbGludC1kaXNhYmxlIGNhbGxiYWNrLXJldHVybiAqL1xyXG4gICAgc291cmNlID0gc291cmNlLnRvU3RyaW5nKCk7XHJcblxyXG4gICAgdmFyIG9mZnNldCA9IDAsXHJcbiAgICAgICAgbGVuZ3RoID0gc291cmNlLmxlbmd0aCxcclxuICAgICAgICBsaW5lID0gMSxcclxuICAgICAgICBsYXN0Q29tbWVudExpbmUgPSAwLFxyXG4gICAgICAgIGNvbW1lbnRzID0ge307XHJcblxyXG4gICAgdmFyIHN0YWNrID0gW107XHJcblxyXG4gICAgdmFyIHN0cmluZ0RlbGltID0gbnVsbDtcclxuXHJcbiAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgbmV4dCAqL1xyXG4gICAgLyoqXHJcbiAgICAgKiBDcmVhdGVzIGFuIGVycm9yIGZvciBpbGxlZ2FsIHN5bnRheC5cclxuICAgICAqIEBwYXJhbSB7c3RyaW5nfSBzdWJqZWN0IFN1YmplY3RcclxuICAgICAqIEByZXR1cm5zIHtFcnJvcn0gRXJyb3IgY3JlYXRlZFxyXG4gICAgICogQGlubmVyXHJcbiAgICAgKi9cclxuICAgIGZ1bmN0aW9uIGlsbGVnYWwoc3ViamVjdCkge1xyXG4gICAgICAgIHJldHVybiBFcnJvcihcImlsbGVnYWwgXCIgKyBzdWJqZWN0ICsgXCIgKGxpbmUgXCIgKyBsaW5lICsgXCIpXCIpO1xyXG4gICAgfVxyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmVhZHMgYSBzdHJpbmcgdGlsbCBpdHMgZW5kLlxyXG4gICAgICogQHJldHVybnMge3N0cmluZ30gU3RyaW5nIHJlYWRcclxuICAgICAqIEBpbm5lclxyXG4gICAgICovXHJcbiAgICBmdW5jdGlvbiByZWFkU3RyaW5nKCkge1xyXG4gICAgICAgIHZhciByZSA9IHN0cmluZ0RlbGltID09PSBcIidcIiA/IHN0cmluZ1NpbmdsZVJlIDogc3RyaW5nRG91YmxlUmU7XHJcbiAgICAgICAgcmUubGFzdEluZGV4ID0gb2Zmc2V0IC0gMTtcclxuICAgICAgICB2YXIgbWF0Y2ggPSByZS5leGVjKHNvdXJjZSk7XHJcbiAgICAgICAgaWYgKCFtYXRjaClcclxuICAgICAgICAgICAgdGhyb3cgaWxsZWdhbChcInN0cmluZ1wiKTtcclxuICAgICAgICBvZmZzZXQgPSByZS5sYXN0SW5kZXg7XHJcbiAgICAgICAgcHVzaChzdHJpbmdEZWxpbSk7XHJcbiAgICAgICAgc3RyaW5nRGVsaW0gPSBudWxsO1xyXG4gICAgICAgIHJldHVybiB1bmVzY2FwZShtYXRjaFsxXSk7XHJcbiAgICB9XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBHZXRzIHRoZSBjaGFyYWN0ZXIgYXQgYHBvc2Agd2l0aGluIHRoZSBzb3VyY2UuXHJcbiAgICAgKiBAcGFyYW0ge251bWJlcn0gcG9zIFBvc2l0aW9uXHJcbiAgICAgKiBAcmV0dXJucyB7c3RyaW5nfSBDaGFyYWN0ZXJcclxuICAgICAqIEBpbm5lclxyXG4gICAgICovXHJcbiAgICBmdW5jdGlvbiBjaGFyQXQocG9zKSB7XHJcbiAgICAgICAgcmV0dXJuIHNvdXJjZS5jaGFyQXQocG9zKTtcclxuICAgIH1cclxuXHJcbiAgICAvKipcclxuICAgICAqIFNldHMgdGhlIGN1cnJlbnQgY29tbWVudCB0ZXh0LlxyXG4gICAgICogQHBhcmFtIHtudW1iZXJ9IHN0YXJ0IFN0YXJ0IG9mZnNldFxyXG4gICAgICogQHBhcmFtIHtudW1iZXJ9IGVuZCBFbmQgb2Zmc2V0XHJcbiAgICAgKiBAcGFyYW0ge2Jvb2xlYW59IGlzTGVhZGluZyBzZXQgaWYgYSBsZWFkaW5nIGNvbW1lbnRcclxuICAgICAqIEByZXR1cm5zIHt1bmRlZmluZWR9XHJcbiAgICAgKiBAaW5uZXJcclxuICAgICAqL1xyXG4gICAgZnVuY3Rpb24gc2V0Q29tbWVudChzdGFydCwgZW5kLCBpc0xlYWRpbmcpIHtcclxuICAgICAgICB2YXIgY29tbWVudCA9IHtcclxuICAgICAgICAgICAgdHlwZTogc291cmNlLmNoYXJBdChzdGFydCsrKSxcclxuICAgICAgICAgICAgbGluZUVtcHR5OiBmYWxzZSxcclxuICAgICAgICAgICAgbGVhZGluZzogaXNMZWFkaW5nLFxyXG4gICAgICAgIH07XHJcbiAgICAgICAgdmFyIGxvb2tiYWNrO1xyXG4gICAgICAgIGlmIChhbHRlcm5hdGVDb21tZW50TW9kZSkge1xyXG4gICAgICAgICAgICBsb29rYmFjayA9IDI7ICAvLyBhbHRlcm5hdGUgY29tbWVudCBwYXJzaW5nOiBcIi8vXCIgb3IgXCIvKlwiXHJcbiAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgbG9va2JhY2sgPSAzOyAgLy8gXCIvLy9cIiBvciBcIi8qKlwiXHJcbiAgICAgICAgfVxyXG4gICAgICAgIHZhciBjb21tZW50T2Zmc2V0ID0gc3RhcnQgLSBsb29rYmFjayxcclxuICAgICAgICAgICAgYztcclxuICAgICAgICBkbyB7XHJcbiAgICAgICAgICAgIGlmICgtLWNvbW1lbnRPZmZzZXQgPCAwIHx8XHJcbiAgICAgICAgICAgICAgICAgICAgKGMgPSBzb3VyY2UuY2hhckF0KGNvbW1lbnRPZmZzZXQpKSA9PT0gXCJcXG5cIikge1xyXG4gICAgICAgICAgICAgICAgY29tbWVudC5saW5lRW1wdHkgPSB0cnVlO1xyXG4gICAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9IHdoaWxlIChjID09PSBcIiBcIiB8fCBjID09PSBcIlxcdFwiKTtcclxuICAgICAgICB2YXIgbGluZXMgPSBzb3VyY2VcclxuICAgICAgICAgICAgLnN1YnN0cmluZyhzdGFydCwgZW5kKVxyXG4gICAgICAgICAgICAuc3BsaXQoc2V0Q29tbWVudFNwbGl0UmUpO1xyXG4gICAgICAgIGZvciAodmFyIGkgPSAwOyBpIDwgbGluZXMubGVuZ3RoOyArK2kpXHJcbiAgICAgICAgICAgIGxpbmVzW2ldID0gbGluZXNbaV1cclxuICAgICAgICAgICAgICAgIC5yZXBsYWNlKGFsdGVybmF0ZUNvbW1lbnRNb2RlID8gc2V0Q29tbWVudEFsdFJlIDogc2V0Q29tbWVudFJlLCBcIlwiKVxyXG4gICAgICAgICAgICAgICAgLnRyaW0oKTtcclxuICAgICAgICBjb21tZW50LnRleHQgPSBsaW5lc1xyXG4gICAgICAgICAgICAuam9pbihcIlxcblwiKVxyXG4gICAgICAgICAgICAudHJpbSgpO1xyXG5cclxuICAgICAgICBjb21tZW50c1tsaW5lXSA9IGNvbW1lbnQ7XHJcbiAgICAgICAgbGFzdENvbW1lbnRMaW5lID0gbGluZTtcclxuICAgIH1cclxuXHJcbiAgICBmdW5jdGlvbiBpc0RvdWJsZVNsYXNoQ29tbWVudExpbmUoc3RhcnRPZmZzZXQpIHtcclxuICAgICAgICB2YXIgZW5kT2Zmc2V0ID0gZmluZEVuZE9mTGluZShzdGFydE9mZnNldCk7XHJcblxyXG4gICAgICAgIC8vIHNlZSBpZiByZW1haW5pbmcgbGluZSBtYXRjaGVzIGNvbW1lbnQgcGF0dGVyblxyXG4gICAgICAgIHZhciBsaW5lVGV4dCA9IHNvdXJjZS5zdWJzdHJpbmcoc3RhcnRPZmZzZXQsIGVuZE9mZnNldCk7XHJcbiAgICAgICAgdmFyIGlzQ29tbWVudCA9IC9eXFxzKlxcL1xcLy8udGVzdChsaW5lVGV4dCk7XHJcbiAgICAgICAgcmV0dXJuIGlzQ29tbWVudDtcclxuICAgIH1cclxuXHJcbiAgICBmdW5jdGlvbiBmaW5kRW5kT2ZMaW5lKGN1cnNvcikge1xyXG4gICAgICAgIC8vIGZpbmQgZW5kIG9mIGN1cnNvcidzIGxpbmVcclxuICAgICAgICB2YXIgZW5kT2Zmc2V0ID0gY3Vyc29yO1xyXG4gICAgICAgIHdoaWxlIChlbmRPZmZzZXQgPCBsZW5ndGggJiYgY2hhckF0KGVuZE9mZnNldCkgIT09IFwiXFxuXCIpIHtcclxuICAgICAgICAgICAgZW5kT2Zmc2V0Kys7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIHJldHVybiBlbmRPZmZzZXQ7XHJcbiAgICB9XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBPYnRhaW5zIHRoZSBuZXh0IHRva2VuLlxyXG4gICAgICogQHJldHVybnMge3N0cmluZ3xudWxsfSBOZXh0IHRva2VuIG9yIGBudWxsYCBvbiBlb2ZcclxuICAgICAqIEBpbm5lclxyXG4gICAgICovXHJcbiAgICBmdW5jdGlvbiBuZXh0KCkge1xyXG4gICAgICAgIGlmIChzdGFjay5sZW5ndGggPiAwKVxyXG4gICAgICAgICAgICByZXR1cm4gc3RhY2suc2hpZnQoKTtcclxuICAgICAgICBpZiAoc3RyaW5nRGVsaW0pXHJcbiAgICAgICAgICAgIHJldHVybiByZWFkU3RyaW5nKCk7XHJcbiAgICAgICAgdmFyIHJlcGVhdCxcclxuICAgICAgICAgICAgcHJldixcclxuICAgICAgICAgICAgY3VycixcclxuICAgICAgICAgICAgc3RhcnQsXHJcbiAgICAgICAgICAgIGlzRG9jLFxyXG4gICAgICAgICAgICBpc0xlYWRpbmdDb21tZW50ID0gb2Zmc2V0ID09PSAwO1xyXG4gICAgICAgIGRvIHtcclxuICAgICAgICAgICAgaWYgKG9mZnNldCA9PT0gbGVuZ3RoKVxyXG4gICAgICAgICAgICAgICAgcmV0dXJuIG51bGw7XHJcbiAgICAgICAgICAgIHJlcGVhdCA9IGZhbHNlO1xyXG4gICAgICAgICAgICB3aGlsZSAod2hpdGVzcGFjZVJlLnRlc3QoY3VyciA9IGNoYXJBdChvZmZzZXQpKSkge1xyXG4gICAgICAgICAgICAgICAgaWYgKGN1cnIgPT09IFwiXFxuXCIpIHtcclxuICAgICAgICAgICAgICAgICAgICBpc0xlYWRpbmdDb21tZW50ID0gdHJ1ZTtcclxuICAgICAgICAgICAgICAgICAgICArK2xpbmU7XHJcbiAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICBpZiAoKytvZmZzZXQgPT09IGxlbmd0aClcclxuICAgICAgICAgICAgICAgICAgICByZXR1cm4gbnVsbDtcclxuICAgICAgICAgICAgfVxyXG5cclxuICAgICAgICAgICAgaWYgKGNoYXJBdChvZmZzZXQpID09PSBcIi9cIikge1xyXG4gICAgICAgICAgICAgICAgaWYgKCsrb2Zmc2V0ID09PSBsZW5ndGgpIHtcclxuICAgICAgICAgICAgICAgICAgICB0aHJvdyBpbGxlZ2FsKFwiY29tbWVudFwiKTtcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgIGlmIChjaGFyQXQob2Zmc2V0KSA9PT0gXCIvXCIpIHsgLy8gTGluZVxyXG4gICAgICAgICAgICAgICAgICAgIGlmICghYWx0ZXJuYXRlQ29tbWVudE1vZGUpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgLy8gY2hlY2sgZm9yIHRyaXBsZS1zbGFzaCBjb21tZW50XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGlzRG9jID0gY2hhckF0KHN0YXJ0ID0gb2Zmc2V0ICsgMSkgPT09IFwiL1wiO1xyXG5cclxuICAgICAgICAgICAgICAgICAgICAgICAgd2hpbGUgKGNoYXJBdCgrK29mZnNldCkgIT09IFwiXFxuXCIpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGlmIChvZmZzZXQgPT09IGxlbmd0aCkge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHJldHVybiBudWxsO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICsrb2Zmc2V0O1xyXG4gICAgICAgICAgICAgICAgICAgICAgICBpZiAoaXNEb2MpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHNldENvbW1lbnQoc3RhcnQsIG9mZnNldCAtIDEsIGlzTGVhZGluZ0NvbW1lbnQpO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgLy8gVHJhaWxpbmcgY29tbWVudCBjYW5ub3Qgbm90IGJlIG11bHRpLWxpbmUsXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAvLyBzbyBsZWFkaW5nIGNvbW1lbnQgc3RhdGUgc2hvdWxkIGJlIHJlc2V0IHRvIGhhbmRsZSBwb3RlbnRpYWwgbmV4dCBjb21tZW50c1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgaXNMZWFkaW5nQ29tbWVudCA9IHRydWU7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgICAgICAgICAgKytsaW5lO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICByZXBlYXQgPSB0cnVlO1xyXG4gICAgICAgICAgICAgICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIC8vIGNoZWNrIGZvciBkb3VibGUtc2xhc2ggY29tbWVudHMsIGNvbnNvbGlkYXRpbmcgY29uc2VjdXRpdmUgbGluZXNcclxuICAgICAgICAgICAgICAgICAgICAgICAgc3RhcnQgPSBvZmZzZXQ7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGlzRG9jID0gZmFsc2U7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGlmIChpc0RvdWJsZVNsYXNoQ29tbWVudExpbmUob2Zmc2V0IC0gMSkpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGlzRG9jID0gdHJ1ZTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGRvIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBvZmZzZXQgPSBmaW5kRW5kT2ZMaW5lKG9mZnNldCk7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgaWYgKG9mZnNldCA9PT0gbGVuZ3RoKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGJyZWFrO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBvZmZzZXQrKztcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBpZiAoIWlzTGVhZGluZ0NvbW1lbnQpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgLy8gVHJhaWxpbmcgY29tbWVudCBjYW5ub3Qgbm90IGJlIG11bHRpLWxpbmVcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgfSB3aGlsZSAoaXNEb3VibGVTbGFzaENvbW1lbnRMaW5lKG9mZnNldCkpO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgb2Zmc2V0ID0gTWF0aC5taW4obGVuZ3RoLCBmaW5kRW5kT2ZMaW5lKG9mZnNldCkgKyAxKTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgICAgICAgICBpZiAoaXNEb2MpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHNldENvbW1lbnQoc3RhcnQsIG9mZnNldCwgaXNMZWFkaW5nQ29tbWVudCk7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBpc0xlYWRpbmdDb21tZW50ID0gdHJ1ZTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgICAgICAgICBsaW5lKys7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHJlcGVhdCA9IHRydWU7XHJcbiAgICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgfSBlbHNlIGlmICgoY3VyciA9IGNoYXJBdChvZmZzZXQpKSA9PT0gXCIqXCIpIHsgLyogQmxvY2sgKi9cclxuICAgICAgICAgICAgICAgICAgICAvLyBjaGVjayBmb3IgLyoqIChyZWd1bGFyIGNvbW1lbnQgbW9kZSkgb3IgLyogKGFsdGVybmF0ZSBjb21tZW50IG1vZGUpXHJcbiAgICAgICAgICAgICAgICAgICAgc3RhcnQgPSBvZmZzZXQgKyAxO1xyXG4gICAgICAgICAgICAgICAgICAgIGlzRG9jID0gYWx0ZXJuYXRlQ29tbWVudE1vZGUgfHwgY2hhckF0KHN0YXJ0KSA9PT0gXCIqXCI7XHJcbiAgICAgICAgICAgICAgICAgICAgZG8ge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICBpZiAoY3VyciA9PT0gXCJcXG5cIikge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgKytsaW5lO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGlmICgrK29mZnNldCA9PT0gbGVuZ3RoKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB0aHJvdyBpbGxlZ2FsKFwiY29tbWVudFwiKTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgICAgICAgICBwcmV2ID0gY3VycjtcclxuICAgICAgICAgICAgICAgICAgICAgICAgY3VyciA9IGNoYXJBdChvZmZzZXQpO1xyXG4gICAgICAgICAgICAgICAgICAgIH0gd2hpbGUgKHByZXYgIT09IFwiKlwiIHx8IGN1cnIgIT09IFwiL1wiKTtcclxuICAgICAgICAgICAgICAgICAgICArK29mZnNldDtcclxuICAgICAgICAgICAgICAgICAgICBpZiAoaXNEb2MpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgc2V0Q29tbWVudChzdGFydCwgb2Zmc2V0IC0gMiwgaXNMZWFkaW5nQ29tbWVudCk7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGlzTGVhZGluZ0NvbW1lbnQgPSB0cnVlO1xyXG4gICAgICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgICAgICByZXBlYXQgPSB0cnVlO1xyXG4gICAgICAgICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgICAgICAgICByZXR1cm4gXCIvXCI7XHJcbiAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9IHdoaWxlIChyZXBlYXQpO1xyXG5cclxuICAgICAgICAvLyBvZmZzZXQgIT09IGxlbmd0aCBpZiB3ZSBnb3QgaGVyZVxyXG5cclxuICAgICAgICB2YXIgZW5kID0gb2Zmc2V0O1xyXG4gICAgICAgIGRlbGltUmUubGFzdEluZGV4ID0gMDtcclxuICAgICAgICB2YXIgZGVsaW0gPSBkZWxpbVJlLnRlc3QoY2hhckF0KGVuZCsrKSk7XHJcbiAgICAgICAgaWYgKCFkZWxpbSlcclxuICAgICAgICAgICAgd2hpbGUgKGVuZCA8IGxlbmd0aCAmJiAhZGVsaW1SZS50ZXN0KGNoYXJBdChlbmQpKSlcclxuICAgICAgICAgICAgICAgICsrZW5kO1xyXG4gICAgICAgIHZhciB0b2tlbiA9IHNvdXJjZS5zdWJzdHJpbmcob2Zmc2V0LCBvZmZzZXQgPSBlbmQpO1xyXG4gICAgICAgIGlmICh0b2tlbiA9PT0gXCJcXFwiXCIgfHwgdG9rZW4gPT09IFwiJ1wiKVxyXG4gICAgICAgICAgICBzdHJpbmdEZWxpbSA9IHRva2VuO1xyXG4gICAgICAgIHJldHVybiB0b2tlbjtcclxuICAgIH1cclxuXHJcbiAgICAvKipcclxuICAgICAqIFB1c2hlcyBhIHRva2VuIGJhY2sgdG8gdGhlIHN0YWNrLlxyXG4gICAgICogQHBhcmFtIHtzdHJpbmd9IHRva2VuIFRva2VuXHJcbiAgICAgKiBAcmV0dXJucyB7dW5kZWZpbmVkfVxyXG4gICAgICogQGlubmVyXHJcbiAgICAgKi9cclxuICAgIGZ1bmN0aW9uIHB1c2godG9rZW4pIHtcclxuICAgICAgICBzdGFjay5wdXNoKHRva2VuKTtcclxuICAgIH1cclxuXHJcbiAgICAvKipcclxuICAgICAqIFBlZWtzIGZvciB0aGUgbmV4dCB0b2tlbi5cclxuICAgICAqIEByZXR1cm5zIHtzdHJpbmd8bnVsbH0gVG9rZW4gb3IgYG51bGxgIG9uIGVvZlxyXG4gICAgICogQGlubmVyXHJcbiAgICAgKi9cclxuICAgIGZ1bmN0aW9uIHBlZWsoKSB7XHJcbiAgICAgICAgaWYgKCFzdGFjay5sZW5ndGgpIHtcclxuICAgICAgICAgICAgdmFyIHRva2VuID0gbmV4dCgpO1xyXG4gICAgICAgICAgICBpZiAodG9rZW4gPT09IG51bGwpXHJcbiAgICAgICAgICAgICAgICByZXR1cm4gbnVsbDtcclxuICAgICAgICAgICAgcHVzaCh0b2tlbik7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIHJldHVybiBzdGFja1swXTtcclxuICAgIH1cclxuXHJcbiAgICAvKipcclxuICAgICAqIFNraXBzIGEgdG9rZW4uXHJcbiAgICAgKiBAcGFyYW0ge3N0cmluZ30gZXhwZWN0ZWQgRXhwZWN0ZWQgdG9rZW5cclxuICAgICAqIEBwYXJhbSB7Ym9vbGVhbn0gW29wdGlvbmFsPWZhbHNlXSBXaGV0aGVyIHRoZSB0b2tlbiBpcyBvcHRpb25hbFxyXG4gICAgICogQHJldHVybnMge2Jvb2xlYW59IGB0cnVlYCB3aGVuIHNraXBwZWQsIGBmYWxzZWAgaWYgbm90XHJcbiAgICAgKiBAdGhyb3dzIHtFcnJvcn0gV2hlbiBhIHJlcXVpcmVkIHRva2VuIGlzIG5vdCBwcmVzZW50XHJcbiAgICAgKiBAaW5uZXJcclxuICAgICAqL1xyXG4gICAgZnVuY3Rpb24gc2tpcChleHBlY3RlZCwgb3B0aW9uYWwpIHtcclxuICAgICAgICB2YXIgYWN0dWFsID0gcGVlaygpLFxyXG4gICAgICAgICAgICBlcXVhbHMgPSBhY3R1YWwgPT09IGV4cGVjdGVkO1xyXG4gICAgICAgIGlmIChlcXVhbHMpIHtcclxuICAgICAgICAgICAgbmV4dCgpO1xyXG4gICAgICAgICAgICByZXR1cm4gdHJ1ZTtcclxuICAgICAgICB9XHJcbiAgICAgICAgaWYgKCFvcHRpb25hbClcclxuICAgICAgICAgICAgdGhyb3cgaWxsZWdhbChcInRva2VuICdcIiArIGFjdHVhbCArIFwiJywgJ1wiICsgZXhwZWN0ZWQgKyBcIicgZXhwZWN0ZWRcIik7XHJcbiAgICAgICAgcmV0dXJuIGZhbHNlO1xyXG4gICAgfVxyXG5cclxuICAgIC8qKlxyXG4gICAgICogR2V0cyBhIGNvbW1lbnQuXHJcbiAgICAgKiBAcGFyYW0ge251bWJlcn0gW3RyYWlsaW5nTGluZV0gTGluZSBudW1iZXIgaWYgbG9va2luZyBmb3IgYSB0cmFpbGluZyBjb21tZW50XHJcbiAgICAgKiBAcmV0dXJucyB7c3RyaW5nfG51bGx9IENvbW1lbnQgdGV4dFxyXG4gICAgICogQGlubmVyXHJcbiAgICAgKi9cclxuICAgIGZ1bmN0aW9uIGNtbnQodHJhaWxpbmdMaW5lKSB7XHJcbiAgICAgICAgdmFyIHJldCA9IG51bGw7XHJcbiAgICAgICAgdmFyIGNvbW1lbnQ7XHJcbiAgICAgICAgaWYgKHRyYWlsaW5nTGluZSA9PT0gdW5kZWZpbmVkKSB7XHJcbiAgICAgICAgICAgIGNvbW1lbnQgPSBjb21tZW50c1tsaW5lIC0gMV07XHJcbiAgICAgICAgICAgIGRlbGV0ZSBjb21tZW50c1tsaW5lIC0gMV07XHJcbiAgICAgICAgICAgIGlmIChjb21tZW50ICYmIChhbHRlcm5hdGVDb21tZW50TW9kZSB8fCBjb21tZW50LnR5cGUgPT09IFwiKlwiIHx8IGNvbW1lbnQubGluZUVtcHR5KSkge1xyXG4gICAgICAgICAgICAgICAgcmV0ID0gY29tbWVudC5sZWFkaW5nID8gY29tbWVudC50ZXh0IDogbnVsbDtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBlbHNlICovXHJcbiAgICAgICAgICAgIGlmIChsYXN0Q29tbWVudExpbmUgPCB0cmFpbGluZ0xpbmUpIHtcclxuICAgICAgICAgICAgICAgIHBlZWsoKTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICBjb21tZW50ID0gY29tbWVudHNbdHJhaWxpbmdMaW5lXTtcclxuICAgICAgICAgICAgZGVsZXRlIGNvbW1lbnRzW3RyYWlsaW5nTGluZV07XHJcbiAgICAgICAgICAgIGlmIChjb21tZW50ICYmICFjb21tZW50LmxpbmVFbXB0eSAmJiAoYWx0ZXJuYXRlQ29tbWVudE1vZGUgfHwgY29tbWVudC50eXBlID09PSBcIi9cIikpIHtcclxuICAgICAgICAgICAgICAgIHJldCA9IGNvbW1lbnQubGVhZGluZyA/IG51bGwgOiBjb21tZW50LnRleHQ7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICAgICAgcmV0dXJuIHJldDtcclxuICAgIH1cclxuXHJcbiAgICByZXR1cm4gT2JqZWN0LmRlZmluZVByb3BlcnR5KHtcclxuICAgICAgICBuZXh0OiBuZXh0LFxyXG4gICAgICAgIHBlZWs6IHBlZWssXHJcbiAgICAgICAgcHVzaDogcHVzaCxcclxuICAgICAgICBza2lwOiBza2lwLFxyXG4gICAgICAgIGNtbnQ6IGNtbnRcclxuICAgIH0sIFwibGluZVwiLCB7XHJcbiAgICAgICAgZ2V0OiBmdW5jdGlvbigpIHsgcmV0dXJuIGxpbmU7IH1cclxuICAgIH0pO1xyXG4gICAgLyogZXNsaW50LWVuYWJsZSBjYWxsYmFjay1yZXR1cm4gKi9cclxufVxyXG4iLCJcInVzZSBzdHJpY3RcIjtcclxubW9kdWxlLmV4cG9ydHMgPSBUeXBlO1xyXG5cclxuLy8gZXh0ZW5kcyBOYW1lc3BhY2VcclxudmFyIE5hbWVzcGFjZSA9IHJlcXVpcmUoXCIuL25hbWVzcGFjZVwiKTtcclxuKChUeXBlLnByb3RvdHlwZSA9IE9iamVjdC5jcmVhdGUoTmFtZXNwYWNlLnByb3RvdHlwZSkpLmNvbnN0cnVjdG9yID0gVHlwZSkuY2xhc3NOYW1lID0gXCJUeXBlXCI7XHJcblxyXG52YXIgRW51bSAgICAgID0gcmVxdWlyZShcIi4vZW51bVwiKSxcclxuICAgIE9uZU9mICAgICA9IHJlcXVpcmUoXCIuL29uZW9mXCIpLFxyXG4gICAgRmllbGQgICAgID0gcmVxdWlyZShcIi4vZmllbGRcIiksXHJcbiAgICBNYXBGaWVsZCAgPSByZXF1aXJlKFwiLi9tYXBmaWVsZFwiKSxcclxuICAgIFNlcnZpY2UgICA9IHJlcXVpcmUoXCIuL3NlcnZpY2VcIiksXHJcbiAgICBNZXNzYWdlICAgPSByZXF1aXJlKFwiLi9tZXNzYWdlXCIpLFxyXG4gICAgUmVhZGVyICAgID0gcmVxdWlyZShcIi4vcmVhZGVyXCIpLFxyXG4gICAgV3JpdGVyICAgID0gcmVxdWlyZShcIi4vd3JpdGVyXCIpLFxyXG4gICAgdXRpbCAgICAgID0gcmVxdWlyZShcIi4vdXRpbFwiKSxcclxuICAgIGVuY29kZXIgICA9IHJlcXVpcmUoXCIuL2VuY29kZXJcIiksXHJcbiAgICBkZWNvZGVyICAgPSByZXF1aXJlKFwiLi9kZWNvZGVyXCIpLFxyXG4gICAgdmVyaWZpZXIgID0gcmVxdWlyZShcIi4vdmVyaWZpZXJcIiksXHJcbiAgICBjb252ZXJ0ZXIgPSByZXF1aXJlKFwiLi9jb252ZXJ0ZXJcIiksXHJcbiAgICB3cmFwcGVycyAgPSByZXF1aXJlKFwiLi93cmFwcGVyc1wiKTtcclxuXHJcbi8qKlxyXG4gKiBDb25zdHJ1Y3RzIGEgbmV3IHJlZmxlY3RlZCBtZXNzYWdlIHR5cGUgaW5zdGFuY2UuXHJcbiAqIEBjbGFzc2Rlc2MgUmVmbGVjdGVkIG1lc3NhZ2UgdHlwZS5cclxuICogQGV4dGVuZHMgTmFtZXNwYWNlQmFzZVxyXG4gKiBAY29uc3RydWN0b3JcclxuICogQHBhcmFtIHtzdHJpbmd9IG5hbWUgTWVzc2FnZSBuYW1lXHJcbiAqIEBwYXJhbSB7T2JqZWN0LjxzdHJpbmcsKj59IFtvcHRpb25zXSBEZWNsYXJlZCBvcHRpb25zXHJcbiAqL1xyXG5mdW5jdGlvbiBUeXBlKG5hbWUsIG9wdGlvbnMpIHtcclxuICAgIE5hbWVzcGFjZS5jYWxsKHRoaXMsIG5hbWUsIG9wdGlvbnMpO1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogTWVzc2FnZSBmaWVsZHMuXHJcbiAgICAgKiBAdHlwZSB7T2JqZWN0LjxzdHJpbmcsRmllbGQ+fVxyXG4gICAgICovXHJcbiAgICB0aGlzLmZpZWxkcyA9IHt9OyAgLy8gdG9KU09OLCBtYXJrZXJcclxuXHJcbiAgICAvKipcclxuICAgICAqIE9uZW9mcyBkZWNsYXJlZCB3aXRoaW4gdGhpcyBuYW1lc3BhY2UsIGlmIGFueS5cclxuICAgICAqIEB0eXBlIHtPYmplY3QuPHN0cmluZyxPbmVPZj59XHJcbiAgICAgKi9cclxuICAgIHRoaXMub25lb2ZzID0gdW5kZWZpbmVkOyAvLyB0b0pTT05cclxuXHJcbiAgICAvKipcclxuICAgICAqIEV4dGVuc2lvbiByYW5nZXMsIGlmIGFueS5cclxuICAgICAqIEB0eXBlIHtudW1iZXJbXVtdfVxyXG4gICAgICovXHJcbiAgICB0aGlzLmV4dGVuc2lvbnMgPSB1bmRlZmluZWQ7IC8vIHRvSlNPTlxyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmVzZXJ2ZWQgcmFuZ2VzLCBpZiBhbnkuXHJcbiAgICAgKiBAdHlwZSB7QXJyYXkuPG51bWJlcltdfHN0cmluZz59XHJcbiAgICAgKi9cclxuICAgIHRoaXMucmVzZXJ2ZWQgPSB1bmRlZmluZWQ7IC8vIHRvSlNPTlxyXG5cclxuICAgIC8qP1xyXG4gICAgICogV2hldGhlciB0aGlzIHR5cGUgaXMgYSBsZWdhY3kgZ3JvdXAuXHJcbiAgICAgKiBAdHlwZSB7Ym9vbGVhbnx1bmRlZmluZWR9XHJcbiAgICAgKi9cclxuICAgIHRoaXMuZ3JvdXAgPSB1bmRlZmluZWQ7IC8vIHRvSlNPTlxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQ2FjaGVkIGZpZWxkcyBieSBpZC5cclxuICAgICAqIEB0eXBlIHtPYmplY3QuPG51bWJlcixGaWVsZD58bnVsbH1cclxuICAgICAqIEBwcml2YXRlXHJcbiAgICAgKi9cclxuICAgIHRoaXMuX2ZpZWxkc0J5SWQgPSBudWxsO1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogQ2FjaGVkIGZpZWxkcyBhcyBhbiBhcnJheS5cclxuICAgICAqIEB0eXBlIHtGaWVsZFtdfG51bGx9XHJcbiAgICAgKiBAcHJpdmF0ZVxyXG4gICAgICovXHJcbiAgICB0aGlzLl9maWVsZHNBcnJheSA9IG51bGw7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBDYWNoZWQgb25lb2ZzIGFzIGFuIGFycmF5LlxyXG4gICAgICogQHR5cGUge09uZU9mW118bnVsbH1cclxuICAgICAqIEBwcml2YXRlXHJcbiAgICAgKi9cclxuICAgIHRoaXMuX29uZW9mc0FycmF5ID0gbnVsbDtcclxuXHJcbiAgICAvKipcclxuICAgICAqIENhY2hlZCBjb25zdHJ1Y3Rvci5cclxuICAgICAqIEB0eXBlIHtDb25zdHJ1Y3Rvcjx7fT59XHJcbiAgICAgKiBAcHJpdmF0ZVxyXG4gICAgICovXHJcbiAgICB0aGlzLl9jdG9yID0gbnVsbDtcclxufVxyXG5cclxuT2JqZWN0LmRlZmluZVByb3BlcnRpZXMoVHlwZS5wcm90b3R5cGUsIHtcclxuXHJcbiAgICAvKipcclxuICAgICAqIE1lc3NhZ2UgZmllbGRzIGJ5IGlkLlxyXG4gICAgICogQG5hbWUgVHlwZSNmaWVsZHNCeUlkXHJcbiAgICAgKiBAdHlwZSB7T2JqZWN0LjxudW1iZXIsRmllbGQ+fVxyXG4gICAgICogQHJlYWRvbmx5XHJcbiAgICAgKi9cclxuICAgIGZpZWxkc0J5SWQ6IHtcclxuICAgICAgICBnZXQ6IGZ1bmN0aW9uKCkge1xyXG5cclxuICAgICAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICAgICAgICAgIGlmICh0aGlzLl9maWVsZHNCeUlkKVxyXG4gICAgICAgICAgICAgICAgcmV0dXJuIHRoaXMuX2ZpZWxkc0J5SWQ7XHJcblxyXG4gICAgICAgICAgICB0aGlzLl9maWVsZHNCeUlkID0ge307XHJcbiAgICAgICAgICAgIGZvciAodmFyIG5hbWVzID0gT2JqZWN0LmtleXModGhpcy5maWVsZHMpLCBpID0gMDsgaSA8IG5hbWVzLmxlbmd0aDsgKytpKSB7XHJcbiAgICAgICAgICAgICAgICB2YXIgZmllbGQgPSB0aGlzLmZpZWxkc1tuYW1lc1tpXV0sXHJcbiAgICAgICAgICAgICAgICAgICAgaWQgPSBmaWVsZC5pZDtcclxuXHJcbiAgICAgICAgICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgaWYgKi9cclxuICAgICAgICAgICAgICAgIGlmICh0aGlzLl9maWVsZHNCeUlkW2lkXSlcclxuICAgICAgICAgICAgICAgICAgICB0aHJvdyBFcnJvcihcImR1cGxpY2F0ZSBpZCBcIiArIGlkICsgXCIgaW4gXCIgKyB0aGlzKTtcclxuXHJcbiAgICAgICAgICAgICAgICB0aGlzLl9maWVsZHNCeUlkW2lkXSA9IGZpZWxkO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIHJldHVybiB0aGlzLl9maWVsZHNCeUlkO1xyXG4gICAgICAgIH1cclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBGaWVsZHMgb2YgdGhpcyBtZXNzYWdlIGFzIGFuIGFycmF5IGZvciBpdGVyYXRpb24uXHJcbiAgICAgKiBAbmFtZSBUeXBlI2ZpZWxkc0FycmF5XHJcbiAgICAgKiBAdHlwZSB7RmllbGRbXX1cclxuICAgICAqIEByZWFkb25seVxyXG4gICAgICovXHJcbiAgICBmaWVsZHNBcnJheToge1xyXG4gICAgICAgIGdldDogZnVuY3Rpb24oKSB7XHJcbiAgICAgICAgICAgIHJldHVybiB0aGlzLl9maWVsZHNBcnJheSB8fCAodGhpcy5fZmllbGRzQXJyYXkgPSB1dGlsLnRvQXJyYXkodGhpcy5maWVsZHMpKTtcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogT25lb2ZzIG9mIHRoaXMgbWVzc2FnZSBhcyBhbiBhcnJheSBmb3IgaXRlcmF0aW9uLlxyXG4gICAgICogQG5hbWUgVHlwZSNvbmVvZnNBcnJheVxyXG4gICAgICogQHR5cGUge09uZU9mW119XHJcbiAgICAgKiBAcmVhZG9ubHlcclxuICAgICAqL1xyXG4gICAgb25lb2ZzQXJyYXk6IHtcclxuICAgICAgICBnZXQ6IGZ1bmN0aW9uKCkge1xyXG4gICAgICAgICAgICByZXR1cm4gdGhpcy5fb25lb2ZzQXJyYXkgfHwgKHRoaXMuX29uZW9mc0FycmF5ID0gdXRpbC50b0FycmF5KHRoaXMub25lb2ZzKSk7XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFRoZSByZWdpc3RlcmVkIGNvbnN0cnVjdG9yLCBpZiBhbnkgcmVnaXN0ZXJlZCwgb3RoZXJ3aXNlIGEgZ2VuZXJpYyBjb25zdHJ1Y3Rvci5cclxuICAgICAqIEFzc2lnbmluZyBhIGZ1bmN0aW9uIHJlcGxhY2VzIHRoZSBpbnRlcm5hbCBjb25zdHJ1Y3Rvci4gSWYgdGhlIGZ1bmN0aW9uIGRvZXMgbm90IGV4dGVuZCB7QGxpbmsgTWVzc2FnZX0geWV0LCBpdHMgcHJvdG90eXBlIHdpbGwgYmUgc2V0dXAgYWNjb3JkaW5nbHkgYW5kIHN0YXRpYyBtZXRob2RzIHdpbGwgYmUgcG9wdWxhdGVkLiBJZiBpdCBhbHJlYWR5IGV4dGVuZHMge0BsaW5rIE1lc3NhZ2V9LCBpdCB3aWxsIGp1c3QgcmVwbGFjZSB0aGUgaW50ZXJuYWwgY29uc3RydWN0b3IuXHJcbiAgICAgKiBAbmFtZSBUeXBlI2N0b3JcclxuICAgICAqIEB0eXBlIHtDb25zdHJ1Y3Rvcjx7fT59XHJcbiAgICAgKi9cclxuICAgIGN0b3I6IHtcclxuICAgICAgICBnZXQ6IGZ1bmN0aW9uKCkge1xyXG4gICAgICAgICAgICByZXR1cm4gdGhpcy5fY3RvciB8fCAodGhpcy5jdG9yID0gVHlwZS5nZW5lcmF0ZUNvbnN0cnVjdG9yKHRoaXMpKCkpO1xyXG4gICAgICAgIH0sXHJcbiAgICAgICAgc2V0OiBmdW5jdGlvbihjdG9yKSB7XHJcblxyXG4gICAgICAgICAgICAvLyBFbnN1cmUgcHJvcGVyIHByb3RvdHlwZVxyXG4gICAgICAgICAgICB2YXIgcHJvdG90eXBlID0gY3Rvci5wcm90b3R5cGU7XHJcbiAgICAgICAgICAgIGlmICghKHByb3RvdHlwZSBpbnN0YW5jZW9mIE1lc3NhZ2UpKSB7XHJcbiAgICAgICAgICAgICAgICAoY3Rvci5wcm90b3R5cGUgPSBuZXcgTWVzc2FnZSgpKS5jb25zdHJ1Y3RvciA9IGN0b3I7XHJcbiAgICAgICAgICAgICAgICB1dGlsLm1lcmdlKGN0b3IucHJvdG90eXBlLCBwcm90b3R5cGUpO1xyXG4gICAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgICAvLyBDbGFzc2VzIGFuZCBtZXNzYWdlcyByZWZlcmVuY2UgdGhlaXIgcmVmbGVjdGVkIHR5cGVcclxuICAgICAgICAgICAgY3Rvci4kdHlwZSA9IGN0b3IucHJvdG90eXBlLiR0eXBlID0gdGhpcztcclxuXHJcbiAgICAgICAgICAgIC8vIE1peCBpbiBzdGF0aWMgbWV0aG9kc1xyXG4gICAgICAgICAgICB1dGlsLm1lcmdlKGN0b3IsIE1lc3NhZ2UsIHRydWUpO1xyXG5cclxuICAgICAgICAgICAgdGhpcy5fY3RvciA9IGN0b3I7XHJcblxyXG4gICAgICAgICAgICAvLyBNZXNzYWdlcyBoYXZlIG5vbi1lbnVtZXJhYmxlIGRlZmF1bHQgdmFsdWVzIG9uIHRoZWlyIHByb3RvdHlwZVxyXG4gICAgICAgICAgICB2YXIgaSA9IDA7XHJcbiAgICAgICAgICAgIGZvciAoOyBpIDwgLyogaW5pdGlhbGl6ZXMgKi8gdGhpcy5maWVsZHNBcnJheS5sZW5ndGg7ICsraSlcclxuICAgICAgICAgICAgICAgIHRoaXMuX2ZpZWxkc0FycmF5W2ldLnJlc29sdmUoKTsgLy8gZW5zdXJlcyBhIHByb3BlciB2YWx1ZVxyXG5cclxuICAgICAgICAgICAgLy8gTWVzc2FnZXMgaGF2ZSBub24tZW51bWVyYWJsZSBnZXR0ZXJzIGFuZCBzZXR0ZXJzIGZvciBlYWNoIHZpcnR1YWwgb25lb2YgZmllbGRcclxuICAgICAgICAgICAgdmFyIGN0b3JQcm9wZXJ0aWVzID0ge307XHJcbiAgICAgICAgICAgIGZvciAoaSA9IDA7IGkgPCAvKiBpbml0aWFsaXplcyAqLyB0aGlzLm9uZW9mc0FycmF5Lmxlbmd0aDsgKytpKVxyXG4gICAgICAgICAgICAgICAgY3RvclByb3BlcnRpZXNbdGhpcy5fb25lb2ZzQXJyYXlbaV0ucmVzb2x2ZSgpLm5hbWVdID0ge1xyXG4gICAgICAgICAgICAgICAgICAgIGdldDogdXRpbC5vbmVPZkdldHRlcih0aGlzLl9vbmVvZnNBcnJheVtpXS5vbmVvZiksXHJcbiAgICAgICAgICAgICAgICAgICAgc2V0OiB1dGlsLm9uZU9mU2V0dGVyKHRoaXMuX29uZW9mc0FycmF5W2ldLm9uZW9mKVxyXG4gICAgICAgICAgICAgICAgfTtcclxuICAgICAgICAgICAgaWYgKGkpXHJcbiAgICAgICAgICAgICAgICBPYmplY3QuZGVmaW5lUHJvcGVydGllcyhjdG9yLnByb3RvdHlwZSwgY3RvclByb3BlcnRpZXMpO1xyXG4gICAgICAgIH1cclxuICAgIH1cclxufSk7XHJcblxyXG4vKipcclxuICogR2VuZXJhdGVzIGEgY29uc3RydWN0b3IgZnVuY3Rpb24gZm9yIHRoZSBzcGVjaWZpZWQgdHlwZS5cclxuICogQHBhcmFtIHtUeXBlfSBtdHlwZSBNZXNzYWdlIHR5cGVcclxuICogQHJldHVybnMge0NvZGVnZW59IENvZGVnZW4gaW5zdGFuY2VcclxuICovXHJcblR5cGUuZ2VuZXJhdGVDb25zdHJ1Y3RvciA9IGZ1bmN0aW9uIGdlbmVyYXRlQ29uc3RydWN0b3IobXR5cGUpIHtcclxuICAgIC8qIGVzbGludC1kaXNhYmxlIG5vLXVuZXhwZWN0ZWQtbXVsdGlsaW5lICovXHJcbiAgICB2YXIgZ2VuID0gdXRpbC5jb2RlZ2VuKFtcInBcIl0sIG10eXBlLm5hbWUpO1xyXG4gICAgLy8gZXhwbGljaXRseSBpbml0aWFsaXplIG11dGFibGUgb2JqZWN0L2FycmF5IGZpZWxkcyBzbyB0aGF0IHRoZXNlIGFyZW4ndCBqdXN0IGluaGVyaXRlZCBmcm9tIHRoZSBwcm90b3R5cGVcclxuICAgIGZvciAodmFyIGkgPSAwLCBmaWVsZDsgaSA8IG10eXBlLmZpZWxkc0FycmF5Lmxlbmd0aDsgKytpKVxyXG4gICAgICAgIGlmICgoZmllbGQgPSBtdHlwZS5fZmllbGRzQXJyYXlbaV0pLm1hcCkgZ2VuXHJcbiAgICAgICAgICAgIChcInRoaXMlcz17fVwiLCB1dGlsLnNhZmVQcm9wKGZpZWxkLm5hbWUpKTtcclxuICAgICAgICBlbHNlIGlmIChmaWVsZC5yZXBlYXRlZCkgZ2VuXHJcbiAgICAgICAgICAgIChcInRoaXMlcz1bXVwiLCB1dGlsLnNhZmVQcm9wKGZpZWxkLm5hbWUpKTtcclxuICAgIHJldHVybiBnZW5cclxuICAgIChcImlmKHApZm9yKHZhciBrcz1PYmplY3Qua2V5cyhwKSxpPTA7aTxrcy5sZW5ndGg7KytpKWlmKHBba3NbaV1dIT1udWxsKVwiKSAvLyBvbWl0IHVuZGVmaW5lZCBvciBudWxsXHJcbiAgICAgICAgKFwidGhpc1trc1tpXV09cFtrc1tpXV1cIik7XHJcbiAgICAvKiBlc2xpbnQtZW5hYmxlIG5vLXVuZXhwZWN0ZWQtbXVsdGlsaW5lICovXHJcbn07XHJcblxyXG5mdW5jdGlvbiBjbGVhckNhY2hlKHR5cGUpIHtcclxuICAgIHR5cGUuX2ZpZWxkc0J5SWQgPSB0eXBlLl9maWVsZHNBcnJheSA9IHR5cGUuX29uZW9mc0FycmF5ID0gbnVsbDtcclxuICAgIGRlbGV0ZSB0eXBlLmVuY29kZTtcclxuICAgIGRlbGV0ZSB0eXBlLmRlY29kZTtcclxuICAgIGRlbGV0ZSB0eXBlLnZlcmlmeTtcclxuICAgIHJldHVybiB0eXBlO1xyXG59XHJcblxyXG4vKipcclxuICogTWVzc2FnZSB0eXBlIGRlc2NyaXB0b3IuXHJcbiAqIEBpbnRlcmZhY2UgSVR5cGVcclxuICogQGV4dGVuZHMgSU5hbWVzcGFjZVxyXG4gKiBAcHJvcGVydHkge09iamVjdC48c3RyaW5nLElPbmVPZj59IFtvbmVvZnNdIE9uZW9mIGRlc2NyaXB0b3JzXHJcbiAqIEBwcm9wZXJ0eSB7T2JqZWN0LjxzdHJpbmcsSUZpZWxkPn0gZmllbGRzIEZpZWxkIGRlc2NyaXB0b3JzXHJcbiAqIEBwcm9wZXJ0eSB7bnVtYmVyW11bXX0gW2V4dGVuc2lvbnNdIEV4dGVuc2lvbiByYW5nZXNcclxuICogQHByb3BlcnR5IHtBcnJheS48bnVtYmVyW118c3RyaW5nPn0gW3Jlc2VydmVkXSBSZXNlcnZlZCByYW5nZXNcclxuICogQHByb3BlcnR5IHtib29sZWFufSBbZ3JvdXA9ZmFsc2VdIFdoZXRoZXIgYSBsZWdhY3kgZ3JvdXAgb3Igbm90XHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIENyZWF0ZXMgYSBtZXNzYWdlIHR5cGUgZnJvbSBhIG1lc3NhZ2UgdHlwZSBkZXNjcmlwdG9yLlxyXG4gKiBAcGFyYW0ge3N0cmluZ30gbmFtZSBNZXNzYWdlIG5hbWVcclxuICogQHBhcmFtIHtJVHlwZX0ganNvbiBNZXNzYWdlIHR5cGUgZGVzY3JpcHRvclxyXG4gKiBAcmV0dXJucyB7VHlwZX0gQ3JlYXRlZCBtZXNzYWdlIHR5cGVcclxuICovXHJcblR5cGUuZnJvbUpTT04gPSBmdW5jdGlvbiBmcm9tSlNPTihuYW1lLCBqc29uKSB7XHJcbiAgICB2YXIgdHlwZSA9IG5ldyBUeXBlKG5hbWUsIGpzb24ub3B0aW9ucyk7XHJcbiAgICB0eXBlLmV4dGVuc2lvbnMgPSBqc29uLmV4dGVuc2lvbnM7XHJcbiAgICB0eXBlLnJlc2VydmVkID0ganNvbi5yZXNlcnZlZDtcclxuICAgIHZhciBuYW1lcyA9IE9iamVjdC5rZXlzKGpzb24uZmllbGRzKSxcclxuICAgICAgICBpID0gMDtcclxuICAgIGZvciAoOyBpIDwgbmFtZXMubGVuZ3RoOyArK2kpXHJcbiAgICAgICAgdHlwZS5hZGQoXHJcbiAgICAgICAgICAgICggdHlwZW9mIGpzb24uZmllbGRzW25hbWVzW2ldXS5rZXlUeXBlICE9PSBcInVuZGVmaW5lZFwiXHJcbiAgICAgICAgICAgID8gTWFwRmllbGQuZnJvbUpTT05cclxuICAgICAgICAgICAgOiBGaWVsZC5mcm9tSlNPTiApKG5hbWVzW2ldLCBqc29uLmZpZWxkc1tuYW1lc1tpXV0pXHJcbiAgICAgICAgKTtcclxuICAgIGlmIChqc29uLm9uZW9mcylcclxuICAgICAgICBmb3IgKG5hbWVzID0gT2JqZWN0LmtleXMoanNvbi5vbmVvZnMpLCBpID0gMDsgaSA8IG5hbWVzLmxlbmd0aDsgKytpKVxyXG4gICAgICAgICAgICB0eXBlLmFkZChPbmVPZi5mcm9tSlNPTihuYW1lc1tpXSwganNvbi5vbmVvZnNbbmFtZXNbaV1dKSk7XHJcbiAgICBpZiAoanNvbi5uZXN0ZWQpXHJcbiAgICAgICAgZm9yIChuYW1lcyA9IE9iamVjdC5rZXlzKGpzb24ubmVzdGVkKSwgaSA9IDA7IGkgPCBuYW1lcy5sZW5ndGg7ICsraSkge1xyXG4gICAgICAgICAgICB2YXIgbmVzdGVkID0ganNvbi5uZXN0ZWRbbmFtZXNbaV1dO1xyXG4gICAgICAgICAgICB0eXBlLmFkZCggLy8gbW9zdCB0byBsZWFzdCBsaWtlbHlcclxuICAgICAgICAgICAgICAgICggbmVzdGVkLmlkICE9PSB1bmRlZmluZWRcclxuICAgICAgICAgICAgICAgID8gRmllbGQuZnJvbUpTT05cclxuICAgICAgICAgICAgICAgIDogbmVzdGVkLmZpZWxkcyAhPT0gdW5kZWZpbmVkXHJcbiAgICAgICAgICAgICAgICA/IFR5cGUuZnJvbUpTT05cclxuICAgICAgICAgICAgICAgIDogbmVzdGVkLnZhbHVlcyAhPT0gdW5kZWZpbmVkXHJcbiAgICAgICAgICAgICAgICA/IEVudW0uZnJvbUpTT05cclxuICAgICAgICAgICAgICAgIDogbmVzdGVkLm1ldGhvZHMgIT09IHVuZGVmaW5lZFxyXG4gICAgICAgICAgICAgICAgPyBTZXJ2aWNlLmZyb21KU09OXHJcbiAgICAgICAgICAgICAgICA6IE5hbWVzcGFjZS5mcm9tSlNPTiApKG5hbWVzW2ldLCBuZXN0ZWQpXHJcbiAgICAgICAgICAgICk7XHJcbiAgICAgICAgfVxyXG4gICAgaWYgKGpzb24uZXh0ZW5zaW9ucyAmJiBqc29uLmV4dGVuc2lvbnMubGVuZ3RoKVxyXG4gICAgICAgIHR5cGUuZXh0ZW5zaW9ucyA9IGpzb24uZXh0ZW5zaW9ucztcclxuICAgIGlmIChqc29uLnJlc2VydmVkICYmIGpzb24ucmVzZXJ2ZWQubGVuZ3RoKVxyXG4gICAgICAgIHR5cGUucmVzZXJ2ZWQgPSBqc29uLnJlc2VydmVkO1xyXG4gICAgaWYgKGpzb24uZ3JvdXApXHJcbiAgICAgICAgdHlwZS5ncm91cCA9IHRydWU7XHJcbiAgICBpZiAoanNvbi5jb21tZW50KVxyXG4gICAgICAgIHR5cGUuY29tbWVudCA9IGpzb24uY29tbWVudDtcclxuICAgIHJldHVybiB0eXBlO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIENvbnZlcnRzIHRoaXMgbWVzc2FnZSB0eXBlIHRvIGEgbWVzc2FnZSB0eXBlIGRlc2NyaXB0b3IuXHJcbiAqIEBwYXJhbSB7SVRvSlNPTk9wdGlvbnN9IFt0b0pTT05PcHRpb25zXSBKU09OIGNvbnZlcnNpb24gb3B0aW9uc1xyXG4gKiBAcmV0dXJucyB7SVR5cGV9IE1lc3NhZ2UgdHlwZSBkZXNjcmlwdG9yXHJcbiAqL1xyXG5UeXBlLnByb3RvdHlwZS50b0pTT04gPSBmdW5jdGlvbiB0b0pTT04odG9KU09OT3B0aW9ucykge1xyXG4gICAgdmFyIGluaGVyaXRlZCA9IE5hbWVzcGFjZS5wcm90b3R5cGUudG9KU09OLmNhbGwodGhpcywgdG9KU09OT3B0aW9ucyk7XHJcbiAgICB2YXIga2VlcENvbW1lbnRzID0gdG9KU09OT3B0aW9ucyA/IEJvb2xlYW4odG9KU09OT3B0aW9ucy5rZWVwQ29tbWVudHMpIDogZmFsc2U7XHJcbiAgICByZXR1cm4gdXRpbC50b09iamVjdChbXHJcbiAgICAgICAgXCJvcHRpb25zXCIgICAgLCBpbmhlcml0ZWQgJiYgaW5oZXJpdGVkLm9wdGlvbnMgfHwgdW5kZWZpbmVkLFxyXG4gICAgICAgIFwib25lb2ZzXCIgICAgICwgTmFtZXNwYWNlLmFycmF5VG9KU09OKHRoaXMub25lb2ZzQXJyYXksIHRvSlNPTk9wdGlvbnMpLFxyXG4gICAgICAgIFwiZmllbGRzXCIgICAgICwgTmFtZXNwYWNlLmFycmF5VG9KU09OKHRoaXMuZmllbGRzQXJyYXkuZmlsdGVyKGZ1bmN0aW9uKG9iaikgeyByZXR1cm4gIW9iai5kZWNsYXJpbmdGaWVsZDsgfSksIHRvSlNPTk9wdGlvbnMpIHx8IHt9LFxyXG4gICAgICAgIFwiZXh0ZW5zaW9uc1wiICwgdGhpcy5leHRlbnNpb25zICYmIHRoaXMuZXh0ZW5zaW9ucy5sZW5ndGggPyB0aGlzLmV4dGVuc2lvbnMgOiB1bmRlZmluZWQsXHJcbiAgICAgICAgXCJyZXNlcnZlZFwiICAgLCB0aGlzLnJlc2VydmVkICYmIHRoaXMucmVzZXJ2ZWQubGVuZ3RoID8gdGhpcy5yZXNlcnZlZCA6IHVuZGVmaW5lZCxcclxuICAgICAgICBcImdyb3VwXCIgICAgICAsIHRoaXMuZ3JvdXAgfHwgdW5kZWZpbmVkLFxyXG4gICAgICAgIFwibmVzdGVkXCIgICAgICwgaW5oZXJpdGVkICYmIGluaGVyaXRlZC5uZXN0ZWQgfHwgdW5kZWZpbmVkLFxyXG4gICAgICAgIFwiY29tbWVudFwiICAgICwga2VlcENvbW1lbnRzID8gdGhpcy5jb21tZW50IDogdW5kZWZpbmVkXHJcbiAgICBdKTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBAb3ZlcnJpZGVcclxuICovXHJcblR5cGUucHJvdG90eXBlLnJlc29sdmVBbGwgPSBmdW5jdGlvbiByZXNvbHZlQWxsKCkge1xyXG4gICAgdmFyIGZpZWxkcyA9IHRoaXMuZmllbGRzQXJyYXksIGkgPSAwO1xyXG4gICAgd2hpbGUgKGkgPCBmaWVsZHMubGVuZ3RoKVxyXG4gICAgICAgIGZpZWxkc1tpKytdLnJlc29sdmUoKTtcclxuICAgIHZhciBvbmVvZnMgPSB0aGlzLm9uZW9mc0FycmF5OyBpID0gMDtcclxuICAgIHdoaWxlIChpIDwgb25lb2ZzLmxlbmd0aClcclxuICAgICAgICBvbmVvZnNbaSsrXS5yZXNvbHZlKCk7XHJcbiAgICByZXR1cm4gTmFtZXNwYWNlLnByb3RvdHlwZS5yZXNvbHZlQWxsLmNhbGwodGhpcyk7XHJcbn07XHJcblxyXG4vKipcclxuICogQG92ZXJyaWRlXHJcbiAqL1xyXG5UeXBlLnByb3RvdHlwZS5nZXQgPSBmdW5jdGlvbiBnZXQobmFtZSkge1xyXG4gICAgcmV0dXJuIHRoaXMuZmllbGRzW25hbWVdXHJcbiAgICAgICAgfHwgdGhpcy5vbmVvZnMgJiYgdGhpcy5vbmVvZnNbbmFtZV1cclxuICAgICAgICB8fCB0aGlzLm5lc3RlZCAmJiB0aGlzLm5lc3RlZFtuYW1lXVxyXG4gICAgICAgIHx8IG51bGw7XHJcbn07XHJcblxyXG4vKipcclxuICogQWRkcyBhIG5lc3RlZCBvYmplY3QgdG8gdGhpcyB0eXBlLlxyXG4gKiBAcGFyYW0ge1JlZmxlY3Rpb25PYmplY3R9IG9iamVjdCBOZXN0ZWQgb2JqZWN0IHRvIGFkZFxyXG4gKiBAcmV0dXJucyB7VHlwZX0gYHRoaXNgXHJcbiAqIEB0aHJvd3Mge1R5cGVFcnJvcn0gSWYgYXJndW1lbnRzIGFyZSBpbnZhbGlkXHJcbiAqIEB0aHJvd3Mge0Vycm9yfSBJZiB0aGVyZSBpcyBhbHJlYWR5IGEgbmVzdGVkIG9iamVjdCB3aXRoIHRoaXMgbmFtZSBvciwgaWYgYSBmaWVsZCwgd2hlbiB0aGVyZSBpcyBhbHJlYWR5IGEgZmllbGQgd2l0aCB0aGlzIGlkXHJcbiAqL1xyXG5UeXBlLnByb3RvdHlwZS5hZGQgPSBmdW5jdGlvbiBhZGQob2JqZWN0KSB7XHJcblxyXG4gICAgaWYgKHRoaXMuZ2V0KG9iamVjdC5uYW1lKSlcclxuICAgICAgICB0aHJvdyBFcnJvcihcImR1cGxpY2F0ZSBuYW1lICdcIiArIG9iamVjdC5uYW1lICsgXCInIGluIFwiICsgdGhpcyk7XHJcblxyXG4gICAgaWYgKG9iamVjdCBpbnN0YW5jZW9mIEZpZWxkICYmIG9iamVjdC5leHRlbmQgPT09IHVuZGVmaW5lZCkge1xyXG4gICAgICAgIC8vIE5PVEU6IEV4dGVuc2lvbiBmaWVsZHMgYXJlbid0IGFjdHVhbCBmaWVsZHMgb24gdGhlIGRlY2xhcmluZyB0eXBlLCBidXQgbmVzdGVkIG9iamVjdHMuXHJcbiAgICAgICAgLy8gVGhlIHJvb3Qgb2JqZWN0IHRha2VzIGNhcmUgb2YgYWRkaW5nIGRpc3RpbmN0IHNpc3Rlci1maWVsZHMgdG8gdGhlIHJlc3BlY3RpdmUgZXh0ZW5kZWRcclxuICAgICAgICAvLyB0eXBlIGluc3RlYWQuXHJcblxyXG4gICAgICAgIC8vIGF2b2lkcyBjYWxsaW5nIHRoZSBnZXR0ZXIgaWYgbm90IGFic29sdXRlbHkgbmVjZXNzYXJ5IGJlY2F1c2UgaXQncyBjYWxsZWQgcXVpdGUgZnJlcXVlbnRseVxyXG4gICAgICAgIGlmICh0aGlzLl9maWVsZHNCeUlkID8gLyogaXN0YW5idWwgaWdub3JlIG5leHQgKi8gdGhpcy5fZmllbGRzQnlJZFtvYmplY3QuaWRdIDogdGhpcy5maWVsZHNCeUlkW29iamVjdC5pZF0pXHJcbiAgICAgICAgICAgIHRocm93IEVycm9yKFwiZHVwbGljYXRlIGlkIFwiICsgb2JqZWN0LmlkICsgXCIgaW4gXCIgKyB0aGlzKTtcclxuICAgICAgICBpZiAodGhpcy5pc1Jlc2VydmVkSWQob2JqZWN0LmlkKSlcclxuICAgICAgICAgICAgdGhyb3cgRXJyb3IoXCJpZCBcIiArIG9iamVjdC5pZCArIFwiIGlzIHJlc2VydmVkIGluIFwiICsgdGhpcyk7XHJcbiAgICAgICAgaWYgKHRoaXMuaXNSZXNlcnZlZE5hbWUob2JqZWN0Lm5hbWUpKVxyXG4gICAgICAgICAgICB0aHJvdyBFcnJvcihcIm5hbWUgJ1wiICsgb2JqZWN0Lm5hbWUgKyBcIicgaXMgcmVzZXJ2ZWQgaW4gXCIgKyB0aGlzKTtcclxuXHJcbiAgICAgICAgaWYgKG9iamVjdC5wYXJlbnQpXHJcbiAgICAgICAgICAgIG9iamVjdC5wYXJlbnQucmVtb3ZlKG9iamVjdCk7XHJcbiAgICAgICAgdGhpcy5maWVsZHNbb2JqZWN0Lm5hbWVdID0gb2JqZWN0O1xyXG4gICAgICAgIG9iamVjdC5tZXNzYWdlID0gdGhpcztcclxuICAgICAgICBvYmplY3Qub25BZGQodGhpcyk7XHJcbiAgICAgICAgcmV0dXJuIGNsZWFyQ2FjaGUodGhpcyk7XHJcbiAgICB9XHJcbiAgICBpZiAob2JqZWN0IGluc3RhbmNlb2YgT25lT2YpIHtcclxuICAgICAgICBpZiAoIXRoaXMub25lb2ZzKVxyXG4gICAgICAgICAgICB0aGlzLm9uZW9mcyA9IHt9O1xyXG4gICAgICAgIHRoaXMub25lb2ZzW29iamVjdC5uYW1lXSA9IG9iamVjdDtcclxuICAgICAgICBvYmplY3Qub25BZGQodGhpcyk7XHJcbiAgICAgICAgcmV0dXJuIGNsZWFyQ2FjaGUodGhpcyk7XHJcbiAgICB9XHJcbiAgICByZXR1cm4gTmFtZXNwYWNlLnByb3RvdHlwZS5hZGQuY2FsbCh0aGlzLCBvYmplY3QpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIFJlbW92ZXMgYSBuZXN0ZWQgb2JqZWN0IGZyb20gdGhpcyB0eXBlLlxyXG4gKiBAcGFyYW0ge1JlZmxlY3Rpb25PYmplY3R9IG9iamVjdCBOZXN0ZWQgb2JqZWN0IHRvIHJlbW92ZVxyXG4gKiBAcmV0dXJucyB7VHlwZX0gYHRoaXNgXHJcbiAqIEB0aHJvd3Mge1R5cGVFcnJvcn0gSWYgYXJndW1lbnRzIGFyZSBpbnZhbGlkXHJcbiAqIEB0aHJvd3Mge0Vycm9yfSBJZiBgb2JqZWN0YCBpcyBub3QgYSBtZW1iZXIgb2YgdGhpcyB0eXBlXHJcbiAqL1xyXG5UeXBlLnByb3RvdHlwZS5yZW1vdmUgPSBmdW5jdGlvbiByZW1vdmUob2JqZWN0KSB7XHJcbiAgICBpZiAob2JqZWN0IGluc3RhbmNlb2YgRmllbGQgJiYgb2JqZWN0LmV4dGVuZCA9PT0gdW5kZWZpbmVkKSB7XHJcbiAgICAgICAgLy8gU2VlIFR5cGUjYWRkIGZvciB0aGUgcmVhc29uIHdoeSBleHRlbnNpb24gZmllbGRzIGFyZSBleGNsdWRlZCBoZXJlLlxyXG5cclxuICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgaWYgKi9cclxuICAgICAgICBpZiAoIXRoaXMuZmllbGRzIHx8IHRoaXMuZmllbGRzW29iamVjdC5uYW1lXSAhPT0gb2JqZWN0KVxyXG4gICAgICAgICAgICB0aHJvdyBFcnJvcihvYmplY3QgKyBcIiBpcyBub3QgYSBtZW1iZXIgb2YgXCIgKyB0aGlzKTtcclxuXHJcbiAgICAgICAgZGVsZXRlIHRoaXMuZmllbGRzW29iamVjdC5uYW1lXTtcclxuICAgICAgICBvYmplY3QucGFyZW50ID0gbnVsbDtcclxuICAgICAgICBvYmplY3Qub25SZW1vdmUodGhpcyk7XHJcbiAgICAgICAgcmV0dXJuIGNsZWFyQ2FjaGUodGhpcyk7XHJcbiAgICB9XHJcbiAgICBpZiAob2JqZWN0IGluc3RhbmNlb2YgT25lT2YpIHtcclxuXHJcbiAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICAgICAgaWYgKCF0aGlzLm9uZW9mcyB8fCB0aGlzLm9uZW9mc1tvYmplY3QubmFtZV0gIT09IG9iamVjdClcclxuICAgICAgICAgICAgdGhyb3cgRXJyb3Iob2JqZWN0ICsgXCIgaXMgbm90IGEgbWVtYmVyIG9mIFwiICsgdGhpcyk7XHJcblxyXG4gICAgICAgIGRlbGV0ZSB0aGlzLm9uZW9mc1tvYmplY3QubmFtZV07XHJcbiAgICAgICAgb2JqZWN0LnBhcmVudCA9IG51bGw7XHJcbiAgICAgICAgb2JqZWN0Lm9uUmVtb3ZlKHRoaXMpO1xyXG4gICAgICAgIHJldHVybiBjbGVhckNhY2hlKHRoaXMpO1xyXG4gICAgfVxyXG4gICAgcmV0dXJuIE5hbWVzcGFjZS5wcm90b3R5cGUucmVtb3ZlLmNhbGwodGhpcywgb2JqZWN0KTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBUZXN0cyBpZiB0aGUgc3BlY2lmaWVkIGlkIGlzIHJlc2VydmVkLlxyXG4gKiBAcGFyYW0ge251bWJlcn0gaWQgSWQgdG8gdGVzdFxyXG4gKiBAcmV0dXJucyB7Ym9vbGVhbn0gYHRydWVgIGlmIHJlc2VydmVkLCBvdGhlcndpc2UgYGZhbHNlYFxyXG4gKi9cclxuVHlwZS5wcm90b3R5cGUuaXNSZXNlcnZlZElkID0gZnVuY3Rpb24gaXNSZXNlcnZlZElkKGlkKSB7XHJcbiAgICByZXR1cm4gTmFtZXNwYWNlLmlzUmVzZXJ2ZWRJZCh0aGlzLnJlc2VydmVkLCBpZCk7XHJcbn07XHJcblxyXG4vKipcclxuICogVGVzdHMgaWYgdGhlIHNwZWNpZmllZCBuYW1lIGlzIHJlc2VydmVkLlxyXG4gKiBAcGFyYW0ge3N0cmluZ30gbmFtZSBOYW1lIHRvIHRlc3RcclxuICogQHJldHVybnMge2Jvb2xlYW59IGB0cnVlYCBpZiByZXNlcnZlZCwgb3RoZXJ3aXNlIGBmYWxzZWBcclxuICovXHJcblR5cGUucHJvdG90eXBlLmlzUmVzZXJ2ZWROYW1lID0gZnVuY3Rpb24gaXNSZXNlcnZlZE5hbWUobmFtZSkge1xyXG4gICAgcmV0dXJuIE5hbWVzcGFjZS5pc1Jlc2VydmVkTmFtZSh0aGlzLnJlc2VydmVkLCBuYW1lKTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBDcmVhdGVzIGEgbmV3IG1lc3NhZ2Ugb2YgdGhpcyB0eXBlIHVzaW5nIHRoZSBzcGVjaWZpZWQgcHJvcGVydGllcy5cclxuICogQHBhcmFtIHtPYmplY3QuPHN0cmluZywqPn0gW3Byb3BlcnRpZXNdIFByb3BlcnRpZXMgdG8gc2V0XHJcbiAqIEByZXR1cm5zIHtNZXNzYWdlPHt9Pn0gTWVzc2FnZSBpbnN0YW5jZVxyXG4gKi9cclxuVHlwZS5wcm90b3R5cGUuY3JlYXRlID0gZnVuY3Rpb24gY3JlYXRlKHByb3BlcnRpZXMpIHtcclxuICAgIHJldHVybiBuZXcgdGhpcy5jdG9yKHByb3BlcnRpZXMpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIFNldHMgdXAge0BsaW5rIFR5cGUjZW5jb2RlfGVuY29kZX0sIHtAbGluayBUeXBlI2RlY29kZXxkZWNvZGV9IGFuZCB7QGxpbmsgVHlwZSN2ZXJpZnl8dmVyaWZ5fS5cclxuICogQHJldHVybnMge1R5cGV9IGB0aGlzYFxyXG4gKi9cclxuVHlwZS5wcm90b3R5cGUuc2V0dXAgPSBmdW5jdGlvbiBzZXR1cCgpIHtcclxuICAgIC8vIFNldHMgdXAgZXZlcnl0aGluZyBhdCBvbmNlIHNvIHRoYXQgdGhlIHByb3RvdHlwZSBjaGFpbiBkb2VzIG5vdCBoYXZlIHRvIGJlIHJlLWV2YWx1YXRlZFxyXG4gICAgLy8gbXVsdGlwbGUgdGltZXMgKFY4LCBzb2Z0LWRlb3B0IHByb3RvdHlwZS1jaGVjaykuXHJcblxyXG4gICAgdmFyIGZ1bGxOYW1lID0gdGhpcy5mdWxsTmFtZSxcclxuICAgICAgICB0eXBlcyAgICA9IFtdO1xyXG4gICAgZm9yICh2YXIgaSA9IDA7IGkgPCAvKiBpbml0aWFsaXplcyAqLyB0aGlzLmZpZWxkc0FycmF5Lmxlbmd0aDsgKytpKVxyXG4gICAgICAgIHR5cGVzLnB1c2godGhpcy5fZmllbGRzQXJyYXlbaV0ucmVzb2x2ZSgpLnJlc29sdmVkVHlwZSk7XHJcblxyXG4gICAgLy8gUmVwbGFjZSBzZXR1cCBtZXRob2RzIHdpdGggdHlwZS1zcGVjaWZpYyBnZW5lcmF0ZWQgZnVuY3Rpb25zXHJcbiAgICB0aGlzLmVuY29kZSA9IGVuY29kZXIodGhpcykoe1xyXG4gICAgICAgIFdyaXRlciA6IFdyaXRlcixcclxuICAgICAgICB0eXBlcyAgOiB0eXBlcyxcclxuICAgICAgICB1dGlsICAgOiB1dGlsXHJcbiAgICB9KTtcclxuICAgIHRoaXMuZGVjb2RlID0gZGVjb2Rlcih0aGlzKSh7XHJcbiAgICAgICAgUmVhZGVyIDogUmVhZGVyLFxyXG4gICAgICAgIHR5cGVzICA6IHR5cGVzLFxyXG4gICAgICAgIHV0aWwgICA6IHV0aWxcclxuICAgIH0pO1xyXG4gICAgdGhpcy52ZXJpZnkgPSB2ZXJpZmllcih0aGlzKSh7XHJcbiAgICAgICAgdHlwZXMgOiB0eXBlcyxcclxuICAgICAgICB1dGlsICA6IHV0aWxcclxuICAgIH0pO1xyXG4gICAgdGhpcy5mcm9tT2JqZWN0ID0gY29udmVydGVyLmZyb21PYmplY3QodGhpcykoe1xyXG4gICAgICAgIHR5cGVzIDogdHlwZXMsXHJcbiAgICAgICAgdXRpbCAgOiB1dGlsXHJcbiAgICB9KTtcclxuICAgIHRoaXMudG9PYmplY3QgPSBjb252ZXJ0ZXIudG9PYmplY3QodGhpcykoe1xyXG4gICAgICAgIHR5cGVzIDogdHlwZXMsXHJcbiAgICAgICAgdXRpbCAgOiB1dGlsXHJcbiAgICB9KTtcclxuXHJcbiAgICAvLyBJbmplY3QgY3VzdG9tIHdyYXBwZXJzIGZvciBjb21tb24gdHlwZXNcclxuICAgIHZhciB3cmFwcGVyID0gd3JhcHBlcnNbZnVsbE5hbWVdO1xyXG4gICAgaWYgKHdyYXBwZXIpIHtcclxuICAgICAgICB2YXIgb3JpZ2luYWxUaGlzID0gT2JqZWN0LmNyZWF0ZSh0aGlzKTtcclxuICAgICAgICAvLyBpZiAod3JhcHBlci5mcm9tT2JqZWN0KSB7XHJcbiAgICAgICAgICAgIG9yaWdpbmFsVGhpcy5mcm9tT2JqZWN0ID0gdGhpcy5mcm9tT2JqZWN0O1xyXG4gICAgICAgICAgICB0aGlzLmZyb21PYmplY3QgPSB3cmFwcGVyLmZyb21PYmplY3QuYmluZChvcmlnaW5hbFRoaXMpO1xyXG4gICAgICAgIC8vIH1cclxuICAgICAgICAvLyBpZiAod3JhcHBlci50b09iamVjdCkge1xyXG4gICAgICAgICAgICBvcmlnaW5hbFRoaXMudG9PYmplY3QgPSB0aGlzLnRvT2JqZWN0O1xyXG4gICAgICAgICAgICB0aGlzLnRvT2JqZWN0ID0gd3JhcHBlci50b09iamVjdC5iaW5kKG9yaWdpbmFsVGhpcyk7XHJcbiAgICAgICAgLy8gfVxyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiB0aGlzO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIEVuY29kZXMgYSBtZXNzYWdlIG9mIHRoaXMgdHlwZS4gRG9lcyBub3QgaW1wbGljaXRseSB7QGxpbmsgVHlwZSN2ZXJpZnl8dmVyaWZ5fSBtZXNzYWdlcy5cclxuICogQHBhcmFtIHtNZXNzYWdlPHt9PnxPYmplY3QuPHN0cmluZywqPn0gbWVzc2FnZSBNZXNzYWdlIGluc3RhbmNlIG9yIHBsYWluIG9iamVjdFxyXG4gKiBAcGFyYW0ge1dyaXRlcn0gW3dyaXRlcl0gV3JpdGVyIHRvIGVuY29kZSB0b1xyXG4gKiBAcmV0dXJucyB7V3JpdGVyfSB3cml0ZXJcclxuICovXHJcblR5cGUucHJvdG90eXBlLmVuY29kZSA9IGZ1bmN0aW9uIGVuY29kZV9zZXR1cChtZXNzYWdlLCB3cml0ZXIpIHtcclxuICAgIHJldHVybiB0aGlzLnNldHVwKCkuZW5jb2RlKG1lc3NhZ2UsIHdyaXRlcik7IC8vIG92ZXJyaWRlcyB0aGlzIG1ldGhvZFxyXG59O1xyXG5cclxuLyoqXHJcbiAqIEVuY29kZXMgYSBtZXNzYWdlIG9mIHRoaXMgdHlwZSBwcmVjZWVkZWQgYnkgaXRzIGJ5dGUgbGVuZ3RoIGFzIGEgdmFyaW50LiBEb2VzIG5vdCBpbXBsaWNpdGx5IHtAbGluayBUeXBlI3ZlcmlmeXx2ZXJpZnl9IG1lc3NhZ2VzLlxyXG4gKiBAcGFyYW0ge01lc3NhZ2U8e30+fE9iamVjdC48c3RyaW5nLCo+fSBtZXNzYWdlIE1lc3NhZ2UgaW5zdGFuY2Ugb3IgcGxhaW4gb2JqZWN0XHJcbiAqIEBwYXJhbSB7V3JpdGVyfSBbd3JpdGVyXSBXcml0ZXIgdG8gZW5jb2RlIHRvXHJcbiAqIEByZXR1cm5zIHtXcml0ZXJ9IHdyaXRlclxyXG4gKi9cclxuVHlwZS5wcm90b3R5cGUuZW5jb2RlRGVsaW1pdGVkID0gZnVuY3Rpb24gZW5jb2RlRGVsaW1pdGVkKG1lc3NhZ2UsIHdyaXRlcikge1xyXG4gICAgcmV0dXJuIHRoaXMuZW5jb2RlKG1lc3NhZ2UsIHdyaXRlciAmJiB3cml0ZXIubGVuID8gd3JpdGVyLmZvcmsoKSA6IHdyaXRlcikubGRlbGltKCk7XHJcbn07XHJcblxyXG4vKipcclxuICogRGVjb2RlcyBhIG1lc3NhZ2Ugb2YgdGhpcyB0eXBlLlxyXG4gKiBAcGFyYW0ge1JlYWRlcnxVaW50OEFycmF5fSByZWFkZXIgUmVhZGVyIG9yIGJ1ZmZlciB0byBkZWNvZGUgZnJvbVxyXG4gKiBAcGFyYW0ge251bWJlcn0gW2xlbmd0aF0gTGVuZ3RoIG9mIHRoZSBtZXNzYWdlLCBpZiBrbm93biBiZWZvcmVoYW5kXHJcbiAqIEByZXR1cm5zIHtNZXNzYWdlPHt9Pn0gRGVjb2RlZCBtZXNzYWdlXHJcbiAqIEB0aHJvd3Mge0Vycm9yfSBJZiB0aGUgcGF5bG9hZCBpcyBub3QgYSByZWFkZXIgb3IgdmFsaWQgYnVmZmVyXHJcbiAqIEB0aHJvd3Mge3V0aWwuUHJvdG9jb2xFcnJvcjx7fT59IElmIHJlcXVpcmVkIGZpZWxkcyBhcmUgbWlzc2luZ1xyXG4gKi9cclxuVHlwZS5wcm90b3R5cGUuZGVjb2RlID0gZnVuY3Rpb24gZGVjb2RlX3NldHVwKHJlYWRlciwgbGVuZ3RoKSB7XHJcbiAgICByZXR1cm4gdGhpcy5zZXR1cCgpLmRlY29kZShyZWFkZXIsIGxlbmd0aCk7IC8vIG92ZXJyaWRlcyB0aGlzIG1ldGhvZFxyXG59O1xyXG5cclxuLyoqXHJcbiAqIERlY29kZXMgYSBtZXNzYWdlIG9mIHRoaXMgdHlwZSBwcmVjZWVkZWQgYnkgaXRzIGJ5dGUgbGVuZ3RoIGFzIGEgdmFyaW50LlxyXG4gKiBAcGFyYW0ge1JlYWRlcnxVaW50OEFycmF5fSByZWFkZXIgUmVhZGVyIG9yIGJ1ZmZlciB0byBkZWNvZGUgZnJvbVxyXG4gKiBAcmV0dXJucyB7TWVzc2FnZTx7fT59IERlY29kZWQgbWVzc2FnZVxyXG4gKiBAdGhyb3dzIHtFcnJvcn0gSWYgdGhlIHBheWxvYWQgaXMgbm90IGEgcmVhZGVyIG9yIHZhbGlkIGJ1ZmZlclxyXG4gKiBAdGhyb3dzIHt1dGlsLlByb3RvY29sRXJyb3J9IElmIHJlcXVpcmVkIGZpZWxkcyBhcmUgbWlzc2luZ1xyXG4gKi9cclxuVHlwZS5wcm90b3R5cGUuZGVjb2RlRGVsaW1pdGVkID0gZnVuY3Rpb24gZGVjb2RlRGVsaW1pdGVkKHJlYWRlcikge1xyXG4gICAgaWYgKCEocmVhZGVyIGluc3RhbmNlb2YgUmVhZGVyKSlcclxuICAgICAgICByZWFkZXIgPSBSZWFkZXIuY3JlYXRlKHJlYWRlcik7XHJcbiAgICByZXR1cm4gdGhpcy5kZWNvZGUocmVhZGVyLCByZWFkZXIudWludDMyKCkpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIFZlcmlmaWVzIHRoYXQgZmllbGQgdmFsdWVzIGFyZSB2YWxpZCBhbmQgdGhhdCByZXF1aXJlZCBmaWVsZHMgYXJlIHByZXNlbnQuXHJcbiAqIEBwYXJhbSB7T2JqZWN0LjxzdHJpbmcsKj59IG1lc3NhZ2UgUGxhaW4gb2JqZWN0IHRvIHZlcmlmeVxyXG4gKiBAcmV0dXJucyB7bnVsbHxzdHJpbmd9IGBudWxsYCBpZiB2YWxpZCwgb3RoZXJ3aXNlIHRoZSByZWFzb24gd2h5IGl0IGlzIG5vdFxyXG4gKi9cclxuVHlwZS5wcm90b3R5cGUudmVyaWZ5ID0gZnVuY3Rpb24gdmVyaWZ5X3NldHVwKG1lc3NhZ2UpIHtcclxuICAgIHJldHVybiB0aGlzLnNldHVwKCkudmVyaWZ5KG1lc3NhZ2UpOyAvLyBvdmVycmlkZXMgdGhpcyBtZXRob2RcclxufTtcclxuXHJcbi8qKlxyXG4gKiBDcmVhdGVzIGEgbmV3IG1lc3NhZ2Ugb2YgdGhpcyB0eXBlIGZyb20gYSBwbGFpbiBvYmplY3QuIEFsc28gY29udmVydHMgdmFsdWVzIHRvIHRoZWlyIHJlc3BlY3RpdmUgaW50ZXJuYWwgdHlwZXMuXHJcbiAqIEBwYXJhbSB7T2JqZWN0LjxzdHJpbmcsKj59IG9iamVjdCBQbGFpbiBvYmplY3QgdG8gY29udmVydFxyXG4gKiBAcmV0dXJucyB7TWVzc2FnZTx7fT59IE1lc3NhZ2UgaW5zdGFuY2VcclxuICovXHJcblR5cGUucHJvdG90eXBlLmZyb21PYmplY3QgPSBmdW5jdGlvbiBmcm9tT2JqZWN0KG9iamVjdCkge1xyXG4gICAgcmV0dXJuIHRoaXMuc2V0dXAoKS5mcm9tT2JqZWN0KG9iamVjdCk7XHJcbn07XHJcblxyXG4vKipcclxuICogQ29udmVyc2lvbiBvcHRpb25zIGFzIHVzZWQgYnkge0BsaW5rIFR5cGUjdG9PYmplY3R9IGFuZCB7QGxpbmsgTWVzc2FnZS50b09iamVjdH0uXHJcbiAqIEBpbnRlcmZhY2UgSUNvbnZlcnNpb25PcHRpb25zXHJcbiAqIEBwcm9wZXJ0eSB7RnVuY3Rpb259IFtsb25nc10gTG9uZyBjb252ZXJzaW9uIHR5cGUuXHJcbiAqIFZhbGlkIHZhbHVlcyBhcmUgYFN0cmluZ2AgYW5kIGBOdW1iZXJgICh0aGUgZ2xvYmFsIHR5cGVzKS5cclxuICogRGVmYXVsdHMgdG8gY29weSB0aGUgcHJlc2VudCB2YWx1ZSwgd2hpY2ggaXMgYSBwb3NzaWJseSB1bnNhZmUgbnVtYmVyIHdpdGhvdXQgYW5kIGEge0BsaW5rIExvbmd9IHdpdGggYSBsb25nIGxpYnJhcnkuXHJcbiAqIEBwcm9wZXJ0eSB7RnVuY3Rpb259IFtlbnVtc10gRW51bSB2YWx1ZSBjb252ZXJzaW9uIHR5cGUuXHJcbiAqIE9ubHkgdmFsaWQgdmFsdWUgaXMgYFN0cmluZ2AgKHRoZSBnbG9iYWwgdHlwZSkuXHJcbiAqIERlZmF1bHRzIHRvIGNvcHkgdGhlIHByZXNlbnQgdmFsdWUsIHdoaWNoIGlzIHRoZSBudW1lcmljIGlkLlxyXG4gKiBAcHJvcGVydHkge0Z1bmN0aW9ufSBbYnl0ZXNdIEJ5dGVzIHZhbHVlIGNvbnZlcnNpb24gdHlwZS5cclxuICogVmFsaWQgdmFsdWVzIGFyZSBgQXJyYXlgIGFuZCAoYSBiYXNlNjQgZW5jb2RlZCkgYFN0cmluZ2AgKHRoZSBnbG9iYWwgdHlwZXMpLlxyXG4gKiBEZWZhdWx0cyB0byBjb3B5IHRoZSBwcmVzZW50IHZhbHVlLCB3aGljaCB1c3VhbGx5IGlzIGEgQnVmZmVyIHVuZGVyIG5vZGUgYW5kIGFuIFVpbnQ4QXJyYXkgaW4gdGhlIGJyb3dzZXIuXHJcbiAqIEBwcm9wZXJ0eSB7Ym9vbGVhbn0gW2RlZmF1bHRzPWZhbHNlXSBBbHNvIHNldHMgZGVmYXVsdCB2YWx1ZXMgb24gdGhlIHJlc3VsdGluZyBvYmplY3RcclxuICogQHByb3BlcnR5IHtib29sZWFufSBbYXJyYXlzPWZhbHNlXSBTZXRzIGVtcHR5IGFycmF5cyBmb3IgbWlzc2luZyByZXBlYXRlZCBmaWVsZHMgZXZlbiBpZiBgZGVmYXVsdHM9ZmFsc2VgXHJcbiAqIEBwcm9wZXJ0eSB7Ym9vbGVhbn0gW29iamVjdHM9ZmFsc2VdIFNldHMgZW1wdHkgb2JqZWN0cyBmb3IgbWlzc2luZyBtYXAgZmllbGRzIGV2ZW4gaWYgYGRlZmF1bHRzPWZhbHNlYFxyXG4gKiBAcHJvcGVydHkge2Jvb2xlYW59IFtvbmVvZnM9ZmFsc2VdIEluY2x1ZGVzIHZpcnR1YWwgb25lb2YgcHJvcGVydGllcyBzZXQgdG8gdGhlIHByZXNlbnQgZmllbGQncyBuYW1lLCBpZiBhbnlcclxuICogQHByb3BlcnR5IHtib29sZWFufSBbanNvbj1mYWxzZV0gUGVyZm9ybXMgYWRkaXRpb25hbCBKU09OIGNvbXBhdGliaWxpdHkgY29udmVyc2lvbnMsIGkuZS4gTmFOIGFuZCBJbmZpbml0eSB0byBzdHJpbmdzXHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIENyZWF0ZXMgYSBwbGFpbiBvYmplY3QgZnJvbSBhIG1lc3NhZ2Ugb2YgdGhpcyB0eXBlLiBBbHNvIGNvbnZlcnRzIHZhbHVlcyB0byBvdGhlciB0eXBlcyBpZiBzcGVjaWZpZWQuXHJcbiAqIEBwYXJhbSB7TWVzc2FnZTx7fT59IG1lc3NhZ2UgTWVzc2FnZSBpbnN0YW5jZVxyXG4gKiBAcGFyYW0ge0lDb252ZXJzaW9uT3B0aW9uc30gW29wdGlvbnNdIENvbnZlcnNpb24gb3B0aW9uc1xyXG4gKiBAcmV0dXJucyB7T2JqZWN0LjxzdHJpbmcsKj59IFBsYWluIG9iamVjdFxyXG4gKi9cclxuVHlwZS5wcm90b3R5cGUudG9PYmplY3QgPSBmdW5jdGlvbiB0b09iamVjdChtZXNzYWdlLCBvcHRpb25zKSB7XHJcbiAgICByZXR1cm4gdGhpcy5zZXR1cCgpLnRvT2JqZWN0KG1lc3NhZ2UsIG9wdGlvbnMpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIERlY29yYXRvciBmdW5jdGlvbiBhcyByZXR1cm5lZCBieSB7QGxpbmsgVHlwZS5kfSAoVHlwZVNjcmlwdCkuXHJcbiAqIEB0eXBlZGVmIFR5cGVEZWNvcmF0b3JcclxuICogQHR5cGUge2Z1bmN0aW9ufVxyXG4gKiBAcGFyYW0ge0NvbnN0cnVjdG9yPFQ+fSB0YXJnZXQgVGFyZ2V0IGNvbnN0cnVjdG9yXHJcbiAqIEByZXR1cm5zIHt1bmRlZmluZWR9XHJcbiAqIEB0ZW1wbGF0ZSBUIGV4dGVuZHMgTWVzc2FnZTxUPlxyXG4gKi9cclxuXHJcbi8qKlxyXG4gKiBUeXBlIGRlY29yYXRvciAoVHlwZVNjcmlwdCkuXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBbdHlwZU5hbWVdIFR5cGUgbmFtZSwgZGVmYXVsdHMgdG8gdGhlIGNvbnN0cnVjdG9yJ3MgbmFtZVxyXG4gKiBAcmV0dXJucyB7VHlwZURlY29yYXRvcjxUPn0gRGVjb3JhdG9yIGZ1bmN0aW9uXHJcbiAqIEB0ZW1wbGF0ZSBUIGV4dGVuZHMgTWVzc2FnZTxUPlxyXG4gKi9cclxuVHlwZS5kID0gZnVuY3Rpb24gZGVjb3JhdGVUeXBlKHR5cGVOYW1lKSB7XHJcbiAgICByZXR1cm4gZnVuY3Rpb24gdHlwZURlY29yYXRvcih0YXJnZXQpIHtcclxuICAgICAgICB1dGlsLmRlY29yYXRlVHlwZSh0YXJnZXQsIHR5cGVOYW1lKTtcclxuICAgIH07XHJcbn07XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG5cclxuLyoqXHJcbiAqIENvbW1vbiB0eXBlIGNvbnN0YW50cy5cclxuICogQG5hbWVzcGFjZVxyXG4gKi9cclxudmFyIHR5cGVzID0gZXhwb3J0cztcclxuXHJcbnZhciB1dGlsID0gcmVxdWlyZShcIi4vdXRpbFwiKTtcclxuXHJcbnZhciBzID0gW1xyXG4gICAgXCJkb3VibGVcIiwgICAvLyAwXHJcbiAgICBcImZsb2F0XCIsICAgIC8vIDFcclxuICAgIFwiaW50MzJcIiwgICAgLy8gMlxyXG4gICAgXCJ1aW50MzJcIiwgICAvLyAzXHJcbiAgICBcInNpbnQzMlwiLCAgIC8vIDRcclxuICAgIFwiZml4ZWQzMlwiLCAgLy8gNVxyXG4gICAgXCJzZml4ZWQzMlwiLCAvLyA2XHJcbiAgICBcImludDY0XCIsICAgIC8vIDdcclxuICAgIFwidWludDY0XCIsICAgLy8gOFxyXG4gICAgXCJzaW50NjRcIiwgICAvLyA5XHJcbiAgICBcImZpeGVkNjRcIiwgIC8vIDEwXHJcbiAgICBcInNmaXhlZDY0XCIsIC8vIDExXHJcbiAgICBcImJvb2xcIiwgICAgIC8vIDEyXHJcbiAgICBcInN0cmluZ1wiLCAgIC8vIDEzXHJcbiAgICBcImJ5dGVzXCIgICAgIC8vIDE0XHJcbl07XHJcblxyXG5mdW5jdGlvbiBiYWtlKHZhbHVlcywgb2Zmc2V0KSB7XHJcbiAgICB2YXIgaSA9IDAsIG8gPSB7fTtcclxuICAgIG9mZnNldCB8PSAwO1xyXG4gICAgd2hpbGUgKGkgPCB2YWx1ZXMubGVuZ3RoKSBvW3NbaSArIG9mZnNldF1dID0gdmFsdWVzW2krK107XHJcbiAgICByZXR1cm4gbztcclxufVxyXG5cclxuLyoqXHJcbiAqIEJhc2ljIHR5cGUgd2lyZSB0eXBlcy5cclxuICogQHR5cGUge09iamVjdC48c3RyaW5nLG51bWJlcj59XHJcbiAqIEBjb25zdFxyXG4gKiBAcHJvcGVydHkge251bWJlcn0gZG91YmxlPTEgRml4ZWQ2NCB3aXJlIHR5cGVcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IGZsb2F0PTUgRml4ZWQzMiB3aXJlIHR5cGVcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IGludDMyPTAgVmFyaW50IHdpcmUgdHlwZVxyXG4gKiBAcHJvcGVydHkge251bWJlcn0gdWludDMyPTAgVmFyaW50IHdpcmUgdHlwZVxyXG4gKiBAcHJvcGVydHkge251bWJlcn0gc2ludDMyPTAgVmFyaW50IHdpcmUgdHlwZVxyXG4gKiBAcHJvcGVydHkge251bWJlcn0gZml4ZWQzMj01IEZpeGVkMzIgd2lyZSB0eXBlXHJcbiAqIEBwcm9wZXJ0eSB7bnVtYmVyfSBzZml4ZWQzMj01IEZpeGVkMzIgd2lyZSB0eXBlXHJcbiAqIEBwcm9wZXJ0eSB7bnVtYmVyfSBpbnQ2ND0wIFZhcmludCB3aXJlIHR5cGVcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IHVpbnQ2ND0wIFZhcmludCB3aXJlIHR5cGVcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IHNpbnQ2ND0wIFZhcmludCB3aXJlIHR5cGVcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IGZpeGVkNjQ9MSBGaXhlZDY0IHdpcmUgdHlwZVxyXG4gKiBAcHJvcGVydHkge251bWJlcn0gc2ZpeGVkNjQ9MSBGaXhlZDY0IHdpcmUgdHlwZVxyXG4gKiBAcHJvcGVydHkge251bWJlcn0gYm9vbD0wIFZhcmludCB3aXJlIHR5cGVcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IHN0cmluZz0yIExkZWxpbSB3aXJlIHR5cGVcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IGJ5dGVzPTIgTGRlbGltIHdpcmUgdHlwZVxyXG4gKi9cclxudHlwZXMuYmFzaWMgPSBiYWtlKFtcclxuICAgIC8qIGRvdWJsZSAgICovIDEsXHJcbiAgICAvKiBmbG9hdCAgICAqLyA1LFxyXG4gICAgLyogaW50MzIgICAgKi8gMCxcclxuICAgIC8qIHVpbnQzMiAgICovIDAsXHJcbiAgICAvKiBzaW50MzIgICAqLyAwLFxyXG4gICAgLyogZml4ZWQzMiAgKi8gNSxcclxuICAgIC8qIHNmaXhlZDMyICovIDUsXHJcbiAgICAvKiBpbnQ2NCAgICAqLyAwLFxyXG4gICAgLyogdWludDY0ICAgKi8gMCxcclxuICAgIC8qIHNpbnQ2NCAgICovIDAsXHJcbiAgICAvKiBmaXhlZDY0ICAqLyAxLFxyXG4gICAgLyogc2ZpeGVkNjQgKi8gMSxcclxuICAgIC8qIGJvb2wgICAgICovIDAsXHJcbiAgICAvKiBzdHJpbmcgICAqLyAyLFxyXG4gICAgLyogYnl0ZXMgICAgKi8gMlxyXG5dKTtcclxuXHJcbi8qKlxyXG4gKiBCYXNpYyB0eXBlIGRlZmF1bHRzLlxyXG4gKiBAdHlwZSB7T2JqZWN0LjxzdHJpbmcsKj59XHJcbiAqIEBjb25zdFxyXG4gKiBAcHJvcGVydHkge251bWJlcn0gZG91YmxlPTAgRG91YmxlIGRlZmF1bHRcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IGZsb2F0PTAgRmxvYXQgZGVmYXVsdFxyXG4gKiBAcHJvcGVydHkge251bWJlcn0gaW50MzI9MCBJbnQzMiBkZWZhdWx0XHJcbiAqIEBwcm9wZXJ0eSB7bnVtYmVyfSB1aW50MzI9MCBVaW50MzIgZGVmYXVsdFxyXG4gKiBAcHJvcGVydHkge251bWJlcn0gc2ludDMyPTAgU2ludDMyIGRlZmF1bHRcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IGZpeGVkMzI9MCBGaXhlZDMyIGRlZmF1bHRcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IHNmaXhlZDMyPTAgU2ZpeGVkMzIgZGVmYXVsdFxyXG4gKiBAcHJvcGVydHkge251bWJlcn0gaW50NjQ9MCBJbnQ2NCBkZWZhdWx0XHJcbiAqIEBwcm9wZXJ0eSB7bnVtYmVyfSB1aW50NjQ9MCBVaW50NjQgZGVmYXVsdFxyXG4gKiBAcHJvcGVydHkge251bWJlcn0gc2ludDY0PTAgU2ludDMyIGRlZmF1bHRcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IGZpeGVkNjQ9MCBGaXhlZDY0IGRlZmF1bHRcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IHNmaXhlZDY0PTAgU2ZpeGVkNjQgZGVmYXVsdFxyXG4gKiBAcHJvcGVydHkge2Jvb2xlYW59IGJvb2w9ZmFsc2UgQm9vbCBkZWZhdWx0XHJcbiAqIEBwcm9wZXJ0eSB7c3RyaW5nfSBzdHJpbmc9XCJcIiBTdHJpbmcgZGVmYXVsdFxyXG4gKiBAcHJvcGVydHkge0FycmF5LjxudW1iZXI+fSBieXRlcz1BcnJheSgwKSBCeXRlcyBkZWZhdWx0XHJcbiAqIEBwcm9wZXJ0eSB7bnVsbH0gbWVzc2FnZT1udWxsIE1lc3NhZ2UgZGVmYXVsdFxyXG4gKi9cclxudHlwZXMuZGVmYXVsdHMgPSBiYWtlKFtcclxuICAgIC8qIGRvdWJsZSAgICovIDAsXHJcbiAgICAvKiBmbG9hdCAgICAqLyAwLFxyXG4gICAgLyogaW50MzIgICAgKi8gMCxcclxuICAgIC8qIHVpbnQzMiAgICovIDAsXHJcbiAgICAvKiBzaW50MzIgICAqLyAwLFxyXG4gICAgLyogZml4ZWQzMiAgKi8gMCxcclxuICAgIC8qIHNmaXhlZDMyICovIDAsXHJcbiAgICAvKiBpbnQ2NCAgICAqLyAwLFxyXG4gICAgLyogdWludDY0ICAgKi8gMCxcclxuICAgIC8qIHNpbnQ2NCAgICovIDAsXHJcbiAgICAvKiBmaXhlZDY0ICAqLyAwLFxyXG4gICAgLyogc2ZpeGVkNjQgKi8gMCxcclxuICAgIC8qIGJvb2wgICAgICovIGZhbHNlLFxyXG4gICAgLyogc3RyaW5nICAgKi8gXCJcIixcclxuICAgIC8qIGJ5dGVzICAgICovIHV0aWwuZW1wdHlBcnJheSxcclxuICAgIC8qIG1lc3NhZ2UgICovIG51bGxcclxuXSk7XHJcblxyXG4vKipcclxuICogQmFzaWMgbG9uZyB0eXBlIHdpcmUgdHlwZXMuXHJcbiAqIEB0eXBlIHtPYmplY3QuPHN0cmluZyxudW1iZXI+fVxyXG4gKiBAY29uc3RcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IGludDY0PTAgVmFyaW50IHdpcmUgdHlwZVxyXG4gKiBAcHJvcGVydHkge251bWJlcn0gdWludDY0PTAgVmFyaW50IHdpcmUgdHlwZVxyXG4gKiBAcHJvcGVydHkge251bWJlcn0gc2ludDY0PTAgVmFyaW50IHdpcmUgdHlwZVxyXG4gKiBAcHJvcGVydHkge251bWJlcn0gZml4ZWQ2ND0xIEZpeGVkNjQgd2lyZSB0eXBlXHJcbiAqIEBwcm9wZXJ0eSB7bnVtYmVyfSBzZml4ZWQ2ND0xIEZpeGVkNjQgd2lyZSB0eXBlXHJcbiAqL1xyXG50eXBlcy5sb25nID0gYmFrZShbXHJcbiAgICAvKiBpbnQ2NCAgICAqLyAwLFxyXG4gICAgLyogdWludDY0ICAgKi8gMCxcclxuICAgIC8qIHNpbnQ2NCAgICovIDAsXHJcbiAgICAvKiBmaXhlZDY0ICAqLyAxLFxyXG4gICAgLyogc2ZpeGVkNjQgKi8gMVxyXG5dLCA3KTtcclxuXHJcbi8qKlxyXG4gKiBBbGxvd2VkIHR5cGVzIGZvciBtYXAga2V5cyB3aXRoIHRoZWlyIGFzc29jaWF0ZWQgd2lyZSB0eXBlLlxyXG4gKiBAdHlwZSB7T2JqZWN0LjxzdHJpbmcsbnVtYmVyPn1cclxuICogQGNvbnN0XHJcbiAqIEBwcm9wZXJ0eSB7bnVtYmVyfSBpbnQzMj0wIFZhcmludCB3aXJlIHR5cGVcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IHVpbnQzMj0wIFZhcmludCB3aXJlIHR5cGVcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IHNpbnQzMj0wIFZhcmludCB3aXJlIHR5cGVcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IGZpeGVkMzI9NSBGaXhlZDMyIHdpcmUgdHlwZVxyXG4gKiBAcHJvcGVydHkge251bWJlcn0gc2ZpeGVkMzI9NSBGaXhlZDMyIHdpcmUgdHlwZVxyXG4gKiBAcHJvcGVydHkge251bWJlcn0gaW50NjQ9MCBWYXJpbnQgd2lyZSB0eXBlXHJcbiAqIEBwcm9wZXJ0eSB7bnVtYmVyfSB1aW50NjQ9MCBWYXJpbnQgd2lyZSB0eXBlXHJcbiAqIEBwcm9wZXJ0eSB7bnVtYmVyfSBzaW50NjQ9MCBWYXJpbnQgd2lyZSB0eXBlXHJcbiAqIEBwcm9wZXJ0eSB7bnVtYmVyfSBmaXhlZDY0PTEgRml4ZWQ2NCB3aXJlIHR5cGVcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IHNmaXhlZDY0PTEgRml4ZWQ2NCB3aXJlIHR5cGVcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IGJvb2w9MCBWYXJpbnQgd2lyZSB0eXBlXHJcbiAqIEBwcm9wZXJ0eSB7bnVtYmVyfSBzdHJpbmc9MiBMZGVsaW0gd2lyZSB0eXBlXHJcbiAqL1xyXG50eXBlcy5tYXBLZXkgPSBiYWtlKFtcclxuICAgIC8qIGludDMyICAgICovIDAsXHJcbiAgICAvKiB1aW50MzIgICAqLyAwLFxyXG4gICAgLyogc2ludDMyICAgKi8gMCxcclxuICAgIC8qIGZpeGVkMzIgICovIDUsXHJcbiAgICAvKiBzZml4ZWQzMiAqLyA1LFxyXG4gICAgLyogaW50NjQgICAgKi8gMCxcclxuICAgIC8qIHVpbnQ2NCAgICovIDAsXHJcbiAgICAvKiBzaW50NjQgICAqLyAwLFxyXG4gICAgLyogZml4ZWQ2NCAgKi8gMSxcclxuICAgIC8qIHNmaXhlZDY0ICovIDEsXHJcbiAgICAvKiBib29sICAgICAqLyAwLFxyXG4gICAgLyogc3RyaW5nICAgKi8gMlxyXG5dLCAyKTtcclxuXHJcbi8qKlxyXG4gKiBBbGxvd2VkIHR5cGVzIGZvciBwYWNrZWQgcmVwZWF0ZWQgZmllbGRzIHdpdGggdGhlaXIgYXNzb2NpYXRlZCB3aXJlIHR5cGUuXHJcbiAqIEB0eXBlIHtPYmplY3QuPHN0cmluZyxudW1iZXI+fVxyXG4gKiBAY29uc3RcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IGRvdWJsZT0xIEZpeGVkNjQgd2lyZSB0eXBlXHJcbiAqIEBwcm9wZXJ0eSB7bnVtYmVyfSBmbG9hdD01IEZpeGVkMzIgd2lyZSB0eXBlXHJcbiAqIEBwcm9wZXJ0eSB7bnVtYmVyfSBpbnQzMj0wIFZhcmludCB3aXJlIHR5cGVcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IHVpbnQzMj0wIFZhcmludCB3aXJlIHR5cGVcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IHNpbnQzMj0wIFZhcmludCB3aXJlIHR5cGVcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IGZpeGVkMzI9NSBGaXhlZDMyIHdpcmUgdHlwZVxyXG4gKiBAcHJvcGVydHkge251bWJlcn0gc2ZpeGVkMzI9NSBGaXhlZDMyIHdpcmUgdHlwZVxyXG4gKiBAcHJvcGVydHkge251bWJlcn0gaW50NjQ9MCBWYXJpbnQgd2lyZSB0eXBlXHJcbiAqIEBwcm9wZXJ0eSB7bnVtYmVyfSB1aW50NjQ9MCBWYXJpbnQgd2lyZSB0eXBlXHJcbiAqIEBwcm9wZXJ0eSB7bnVtYmVyfSBzaW50NjQ9MCBWYXJpbnQgd2lyZSB0eXBlXHJcbiAqIEBwcm9wZXJ0eSB7bnVtYmVyfSBmaXhlZDY0PTEgRml4ZWQ2NCB3aXJlIHR5cGVcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IHNmaXhlZDY0PTEgRml4ZWQ2NCB3aXJlIHR5cGVcclxuICogQHByb3BlcnR5IHtudW1iZXJ9IGJvb2w9MCBWYXJpbnQgd2lyZSB0eXBlXHJcbiAqL1xyXG50eXBlcy5wYWNrZWQgPSBiYWtlKFtcclxuICAgIC8qIGRvdWJsZSAgICovIDEsXHJcbiAgICAvKiBmbG9hdCAgICAqLyA1LFxyXG4gICAgLyogaW50MzIgICAgKi8gMCxcclxuICAgIC8qIHVpbnQzMiAgICovIDAsXHJcbiAgICAvKiBzaW50MzIgICAqLyAwLFxyXG4gICAgLyogZml4ZWQzMiAgKi8gNSxcclxuICAgIC8qIHNmaXhlZDMyICovIDUsXHJcbiAgICAvKiBpbnQ2NCAgICAqLyAwLFxyXG4gICAgLyogdWludDY0ICAgKi8gMCxcclxuICAgIC8qIHNpbnQ2NCAgICovIDAsXHJcbiAgICAvKiBmaXhlZDY0ICAqLyAxLFxyXG4gICAgLyogc2ZpeGVkNjQgKi8gMSxcclxuICAgIC8qIGJvb2wgICAgICovIDBcclxuXSk7XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG5cclxuLyoqXHJcbiAqIFZhcmlvdXMgdXRpbGl0eSBmdW5jdGlvbnMuXHJcbiAqIEBuYW1lc3BhY2VcclxuICovXHJcbnZhciB1dGlsID0gbW9kdWxlLmV4cG9ydHMgPSByZXF1aXJlKFwiLi91dGlsL21pbmltYWxcIik7XHJcblxyXG52YXIgcm9vdHMgPSByZXF1aXJlKFwiLi9yb290c1wiKTtcclxuXHJcbnZhciBUeXBlLCAvLyBjeWNsaWNcclxuICAgIEVudW07XHJcblxyXG51dGlsLmNvZGVnZW4gPSByZXF1aXJlKFwiQHByb3RvYnVmanMvY29kZWdlblwiKTtcclxudXRpbC5mZXRjaCAgID0gcmVxdWlyZShcIkBwcm90b2J1ZmpzL2ZldGNoXCIpO1xyXG51dGlsLnBhdGggICAgPSByZXF1aXJlKFwiQHByb3RvYnVmanMvcGF0aFwiKTtcclxuXHJcbi8qKlxyXG4gKiBOb2RlJ3MgZnMgbW9kdWxlIGlmIGF2YWlsYWJsZS5cclxuICogQHR5cGUge09iamVjdC48c3RyaW5nLCo+fVxyXG4gKi9cclxudXRpbC5mcyA9IHV0aWwuaW5xdWlyZShcImZzXCIpO1xyXG5cclxuLyoqXHJcbiAqIENvbnZlcnRzIGFuIG9iamVjdCdzIHZhbHVlcyB0byBhbiBhcnJheS5cclxuICogQHBhcmFtIHtPYmplY3QuPHN0cmluZywqPn0gb2JqZWN0IE9iamVjdCB0byBjb252ZXJ0XHJcbiAqIEByZXR1cm5zIHtBcnJheS48Kj59IENvbnZlcnRlZCBhcnJheVxyXG4gKi9cclxudXRpbC50b0FycmF5ID0gZnVuY3Rpb24gdG9BcnJheShvYmplY3QpIHtcclxuICAgIGlmIChvYmplY3QpIHtcclxuICAgICAgICB2YXIga2V5cyAgPSBPYmplY3Qua2V5cyhvYmplY3QpLFxyXG4gICAgICAgICAgICBhcnJheSA9IG5ldyBBcnJheShrZXlzLmxlbmd0aCksXHJcbiAgICAgICAgICAgIGluZGV4ID0gMDtcclxuICAgICAgICB3aGlsZSAoaW5kZXggPCBrZXlzLmxlbmd0aClcclxuICAgICAgICAgICAgYXJyYXlbaW5kZXhdID0gb2JqZWN0W2tleXNbaW5kZXgrK11dO1xyXG4gICAgICAgIHJldHVybiBhcnJheTtcclxuICAgIH1cclxuICAgIHJldHVybiBbXTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBDb252ZXJ0cyBhbiBhcnJheSBvZiBrZXlzIGltbWVkaWF0ZWx5IGZvbGxvd2VkIGJ5IHRoZWlyIHJlc3BlY3RpdmUgdmFsdWUgdG8gYW4gb2JqZWN0LCBvbWl0dGluZyB1bmRlZmluZWQgdmFsdWVzLlxyXG4gKiBAcGFyYW0ge0FycmF5LjwqPn0gYXJyYXkgQXJyYXkgdG8gY29udmVydFxyXG4gKiBAcmV0dXJucyB7T2JqZWN0LjxzdHJpbmcsKj59IENvbnZlcnRlZCBvYmplY3RcclxuICovXHJcbnV0aWwudG9PYmplY3QgPSBmdW5jdGlvbiB0b09iamVjdChhcnJheSkge1xyXG4gICAgdmFyIG9iamVjdCA9IHt9LFxyXG4gICAgICAgIGluZGV4ICA9IDA7XHJcbiAgICB3aGlsZSAoaW5kZXggPCBhcnJheS5sZW5ndGgpIHtcclxuICAgICAgICB2YXIga2V5ID0gYXJyYXlbaW5kZXgrK10sXHJcbiAgICAgICAgICAgIHZhbCA9IGFycmF5W2luZGV4KytdO1xyXG4gICAgICAgIGlmICh2YWwgIT09IHVuZGVmaW5lZClcclxuICAgICAgICAgICAgb2JqZWN0W2tleV0gPSB2YWw7XHJcbiAgICB9XHJcbiAgICByZXR1cm4gb2JqZWN0O1xyXG59O1xyXG5cclxudmFyIHNhZmVQcm9wQmFja3NsYXNoUmUgPSAvXFxcXC9nLFxyXG4gICAgc2FmZVByb3BRdW90ZVJlICAgICA9IC9cIi9nO1xyXG5cclxuLyoqXHJcbiAqIFRlc3RzIHdoZXRoZXIgdGhlIHNwZWNpZmllZCBuYW1lIGlzIGEgcmVzZXJ2ZWQgd29yZCBpbiBKUy5cclxuICogQHBhcmFtIHtzdHJpbmd9IG5hbWUgTmFtZSB0byB0ZXN0XHJcbiAqIEByZXR1cm5zIHtib29sZWFufSBgdHJ1ZWAgaWYgcmVzZXJ2ZWQsIG90aGVyd2lzZSBgZmFsc2VgXHJcbiAqL1xyXG51dGlsLmlzUmVzZXJ2ZWQgPSBmdW5jdGlvbiBpc1Jlc2VydmVkKG5hbWUpIHtcclxuICAgIHJldHVybiAvXig/OmRvfGlmfGlufGZvcnxsZXR8bmV3fHRyeXx2YXJ8Y2FzZXxlbHNlfGVudW18ZXZhbHxmYWxzZXxudWxsfHRoaXN8dHJ1ZXx2b2lkfHdpdGh8YnJlYWt8Y2F0Y2h8Y2xhc3N8Y29uc3R8c3VwZXJ8dGhyb3d8d2hpbGV8eWllbGR8ZGVsZXRlfGV4cG9ydHxpbXBvcnR8cHVibGljfHJldHVybnxzdGF0aWN8c3dpdGNofHR5cGVvZnxkZWZhdWx0fGV4dGVuZHN8ZmluYWxseXxwYWNrYWdlfHByaXZhdGV8Y29udGludWV8ZGVidWdnZXJ8ZnVuY3Rpb258YXJndW1lbnRzfGludGVyZmFjZXxwcm90ZWN0ZWR8aW1wbGVtZW50c3xpbnN0YW5jZW9mKSQvLnRlc3QobmFtZSk7XHJcbn07XHJcblxyXG4vKipcclxuICogUmV0dXJucyBhIHNhZmUgcHJvcGVydHkgYWNjZXNzb3IgZm9yIHRoZSBzcGVjaWZpZWQgcHJvcGVydHkgbmFtZS5cclxuICogQHBhcmFtIHtzdHJpbmd9IHByb3AgUHJvcGVydHkgbmFtZVxyXG4gKiBAcmV0dXJucyB7c3RyaW5nfSBTYWZlIGFjY2Vzc29yXHJcbiAqL1xyXG51dGlsLnNhZmVQcm9wID0gZnVuY3Rpb24gc2FmZVByb3AocHJvcCkge1xyXG4gICAgaWYgKCEvXlskXFx3X10rJC8udGVzdChwcm9wKSB8fCB1dGlsLmlzUmVzZXJ2ZWQocHJvcCkpXHJcbiAgICAgICAgcmV0dXJuIFwiW1xcXCJcIiArIHByb3AucmVwbGFjZShzYWZlUHJvcEJhY2tzbGFzaFJlLCBcIlxcXFxcXFxcXCIpLnJlcGxhY2Uoc2FmZVByb3BRdW90ZVJlLCBcIlxcXFxcXFwiXCIpICsgXCJcXFwiXVwiO1xyXG4gICAgcmV0dXJuIFwiLlwiICsgcHJvcDtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBDb252ZXJ0cyB0aGUgZmlyc3QgY2hhcmFjdGVyIG9mIGEgc3RyaW5nIHRvIHVwcGVyIGNhc2UuXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBzdHIgU3RyaW5nIHRvIGNvbnZlcnRcclxuICogQHJldHVybnMge3N0cmluZ30gQ29udmVydGVkIHN0cmluZ1xyXG4gKi9cclxudXRpbC51Y0ZpcnN0ID0gZnVuY3Rpb24gdWNGaXJzdChzdHIpIHtcclxuICAgIHJldHVybiBzdHIuY2hhckF0KDApLnRvVXBwZXJDYXNlKCkgKyBzdHIuc3Vic3RyaW5nKDEpO1xyXG59O1xyXG5cclxudmFyIGNhbWVsQ2FzZVJlID0gL18oW2Etel0pL2c7XHJcblxyXG4vKipcclxuICogQ29udmVydHMgYSBzdHJpbmcgdG8gY2FtZWwgY2FzZS5cclxuICogQHBhcmFtIHtzdHJpbmd9IHN0ciBTdHJpbmcgdG8gY29udmVydFxyXG4gKiBAcmV0dXJucyB7c3RyaW5nfSBDb252ZXJ0ZWQgc3RyaW5nXHJcbiAqL1xyXG51dGlsLmNhbWVsQ2FzZSA9IGZ1bmN0aW9uIGNhbWVsQ2FzZShzdHIpIHtcclxuICAgIHJldHVybiBzdHIuc3Vic3RyaW5nKDAsIDEpXHJcbiAgICAgICAgICsgc3RyLnN1YnN0cmluZygxKVxyXG4gICAgICAgICAgICAgICAucmVwbGFjZShjYW1lbENhc2VSZSwgZnVuY3Rpb24oJDAsICQxKSB7IHJldHVybiAkMS50b1VwcGVyQ2FzZSgpOyB9KTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBDb21wYXJlcyByZWZsZWN0ZWQgZmllbGRzIGJ5IGlkLlxyXG4gKiBAcGFyYW0ge0ZpZWxkfSBhIEZpcnN0IGZpZWxkXHJcbiAqIEBwYXJhbSB7RmllbGR9IGIgU2Vjb25kIGZpZWxkXHJcbiAqIEByZXR1cm5zIHtudW1iZXJ9IENvbXBhcmlzb24gdmFsdWVcclxuICovXHJcbnV0aWwuY29tcGFyZUZpZWxkc0J5SWQgPSBmdW5jdGlvbiBjb21wYXJlRmllbGRzQnlJZChhLCBiKSB7XHJcbiAgICByZXR1cm4gYS5pZCAtIGIuaWQ7XHJcbn07XHJcblxyXG4vKipcclxuICogRGVjb3JhdG9yIGhlbHBlciBmb3IgdHlwZXMgKFR5cGVTY3JpcHQpLlxyXG4gKiBAcGFyYW0ge0NvbnN0cnVjdG9yPFQ+fSBjdG9yIENvbnN0cnVjdG9yIGZ1bmN0aW9uXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBbdHlwZU5hbWVdIFR5cGUgbmFtZSwgZGVmYXVsdHMgdG8gdGhlIGNvbnN0cnVjdG9yJ3MgbmFtZVxyXG4gKiBAcmV0dXJucyB7VHlwZX0gUmVmbGVjdGVkIHR5cGVcclxuICogQHRlbXBsYXRlIFQgZXh0ZW5kcyBNZXNzYWdlPFQ+XHJcbiAqIEBwcm9wZXJ0eSB7Um9vdH0gcm9vdCBEZWNvcmF0b3JzIHJvb3RcclxuICovXHJcbnV0aWwuZGVjb3JhdGVUeXBlID0gZnVuY3Rpb24gZGVjb3JhdGVUeXBlKGN0b3IsIHR5cGVOYW1lKSB7XHJcblxyXG4gICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICBpZiAoY3Rvci4kdHlwZSkge1xyXG4gICAgICAgIGlmICh0eXBlTmFtZSAmJiBjdG9yLiR0eXBlLm5hbWUgIT09IHR5cGVOYW1lKSB7XHJcbiAgICAgICAgICAgIHV0aWwuZGVjb3JhdGVSb290LnJlbW92ZShjdG9yLiR0eXBlKTtcclxuICAgICAgICAgICAgY3Rvci4kdHlwZS5uYW1lID0gdHlwZU5hbWU7XHJcbiAgICAgICAgICAgIHV0aWwuZGVjb3JhdGVSb290LmFkZChjdG9yLiR0eXBlKTtcclxuICAgICAgICB9XHJcbiAgICAgICAgcmV0dXJuIGN0b3IuJHR5cGU7XHJcbiAgICB9XHJcblxyXG4gICAgLyogaXN0YW5idWwgaWdub3JlIG5leHQgKi9cclxuICAgIGlmICghVHlwZSlcclxuICAgICAgICBUeXBlID0gcmVxdWlyZShcIi4vdHlwZVwiKTtcclxuXHJcbiAgICB2YXIgdHlwZSA9IG5ldyBUeXBlKHR5cGVOYW1lIHx8IGN0b3IubmFtZSk7XHJcbiAgICB1dGlsLmRlY29yYXRlUm9vdC5hZGQodHlwZSk7XHJcbiAgICB0eXBlLmN0b3IgPSBjdG9yOyAvLyBzZXRzIHVwIC5lbmNvZGUsIC5kZWNvZGUgZXRjLlxyXG4gICAgT2JqZWN0LmRlZmluZVByb3BlcnR5KGN0b3IsIFwiJHR5cGVcIiwgeyB2YWx1ZTogdHlwZSwgZW51bWVyYWJsZTogZmFsc2UgfSk7XHJcbiAgICBPYmplY3QuZGVmaW5lUHJvcGVydHkoY3Rvci5wcm90b3R5cGUsIFwiJHR5cGVcIiwgeyB2YWx1ZTogdHlwZSwgZW51bWVyYWJsZTogZmFsc2UgfSk7XHJcbiAgICByZXR1cm4gdHlwZTtcclxufTtcclxuXHJcbnZhciBkZWNvcmF0ZUVudW1JbmRleCA9IDA7XHJcblxyXG4vKipcclxuICogRGVjb3JhdG9yIGhlbHBlciBmb3IgZW51bXMgKFR5cGVTY3JpcHQpLlxyXG4gKiBAcGFyYW0ge09iamVjdH0gb2JqZWN0IEVudW0gb2JqZWN0XHJcbiAqIEByZXR1cm5zIHtFbnVtfSBSZWZsZWN0ZWQgZW51bVxyXG4gKi9cclxudXRpbC5kZWNvcmF0ZUVudW0gPSBmdW5jdGlvbiBkZWNvcmF0ZUVudW0ob2JqZWN0KSB7XHJcblxyXG4gICAgLyogaXN0YW5idWwgaWdub3JlIGlmICovXHJcbiAgICBpZiAob2JqZWN0LiR0eXBlKVxyXG4gICAgICAgIHJldHVybiBvYmplY3QuJHR5cGU7XHJcblxyXG4gICAgLyogaXN0YW5idWwgaWdub3JlIG5leHQgKi9cclxuICAgIGlmICghRW51bSlcclxuICAgICAgICBFbnVtID0gcmVxdWlyZShcIi4vZW51bVwiKTtcclxuXHJcbiAgICB2YXIgZW5tID0gbmV3IEVudW0oXCJFbnVtXCIgKyBkZWNvcmF0ZUVudW1JbmRleCsrLCBvYmplY3QpO1xyXG4gICAgdXRpbC5kZWNvcmF0ZVJvb3QuYWRkKGVubSk7XHJcbiAgICBPYmplY3QuZGVmaW5lUHJvcGVydHkob2JqZWN0LCBcIiR0eXBlXCIsIHsgdmFsdWU6IGVubSwgZW51bWVyYWJsZTogZmFsc2UgfSk7XHJcbiAgICByZXR1cm4gZW5tO1xyXG59O1xyXG5cclxuXHJcbi8qKlxyXG4gKiBTZXRzIHRoZSB2YWx1ZSBvZiBhIHByb3BlcnR5IGJ5IHByb3BlcnR5IHBhdGguIElmIGEgdmFsdWUgYWxyZWFkeSBleGlzdHMsIGl0IGlzIHR1cm5lZCB0byBhbiBhcnJheVxyXG4gKiBAcGFyYW0ge09iamVjdC48c3RyaW5nLCo+fSBkc3QgRGVzdGluYXRpb24gb2JqZWN0XHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBwYXRoIGRvdCAnLicgZGVsaW1pdGVkIHBhdGggb2YgdGhlIHByb3BlcnR5IHRvIHNldFxyXG4gKiBAcGFyYW0ge09iamVjdH0gdmFsdWUgdGhlIHZhbHVlIHRvIHNldFxyXG4gKiBAcmV0dXJucyB7T2JqZWN0LjxzdHJpbmcsKj59IERlc3RpbmF0aW9uIG9iamVjdFxyXG4gKi9cclxudXRpbC5zZXRQcm9wZXJ0eSA9IGZ1bmN0aW9uIHNldFByb3BlcnR5KGRzdCwgcGF0aCwgdmFsdWUpIHtcclxuICAgIGZ1bmN0aW9uIHNldFByb3AoZHN0LCBwYXRoLCB2YWx1ZSkge1xyXG4gICAgICAgIHZhciBwYXJ0ID0gcGF0aC5zaGlmdCgpO1xyXG4gICAgICAgIGlmIChwYXJ0ID09PSBcIl9fcHJvdG9fX1wiIHx8IHBhcnQgPT09IFwicHJvdG90eXBlXCIpIHtcclxuICAgICAgICAgIHJldHVybiBkc3Q7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIGlmIChwYXRoLmxlbmd0aCA+IDApIHtcclxuICAgICAgICAgICAgZHN0W3BhcnRdID0gc2V0UHJvcChkc3RbcGFydF0gfHwge30sIHBhdGgsIHZhbHVlKTtcclxuICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICB2YXIgcHJldlZhbHVlID0gZHN0W3BhcnRdO1xyXG4gICAgICAgICAgICBpZiAocHJldlZhbHVlKVxyXG4gICAgICAgICAgICAgICAgdmFsdWUgPSBbXS5jb25jYXQocHJldlZhbHVlKS5jb25jYXQodmFsdWUpO1xyXG4gICAgICAgICAgICBkc3RbcGFydF0gPSB2YWx1ZTtcclxuICAgICAgICB9XHJcbiAgICAgICAgcmV0dXJuIGRzdDtcclxuICAgIH1cclxuXHJcbiAgICBpZiAodHlwZW9mIGRzdCAhPT0gXCJvYmplY3RcIilcclxuICAgICAgICB0aHJvdyBUeXBlRXJyb3IoXCJkc3QgbXVzdCBiZSBhbiBvYmplY3RcIik7XHJcbiAgICBpZiAoIXBhdGgpXHJcbiAgICAgICAgdGhyb3cgVHlwZUVycm9yKFwicGF0aCBtdXN0IGJlIHNwZWNpZmllZFwiKTtcclxuXHJcbiAgICBwYXRoID0gcGF0aC5zcGxpdChcIi5cIik7XHJcbiAgICByZXR1cm4gc2V0UHJvcChkc3QsIHBhdGgsIHZhbHVlKTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBEZWNvcmF0b3Igcm9vdCAoVHlwZVNjcmlwdCkuXHJcbiAqIEBuYW1lIHV0aWwuZGVjb3JhdGVSb290XHJcbiAqIEB0eXBlIHtSb290fVxyXG4gKiBAcmVhZG9ubHlcclxuICovXHJcbk9iamVjdC5kZWZpbmVQcm9wZXJ0eSh1dGlsLCBcImRlY29yYXRlUm9vdFwiLCB7XHJcbiAgICBnZXQ6IGZ1bmN0aW9uKCkge1xyXG4gICAgICAgIHJldHVybiByb290c1tcImRlY29yYXRlZFwiXSB8fCAocm9vdHNbXCJkZWNvcmF0ZWRcIl0gPSBuZXcgKHJlcXVpcmUoXCIuL3Jvb3RcIikpKCkpO1xyXG4gICAgfVxyXG59KTtcclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbm1vZHVsZS5leHBvcnRzID0gTG9uZ0JpdHM7XHJcblxyXG52YXIgdXRpbCA9IHJlcXVpcmUoXCIuLi91dGlsL21pbmltYWxcIik7XHJcblxyXG4vKipcclxuICogQ29uc3RydWN0cyBuZXcgbG9uZyBiaXRzLlxyXG4gKiBAY2xhc3NkZXNjIEhlbHBlciBjbGFzcyBmb3Igd29ya2luZyB3aXRoIHRoZSBsb3cgYW5kIGhpZ2ggYml0cyBvZiBhIDY0IGJpdCB2YWx1ZS5cclxuICogQG1lbWJlcm9mIHV0aWxcclxuICogQGNvbnN0cnVjdG9yXHJcbiAqIEBwYXJhbSB7bnVtYmVyfSBsbyBMb3cgMzIgYml0cywgdW5zaWduZWRcclxuICogQHBhcmFtIHtudW1iZXJ9IGhpIEhpZ2ggMzIgYml0cywgdW5zaWduZWRcclxuICovXHJcbmZ1bmN0aW9uIExvbmdCaXRzKGxvLCBoaSkge1xyXG5cclxuICAgIC8vIG5vdGUgdGhhdCB0aGUgY2FzdHMgYmVsb3cgYXJlIHRoZW9yZXRpY2FsbHkgdW5uZWNlc3NhcnkgYXMgb2YgdG9kYXksIGJ1dCBvbGRlciBzdGF0aWNhbGx5XHJcbiAgICAvLyBnZW5lcmF0ZWQgY29udmVydGVyIGNvZGUgbWlnaHQgc3RpbGwgY2FsbCB0aGUgY3RvciB3aXRoIHNpZ25lZCAzMmJpdHMuIGtlcHQgZm9yIGNvbXBhdC5cclxuXHJcbiAgICAvKipcclxuICAgICAqIExvdyBiaXRzLlxyXG4gICAgICogQHR5cGUge251bWJlcn1cclxuICAgICAqL1xyXG4gICAgdGhpcy5sbyA9IGxvID4+PiAwO1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogSGlnaCBiaXRzLlxyXG4gICAgICogQHR5cGUge251bWJlcn1cclxuICAgICAqL1xyXG4gICAgdGhpcy5oaSA9IGhpID4+PiAwO1xyXG59XHJcblxyXG4vKipcclxuICogWmVybyBiaXRzLlxyXG4gKiBAbWVtYmVyb2YgdXRpbC5Mb25nQml0c1xyXG4gKiBAdHlwZSB7dXRpbC5Mb25nQml0c31cclxuICovXHJcbnZhciB6ZXJvID0gTG9uZ0JpdHMuemVybyA9IG5ldyBMb25nQml0cygwLCAwKTtcclxuXHJcbnplcm8udG9OdW1iZXIgPSBmdW5jdGlvbigpIHsgcmV0dXJuIDA7IH07XHJcbnplcm8uenpFbmNvZGUgPSB6ZXJvLnp6RGVjb2RlID0gZnVuY3Rpb24oKSB7IHJldHVybiB0aGlzOyB9O1xyXG56ZXJvLmxlbmd0aCA9IGZ1bmN0aW9uKCkgeyByZXR1cm4gMTsgfTtcclxuXHJcbi8qKlxyXG4gKiBaZXJvIGhhc2guXHJcbiAqIEBtZW1iZXJvZiB1dGlsLkxvbmdCaXRzXHJcbiAqIEB0eXBlIHtzdHJpbmd9XHJcbiAqL1xyXG52YXIgemVyb0hhc2ggPSBMb25nQml0cy56ZXJvSGFzaCA9IFwiXFwwXFwwXFwwXFwwXFwwXFwwXFwwXFwwXCI7XHJcblxyXG4vKipcclxuICogQ29uc3RydWN0cyBuZXcgbG9uZyBiaXRzIGZyb20gdGhlIHNwZWNpZmllZCBudW1iZXIuXHJcbiAqIEBwYXJhbSB7bnVtYmVyfSB2YWx1ZSBWYWx1ZVxyXG4gKiBAcmV0dXJucyB7dXRpbC5Mb25nQml0c30gSW5zdGFuY2VcclxuICovXHJcbkxvbmdCaXRzLmZyb21OdW1iZXIgPSBmdW5jdGlvbiBmcm9tTnVtYmVyKHZhbHVlKSB7XHJcbiAgICBpZiAodmFsdWUgPT09IDApXHJcbiAgICAgICAgcmV0dXJuIHplcm87XHJcbiAgICB2YXIgc2lnbiA9IHZhbHVlIDwgMDtcclxuICAgIGlmIChzaWduKVxyXG4gICAgICAgIHZhbHVlID0gLXZhbHVlO1xyXG4gICAgdmFyIGxvID0gdmFsdWUgPj4+IDAsXHJcbiAgICAgICAgaGkgPSAodmFsdWUgLSBsbykgLyA0Mjk0OTY3Mjk2ID4+PiAwO1xyXG4gICAgaWYgKHNpZ24pIHtcclxuICAgICAgICBoaSA9IH5oaSA+Pj4gMDtcclxuICAgICAgICBsbyA9IH5sbyA+Pj4gMDtcclxuICAgICAgICBpZiAoKytsbyA+IDQyOTQ5NjcyOTUpIHtcclxuICAgICAgICAgICAgbG8gPSAwO1xyXG4gICAgICAgICAgICBpZiAoKytoaSA+IDQyOTQ5NjcyOTUpXHJcbiAgICAgICAgICAgICAgICBoaSA9IDA7XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG4gICAgcmV0dXJuIG5ldyBMb25nQml0cyhsbywgaGkpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIENvbnN0cnVjdHMgbmV3IGxvbmcgYml0cyBmcm9tIGEgbnVtYmVyLCBsb25nIG9yIHN0cmluZy5cclxuICogQHBhcmFtIHtMb25nfG51bWJlcnxzdHJpbmd9IHZhbHVlIFZhbHVlXHJcbiAqIEByZXR1cm5zIHt1dGlsLkxvbmdCaXRzfSBJbnN0YW5jZVxyXG4gKi9cclxuTG9uZ0JpdHMuZnJvbSA9IGZ1bmN0aW9uIGZyb20odmFsdWUpIHtcclxuICAgIGlmICh0eXBlb2YgdmFsdWUgPT09IFwibnVtYmVyXCIpXHJcbiAgICAgICAgcmV0dXJuIExvbmdCaXRzLmZyb21OdW1iZXIodmFsdWUpO1xyXG4gICAgaWYgKHV0aWwuaXNTdHJpbmcodmFsdWUpKSB7XHJcbiAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIGVsc2UgKi9cclxuICAgICAgICBpZiAodXRpbC5Mb25nKVxyXG4gICAgICAgICAgICB2YWx1ZSA9IHV0aWwuTG9uZy5mcm9tU3RyaW5nKHZhbHVlKTtcclxuICAgICAgICBlbHNlXHJcbiAgICAgICAgICAgIHJldHVybiBMb25nQml0cy5mcm9tTnVtYmVyKHBhcnNlSW50KHZhbHVlLCAxMCkpO1xyXG4gICAgfVxyXG4gICAgcmV0dXJuIHZhbHVlLmxvdyB8fCB2YWx1ZS5oaWdoID8gbmV3IExvbmdCaXRzKHZhbHVlLmxvdyA+Pj4gMCwgdmFsdWUuaGlnaCA+Pj4gMCkgOiB6ZXJvO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIENvbnZlcnRzIHRoaXMgbG9uZyBiaXRzIHRvIGEgcG9zc2libHkgdW5zYWZlIEphdmFTY3JpcHQgbnVtYmVyLlxyXG4gKiBAcGFyYW0ge2Jvb2xlYW59IFt1bnNpZ25lZD1mYWxzZV0gV2hldGhlciB1bnNpZ25lZCBvciBub3RcclxuICogQHJldHVybnMge251bWJlcn0gUG9zc2libHkgdW5zYWZlIG51bWJlclxyXG4gKi9cclxuTG9uZ0JpdHMucHJvdG90eXBlLnRvTnVtYmVyID0gZnVuY3Rpb24gdG9OdW1iZXIodW5zaWduZWQpIHtcclxuICAgIGlmICghdW5zaWduZWQgJiYgdGhpcy5oaSA+Pj4gMzEpIHtcclxuICAgICAgICB2YXIgbG8gPSB+dGhpcy5sbyArIDEgPj4+IDAsXHJcbiAgICAgICAgICAgIGhpID0gfnRoaXMuaGkgICAgID4+PiAwO1xyXG4gICAgICAgIGlmICghbG8pXHJcbiAgICAgICAgICAgIGhpID0gaGkgKyAxID4+PiAwO1xyXG4gICAgICAgIHJldHVybiAtKGxvICsgaGkgKiA0Mjk0OTY3Mjk2KTtcclxuICAgIH1cclxuICAgIHJldHVybiB0aGlzLmxvICsgdGhpcy5oaSAqIDQyOTQ5NjcyOTY7XHJcbn07XHJcblxyXG4vKipcclxuICogQ29udmVydHMgdGhpcyBsb25nIGJpdHMgdG8gYSBsb25nLlxyXG4gKiBAcGFyYW0ge2Jvb2xlYW59IFt1bnNpZ25lZD1mYWxzZV0gV2hldGhlciB1bnNpZ25lZCBvciBub3RcclxuICogQHJldHVybnMge0xvbmd9IExvbmdcclxuICovXHJcbkxvbmdCaXRzLnByb3RvdHlwZS50b0xvbmcgPSBmdW5jdGlvbiB0b0xvbmcodW5zaWduZWQpIHtcclxuICAgIHJldHVybiB1dGlsLkxvbmdcclxuICAgICAgICA/IG5ldyB1dGlsLkxvbmcodGhpcy5sbyB8IDAsIHRoaXMuaGkgfCAwLCBCb29sZWFuKHVuc2lnbmVkKSlcclxuICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgbmV4dCAqL1xyXG4gICAgICAgIDogeyBsb3c6IHRoaXMubG8gfCAwLCBoaWdoOiB0aGlzLmhpIHwgMCwgdW5zaWduZWQ6IEJvb2xlYW4odW5zaWduZWQpIH07XHJcbn07XHJcblxyXG52YXIgY2hhckNvZGVBdCA9IFN0cmluZy5wcm90b3R5cGUuY2hhckNvZGVBdDtcclxuXHJcbi8qKlxyXG4gKiBDb25zdHJ1Y3RzIG5ldyBsb25nIGJpdHMgZnJvbSB0aGUgc3BlY2lmaWVkIDggY2hhcmFjdGVycyBsb25nIGhhc2guXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBoYXNoIEhhc2hcclxuICogQHJldHVybnMge3V0aWwuTG9uZ0JpdHN9IEJpdHNcclxuICovXHJcbkxvbmdCaXRzLmZyb21IYXNoID0gZnVuY3Rpb24gZnJvbUhhc2goaGFzaCkge1xyXG4gICAgaWYgKGhhc2ggPT09IHplcm9IYXNoKVxyXG4gICAgICAgIHJldHVybiB6ZXJvO1xyXG4gICAgcmV0dXJuIG5ldyBMb25nQml0cyhcclxuICAgICAgICAoIGNoYXJDb2RlQXQuY2FsbChoYXNoLCAwKVxyXG4gICAgICAgIHwgY2hhckNvZGVBdC5jYWxsKGhhc2gsIDEpIDw8IDhcclxuICAgICAgICB8IGNoYXJDb2RlQXQuY2FsbChoYXNoLCAyKSA8PCAxNlxyXG4gICAgICAgIHwgY2hhckNvZGVBdC5jYWxsKGhhc2gsIDMpIDw8IDI0KSA+Pj4gMFxyXG4gICAgLFxyXG4gICAgICAgICggY2hhckNvZGVBdC5jYWxsKGhhc2gsIDQpXHJcbiAgICAgICAgfCBjaGFyQ29kZUF0LmNhbGwoaGFzaCwgNSkgPDwgOFxyXG4gICAgICAgIHwgY2hhckNvZGVBdC5jYWxsKGhhc2gsIDYpIDw8IDE2XHJcbiAgICAgICAgfCBjaGFyQ29kZUF0LmNhbGwoaGFzaCwgNykgPDwgMjQpID4+PiAwXHJcbiAgICApO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIENvbnZlcnRzIHRoaXMgbG9uZyBiaXRzIHRvIGEgOCBjaGFyYWN0ZXJzIGxvbmcgaGFzaC5cclxuICogQHJldHVybnMge3N0cmluZ30gSGFzaFxyXG4gKi9cclxuTG9uZ0JpdHMucHJvdG90eXBlLnRvSGFzaCA9IGZ1bmN0aW9uIHRvSGFzaCgpIHtcclxuICAgIHJldHVybiBTdHJpbmcuZnJvbUNoYXJDb2RlKFxyXG4gICAgICAgIHRoaXMubG8gICAgICAgICYgMjU1LFxyXG4gICAgICAgIHRoaXMubG8gPj4+IDggICYgMjU1LFxyXG4gICAgICAgIHRoaXMubG8gPj4+IDE2ICYgMjU1LFxyXG4gICAgICAgIHRoaXMubG8gPj4+IDI0ICAgICAgLFxyXG4gICAgICAgIHRoaXMuaGkgICAgICAgICYgMjU1LFxyXG4gICAgICAgIHRoaXMuaGkgPj4+IDggICYgMjU1LFxyXG4gICAgICAgIHRoaXMuaGkgPj4+IDE2ICYgMjU1LFxyXG4gICAgICAgIHRoaXMuaGkgPj4+IDI0XHJcbiAgICApO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIFppZy16YWcgZW5jb2RlcyB0aGlzIGxvbmcgYml0cy5cclxuICogQHJldHVybnMge3V0aWwuTG9uZ0JpdHN9IGB0aGlzYFxyXG4gKi9cclxuTG9uZ0JpdHMucHJvdG90eXBlLnp6RW5jb2RlID0gZnVuY3Rpb24genpFbmNvZGUoKSB7XHJcbiAgICB2YXIgbWFzayA9ICAgdGhpcy5oaSA+PiAzMTtcclxuICAgIHRoaXMuaGkgID0gKCh0aGlzLmhpIDw8IDEgfCB0aGlzLmxvID4+PiAzMSkgXiBtYXNrKSA+Pj4gMDtcclxuICAgIHRoaXMubG8gID0gKCB0aGlzLmxvIDw8IDEgICAgICAgICAgICAgICAgICAgXiBtYXNrKSA+Pj4gMDtcclxuICAgIHJldHVybiB0aGlzO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIFppZy16YWcgZGVjb2RlcyB0aGlzIGxvbmcgYml0cy5cclxuICogQHJldHVybnMge3V0aWwuTG9uZ0JpdHN9IGB0aGlzYFxyXG4gKi9cclxuTG9uZ0JpdHMucHJvdG90eXBlLnp6RGVjb2RlID0gZnVuY3Rpb24genpEZWNvZGUoKSB7XHJcbiAgICB2YXIgbWFzayA9IC0odGhpcy5sbyAmIDEpO1xyXG4gICAgdGhpcy5sbyAgPSAoKHRoaXMubG8gPj4+IDEgfCB0aGlzLmhpIDw8IDMxKSBeIG1hc2spID4+PiAwO1xyXG4gICAgdGhpcy5oaSAgPSAoIHRoaXMuaGkgPj4+IDEgICAgICAgICAgICAgICAgICBeIG1hc2spID4+PiAwO1xyXG4gICAgcmV0dXJuIHRoaXM7XHJcbn07XHJcblxyXG4vKipcclxuICogQ2FsY3VsYXRlcyB0aGUgbGVuZ3RoIG9mIHRoaXMgbG9uZ2JpdHMgd2hlbiBlbmNvZGVkIGFzIGEgdmFyaW50LlxyXG4gKiBAcmV0dXJucyB7bnVtYmVyfSBMZW5ndGhcclxuICovXHJcbkxvbmdCaXRzLnByb3RvdHlwZS5sZW5ndGggPSBmdW5jdGlvbiBsZW5ndGgoKSB7XHJcbiAgICB2YXIgcGFydDAgPSAgdGhpcy5sbyxcclxuICAgICAgICBwYXJ0MSA9ICh0aGlzLmxvID4+PiAyOCB8IHRoaXMuaGkgPDwgNCkgPj4+IDAsXHJcbiAgICAgICAgcGFydDIgPSAgdGhpcy5oaSA+Pj4gMjQ7XHJcbiAgICByZXR1cm4gcGFydDIgPT09IDBcclxuICAgICAgICAgPyBwYXJ0MSA9PT0gMFxyXG4gICAgICAgICAgID8gcGFydDAgPCAxNjM4NFxyXG4gICAgICAgICAgICAgPyBwYXJ0MCA8IDEyOCA/IDEgOiAyXHJcbiAgICAgICAgICAgICA6IHBhcnQwIDwgMjA5NzE1MiA/IDMgOiA0XHJcbiAgICAgICAgICAgOiBwYXJ0MSA8IDE2Mzg0XHJcbiAgICAgICAgICAgICA/IHBhcnQxIDwgMTI4ID8gNSA6IDZcclxuICAgICAgICAgICAgIDogcGFydDEgPCAyMDk3MTUyID8gNyA6IDhcclxuICAgICAgICAgOiBwYXJ0MiA8IDEyOCA/IDkgOiAxMDtcclxufTtcclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbnZhciB1dGlsID0gZXhwb3J0cztcclxuXHJcbi8vIHVzZWQgdG8gcmV0dXJuIGEgUHJvbWlzZSB3aGVyZSBjYWxsYmFjayBpcyBvbWl0dGVkXHJcbnV0aWwuYXNQcm9taXNlID0gcmVxdWlyZShcIkBwcm90b2J1ZmpzL2FzcHJvbWlzZVwiKTtcclxuXHJcbi8vIGNvbnZlcnRzIHRvIC8gZnJvbSBiYXNlNjQgZW5jb2RlZCBzdHJpbmdzXHJcbnV0aWwuYmFzZTY0ID0gcmVxdWlyZShcIkBwcm90b2J1ZmpzL2Jhc2U2NFwiKTtcclxuXHJcbi8vIGJhc2UgY2xhc3Mgb2YgcnBjLlNlcnZpY2VcclxudXRpbC5FdmVudEVtaXR0ZXIgPSByZXF1aXJlKFwiQHByb3RvYnVmanMvZXZlbnRlbWl0dGVyXCIpO1xyXG5cclxuLy8gZmxvYXQgaGFuZGxpbmcgYWNjcm9zcyBicm93c2Vyc1xyXG51dGlsLmZsb2F0ID0gcmVxdWlyZShcIkBwcm90b2J1ZmpzL2Zsb2F0XCIpO1xyXG5cclxuLy8gcmVxdWlyZXMgbW9kdWxlcyBvcHRpb25hbGx5IGFuZCBoaWRlcyB0aGUgY2FsbCBmcm9tIGJ1bmRsZXJzXHJcbnV0aWwuaW5xdWlyZSA9IHJlcXVpcmUoXCJAcHJvdG9idWZqcy9pbnF1aXJlXCIpO1xyXG5cclxuLy8gY29udmVydHMgdG8gLyBmcm9tIHV0ZjggZW5jb2RlZCBzdHJpbmdzXHJcbnV0aWwudXRmOCA9IHJlcXVpcmUoXCJAcHJvdG9idWZqcy91dGY4XCIpO1xyXG5cclxuLy8gcHJvdmlkZXMgYSBub2RlLWxpa2UgYnVmZmVyIHBvb2wgaW4gdGhlIGJyb3dzZXJcclxudXRpbC5wb29sID0gcmVxdWlyZShcIkBwcm90b2J1ZmpzL3Bvb2xcIik7XHJcblxyXG4vLyB1dGlsaXR5IHRvIHdvcmsgd2l0aCB0aGUgbG93IGFuZCBoaWdoIGJpdHMgb2YgYSA2NCBiaXQgdmFsdWVcclxudXRpbC5Mb25nQml0cyA9IHJlcXVpcmUoXCIuL2xvbmdiaXRzXCIpO1xyXG5cclxuLyoqXHJcbiAqIFdoZXRoZXIgcnVubmluZyB3aXRoaW4gbm9kZSBvciBub3QuXHJcbiAqIEBtZW1iZXJvZiB1dGlsXHJcbiAqIEB0eXBlIHtib29sZWFufVxyXG4gKi9cclxudXRpbC5pc05vZGUgPSBCb29sZWFuKHR5cGVvZiBnbG9iYWwgIT09IFwidW5kZWZpbmVkXCJcclxuICAgICAgICAgICAgICAgICAgICYmIGdsb2JhbFxyXG4gICAgICAgICAgICAgICAgICAgJiYgZ2xvYmFsLnByb2Nlc3NcclxuICAgICAgICAgICAgICAgICAgICYmIGdsb2JhbC5wcm9jZXNzLnZlcnNpb25zXHJcbiAgICAgICAgICAgICAgICAgICAmJiBnbG9iYWwucHJvY2Vzcy52ZXJzaW9ucy5ub2RlKTtcclxuXHJcbi8qKlxyXG4gKiBHbG9iYWwgb2JqZWN0IHJlZmVyZW5jZS5cclxuICogQG1lbWJlcm9mIHV0aWxcclxuICogQHR5cGUge09iamVjdH1cclxuICovXHJcbnV0aWwuZ2xvYmFsID0gdXRpbC5pc05vZGUgJiYgZ2xvYmFsXHJcbiAgICAgICAgICAgfHwgdHlwZW9mIHdpbmRvdyAhPT0gXCJ1bmRlZmluZWRcIiAmJiB3aW5kb3dcclxuICAgICAgICAgICB8fCB0eXBlb2Ygc2VsZiAgICE9PSBcInVuZGVmaW5lZFwiICYmIHNlbGZcclxuICAgICAgICAgICB8fCB0aGlzOyAvLyBlc2xpbnQtZGlzYWJsZS1saW5lIG5vLWludmFsaWQtdGhpc1xyXG5cclxuLyoqXHJcbiAqIEFuIGltbXVhYmxlIGVtcHR5IGFycmF5LlxyXG4gKiBAbWVtYmVyb2YgdXRpbFxyXG4gKiBAdHlwZSB7QXJyYXkuPCo+fVxyXG4gKiBAY29uc3RcclxuICovXHJcbnV0aWwuZW1wdHlBcnJheSA9IE9iamVjdC5mcmVlemUgPyBPYmplY3QuZnJlZXplKFtdKSA6IC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0ICovIFtdOyAvLyB1c2VkIG9uIHByb3RvdHlwZXNcclxuXHJcbi8qKlxyXG4gKiBBbiBpbW11dGFibGUgZW1wdHkgb2JqZWN0LlxyXG4gKiBAdHlwZSB7T2JqZWN0fVxyXG4gKiBAY29uc3RcclxuICovXHJcbnV0aWwuZW1wdHlPYmplY3QgPSBPYmplY3QuZnJlZXplID8gT2JqZWN0LmZyZWV6ZSh7fSkgOiAvKiBpc3RhbmJ1bCBpZ25vcmUgbmV4dCAqLyB7fTsgLy8gdXNlZCBvbiBwcm90b3R5cGVzXHJcblxyXG4vKipcclxuICogVGVzdHMgaWYgdGhlIHNwZWNpZmllZCB2YWx1ZSBpcyBhbiBpbnRlZ2VyLlxyXG4gKiBAZnVuY3Rpb25cclxuICogQHBhcmFtIHsqfSB2YWx1ZSBWYWx1ZSB0byB0ZXN0XHJcbiAqIEByZXR1cm5zIHtib29sZWFufSBgdHJ1ZWAgaWYgdGhlIHZhbHVlIGlzIGFuIGludGVnZXJcclxuICovXHJcbnV0aWwuaXNJbnRlZ2VyID0gTnVtYmVyLmlzSW50ZWdlciB8fCAvKiBpc3RhbmJ1bCBpZ25vcmUgbmV4dCAqLyBmdW5jdGlvbiBpc0ludGVnZXIodmFsdWUpIHtcclxuICAgIHJldHVybiB0eXBlb2YgdmFsdWUgPT09IFwibnVtYmVyXCIgJiYgaXNGaW5pdGUodmFsdWUpICYmIE1hdGguZmxvb3IodmFsdWUpID09PSB2YWx1ZTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBUZXN0cyBpZiB0aGUgc3BlY2lmaWVkIHZhbHVlIGlzIGEgc3RyaW5nLlxyXG4gKiBAcGFyYW0geyp9IHZhbHVlIFZhbHVlIHRvIHRlc3RcclxuICogQHJldHVybnMge2Jvb2xlYW59IGB0cnVlYCBpZiB0aGUgdmFsdWUgaXMgYSBzdHJpbmdcclxuICovXHJcbnV0aWwuaXNTdHJpbmcgPSBmdW5jdGlvbiBpc1N0cmluZyh2YWx1ZSkge1xyXG4gICAgcmV0dXJuIHR5cGVvZiB2YWx1ZSA9PT0gXCJzdHJpbmdcIiB8fCB2YWx1ZSBpbnN0YW5jZW9mIFN0cmluZztcclxufTtcclxuXHJcbi8qKlxyXG4gKiBUZXN0cyBpZiB0aGUgc3BlY2lmaWVkIHZhbHVlIGlzIGEgbm9uLW51bGwgb2JqZWN0LlxyXG4gKiBAcGFyYW0geyp9IHZhbHVlIFZhbHVlIHRvIHRlc3RcclxuICogQHJldHVybnMge2Jvb2xlYW59IGB0cnVlYCBpZiB0aGUgdmFsdWUgaXMgYSBub24tbnVsbCBvYmplY3RcclxuICovXHJcbnV0aWwuaXNPYmplY3QgPSBmdW5jdGlvbiBpc09iamVjdCh2YWx1ZSkge1xyXG4gICAgcmV0dXJuIHZhbHVlICYmIHR5cGVvZiB2YWx1ZSA9PT0gXCJvYmplY3RcIjtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBDaGVja3MgaWYgYSBwcm9wZXJ0eSBvbiBhIG1lc3NhZ2UgaXMgY29uc2lkZXJlZCB0byBiZSBwcmVzZW50LlxyXG4gKiBUaGlzIGlzIGFuIGFsaWFzIG9mIHtAbGluayB1dGlsLmlzU2V0fS5cclxuICogQGZ1bmN0aW9uXHJcbiAqIEBwYXJhbSB7T2JqZWN0fSBvYmogUGxhaW4gb2JqZWN0IG9yIG1lc3NhZ2UgaW5zdGFuY2VcclxuICogQHBhcmFtIHtzdHJpbmd9IHByb3AgUHJvcGVydHkgbmFtZVxyXG4gKiBAcmV0dXJucyB7Ym9vbGVhbn0gYHRydWVgIGlmIGNvbnNpZGVyZWQgdG8gYmUgcHJlc2VudCwgb3RoZXJ3aXNlIGBmYWxzZWBcclxuICovXHJcbnV0aWwuaXNzZXQgPVxyXG5cclxuLyoqXHJcbiAqIENoZWNrcyBpZiBhIHByb3BlcnR5IG9uIGEgbWVzc2FnZSBpcyBjb25zaWRlcmVkIHRvIGJlIHByZXNlbnQuXHJcbiAqIEBwYXJhbSB7T2JqZWN0fSBvYmogUGxhaW4gb2JqZWN0IG9yIG1lc3NhZ2UgaW5zdGFuY2VcclxuICogQHBhcmFtIHtzdHJpbmd9IHByb3AgUHJvcGVydHkgbmFtZVxyXG4gKiBAcmV0dXJucyB7Ym9vbGVhbn0gYHRydWVgIGlmIGNvbnNpZGVyZWQgdG8gYmUgcHJlc2VudCwgb3RoZXJ3aXNlIGBmYWxzZWBcclxuICovXHJcbnV0aWwuaXNTZXQgPSBmdW5jdGlvbiBpc1NldChvYmosIHByb3ApIHtcclxuICAgIHZhciB2YWx1ZSA9IG9ialtwcm9wXTtcclxuICAgIGlmICh2YWx1ZSAhPSBudWxsICYmIG9iai5oYXNPd25Qcm9wZXJ0eShwcm9wKSkgLy8gZXNsaW50LWRpc2FibGUtbGluZSBlcWVxZXEsIG5vLXByb3RvdHlwZS1idWlsdGluc1xyXG4gICAgICAgIHJldHVybiB0eXBlb2YgdmFsdWUgIT09IFwib2JqZWN0XCIgfHwgKEFycmF5LmlzQXJyYXkodmFsdWUpID8gdmFsdWUubGVuZ3RoIDogT2JqZWN0LmtleXModmFsdWUpLmxlbmd0aCkgPiAwO1xyXG4gICAgcmV0dXJuIGZhbHNlO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIEFueSBjb21wYXRpYmxlIEJ1ZmZlciBpbnN0YW5jZS5cclxuICogVGhpcyBpcyBhIG1pbmltYWwgc3RhbmQtYWxvbmUgZGVmaW5pdGlvbiBvZiBhIEJ1ZmZlciBpbnN0YW5jZS4gVGhlIGFjdHVhbCB0eXBlIGlzIHRoYXQgZXhwb3J0ZWQgYnkgbm9kZSdzIHR5cGluZ3MuXHJcbiAqIEBpbnRlcmZhY2UgQnVmZmVyXHJcbiAqIEBleHRlbmRzIFVpbnQ4QXJyYXlcclxuICovXHJcblxyXG4vKipcclxuICogTm9kZSdzIEJ1ZmZlciBjbGFzcyBpZiBhdmFpbGFibGUuXHJcbiAqIEB0eXBlIHtDb25zdHJ1Y3RvcjxCdWZmZXI+fVxyXG4gKi9cclxudXRpbC5CdWZmZXIgPSAoZnVuY3Rpb24oKSB7XHJcbiAgICB0cnkge1xyXG4gICAgICAgIHZhciBCdWZmZXIgPSB1dGlsLmlucXVpcmUoXCJidWZmZXJcIikuQnVmZmVyO1xyXG4gICAgICAgIC8vIHJlZnVzZSB0byB1c2Ugbm9uLW5vZGUgYnVmZmVycyBpZiBub3QgZXhwbGljaXRseSBhc3NpZ25lZCAocGVyZiByZWFzb25zKTpcclxuICAgICAgICByZXR1cm4gQnVmZmVyLnByb3RvdHlwZS51dGY4V3JpdGUgPyBCdWZmZXIgOiAvKiBpc3RhbmJ1bCBpZ25vcmUgbmV4dCAqLyBudWxsO1xyXG4gICAgfSBjYXRjaCAoZSkge1xyXG4gICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0ICovXHJcbiAgICAgICAgcmV0dXJuIG51bGw7XHJcbiAgICB9XHJcbn0pKCk7XHJcblxyXG4vLyBJbnRlcm5hbCBhbGlhcyBvZiBvciBwb2x5ZnVsbCBmb3IgQnVmZmVyLmZyb20uXHJcbnV0aWwuX0J1ZmZlcl9mcm9tID0gbnVsbDtcclxuXHJcbi8vIEludGVybmFsIGFsaWFzIG9mIG9yIHBvbHlmaWxsIGZvciBCdWZmZXIuYWxsb2NVbnNhZmUuXHJcbnV0aWwuX0J1ZmZlcl9hbGxvY1Vuc2FmZSA9IG51bGw7XHJcblxyXG4vKipcclxuICogQ3JlYXRlcyBhIG5ldyBidWZmZXIgb2Ygd2hhdGV2ZXIgdHlwZSBzdXBwb3J0ZWQgYnkgdGhlIGVudmlyb25tZW50LlxyXG4gKiBAcGFyYW0ge251bWJlcnxudW1iZXJbXX0gW3NpemVPckFycmF5PTBdIEJ1ZmZlciBzaXplIG9yIG51bWJlciBhcnJheVxyXG4gKiBAcmV0dXJucyB7VWludDhBcnJheXxCdWZmZXJ9IEJ1ZmZlclxyXG4gKi9cclxudXRpbC5uZXdCdWZmZXIgPSBmdW5jdGlvbiBuZXdCdWZmZXIoc2l6ZU9yQXJyYXkpIHtcclxuICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0ICovXHJcbiAgICByZXR1cm4gdHlwZW9mIHNpemVPckFycmF5ID09PSBcIm51bWJlclwiXHJcbiAgICAgICAgPyB1dGlsLkJ1ZmZlclxyXG4gICAgICAgICAgICA/IHV0aWwuX0J1ZmZlcl9hbGxvY1Vuc2FmZShzaXplT3JBcnJheSlcclxuICAgICAgICAgICAgOiBuZXcgdXRpbC5BcnJheShzaXplT3JBcnJheSlcclxuICAgICAgICA6IHV0aWwuQnVmZmVyXHJcbiAgICAgICAgICAgID8gdXRpbC5fQnVmZmVyX2Zyb20oc2l6ZU9yQXJyYXkpXHJcbiAgICAgICAgICAgIDogdHlwZW9mIFVpbnQ4QXJyYXkgPT09IFwidW5kZWZpbmVkXCJcclxuICAgICAgICAgICAgICAgID8gc2l6ZU9yQXJyYXlcclxuICAgICAgICAgICAgICAgIDogbmV3IFVpbnQ4QXJyYXkoc2l6ZU9yQXJyYXkpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIEFycmF5IGltcGxlbWVudGF0aW9uIHVzZWQgaW4gdGhlIGJyb3dzZXIuIGBVaW50OEFycmF5YCBpZiBzdXBwb3J0ZWQsIG90aGVyd2lzZSBgQXJyYXlgLlxyXG4gKiBAdHlwZSB7Q29uc3RydWN0b3I8VWludDhBcnJheT59XHJcbiAqL1xyXG51dGlsLkFycmF5ID0gdHlwZW9mIFVpbnQ4QXJyYXkgIT09IFwidW5kZWZpbmVkXCIgPyBVaW50OEFycmF5IC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0ICovIDogQXJyYXk7XHJcblxyXG4vKipcclxuICogQW55IGNvbXBhdGlibGUgTG9uZyBpbnN0YW5jZS5cclxuICogVGhpcyBpcyBhIG1pbmltYWwgc3RhbmQtYWxvbmUgZGVmaW5pdGlvbiBvZiBhIExvbmcgaW5zdGFuY2UuIFRoZSBhY3R1YWwgdHlwZSBpcyB0aGF0IGV4cG9ydGVkIGJ5IGxvbmcuanMuXHJcbiAqIEBpbnRlcmZhY2UgTG9uZ1xyXG4gKiBAcHJvcGVydHkge251bWJlcn0gbG93IExvdyBiaXRzXHJcbiAqIEBwcm9wZXJ0eSB7bnVtYmVyfSBoaWdoIEhpZ2ggYml0c1xyXG4gKiBAcHJvcGVydHkge2Jvb2xlYW59IHVuc2lnbmVkIFdoZXRoZXIgdW5zaWduZWQgb3Igbm90XHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIExvbmcuanMncyBMb25nIGNsYXNzIGlmIGF2YWlsYWJsZS5cclxuICogQHR5cGUge0NvbnN0cnVjdG9yPExvbmc+fVxyXG4gKi9cclxudXRpbC5Mb25nID0gLyogaXN0YW5idWwgaWdub3JlIG5leHQgKi8gdXRpbC5nbG9iYWwuZGNvZGVJTyAmJiAvKiBpc3RhbmJ1bCBpZ25vcmUgbmV4dCAqLyB1dGlsLmdsb2JhbC5kY29kZUlPLkxvbmdcclxuICAgICAgICAgfHwgLyogaXN0YW5idWwgaWdub3JlIG5leHQgKi8gdXRpbC5nbG9iYWwuTG9uZ1xyXG4gICAgICAgICB8fCB1dGlsLmlucXVpcmUoXCJsb25nXCIpO1xyXG5cclxuLyoqXHJcbiAqIFJlZ3VsYXIgZXhwcmVzc2lvbiB1c2VkIHRvIHZlcmlmeSAyIGJpdCAoYGJvb2xgKSBtYXAga2V5cy5cclxuICogQHR5cGUge1JlZ0V4cH1cclxuICogQGNvbnN0XHJcbiAqL1xyXG51dGlsLmtleTJSZSA9IC9edHJ1ZXxmYWxzZXwwfDEkLztcclxuXHJcbi8qKlxyXG4gKiBSZWd1bGFyIGV4cHJlc3Npb24gdXNlZCB0byB2ZXJpZnkgMzIgYml0IChgaW50MzJgIGV0Yy4pIG1hcCBrZXlzLlxyXG4gKiBAdHlwZSB7UmVnRXhwfVxyXG4gKiBAY29uc3RcclxuICovXHJcbnV0aWwua2V5MzJSZSA9IC9eLT8oPzowfFsxLTldWzAtOV0qKSQvO1xyXG5cclxuLyoqXHJcbiAqIFJlZ3VsYXIgZXhwcmVzc2lvbiB1c2VkIHRvIHZlcmlmeSA2NCBiaXQgKGBpbnQ2NGAgZXRjLikgbWFwIGtleXMuXHJcbiAqIEB0eXBlIHtSZWdFeHB9XHJcbiAqIEBjb25zdFxyXG4gKi9cclxudXRpbC5rZXk2NFJlID0gL14oPzpbXFxcXHgwMC1cXFxceGZmXXs4fXwtPyg/OjB8WzEtOV1bMC05XSopKSQvO1xyXG5cclxuLyoqXHJcbiAqIENvbnZlcnRzIGEgbnVtYmVyIG9yIGxvbmcgdG8gYW4gOCBjaGFyYWN0ZXJzIGxvbmcgaGFzaCBzdHJpbmcuXHJcbiAqIEBwYXJhbSB7TG9uZ3xudW1iZXJ9IHZhbHVlIFZhbHVlIHRvIGNvbnZlcnRcclxuICogQHJldHVybnMge3N0cmluZ30gSGFzaFxyXG4gKi9cclxudXRpbC5sb25nVG9IYXNoID0gZnVuY3Rpb24gbG9uZ1RvSGFzaCh2YWx1ZSkge1xyXG4gICAgcmV0dXJuIHZhbHVlXHJcbiAgICAgICAgPyB1dGlsLkxvbmdCaXRzLmZyb20odmFsdWUpLnRvSGFzaCgpXHJcbiAgICAgICAgOiB1dGlsLkxvbmdCaXRzLnplcm9IYXNoO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIENvbnZlcnRzIGFuIDggY2hhcmFjdGVycyBsb25nIGhhc2ggc3RyaW5nIHRvIGEgbG9uZyBvciBudW1iZXIuXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBoYXNoIEhhc2hcclxuICogQHBhcmFtIHtib29sZWFufSBbdW5zaWduZWQ9ZmFsc2VdIFdoZXRoZXIgdW5zaWduZWQgb3Igbm90XHJcbiAqIEByZXR1cm5zIHtMb25nfG51bWJlcn0gT3JpZ2luYWwgdmFsdWVcclxuICovXHJcbnV0aWwubG9uZ0Zyb21IYXNoID0gZnVuY3Rpb24gbG9uZ0Zyb21IYXNoKGhhc2gsIHVuc2lnbmVkKSB7XHJcbiAgICB2YXIgYml0cyA9IHV0aWwuTG9uZ0JpdHMuZnJvbUhhc2goaGFzaCk7XHJcbiAgICBpZiAodXRpbC5Mb25nKVxyXG4gICAgICAgIHJldHVybiB1dGlsLkxvbmcuZnJvbUJpdHMoYml0cy5sbywgYml0cy5oaSwgdW5zaWduZWQpO1xyXG4gICAgcmV0dXJuIGJpdHMudG9OdW1iZXIoQm9vbGVhbih1bnNpZ25lZCkpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIE1lcmdlcyB0aGUgcHJvcGVydGllcyBvZiB0aGUgc291cmNlIG9iamVjdCBpbnRvIHRoZSBkZXN0aW5hdGlvbiBvYmplY3QuXHJcbiAqIEBtZW1iZXJvZiB1dGlsXHJcbiAqIEBwYXJhbSB7T2JqZWN0LjxzdHJpbmcsKj59IGRzdCBEZXN0aW5hdGlvbiBvYmplY3RcclxuICogQHBhcmFtIHtPYmplY3QuPHN0cmluZywqPn0gc3JjIFNvdXJjZSBvYmplY3RcclxuICogQHBhcmFtIHtib29sZWFufSBbaWZOb3RTZXQ9ZmFsc2VdIE1lcmdlcyBvbmx5IGlmIHRoZSBrZXkgaXMgbm90IGFscmVhZHkgc2V0XHJcbiAqIEByZXR1cm5zIHtPYmplY3QuPHN0cmluZywqPn0gRGVzdGluYXRpb24gb2JqZWN0XHJcbiAqL1xyXG5mdW5jdGlvbiBtZXJnZShkc3QsIHNyYywgaWZOb3RTZXQpIHsgLy8gdXNlZCBieSBjb252ZXJ0ZXJzXHJcbiAgICBmb3IgKHZhciBrZXlzID0gT2JqZWN0LmtleXMoc3JjKSwgaSA9IDA7IGkgPCBrZXlzLmxlbmd0aDsgKytpKVxyXG4gICAgICAgIGlmIChkc3Rba2V5c1tpXV0gPT09IHVuZGVmaW5lZCB8fCAhaWZOb3RTZXQpXHJcbiAgICAgICAgICAgIGRzdFtrZXlzW2ldXSA9IHNyY1trZXlzW2ldXTtcclxuICAgIHJldHVybiBkc3Q7XHJcbn1cclxuXHJcbnV0aWwubWVyZ2UgPSBtZXJnZTtcclxuXHJcbi8qKlxyXG4gKiBDb252ZXJ0cyB0aGUgZmlyc3QgY2hhcmFjdGVyIG9mIGEgc3RyaW5nIHRvIGxvd2VyIGNhc2UuXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBzdHIgU3RyaW5nIHRvIGNvbnZlcnRcclxuICogQHJldHVybnMge3N0cmluZ30gQ29udmVydGVkIHN0cmluZ1xyXG4gKi9cclxudXRpbC5sY0ZpcnN0ID0gZnVuY3Rpb24gbGNGaXJzdChzdHIpIHtcclxuICAgIHJldHVybiBzdHIuY2hhckF0KDApLnRvTG93ZXJDYXNlKCkgKyBzdHIuc3Vic3RyaW5nKDEpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIENyZWF0ZXMgYSBjdXN0b20gZXJyb3IgY29uc3RydWN0b3IuXHJcbiAqIEBtZW1iZXJvZiB1dGlsXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBuYW1lIEVycm9yIG5hbWVcclxuICogQHJldHVybnMge0NvbnN0cnVjdG9yPEVycm9yPn0gQ3VzdG9tIGVycm9yIGNvbnN0cnVjdG9yXHJcbiAqL1xyXG5mdW5jdGlvbiBuZXdFcnJvcihuYW1lKSB7XHJcblxyXG4gICAgZnVuY3Rpb24gQ3VzdG9tRXJyb3IobWVzc2FnZSwgcHJvcGVydGllcykge1xyXG5cclxuICAgICAgICBpZiAoISh0aGlzIGluc3RhbmNlb2YgQ3VzdG9tRXJyb3IpKVxyXG4gICAgICAgICAgICByZXR1cm4gbmV3IEN1c3RvbUVycm9yKG1lc3NhZ2UsIHByb3BlcnRpZXMpO1xyXG5cclxuICAgICAgICAvLyBFcnJvci5jYWxsKHRoaXMsIG1lc3NhZ2UpO1xyXG4gICAgICAgIC8vIF4ganVzdCByZXR1cm5zIGEgbmV3IGVycm9yIGluc3RhbmNlIGJlY2F1c2UgdGhlIGN0b3IgY2FuIGJlIGNhbGxlZCBhcyBhIGZ1bmN0aW9uXHJcblxyXG4gICAgICAgIE9iamVjdC5kZWZpbmVQcm9wZXJ0eSh0aGlzLCBcIm1lc3NhZ2VcIiwgeyBnZXQ6IGZ1bmN0aW9uKCkgeyByZXR1cm4gbWVzc2FnZTsgfSB9KTtcclxuXHJcbiAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIG5leHQgKi9cclxuICAgICAgICBpZiAoRXJyb3IuY2FwdHVyZVN0YWNrVHJhY2UpIC8vIG5vZGVcclxuICAgICAgICAgICAgRXJyb3IuY2FwdHVyZVN0YWNrVHJhY2UodGhpcywgQ3VzdG9tRXJyb3IpO1xyXG4gICAgICAgIGVsc2VcclxuICAgICAgICAgICAgT2JqZWN0LmRlZmluZVByb3BlcnR5KHRoaXMsIFwic3RhY2tcIiwgeyB2YWx1ZTogbmV3IEVycm9yKCkuc3RhY2sgfHwgXCJcIiB9KTtcclxuXHJcbiAgICAgICAgaWYgKHByb3BlcnRpZXMpXHJcbiAgICAgICAgICAgIG1lcmdlKHRoaXMsIHByb3BlcnRpZXMpO1xyXG4gICAgfVxyXG5cclxuICAgIEN1c3RvbUVycm9yLnByb3RvdHlwZSA9IE9iamVjdC5jcmVhdGUoRXJyb3IucHJvdG90eXBlLCB7XHJcbiAgICAgICAgY29uc3RydWN0b3I6IHtcclxuICAgICAgICAgICAgdmFsdWU6IEN1c3RvbUVycm9yLFxyXG4gICAgICAgICAgICB3cml0YWJsZTogdHJ1ZSxcclxuICAgICAgICAgICAgZW51bWVyYWJsZTogZmFsc2UsXHJcbiAgICAgICAgICAgIGNvbmZpZ3VyYWJsZTogdHJ1ZSxcclxuICAgICAgICB9LFxyXG4gICAgICAgIG5hbWU6IHtcclxuICAgICAgICAgICAgZ2V0OiBmdW5jdGlvbiBnZXQoKSB7IHJldHVybiBuYW1lOyB9LFxyXG4gICAgICAgICAgICBzZXQ6IHVuZGVmaW5lZCxcclxuICAgICAgICAgICAgZW51bWVyYWJsZTogZmFsc2UsXHJcbiAgICAgICAgICAgIC8vIGNvbmZpZ3VyYWJsZTogZmFsc2Ugd291bGQgYWNjdXJhdGVseSBwcmVzZXJ2ZSB0aGUgYmVoYXZpb3Igb2ZcclxuICAgICAgICAgICAgLy8gdGhlIG9yaWdpbmFsLCBidXQgSSdtIGd1ZXNzaW5nIHRoYXQgd2FzIG5vdCBpbnRlbnRpb25hbC5cclxuICAgICAgICAgICAgLy8gRm9yIGFuIGFjdHVhbCBlcnJvciBzdWJjbGFzcywgdGhpcyBwcm9wZXJ0eSB3b3VsZFxyXG4gICAgICAgICAgICAvLyBiZSBjb25maWd1cmFibGUuXHJcbiAgICAgICAgICAgIGNvbmZpZ3VyYWJsZTogdHJ1ZSxcclxuICAgICAgICB9LFxyXG4gICAgICAgIHRvU3RyaW5nOiB7XHJcbiAgICAgICAgICAgIHZhbHVlOiBmdW5jdGlvbiB2YWx1ZSgpIHsgcmV0dXJuIHRoaXMubmFtZSArIFwiOiBcIiArIHRoaXMubWVzc2FnZTsgfSxcclxuICAgICAgICAgICAgd3JpdGFibGU6IHRydWUsXHJcbiAgICAgICAgICAgIGVudW1lcmFibGU6IGZhbHNlLFxyXG4gICAgICAgICAgICBjb25maWd1cmFibGU6IHRydWUsXHJcbiAgICAgICAgfSxcclxuICAgIH0pO1xyXG5cclxuICAgIHJldHVybiBDdXN0b21FcnJvcjtcclxufVxyXG5cclxudXRpbC5uZXdFcnJvciA9IG5ld0Vycm9yO1xyXG5cclxuLyoqXHJcbiAqIENvbnN0cnVjdHMgYSBuZXcgcHJvdG9jb2wgZXJyb3IuXHJcbiAqIEBjbGFzc2Rlc2MgRXJyb3Igc3ViY2xhc3MgaW5kaWNhdGluZyBhIHByb3RvY29sIHNwZWNpZmMgZXJyb3IuXHJcbiAqIEBtZW1iZXJvZiB1dGlsXHJcbiAqIEBleHRlbmRzIEVycm9yXHJcbiAqIEB0ZW1wbGF0ZSBUIGV4dGVuZHMgTWVzc2FnZTxUPlxyXG4gKiBAY29uc3RydWN0b3JcclxuICogQHBhcmFtIHtzdHJpbmd9IG1lc3NhZ2UgRXJyb3IgbWVzc2FnZVxyXG4gKiBAcGFyYW0ge09iamVjdC48c3RyaW5nLCo+fSBbcHJvcGVydGllc10gQWRkaXRpb25hbCBwcm9wZXJ0aWVzXHJcbiAqIEBleGFtcGxlXHJcbiAqIHRyeSB7XHJcbiAqICAgICBNeU1lc3NhZ2UuZGVjb2RlKHNvbWVCdWZmZXIpOyAvLyB0aHJvd3MgaWYgcmVxdWlyZWQgZmllbGRzIGFyZSBtaXNzaW5nXHJcbiAqIH0gY2F0Y2ggKGUpIHtcclxuICogICAgIGlmIChlIGluc3RhbmNlb2YgUHJvdG9jb2xFcnJvciAmJiBlLmluc3RhbmNlKVxyXG4gKiAgICAgICAgIGNvbnNvbGUubG9nKFwiZGVjb2RlZCBzbyBmYXI6IFwiICsgSlNPTi5zdHJpbmdpZnkoZS5pbnN0YW5jZSkpO1xyXG4gKiB9XHJcbiAqL1xyXG51dGlsLlByb3RvY29sRXJyb3IgPSBuZXdFcnJvcihcIlByb3RvY29sRXJyb3JcIik7XHJcblxyXG4vKipcclxuICogU28gZmFyIGRlY29kZWQgbWVzc2FnZSBpbnN0YW5jZS5cclxuICogQG5hbWUgdXRpbC5Qcm90b2NvbEVycm9yI2luc3RhbmNlXHJcbiAqIEB0eXBlIHtNZXNzYWdlPFQ+fVxyXG4gKi9cclxuXHJcbi8qKlxyXG4gKiBBIE9uZU9mIGdldHRlciBhcyByZXR1cm5lZCBieSB7QGxpbmsgdXRpbC5vbmVPZkdldHRlcn0uXHJcbiAqIEB0eXBlZGVmIE9uZU9mR2V0dGVyXHJcbiAqIEB0eXBlIHtmdW5jdGlvbn1cclxuICogQHJldHVybnMge3N0cmluZ3x1bmRlZmluZWR9IFNldCBmaWVsZCBuYW1lLCBpZiBhbnlcclxuICovXHJcblxyXG4vKipcclxuICogQnVpbGRzIGEgZ2V0dGVyIGZvciBhIG9uZW9mJ3MgcHJlc2VudCBmaWVsZCBuYW1lLlxyXG4gKiBAcGFyYW0ge3N0cmluZ1tdfSBmaWVsZE5hbWVzIEZpZWxkIG5hbWVzXHJcbiAqIEByZXR1cm5zIHtPbmVPZkdldHRlcn0gVW5ib3VuZCBnZXR0ZXJcclxuICovXHJcbnV0aWwub25lT2ZHZXR0ZXIgPSBmdW5jdGlvbiBnZXRPbmVPZihmaWVsZE5hbWVzKSB7XHJcbiAgICB2YXIgZmllbGRNYXAgPSB7fTtcclxuICAgIGZvciAodmFyIGkgPSAwOyBpIDwgZmllbGROYW1lcy5sZW5ndGg7ICsraSlcclxuICAgICAgICBmaWVsZE1hcFtmaWVsZE5hbWVzW2ldXSA9IDE7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBAcmV0dXJucyB7c3RyaW5nfHVuZGVmaW5lZH0gU2V0IGZpZWxkIG5hbWUsIGlmIGFueVxyXG4gICAgICogQHRoaXMgT2JqZWN0XHJcbiAgICAgKiBAaWdub3JlXHJcbiAgICAgKi9cclxuICAgIHJldHVybiBmdW5jdGlvbigpIHsgLy8gZXNsaW50LWRpc2FibGUtbGluZSBjb25zaXN0ZW50LXJldHVyblxyXG4gICAgICAgIGZvciAodmFyIGtleXMgPSBPYmplY3Qua2V5cyh0aGlzKSwgaSA9IGtleXMubGVuZ3RoIC0gMTsgaSA+IC0xOyAtLWkpXHJcbiAgICAgICAgICAgIGlmIChmaWVsZE1hcFtrZXlzW2ldXSA9PT0gMSAmJiB0aGlzW2tleXNbaV1dICE9PSB1bmRlZmluZWQgJiYgdGhpc1trZXlzW2ldXSAhPT0gbnVsbClcclxuICAgICAgICAgICAgICAgIHJldHVybiBrZXlzW2ldO1xyXG4gICAgfTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBBIE9uZU9mIHNldHRlciBhcyByZXR1cm5lZCBieSB7QGxpbmsgdXRpbC5vbmVPZlNldHRlcn0uXHJcbiAqIEB0eXBlZGVmIE9uZU9mU2V0dGVyXHJcbiAqIEB0eXBlIHtmdW5jdGlvbn1cclxuICogQHBhcmFtIHtzdHJpbmd8dW5kZWZpbmVkfSB2YWx1ZSBGaWVsZCBuYW1lXHJcbiAqIEByZXR1cm5zIHt1bmRlZmluZWR9XHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIEJ1aWxkcyBhIHNldHRlciBmb3IgYSBvbmVvZidzIHByZXNlbnQgZmllbGQgbmFtZS5cclxuICogQHBhcmFtIHtzdHJpbmdbXX0gZmllbGROYW1lcyBGaWVsZCBuYW1lc1xyXG4gKiBAcmV0dXJucyB7T25lT2ZTZXR0ZXJ9IFVuYm91bmQgc2V0dGVyXHJcbiAqL1xyXG51dGlsLm9uZU9mU2V0dGVyID0gZnVuY3Rpb24gc2V0T25lT2YoZmllbGROYW1lcykge1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogQHBhcmFtIHtzdHJpbmd9IG5hbWUgRmllbGQgbmFtZVxyXG4gICAgICogQHJldHVybnMge3VuZGVmaW5lZH1cclxuICAgICAqIEB0aGlzIE9iamVjdFxyXG4gICAgICogQGlnbm9yZVxyXG4gICAgICovXHJcbiAgICByZXR1cm4gZnVuY3Rpb24obmFtZSkge1xyXG4gICAgICAgIGZvciAodmFyIGkgPSAwOyBpIDwgZmllbGROYW1lcy5sZW5ndGg7ICsraSlcclxuICAgICAgICAgICAgaWYgKGZpZWxkTmFtZXNbaV0gIT09IG5hbWUpXHJcbiAgICAgICAgICAgICAgICBkZWxldGUgdGhpc1tmaWVsZE5hbWVzW2ldXTtcclxuICAgIH07XHJcbn07XHJcblxyXG4vKipcclxuICogRGVmYXVsdCBjb252ZXJzaW9uIG9wdGlvbnMgdXNlZCBmb3Ige0BsaW5rIE1lc3NhZ2UjdG9KU09OfSBpbXBsZW1lbnRhdGlvbnMuXHJcbiAqXHJcbiAqIFRoZXNlIG9wdGlvbnMgYXJlIGNsb3NlIHRvIHByb3RvMydzIEpTT04gbWFwcGluZyB3aXRoIHRoZSBleGNlcHRpb24gdGhhdCBpbnRlcm5hbCB0eXBlcyBsaWtlIEFueSBhcmUgaGFuZGxlZCBqdXN0IGxpa2UgbWVzc2FnZXMuIE1vcmUgcHJlY2lzZWx5OlxyXG4gKlxyXG4gKiAtIExvbmdzIGJlY29tZSBzdHJpbmdzXHJcbiAqIC0gRW51bXMgYmVjb21lIHN0cmluZyBrZXlzXHJcbiAqIC0gQnl0ZXMgYmVjb21lIGJhc2U2NCBlbmNvZGVkIHN0cmluZ3NcclxuICogLSAoU3ViLSlNZXNzYWdlcyBiZWNvbWUgcGxhaW4gb2JqZWN0c1xyXG4gKiAtIE1hcHMgYmVjb21lIHBsYWluIG9iamVjdHMgd2l0aCBhbGwgc3RyaW5nIGtleXNcclxuICogLSBSZXBlYXRlZCBmaWVsZHMgYmVjb21lIGFycmF5c1xyXG4gKiAtIE5hTiBhbmQgSW5maW5pdHkgZm9yIGZsb2F0IGFuZCBkb3VibGUgZmllbGRzIGJlY29tZSBzdHJpbmdzXHJcbiAqXHJcbiAqIEB0eXBlIHtJQ29udmVyc2lvbk9wdGlvbnN9XHJcbiAqIEBzZWUgaHR0cHM6Ly9kZXZlbG9wZXJzLmdvb2dsZS5jb20vcHJvdG9jb2wtYnVmZmVycy9kb2NzL3Byb3RvMz9obD1lbiNqc29uXHJcbiAqL1xyXG51dGlsLnRvSlNPTk9wdGlvbnMgPSB7XHJcbiAgICBsb25nczogU3RyaW5nLFxyXG4gICAgZW51bXM6IFN0cmluZyxcclxuICAgIGJ5dGVzOiBTdHJpbmcsXHJcbiAgICBqc29uOiB0cnVlXHJcbn07XHJcblxyXG4vLyBTZXRzIHVwIGJ1ZmZlciB1dGlsaXR5IGFjY29yZGluZyB0byB0aGUgZW52aXJvbm1lbnQgKGNhbGxlZCBpbiBpbmRleC1taW5pbWFsKVxyXG51dGlsLl9jb25maWd1cmUgPSBmdW5jdGlvbigpIHtcclxuICAgIHZhciBCdWZmZXIgPSB1dGlsLkJ1ZmZlcjtcclxuICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBpZiAqL1xyXG4gICAgaWYgKCFCdWZmZXIpIHtcclxuICAgICAgICB1dGlsLl9CdWZmZXJfZnJvbSA9IHV0aWwuX0J1ZmZlcl9hbGxvY1Vuc2FmZSA9IG51bGw7XHJcbiAgICAgICAgcmV0dXJuO1xyXG4gICAgfVxyXG4gICAgLy8gYmVjYXVzZSBub2RlIDQueCBidWZmZXJzIGFyZSBpbmNvbXBhdGlibGUgJiBpbW11dGFibGVcclxuICAgIC8vIHNlZTogaHR0cHM6Ly9naXRodWIuY29tL2Rjb2RlSU8vcHJvdG9idWYuanMvcHVsbC82NjVcclxuICAgIHV0aWwuX0J1ZmZlcl9mcm9tID0gQnVmZmVyLmZyb20gIT09IFVpbnQ4QXJyYXkuZnJvbSAmJiBCdWZmZXIuZnJvbSB8fFxyXG4gICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0ICovXHJcbiAgICAgICAgZnVuY3Rpb24gQnVmZmVyX2Zyb20odmFsdWUsIGVuY29kaW5nKSB7XHJcbiAgICAgICAgICAgIHJldHVybiBuZXcgQnVmZmVyKHZhbHVlLCBlbmNvZGluZyk7XHJcbiAgICAgICAgfTtcclxuICAgIHV0aWwuX0J1ZmZlcl9hbGxvY1Vuc2FmZSA9IEJ1ZmZlci5hbGxvY1Vuc2FmZSB8fFxyXG4gICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0ICovXHJcbiAgICAgICAgZnVuY3Rpb24gQnVmZmVyX2FsbG9jVW5zYWZlKHNpemUpIHtcclxuICAgICAgICAgICAgcmV0dXJuIG5ldyBCdWZmZXIoc2l6ZSk7XHJcbiAgICAgICAgfTtcclxufTtcclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbm1vZHVsZS5leHBvcnRzID0gdmVyaWZpZXI7XHJcblxyXG52YXIgRW51bSAgICAgID0gcmVxdWlyZShcIi4vZW51bVwiKSxcclxuICAgIHV0aWwgICAgICA9IHJlcXVpcmUoXCIuL3V0aWxcIik7XHJcblxyXG5mdW5jdGlvbiBpbnZhbGlkKGZpZWxkLCBleHBlY3RlZCkge1xyXG4gICAgcmV0dXJuIGZpZWxkLm5hbWUgKyBcIjogXCIgKyBleHBlY3RlZCArIChmaWVsZC5yZXBlYXRlZCAmJiBleHBlY3RlZCAhPT0gXCJhcnJheVwiID8gXCJbXVwiIDogZmllbGQubWFwICYmIGV4cGVjdGVkICE9PSBcIm9iamVjdFwiID8gXCJ7azpcIitmaWVsZC5rZXlUeXBlK1wifVwiIDogXCJcIikgKyBcIiBleHBlY3RlZFwiO1xyXG59XHJcblxyXG4vKipcclxuICogR2VuZXJhdGVzIGEgcGFydGlhbCB2YWx1ZSB2ZXJpZmllci5cclxuICogQHBhcmFtIHtDb2RlZ2VufSBnZW4gQ29kZWdlbiBpbnN0YW5jZVxyXG4gKiBAcGFyYW0ge0ZpZWxkfSBmaWVsZCBSZWZsZWN0ZWQgZmllbGRcclxuICogQHBhcmFtIHtudW1iZXJ9IGZpZWxkSW5kZXggRmllbGQgaW5kZXhcclxuICogQHBhcmFtIHtzdHJpbmd9IHJlZiBWYXJpYWJsZSByZWZlcmVuY2VcclxuICogQHJldHVybnMge0NvZGVnZW59IENvZGVnZW4gaW5zdGFuY2VcclxuICogQGlnbm9yZVxyXG4gKi9cclxuZnVuY3Rpb24gZ2VuVmVyaWZ5VmFsdWUoZ2VuLCBmaWVsZCwgZmllbGRJbmRleCwgcmVmKSB7XHJcbiAgICAvKiBlc2xpbnQtZGlzYWJsZSBuby11bmV4cGVjdGVkLW11bHRpbGluZSAqL1xyXG4gICAgaWYgKGZpZWxkLnJlc29sdmVkVHlwZSkge1xyXG4gICAgICAgIGlmIChmaWVsZC5yZXNvbHZlZFR5cGUgaW5zdGFuY2VvZiBFbnVtKSB7IGdlblxyXG4gICAgICAgICAgICAoXCJzd2l0Y2goJXMpe1wiLCByZWYpXHJcbiAgICAgICAgICAgICAgICAoXCJkZWZhdWx0OlwiKVxyXG4gICAgICAgICAgICAgICAgICAgIChcInJldHVybiVqXCIsIGludmFsaWQoZmllbGQsIFwiZW51bSB2YWx1ZVwiKSk7XHJcbiAgICAgICAgICAgIGZvciAodmFyIGtleXMgPSBPYmplY3Qua2V5cyhmaWVsZC5yZXNvbHZlZFR5cGUudmFsdWVzKSwgaiA9IDA7IGogPCBrZXlzLmxlbmd0aDsgKytqKSBnZW5cclxuICAgICAgICAgICAgICAgIChcImNhc2UgJWk6XCIsIGZpZWxkLnJlc29sdmVkVHlwZS52YWx1ZXNba2V5c1tqXV0pO1xyXG4gICAgICAgICAgICBnZW5cclxuICAgICAgICAgICAgICAgICAgICAoXCJicmVha1wiKVxyXG4gICAgICAgICAgICAoXCJ9XCIpO1xyXG4gICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgIGdlblxyXG4gICAgICAgICAgICAoXCJ7XCIpXHJcbiAgICAgICAgICAgICAgICAoXCJ2YXIgZT10eXBlc1slaV0udmVyaWZ5KCVzKTtcIiwgZmllbGRJbmRleCwgcmVmKVxyXG4gICAgICAgICAgICAgICAgKFwiaWYoZSlcIilcclxuICAgICAgICAgICAgICAgICAgICAoXCJyZXR1cm4laitlXCIsIGZpZWxkLm5hbWUgKyBcIi5cIilcclxuICAgICAgICAgICAgKFwifVwiKTtcclxuICAgICAgICB9XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICAgIHN3aXRjaCAoZmllbGQudHlwZSkge1xyXG4gICAgICAgICAgICBjYXNlIFwiaW50MzJcIjpcclxuICAgICAgICAgICAgY2FzZSBcInVpbnQzMlwiOlxyXG4gICAgICAgICAgICBjYXNlIFwic2ludDMyXCI6XHJcbiAgICAgICAgICAgIGNhc2UgXCJmaXhlZDMyXCI6XHJcbiAgICAgICAgICAgIGNhc2UgXCJzZml4ZWQzMlwiOiBnZW5cclxuICAgICAgICAgICAgICAgIChcImlmKCF1dGlsLmlzSW50ZWdlciglcykpXCIsIHJlZilcclxuICAgICAgICAgICAgICAgICAgICAoXCJyZXR1cm4lalwiLCBpbnZhbGlkKGZpZWxkLCBcImludGVnZXJcIikpO1xyXG4gICAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgICAgIGNhc2UgXCJpbnQ2NFwiOlxyXG4gICAgICAgICAgICBjYXNlIFwidWludDY0XCI6XHJcbiAgICAgICAgICAgIGNhc2UgXCJzaW50NjRcIjpcclxuICAgICAgICAgICAgY2FzZSBcImZpeGVkNjRcIjpcclxuICAgICAgICAgICAgY2FzZSBcInNmaXhlZDY0XCI6IGdlblxyXG4gICAgICAgICAgICAgICAgKFwiaWYoIXV0aWwuaXNJbnRlZ2VyKCVzKSYmISglcyYmdXRpbC5pc0ludGVnZXIoJXMubG93KSYmdXRpbC5pc0ludGVnZXIoJXMuaGlnaCkpKVwiLCByZWYsIHJlZiwgcmVmLCByZWYpXHJcbiAgICAgICAgICAgICAgICAgICAgKFwicmV0dXJuJWpcIiwgaW52YWxpZChmaWVsZCwgXCJpbnRlZ2VyfExvbmdcIikpO1xyXG4gICAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgICAgIGNhc2UgXCJmbG9hdFwiOlxyXG4gICAgICAgICAgICBjYXNlIFwiZG91YmxlXCI6IGdlblxyXG4gICAgICAgICAgICAgICAgKFwiaWYodHlwZW9mICVzIT09XFxcIm51bWJlclxcXCIpXCIsIHJlZilcclxuICAgICAgICAgICAgICAgICAgICAoXCJyZXR1cm4lalwiLCBpbnZhbGlkKGZpZWxkLCBcIm51bWJlclwiKSk7XHJcbiAgICAgICAgICAgICAgICBicmVhaztcclxuICAgICAgICAgICAgY2FzZSBcImJvb2xcIjogZ2VuXHJcbiAgICAgICAgICAgICAgICAoXCJpZih0eXBlb2YgJXMhPT1cXFwiYm9vbGVhblxcXCIpXCIsIHJlZilcclxuICAgICAgICAgICAgICAgICAgICAoXCJyZXR1cm4lalwiLCBpbnZhbGlkKGZpZWxkLCBcImJvb2xlYW5cIikpO1xyXG4gICAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgICAgIGNhc2UgXCJzdHJpbmdcIjogZ2VuXHJcbiAgICAgICAgICAgICAgICAoXCJpZighdXRpbC5pc1N0cmluZyglcykpXCIsIHJlZilcclxuICAgICAgICAgICAgICAgICAgICAoXCJyZXR1cm4lalwiLCBpbnZhbGlkKGZpZWxkLCBcInN0cmluZ1wiKSk7XHJcbiAgICAgICAgICAgICAgICBicmVhaztcclxuICAgICAgICAgICAgY2FzZSBcImJ5dGVzXCI6IGdlblxyXG4gICAgICAgICAgICAgICAgKFwiaWYoISglcyYmdHlwZW9mICVzLmxlbmd0aD09PVxcXCJudW1iZXJcXFwifHx1dGlsLmlzU3RyaW5nKCVzKSkpXCIsIHJlZiwgcmVmLCByZWYpXHJcbiAgICAgICAgICAgICAgICAgICAgKFwicmV0dXJuJWpcIiwgaW52YWxpZChmaWVsZCwgXCJidWZmZXJcIikpO1xyXG4gICAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG4gICAgcmV0dXJuIGdlbjtcclxuICAgIC8qIGVzbGludC1lbmFibGUgbm8tdW5leHBlY3RlZC1tdWx0aWxpbmUgKi9cclxufVxyXG5cclxuLyoqXHJcbiAqIEdlbmVyYXRlcyBhIHBhcnRpYWwga2V5IHZlcmlmaWVyLlxyXG4gKiBAcGFyYW0ge0NvZGVnZW59IGdlbiBDb2RlZ2VuIGluc3RhbmNlXHJcbiAqIEBwYXJhbSB7RmllbGR9IGZpZWxkIFJlZmxlY3RlZCBmaWVsZFxyXG4gKiBAcGFyYW0ge3N0cmluZ30gcmVmIFZhcmlhYmxlIHJlZmVyZW5jZVxyXG4gKiBAcmV0dXJucyB7Q29kZWdlbn0gQ29kZWdlbiBpbnN0YW5jZVxyXG4gKiBAaWdub3JlXHJcbiAqL1xyXG5mdW5jdGlvbiBnZW5WZXJpZnlLZXkoZ2VuLCBmaWVsZCwgcmVmKSB7XHJcbiAgICAvKiBlc2xpbnQtZGlzYWJsZSBuby11bmV4cGVjdGVkLW11bHRpbGluZSAqL1xyXG4gICAgc3dpdGNoIChmaWVsZC5rZXlUeXBlKSB7XHJcbiAgICAgICAgY2FzZSBcImludDMyXCI6XHJcbiAgICAgICAgY2FzZSBcInVpbnQzMlwiOlxyXG4gICAgICAgIGNhc2UgXCJzaW50MzJcIjpcclxuICAgICAgICBjYXNlIFwiZml4ZWQzMlwiOlxyXG4gICAgICAgIGNhc2UgXCJzZml4ZWQzMlwiOiBnZW5cclxuICAgICAgICAgICAgKFwiaWYoIXV0aWwua2V5MzJSZS50ZXN0KCVzKSlcIiwgcmVmKVxyXG4gICAgICAgICAgICAgICAgKFwicmV0dXJuJWpcIiwgaW52YWxpZChmaWVsZCwgXCJpbnRlZ2VyIGtleVwiKSk7XHJcbiAgICAgICAgICAgIGJyZWFrO1xyXG4gICAgICAgIGNhc2UgXCJpbnQ2NFwiOlxyXG4gICAgICAgIGNhc2UgXCJ1aW50NjRcIjpcclxuICAgICAgICBjYXNlIFwic2ludDY0XCI6XHJcbiAgICAgICAgY2FzZSBcImZpeGVkNjRcIjpcclxuICAgICAgICBjYXNlIFwic2ZpeGVkNjRcIjogZ2VuXHJcbiAgICAgICAgICAgIChcImlmKCF1dGlsLmtleTY0UmUudGVzdCglcykpXCIsIHJlZikgLy8gc2VlIGNvbW1lbnQgYWJvdmU6IHggaXMgb2ssIGQgaXMgbm90XHJcbiAgICAgICAgICAgICAgICAoXCJyZXR1cm4lalwiLCBpbnZhbGlkKGZpZWxkLCBcImludGVnZXJ8TG9uZyBrZXlcIikpO1xyXG4gICAgICAgICAgICBicmVhaztcclxuICAgICAgICBjYXNlIFwiYm9vbFwiOiBnZW5cclxuICAgICAgICAgICAgKFwiaWYoIXV0aWwua2V5MlJlLnRlc3QoJXMpKVwiLCByZWYpXHJcbiAgICAgICAgICAgICAgICAoXCJyZXR1cm4lalwiLCBpbnZhbGlkKGZpZWxkLCBcImJvb2xlYW4ga2V5XCIpKTtcclxuICAgICAgICAgICAgYnJlYWs7XHJcbiAgICB9XHJcbiAgICByZXR1cm4gZ2VuO1xyXG4gICAgLyogZXNsaW50LWVuYWJsZSBuby11bmV4cGVjdGVkLW11bHRpbGluZSAqL1xyXG59XHJcblxyXG4vKipcclxuICogR2VuZXJhdGVzIGEgdmVyaWZpZXIgc3BlY2lmaWMgdG8gdGhlIHNwZWNpZmllZCBtZXNzYWdlIHR5cGUuXHJcbiAqIEBwYXJhbSB7VHlwZX0gbXR5cGUgTWVzc2FnZSB0eXBlXHJcbiAqIEByZXR1cm5zIHtDb2RlZ2VufSBDb2RlZ2VuIGluc3RhbmNlXHJcbiAqL1xyXG5mdW5jdGlvbiB2ZXJpZmllcihtdHlwZSkge1xyXG4gICAgLyogZXNsaW50LWRpc2FibGUgbm8tdW5leHBlY3RlZC1tdWx0aWxpbmUgKi9cclxuXHJcbiAgICB2YXIgZ2VuID0gdXRpbC5jb2RlZ2VuKFtcIm1cIl0sIG10eXBlLm5hbWUgKyBcIiR2ZXJpZnlcIilcclxuICAgIChcImlmKHR5cGVvZiBtIT09XFxcIm9iamVjdFxcXCJ8fG09PT1udWxsKVwiKVxyXG4gICAgICAgIChcInJldHVybiVqXCIsIFwib2JqZWN0IGV4cGVjdGVkXCIpO1xyXG4gICAgdmFyIG9uZW9mcyA9IG10eXBlLm9uZW9mc0FycmF5LFxyXG4gICAgICAgIHNlZW5GaXJzdEZpZWxkID0ge307XHJcbiAgICBpZiAob25lb2ZzLmxlbmd0aCkgZ2VuXHJcbiAgICAoXCJ2YXIgcD17fVwiKTtcclxuXHJcbiAgICBmb3IgKHZhciBpID0gMDsgaSA8IC8qIGluaXRpYWxpemVzICovIG10eXBlLmZpZWxkc0FycmF5Lmxlbmd0aDsgKytpKSB7XHJcbiAgICAgICAgdmFyIGZpZWxkID0gbXR5cGUuX2ZpZWxkc0FycmF5W2ldLnJlc29sdmUoKSxcclxuICAgICAgICAgICAgcmVmICAgPSBcIm1cIiArIHV0aWwuc2FmZVByb3AoZmllbGQubmFtZSk7XHJcblxyXG4gICAgICAgIGlmIChmaWVsZC5vcHRpb25hbCkgZ2VuXHJcbiAgICAgICAgKFwiaWYoJXMhPW51bGwmJm0uaGFzT3duUHJvcGVydHkoJWopKXtcIiwgcmVmLCBmaWVsZC5uYW1lKTsgLy8gIT09IHVuZGVmaW5lZCAmJiAhPT0gbnVsbFxyXG5cclxuICAgICAgICAvLyBtYXAgZmllbGRzXHJcbiAgICAgICAgaWYgKGZpZWxkLm1hcCkgeyBnZW5cclxuICAgICAgICAgICAgKFwiaWYoIXV0aWwuaXNPYmplY3QoJXMpKVwiLCByZWYpXHJcbiAgICAgICAgICAgICAgICAoXCJyZXR1cm4lalwiLCBpbnZhbGlkKGZpZWxkLCBcIm9iamVjdFwiKSlcclxuICAgICAgICAgICAgKFwidmFyIGs9T2JqZWN0LmtleXMoJXMpXCIsIHJlZilcclxuICAgICAgICAgICAgKFwiZm9yKHZhciBpPTA7aTxrLmxlbmd0aDsrK2kpe1wiKTtcclxuICAgICAgICAgICAgICAgIGdlblZlcmlmeUtleShnZW4sIGZpZWxkLCBcImtbaV1cIik7XHJcbiAgICAgICAgICAgICAgICBnZW5WZXJpZnlWYWx1ZShnZW4sIGZpZWxkLCBpLCByZWYgKyBcIltrW2ldXVwiKVxyXG4gICAgICAgICAgICAoXCJ9XCIpO1xyXG5cclxuICAgICAgICAvLyByZXBlYXRlZCBmaWVsZHNcclxuICAgICAgICB9IGVsc2UgaWYgKGZpZWxkLnJlcGVhdGVkKSB7IGdlblxyXG4gICAgICAgICAgICAoXCJpZighQXJyYXkuaXNBcnJheSglcykpXCIsIHJlZilcclxuICAgICAgICAgICAgICAgIChcInJldHVybiVqXCIsIGludmFsaWQoZmllbGQsIFwiYXJyYXlcIikpXHJcbiAgICAgICAgICAgIChcImZvcih2YXIgaT0wO2k8JXMubGVuZ3RoOysraSl7XCIsIHJlZik7XHJcbiAgICAgICAgICAgICAgICBnZW5WZXJpZnlWYWx1ZShnZW4sIGZpZWxkLCBpLCByZWYgKyBcIltpXVwiKVxyXG4gICAgICAgICAgICAoXCJ9XCIpO1xyXG5cclxuICAgICAgICAvLyByZXF1aXJlZCBvciBwcmVzZW50IGZpZWxkc1xyXG4gICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgIGlmIChmaWVsZC5wYXJ0T2YpIHtcclxuICAgICAgICAgICAgICAgIHZhciBvbmVvZlByb3AgPSB1dGlsLnNhZmVQcm9wKGZpZWxkLnBhcnRPZi5uYW1lKTtcclxuICAgICAgICAgICAgICAgIGlmIChzZWVuRmlyc3RGaWVsZFtmaWVsZC5wYXJ0T2YubmFtZV0gPT09IDEpIGdlblxyXG4gICAgICAgICAgICAoXCJpZihwJXM9PT0xKVwiLCBvbmVvZlByb3ApXHJcbiAgICAgICAgICAgICAgICAoXCJyZXR1cm4lalwiLCBmaWVsZC5wYXJ0T2YubmFtZSArIFwiOiBtdWx0aXBsZSB2YWx1ZXNcIik7XHJcbiAgICAgICAgICAgICAgICBzZWVuRmlyc3RGaWVsZFtmaWVsZC5wYXJ0T2YubmFtZV0gPSAxO1xyXG4gICAgICAgICAgICAgICAgZ2VuXHJcbiAgICAgICAgICAgIChcInAlcz0xXCIsIG9uZW9mUHJvcCk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgZ2VuVmVyaWZ5VmFsdWUoZ2VuLCBmaWVsZCwgaSwgcmVmKTtcclxuICAgICAgICB9XHJcbiAgICAgICAgaWYgKGZpZWxkLm9wdGlvbmFsKSBnZW5cclxuICAgICAgICAoXCJ9XCIpO1xyXG4gICAgfVxyXG4gICAgcmV0dXJuIGdlblxyXG4gICAgKFwicmV0dXJuIG51bGxcIik7XHJcbiAgICAvKiBlc2xpbnQtZW5hYmxlIG5vLXVuZXhwZWN0ZWQtbXVsdGlsaW5lICovXHJcbn0iLCJcInVzZSBzdHJpY3RcIjtcclxuXHJcbi8qKlxyXG4gKiBXcmFwcGVycyBmb3IgY29tbW9uIHR5cGVzLlxyXG4gKiBAdHlwZSB7T2JqZWN0LjxzdHJpbmcsSVdyYXBwZXI+fVxyXG4gKiBAY29uc3RcclxuICovXHJcbnZhciB3cmFwcGVycyA9IGV4cG9ydHM7XHJcblxyXG52YXIgTWVzc2FnZSA9IHJlcXVpcmUoXCIuL21lc3NhZ2VcIik7XHJcblxyXG4vKipcclxuICogRnJvbSBvYmplY3QgY29udmVydGVyIHBhcnQgb2YgYW4ge0BsaW5rIElXcmFwcGVyfS5cclxuICogQHR5cGVkZWYgV3JhcHBlckZyb21PYmplY3RDb252ZXJ0ZXJcclxuICogQHR5cGUge2Z1bmN0aW9ufVxyXG4gKiBAcGFyYW0ge09iamVjdC48c3RyaW5nLCo+fSBvYmplY3QgUGxhaW4gb2JqZWN0XHJcbiAqIEByZXR1cm5zIHtNZXNzYWdlPHt9Pn0gTWVzc2FnZSBpbnN0YW5jZVxyXG4gKiBAdGhpcyBUeXBlXHJcbiAqL1xyXG5cclxuLyoqXHJcbiAqIFRvIG9iamVjdCBjb252ZXJ0ZXIgcGFydCBvZiBhbiB7QGxpbmsgSVdyYXBwZXJ9LlxyXG4gKiBAdHlwZWRlZiBXcmFwcGVyVG9PYmplY3RDb252ZXJ0ZXJcclxuICogQHR5cGUge2Z1bmN0aW9ufVxyXG4gKiBAcGFyYW0ge01lc3NhZ2U8e30+fSBtZXNzYWdlIE1lc3NhZ2UgaW5zdGFuY2VcclxuICogQHBhcmFtIHtJQ29udmVyc2lvbk9wdGlvbnN9IFtvcHRpb25zXSBDb252ZXJzaW9uIG9wdGlvbnNcclxuICogQHJldHVybnMge09iamVjdC48c3RyaW5nLCo+fSBQbGFpbiBvYmplY3RcclxuICogQHRoaXMgVHlwZVxyXG4gKi9cclxuXHJcbi8qKlxyXG4gKiBDb21tb24gdHlwZSB3cmFwcGVyIHBhcnQgb2Yge0BsaW5rIHdyYXBwZXJzfS5cclxuICogQGludGVyZmFjZSBJV3JhcHBlclxyXG4gKiBAcHJvcGVydHkge1dyYXBwZXJGcm9tT2JqZWN0Q29udmVydGVyfSBbZnJvbU9iamVjdF0gRnJvbSBvYmplY3QgY29udmVydGVyXHJcbiAqIEBwcm9wZXJ0eSB7V3JhcHBlclRvT2JqZWN0Q29udmVydGVyfSBbdG9PYmplY3RdIFRvIG9iamVjdCBjb252ZXJ0ZXJcclxuICovXHJcblxyXG4vLyBDdXN0b20gd3JhcHBlciBmb3IgQW55XHJcbndyYXBwZXJzW1wiLmdvb2dsZS5wcm90b2J1Zi5BbnlcIl0gPSB7XHJcblxyXG4gICAgZnJvbU9iamVjdDogZnVuY3Rpb24ob2JqZWN0KSB7XHJcblxyXG4gICAgICAgIC8vIHVud3JhcCB2YWx1ZSB0eXBlIGlmIG1hcHBlZFxyXG4gICAgICAgIGlmIChvYmplY3QgJiYgb2JqZWN0W1wiQHR5cGVcIl0pIHtcclxuICAgICAgICAgICAgIC8vIE9ubHkgdXNlIGZ1bGx5IHF1YWxpZmllZCB0eXBlIG5hbWUgYWZ0ZXIgdGhlIGxhc3QgJy8nXHJcbiAgICAgICAgICAgIHZhciBuYW1lID0gb2JqZWN0W1wiQHR5cGVcIl0uc3Vic3RyaW5nKG9iamVjdFtcIkB0eXBlXCJdLmxhc3RJbmRleE9mKFwiL1wiKSArIDEpO1xyXG4gICAgICAgICAgICB2YXIgdHlwZSA9IHRoaXMubG9va3VwKG5hbWUpO1xyXG4gICAgICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgZWxzZSAqL1xyXG4gICAgICAgICAgICBpZiAodHlwZSkge1xyXG4gICAgICAgICAgICAgICAgLy8gdHlwZV91cmwgZG9lcyBub3QgYWNjZXB0IGxlYWRpbmcgXCIuXCJcclxuICAgICAgICAgICAgICAgIHZhciB0eXBlX3VybCA9IG9iamVjdFtcIkB0eXBlXCJdLmNoYXJBdCgwKSA9PT0gXCIuXCIgP1xyXG4gICAgICAgICAgICAgICAgICAgIG9iamVjdFtcIkB0eXBlXCJdLnNsaWNlKDEpIDogb2JqZWN0W1wiQHR5cGVcIl07XHJcbiAgICAgICAgICAgICAgICAvLyB0eXBlX3VybCBwcmVmaXggaXMgb3B0aW9uYWwsIGJ1dCBwYXRoIHNlcGVyYXRvciBpcyByZXF1aXJlZFxyXG4gICAgICAgICAgICAgICAgaWYgKHR5cGVfdXJsLmluZGV4T2YoXCIvXCIpID09PSAtMSkge1xyXG4gICAgICAgICAgICAgICAgICAgIHR5cGVfdXJsID0gXCIvXCIgKyB0eXBlX3VybDtcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgIHJldHVybiB0aGlzLmNyZWF0ZSh7XHJcbiAgICAgICAgICAgICAgICAgICAgdHlwZV91cmw6IHR5cGVfdXJsLFxyXG4gICAgICAgICAgICAgICAgICAgIHZhbHVlOiB0eXBlLmVuY29kZSh0eXBlLmZyb21PYmplY3Qob2JqZWN0KSkuZmluaXNoKClcclxuICAgICAgICAgICAgICAgIH0pO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICByZXR1cm4gdGhpcy5mcm9tT2JqZWN0KG9iamVjdCk7XHJcbiAgICB9LFxyXG5cclxuICAgIHRvT2JqZWN0OiBmdW5jdGlvbihtZXNzYWdlLCBvcHRpb25zKSB7XHJcblxyXG4gICAgICAgIC8vIERlZmF1bHQgcHJlZml4XHJcbiAgICAgICAgdmFyIGdvb2dsZUFwaSA9IFwidHlwZS5nb29nbGVhcGlzLmNvbS9cIjtcclxuICAgICAgICB2YXIgcHJlZml4ID0gXCJcIjtcclxuICAgICAgICB2YXIgbmFtZSA9IFwiXCI7XHJcblxyXG4gICAgICAgIC8vIGRlY29kZSB2YWx1ZSBpZiByZXF1ZXN0ZWQgYW5kIHVubWFwcGVkXHJcbiAgICAgICAgaWYgKG9wdGlvbnMgJiYgb3B0aW9ucy5qc29uICYmIG1lc3NhZ2UudHlwZV91cmwgJiYgbWVzc2FnZS52YWx1ZSkge1xyXG4gICAgICAgICAgICAvLyBPbmx5IHVzZSBmdWxseSBxdWFsaWZpZWQgdHlwZSBuYW1lIGFmdGVyIHRoZSBsYXN0ICcvJ1xyXG4gICAgICAgICAgICBuYW1lID0gbWVzc2FnZS50eXBlX3VybC5zdWJzdHJpbmcobWVzc2FnZS50eXBlX3VybC5sYXN0SW5kZXhPZihcIi9cIikgKyAxKTtcclxuICAgICAgICAgICAgLy8gU2VwYXJhdGUgdGhlIHByZWZpeCB1c2VkXHJcbiAgICAgICAgICAgIHByZWZpeCA9IG1lc3NhZ2UudHlwZV91cmwuc3Vic3RyaW5nKDAsIG1lc3NhZ2UudHlwZV91cmwubGFzdEluZGV4T2YoXCIvXCIpICsgMSk7XHJcbiAgICAgICAgICAgIHZhciB0eXBlID0gdGhpcy5sb29rdXAobmFtZSk7XHJcbiAgICAgICAgICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBlbHNlICovXHJcbiAgICAgICAgICAgIGlmICh0eXBlKVxyXG4gICAgICAgICAgICAgICAgbWVzc2FnZSA9IHR5cGUuZGVjb2RlKG1lc3NhZ2UudmFsdWUpO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgLy8gd3JhcCB2YWx1ZSBpZiB1bm1hcHBlZFxyXG4gICAgICAgIGlmICghKG1lc3NhZ2UgaW5zdGFuY2VvZiB0aGlzLmN0b3IpICYmIG1lc3NhZ2UgaW5zdGFuY2VvZiBNZXNzYWdlKSB7XHJcbiAgICAgICAgICAgIHZhciBvYmplY3QgPSBtZXNzYWdlLiR0eXBlLnRvT2JqZWN0KG1lc3NhZ2UsIG9wdGlvbnMpO1xyXG4gICAgICAgICAgICB2YXIgbWVzc2FnZU5hbWUgPSBtZXNzYWdlLiR0eXBlLmZ1bGxOYW1lWzBdID09PSBcIi5cIiA/XHJcbiAgICAgICAgICAgICAgICBtZXNzYWdlLiR0eXBlLmZ1bGxOYW1lLnNsaWNlKDEpIDogbWVzc2FnZS4kdHlwZS5mdWxsTmFtZTtcclxuICAgICAgICAgICAgLy8gRGVmYXVsdCB0byB0eXBlLmdvb2dsZWFwaXMuY29tIHByZWZpeCBpZiBubyBwcmVmaXggaXMgdXNlZFxyXG4gICAgICAgICAgICBpZiAocHJlZml4ID09PSBcIlwiKSB7XHJcbiAgICAgICAgICAgICAgICBwcmVmaXggPSBnb29nbGVBcGk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgbmFtZSA9IHByZWZpeCArIG1lc3NhZ2VOYW1lO1xyXG4gICAgICAgICAgICBvYmplY3RbXCJAdHlwZVwiXSA9IG5hbWU7XHJcbiAgICAgICAgICAgIHJldHVybiBvYmplY3Q7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICByZXR1cm4gdGhpcy50b09iamVjdChtZXNzYWdlLCBvcHRpb25zKTtcclxuICAgIH1cclxufTtcclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbm1vZHVsZS5leHBvcnRzID0gV3JpdGVyO1xyXG5cclxudmFyIHV0aWwgICAgICA9IHJlcXVpcmUoXCIuL3V0aWwvbWluaW1hbFwiKTtcclxuXHJcbnZhciBCdWZmZXJXcml0ZXI7IC8vIGN5Y2xpY1xyXG5cclxudmFyIExvbmdCaXRzICA9IHV0aWwuTG9uZ0JpdHMsXHJcbiAgICBiYXNlNjQgICAgPSB1dGlsLmJhc2U2NCxcclxuICAgIHV0ZjggICAgICA9IHV0aWwudXRmODtcclxuXHJcbi8qKlxyXG4gKiBDb25zdHJ1Y3RzIGEgbmV3IHdyaXRlciBvcGVyYXRpb24gaW5zdGFuY2UuXHJcbiAqIEBjbGFzc2Rlc2MgU2NoZWR1bGVkIHdyaXRlciBvcGVyYXRpb24uXHJcbiAqIEBjb25zdHJ1Y3RvclxyXG4gKiBAcGFyYW0ge2Z1bmN0aW9uKCosIFVpbnQ4QXJyYXksIG51bWJlcil9IGZuIEZ1bmN0aW9uIHRvIGNhbGxcclxuICogQHBhcmFtIHtudW1iZXJ9IGxlbiBWYWx1ZSBieXRlIGxlbmd0aFxyXG4gKiBAcGFyYW0geyp9IHZhbCBWYWx1ZSB0byB3cml0ZVxyXG4gKiBAaWdub3JlXHJcbiAqL1xyXG5mdW5jdGlvbiBPcChmbiwgbGVuLCB2YWwpIHtcclxuXHJcbiAgICAvKipcclxuICAgICAqIEZ1bmN0aW9uIHRvIGNhbGwuXHJcbiAgICAgKiBAdHlwZSB7ZnVuY3Rpb24oVWludDhBcnJheSwgbnVtYmVyLCAqKX1cclxuICAgICAqL1xyXG4gICAgdGhpcy5mbiA9IGZuO1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogVmFsdWUgYnl0ZSBsZW5ndGguXHJcbiAgICAgKiBAdHlwZSB7bnVtYmVyfVxyXG4gICAgICovXHJcbiAgICB0aGlzLmxlbiA9IGxlbjtcclxuXHJcbiAgICAvKipcclxuICAgICAqIE5leHQgb3BlcmF0aW9uLlxyXG4gICAgICogQHR5cGUge1dyaXRlci5PcHx1bmRlZmluZWR9XHJcbiAgICAgKi9cclxuICAgIHRoaXMubmV4dCA9IHVuZGVmaW5lZDtcclxuXHJcbiAgICAvKipcclxuICAgICAqIFZhbHVlIHRvIHdyaXRlLlxyXG4gICAgICogQHR5cGUgeyp9XHJcbiAgICAgKi9cclxuICAgIHRoaXMudmFsID0gdmFsOyAvLyB0eXBlIHZhcmllc1xyXG59XHJcblxyXG4vKiBpc3RhbmJ1bCBpZ25vcmUgbmV4dCAqL1xyXG5mdW5jdGlvbiBub29wKCkge30gLy8gZXNsaW50LWRpc2FibGUtbGluZSBuby1lbXB0eS1mdW5jdGlvblxyXG5cclxuLyoqXHJcbiAqIENvbnN0cnVjdHMgYSBuZXcgd3JpdGVyIHN0YXRlIGluc3RhbmNlLlxyXG4gKiBAY2xhc3NkZXNjIENvcGllZCB3cml0ZXIgc3RhdGUuXHJcbiAqIEBtZW1iZXJvZiBXcml0ZXJcclxuICogQGNvbnN0cnVjdG9yXHJcbiAqIEBwYXJhbSB7V3JpdGVyfSB3cml0ZXIgV3JpdGVyIHRvIGNvcHkgc3RhdGUgZnJvbVxyXG4gKiBAaWdub3JlXHJcbiAqL1xyXG5mdW5jdGlvbiBTdGF0ZSh3cml0ZXIpIHtcclxuXHJcbiAgICAvKipcclxuICAgICAqIEN1cnJlbnQgaGVhZC5cclxuICAgICAqIEB0eXBlIHtXcml0ZXIuT3B9XHJcbiAgICAgKi9cclxuICAgIHRoaXMuaGVhZCA9IHdyaXRlci5oZWFkO1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogQ3VycmVudCB0YWlsLlxyXG4gICAgICogQHR5cGUge1dyaXRlci5PcH1cclxuICAgICAqL1xyXG4gICAgdGhpcy50YWlsID0gd3JpdGVyLnRhaWw7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBDdXJyZW50IGJ1ZmZlciBsZW5ndGguXHJcbiAgICAgKiBAdHlwZSB7bnVtYmVyfVxyXG4gICAgICovXHJcbiAgICB0aGlzLmxlbiA9IHdyaXRlci5sZW47XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBOZXh0IHN0YXRlLlxyXG4gICAgICogQHR5cGUge1N0YXRlfG51bGx9XHJcbiAgICAgKi9cclxuICAgIHRoaXMubmV4dCA9IHdyaXRlci5zdGF0ZXM7XHJcbn1cclxuXHJcbi8qKlxyXG4gKiBDb25zdHJ1Y3RzIGEgbmV3IHdyaXRlciBpbnN0YW5jZS5cclxuICogQGNsYXNzZGVzYyBXaXJlIGZvcm1hdCB3cml0ZXIgdXNpbmcgYFVpbnQ4QXJyYXlgIGlmIGF2YWlsYWJsZSwgb3RoZXJ3aXNlIGBBcnJheWAuXHJcbiAqIEBjb25zdHJ1Y3RvclxyXG4gKi9cclxuZnVuY3Rpb24gV3JpdGVyKCkge1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogQ3VycmVudCBsZW5ndGguXHJcbiAgICAgKiBAdHlwZSB7bnVtYmVyfVxyXG4gICAgICovXHJcbiAgICB0aGlzLmxlbiA9IDA7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBPcGVyYXRpb25zIGhlYWQuXHJcbiAgICAgKiBAdHlwZSB7T2JqZWN0fVxyXG4gICAgICovXHJcbiAgICB0aGlzLmhlYWQgPSBuZXcgT3Aobm9vcCwgMCwgMCk7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBPcGVyYXRpb25zIHRhaWxcclxuICAgICAqIEB0eXBlIHtPYmplY3R9XHJcbiAgICAgKi9cclxuICAgIHRoaXMudGFpbCA9IHRoaXMuaGVhZDtcclxuXHJcbiAgICAvKipcclxuICAgICAqIExpbmtlZCBmb3JrZWQgc3RhdGVzLlxyXG4gICAgICogQHR5cGUge09iamVjdHxudWxsfVxyXG4gICAgICovXHJcbiAgICB0aGlzLnN0YXRlcyA9IG51bGw7XHJcblxyXG4gICAgLy8gV2hlbiBhIHZhbHVlIGlzIHdyaXR0ZW4sIHRoZSB3cml0ZXIgY2FsY3VsYXRlcyBpdHMgYnl0ZSBsZW5ndGggYW5kIHB1dHMgaXQgaW50byBhIGxpbmtlZFxyXG4gICAgLy8gbGlzdCBvZiBvcGVyYXRpb25zIHRvIHBlcmZvcm0gd2hlbiBmaW5pc2goKSBpcyBjYWxsZWQuIFRoaXMgYm90aCBhbGxvd3MgdXMgdG8gYWxsb2NhdGVcclxuICAgIC8vIGJ1ZmZlcnMgb2YgdGhlIGV4YWN0IHJlcXVpcmVkIHNpemUgYW5kIHJlZHVjZXMgdGhlIGFtb3VudCBvZiB3b3JrIHdlIGhhdmUgdG8gZG8gY29tcGFyZWRcclxuICAgIC8vIHRvIGZpcnN0IGNhbGN1bGF0aW5nIG92ZXIgb2JqZWN0cyBhbmQgdGhlbiBlbmNvZGluZyBvdmVyIG9iamVjdHMuIEluIG91ciBjYXNlLCB0aGUgZW5jb2RpbmdcclxuICAgIC8vIHBhcnQgaXMganVzdCBhIGxpbmtlZCBsaXN0IHdhbGsgY2FsbGluZyBvcGVyYXRpb25zIHdpdGggYWxyZWFkeSBwcmVwYXJlZCB2YWx1ZXMuXHJcbn1cclxuXHJcbnZhciBjcmVhdGUgPSBmdW5jdGlvbiBjcmVhdGUoKSB7XHJcbiAgICByZXR1cm4gdXRpbC5CdWZmZXJcclxuICAgICAgICA/IGZ1bmN0aW9uIGNyZWF0ZV9idWZmZXJfc2V0dXAoKSB7XHJcbiAgICAgICAgICAgIHJldHVybiAoV3JpdGVyLmNyZWF0ZSA9IGZ1bmN0aW9uIGNyZWF0ZV9idWZmZXIoKSB7XHJcbiAgICAgICAgICAgICAgICByZXR1cm4gbmV3IEJ1ZmZlcldyaXRlcigpO1xyXG4gICAgICAgICAgICB9KSgpO1xyXG4gICAgICAgIH1cclxuICAgICAgICAvKiBpc3RhbmJ1bCBpZ25vcmUgbmV4dCAqL1xyXG4gICAgICAgIDogZnVuY3Rpb24gY3JlYXRlX2FycmF5KCkge1xyXG4gICAgICAgICAgICByZXR1cm4gbmV3IFdyaXRlcigpO1xyXG4gICAgICAgIH07XHJcbn07XHJcblxyXG4vKipcclxuICogQ3JlYXRlcyBhIG5ldyB3cml0ZXIuXHJcbiAqIEBmdW5jdGlvblxyXG4gKiBAcmV0dXJucyB7QnVmZmVyV3JpdGVyfFdyaXRlcn0gQSB7QGxpbmsgQnVmZmVyV3JpdGVyfSB3aGVuIEJ1ZmZlcnMgYXJlIHN1cHBvcnRlZCwgb3RoZXJ3aXNlIGEge0BsaW5rIFdyaXRlcn1cclxuICovXHJcbldyaXRlci5jcmVhdGUgPSBjcmVhdGUoKTtcclxuXHJcbi8qKlxyXG4gKiBBbGxvY2F0ZXMgYSBidWZmZXIgb2YgdGhlIHNwZWNpZmllZCBzaXplLlxyXG4gKiBAcGFyYW0ge251bWJlcn0gc2l6ZSBCdWZmZXIgc2l6ZVxyXG4gKiBAcmV0dXJucyB7VWludDhBcnJheX0gQnVmZmVyXHJcbiAqL1xyXG5Xcml0ZXIuYWxsb2MgPSBmdW5jdGlvbiBhbGxvYyhzaXplKSB7XHJcbiAgICByZXR1cm4gbmV3IHV0aWwuQXJyYXkoc2l6ZSk7XHJcbn07XHJcblxyXG4vLyBVc2UgVWludDhBcnJheSBidWZmZXIgcG9vbCBpbiB0aGUgYnJvd3NlciwganVzdCBsaWtlIG5vZGUgZG9lcyB3aXRoIGJ1ZmZlcnNcclxuLyogaXN0YW5idWwgaWdub3JlIGVsc2UgKi9cclxuaWYgKHV0aWwuQXJyYXkgIT09IEFycmF5KVxyXG4gICAgV3JpdGVyLmFsbG9jID0gdXRpbC5wb29sKFdyaXRlci5hbGxvYywgdXRpbC5BcnJheS5wcm90b3R5cGUuc3ViYXJyYXkpO1xyXG5cclxuLyoqXHJcbiAqIFB1c2hlcyBhIG5ldyBvcGVyYXRpb24gdG8gdGhlIHF1ZXVlLlxyXG4gKiBAcGFyYW0ge2Z1bmN0aW9uKFVpbnQ4QXJyYXksIG51bWJlciwgKil9IGZuIEZ1bmN0aW9uIHRvIGNhbGxcclxuICogQHBhcmFtIHtudW1iZXJ9IGxlbiBWYWx1ZSBieXRlIGxlbmd0aFxyXG4gKiBAcGFyYW0ge251bWJlcn0gdmFsIFZhbHVlIHRvIHdyaXRlXHJcbiAqIEByZXR1cm5zIHtXcml0ZXJ9IGB0aGlzYFxyXG4gKiBAcHJpdmF0ZVxyXG4gKi9cclxuV3JpdGVyLnByb3RvdHlwZS5fcHVzaCA9IGZ1bmN0aW9uIHB1c2goZm4sIGxlbiwgdmFsKSB7XHJcbiAgICB0aGlzLnRhaWwgPSB0aGlzLnRhaWwubmV4dCA9IG5ldyBPcChmbiwgbGVuLCB2YWwpO1xyXG4gICAgdGhpcy5sZW4gKz0gbGVuO1xyXG4gICAgcmV0dXJuIHRoaXM7XHJcbn07XHJcblxyXG5mdW5jdGlvbiB3cml0ZUJ5dGUodmFsLCBidWYsIHBvcykge1xyXG4gICAgYnVmW3Bvc10gPSB2YWwgJiAyNTU7XHJcbn1cclxuXHJcbmZ1bmN0aW9uIHdyaXRlVmFyaW50MzIodmFsLCBidWYsIHBvcykge1xyXG4gICAgd2hpbGUgKHZhbCA+IDEyNykge1xyXG4gICAgICAgIGJ1Zltwb3MrK10gPSB2YWwgJiAxMjcgfCAxMjg7XHJcbiAgICAgICAgdmFsID4+Pj0gNztcclxuICAgIH1cclxuICAgIGJ1Zltwb3NdID0gdmFsO1xyXG59XHJcblxyXG4vKipcclxuICogQ29uc3RydWN0cyBhIG5ldyB2YXJpbnQgd3JpdGVyIG9wZXJhdGlvbiBpbnN0YW5jZS5cclxuICogQGNsYXNzZGVzYyBTY2hlZHVsZWQgdmFyaW50IHdyaXRlciBvcGVyYXRpb24uXHJcbiAqIEBleHRlbmRzIE9wXHJcbiAqIEBjb25zdHJ1Y3RvclxyXG4gKiBAcGFyYW0ge251bWJlcn0gbGVuIFZhbHVlIGJ5dGUgbGVuZ3RoXHJcbiAqIEBwYXJhbSB7bnVtYmVyfSB2YWwgVmFsdWUgdG8gd3JpdGVcclxuICogQGlnbm9yZVxyXG4gKi9cclxuZnVuY3Rpb24gVmFyaW50T3AobGVuLCB2YWwpIHtcclxuICAgIHRoaXMubGVuID0gbGVuO1xyXG4gICAgdGhpcy5uZXh0ID0gdW5kZWZpbmVkO1xyXG4gICAgdGhpcy52YWwgPSB2YWw7XHJcbn1cclxuXHJcblZhcmludE9wLnByb3RvdHlwZSA9IE9iamVjdC5jcmVhdGUoT3AucHJvdG90eXBlKTtcclxuVmFyaW50T3AucHJvdG90eXBlLmZuID0gd3JpdGVWYXJpbnQzMjtcclxuXHJcbi8qKlxyXG4gKiBXcml0ZXMgYW4gdW5zaWduZWQgMzIgYml0IHZhbHVlIGFzIGEgdmFyaW50LlxyXG4gKiBAcGFyYW0ge251bWJlcn0gdmFsdWUgVmFsdWUgdG8gd3JpdGVcclxuICogQHJldHVybnMge1dyaXRlcn0gYHRoaXNgXHJcbiAqL1xyXG5Xcml0ZXIucHJvdG90eXBlLnVpbnQzMiA9IGZ1bmN0aW9uIHdyaXRlX3VpbnQzMih2YWx1ZSkge1xyXG4gICAgLy8gaGVyZSwgdGhlIGNhbGwgdG8gdGhpcy5wdXNoIGhhcyBiZWVuIGlubGluZWQgYW5kIGEgdmFyaW50IHNwZWNpZmljIE9wIHN1YmNsYXNzIGlzIHVzZWQuXHJcbiAgICAvLyB1aW50MzIgaXMgYnkgZmFyIHRoZSBtb3N0IGZyZXF1ZW50bHkgdXNlZCBvcGVyYXRpb24gYW5kIGJlbmVmaXRzIHNpZ25pZmljYW50bHkgZnJvbSB0aGlzLlxyXG4gICAgdGhpcy5sZW4gKz0gKHRoaXMudGFpbCA9IHRoaXMudGFpbC5uZXh0ID0gbmV3IFZhcmludE9wKFxyXG4gICAgICAgICh2YWx1ZSA9IHZhbHVlID4+PiAwKVxyXG4gICAgICAgICAgICAgICAgPCAxMjggICAgICAgPyAxXHJcbiAgICAgICAgOiB2YWx1ZSA8IDE2Mzg0ICAgICA/IDJcclxuICAgICAgICA6IHZhbHVlIDwgMjA5NzE1MiAgID8gM1xyXG4gICAgICAgIDogdmFsdWUgPCAyNjg0MzU0NTYgPyA0XHJcbiAgICAgICAgOiAgICAgICAgICAgICAgICAgICAgIDUsXHJcbiAgICB2YWx1ZSkpLmxlbjtcclxuICAgIHJldHVybiB0aGlzO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIFdyaXRlcyBhIHNpZ25lZCAzMiBiaXQgdmFsdWUgYXMgYSB2YXJpbnQuXHJcbiAqIEBmdW5jdGlvblxyXG4gKiBAcGFyYW0ge251bWJlcn0gdmFsdWUgVmFsdWUgdG8gd3JpdGVcclxuICogQHJldHVybnMge1dyaXRlcn0gYHRoaXNgXHJcbiAqL1xyXG5Xcml0ZXIucHJvdG90eXBlLmludDMyID0gZnVuY3Rpb24gd3JpdGVfaW50MzIodmFsdWUpIHtcclxuICAgIHJldHVybiB2YWx1ZSA8IDBcclxuICAgICAgICA/IHRoaXMuX3B1c2god3JpdGVWYXJpbnQ2NCwgMTAsIExvbmdCaXRzLmZyb21OdW1iZXIodmFsdWUpKSAvLyAxMCBieXRlcyBwZXIgc3BlY1xyXG4gICAgICAgIDogdGhpcy51aW50MzIodmFsdWUpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIFdyaXRlcyBhIDMyIGJpdCB2YWx1ZSBhcyBhIHZhcmludCwgemlnLXphZyBlbmNvZGVkLlxyXG4gKiBAcGFyYW0ge251bWJlcn0gdmFsdWUgVmFsdWUgdG8gd3JpdGVcclxuICogQHJldHVybnMge1dyaXRlcn0gYHRoaXNgXHJcbiAqL1xyXG5Xcml0ZXIucHJvdG90eXBlLnNpbnQzMiA9IGZ1bmN0aW9uIHdyaXRlX3NpbnQzMih2YWx1ZSkge1xyXG4gICAgcmV0dXJuIHRoaXMudWludDMyKCh2YWx1ZSA8PCAxIF4gdmFsdWUgPj4gMzEpID4+PiAwKTtcclxufTtcclxuXHJcbmZ1bmN0aW9uIHdyaXRlVmFyaW50NjQodmFsLCBidWYsIHBvcykge1xyXG4gICAgd2hpbGUgKHZhbC5oaSkge1xyXG4gICAgICAgIGJ1Zltwb3MrK10gPSB2YWwubG8gJiAxMjcgfCAxMjg7XHJcbiAgICAgICAgdmFsLmxvID0gKHZhbC5sbyA+Pj4gNyB8IHZhbC5oaSA8PCAyNSkgPj4+IDA7XHJcbiAgICAgICAgdmFsLmhpID4+Pj0gNztcclxuICAgIH1cclxuICAgIHdoaWxlICh2YWwubG8gPiAxMjcpIHtcclxuICAgICAgICBidWZbcG9zKytdID0gdmFsLmxvICYgMTI3IHwgMTI4O1xyXG4gICAgICAgIHZhbC5sbyA9IHZhbC5sbyA+Pj4gNztcclxuICAgIH1cclxuICAgIGJ1Zltwb3MrK10gPSB2YWwubG87XHJcbn1cclxuXHJcbi8qKlxyXG4gKiBXcml0ZXMgYW4gdW5zaWduZWQgNjQgYml0IHZhbHVlIGFzIGEgdmFyaW50LlxyXG4gKiBAcGFyYW0ge0xvbmd8bnVtYmVyfHN0cmluZ30gdmFsdWUgVmFsdWUgdG8gd3JpdGVcclxuICogQHJldHVybnMge1dyaXRlcn0gYHRoaXNgXHJcbiAqIEB0aHJvd3Mge1R5cGVFcnJvcn0gSWYgYHZhbHVlYCBpcyBhIHN0cmluZyBhbmQgbm8gbG9uZyBsaWJyYXJ5IGlzIHByZXNlbnQuXHJcbiAqL1xyXG5Xcml0ZXIucHJvdG90eXBlLnVpbnQ2NCA9IGZ1bmN0aW9uIHdyaXRlX3VpbnQ2NCh2YWx1ZSkge1xyXG4gICAgdmFyIGJpdHMgPSBMb25nQml0cy5mcm9tKHZhbHVlKTtcclxuICAgIHJldHVybiB0aGlzLl9wdXNoKHdyaXRlVmFyaW50NjQsIGJpdHMubGVuZ3RoKCksIGJpdHMpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIFdyaXRlcyBhIHNpZ25lZCA2NCBiaXQgdmFsdWUgYXMgYSB2YXJpbnQuXHJcbiAqIEBmdW5jdGlvblxyXG4gKiBAcGFyYW0ge0xvbmd8bnVtYmVyfHN0cmluZ30gdmFsdWUgVmFsdWUgdG8gd3JpdGVcclxuICogQHJldHVybnMge1dyaXRlcn0gYHRoaXNgXHJcbiAqIEB0aHJvd3Mge1R5cGVFcnJvcn0gSWYgYHZhbHVlYCBpcyBhIHN0cmluZyBhbmQgbm8gbG9uZyBsaWJyYXJ5IGlzIHByZXNlbnQuXHJcbiAqL1xyXG5Xcml0ZXIucHJvdG90eXBlLmludDY0ID0gV3JpdGVyLnByb3RvdHlwZS51aW50NjQ7XHJcblxyXG4vKipcclxuICogV3JpdGVzIGEgc2lnbmVkIDY0IGJpdCB2YWx1ZSBhcyBhIHZhcmludCwgemlnLXphZyBlbmNvZGVkLlxyXG4gKiBAcGFyYW0ge0xvbmd8bnVtYmVyfHN0cmluZ30gdmFsdWUgVmFsdWUgdG8gd3JpdGVcclxuICogQHJldHVybnMge1dyaXRlcn0gYHRoaXNgXHJcbiAqIEB0aHJvd3Mge1R5cGVFcnJvcn0gSWYgYHZhbHVlYCBpcyBhIHN0cmluZyBhbmQgbm8gbG9uZyBsaWJyYXJ5IGlzIHByZXNlbnQuXHJcbiAqL1xyXG5Xcml0ZXIucHJvdG90eXBlLnNpbnQ2NCA9IGZ1bmN0aW9uIHdyaXRlX3NpbnQ2NCh2YWx1ZSkge1xyXG4gICAgdmFyIGJpdHMgPSBMb25nQml0cy5mcm9tKHZhbHVlKS56ekVuY29kZSgpO1xyXG4gICAgcmV0dXJuIHRoaXMuX3B1c2god3JpdGVWYXJpbnQ2NCwgYml0cy5sZW5ndGgoKSwgYml0cyk7XHJcbn07XHJcblxyXG4vKipcclxuICogV3JpdGVzIGEgYm9vbGlzaCB2YWx1ZSBhcyBhIHZhcmludC5cclxuICogQHBhcmFtIHtib29sZWFufSB2YWx1ZSBWYWx1ZSB0byB3cml0ZVxyXG4gKiBAcmV0dXJucyB7V3JpdGVyfSBgdGhpc2BcclxuICovXHJcbldyaXRlci5wcm90b3R5cGUuYm9vbCA9IGZ1bmN0aW9uIHdyaXRlX2Jvb2wodmFsdWUpIHtcclxuICAgIHJldHVybiB0aGlzLl9wdXNoKHdyaXRlQnl0ZSwgMSwgdmFsdWUgPyAxIDogMCk7XHJcbn07XHJcblxyXG5mdW5jdGlvbiB3cml0ZUZpeGVkMzIodmFsLCBidWYsIHBvcykge1xyXG4gICAgYnVmW3BvcyAgICBdID0gIHZhbCAgICAgICAgICYgMjU1O1xyXG4gICAgYnVmW3BvcyArIDFdID0gIHZhbCA+Pj4gOCAgICYgMjU1O1xyXG4gICAgYnVmW3BvcyArIDJdID0gIHZhbCA+Pj4gMTYgICYgMjU1O1xyXG4gICAgYnVmW3BvcyArIDNdID0gIHZhbCA+Pj4gMjQ7XHJcbn1cclxuXHJcbi8qKlxyXG4gKiBXcml0ZXMgYW4gdW5zaWduZWQgMzIgYml0IHZhbHVlIGFzIGZpeGVkIDMyIGJpdHMuXHJcbiAqIEBwYXJhbSB7bnVtYmVyfSB2YWx1ZSBWYWx1ZSB0byB3cml0ZVxyXG4gKiBAcmV0dXJucyB7V3JpdGVyfSBgdGhpc2BcclxuICovXHJcbldyaXRlci5wcm90b3R5cGUuZml4ZWQzMiA9IGZ1bmN0aW9uIHdyaXRlX2ZpeGVkMzIodmFsdWUpIHtcclxuICAgIHJldHVybiB0aGlzLl9wdXNoKHdyaXRlRml4ZWQzMiwgNCwgdmFsdWUgPj4+IDApO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIFdyaXRlcyBhIHNpZ25lZCAzMiBiaXQgdmFsdWUgYXMgZml4ZWQgMzIgYml0cy5cclxuICogQGZ1bmN0aW9uXHJcbiAqIEBwYXJhbSB7bnVtYmVyfSB2YWx1ZSBWYWx1ZSB0byB3cml0ZVxyXG4gKiBAcmV0dXJucyB7V3JpdGVyfSBgdGhpc2BcclxuICovXHJcbldyaXRlci5wcm90b3R5cGUuc2ZpeGVkMzIgPSBXcml0ZXIucHJvdG90eXBlLmZpeGVkMzI7XHJcblxyXG4vKipcclxuICogV3JpdGVzIGFuIHVuc2lnbmVkIDY0IGJpdCB2YWx1ZSBhcyBmaXhlZCA2NCBiaXRzLlxyXG4gKiBAcGFyYW0ge0xvbmd8bnVtYmVyfHN0cmluZ30gdmFsdWUgVmFsdWUgdG8gd3JpdGVcclxuICogQHJldHVybnMge1dyaXRlcn0gYHRoaXNgXHJcbiAqIEB0aHJvd3Mge1R5cGVFcnJvcn0gSWYgYHZhbHVlYCBpcyBhIHN0cmluZyBhbmQgbm8gbG9uZyBsaWJyYXJ5IGlzIHByZXNlbnQuXHJcbiAqL1xyXG5Xcml0ZXIucHJvdG90eXBlLmZpeGVkNjQgPSBmdW5jdGlvbiB3cml0ZV9maXhlZDY0KHZhbHVlKSB7XHJcbiAgICB2YXIgYml0cyA9IExvbmdCaXRzLmZyb20odmFsdWUpO1xyXG4gICAgcmV0dXJuIHRoaXMuX3B1c2god3JpdGVGaXhlZDMyLCA0LCBiaXRzLmxvKS5fcHVzaCh3cml0ZUZpeGVkMzIsIDQsIGJpdHMuaGkpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIFdyaXRlcyBhIHNpZ25lZCA2NCBiaXQgdmFsdWUgYXMgZml4ZWQgNjQgYml0cy5cclxuICogQGZ1bmN0aW9uXHJcbiAqIEBwYXJhbSB7TG9uZ3xudW1iZXJ8c3RyaW5nfSB2YWx1ZSBWYWx1ZSB0byB3cml0ZVxyXG4gKiBAcmV0dXJucyB7V3JpdGVyfSBgdGhpc2BcclxuICogQHRocm93cyB7VHlwZUVycm9yfSBJZiBgdmFsdWVgIGlzIGEgc3RyaW5nIGFuZCBubyBsb25nIGxpYnJhcnkgaXMgcHJlc2VudC5cclxuICovXHJcbldyaXRlci5wcm90b3R5cGUuc2ZpeGVkNjQgPSBXcml0ZXIucHJvdG90eXBlLmZpeGVkNjQ7XHJcblxyXG4vKipcclxuICogV3JpdGVzIGEgZmxvYXQgKDMyIGJpdCkuXHJcbiAqIEBmdW5jdGlvblxyXG4gKiBAcGFyYW0ge251bWJlcn0gdmFsdWUgVmFsdWUgdG8gd3JpdGVcclxuICogQHJldHVybnMge1dyaXRlcn0gYHRoaXNgXHJcbiAqL1xyXG5Xcml0ZXIucHJvdG90eXBlLmZsb2F0ID0gZnVuY3Rpb24gd3JpdGVfZmxvYXQodmFsdWUpIHtcclxuICAgIHJldHVybiB0aGlzLl9wdXNoKHV0aWwuZmxvYXQud3JpdGVGbG9hdExFLCA0LCB2YWx1ZSk7XHJcbn07XHJcblxyXG4vKipcclxuICogV3JpdGVzIGEgZG91YmxlICg2NCBiaXQgZmxvYXQpLlxyXG4gKiBAZnVuY3Rpb25cclxuICogQHBhcmFtIHtudW1iZXJ9IHZhbHVlIFZhbHVlIHRvIHdyaXRlXHJcbiAqIEByZXR1cm5zIHtXcml0ZXJ9IGB0aGlzYFxyXG4gKi9cclxuV3JpdGVyLnByb3RvdHlwZS5kb3VibGUgPSBmdW5jdGlvbiB3cml0ZV9kb3VibGUodmFsdWUpIHtcclxuICAgIHJldHVybiB0aGlzLl9wdXNoKHV0aWwuZmxvYXQud3JpdGVEb3VibGVMRSwgOCwgdmFsdWUpO1xyXG59O1xyXG5cclxudmFyIHdyaXRlQnl0ZXMgPSB1dGlsLkFycmF5LnByb3RvdHlwZS5zZXRcclxuICAgID8gZnVuY3Rpb24gd3JpdGVCeXRlc19zZXQodmFsLCBidWYsIHBvcykge1xyXG4gICAgICAgIGJ1Zi5zZXQodmFsLCBwb3MpOyAvLyBhbHNvIHdvcmtzIGZvciBwbGFpbiBhcnJheSB2YWx1ZXNcclxuICAgIH1cclxuICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0ICovXHJcbiAgICA6IGZ1bmN0aW9uIHdyaXRlQnl0ZXNfZm9yKHZhbCwgYnVmLCBwb3MpIHtcclxuICAgICAgICBmb3IgKHZhciBpID0gMDsgaSA8IHZhbC5sZW5ndGg7ICsraSlcclxuICAgICAgICAgICAgYnVmW3BvcyArIGldID0gdmFsW2ldO1xyXG4gICAgfTtcclxuXHJcbi8qKlxyXG4gKiBXcml0ZXMgYSBzZXF1ZW5jZSBvZiBieXRlcy5cclxuICogQHBhcmFtIHtVaW50OEFycmF5fHN0cmluZ30gdmFsdWUgQnVmZmVyIG9yIGJhc2U2NCBlbmNvZGVkIHN0cmluZyB0byB3cml0ZVxyXG4gKiBAcmV0dXJucyB7V3JpdGVyfSBgdGhpc2BcclxuICovXHJcbldyaXRlci5wcm90b3R5cGUuYnl0ZXMgPSBmdW5jdGlvbiB3cml0ZV9ieXRlcyh2YWx1ZSkge1xyXG4gICAgdmFyIGxlbiA9IHZhbHVlLmxlbmd0aCA+Pj4gMDtcclxuICAgIGlmICghbGVuKVxyXG4gICAgICAgIHJldHVybiB0aGlzLl9wdXNoKHdyaXRlQnl0ZSwgMSwgMCk7XHJcbiAgICBpZiAodXRpbC5pc1N0cmluZyh2YWx1ZSkpIHtcclxuICAgICAgICB2YXIgYnVmID0gV3JpdGVyLmFsbG9jKGxlbiA9IGJhc2U2NC5sZW5ndGgodmFsdWUpKTtcclxuICAgICAgICBiYXNlNjQuZGVjb2RlKHZhbHVlLCBidWYsIDApO1xyXG4gICAgICAgIHZhbHVlID0gYnVmO1xyXG4gICAgfVxyXG4gICAgcmV0dXJuIHRoaXMudWludDMyKGxlbikuX3B1c2god3JpdGVCeXRlcywgbGVuLCB2YWx1ZSk7XHJcbn07XHJcblxyXG4vKipcclxuICogV3JpdGVzIGEgc3RyaW5nLlxyXG4gKiBAcGFyYW0ge3N0cmluZ30gdmFsdWUgVmFsdWUgdG8gd3JpdGVcclxuICogQHJldHVybnMge1dyaXRlcn0gYHRoaXNgXHJcbiAqL1xyXG5Xcml0ZXIucHJvdG90eXBlLnN0cmluZyA9IGZ1bmN0aW9uIHdyaXRlX3N0cmluZyh2YWx1ZSkge1xyXG4gICAgdmFyIGxlbiA9IHV0ZjgubGVuZ3RoKHZhbHVlKTtcclxuICAgIHJldHVybiBsZW5cclxuICAgICAgICA/IHRoaXMudWludDMyKGxlbikuX3B1c2godXRmOC53cml0ZSwgbGVuLCB2YWx1ZSlcclxuICAgICAgICA6IHRoaXMuX3B1c2god3JpdGVCeXRlLCAxLCAwKTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBGb3JrcyB0aGlzIHdyaXRlcidzIHN0YXRlIGJ5IHB1c2hpbmcgaXQgdG8gYSBzdGFjay5cclxuICogQ2FsbGluZyB7QGxpbmsgV3JpdGVyI3Jlc2V0fHJlc2V0fSBvciB7QGxpbmsgV3JpdGVyI2xkZWxpbXxsZGVsaW19IHJlc2V0cyB0aGUgd3JpdGVyIHRvIHRoZSBwcmV2aW91cyBzdGF0ZS5cclxuICogQHJldHVybnMge1dyaXRlcn0gYHRoaXNgXHJcbiAqL1xyXG5Xcml0ZXIucHJvdG90eXBlLmZvcmsgPSBmdW5jdGlvbiBmb3JrKCkge1xyXG4gICAgdGhpcy5zdGF0ZXMgPSBuZXcgU3RhdGUodGhpcyk7XHJcbiAgICB0aGlzLmhlYWQgPSB0aGlzLnRhaWwgPSBuZXcgT3Aobm9vcCwgMCwgMCk7XHJcbiAgICB0aGlzLmxlbiA9IDA7XHJcbiAgICByZXR1cm4gdGhpcztcclxufTtcclxuXHJcbi8qKlxyXG4gKiBSZXNldHMgdGhpcyBpbnN0YW5jZSB0byB0aGUgbGFzdCBzdGF0ZS5cclxuICogQHJldHVybnMge1dyaXRlcn0gYHRoaXNgXHJcbiAqL1xyXG5Xcml0ZXIucHJvdG90eXBlLnJlc2V0ID0gZnVuY3Rpb24gcmVzZXQoKSB7XHJcbiAgICBpZiAodGhpcy5zdGF0ZXMpIHtcclxuICAgICAgICB0aGlzLmhlYWQgICA9IHRoaXMuc3RhdGVzLmhlYWQ7XHJcbiAgICAgICAgdGhpcy50YWlsICAgPSB0aGlzLnN0YXRlcy50YWlsO1xyXG4gICAgICAgIHRoaXMubGVuICAgID0gdGhpcy5zdGF0ZXMubGVuO1xyXG4gICAgICAgIHRoaXMuc3RhdGVzID0gdGhpcy5zdGF0ZXMubmV4dDtcclxuICAgIH0gZWxzZSB7XHJcbiAgICAgICAgdGhpcy5oZWFkID0gdGhpcy50YWlsID0gbmV3IE9wKG5vb3AsIDAsIDApO1xyXG4gICAgICAgIHRoaXMubGVuICA9IDA7XHJcbiAgICB9XHJcbiAgICByZXR1cm4gdGhpcztcclxufTtcclxuXHJcbi8qKlxyXG4gKiBSZXNldHMgdG8gdGhlIGxhc3Qgc3RhdGUgYW5kIGFwcGVuZHMgdGhlIGZvcmsgc3RhdGUncyBjdXJyZW50IHdyaXRlIGxlbmd0aCBhcyBhIHZhcmludCBmb2xsb3dlZCBieSBpdHMgb3BlcmF0aW9ucy5cclxuICogQHJldHVybnMge1dyaXRlcn0gYHRoaXNgXHJcbiAqL1xyXG5Xcml0ZXIucHJvdG90eXBlLmxkZWxpbSA9IGZ1bmN0aW9uIGxkZWxpbSgpIHtcclxuICAgIHZhciBoZWFkID0gdGhpcy5oZWFkLFxyXG4gICAgICAgIHRhaWwgPSB0aGlzLnRhaWwsXHJcbiAgICAgICAgbGVuICA9IHRoaXMubGVuO1xyXG4gICAgdGhpcy5yZXNldCgpLnVpbnQzMihsZW4pO1xyXG4gICAgaWYgKGxlbikge1xyXG4gICAgICAgIHRoaXMudGFpbC5uZXh0ID0gaGVhZC5uZXh0OyAvLyBza2lwIG5vb3BcclxuICAgICAgICB0aGlzLnRhaWwgPSB0YWlsO1xyXG4gICAgICAgIHRoaXMubGVuICs9IGxlbjtcclxuICAgIH1cclxuICAgIHJldHVybiB0aGlzO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIEZpbmlzaGVzIHRoZSB3cml0ZSBvcGVyYXRpb24uXHJcbiAqIEByZXR1cm5zIHtVaW50OEFycmF5fSBGaW5pc2hlZCBidWZmZXJcclxuICovXHJcbldyaXRlci5wcm90b3R5cGUuZmluaXNoID0gZnVuY3Rpb24gZmluaXNoKCkge1xyXG4gICAgdmFyIGhlYWQgPSB0aGlzLmhlYWQubmV4dCwgLy8gc2tpcCBub29wXHJcbiAgICAgICAgYnVmICA9IHRoaXMuY29uc3RydWN0b3IuYWxsb2ModGhpcy5sZW4pLFxyXG4gICAgICAgIHBvcyAgPSAwO1xyXG4gICAgd2hpbGUgKGhlYWQpIHtcclxuICAgICAgICBoZWFkLmZuKGhlYWQudmFsLCBidWYsIHBvcyk7XHJcbiAgICAgICAgcG9zICs9IGhlYWQubGVuO1xyXG4gICAgICAgIGhlYWQgPSBoZWFkLm5leHQ7XHJcbiAgICB9XHJcbiAgICAvLyB0aGlzLmhlYWQgPSB0aGlzLnRhaWwgPSBudWxsO1xyXG4gICAgcmV0dXJuIGJ1ZjtcclxufTtcclxuXHJcbldyaXRlci5fY29uZmlndXJlID0gZnVuY3Rpb24oQnVmZmVyV3JpdGVyXykge1xyXG4gICAgQnVmZmVyV3JpdGVyID0gQnVmZmVyV3JpdGVyXztcclxuICAgIFdyaXRlci5jcmVhdGUgPSBjcmVhdGUoKTtcclxuICAgIEJ1ZmZlcldyaXRlci5fY29uZmlndXJlKCk7XHJcbn07XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG5tb2R1bGUuZXhwb3J0cyA9IEJ1ZmZlcldyaXRlcjtcclxuXHJcbi8vIGV4dGVuZHMgV3JpdGVyXHJcbnZhciBXcml0ZXIgPSByZXF1aXJlKFwiLi93cml0ZXJcIik7XHJcbihCdWZmZXJXcml0ZXIucHJvdG90eXBlID0gT2JqZWN0LmNyZWF0ZShXcml0ZXIucHJvdG90eXBlKSkuY29uc3RydWN0b3IgPSBCdWZmZXJXcml0ZXI7XHJcblxyXG52YXIgdXRpbCA9IHJlcXVpcmUoXCIuL3V0aWwvbWluaW1hbFwiKTtcclxuXHJcbi8qKlxyXG4gKiBDb25zdHJ1Y3RzIGEgbmV3IGJ1ZmZlciB3cml0ZXIgaW5zdGFuY2UuXHJcbiAqIEBjbGFzc2Rlc2MgV2lyZSBmb3JtYXQgd3JpdGVyIHVzaW5nIG5vZGUgYnVmZmVycy5cclxuICogQGV4dGVuZHMgV3JpdGVyXHJcbiAqIEBjb25zdHJ1Y3RvclxyXG4gKi9cclxuZnVuY3Rpb24gQnVmZmVyV3JpdGVyKCkge1xyXG4gICAgV3JpdGVyLmNhbGwodGhpcyk7XHJcbn1cclxuXHJcbkJ1ZmZlcldyaXRlci5fY29uZmlndXJlID0gZnVuY3Rpb24gKCkge1xyXG4gICAgLyoqXHJcbiAgICAgKiBBbGxvY2F0ZXMgYSBidWZmZXIgb2YgdGhlIHNwZWNpZmllZCBzaXplLlxyXG4gICAgICogQGZ1bmN0aW9uXHJcbiAgICAgKiBAcGFyYW0ge251bWJlcn0gc2l6ZSBCdWZmZXIgc2l6ZVxyXG4gICAgICogQHJldHVybnMge0J1ZmZlcn0gQnVmZmVyXHJcbiAgICAgKi9cclxuICAgIEJ1ZmZlcldyaXRlci5hbGxvYyA9IHV0aWwuX0J1ZmZlcl9hbGxvY1Vuc2FmZTtcclxuXHJcbiAgICBCdWZmZXJXcml0ZXIud3JpdGVCeXRlc0J1ZmZlciA9IHV0aWwuQnVmZmVyICYmIHV0aWwuQnVmZmVyLnByb3RvdHlwZSBpbnN0YW5jZW9mIFVpbnQ4QXJyYXkgJiYgdXRpbC5CdWZmZXIucHJvdG90eXBlLnNldC5uYW1lID09PSBcInNldFwiXHJcbiAgICAgICAgPyBmdW5jdGlvbiB3cml0ZUJ5dGVzQnVmZmVyX3NldCh2YWwsIGJ1ZiwgcG9zKSB7XHJcbiAgICAgICAgICBidWYuc2V0KHZhbCwgcG9zKTsgLy8gZmFzdGVyIHRoYW4gY29weSAocmVxdWlyZXMgbm9kZSA+PSA0IHdoZXJlIEJ1ZmZlcnMgZXh0ZW5kIFVpbnQ4QXJyYXkgYW5kIHNldCBpcyBwcm9wZXJseSBpbmhlcml0ZWQpXHJcbiAgICAgICAgICAvLyBhbHNvIHdvcmtzIGZvciBwbGFpbiBhcnJheSB2YWx1ZXNcclxuICAgICAgICB9XHJcbiAgICAgICAgLyogaXN0YW5idWwgaWdub3JlIG5leHQgKi9cclxuICAgICAgICA6IGZ1bmN0aW9uIHdyaXRlQnl0ZXNCdWZmZXJfY29weSh2YWwsIGJ1ZiwgcG9zKSB7XHJcbiAgICAgICAgICBpZiAodmFsLmNvcHkpIC8vIEJ1ZmZlciB2YWx1ZXNcclxuICAgICAgICAgICAgdmFsLmNvcHkoYnVmLCBwb3MsIDAsIHZhbC5sZW5ndGgpO1xyXG4gICAgICAgICAgZWxzZSBmb3IgKHZhciBpID0gMDsgaSA8IHZhbC5sZW5ndGg7KSAvLyBwbGFpbiBhcnJheSB2YWx1ZXNcclxuICAgICAgICAgICAgYnVmW3BvcysrXSA9IHZhbFtpKytdO1xyXG4gICAgICAgIH07XHJcbn07XHJcblxyXG5cclxuLyoqXHJcbiAqIEBvdmVycmlkZVxyXG4gKi9cclxuQnVmZmVyV3JpdGVyLnByb3RvdHlwZS5ieXRlcyA9IGZ1bmN0aW9uIHdyaXRlX2J5dGVzX2J1ZmZlcih2YWx1ZSkge1xyXG4gICAgaWYgKHV0aWwuaXNTdHJpbmcodmFsdWUpKVxyXG4gICAgICAgIHZhbHVlID0gdXRpbC5fQnVmZmVyX2Zyb20odmFsdWUsIFwiYmFzZTY0XCIpO1xyXG4gICAgdmFyIGxlbiA9IHZhbHVlLmxlbmd0aCA+Pj4gMDtcclxuICAgIHRoaXMudWludDMyKGxlbik7XHJcbiAgICBpZiAobGVuKVxyXG4gICAgICAgIHRoaXMuX3B1c2goQnVmZmVyV3JpdGVyLndyaXRlQnl0ZXNCdWZmZXIsIGxlbiwgdmFsdWUpO1xyXG4gICAgcmV0dXJuIHRoaXM7XHJcbn07XHJcblxyXG5mdW5jdGlvbiB3cml0ZVN0cmluZ0J1ZmZlcih2YWwsIGJ1ZiwgcG9zKSB7XHJcbiAgICBpZiAodmFsLmxlbmd0aCA8IDQwKSAvLyBwbGFpbiBqcyBpcyBmYXN0ZXIgZm9yIHNob3J0IHN0cmluZ3MgKHByb2JhYmx5IGR1ZSB0byByZWR1bmRhbnQgYXNzZXJ0aW9ucylcclxuICAgICAgICB1dGlsLnV0Zjgud3JpdGUodmFsLCBidWYsIHBvcyk7XHJcbiAgICBlbHNlIGlmIChidWYudXRmOFdyaXRlKVxyXG4gICAgICAgIGJ1Zi51dGY4V3JpdGUodmFsLCBwb3MpO1xyXG4gICAgZWxzZVxyXG4gICAgICAgIGJ1Zi53cml0ZSh2YWwsIHBvcyk7XHJcbn1cclxuXHJcbi8qKlxyXG4gKiBAb3ZlcnJpZGVcclxuICovXHJcbkJ1ZmZlcldyaXRlci5wcm90b3R5cGUuc3RyaW5nID0gZnVuY3Rpb24gd3JpdGVfc3RyaW5nX2J1ZmZlcih2YWx1ZSkge1xyXG4gICAgdmFyIGxlbiA9IHV0aWwuQnVmZmVyLmJ5dGVMZW5ndGgodmFsdWUpO1xyXG4gICAgdGhpcy51aW50MzIobGVuKTtcclxuICAgIGlmIChsZW4pXHJcbiAgICAgICAgdGhpcy5fcHVzaCh3cml0ZVN0cmluZ0J1ZmZlciwgbGVuLCB2YWx1ZSk7XHJcbiAgICByZXR1cm4gdGhpcztcclxufTtcclxuXHJcblxyXG4vKipcclxuICogRmluaXNoZXMgdGhlIHdyaXRlIG9wZXJhdGlvbi5cclxuICogQG5hbWUgQnVmZmVyV3JpdGVyI2ZpbmlzaFxyXG4gKiBAZnVuY3Rpb25cclxuICogQHJldHVybnMge0J1ZmZlcn0gRmluaXNoZWQgYnVmZmVyXHJcbiAqL1xyXG5cclxuQnVmZmVyV3JpdGVyLl9jb25maWd1cmUoKTtcclxuIiwiZXhwb3J0IGNvbnN0IFdFQlNPQ0tFVF9TRVJWRVJfVVJMID0gXCJ3c3M6Ly9sb2NhbC5nYWxhY3RpY29keXNzZXkuc3BhY2UvZ2Fvcy93c1wiIiwiaW1wb3J0IHsgUHJvdG9idWZSb290IH0gZnJvbSAnLi9wcm90byc7XHJcbmltcG9ydCB7IFdlYlNvY2tldENsaWVudCB9IGZyb20gJy4vd3NDbGllbnQnO1xyXG5pbXBvcnQgeyBCYXNlTWVzc2FnZXMgfSBmcm9tICcuL21lc3NhZ2VzL3VuaXR5QnJvd3Nlck1lc3NhZ2luZy9CYXNlTWVzc2FnZXMnO1xyXG5pbXBvcnQgeyBXc0F1dGhlbnRpY2F0aW9uIH0gZnJvbSAnLi9tZXNzYWdlcy9Xc0F1dGhlbnRpY2F0aW9uJztcclxuXHJcbmNvbnN0IEZJTEUgPSAnZGlzcGF0Y2hlci50cyc7XHJcblxyXG5leHBvcnQgY29uc3QgTkFNRVNQQUNFX0lEX19XZWJTb2NrZXQgPSAxO1xyXG5leHBvcnQgY29uc3QgQ0xBU1NfSURfQXV0aGVudGljYXRlID0gMjtcclxuZXhwb3J0IGNvbnN0IE1FVEhPRF9JRF9BdXRoZW50aWNhdGVSZXF1ZXN0ID0gMTtcclxuZXhwb3J0IGNvbnN0IE1FVEhPRF9JRF9BdXRoZW50aWNhdGVSZXNwb25zZSA9IDI7XHJcblxyXG5leHBvcnQgY29uc3QgTkFNRVNQQUNFX0lEX19Vbml0eUJyb3dzZXJDaGFubmVsID0gMjtcclxuZXhwb3J0IGNvbnN0IENMQVNTX0lEX0Jhc2VNZXNzYWdlcyA9IDE7XHJcbmV4cG9ydCBjb25zdCBNRVRIT0RfSURfUmVjZWl2ZVN0cmluZyA9IDE7XHJcblxyXG5cclxuXHJcbmV4cG9ydCBjbGFzcyBEaXNwYXRjaGVyXHJcbntcclxuICAgIHByaXZhdGUgcGJSb290OiBQcm90b2J1ZlJvb3Q7IFxyXG4gICAgcHJpdmF0ZSB3c0NsaWVudDogV2ViU29ja2V0Q2xpZW50O1xyXG4gICAgcHJpdmF0ZSBwYk1lc3NhZ2VIZWFkZXI6IGFueTtcclxuXHJcbiAgICBwdWJsaWMgY29uc3RydWN0b3IocGJSb290OiBQcm90b2J1ZlJvb3QsIHdzQ2xpZW50OiBXZWJTb2NrZXRDbGllbnQpIHtcclxuICAgICAgICB0aGlzLnBiUm9vdCA9IHBiUm9vdDtcclxuICAgICAgICB0aGlzLndzQ2xpZW50ID0gd3NDbGllbnQ7XHJcbiAgICAgICAgdGhpcy5wYk1lc3NhZ2VIZWFkZXIgPSBwYlJvb3Qucm9vdC5sb29rdXBUeXBlKCdHYW9Qcm90b2J1Zi5NZXNzYWdlSGVhZGVyJyk7XHJcblxyXG4gICAgfVxyXG5cclxuICAgIHByaXZhdGUgc3RhdGljIHJlYWRNZXNzYWdlT2JqZWN0U2l6ZShtZXNzYWdlOiBBcnJheUJ1ZmZlciwgb2Zmc2V0OiBudW1iZXIpOiBudW1iZXIge1xyXG4gICAgICAgIGxldCB2aWV3ID0gbmV3IERhdGFWaWV3KG1lc3NhZ2UpO1xyXG4gICAgICAgIHJldHVybiB2aWV3LmdldFVpbnQzMihvZmZzZXQsIGZhbHNlKTtcclxuICAgIH1cclxuXHJcbiAgICBwcml2YXRlIHN0YXRpYyBlbmNvZGVNZXNzYWdlT2JqZWN0U2l6ZShzaXplOiBudW1iZXIpOiBBcnJheUJ1ZmZlciB7XHJcbiAgICAgICAgbGV0IGRhdGEgPSBuZXcgQXJyYXlCdWZmZXIoNCk7XHJcbiAgICAgICAgbGV0IHZpZXcgPSBuZXcgRGF0YVZpZXcoZGF0YSk7XHJcbiAgICAgICAgdmlldy5zZXRVaW50MzIoMCwgc2l6ZSwgZmFsc2UpO1xyXG4gICAgICAgIHJldHVybiBkYXRhXHJcbiAgICB9XHJcblxyXG4gICAgcHVibGljIHN0YXRpYyByZWFkTWVzc2FnZU9iamVjdChtZXNzYWdlOiBBcnJheUJ1ZmZlciwgb2Zmc2V0OiBudW1iZXIsIHBiTWVzc2FnZU9iamVjdDogYW55KTogeyBtb01lc3NhZ2VPYmplY3Q6IGFueSwgc2l6ZTogbnVtYmVyfSB7XHJcbiAgICAgICAgY29uc3QgRlVOQyA9ICdyZWFkTWVzc2FnZU9iamVjdCgpJztcclxuICAgICAgICB0cnkge1xyXG4gICAgICAgICAgICBsZXQgaGVhZGVyU2l6ZSA9IERpc3BhdGNoZXIucmVhZE1lc3NhZ2VPYmplY3RTaXplKG1lc3NhZ2UsIG9mZnNldCk7XHJcbiAgICAgICAgICAgIGxldCBkYXRhID0gbmV3IFVpbnQ4QXJyYXkobWVzc2FnZSwgb2Zmc2V0ICsgNCwgaGVhZGVyU2l6ZSk7XHJcbiAgICAgICAgICAgIGxldCBtb01lc3NhZ2VPYmplY3QgPSBwYk1lc3NhZ2VPYmplY3QuZGVjb2RlKGRhdGEpO1xyXG4gICAgICAgICAgICByZXR1cm4geyBtb01lc3NhZ2VPYmplY3QsIHNpemU6IDQgKyBoZWFkZXJTaXplIH07XHJcbiAgICAgICAgfSBjYXRjaCAoZXJyKSB7XHJcbiAgICAgICAgICAgIGNvbnNvbGUuZXJyb3IoYCR7RklMRX06JHtGVU5DfTogZXJyb3JgLCBlcnIpO1xyXG4gICAgICAgICAgICB0aHJvdyBuZXcgRXJyb3IoJ3JlYWRNZXNzYWdlSGVhZGVyIGZhaWxlZCcpO1xyXG4gICAgICAgIH1cclxuICAgIH1cclxuXHJcbiAgICBwdWJsaWMgc3RhdGljIGVuY29kZU1lc3NhZ2VPYmplY3QocGJNZXNzYWdlT2JqZWN0OiBhbnksIG1vTWVzc2FnZU9iamVjdDogYW55KTogQXJyYXlCdWZmZXIge1xyXG4gICAgICAgIGNvbnN0IEZVTkMgPSAnZW5jb2RlTWVzc2FnZU9iamVjdCgpJztcclxuICAgICAgICB0cnkge1xyXG4gICAgICAgICAgICBsZXQgZGF0YU1vID0gcGJNZXNzYWdlT2JqZWN0LmVuY29kZShtb01lc3NhZ2VPYmplY3QpLmZpbmlzaCgpO1xyXG4gICAgICAgICAgICBsZXQgZGF0YU1vU2l6ZSA9IERpc3BhdGNoZXIuZW5jb2RlTWVzc2FnZU9iamVjdFNpemUoZGF0YU1vLmxlbmd0aCk7XHJcblxyXG4gICAgICAgICAgICBsZXQgZGF0YSA9IG5ldyBVaW50OEFycmF5KGRhdGFNb1NpemUuYnl0ZUxlbmd0aCArIGRhdGFNby5sZW5ndGgpO1xyXG4gICAgICAgICAgICAvLyBzZXJpYWxpemUgbWVzc2FnZSBvYmplY3Qgc2l6ZSB0byB2aWV3XHJcbiAgICAgICAgICAgIGRhdGEuc2V0KG5ldyBVaW50OEFycmF5KGRhdGFNb1NpemUpLCAwKTtcclxuICAgICAgICAgICAgLy8gc2VyaWFsaXplIG1lc3NhZ2Ugb2JqZWN0IHRvIHZpZXdcclxuICAgICAgICAgICAgZGF0YS5zZXQoZGF0YU1vLCBkYXRhTW9TaXplLmJ5dGVMZW5ndGgpO1xyXG5cclxuICAgICAgICAgICAgcmV0dXJuIGRhdGEuYnVmZmVyXHJcblxyXG4gICAgICAgIH0gY2F0Y2ggKGVycikge1xyXG4gICAgICAgICAgICBjb25zb2xlLmVycm9yKEZJTEUsIEZVTkMsIGVycik7XHJcbiAgICAgICAgICAgIHRocm93IG5ldyBFcnJvcignZW5jb2RlTWVzc2FnZU9iamVjdCgpIGZhaWxlZCcpO1xyXG4gICAgICAgIH1cclxuICAgIH1cclxuXHJcbiAgICBwcml2YXRlIF9kaXNwYXRjaChtZXNzYWdlOiBBcnJheUJ1ZmZlciwgb2Zmc2V0OiBudW1iZXIpIHtcclxuICAgICAgICBsZXQgeyBtb01lc3NhZ2VPYmplY3Q6IG1vTWVzc2FnZUhlYWRlciwgc2l6ZSB9ID0gRGlzcGF0Y2hlci5yZWFkTWVzc2FnZU9iamVjdChtZXNzYWdlLCBvZmZzZXQsIHRoaXMucGJNZXNzYWdlSGVhZGVyKTtcclxuICAgICAgICBvZmZzZXQgKz0gc2l6ZTtcclxuICAgICAgICB0aGlzLl9fZGlzcGF0Y2gobWVzc2FnZSwgb2Zmc2V0LCBtb01lc3NhZ2VIZWFkZXIpO1xyXG4gICAgfVxyXG5cclxuICAgIHByaXZhdGUgX19kaXNwYXRjaChtZXNzYWdlOiBBcnJheUJ1ZmZlciwgb2Zmc2V0OiBudW1iZXIsIG1vTWVzc2FnZUhlYWRlcjogYW55KSB7XHJcbiAgICAgICAgY29uc3QgRlVOQyA9ICdfX2Rpc3BhdGNoKCknO1xyXG4gICAgICAgIGlmIChtb01lc3NhZ2VIZWFkZXIubmFtZXNwYWNlSWQgPT09IE5BTUVTUEFDRV9JRF9fVW5pdHlCcm93c2VyQ2hhbm5lbCkge1xyXG5cclxuICAgICAgICAgICAgaWYgKG1vTWVzc2FnZUhlYWRlci5jbGFzc0lkID09PSBDTEFTU19JRF9CYXNlTWVzc2FnZXMpIHtcclxuICAgICAgICAgICAgICAgIGlmIChtb01lc3NhZ2VIZWFkZXIubWV0aG9kSWQgPT09IE1FVEhPRF9JRF9SZWNlaXZlU3RyaW5nKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgbGV0IHBiTWVzc2FnZSA9IHRoaXMucGJSb290LnJvb3QubG9va3VwVHlwZSgnR2FvUHJvdG9idWYuU3RyaW5nTWVzc2FnZScpO1xyXG4gICAgICAgICAgICAgICAgICAgIGxldCB7IG1vTWVzc2FnZU9iamVjdDogbW9NZXNzYWdlLCBzaXplIH0gPSBEaXNwYXRjaGVyLnJlYWRNZXNzYWdlT2JqZWN0KG1lc3NhZ2UsIG9mZnNldCwgcGJNZXNzYWdlKTtcclxuICAgICAgICAgICAgICAgICAgICBCYXNlTWVzc2FnZXMucmVjZWl2ZVN0cmluZyhtb01lc3NhZ2Uuc3RyKTtcclxuICAgICAgICAgICAgICAgICAgICBvZmZzZXQgKz0gc2l6ZTtcclxuICAgICAgICAgICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgICAgICAgICAgY29uc29sZS53YXJuKGAke0ZJTEV9OiR7RlVOQ306IHVua25vd24gbWV0aG9kSWQ6ICR7bW9NZXNzYWdlSGVhZGVyLm1ldGhvZElkfWApO1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICAgICAgY29uc29sZS53YXJuKGAke0ZJTEV9OiR7RlVOQ306IHVua25vd24gY2xhc3NJZDogJHttb01lc3NhZ2VIZWFkZXIuY2xhc3NJZH1gKTtcclxuICAgICAgICAgICAgfVxyXG5cclxuICAgICAgICB9IGVsc2UgaWYgKG1vTWVzc2FnZUhlYWRlci5uYW1lc3BhY2VJZCA9PT0gTkFNRVNQQUNFX0lEX19XZWJTb2NrZXQpIHtcclxuXHJcbiAgICAgICAgICAgIGlmIChtb01lc3NhZ2VIZWFkZXIuY2xhc3NJZCA9PT0gQ0xBU1NfSURfQXV0aGVudGljYXRlKSB7XHJcbiAgICAgICAgICAgICAgICBpZiAobW9NZXNzYWdlSGVhZGVyLm1ldGhvZElkID09PSBNRVRIT0RfSURfQXV0aGVudGljYXRlUmVzcG9uc2UpIHtcclxuICAgICAgICAgICAgICAgICAgICBsZXQgcGJBdXRoZW50aWNhdGVSZXNwb25zZSA9IHRoaXMucGJSb290LnJvb3QubG9va3VwVHlwZSgnR2FvUHJvdG9idWYuQXV0aGVudGljYXRlUmVzcG9uc2UnKTtcclxuICAgICAgICAgICAgICAgICAgICBsZXQgeyBtb01lc3NhZ2VPYmplY3Q6IG1vTWVzc2FnZSwgc2l6ZSB9ID0gRGlzcGF0Y2hlci5yZWFkTWVzc2FnZU9iamVjdChtZXNzYWdlLCBvZmZzZXQsIHBiQXV0aGVudGljYXRlUmVzcG9uc2UpO1xyXG4gICAgICAgICAgICAgICAgICAgIFdzQXV0aGVudGljYXRpb24ucmVjZWl2ZUF1dGhlbnRpY2F0ZVJlc3BvbnNlKG1vTWVzc2FnZSwgdGhpcy5wYlJvb3Qucm9vdC5sb29rdXBFbnVtKCdHYW9Qcm90b2J1Zi5BdXRoZW50aWNhdGlvblJlc3VsdCcpKTtcclxuICAgICAgICAgICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgICAgICAgICAgY29uc29sZS53YXJuKGAke0ZJTEV9OiR7RlVOQ306IHVua25vd24gbWV0aG9kSWQ6ICR7bW9NZXNzYWdlSGVhZGVyLm1ldGhvZElkfWApO1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICAgICAgY29uc29sZS53YXJuKGAke0ZJTEV9OiR7RlVOQ306IHVua25vd24gY2xhc3NJZDogJHttb01lc3NhZ2VIZWFkZXIuY2xhc3NJZH1gKTtcclxuICAgICAgICAgICAgfVxyXG5cclxuICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICBjb25zb2xlLndhcm4oYCR7RklMRX06JHtGVU5DfTogdW5rbm93biBuYW1lc3BhY2VJZDogJHttb01lc3NhZ2VIZWFkZXIubmFtZXNwYWNlSWR9YCk7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICB0aGlzLmRpc3Bvc2VSZXF1ZXN0cygpO1xyXG4gICAgfVxyXG5cclxuICAgIHB1YmxpYyBkaXNwYXRjaChtZXNzYWdlOiBBcnJheUJ1ZmZlcikge1xyXG4gICAgICAgIHRyeVxyXG4gICAgICAgIHtcclxuICAgICAgICAgICAgdGhpcy5fZGlzcGF0Y2gobWVzc2FnZSwgMCk7XHJcbiAgICAgICAgfSBjYXRjaCAoZXJyKSB7XHJcbiAgICAgICAgICAgIGNvbnNvbGUuZXJyb3IoYCR7RklMRX06ZGlzcGF0Y2goKTogJHtlcnJ9YCwgZXJyKTtcclxuICAgICAgICAgICAgdGhyb3cgbmV3IEVycm9yKCdkaXNwYXRjaCgpIGZhaWxlZCcpO1xyXG4gICAgICAgIH1cclxuICAgIH1cclxuXHJcbiAgICBwcml2YXRlIGRpc3Bvc2VSZXF1ZXN0cygpIHtcclxuICAgICAgICAvLyBkaXNwb3NlIHJlcXVlc3RzXHJcbiAgICAgICAgV3NBdXRoZW50aWNhdGlvbi5kaXBvc2VSZXF1ZXN0cygpO1xyXG4gICAgfVxyXG5cclxuXHJcbn1cclxuXHJcbiIsIlxyXG5leHBvcnQgZnVuY3Rpb24gZW1pdFVuaXR5TWVzc2FnZUV2ZW50KGpzb246IGFueSk6IHZvaWQge1xyXG4gICAgY29uc3QgZXZlbnQgPSBuZXcgQ3VzdG9tRXZlbnQoXCJ1bml0eV9tZXNzYWdlXCIsIHsgZGV0YWlsOiBqc29ufSk7XHJcbiAgICBkb2N1bWVudC5kaXNwYXRjaEV2ZW50KGV2ZW50KTtcclxufSIsImltcG9ydCB7IFdlYlNvY2tldENsaWVudCB9IGZyb20gJy4uL3dzQ2xpZW50JztcclxuaW1wb3J0IHtcclxuICAgIE5BTUVTUEFDRV9JRF9fV2ViU29ja2V0LFxyXG4gICAgQ0xBU1NfSURfQXV0aGVudGljYXRlLFxyXG4gICAgTUVUSE9EX0lEX0F1dGhlbnRpY2F0ZVJlcXVlc3QsXHJcbiAgICBNRVRIT0RfSURfQXV0aGVudGljYXRlUmVzcG9uc2VcclxufSBmcm9tICcuLi9kaXNwYXRjaGVyJztcclxuaW1wb3J0IHsgRGlzcGF0Y2hlciB9IGZyb20gJy4uL2Rpc3BhdGNoZXInO1xyXG5cclxuaW1wb3J0IHsgdjQgYXMgdXVpZHY0IH0gZnJvbSAndXVpZCc7XHJcblxyXG5cclxuZW51bSBBdXRoZW50aWNhdGlvblN0YXR1cyB7XHJcbiAgICBVTkFVVEhFTlRJQ0FURUQgPSAwLFxyXG4gICAgQVVUSEVOVElDQVRJTkcgPSAxLFxyXG4gICAgQVVUSEVOVElDQVRFRCA9IDIsXHJcbiAgICBFUlJPUiA9IDNcclxuXHJcbn1cclxuXHJcbmV4cG9ydCBjbGFzcyBXc0F1dGhlbnRpY2F0aW9uIHtcclxuICAgIHN0YXRpYyBDTEFTUyA9ICdXc0F1dGhlbnRpY2F0aW9uJztcclxuICAgIGF1dGhlbnRpY2F0aW9uU3RhdHVzOiBBdXRoZW50aWNhdGlvblN0YXR1cyA9IEF1dGhlbnRpY2F0aW9uU3RhdHVzLlVOQVVUSEVOVElDQVRFRDtcclxuXHJcbiAgICBzdGF0aWMgcmVxdWVzdHM6IHtba2V5OiBzdHJpbmddOiBXc0F1dGhlbnRpY2F0aW9ufSA9IHt9O1xyXG4gICAgcHVibGljIHJlcXVlc3RTdGFydEF0OiBEYXRlO1xyXG5cclxuXHJcbiAgICBwcml2YXRlIHdzQ2xpZW50OiBXZWJTb2NrZXRDbGllbnQ7XHJcblxyXG4gICAgY29uc3RydWN0b3Iod3NDbGllbnQ6IFdlYlNvY2tldENsaWVudCkge1xyXG4gICAgICAgIHRoaXMud3NDbGllbnQgPSB3c0NsaWVudDtcclxuICAgIH1cclxuXHJcblxyXG4gICAgcHVibGljIHN0YXRpYyBhdXRoZW50aWNhdGUod3NDbGllbnQ6IFdlYlNvY2tldENsaWVudCwgdG9rZW46IHN0cmluZyk6IHZvaWQge1xyXG4gICAgICAgIGNvbnN0IEZVTkMgPSAnYXV0aGVudGljYXRlKCknO1xyXG4gICAgICAgIGNvbnNvbGUubG9nKGAke1dzQXV0aGVudGljYXRpb24uQ0xBU1N9OiR7RlVOQ306IGF1dGhlbnRpY2F0aW5nLi4uYCk7XHJcblxyXG4gICAgICAgIGxldCB3c0F1dGhlbnRpY2F0aW9uID0gbmV3IFdzQXV0aGVudGljYXRpb24od3NDbGllbnQpO1xyXG4gICAgICAgIHRyeSB7XHJcbiAgICAgICAgICAgIHdzQXV0aGVudGljYXRpb24uYXV0aGVudGljYXRpb25TdGF0dXMgPSBBdXRoZW50aWNhdGlvblN0YXR1cy5BVVRIRU5USUNBVElORztcclxuXHJcbiAgICAgICAgICAgIGxldCBwYk1lc3NhZ2VIZWFkZXIgPSBXZWJTb2NrZXRDbGllbnQuZ1BiUm9vdC5yb290Lmxvb2t1cFR5cGUoJ0dhb1Byb3RvYnVmLk1lc3NhZ2VIZWFkZXInKTtcclxuICAgICAgICAgICAgbGV0IHBiQXV0aGVudGljYXRlUmVxdWVzdCA9IFdlYlNvY2tldENsaWVudC5nUGJSb290LnJvb3QubG9va3VwVHlwZSgnR2FvUHJvdG9idWYuQXV0aGVudGljYXRlUmVxdWVzdCcpO1xyXG5cclxuICAgICAgICAgICAgbGV0IG1vTWVzc2FnZUhlYWRlciA9IHBiTWVzc2FnZUhlYWRlci5jcmVhdGUoe25hbWVzcGFjZUlkOiBOQU1FU1BBQ0VfSURfX1dlYlNvY2tldCwgY2xhc3NJZDogQ0xBU1NfSURfQXV0aGVudGljYXRlLCBtZXRob2RJZDogTUVUSE9EX0lEX0F1dGhlbnRpY2F0ZVJlcXVlc3R9KTtcclxuICAgICAgICAgICAgbGV0IG1vQXV0aGVudGljYXRlUmVxdWVzdCA9IHBiQXV0aGVudGljYXRlUmVxdWVzdC5jcmVhdGUoe1xyXG4gICAgICAgICAgICAgICAgdG9rZW46IHRva2VuLFxyXG4gICAgICAgICAgICAgICAgcmVxdWVzdElkOiB1dWlkdjQoKVxyXG4gICAgICAgICAgICB9KTtcclxuXHJcbiAgICAgICAgICAgIHdzQXV0aGVudGljYXRpb24ucmVxdWVzdFN0YXJ0QXQgPSBuZXcgRGF0ZSgpO1xyXG4gICAgICAgICAgICBXc0F1dGhlbnRpY2F0aW9uLnJlcXVlc3RzW21vQXV0aGVudGljYXRlUmVxdWVzdC5yZXF1ZXN0SWRdID0gd3NBdXRoZW50aWNhdGlvbjtcclxuXHJcblxyXG4gICAgICAgICAgICAvLyBlbmNvZGUgbWVzc2FnZSBoZWFkZXJcclxuICAgICAgICAgICAgbGV0IGRhdGFNZXNzYWdlSGVhZGVyID0gRGlzcGF0Y2hlci5lbmNvZGVNZXNzYWdlT2JqZWN0KHBiTWVzc2FnZUhlYWRlciwgbW9NZXNzYWdlSGVhZGVyKTtcclxuICAgICAgICAgICAgbGV0IGRhdGFBdXRoZW50aWNhdGVSZXF1ZXN0ID0gRGlzcGF0Y2hlci5lbmNvZGVNZXNzYWdlT2JqZWN0KHBiQXV0aGVudGljYXRlUmVxdWVzdCwgbW9BdXRoZW50aWNhdGVSZXF1ZXN0KTtcclxuXHJcbiAgICAgICAgICAgIC8vIGNvbmNhdGVuYXRlIG1lc3NhZ2UgaGVhZGVyIGFuZCBtZXNzYWdlIHN0cmluZ1xyXG4gICAgICAgICAgICBsZXQgZGF0YSA9IG5ldyBVaW50OEFycmF5KGRhdGFNZXNzYWdlSGVhZGVyLmJ5dGVMZW5ndGggKyBkYXRhQXV0aGVudGljYXRlUmVxdWVzdC5ieXRlTGVuZ3RoKTtcclxuICAgICAgICAgICAgZGF0YS5zZXQobmV3IFVpbnQ4QXJyYXkoZGF0YU1lc3NhZ2VIZWFkZXIpLCAwKTtcclxuICAgICAgICAgICAgZGF0YS5zZXQobmV3IFVpbnQ4QXJyYXkoZGF0YUF1dGhlbnRpY2F0ZVJlcXVlc3QpLCBkYXRhTWVzc2FnZUhlYWRlci5ieXRlTGVuZ3RoKTtcclxuXHJcbiAgICAgICAgICAgIHdzQ2xpZW50LnNlbmQobW9NZXNzYWdlSGVhZGVyLCBkYXRhLmJ1ZmZlcik7XHJcbiAgICAgICAgfSBjYXRjaCAoZXJyKSB7XHJcbiAgICAgICAgICAgIHdzQXV0aGVudGljYXRpb24uYXV0aGVudGljYXRpb25TdGF0dXMgPSBBdXRoZW50aWNhdGlvblN0YXR1cy5FUlJPUjtcclxuICAgICAgICAgICAgY29uc29sZS5lcnJvcihgJHtXc0F1dGhlbnRpY2F0aW9uLkNMQVNTfToke0ZVTkN9OiAke2Vycn1gKTtcclxuICAgICAgICB9XHJcblxyXG4gICAgfVxyXG5cclxuICAgIHN0YXRpYyByZWNlaXZlQXV0aGVudGljYXRlUmVzcG9uc2UobW9BdXRoZW50aWNhdGVSZXFwb25zZTogYW55LCBwYkF1dGhlbnRpY2F0aW9uUmVzdWx0RW51bTogYW55KTogdm9pZCB7XHJcbiAgICAgICAgY29uc3QgRlVOQyA9ICdyZWNlaXZlQXV0aGVudGljYXRlUmVzcG9uc2UoKSc7XHJcbiAgICAgICAgbGV0IHJlcXVlc3RJZCA9IG1vQXV0aGVudGljYXRlUmVxcG9uc2UucmVxdWVzdElkO1xyXG5cclxuICAgICAgICAvLyBmaW5kIHRoZSByZXF1ZXN0XHJcbiAgICAgICAgbGV0IHJlcXVlc3QgPSBXc0F1dGhlbnRpY2F0aW9uLnJlcXVlc3RzW3JlcXVlc3RJZF07XHJcbiAgICAgICAgaWYgKHJlcXVlc3QpIHtcclxuICAgICAgICAgICAgaWYgKG1vQXV0aGVudGljYXRlUmVxcG9uc2UucmVzdWx0ID09IHBiQXV0aGVudGljYXRpb25SZXN1bHRFbnVtLnZhbHVlcy5zdWNjZXNzKSB7XHJcbiAgICAgICAgICAgICAgICByZXF1ZXN0LmF1dGhlbnRpY2F0aW9uU3RhdHVzID0gQXV0aGVudGljYXRpb25TdGF0dXMuQVVUSEVOVElDQVRFRDtcclxuICAgICAgICAgICAgICAgIGNvbnNvbGUubG9nKGAke1dzQXV0aGVudGljYXRpb24uQ0xBU1N9OiR7RlVOQ306IGF0aGVudGljYXRlZGApO1xyXG4gICAgICAgICAgICAgICAgcmVxdWVzdC53c0NsaWVudC5zZXRBdXRoZW50aWNhdGVkKCk7XHJcbiAgICAgICAgICAgIH0gZWxzZSBpZiAobW9BdXRoZW50aWNhdGVSZXFwb25zZS5yZXN1bHQgPT0gcGJBdXRoZW50aWNhdGlvblJlc3VsdEVudW0udmFsdWVzLnVuYXV0aG9yaXplZCkge1xyXG4gICAgICAgICAgICAgICAgcmVxdWVzdC5hdXRoZW50aWNhdGlvblN0YXR1cyA9IEF1dGhlbnRpY2F0aW9uU3RhdHVzLlVOQVVUSEVOVElDQVRFRFxyXG4gICAgICAgICAgICB9IGVsc2UgaWYgKG1vQXV0aGVudGljYXRlUmVxcG9uc2UucmVzdWx0ID09IHBiQXV0aGVudGljYXRpb25SZXN1bHRFbnVtLnZhbHVlcy5lcnJvcikge1xyXG4gICAgICAgICAgICAgICAgcmVxdWVzdC5hdXRoZW50aWNhdGlvblN0YXR1cyA9IEF1dGhlbnRpY2F0aW9uU3RhdHVzLkVSUk9SO1xyXG4gICAgICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICAgICAgY29uc29sZS53YXJuKGAke1dzQXV0aGVudGljYXRpb24uQ0xBU1N9OiR7RlVOQ306IHVua25vd24gcmVzdWx0OiAke21vQXV0aGVudGljYXRlUmVxcG9uc2UucmVzdWx0fWApO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIGRlbGV0ZSBXc0F1dGhlbnRpY2F0aW9uLnJlcXVlc3RzW3JlcXVlc3RJZF07XHJcbiAgICAgICAgfVxyXG4gICAgICAgIGVsc2Uge1xyXG4gICAgICAgICAgICBjb25zb2xlLndhcm4oYCR7V3NBdXRoZW50aWNhdGlvbi5DTEFTU306JHtGVU5DfTogcmVxdWVzdCBub3QgZm91bmRgKTtcclxuICAgICAgICB9XHJcbiAgICB9XHJcblxyXG4gICAgc3RhdGljIGRpcG9zZVJlcXVlc3RzKCk6IHZvaWQge1xyXG4gICAgICAgIGNvbnN0IEZVTkMgPSAnZGlwb3NlUmVxdWVzdHMoKSc7XHJcbiAgICAgICAgLy8gZGlzcG9zZSByZXF1ZXN0cyB0aGF0IGFyZSBvbGRlciB0aGFuIDEwIHNlY29uZHMgb3IgaGF2ZSBzdGF0dXMgb3RoZXIgdGhhbiBBVVRIRU5USUNBVElOR1xyXG4gICAgICAgIGxldCBub3cgPSBuZXcgRGF0ZSgpO1xyXG4gICAgICAgIGZvciAobGV0IGtleSBpbiBXc0F1dGhlbnRpY2F0aW9uLnJlcXVlc3RzKSB7XHJcbiAgICAgICAgICAgIGxldCByZXF1ZXN0ID0gV3NBdXRoZW50aWNhdGlvbi5yZXF1ZXN0c1trZXldO1xyXG4gICAgICAgICAgICBpZiAobm93LmdldFRpbWUoKSAtIHJlcXVlc3QucmVxdWVzdFN0YXJ0QXQuZ2V0VGltZSgpID4gMTAwMDApIHsgXHJcbiAgICAgICAgICAgICAgICAgIGlmIChyZXF1ZXN0LmF1dGhlbnRpY2F0aW9uU3RhdHVzICE9IEF1dGhlbnRpY2F0aW9uU3RhdHVzLkFVVEhFTlRJQ0FUSU5HKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgY29uc29sZS53YXJuKGAke1dzQXV0aGVudGljYXRpb24uQ0xBU1N9OiR7RlVOQ306IHJlcXVlc3QgdGltZWQgb3V0OiAke2tleX1gKTtcclxuICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgZGVsZXRlIFdzQXV0aGVudGljYXRpb24ucmVxdWVzdHNba2V5XTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuICAgIH1cclxuXHJcblxyXG59IiwiXHJcbmltcG9ydCB7IFdlYlNvY2tldENsaWVudCB9IGZyb20gJy4uLy4uL3dzQ2xpZW50JztcclxuaW1wb3J0IHsgUHJvdG9idWZSb290IH0gZnJvbSAnLi4vLi4vcHJvdG8nO1xyXG5pbXBvcnQgeyBEaXNwYXRjaGVyIH0gZnJvbSAnLi4vLi4vZGlzcGF0Y2hlcic7XHJcbmltcG9ydCB7XHJcbiAgICBOQU1FU1BBQ0VfSURfX1VuaXR5QnJvd3NlckNoYW5uZWwsXHJcbiAgICBDTEFTU19JRF9CYXNlTWVzc2FnZXMsXHJcbiAgICBNRVRIT0RfSURfUmVjZWl2ZVN0cmluZ1xyXG59IGZyb20gJy4uLy4uL2Rpc3BhdGNoZXInO1xyXG5pbXBvcnQgeyBlbWl0VW5pdHlNZXNzYWdlRXZlbnQgfSBmcm9tICcuLi8uLi9ldmVudHMnO1xyXG5cclxuZXhwb3J0IGNsYXNzIEJhc2VNZXNzYWdlc1xyXG57XHJcbiAgICBzdGF0aWMgIENMQVNTX05BTUUgPSAnQmFzZU1lc3NhZ2VzJztcclxuICAgIHN0YXRpYyByZWNlaXZlU3RyaW5nKHN0cjogc3RyaW5nKTogdm9pZCB7XHJcbiAgICAgICAgY29uc3QgRlVOQyA9ICdyZWNlaXZlU3RyaW5nKCknO1xyXG4gICAgICAgIGNvbnNvbGUubG9nKGAke0Jhc2VNZXNzYWdlcy5DTEFTU19OQU1FfToke0ZVTkN9OiBpbiBicm93c2VyOiAke3N0cn1gKTtcclxuICAgICAgICB0cnkge1xyXG4gICAgICAgICAgICBsZXQganNvbiA9IEpTT04ucGFyc2Uoc3RyKTtcclxuICAgICAgICAgICAgZW1pdFVuaXR5TWVzc2FnZUV2ZW50KGpzb24pO1xyXG4gICAgICAgIH0gY2F0Y2ggKGVycikge1xyXG4gICAgICAgICAgICBjb25zb2xlLmVycm9yKGAke0Jhc2VNZXNzYWdlcy5DTEFTU19OQU1FfToke0ZVTkN9OiBpbiBicm93c2VyOiAke2Vycn1gKTtcclxuICAgICAgICB9XHJcbiAgICB9XHJcblxyXG4gICAgc3RhdGljIHNlbmRTdHJpbmcoc3RyOiBzdHJpbmcpOiB2b2lkIHtcclxuICAgICAgICBjb25zdCBGVU5DID0gJ3NlbmRTdHJpbmcoKSc7XHJcbiAgICAgICAgdHJ5XHJcbiAgICAgICAge1xyXG4gICAgICAgICAgICBpZiAoKHdpbmRvdyBhcyBhbnkpLkdBT19Vbml0eUJyb3dzZXJDaGFubmVsKSB7XHJcbiAgICAgICAgICAgICAgICAvLyB1bml0eSBpcyBydW5uaW5nIGluIGJyb3dzZXIsY2FsbCB1bml0eSBkaXJlY3RseVxyXG4gICAgICAgICAgICAgICAgdHJ5XHJcbiAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWYgKCEod2luZG93IGFzIGFueSkuR0FPX1VuaXR5QnJvd3NlckNoYW5uZWwpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgdGhyb3cgbmV3IEVycm9yKCd3aW5kb3cuR0FPX1VuaXR5QnJvd3NlckNoYW5uZWwgaXMgbm90IGRlZmluZWQnKTtcclxuICAgICAgICAgICAgICAgICAgICB9IFxyXG4gICAgICAgICAgICAgICAgICAgIGlmICghKHdpbmRvdyBhcyBhbnkpLkdBT19Vbml0eUJyb3dzZXJDaGFubmVsLkJhc2VNZXNzYWdlcykge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICB0aHJvdyBuZXcgRXJyb3IoJ3dpbmRvdy5HQU9fVW5pdHlCcm93c2VyQ2hhbm5lbC5CYXNlTWVzc2FnZXMgaXMgbm90IGRlZmluZWQnKTtcclxuICAgICAgICAgICAgICAgICAgICB9IFxyXG4gICAgICAgICAgICAgICAgICAgIGlmICghKHdpbmRvdyBhcyBhbnkpLkdBT19Vbml0eUJyb3dzZXJDaGFubmVsLkJhc2VNZXNzYWdlcy5zZW5kU3RyaW5nKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHRocm93IG5ldyBFcnJvcignd2luZG93LkdBT19Vbml0eUJyb3dzZXJDaGFubmVsLkJhc2VNZXNzYWdlcy5zZW5kU3RyaW5nIGlzIG5vdCBkZWZpbmVkJyk7XHJcbiAgICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgICAgICh3aW5kb3cgYXMgYW55KS5HQU9fVW5pdHlCcm93c2VyQ2hhbm5lbC5CYXNlTWVzc2FnZXMuc2VuZFN0cmluZyhzdHIpO1xyXG4gICAgICAgICAgICAgICAgfSBjYXRjaCAoZXJyKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgY29uc29sZS5lcnJvcihgJHtCYXNlTWVzc2FnZXMuQ0xBU1NfTkFNRX06JHtGVU5DfTogaW4gYnJvd3NlcjogZXJyb3Igc2VuZGluZyBkaXJlY3RseSwgJHtlcnJ9YCk7XHJcbiAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgICAgICB0cnkge1xyXG4gICAgICAgICAgICAgICAgICAgIC8vIHVuaXR5IGlzIHJ1bm5pbmcgaW4gZWRpdG9yLCBjYWxsIHdzQ2xpZW50XHJcblxyXG4gICAgICAgICAgICAgICAgICAgIGxldCBkaXNwYXRjaGVyOiBEaXNwYXRjaGVyID0gV2ViU29ja2V0Q2xpZW50LmdXc0NsaWVudC5kaXNwYXRjaGVyO1xyXG5cclxuICAgICAgICAgICAgICAgICAgICBsZXQgcGJNZXNzYWdlSGVhZGVyID0gV2ViU29ja2V0Q2xpZW50LmdQYlJvb3Qucm9vdC5sb29rdXBUeXBlKCdHYW9Qcm90b2J1Zi5NZXNzYWdlSGVhZGVyJyk7XHJcbiAgICAgICAgICAgICAgICAgICAgbGV0IHBiU3RyaW5nTWVzc2FnZSA9IFdlYlNvY2tldENsaWVudC5nUGJSb290LnJvb3QubG9va3VwVHlwZSgnR2FvUHJvdG9idWYuU3RyaW5nTWVzc2FnZScpO1xyXG5cclxuICAgICAgICAgICAgICAgICAgICBsZXQgbW9NZXNzYWdlSGVhZGVyID0gcGJNZXNzYWdlSGVhZGVyLmNyZWF0ZSh7bmFtZXNwYWNlSWQ6IE5BTUVTUEFDRV9JRF9fVW5pdHlCcm93c2VyQ2hhbm5lbCwgY2xhc3NJZDogQ0xBU1NfSURfQmFzZU1lc3NhZ2VzLCBtZXRob2RJZDogTUVUSE9EX0lEX1JlY2VpdmVTdHJpbmd9KTtcclxuICAgICAgICAgICAgICAgICAgICBsZXQgbW9TdHJpbmdNZXNzYWdlID0gcGJTdHJpbmdNZXNzYWdlLmNyZWF0ZSh7c3RyOiBzdHJ9KTtcclxuXHJcbiAgICAgICAgICAgICAgICAgICAgLy8gZW5jb2RlIG1lc3NhZ2UgaGVhZGVyXHJcbiAgICAgICAgICAgICAgICAgICAgbGV0IGRhdGFNZXNzYWdlSGVhZGVyID0gRGlzcGF0Y2hlci5lbmNvZGVNZXNzYWdlT2JqZWN0KHBiTWVzc2FnZUhlYWRlciwgbW9NZXNzYWdlSGVhZGVyKTtcclxuICAgICAgICAgICAgICAgICAgICBsZXQgZGF0YVN0cmluZ01lc3NhZ2UgPSBEaXNwYXRjaGVyLmVuY29kZU1lc3NhZ2VPYmplY3QocGJTdHJpbmdNZXNzYWdlLCBtb1N0cmluZ01lc3NhZ2UpO1xyXG5cclxuICAgICAgICAgICAgICAgICAgICAvLyBjb25jYXRlbmF0ZSBtZXNzYWdlIGhlYWRlciBhbmQgbWVzc2FnZSBzdHJpbmdcclxuICAgICAgICAgICAgICAgICAgICBsZXQgZGF0YSA9IG5ldyBVaW50OEFycmF5KGRhdGFNZXNzYWdlSGVhZGVyLmJ5dGVMZW5ndGggKyBkYXRhU3RyaW5nTWVzc2FnZS5ieXRlTGVuZ3RoKTtcclxuICAgICAgICAgICAgICAgICAgICBkYXRhLnNldChuZXcgVWludDhBcnJheShkYXRhTWVzc2FnZUhlYWRlciksIDApO1xyXG4gICAgICAgICAgICAgICAgICAgIGRhdGEuc2V0KG5ldyBVaW50OEFycmF5KGRhdGFTdHJpbmdNZXNzYWdlKSwgZGF0YU1lc3NhZ2VIZWFkZXIuYnl0ZUxlbmd0aCk7XHJcblxyXG4gICAgICAgICAgICAgICAgICAgIFdlYlNvY2tldENsaWVudC5nV3NDbGllbnQuc2VuZChtb01lc3NhZ2VIZWFkZXIsIGRhdGEuYnVmZmVyKTtcclxuICAgICAgICAgICAgICAgIH0gY2F0Y2ggKGVycikge1xyXG4gICAgICAgICAgICAgICAgICAgIGNvbnNvbGUuZXJyb3IoYCR7QmFzZU1lc3NhZ2VzLkNMQVNTX05BTUV9OiR7RlVOQ306IGluIGJyb3dzZXI6IGVycm9yIHNlbmRpbmcgdmlhIHdzQ2xpZW50LCAke2Vycn1gKTtcclxuICAgICAgICAgICAgICAgIH0gICBcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH0gY2F0Y2ggKGVycikge1xyXG4gICAgICAgICAgICBjb25zb2xlLmVycm9yKGAke0Jhc2VNZXNzYWdlcy5DTEFTU19OQU1FfToke0ZVTkN9OiBpbiBicm93c2VyOiAke2Vycn1gKTtcclxuICAgICAgICB9XHJcbiAgICB9XHJcbn1cclxuIiwiaW1wb3J0IHB0b3RvYnVmSnNvbkRlc2NyaXB0b3IgZnJvbSAnLi9idW5kbGUuanNvbic7XHJcbmltcG9ydCAqIGFzIHByb3RvYnVmIGZyb20gJ3Byb3RvYnVmanMnO1xyXG5cclxuLypcclxuZXhwb3J0IGludGVyZmFjZSBJUHJvdG9idWZSb290IHtcclxuICAgIHJvb3Q6IGFueTtcclxufSBcclxuXHJcblxyXG5leHBvcnQgZnVuY3Rpb24gZ2V0UHJvdG9idWZSb290KCk6IElQcm90b2J1ZlJvb3Qge1xyXG4gICAgcmV0dXJue1xyXG4gICAgICAgIHJvb3Q6IHByb3RvYnVmLlJvb3QuZnJvbUpTT04ocHRvdG9idWZKc29uRGVzY3JpcHRvcilcclxuICAgIH1cclxufVxyXG4gICAgKi9cclxuXHJcbmV4cG9ydCBjbGFzcyBQcm90b2J1ZlJvb3Qge1xyXG4gICAgcHVibGljIHJvb3Q6IGFueTtcclxuICAgIGNvbnN0cnVjdG9yKCkge1xyXG4gICAgICAgIHRoaXMucm9vdCA9IHByb3RvYnVmLlJvb3QuZnJvbUpTT04ocHRvdG9idWZKc29uRGVzY3JpcHRvcik7XHJcbiAgICB9XHJcbn0iLCJpbXBvcnQgeyBQcm90b2J1ZlJvb3QgfSBmcm9tIFwiLi9wcm90b1wiO1xyXG5pbXBvcnQgeyBEaXNwYXRjaGVyIH0gZnJvbSBcIi4vZGlzcGF0Y2hlclwiO1xyXG5pbXBvcnQgeyBXc0F1dGhlbnRpY2F0aW9uIH0gZnJvbSBcIi4vbWVzc2FnZXMvV3NBdXRoZW50aWNhdGlvblwiO1xyXG5pbXBvcnQgeyBOQU1FU1BBQ0VfSURfX1VuaXR5QnJvd3NlckNoYW5uZWwgfSBmcm9tIFwiLi9kaXNwYXRjaGVyXCI7XHJcbmltcG9ydCB7IFdFQlNPQ0tFVF9TRVJWRVJfVVJMIH0gZnJvbSBcIi4vY29uZmlnXCJcclxuXHJcblxyXG5leHBvcnQgY2xhc3MgV2ViU29ja2V0Q2xpZW50IFxyXG57XHJcbiAgc3RhdGljIENMQVNTID0gJ1dlYlNvY2tldENsaWVudCc7XHJcblxyXG4gIHdzOiBhbnk7XHJcbiAgZGlzcGF0Y2hlcjogRGlzcGF0Y2hlcjtcclxuICBwcml2YXRlIGlzQXV0aGVudGljYXRlZDogYm9vbGVhblxyXG5cclxuICBzdGF0aWMgZ1BiUm9vdDogUHJvdG9idWZSb290ID0gbmV3IFByb3RvYnVmUm9vdCgpOyBcclxuICBzdGF0aWMgZ1dzQ2xpZW50OiBXZWJTb2NrZXRDbGllbnQgPSBuZXcgV2ViU29ja2V0Q2xpZW50KCk7XHJcbiAgc3RhdGljIGp3dFRva2VuOiBzdHJpbmcgPSBudWxsO1xyXG5cclxuICBjb25zdHJ1Y3RvcigpIHtcclxuICAgIHRoaXMuZGlzcGF0Y2hlciA9IG51bGw7XHJcbiAgICB0aGlzLmlzQXV0aGVudGljYXRlZCA9IGZhbHNlO1xyXG4gIH1cclxuXHJcbiAgc3RhdGljIHNldEp3dFRva2VuKHRva2VuOiBzdHJpbmcpIHsgXHJcbiAgICBXZWJTb2NrZXRDbGllbnQuand0VG9rZW4gPSB0b2tlbjtcclxuICAgIGlmIChXZWJTb2NrZXRDbGllbnQuZ1dzQ2xpZW50KSB7XHJcbiAgICAgIFdzQXV0aGVudGljYXRpb24uYXV0aGVudGljYXRlKFdlYlNvY2tldENsaWVudC5nV3NDbGllbnQsIFdlYlNvY2tldENsaWVudC5qd3RUb2tlbik7XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICBzdGFydCgpOiB2b2lkIHtcclxuXHJcbiAgICB0aGlzLndzID0gbmV3IFdlYlNvY2tldChXRUJTT0NLRVRfU0VSVkVSX1VSTCk7XHJcbiAgICB0aGlzLndzLmJpbmFyeVR5cGUgPSAnYXJyYXlidWZmZXInO1xyXG4gICAgdGhpcy5pc0F1dGhlbnRpY2F0ZWQgPSBmYWxzZTtcclxuXHJcbiAgICB0aGlzLndzLm9ub3BlbiA9ICgpID0+IHtcclxuICAgICAgY29uc29sZS5sb2coJ2Nvbm5lY3RlZCcpO1xyXG4gICAgICB0aGlzLmRpc3BhdGNoZXIgPSBuZXcgRGlzcGF0Y2hlcihXZWJTb2NrZXRDbGllbnQuZ1BiUm9vdCwgV2ViU29ja2V0Q2xpZW50LmdXc0NsaWVudCk7XHJcbiAgICAgIGlmIChXZWJTb2NrZXRDbGllbnQuand0VG9rZW4pIHtcclxuICAgICAgICBXc0F1dGhlbnRpY2F0aW9uLmF1dGhlbnRpY2F0ZSh0aGlzLCBXZWJTb2NrZXRDbGllbnQuand0VG9rZW4pO1xyXG4gICAgICB9XHJcbiAgICB9O1xyXG5cclxuICAgIHRoaXMud3Mub25tZXNzYWdlID0gKGU6IGFueSkgPT4ge1xyXG4gICAgICBjb25zdCBGVU5DID0gJ29ubWVzc2FnZSgpJztcclxuICAgICAgaWYgKGUuZGF0YSBpbnN0YW5jZW9mIEFycmF5QnVmZmVyKSB7XHJcbiAgICAgICAgaWYgKHRoaXMuZGlzcGF0Y2hlcikge1xyXG4gICAgICAgICAgdGhpcy5kaXNwYXRjaGVyLmRpc3BhdGNoKGUuZGF0YSk7XHJcbiAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgIGNvbnNvbGUud2FybihgJHtXZWJTb2NrZXRDbGllbnQuQ0xBU1N9OiR7RlVOQ306IGRpc3BhdGNoZXIgbm90IHJlYWR5LCBtZXNzYWdlIGlnbm9yZWRgKTtcclxuICAgICAgICB9XHJcbiAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgY29uc29sZS5lcnJvcihgJHtXZWJTb2NrZXRDbGllbnQuQ0xBU1N9OiR7RlVOQ306IHJlY2VpdmVkIGEgbm9uLWFycmF5YnVmZmVyIG1lc3NhZ2UsIGlnbm9yZWRgKVxyXG4gICAgICB9XHJcbiAgICB9O1xyXG5cclxuICAgIHRoaXMud3Mub25jbG9zZSA9ICgpID0+IHtcclxuICAgICAgY29uc29sZS5sb2coJ2Rpc2Nvbm5lY3RlZCcpO1xyXG4gICAgICBzZXRUaW1lb3V0KCgpID0+IHtcclxuICAgICAgICBXZWJTb2NrZXRDbGllbnQuZ1dzQ2xpZW50ID0gbmV3IFdlYlNvY2tldENsaWVudCgpO1xyXG4gICAgICAgIFdlYlNvY2tldENsaWVudC5nV3NDbGllbnQuc3RhcnQoKTtcclxuICAgICAgfSwgOTAwMClcclxuICAgIH07XHJcblxyXG4gIH07XHJcblxyXG4gIGNoZWNrSWZBdXRoZW50aWNhdGVkVG9TZW5kKG1vTWVzc2FnZUhlYWRlcjogYW55KTogYm9vbGVhbiB7XHJcbiAgICBpZiAobW9NZXNzYWdlSGVhZGVyLm5hbWVzcGFjZUlkID09PSBOQU1FU1BBQ0VfSURfX1VuaXR5QnJvd3NlckNoYW5uZWwpIHtcclxuICAgICAgcmV0dXJuIHRoaXMuaXNBdXRoZW50aWNhdGVkO1xyXG4gICAgfSBlbHNlIHtcclxuICAgICAgcmV0dXJuIHRydWU7XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICBzZW5kKG1vTWVzc2FnZUhlYWRlcjogYW55LCBtZXNzYWdlOiBBcnJheUJ1ZmZlcik6IHZvaWQge1xyXG4gICAgY29uc3QgRlVOQyA9ICdzZW5kKCknO1xyXG4gICAgaWYgKCF0aGlzLmNoZWNrSWZBdXRoZW50aWNhdGVkVG9TZW5kKG1vTWVzc2FnZUhlYWRlcikpIHtcclxuICAgICAgY29uc29sZS53YXJuKGAke1dlYlNvY2tldENsaWVudC5DTEFTU306JHtGVU5DfTogY2Fubm90IHNlbmQgYSBtZXNzYWdlLCB3ZWJzb2NrZXQgaXMgbm90IGF1dGhlbnRpY2F0ZWQsIG1lc3NhZ2UgaWdub3JlZGApO1xyXG4gICAgICByZXR1cm47XHJcbiAgICB9XHJcbiAgICBpZiAodGhpcy53cy5yZWFkeVN0YXRlICE9PSBXZWJTb2NrZXQuT1BFTikge1xyXG4gICAgICAgIGNvbnNvbGUud2FybihgJHtXZWJTb2NrZXRDbGllbnQuQ0xBU1N9OiR7RlVOQ306IHdzIGlzIG5vdCBvcGVuLCBtZXNzYWdlIGlnbm9yZWRgKTtcclxuICAgICAgICByZXR1cm47XHJcbiAgICB9XHJcbiAgICB0aGlzLndzLnNlbmQobWVzc2FnZSk7XHJcbiAgfVxyXG5cclxuICBzZXRBdXRoZW50aWNhdGVkKCk6IHZvaWQge1xyXG4gICAgdGhpcy5pc0F1dGhlbnRpY2F0ZWQgPSB0cnVlO1xyXG4gIH1cclxuXHJcbiAgZ2V0SXNBdXRoZW50aWNhdGVkKCk6IGJvb2xlYW4ge1xyXG4gICAgcmV0dXJuIHRoaXMuaXNBdXRoZW50aWNhdGVkO1xyXG4gIH1cclxufVxyXG4iLCJ2YXIgcmFuZG9tVVVJRCA9IHR5cGVvZiBjcnlwdG8gIT09ICd1bmRlZmluZWQnICYmIGNyeXB0by5yYW5kb21VVUlEICYmIGNyeXB0by5yYW5kb21VVUlELmJpbmQoY3J5cHRvKTtcbmV4cG9ydCBkZWZhdWx0IHtcbiAgcmFuZG9tVVVJRFxufTsiLCJleHBvcnQgZGVmYXVsdCAvXig/OlswLTlhLWZdezh9LVswLTlhLWZdezR9LVsxLThdWzAtOWEtZl17M30tWzg5YWJdWzAtOWEtZl17M30tWzAtOWEtZl17MTJ9fDAwMDAwMDAwLTAwMDAtMDAwMC0wMDAwLTAwMDAwMDAwMDAwMHxmZmZmZmZmZi1mZmZmLWZmZmYtZmZmZi1mZmZmZmZmZmZmZmYpJC9pOyIsIi8vIFVuaXF1ZSBJRCBjcmVhdGlvbiByZXF1aXJlcyBhIGhpZ2ggcXVhbGl0eSByYW5kb20gIyBnZW5lcmF0b3IuIEluIHRoZSBicm93c2VyIHdlIHRoZXJlZm9yZVxuLy8gcmVxdWlyZSB0aGUgY3J5cHRvIEFQSSBhbmQgZG8gbm90IHN1cHBvcnQgYnVpbHQtaW4gZmFsbGJhY2sgdG8gbG93ZXIgcXVhbGl0eSByYW5kb20gbnVtYmVyXG4vLyBnZW5lcmF0b3JzIChsaWtlIE1hdGgucmFuZG9tKCkpLlxuXG52YXIgZ2V0UmFuZG9tVmFsdWVzO1xudmFyIHJuZHM4ID0gbmV3IFVpbnQ4QXJyYXkoMTYpO1xuZXhwb3J0IGRlZmF1bHQgZnVuY3Rpb24gcm5nKCkge1xuICAvLyBsYXp5IGxvYWQgc28gdGhhdCBlbnZpcm9ubWVudHMgdGhhdCBuZWVkIHRvIHBvbHlmaWxsIGhhdmUgYSBjaGFuY2UgdG8gZG8gc29cbiAgaWYgKCFnZXRSYW5kb21WYWx1ZXMpIHtcbiAgICAvLyBnZXRSYW5kb21WYWx1ZXMgbmVlZHMgdG8gYmUgaW52b2tlZCBpbiBhIGNvbnRleHQgd2hlcmUgXCJ0aGlzXCIgaXMgYSBDcnlwdG8gaW1wbGVtZW50YXRpb24uXG4gICAgZ2V0UmFuZG9tVmFsdWVzID0gdHlwZW9mIGNyeXB0byAhPT0gJ3VuZGVmaW5lZCcgJiYgY3J5cHRvLmdldFJhbmRvbVZhbHVlcyAmJiBjcnlwdG8uZ2V0UmFuZG9tVmFsdWVzLmJpbmQoY3J5cHRvKTtcbiAgICBpZiAoIWdldFJhbmRvbVZhbHVlcykge1xuICAgICAgdGhyb3cgbmV3IEVycm9yKCdjcnlwdG8uZ2V0UmFuZG9tVmFsdWVzKCkgbm90IHN1cHBvcnRlZC4gU2VlIGh0dHBzOi8vZ2l0aHViLmNvbS91dWlkanMvdXVpZCNnZXRyYW5kb212YWx1ZXMtbm90LXN1cHBvcnRlZCcpO1xuICAgIH1cbiAgfVxuICByZXR1cm4gZ2V0UmFuZG9tVmFsdWVzKHJuZHM4KTtcbn0iLCJpbXBvcnQgdmFsaWRhdGUgZnJvbSAnLi92YWxpZGF0ZS5qcyc7XG5cbi8qKlxuICogQ29udmVydCBhcnJheSBvZiAxNiBieXRlIHZhbHVlcyB0byBVVUlEIHN0cmluZyBmb3JtYXQgb2YgdGhlIGZvcm06XG4gKiBYWFhYWFhYWC1YWFhYLVhYWFgtWFhYWC1YWFhYWFhYWFhYWFhcbiAqL1xudmFyIGJ5dGVUb0hleCA9IFtdO1xuZm9yICh2YXIgaSA9IDA7IGkgPCAyNTY7ICsraSkge1xuICBieXRlVG9IZXgucHVzaCgoaSArIDB4MTAwKS50b1N0cmluZygxNikuc2xpY2UoMSkpO1xufVxuZXhwb3J0IGZ1bmN0aW9uIHVuc2FmZVN0cmluZ2lmeShhcnIsIG9mZnNldCA9IDApIHtcbiAgLy8gTm90ZTogQmUgY2FyZWZ1bCBlZGl0aW5nIHRoaXMgY29kZSEgIEl0J3MgYmVlbiB0dW5lZCBmb3IgcGVyZm9ybWFuY2VcbiAgLy8gYW5kIHdvcmtzIGluIHdheXMgeW91IG1heSBub3QgZXhwZWN0LiBTZWUgaHR0cHM6Ly9naXRodWIuY29tL3V1aWRqcy91dWlkL3B1bGwvNDM0XG4gIC8vXG4gIC8vIE5vdGUgdG8gZnV0dXJlLXNlbGY6IE5vLCB5b3UgY2FuJ3QgcmVtb3ZlIHRoZSBgdG9Mb3dlckNhc2UoKWAgY2FsbC5cbiAgLy8gUkVGOiBodHRwczovL2dpdGh1Yi5jb20vdXVpZGpzL3V1aWQvcHVsbC82NzcjaXNzdWVjb21tZW50LTE3NTczNTEzNTFcbiAgcmV0dXJuIChieXRlVG9IZXhbYXJyW29mZnNldCArIDBdXSArIGJ5dGVUb0hleFthcnJbb2Zmc2V0ICsgMV1dICsgYnl0ZVRvSGV4W2FycltvZmZzZXQgKyAyXV0gKyBieXRlVG9IZXhbYXJyW29mZnNldCArIDNdXSArICctJyArIGJ5dGVUb0hleFthcnJbb2Zmc2V0ICsgNF1dICsgYnl0ZVRvSGV4W2FycltvZmZzZXQgKyA1XV0gKyAnLScgKyBieXRlVG9IZXhbYXJyW29mZnNldCArIDZdXSArIGJ5dGVUb0hleFthcnJbb2Zmc2V0ICsgN11dICsgJy0nICsgYnl0ZVRvSGV4W2FycltvZmZzZXQgKyA4XV0gKyBieXRlVG9IZXhbYXJyW29mZnNldCArIDldXSArICctJyArIGJ5dGVUb0hleFthcnJbb2Zmc2V0ICsgMTBdXSArIGJ5dGVUb0hleFthcnJbb2Zmc2V0ICsgMTFdXSArIGJ5dGVUb0hleFthcnJbb2Zmc2V0ICsgMTJdXSArIGJ5dGVUb0hleFthcnJbb2Zmc2V0ICsgMTNdXSArIGJ5dGVUb0hleFthcnJbb2Zmc2V0ICsgMTRdXSArIGJ5dGVUb0hleFthcnJbb2Zmc2V0ICsgMTVdXSkudG9Mb3dlckNhc2UoKTtcbn1cbmZ1bmN0aW9uIHN0cmluZ2lmeShhcnIsIG9mZnNldCA9IDApIHtcbiAgdmFyIHV1aWQgPSB1bnNhZmVTdHJpbmdpZnkoYXJyLCBvZmZzZXQpO1xuICAvLyBDb25zaXN0ZW5jeSBjaGVjayBmb3IgdmFsaWQgVVVJRC4gIElmIHRoaXMgdGhyb3dzLCBpdCdzIGxpa2VseSBkdWUgdG8gb25lXG4gIC8vIG9mIHRoZSBmb2xsb3dpbmc6XG4gIC8vIC0gT25lIG9yIG1vcmUgaW5wdXQgYXJyYXkgdmFsdWVzIGRvbid0IG1hcCB0byBhIGhleCBvY3RldCAobGVhZGluZyB0b1xuICAvLyBcInVuZGVmaW5lZFwiIGluIHRoZSB1dWlkKVxuICAvLyAtIEludmFsaWQgaW5wdXQgdmFsdWVzIGZvciB0aGUgUkZDIGB2ZXJzaW9uYCBvciBgdmFyaWFudGAgZmllbGRzXG4gIGlmICghdmFsaWRhdGUodXVpZCkpIHtcbiAgICB0aHJvdyBUeXBlRXJyb3IoJ1N0cmluZ2lmaWVkIFVVSUQgaXMgaW52YWxpZCcpO1xuICB9XG4gIHJldHVybiB1dWlkO1xufVxuZXhwb3J0IGRlZmF1bHQgc3RyaW5naWZ5OyIsImltcG9ydCBuYXRpdmUgZnJvbSAnLi9uYXRpdmUuanMnO1xuaW1wb3J0IHJuZyBmcm9tICcuL3JuZy5qcyc7XG5pbXBvcnQgeyB1bnNhZmVTdHJpbmdpZnkgfSBmcm9tICcuL3N0cmluZ2lmeS5qcyc7XG5mdW5jdGlvbiB2NChvcHRpb25zLCBidWYsIG9mZnNldCkge1xuICBpZiAobmF0aXZlLnJhbmRvbVVVSUQgJiYgIWJ1ZiAmJiAhb3B0aW9ucykge1xuICAgIHJldHVybiBuYXRpdmUucmFuZG9tVVVJRCgpO1xuICB9XG4gIG9wdGlvbnMgPSBvcHRpb25zIHx8IHt9O1xuICB2YXIgcm5kcyA9IG9wdGlvbnMucmFuZG9tIHx8IChvcHRpb25zLnJuZyB8fCBybmcpKCk7XG5cbiAgLy8gUGVyIDQuNCwgc2V0IGJpdHMgZm9yIHZlcnNpb24gYW5kIGBjbG9ja19zZXFfaGlfYW5kX3Jlc2VydmVkYFxuICBybmRzWzZdID0gcm5kc1s2XSAmIDB4MGYgfCAweDQwO1xuICBybmRzWzhdID0gcm5kc1s4XSAmIDB4M2YgfCAweDgwO1xuXG4gIC8vIENvcHkgYnl0ZXMgdG8gYnVmZmVyLCBpZiBwcm92aWRlZFxuICBpZiAoYnVmKSB7XG4gICAgb2Zmc2V0ID0gb2Zmc2V0IHx8IDA7XG4gICAgZm9yICh2YXIgaSA9IDA7IGkgPCAxNjsgKytpKSB7XG4gICAgICBidWZbb2Zmc2V0ICsgaV0gPSBybmRzW2ldO1xuICAgIH1cbiAgICByZXR1cm4gYnVmO1xuICB9XG4gIHJldHVybiB1bnNhZmVTdHJpbmdpZnkocm5kcyk7XG59XG5leHBvcnQgZGVmYXVsdCB2NDsiLCJpbXBvcnQgUkVHRVggZnJvbSAnLi9yZWdleC5qcyc7XG5mdW5jdGlvbiB2YWxpZGF0ZSh1dWlkKSB7XG4gIHJldHVybiB0eXBlb2YgdXVpZCA9PT0gJ3N0cmluZycgJiYgUkVHRVgudGVzdCh1dWlkKTtcbn1cbmV4cG9ydCBkZWZhdWx0IHZhbGlkYXRlOyIsIi8vIFRoZSBtb2R1bGUgY2FjaGVcbnZhciBfX3dlYnBhY2tfbW9kdWxlX2NhY2hlX18gPSB7fTtcblxuLy8gVGhlIHJlcXVpcmUgZnVuY3Rpb25cbmZ1bmN0aW9uIF9fd2VicGFja19yZXF1aXJlX18obW9kdWxlSWQpIHtcblx0Ly8gQ2hlY2sgaWYgbW9kdWxlIGlzIGluIGNhY2hlXG5cdHZhciBjYWNoZWRNb2R1bGUgPSBfX3dlYnBhY2tfbW9kdWxlX2NhY2hlX19bbW9kdWxlSWRdO1xuXHRpZiAoY2FjaGVkTW9kdWxlICE9PSB1bmRlZmluZWQpIHtcblx0XHRyZXR1cm4gY2FjaGVkTW9kdWxlLmV4cG9ydHM7XG5cdH1cblx0Ly8gQ3JlYXRlIGEgbmV3IG1vZHVsZSAoYW5kIHB1dCBpdCBpbnRvIHRoZSBjYWNoZSlcblx0dmFyIG1vZHVsZSA9IF9fd2VicGFja19tb2R1bGVfY2FjaGVfX1ttb2R1bGVJZF0gPSB7XG5cdFx0Ly8gbm8gbW9kdWxlLmlkIG5lZWRlZFxuXHRcdC8vIG5vIG1vZHVsZS5sb2FkZWQgbmVlZGVkXG5cdFx0ZXhwb3J0czoge31cblx0fTtcblxuXHQvLyBFeGVjdXRlIHRoZSBtb2R1bGUgZnVuY3Rpb25cblx0X193ZWJwYWNrX21vZHVsZXNfX1ttb2R1bGVJZF0uY2FsbChtb2R1bGUuZXhwb3J0cywgbW9kdWxlLCBtb2R1bGUuZXhwb3J0cywgX193ZWJwYWNrX3JlcXVpcmVfXyk7XG5cblx0Ly8gUmV0dXJuIHRoZSBleHBvcnRzIG9mIHRoZSBtb2R1bGVcblx0cmV0dXJuIG1vZHVsZS5leHBvcnRzO1xufVxuXG4iLCIvLyBnZXREZWZhdWx0RXhwb3J0IGZ1bmN0aW9uIGZvciBjb21wYXRpYmlsaXR5IHdpdGggbm9uLWhhcm1vbnkgbW9kdWxlc1xuX193ZWJwYWNrX3JlcXVpcmVfXy5uID0gKG1vZHVsZSkgPT4ge1xuXHR2YXIgZ2V0dGVyID0gbW9kdWxlICYmIG1vZHVsZS5fX2VzTW9kdWxlID9cblx0XHQoKSA9PiAobW9kdWxlWydkZWZhdWx0J10pIDpcblx0XHQoKSA9PiAobW9kdWxlKTtcblx0X193ZWJwYWNrX3JlcXVpcmVfXy5kKGdldHRlciwgeyBhOiBnZXR0ZXIgfSk7XG5cdHJldHVybiBnZXR0ZXI7XG59OyIsIi8vIGRlZmluZSBnZXR0ZXIgZnVuY3Rpb25zIGZvciBoYXJtb255IGV4cG9ydHNcbl9fd2VicGFja19yZXF1aXJlX18uZCA9IChleHBvcnRzLCBkZWZpbml0aW9uKSA9PiB7XG5cdGZvcih2YXIga2V5IGluIGRlZmluaXRpb24pIHtcblx0XHRpZihfX3dlYnBhY2tfcmVxdWlyZV9fLm8oZGVmaW5pdGlvbiwga2V5KSAmJiAhX193ZWJwYWNrX3JlcXVpcmVfXy5vKGV4cG9ydHMsIGtleSkpIHtcblx0XHRcdE9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBrZXksIHsgZW51bWVyYWJsZTogdHJ1ZSwgZ2V0OiBkZWZpbml0aW9uW2tleV0gfSk7XG5cdFx0fVxuXHR9XG59OyIsIl9fd2VicGFja19yZXF1aXJlX18uZyA9IChmdW5jdGlvbigpIHtcblx0aWYgKHR5cGVvZiBnbG9iYWxUaGlzID09PSAnb2JqZWN0JykgcmV0dXJuIGdsb2JhbFRoaXM7XG5cdHRyeSB7XG5cdFx0cmV0dXJuIHRoaXMgfHwgbmV3IEZ1bmN0aW9uKCdyZXR1cm4gdGhpcycpKCk7XG5cdH0gY2F0Y2ggKGUpIHtcblx0XHRpZiAodHlwZW9mIHdpbmRvdyA9PT0gJ29iamVjdCcpIHJldHVybiB3aW5kb3c7XG5cdH1cbn0pKCk7IiwiX193ZWJwYWNrX3JlcXVpcmVfXy5vID0gKG9iaiwgcHJvcCkgPT4gKE9iamVjdC5wcm90b3R5cGUuaGFzT3duUHJvcGVydHkuY2FsbChvYmosIHByb3ApKSIsIi8vIGRlZmluZSBfX2VzTW9kdWxlIG9uIGV4cG9ydHNcbl9fd2VicGFja19yZXF1aXJlX18uciA9IChleHBvcnRzKSA9PiB7XG5cdGlmKHR5cGVvZiBTeW1ib2wgIT09ICd1bmRlZmluZWQnICYmIFN5bWJvbC50b1N0cmluZ1RhZykge1xuXHRcdE9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBTeW1ib2wudG9TdHJpbmdUYWcsIHsgdmFsdWU6ICdNb2R1bGUnIH0pO1xuXHR9XG5cdE9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCAnX19lc01vZHVsZScsIHsgdmFsdWU6IHRydWUgfSk7XG59OyIsImltcG9ydCB7IEJhc2VNZXNzYWdlcyB9IGZyb20gJy4vbWVzc2FnZXMvdW5pdHlCcm93c2VyTWVzc2FnaW5nL0Jhc2VNZXNzYWdlcyc7XHJcblxyXG5jb25zdCBGSUxFID0gJ2luZGV4SnNMaWIudHMnO1xyXG5cclxuaWYgKCEod2luZG93IGFzIGFueSkuR0FPX1VuaXR5QnJvd3NlckNoYW5uZWwpIHtcclxuICAgICh3aW5kb3cgYXMgYW55KS5HQU9fVW5pdHlCcm93c2VyQ2hhbm5lbCA9IHt9XHJcbn0gXHJcbmlmICghKHdpbmRvdyBhcyBhbnkpLkdBT19Vbml0eUJyb3dzZXJDaGFubmVsLkJhc2VNZXNzYWdlcykge1xyXG4gICAgKHdpbmRvdyBhcyBhbnkpLkdBT19Vbml0eUJyb3dzZXJDaGFubmVsLkJhc2VNZXNzYWdlcyA9IHt9XHJcbn0gXHJcblxyXG4od2luZG93IGFzIGFueSkuR0FPX1VuaXR5QnJvd3NlckNoYW5uZWwuQmFzZU1lc3NhZ2VzLnJlY2VpdmVTdHJpbmcgPSBmdW5jdGlvbiAoc3RyOiBzdHJpbmcpIHtcclxuICAgIGNvbnNvbGUubG9nKGBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQCBjcCAyMDAwOiBHQU9fVW5pdHlCcm93c2VyQ2hhbm5lbC5CYXNlTWVzc2FnZXMucmVjZWl2ZVN0cmluZygpOiAke3N0cn1gKTtcclxuICAgIEJhc2VNZXNzYWdlcy5yZWNlaXZlU3RyaW5nKHN0cik7XHJcbn1cclxuXHJcblxyXG5leHBvcnQgZnVuY3Rpb24gIHNlbmRTdHJpbmdUb1VuaXR5KHN0cjogc3RyaW5nKTogdm9pZCB7XHJcbiAgICBjb25zdCBGVU5DID0gJ3NlbmRTdHJpbmdUb1VuaXR5KCknO1xyXG4gICAgY29uc29sZS5sb2coYEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAIGNwIDIxMDA6IHNlbmRTdHJpbmdUb1VuaXR5KCk6ICR7c3RyfWApO1xyXG4gICAgdHJ5XHJcbiAgICB7XHJcbiAgICAgICAgQmFzZU1lc3NhZ2VzLnNlbmRTdHJpbmcoc3RyKTtcclxuICAgIH0gY2F0Y2ggKGVycikge1xyXG4gICAgICAgIGNvbnNvbGUuZXJyb3IoYCR7RklMRX06JHtGVU5DfTogJHtlcnJ9YCk7XHJcbiAgICB9XHJcbn1cclxuXHJcbmZ1bmN0aW9uIGtlZXBQaW5naW5nKCkge1xyXG4gICAgc2V0SW50ZXJ2YWwoKCkgPT4ge1xyXG4gICAgICAgIGxldCBtc2cgPSBKU09OLnN0cmluZ2lmeShcclxuICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgbWVzc2FnZTogXCJIZWxsbyBmcm9tIGJyb3dzZXIhXCJcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgICk7XHJcbiAgICAgICAgc2VuZFN0cmluZ1RvVW5pdHkobXNnKTtcclxuICAgIH0sIDUwMDApXHJcblxyXG59XHJcblxyXG4vLyBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBcclxua2VlcFBpbmdpbmcoKTtcclxuLy8gQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAIl0sIm5hbWVzIjpbXSwic291cmNlUm9vdCI6IiJ9