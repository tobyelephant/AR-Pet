using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class petMoveTo : MonoBehaviour
{
    // Transforms to act as start and end markers for the journey.
    public Transform startMarker;
    public Vector3 endMarker;

    // Movement speed in units per second.
    private float speed = 0.2F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    private Animator petAnim;

    void Start()
    {
       journeyLength = 0;
       petAnim = GetComponent<Animator>();
    }

    // Move to the target end position.
    void Update()
    {
        // Distance moved equals elapsed time times speed..
        if(journeyLength > 0) {
            float distCovered = (Time.time - startTime) * speed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(startMarker.position, endMarker, fractionOfJourney);

            if(fractionOfJourney<0.1) {
            var lookPos = endMarker - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
            }
        }

        if(Vector3.Distance(startMarker.position, endMarker) <0.1){
            petAnim.SetBool("isRunning", false);
        }
    }

    public void StartMove(Vector3 endPos)
    {
         // Keep a note of the time the movement started.
         petAnim.SetBool("isRunning", true);
        startMarker = this.transform;
        endMarker = endPos;
        startTime = Time.time;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(startMarker.position, endMarker);
        Debug.Log("journey length is " + journeyLength);
    }
}
