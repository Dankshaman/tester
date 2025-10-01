using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x0200022B RID: 555
[Serializable]
public class PhysicsState : ISerializable
{
	// Token: 0x06001B9F RID: 7071 RVA: 0x00002594 File Offset: 0x00000794
	public PhysicsState()
	{
	}

	// Token: 0x06001BA0 RID: 7072 RVA: 0x000BE4D0 File Offset: 0x000BC6D0
	public PhysicsState(SerializationInfo info, StreamingContext ctxt)
	{
		this.name = (string)info.GetValue("name", typeof(string));
		this.idint = (int)info.GetValue("idint", typeof(int));
		this.posX = (float)info.GetValue("posX", typeof(float));
		this.posY = (float)info.GetValue("posY", typeof(float));
		this.posZ = (float)info.GetValue("posZ", typeof(float));
		this.rotX = (float)info.GetValue("rotX", typeof(float));
		this.rotY = (float)info.GetValue("rotY", typeof(float));
		this.rotZ = (float)info.GetValue("rotZ", typeof(float));
		this.MatInt = (int)info.GetValue("MatInt", typeof(int));
		this.MeshInt = (int)info.GetValue("MeshInt", typeof(int));
		this.LayerInt = (int)info.GetValue("LayerInt", typeof(int));
		this.Num = (int)info.GetValue("Num", typeof(int));
		this.bAltSound = (bool)info.GetValue("bAltSound", typeof(bool));
		this.List = (List<int>)info.GetValue("List", typeof(List<int>));
		this.ModeState = (Mode)info.GetValue("ModeState", typeof(Mode));
		this.Note = (string)info.GetValue("Note", typeof(string));
		this.PlayerTurn = (string)info.GetValue("PlayerTurn", typeof(string));
		this.StringList = (List<string>)info.GetValue("StringList", typeof(List<string>));
		this.CustomList = (List<string>)info.GetValue("CustomList", typeof(List<string>));
	}

	// Token: 0x06001BA1 RID: 7073 RVA: 0x000BE744 File Offset: 0x000BC944
	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		info.AddValue("name", this.name);
		info.AddValue("idint", this.idint);
		info.AddValue("posX", this.posX);
		info.AddValue("posY", this.posY);
		info.AddValue("posZ", this.posZ);
		info.AddValue("rotX", this.rotX);
		info.AddValue("rotY", this.rotY);
		info.AddValue("rotZ", this.rotZ);
		info.AddValue("List", this.List);
		info.AddValue("MatInt", this.MatInt);
		info.AddValue("MeshInt", this.MeshInt);
		info.AddValue("LayerInt", this.LayerInt);
		info.AddValue("Num", this.Num);
		info.AddValue("bAltSound", this.bAltSound);
		info.AddValue("ModeState", this.ModeState);
		info.AddValue("Note", this.Note);
		info.AddValue("PlayerTurn", this.PlayerTurn);
		info.AddValue("StringList", this.StringList);
		info.AddValue("CustomList", this.CustomList);
	}

	// Token: 0x0400115E RID: 4446
	public string name;

	// Token: 0x0400115F RID: 4447
	public int idint;

	// Token: 0x04001160 RID: 4448
	public float posX;

	// Token: 0x04001161 RID: 4449
	public float posY;

	// Token: 0x04001162 RID: 4450
	public float posZ;

	// Token: 0x04001163 RID: 4451
	public float rotX;

	// Token: 0x04001164 RID: 4452
	public float rotY;

	// Token: 0x04001165 RID: 4453
	public float rotZ;

	// Token: 0x04001166 RID: 4454
	public int MatInt;

	// Token: 0x04001167 RID: 4455
	public int MeshInt;

	// Token: 0x04001168 RID: 4456
	public int LayerInt;

	// Token: 0x04001169 RID: 4457
	public int Num;

	// Token: 0x0400116A RID: 4458
	public bool bAltSound;

	// Token: 0x0400116B RID: 4459
	public List<int> List;

	// Token: 0x0400116C RID: 4460
	public Mode ModeState;

	// Token: 0x0400116D RID: 4461
	public string Note;

	// Token: 0x0400116E RID: 4462
	public string PlayerTurn;

	// Token: 0x0400116F RID: 4463
	public List<string> StringList;

	// Token: 0x04001170 RID: 4464
	public List<string> CustomList;
}
