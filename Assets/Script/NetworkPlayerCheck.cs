using System.Collections;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
public class NetworkPlayerCheck : Photon.PunBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!photonView.isMine){
        	GetComponent<ThirdPersonUserControl>().enabled = false;
        	
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
}