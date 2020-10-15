using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileAnimator : MonoBehaviour
{
    //public Tilemap tilemap;
    Tilemap tilemap;
    public float gapTime=0.1f;
    public Tile[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    IEnumerator StartAnim(Vector3Int pos)
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            tilemap.SetTile(pos,sprites[i]);
            yield return new WaitForSeconds(gapTime);
        }
        yield return true;

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 wordPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(wordPosition);
            StartCoroutine(StartAnim(cellPosition));
        }
    }
}
