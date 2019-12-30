using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossMovementLaser: MonoBehaviour
{
    public float StartingHeight;
    public float EndingHeight;

    public float Speed;
    public float timePassed = 1.5f;
    public Vector2 tempPosition;
    public BossState CurrentBossState;
    
    public EnemyBossLifeManager bossLifeManager;

    public Transform bigLaser;
    public Transform smallLaser;

    protected Transform _transform;
    protected Rigidbody2D _rigidbody;

    public float laserTimer = 3f;
    public float _timer = 3f;
    private bool laserActive;
    
    public Transform projectile1;
    public Transform projectile2;
    public Transform projectile3;
    public Transform targetCircle;

    public float projectile2AttackTimer = 0.4f;
    public float projectile3AttackTimer = 1f;

    public float _attackTimer = 1.5f; //change down near attack1
    
    private Transform target;


    private void Awake()
    {

        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();

        _transform.position = new Vector3(0, StartingHeight, 0);

        if (_rigidbody)
            _rigidbody.AddForce(Vector2.down * Speed);

        CurrentBossState = BossState.ENTERING;

        _timer = laserTimer + 2f;
        
        bigLaser = transform.Find("Boss Laser Big");
        smallLaser = transform.Find("Boss Laser Small");

    }
    
    private void Update()
    {
        //move boss left/right
        if (CurrentBossState != BossState.ENTERING)
        {
            float time = Mathf.PingPong(timePassed * 0.42f, 1);
            transform.position = new Vector2(tempPosition.x + Mathf.Lerp(3.5f, -3.5f, time), tempPosition.y);
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
                //bossLifeManager.startShield = true;
                ActivateLaser();
            }
        }

        else if (CurrentBossState == BossState.ATTACK_1)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                if (!laserActive)
                {
                    _timer = laserTimer + 2f;
                    ActivateLaser();
                }
                else
                {
                    _timer = laserTimer;
                    DeactivateLaser();
                }

            }
            
            _attackTimer -= Time.deltaTime;
            if (_attackTimer <= 0)
            {
                Attack1();
                _attackTimer = 1.5f;
            }
            
            //check if health lower than switch attack
            if (bossLifeManager.currentHealthPercentage <= 0.67)
            {
                //turn off laser
                DeactivateLaser();
                CurrentBossState = BossState.ATTACK_2;
            }
        }
        
        else if(CurrentBossState == BossState.ATTACK_2)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _timer = projectile2AttackTimer;
                Attack2();
            }

            //check if health lower than switch attack
            if (bossLifeManager.currentHealthPercentage <= 0.45)
            {
                CurrentBossState = BossState.ATTACK_3;
                //smallLaser.gameObject.SetActive(true);

            }
        }
        
        else if (CurrentBossState == BossState.ATTACK_3)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _timer = projectile3AttackTimer;
                Attack3();
            }

        }
    }

    //SHOT WHILE LASER
    public void Attack1()
    {
        Transform laserBeam = Instantiate(projectile1, _transform.position + new Vector3(0.7f,0,0), Quaternion.identity);
        
        Vector2 direction1 = new Vector2(0.5f, -0.5f);
        
        if (laserBeam.GetComponent<LaserBeam>())
            laserBeam.GetComponent<LaserBeam>().Launch(direction1);
        
        Transform laserBeam2 = Instantiate(projectile1, _transform.position + new Vector3(-0.7f,0,0), Quaternion.identity);

        Vector3 direction2 = new Vector3(-0.5f, -0.5f);
        
        if (laserBeam2.GetComponent<LaserBeam>())
            laserBeam2.GetComponent<LaserBeam>().Launch(direction2);

    }
    
    //SHOOT TWO LINES
    private void Attack2()
    {
        Transform laserBeam = Instantiate(projectile2, _transform.position, Quaternion.identity);
        
        float time = Mathf.PingPong(timePassed * 0.3f, 1);
        Vector3 pos = new Vector3(Mathf.Lerp(7, -7, time), -3.7f,0) - laserBeam.transform.position;

        if (laserBeam.GetComponent<LaserBeam>())
            laserBeam.GetComponent<LaserBeam>().Launch(pos);

        pos.x = -pos.x;
        Transform laserBeam2 = Instantiate(projectile2, _transform.position, Quaternion.identity);

        if (laserBeam2.GetComponent<LaserBeam>())
            laserBeam2.GetComponent<LaserBeam>().Launch(pos);

    }

    //Discarded small laser attack
    private void SmallLaserAttack()
    {
//        Transform laserBeam = Instantiate(projectile2, _transform.position, Quaternion.identity);
//
        float valueX = Random.Range(-3, 3);
        float valueY = Random.Range(1, -3);
        Vector3 position = new Vector3(valueX + transform.position.x, valueY,0);
        
        if(target)
            Destroy(target.gameObject);
        
        target = Instantiate(targetCircle, position, Quaternion.identity);
//
//        if (laserBeam.GetComponent<LaserBeam>())
//            laserBeam.GetComponent<LaserBeam>().Launch(position);
        smallLaser.GetComponent<LaserScript2>()._endPosition = position - transform.position;
    }

    private void Attack3()
    {
        float valueX = Random.Range(-3, 3);
        float valueY = Random.Range(1, -3);
        Vector3 position = new Vector3(valueX + transform.position.x, valueY,0);
        
        if(target)
            Destroy(target.gameObject);
        
        target = Instantiate(targetCircle, position, Quaternion.identity);

        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(LaunchLasers(0.2f*i, position));
        }
        
        
    }

    
    private IEnumerator LaunchLasers(float delay, Vector3 direction)
    {
        yield return new WaitForSeconds( delay ) ;
        Transform laserBeam = Instantiate(projectile3, _transform.position, Quaternion.identity);

        if (laserBeam.GetComponent<LaserBeam>())
            laserBeam.GetComponent<LaserBeam>().Launch(direction  - laserBeam.transform.position);
    }


    public void ActivateLaser()
    {
        bigLaser.gameObject.SetActive(true);
        laserActive = true;
    }

    public void DeactivateLaser()
    {
        bigLaser.gameObject.SetActive(false);
        laserActive = false;

    }
    
    

}
