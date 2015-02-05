using UnityEngine;
using System.Collections;

/// <summary>
/// 触屏控制
/// </summary>
public class TouchScreenController : MonoBehaviour 
{
    int screenWidth;
    int screenHeight;

	// Use this for initialization
	void Start () 
    {
        // 获得屏幕宽和高
        screenWidth = Screen.width;
        screenHeight = Screen.height;
	}

    /// <summary>
    /// 启动时调用，这里开始注册手势操作的事件
    /// </summary>
    void OnEnable()
    {
        //上、下、左、右、四个方向的手势滑动
        FingerGestures.OnFingerSwipe += OnFingerSwipe;
        //按下事件： OnFingerDown就是按下事件监听的方法
        FingerGestures.OnFingerDown += OnFingerDown;
        //抬起事件
        FingerGestures.OnFingerUp += OnFingerUp;
        //连击事件 连续点击事件
        FingerGestures.OnFingerTap += OnFingerTap;
        //长按事件
        FingerGestures.OnFingerLongPress += OnFingerLongPress;
    }

    /// <summary>
    /// 关闭时调用，这里销毁手势操作的事件
    /// </summary>
    void OnDisable()
    {
        FingerGestures.OnFingerDown -= OnFingerDown;
        FingerGestures.OnFingerUp -= OnFingerUp;
        FingerGestures.OnFingerSwipe -= OnFingerSwipe;
        FingerGestures.OnFingerTap -= OnFingerTap;
        FingerGestures.OnFingerLongPress -= OnFingerLongPress;
    }

    /// <summary>
    /// 按下时调用
    /// </summary>
    /// <param name="fingerIndex"> 第一按下的手指就是 0 第二个按下的手指就是1</param>
    /// <param name="fingerPos">手指按下屏幕中的2D坐标</param>
    void OnFingerDown(int fingerIndex, Vector2 fingerPos)
    {
        //将2D坐标转换成3D坐标
        transform.position = GetWorldPos(fingerPos);
        Debug.Log("Time=" + Time.time + " "+" OnFingerDown =" + fingerPos);
    }

    /// <summary>
    /// 抬起时调用
    /// </summary>
    /// <param name="fingerIndex"></param>
    /// <param name="fingerPos"></param>
    /// <param name="timeHeldDown"></param>
    void OnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
    {

        Debug.Log("Time=" + Time.time + " " + " OnFingerUp =" + fingerPos);
    }

    /// <summary>
    /// 上下左右四方方向滑动手势操作
    /// </summary>
    /// <param name="fingerIndex"></param>
    /// <param name="startPos"></param>
    /// <param name="direction"></param>
    /// <param name="velocity"></param>
    void OnFingerSwipe(int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
    {
        //结果是 Up Down Left Right 四个方向
        Debug.Log("Time=" + Time.time + " " + "OnFingerSwipe " + direction + " with finger " + fingerIndex + " startPos=" + startPos + " velocity" + velocity);

    }

    /// <summary>
    /// 连续按下事件， tapCount就是当前连续按下几次
    /// </summary>
    /// <param name="fingerIndex"></param>
    /// <param name="fingerPos"></param>
    /// <param name="tapCount"></param>
    void OnFingerTap(int fingerIndex, Vector2 fingerPos, int tapCount)
    {

        Debug.Log("Time=" + Time.time + " " + "OnFingerTap " + tapCount + " times with finger " + fingerIndex);

    }

    /// <summary>
    /// 长按事件
    /// </summary>
    /// <param name="fingerIndex"></param>
    /// <param name="fingerPos"></param>
    void OnFingerLongPress(int fingerIndex, Vector2 fingerPos)
    {

        Debug.Log("Time=" + Time.time + " " + "OnFingerLongPress " + fingerPos);
    }

    /// <summary>
    /// 把Unity屏幕坐标换算成3D坐标
    /// </summary>
    /// <param name="screenPos"></param>
    /// <returns></returns>
    Vector3 GetWorldPos(Vector2 screenPos)
    {
        Camera mainCamera = Camera.main;
        return mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(transform.position.z - mainCamera.transform.position.z)));
    }
}
