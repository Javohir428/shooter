using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

namespace Assets
{
	public sealed class Game : MonoBehaviour 
	{
        private int _score;
        public Player _playerPrefab;
        public Spawner _spawnerPrefab;

        public UnityEvent _onGameOver;

        public Text _scoreText;
        public Button _restartButton;
        private Spawner _spawner;

        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
                _scoreText.text = value.ToString();
            }
        }

        public void NewGame()
        {
            StartCoroutine(DimLights());
            Score = 0;
            _restartButton.gameObject.SetActive(false);

            var p = Instantiate(_playerPrefab);
            p._onDead.AddListener(GameOver);
            p._onKill.AddListener(() => Score++);
            p._enemyRegister = _spawner.transform;

            _spawner.enabled = true;
        }

        private void GameOver()
        {
            StopAllCoroutines();
            RenderSettings.ambientLight = Color.white;
            _onGameOver.Invoke();
            _restartButton.gameObject.SetActive(true);
        }

        private void Start()
        {
            _spawner = Instantiate(_spawnerPrefab);
            _onGameOver.AddListener(() => _spawner.enabled = false);

            NewGame();
            _restartButton.onClick.AddListener(NewGame);
		}

        private IEnumerator DimLights()
        {
            const float duration = 2;
            float timer = 2;
            while(timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
                RenderSettings.ambientLight = Color.white * (timer / duration);
            }
        }
	}
}