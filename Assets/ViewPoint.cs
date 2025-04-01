using UnityEngine;
using UnityEngine.UIElements;

public class ViewPoint : MonoBehaviour
{
    VisualElement root;
    VisualElement cameraPoint;
    private void Start()
    {
        root = GameObject.Find("UIDocument").GetComponent<UIDocument>().rootVisualElement;
        VisualElement cameraPointTemplate = Resources.Load<VisualTreeAsset>("CameraPointTemplate").CloneTree();
        cameraPointTemplate.name = $"button_{gameObject.name}";
        root.Add(cameraPointTemplate);
        cameraPoint = cameraPointTemplate.Q<VisualElement>($"button_{gameObject.name}");

        GameObject test = GameObject.CreatePrimitive(PrimitiveType.Cube);
        test.transform.position = transform.position;
        test.transform.rotation = transform.rotation;
        Debug.Log(Camera.main.WorldToScreenPoint(transform.position));

        
    }

    private void LateUpdate()
    {
        Vector3 pointInScreen = Camera.main.WorldToScreenPoint(transform.position);

        cameraPoint.style.left = pointInScreen.x;
        cameraPoint.style.top = Screen.height - pointInScreen.y;
    }
}
