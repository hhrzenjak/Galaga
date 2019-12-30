using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TargetEnemyShooting : EnemyShooting
{

    public Transform Target;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
        if (Target == null)
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    #region DELETE ?

    private float _timer; 

    private void Start()
    {
        _timer = 4.5f;
    }

    private void Update()
    {

        _timer -= Time.deltaTime;

        if (_timer < 0)
        {
            _timer = 4.5f;
            Shoot();
        }
        
        if (Target == null)
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
        }

    }

    #endregion

    public override void Shoot()
    {

        if (LaserBeamPrefab == null || Target == null)
            return;

        Transform laserBeam = Instantiate(LaserBeamPrefab, _transform.position, Quaternion.identity);

        if (laserBeam.GetComponent<LaserBeam>())
            laserBeam.GetComponent<LaserBeam>().Launch(Target.position - _transform.position);

    }
}
