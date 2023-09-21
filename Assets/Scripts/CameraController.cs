using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float m_sensitivity = 0.1f;
    [SerializeField] float m_forwardSpeed = 15.0f;
    [SerializeField] float m_horizontalSpeed = 5.0f;

    private Vector3 m_posAnchorPoint;

    private Vector3 m_rotAnchorPoint;
    private Quaternion m_anchorRot;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_posAnchorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        }

        if (Input.GetMouseButtonDown(1))
        {
            m_rotAnchorPoint = new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
            m_anchorRot = transform.rotation;
        }

        Vector3 vertical = Input.mouseScrollDelta.y * Vector3.forward;
        Vector3 move = vertical * m_forwardSpeed * Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPosAnchorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            Vector3 horizontal = m_posAnchorPoint - currentPosAnchorPoint;
            m_posAnchorPoint = currentPosAnchorPoint;

            move += horizontal * m_horizontalSpeed * Time.deltaTime;
        }

        transform.Translate(move * 9.1f);

        if (Input.GetMouseButton(1))
        {
            Quaternion rot = m_anchorRot;
            Vector3 diff = m_rotAnchorPoint - new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
            rot.eulerAngles += diff * m_sensitivity;
            transform.rotation = rot;
        }
    }
}
