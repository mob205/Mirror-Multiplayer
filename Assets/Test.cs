using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] int testNumber;
    void Start()
    {
        Debug.Log($"Successfully executed Test script. Number: {testNumber}");
    }
}
