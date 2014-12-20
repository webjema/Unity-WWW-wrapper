// #define UNITY_PRO_LICENSE

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ucss;

public class TestWWW : MonoBehaviour 
{
    public MeshRenderer cube1;
    public AudioSource audioSource;

    private float bigTextureProgress;
    private float movieTextureProgress;

    void OnGUI()
    {
        GUILayout.BeginVertical();
        if (GUILayout.Button("Get Text"))
        {
            this.GetText();
        }

        if (GUILayout.Button("Get Bytes"))
        {
            this.GetBytes();
        }

        if (GUILayout.Button("Get Texture"))
        {
            this.GetTexture();
        }

        if (GUILayout.Button("Get Big Tex. Non Readable"))
        {
            this.GetBigTextureNonReadable();
        }
        if (this.bigTextureProgress > 0)
        {
            GUILayout.Label((this.bigTextureProgress * 100).ToString() + " downloaded");
        }
#if UNITY_PRO_LICENSE
        if (GUILayout.Button("Get AssetBundle"))
        {
            this.GetAssetBundle();
        }

        if (GUILayout.Button("Get AssetBundle (cache)"))
        {
            this.GetAssetBundleCache();
        }
#endif

        if (GUILayout.Button("Get AudioClip"))
        {
            this.GetAudioClip();
        }
#if UNITY_PRO_LICENSE
        if (GUILayout.Button("Get Movie"))
        {
            this.GetMovie();
        }
        if (this.movieTextureProgress > 0)
        {
            GUILayout.Label((this.movieTextureProgress * 100).ToString() + " downloaded");
        }
#endif


        if (GUILayout.Button("Post Bytes"))
        {
            this.PostBytes();
        }

        if (GUILayout.Button("Post Form"))
        {
            this.PostForm();
        }

        if (GUILayout.Button("Test timeout"))
        {
            this.TestTimeout();
        }

        if (GUILayout.Button("Test error (404)"))
        {
            this.TestError();
        }

        GUILayout.EndVertical();
    }

    void GetText()
    {
        UCSS.HTTP.GetString("http://ucss.webjema.com/tests/http/return-string.php", new EventHandlerHTTPString(this.OnTextDownloaded), new EventHandlerServiceError(this.OnHTTPError), new EventHandlerServiceTimeOut(this.OnHTTPTimeOut), 10);
    }

    void OnTextDownloaded(string text, string transactionId)
    {
        Debug.Log("[TestWWWScene] [OnTextDownloaded] text = " + text);
    }

    void OnHTTPError(string error, string transactionId)
    {
        Debug.LogError("[TestWWWScene] [OnHTTPError] error = " + error + " (transaction [" + transactionId + "])");
    }

    void OnHTTPTimeOut(string transactionId)
    {
        Debug.LogError("[TestWWWScene] [OnHTTPTimeOut] transactionId = " + transactionId);
        UCSS.HTTP.RemoveTransaction(transactionId);
    }

    void GetBytes()
    {
        UCSS.HTTP.GetBytes("http://ucss.webjema.com/tests/http/return-string.php", new EventHandlerHTTPBytes(this.OnBytesDownloaded), new EventHandlerServiceError(this.OnHTTPError), new EventHandlerServiceTimeOut(this.OnHTTPTimeOut), 10);
    }

    void GetTexture()
    {
        this.cube1.material.mainTexture = null;
        UCSS.HTTP.GetTexture("http://ucss.webjema.com/tests/http/images/texture1.jpg", new EventHandlerHTTPTexture(this.OnTextureDownloaded), new EventHandlerServiceError(this.OnHTTPError), new EventHandlerServiceTimeOut(this.OnHTTPTimeOut), 10);
    }

    void OnTextureDownloaded(Texture texture, string transactionId)
    {
        Debug.Log("[TestWWWScene] [OnTextureDownloaded] texture.width = " + texture.width + ", texture.height = " + texture.height);
        this.cube1.material.mainTexture = texture;
    }

    void GetBigTextureNonReadable()
    {
        this.cube1.material.mainTexture = null;
        this.bigTextureProgress = 0;

        HTTPRequest request = new HTTPRequest();
        request.url = "http://ucss.webjema.com/tests/http/images/texture1_big.jpg";
        request.transactionId = "GetBigTextureNonReadable";
        request.textureNonReadableCallback = new EventHandlerHTTPTexture(this.OnTextureDownloaded);
        request.onError = new EventHandlerServiceError(this.OnHTTPError);
        request.onTimeOut = null;
        request.timeOut = 0;

        request.downloadProgress = new EventHandlerDownloadProgress(this.OnBigTextureDownloadProgress);

        UCSS.HTTP.GetTexture(request);
    }

    void OnBigTextureDownloadProgress(float progress)
    {
        this.bigTextureProgress = progress;
    }

#if UNITY_PRO_LICENSE
    void GetAssetBundle()
    {
        GameObject go = GameObject.Find("CubesInAssetBundle(Clone)");
        if (go != null)
        {
            GameObject.Destroy(go);
        }
        UCSS.HTTP.GetAssetBundle("http://ucss.webjema.com/tests/http/assets/CubesInAssetBundle.unity3d", new EventHandlerAssetBundle(this.OnAssetDownloaded), new EventHandlerServiceError(this.OnHTTPError), new EventHandlerServiceTimeOut(this.OnHTTPTimeOut), 10);
    }

