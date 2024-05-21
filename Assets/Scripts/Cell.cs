using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public enum State {Empty, HasTrash, Wall}
    [SerializeField] private GameObject trashImage;

    private State state;
    private Vector2 coordinates;
    public State CellState { get => state; set{ state = value; } }
    public Vector2 Coordinates { get => coordinates; set{ coordinates = value; } }

    private void Start()
    {
        state = State.Empty;
        coordinates = new Vector2(0, 0);
    }

    public void UpdateCell()
    {
        if(state == State.Wall)
        {
            gameObject.GetComponent<Image>().enabled = false;
            trashImage.SetActive(false);
        }

        else
        {
            gameObject.GetComponent<Image>().enabled = true;
            
            trashImage.SetActive(state == State.HasTrash);
        }
    }
}
