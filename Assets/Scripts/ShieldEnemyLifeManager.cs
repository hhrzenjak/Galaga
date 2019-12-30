using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemyLifeManager : EnemyLifeManager
{

    public Transform ShieldPrefab;
   
    public float shieldTimer = 8.5f;
    public bool shieldOn;

    private float _shieldTimer = 8.5f;
    private Transform _shield;


    private void Update()
    {

        _shieldTimer -= Time.deltaTime;
        if (_shieldTimer < 0)
        {
            SwitchShield();
        }
    }
    
    
    private void SwitchShield()
    {
        if (shieldOn)
        {
            SwitchShieldOff();
        }
        else
        {
            SwitchShieldOn();
        }
    }

    private void SwitchShieldOff()
    {
        shieldOn = false;

        Activate();
        _shieldTimer = shieldTimer;
        
        if (_shield == null)
            return;

        Destroy(_shield.gameObject);
    }

    private void SwitchShieldOn()
    {
        shieldOn = true;
        Deactivate();
        _shield = Instantiate(ShieldPrefab, transform);
        _shieldTimer = shieldTimer;
    }
}
