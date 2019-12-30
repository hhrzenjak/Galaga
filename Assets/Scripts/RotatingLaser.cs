using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingLaser : LaserBeam
{
    // Update is called once per frame
    void Update()
    {
       transform.Rotate(0,0,20*Time.deltaTime);
    }
}
