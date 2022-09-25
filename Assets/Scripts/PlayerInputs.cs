using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Inputs del player
 * 
 * @author Cesar Himura
 */
public class PlayerInputs : MonoBehaviour
{
    private Vector2 delta;
    private Vector2 deltaRotate;
    private bool isMove;
    private bool isFire;
    private bool isRotate;
    private bool bStart;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * Inputs del movimiento del player
     */
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started) //code for when action starts (key down)
        {

        }
        else if (context.performed) //code for when action is executed
        {
            delta = context.ReadValue<Vector2>();
            isMove = true;
        }
        else if (context.canceled) //code for when action is completed or stopped (key up)
        {
            isMove = false;
        }
    }

    /**
     * Inputs para el disparo
     */
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started) //code for when action starts (key down)
        {

        }
        else if (context.performed) //code for when action is executed
        {
            isFire = true;
        }
        else if (context.canceled) //code for when action is completed or stopped (key up)
        {
            isFire = false;
        }
    }

    /**
     * Inputs para la rotacion
     */
    public void OnRotate(InputAction.CallbackContext context)
    {
        if (context.started) //code for when action starts (key down)
        {

        }
        else if (context.performed) //code for when action is executed
        {
            deltaRotate = context.ReadValue<Vector2>();
            //context..
            isRotate = true;
        }
        else if (context.canceled) //code for when action is completed or stopped (key up)
        {
            isRotate = false;
        }
    }

    /**
     * Obtiene si esta en movimiento
     */
    public bool IsMoving()
    {
        return isMove;
    }

    /**
     * Obtiene si esta disparando
     */
    public bool IsFiring()
    {
        return isFire;
    }

    /**
     * Obtiene el desplazamiento
     */
    public Vector2 GetDelta()
    {
        return delta;
    }

    public bool IsRotating()
    {
        return isRotate;
    }

    public Vector2 GetDeltaRotate()
    {
        return deltaRotate;
    }

    public void OnStart(InputAction.CallbackContext context)
    {
        if (context.started) //code for when action starts (key down)
        {
            bStart = true;
        }
        else if (context.performed) //code for when action is executed
        {
            
        }
        else if (context.canceled) //code for when action is completed or stopped (key up)
        {
            bStart = false;
        }
    }

    public bool isPressStart()
    {
        return bStart;
    }

    public void PressStartFalse()
    {
        bStart = false;
    }
}
