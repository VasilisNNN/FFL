using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Anima2D;
public class TrapGenerator : MonoBehaviour
{
    private List<Vector3> Positions = new List<Vector3>();
    private Player pl;
    private List<GameObject> Segment = new List<GameObject>();
    private float Deley = 1;
    private float SetTimer;
    public float LevelSpeed=-0.006f;
    private float MinBorder, RightBorder;
    private LevelDatabase LevelData;
    public int CurrentX { get; set; }
    public int CurrentXUI { get; set; }
    private float CN = 1;
    private int CurrentLevel;
    // Start is called before the first frame update
    public float SegmentsDistance = 1.5f;
    public int MaxLevels { get; set; }
    public bool BOSS { get; set; }
    public int BOSSHP { get; set; }
    public int MAXBOSSHP { get; set; }
    public bool END { get; set; }

    void Start()
    {
       
        LevelData = GameObject.Find("LevelDatabase").GetComponent<LevelDatabase>();

     
        pl = GameObject.Find("Player").GetComponent<Player>();
        for (int i = 0; i <LevelData.level[CurrentLevel].MaxNum; i++)
            Positions.Add(GameObject.Find("Position (" + i + ")").transform.position);

        for (int i = 0; i < 9; i++)
            Segment.Add(GameObject.Find("Segment (" + i + ")"));

        for (int i = 0; i < 9; i++)
            Segment[i].transform.position = new Vector3((i* SegmentsDistance)-2, 0, 0);

        transform.position = new Vector3((Segment.Count-6) * SegmentsDistance, 0, 0);

        GameObject.Find("TrapEnd").transform.position = new Vector3(-6 * SegmentsDistance, 0, 0);
        MinBorder = GameObject.Find("TrapEnd").transform.position.x;
        RightBorder = GameObject.Find("TrapRightBorder").transform.position.x;
        int c = 1;
        for (int i = 0; i < LevelData.level[CurrentLevel].MaxNum - 1; i++) c *= 10;
        CN = c;


        if (SceneManager.GetActiveScene().name == "Level0")
        {
            BOSS = true;
            BOSSHP = 10;
            MAXBOSSHP = 10;
            GameObject b = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Bosses/Boss_Level_" + PlayerPrefs.GetInt("DateLevel")));
            b.name = "Boss_Level";
        }
        
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < LevelData.level[CurrentLevel].MaxNum; i++)
            Positions[i] = GameObject.Find("Position (" + i + ")").transform.position;


        if (LevelData.level[CurrentLevel].MaxNum == 6)
            Camera.main.orthographicSize = 1.7f;
        if (LevelData.level[CurrentLevel].MaxNum == 7)
            Camera.main.orthographicSize = 2;

        CreateExit();

