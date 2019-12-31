using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebManager : MonoBehaviour
{

    #region 单例模式控制web请求相关
    private static WebManager _mgr = null;
    float startTime = 0;
    public static WebManager _instance
    {
        get
        {
            if (_mgr == null)
            {
                GameObject mgrGo = new GameObject("WebManager");
                _mgr = mgrGo.AddComponent<WebManager>();
            }
            return _mgr;
        }

        set
        {
            _mgr = value;
        }
    }
    #endregion

    // 获取Commoneader
    public static Dictionary<string, string> GetCommonHeaderDic()
    {
        return new Dictionary<string, string> {

            { "appId","BBYApETV"},
            { "appSecret","ae402deba8aeea8ca3da5d3a714e40df657e3d43"},
            { "deviceId","asdf"},

        };
    }

    // //UnityWebRequest请求方式   common
    // public void SendRequest(string url, byte[] postData, Dictionary<string, string> headerDic, Action<UnityWebRequest> successCallBack, Action<int, String> failedCallBack)
    // {
    //     startTime = Time.realtimeSinceStartup;
    //     StartCoroutine(CoroutineRequest(AppConst.BaseUrl + url, postData, headerDic, successCallBack, failedCallBack));
    // }
    // //postData为空的时候是Get请求，反之则为Post请求
    // IEnumerator CoroutineRequest(string url, byte[] postData, Dictionary<string, string> headerDic, Action<UnityWebRequest> successCallBack, Action<int, String> failedCallBack)
    // {
    //     var verb = postData == null ? UnityWebRequest.kHttpVerbGET : UnityWebRequest.kHttpVerbPOST;
    //     UnityWebRequest uwr = new UnityWebRequest(url, verb);
    //     uwr.chunkedTransfer = false;
    //     UploadHandler formUploadHandler = new UploadHandlerRaw(postData);
    //     formUploadHandler.contentType = "application/json";
    //     uwr.uploadHandler = formUploadHandler;
    //     uwr.downloadHandler = new DownloadHandlerBuffer();
    //     foreach (var header in headerDic)
    //     {
    //         uwr.SetRequestHeader(header.Key, header.Value);
    //     }
    //     yield return uwr.SendWebRequest();
    //     CommonMethod(uwr, successCallBack, failedCallBack);

    // }

    #region 分开的 get Post 请求
    #region get  请求 获取字符串类型
    public void SendGetRquest<T>(string url, Dictionary<string, string> form, Action<T> successCallBack, Action<int, String> failedCallBack)
    {

        StartCoroutine(GetRequest<T>(AppConst.BaseUrl + url, form, GetCommonHeaderDic(), successCallBack, failedCallBack));

    }
    private IEnumerator GetRequest<T>(string url, Dictionary<string, string> form, Dictionary<string, string> headerDic, Action<T> callback, Action<int, String> failedCallBack)
    {

        if (form != null && form.Count > 0)
        {
            url += "?";
            foreach (var key in form.Keys)
            {
                url += (key + "=" + form[key] + "&");
            }
        }
        using (UnityWebRequest uwr = UnityWebRequest.Get(url))
        {
            foreach (var key in headerDic.Keys)
            {
                uwr.SetRequestHeader(key, headerDic[key]);
            }
            yield return uwr.SendWebRequest();
            CommonMethod<T>(uwr, callback, failedCallBack);
        }
    }
    #endregion
    #region Post请求方式

    public void SendPostRequest<T>(string url, Dictionary<string, string> form, Action<T> successCallBack, Action<int, String> failedCallBack)
    {
        StartCoroutine(PostRequest<T>(AppConst.BaseUrl + url, form, GetCommonHeaderDic(), successCallBack, failedCallBack));
    }
    IEnumerator PostRequest<T>(string url, Dictionary<string, string> form, Dictionary<string, string> headerDic, Action<T> callback, Action<int, String> failedCallBack)
    {
        UnityWebRequest uwr = UnityWebRequest.Post(url, form);
        foreach (var key in headerDic.Keys)
        {
            uwr.SetRequestHeader(key, headerDic[key]);
        }
        yield return uwr.SendWebRequest();
        CommonMethod<T>(uwr, callback, failedCallBack);

    }

    #endregion
    #endregion


    #region 获取Texture
    public void RequestTexture(string url, Action<Texture2D> callback)
    {
        startTime = Time.realtimeSinceStartup;
        StartCoroutine(GetTexture(url, callback));
    }
    IEnumerator GetTexture(string url, Action<Texture2D> callback)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(url);
        DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
        uwr.downloadHandler = downloadTexture;
        yield return uwr.SendWebRequest();
        Texture2D t = null;
        if (!(uwr.isNetworkError || uwr.isHttpError))
        {
            t = downloadTexture.texture;
        }
        else
        {
            Debug.Log("download Texture Error");
        }

        if (callback != null)
        {
            Debug.Log("请求消耗时间：" + (Time.realtimeSinceStartup - startTime));
            callback(t);
        }


    }
    #endregion

    #region 获取视频

    //public void RequestVideo(string url,Action<MovieTexture> callback)
    //{
    //    StartCoroutine(GetVideo(baseUrl+url, callback));
    //}
    //IEnumerator GetVideo(string url,Action<MovieTexture> callbcak)
    //{
    //    using(var uwr= UnityWebRequestMultimedia.GetMovieTexture(url))
    //    {
    //        yield return uwr.SendWebRequest();
    //        if(!(uwr.isNetworkError||uwr.isHttpError))
    //        {
    //            if(callbcak!=null)
    //            {
    //                callbcak(DownloadHandlerMovieTexture.GetContent(uwr));
    //            }
    //        }
    //    }
    //}
    #endregion
    #region 获取音频
    public void RequestAudio(string url, Action<AudioClip> callback, AudioType audioType = AudioType.WAV)
    {
        StartCoroutine(GetAudioClip(AppConst.BaseUrl + url, callback, audioType));
    }
    IEnumerator GetAudioClip(string url, Action<AudioClip> callback, AudioType audioType = AudioType.WAV)
    {
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
        {
            yield return uwr.SendWebRequest();
            if (!(uwr.isNetworkError || uwr.isHttpError))
            {
                if (callback != null)
                {
                    callback(DownloadHandlerAudioClip.GetContent(uwr));
                }
            }
        }
    }
    #endregion



    #region 下载AssetBundle 

    public void RequestAB(string url, Action<AssetBundle> callbcak)
    {
        StartCoroutine(GetAssetBundle(AppConst.BaseUrl + url, callbcak));
    }
    IEnumerator GetAssetBundle(string url, Action<AssetBundle> callbcak)
    {
        UnityWebRequest request = new UnityWebRequest(url);
        DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(request.url, uint.MaxValue);
        request.downloadHandler = handler;
        yield return request.SendWebRequest();
        if (!(request.isHttpError || request.isNetworkError))
        {
            if (callbcak != null)
            {
                callbcak(handler.assetBundle);
            }
        }
    }

    #endregion

    #region  文件上传相关

    public void UploadFile(string url, byte[] contentBytes, Action<bool> callback)
    {
        StartCoroutine(UploadByPut(AppConst.BaseUrl + url, contentBytes, callback));
    }
    // 可以结合WebUtility中的Formatter方法将对象转化为byte数组传递给服务端
    IEnumerator UploadByPut(string url, byte[] contentBytes, Action<bool> callback, string contentType = "application/octet-stream")
    {
        UnityWebRequest uwr = new UnityWebRequest(url);
        UploadHandler uploader = new UploadHandlerRaw(contentBytes);
        uploader.contentType = contentType;

        uwr.uploadHandler = uploader;

        yield return uwr.SendWebRequest();

        bool isUploadSuccess = true;
        if (uwr.isNetworkError || uwr.isHttpError)
        {
            isUploadSuccess = false;
        }
        if (callback != null)
        {
            callback(isUploadSuccess);  //isUploadSuccess参数表示是否上传成功
        }
    }
    #endregion


    //HttpWebRequest Get请求
    public void HttpRequest(string url, Action<string> callback)
    {
        startTime = Time.realtimeSinceStartup;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(AppConst.BaseUrl + url);
        request.Method = "GET";
        request.ContentType = "application/json";
        request.UserAgent = null;
        request.Timeout = 10000; //这里设置了十秒超时时间
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream resStream = response.GetResponseStream();
        StreamReader streamReader = new StreamReader(resStream, Encoding.UTF8);
        string readerStr = streamReader.ReadToEnd();
        streamReader.Close();
        resStream.Close();
        if (callback != null)
        {
            Debug.Log("请求消耗时间：" + (Time.realtimeSinceStartup - startTime));
            callback(readerStr);
        }
    }

    //HttpWebRequest Post请求
    public void HttpRequest(string url, string postData, Dictionary<string, string> headerDic, Action<string> callback)
    {
        startTime = Time.realtimeSinceStartup;
        byte[] data = Encoding.UTF8.GetBytes(postData);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(AppConst.BaseUrl + url);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.UserAgent = null;
        request.Timeout = 100000; //这里设置了十秒超时时间
        foreach (var key in headerDic.Keys)
        {
            request.Headers.Add(key, headerDic[key]);
        }

        request.ContentLength = data.Length;

        using (Stream newStream = request.GetRequestStream())
        {
            //// Send the data.
            newStream.Write(data, 0, data.Length);
        }

        // Get response
        HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
        string content = reader.ReadToEnd();
        if (callback != null)
        {
            Debug.Log("请求消耗时间：" + (Time.realtimeSinceStartup - startTime));
            callback(content);
        }
    }
    void CommonMethod<T>(UnityWebRequest uwr, Action<T> callback, Action<int, String> failedCallBack)
    {
        switch (uwr.responseCode)
        {
            case 200:
                if (callback != null)
                {
                    // 解析数据
                    String baseResponseStr = uwr.downloadHandler.text;
                    Debug.Log(baseResponseStr);

                    BaseResponse<T> baseResponse = JsonUtility.FromJson<BaseResponse<T>>(baseResponseStr);

                    if (baseResponse.code == 0)
                    {
                        callback(baseResponse.data);
                    }
                    else
                    {
                        failedCallBack(baseResponse.code, "");
                    }
                }
                break;
            // case 40Ï4:
            default:
                //此处UI提示
                //{"timestamp":1562662472679,"status":404,"error":"Not Found","message":"No message available","path":"/game/v1/user/%7B2175b2265eb37c3242c1ed258839b650%7D/login"}
                failedCallBack((int)uwr.responseCode, "");
                //UIManager.HideAll();
                // UIManager.Show("UILoading");
                break;
        }
    }
}
