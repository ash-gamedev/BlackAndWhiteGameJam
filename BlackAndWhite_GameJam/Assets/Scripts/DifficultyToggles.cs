using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class DifficultyToggles : MonoBehaviour
    {
        public GameObject DifficultyToggleGameObject;
        GameController gameController = null; 

        // Use this for initialization
        void Start()
        {
            gameController = FindObjectOfType<GameController>();
            DifficultyToggleGameObject.transform.GetChild((int)gameController.DifficultySetting).GetComponent<Toggle>().isOn = true;
        }

        public void SetEasyDifficulty()
        {
            if (gameController != null)
                gameController.SetDifficultySetting(EnumDifficulty.Easy);
        }

        public void SetNormalDifficulty()
        {
            if (gameController != null)
                gameController.SetDifficultySetting(EnumDifficulty.Normal);
        }

        public void SetHardDifficulty()
        {
            if (gameController != null)
                gameController.SetDifficultySetting(EnumDifficulty.Hard);
        }
    }
}