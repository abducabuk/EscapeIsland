using CbkSDK.Core.ServiceLocator;
using CbkSDK.Core.ServiceLocator.Attribute;
using CbkSDK.Core.ServiceLocator.Interface;

namespace CbkSDK.State
{
    public interface IGameStateService : IService
    {
        public bool IsMainMenu { get; }
        public bool IsGameplay { get; }
        public bool IsLevelSuccess { get; }
        public bool IsLevelFailed { get; }

        public void EnterMainMenu();
        public void EnterGameplay();
        public void EnterLevelSuccess();
        public void EnterLevelFailed();

    }

    [Service(false)]
    public class GameStateService : BaseMonoService ,IGameStateService
    {
        private class MainMenuState: State
        {
            protected override void OnEntered()
            {
            }

            protected override void OnExited()
            {
            }
        }
        private class GameplayState: State
        {
            protected override void OnEntered()
            {
            }

            protected override void OnExited()
            {
            }
        }
        private class LevelSuccessState: State
        {
            protected override void OnEntered()
            {
            }

            protected override void OnExited()
            {
            }
        }
        private class LevelFailedState: State
        {
            protected override void OnEntered()
            {
            }

            protected override void OnExited()
            {
            }
        }

        private State mainMenu;
        private State gameplay;
        private State levelSuccess;
        private State levelFailed;

        private StateMachine _stateMachine;

        public void Initialize()
        {
            mainMenu = new MainMenuState();
            gameplay = new GameplayState();
            levelSuccess = new LevelSuccessState();
            levelFailed = new LevelFailedState();
            mainMenu.OnEnter.AddListener(OnMainMenuEnter);
            mainMenu.OnExit.AddListener(OnMainMenuExit);
            gameplay.OnEnter.AddListener(OnGamePlayEnter);
            gameplay.OnExit.AddListener(OnGamePlayExit);
            levelSuccess.OnEnter.AddListener(OnLevelSuccessEnter);
            levelSuccess.OnExit.AddListener(OnLevelSuccessExit);
            levelFailed.OnEnter.AddListener(OnLevelFailedEnter);
            levelFailed.OnExit.AddListener(OnLevelFailedExit);
            _stateMachine = new StateMachine(mainMenu,gameplay,levelSuccess,levelFailed);
            EnterMainMenu();
        }
    
        private void OnMainMenuEnter()
        {
            Fire(GameStateEvents.ON_MAINMENU_ENTER);   
        }
    
        private void OnMainMenuExit()
        {
            Fire(GameStateEvents.ON_MAINMENU_EXIT);
        }
    
        private void OnGamePlayEnter()
        {
            Fire(GameStateEvents.ON_GAMEPLAY_ENTER);   

        }
    
        private void OnGamePlayExit()
        {
            Fire(GameStateEvents.ON_GAMEPLAY_EXIT);   

        }

        private void OnLevelFailedEnter()
        {
            Fire(GameStateEvents.ON_LEVELFAILED_ENTER);   

        }

        private void OnLevelFailedExit()
        {
            Fire(GameStateEvents.ON_LEVELFAILED_EXIT);   

        }

        private void OnLevelSuccessEnter()
        {
            Fire(GameStateEvents.ON_LEVELSUCCESS_ENTER);   

        }

        private void OnLevelSuccessExit()
        {
            Fire(GameStateEvents.ON_LEVELSUCCESS_EXIT);
        }

        public bool IsMainMenu => _stateMachine.Current is MainMenuState;
        public bool IsGameplay => _stateMachine.Current is GameplayState;
        public bool IsLevelSuccess => _stateMachine.Current is LevelSuccessState;
        public bool IsLevelFailed => _stateMachine.Current is LevelFailedState;
        public void EnterMainMenu() => _stateMachine.SetState(mainMenu);
        public void EnterGameplay() => _stateMachine.SetState(gameplay);
        public void EnterLevelSuccess() => _stateMachine.SetState(levelSuccess);
        public void EnterLevelFailed() => _stateMachine.SetState(levelFailed);
    }
}