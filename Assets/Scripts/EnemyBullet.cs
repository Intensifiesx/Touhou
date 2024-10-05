using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Enemy enemy;
    public void SetEnemy(Enemy enemy){
        this.enemy = enemy;
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag != "Enemy Bullet" && col.gameObject.tag != "Enemy"){
            if(enemy != null){
                enemy.DeleteBullet(this.gameObject);
            }else{
                Destroy(this.gameObject);
            }

            if(col.gameObject.tag == "Player"){
                col.gameObject.transform.parent.gameObject.GetComponent<Player>().Hit();
            }
        }
    }
}
