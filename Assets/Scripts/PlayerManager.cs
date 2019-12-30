using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public float EnterSpeed;
    public float ExitSpeed;

    public Transform ShieldPrefab;

    public Vector2 StartingPosition;

    private bool _respawning;

    private Transform _shield;

    private PlayerAction _playerAction;
    private PlayerLifeManager _playerLifeManager;
    private PlayerMovement _playerMovement;

    private float shieldTimer = 1.5f;
    private bool _shieldUp = false;

    private Transform _transform;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {

        if (StageManager.SM)
            StageManager.SM.StageClearedEvent.AddListener(StageCleared);

        _playerAction = GetComponent<PlayerAction>();
        _playerLifeManager = GetComponent<PlayerLifeManager>();
        _playerMovement = GetComponent<PlayerMovement>();

        _respawning = false;

        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();

        PlayerLifeManager.PlayerRespawnEvent.AddListener(Respawn);

    }

    private void Start()
    {
        Spawn();
    }

    private void Update()
    {

        if (_respawning && _transform.position.y >= StartingPosition.y)
        {
            _respawning = false;

            _rigidbody.velocity = Vector2.zero;
            _transform.position = new Vector2(_transform.position.x, StartingPosition.y);
            
            if (_playerAction)
                _playerAction.Activate();
            
            if (_playerMovement)
                _playerMovement.Activate();

        }

        if (shieldTimer < 0 && _shieldUp)
        {
            
            Destroy(_shield.gameObject);
            if (_playerLifeManager)
                _playerLifeManager.Activate();
            _shieldUp = false;
        }

        if(_shieldUp)
            shieldTimer -= Time.deltaTime;

    }

    private void Respawn()
    {
        gameObject.SetActive(false);
        Invoke("Spawn", 0.5f);
    }

    private void Spawn()
    {
        gameObject.SetActive(true);

        if (_respawning)
            return;

        if (_playerAction)
            _playerAction.Deactivate();

        if (_playerLifeManager)
            _playerLifeManager.Deactivate();

        if (_playerMovement)
            _playerMovement.Deactivate();

        if (ShieldPrefab)
        {
            if (!_shieldUp)
            {
                _shield = Instantiate(ShieldPrefab, _transform);
                shieldTimer = 2f;
                _shieldUp = true;
            }

        }

        if (_rigidbody)
        {

            _respawning = true;
            _transform.position = StartingPosition - new Vector2(0, 10);

            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(Vector2.up * EnterSpeed);

        }

        else
            _transform.position = StartingPosition;

    }

    private void StageCleared()
    {

        if (GameManager.GM)
            GameManager.GM.SavePlayerData(_playerLifeManager.GetLives(), _playerAction.GetShootingLevel());

        if (_playerAction)
            Destroy(_playerAction);

        if (_playerLifeManager)
            Destroy(_playerLifeManager);

        if (_playerMovement)
            Destroy(_playerMovement);

        if (ShieldPrefab)
            _shield = Instantiate(ShieldPrefab, _transform);

        if (_rigidbody)
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(Vector2.up * ExitSpeed);
        }

    }

}
