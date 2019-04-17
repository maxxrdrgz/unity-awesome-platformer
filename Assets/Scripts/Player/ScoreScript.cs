using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    private Text coinTextScore;
    private AudioSource audioManager;
    private int scoreCount;


    void Awake()
    {
        audioManager = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        coinTextScore = GameObject.Find("CoinText").GetComponent<Text>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Tags.COIN_TAG)
        {
            collision.gameObject.SetActive(false);
            scoreCount++;
            coinTextScore.text = "x" + scoreCount;
            audioManager.Play();
        }
    }

    public void IncreaseScore(int score)
    {
        scoreCount += score;
        coinTextScore.text = "x" + scoreCount;
        audioManager.Play();
    }
}
