using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public CinemachineFreeLook PlayCam;
    public CinemachineVirtualCamera ConversationCam;
    public static GameManager instance;
    public GameObject Player;
    Manager PlayerScript;
    public Transform Portal;
    public Transform DialoguePlace;
    public GameObject Parcour;
    private Tween Currenttween;
    [HideInInspector]
    public bool StartConversation;
    public Image bubble;
    Manager manager;


    Color Fadecolor;
    [HideInInspector]
    public bool Startgame;
    private bool ExitPortal;
    public int Score;
    void Awake()
    {
       instance = this;
       StartConversation = false;
        manager = GameObject.Find("Player").GetComponent<Manager>();
        PlayerScript = Player.GetComponent<Manager>();



   

    }
    private void Update()
    { 
        
        if(Startgame)
        {
            if(Vector3.Distance(Player.transform.position,Portal.position)>1.5F)
            {
                ExitPortal = true;
                PlayerScript.SpawnPointObj = true;
            }
            if(ExitPortal && Vector3.Distance(Player.transform.position,Portal.position)<.85F)
            {
                StartCoroutine(EndGame());
            }
        }
    }

    public void BlendCam(int Cam1, int Cam2)
    {
        PlayCam.Priority = Cam1;
        ConversationCam.Priority = Cam2;

    }
    public void Speechbubble(Image Bubble)
    {
        bubble = Bubble;
        Bubble.enabled = true;
        StartConversation = true;
        StartCoroutine(AnimateBubble(Bubble));
           
         
      
    }
    IEnumerator AnimateBubble(Image Bubble)
    {
        Canvas Bubblecanvas = Bubble.gameObject.transform.parent.GetComponent<Canvas>();
        if(Currenttween == null || Currenttween.IsComplete())
        {
             
          Bubblecanvas.transform.DOScale(.0015F, 1f).OnComplete(()=>
          {
             Currenttween = Bubblecanvas.transform.DOScale(.001f, 1f).OnComplete(() => { Currenttween = null; });
          });
          }
       
        Bubble.transform.LookAt(PlayCam.transform.position);

        yield return null;
        if(Bubble.enabled)
        { StartCoroutine(AnimateBubble(Bubble)); }  
    }
    public void StartDialogue(string[] Sentences, bool decision)
    {
        // Disable Speechbubble
        bubble.enabled = false;
        FindObjectOfType<Dialogue>().dialogue(Sentences,decision);

    }
    public void EndConversation()
    {
        BlendCam(20,12);
      
    }

    public IEnumerator StartGame()
    {
       
        
        yield return new WaitForSeconds(2F);
        FadeScreen(.003f);
        yield return new WaitForSeconds(2);
        FadeScreen(-.004F);
         
        BlendCam(20, 12);
        Parcour.SetActive(true);
       
        Quaternion Rotation = Quaternion.Euler(0, -90, 0);
        PlayerScript.SpawnPoint(Portal,true,Rotation);
        yield return new WaitForSeconds(.5F);
        StartDialogue(FindObjectOfType<NPC>().Instructions,false);
    }

    public void FadeScreen(float Value)
    {
        Image Fade = GameObject.Find("Fade").GetComponent<Image>();
        DOVirtual.Float(0, 100, 2, value => Fadecolor.a = value).OnUpdate(() =>
        {
            Fadecolor.a += Value;
            Fade.color = Fadecolor;
        });

    }
    public IEnumerator EndGame()
    {
        //BlackScreen
        FadeScreen(.0003f);
        yield return new WaitForSeconds(2.5f);
        FadeScreen(-.003F);
        // Player Pos to NPC
        Quaternion Rotation = Quaternion.Euler(0, -65, 0);
        PlayerScript.SpawnPoint(DialoguePlace,false,Rotation);
   
        BlendCam(12,20);
        //Destroy Objects
        Parcour.SetActive(false);
         
        // Calculate Points
        Score = PlayerScript.CalculatePoints();
        FindObjectOfType<NPC>().ScoreMessage();
        //yield return new WaitForSeconds(1f);
        // Dialogue
        StartDialogue(FindObjectOfType<NPC>().SentencesEndGame,false);
        PlayerScript.SpawnPointObj = false;
        ExitPortal = false;
        
        //Switch Cam to Main Cam
        yield return new WaitForSeconds((FindObjectOfType<NPC>().SentencesEndGame.Length *2) +2);
        BlendCam(20, 12);
        //Destroy Objects
        PlayerScript.DestroyPoints();
    }
}
