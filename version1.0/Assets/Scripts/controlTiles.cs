using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class controlTiles : MonoBehaviour
{
    public GameObject character;
    public float pauseTime = 0.003f;
    [System.Serializable]
    public class pos
    {
        public int x;
        public int y;
    };
    [System.Serializable]
    public class movingTiles
    {
        public GameObject tile;
        public List<pos> Pos;
    };
    public GameObject controller;
    public List<movingTiles> tiles;
    int[] hasMoved;
    Vector3 playerPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = character.transform.position;
        detect();
    }
    IEnumerator moving()
    {
        Vector3 originPos = tiles[0].tile.transform.position;

        for (int i = 0; i < tiles[0].Pos.ToArray().Length; i++)
        {
            for (float j = 0; j < 1.0f; j += Time.deltaTime)
            {
                yield return new WaitForSeconds(pauseTime);
                tiles[0].tile.transform.position = originPos +
                    new Vector3(tiles[0].Pos[i].x * j, tiles[0].Pos[i].y * j, 0);
                //Debug.Log(transform.position);
            }
            originPos = tiles[0].tile.transform.position;
            yield return new WaitForSeconds(pauseTime);
        }
    }
    //private void OnCollisionStay2D(Collision2D collision)
    void detect()
    {
        float dis = (playerPos - controller.transform.position).magnitude;
        float min = 1.5f;
        //Debug.Log(playerPos + " " + controller.transform.position+" "+dis);
        if (dis<min)//if (collision.collider.tag == "Player" )
        {
            //Debug.Log("collid");
            if (Input.GetKeyDown(KeyCode.F))
            {
                //Debug.Log("F");
                StartCoroutine(moving());
            }
            //hasMoved = 1;
        }
    }
}
