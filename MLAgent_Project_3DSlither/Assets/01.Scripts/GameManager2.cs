
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager2 : MonoBehaviour
{
    public static GameManager2 Instance;
    [SerializeField] private FoodSpawner foodSpawner;
    public InGameUI gameUI;
    public SnakeAgent agentPrefab;
    public PlayerSnake playerPrefab;

    
    public Transform stage; 
    public PlayerSnake player;
    public List<SnakeAgent> enemyList = new List<SnakeAgent>();
    public int enemyCount = 5; 
    
    private void Awake()
    {
        Instance = this; 
    }

    private void Start()
    {
        StartGame(); 
    }

    public void Reset()
    {
        
    }

    private void ResetEnemey()
    {
    
        
        
    }

    public void EndGame()
    {
        Destroy(player);
        gameUI.gameObject.SetActive(true);

    }
    public void StartGame()
    {
        foodSpawner.ResetAll();
        for (int i = 0; i < enemyList.Count; i++)
        {
            Destroy(enemyList[i].gameObject);
        }
        enemyList.Clear();
        
        player = Instantiate(playerPrefab, stage);
        player.transform.localPosition = foodSpawner.ReturnRandomPos();
        player.OnDeadEvent += EndGame; 
        // 적 생성 
        for (int i = 0; i < enemyCount; i++)
        {
            SnakeAgent enemy; 
            enemy = Instantiate(agentPrefab, stage);
            enemy.transform.localPosition = foodSpawner.ReturnRandomPos();
            enemyList.Add(enemy);
        }
    }
}