    void OnAssetDownloaded(AssetBundle asset, string transactionId)
    {
        Debug.Log("[TestWWWScene] [OnAssetDownloaded] asset = " + asset.ToString());
        Instantiate(asset.mainAsset);
    }

    void GetAssetBundleCache()
    {
        GameObject go = GameObject.Find("SpheresInAssetBundle(Clone)");
        if (go != null)
        {
            GameObject.Destroy(go);
        }

        HTTPRequest request = new HTTPRequest();
        request.url = "http://ucss.webjema.com/tests/http/assets/SpheresInAssetBundle.unity3d";
        // request.transactionId = ""; // will be generated automatically
        request.onError = new EventHandlerServiceError(this.OnHTTPError);
        request.onTimeOut = new EventHandlerServiceTimeOut(this.OnHTTPTimeOut);
        request.timeOut = 5;

        request.assetBundleCallback = new EventHandlerAssetBundle(this.OnAssetDownloaded);
        request.assetVersion = 1;
        request.assetCRC = 0;

        UCSS.HTTP.GetAssetBundleOrCache(request);
    }
#endif

    void GetAudioClip()
    {
        UCSS.HTTP.GetAudioClip("http://ucss.webjema.com/tests/http/sounds/game-sound-correct.wav", new EventHandlerAudioClip(this.OnAudioDownloaded), new EventHandlerServiceError(this.OnHTTPError));
    }

    void OnAudioDownloaded(AudioClip audioClip, string transactionId)
    {
        Debug.Log("[TestWWWScene] [OnAudioDownloaded] audioClip.length = " + audioClip.length);
        this.audioSource.clip = audioClip;
        this.audioSource.Play();
    }
#if UNITY_PRO_LICENSE
    void GetMovie()
    {
        this.cube1.material.mainTexture = null;
        this.movieTextureProgress = 0;

        HTTPRequest request = new HTTPRequest();
        request.url = "http://ucss.webjema.com/tests/http/images/sky-fog-mp4basic-small.ogv";
        request.movieTextureCallback = new EventHandlerMovieTexture(this.OnMovieDownloaded);
        request.onError = new EventHandlerServiceError(this.OnHTTPError);
        request.onTimeOut = null;
        request.timeOut = 0;

        request.downloadProgress = new EventHandlerDownloadProgress(this.OnMovieDownloadProgress);

        UCSS.HTTP.GetMovie(request);
    }

    void OnMovieDownloadProgress(float progress)
    {
        this.movieTextureProgress = progress;
    }

    void OnMovieDownloaded(MovieTexture movie, string transactionId)
    {
        Debug.Log("[TestWWWScene] [OnMovieDownloaded] movie.duration = " + movie.duration + ", movie.isReadyToPlay = " + movie.isReadyToPlay);
        this.cube1.material.mainTexture = movie;
        movie.Play();
        movie.loop = true;
    }
#endif

    void PostBytes()
    {
        byte[] bytes = new byte[10] { 0x25, 0x26, 0x27, 0x28, 0x29, 0x30, 0x31, 0x32, 0x33, 0x0D };
        UCSS.HTTP.PostBytes("http://ucss.webjema.com/tests/http/save-bytes.php", bytes, null, new EventHandlerHTTPString(this.OnTextDownloaded), new EventHandlerServiceError(this.OnHTTPError));
    } // PostBytes

    void PostForm()
    {
        WWWForm data = new WWWForm();
        data.AddField("firstName", "John");
        data.AddField("lastName", "Smith");
        data.AddField("age", "32");
        data.AddField("time", Common.GetSeconds());

        UCSS.HTTP.PostForm("http://ucss.webjema.com/tests/http/save-form.php", data, new EventHandlerHTTPBytes(this.OnBytesDownloaded), new EventHandlerServiceError(this.OnHTTPError));
    } // PostForm

    void OnBytesDownloaded(byte[] bytes, string transactionId)
    {
        Debug.Log("[TestWWWScene] [OnBytesDownloaded] bytes answer length = " + bytes.Length + ", to string = \"" + System.Text.Encoding.UTF8.GetString(bytes) + "\"");
    }

    void TestTimeout()
    {
        Debug.Log("Wait for 5 seconds...");
        UCSS.HTTP.GetString("http://ucss.webjema.com/tests/http/timeout.php", new EventHandlerHTTPString(this.OnTextDownloaded), new EventHandlerServiceError(this.OnHTTPError), new EventHandlerServiceTimeOut(this.OnHTTPTimeOut), 5);
    }

    void TestError()
    {
        UCSS.HTTP.GetString("http://ucss.webjema.com/tests/NoT-ExistS/http/timeout.php", new EventHandlerHTTPString(this.OnTextDownloaded), new EventHandlerServiceError(this.OnHTTPError));
    }

} // TestWWW