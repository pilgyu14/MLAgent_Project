using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SnakeUpgradeComp_V2 : MonoBehaviour
{
    [SerializeField] private float tailSpeed; 
    [SerializeField] private int Gap = 10; 
    [SerializeField] private ColliderReceiver tail;
    [SerializeField] private List<ColliderReceiver> tailList = new List<ColliderReceiver>(); 
    [SerializeField] private List<Vector3> positionHistoryList = new List<Vector3>();

    public bool CheckMyTail(GameObject _obj)
    {
        for (int i = 0; i < tailList.Count; i++)
        {
            if (tailList[i].gameObject == _obj)
                return true; 
        }

        return false; 
    }
    private void Start()
    {/*
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();*/
    }

    public void ResetAll()
    {
        positionHistoryList.Clear();
        for (int i = 0; i < tailList.Count; i++)
        {
            Destroy(tailList[i].gameObject);;
        }
        tailList.Clear();
    }
    public void UpdateTailPos()
    {
        positionHistoryList.Insert(0,transform.position);
        if (positionHistoryList.Count > 100)
        {
            for (int i = positionHistoryList.Count; i > 1000; i--)
            {
                positionHistoryList.RemoveAt(i - 1);
            }
        }
        int index = 1; 
        foreach (var tail in tailList)
        {
            Vector3 point = positionHistoryList[Mathf.Min(index * Gap,positionHistoryList.Count-1)];
            var position = tail.transform.position;
            Vector3 moveDir = point - position; 
            position += moveDir * tailSpeed * Time.deltaTime ;
           // tail.Rigid.MovePosition(position);
            tail.Rigid.transform.position = position; 
            //tail.transform.position = position;
            tail.transform.LookAt(point);
            index++; 

        }
    }

    public ColliderReceiver GrowSnake()
    {
        ColliderReceiver body = Instantiate(tail,transform);
        tailList.Add(body);
        return body; 
    }
}
