using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    private Text coinTextScore;
    private int scoreCount;


    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        coinTextScore = GameObject.Find("CoinText").GetComponent<Text>();
    }

    /** 
        Detects collision with a coin. Disables the coin, increases the score
        count and plays the coin sound.

        @param {Collider2D} The Collision2D data associated with this collision.
    */
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Tags.COIN_TAG)
        {
            collision.gameObject.SetActive(false);
            scoreCount++;
            coinTextScore.text = "x" + scoreCount;
            SoundManager.instance.PlayCoinSound();
        }
    }

    /** 
        Increases the scorecount, plays the coin sound and updates the coin
        text
    */
    public void IncreaseScore(int score)
    {
        scoreCount += score;
        coinTextScore.text = "x" + scoreCount;
        SoundManager.instance.PlayCoinSound();
    }
}
