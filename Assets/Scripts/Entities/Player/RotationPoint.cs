using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPoint : Singleton<RotationPoint>
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject Graphics;
    [SerializeField] private int deadzone = 20;
    [SerializeField] private GameObject neck;
    [SerializeField] private float neckRotationOffset;
    private Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        followMouse();
    }

    // Rotates the rotation point to follow the mouse
    private void followMouse()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 _rotation = mousePos - transform.position;
        float _rotZ = Mathf.Atan2(_rotation.y, _rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, _rotZ);
        

        /*
        if (transform.rotation.eulerAngles.z < 90 - deadzone || transform.rotation.eulerAngles.z > 270 - deadzone)
        {
            Debug.Log("Flip forwards");
            Graphics.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(transform.rotation.eulerAngles.z > 90 - deadzone && transform.rotation.eulerAngles.z < 270 - deadzone)
        {
            Debug.Log("Flip backwards");
            Graphics.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        */

        // Debug.Log(mousePos);
        float _mouseDistance = mousePos.x - Graphics.transform.position.x;

        if( _mouseDistance > 0 + deadzone)
        {
            // Debug.Log("Flip forwards");
            Graphics.transform.rotation = Quaternion.Euler(0, 0, 0);
            neck.transform.rotation = Quaternion.Euler(0, 0, _rotZ + neckRotationOffset);
        }
        else if( _mouseDistance < 0 - deadzone)
        {
            // Debug.Log("Flip backwards");
            Graphics.transform.rotation = Quaternion.Euler(0, -180, 0);
            neck.transform.rotation = Quaternion.Euler(0, -180, (_rotZ * -1) + neckRotationOffset + 180);
        }

    }
}
