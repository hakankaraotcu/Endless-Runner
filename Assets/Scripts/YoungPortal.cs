using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Portals/YoungPortal")]
public class YoungPortal : PortalEffect
{
    public override void Apply()
    {
        PlayerChanger.GetInstance().young.SetActive(true);
        PlayerChanger.GetInstance().mature.SetActive(false);
    }
}
