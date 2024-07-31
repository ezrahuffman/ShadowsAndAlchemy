using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MageController : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    // Input
    [SerializeField] InputAction moveInput;
    [SerializeField] InputAction transformInput;
    [SerializeField] InputAction useDoorInput;
    [SerializeField] InputAction attackInput;

    [SerializeField] Animator animator;
    [SerializeField] GameObject mage;
    [SerializeField] bool canMove;
    
    int runAnimationHash;
    int idleAnimationHash;


    int currentAnimHash;
    Vector2 move;
    Vector2 prevMove;

    Door currentDoor = null;        // Holds nearby door
    InteractableObject currentInteractableObj = null;        // Holds nearby object
    Weapon currentWeapon = null;

    bool isAttacking;
    bool _breakOnFinishAttack = false;
    Weapon _weaponToBreak;
    bool usingDoor;

    HashSet<CustomLight> customLights;
    GameController gameController;
    //[SerializeField] InputAction fireDirInput;
    //[SerializeField] InputAction fireInput;
    //[SerializeField] InputAction interactInput;
    //[SerializeField] InputAction drinkInput;
    //[SerializeField] InputAction foodInput;

    private void Awake()
    {
        moveInput.Enable();
        transformInput.Enable();
        useDoorInput.Enable(); 
        attackInput.Enable();

        transformInput.started += TransformInput_started;
        useDoorInput.started += UseDoorInput_started;
        attackInput.started += AttackInput_started;

        runAnimationHash = Animator.StringToHash("Running_A");
        idleAnimationHash =  Animator.StringToHash("Idle");

        currentAnimHash = idleAnimationHash;

        _currEnergy = maxEnergy;

        customLights = new HashSet<CustomLight>();
    }

    private void Start()
    {
        gameController = GameController.instance;
    }

    private void UseDoorInput_started(InputAction.CallbackContext context)
    {
        if (currentInteractableObj == null && currentDoor != null)
        {
            //usingDoor = true;
            UseDoor();
        } else if(currentInteractableObj != null) {
            TransformableObject transformableObject = currentInteractableObj?.GetComponent<TransformableObject>();
            if (transformableObject != null)
            {
                TransformObject(transformableObject);
            }
        }
    }

    private void TransformInput_started(InputAction.CallbackContext context)
    {
        TransformableObject transformableObject = currentInteractableObj?.GetComponent<TransformableObject>();
        if (transformableObject != null)
        {
            TransformObject(transformableObject);
        }
    }

    private void AttackInput_started(InputAction.CallbackContext context)
    {
        TryToAttack();
    }

    // Update is called once per frame
    void Update()
    {
        move = moveInput.ReadValue<Vector2>();

        //Debug.Log($"player lit = {IsVisable()}");

        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        // Wait for the attack animation to finish
        if(isAttacking) { return; }
        if (move.magnitude > 0.001f && currentAnimHash != runAnimationHash)
        {
            animator.Play(runAnimationHash);
            currentAnimHash = runAnimationHash;
        }
        if (move.magnitude <= 0.001f && currentAnimHash != idleAnimationHash) 
        {
            animator.Play(idleAnimationHash);
            currentAnimHash = idleAnimationHash;
        }
    }


    private void FixedUpdate()
    {
        // Don't move while attacking
        if (isAttacking || !canMove) { return; }

        Vector3 moveVect = Vector3.zero;
        moveVect.x = move.x;
        moveVect.z = move.y;

        Vector3 delta = moveVect * Time.fixedDeltaTime * moveSpeed;
        transform.position += delta; // might be better to rb


        //Rotate player
        Quaternion rotation = Quaternion.identity;

        if (delta != Vector3.zero)
        {
            rotation.SetLookRotation(delta);
        }

        if (prevMove != move && move.magnitude > 0.001f)
        {
            mage.transform.rotation = rotation;
        }
        prevMove = move;

        if (usingDoor) {
            currentDoor.Use();
            usingDoor = false;
        }
    }

    /// ENERGY SYSTEM
    /// The player uses energy to transform objects
    ///

    int _currEnergy;
    [SerializeField] int maxEnergy;

    void TransformObject(TransformableObject obj)
    {
        int prevEnergy = _currEnergy;
        bool hasEnergy = obj.Cost <= _currEnergy;
        int trueCost = 0;
        if (hasEnergy)
        {
            // use the energy
            bool successful = obj.TryToInteract();

            if (successful)
            {
                _currEnergy -= obj.Cost;
                trueCost = obj.Cost;
                currentInteractableObj = null;

                Weapon weapon = obj.TransformedObject.GetComponent<Weapon>();
                if (weapon != null)
                {
                    SetWeapon(weapon);
                }
            }
        }
        else
        {
            // Unable to cast
            // TODO: do something here
            Debug.Log("Not enough energy to do that");
        }

        Debug.Log($"had {prevEnergy} energy and used {trueCost} engergy. Now we have {_currEnergy} energy");
    }


    public void TryToAttack()
    {
        Debug.Log("Try to attack");
        if (currentWeapon == null) {
            Debug.Log("Null weapon when trying to attack.");
            return; 
        }

        if(isAttacking)
        {
            return;
        }

        gameController.PauseEnemies();
        currentWeapon.Attack(animator);
        isAttacking = true;
    }



    internal Vector3 GetPlayerPosition()
    {
        return mage.transform.position;
    }

    internal Vector3 GetPlayerForward()
    {
        return mage.transform.forward;
    }

    internal void AddDoor(Door door)
    {
        currentDoor = door;
    }

    internal void RemoveDoor(Door door)
    {
        if(currentDoor == door)
        {
            currentDoor = null;
        }
    }

    void UseDoor()
    {
        if(currentDoor == null)
        {
            return;
        }

        usingDoor = true;
        //currentDoor.Use();
    }

    // TODO(later): should check for overlaps when doing this
    public void Teleport(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void  SetInteractableObject(InteractableObject obj)
    {
        if(currentInteractableObj == obj) { return; }
        currentInteractableObj = obj;
    }

    public void RemoveInteractableObject(InteractableObject obj) 
    { 
        if (currentInteractableObj == obj)
        {
            currentInteractableObj = null;
        }
    }

    public void SetWeapon(Weapon obj)
    {
        if (currentWeapon == obj) { return; }
        RemoveWeapon(currentWeapon);
        currentWeapon = obj;
        currentWeapon.onBreak += OnBreakWeaponBreak;
    }

    public void RemoveWeapon(Weapon obj)
    {
        if (currentWeapon == obj && currentWeapon != null)
        {
            if (currentWeapon.onBreak != null)
            {
                currentWeapon.onBreak -= OnBreakWeaponBreak;
            }
            currentWeapon = null;
        }
    }

    public void OnBreakWeaponBreak(Weapon weapon)
    { 
        _breakOnFinishAttack = true;
        _weaponToBreak = weapon;
    }

    private void BreakWeaponAfterAnimation()
    {
        // TODO: add some animations and stuff
        Debug.Log($"broke {_weaponToBreak.gameObject.name}");
        RemoveWeapon(_weaponToBreak);
        _weaponToBreak.gameObject.SetActive(false);
    }

    internal void FinishAttack()
    {
        Debug.Log("Finnished Attack");
        currentAnimHash = 0; // reset current animation
        isAttacking = false;
        UpdateAnimations();

        if (_breakOnFinishAttack == true)
        {
            BreakWeaponAfterAnimation();
            _breakOnFinishAttack = false;
        }
    }

    internal bool IsVisable()
    {
        return customLights.Count != 0;
    }

    public void AddLight(CustomLight light)
    {
       customLights.Add(light);
    }

    public void RemoveLight(CustomLight light)
    {
        customLights.Remove(light);
    }
}
