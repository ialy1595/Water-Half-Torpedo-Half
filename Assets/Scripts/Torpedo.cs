using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
    public static List<Torpedo> TorpedoList = new List<Torpedo>();
    
    public GameObject DetectObject;

    public float torpedoMoveSpeed = 0;

    private float torpedoY;


    // Start is called before the first frame update
    void Start()
    {
        torpedoY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        torpedoY -= torpedoMoveSpeed;
        transform.position = (new Vector3(transform.position.x, torpedoY, transform.position.z));
    }
}
