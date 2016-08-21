// #define UNITY_PRO_LICENSE && !UNITY_WEBGL

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ucss
{
    public delegate void EventHandlerHTTPWWW(WWW www, string transactionId);
    public delegate void EventHandlerHTTPBytes(byte[] bytes, string transactionId);
    public delegate void EventHandlerHTTPTexture(Texture2D texture, string transactionId);
    public delegate void EventHandlerHTTPString(string text, string transactionId);

    public delegate void EventHandlerAudioClip(AudioClip audioClip, string transactionId);

	public delegate void EventHandlerDownloadProgress(float progress);
    public delegate void EventHandlerUploadProgress(float progress);

    #if UNITY_PRO_LICENSE && !UNITY_WEBGL
    public delegate void EventHandlerMovieTexture(MovieTexture movieTexture, string transactionId);
	public delegate void EventHandlerAssetBundle(AssetBundle assetBundle, string transactionId);
	#endif

    public class HTTPProtocol : BaseProtocol
    {

        public void Init()
        {
            this.InitBase(null, null);

            this.TimeOut = UCSSconfig.requestDefaultTimeOut;
            this.TimeOutCheck = UCSSconfig.requestDefaultTimeOutCheck;
        }

        // *** POST BYTES ***
        public void PostBytes(HTTPRequest request)
        {
            StartCoroutine(RunPostBytesCoroutine(request));
        }

        public string PostBytes(string url, byte[] bytes, Dictionary<string, string> headers, EventHandlerHTTPBytes bytesCallback, EventHandlerServiceError onError = null, EventHandlerServiceTimeOut onTimeOut = null, int timeOut = 0)
        {
            HTTPRequest request = new HTTPRequest();
            request.url = url;
            request.bytes = bytes;
            request.headers = headers;
            request.transactionId = UCSS.GenerateTransactionId(Common.Md5Sum(url));
            request.bytesCallback = bytesCallback;
            request.onError = onError;
            request.onTimeOut = onTimeOut;
            request.timeOut = timeOut;

            StartCoroutine(RunPostBytesCoroutine(request));

            return request.transactionId;
        }

        public string PostBytes(string url, byte[] bytes, Dictionary<string, string> headers, EventHandlerHTTPString stringCallback, EventHandlerServiceError onError = null, EventHandlerServiceTimeOut onTimeOut = null, int timeOut = 0)
        {
            HTTPRequest request = new HTTPRequest();
            request.url = url;
            request.bytes = bytes;
            request.headers = headers;
            request.transactionId = UCSS.GenerateTransactionId(Common.Md5Sum(url));
            request.stringCallback = stringCallback;
            request.onError = onError;
            request.onTimeOut = onTimeOut;
            request.timeOut = timeOut;

            StartCoroutine(RunPostBytesCoroutine(request));

            return request.transactionId;
        }
        // *** END post bytes ***

        // *** POST FORM ***
        public void PostForm(HTTPRequest request)
        {
            StartCoroutine(RunPostFormCoroutine(request));
        }

        public string PostForm(string url, WWWForm formData, EventHandlerHTTPBytes bytesCallback, EventHandlerServiceError onError = null, EventHandlerServiceTimeOut onTimeOut = null, int timeOut = 0)
        {
            HTTPRequest request = new HTTPRequest();
            request.url = url;
            request.formData = formData;
            request.transactionId = UCSS.GenerateTransactionId(Common.Md5Sum(url));
            request.bytesCallback = bytesCallback;
            request.onError = onError;
            request.onTimeOut = onTimeOut;
            request.timeOut = timeOut;

            StartCoroutine(RunPostFormCoroutine(request));

            return request.transactionId;
        }

        public string PostForm(string url, WWWForm formData, EventHandlerHTTPString stringCallback, EventHandlerServiceError onError = null, EventHandlerServiceTimeOut onTimeOut = null, int timeOut = 0)
        {
            HTTPRequest request = new HTTPRequest();
            request.url = url;
            request.formData = formData;
            request.transactionId = UCSS.GenerateTransactionId(Common.Md5Sum(url));
            request.stringCallback = stringCallback;
            request.onError = onError;
            request.onTimeOut = onTimeOut;
            request.timeOut = timeOut;

            StartCoroutine(RunPostFormCoroutine(request));

            return request.transactionId;
        }
        // *** END post form ***


        // *** GET BYTES ***
        public void GetBytes(HTTPRequest request)
        {
            StartCoroutine(RunGetDataCoroutine(request));
        }

        public string GetBytes(string url, EventHandlerHTTPBytes bytesCallback, EventHandlerServiceError onError = null, EventHandlerServiceTimeOut onTimeOut = null, int timeOut = 0)
        {
            HTTPRequest request = new HTTPRequest();
            request.url = url;
            request.transactionId = UCSS.GenerateTransactionId(Common.Md5Sum(url));
            request.bytesCallback = bytesCallback;
            request.onError = onError;
            request.onTimeOut = onTimeOut;
            request.timeOut = timeOut;

            StartCoroutine(RunGetDataCoroutine(request));

            return request.transactionId;
        }
        // *** END bytes ***


        // *** STRING ***
        public void GetString(HTTPRequest request)
        {
            StartCoroutine(RunGetDataCoroutine(request));
        }

        public string GetString(string url, EventHandlerHTTPString stringCallback, EventHandlerServiceError onError = null, EventHandlerServiceTimeOut onTimeOut = null, int timeOut = 0)
        {
            HTTPRequest request = new HTTPRequest();
            request.url = url;
            request.transactionId = UCSS.GenerateTransactionId(Common.Md5Sum(url));
            request.stringCallback = stringCallback;
            request.onError = onError;
            request.onTimeOut = onTimeOut;
            request.timeOut = timeOut;

            StartCoroutine(RunGetDataCoroutine(request));

            return request.transactionId;
        }
        // *** END string ***

        // *** TEXTURE ***
        public void GetTexture(HTTPRequest request)
        {
            StartCoroutine(RunGetDataCoroutine(request));
        }

        public string GetTexture(string url, EventHandlerHTTPTexture textureCallback, EventHandlerServiceError onError = null, EventHandlerServiceTimeOut onTimeOut = null, int timeOut = 0)
        {
            HTTPRequest request = new HTTPRequest();
            request.url = url;
            request.transactionId = UCSS.GenerateTransactionId(Common.Md5Sum(url) + Random.RandomRange(1, 9999999).ToString());
            request.textureCallback = textureCallback;
            request.onError = onError;
            request.onTimeOut = onTimeOut;
            request.timeOut = timeOut;

            StartCoroutine(RunGetDataCoroutine(request));

            return request.transactionId;
        }

        public string GetTextureNonReadable(string url, EventHandlerHTTPTexture textureCallback, EventHandlerServiceError onError = null, EventHandlerServiceTimeOut onTimeOut = null, int timeOut = 0)
        {
            HTTPRequest request = new HTTPRequest();
            request.url = url;
            request.transactionId = UCSS.GenerateTransactionId(Common.Md5Sum(url));
            request.textureNonReadableCallback = textureCallback;
            request.onError = onError;
            request.onTimeOut = onTimeOut;
            request.timeOut = timeOut;

            StartCoroutine(RunGetDataCoroutine(request));

            return request.transactionId;
        }
        // *** END texture ***

        // *** AssetBundle ***
#if UNITY_PRO_LICENSE && !UNITY_WEBGL
        public void GetAssetBundle(HTTPRequest request)
        {
            StartCoroutine(RunGetDataCoroutine(request));
        }

        public string GetAssetBundle(string url, EventHandlerAssetBundle assetBundleCallback, EventHandlerServiceError onError = null, EventHandlerServiceTimeOut onTimeOut = null, int timeOut = 0)
        {
            HTTPRequest request = new HTTPRequest();
            request.url = url;
            request.transactionId = UCSS.GenerateTransactionId(Common.Md5Sum(url));
            request.assetBundleCallback = assetBundleCallback;
            request.onError = onError;
            request.onTimeOut = onTimeOut;
            request.timeOut = timeOut;

            StartCoroutine(RunGetDataCoroutine(request));

            return request.transactionId;
        }

        public void GetAssetBundleOrCache(HTTPRequest request)
        {
            StartCoroutine(RunGetAssetBundleCoroutine(request));
        }
#endif
        // *** END AssetBundle ***

        // *** AudioClip ***
        public void GetAudioClip(HTTPRequest request)
        {
            StartCoroutine(RunGetDataCoroutine(request));
        }

        public string GetAudioClip(string url, EventHandlerAudioClip audioClipCallback, EventHandlerServiceError onError = null, EventHandlerServiceTimeOut onTimeOut = null, int timeOut = 0)
        {
            HTTPRequest request = new HTTPRequest();
            request.url = url;
            request.transactionId = UCSS.GenerateTransactionId(Common.Md5Sum(url));
            request.audioClipCallback = audioClipCallback;
            request.onError = onError;
            request.onTimeOut = onTimeOut;
            request.timeOut = timeOut;

            StartCoroutine(RunGetDataCoroutine(request));

            return request.transactionId;
        }
        // *** END AudioClip ***

        // *** Movie ***
#if UNITY_PRO_LICENSE && !UNITY_WEBGL
        public void GetMovie(HTTPRequest request)
        {
            StartCoroutine(RunGetDataCoroutine(request));
        }

        public string GetMovie(string url, EventHandlerMovieTexture movieCallback, EventHandlerServiceError onError = null, EventHandlerServiceTimeOut onTimeOut = null, int timeOut = 0)
        {
            HTTPRequest request = new HTTPRequest();
            request.url = url;
            request.transactionId = UCSS.GenerateTransactionId(Common.Md5Sum(url));
            request.movieTextureCallback = movieCallback;
            request.onError = onError;
            request.onTimeOut = onTimeOut;
            request.timeOut = timeOut;

            StartCoroutine(RunGetDataCoroutine(request));

            return request.transactionId;
        }
#endif
        // *** END Movie ***


        // Coroutines

        private IEnumerator RunGetDataCoroutine(HTTPRequest request)
        {
            if (string.IsNullOrEmpty(request.transactionId))
            {
                request.transactionId = UCSS.GenerateTransactionId(Common.Md5Sum(request.url));
            }
            if (request.timeOut == 0)
            {
                request.timeOut = this.TimeOut;
            }

            this.AddTransaction(request.transactionId, request.url, request, request.timeOut);
            this.SetTransactionStatus(request.transactionId, transactionStatus.sending);

            WWW www = new WWW(request.url);
#if !UNITY_WEBGL
            if (request.threadPriority != ThreadPriority.Normal)
            {
                www.threadPriority = request.threadPriority;
            }
#endif

            float lastDownloadProgress = 0.0f;
            float lastUploadProgress = 0.0f;
            while (!www.isDone)
            {
                if (request.downloadProgress != null)
                {
                    float progress = www.progress;
                    if (progress != lastDownloadProgress)
                    {
                        lastDownloadProgress = progress;
                        request.downloadProgress(progress);
                    }
                }
                if (request.uploadProgress != null)
                {
                    float uploadProgress = www.uploadProgress;
                    if (uploadProgress != lastUploadProgress)
                    {
                        lastUploadProgress = uploadProgress;
                        request.uploadProgress(uploadProgress);
                    }
                }
                yield return null;
            }

            if (!this.TransactionValid(request.transactionId))
            {
                Debug.LogWarning("[HTTPProtocol] [RunGetDataCoroutine] transaction [" + request.transactionId + "] finished, but is no longer valid.");
                yield break;
            }

            StartCoroutine(this.DoCallBack(www, request));
        } // RunGetDataCoroutine

        private IEnumerator RunPostBytesCoroutine(HTTPRequest request)
        {
            if (string.IsNullOrEmpty(request.transactionId))
            {
                request.transactionId = UCSS.GenerateTransactionId(Common.Md5Sum(request.url));
            }
            if (request.timeOut == 0)
            {
                request.timeOut = this.TimeOut;
            }

            this.AddTransaction(request.transactionId, request.url, request, request.timeOut);
            this.SetTransactionStatus(request.transactionId, transactionStatus.sending);

            WWW www = null;
            if (request.headers != null)
            {
                www = new WWW(request.url, request.bytes, request.headers);
            }
            else
            {
                www = new WWW(request.url, request.bytes);
            }
#if !UNITY_WEBGL
            if (request.threadPriority != ThreadPriority.Normal)
            {
                www.threadPriority = request.threadPriority;
            }
#endif

            float lastDownloadProgress = 0.0f;
            float lastUploadProgress = 0.0f;
            while (!www.isDone)
            {
                if (request.downloadProgress != null)
                {
                    float progress = www.progress;
                    if (progress != lastDownloadProgress)
                    {
                        lastDownloadProgress = progress;
                        request.downloadProgress(progress);
                    }
                }
                if (request.uploadProgress != null)
                {
                    float uploadProgress = www.uploadProgress;
                    if (uploadProgress != lastUploadProgress)
                    {
                        lastUploadProgress = uploadProgress;
                        request.uploadProgress(uploadProgress);
                    }
                }
                yield return null;
            }

            if (!this.TransactionValid(request.transactionId))
            {
                Debug.LogWarning("[HTTPProtocol] [RunPostBytesCoroutine] transaction [" + request.transactionId + "] finished, but is no longer valid.");
                yield break;
            }

            StartCoroutine(this.DoCallBack(www, request));
        } // RunPostBytesCoroutine

        private IEnumerator RunPostFormCoroutine(HTTPRequest request)
        {
            if (string.IsNullOrEmpty(request.transactionId))
            {
                request.transactionId = UCSS.GenerateTransactionId(Common.Md5Sum(request.url));
            }
            if (request.timeOut == 0)
            {
                request.timeOut = this.TimeOut;
            }

            this.AddTransaction(request.transactionId, request.url, request, request.timeOut);
            this.SetTransactionStatus(request.transactionId, transactionStatus.sending);

            WWW www;
            if (request.headers != null && request.formData == null)
            {
                www = new WWW(request.url, null, request.headers);
            }
            else
            {
                if (request.headers != null)
                {
                    www = new WWW(request.url, request.formData.data, request.headers);
                }
                else
                {
                    www = new WWW(request.url, request.formData);
                }
            }

#if !UNITY_WEBGL
            if (request.threadPriority != ThreadPriority.Normal)
            {
                www.threadPriority = request.threadPriority;
            }
#endif

            float lastDownloadProgress = 0.0f;
            float lastUploadProgress = 0.0f;
            while (!www.isDone)
            {
                if (request.downloadProgress != null)
                {
                    float progress = www.progress;
                    if (progress != lastDownloadProgress)
                    {
                        lastDownloadProgress = progress;
                        request.downloadProgress(progress);
                    }
                }
                if (request.uploadProgress != null)
                {
                    float uploadProgress = www.uploadProgress;
                    if (uploadProgress != lastUploadProgress)
                    {
                        lastUploadProgress = uploadProgress;
                        request.uploadProgress(uploadProgress);
                    }
                }
                yield return null;
            }

            if (!this.TransactionValid(request.transactionId))
            {
                Debug.LogWarning("[HTTPProtocol] [RunPostFormCoroutine] transaction [" + request.transactionId + "] finished, but is no longer valid.");
                yield break;
            }

            StartCoroutine(this.DoCallBack(www, request));
        } // RunPostFormCoroutine

        private IEnumerator RunGetAssetBundleCoroutine(HTTPRequest request)
        {
            if (string.IsNullOrEmpty(request.transactionId))
            {
                request.transactionId = UCSS.GenerateTransactionId(Common.Md5Sum(request.url));
            }
            if (request.timeOut == 0)
            {
                request.timeOut = this.TimeOut;
            }

            this.AddTransaction(request.transactionId, request.url, request, request.timeOut);
            this.SetTransactionStatus(request.transactionId, transactionStatus.sending);

            WWW www = WWW.LoadFromCacheOrDownload(request.url, request.assetVersion, request.assetCRC);

#if !UNITY_WEBGL
            if (request.threadPriority != ThreadPriority.Normal)
            {
                www.threadPriority = request.threadPriority;
            }
#endif

            float lastDownloadProgress = 0.0f;
            float lastUploadProgress = 0.0f;
            while (!www.isDone)
            {
                if (request.downloadProgress != null)
                {
                    float progress = www.progress;
                    if (progress != lastDownloadProgress)
                    {
                        lastDownloadProgress = progress;
                        request.downloadProgress(progress);
                    }
                }
                if (request.uploadProgress != null)
                {
                    float uploadProgress = www.uploadProgress;
                    if (uploadProgress != lastUploadProgress)
                    {
                        lastUploadProgress = uploadProgress;
                        request.uploadProgress(uploadProgress);
                    }
                }
                yield return null;
            }

            if (!this.TransactionValid(request.transactionId))
            {
                Debug.LogWarning("[HTTPProtocol] [RunGetAssetBundleCoroutine] transaction [" + request.transactionId + "] finished, but is no longer valid.");
                yield break;
            }

            StartCoroutine(this.DoCallBack(www, request));
        } // RunGetAssetBundleCoroutine

        private IEnumerator DoCallBack(WWW www, HTTPRequest request)
        {
            this.SetTransactionWWW(request.transactionId, www);
            if (string.IsNullOrEmpty(www.error))
            {
                this.SetTransactionStatus(request.transactionId, transactionStatus.completed);
                if (request.wwwCallback != null)
                {
                    request.wwwCallback(www, request.transactionId);
                }
#if UNITY_PRO_LICENSE && !UNITY_WEBGL
                else if (request.assetBundleCallback != null)
                {
                    request.assetBundleCallback(www.assetBundle, request.transactionId);
                }
                else if (request.movieTextureCallback != null)
                {
                    MovieTexture movieTexture = www.movie;
                    while (!movieTexture.isReadyToPlay)
                    {
                        yield return null;
                    }
                    request.movieTextureCallback(movieTexture, request.transactionId);
                }
#endif
				else if (request.audioClipCallback != null)
				{
					request.audioClipCallback(www.audioClip, request.transactionId);
				}
                else if (request.textureCallback != null)
                {
                    request.textureCallback(www.texture, request.transactionId);
                }
                else if (request.textureNonReadableCallback != null)
                {
                    request.textureNonReadableCallback(www.textureNonReadable, request.transactionId);
                }
                else if (request.stringCallback != null)
                {
                    request.stringCallback(www.text, request.transactionId);
                }
                else if (request.bytesCallback != null)
                {
                    request.bytesCallback(www.bytes, request.transactionId);
                }
                this.RemoveTransaction(request.transactionId);
            }
            else
            {
                this.SetTransactionStatus(request.transactionId, transactionStatus.error);
                if (request.onError != null)
                {
                    request.onError(www.error, request.transactionId);
                }
                this.RemoveTransaction(request.transactionId);
            }
            yield break;
        } // DoCallBack

    } // HTTPProtocol
}