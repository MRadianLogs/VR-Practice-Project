using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapeMeasureUIBehavior : MonoBehaviour
{
    [SerializeField]
    private Text measuredDistanceText;

    public void SetMeasuredDistanceText(float newMeasuredDistance)
    {
        measuredDistanceText.text = newMeasuredDistance.ToString() + " meters";
    }
}
