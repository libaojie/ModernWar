using UnityEngine;
using System.Collections;

/// <summary>
/// 左边控制Tank移动的摇杆
/// </summary>
public class JoystickTest1 : MonoBehaviour
{
	/// <summary>
	/// 移动速度
	/// </summary>
    public float MoveSpeed = 20f;
	
	/// <summary>
	/// 转动速度
	/// </summary>
	public float RotationSpeed = 20f;
	
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
        if (joystick.JoystickName == "NguiJoystick")
        {
			transform.Rotate (new Vector3 (0f, joystick.JoystickAxis.x * RotationSpeed, 0f));
			transform.Translate (Vector3.forward * joystick.JoystickAxis.y * MoveSpeed * Time.deltaTime);
        }
        
    }

   
}
