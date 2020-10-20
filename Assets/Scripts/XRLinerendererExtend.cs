using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRLinerendererExtend : MonoBehaviour
{
    [SerializeField] private bool isActive;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private Color validColor;
    [SerializeField] private Color invalidColor;
    [SerializeField] private Color UIHoverColor;
    [SerializeField] private Color UISelectColor;
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.material = lineMaterial;
    }

    public void ChangeLine(bool _flag)
    {
        if (!isActive)
            return;

        if (_flag)
        {
            lineMaterial.SetColor("_Color", validColor);
        }
        else
        {
            lineMaterial.SetColor("_Color", invalidColor);
        }
    }

    public void ChangeLine(int _state)
    {
        if (!isActive)
            return;

        switch (_state)
        {
            case 0:
                lineMaterial.SetColor("_Color", invalidColor);
                break;
            case 1:
                lineMaterial.SetColor("_Color", UIHoverColor);
                break;
            case 2:
                lineMaterial.SetColor("_Color", UISelectColor);
                break;
            default:
                break;
        }
    }
}
