using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detected : MonoBehaviour
{
    private int life = 30;
    private int disappearing = 30;

    [HideInInspector] public Color detectedColor = new Color(1f, 1f, 0f);
    public bool disappearable = true;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(disappearable)
        {
            if(life >= 0)
            {
                life--;
                sr.color = detectedColor;
            }
            else if(disappearing >= 0)
            {
                disappearing--;
                Color tc = sr.color;
                sr.color = new Color(tc.r, tc.g, tc.b, (float)disappearing / 60f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
