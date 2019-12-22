using UnityEngine;
using System.Collections;
using _3rdParty;
using System;

public class AdBuffer : MonoBehaviour
{
    /// <summary>
    /// is an ad currently buffered and ready to be displayed?
    /// </summary>
    bool m_isABufferedAdReady;

    /// <summary>
    /// are we already querying for an ad? (we consider that we can only require one ad at a time)
    /// </summary>
    bool m_currentWaitingForAd;

    static AdBuffer _Instance;
    public static AdBuffer Instance
    {
        get
        {
            return _Instance;
        }
    }

    private void Awake()
    {
        if(_Instance!=null)
        {
            Destroy(gameObject);
            return;
        }
        _Instance = this;
    }

    IEnumerator Start()
    {
        TopAds.OnAdLoadedEvent += TopAds_OnAdLoadedEvent;
        TopAds.OnAdFailedEvent += TopAds_OnAdFailedEvent;
        TopAds.OnAdShownEvent += TopAds_OnAdShownEvent;
        while(!RGPDManager.RgpdConsentalue.HasValue)
        {
            yield return null;
        }
        //once the rgpd consent has a value, we buffer the first ad in anticipation
        BufferAd();
    }

    /// <summary>
    /// we have just shown an ad, buffering the next one right away
    /// </summary>
    private void TopAds_OnAdShownEvent()
    {
        //currently buffered ad was consumed
        m_isABufferedAdReady = false;
        //...buffering new ad
        BufferAd();
    }

    /// <summary>
    /// the current request for an ad has failed, retrying...
    /// </summary>
    private void TopAds_OnAdFailedEvent()
    {
        //failed to buffer ad, trying again...
        BufferAd();
    }

    /// <summary>
    /// the current request for an ad is successfull, we now consider we have an ad buffered and ready to go
    /// </summary>
    private void TopAds_OnAdLoadedEvent()
    {
        //a new ad is now buffered
        m_isABufferedAdReady = true;
    }

    /// <summary>
    /// displaying the ad,
    /// </summary>
    public void DisplayAd()
    {
        if(m_isABufferedAdReady)
        {
            TopAds.ShowAd(AdUnitIdWrapper.Instance.adUnitId);
        }else if(!m_currentWaitingForAd)
        {
            StartCoroutine(WaitForAdRoutine(() => {
                TopAds.ShowAd(AdUnitIdWrapper.Instance.adUnitId);
            }));
        }
    }

    /// <summary>
    /// routine to await an ad, with optional callback
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    IEnumerator WaitForAdRoutine(Action callback=null)
    {
        m_currentWaitingForAd = true;
        while (!m_isABufferedAdReady)
        {
            yield return null;
        }
        m_currentWaitingForAd = false;
        callback?.Invoke();
    }

    /// <summary>
    /// action of starting RequestAd
    /// </summary>
    void BufferAd()
    {
        TopAds.RequestAd(AdUnitIdWrapper.Instance.adUnitId);
    }

}
