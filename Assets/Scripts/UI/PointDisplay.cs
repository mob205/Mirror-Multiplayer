using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointDisplay : MonoBehaviour
{
    [HideInInspector] public uint target;

    private TextMeshProUGUI text;
    private PointTracker tracker;
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        tracker = FindObjectOfType<PointTracker>();
    }
    private void Update()
    {
        if (target != 0 && tracker)
        {
            text.text = tracker.GetPoints(target).ToString();
        }
    }
}
