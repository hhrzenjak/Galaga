using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PredeterminedEnemyShooting : EnemyShooting
{

    public EnemyShootingDirections ShootingDirections;

    private Transform _transform;

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

    }

    #endregion

    private void Awake()
    {
        _transform = transform;
    }

    public override void Shoot()
    {

        if (LaserBeamPrefab == null)
            return;
        
        foreach (Vector2 direction in ShootingDirections.ShootingDirections)
        {

            Transform laserBeam = Instantiate(LaserBeamPrefab, _transform.position, Quaternion.identity);

            if (laserBeam.GetComponent<LaserBeam>())
                laserBeam.GetComponent<LaserBeam>().Launch(direction);

        }

    }

}
