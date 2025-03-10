﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    [Header("Assignables")]
    public GameObject score;
    public Transform container;
    public Animation anim;
    public TextMeshProUGUI counter;
    List<GameObject> scores = new List<GameObject>();
    List<PlayerScore> playerScores = new List<PlayerScore>();
    List<Vector3> startPositions = new List<Vector3>();

    public class PlayerScore
    {
        public int id;
        public int score;
        public string username;
        public Material material;
        public GameObject panel;
        public ScorePanel scorePanel;

        public PlayerScore()
        {
            id = 0;
            score = 0;
            material = null;
            panel = null;
            scorePanel = null;
        }
    }

    private void Start()
    {
        //Initialize(debugList);
        //PassScores(debugScores);
        //StartCoroutine(PlayAnim());
    }

    public void Initialize(List<Client> clients)
    {
        foreach (Transform child in container)
        {
            startPositions.Add(child.position);
        }
        for (int i = 0; i < clients.Count; i++)
        {
            GameObject instance = Instantiate(score, container);
            instance.transform.position = startPositions[i];
            scores.Add(instance);
            PlayerScore pscr = new PlayerScore();
            pscr.id = clients[i].id;
            pscr.material = clients[i].material;
            pscr.username = clients[i].username;
            pscr.panel = instance;
            pscr.scorePanel = instance.GetComponent<ScorePanel>();
            playerScores.Add(pscr);
            pscr.scorePanel.SetColor(clients[i].color);
        }
    }

    public void PassScores(List<int> _scores)
    {
        for (int i = 0; i < _scores.Count; i++)
        {
            foreach (PlayerScore pscr in playerScores)
            {
                if (pscr.id - 1 == i)
                {
                    pscr.score = _scores[i];
                    Debug.Log("passing");
                    break;
                }
            }
        }
    }

    public void PlayAnim()
    {
        StartCoroutine(IE_PlayAnim());
    }

    public void SetDisplay(int rounds)
    {
        counter.text = $"rounds remaining: {rounds}";
    }

    public void SetScores()
    {
        for (int i = 0; i < playerScores.Count; i++)
        {
            playerScores[i].scorePanel.SetScore(playerScores[i].score);
        }
    }

    public IEnumerator IE_PlayAnim()
    {
        Debug.Log("playing..");
        int max = playerScores[0].score;
        for (int i = 0; i < playerScores.Count; i++)
        {   
            if (i == 0)
            {
                playerScores[0].scorePanel.SetFill(1f);
            }
            else
            {
                playerScores[i].scorePanel.SetFill((float)playerScores[i].score / (float)max);
            }
        }
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < playerScores.Count; i++)
        {
            if (i == 0)
            {
                playerScores[i].scorePanel.MoveTo(startPositions[0]);
            }
            else
            {
                playerScores[i].scorePanel.MoveTo(startPositions[i]);
            }
        }
    }
    
    public void ResetFills()
    {
        foreach (PlayerScore scr in playerScores)
        {
            scr.scorePanel.SetFill(0, true);
        }
    }



    public void Sort()
    {
        PlayerScore temp;

        for (int i = 1; i < playerScores.Count; i++)
        {
            int j = i;
            while (playerScores[j - 1].score < playerScores[j].score)
            {
                temp = playerScores[j - 1];
                playerScores[j - 1] = playerScores[j];
                playerScores[j] = temp;
                if (j != 1)
                {
                    j--;
                }
            }
        }
    }
}
