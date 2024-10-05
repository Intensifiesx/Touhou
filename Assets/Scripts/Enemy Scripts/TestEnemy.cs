using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TestEnemy : Enemy
{
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private float shotVelocity;
    public override void Reset(){
        //Disable enemy in enemy manager
    }
    public override void Move(){
        transform.position = initialPosition + new Vector3(Mathf.Cos(Time.time * 5f) * 10f, Mathf.Sin(Time.time * 5f) * 1f, 0);
    }
    
    public override void Shoot(ObjectPool<GameObject> bulletPool, GameObject player){
        GameObject bullet = bulletPool.Get();
        bullet.transform.position = transform.position;
        bullet.transform.right = player.transform.position - bullet.transform.position;
        bullet.GetComponent<Rigidbody2D>().AddForce(Vector3.Normalize(bullet.transform.right) * shotVelocity, ForceMode2D.Impulse);
        bullet.GetComponent<EnemyBullet>().SetEnemy(this);

        bullet = bulletPool.Get();
        bullet.transform.position = transform.position;
        bullet.transform.right = player.transform.position - bullet.transform.position;
        bullet.transform.RotateAround(bullet.transform.position, Vector3.forward, 10);
        bullet.GetComponent<Rigidbody2D>().AddForce(Vector3.Normalize(bullet.transform.right) * shotVelocity, ForceMode2D.Impulse);
        bullet.GetComponent<EnemyBullet>().SetEnemy(this);

        bullet = bulletPool.Get();
        bullet.transform.position = transform.position;
        bullet.transform.right = player.transform.position - bullet.transform.position;
        bullet.transform.RotateAround(bullet.transform.position, Vector3.forward, -10);
        bullet.GetComponent<Rigidbody2D>().AddForce(Vector3.Normalize(bullet.transform.right) * shotVelocity, ForceMode2D.Impulse);
        bullet.GetComponent<EnemyBullet>().SetEnemy(this);
    }
}
