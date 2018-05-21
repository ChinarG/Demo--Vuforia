using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class ControllerAiXi : MonoBehaviour
{
    private Camera ARCamera;


    void Start()
    {
        ARCamera = GetComponent<Camera>();
    }


    // Update is called once per frame
    void Update()
    {
    }


    /// <summary>
    /// 当点击按钮的时间
    /// </summary>
    public void OnScreenShortClick()
    {
        System.DateTime now   = System.DateTime.Now;     //系统当前时间
        string          times = now.ToString();          //转字符串
        times                 = times.Trim();            //去空格
        times                 = times.Replace("/", "-"); //把字符串中 指定字符转为新字符

        string fileName = "ARScreenShort" + times + ".png";  //定义文件名
        if (Application.platform == RuntimePlatform.Android) //当前平台是安卓平台
        {
            //Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false); //构造一个屏幕纹理对象
            //texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);                      //读取像素
            //texture.Apply();                                                                            //应用

            //byte[] bytes = texture.EncodeToPNG(); //转为字节数组

            //string destination = "root/DCIM/100MEDIA";
            //if (!Directory.Exists(destination)) //如果不存在
            //{
            //    Directory.CreateDirectory(destination); //就创建一个目录
            //}

            //string pathSave = destination + "/" + fileName; //保存文件的路径
            //File.WriteAllBytes(pathSave, bytes);            //写入数据

            RenderTexture rt       = new RenderTexture(Screen.width, Screen.height, 1); //获取 AR相机的渲染纹理
            ARCamera.targetTexture = rt;                                                //设置相机目标纹理
            ARCamera.Render();                                                          //手动的刷新纹理
            RenderTexture.active = rt;                                                  //激活纹理渲染器，并设置为rt

            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false); //构造一个屏幕纹理对象
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);                      //读取像素
            texture.Apply();                                                                            //应用

            ARCamera.targetTexture = null; //质空
            RenderTexture.active   = null; //质空
            Destroy(rt);                   //删除

            byte[] bytes = texture.EncodeToPNG(); //转为字节数组

            string destination = "sdcard/DCIM/截屏";
            if (!Directory.Exists(destination)) //如果不存在
            {
                Directory.CreateDirectory(destination); //就创建一个目录
            }

            string pathSave = destination + "/" + fileName; //保存文件的路径
            File.WriteAllBytes(pathSave, bytes);            //写入数据


            //不包含UI方式截屏
        }
    }
}