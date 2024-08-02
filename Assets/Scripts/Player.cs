using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    public PlayerInput playerControls;
    [SerializeField] private GameObject bulletObject;
    [SerializeField] private int bombCount, bombMax, healthCount, healthMax;
    [SerializeField] private float speed = 8.0f, focusMultiplier = .5f;
    Vector2 moveDirection = Vector2.zero;
    private InputAction move, fire, bomb, focus;
    private SpriteRenderer hitboxSprite;
    private float lastShot;

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
        if (Time.time - lastShot < 0.5f)
            return;
        lastShot = Time.time;
        GameObject bullet = Instantiate(bulletObject, transform.position, Quaternion.identity);
        bullet.AddComponent<Rigidbody2D>().GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        bullet.GetComponent<Rigidbody2D>().gravityScale = 0;
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
