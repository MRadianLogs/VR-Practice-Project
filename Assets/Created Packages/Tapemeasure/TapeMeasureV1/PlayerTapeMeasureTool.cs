using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script will be used to allow a vr user to create a tapemeasuring tool, 
 * using Oculus controllers, in order to measure things in the vr simulation. 
 * 
 *      How to setup script: 
 * 
 * 
 *      How to use function: 
 * The player will hold down ... (Pinch both fingers on 
 * both controllers to generate the tapemeasure. The user may release one hand 
 * to leave a floating measure point. Their other hand will then be the moveable 
 * other measuring point. As soon as the points are generated, measuring will 
 * be done and displayed on the screen.)
 */
public class PlayerTapeMeasureTool : MonoBehaviour
{
    [SerializeField]
    private GameObject tapeMeasurePointPrefab = null;
    [SerializeField]
    private GameObject tapeMeasureDistanceUIPrefab = null;
    [SerializeField]
    private GameObject tapeMeasureTapePrefab = null;

    [SerializeField]
    private GameObject leftHandAnchor = null;
    [SerializeField]
    private GameObject rightHandAnchor = null;
    [SerializeField]
    private GameObject centerEyeAnchor = null;

    private bool isActive = false;

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        if (!isActive) //Only checks for this if there isnt already a tapemeasure active. Otherwise there would be n many tapemeasure coroutines, where n is the number of frames that pass... 
        {
            //If both buttons on both controllers are held down,
            if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger)
                && OVRInput.Get(OVRInput.RawButton.Y)
                && OVRInput.Get(OVRInput.RawButton.RIndexTrigger)
                && OVRInput.Get(OVRInput.RawButton.B))
            {
                //Tapemeasure object should be generated.
                Debug.Log("Creating Tapemeasure!");
                //Should I use a coroutine that is started here? That way the if condition above activates it? Then the coroutine can have its own if to ensure that one hand always has the tape measure.
                StartCoroutine(ActivateTapeMeasure());
            }
        }
    }

    //Create a 3d object? Or just draw a line?
    //Either way, I need to calculate the distance between the hands/tapemeasure points, and display it.
    IEnumerator ActivateTapeMeasure()
    {
        bool alreadyActive = false;
        GameObject tapeMeasurePointA = null; //Left hand.
        GameObject tapeMeasurePointB = null; //Right hand.
        GameObject tapeMeasureDistanceUI = null; //The UI that displays the measured distance of the tapemeasure.
        GameObject tapeMeasureTape = null; //The line for the tapemeasure.

        isActive = true;
        bool leftHandPinched = (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) && OVRInput.Get(OVRInput.RawButton.Y));
        bool rightHandPinched = (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && OVRInput.Get(OVRInput.RawButton.B));
        while (leftHandPinched || rightHandPinched) //While either hand has "pinching" buttons held, keep this coroutine active.
        {
            leftHandPinched = (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) && OVRInput.Get(OVRInput.RawButton.Y));
            rightHandPinched = (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && OVRInput.Get(OVRInput.RawButton.B));
            if (alreadyActive == true) //If the tapemeasure is already active,
            {
                //No need to recreate tape measure objects. Keep tapemeasure points at hands. Only at the hands being held down.
                if(leftHandPinched)
                {
                    tapeMeasurePointA.transform.position = leftHandAnchor.transform.position;
                    tapeMeasurePointA.transform.rotation = leftHandAnchor.transform.rotation;
                }
                if(rightHandPinched)
                {
                    tapeMeasurePointB.transform.position = rightHandAnchor.transform.position;
                    tapeMeasurePointB.transform.rotation = rightHandAnchor.transform.rotation;
                }
                tapeMeasureDistanceUI.transform.position = ((tapeMeasurePointA.transform.position + tapeMeasurePointB.transform.position) / 2);
                tapeMeasureDistanceUI.transform.rotation = leftHandAnchor.transform.rotation;                                                                                                 //TODO Need to change rotation to face camera.
                tapeMeasureTape.GetComponent<LineRenderer>().SetPosition(0, tapeMeasurePointA.transform.position);
                tapeMeasureTape.GetComponent<LineRenderer>().SetPosition(1, tapeMeasurePointB.transform.position);
                //Calculate distance between the two objects.
                float distance = Vector2.Distance(tapeMeasurePointA.transform.position, tapeMeasurePointB.transform.position);
                Debug.Log("Distance from tapemeasure: " + distance);
                tapeMeasureDistanceUI.GetComponent<TextMesh>().text = (distance.ToString() + "ft.");
            }
            else //Tape measure objects need to be generated.
            {
                //Generate tape measure objects at hands.
                tapeMeasurePointA = Instantiate(tapeMeasurePointPrefab, leftHandAnchor.transform.position, leftHandAnchor.transform.rotation);
                tapeMeasurePointB = Instantiate(tapeMeasurePointPrefab, rightHandAnchor.transform.position, rightHandAnchor.transform.rotation);
                //Create UI for distance display.
                tapeMeasureDistanceUI = Instantiate(tapeMeasureDistanceUIPrefab, ((tapeMeasurePointA.transform.position + tapeMeasurePointB.transform.position)/2), leftHandAnchor.transform.rotation);                    //TODO: Need to change the rotation to be facing the camera. Center eye anchor.
                tapeMeasureTape = Instantiate(tapeMeasureTapePrefab, ((tapeMeasurePointA.transform.position + tapeMeasurePointB.transform.position)/2), leftHandAnchor.transform.rotation);
                alreadyActive = true;
            }
            yield return null; //THIS NEEDS TO BE IN THE WHILE LOOP! CRASHES OTHERWISE.
        }
        alreadyActive = false;

        //Tape measure should disappear.
        Destroy(tapeMeasurePointA);
        Destroy(tapeMeasurePointB);
        Destroy(tapeMeasureDistanceUI);
        Destroy(tapeMeasureTape);
        isActive = false;
        StopCoroutine(ActivateTapeMeasure()); 
    }

    private void FixedUpdate()
    {
        OVRInput.FixedUpdate();
    }
}
