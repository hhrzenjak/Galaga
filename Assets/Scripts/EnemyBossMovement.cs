using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemyBossMovement : MonoBehaviour
{

    public float StartingHeight;
    public float EndingHeight;

    public float Speed;
    public float timePassed = 1.5f;
    public Vector2 tempPosition;
    public BossState CurrentBossState;

    protected Transform _transform;
    protected Rigidbody2D _rigidbody;


    private void Awake()
    {

        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();

        _transform.position = new Vector3(0, StartingHeight, 0);

        if (_rigidbody)
            _rigidbody.AddForce(Vector2.down * Speed);

        CurrentBossState = BossState.ENTERING;

    }

    private void Update()
    {

        if (transform.position.y <= EndingHeight && CurrentBossState==BossState.ENTERING)
        {
            if (_rigidbody)
                _rigidbody.velocity = Vector2.zero;
            tempPosition = transform.position;
            CurrentBossState = BossState.ATTACK_1;
        }

        if (CurrentBossState!=BossState.ENTERING)
        {
            //transform.position = new Vector2(+ Mathf.PingPong(timePassed * 1f, 3),  tempPosition.y);
            float time = Mathf.PingPong(timePassed * 0.4f, 1);
            //transform.position = new Vector2(tempPosition.x + Mathf.Lerp( 3,-3,time), tempPosition.y);
            timePassed += Time.deltaTime;
            
//            float newPosX = Mathf.Cos(timePassed);
//            float newPosY = Mathf.Sin(3 * timePassed) / 2 + tempPosition.y;
//            Debug.Log(newPosX);
//            transform.position = new Vector2(newPosX, newPosY);
//            timePassed += Time.deltaTime;
            
        }




    }

}
