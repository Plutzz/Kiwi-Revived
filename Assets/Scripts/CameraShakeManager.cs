using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeManager : Singleton<CameraShakeManager>
{
    [Serialized]
    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce();
    }
}
