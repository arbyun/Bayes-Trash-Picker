using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public enum State {Empty, HasTrash, Wall}
    [SerializeField] private GameObject trashImage;

    private State state;
    private Vector2 coordinates;
    private Image cellImage;
    public State CellState { get => state; set{ state = value; } }
    public Vector2 Coordinates { get => coordinates; set{ coordinates = value; } }

    private void Awake()
    {
        state = State.Empty;
        cellImage = GetComponent<Image>();
        coordinates = new Vector2(0, 0);
    }

    public void UpdateCell()
    {
        if(state == State.Wall)
        {
            cellImage.enabled = false;
            trashImage.SetActive(false);
        }

        else
        {
            cellImage.enabled = true;
            trashImage.SetActive(state == State.HasTrash);
        }
    }
}
