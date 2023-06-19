using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderReceiver : MonoBehaviour
{
    public string[] targetTag;
    public int id;
    public Action OnCollisionEvent = null;

    private Rigidbody rigid;

    public Rigidbody Rigid => rigid; 
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>(); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (var tag in targetTag)
        {
            if (collision.transform.CompareTag(tag))
            {
                OnCollisionEvent?.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (var tag in targetTag)
        {
            if (other.transform.CompareTag(tag))
            {
                OnCollisionEvent?.Invoke();
            }
        }
    }

    public void SetCollisionEvent(Action _evt)
    {
        OnCollisionEvent += _evt; 
    }
}
