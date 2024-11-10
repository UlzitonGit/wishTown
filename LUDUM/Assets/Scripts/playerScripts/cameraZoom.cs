using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 10f;
    public float zoomAmount = 5f;
    public float smoothSpeed = 5f;

    private float defaultFOV;
    private float targetFOV;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam != null)
        {
            defaultFOV = cam.fieldOfView;
            targetFOV = defaultFOV;
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            targetFOV = defaultFOV - zoomAmount;
        }
        else
        {
            targetFOV = defaultFOV;
        }

        if (cam != null)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * smoothSpeed);
        }
    }
}