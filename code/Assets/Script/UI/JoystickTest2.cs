using UnityEngine;
using System.Collections;

/// <summary>
/// 右边摇杆，控制坦克瞄准方向
/// </summary>
public class JoystickTest2 : MonoBehaviour
{
    public float speed = 2f;
	
	private float x = 0f;
	private float y = 0f;

	void OnEnable()
    {
        Joystick.On_JoystickHolding += Joystick_On_JoystickHolding;
    }

    void OnDisable()
    {
        Joystick.On_JoystickHolding -= Joystick_On_JoystickHolding;
    }

    private float currentTop = 0;
    private Vector3 currentPos;
    private void Joystick_On_JoystickHolding(Joystick joystick)
    {
				//Debug.Log(joystick.JoystickAxis.x+" "+ joystick.JoystickAxis.y);
				Debug.Log(transform.rotation); 
        if (joystick.JoystickName == "NguiJoystickTop")
        {
			x = joystick.JoystickAxis.x;
			y = joystick.JoystickAxis.y;

//						rotateLock(new Vector3(0, x, 0));
//						rotateLock(new Vector3(-y, 0, 0));

			transform.Rotate(new Vector3(0, x, 0)); // 左右旋转  没问题
//			transform.Rotate(new Vector3(-y, 0, 0));// 上下旋转 没问题

//						transform.Rotate(Vector3.up *Time.deltaTime * 50 * x); // 左右旋转 没问题
//						transform.Rotate(Vector3.right *Time.deltaTime * 50 * y); // 上下旋转 没问题

			currentPos = transform.eulerAngles;
						transform.eulerAngles = new Vector3(-y * 25, transform.eulerAngles.y, 0);

			//transform.Rotate(new Vector3(-joystick.JoystickAxis.y, joystick.JoystickAxis.x, 0));
			//transform.rotation = Quaternion.LookRotation(new Vector3(joystick.JoystickAxis.x, joystick.JoystickAxis.y,0));
            //transform.Translate(new Vector3(joystick.JoystickAxis.x, 0f, joystick.JoystickAxis.y) * speed * Time.deltaTime, Space.World);
        }
    }
		object syncLocker = new object();
    private void rotateLock(Vector3 pos)
    {
				lock(syncLocker)
				{
						transform.Rotate(pos);
				}
    }

   
}
