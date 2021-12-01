using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEnterHang : MonoBehaviour
{
    public Text level;
    public int GoToLevel;
    void Awake() 
    {
        level.text = GoToLevel.ToString();
    }
    void OnTriggerEnter(Collider trig) 
    {
        Debug.Log("Triggered");
        SceneManager.LoadScene(GoToLevel);
    }
}
