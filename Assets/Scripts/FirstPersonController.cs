using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    private float xRotation = 0f;
    private Camera cam;
    private AudioSource footstepsAudio;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        footstepsAudio = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (x != 0 || z != 0)
        {
            if (!footstepsAudio.isPlaying)
            {
                footstepsAudio.Play();
            }
        }
        else
        {
            footstepsAudio.Stop();
        }

        Vector3 move = transform.right * x + transform.forward * z;
        transform.position += move * moveSpeed * Time.deltaTime;

        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
