using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    //Menu
    public GameObject startMenu;
    public GameObject deathMenu;
    public GameObject currentMenu;

    private bool start;

    public void OnStartGame()//点击“开始游戏”时执行此方法
    {
        
            
    }

    public void RestartGame()//点击“重新开始”时执行此方法
    {
        
    }

    public void OnQuitGame()//点击“退出游戏”时执行此方法
    {
        Application.Quit();
    }

    public void OnResumeGame()//点击“继续游戏”时执行此方法
    {
        Time.timeScale = 1.0f;
        start = true;
        //inMenu.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        startMenu.SetActive(true);
        deathMenu.SetActive(false);
        currentMenu.SetActive(false);
        start = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!start && (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Mouse0)))
        {
            Time.timeScale = 1.0f;
            start = true;
            startMenu.SetActive(false);
            currentMenu.SetActive(true);
            deathMenu.SetActive(true);
        }
    }
}