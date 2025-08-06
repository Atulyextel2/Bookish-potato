using UnityEngine;

public class GameCompositionRoot : MonoBehaviour
{
        [SerializeField] private GameObject InputProviders;
        [SerializeField] GameObject _cardPrefab;

        #region  All the internal dependencies

        private IInputProvider inputProvider;
        private ICardDataProvider dataProvider;
        private CardFactory cardFactory;
        private CardViewRegistry cardViewRegistry;

        #endregion

        private void Awake()
        {
                SetUpInput();
                dataProvider = new ResourcesCardDataProvider();
                cardFactory = new CardFactory(_cardPrefab);
                cardViewRegistry = new CardViewRegistry();
                cardFactory.OnCardCreated += cardViewRegistry.Register;
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