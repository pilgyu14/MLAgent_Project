using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Food : MonoBehaviour
{
    private Material mat; 
    public int point = 1;

    public Action OnEndEvent = null; 
    
    private Material Mat
    {
        get
        {
            if(mat == null) 
                mat = GetComponent<MeshRenderer>().material;
            return mat; 
        }
    }
    private void Awake()
    {
        mat = GetComponent<MeshRenderer>().material; 
    }

    public int GetFood()
    {
        return point; 
    }

    public void SetRandomData(int _point, float _scale, Color _color)
    {
        this.point = _point;
        transform.localScale = Vector3.one * _scale;
        mat.color = _color; 
    }

    public void End()
    {
        OnEndEvent?.Invoke();
        Destroy(gameObject);
    }
}
