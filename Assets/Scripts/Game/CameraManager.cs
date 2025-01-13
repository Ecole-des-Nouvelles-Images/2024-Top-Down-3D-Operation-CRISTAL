using System;
using UnityEngine;
using Unity.Cinemachine;
using Internal;

namespace Game
{
    public class CameraManager: SingletonMonoBehaviour<CameraManager>
    {
        [Header("Camera systems")]
        public GameObject LeftCam;
        public GameObject RightCam;
        public GameObject FollowCam;

        [Header("Settings")]
        public float TransitShakeAmplitude;
        
        public GameObject CurrentCamera { get; private set; }

        private void Start()
        {
            CinemachineCamera vcam = FollowCam.GetComponent<CinemachineCamera>();
            vcam.GetComponent<CinemachineBasicMultiChannelPerlin>().AmplitudeGain = TransitShakeAmplitude;
        }

        public void SwitchCameraFocus(bool switchToFollowCamera, Side mode)
        {
            FollowCam.SetActive(switchToFollowCamera);

            switch (mode)
            {
                case Side.Left:
                    LeftCam?.SetActive(true);
                    RightCam?.SetActive(false);
                    CurrentCamera = LeftCam;
                    break;
                case Side.Right:
                    LeftCam?.SetActive(false);
                    RightCam?.SetActive(true);
                    CurrentCamera = RightCam;
                    break;
                case Side.None:
                    // Change nothing...;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }

            /* if (useFarthermostCamera)
                FindCameraByFOV(false).SetActive(false);
            else
                FindCameraByFOV(true).SetActive(false); */
        }

        private GameObject FindCameraByFOV(bool findFarthermost)
        {
            if (CurrentCamera is null) return null;

            GameObject cameraToFind = null;
            float fov = (findFarthermost) ? Mathf.NegativeInfinity : Mathf.Infinity;
            
            foreach (Transform child in CurrentCamera.transform)
            {
                CinemachineCamera cam = child.GetComponent<CinemachineCamera>();

                if (!cam) continue;

                if (findFarthermost)
                {
                    if (!(cam.Lens.FieldOfView > fov)) continue;
                    
                    cameraToFind = cam.gameObject;
                    fov = cam.Lens.FieldOfView;
                }
                else
                {
                    if (!(cam.Lens.FieldOfView < fov)) continue;
                    
                    cameraToFind = cam.gameObject;
                    fov = cam.Lens.FieldOfView;
                }
            }
            
            return cameraToFind;
        }
    }
}
