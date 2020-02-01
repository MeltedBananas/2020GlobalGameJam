using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public float Distance = 5.0f;
    public float YawSpeed = 1.0f;
    public float PitchSpeed = -1.0f;
    public Vector3 Offset = Vector3.zero;
    public GameObject Subject;

    public float Yaw = 0.0f;
    public float Pitch = 0.0f;
    Vector3 LastMousePositon;

    void Start()
    {
        RefresPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            LastMousePositon = Input.mousePosition;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if(Input.GetMouseButton(1))
        {
            Vector3 mouseDelta = Input.mousePosition - LastMousePositon;
            Yaw += Input.GetAxis("MouseX") * YawSpeed * Time.deltaTime;
            Pitch = Mathf.Clamp(Pitch + Input.GetAxis("MouseY") * YawSpeed * Time.deltaTime, -89.9f, 89.9f);

            RefresPosition();

            LastMousePositon = Input.mousePosition;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void RefresPosition()
    {
        Vector3 horizontal = new Vector3(Mathf.Cos(Mathf.Deg2Rad * Yaw) * Distance, 0.0f, Mathf.Sin(Mathf.Deg2Rad * Yaw) * Distance);
        Quaternion pitchQuat = Quaternion.AngleAxis(Pitch, Vector3.Cross(horizontal, Vector3.up));
        gameObject.transform.position = Subject.transform.position + pitchQuat * horizontal;
        gameObject.transform.LookAt(Subject.transform.position);
        gameObject.transform.position += gameObject.transform.rotation * Offset;
    }
}
