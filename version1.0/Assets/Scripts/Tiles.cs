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
    //loadTiles loadtiles;
    int[,] Map;
    int width, height;
    //Dictionary<int, string> Dic=new Dictionary<int, string>();
    Dictionary<int, Tile> Dic = new Dictionary<int, Tile>();
    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {
        //movingTiles.array= new int[movingTileNum,2];
        Dic.Clear();
        makeDic();
        foreach (var str in Dic)
        {
            //makeMap(tile,idx);
            makeMap(str.Value,str.Key);
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.transform.position;
        cellPosition = tilemap.WorldToCell(playerPosition);
        handleTiles();
    }
    IEnumerator fade(Vector3Int pos)
    {
        int t = Map[height / 2 - pos.y - 1, pos.x + width / 2],flag=0;
        Tile tile0 = Dic[t];
        float time = 0.05f;
        for (float i = 1.0f; i >= 0.0f; i -= 5*Time.deltaTime)
        {
            yield return new WaitForSeconds(time);
            Color c = tile0.color;
            //Color c = tilemap.color;
            c.a = i;
            //tilemap.color = c;
            //tilemap.SetTile(pos, tile0);
            tilemap.SetColor(pos, c);
           // Debug.Log(tile0.color.a);
        }
        /*tilemap.SetTile(pos,Dic[t+1]);
        for (float i = 0.4f; i <= 1; i += 5 * Time.deltaTime)
        {
            yield return new WaitForSeconds(time);
            Color c = tile0.color;
            c.a = i;
            tilemap.SetColor(pos, c);
        }*/
        yield return new WaitForSeconds(time);
        Color cc = Dic[t].color;
        cc.a = 0;
        tilemap.SetColor(pos,cc);
        //tilemap.SetTile(pos, Dic[t]);
        Debug.Log("fade");
        yield return new WaitForSeconds(time*10);
        //StartCoroutine(show(pos));
    }
    
    void makeDic()
    {
        string dicText = DicText.text;
        string[] pairs = dicText.Split('\n');
        foreach (string str in pairs)
        {
            string[] pair = str.Split(',');
            Tile tile = Resources.Load("Tiles/"+pair[1], typeof(Tile)) as Tile;
            
            //Color c = new Color(); c.a = 0.0f;
            //tile.color=c;
            //Dic.Add(int.Parse(pair[0]), "Tiles/" + pair[1]);
            Dic.Add(int.Parse(pair[0]), tile);
        }
    }
    void makeMap(Tile tile,int key)
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
                if (Map[j, i] ==key)
                {
                    //string path =Dic[key];
                    //tile= Resources.Load(path, typeof(Tile)) as Tile;
                    //if (tile) Debug.Log("2:"+tile.color.a);
                    tilemap.SetTile(new Vector3Int((i - width / 2), height / 2 - j-1, 0), tile);
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
            if (tb == null)//||Map[height / 2 - realCellPosition.y-1, realCellPosition.x + width / 2] !=1)
            {
                return;
            }
            //Debug.Log("succeed erase");
            //tb.hideFlags = HideFlags.None;
            tilemap.SetTile(realCellPosition, null);
            //tilemap.RefreshAllTiles();
        }
        //
        if (Input.GetKeyDown(KeyCode.M)&&this.tag!="GreenMap")
        {
            Vector3 realCellPosition = cellPosition + new Vector3(0.5f, 0.5f, 0);
            Vector3Int realCellPosition2 = cellPosition + new Vector3Int(0, -1, 0);
            tilemap.SetTileFlags(realCellPosition2,TileFlags.None);
            //tilemap.SetColor(realCellPosition2, Color.red);
            StartCoroutine(fade(realCellPosition2));
            //tilemap.SetTile(realCellPosition2, 
            //  Dic[Map[height / 2 - realCellPosition2.y - 1, realCellPosition2.x + width / 2]]);
            //Debug.Log(Dic[Map[height / 2 - realCellPosition2.y - 1, realCellPosition2.x + width / 2]].color.a);
            GameObject newGrass = Instantiate(grass, realCellPosition,Quaternion.identity);
        }
    }
    private void OnDisable()
    {
        string dicText = DicText.text;
        string[] pairs = dicText.Split('\n');
        foreach (string str in pairs)
        {
            string[] pair = str.Split(',');
            Tile tile = Resources.Load("Tiles/" + pair[1], typeof(Tile)) as Tile;
            //Debug.Log(tile.sprite);
            Color c = tile.color;
            c.a = 1;
            tile.color=c;
        }
    }
}
