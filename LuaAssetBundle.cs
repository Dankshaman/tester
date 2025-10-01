using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;

// Token: 0x02000170 RID: 368
public class LuaAssetBundle : LuaComponent
{
	// Token: 0x06001197 RID: 4503 RVA: 0x00078CA7 File Offset: 0x00076EA7
	[MoonSharpHidden]
	public LuaAssetBundle(LuaGameObjectScript LGOS) : base(LGOS)
	{
	}

	// Token: 0x06001198 RID: 4504 RVA: 0x00079112 File Offset: 0x00077312
	[MoonSharpHidden]
	public Table EffectToTable(TTSAssetBundleEffects.TTSEffect effect, int index, Script script)
	{
		Table table = new Table(script);
		table["index"] = index;
		table["name"] = effect.Name;
		return table;
	}

	// Token: 0x06001199 RID: 4505 RVA: 0x0007913C File Offset: 0x0007733C
	public bool playLoopingEffect(int index)
	{
		this.LGOS.NPO.customAssetbundle.RPCLoopEffect(index);
		return true;
	}

	// Token: 0x0600119A RID: 4506 RVA: 0x00079155 File Offset: 0x00077355
	public bool playTriggerEffect(int index)
	{
		this.LGOS.NPO.customAssetbundle.RPCTriggerEffect(index);
		return true;
	}

	// Token: 0x0600119B RID: 4507 RVA: 0x00079170 File Offset: 0x00077370
	public Table getLoopingEffects(Script script)
	{
		Table table = new Table(script);
		List<TTSAssetBundleEffects.TTSLoopingEffect> loopingEffects = this.LGOS.NPO.customAssetbundle.assetBundleEffects.LoopingEffects;
		if (loopingEffects == null || loopingEffects.Count == 0)
		{
			return null;
		}
		for (int i = 0; i < loopingEffects.Count; i++)
		{
			TTSAssetBundleEffects.TTSEffect effect = loopingEffects[i];
			table[i + 1] = this.EffectToTable(effect, i, script);
		}
		return table;
	}

	// Token: 0x0600119C RID: 4508 RVA: 0x000791E0 File Offset: 0x000773E0
	public Table getTriggerEffects(Script script)
	{
		Table table = new Table(script);
		List<TTSAssetBundleEffects.TTSTriggerEffect> triggerEffects = this.LGOS.NPO.customAssetbundle.assetBundleEffects.TriggerEffects;
		if (triggerEffects == null || triggerEffects.Count == 0)
		{
			return null;
		}
		for (int i = 0; i < triggerEffects.Count; i++)
		{
			TTSAssetBundleEffects.TTSEffect effect = triggerEffects[i];
			table[i + 1] = this.EffectToTable(effect, i, script);
		}
		return table;
	}

	// Token: 0x0600119D RID: 4509 RVA: 0x0007924D File Offset: 0x0007744D
	public int getLoopingEffectIndex()
	{
		return this.LGOS.NPO.customAssetbundle.LoopEffectIndex;
	}
}
