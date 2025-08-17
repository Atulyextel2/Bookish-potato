using System;
using Unity.VisualScripting;
using UnityEngine;

// This class is responsible for passing the dependenices to everyone
public class GameCompositionRoot : MonoBehaviour
{
        [SerializeField] private GameObject InputProviders;
        [SerializeField] private GameObject _cardPrefab;
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private SoundEffectConfig _soundConfig;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private RectTransform _boardContainer;
        [SerializeField] private UIController _uIController;
        [SerializeField] private LayoutSelector layoutSelector;

        #region  All the internal dependencies

        private IInputProvider _inputProvider;
        private ICardDataProvider _dataProvider;
        private CardFactory _cardFactory;
        private CardViewRegistry _cardViewRegistry;
        private ILayoutStrategy _layoutStrategy;
        private BoardManager _boardManager;
        private IAudioService _audioManager;
        private IProgressRepository _progressRepository;
        private ScoreManager _scoreManager;
        private FlipCommandQueue _flipCommandQueue;
        private GameController _gameController;
        private AnimationManager _animationManager;
        private GameStateMachine _fsm;
        private int _selRows, _selCols, _selPresetIdx;

        #endregion

        private void Awake()
        {
                if (!ValidateFields())
                {
                        Debug.LogError("Missing fields");
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                }
                // getting the input manager
                SetupInput();
                // getting the audio manager
                _audioManager = new AudioManager(_soundConfig, _audioSource);
                // point from where to save and load the LeaderBoard
                _progressRepository = new JsonFileProgressRepository();
                GameProgress _lastSavedGameProgress = _progressRepository.Load();
                // getting the scoremanager for handling users score
                _scoreManager = new ScoreManager();
                _scoreManager.OnScoreChanged += _uIController.UpdateScore;
                // Hooking up the UI
                int savedIndex = _gameConfig.presets.FindIndex(p => p.name == _lastSavedGameProgress.SelectedPresetName);
                int defaultIdx = Mathf.Clamp(savedIndex, 0, _gameConfig.presets.Count - 1);
                HookUpUI(defaultIdx);
        }

        private void HookUpUI(int defaultIdx)
        {
                _uIController.OnLevelSelected += OnLevelSelected;
                _uIController.InitializeLevelDropdown(_gameConfig.presets, defaultIdx);
                _uIController.OnStart += OnStartPressed;
                _uIController.OnHome += OnHomePressed;
        }

        private void ClearGame()
        {
                _inputProvider?.Disable();

                if (_fsm != null)
                {
                        _fsm.OnGameOver -= OnHomePressed;
                        _gameController.ClearAllEvents();
                }

                if (_gameController != null)
                {
                        Destroy(_gameController);
                }
                (_animationManager as IDisposable)?.Dispose();
                if (_cardFactory != null)
                {
                        _cardFactory.OnCardCreated -= _cardViewRegistry.Register;
                }
                if (_boardContainer != null)
                {
                        foreach (Transform child in _boardContainer)
                        {
                                Destroy(child.gameObject);
                        }
                }
                _dataProvider = null;
                _cardFactory = null;
                _cardViewRegistry = null;
                _animationManager = null;
                _layoutStrategy = null;
                _boardManager = null;
                _fsm = null;
                _flipCommandQueue = null;
                _gameController = null;
        }
        private void OnGameOver()
        {
                _boardContainer.gameObject.SetActive(false);
                _inputProvider.Disable();
        }

        private void OnHomePressed()
        {
                ClearGame();
        }
        private void OnLevelSelected(int rows, int cols, int idx)
        {
                _selRows = rows;
                _selCols = cols;
                _selPresetIdx = idx;
                // _progress.SelectedPresetName = _config.presets[idx].name;
                // _repo.Save(_progress);
        }

        private void OnStartPressed()
        {
                ClearGame();
                _boardContainer.gameObject.SetActive(true);
                _scoreManager.SetInitial(0, 0);
                #region Building Game Board 

                _dataProvider = new ResourcesCardDataProvider();
                _cardFactory = new CardFactory(_cardPrefab);
                _cardViewRegistry = new CardViewRegistry();
                _cardFactory.OnCardCreated += _cardViewRegistry.Register;
                // TODO: need to handle gameController in a genric way so the just needs to be reinit instead of remove and adding again.
                _gameController = this.gameObject.AddComponent<GameController>();
                _animationManager = new AnimationManager(_cardFactory, _gameController);
                _layoutStrategy = new GridLayoutStrategy();
                _boardManager = new BoardManager(_dataProvider, _cardFactory, _layoutStrategy, _gameConfig.matchGroupSize);
                _boardManager.SetupBoard(_selRows, _selCols, _boardContainer);

                #endregion

                #region Building FSM
                int totalGroups = (_selRows * _selCols) / _gameConfig.matchGroupSize;
                _fsm = new GameStateMachine(_audioManager, _scoreManager, _gameConfig.matchGroupSize, totalGroups);
                _fsm.OnGameOver += OnGameOver;

                #endregion

                #region  Add the GameController


                _flipCommandQueue = new FlipCommandQueue();
                _gameController.Initialize(_inputProvider, _flipCommandQueue, _fsm, _gameConfig.flipAnimationDuration);

                #endregion

        }

        private bool ValidateFields()
        {
                return InputProviders != null
                    && _cardPrefab != null
                    && _gameConfig != null
                    && _soundConfig != null
                    && _audioSource != null
                    && _uIController != null
                    && _boardContainer != null
                    && layoutSelector != null;
        }

        private void SetupInput()
        {
#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
                _inputProvider = InputProviders.GetComponentInChildren<MouseVectorInputProvider>();
#elif UNITY_IOS || UNITY_ANDROID
                _inputProvider = InputProviders.GetComponentInChildren<TouchVectorInputProvider>();
#else
                _inputProvider = InputProviders.GetComponentInChildren<MouseVectorInputProvider>();
#endif
        }

}