using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;
    public Camera activeCamera;
    public Transform lookAt;
    public Transform goTo;

    void Update()
    {
        if (lookAt)
        {
            Vector3 direction = lookAt.position - activeCamera.transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            activeCamera.transform.rotation = Quaternion.Slerp(
                activeCamera.transform.rotation,
                targetRotation,
                rotateSpeed * Time.deltaTime
            );
        }
        if (goTo)
        {
            Vector3 targetPosition = goTo.transform.position;

            activeCamera.transform.position = Vector3.Lerp(
                activeCamera.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
        }
    }
}
