using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using TMPro;
public class Player : MonoBehaviour
{
    public PlayerInput playerControls;
    [SerializeField] private GameObject playerBulletPrefab, bulletContainer;
    [SerializeField] private int bombCount, bombMax, healthCount, healthMax;
    [SerializeField] private float speed = 8.0f, focusMultiplier = .5f, shotDelay = 0.05f, shotVelocity = 100;
    [SerializeField] private TMP_Text healthText, bombText;
    [SerializeField] private FailScreen failScreen;
    Vector2 moveDirection = Vector2.zero;
    private InputAction move, fire, bomb, focus;
    private SpriteRenderer hitboxSprite;
    private GameObject bombHitbox;
    private float lastShot;
    private ObjectPool<GameObject> bulletPool;

    void Start()
    {
        hitboxSprite = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        bombHitbox = gameObject.transform.GetChild(2).gameObject;
        playerControls = new PlayerInput();
        move = playerControls.Player.Move;
        fire = playerControls.Player.Fire;
        bomb = playerControls.Player.Bomb;
        focus = playerControls.Player.Focus;
        healthText.text = "Health: " + healthCount + "/" + healthMax;
        bombText.text = "Bombs: " + bombCount + "/" + bombMax;
        move.Enable();
        fire.Enable();
        bomb.Enable();
        focus.Enable();

        bulletPool = new ObjectPool<GameObject>(() =>{
            return Instantiate(playerBulletPrefab, bulletContainer.transform);
        }, playerBullet => {
            playerBullet.SetActive(true);
        }, playerBullet => {
            playerBullet.SetActive(false);
        }, playerBullet => {
            Destroy(playerBullet.gameObject);
        }, true, 100, 100);

    }

    public void GameOver()
    {
        failScreen.Setup();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
        // hold down the fire button to shoot
        if (fire.phase == InputActionPhase.Performed)
            Fire();
    }

    void FixedUpdate()
    {
        transform.position += speed * Time.deltaTime * (Vector3)moveDirection;
    }

    void Fire()
    {
        if (Time.time - lastShot < shotDelay)
            return;
        lastShot = Time.time;
        GameObject bullet = bulletPool.Get();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;

        bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.up * shotVelocity, ForceMode2D.Impulse);

        bullet.GetComponent<PlayerBullet>().SetPlayer(this);
    }

    public void DeleteBullet(GameObject targetBullet){
        bulletPool.Release(targetBullet);
    }

    public void Bomb(InputAction.CallbackContext context)
    {
        if(context.started && bombCount > 0)
        {
            bombHitbox.SetActive(true);
            bombCount --;
            bombText.text = "Bombs: " + bombCount + "/" + bombMax;
            StartCoroutine(ClearBomb());
        }
    }
    IEnumerator ClearBomb(){
        yield return new WaitForSeconds(.1f);
        bombHitbox.SetActive(false);
    }

    public void Focus(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            speed *= focusMultiplier;
            hitboxSprite.enabled = true;
        }
        else if(context.canceled)
        {
            speed /= focusMultiplier;
            hitboxSprite.enabled = false;
        }
    }

    public void Hit(){
        if(healthCount < 1){
            GameOver();
            return;
        }
        healthCount --;
        healthText.text = "Health: " + healthCount + "/" + healthMax;
    }
}
