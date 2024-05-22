using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    [SerializeField] private KeyCode MoveUpKey;
    [SerializeField] private KeyCode MoveDownKey;
    [SerializeField] private KeyCode MoveLeftKey;
    [SerializeField] private KeyCode MoveRightKey;
    [SerializeField] private KeyCode MoveRandomKey;
    [SerializeField] private KeyCode DontMoveKey;
    [SerializeField] private KeyCode PickItemKey;
    
    public enum State {None, Human, AI}
    public State ControlMode 
    { 
        get => controlMode;
        private set
        {
            controlMode = value;
            OnControlModeChange();
        }
    }
    
    private State controlMode;
    private LusoBehaviour lb;
    private GameManager gm;
    private bool hasActionPlayed;
    
    
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void GameStart(bool isPlayerHuman)
    {
        lb = FindObjectOfType<LusoBehaviour>();

        if (isPlayerHuman) ControlMode = State.Human;
    }

    public void EndGame()
    {
        ControlMode = State.None;
    }

    private void Update()
    { 
        if (controlMode == State.Human) PlayerInput();
    }

    private void PlayerInput()
    {
        hasActionPlayed = false;
        
        if (Input.GetKeyDown(MoveUpKey))
        {
            lb.MoveUp();
            hasActionPlayed = true;
        }

        else if (Input.GetKeyDown(MoveDownKey))
        {
            lb.MoveDown();
            hasActionPlayed = true;
        }

        else if (Input.GetKeyDown(MoveRightKey))
        {
            lb.MoveRight();
            hasActionPlayed = true;
        }

        else if (Input.GetKeyDown(MoveLeftKey))
        {
            lb.MoveLeft();
            hasActionPlayed = true;
        }

        else if (Input.GetKeyDown(MoveRandomKey))
        {
            lb.MoveRandom();
            hasActionPlayed = true;
        }

        else if (Input.GetKeyDown(DontMoveKey))
        {
            hasActionPlayed = true;
        }

        else if (Input.GetKeyDown(PickItemKey))
        {
            gm.TryPickingItem();
            hasActionPlayed = true;
        }
        
        if(hasActionPlayed) gm.ActionPlayed();
    }
    
    private void OnControlModeChange()
    {

        switch (ControlMode)
        {
            case State.None:
                ChangedToNone();
                break;
            case State.Human:
                ChangedToHuman();
                break;
            case State.AI:
                ChangedToAI();
                break;
        }
    }
    
    private void LockCursor() => Cursor.lockState = CursorLockMode.Locked;
    private void UnlockCursor() => Cursor.lockState = CursorLockMode.None;
    private void ShowCursor(bool value) => Cursor.visible = value;

    private void ChangedToNone()
    {
        UnlockCursor();
        ShowCursor(true);
    }

    private void ChangedToHuman()
    {
        LockCursor();
        ShowCursor(false);
    }

    private void ChangedToAI()
    {
        LockCursor();
        ShowCursor(false);
    }
}
