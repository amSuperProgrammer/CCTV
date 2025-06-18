using UnityEngine;
using UnityEngine.UIElements;

public class ViewPoint : MonoBehaviour
{
    [Tooltip("Блокирует поворот по горизонтали если поставлена галочка.")]
    public bool lockHorizontalRotation = false;
    [Range(0, 180)] public float horizontalMinAngle = 120f;
    [Range(180, 360)] public float horizontalMaxAngle = 240f;
    [Space(10)]
    [Range(0, 90)] public float verticalMaxAngle = 45f;
    [Range(-90, 0)] public float verticalMinAngle = -45f;


    [Space(10)]
    [Tooltip("Если включен то изменяет поворот точки, сохраняя текущий поворот камеры.")]
    public bool savePointRotate = false;
    public float fromToMoveSpeed = 5;
    public float fromToRotateSpeed = 5;

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
        pointButton.style.opacity = angle < _viewManager.buttonHideAngle ? 1 : 0;
        pointButton.style.display = _viewManager.currentViewPoint != this ? DisplayStyle.Flex : DisplayStyle.None;

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
