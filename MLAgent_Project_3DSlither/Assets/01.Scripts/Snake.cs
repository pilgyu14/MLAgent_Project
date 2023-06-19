using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Extensions.Sensors;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Snake : MonoBehaviour
{
    [SerializeField] 
    private float rotationSpeed;
    [SerializeField]
    private float moveSpeed; 

    private Vector3 targetDir;
    private Quaternion targetRot; 
    
    private Camera cam;
    private Rigidbody rigid; 
    
    private void Awake()
    {
        cam = Camera.main;
        rigid = GetComponent<Rigidbody>(); 
    }

    private void Update()
    {
        Test(); 
        //MoveAndRoateToCursor(); 
        //transform.Translate(transform.forward * (moveSpeed * Time.deltaTime));

        /*
        DampedTransform a = GetComponent<DampedTransform>();
        DampedTransformData b = a.data;
        b.constrainedObject = transform;*/
    }

    private void Test()
    {
        // Get input from the WASD keys
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate the movement and rotation based on input
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized * moveSpeed * Time.deltaTime;

        // Rotate the object based on horizontal input
        Quaternion rotation = Quaternion.Euler(0f, moveHorizontal * rotationSpeed * Time.deltaTime, 0f);
        transform.rotation *= rotation;

        // Move the object in the direction it is facing
        transform.Translate(movement, Space.Self);
    }
    private void MoveAndRoateToCursor()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0)
        {
            targetDir = new Vector3(h, 0, v).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

        }
        
        //targetDir = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        targetRot = Quaternion.AngleAxis(angle, Vector3.forward);

        //transform.rotation = Quaternion.LookRotation(targetDir);

    }
    
    
}