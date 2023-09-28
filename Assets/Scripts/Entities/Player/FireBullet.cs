using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public FiringType firingType;

    public float velocity = 30.0f;

    //how wide the fire wave is
    public int waveWidth = 45;

    //how fast the fire waves
    public static int rateOfWave = 25; 
    private static int waveAngle = 0;

    public int damage = 10;

    //lowest and highest direction flame can spawn
    [Header ("Vertical direction of flames")]
    public int maxRange, minRange = 0;

    private Rigidbody2D fireParticle;

    // Start is called before the first frame update
    void Start()
    {
        fireParticle = GetComponent<Rigidbody2D>();

        switch((int)firingType)
        {
            case(0):
            {
                //changes rotation, scattering shots
                transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + waveAngle);

                waveAngle += rateOfWave;

                if(waveAngle >= waveWidth || waveAngle <= -waveWidth)
                {
                    rateOfWave *= -1;
                }
            }
            break;

            case(1):
            {
                //changes rotation, scattering shots
                transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + Random.Range(minRange, maxRange));
            }
            break;
        }

        //speed of projectile
        fireParticle.velocity = transform.right * velocity;
    }

    public enum FiringType
    {
        wave,
        flameThrower
    }
}
