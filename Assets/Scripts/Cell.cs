using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines a cell and it's behaviour; whether it's empty, a wall, or has trash in it.
/// </summary>
public class Cell : MonoBehaviour
{
    /// <summary>
    /// The state that represents a cell's behaviour.
    /// </summary>
    /// <seealso cref="Cell"/>
    public enum State
    {
        Empty, 
        HasTrash, 
        Wall
    }
    
    [SerializeField] private GameObject trashImage;
    [SerializeField] private GameObject fogImage;

    private State _state;
    private Vector2 _coordinates;
    private Image _cellImage;

    /// <summary>
    /// The current state of the cell.
    /// </summary>
    public State CellState
    {
        get => _state; 
        set => _state = value;
    }

    /// <summary>
    /// Represents a cell's coordinates.
    /// </summary>
    public Vector2 Coordinates
    {
        get => _coordinates; 
        set => _coordinates = value;
    }

    private void Awake()
    {
        _state = State.Empty;
        _cellImage = GetComponent<Image>();
        _coordinates = new Vector2(0, 0);
    }

    /// <summary>
    /// Updates the cell's image to reflect its current state.    
    /// If the cell is a wall, it will be invisible.
    /// Otherwise, if there is trash in the cell,an image of trash will
    /// appear on top of it.
    /// </summary>
    public void UpdateCell()
    {
        if(_state == State.Wall)
        {
            _cellImage.enabled = false;
            trashImage.SetActive(false);
        }

        else
        {
            _cellImage.enabled = true;
            trashImage.SetActive(_state == State.HasTrash);
        }
    }

    public void ShowFog(bool state)
    {
        fogImage.SetActive(state);   
    }
}
