using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Player;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = new Vector3(Player.position.x , 0f , -10f);
    }
}
