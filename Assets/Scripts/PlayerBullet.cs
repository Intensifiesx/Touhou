using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Player player;
    public void setPlayer(Player player){
        this.player = player;
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Bullet Deleter"){
            if(player != null){
                player.DeleteBullet(this.gameObject);
            }else{
                Destroy(this.gameObject);
            }
        }
    }
}
