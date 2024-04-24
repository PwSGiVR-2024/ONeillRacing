using Cinemachine;
using UnityEngine;

public class MenuCameraOperator : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera[] cameraID;
    MainMenu menu;

    private void Start() {
        menu = GameObject.FindGameObjectWithTag("Menu").GetComponent<MainMenu>();
        menu.cameraDelegate += SetCamera;
        SwitchCamera(cameraID[0]);
    }

    private void SetCamera(int cam) {
        SwitchCamera(cameraID[cam]);
    }

    private void SwitchCamera(CinemachineVirtualCamera target) {
        foreach (CinemachineVirtualCamera camera in cameraID)
            camera.enabled = camera == target;
    }
}
