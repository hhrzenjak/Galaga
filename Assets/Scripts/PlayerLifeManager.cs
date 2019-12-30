using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class PlayerLifeManager : LifeManager
{

    public static UnityEvent GameOverEvent = new UnityEvent();
    public static UnityEvent PlayerLifeLostEvent = new UnityEvent();
    public static UnityEvent PlayerRespawnEvent = new UnityEvent();

    [Header("Life Display Data")]

    public LifeDisplay LifeDisplay;

    public Transform deathParticles;

    private void Awake()
    {

        if (GameManager.GM != null && GameManager.GM.PlayerData != null)
            _lives = GameManager.GM.PlayerData.PlayerLives;
        else
            _lives = MaximumLives;
        
    }

    private void Start()
    {

        if (LifeDisplay)
        {
            LifeDisplay.SetUpLifeDisplay(MaximumLives);
            LifeDisplay.RefreshLifeDisplay(_lives);
        }

        PickUp.PickUpCollectedEvent.AddListener(PickUpHandler);

    }

    private void PickUpHandler(PickUpType type)
    {

        if (type == PickUpType.LIFE)
            _lives = _lives < MaximumLives ? _lives + 1 : _lives;

        if (type == PickUpType.LIFE_MAXIMUM)
            _lives = MaximumLives;

        if (LifeDisplay)
            LifeDisplay.RefreshLifeDisplay(_lives);

    }

    public override void Hit()
    {

        if (!_active)
            return;
        
        _lives--;
        Shake();
        Instantiate(deathParticles, transform.position, Quaternion.identity);


        if (LifeDisplay)
            LifeDisplay.RefreshLifeDisplay(_lives);

        if (_lives <= 0)
        {
            GameOverEvent.Invoke();
            Destroy(gameObject);
        }

        else
        {
            PlayerLifeLostEvent.Invoke();
            PlayerRespawnEvent.Invoke();
        }
        
        
        

    }
    
    public int GetLives()
    {
        return _lives;
    }

}
