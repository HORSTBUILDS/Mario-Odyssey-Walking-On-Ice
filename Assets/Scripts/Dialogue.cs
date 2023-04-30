using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public Queue <string> sentences = new Queue<string> ();
    public TextMeshProUGUI UIDialogue;
    public TextMeshProUGUI NameText;
    public GameObject Nametext;
    bool Decision;
    public GameObject DecidePos;
    public GameObject DecideNeg;

   
     
  
 
    public void dialogue(string[] Sentences, bool decision)
    {
        Nametext.SetActive(true);
        UIDialogue.enabled = true;

        Decision = decision;

        sentences.Clear();
      
        foreach (string sentence in Sentences)
        {
            
            sentences.Enqueue(sentence);
        }
        NextSentence();
    }
    public void NextSentence()
    {
        // Decision
        if (sentences.Count == 0 && Decision)
        {
            DecidePos.SetActive(true);
            DecideNeg.SetActive(true);
            return;
        }
        if ( sentences.Count ==0)
        {
            if(Decision !=true)
            {
            
             UIDialogue.text = "";
             Nametext.SetActive (false);
             return;
            }
                  
        }
        string sentence = sentences.Dequeue();
        
         
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
       

    }
    IEnumerator TypeSentence (string sentence)
    {
        UIDialogue.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
           UIDialogue.text += letter;
            yield return new WaitForSeconds(.02F);
        }
        Invoke("NextSentence", 2F);
    }
    public void DecidePositive()
    {
        UIDialogue.text = "";
        string[] SentencesAgreed = FindObjectOfType<NPC>().SentencesAgreed;
        // Start new Dialogue
        dialogue(SentencesAgreed, false);
        //Disable Buttons
        DecidePos.SetActive(false);
        DecideNeg.SetActive(false);
        // STart Game
        GameManager.instance.StartCoroutine("StartGame");
    }

    public void DecideNegative()
    {
        UIDialogue.text = "";
        string[] SentencesDisagreed = FindObjectOfType<NPC>().SentencesDisagreed;
        // Start new Dialogue
        dialogue(SentencesDisagreed, false);

        // Disable Buttons
        DecidePos.SetActive(false);
        DecideNeg.SetActive(false); 

        GameManager.instance.Invoke("EndConversation",SentencesDisagreed.Length * 2);
    }
}
