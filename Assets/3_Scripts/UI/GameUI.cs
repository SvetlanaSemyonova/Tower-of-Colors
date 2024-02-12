using UnityEngine;
using UnityEngine.EventSystems;

public class GameUI : MonoBehaviour
{
    [SerializeField] private BallShooter ballShooter;
    [SerializeField] private Transform cameraInputPivot;
    [SerializeField] private float rotationFactor;

    [SerializeField] private float rotationTime;

    private float rotationSpeed;
    private bool dragging = false;
    private float targetAngle = 0;
    private float currAngle = 0;

    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
        dragging = false;
    }

    private void Update()
    {
        var delta = targetAngle - currAngle;
        if (Mathf.Abs(delta) > Mathf.Epsilon) {
            rotationSpeed = (delta / (rotationTime * Time.timeScale)) * Time.deltaTime;
            currAngle += rotationSpeed;
            cameraInputPivot.Rotate(Vector3.up, rotationSpeed);
        }
    }

    public void OnPointerDown(BaseEventData eventData)
    {
        var pointerEventData = eventData as PointerEventData;
        currAngle = cameraInputPivot.localEulerAngles.y;
        targetAngle = currAngle;
    }

    public void OnPointerDrag(BaseEventData eventData)
    {
        dragging = true;
        var pointerEventData = eventData as PointerEventData;
        targetAngle += pointerEventData.delta.x * rotationFactor;
    }

    public void OnPointerUp(BaseEventData eventData)
    {
        if (!dragging) {
            var pointerEventData = eventData as PointerEventData;
            var ray = mainCamera.ScreenPointToRay(pointerEventData.position);
            RaycastHit hit;
            if (Physics.SphereCast(ray, 0.15f, out hit, 100f, 1, QueryTriggerInteraction.Ignore)) {
                var tile = hit.collider.GetComponent<TowerTile>();
                if (tile && tile.Active)
                    ballShooter.ShootTarget(hit.point, tile);
            }
        } else {
            dragging = false;
        }
    }
}
