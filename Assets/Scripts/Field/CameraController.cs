using System;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float panSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float minScale;
    [SerializeField] private float maxScale;
    private Camera camera;
    private Vector2 size;
    private float currentScale;
    private float originalScale;

    void Start()
    {
        camera = GetComponent<Camera>();
        originalScale = camera.orthographicSize;
        currentScale = minScale;
        camera.orthographicSize = originalScale / currentScale;
    }

    void Update()
    {
        bool panned = false;
        bool zoomed = false;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            camera.transform.position += Vector3.up * panSpeed;
            panned = true;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            camera.transform.position += Vector3.left * panSpeed;
            panned = true;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            camera.transform.position += Vector3.down * panSpeed;
            panned = true;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            camera.transform.position += Vector3.right * panSpeed;
            panned = true;
        }

        if (Input.mouseScrollDelta.y > 0 || Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Equals))
        {
            currentScale += zoomSpeed;
            zoomed = true;
        }

        if (Input.mouseScrollDelta.y < 0 || Input.GetKey(KeyCode.KeypadMinus) || Input.GetKey(KeyCode.Minus))
        {
            currentScale -= zoomSpeed;
            zoomed = true;
        }

        if (zoomed)
        {
            currentScale = Mathf.Clamp(currentScale, minScale, maxScale);
            camera.orthographicSize = originalScale / currentScale;
        }

        if (panned || zoomed)
        {
            Reset();
        }
    }

    public void SetSize(float width, float height)
    {
        size.Set(width, height);
        Reset();
    }

    private void Reset()
    {
        Vector3 bottomLeft = camera.ViewportToWorldPoint(Vector3.zero);
        Vector3 topRight = camera.ViewportToWorldPoint(Vector3.one);
        
        if (bottomLeft.x < 0)
        {
            camera.transform.position += Vector3.right * -bottomLeft.x;
        }

        if (topRight.x > size.x)
        {
            camera.transform.position += Vector3.left * (topRight.x - size.x);
        }

        if (bottomLeft.y < 0)
        {
            camera.transform.position += Vector3.up * -bottomLeft.y;
        }

        if (topRight.y > size.y)
        {
            camera.transform.position += Vector3.down * (topRight.y - size.y);
        }
    }
}
