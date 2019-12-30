using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class EnemyShooting : MonoBehaviour
{

    public Transform LaserBeamPrefab;

    public abstract void Shoot();

}
