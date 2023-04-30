using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject Point;
    public Transform Player;
    public List<GameObject> Points = new List<GameObject>();
    public GameObject Arrow;
    private List<GameObject> Arrows = new List<GameObject>(); 
    public int MaxPoints = 45;
    public float Distanceforpoints =.1F;
    private Vector3 PointPos;
    private Collider PointCollider;
    private GameObject NEWPoint;
    [HideInInspector]
    public bool SpawnPointObj;
    void Start()
    {
       for(int i = 0; i < Arrow.transform.childCount; i++)
        {
            GameObject Child = Arrow.transform.GetChild(i).gameObject;
            Arrows.Add(Child);
        }
       PointCollider = Point.GetComponent<Collider>();
        
        
    }
 
    void Update()
    {
       
        if (SpawnPointObj)
        {
           
         // Position Point Spawns
        PointPos = new Vector3(Player.position.x, 0, Player.position.z);
        // First Point Spawn
        if (Points.Count == 0)
        {
            NEWPoint =Instantiate(Point);
            NEWPoint.transform.position = PointPos;
            Points.Add(NEWPoint);   
        }

        // Delete oldest Point if to many in Game
        if (Points.Count >= MaxPoints )
        {
            GameObject oldPoint = Points[0];
            Destroy(oldPoint.gameObject);
            Points.RemoveAt(0);
             

        }

        if (Points.Count > 0 && Vector3.Distance(Player.transform.position, NEWPoint.transform.position) > 1F && Point != null)
        {
            NEWPoint = Instantiate(Point);
            NEWPoint.transform.position = PointPos;
            Points.Add(NEWPoint);
        }

     
        }
        // Parameter for Dialogue
        if (GameManager.instance.StartConversation)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Switch Cameras to Group Target
                GameManager.instance.BlendCam(10, 20);
                //Start Interaction
                GameManager.instance.StartDialogue(FindObjectOfType<NPC>().Sentences, true);

            }
        }



    }

    public int CalculatePoints()
    {
        int points =0;
       // float nearest = Mathf.Infinity;
       foreach (GameObject arrows in Arrows)
        {
        foreach(GameObject point in Points)
         {
            if(point != null)
                {
                  float distance = Vector3.Distance(arrows.transform.position, point.transform.position);    
                 if(distance < Distanceforpoints)
                 {
                   int Increment = 100 / Arrows.Count;
                   points += Increment;
                 }
         }
            }
        
        }
         
        return points;
    }
 

    public void SpawnPoint(Transform Pos, bool Startgame,Quaternion Rotation)
    {
        transform.DOMove(Pos.position, .1f).OnComplete(() => { GameManager.instance.Startgame = Startgame; transform.rotation = Rotation; });
        
    }
    public void DestroyPoints()
    {
    
        foreach(GameObject p in Points)
        {
            Destroy(p);
        }
        Points.Clear();
    }
}
