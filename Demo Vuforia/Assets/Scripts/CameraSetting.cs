using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Vuforia
{
    public class CameraSetting : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            var vuforia = VuforiaARController.Instance;
            vuforia.RegisterVuforiaStartedCallback(OnVuforiaStarted); //程序启动的时候回调函数
            vuforia.RegisterOnPauseCallback(OnPuased);                //程序暂停的时候
        }


        // Update is called once per frame
        void Update()
        {
        }


        /// <summary>
        /// Vuforia 启动完成的时候，回调函数
        /// </summary>
        private void OnVuforiaStarted()
        {
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO); //自动对焦
        }


        /// <summary>
        /// Vuforia 暂停的时候，回调函数
        /// </summary>
        /// <param name="isPaused"></param>
        private void OnPuased(bool isPaused)
        {
        }


        /// <summary>
        /// 当点击按钮的时候，自动对焦
        /// </summary>
        public void OnClick()
        {
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO); //强制对焦
        }


        private bool isFront;


        /// <summary>
        /// 设置前后摄像头
        /// </summary>
        public void 设置前后摄像头()
        {
            CameraDevice.Instance.Stop();   //停止
            CameraDevice.Instance.Deinit(); //取消初始化
            if (isFront == false)
            {
                CameraDevice.Instance.Init(CameraDevice.CameraDirection.CAMERA_FRONT); //初始化前、后
                isFront = true;
            }
            else
            {
                CameraDevice.Instance.Init(CameraDevice.CameraDirection.CAMERA_BACK); //初始化前、后
                isFront = false;
            }
            CameraDevice.Instance.Start(); //开启
        }


        private bool on;

        /// <summary>
        /// 是否打开闪光灯
        /// </summary>
        /// <param name="on"></param>
        public void FlashTouch()
        {
            if (on)
            {
                on = false;
            }
            else
            {
                on = true;
            }
            CameraDevice.Instance.SetFlashTorchMode(on);
        }
    }
}