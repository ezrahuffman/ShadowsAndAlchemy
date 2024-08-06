using UnityEngine;

public class TransformableObject : InteractableObject
{
    public int Cost { get { return cost; }}
    [SerializeField] int cost;

    [SerializeField] GameObject defaultObject;


    // NOTE: this should be on the player for now, but be an independant item later
    [SerializeField] GameObject transformedObject;
    [SerializeField] AudioSource audioSource;

    protected override void OverrideableStart()
    {
        message = $"Convert {defaultObject.name} into {transformedObject.name}.\r\ncost: {cost}";
    }

    void Transform()
    {
        defaultObject.SetActive(false);
        transformedObject.SetActive(true);

        audioSource.Play();

        this.enabled = false;

    }

    protected override void Prompt()
    {
        if (mageController != null)
        {
            if (mageController.CurrentMana() < cost)
            {
                gameController.PromptUse($"Not enough mana, need {cost}");
            }
            else
            {
                gameController.PromptUse(message);
            }
        }
    }

    public override void Interact()
    {
        Transform();
    }

    public GameObject TransformedObject { get { return transformedObject; } }
}
