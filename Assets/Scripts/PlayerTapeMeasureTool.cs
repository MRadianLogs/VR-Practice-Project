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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        //If both buttons on both controllers are held down,
        if(OVRInput.Get(OVRInput.RawButton.LIndexTrigger) 
            && OVRInput.Get(OVRInput.RawButton.Y)
            && OVRInput.Get(OVRInput.RawButton.RIndexTrigger) 
            && OVRInput.Get(OVRInput.RawButton.B))
        {
            //Tapemeasure object should be generated.
            Debug.Log("Creating Tapemeasure!");
            //Should I use a coroutine that is started here? That way the if condition above activates it? Then the coroutine can have its own if to ensure that one hand always has the tape measure.
        }
    }

    //Create a 3d object? Or just draw a line?
    //Either way, I need to calculate the distance between the hands/tapemeasure points, and display it.
    IEnumerator ActivateTapeMeasure()
    {
        bool alreadyActive = false;
        while((OVRInput.Get(OVRInput.RawButton.LIndexTrigger)
            && OVRInput.Get(OVRInput.RawButton.Y))
            || (OVRInput.Get(OVRInput.RawButton.RIndexTrigger)
            && OVRInput.Get(OVRInput.RawButton.B))) //While either hand has "pinching" buttons held, keep this coroutine active.
        {
            if (alreadyActive == true) //If the tapemeasure is already active,
            {
                //No need to recreate tape measure objects.
                //Calculate distance between the two objects.
            }
            else //Tape measure objects need to be generated.
            {
                //Generate tape measure objects at hands.
                alreadyActive = true;
            }
        }
        alreadyActive = false;
        StopCoroutine(ActivateTapeMeasure()); //Tape measure should disappear.
    }

    private void FixedUpdate()
    {
        OVRInput.FixedUpdate();
    }
}
