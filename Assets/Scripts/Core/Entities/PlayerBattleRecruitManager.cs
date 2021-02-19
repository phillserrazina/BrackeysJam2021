using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace BrackeysJam.Core.Entities
{
    public class PlayerBattleRecruitManager : MonoBehaviour
    {
        // VARIABLES
        public IEnumerable<RecruitableTypes> RecruitTypes {
            get {
                return Enum.GetValues(typeof(RecruitableTypes)).Cast<RecruitableTypes>();
            } 
        }

        [SerializeField] private Transform spawnPosition = null;
        [SerializeField] private PlayerRecruitManager player = null;
        [SerializeField] private Transform boss = null;
        [SerializeField] private Recruitable[] recruitablePrefabs = null;

        private List<Queue<Recruitable>> queues = new List<Queue<Recruitable>>();

        private float[] cooldowns;

        private float[] attackCooldowns = { 0.2f, 1f, 3f, 5f };

        public static PlayerBattleRecruitManager Instance { get; private set; }

        // EXECUTION FUNCTIONS
        private void Awake() {
            Instance = this;
            cooldowns = new float[attackCooldowns.Length];
        }

        private void Start()
        {
            var recruitArrays = RecruitTypes.ToArray();
            for (int i = 0; i < recruitArrays.Length; i++) 
            {
                int amountToSpawn = PlayerPrefs.GetInt($"{recruitArrays[i].ToString()}");
                Debug.Log("Should spawn " + amountToSpawn + " " + recruitArrays[i].ToString());
            
                var toSpawn = recruitablePrefabs.Where(r => r.Type == recruitArrays[i]).ToArray()[0];
                queues.Add(new Queue<Recruitable>());

                for (int j = 0; j < amountToSpawn; j++) 
                {
                    var spawnedObject = Instantiate(toSpawn, spawnPosition.position, spawnPosition.rotation);
                    spawnedObject.transform.SetParent(transform);

                    spawnedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

                    player.Recruit(spawnedObject);
                    queues[i].Enqueue(spawnedObject);

                    RecruitsGroupMovementManager.Instance.Add(spawnedObject);
                }
            }
        }

        private void Update() {
            for (int i = 0; i < cooldowns.Length; i++) {
                if (cooldowns[i] > 0f) {
                    cooldowns[i] -= Time.deltaTime;
                }
            }

            int index = -1;

            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                index = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                index = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                index = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4)) {
                index = 3;
            }

            if (index == -1 || cooldowns[index] > 0f) return;

            if (queues[index].Count > 0) {
                Recruitable chosen = queues[index].Dequeue();

                while (chosen == null) {
                    if (queues[index].Count <= 0) break;
                    
                    chosen = queues[index].Dequeue();
                }

                Use(chosen);
                
                cooldowns[index] = attackCooldowns[index];
                index = -1;
            }
        }

        private void Use(Recruitable chosen) {
            if (chosen == null) {
                player.Remove(chosen);
                RecruitsGroupMovementManager.Instance.Remove(chosen);
                return;
            }

            chosen.Use(boss);
            player.Remove(chosen);
            RecruitsGroupMovementManager.Instance.Remove(chosen);
        }

        public void Requeue(Recruitable r) {
            var recruitArrays = RecruitTypes.ToArray();
            for (int i = 0; i < recruitArrays.Length; i++) 
            {
                if (r.Type == recruitArrays[i]) {
                    queues[i].Enqueue(r);
                }
            }
        }
    }
}
