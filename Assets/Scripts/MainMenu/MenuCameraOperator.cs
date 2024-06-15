using Cinemachine;
using UnityEngine;

public class MenuCameraOperator : MonoBehaviour
{
    [SerializeField] CinemachineBrain brain;
    [SerializeField] CinemachineVirtualCamera[] cameraID;
    MainMenu menu;

    private void Start() {
        menu = GameObject.FindGameObjectWithTag("Menu").GetComponent<MainMenu>();
        menu.cutCameraDelegate += SetCamera;
        menu.blendCameraDelegate += SetCamera;
        SwitchCamera(cameraID[0]);
    }

    private void SetCamera(int cam) {
        brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
        SwitchCamera(cameraID[cam]);
    }

    private void SetCamera(int cam, CinemachineBlendDefinition.Style blend, float time) {
        brain.m_DefaultBlend.m_Style = blend;
        brain.m_DefaultBlend.m_Time = time;
        SwitchCamera(cameraID[cam]);
    }

    private void SwitchCamera(CinemachineVirtualCamera target) {
        foreach (CinemachineVirtualCamera camera in cameraID)
            camera.enabled = camera == target;
    }
}
