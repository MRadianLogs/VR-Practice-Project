using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Key Differences from V1:
 *  1. Have the user place down 2(4?) points using a pointer.
 *      - Calc the distance between points and display to screen(not tapemeasure object).
 *  2. Will only require 1 button to activate
 *      - Eventually activated using wrist button
 *  3. Performance/Bugs: No coroutine will be used. This should allow the use of other project functions without stealing controls.
 */


/* IDEAS:
 *  1. Have ammo count displayed for number of points placed out of total.
 * 
 */
public class PlayerTapeMeasureToolV2 : MonoBehaviour
{
    [SerializeField]
    private OVRInput.RawButton activationButton = OVRInput.RawButton.B; //Change this to whatever button we want to activate the tapemeasure.
    [SerializeField]
    private OVRInput.RawButton placeTapeMeasurePointButton = OVRInput.RawButton.RIndexTrigger; //Change this to whatever button we want to use to place tapemeasuring points. Distance will be measured between these.

    private bool isActive = false;
    [SerializeField]
    private GameObject testActiveObject;

    [SerializeField]
    private int maxNumTapeMeasurePoints = 2;
    private int currentNumTapeMeasurePoints = 0;
    [SerializeField]
    private GameObject tapeMeasurePointPrefab;
    private GameObject[] tapeMeasurePoints;

    [SerializeField]
    private GameObject tapeMeasureTapePrefab;
    private int currentNumTapeMeasureTapes = 0;
    private GameObject[] tapeMeasureTapes;

    [SerializeField]
    private GameObject laserPointer;
    [SerializeField]
    private GameObject laserPointerPoint;

    private float measuredDistance = 0f; //The cumulative measured distance between all placed points.
    [SerializeField]
    private GameObject tapeMeasureUICanvas;

    private void Start()
    {
        tapeMeasurePoints = new GameObject[maxNumTapeMeasurePoints];
        tapeMeasureTapes = new GameObject[maxNumTapeMeasurePoints-1];
    }

    // Update is called once per frame
    void Update()
    {
        //If activation button is pressed.
        if (OVRInput.GetDown(activationButton))
        {
            if(isActive == true) //What to do if the tapemeasure is already active. Disable it.
            {
                //Tapemeasure object should be disabled.
                Debug.Log("Turning off tapemeasure...");
                testActiveObject.SetActive(false);
                DeactivateTapeMeasure();
                isActive = false;
            }
            else //Activate it.
            {
                //Tapemeasure object should be generated.
                Debug.Log("Creating tapemeasure!");
                testActiveObject.SetActive(true);
                ActivateTapeMeasure();
                isActive = true;
            }
        }
        //If the tapemeasure is active, enable placing tapemeasure points.
        if(isActive && OVRInput.GetDown(placeTapeMeasurePointButton)) //If placing button pressed,
        {
            if (currentNumTapeMeasurePoints < maxNumTapeMeasurePoints) //If not max number of points placed,
            {
                //Place tapemeasure point at straight line from hand. (Use pointer line.)
                //TODO Instantiate prefab at laser pointer end and add to list at [currentNumTapeMeasurePoints].
                tapeMeasurePoints[currentNumTapeMeasurePoints] = Instantiate(tapeMeasurePointPrefab, laserPointer.GetComponent<LineRenderer>().GetPosition(1), laserPointer.transform.rotation);
                currentNumTapeMeasurePoints++;
                if (currentNumTapeMeasurePoints >= 2) //If there is at least 2 points to measure the distance between and draws lines.
                {
                    //Instantiate line between the points.
                    tapeMeasureTapes[currentNumTapeMeasureTapes] = Instantiate(tapeMeasureTapePrefab, tapeMeasurePoints[currentNumTapeMeasurePoints - 1].transform.position, tapeMeasurePoints[currentNumTapeMeasurePoints - 1].transform.rotation);
                    tapeMeasureTapes[currentNumTapeMeasureTapes].GetComponent<LineRenderer>().SetPosition(0, tapeMeasurePoints[currentNumTapeMeasurePoints - 1].transform.position);
                    tapeMeasureTapes[currentNumTapeMeasureTapes].GetComponent<LineRenderer>().SetPosition(1, tapeMeasurePoints[currentNumTapeMeasurePoints - 2].transform.position);
                    currentNumTapeMeasureTapes++;
                    //Calculate the new cumulative measured distance between new point and last point.
                    measuredDistance += Vector2.Distance(tapeMeasurePoints[currentNumTapeMeasurePoints - 1].transform.position, tapeMeasurePoints[currentNumTapeMeasurePoints - 2].transform.position);
                    tapeMeasureUICanvas.GetComponent<TapeMeasureUIBehavior>().SetMeasuredDistanceText(measuredDistance);
                }
            }
            else //Max points placed. Remove all points and lines.
            {
                RemoveAllTapeMeasurePoints();
                RemoveAllTapeMeasureLines();
            }

        }
    }

    private void ActivateTapeMeasure()
    {
        //Enable pointer.
        laserPointer.SetActive(true);
        //Enable UI for displaying measurements.
        tapeMeasureUICanvas.SetActive(true);
    }

    private void DeactivateTapeMeasure()
    {
        //Disable pointer.
        laserPointer.SetActive(false);
        //Disable UI for displaying measurements.
        tapeMeasureUICanvas.SetActive(false);
        //Remove any tapemeasure points, and lines between them.
        RemoveAllTapeMeasurePoints();
        RemoveAllTapeMeasureLines();
    }

    private void RemoveAllTapeMeasurePoints()
    {
        foreach(GameObject point in tapeMeasurePoints) //TODO Make sure this doesnt error.
        {
            if(point != null)
            {
                Destroy(point);
            }
        }
        currentNumTapeMeasurePoints = 0;
        //Resets measured distance.
        measuredDistance = 0f;
        tapeMeasureUICanvas.GetComponent<TapeMeasureUIBehavior>().SetMeasuredDistanceText(measuredDistance); //TODO Might break.
    }
    private void RemoveAllTapeMeasureLines() //TODO Make sure this doesnt error.
    {
        foreach (GameObject tape in tapeMeasureTapes)
        {
            if (tape != null)
            {
                Destroy(tape);
            }
        }
        currentNumTapeMeasureTapes = 0;
    }
}
