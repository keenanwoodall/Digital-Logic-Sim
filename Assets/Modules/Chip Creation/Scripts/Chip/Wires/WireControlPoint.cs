using System.Collections;
using DLS.ChipCreation;
using UnityEngine;
using SebInput.Internal;

public class WireControlPoint : MonoBehaviour
{
    [SerializeField] private float zDepth = -0.2f;

    private Wire _wire;
    private int _index;
    private Vector3 _startScale;

    private Vector2 _dragOffset;

    public void Initialize(Wire wire, int index)
    {
        _wire = wire;
        _index = index;

        _startScale = transform.localScale;
        
        var listener = gameObject.AddComponent<MouseInteractionListener>();
        listener.LeftMouseDown += OnLeftMouseDown;
        listener.MouseEntered += OnMouseEntered;
        listener.MouseExitted += OnMouseExited;
    }

    private void OnLeftMouseDown()
    {
        var mousePosition = MouseHelper.GetMouseWorldPosition();
        _dragOffset = (Vector2)transform.position - mousePosition;

        StartCoroutine(DragRoutine());
    }
    
    private void OnMouseEntered()
    {
        transform.localScale = _startScale * 1.1f;
    }
    
    private void OnMouseExited()
    {
        transform.localScale = _startScale;
    }

    private IEnumerator DragRoutine()
    {
        while (!MouseHelper.LeftMouseReleasedThisFrame())
        {
            var newPosition = MouseHelper.GetMouseWorldPosition() + _dragOffset;
            _wire.UpdateAnchorPoint(_index, newPosition);
            yield return null;
        } 
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = new Vector3(position.x, position.y, zDepth);
    }
}
