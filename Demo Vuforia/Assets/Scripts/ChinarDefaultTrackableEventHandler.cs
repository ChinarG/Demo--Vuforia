/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using System.Collections;
using UnityEngine;
using Vuforia;


/// <summary>
///     A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class ChinarDefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    private AudioSource audioSource;
    public  AudioClip   WelcomeClip; //欢迎音效
    public  GameObject  AixiGameObject;
    public  GameObject  Huo;
    public  GameObject  Effect;

    #region PRIVATE_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;

    #endregion // PRIVATE_MEMBER_VARIABLES

    #region UNTIY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        audioSource = GetComponent<AudioSource>();
    }

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED  ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus      == TrackableBehaviour.Status.NOT_FOUND)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PRIVATE_METHODS

    /// <summary>
    /// 当我们找到了需要识别的物体
    /// </summary>
    protected virtual void OnTrackingFound()
    {
        if (!audioSource.isPlaying) //如果音乐没有在播放
        {
            audioSource.Play();
        }
        GameObject aiXi              = Instantiate(AixiGameObject, transform.position - new Vector3(0, 1.8f, 0), Quaternion.identity); //实例化 寒冰
        aiXi.transform.parent        = transform;                                                                                      //设置父物体
        GameObject flame             = Instantiate(Huo, transform.position, Quaternion.identity);                                      //实例化 火焰
        flame.transform.parent       = transform;                                                                                      //设置父物体
        GameObject bloodPuddle       = Instantiate(Effect, transform.position, Quaternion.identity);                                   //实例化 血池
        bloodPuddle.transform.parent = transform;                                                                                      //设置父物体

        StopCoroutine(XieCheng(flame, bloodPuddle));
        StartCoroutine(XieCheng(flame, bloodPuddle)); //开启携程
    }


    /// <summary>
    /// 删除特效携程
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private IEnumerator XieCheng(GameObject a, GameObject b)
    {
        yield return new WaitForSeconds(4f);
        Destroy(a);
        Destroy(b);
        AudioSource.PlayClipAtPoint(WelcomeClip, transform.position);
    }


    /// <summary>
    /// 丢失了需要识别的物体
    /// </summary>
    protected virtual void OnTrackingLost()
    {
        Destroy(GameObject.Find("AiXi(Clone)"));         //删除寒冰
        Destroy(GameObject.Find("Tonado_Flame(Clone)")); //删除寒冰
        Destroy(GameObject.Find("Blood_Puddle(Clone)")); //删除寒冰
    }

    #endregion // PRIVATE_METHODS
}