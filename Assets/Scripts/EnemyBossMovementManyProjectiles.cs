using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemyBossMovementManyProjectiles : MonoBehaviour
{
    public EnemyShootingDirections ShootingDirections;

    public float StartingHeight;
    public float EndingHeight;

    public float Speed;
    public float timePassed = 1.5f;
    public Vector2 tempPosition;
    public BossState CurrentBossState;

    public Transform _transform;
    protected Rigidbody2D _rigidbody;

    public EnemyBossLifeManager bossLifeManager;

    public Transform projectile1;
    public Transform projectile2;
    public Transform projectile3;

    public float firstAttackTimer = 0.25f;
    public float secondAttackTimer = 0.32f;
    public float thirdAttackTimer = 0.8f;

    private float _timer;
    

    private void Awake()
    {

        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();

        transform.position = new Vector3(0, StartingHeight, 0);

        if (_rigidbody)
            _rigidbody.AddForce(Vector2.down * Speed);

        CurrentBossState = BossState.ENTERING;

    }

    private void Update()
    {
        //move boss left/right
        if (CurrentBossState != BossState.ENTERING)
        {
            float time = Mathf.PingPong(timePassed * 0.4f, 1);
            transform.position = new Vector2(tempPosition.x + Mathf.Lerp(3, -3, time), tempPosition.y);
            timePassed += Time.deltaTime;
        }
        
        if (CurrentBossState == BossState.ENTERING)
        {
            if (transform.position.y <= EndingHeight)
            {
                if (_rigidbody)
                    _rigidbody.velocity = Vector2.zero;
                tempPosition = transform.position;
                CurrentBossState = BossState.ATTACK_1;
                bossLifeManager.startShield = true;
            }
        }

        else if (CurrentBossState == BossState.ATTACK_1)
        {
            //attack
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _timer = firstAttackTimer;
                Attack1();

            }
            
            //check if health lower than switch attack
            if (bossLifeManager.currentHealthPercentage <= 0.67)
            {
                CurrentBossState = BossState.ATTACK_2;
            }
        }
        
        else if(CurrentBossState == BossState.ATTACK_2)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _timer = secondAttackTimer;
                Attack2();

            }
            //check if health lower than switch attack
            if (bossLifeManager.currentHealthPercentage <= 0.33)
            {
                CurrentBossState = BossState.ATTACK_3;
            }
        }
        
        else if (CurrentBossState == BossState.ATTACK_3)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _timer = thirdAttackTimer;
                Attack3();

            }
        }
    }

    //RANDOM SHOT IN RANGE
    public void Attack1()
    {
        Transform laserBeam = Instantiate(projectile1, _transform.position, Quaternion.identity);

        float value = Random.Range(-7, 7);
        Vector3 position = new Vector3(value, -3.7f,0) - laserBeam.transform.position;

        if (laserBeam.GetComponent<LaserBeam>())
            laserBeam.GetComponent<LaserBeam>().Launch(position);

    }
    
    //SHOOT FROM LEFT TO RIGHT
    private void Attack2()
    {
        Transform laserBeam = Instantiate(projectile2, _transform.position, Quaternion.identity);
        
        float time = Mathf.PingPong(timePassed * 0.4f, 1);
        Vector3 pos = new Vector3(Mathf.Lerp(7, -7, time), -3.7f,0) - laserBeam.transform.position;

        if (laserBeam.GetComponent<LaserBeam>())
            laserBeam.GetComponent<LaserBeam>().Launch(pos);
    }

    //CONTINUOUS SHOOT IN THREE DIRECTIONS
    private void Attack3()
    {
        foreach (Vector2 direction in ShootingDirections.ShootingDirections)
        {

            Transform laserBeam = Instantiate(projectile3, _transform.position, Quaternion.identity);

            if (laserBeam.GetComponent<LaserBeam>())
                laserBeam.GetComponent<LaserBeam>().Launch(direction);

        }
    }


}