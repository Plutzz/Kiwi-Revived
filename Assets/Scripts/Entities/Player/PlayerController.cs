using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject rotationPoint;
    private Vector3 mousePos;
    
    void Start()
    {
        
    }

    void Update()
    {
        followMouse();
    }




    private void followMouse()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        rotationPoint.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
    