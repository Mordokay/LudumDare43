using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControler : MonoBehaviour {

    public float cameraPanSpeed;
    public float zoomSensitivity;

    void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += Vector3.forward * Time.deltaTime * cameraPanSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += -Vector3.right * Time.deltaTime * cameraPanSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += -Vector3.forward * Time.deltaTime * cameraPanSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += Vector3.right * Time.deltaTime * cameraPanSpeed;
        }

        if(!EventSystem.current.IsPointerOverGameObject() && Input.GetAxis("Mouse ScrollWheel") != 0 && Camera.main.transform.position.y >= 1.5f && Camera.main.transform.position.y <= 6.0f)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0.0f && Camera.main.transform.position.y < 6.0f ||
                Input.GetAxis("Mouse ScrollWheel") > 0.0f && Camera.main.transform.position.y > 1.5f)
            {
                this.transform.position += this.transform.forward * (Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity);
            }
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
                Mathf.Clamp(Camera.main.transform.position.y, 1.5f, 6.0f), Camera.main.transform.position.z);
        }
    }
}
