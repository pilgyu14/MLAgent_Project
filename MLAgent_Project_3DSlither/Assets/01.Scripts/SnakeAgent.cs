    using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SnakeAgent : Agent
{
    
    [SerializeField] 
    private float rotationSpeed;
    [SerializeField]
    private float moveSpeed;

    [SerializeField] private int exp;
    [SerializeField] public int level = 1;
    [SerializeField] private int expMax = 5;

    [SerializeField] private FoodSpawner foodSpawner;
    [SerializeField] private Transform tailParent;

    public Action OnDeadEvent = null; 
    //private 
    private SnakeUpgradeComp upgradeComp;
    private SnakeUpgradeComp_V2 upgradeComp_V2; 
    
    private Vector3 targetDir;
    private Quaternion targetRot; 
    
    private Rigidbody rigid; 
    
    
    public override void Initialize()
    {
        rigid = GetComponent<Rigidbody>();
        upgradeComp = GetComponent<SnakeUpgradeComp>();
        upgradeComp_V2 = GetComponent<SnakeUpgradeComp_V2>();
        foodSpawner = GetComponentInParent<FoodSpawner>(); 
    }
    

    // 한판이 시작될때? 호출
    public override void OnEpisodeBegin()
    {
        ResetAll();
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
        
        transform.Rotate(Vector3.up * moveHorizontal * rotationSpeed * Time.deltaTime);
        MoveAuto();
        
        upgradeComp_V2.UpdateTailPos();
        AddReward(-1/MaxStep);

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


        action[0] = h;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            Food food = other.GetComponent<Food>();
            AddExp(food.GetFood());
            AddReward(2f);
            food.End();
        }
        if (other.transform.CompareTag("Tail"))
        {
            if(upgradeComp_V2.CheckMyTail(other.gameObject) == true) return; 
            OnDeadEvent?.Invoke();
            AddReward(-300f);
            EndEpisode();
            // 리셋 
            ResetAll(); 
        }

  
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Tail"))
        {
            if(upgradeComp_V2.CheckMyTail(collision.gameObject) == true) return; 
            AddReward(-50f);
            EndEpisode();
            // 리셋 
            ResetAll(); 
        }
        if (collision.transform.CompareTag("Wall"))
        {
            AddReward(-100f);
            //EndEpisode();
        }
        
    }
    
    private void OnCollisionStay(Collision collision)
    {

        if (collision.transform.CompareTag("Wall"))
        {
            AddReward(-1f);
        }

    }

    private void AddExp(int _exp)
    {
        exp += _exp;
        Mathf.Log(exp); 
        
        if (exp > expMax * level)
        {
            exp = 0; 
            //upgradeComp.LevelUp();
            var _tail = upgradeComp_V2.GrowSnake();
            _tail.SetCollisionEvent(() => AddReward(30f)); 
            _tail.transform.SetParent(tailParent,false);
            AddReward(0.5f);
        }
    }

    private void MoveAuto()
    {
        rigid.MovePosition(rigid.position + transform.forward * moveSpeed * Time.deltaTime);
        //rigid.velocity = transform.forward * moveSpeed ;
     //   transform.Translate(transform.forward * moveSpeed * Time.deltaTime);
    }

    private void ResetAll()
    {
        transform.rotation = Quaternion.identity;
        transform.localPosition = GetRandomPos();
        exp = 0;
        level = 0;
        upgradeComp_V2.ResetAll();
  //      foodSpawner.ResetAll();
    }

    private Vector3 GetRandomPos()
    {
        float x = Random.Range(-foodSpawner.MapSize.x/2, foodSpawner.MapSize.x/2);
        float y = Random.Range(-foodSpawner.MapSize.y/2, foodSpawner.MapSize.y/2);
        return new Vector3(x,5,y);
    }
    
}
