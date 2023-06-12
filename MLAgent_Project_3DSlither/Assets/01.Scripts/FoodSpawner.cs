using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private Food foodPrefab;
    [SerializeField] private float delay;
    [SerializeField] private List<Food> foodList = new List<Food>();
    [SerializeField] private bool isSpawning = true;
    [SerializeField] private int maxCount = 50; 

    [SerializeField] private Vector2 mapSize;

    public Vector2 MapSize => mapSize; 
    private void Start()
    {
        StartCoroutine(StartCreateFood()); 
    }

    public IEnumerator StartCreateFood()
    {
        WaitForSeconds w = new WaitForSeconds(delay);

        while (isSpawning == true)
        {
            CreateFood();
            yield return w;
        }
    }

    public void CreateFood()
    {
        if (foodList.Count >= maxCount) return; 
        Food food = Instantiate(foodPrefab, transform);
        food.OnEndEvent = () => foodList.Remove(food); 
        int point = Random.Range(1, 5);
        Color color = Random.ColorHSV(); 
        food.SetRandomData(
            point, point /2, color);

        float sizeX = Random.Range(-mapSize.x, mapSize.x);
        float sizeY= Random.Range(-mapSize.y, mapSize.y);
        food.transform.localPosition = new Vector3(sizeX, 0, sizeY);
        
        foodList.Add(food);
    }

    public void ResetAll()
    {
        foreach (var food in foodList)
        {
            Destroy(food);
        }
    }
}
