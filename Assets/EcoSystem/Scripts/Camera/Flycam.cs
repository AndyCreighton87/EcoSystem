using UnityEngine;

public class Flycam : MonoBehaviour {
    public float movementSpeed = 10f;
    public float lookSpeed = 2f;
    public float scrollSpeed = 10f;

    private float yaw;
    private float pitch;

    void Update() {
        // Mouse look
        yaw += lookSpeed * Input.GetAxis("Mouse X");
        pitch -= lookSpeed * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        // WASD movement
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(move * movementSpeed * Time.deltaTime, Space.Self);

        // Scroll wheel zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(Vector3.forward * scroll * scrollSpeed, Space.Self);
    }
}