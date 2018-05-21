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
    public  AudioClip   WelcomeClip; //��ӭ��Ч
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
    /// �������ҵ�����Ҫʶ�������
    /// </summary>
    protected virtual void OnTrackingFound()
    {
        if (!audioSource.isPlaying) //�������û���ڲ���
        {
            audioSource.Play();
        }
        GameObject aiXi              = Instantiate(AixiGameObject, transform.position - new Vector3(0, 1.8f, 0), Quaternion.identity); //ʵ���� ����
        aiXi.transform.parent        = transform;                                                                                      //���ø�����
        GameObject flame             = Instantiate(Huo, transform.position, Quaternion.identity);                                      //ʵ���� ����
        flame.transform.parent       = transform;                                                                                      //���ø�����
        GameObject bloodPuddle       = Instantiate(Effect, transform.position, Quaternion.identity);                                   //ʵ���� Ѫ��
        bloodPuddle.transform.parent = transform;                                                                                      //���ø�����

        StopCoroutine(XieCheng(flame, bloodPuddle));
        StartCoroutine(XieCheng(flame, bloodPuddle)); //����Я��
    }


    /// <summary>
    /// ɾ����ЧЯ��
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
    /// ��ʧ����Ҫʶ�������
    /// </summary>
    protected virtual void OnTrackingLost()
    {
        Destroy(GameObject.Find("AiXi(Clone)"));         //ɾ������
        Destroy(GameObject.Find("Tonado_Flame(Clone)")); //ɾ������
        Destroy(GameObject.Find("Blood_Puddle(Clone)")); //ɾ������
    }

    #endregion // PRIVATE_METHODS
}