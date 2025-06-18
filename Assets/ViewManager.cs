using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class ViewManager : MonoBehaviour
{
    public ViewPoint currentViewPoint;
    [SerializeField] List<ViewPoint> viewList = new List<ViewPoint>();
    [SerializeField] float cameraSensivity = 60;

    public float buttonHideAngle = 45;

    private void Start()
    {
        if (currentViewPoint == null)
            currentViewPoint = viewList[0];

        transform.position = currentViewPoint.transform.position;
        transform.rotation = currentViewPoint.transform.rotation;
        
        for (int i = 0; i < 360; i += 10)
        {
            int ii = i;
            ii -= 180;
            ii *= -1;
            ii = Mathf.Clamp(ii, 60 - 180, 300 - 180);
            ii += 180;
            Debug.Log(ii);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            PreviousViewPoint(); 
            ReBuildViewPointOrder();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            NextViewPoint(); 
            ReBuildViewPointOrder();
        }

        if ((transform.position - currentViewPoint.transform.position).magnitude > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, currentViewPoint.transform.position, Time.deltaTime * currentViewPoint.fromToMoveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, currentViewPoint.transform.rotation, Time.deltaTime * currentViewPoint.fromToRotateSpeed);
        }
        else if (Input.GetMouseButton(1))
        {
            float horizontalMouse = Input.GetAxis("Mouse X");
            horizontalMouse = horizontalMouse * Time.deltaTime * cameraSensivity;
            float verticalMouse = -Input.GetAxis("Mouse Y");
            verticalMouse = verticalMouse * Time.deltaTime * cameraSensivity;

#region  horizontalRotateClamp
            if (currentViewPoint.lockHorizontalRotation)
            {
                Vector3 horizontalEuler = transform.eulerAngles;
                float newHorizontalRotate = horizontalEuler.y + horizontalMouse;
                newHorizontalRotate = Mathf.Clamp(newHorizontalRotate, currentViewPoint.horizontalMinAngle, currentViewPoint.horizontalMaxAngle);
                transform.eulerAngles = new Vector3(horizontalEuler.x, newHorizontalRotate, horizontalEuler.z);
            } else transform.Rotate(Vector3.up, horizontalMouse, Space.World);
#endregion

#region verticalRotateClamp
            Vector3 verticalEuler = transform.eulerAngles;
            float verticalRotate = verticalEuler.x + verticalMouse;

            if (verticalRotate < 180) verticalRotate *= -1;
            else if (verticalRotate > 180) verticalRotate = 360 - verticalRotate;

            verticalRotate = Mathf.Clamp(verticalRotate, currentViewPoint.verticalMinAngle, currentViewPoint.verticalMaxAngle);

            if (verticalRotate < 0) verticalRotate *= -1;
            else if (verticalRotate > 0) verticalRotate = 360 - verticalRotate;

            transform.eulerAngles = new Vector3(verticalRotate, verticalEuler.y, verticalEuler.z);
#endregion

            if (currentViewPoint.savePointRotate)
                currentViewPoint.transform.rotation = transform.rotation;
        }
    }

    public void PreviousViewPoint()
    {
        int viewPointIndex = viewList.IndexOf(currentViewPoint);
            if (viewPointIndex == 0)
                currentViewPoint = viewList[viewList.Count - 1];
            else
                currentViewPoint = viewList[viewPointIndex - 1];
    }

    public void NextViewPoint()
    {
        int viewPointIndex = viewList.IndexOf(currentViewPoint);
        if (viewPointIndex == viewList.Count - 1)
            currentViewPoint = viewList[0];
        else
            currentViewPoint = viewList[viewPointIndex + 1];
    }

    public void SetActiveViewPoint(ViewPoint viewPoint) => currentViewPoint = viewPoint;

    public void ReBuildViewPointOrder()
    {
        Dictionary<float, ViewPoint> viewPointDictionary = new Dictionary<float, ViewPoint>();
        List<float> distances = new List<float>();
        foreach (ViewPoint viewPoint in viewList)
        {
            float distance = Vector3.Distance(viewPoint.transform.position, currentViewPoint.transform.position);
            viewPointDictionary.Add(distance, viewPoint);
            distances.Add(distance);
        }
        distances.Sort();
        distances.Reverse();

        foreach (float distance in distances)
            viewPointDictionary[distance].UpUIHierarchy();
    }
}