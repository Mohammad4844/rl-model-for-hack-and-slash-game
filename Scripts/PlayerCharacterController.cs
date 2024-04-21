using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : MonoBehaviour
{
    public float movementSpeed = 1f;
    PlayerCharacterRenderer isoRenderer;
    public bool canMove = true; 

    public InputActionAsset actionAsset;
    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction dashAction;
    private InputAction castAction;
    
    Rigidbody2D rb;

    public float dashDistance = 1f;
    public int dashTotalFrames = 10;
    private int currentDashFrame = 0;
    private Vector2 dashDirection;
    public bool isDashing = false;

    public int castTotalFrames = 10;
    private int currentCastFrame = 0;
    public bool isCasting = false;
    public GameObject castPrefab;
    
    public float dashCooldown = 3f;
    public float castCooldown = 6f;
    private float lastDashTime = -Mathf.Infinity; 
    private float lastCastTime = -Mathf.Infinity; 

    public Vector2 lastDirection;

    private AttackHitboxController attackController;

    private bool isNotAgent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<PlayerCharacterRenderer>();
        attackController = GetComponentInChildren<AttackHitboxController>();

        var playerControls = actionAsset.FindActionMap("Gameplay");
        moveAction = playerControls.FindAction("Move");
        attackAction = playerControls.FindAction("Attack");
        attackAction.performed += OnAttackPerformed;
        dashAction = playerControls.FindAction("Dash");
        dashAction.performed += OnDashPerformed;
        castAction = playerControls.FindAction("Cast");
        castAction.performed += OnCastPerformed;


        PlayerCharacterAgent agent = GetComponent<PlayerCharacterAgent>();
        isNotAgent = agent == null; 

        if (isNotAgent)
        {
            moveAction.Enable();
            attackAction.Enable();
            dashAction.Enable();
            castAction.Enable();   
        }
    }

    void Update()
    {
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            canMove = false;
            isoRenderer.Attack();

            int direction = isoRenderer.LastDirection();
            attackController.StartAttack(direction);
        }
    }

    public void OnAttackAnimationEnd()
    {
        attackController.EndAttack();
        canMove = true;
    }

    public void OnDashPerformed(InputAction.CallbackContext context)
    {
        if (!isDashing && Time.time - lastDashTime >= dashCooldown)
        {
            StartDash();
            lastDashTime = Time.time;
        }
    }

    void StartDash()
    {
        canMove = false;
        isDashing = true;
        isoRenderer.Dash();
        currentDashFrame = dashTotalFrames;
        dashDirection = lastDirection.normalized;
    }

    void PerformDash()
    {
        if (currentDashFrame > 0)
        {
            float dashDistanceForFrame = dashDistance / dashTotalFrames;
            Vector2 newPos = rb.position + dashDirection * dashDistanceForFrame;
            rb.MovePosition(newPos);

            currentDashFrame--;
        }
        else
        {
            isDashing = false;
            canMove = true;
        }
    }

    void OnCastPerformed(InputAction.CallbackContext context)
    {
        if (!isCasting && Time.time - lastCastTime >= castCooldown)
        {
            StartCast();
            lastCastTime = Time.time;
        }

    }

    void StartCast()
    {
        canMove = false;
        isCasting = true;
        isoRenderer.Cast();
        
        currentCastFrame = castTotalFrames;

        Instantiate(castPrefab, transform.position, transform.rotation); // transform doesnt really matter here
    }

    void CastAnimation()
    {
        if (currentCastFrame > 0)
        {
            currentCastFrame--;
        }
        else
        {
            isCasting = false;
            canMove = true;
            isoRenderer.EndCast();
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            PerformDash();
        }
        else if (isCasting)
        {
            CastAnimation();
        }

        if (isNotAgent)
        {
            Vector2 inputVector = moveAction.ReadValue<Vector2>();
            MovePlayer(inputVector);
        }
    }

    public void MovePlayer(Vector2 direction) {
        if (canMove) {
            lastDirection = direction;
            Vector2 currentPos = rb.position;
            Vector2 movement = Vector2.ClampMagnitude(direction, 1) * movementSpeed;
            Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
            isoRenderer.SetDirection(movement);
            rb.MovePosition(newPos);
        }
    }

    // for use by agent

    public void TriggerAttack() {
        OnAttackPerformed(new InputAction.CallbackContext());
    }

    public void TriggerDash() {
        if (!isDashing) 
            OnDashPerformed(new InputAction.CallbackContext());
    }

    public void TriggerCast() {
        if (!isCasting)
            OnCastPerformed(new InputAction.CallbackContext());
    }


}
