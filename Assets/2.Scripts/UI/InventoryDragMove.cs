using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDragMove : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private RectTransform targetRect; // 드래그로 이동시킬 실제 패널
    private Canvas canvas;
    private Vector2 offset;

    private void Awake()
    {
        if (targetRect == null)
            targetRect = GetComponent<RectTransform>();

        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // canvas 기준으로 offset 계산
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

        offset = targetRect.anchoredPosition - localPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
        {
            targetRect.anchoredPosition = localPoint + offset;
        }
    }
}
