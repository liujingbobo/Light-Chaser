using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingTiles : MonoBehaviour
{
    public GameObject tile;
    public float pauseTime = 0.003f;
    [System.Serializable]
    public class pos
    {
        public int x;
        public int y;
    };
    public List<pos> postions;
    int hasMoved = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator moving()
    {
        Vector3 originPos = transform.position;
        
        for (int i = 0; i < postions.ToArray().Length; i++)
        {
            for (float j = 0; j < 1.0f; j += Time.deltaTime)
            {
                yield return new WaitForSeconds(pauseTime);
                transform.position = originPos +
                    new Vector3(postions[i].x * j, postions[i].y * j, 0);
                //Debug.Log(transform.position);
            }
            originPos = transform.position;
            yield return new WaitForSeconds(pauseTime); 
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player"&&hasMoved==0)
        {
            StartCoroutine(moving());
            hasMoved = 1;
        }
    }
}
