
using UnityEngine;
using System.Collections;

public class LineRendererScript : Singleton<LineRendererScript>
{
	[SerializeField]
	private LineRenderer BodyLineRenderer;

	private bool IsReady = false;
	public Vector3 StartPos;
	public Vector3 EndPos;
	public AnimationCurve curve;
	public float Height;
    protected override void Awake()
	{
		base.Awake();
	}
	public void StartPosSet()
    {
		IsReady = true;
		StartPos = DataSave.Instance.currentObj.transform.position;
		BodyLineRenderer.enabled = true;
	}
	public void EndPosSet(Vector3 pos)
    {
		EndPos = pos;
    }
	public void EndLine()
    {
		IsReady = false;
		BodyLineRenderer.enabled = false;
	}
	void Update()
	{
        if (!UpdataManager.Instance.skillLineOpen)
        {
			EndLine();			
        }
      
		if (UpdataManager.Instance.moveButtonPushed)
        {
			BodyLineRenderer.material.color = Color.blue;
			IsReady = true;
        }
        else
        {
			BodyLineRenderer.material.color = Color.yellow;
		}
		if (IsReady)
		{
			BodyLineRenderer.positionCount = 24;
			//	BodyLineRenderer.SetPosition(0, StartPos);
			//	BodyLineRenderer.SetPosition(1, EndPos);
			//¶þ´ÎB(t)=(1?t)2 P0+2t(1?t) P1+t2P2, t¡Ê[0,1] t= (i/ BodyLineRenderer.positionCount)
			//t 0,1   /24
			for (float i = 0; i < BodyLineRenderer.positionCount; i++)
            {
				Vector3 temp = Mathf.Pow((1 - (i / BodyLineRenderer.positionCount)), 2) * StartPos + 2 * (i / BodyLineRenderer.positionCount) * (1 - (i / BodyLineRenderer.positionCount)) * ((StartPos + EndPos) / 2 + Vector3.up * Height) +
					Mathf.Pow((i / BodyLineRenderer.positionCount), 2) * EndPos;
				BodyLineRenderer.SetPosition((int)i, temp);
			}
			BodyLineRenderer.widthCurve = curve;


		}
	}

}
