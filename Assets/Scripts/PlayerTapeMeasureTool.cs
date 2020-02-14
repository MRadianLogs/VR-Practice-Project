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


        }
    }

    private void FixedUpdate()
    {
        OVRInput.FixedUpdate();
    }
}
