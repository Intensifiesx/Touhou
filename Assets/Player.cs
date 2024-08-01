using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    public PlayerInput playerControls;
    [SerializeField] private GameObject bulletObject;
    Vector2 moveDirection = Vector2.zero;
    private InputAction move, fire;
    private float lastShot;

    void Start()
    {
        playerControls = new PlayerInput();
        move = playerControls.Player.Move;
        fire = playerControls.Player.Fire;
        move.Enable();
        fire.Enable();
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
        transform.position += 8 * Time.deltaTime * (Vector3)moveDirection;
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
}
