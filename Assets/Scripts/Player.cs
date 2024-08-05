using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
public class Player : MonoBehaviour
{
    public PlayerInput playerControls;
    [SerializeField] private GameObject playerBulletPrefab, bulletContainer;
    [SerializeField] private int bombCount, bombMax, healthCount, healthMax;
    [SerializeField] private float speed = 8.0f, focusMultiplier = .5f, shotDelay = 0.05f, shotVelocity = 100;
    Vector2 moveDirection = Vector2.zero;
    private InputAction move, fire, bomb, focus;
    private SpriteRenderer hitboxSprite;
    private float lastShot;
    private ObjectPool<GameObject> bulletPool;

    void Start()
    {
        hitboxSprite = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        playerControls = new PlayerInput();
        move = playerControls.Player.Move;
        fire = playerControls.Player.Fire;
        bomb = playerControls.Player.Bomb;
        focus = playerControls.Player.Focus;
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

        bullet.GetComponent<PlayerBullet>().setPlayer(this);
    }

    public void DeleteBullet(GameObject targetBullet){
        bulletPool.Release(targetBullet);
    }

    public void Bomb(InputAction.CallbackContext context)
    {
        if(context.started && bombCount > 0)
        {
            //Bomb code
        }
    }

    public void Focus(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            speed = speed * focusMultiplier;
            hitboxSprite.enabled = true;
        }
        else if(context.canceled)
        {
            speed = speed / focusMultiplier;
            hitboxSprite.enabled = false;
        }
    }
}
