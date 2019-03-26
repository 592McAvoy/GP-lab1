using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : UnitySingleton<GameManager>
{
    public GameObject player;
    public GameObject deathMenu;
    public GameObject currentScore;
    public GameObject tile;
    public Text curScoreText;
    public Text pickUpText;

    private bool isDead;
    private float score;
    private int pickupNum;
    
    
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        isDead = false;
        score = 0.0f;
        pickupNum = -1;
        updateScore();
        addPickUp();
    }

    void Update()
    {
        if (!isDead) {
            //// TODO: Your Implementation:
            //// 1. update score (Hint: you can use running time as the score)
            score += Time.deltaTime;
            //// 2. show score (Hint: show in Canvas/CurrentScore/Text)
            updateScore();
            
        }
        else if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Mouse0)) {
            Restart();
        }
    }

    void updateScore()
    {
        curScoreText.text = ((int)score).ToString();
    }

    public bool IsDead() {
        return isDead;
    }
    public void addPickUp()
    {
        pickupNum += 1;
        pickUpText.text = "Pick Up: " + pickupNum.ToString();
    }

    public void OnDeath(bool collision){
        isDead = true;
        print("GameOver");
        //// TODO: Your Implementation:
        //// 4. record high score (Hint: use PlayerPrefs)
        score += 0.5f * pickupNum;
        float curMax = PlayerPrefs.GetFloat("score");
        if (score > curMax)
        {
            curMax = score;
            PlayerPrefs.SetFloat("score", curMax);
        }
        //// 1. show DeathMenu (Hint: you can use Show() in DeathMenu.cs)
        currentScore.SetActive(false);
        deathMenu.GetComponent<DeathMenu>().Show((int)score, (int)curMax);
        //// 2. stop player
        player.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
        //// 3. hide all tiles (Hint: call function in TileManager.cs)
        tile.GetComponent<TileManager>().hideAll();        
    }

    public void Restart(){
        score = 0.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
