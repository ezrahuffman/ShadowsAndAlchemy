using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] float maxLookAngle;
    [SerializeField] Transform side_1;
    [SerializeField] Transform side_2;
    [SerializeField] LayerMask doorLayermask;


    GameObject Player = null;
    bool canUse = false;
    GameController gameController;

    [SerializeField] GameObject outsideLight;

    [SerializeField] bool lightToDark;
    [SerializeField] bool needsWeapon;
    [SerializeField] bool oneWay;

    [SerializeField] AudioSource audioSource;
    


    private void Start()
    {
        gameController = GameController.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player = other.gameObject;
            Player.GetComponent<MageController>().AddDoor(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Player.GetComponent<MageController>().RemoveDoor(this);
            Player = null;
        }
    }

    private void Update()
    {
        if (Player != null)
        {
            if (PlayerIsLookingAtObject(Player))
            {
                Vector3 playerPos = Player.GetComponent<MageController>().GetPlayerPosition();
                bool isSideOne = Vector3.Distance(side_1.position, playerPos) < Vector3.Distance(side_2.position, playerPos);
                if (oneWay && !isSideOne)
                {
                    gameController.PromptUse("You can't go this way.");
                    canUse = false;
                }
                else if (needsWeapon && !Player.GetComponent<MageController>().HasWeapon())
                {
                    gameController.PromptUse("You dont have a weapon.\n\rTransform one of the items on the table.");
                    canUse = false;
                }
                else
                {
                    gameController.PromptUse("Press 'E' to use door");
                    canUse = true;
                }
            }
            else
            {
                canUse = false;
            }
        } else
        {
            canUse = false;
        }
    }

    bool PlayerIsLookingAtObject(GameObject Player)
    {
        MageController mage = Player.GetComponent<MageController>();
        //Debug.DrawRay(mage.GetPlayerPosition() + (1f * Vector3.up), mage.GetPlayerForward(), Color.red);
        return Physics.Raycast(mage.GetPlayerPosition() + (1f *Vector3.up), mage.GetPlayerForward(), 6f, doorLayermask);
    }

    public void Use()
    {
        if (!canUse)
        {
            return;
        }

        MageController  mage = Player.GetComponent<MageController>();
        Vector3 playerPos = mage.GetPlayerPosition();
        bool isSideOne = Vector3.Distance(side_1.position, playerPos) < Vector3.Distance(side_2.position, playerPos);

        audioSource.Play();

        if (isSideOne)
        {
            Debug.Log("go inside");
            if (lightToDark)
            {
                outsideLight.SetActive(false);
            }
            mage.Teleport(side_2.position);
        }
        else
        {
            Debug.Log("go outside");
            if (lightToDark)
            {
                outsideLight.SetActive(true);
            }
            mage.Teleport(side_1.position);
        }
    }
}
