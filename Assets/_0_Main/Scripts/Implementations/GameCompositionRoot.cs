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

        private IInputProvider inputProvider;
        private ICardDataProvider dataProvider;
        private CardFactory cardFactory;
        private CardViewRegistry cardViewRegistry;
        private ILayoutStrategy layoutStrategy;
        private BoardManager boardManager;
        private IAudioService audioManager;
        private IProgressRepository _progressRepository;
        private ScoreManager scoreManager;
        private FlipCommandQueue flipCommandQueue;
        private GameController gameController;
        private int _selRows, _selCols, _selPresetIdx;

        #endregion

        private void Awake()
        {
                if (HandledSerializeFieldDependencies())
                {
                        _progressRepository = new JsonFileProgressRepository();
                        GameProgress _lastSavedGameProgress = _progressRepository.Load();
                        scoreManager = new ScoreManager();
                        SetUpInput();
                        audioManager = new AudioManager(_soundConfig, _audioSource);
                        int savedIndex = _gameConfig.presets.FindIndex(p => p.name == _lastSavedGameProgress.SelectedPresetName);
                        int defaultIdx = Mathf.Clamp(savedIndex, 0, _gameConfig.presets.Count - 1);
                        HookUpUI(defaultIdx);
                }
                else
                {
                        Debug.LogError("Some of the serializeField Dependencies are not resolved");
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                }
        }

        private void HookUpUI(int defaultIdx)
        {
                scoreManager.OnScoreChanged += _uIController.UpdateScore;
                _uIController.OnLevelSelected += OnLevelSelected;
                _uIController.InitializeLevelDropdown(_gameConfig.presets, defaultIdx);
                _uIController.OnStart += OnStartPressed;
                _uIController.OnHome += OnHomePressed;
        }

        void OnGameOver()
        {
                // Simply disable input; UIController will still show the play canvas,
                // and the Home button is already wired for returning to start.
                GetComponent<IInputProvider>().Disable();
        }

        void OnHomePressed()
        {
                foreach (Transform child in _boardContainer)
                        Destroy(child.gameObject);

                inputProvider.Disable();
        }

        void OnLevelSelected(int rows, int cols, int idx)
        {
                _selRows = rows;
                _selCols = cols;
                _selPresetIdx = idx;
                // _progress.SelectedPresetName = _config.presets[idx].name;
                // _repo.Save(_progress);
        }

        private void OnStartPressed()
        {
                scoreManager.SetInitial(0, 0);
                #region Building Game Board 

                dataProvider = new ResourcesCardDataProvider();
                cardFactory = new CardFactory(_cardPrefab);
                cardViewRegistry = new CardViewRegistry();
                cardFactory.OnCardCreated += cardViewRegistry.Register;
                layoutStrategy = new GridLayoutStrategy();
                boardManager = new BoardManager(dataProvider, cardFactory, layoutStrategy, _gameConfig.matchGroupSize);
                boardManager.SetupBoard(_selRows, _selCols, _boardContainer);

                #endregion

                #region Building FSM

                // FSM with dynamic match size
                int totalGroups = (_selRows * _selRows) / _gameConfig.matchGroupSize;
                GameStateMachine fsm = new GameStateMachine(audioManager, scoreManager, _gameConfig.matchGroupSize, totalGroups);
                fsm.GameOver += OnGameOver;

                #endregion

                #region  Add the GameController

                // TODO: need to handle gameController in a genric way so the just needs to be reinit instead of remove and adding again.
                gameController = this.gameObject.AddComponent<GameController>();
                flipCommandQueue = new FlipCommandQueue();
                gameController.Initialize(inputProvider, flipCommandQueue, fsm, _gameConfig.flipAnimationDuration, audioManager);

                #endregion

        }

        private bool HandledSerializeFieldDependencies()
        {
                if (InputProviders == null || _cardPrefab == null
                || _gameConfig == null || _soundConfig == null
                || _audioSource == null || _boardContainer == null
                || _uIController == null || layoutSelector == null)
                {
                        return false;
                }
                return true;
        }

        private void SetUpInput()
        {
#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
                inputProvider = InputProviders.GetComponentInChildren<MouseVectorInputProvider>();
#elif UNITY_IOS || UNITY_ANDROID
                inputProvider = IInputProviders.GetComponentInChildren<TouchVectorInputProvider>();
#else
                inputProvider = InputProviders.GetComponentInChildren<MouseVectorInputProvider>();
#endif
        }

}