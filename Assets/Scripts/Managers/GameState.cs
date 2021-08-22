namespace GameState
{
    public class GameStateManager
    {
        // Static constants
        public static IGameState[] states = new IGameState[] {
            new MainMenuGameState("MainMenu", "GamePlay"),
            new GamePlayGameState("GamePlay"),
            new PauseGameState("Pause"),
            new GameOverGameState("GameOver")
        };

        // Public variables
        public IGameState currentState { get; private set; }

        // Constructor
        public GameStateManager()
        {
            currentState = states[0];
        }

        // Public methods
        public void ChangeState(string stateName)
        {
            // If the state exists, change to it
            for (int i = 0; i < states.Length; i++)
            {
                if (states[i].name == stateName)
                {
                    SetGameState(states[i]);
                }
            }

            // Otherwise, throw an error
            throw new System.Exception($"State [{stateName}] does not exist");

            //UnityEngine.Debug.LogError("GameManager: State " + stateName + " does not exist.");
        }

        private void SetGameState(IGameState state)
        {
            currentState?.OnExit();
            state.prevState = currentState.name;
            currentState = state;
            currentState.OnEnter();
        }

        public bool GoToNextState()
        {
            if (!string.IsNullOrEmpty(currentState.nextState))
            {
                ChangeState(currentState.nextState);
                return true;
            }

            return false;
        }

        public bool GoToPrevState()
        {
            if (!string.IsNullOrEmpty(currentState.prevState))
            {
                ChangeState(currentState.prevState);
                return true;
            }

            return false;
        }
    }

    public abstract class IGameState
    {
        public string name;
        public string prevState;
        public string nextState;

        public delegate void StateDelegate();
        public StateDelegate onStateEnter;
        public StateDelegate onStateExit;

        public IGameState(string name, string prevState = null, string nextState = null)
        {
            this.name = name;
            this.prevState = prevState;
            this.nextState = nextState;
        }

        private void Enter()
        {
            OnEnter();
            onStateEnter?.Invoke();
        }

        private void Exit()
        {
            onStateExit?.Invoke();
            OnExit();
        }

        internal abstract void OnEnter();
        internal abstract void OnExit();
    }

    public abstract class SimpleGameState : IGameState
    {
        public SimpleGameState(string name, string prevState = null, string nextState = null) : base(name, prevState, nextState)
        { }
        internal override void OnEnter()
        { }
        internal override void OnExit()
        { }
    }

    public class MainMenuGameState : SimpleGameState
    {
        public MainMenuGameState(string name, string prevState = null, string nextState = null) : base(name, prevState, nextState) { }
    }

    public class GamePlayGameState : SimpleGameState
    {
        public GamePlayGameState(string name, string prevState = null, string nextState = null) : base(name, prevState, nextState) { }
    }

    public class PauseGameState : IGameState
    {
        public PauseGameState(string name, string prevState = null, string nextState = null) : base(name, prevState, nextState) { }

        internal override void OnEnter()
        {
            UnityEngine.Time.timeScale = 0;
        }

        internal override void OnExit()
        {
            UnityEngine.Time.timeScale = 1;
        }
    }

    public class GameOverGameState : IGameState
    {
        public GameOverGameState(string name, string prevState = null, string nextState = null) : base(name, prevState, nextState) { }
        internal override void OnEnter()
        { }
        internal override void OnExit()
        { }
    }
}