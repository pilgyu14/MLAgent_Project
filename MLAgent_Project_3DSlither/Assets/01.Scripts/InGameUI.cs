using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EndUI
{
    public Text levelText;
    public Text rankText; 
}
public class InGameUI : MonoBehaviour
{
    public Text levelText;
    public Button restartButton; 
    
    public List<EndUI> endUIList = new List<EndUI>(); 
    
    
    public void UpdateUI(int level)
    {
        levelText.text= "LV_" + level.ToString(); 
    }
}
