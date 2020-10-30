using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ThrowableProvider : MonoBehaviour
{
    private XRBaseInteractable baseInteractable;
    [SerializeField] float velocity;
    private bool isHolding;

    private void Start()
    {
        baseInteractable = GetComponent<XRBaseInteractable>();
    }

    private void Update()
    {
        if (baseInteractable.isSelected)
        {
            isHolding = true;
        }
        else
        {
            ExecuteThrowing();

            isHolding = false;
        }
    }

    private void ExecuteThrowing()
    {
        if (!isHolding)
            return;


    }
}
