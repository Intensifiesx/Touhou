using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletContainer; //The object to store all the bullet objects in as children; easier to see in editor
    [SerializeField] private GameObject player;
    [SerializeField] private float shotDelay = .5f;
    private float lastShot;
    private ObjectPool<GameObject> bulletPool;

    private void Start(){ //Immediately sets the bullet pool up
        bulletPool = new ObjectPool<GameObject>(() =>{ // We have no bullets, so we want to create a new bullet
            return Instantiate(bulletPrefab, bulletContainer.transform);
        }, bullet => { // GETTER(): If we have bullets, we want to set them to active
            bullet.SetActive(true);
        }, bullet => { // RELEASE(): If we have bullets, we want to set them to inactive
            bullet.SetActive(false);
        }, bullet => { // DESTROY(): If we have bullets, we want to destroy them 
            Destroy(bullet.gameObject);
        }, true, 100, 100);
    }

    public virtual void Reset(){}

    private void FixedUpdate(){
        Move(); //To be determined by the enemy itself that inherents
        if (Time.time - lastShot > shotDelay){ //Calcs when to shoot again
            Shoot(bulletPool, player);
            lastShot = Time.time;
        }
    }

    public virtual void Move(){}

    public virtual void Shoot(ObjectPool<GameObject> bulletPool, GameObject player){}
    
    public void DeleteBullet(GameObject targetBullet){ //Easier to understand phrasing of releasing from the bullet pool *disabling the bullet*
        if (targetBullet.activeInHierarchy)
            bulletPool.Release(targetBullet);
    }
}
