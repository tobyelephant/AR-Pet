using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliderDetect : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject Food;
    // Update is called once per frame
    
    private void OnTriggerEnter(Collider other)
    {
        Food.SetActive(false);
    }
}
