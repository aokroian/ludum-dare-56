using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InputUtils
{
    public class LookInputSurface : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        private Vector2 _lastPointerPosition;
        private bool _isDragging;
        private Vector2 _lookInput;
        private Vector2 _currentPointerPos;

        public PlayerInputsService playerInputService;

        private void Update()
        {
            var pointerDelta = _currentPointerPos - _lastPointerPosition;
            pointerDelta = new Vector2(pointerDelta.x, -pointerDelta.y);
            _lastPointerPosition = _currentPointerPos;
            playerInputService.LookInput(pointerDelta);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _currentPointerPos = eventData.position;
            _lastPointerPosition = eventData.position;
            _isDragging = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isDragging = false;
            _lookInput = Vector2.zero;
            _lastPointerPosition = Vector2.zero;
            _currentPointerPos = Vector2.zero;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _currentPointerPos = eventData.position;
        }
    }
}