using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public Transform target;

    public float distance = 10.0f;
    public float height = 5.0f;

    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;

    Vector2 _lastPosition = Vector2.zero;
    Camera cam;

    private void Start()
    {
        cam = this.GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (!target) return;

        if (Input.GetMouseButtonDown(1)) _lastPosition = Input.mousePosition;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            cam.fieldOfView = (cam.fieldOfView < 10) ? 10 : --cam.fieldOfView;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            cam.fieldOfView = (cam.fieldOfView > 60) ? 60 : ++cam.fieldOfView;


        // Calculate the current rotation angles.
        float wantedRotationAngle;
        float wantedHeight;

        if (Input.GetMouseButton(1)) {
            wantedRotationAngle = target.eulerAngles.y + (_lastPosition.x - Input.mousePosition.x);
            wantedHeight = target.position.y + height - (_lastPosition.y - Input.mousePosition.y);
            wantedHeight = (wantedHeight > 10) ? 10 : (wantedHeight < 1 + ((60 - cam.fieldOfView) / 10)) ? 1 + ((60 - cam.fieldOfView) / 10) : wantedHeight;
        }
        else
        {
            wantedRotationAngle = target.eulerAngles.y;
            wantedHeight = target.position.y + height;
        }

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight - ((60 - cam.fieldOfView) / 10), heightDamping * Time.deltaTime);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        // Set the height of the camera
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // Always look at the target
        transform.LookAt(target);
    }
}
