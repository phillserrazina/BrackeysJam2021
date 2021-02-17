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

        // EXECUTION FUNCTIONS
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
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                if (queues[0].Count > 0) {
                    var chosen = queues[0].Dequeue();
                    Use(chosen);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                if (queues[1].Count > 0) {
                    var chosen = queues[1].Dequeue();
                    Use(chosen);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                if (queues[2].Count > 0) {
                    var chosen = queues[2].Dequeue();
                    Use(chosen);
                }
            }
        }

        private void Use(Recruitable chosen) {
            chosen.Use(boss);
            player.Remove(chosen);
            RecruitsGroupMovementManager.Instance.Remove(chosen);
        }
    }
}
