using UnityEngine;
using System.Collections;

/// <summary>
/// 左边控制Tank移动的摇杆
/// </summary>
public class JoystickTest1 : MonoBehaviour
{
    public float speed = 2f;
	
	void OnEnable()
    {
        Joystick.On_JoystickHolding += Joystick_On_JoystickHolding;
    }

    void OnDisable()
    {
        Joystick.On_JoystickHolding -= Joystick_On_JoystickHolding;
    }

    private void Joystick_On_JoystickHolding(Joystick joystick)
    {
        if(joystick.JoystickName == "NguiJoystick")
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(joystick.JoystickAxis.x, 0f, joystick.JoystickAxis.y));
            transform.Translate(new Vector3(joystick.JoystickAxis.x, 0f, joystick.JoystickAxis.y) * speed * Time.deltaTime, Space.World);
        }
        
    }

   
}
