using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraTarget;
    private Vector3 _dist = new Vector3(0f, 2f, -2f);

    private float targetSpeed = PlayerController.RunningSpeed;
    // Start is called before the first frame update
    private void LateUpdate() {
        Vector3 nextPos = _cameraTarget.position + _dist;
        Vector3 instantPos = Vector3.Lerp(transform.position, nextPos, targetSpeed * Time.deltaTime);
        transform.position = instantPos;
    }
}
