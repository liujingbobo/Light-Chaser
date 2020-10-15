using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class loadTiles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Tile loadTile(string path)
    {
        Tile tile= Resources.Load(path, typeof(Tile)) as Tile;
        if (tile) Debug.Log(tile.sprite);
        else Debug.Log("load:null");
        return tile;
    }
}
