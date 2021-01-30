using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMouseAimInput
{
    Vector2 mousePos { get; }
    bool usingMouse { get; }
}