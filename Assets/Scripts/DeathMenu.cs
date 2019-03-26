using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//// UI controller for DeathMenu
public class DeathMenu : MonoBehaviour
{
    public Text endScoreText;
    public Text highScoreText;
    public Image bgImage;
    private bool isShowned;

    private float bgTransition;

    void Start()
    {
        //// hide the Death Menu at first
        gameObject.SetActive(false);
        isShowned = false;
        bgTransition = 0.0f;
        endScoreText.text = "";
    }
    void Update()
    {
        if (!isShowned)
        {
            gameObject.SetActive(false);
            return;
        }
            
        
        //// fading effect
        bgTransition += Time.deltaTime;
        bgImage.color = Color.Lerp(new Color(0,0,0,0), Color.black, bgTransition);
    }

    public void Show(float score, float high){
        //// TODO: Your Implementation:
        //// - show Death Menu
        //// - show score information
        gameObject.SetActive(true);
        print("Show death menu");
        
        isShowned = true;
        endScoreText.text = "Your Score: " + score.ToString();
        highScoreText.text = "Highest Score: " + high.ToString();
    }
}
