using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    private GameObject Player;
    private float weight;
    MultiAimConstraint LookConstraint;
    public Image  Speechbubble;

    [TextArea(2, 5)]
    public string[] Sentences;

    [TextArea(2, 5)]
    public string[] SentencesAgreed;

    [TextArea(2, 5)]
    public string[] SentencesDisagreed;
    [TextArea(2, 5)]
    public string[] SentencesEndGame;
    [TextArea(2, 5)]
    public string[] Instructions;
    int Arraystartsize;

    void Start()
    {
        LookConstraint = GameObject.Find("HeadAim").GetComponent<MultiAimConstraint>();
        Speechbubble.enabled = false;
          Arraystartsize = SentencesEndGame.Length;    
        
    }

   
        
     
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            TrySpeak();
            Player = other.gameObject;
            GameManager.instance.Speechbubble(Speechbubble);
           
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag =="Player")
        {
             DOTween.To(()=> 1F, x => weight = x, 0, .5F).OnUpdate(()=>
             {
                LookConstraint.weight = weight;
             });

            Speechbubble.enabled = false;
        }
      
      
    }

    void TrySpeak()
    {
        LookConstraint.weight = 1;
    }
    public void ScoreMessage()
    {      // If Statement to avoid multiple Score Messages
        if (SentencesEndGame.Length + 1 != Arraystartsize + 1)
        {
            SentencesEndGame[SentencesEndGame.Length - 1] = "You Scored   " + GameManager.instance.Score + "    Points";
         
        }
      
        if (SentencesEndGame.Length+1 == Arraystartsize+1)
        {
            Array.Resize(ref SentencesEndGame, SentencesEndGame.Length + 1);
            SentencesEndGame[SentencesEndGame.Length - 1] = "You Scored  " + GameManager.instance.Score + "   Points";
             

        }
      
    }
}
