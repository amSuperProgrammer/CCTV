using UnityEngine;
using UnityEngine.UIElements;

public class ViewPoint : MonoBehaviour
{
    [Tooltip("If true, the camera will not rotate horizontally when the mouse is moved left or right.")]
    public bool lockHorizontalRotation = false;
    public float horizontalLockAngle = 45f;
    [Space(20)]
    [Tooltip("If true, the camera will not rotate vertically when the mouse is moved up or down.")]
    public bool lockVerticalRotation = false;
    public float verticalLockAngle = 45f;

    ViewManager _viewManager;

    VisualElement root;
    VisualElement cameraPoint;
    Button pointButton;
    private void Start()
    {
        _viewManager = Camera.main.GetComponent<ViewManager>();

        root = GameObject.Find("UIDocument").GetComponent<UIDocument>().rootVisualElement;
        VisualElement cameraPointTemplate = Resources.Load<VisualTreeAsset>("CameraPointTemplate").CloneTree();
        cameraPointTemplate.name = $"point_{gameObject.name}";
        root.Add(cameraPointTemplate);
        cameraPoint = cameraPointTemplate.Q<VisualElement>($"point_{gameObject.name}");

        pointButton = cameraPoint.Q<Button>();
        pointButton.clicked += () => MakeThisViewPointAsActive();
    }

    private void LateUpdate()
    {
        float angle = Vector3.Angle(Camera.main.transform.forward, transform.position - Camera.main.transform.position);
        pointButton.style.opacity = angle < _viewManager.buttonHideAngle && _viewManager.currentViewPoint != this ? 1 : 0;

        Vector2 pointInScreen = (Vector2)Camera.main.WorldToScreenPoint(transform.position);
        cameraPoint.style.left = pointInScreen.x;
        cameraPoint.style.top = Screen.height - pointInScreen.y;
    }

    public void MakeThisViewPointAsActive()
    {
         _viewManager.currentViewPoint = this; 
         _viewManager.ReBuildViewPointOrder();
    }

    public void UpUIHierarchy() => cameraPoint.BringToFront();
}
