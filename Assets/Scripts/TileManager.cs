using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//// Manager for endless tiles
public class TileManager : UnitySingleton<TileManager>
{
    public GameObject PickUp;
    
    //// player's tranform
    private Transform playerTransform;

    //// current furtherest tile Z position
    private float spawnZ = 0.0f;

    //// length of each tile
    private float tileLength = 15.2f;

    //// how many tiles shown on screen simultaneously
    public int amnTilesOnScreen = 6;

    //// variables for generate hurdle on Road
    private int maxHurdleNum = 4;
    private float[] hurdleXOff = {-1.2f, 0.0f, 1.2f};
    private float[] hurdleZOff = {-11.4f, -7.6f, -3.8f, 0.0f};
    private int[] maxHurdleEachRow = { 2, 2, 2, 1 };

    //// current active tiles
    private List<GameObject> activeTiles;

    void Start()
    {   
        //// find player and init the list
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        activeTiles = new List<GameObject>();
        StartCoroutine(InitPickUp());
    }

    IEnumerator InitPickUp()
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            int Cnt = Random.Range(2, 5);
            for (int i = 0; i < Cnt; i++)
            {
                ////Instanciate Pick Up
                Vector3 playerPos = playerTransform.position;
                Vector3 rockPosition = new Vector3(Random.Range(-1.2f, 1.2f), 1.0f, playerPos.z + 10.0f);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(PickUp, rockPosition, spawnRotation);

                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(2.0f);

            if (GameManager.Instance.IsDead()) break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        //// skip if IsDead
        if (GameManager.Instance.IsDead()) return;

        //// When there is no enough tiles, generate tiles
        while (activeTiles.Count < amnTilesOnScreen){
            if(activeTiles.Count < 3)
                spawnTile(0, 0);
            else
                spawnRandomTile();
        }

        //// automatically generate tile ahead and hide tile in the back
        if(playerTransform.position.z > (spawnZ - amnTilesOnScreen * tileLength)){
            spawnRandomTile();
            hideTile();
        }
    }

    //// spawn a random tile
    private void spawnRandomTile(){
        int index = Random.Range(0, TilePool.Instance.tilePrefabs.Length*3/2);
        if(index >= TilePool.Instance.tilePrefabs.Length)
            index = 0;

        //// increse hurdle nums as game running
        float time = Time.timeSinceLevelLoad;
        int num = Mathf.Max((int)time / 25, 5);
        spawnTile(index, num);
    }

    //// spawn a tile with specific prefabIndex and hurdles
    private void spawnTile(int prefabIndex = -1, int hurdleNum = 0){
        GameObject go;
        
        go = TilePool.Instance.GetPooledObject(prefabIndex);
        go.transform.SetParent(transform);
        go.transform.position = Vector3.forward * spawnZ;
        
        spawnZ += tileLength;

        go.SetActive(true);
        activeTiles.Add(go);
        
        if(prefabIndex == 0)
            activeHurdles(go, hurdleNum);
    }

    //// active hurdles on a tile ( only available for 'Road' )
    private void activeHurdles(GameObject go, int hurdleNum){
        if(hurdleNum == 0)
            return;
        if(hurdleNum > maxHurdleNum)
            hurdleNum = maxHurdleNum;
        
        //// count hurdle in each row, avoid too much hurdles in same row
        int[] hurdleEachRow = new int[hurdleZOff.Length];
        for (int i=0; i<hurdleEachRow.Length; i++) 
            hurdleEachRow[i] = 0;

        //// active hurdles
        int offset = Random.Range(0, maxHurdleNum);
        for (int i = 1; i <= hurdleNum; i++){
            string hurdleName = "Hurdle0" + ((i + offset) % maxHurdleNum).ToString();
            Transform hurdleTransform = go.transform.Find(hurdleName);

            
            if(hurdleTransform){
                GameObject hurdle = hurdleTransform.gameObject;
                hurdle.SetActive(true);
                
                //// choose proper location
                int location, XOff, ZOff;
                do{
                    location = Random.Range(0, hurdleXOff.Length * hurdleZOff.Length);
                    XOff = location / hurdleZOff.Length;
                    ZOff = location % hurdleZOff.Length;
                }while(hurdleEachRow[ZOff] >= maxHurdleEachRow[ZOff]);

                hurdle.transform.localPosition = new Vector3(hurdleXOff[XOff], 0, hurdleZOff[ZOff]);
            }
        }
    }

    //// hide the last active tile in activeTiles
    private void hideTile(){
        activeTiles[0].SetActive(false);
        //// deactive all hurdles
        for (int i = 1; i <= maxHurdleNum; i++){
            string hurdleName = "Hurdle0" + i.ToString();
            Transform hurdleTransform = activeTiles[0].transform.Find(hurdleName);

            if(hurdleTransform)
                hurdleTransform.gameObject.SetActive(false);
            else
                break;
        }
        activeTiles.RemoveAt(0);

    }

    //// hide all tiles in activeTiles
    public void hideAll() {
        while (activeTiles.Count != 0) {
            hideTile();
        }
    }
}
