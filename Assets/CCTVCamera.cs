using System;
using UnityEngine;

public class CCTVCamera : MonoBehaviour
{
    [SerializeField] Transform transformInThisCamera;

    private void Start()
    {
        transformInThisCamera = transform.Find("CameraTransform");
    }

    delegate void OnCCTVActivated(GameObject activeCCTV);
    Action<GameObject> OnCCTVActivatedEvent;
    public void SetActiveCCTV(bool permanently = false)
    {
        OnCCTVActivatedEvent.Invoke(gameObject);
        Transform CameraTransform = Camera.main.transform;

        if (permanently)
        {
            CameraTransform.position = transformInThisCamera.position;
            CameraTransform.rotation = transformInThisCamera.rotation;
            return;
        }

        Debug.Log("CCTV Camera is active");
    }
}
