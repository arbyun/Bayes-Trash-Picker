using System;
using UnityEngine;

/// <summary>
/// Basic input listener and state manager.
/// </summary>
/// <remarks> This class takes care of listening to the movement input of the player
/// and to check if it's an human player. It also gives the 'green light' to the AI to
/// play if the current state is set to AI.</remarks>
[RequireComponent(typeof(DataCollector))]
public class ControlsManager : MonoBehaviour
{
    [SerializeField] private KeyCode moveUpKey;
    [SerializeField] private KeyCode moveDownKey;
    [SerializeField] private KeyCode moveLeftKey;
    [SerializeField] private KeyCode moveRightKey;
    [SerializeField] private KeyCode moveRandomKey;
    [SerializeField] private KeyCode dontMoveKey;
    [SerializeField] private KeyCode pickItemKey;

    /// <summary>
    /// Defines whoever is playing right now, and alters
    /// <see cref="ControlsManager"/> to behave accordingly.
    /// </summary>
    public enum State
    {
        None, 
        Human, 
        AI
    }
    
    /// <summary>
    /// The current controlling state of the class.
    /// </summary>
    public State ControlMode 
    { 
        get => _controlMode;
        private set
        {
            _controlMode = value;
            OnControlModeChange();
        }
    }
    
    private State _controlMode;
    private LusoBehaviour _lb;
    private LusoAIBehaviour _lbAI;
    private GameManager _gm;
    private DataCollector _dataCollector;
    private bool _hasActionPlayed;

    private void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        _dataCollector = FindObjectOfType<DataCollector>();
    }

    /// <inheritdoc cref="GameManager.StartGame"/>
    public void GameStart(bool isPlayerHuman)
    {
        _lb = FindObjectOfType<LusoBehaviour>();
        _lbAI = FindObjectOfType<LusoAIBehaviour>();

        ControlMode = isPlayerHuman switch
        {
            true => State.Human,
            false => State.AI
        };
    }

    /// <inheritdoc cref="GameManager.GameOver"/>
    public void EndGame()
    {
        ControlMode = State.None;
    }

    /// <summary>
    /// Handles the player's input and calls the appropriate functions to move the character.
    /// Alternatively, handles the AI's turn.
    /// </summary>   
    private void Update()
    { 
        if (_controlMode == State.Human) PlayerInput();
        else if (_controlMode == State.AI) _lbAI.Play();
    }

    /// <summary>
    /// Method responsible for taking in player input and calling the appropriate functions.    
    /// It also records the action that was taken by the player for training the AI.
    /// </summary>
    private void PlayerInput()
    {
        _hasActionPlayed = false;
        var neighboringCells = _lb.GetNeighboringCells();
        
        if (Input.GetKeyDown(moveUpKey))
        {
            _lb.MoveUp();
            RecordAction(neighboringCells, _lb.MoveUp);
            _hasActionPlayed = true;
        }

        else if (Input.GetKeyDown(moveDownKey))
        {
            _lb.MoveDown();
            RecordAction(neighboringCells, _lb.MoveDown);
            _hasActionPlayed = true;
        }

        else if (Input.GetKeyDown(moveRightKey))
        {
            _lb.MoveRight();
            RecordAction(neighboringCells, _lb.MoveRight);
            _hasActionPlayed = true;
        }

        else if (Input.GetKeyDown(moveLeftKey))
        {
            _lb.MoveLeft();
            RecordAction(neighboringCells, _lb.MoveLeft);
            _hasActionPlayed = true;
        }

        else if (Input.GetKeyDown(moveRandomKey))
        {
            _lb.MoveRandom();
            RecordAction(neighboringCells, _lb.MoveRandom);
            _hasActionPlayed = true;
        }

        else if (Input.GetKeyDown(dontMoveKey))
        {
            _hasActionPlayed = true;
        }

        else if (Input.GetKeyDown(pickItemKey))
        {
            _gm.TryPickingItem();
            RecordAction(neighboringCells, _gm.TryPickingItem);
            _hasActionPlayed = true;
        }
        
        if(_hasActionPlayed) _gm.ActionPlayed();
    }
    
    /// <summary>
    /// Called when <see cref="ControlMode"/> changes.    
    /// It calls a function based on the new value of ControlMode.
    /// </summary>
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

    /// <summary>
    /// Called when neither a human nor an AI are playing.
    /// Sets the state to None.    
    /// It unlocks the cursor and shows it.
    /// </summary>
    private void ChangedToNone()
    {
        UnlockCursor();
        ShowCursor(true);
    }

    /// <summary>
    /// Called when the state changes to human controlled.    
    /// It locks the cursor and hides it.
    /// </summary>
    private void ChangedToHuman()
    {
        LockCursor();
        ShowCursor(false);
    }

    /// <summary>
    /// Called when the player changes to AI mode.    
    /// It locks the cursor and hides it.
    /// </summary>
    private void ChangedToAI()
    {
        LockCursor();
        ShowCursor(false);
    }
    
    /// <summary>
    /// Records the action that was taken by the player to a <see cref="DataCollector"/>
    /// which will posteriorly be used to train the AI.
    /// </summary>
    /// <param name="neighboringCells"> The states of the neighboring cells.</param>
    /// <param name="action"> The action that the agent took.</param>
    private void RecordAction(Cell.State[] neighboringCells, Action action)
    {
        var dataCollector = FindObjectOfType<DataCollector>();
        dataCollector.RecordAction(_lb.CurrentCellIndex, neighboringCells, action);
    }
}
