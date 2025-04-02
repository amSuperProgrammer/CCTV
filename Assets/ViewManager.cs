using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class ViewManager : MonoBehaviour
{
    [SerializeField] List<ViewPoint> viewList = new List<ViewPoint>();
    public ViewPoint currentViewPoint;

    [SerializeField] float cameraSensivity = 60;
    [SerializeField] float fromToMoveSpeed = 5;
    [SerializeField] float fromToRotateSpeed = 5;

    public float buttonHideAngle = 45;

    private void Start()
    {
        currentViewPoint = viewList[0];

        transform.position = currentViewPoint.transform.position;
        transform.rotation = currentViewPoint.transform.rotation;
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

        if (transform.position != currentViewPoint.transform.position)
        {
            transform.position = Vector3.Lerp(transform.position, currentViewPoint.transform.position, Time.deltaTime * fromToMoveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, currentViewPoint.transform.rotation, Time.deltaTime * fromToRotateSpeed);
        }
        else if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            mouseX = mouseX * Time.deltaTime * cameraSensivity;
            float mouseY = -Input.GetAxis("Mouse Y");
            mouseY = mouseY * Time.deltaTime * cameraSensivity;

            if (currentViewPoint.lockHorizontalRotation)
            {
                float angle = Vector3.Angle(Camera.main.transform.forward, currentViewPoint.transform.forward);
                Debug.Log($"Next angle: {angle - Mathf.Abs(mouseX)}");
                // if (angle - Mathf.Abs(mouseX) < currentViewPoint.horizontalLockAngle)
                    transform.Rotate(Vector3.up, mouseX, Space.World);
            }
            else
                transform.Rotate(Vector3.up, mouseX, Space.World);

            
            if (currentViewPoint.lockVerticalRotation)
            {
                float angle = Vector3.Angle(Camera.main.transform.forward, currentViewPoint.transform.forward);
                Debug.Log($"Next angle: {angle - Mathf.Abs(mouseY)}");
                // if (angle - Mathf.Abs(mouseY) < currentViewPoint.verticalLockAngle)
                    transform.Rotate(Vector3.right, mouseY);
            }
            else
                transform.Rotate(Vector3.right, mouseY);
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