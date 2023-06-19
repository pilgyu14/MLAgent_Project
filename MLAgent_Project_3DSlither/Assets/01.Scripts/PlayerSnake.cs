using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSnake : MonoBehaviour
{
    [SerializeField] 
    private float rotationSpeed;
    [SerializeField]
    private float moveSpeed;
    
    [SerializeField] private int exp;
    [SerializeField] public int level = 1;
    [SerializeField] private int expMax = 5;

    [SerializeField] private Transform tailParent;

    private SnakeUpgradeComp_V2 upgradeComp_V2; 
    
    private Vector3 targetDir;
    private Quaternion targetRot; 
    
    private Rigidbody rigid;
    public Action OnDeadEvent = null; 

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        upgradeComp_V2 = GetComponent<SnakeUpgradeComp_V2>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveAuto(); 
        Rotate();
    }

    private void Rotate()
    {
        int h = 0; //(int) Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.A))
        {
            h = -1; 
        }
        if (Input.GetKey(KeyCode.D))
        {
            h = 1; 
        }
        
        transform.Rotate(Vector3.up * h * rotationSpeed * Time.deltaTime);
        upgradeComp_V2.UpdateTailPos();


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            Food food = other.GetComponent<Food>();
            AddExp(food.GetFood());
            food.End();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Tail"))
        {
            if(upgradeComp_V2.CheckMyTail(collision.gameObject) == true) return; 
            // 리셋 
            GameManager2.Instance.EndGame();
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
            _tail.transform.SetParent(tailParent,false);
        }
    }
    
    private void MoveAuto()
    {
        rigid.MovePosition(rigid.position + transform.forward * moveSpeed * Time.deltaTime);
        //   transform.Translate(transform.forward * moveSpeed * Time.deltaTime);
    }

    private void ResetAll()
    {
        transform.rotation = Quaternion.identity;
        exp = 0;
        level = 0;
        upgradeComp_V2.ResetAll();
        //      foodSpawner.ResetAll();
    }
}
