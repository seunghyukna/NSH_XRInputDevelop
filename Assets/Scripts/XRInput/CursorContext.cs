using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorContext : MonoBehaviour
{
    public enum CursorAction
    {
        None = 0,
        Move,
        Rotate,
        Scale
    }

    [SerializeField]
    private CursorAction currentCursorAction = CursorAction.None;
    public CursorAction CurrentCursorAction
    {
        get => currentCursorAction;
        set { currentCursorAction = value; }
    }
}
