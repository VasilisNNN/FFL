using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathAnim : MonoBehaviour {
    public SpriteRenderer HeadSPRT;
    private SpriteRenderer BodySPRT;
    public Sprite WalkingBody;

    private List<GameObject> coll_obj = new List<GameObject>();
    private List<GameObject> Legscoll_obj = new List<GameObject>();
    public GameObject Legs;
    public bool Skel;
    private Transform _transform;
    public bool Dead { get; set; }
    private float TraseTimer,TraseTimerMax;
    public bool Invis { get; set; }
    public bool Cold;
    private Vector2 HeadForce;
    // Use this for initialization
    void Start () {
        
        
        BodySPRT = transform.Find("BodySPRT").GetComponent<SpriteRenderer>();
        
        HeadSPRT = transform.Find("Head").gameObject.GetComponent<SpriteRenderer>();
        Legs = transform.Find("LegsSPRT").gameObject;

        if (GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().freezeRotation = true;
        }
     
        _transform = transform;

        if (HeadSPRT.transform.Find("Breath") == null && Cold)
        {
            GameObject breath = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Breath"), HeadSPRT.transform);
            breath.name = "Breath";

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (HeadSPRT.GetComponent<Dialog>() != null) HeadSPRT.GetComponent<Dialog>().SetDead(Dead);

        for (int i = 0; i < coll_obj.Count; i++)
        {
            if (coll_obj[i] != null)
            {
                if (coll_obj[i].GetComponent<BoxCollider2D>() != null)
                {
                    if (!coll_obj[i].GetComponent<BoxCollider2D>().enabled)
                    {
                        coll_obj.RemoveAt(i);

                    }
                }
            }
            else coll_obj.RemoveAt(i);
        }

        BloodDeath();
        HeadTrase();

        if (Dead)
        {
            if (transform.parent != null)
            {
                
                if (BodySPRT != null) BodySPRT.color = new Color(0.5f, 0.5f, 0.5f, 1);
               

            }

            PlayerPrefs.SetInt(name + "Destroy" + PlayerPrefs.GetInt("CurrentLevel"), PlayerPrefs.GetInt("CurrentLevel"));

            for (int i = 0; i < _transform.childCount; i++)
            {
                if (_transform.GetChild(i).name != "Player (1)"&&_transform.GetChild(i).name != "BodySPRT" && _transform.GetChild(i).name != "LegsSPRT" && _transform.GetChild(i).name != "Head")
                    Destroy(_transform.GetChild(i).gameObject);
            }
        }
    }
    void BloodDeath()
    {
       

        if (name == "Player")
        {
            coll_obj = GetComponent<Player>().Getcollob();
            Legscoll_obj = GetComponent<Player>().GetLegscollob();
        }
        
        if (Dead) { 
           if(transform.parent != null) transform.parent.tag = "Flipping";
               
                    if (transform.GetComponent<Player>() != null)
                    {
                        GetComponent<Player>().HP = 0;
                    }

                    if (transform.Find("PlayerHands")!= null)
                    {
                        GameObject.Find("Player").GetComponent<Player>().HP = 0;
                     
                        Destroy(transform.Find("PlayerHands").gameObject); 
                    }


                    
                    if (HeadSPRT.transform.Find("BleedingHead") == null)
                    {
                        if (!Skel)
                        {

                            Destroy(Legs);
                        }

               // print("BH");
                        Sprite[] sprt = Resources.LoadAll<Sprite>("Sprites/Effects/BloodFaceEffect");
                        Sprite[] sprtbody = Resources.LoadAll<Sprite>("Sprites/Effects/BleedingBody");
                        Sprite[] sprtblood = Resources.LoadAll<Sprite>("Sprites/Effects/BloodWallEffect");
                        Sprite[] skeletonblood = Resources.LoadAll<Sprite>("Sprites/Effects/SkeletonBody");


                        GameObject b = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/BleedingHead"), HeadSPRT.transform);
                        b.name = "BleedingHead";

                        GameObject bloodwall = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/BleedingWall"), null);
                        bloodwall.name = "BleedingWall";
                        bloodwall.transform.position = HeadSPRT.transform.position;
                        bloodwall.GetComponent<SpriteRenderer>().sprite = sprtblood[Random.Range(0, 7)];
                
                            GameObject skeleton = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/SkeletonBody"), transform);
                            skeleton.name = "SkeletonBody";
                            skeleton.transform.position = BodySPRT.transform.position;
                            skeleton.GetComponent<SpriteRenderer>().sprite = skeletonblood[Random.Range(0, skeletonblood.Length - 1)];
                          // Destroy(BodySPRT.gameObject);
                        
                       

                        b.GetComponent<SpriteRenderer>().sprite = sprt[Random.Range(0, 15)];
                        b.GetComponent<SpriteRenderer>().sortingLayerName = HeadSPRT.sortingLayerName;
                        b.GetComponent<SpriteRenderer>().sortingOrder = HeadSPRT.sortingOrder + 1;
                        TraseTimerMax = Time.fixedTime + 2.3f;
                       


                        HeadBoom(new Vector2(Random.Range(30,50), Random.Range(10, 20)));

                        PlayerPrefs.SetInt("Death", PlayerPrefs.GetInt("Death")+1);
                    }


            if (Legs != null)
            {
                if (Legs.GetComponent<Animator>()!=null)
                {
                    if (Legs.GetComponent<Animator>().GetBool("Walk"))
                    {
                        Legs.GetComponent<Animator>().SetBool("Walk", false);
                        Legs.GetComponent<Animator>().SetBool("Stand", true);
                    }
                }
            }
            if (BodySPRT != null)
            {
                if (BodySPRT.transform.Find("BodiesWorms") != null)
                {

                    BodySPRT.transform.Find("BodiesWorms").GetComponent<SpriteRenderer>().sortingLayerName = BodySPRT.sortingLayerName;
                    BodySPRT.transform.Find("BodiesWorms").GetComponent<SpriteRenderer>().sortingOrder = BodySPRT.sortingOrder + 1;
                }
            }
            if (transform.Find("BodiesCover") != null&& BodySPRT!=null)
            {
                transform.Find("BodiesCover").GetComponent<SpriteRenderer>().sortingLayerName = BodySPRT.sortingLayerName;
                transform.Find("BodiesCover").GetComponent<SpriteRenderer>().sortingOrder = BodySPRT.sortingOrder + 1;
            }
            
        }
    }
   
    void HeadBoom(Vector2 F)
    {

        
        if (HeadSPRT.GetComponent<Rigidbody2D>() == null)HeadSPRT.gameObject.AddComponent<Rigidbody2D>();
            
        if (HeadSPRT.GetComponent<BoxCollider2D>() == null) HeadSPRT.gameObject.AddComponent<BoxCollider2D>();


        HeadSPRT.GetComponent<Rigidbody2D>().gravityScale = 0.1f;
        HeadSPRT.GetComponent<Rigidbody2D>().AddForce(F);
        HeadForce = F;


        HeadSPRT.transform.parent = null;

    }
    void HeadTrase()
    {

        if (TraseTimerMax > Time.fixedTime)
        {
            if (TraseTimer < Time.fixedTime)
            {
                GameObject floarblood = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/FloarBlood"));
                floarblood.name = "Trase";
                floarblood.transform.position = HeadSPRT.transform.position;
                floarblood.GetComponent<SpriteRenderer>().sortingLayerName = HeadSPRT.sortingLayerName;


                floarblood.GetComponent<SpriteRenderer>().sprite =
                Resources.LoadAll<Sprite>("Sprites/Effects/BloodWallHeadEffect")[Random.Range(0, 10)];

                TraseTimer = Time.fixedTime + 0.25f;
            }
        }
        else if (HeadSPRT != null)
        {
            HeadPick();
        }
       
    }

    void HeadPick()
    {
        if (HeadSPRT.GetComponent<Rigidbody2D>() != null)
        {
            HeadSPRT.GetComponent<Rigidbody2D>().gravityScale = 0;

           // HeadSPRT.GetComponent<BoxCollider2D>().isTrigger = true;
           HeadSPRT.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
           
        }
        
    }

   
    private void OnTriggerStay2D(Collider2D c)
    {
        if (name != "Player")
        {
            if (!coll_obj.Contains(c.gameObject))
            {
                coll_obj.Add(c.gameObject);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D c)
    {

        if (name != "Player")
        {
            if (coll_obj.Contains(c.gameObject))
            {
                coll_obj.Remove(c.gameObject);
            }
        }
    }

}
