using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) return;
        if(col.CompareTag("Enemy"))
            Debug.Log($"HIT {col.tag}");
    }
}
