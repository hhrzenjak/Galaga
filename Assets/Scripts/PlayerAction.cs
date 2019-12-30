using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{

    public InGameUIManager UserInterfaceManager;

    [Header("Shield Data")]

    public Transform ShieldPrefab;

    [Header("Shooting Data")]

    public Transform LaserBeamPrefab;
    public float ShootingTimeStep;
    public float ShootingWaitTime;

    [Header("Overheat Data")]

    public Slider OverheatSlider;
    public Image OverheatSliderFill;
    public float OverheatTimeStep;

    private Dictionary<int, List<Vector2>> _laserBeamDirections = new Dictionary<int, List<Vector2>>()
    {
        { 0, new List<Vector2> { new Vector2(0.00f, 1)}},
        { 1, new List<Vector2> { new Vector2(-0.10f, 1), new Vector2(0.10f, 1)}},
        { 2, new List<Vector2> { new Vector2(-0.20f, 1), new Vector2(0.00f, 1), new Vector2(0.20f, 1)}},
        { 3, new List<Vector2> { new Vector2(-0.30f, 1), new Vector2(-0.10f, 1), new Vector2(0.10f, 1), new Vector2(0.30f, 1)}},
    };

    private bool _active;

    private PlayerLifeManager _lifeManager;
    private Transform _shield;

    private bool _overheat;
    private float _overheatPoints;
    private float _overheatTimer;

    private bool _shooting;
    private int _shootingLevel;
    private float _shootingTimer;
    private float _shootingWait;

    private Transform _transform;

    private void Awake()
    {

        if (GameManager.GM != null && GameManager.GM.PlayerData != null)
            _shootingLevel = GameManager.GM.PlayerData.PlayerShootingLevel;
        else
            _shootingLevel = 0;

        _active = true;

        _lifeManager = GetComponent<PlayerLifeManager>();
        _transform = transform;

        if (InputManager.IM)
            InputManager.IM.ButtonInputEvent.AddListener(InputHandler);

        PickUp.PickUpCollectedEvent.AddListener(PickUpHandler);
        PlayerLifeManager.PlayerLifeLostEvent.AddListener(ResetLaser);

    }

    private void Update()
    {

        if (_overheat == false && _shield)
        {
            _overheatPoints += 0.5f;
            UpdateOverheatData();
            return;

        }

        if (_overheat == false && _shooting)
        {

            if (_shootingLevel == 0)
                _overheatPoints += 0.3f;
            else
                _overheatPoints += (0.25f * Mathf.Sqrt(_shootingLevel + 1));
            
            _shootingTimer += Time.deltaTime;

            if (_shootingTimer > ShootingTimeStep)
            {
                _shootingTimer = 0;
                Shoot();
            }

            UpdateOverheatData();
            return;

        }

        _overheatTimer += Time.deltaTime;

        if (_overheatTimer > OverheatTimeStep)
        {

            _overheatPoints -= 1.00f;
            _overheatTimer = 0;

            UpdateOverheatData();

        }

    }

    private void InputHandler(InputType input)
    {

        if (GameManager.GM && GameManager.GM.Paused)
            return;

        if (_active == false)
            return;

        if (input == InputType.RELEASED_SHIELD)
            DeactivateShield();

        if (input == InputType.RELEASED_SHOOT)
            StopShooting();

        if (input == InputType.SHIELD)
            ActivateShield();

        if (input == InputType.SHOOT)
            StartShooting();

    }

    private void PickUpHandler(PickUpType type)
    {
        if (type == PickUpType.LASER)
            BoostShooting();
    }

    public void Activate()
    {
        _active = true;
    }

    public void Deactivate()
    {

        _active = false;

        DeactivateShield();
        StopShooting();

        _overheatPoints = 0;
        UpdateOverheatData();

    }

    public int GetShootingLevel()
    {
        return _shootingLevel;
    }

    #region METHODS FOR SHIELD

    public void ActivateShield()
    {

        if (_overheat || _shield || _shooting)
            return;

        _shield = Instantiate(ShieldPrefab, _transform);

        if (_lifeManager)
            _lifeManager.Deactivate();

    }

    private void DeactivateShield()
    {

        if (_shield == null)
            return;

        Destroy(_shield.gameObject);

        if (_lifeManager)
            _lifeManager.Activate();

    }


    #endregion

    #region METHODS FOR SHOOTING

    private void BoostShooting()
    {
        _shootingLevel = _shootingLevel < _laserBeamDirections.Count - 1 ? _shootingLevel + 1 : _shootingLevel;
    }

    private void ResetLaser()
    {
        _shootingLevel = 0;
    }

    private void Shoot()
    {

        foreach (Vector2 direction in _laserBeamDirections[_shootingLevel])
        {

            Transform laserBeam = Instantiate(LaserBeamPrefab, _transform.position, Quaternion.identity);

            if (laserBeam.GetComponent<LaserBeam>())
                laserBeam.GetComponent<LaserBeam>().Launch(direction);

        }

    }

    private void StartShooting()
    {

        if (_overheat || _shooting || _shield)
            return;

        _shooting = true;
        Shoot();

    }

    private void StopShooting()
    {
        _shooting = false;
    }

    #endregion

    #region METHODS FOR OVERHEAT

    private void ResetOverheatData()
    {

        if (OverheatSlider == null || OverheatSliderFill == null)
            return;

        if (UserInterfaceManager)
            UserInterfaceManager.TurnOffOverheatScreen();

        OverheatSlider.value = 0;
        OverheatSliderFill.color = Color.white;

        _overheat = false;
        _overheatPoints = 0;

    }

    private void UpdateOverheatData()
    {

        if (OverheatSlider == null || OverheatSliderFill == null)
            return;

        if (_overheatPoints <= 0)
            ResetOverheatData();

        else if (_overheatPoints > 0 && _overheatPoints < 100)
            OverheatSlider.value = _overheatPoints / 100;

        else
        {

            if (UserInterfaceManager)
                UserInterfaceManager.TurnOnOverheatScreen();

            if (_shield)
                DeactivateShield();

            if (_shooting)
                StopShooting();

            OverheatSlider.value = 1;
            OverheatSliderFill.color = Color.red;

            _overheat = true;
            _overheatPoints = 100;

        }

    }

    #endregion

}
