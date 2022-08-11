using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class petMoveToFood : MonoBehaviour
{
    private Animator petAnim;

    public GameObject Food;

    public float speed = 1f;

    void Start()
    {
       petAnim = GetComponent<Animator>();
       
    }

    // Move to the target end position.

    ////not used
    public void MovetoFood()
    {  
        if (Food.activeSelf)
        {
            transform.position = Vector3.MoveTowards (transform.position, Food.transform.position, speed*Time.deltaTime);

            var lookPos = Food.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
        }
       
    }

    }

