using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

//This script controlls the timer
public class Timer : Singleton<Timer>
{
    [Header("Time Varibles (Seconds)")]
    [Tooltip("How long the timer lasts in the resource run")]
    private float TimeElapsed;
    public bool TimerOn = false;

    public TextMeshProUGUI TimerTxt;
    void Start()
    {
        TimerOn = true;
    }

    void Update()
    {
        // Checks if time is up and stops the timer if it has
        if (TimerOn)
        {
            TimeElapsed += Time.deltaTime;
            updateTimer(TimeElapsed);
            
        }
    }

    // Updates the timer text
    void updateTimer(float currentTime)
    {
        //currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void CallLoadStartScene()
    {
        StartCoroutine(LoadStartScene());
    }

    public IEnumerator LoadStartScene()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }

}