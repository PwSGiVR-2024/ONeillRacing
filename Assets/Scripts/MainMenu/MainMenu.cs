using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public delegate void CameraDelegate(int cameraID);
    public CameraDelegate cameraDelegate;

    public void CameraMain() {
        cameraDelegate(0);
    }

    public void CameraLeft() {
        cameraDelegate(1);
    }

    public void CameraRight() {
        cameraDelegate(2);
    }
}
