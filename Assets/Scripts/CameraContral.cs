using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContral : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation;
    float yRotation;
    public float speed;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        xRotation = this.transform.rotation.x;
        yRotation = this.transform.rotation.y;

    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

       /* float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        this.transform.position += speed * move * Time.deltaTime;*/

    }
}
