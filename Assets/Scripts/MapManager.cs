using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager managerInstance;
    public int currentMap = 0;
    [SerializeField] int lastZone;
    [SerializeField] public float zoneOffset;
    [SerializeField] int initialZoneCount;
    private int generatedZones;
    [SerializeField] List<GameObject> zoneList;
    List<GameObject> allZones = new List<GameObject>();//All non active and active
    Queue<GameObject> activeZones = new Queue<GameObject>();
    GameObject prevZone;

    AudioManager am;
    private void Awake()
    {
        am = AudioManager.amInstance;
        managerInstance = this; //Note to self: Used in trigger map update
    }
    void Start()
    {
        generatedZones = 1;
        for (int x = 0; x < zoneList.Count; x++)
        {
            for(int y = 0; y<3;y++)
            {
                GameObject thisObj = Instantiate(zoneList[x]);
                allZones.Add(thisObj);
                thisObj.transform.parent = gameObject.transform;                          
                thisObj.SetActive(false);
            }
        }
        for(int x=0;x<initialZoneCount;x++)
        {
            addZones();
        }

        am.plyBGM("gameBGM");
        /* Cheat test
        foreach(GameObject obs in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            obs.GetComponent<BoxCollider>().enabled = false;
        }
        //*/
    }
    public void addZones()
    {
        lastZone++;
        int randZoneIndex = Random.Range(0, allZones.Count);
        GameObject randZone = allZones[randZoneIndex];
        randZone.SetActive(true);
        Transform coinChild = randZone.transform.Find("Coins").transform;
        for (int x =0; x< coinChild.childCount;x++)
        {
            coinChild.GetChild(x).gameObject.SetActive(true); // or false
        }
        randZone.transform.position = new Vector3(0, 0, lastZone * zoneOffset);
        activeZones.Enqueue(randZone);
        allZones.RemoveAt(randZoneIndex);

        generatedZones++;
    }
    public void updateCurrentMap(GameObject zone)
    {
        currentMap++;
        
        if(prevZone)
        {
            //Debug.Log($"Prev zone inactive:{zone.name}");
            prevZone.SetActive(false);
            allZones.Add(prevZone);
        }

        prevZone = zone;
        addZones();
        //*/
    }
    public void resetRegenCount()
    {
        lastZone = initialZoneCount+1;
    }
}
