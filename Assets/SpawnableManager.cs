using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpawnableManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    ARRaycastManager m_RaycastManager; //create a raycast manager
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    [SerializeField]
    GameObject spawnablePrefab;
    public GameObject foodPrefab;
    Camera arCam;
    public GameObject spawnedObject;
    public int numberOfPetsAllowed = 1;
    private int currentNumberOfCats  = 0;
    public Animator petAnim;
    public ARSessionOrigin ar_session_origin;
    public GameObject Food;
    public float speed = 0.3f;


    void Start()
    {
        spawnedObject = null;
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.touchCount == 0)
            return;


        RaycastHit hit;
        Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);

        if (m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hits)) // Only returns true if there is at least one hit
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && spawnedObject == null) // when first touch is detected and no spawned object is created
            {
                if (Physics.Raycast(ray, out hit)) // // Does the ray intersect any objects excluding the player layer
                {
                    if (hit.collider.gameObject.tag == "Spawnable") // if hit object is spawnable object
                    {
                        spawnedObject = hit.collider.gameObject;

                    }
                    else{
                        SpawnPrefab(m_Hits[0].pose.position); //instantiate the prefab and assign that object to our spawnedobject variable.
                        foreach(var plane in ar_session_origin.GetComponent<ARPlaneManager>().trackables){
                            plane.gameObject.SetActive(false);
                            }
                        ar_session_origin.GetComponent<ARPlaneManager>().enabled = false;
                    }
                }
            }

            else if(Input.GetTouch(0).phase == TouchPhase.Moved&&spawnedObject != null)//Determine if the touch is a moving touch
            {
                // spawnedObject.transform.position = m_Hits[0].pose.position;

                spawnedObject.GetComponent<petMoveTo>().StartMove(m_Hits[0].pose.position);

                petAnim = spawnedObject.GetComponent<Animator>();

                if (Input.touchCount == 2 && petAnim.GetCurrentAnimatorStateInfo(0).IsName("Fish_Armature|Swimming_Normal"))
                 {
                   petAnim.Play("Fish_Armature|Death");
                 }

                else if (Input.touchCount == 3 && petAnim.GetCurrentAnimatorStateInfo(0).IsName("Fish_Armature|Swimming_Normal"))
                {
                    petAnim.Play("Fish_Armature|Out_Of_Water");
                }

                else if (Input.touchCount == 4)
                {
                    foodSpawn(m_Hits[0].pose.position);
                    
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
            

            // if(Input.GetTouch(0).phase == TouchPhase.Ended) //the touch has ended when it ends
            // {
            //     spawnedObject = null;
            // }
        }
    }

    private void SpawnPrefab(Vector3 spawnPosition)
    {
        if(currentNumberOfCats < numberOfPetsAllowed){
            currentNumberOfCats = currentNumberOfCats +1;
            spawnedObject = Instantiate(spawnablePrefab, spawnPosition, Quaternion.identity);
            spawnedObject.transform.LookAt(arCam.transform);
            spawnedObject.transform.rotation = Quaternion.Euler(0.0f,
            spawnedObject.transform.rotation.eulerAngles.y, spawnedObject.transform.rotation.z);
        }
        
    }

    private void foodSpawn(Vector3 foodPosition)
    {
        Food = Instantiate(foodPrefab, foodPosition, Quaternion.identity);
    }

}
