//#define UNITY_PRO_LICENSE

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ucss
{
    public class HTTPRequest : UCSSRequest
    {
        public byte[] bytes;
        public WWWForm formData;
        public Dictionary<string, string> headers;

        public int assetVersion;
        public uint assetCRC;

        public System.Func<IEnumerator> coroutineFactory;
        public Coroutine coroutine;

        public ThreadPriority threadPriority = ThreadPriority.Normal;

        public EventHandlerHTTPWWW          wwwCallback;
        public EventHandlerHTTPTexture      textureCallback;
        public EventHandlerHTTPTexture      textureNonReadableCallback;
        public EventHandlerHTTPBytes        bytesCallback;
        public EventHandlerHTTPString       stringCallback;
        public EventHandlerAudioClip        audioClipCallback;

#if UNITY_PRO_LICENSE && !UNITY_WEBGL
        public EventHandlerMovieTexture     movieTextureCallback;
		public EventHandlerAssetBundle      assetBundleCallback;
#endif

        public EventHandlerDownloadProgress downloadProgress;
        public EventHandlerUploadProgress   uploadProgress;
    }
}