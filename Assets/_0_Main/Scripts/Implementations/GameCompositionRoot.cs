using UnityEngine;

public class GameCompositionRoot : MonoBehaviour
{
        [SerializeField] private GameObject InputProviders;
        private IInputProvider inputProvider;

        private void Awake()
        {
                SetUpInput();
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

                inputProvider.Enable();

                inputProvider.OnFlipRequest += pos =>
                {

                };
        }

}