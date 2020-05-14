using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Controller : MonoBehaviour
{
    public GameObject Water_Indicator;
    [Range(0,10)]public float Water_Level;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Water_Indicator.transform.position = new Vector3(0,Water_Level,0);
    }
}
