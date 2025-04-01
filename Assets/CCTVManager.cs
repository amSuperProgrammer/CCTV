using UnityEngine;
using System.Collections.Generic;

public class CCTVManager : MonoBehaviour
{
    CCTVCamera activeCCTV;

    [SerializeField] List<CCTVCamera> CCTVList = new List<CCTVCamera>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CCTVList[0].SetActiveCCTV(permanently: true);
    }

    
}
