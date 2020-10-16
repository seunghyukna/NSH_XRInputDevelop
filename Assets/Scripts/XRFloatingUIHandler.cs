using UnityEngine;

public static class XRFloatingUIHandler
{
    public delegate void VoidDelegate();

    public static event VoidDelegate OnUIOpened;
    public static event VoidDelegate OnUIClosed;

    public static void UIOpened()
    {
        if (OnUIOpened != null)
        {
            OnUIOpened();
        }
    }

    public static void UIClosed()
    {
        if (OnUIClosed != null)
        {
            OnUIClosed();
        }
    }
}
