using Unity.VisualScripting;
using UnityEngine;

public class GameCompositionRoot : MonoBehaviour
{
        [SerializeField] private GameObject InputProviders;
        [SerializeField] private GameObject _cardPrefab;
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private SoundEffectConfig _soundConfig;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private RectTransform _boardContainer;
        [SerializeField] private UIController uIController;
        [SerializeField] private LayoutSelector layoutSelector;

        #region  All the internal dependencies

        private IInputProvider inputProvider;
        private ICardDataProvider dataProvider;
        private CardFactory cardFactory;
        private CardViewRegistry cardViewRegistry;
        private ILayoutStrategy layoutStrategy;
        private BoardManager boardManager;
        private IAudioService audioManager;
        private IProgressRepository progressRepository;
        private ScoreManager scoreManager;
        private FlipCommandQueue flipCommandQueue;
        private GameController gameController;

        #endregion

        private void Awake()
        {
                if (HandledSerializeFieldDependencies())
                {
                        progressRepository = new JsonFileProgressRepository();
                        GameProgress gameProgress = progressRepository.Load();
                        scoreManager = new ScoreManager();
                        scoreManager.SetInitial(gameProgress.Matches, gameProgress.Tries);
                        scoreManager.OnScoreChanged += uIController.UpdateScore;
                        SetUpInput();
                        dataProvider = new ResourcesCardDataProvider();
                        cardFactory = new CardFactory(_cardPrefab);
                        cardViewRegistry = new CardViewRegistry();
                        cardFactory.OnCardCreated += cardViewRegistry.Register;
                        layoutStrategy = new GridLayoutStrategy();
                        boardManager = new BoardManager(dataProvider, cardFactory, layoutStrategy);
                        audioManager = new AudioManager(_soundConfig, _audioSource);
                        flipCommandQueue = new FlipCommandQueue();
                        gameController = this.gameObject.AddComponent<GameController>();
                        int savedIndex = gameConfig.presets.FindIndex(p => p.name == gameProgress.SelectedPresetName);
                        int defaultIdx = Mathf.Clamp(savedIndex, 0, gameConfig.presets.Count - 1);
                        layoutSelector.Initialize(gameConfig.presets, defaultIdx);

                        // Build on selection
                        layoutSelector.OnLayoutSelected += (rows, cols, presetIdx) =>
                        {
                                // Persist choice
                                gameProgress.SelectedPresetName = gameConfig.presets[presetIdx].name;
                                progressRepository.Save(gameProgress);

                                // Board
                                var board = new BoardManager(dataProvider, cardFactory, layoutStrategy);
                                board.SetupBoard(rows, cols, _boardContainer);

                                // FSM with dynamic match size
                                int totalGroups = (rows * cols) / gameConfig.matchGroupSize;
                                var fsm = new GameStateMachine(audioManager, scoreManager, gameConfig.matchGroupSize, totalGroups);

                                // Controller init
                                gameController.Initialize(inputProvider, flipCommandQueue, fsm, gameConfig.flipAnimationDuration);
                        };
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

        private bool HandledSerializeFieldDependencies()
        {
                if (InputProviders == null || _cardPrefab == null
                || gameConfig == null || _soundConfig == null
                || _audioSource == null || _boardContainer == null
                || uIController == null || layoutSelector == null)
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