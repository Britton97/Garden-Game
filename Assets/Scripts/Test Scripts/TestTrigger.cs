using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrigger : MonoBehaviour
{
    //trigger2d
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered");
    }
}
