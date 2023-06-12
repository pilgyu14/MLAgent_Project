using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderReceiver : MonoBehaviour
{
    public string[] targetTag;
    public int id;
    public Action OnCollisionEvent; 
    
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
}
