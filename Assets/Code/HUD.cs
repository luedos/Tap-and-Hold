using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    public GameObject myGameOverMenu;

    public Text ScoreText;
    public Animation ScoreAnimation;
    public string FastAnimName;

    public Text EndScoreText;

    
    public void GameOverMenu(bool OpenOrClose)
    {
        myGameOverMenu.SetActive(OpenOrClose);
        ScoreText.gameObject.SetActive(!OpenOrClose);

        if (OpenOrClose)
        {
            EndScoreText.text = ScoreText.text;
        }

    }
    

    public void UpdateScore(int inScore)
    {
        if (ScoreAnimation != null)
            ScoreAnimation.Play(FastAnimName);

        ScoreText.text = inScore.ToString();
    }
    
}
