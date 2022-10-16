using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SkillRange : MonoBehaviour
{

    public DecalProjector projector;
    // Start is called before the first frame update
    void Start()
    {
        projector = this.GetComponent<DecalProjector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!UpdataManager.Instance.skillMarkOpen)
        {
            projector.enabled = false;
        }
        else
        {
            projector.enabled = true;
        }
    }
}
