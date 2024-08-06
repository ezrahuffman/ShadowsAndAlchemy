using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] float maxLookAngle;

    GameObject Player = null;

    protected bool canInteract = false;

    protected MageController mageController;
    protected GameController gameController;
    protected string message;

    private void Start()
    {
        gameController = GameController.instance;
        OverrideableStart();
    }

    protected virtual void OverrideableStart()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.GetComponent<MageController>()?.RemoveInteractableObject(this);
            Player = null;
        }
    }

    private void Update()
    {
        if (Player != null)
        {
            if (PlayerIsLookingAtObject(Player))
            {
                Debug.Log($"Player Can Interact With {gameObject.name}");
                canInteract = true;
                mageController?.SetInteractableObject(this);


                Prompt();
                
            } else
            {
                canInteract=false;
            }
        }else {
            canInteract = false;
        }
    }

    protected virtual void Prompt()
    {
        gameController?.PromptUse(message);
    }

    bool PlayerIsLookingAtObject(GameObject Player)
    {
        mageController = Player.GetComponent<MageController>();
        if (mageController != null)
        {
            Vector3 fromPlayerToObject = transform.position - mageController.GetPlayerPosition();
            Vector3 playerForward = mageController.GetPlayerForward();
            Quaternion fromTo = Quaternion.FromToRotation(playerForward, fromPlayerToObject);
            float fromToAngleDeg = fromTo.eulerAngles.y;


            // Test the amount of rotation needed to see if it is within the line of sight of the player
            float half = (maxLookAngle / 2);
            if (fromToAngleDeg < half || fromToAngleDeg > 360 - half)
            {
                //Debug.Log($"fromToAngleDeg: {fromToAngleDeg}; < {half} or > {360-half}  HALF = {half}");
                return true;
            }

        }

        // Not looking at the object, or not the player
        return false;
    }

    public bool TryToInteract()
    {
        if (!canInteract)
        {
            return false;
        }

        Interact();
        return true;
    }

    public virtual void Interact()
    {

    }
}
