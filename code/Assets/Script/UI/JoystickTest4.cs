using UnityEngine;
using System.Collections;

/// <summary>
/// 右边摇杆，控制坦克瞄准方向
/// </summary>
public class JoystickTest4 : MonoBehaviour
{
	/// <summary>
	/// 向右旋转的速度
	/// </summary>
    public float RightSpeed = 20f;

    /// <summary>
    /// 向上旋转的速度
    /// </summary>
    public float TopSpeed = 20f;

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

		if (networkView.isMine && joystick.JoystickName == "NguiJoystickTop")
        {
			// 左右旋转
			transform.Rotate(new Vector3(0, joystick.JoystickAxis.x * RightSpeed, 0)); // 左右旋转  没问题

			// 上下旋转
			transform.eulerAngles = new Vector3(-joystick.JoystickAxis.y * TopSpeed, transform.eulerAngles.y, 0);

        }
    }

   
}
