using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Transform sun;
    public Transform ship;

    public float minSize = 5f;
    public float sensitivity = 0.5f;
    public float smoothing = 2f;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (ship == null || sun == null) return;

        float distance = Vector2.Distance(sun.position, ship.position);
        float targetSize = minSize + (distance * sensitivity);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * smoothing);

        transform.position = Vector3.Lerp(transform.position, new Vector3(ship.position.x / 4, ship.position.y / 4, -10), Time.deltaTime);
    }
}
