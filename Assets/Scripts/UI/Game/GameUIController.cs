using Common.MVB;
using ExitGames.Client.Photon;
using Photon.Pun;
using Tanks.Extensions;
using UnityEngine;

namespace Tanks.UI
{
    public class GameUIController : MonoBehaviourPunCallbacks
    {
        [field: SerializeField]
        public GameProperties GameProperties { get; private set; }

        [field: SerializeField]
        public ScriptableKeyCode ExitKeyCode { get; private set; }
        [field: SerializeField]
        public ScriptableKeyCode ScoresKeyCode { get; private set; }

        [field: SerializeField]
        public GameObject ExitPanel { get; private set; }
        [field: SerializeField]
        public GameObject ScoresPanel { get; private set; }

        private GameObject _currentPanel;

        private void ChangeToPanel(GameObject panel)
        {
            _currentPanel?.SetActive(false);
            _currentPanel = panel;
            _currentPanel?.SetActive(true);
        }

        private void OnGameFinished()
        {
            ChangeToPanel(ScoresPanel);
        }

        #region Photon methods
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.TryGetTeamWon(out _))
            {
                OnGameFinished();
            }
        }
        #endregion

        #region Unity methods
        private void Update()
        {
            if (Input.GetKeyDown(ExitKeyCode))
            {
                ChangeToPanel(ExitPanel.activeSelf ? null : ExitPanel);
            }

            if (Input.GetKeyDown(ScoresKeyCode))
            {
                ChangeToPanel(ScoresPanel);
            }
            if (Input.GetKeyUp(ScoresKeyCode))
            {
                ChangeToPanel(null);
            }
        }
        #endregion
    }
}
