using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        [SerializeField] private AudioClip[] alarmSongs;
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private GameObject alarmCanvas;
        private AudioClip _chosenClip;

        private AudioSource _audioSource;
        private int _chosenIndex = 0;

        private string savePath;

        private void Awake()
        {
            savePath = Application.persistentDataPath + "/audio.dat";
            Instance = this;
            _audioSource = GetComponent<AudioSource>();
            LoadData();
            if (_chosenIndex < 0 || _chosenIndex > alarmSongs.Length - 1)
            {
                _chosenIndex = -1;
                _chosenClip = null;

            }
            _chosenClip = alarmSongs[_chosenIndex];
            SetToggleDefault();
        }

        public int GetSelectedToggle()
        {
            var selection = toggleGroup.ActiveToggles().FirstOrDefault();
            if (selection == default) return -1;

            return Convert.ToInt32(selection.name);
        }

        public void SetToggleDefault()
        {
            if (_chosenIndex != -1)
                toggleGroup.transform.GetChild(0).GetChild(_chosenIndex).GetComponent<Toggle>().isOn = true;
        }
        public void PlayClip(int index)
        {
            _audioSource.Stop();
            _audioSource.clip = alarmSongs[index];
            _audioSource.Play();
        }

        public void StopClip()
        {
            _audioSource.Stop();
        }
        public AudioClip GetChosenClip()
        {
            return _chosenClip;
        }

        public void SetAudioClip(int index)
        {
            if (index == -1)
            {
                _chosenClip = null;
                return;
            }
            _chosenClip = alarmSongs[index];
        }

        public void OnSave()
        {
            _audioSource.Stop();
            var index = GetSelectedToggle();
            SetAudioClip(index);
            alarmCanvas.SetActive(false);
            _chosenIndex = index;
            SaveData();
        }

        private void SaveData()
        {
            FileStream fs = new FileStream(savePath, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, _chosenIndex);
            fs.Close();
        }
        private void LoadData()
        {
            if (!File.Exists(savePath)) return;
            using (Stream stream = File.Open(savePath, FileMode.Open))
            {
                var bf = new BinaryFormatter();

                _chosenIndex = (int)bf.Deserialize(stream);
            }
        }
    }
}
