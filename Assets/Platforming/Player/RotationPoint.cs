using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPoint : Singleton<RotationPoint>
{
    [SerializeField] private GameObject Graphics;
    [SerializeField] private int deadzone = 20;
    [SerializeField] private GameObject neck;
    [SerializeField] private float neckRotationOffset;
    [SerializeField] private Camera playerCamera;
    private Vector3 mousePos;

    public bool FacingForward { get; private set; }

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
        mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 _rotation = mousePos - transform.position;
        Debug.Log(_rotation);
        float _rotZ = Mathf.Atan2(_rotation.y, _rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, _rotZ);
        
        float _mouseDistance = mousePos.x - Graphics.transform.position.x;


        // Flips player based on mouse direction from player
        if( _mouseDistance > 0 + deadzone) //Flip Forward
        {
            FacingForward = true;
            Graphics.transform.rotation = Quaternion.Euler(0, 0, 0);
            neck.transform.rotation = Quaternion.Euler(0, 0, _rotZ + neckRotationOffset);
        }
        else if( _mouseDistance < 0 - deadzone) // Flip backwards
        {
            FacingForward = false;
            Graphics.transform.rotation = Quaternion.Euler(0, -180, 0);
            neck.transform.rotation = Quaternion.Euler(0, -180, (_rotZ * -1) + neckRotationOffset + 180);
        }

    }


}
