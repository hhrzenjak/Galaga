using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyBossLifeManager : LifeManager
{

    public static UnityEvent BossKilledEvent = new UnityEvent();

    public int Points;
    public Image healthBar;

    private bool _dead;
    
    public Transform ShieldPrefab;

    public float shieldTimer = 6f;
    public bool shieldOn;
    private float _shieldTimer = 6f;
    private Transform _shield;
    public bool startShield = false;

    private void Update()
    {
        if (startShield)
        {
            _shieldTimer -= Time.deltaTime;
            if (_shieldTimer < 0)
            {
                SwitchShield();
            } 
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<LifeManager>())
                other.GetComponent<LifeManager>().Hit();
            Hit();
        }

//        if (other.CompareTag("Screen"))
//            _active = true;

    }

    public override void Hit()
    {

        if (_active == false)
            return;

        _lives--;
        Shake();
        currentHealthPercentage = (float)_lives / MaximumLives;
        SetHealthBar();

        if (_dead == false && _lives <= 0)
        {

            _dead = true;

            if (GameManager.GM)
                GameManager.GM.AddToScore(Points);

            BossKilledEvent.Invoke();
            Destroy(gameObject);

        }

    }

    public void SetHealthBar()
    {
        healthBar.rectTransform.localScale = new Vector3(currentHealthPercentage, healthBar.rectTransform.localScale.y, healthBar.rectTransform.localScale.z);
        Color color = Color.Lerp(Color.red, Color.green, currentHealthPercentage);
        healthBar.color = color;
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
        _shieldTimer = shieldTimer + 7f;
        
        if (_shield == null)
            return;

        Destroy(_shield.gameObject);
    }

    private void SwitchShieldOn()
    {
        shieldOn = true;
        _shield = Instantiate(ShieldPrefab, transform);
        Deactivate();
        _shieldTimer = shieldTimer;
    }

}