        if (!pl._gameover&& !pl.Options)
        {
            MaxLevels = LevelData.level.Count - 1;

            // PlayerPrefs.SetInt("CurrentLevel", 7);
            CurrentLevel = PlayerPrefs.GetInt("DateLevel");

            if (!BOSS)
            {
                pl.MaxSeconds = LevelData.level[CurrentLevel].traps.Length;
            }
            else
            {
                pl.MaxSeconds = MAXBOSSHP;
                CurrentXUI = MAXBOSSHP - BOSSHP;

            }

            for (int i = 0; i < Segment.Count; i++)
            {
                Segment[i].transform.position = new Vector3(Segment[i].transform.position.x + LevelSpeed, Segment[i].transform.position.y, 1f);

                if (Segment[i].transform.position.x < MinBorder)
                {
                    ResetTraps(i);
                    if (!BOSS)
                    {
                        if (CurrentX < LevelData.level[CurrentLevel].traps.Length - 1) CurrentX++;
                        if (CurrentXUI < pl.MaxSeconds) CurrentXUI++;
                    }
                    else
                    {
                        if (CurrentX < LevelData.level[CurrentLevel].traps.Length - 1) CurrentX++;
                        else CurrentX = 0;
                    }

                }





                if (Segment[i].transform.childCount > 0)
                {
                    for (int s = 0; s < Segment[i].transform.childCount; s++)
                    {
                        
                        if (pl.transform.position.y-0.25f < Segment[i].transform.GetChild(s).position.y)
                        Segment[i].transform.GetChild(s).GetComponent<SpriteRenderer>().sortingLayerName = "Pers";
                        else Segment[i].transform.GetChild(s).GetComponent<SpriteRenderer>().sortingLayerName = "ItemFG";

                        if (pl.transform.position.y - 0.25f < Segment[i].transform.GetChild(s).position.y)
                        {
                            if (Segment[i].transform.GetChild(s).GetComponent<SpriteMeshInstance>() != null)
                            Segment[i].transform.GetChild(s).GetComponent<SpriteMeshInstance>().sortingLayerName = "Pers";
                            
                        }
                        else
                        {
                            if (Segment[i].transform.GetChild(s).GetComponent<SpriteMeshInstance>() != null)
                                Segment[i].transform.GetChild(s).GetComponent<SpriteMeshInstance>().sortingLayerName = "ItemFG";
                        }

                        for (int j = 0; j < Segment[i].transform.GetChild(s).childCount; j++)
                        {
                            Transform cht = Segment[i].transform.GetChild(s).GetChild(j);
                            if (cht != null)
                            {
                                if (pl.transform.position.y - 0.25f < cht.position.y)
                                {
                                    if (cht.GetComponent<SpriteMeshInstance>() != null)
                                        cht.GetComponent<SpriteMeshInstance>().sortingLayerName = "Pers";

                                }
                                else
                                {
                                    if (cht.GetComponent<SpriteMeshInstance>() != null)
                                        cht.GetComponent<SpriteMeshInstance>().sortingLayerName = "ItemFG";
                                }
                            }
                        }

                        if (Segment[i].transform.GetChild(s).tag == "Pillar")
                        {
                            for (int b = 0; b < Segment[i].transform.GetChild(s).transform.childCount; b++)
                            {
                                Segment[i].transform.GetChild(s).transform.GetChild(b).GetComponent<SpriteRenderer>().sortingOrder = Segment[i].transform.GetChild(s).GetComponent<SpriteRenderer>().sortingOrder + b + 1;
                                Segment[i].transform.GetChild(s).transform.GetChild(b).GetComponent<SpriteRenderer>().sortingLayerName = Segment[i].transform.GetChild(s).GetComponent<SpriteRenderer>().sortingLayerName;

                                if (Segment[i].transform.GetChild(s).transform.GetChild(b).GetComponent<SpriteMeshInstance>() != null)
                                {
                                    Segment[i].transform.GetChild(s).transform.GetChild(b).GetComponent<SpriteMeshInstance>().sortingOrder = Segment[i].transform.GetChild(s).GetComponent<SpriteRenderer>().sortingOrder + b + 1;
                                    Segment[i].transform.GetChild(s).transform.GetChild(b).GetComponent<SpriteMeshInstance>().sortingLayerName = Segment[i].transform.GetChild(s).GetComponent<SpriteRenderer>().sortingLayerName;

                                }
                            }
                        }
                    }
                }

                if (BOSS && BOSSHP <= 0)
                {
                    if (Segment[i].transform.position.x > GameObject.Find("EndPoint").transform.position.x)
                    {
                        if (Segment[i].transform.childCount > 0 && GameObject.Find("END(Clone)") == null)
                        {
                            for (int s = 0; s < Segment[i].transform.childCount; s++)
                            {
                                Destroy(Segment[i].transform.GetChild(s).gameObject);

                            }
                            END = true;
                            GameObject Trap = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Traps/END"), Segment[i].transform);
                            Trap.transform.position = new Vector3(Segment[i].transform.position.x, pl.GetPositions()[2].y, 1);

                            Destroy(GameObject.Find("Boss_Level"));
                        }

                    }
                }
            }
        }
    }

    void ResetTraps(int CurrentSerment)
    {
        int c = 1;
        for (int i = 0; i < LevelData.level[CurrentLevel].MaxNum - 1; i++)c *= 10;
        CN = c;

        Segment[CurrentSerment].transform.position = transform.position;

        for (int i = 0; i < Segment[CurrentSerment].transform.childCount; i++)
        {
            Destroy( Segment[CurrentSerment].transform.GetChild(i).gameObject);
           
        }

        for (int i = LevelData.level[CurrentLevel].MaxNum-1; i > -1; i--)
        {
            
                if (pl.GetSeconds() < pl.GetMaxSeconds())
                {


                    if ((int)((LevelData.level[CurrentLevel].traps[CurrentX] / CN) % 10) == 1)
                    CreateTrap(CurrentSerment, i, "Prefabs/Traps/Mine", "Prefabs/Traps/Mine");

                    if ((int)((LevelData.level[CurrentLevel].traps[CurrentX] / CN) % 10) == 2)
                    CreateTrap(CurrentSerment, i, "Prefabs/Traps/Block", "Prefabs/Traps/Block");

                    if ((int)((LevelData.level[CurrentLevel].traps[CurrentX] / CN) % 10) == 3)
                    CreateTrap(CurrentSerment,i,"Prefabs/Traps/Money", "Prefabs/Traps/Pillar");

                    if ((int)((LevelData.level[CurrentLevel].traps[CurrentX] / CN) % 10) == 4)
                    CreateTrap(CurrentSerment, i, "Prefabs/Traps/Push", "Prefabs/Traps/Push");
                
                    if ((int)((LevelData.level[CurrentLevel].traps[CurrentX] / CN) % 10) == 5)
                    CreateTrap(CurrentSerment, i, "Prefabs/Traps/Mine", "Prefabs/Traps/Mine");
                
                    if ((int)((LevelData.level[CurrentLevel].traps[CurrentX] / CN) % 10) == 6)
                    {
                        GameObject Trap = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Traps/Beam"), Segment[CurrentSerment].transform);
                        Trap.transform.position = new Vector3(transform.position.x + 0.1f * i, pl.GetPositions()[i].y, 1);
                    
                }

                    if ((int)((LevelData.level[CurrentLevel].traps[CurrentX] / CN) % 10) == 7)
                    CreateTrap(CurrentSerment, i, "Prefabs/Traps/HPPlus", "Prefabs/Traps/HPPlus");
                
                if ((int)((LevelData.level[CurrentLevel].traps[CurrentX] / CN) % 10) == 8)
                    CreateTrap(CurrentSerment, i, "Prefabs/Traps/Stable", "Prefabs/Traps/PillarBlow");

                if ((int)((LevelData.level[CurrentLevel].traps[CurrentX] / CN) % 10) == 9)
                    CreateTrap(CurrentSerment, i, "Prefabs/Traps/Stable", "Prefabs/Traps/Pillar999");

                CN /= 10;

                }
                
            
        }
        
    }
    void CreateExit()
    {
        for (int i = 0; i < Segment.Count; i++)
        {
            if (CurrentX >= LevelData.level[CurrentLevel].traps.Length - 1 && !BOSS)
            {
                if (Segment[i].transform.position.x > GameObject.Find("EndPoint").transform.position.x)
                {

                    if (GameObject.Find("END(Clone)") == null)
                    {
                        END = true;
                        GameObject Trap = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Traps/END"), Segment[i].transform);
                        Trap.transform.position = new Vector3(Segment[i].transform.position.x, pl.GetPositions()[2].y, 1);
                        
                        if (Segment[i].transform.childCount > 0)
                        {
                            for (int s = 0; s < Segment[i].transform.childCount; s++)
                            {
                                if (Segment[i].transform.GetChild(s).tag != "Doors")
                                    Destroy(Segment[i].transform.GetChild(s).gameObject);

                            }
                        }
                    }


                    if (Segment[i].transform.childCount > 0 && GameObject.Find("END(Clone)") != null)
                    {
                        for (int s = 0; s < Segment[i].transform.childCount; s++)
                        {
                            if (Segment[i].transform.GetChild(s).tag != "Doors")
                                Destroy(Segment[i].transform.GetChild(s).gameObject);
                        }

                    }

                    for (int s = 0; s < Segment[i].transform.childCount; s++)
                    {
                        if (Segment[i].transform.position.x > GameObject.Find("END(Clone)").transform.position.x)
                        {
                            if (Segment[i].transform.GetChild(s).tag != "Doors")
                                Destroy(Segment[i].transform.GetChild(s).gameObject);
                        }
                    }
                }
            }
        }
    }
  

    void CreateTrap(int currents,int i, string Path0State,string Path1State)
    {
        string PathName = "";
        if (LevelData.level[CurrentLevel].state == 0) PathName = Path0State;
        else if (LevelData.level[CurrentLevel].state == 1) PathName = Path1State;
        else PathName = Path0State;

        GameObject Trap = Instantiate<GameObject>(Resources.Load<GameObject>(PathName), Segment[currents].transform);
        Trap.transform.position = new Vector3(transform.position.x + 0.1f * i, pl.GetPositions()[i].y, 1);
        Trap.GetComponent<SpriteRenderer>().sortingOrder = i;


    }
    public float GetSegmentsDistance()
    {
        return SegmentsDistance;
    }
    public List<GameObject> GetSegments()
    {
        return Segment;
    }
}
