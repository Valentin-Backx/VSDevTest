using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdUnitIdWrapper : MonoBehaviour
{
    static AdUnitIdWrapper _Instance;
    public static AdUnitIdWrapper Instance
    {
        get
        {
            return _Instance;
        }
    }

    /// <summary>
    /// change the Ad Unit Id Here
    /// </summary>
    [SerializeField]
    string m_adUnitId = "f4280fh0318rf0h2";
    public string adUnitId
    {
        get
        {
            return m_adUnitId;
        }
    }

    private void Awake()
    {
        if (_Instance != null)
        {
            Destroy(this);
            return;
        }
        _Instance = this;
    }
}
