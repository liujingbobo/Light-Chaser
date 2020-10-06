using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tiles : MonoBehaviour
{
    [Header("地图矩阵，第一行宽和高，逗号隔开")]
    public TextAsset MapText;
    [Header("数字和对应的地块名称，地块放Resources/Tiles里，逗号隔开")]
    public TextAsset DicText;
    [Header("主角")]
    public GameObject player;
    [Header("")]
    public GameObject grass;
    [Header("要绘制的tilemap")]
    public Tilemap tilemap;
    Vector3 playerPosition;
    Vector3Int  cellPosition;
    int[,] Map;
    int width, height;
    //public 
    Tile tile1;
    Dictionary<int, string> Dic=new Dictionary<int, string>();
    // Start is called before the first frame update
    void Start()
    {
        makeDic();
        makeMap();
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.transform.position;
        cellPosition = tilemap.WorldToCell(playerPosition);
        handleTiles();
    }
    void makeDic()
    {
        string dicText = DicText.text;
        string[] pairs = dicText.Split('\n');
        foreach (string str in pairs)
        {
            string[] pair = str.Split(',');
            Dic.Add(int.Parse(pair[0]), "Tiles/" + pair[1]);
            //Debug.Log(Dic[1]);
        }
    }
    void makeMap()
    {
        string mapText = MapText.text;
        string[] col = mapText.Split('\n');
        string[] firstRow = col[0].Split(',');
        //Debug.Log(firstRow[0]+" "+firstRow[1]);
        width = int.Parse(firstRow[0]);
        height =int.Parse(firstRow[1]);
        Map=new int[height,width];
        for (int i=1;i<col.Length;i++)
        {
            string[] row = col[i].Split(',');
            for(int j=0;j<row.Length;j++)Map[i - 1,j] = int.Parse(row[j]);
            //Debug.Log(map[i-1,3]);
        }
        //for (int j = height-1; j >=0; j--)
           // for (int i = 0; i < width; i++) Debug.Log(map[j, i]);
        for (int j = height - 1; j >= 0; j--)
        {
            for (int i = 0; i < width; i++)
            {
                if (Map[j, i] > 0)
                {
                    string path =Dic[Map[j,i]];
                    tile1 = Resources.Load(path, typeof(Tile)) as Tile;
                    //Debug.Log(path);
                    tilemap.SetTile(new Vector3Int((i - width / 2), height / 2 - j-1, 0), tile1);
                }
            }
        }
    }
    private void handleTiles()
    {
        //消除tile
        if (Input.GetKeyDown(KeyCode.E))
        {
            //tilemap.SetTile(cellPosition, gameUI.GetSelectColor().colorData.mTile);
            Vector3Int realCellPosition = cellPosition + new Vector3Int(0, -1, 0);
            TileBase tb = tilemap.GetTile(realCellPosition);
            //Debug.Log("人物坐标" + playerPosition + "cell" + cellPosition + "tb"+(cellPosition+new Vector3Int(0,-1,0)));
            //Debug.Log(height/2-realCellPosition.y+" "+ (realCellPosition.x+width/2)+" "
              //  +Map[height / 2 - realCellPosition.y, realCellPosition.x+width/2]);
            if (tb == null||Map[height / 2 - realCellPosition.y-1, realCellPosition.x + width / 2] !=1)
            {
                return;
            }
            //Debug.Log("succeed erase");
            //tb.hideFlags = HideFlags.None;
            tilemap.SetTile(realCellPosition, null);
            //tilemap.RefreshAllTiles();
        }
        //
        if (Input.GetKeyDown(KeyCode.M))
        {
            Vector3Int realCellPosition = cellPosition + new Vector3Int(0, 1, 0);
            GameObject newGrass = Instantiate(grass, realCellPosition,Quaternion.identity);
        }
    }
}
