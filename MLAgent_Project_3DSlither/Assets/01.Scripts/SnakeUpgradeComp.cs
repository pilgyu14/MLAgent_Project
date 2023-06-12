using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SnakeUpgradeComp : MonoBehaviour
{
    [SerializeField,Header("시작시 개수")] private int initCount = 3; 
    
    [SerializeField] private GameObject tail;
    [SerializeField] private DampedTransform damp;

    [SerializeField] private Transform tailRoot; 
    [SerializeField] private Transform tailParent; 
    [SerializeField] private Transform dampParent;

    private List<GameObject> tailList = new List<GameObject>();

    private int index = 0;

    private void Start()
    {
        //InitTails();
    }

    private void InitTails()
    {
        for(int i =0;i < tailList.Count; i++)
        {
            Destroy(tailList[i]);
        }
        tailList.Clear();
        
        tailList.Add(tailRoot.gameObject);

        for (int i = 0; i < initCount; i++)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        Transform _newTail = Instantiate(tail, tailList[index].transform).transform;
        //tailParent = _newTail; 
        tailList.Add(_newTail.gameObject);
        
        
        DampedTransform _damp = Instantiate(damp, dampParent);
        DampedTransformData _data = _damp.data;
        _data.constrainedObject = _newTail;
        _data.sourceObject = tailList[index].transform;
        
        ++index; 
        
        _damp.data = _data;
        _damp.data.dampPosition = Mathf.Clamp01(_data.dampPosition);
        _damp.data.dampRotation = Mathf.Clamp01(_data.dampRotation);
        _damp.weight = Mathf.Clamp01(_damp.weight);
    }
}