using System.IO;
using System.Linq;
using System.Diagnostics;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using System.Collections.Generic;

public class LoginScene : MonoBehaviour
{

    BaseResponse<Data> singStateEntity;

    public void OnLoginClick()
    {
        Debug.Log("点击登录");

        // WebManager._instance.SendGetRquest<Data>("checkIn/signState", null,
        //     successCallBack: (Data value) =>
        //      {
        //          Debug.Log(value.statusList[0].time);
        //      },
        //     failedCallBack: (int code, string msg) =>
        //      {
        //          Debug.Log(code);
        //      });


        Dictionary<string, string> body1 = new Dictionary<string, string>();
        body1.Add("version", "0.0.2");
        body1.Add("os", "1");
        WebManager._instance.SendGetRquest<Data>("user/version", body1,
           successCallBack: (Data value) =>
            {
                // Debug.Log(value.statusList[0].time);
            },
           failedCallBack: (int code, string msg) =>
            {
                Debug.Log(code);
            });




        // Dictionary<string, string> body = new Dictionary<string, string>();
        // body.Add("thirdPartyId", "sdafklajsf");
        // body.Add("thirdPartyType", "1");

        // WebManager._instance.SendPostRequest<Data>("user/checkLogin", body,
        //         successCallBack: (Data value) =>
        //     {
        //         Debug.Log(value.statusList[0].time);
        //     },
        //     failedCallBack: (int code, string msg) =>
        //     {
        //         Debug.Log("登录失败" + code);
        //     });
    }




    // String loginInfoStr = value.downloadHandler.text;
    // Text text = GameObject.Find("TextLoginInfo").GetComponent<Text>();
    // singStateEntity = JsonUtility.FromJson<BaseResponse<Data>>(loginInfoStr);
    // text.text = loginInfoStr;
}