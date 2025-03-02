using TMPro;
using UnityEditor;
using UnityEngine;
using System;
using System.Collections;

public class GameController : MonoBehaviour
{
    public int number;
    public TMP_Text numberText, hpText, leaderText;
    public bool GameEnded;
    public GameObject EndPanel, heart, numberObj;
    int hp;
    GameObject thisCar;
    void Start()
    {
        number = 2;
        numberText.text = number.ToString();
        GameEnded = false;
        hp = 100;
        hpText.text = hp.ToString();
        LoadHighestScore();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BotCarController>().number > number)
        {
            thisCar = other.gameObject;
            Crash();
        }
        else if (other.gameObject.GetComponent<BotCarController>().number < number)
        {
            //HP System
            hp -= 50 - 10 * ((int)(Mathf.Log(gameObject.GetComponent<GameController>().number) / Mathf.Log(2)) - (int)(Mathf.Log(other.GetComponent<BotCarController>().number) / Mathf.Log(2)) - 1);
            thisCar = other.gameObject;
            if (hp <= 0)
            {
                Crash();
            }
            else
            {
                AudioManager.Instance.PlaySFX("HealthDown");
                StartCoroutine(AnimateHeart());
            }
            hpText.text = hp.ToString();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<BotCarController>().number == number)
        {
            StartCoroutine(AnimateNumber());
            
            AudioManager.Instance.PlaySFX("Score");
        }
    }
    void Crash()
    {
        thisCar.GetComponent<BotCarController>().moveSpeed = 0;
        gameObject.GetComponent<CarController>().moveSpeed = 0;
        GameEnded = true;
        AudioManager.Instance.MusicSource.mute = true;
        AudioManager.Instance.PlaySFX("Crash");
        hp = 0;
        hpText.text = hp.ToString();
        EndPanel.SetActive(true);
        UpdateHighScore();
    }
    //Heart and scoretext anims
    IEnumerator AnimateNumber()
    {
        numberObj.GetComponent<Animator>().Play("Selected");
        yield return new WaitForSeconds(.2f);
        number *= 2;
        numberText.text = number.ToString();
        yield return new WaitForSeconds(.3f);
        numberObj.GetComponent<Animator>().Play("Normal");
    }
    IEnumerator AnimateHeart()
    {
        heart.GetComponent<Animator>().Play("Selected");
        yield return new WaitForSeconds(.5f);
        heart.GetComponent<Animator>().Play("Normal");
    }
    //Keeping highscore
    private void LoadHighestScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        leaderText.text = highScore.ToString();
    }

    private void UpdateHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (number > highScore)
        {
            PlayerPrefs.SetInt("HighScore", number);
        }
        PlayerPrefs.Save();
        LoadHighestScore();
    }
}
