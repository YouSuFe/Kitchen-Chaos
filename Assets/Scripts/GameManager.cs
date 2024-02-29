using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;

    private enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver
    }

    private State state;
    private float waitingForStartTimer = 1f;
    private float countDownForStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 10f;

    private void Awake()
    {
        state = State.WaitingToStart;
        Instance = this;
    }

    private void Update()
    {
        switch(state)
        {
            case State.WaitingToStart:
                waitingForStartTimer -= Time.deltaTime;
                if(waitingForStartTimer < 0 )
                {
                    state = State.CountDownToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.CountDownToStart:
                countDownForStartTimer -= Time.deltaTime;
                if (countDownForStartTimer < 0)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GameOver:
                break;

        }
        Debug.Log(state.ToString());
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state == State.CountDownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countDownForStartTimer;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }
}
