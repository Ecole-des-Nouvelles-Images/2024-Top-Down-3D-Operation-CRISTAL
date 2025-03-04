using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Convoy.Drones;
using Game.Player;
using UnityEngine;

namespace Game.Convoy.Modules
{
    public class DroneController : Module
    {
        [Header("Drone specifics")] 
        [SerializeField] private GameObject _dronePrefab;
        [SerializeField] private Transform _droneParent;
        [SerializeField] private List<Anchor> _anchors;
        [Space]
        [SerializeField] private float _droneMoveSpeed;
        [SerializeField] [Tooltip("Crystals mined per seconds")] private float _droneMiningSpeed;
        [SerializeField] private int _droneArmorPlates = 2;
        [SerializeField] private float _droneRebuildTime;
        [SerializeField] private bool _enableDroneAutoRebuild;

        public static float DroneMoveSpeed;
        public static float DroneMiningSpeed;
        public static int DroneArmorPlates;
        
        private List<Drone> _drones;

        protected override void Awake()
        {
            base.Awake();
            _drones = new List<Drone>(MaximumControllers);
            DroneMoveSpeed = _droneMoveSpeed;
            DroneMiningSpeed = _droneMiningSpeed;
            DroneArmorPlates = _droneArmorPlates;
        }

        private void Start()
        {
            if (_enableDroneAutoRebuild)
                StartCoroutine(DroneAutoRebuildWatcher());
        }

        protected override void Update()
        {
            if (!Online)
            {
                foreach (Drone drone in _drones)
                {
                    drone.Active = false;
                }
                
                Deactivate();
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();
            StopCoroutine(DroneAutoRebuildWatcher());
        }

        #region Actions

        public override bool EnterModule(PlayerController pilot)
        {
            if (!base.EnterModule(pilot)) {
                return false;
            }
            
            ActivateDrone(pilot);
            return true;
        }

        public override bool ExitModule(PlayerController pilot)
        {
            DeactivateDrone(pilot);
            return base.ExitModule(pilot);
        }
        
        public override void Operate(PlayerController currentController)
        {
            Drone userDrone = _drones.Find(drone => drone.Pilot == currentController);

            userDrone.Operating = !userDrone.Operating;
        }

        public override void Interact(PlayerController currentController)
        {
            Drone userDrone = _drones.Find(drone => drone.Pilot == currentController);

            userDrone.Interacting = !userDrone.Interacting;
        }

        #endregion

        #region Drone Management

        private void ActivateDrone(PlayerController pilot)
        {
            Drone userDrone = _drones.Find(drone => drone.IsRegistered && drone.Pilot == pilot);

            
            if (!userDrone) 
                userDrone = RegisterNewDrone(pilot);

            if (!userDrone) // If userDrone still unassigned after registering;
                return;
            
            userDrone.Active = true;
        }

        private void DeactivateDrone(PlayerController pilot)
        {
            Drone userDrone = _drones.Find(drone => drone.IsRegistered && drone.Pilot == pilot);
            
            if (!userDrone) return;
            
            userDrone.Operating = false;
            userDrone.Interacting = false;
            userDrone.Active = false;
        }

        private Drone RegisterNewDrone(PlayerController pilot)
        {
            Drone userDrone = _drones.Find(drone => !drone.IsRegistered);
            
            if (!userDrone) {
                Debug.Log("No available drone to control");
                return null;
            }
                
            userDrone.RegisterPilot(pilot);
            userDrone.MaterialInstance.color = pilot.PlayerColor;
            return userDrone;
        }

        public void UnregisterDrone(Drone drone)
        {
            if (! _drones.Remove(drone))
                throw new Exception($"Error: trying to remove a rogue drone. ID #{drone.ID} wasn't registered");
        }

        private IEnumerator DroneAutoRebuildWatcher()
        {
            while (Online)
            {
                if (_drones.Count < 4)
                {
                    yield return new WaitForSeconds(_droneRebuildTime);
                    
                    Anchor availableAnchor = _anchors.First(anchor => anchor.Available);
                    
                    if (!availableAnchor)
                        throw new Exception("No suitable drone anchor found when at least one should be.");

                    Drone drone = Instantiate(_dronePrefab, availableAnchor.Position, Quaternion.identity, _droneParent).GetComponent<Drone>();
                    availableAnchor.Occupant = drone;
                    drone.name = "Drone_" + drone.ID;
                    drone.DroneController = this;
                    drone.AssignedAnchor = availableAnchor;
                    drone.ArmorPlates = DroneArmorPlates;
                    _drones.Add(drone);
                }

                yield return null;
            }
        }

        #endregion
    }
}
