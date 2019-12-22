using UnityEngine;
using System.Collections;
using System;

public class RGPDManager : MonoBehaviour
{
    const string REG_RGPD = "RGPD_CONSENT";

    /// <summary>
    /// UI for querying RGPD consent from the user
    /// </summary>
    [SerializeField]
    GameObject m_rgpdConsentUI=null;

    static bool? m_rgpdConsentValue;
    public static bool? RgpdConsentalue
    {
        get
        {
            return m_rgpdConsentValue;
        }
    }

    /// <summary>
    /// callback when the use answered to the RGPD consent query
    /// </summary>
    /// <param name="consent"></param>
    public void UserAnsweredRGPDConsentRequest(bool consent)
    {
        m_rgpdConsentValue = consent;
        PlayerPrefs.SetInt(REG_RGPD, consent ? 1 : 0);
        m_rgpdConsentUI.SetActive(false);
    }

    /// <summary>
    /// checks if the RGPD consent query has been previously answered,
    /// </summary>
    /// <param name="onConsentQueryAnswered">Called when the user has granted or denied consent</param>
    public void RetrieveRGPDValue(Action onConsentQueryAnswered)
    {
        if(!PlayerPrefs.HasKey(REG_RGPD))
        {
            AskConsent(onConsentQueryAnswered);
        }else
        {
            m_rgpdConsentValue = PlayerPrefs.GetInt(REG_RGPD, -1)==1;
            onConsentQueryAnswered();
        }
    }

    /// <summary>
    /// asks user for consent, activates the rgpd dialog
    /// </summary>
    /// <param name="startGame"></param>
    void AskConsent(Action startGame)
    {
        m_rgpdConsentUI.SetActive(true);
        StartCoroutine(WaitForConsentValue(startGame));
    }

    /// <summary>
    /// coroutine running for as long as the user did not filled the consent value
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    IEnumerator WaitForConsentValue(Action callback)
    {
        while(!m_rgpdConsentValue.HasValue)
        {
            yield return null;
        }
        callback();
    }
}
