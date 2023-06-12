using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class SnakeAgent : Agent
{
    [SerializeField] 
    private float rotationSpeed;
    [SerializeField]
    private float moveSpeed;

    [SerializeField] private int exp;
    [SerializeField] private int level = 1;
    [SerializeField] private int expMax = 5; 
    private SnakeUpgradeComp upgradeComp; 
    
    private Vector3 targetDir;
    private Quaternion targetRot; 
    
    private Rigidbody rigid; 
    
    public override void Initialize()
    {
        rigid = GetComponent<Rigidbody>();
        upgradeComp = GetComponent<SnakeUpgradeComp>(); 
    }

    // 한판이 시작될때? 호출
    public override void OnEpisodeBegin()
    {
    }

    // 관측 설정 
    public override void CollectObservations(VectorSensor sensor)
    {
        
    }

    // 행동 설정 
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveHorizontal = 0f;
        float moveVertical = 0f; 
        if (actions.DiscreteActions[0] == 1)
        {
            moveHorizontal = -1; 
        }
        else if (actions.DiscreteActions[0] == 2)
        {
            moveHorizontal = 1; 
        }
        
        if (actions.DiscreteActions[1] == 1)
        {
            moveVertical = 1; 
        }

        // Calculate the movement and rotation based on input
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized * moveSpeed * Time.deltaTime;

        // Rotate the object based on horizontal input
        Quaternion rotation = Quaternion.Euler(0f, moveHorizontal * rotationSpeed * Time.deltaTime, 0f);
        transform.rotation *= rotation;

        // Move the object in the direction it is facing
        transform.Translate(movement, Space.Self);

    } 

    // 사용자가 테스트하기 위함 
    public override void Heuristic(in ActionBuffers actionOut)
    {
        var action = actionOut.DiscreteActions; 
        actionOut.Clear();
        int h = 0; //(int) Input.GetAxisRaw("Horizontal");
        int v = 0; //(int) Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.A))
        {
            h = 1; 
        }
        if (Input.GetKey(KeyCode.D))
        {
            h = 2; 
        }
        if (Input.GetKey(KeyCode.W))
        {
            v = 1; 
        }


        action[0] = h;
        action[1] = v; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            Food food = other.GetComponent<Food>();
            AddExp(food.GetFood());
            AddReward(0.1f);
            food.End();
        }
    }

    private void AddExp(int _exp)
    {
        exp += _exp;
        if (exp > expMax * level)
        {
            exp = 0; 
            upgradeComp.LevelUp();
        }
    }

    private void MoveAuto()
    {
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime);
    }
}
