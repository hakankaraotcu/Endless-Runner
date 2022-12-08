using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Portals/MaturePortal")]
public class MaturePortal : PortalEffect
{
    public override void Apply()
    {
        PlayerChanger.GetInstance().mature.SetActive(true);
        PlayerChanger.GetInstance().young.SetActive(false);
    }
}
