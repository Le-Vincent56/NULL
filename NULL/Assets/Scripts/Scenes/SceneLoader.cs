using System.Threading.Tasks;
using UnityEngine;

namespace NULL.Scenes
{
    public class SceneLoader : MonoBehaviour
    {
        [Header("Scenes")]
        [SerializeField] private SceneGroupData sceneGroupData;
        private bool isLoading;

        public SceneGroupManager Manager;
        public SceneGroup[] SceneGroups { get => sceneGroupData.SceneGroups; }

        public bool IsLoading { get => isLoading; }

        private void Awake()
        {
            Manager = new SceneGroupManager();
        }

        private async void Start()
        {
            await LoadSceneGroup(sceneGroupData.InitialScene);
        }

        /// <summary>
        /// Load a Scene Group
        /// </summary>
        public async Task LoadSceneGroup(int index)
        {
            // Exit case - the index is not valid
            if (index < 0 || index >= sceneGroupData.SceneGroups.Length) return;

            // Create a new LoadingProgress instance
            LoadingProgress progress = new LoadingProgress();

            // Start loading
            HandleLoading(true);

            await Manager.LoadScenes(index, sceneGroupData.SceneGroups[index], progress);
        }

        /// <summary>
        /// Handle loading for the Scene Loader
        /// </summary>
        private void HandleLoading(bool isLoading)
        {
            // Set isLoading
            this.isLoading = isLoading;

            // Check if loading
            if (!isLoading)
            {
                // TODO: Fade in the scene
            }
        }
    }
}
