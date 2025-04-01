using UnityEngine;
using System.Collections.Generic;

public class ViewManager : MonoBehaviour
{
    [SerializeField] List<ViewPoint> viewList = new List<ViewPoint>();
    ViewPoint currentViewPoint;

    [SerializeField] float cameraSensivity = 60;
    [SerializeField] float fromToMoveSpeed = 5;
    [SerializeField] float fromToRotateSpeed = 5;

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
            int viewPointIndex = viewList.IndexOf(currentViewPoint);
            if (viewPointIndex == 0)
                currentViewPoint = viewList[viewList.Count - 1];
            else
                currentViewPoint = viewList[viewPointIndex - 1];
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            int viewPointIndex = viewList.IndexOf(currentViewPoint);
            if (viewPointIndex == viewList.Count - 1)
                currentViewPoint = viewList[0];
            else
                currentViewPoint = viewList[viewPointIndex + 1];
        }

        if (transform.position != currentViewPoint.transform.position)
        {
            transform.position = Vector3.Lerp(transform.position, currentViewPoint.transform.position, Time.deltaTime * fromToMoveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, currentViewPoint.transform.rotation, Time.deltaTime * fromToRotateSpeed);
        }
        else if (Input.GetMouseButton(0)) CameraRotate();

    }

    private void CameraRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        mouseX = mouseX * Time.deltaTime * cameraSensivity;
        float mouseY = -Input.GetAxis("Mouse Y");
        mouseY = mouseY * Time.deltaTime * cameraSensivity;

        transform.Rotate(Vector3.up, mouseX, Space.World);
        transform.Rotate(Vector3.right, mouseY);
    }
}
