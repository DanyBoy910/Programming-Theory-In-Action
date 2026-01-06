using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Header("Posición")]
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 5, -10);

    [Header("Rotación")]
    public float rotationSmoothSpeed = 5f; // Velocidad de rotación

    private void Start()
    {
        
    }

    public void SetOffset_Camera()
    {
        offset = target.GetComponent<Aircraft>().IdealCameraOffset;
    }
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);


        Vector3 directionToTarget = target.position - transform.position;

        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, target.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeed * Time.deltaTime);
        }
    }
}