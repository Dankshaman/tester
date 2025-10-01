using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using MoonSharp.Interpreter;
using NewNet;
using UnityEngine;

// Token: 0x020001A0 RID: 416
public class LuaGlobalScriptManager : LuaScript
{
	// Token: 0x14000055 RID: 85
	// (add) Token: 0x06001449 RID: 5193 RVA: 0x00084EC8 File Offset: 0x000830C8
	// (remove) Token: 0x0600144A RID: 5194 RVA: 0x00084EFC File Offset: 0x000830FC
	public static event LuaGlobalScriptManager.ExternalCustomMessage OnExternalCustomMessage;

	// Token: 0x0600144B RID: 5195 RVA: 0x00084F2F File Offset: 0x0008312F
	public static void TriggerExternalCustomMessage(Dictionary<object, object> message)
	{
		LuaGlobalScriptManager.ExternalCustomMessage onExternalCustomMessage = LuaGlobalScriptManager.OnExternalCustomMessage;
		if (onExternalCustomMessage == null)
		{
			return;
		}
		onExternalCustomMessage(message);
	}

	// Token: 0x0600144C RID: 5196 RVA: 0x00084F44 File Offset: 0x00083144
	protected override void Awake()
	{
		this.RegisterTypes();
		this.loaded = true;
		base.Awake();
		this.GlobalDummyObject = base.gameObject.AddComponent<LuaGameObjectScript>();
		this.GlobalDummyObject.IsGlobalDummyObject = true;
		this.GlobalDummyObject.script_code = this.script_code;
		this.GlobalDummyObject.script_state = this.script_state;
		this.Reset();
		this.loaded = true;
		this.copyObjectStates = new List<ObjectState>();
		this.GlobalPlayer = new LuaGlobalPlayer();
		this.GlobalTimer = new LuaTimer();
		this.GlobalLighting = new LuaLighting();
		this.GlobalGrid = new LuaGrid();
		this.GlobalTurns = new LuaTurns();
		this.GlobalPhysics = new LuaPhysics();
		this.GlobalWebRequest = base.GetComponent<LuaWebRequestManager>();
		base.XmlUI = base.GetComponent<XmlUIScript>();
		this.GlobalUI = new LuaUI(base.XmlUI);
		this.GlobalNotes = new LuaNotes();
		this.GlobalWait = new LuaWait();
		this.GlobalMusicPlayer = new LuaMusicPlayer();
		this.GlobalTime = new LuaTime();
		this.GlobalHands = new LuaHands();
		this.GlobalTables = new LuaTables();
		this.GlobalBackgrounds = new LuaBackgrounds();
		this.GlobalInfo = new LuaInfo();
		EventManager.OnFileSave += this.OnFileSave;
		EventManager.OnNetworkObjectSpawnFromUI += this.OnNetworkObjectSpawnFromUI;
	}

	// Token: 0x0600144D RID: 5197 RVA: 0x000850A0 File Offset: 0x000832A0
	protected override void Start()
	{
		base.Start();
		this.script_code = "--[[ Lua code. See documentation: https://api.tabletopsimulator.com/ --]]\n\n--[[ The onLoad event is called after the game save finishes loading. --]]\nfunction onLoad()\n    --[[ print('onLoad!') --]]\nend\n\n--[[ The onUpdate event is called once per frame. --]]\nfunction onUpdate()\n    --[[ print('onUpdate loop!') --]]\nend";
	}

	// Token: 0x0600144E RID: 5198 RVA: 0x000850B3 File Offset: 0x000832B3
	protected override void OnDestroy()
	{
		EventManager.OnFileSave -= this.OnFileSave;
		EventManager.OnNetworkObjectSpawnFromUI -= this.OnNetworkObjectSpawnFromUI;
		base.OnDestroy();
		this.CleanupLuaJob();
	}

	// Token: 0x0600144F RID: 5199 RVA: 0x000850E3 File Offset: 0x000832E3
	public void OnApplicationQuit()
	{
		this.CleanupLuaJob();
	}

	// Token: 0x06001450 RID: 5200 RVA: 0x000850EC File Offset: 0x000832EC
	protected override void Update()
	{
		base.Update();
		if (!UICamera.SelectIsInput())
		{
			for (int i = 1; i <= zInput.ScriptingButtons.Count; i++)
			{
				if (zInput.GetScriptingButtonDown(i))
				{
					base.networkView.RPC<int>(RPCTarget.Server, new Action<int>(this.ScriptingButtonDown), i);
				}
				if (zInput.GetScriptingButtonUp(i))
				{
					base.networkView.RPC<int>(RPCTarget.Server, new Action<int>(this.ScriptingButtonUp), i);
				}
			}
		}
	}

	// Token: 0x06001451 RID: 5201 RVA: 0x00085160 File Offset: 0x00083360
	private void RegisterTypes()
	{
		UserData.RegisterType<LuaGameObjectScript>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaClock>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaPlayer>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaCounter>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaRPGFigurine>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaBook>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaBrowser>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaTextTool>(InteropAccessMode.Default, null);
		UserData.RegisterType(LuaGlobalPlayer.UserDataDescriptor);
		UserData.RegisterType<LuaAssetBundle>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaLighting>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaGrid>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaPhysics>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaWebRequestManager>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaWebRequest>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaTurns>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaUI>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaNotes>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaWait>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaMusicPlayer>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaTime>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaHands>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaTables>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaBackgrounds>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaInfo>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaTags>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaContainer>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaDeck>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaCard>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaStack>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaStackObject>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaBag>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaInfiniteBag>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaZone>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaLayoutZone>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaScriptWrapper>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaGameObjectReference>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaComponentReference>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaMaterialReference>(InteropAccessMode.Default, null);
		UserData.RegisterType<LuaTimer>(InteropAccessMode.Default, null);
		LuaCustomConverter.RegisterConvertionTypeToTable<Vector3>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<Vector4>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<Color>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<Vector3?>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<Vector4?>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<Color?>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaCast>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaHit>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaBoundsState>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaTransform>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaUIButtonState>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaUIInputState>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaNotes.LuaNotebookParameters>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaGameObjectScript.LuaRotationValue>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaGlobalScriptManager.LuaObjectParameters>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaGlobalScriptManager.LuaJsonObjectParameters>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaGlobalScriptManager.LuaDataObjectParameters>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaGameObjectScript.LuaTakeObjectParameters>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaGameObjectScript.LuaSnapPointParameters>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaGameObjectScript.LuaJointParameters>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaPlayer.LuaCameraParameters>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaUI.LuaCustomAsset>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaGameObjectScript.LuaVectorLine>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaGameObjectScript.LuaDecal>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaGameObjectScript.LUAFogOfWarReveal>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaGameObjectScript.LUAFogOfWar>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaLayoutZoneOptions>(false, false);
		LuaCustomConverter.RegisterConvertionStringToType<Color>();
		LuaCustomConverter.RegisterConvertionStringToType<Color?>();
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaGlobalScriptManager.ObjectInfo>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaGlobalScriptManager.FunctionInfo>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<LuaGlobalScriptManager.VariableInfo>(false, false);
		LuaCustomConverter.RegisterConvertionTypeToTable<SaveState>(true, true);
	}

	// Token: 0x06001452 RID: 5202 RVA: 0x00085394 File Offset: 0x00083594
	[Remote(SendType.ReliableNoDelay)]
	private void ScriptingButtonDown(int index)
	{
		string playerColor = Colour.LabelFromColour(NetworkSingleton<PlayerManager>.Instance.ColourFromID((int)Network.sender.id));
		EventManager.TriggerScriptingButtonDown(index, playerColor);
	}

	// Token: 0x06001453 RID: 5203 RVA: 0x000853CC File Offset: 0x000835CC
	[Remote(SendType.ReliableNoDelay)]
	private void ScriptingButtonUp(int index)
	{
		string playerColor = Colour.LabelFromColour(NetworkSingleton<PlayerManager>.Instance.ColourFromID((int)Network.sender.id));
		EventManager.TriggerScriptingButtonUp(index, playerColor);
	}

	// Token: 0x06001454 RID: 5204 RVA: 0x00085402 File Offset: 0x00083602
	private void CleanupLuaJob()
	{
		if (this.luaJob != null)
		{
			this.luaJob.alive = false;
			this.luaJob.server.Stop();
			this.luaJob.Abort();
			this.luaJob = null;
		}
	}

	// Token: 0x06001455 RID: 5205 RVA: 0x0008543C File Offset: 0x0008363C
	public void Reset()
	{
		this.autoLoadOnce = false;
		this.script_code = "--[[ Lua code. See documentation: https://api.tabletopsimulator.com/ --]]\n\n--[[ The onLoad event is called after the game save finishes loading. --]]\nfunction onLoad()\n    --[[ print('onLoad!') --]]\nend\n\n--[[ The onUpdate event is called once per frame. --]]\nfunction onUpdate()\n    --[[ print('onUpdate loop!') --]]\nend";
		this.script_state = "";
		this.lua = new Script(CoreModules.Preset_SoftSandbox);
		this.GlobalDummyObject.lua = this.lua;
		this.GlobalDummyObject.script_code = this.script_code;
		this.GlobalDummyObject.script_state = this.script_state;
		this.copyObjectStates = new List<ObjectState>();
		this.CleanupLuaJob();
		if (this.editorServerCoroutine != null)
		{
			base.StopCoroutine(this.editorServerCoroutine);
			this.editorServerCoroutine = null;
		}
		if (this.GlobalTimer != null && this.GlobalTimer.Timers != null)
		{
			foreach (KeyValuePair<string, UnityEngine.Coroutine> keyValuePair in this.GlobalTimer.Timers)
			{
				if (keyValuePair.Value != null)
				{
					base.StopCoroutine(keyValuePair.Value);
				}
			}
			this.GlobalTimer.Timers.Clear();
		}
		if (this.LuaCoroutines != null)
		{
			foreach (UnityEngine.Coroutine coroutine in this.LuaCoroutines)
			{
				if (coroutine != null)
				{
					base.StopCoroutine(coroutine);
				}
			}
			this.LuaCoroutines.Clear();
		}
		LuaWait globalWait = this.GlobalWait;
		if (globalWait == null)
		{
			return;
		}
		globalWait.StopAll();
	}

	// Token: 0x06001456 RID: 5206 RVA: 0x000855BC File Offset: 0x000837BC
	public void Init()
	{
		if (Network.isClient)
		{
			return;
		}
		this.lua.Options.DebugPrint = delegate(string s)
		{
			Chat.Log(s, Colour.White, ChatMessageType.All, false);
			this.PushLuaPrintMessage(s);
		};
		this.lua.Globals["self"] = this.GlobalDummyObject;
		this.AddFunctions(this.lua);
		try
		{
			if (!string.IsNullOrEmpty(this.script_code) && ConfigGame.Settings.Scripting && this.script_code != "--[[ Lua code. See documentation: https://api.tabletopsimulator.com/ --]]\n\n--[[ The onLoad event is called after the game save finishes loading. --]]\nfunction onLoad()\n    --[[ print('onLoad!') --]]\nend\n\n--[[ The onUpdate event is called once per frame. --]]\nfunction onUpdate()\n    --[[ print('onUpdate loop!') --]]\nend" && !this.OldDefaultGlobalLuaCodes.Contains(this.script_code))
			{
				if (NetworkSingleton<NetworkUI>.Instance.bAutoRunScripts || this.autoLoadOnce)
				{
					base.DoString();
				}
				else
				{
					NetworkSingleton<NetworkUI>.Instance.GUIStartScripts.AddScript(this);
				}
			}
		}
		catch (Exception e)
		{
			base.LogError(e);
		}
		if (this.editorServerCoroutine == null)
		{
			this.editorServerCoroutine = base.StartCoroutine(this.StartLuaEditorServer());
		}
	}

	// Token: 0x06001457 RID: 5207 RVA: 0x000856B4 File Offset: 0x000838B4
	public void AddFunctions(Script lua)
	{
		lua.Globals["Global"] = this.GlobalDummyObject;
		lua.Globals["Player"] = this.GlobalPlayer;
		lua.Globals["JSON"] = lua.DoString(LuaJSON.JSON, null, null);
		lua.Globals["Lighting"] = this.GlobalLighting;
		lua.Globals["Grid"] = this.GlobalGrid;
		lua.Globals["Turns"] = this.GlobalTurns;
		lua.Globals["Physics"] = this.GlobalPhysics;
		lua.Globals["WebRequest"] = this.GlobalWebRequest;
		lua.Globals["UI"] = this.GlobalUI;
		lua.Globals["Notes"] = this.GlobalNotes;
		lua.Globals["Wait"] = this.GlobalWait;
		this.GlobalWait.Collect = lua.DoString("    \r\nreturn function (expected_ids, on_finished, on_add, on_error)\r\n    expected_table = {}\r\n    for k, v in ipairs(expected_ids) do\r\n        expected_table[v] = 0\r\n    end\r\n    return {\r\n        expected = expected_table,\r\n        results = {},\r\n        add = function(self, id, ...)\r\n            if self.expected[id] ~= nil then\r\n                self.expected[id] = self.expected[id] + 1\r\n                if self.expected[id] == 1 then\r\n                    self.results[id] = ...\r\n                    if on_add then on_add(...) end\r\n                    for k, v in pairs(self.expected) do\r\n                        if v == 0 then return end\r\n                    end\r\n                    on_finished(self.results)\r\n                else\r\n                    if on_error then on_error(Wait.COLLECT_DUPLICATE, id, ...) end\r\n                end\r\n            else\r\n                if on_error then on_error(Wait.COLLECT_UNKNOWN, id, ...) end\r\n            end\r\n        end ,\r\n        reset = function(self)\r\n            for k, v in pairs(self.expected) do\r\n                self.expected[k] = 0\r\n            end\r\n            results = {}\r\n        end\r\n    }\r\nend\r\n", null, null);
		lua.Globals["MusicPlayer"] = this.GlobalMusicPlayer;
		lua.Globals["Hands"] = this.GlobalHands;
		lua.Globals["Tables"] = this.GlobalTables;
		lua.Globals["Backgrounds"] = this.GlobalBackgrounds;
		lua.Globals["Info"] = this.GlobalInfo;
		lua.Globals["Time"] = this.GlobalTime;
		lua.Globals["Vector"] = lua.DoString("\r\nlocal Vector = {}\r\nVector.__isVector = true\r\nVector.__version = '1.0.1'\r\n\r\nfunction Vector.new(...)\r\n    local vec = setmetatable({\r\n        x = 0,\r\n        y = 0,\r\n        z = 0\r\n    }, Vector)\r\n    \r\n    local argNum = select('#', ...)\r\n    if argNum == 3 then\r\n        -- Vector.new(x, y, z)\r\n        vec.x, vec.y, vec.z = ...\r\n    elseif argNum == 1 and type(...) == 'table' then\r\n        -- Vector.new(table)\r\n        local src = ...\r\n        vec.x = src.x or src[1] or vec.x\r\n        vec.y = src.y or src[2] or vec.y\r\n        vec.z = src.z or src[3] or vec.z\r\n    end\r\n    \r\n    return vec\r\nend\r\nsetmetatable(Vector, {__call = function(_, ...) return Vector.new(...) end})\r\n\r\ndo\r\n    local remap = {'x', 'y', 'z'}\r\n    function Vector:__index(k)\r\n        k = remap[k] or k\r\n        return rawget(self, k) or Vector[k]\r\n    end\r\n    function Vector:__newindex(k, v)\r\n        k = remap[k] or k\r\n        return rawset(self, k, v)\r\n    end\r\nend\r\n\r\nfunction Vector:setAt(k, v)\r\n    self[k] = v\r\n    return self\r\nend\r\n\r\nfunction Vector:set(x, y, z)\r\n    self.x = x or self.x\r\n    self.y = y or self.y\r\n    self.z = z or self.z\r\n    return self\r\nend\r\n\r\nfunction Vector:get()\r\n    return self.x, self.y, self.z\r\nend\r\n\r\nfunction Vector:copy()\r\n    return Vector(self)\r\nend\r\n\r\nfunction Vector:add(other)\r\n    self.x = self.x + other.x\r\n    self.y = self.y + other.y\r\n    self.z = self.z + other.z\r\n    return self\r\nend\r\n\r\nfunction Vector.__add(v1, v2)\r\n    return v1:copy():add(v2)\r\nend\r\n\r\nfunction Vector:sub(other)\r\n    self.x = self.x - other.x\r\n    self.y = self.y - other.y\r\n    self.z = self.z - other.z\r\n    return self\r\nend\r\n\r\nfunction Vector.__sub(v1, v2)\r\n    return v1:copy():sub(v2)\r\nend\r\n\r\nfunction Vector:dot(other)\r\n    return self.x * other.x\r\n        + self.y * other.y\r\n        + self.z * other.z\r\nend\r\n\r\nfunction Vector:sqrMagnitude()\r\n    return self:dot(self)\r\nend\r\n\r\nfunction Vector:magnitude()\r\n    return math.sqrt(self:dot(self))\r\nend\r\n\r\nfunction Vector:scale(...)\r\n    local sx, sy, sz\r\n    \r\n    local argNum = select('#', ...)\r\n    if argNum == 1 then\r\n        local arg = ...\r\n        if type(arg) == 'number' then\r\n            -- vec:scale(number)\r\n            sx, sy, sz = arg, arg, arg\r\n        else\r\n            -- vec:scale(vector)\r\n            sx, sy, sz = arg:get()\r\n        end\r\n    elseif argNum == 3 then\r\n        -- vec:scale(x, y, z)\r\n        sx, sy, sz = ...\r\n        sx = sx or 1\r\n        sy = sy or 1\r\n        sz = sz or 1\r\n    end\r\n    \r\n    self.x = self.x * sx\r\n    self.y = self.y * sy\r\n    self.z = self.z * sz\r\n    return self\r\nend\r\n\r\nfunction Vector.__mul(val1, val2)\r\n    if type(val1) == 'number' then\r\n        val1, val2 = val2, val1\r\n    end\r\n    return val1:copy():scale(val2)\r\nend\r\n\r\nfunction Vector:sqrDistance(other)\r\n    local dx = self.x - other.x\r\n    local dy = self.y - other.y\r\n    local dz = self.z - other.z\r\n    return dx*dx + dy*dy + dz*dz\r\nend\r\n\r\nfunction Vector:distance(other)\r\n    return math.sqrt(self:sqrDistance(other))\r\nend\r\n\r\nfunction Vector:equals(other, margin)\r\n    margin = margin or 1e-3\r\n    return self:sqrDistance(other) <= margin\r\nend\r\n\r\nVector.__eq = Vector.equals\r\n\r\nfunction Vector:string(prefix)\r\n    prefix = prefix and (prefix .. ': ') or ''\r\n    return (('%s{ %f, %f, %f }'):format(\r\n        prefix,\r\n        self.x,\r\n        self.y,\r\n        self.z\r\n        -- cut trailing zeroes from numbers\r\n    ):gsub('%.0+([ ,])', '%1'):gsub('%.(%d-)0+([ ,])', '.%1%2'))\r\nend\r\n\r\nfunction Vector:__tostring()\r\n    return self:string('Vector')\r\nend\r\n\r\nfunction Vector:angle(other)\r\n    local cosAng = self:dot(other) / (self:magnitude() * other:magnitude())\r\n    return math.deg(math.acos(cosAng))\r\nend\r\n\r\nfunction Vector:clamp(maxLen)\r\n    local len = self:magnitude()\r\n    if len <= maxLen then\r\n        return self\r\n    end\r\n    local factor = maxLen/len\r\n    return self:scale(factor)\r\nend\r\n\r\nfunction Vector:cross(other)\r\n    return Vector(\r\n        self.y * other.z - self.z * other.y,\r\n        self.z * other.x - self.x * other.z,\r\n        self.x * other.y - self.y * other.x\r\n    )\r\nend\r\n\r\nfunction Vector.between(from, to)\r\n    return to - from\r\nend\r\n\r\nfunction Vector:lerp(target, t)\r\n    return self:between(target):scale(t):add(self)\r\nend\r\n\r\nfunction Vector:moveTowards(target, maxDist)\r\n    local delta = self:between(target):clamp(maxDist)\r\n    return self:add(delta)\r\nend\r\n\r\nfunction Vector:normalize()\r\n    local sqrLen = self:sqrMagnitude()\r\n    if sqrLen == 1 or sqrLen == 0 then\r\n        return self\r\n    end\r\n        \r\n    return self:scale(1/math.sqrt(sqrLen))\r\nend\r\n\r\nfunction Vector:normalized()\r\n    return self:copy():normalize()\r\nend\r\n\r\nfunction Vector:project(other)\r\n    if other:sqrMagnitude() ~= 1 then\r\n        other = other:normalized()\r\n    end\r\n    local scalar = self:dot(other)\r\n    return self:set(other:get()):scale(scalar)\r\nend\r\n\r\ndo\r\n    -- skew symmetric cross product\r\n    local function sscp(v)\r\n        return { Vector(   0,  -v.z,  v.y ),\r\n                 Vector(  v.z,   0,  -v.x ),\r\n                 Vector( -v.y,  v.x,   0  ) }\r\n    end\r\n    \r\n    --[[\r\n    local function mDump(m)\r\n        return ('{ %s,\\n  %s,\\n  %s }'):format(\r\n            m[1], m[2], m[3]\r\n        )\r\n    end\r\n    --]]\r\n    \r\n    local function qMult(x, y, z, w, factor)\r\n        return x*factor, y*factor, z*factor, w*factor\r\n    end\r\n    \r\n    local function qNormalize(x, y, z, w)\r\n        local norm = math.sqrt(x*x + y*y + z*z + w*w)\r\n        return qMult(x, y, z, w, 1/norm)\r\n    end\r\n    \r\n    -- interpolate quaternion\r\n    local function qSlerp(x, y, z, w, t)\r\n        local dot = w\r\n        if dot > 0.0005 then\r\n            return qNormalize(t*x, t*y, t*z, 1 + t*w - t)\r\n        end\r\n        \r\n        local th0 = math.acos(dot)\r\n        local th = th0*t\r\n        local sth = math.sin(th)\r\n        local sth0 = math.sin(th0)\r\n        \r\n        local s1 = sth/sth0\r\n        local s0 = math.cos(th) - dot*s1\r\n\r\n        return qMult(x, y, z, w + s0, s1)\r\n    end\r\n        \r\n    local function matrixToQuat(m, scale)\r\n        local w = math.sqrt(1.0 + m[1].x + m[2].y + m[3].z)/2;\r\n        local x = (m[3].y - m[2].z) / (w * 4)\r\n        local y = (m[1].z - m[3].x) / (w * 4)\r\n        local z = (m[2].x - m[1].y) / (w * 4)\r\n        if scale then\r\n            x, y, z, w = qSlerp(x, y, z, w, scale)\r\n        end\r\n        return x, y, z, w\r\n    end\r\n    \r\n    local function quatToMatrix(x, y, z, w)\r\n        return {\r\n            Vector(1-2*y*y-2*z*z, 2*x*y-2*z*w, 2*x*z+2*y*w),\r\n            Vector(2*x*y+2*z*w, 1-2*x*x-2*z*z, 2*y*z-2*x*w),\r\n            Vector(2*x*z-2*y*w, 2*y*z+2*x*w, 1-2*x*x-2*y*y)\r\n        }\r\n    end\r\n    \r\n    -- scale rotation matrix\r\n    local function mInterp(m, factor)\r\n        return quatToMatrix(matrixToQuat(m, factor))\r\n    end\r\n    \r\n    local function mScale(m, factor)\r\n        for _, row in ipairs(m) do\r\n            row:scale(factor)\r\n        end\r\n        return m\r\n    end\r\n    \r\n    -- apply a rotation matrix to vector\r\n    local function mApply(m, v)\r\n        v:set(m[1]:dot(v), m[2]:dot(v), m[3]:dot(v))\r\n        return v\r\n    end\r\n    \r\n    -- matrix multiplication\r\n    local function mMult(m1, m2)\r\n        local trans = {\r\n            Vector(m2[1].x, m2[2].x, m2[3].x),\r\n            Vector(m2[1].y, m2[2].y, m2[3].y),\r\n            Vector(m2[1].z, m2[2].z, m2[3].z),\r\n        }\r\n        local result = { Vector(), Vector(), Vector() }\r\n        for row = 1, 3 do\r\n            result[row]:set( m1[row]:dot(trans[1]), m1[row]:dot(trans[2]), m1[row]:dot(trans[3]) )\r\n        end\r\n        return result\r\n    end\r\n    \r\n    -- matrix addition\r\n    local function mAdd(m1, m2)\r\n        for row = 1, 3 do\r\n            m1[row]:add(m2[row])\r\n        end\r\n        return m1\r\n    end\r\n    \r\n    -- eye matrix\r\n    local function mUnit()\r\n        return { Vector(1, 0, 0),\r\n                 Vector(0, 1, 0),\r\n                 Vector(0, 0, 1) }\r\n    end\r\n    \r\n    local function rotationMatrix(from, to)\r\n        local sscross = sscp(from:cross(to))\r\n        local dot = from:dot(to)\r\n        -- maybe skew one of them instead to not bother the user\r\n        -- or include a bool/etc result\r\n        assert(math.abs(dot + 1) > 0.05, 'Vectors too close to opposite of each other')\r\n        local factor = 1 / (1 + dot)\r\n        local ss2scaled = mScale(mMult(sscross, sscross), factor)\r\n        return mAdd(mAdd(mUnit(), sscross), ss2scaled)\r\n    end\r\n    \r\n    local function mTrail(m)\r\n        return m[1].x + m[2].y + m[3].z\r\n    end\r\n    \r\n    -- Amount of rotation in degrees\r\n    local function mDeg(m)\r\n        return math.deg(math.acos((mTrail(m)-1)/2))\r\n    end    \r\n    \r\n    function Vector:rotateTowardsUnit(unitTarget, maxDelta)\r\n        local m = rotationMatrix(self, unitTarget)\r\n        if maxDelta then\r\n            local angle = mDeg(m)\r\n            if angle > maxDelta then\r\n                m = mInterp(m, maxDelta/angle)\r\n            end\r\n        end\r\n        return mApply(m, self)\r\n    end\r\n    \r\n    function Vector:rotateTowards(target, maxDelta)\r\n        local len = self:magnitude()\r\n        return self:normalize()\r\n            :rotateTowardsUnit(target:normalized(), maxDelta)\r\n            :scale(len)\r\n    end\r\n    \r\n    function Vector:reflect(planeNormal)\r\n        local x, y, z = planeNormal:normalized():get()\r\n        local reflectMatrix = {\r\n            Vector(1-2*x*x, -2*x*y, -2*x*z),\r\n            Vector(-2*x*y, 1-2*y*y, -2*y*z),\r\n            Vector(-2*x*z, -2*y*z, 1-2*z*z)\r\n        }\r\n        return mApply(reflectMatrix, self)\r\n    end\r\n    \r\n    local function basicRotationMatrix(axis, angle)\r\n        angle = math.rad(angle)\r\n        local sin, cos = math.sin(angle), math.cos(angle)\r\n        \r\n        if axis == 'x' then\r\n            return {\r\n                Vector( 1,   0,    0  ),\r\n                Vector( 0,  cos, -sin ),\r\n                Vector( 0,  sin,  cos )\r\n            }\r\n        elseif axis == 'y' then\r\n            return {\r\n                Vector( cos,  0,  sin ),\r\n                Vector(  0,   1,   0  ),\r\n                Vector(-sin,  0,  cos )\r\n            }\r\n        elseif axis == 'z' then\r\n            return {\r\n                Vector( cos, -sin,  0 ),\r\n                Vector( sin,  cos,  0 ),\r\n                Vector(  0,    0,   1 )\r\n            }\r\n        end\r\n    end    \r\n    \r\n    function Vector:rotateOver(axis, angle)\r\n        return mApply(basicRotationMatrix(axis, angle), self)\r\n    end\r\nend\r\n\r\nfunction Vector.max(v1, v2)\r\n    return Vector(\r\n        math.max(v1.x, v2.x),\r\n        math.max(v1.y, v2.y),\r\n        math.max(v1.z, v2.z)\r\n    )\r\nend\r\n\r\nfunction Vector.min(v1, v2)\r\n    return Vector(\r\n        math.min(v1.x, v2.x),\r\n        math.min(v1.y, v2.y),\r\n        math.min(v1.z, v2.z)\r\n    )\r\nend\r\n\r\nfunction Vector:inverse()\r\n    return self:set(-1*self.x, -1*self.y, -1*self.z)\r\nend\r\n\r\nfunction Vector:projectOnPlane(planeNormal)\r\n    local _, _, planeShadow = planeNormal:orthoNormalize(self)\r\n    if self:angle(planeShadow) > 90 then\r\n        planeShadow:inverse()\r\n    end\r\n    return self:project(planeShadow)\r\nend\r\n\r\nfunction Vector:orthoNormalize(binormalPlanar)\r\n    \r\n    -- if no vector was supplied, create an arbitrary one\r\n    if not binormalPlanar then\r\n        binormalPlanar = Vector(1, 0, 0)\r\n        if self:angle(binormalPlanar) < 10 then\r\n            binormalPlanar = Vector(0, 0, 1)\r\n        end\r\n    elseif binormalPlanar:sqrMagnitude() ~= 1 then\r\n        binormalPlanar = binormalPlanar:normalized()\r\n    end\r\n    \r\n    local base = self:normalized()\r\n    local normal = base:cross(binormalPlanar:normalized())\r\n    local binormal = base:cross(normal)\r\n   \r\n    return base, normal, binormal\r\nend\r\n\r\nfunction Vector:heading(axis)\r\n    if not axis then\r\n        return self:heading('x'), self:heading('y'), self:heading('z')\r\n    end\r\n    \r\n    local c1, c2\r\n    \r\n    if axis == 'x' then\r\n        c1, c2 = self.y, self.z\r\n    elseif axis == 'y' then\r\n        c1, c2 = self.x, self.z\r\n    elseif axis == 'z' then\r\n        c1, c2 = self.x, self.y\r\n    end\r\n    \r\n    return math.deg(math.atan2(c1, c2))\r\nend\r\n\r\nreturn Vector\r\n        ", null, null);
		lua.Globals["Color"] = lua.DoString("\r\nlocal Color = {}\r\nColor.__isColor = true\r\nColor.__version = '1.0.1'\r\n\r\nlocal colorMt = {}\r\n\r\nlocal function clamp(val)\r\n    return math.min(1, math.max(0, val or 0))\r\nend\r\nlocal function clampMultiple(a, b, c, d)\r\n    return clamp(a), clamp(b), clamp(c), clamp(d)\r\nend\r\n\r\nfunction Color.new(...)\r\n    local col = setmetatable({\r\n        r = 0,\r\n        g = 0,\r\n        b = 0,\r\n        a = 1\r\n    }, Color)\r\n\r\n    local argNum = select('#', ...)\r\n    if argNum == 1 and type(...) == 'table' then\r\n        -- Color.new(colorTable)\r\n        local arg = ...\r\n        col.r = clamp(arg.r or col.r)\r\n        col.g = clamp(arg.g or col.g)\r\n        col.b = clamp(arg.b or col.b)\r\n        col.a = clamp(arg.a or col.a)\r\n    elseif argNum == 3 then\r\n        -- Color.new(r, g, b)\r\n        col.r, col.g, col.b = clampMultiple(...)\r\n    elseif argNum == 4 then\r\n        col.r, col.g, col.b, col.a = clampMultiple(...)\r\n    end\r\n    \r\n    return col\r\nend\r\ncolorMt.__call = function(_, ...) return Color.new(...) end\r\n\r\nlocal function normalizeName(str)\r\n    return str:sub(1, 1):upper() .. str:sub(2, -1):lower()\r\nend\r\n\r\nlocal playerColors = {\r\n    ['white']  = Color.new(1, 1, 1),\r\n    ['brown']  = Color.new(0.443, 0.231, 0.09),\r\n    ['red']    = Color.new(0.856, 0.1, 0.094),\r\n    ['orange'] = Color.new(0.956, 0.392, 0.113),\r\n    ['yellow'] = Color.new(0.905, 0.898, 0.172),\r\n    ['green']  = Color.new(0.192, 0.701, 0.168),\r\n    ['teal']   = Color.new(0.129, 0.694, 0.607),\r\n    ['blue']   = Color.new(0.118, 0.53, 1),\r\n    ['purple'] = Color.new(0.627, 0.125, 0.941),\r\n    ['pink']   = Color.new(0.96, 0.439, 0.807),\r\n    ['grey']   = Color.new(0.5, 0.5, 0.5),\r\n    ['black']  = Color.new(0.25, 0.25, 0.25),\r\n}\r\ncolorMt.__index = function(_, colorName)\r\n    colorName = colorName:lower()\r\n    if playerColors[colorName] then\r\n        return playerColors[colorName]:copy()\r\n    end\r\n    return nil\r\nend\r\n\r\nColor.list = {}\r\nfor colorName in pairs(playerColors) do\r\n    table.insert(Color.list, normalizeName(colorName))\r\nend\r\n\r\nfunction Color.Add(name, color)\r\n    name = name:lower()\r\n    assert(not playerColors[name], 'Color ' .. name .. ' already defined')\r\n    assert(color.__isColor, tostring(color) .. ' is not a Color instance')\r\n    playerColors[name] = color\r\n    table.insert(Color.list, normalizeName(name))\r\nend\r\n\r\nfunction Color.fromString(strColor)\r\n    local color = assert(playerColors[strColor:lower()], strColor .. ' is not a valid color string')\r\n    return color:copy()\r\nend\r\n\r\nfunction Color.fromHex(hexColor)\r\n    local rStr, gStr, bStr, aStr = hexColor:match('^#?(%x%x)(%x%x)(%x%x)(%x?%x?)$')\r\n\r\n    assert(rStr and gStr and bStr and (aStr:len() == 0 or aStr:len() == 2), tostring(hexColor) .. ' is not a valid color hex string')\r\n\r\n    if aStr == '' then\r\n        return Color(\r\n            tonumber(rStr, 16)/255,\r\n            tonumber(gStr, 16)/255,\r\n            tonumber(bStr, 16)/255,\r\n            1\r\n        )\r\n    else\r\n        return Color(\r\n            tonumber(rStr, 16)/255,\r\n            tonumber(gStr, 16)/255,\r\n            tonumber(bStr, 16)/255,\r\n            tonumber(aStr, 16)/255\r\n        )\r\n    end\r\nend\r\n\r\n\r\nfunction Color:get()\r\n    return self.r, self.g, self.b, self.a\r\nend\r\n\r\nfunction Color:toHex(includeAlpha)\r\n    if includeAlpha then\r\n        return ('%02x%02x%02x%02x'):format(\r\n            self.r*255,\r\n            self.g*255,\r\n            self.b*255,\r\n            self.a*255\r\n        )\r\n    else\r\n        return ('%02x%02x%02x'):format(\r\n            self.r*255,\r\n            self.g*255,\r\n            self.b*255\r\n        )\r\n    end\r\nend\r\n\r\nfunction Color:toString(tolerance)\r\n    for name, color in pairs(playerColors) do\r\n        if self:equals(color, tolerance) then\r\n            return normalizeName(name)\r\n        end\r\n    end\r\n    return nil\r\nend\r\n\r\ndo\r\n    local remap = {'r', 'g', 'b', 'a'}\r\n    function Color:__index(k)\r\n        k = remap[k] or k\r\n        return rawget(self, k) or Color[k]\r\n    end\r\n    function Color:__newindex(k, v)\r\n        k = remap[k] or k\r\n        return rawset(self, k, v)\r\n    end\r\nend\r\n\r\nfunction Color:set(r, g, b, a)\r\n    self.r = clamp(r or self.r)\r\n    self.g = clamp(g or self.g)\r\n    self.b = clamp(b or self.b)\r\n    self.a = clamp(a or self.a)\r\nend\r\n\r\nfunction Color:setAt(key, value)\r\n    self[key] = clamp(value)\r\n    return self\r\nend\r\n\r\nfunction Color:equals(other, margin)\r\n    margin = margin or 1e-2\r\n    local diff = math.abs(self.r - other.r)\r\n        + math.abs(self.g - other.g)\r\n        + math.abs(self.b - other.b)\r\n        + math.abs(self.a - other.a)\r\n    return diff <= margin\r\nend\r\nColor.__eq = Color.equals\r\n\r\nfunction Color:copy()\r\n    return Color(self:get())\r\nend\r\n\r\nfunction Color:dump(prefix)\r\n    local name = self:toString()\r\n    local str = (self.a < 1) and '%s%s{ r = %f, g = %f, b = %f, a = %f }' or '%s%s{ r = %f, g = %f, b = %f }'\r\n    return (str:format(\r\n        prefix and (prefix .. ': ') or '',\r\n        name and (name .. ' ') or '',\r\n        self:get()\r\n        -- cut trailing zeroes from numbers\r\n    ):gsub('%.0+([ ,])', '%1'):gsub('%.(%d-)0+([ ,])', '.%1%2'))\r\nend\r\n\r\nfunction Color:__tostring()\r\n    return self:dump('Color')\r\nend\r\n\r\nfunction Color:lerp(other, t)\r\n    local mid = self:copy()\r\n    mid.r = mid.r + (other.r - self.r)*t\r\n    mid.g = mid.g + (other.g - self.g)*t\r\n    mid.b = mid.b + (other.b - self.b)*t\r\n    mid.a = mid.a + (other.a - self.a)*t\r\n    return mid\r\nend\r\n\r\nsetmetatable(Color, colorMt)\r\n\r\nreturn Color\r\n", null, null);
		lua.Globals["startLuaCoroutine"] = new Func<LuaGameObjectScript, string, Script, bool>(this.StartLuaCoroutine);
		lua.Globals["log"] = new Func<DynValue, string, string, bool>(this.Log);
		lua.Globals["logString"] = new global::Func<DynValue, string, string, bool, bool, string>(this.LogFormat);
		lua.Globals["logStyle"] = new Func<string, Color, string, string, bool>(this.LogStyle);
		lua.Globals["printToAll"] = new Func<string, Color?, bool>(this.PrintToAll);
		lua.Globals["printToColor"] = new Func<string, string, Color?, bool>(this.PrintToColor);
		lua.Globals["broadcastToAll"] = new Func<string, Color?, bool>(this.BroadcastToAll);
		lua.Globals["broadcastToColor"] = new Func<string, string, Color?, bool>(this.BroadcastToColor);
		lua.Globals["stringColorToRGB"] = new Func<string, Color?>(LuaGlobalScriptManager.StringColorToRGB);
		lua.Globals["flipTable"] = new Func<bool>(LuaGlobalScriptManager.FlipTable);
		lua.Globals["chooseInHand"] = new global::Func<string, List<string>, int, int, string, List<string>>(this.ChooseInHand);
		lua.Globals["chooseInHandOrCancel"] = new global::Func<string, List<string>, int, int, string, List<string>>(this.ChooseInHandOrCancel);
		lua.Globals["clearChooseInHand"] = new Func<List<string>, List<string>>(this.clearChooseInHand);
		lua.Globals["currentChooseInHand"] = new Func<string, string>(this.currentChooseInHand);
		lua.Globals["copy"] = new Func<List<LuaGameObjectScript>, bool>(LuaGlobalScriptManager.Copy);
		lua.Globals["paste"] = new Func<LuaGlobalScriptManager.LuaObjectParameters, List<LuaGameObjectScript>>(LuaGlobalScriptManager.Paste);
		lua.Globals["setLookingForPlayers"] = new Func<bool, bool>(LuaGlobalScriptManager.SetLookingForPlayers);
		lua.Globals["sendExternalMessage"] = new Func<Dictionary<object, object>, bool>(this.SendExternalCustomMessage);
		lua.Globals["inspect"] = new Func<object, LuaGlobalScriptManager.ObjectInfo>(LuaGlobalScriptManager.Inspect);
		lua.Globals["color"] = new global::Func<Script, float?, float?, float?, float?, Color>(this.ConstructColor);
		lua.Globals["vector"] = new Func<Script, float?, float?, float?, Vector3>(this.ConstructVector);
		lua.Globals["getObjects"] = new Func<List<LuaGameObjectScript>>(LuaGlobalScriptManager.GetObjects);
		lua.Globals["getObjectsWithTag"] = new Func<string, List<LuaGameObjectScript>>(LuaGlobalScriptManager.GetObjectsWithTag);
		lua.Globals["getObjectsWithAnyTags"] = new Func<List<string>, List<LuaGameObjectScript>>(LuaGlobalScriptManager.GetObjectsWithAnyTags);
		lua.Globals["getObjectsWithAllTags"] = new Func<List<string>, List<LuaGameObjectScript>>(LuaGlobalScriptManager.GetObjectsWithAllTags);
		lua.Globals["getAllTags"] = new Func<List<string>>(LuaGlobalScriptManager.GetAllTags);
		lua.Globals["getObjectFromGUID"] = new Func<string, LuaGameObjectScript>(LuaGlobalScriptManager.GetObjectFromGUID);
		lua.Globals["spawnObject"] = new Func<LuaGlobalScriptManager.LuaObjectParameters, LuaGameObjectScript>(this.SpawnObject);
		lua.Globals["spawnObjectJSON"] = new Func<LuaGlobalScriptManager.LuaJsonObjectParameters, LuaGameObjectScript>(this.SpawnObjectJSON);
		lua.Globals["spawnObjectData"] = new Func<LuaGlobalScriptManager.LuaDataObjectParameters, LuaGameObjectScript>(this.SpawnObjectData);
		lua.Globals["destroyObject"] = new Func<LuaGameObjectScript, bool>(LuaGameObjectScript.DestroyObject);
		lua.Globals["group"] = new Func<List<LuaGameObjectScript>, List<LuaGameObjectScript>>(this.Group);
		lua.Globals["addContextMenuItem"] = new Func<string, Closure, bool, bool, bool>(this.AddContextMenuItem);
		lua.Globals["clearContextMenu"] = new Func<bool>(this.ClearContextMenu);
		lua.Globals["addHotkey"] = new Func<string, Closure, bool, bool>(this.AddHotkey);
		lua.Globals["clearHotkeys"] = new Func<bool>(this.ClearHotkeys);
		lua.Globals["showHotkeyConfig"] = new Func<string, bool>(this.ShowHotkeyConfig);
		lua.Globals["addConsoleCommand"] = new Func<string, Closure, bool, bool>(this.AddConsoleCommand);
		lua.Globals["clearConsoleCommands"] = new Func<bool>(this.ClearConsoleCommands);
		lua.Globals["getAllObjects"] = new Func<List<LuaGameObjectScript>>(LuaGlobalScriptManager.GetAllObjects);
		lua.Globals["clearPixelPaint"] = new Func<bool>(LuaGlobalScriptManager.ClearPixelPaint);
		lua.Globals["clearVectorPaint"] = new Func<bool>(LuaGlobalScriptManager.ClearVectorPaint);
		lua.Globals["Timer"] = this.GlobalTimer;
		lua.Globals["getNotes"] = new Func<string>(LuaGlobalScriptManager.Instance.GlobalNotes.GetNotes);
		lua.Globals["setNotes"] = new Func<string, bool>(LuaGlobalScriptManager.Instance.GlobalNotes.SetNotes);
		lua.Globals["getNotebookTabs"] = new Func<List<LuaNotes.LuaNotebookParameters>>(LuaGlobalScriptManager.Instance.GlobalNotes.GetNotebookTabs);
		lua.Globals["editNotebookTab"] = new Func<LuaNotes.LuaNotebookParameters, bool>(LuaGlobalScriptManager.Instance.GlobalNotes.EditNotebookTab);
		lua.Globals["addNotebookTab"] = new Func<LuaNotes.LuaNotebookParameters, int>(LuaGlobalScriptManager.Instance.GlobalNotes.AddNotebookTab);
		lua.Globals["removeNotebookTab"] = new Func<int, bool>(LuaGlobalScriptManager.Instance.GlobalNotes.RemoveNotebookTab);
		lua.Globals["getSeatedPlayers"] = new Func<List<string>>(LuaGlobalScriptManager.GetSeatedPlayers);
		lua.Globals["callLuaFunctionInOtherScript"] = new Func<Script, LuaGameObjectScript, string, DynValue>(this.CallLuaFunctionInOtherScript);
		lua.Globals["callLuaFunctionInOtherScriptWithParams"] = new Func<Script, LuaGameObjectScript, string, Table, DynValue>(this.CallLuaFunctionInOtherScript);
		lua.Globals["getPointerPosition"] = new Func<Script, string, Table>(LuaGlobalScriptManager.GetPointerPosition);
		lua.Globals["getPointerRotation"] = new Func<Script, string, float?>(LuaGlobalScriptManager.GetPointerRotation);
		lua.Globals["getGlobalScriptVar"] = new Func<string, object>(LuaGlobalScriptManager.GetGlobalVar);
		lua.Globals["setGlobalScriptVar"] = new Func<string, object, bool>(LuaGlobalScriptManager.SetGlobalScriptVar);
		lua.Globals["getGlobalScriptTable"] = new Func<Script, string, Table>(LuaGlobalScriptManager.GetGlobalTable);
		lua.Globals["setGlobalScriptTable"] = new Func<string, Table, bool>(LuaGlobalScriptManager.SetGlobalTable);
		lua.Globals["getPlayer"] = new Func<string, LuaPlayer>(LuaGlobalScriptManager.GetPlayer);
		lua.Globals["colorToPlayerName"] = new Func<string, string>(this.ColorToPlayerName);
		lua.Globals["getPlayerHandPositionAndRotation"] = new Func<Script, string, Table>(LuaGlobalScriptManager.GetPlayerHandPositionAndRotation);
	}

	// Token: 0x06001458 RID: 5208 RVA: 0x00085F6C File Offset: 0x0008416C
	public static bool HasCorrectNumOfParams<T>(List<T> L, int NumParams, string FunctionName)
	{
		if (L.Count != NumParams)
		{
			Chat.LogError(string.Concat(new object[]
			{
				"Error: Incorrect number of elements in table parameter for function ",
				FunctionName,
				". Expected ",
				NumParams,
				" but found ",
				L.Count,
				"."
			}), true);
			LuaGlobalScriptManager.Instance.PushLuaErrorMessage("", "-1", string.Concat(new object[]
			{
				"Error: Incorrect number of elements in table parameter for function ",
				FunctionName,
				". Expected ",
				NumParams,
				" but found ",
				L.Count,
				"."
			}));
		}
		return true;
	}

	// Token: 0x06001459 RID: 5209 RVA: 0x0008602C File Offset: 0x0008422C
	public static LuaScriptState[] GetAllLuaScriptStates()
	{
		List<LuaScriptState> list = new List<LuaScriptState>();
		LuaScriptState item = new LuaScriptState
		{
			name = "Global",
			guid = "-1",
			script = LuaGlobalScriptManager.Instance.script_code.TrimEnd(LuaGlobalScriptManager.scriptTrimChars),
			ui = ((!string.IsNullOrEmpty(LuaGlobalScriptManager.Instance.XmlUI.xmlui_code)) ? LuaGlobalScriptManager.Instance.XmlUI.xmlui_code.TrimEnd(LuaGlobalScriptManager.scriptTrimChars) : null)
		};
		list.Add(item);
		List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
		for (int i = 0; i < grabbableNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = grabbableNPOs[i];
			LuaGameObjectScript luaGameObjectScript = networkPhysicsObject.luaGameObjectScript;
			if (!string.IsNullOrEmpty(luaGameObjectScript.script_code) || !string.IsNullOrEmpty(luaGameObjectScript.XmlUI.xmlui_code))
			{
				list.Add(LuaGlobalScriptManager.GetScriptState(networkPhysicsObject.gameObject));
			}
		}
		return list.ToArray();
	}

	// Token: 0x0600145A RID: 5210 RVA: 0x0008611C File Offset: 0x0008431C
	public static LuaScriptState GetScriptState(GameObject GO)
	{
		NetworkPhysicsObject component = GO.GetComponent<NetworkPhysicsObject>();
		LuaGameObjectScript luaGameObjectScript = component.luaGameObjectScript;
		return new LuaScriptState
		{
			name = TTSUtilities.CleanName(component),
			guid = component.GUID,
			script = luaGameObjectScript.script_code.TrimEnd(LuaGlobalScriptManager.scriptTrimChars),
			ui = ((!string.IsNullOrEmpty(luaGameObjectScript.XmlUI.xmlui_code)) ? luaGameObjectScript.XmlUI.xmlui_code.TrimEnd(LuaGlobalScriptManager.scriptTrimChars) : null)
		};
	}

	// Token: 0x0600145B RID: 5211 RVA: 0x0008619C File Offset: 0x0008439C
	public void SetAllLuaObjectsFromStates(LuaScriptState[] ScriptStates)
	{
		foreach (LuaScriptState luaScriptState in ScriptStates)
		{
			if (luaScriptState.guid == "-1")
			{
				LuaGlobalScriptManager.Instance.script_code = luaScriptState.script;
				LuaGlobalScriptManager.Instance.XmlUI.xmlui_code = ((!string.IsNullOrEmpty(luaScriptState.ui)) ? luaScriptState.ui : null);
			}
			else
			{
				NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGUID(luaScriptState.guid);
				if (networkPhysicsObject != null)
				{
					networkPhysicsObject.luaGameObjectScript.script_code = luaScriptState.script;
					networkPhysicsObject.xmlUI.xmlui_code = ((!string.IsNullOrEmpty(luaScriptState.ui)) ? luaScriptState.ui : null);
				}
				else
				{
					Debug.LogError("COULDN'T FIND GUID TO SAVE SCRIPT STATE :(");
				}
			}
		}
	}

	// Token: 0x0600145C RID: 5212 RVA: 0x00086262 File Offset: 0x00084462
	public void PushNewLuaObject(GameObject GO)
	{
		this.pushNewLuaObject = GO;
		base.StartCoroutine(this.PushLuaMessage(new LuaExternalNewObject(new LuaScriptState[]
		{
			LuaGlobalScriptManager.GetScriptState(GO)
		}), new Action<LuaEditorPushJob>(this.PushNewLuaObjectCallback)));
	}

	// Token: 0x0600145D RID: 5213 RVA: 0x00086298 File Offset: 0x00084498
	private void PushNewLuaObjectCallback(LuaEditorPushJob luaPushJob)
	{
		if (!string.IsNullOrEmpty(luaPushJob.Ex))
		{
			NetworkSingleton<NetworkUI>.Instance.GUILuaNotepad.Init(this.pushNewLuaObject);
		}
	}

	// Token: 0x0600145E RID: 5214 RVA: 0x000862BC File Offset: 0x000844BC
	public void PushLuaPrintMessage(string Message)
	{
		base.StartCoroutine(this.PushLuaMessage(new LuaExternalPrintMessage(Message), null));
	}

	// Token: 0x0600145F RID: 5215 RVA: 0x000862D2 File Offset: 0x000844D2
	public void PushLuaErrorMessage(string Error, string GUID, string MessagePrefix)
	{
		base.StartCoroutine(this.PushLuaMessage(new LuaExternalErrorMessage(Error, GUID, MessagePrefix), null));
	}

	// Token: 0x06001460 RID: 5216 RVA: 0x000862EC File Offset: 0x000844EC
	public static Table CopyLuaTable(Table SourceTable, Script CopyTableOwnerScript)
	{
		if (SourceTable == null)
		{
			return null;
		}
		Table table = new Table(CopyTableOwnerScript);
		if (SourceTable.MetaTable != null && SourceTable.MetaTable.RawGet("__isVector") != null)
		{
			table.MetaTable = (Table)CopyTableOwnerScript.Globals["Vector"];
		}
		if (SourceTable.MetaTable != null && SourceTable.MetaTable.RawGet("__isColor") != null)
		{
			table.MetaTable = (Table)CopyTableOwnerScript.Globals["Color"];
		}
		foreach (DynValue key in SourceTable.Keys)
		{
			DynValue dynValue = DynValue.FromObject(CopyTableOwnerScript, SourceTable[key]);
			if (dynValue.Type == DataType.Table)
			{
				table[key] = LuaGlobalScriptManager.CopyLuaTable(dynValue.Table, CopyTableOwnerScript);
			}
			else
			{
				table[key] = SourceTable[key];
			}
		}
		return table;
	}

	// Token: 0x06001461 RID: 5217 RVA: 0x000863E4 File Offset: 0x000845E4
	public override void OnLoad()
	{
		base.OnLoad();
		this.SendLuaExternalNewGame();
	}

	// Token: 0x06001462 RID: 5218 RVA: 0x000863F4 File Offset: 0x000845F4
	[MoonSharpHidden]
	public static Script FunctionOwnerToScript(ref LuaGameObjectScript FunctionOwner)
	{
		Script lua;
		if (FunctionOwner == null)
		{
			lua = LuaGlobalScriptManager.Instance.lua;
			FunctionOwner = LuaGlobalScriptManager.Instance.GlobalDummyObject;
		}
		else
		{
			lua = FunctionOwner.lua;
		}
		return lua;
	}

	// Token: 0x06001463 RID: 5219 RVA: 0x00086430 File Offset: 0x00084630
	public static LuaGlobalScriptManager.ObjectInfo Inspect(object obj)
	{
		if (obj == null)
		{
			return null;
		}
		Type type = obj.GetType();
		LuaGlobalScriptManager.ObjectInfo objectInfo = new LuaGlobalScriptManager.ObjectInfo
		{
			name = TypeDescriptor.GetClassName(obj).ToString()
		};
		foreach (object obj2 in TypeDescriptor.GetProperties(obj))
		{
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj2;
			string name = propertyDescriptor.Name;
			Type propertyType = propertyDescriptor.PropertyType;
			if (UserData.IsTypeRegistered(propertyType) || propertyType.Namespace == "System")
			{
				objectInfo.variables.Add(new LuaGlobalScriptManager.VariableInfo
				{
					name = name,
					type = propertyDescriptor.PropertyType.ToString().Replace("System.", "").Replace("Lua", "")
				});
			}
		}
		foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Public))
		{
			Chat.Log(methodInfo.Name + " : " + methodInfo.ReturnType, ChatMessageType.Game);
		}
		return objectInfo;
	}

	// Token: 0x06001464 RID: 5220 RVA: 0x00086564 File Offset: 0x00084764
	public bool StartLuaCoroutine(LuaGameObjectScript FunctionOwner, string LuaFunction, Script s)
	{
		if (this.loaded)
		{
			LuaScript luaScript = LuaScript.ScriptToLuaScript(s);
			Script script = LuaGlobalScriptManager.FunctionOwnerToScript(ref FunctionOwner);
			if (script != null && script.Globals[LuaFunction] != null)
			{
				try
				{
					DynValue function = script.Globals.Get(LuaFunction);
					DynValue coroutine = script.CreateCoroutine(function);
					this.LuaCoroutines.Add(base.StartCoroutine(this.LuaCoroutine(coroutine, LuaFunction, luaScript)));
					return true;
				}
				catch (Exception e)
				{
					luaScript.LogError("startLuaCoroutine/" + LuaFunction, e, null);
					return true;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	// Token: 0x06001465 RID: 5221 RVA: 0x000865F8 File Offset: 0x000847F8
	private IEnumerator LuaCoroutine(DynValue coroutine, string LuaFunction, LuaScript luaScript)
	{
		double num = 0.0;
		try
		{
			num = coroutine.Coroutine.Resume().Number;
			goto IL_AE;
		}
		catch (Exception e)
		{
			luaScript.LogError("startLuaCoroutine/" + LuaFunction, e, null);
			yield break;
		}
		IL_5B:
		yield return null;
		try
		{
			num = coroutine.Coroutine.Resume().Number;
		}
		catch (Exception e2)
		{
			luaScript.LogError("startLuaCoroutine/" + LuaFunction, e2, null);
			yield break;
		}
		IL_AE:
		if (num != 0.0)
		{
			yield break;
		}
		goto IL_5B;
	}

	// Token: 0x06001466 RID: 5222 RVA: 0x00086618 File Offset: 0x00084818
	public string LogFormat(DynValue value, string label, string tags, bool concise = false, bool displayTag = false)
	{
		Colour colour = Singleton<SystemConsole>.Instance.DefaultLogTag.colour;
		string text = Singleton<SystemConsole>.Instance.DefaultLogTag.prefix;
		string text2 = Singleton<SystemConsole>.Instance.DefaultLogTag.postfix;
		bool flag = false;
		string text3 = "";
		string text4 = null;
		if (tags != null)
		{
			text4 = LibString.bite(ref tags, true, ' ', false, false, '\0');
		}
		if (tags == null || text4 == null)
		{
			bool flag2 = Singleton<SystemConsole>.Instance.LogTagsIncluded.Count == 0;
		}
		else
		{
			bool flag2 = false;
			while (text4 != null)
			{
				text3 = text3 + " " + text4;
				SystemConsole.LogTag value2;
				if (!Singleton<SystemConsole>.Instance.LogTags.TryGetValue(text4, out value2))
				{
					value2 = new SystemConsole.LogTag(text4, colour, text, text2);
					Singleton<SystemConsole>.Instance.LogTags[text4] = value2;
				}
				if (Singleton<SystemConsole>.Instance.LogTagShouldDisplay(text4) && !flag2)
				{
					Singleton<SystemConsole>.Instance.SetValuesFromLogTag(text4, ref colour, ref text, ref text2);
					flag2 = true;
				}
				if (Singleton<SystemConsole>.Instance.LogTagShouldHighlight(text4))
				{
					flag = true;
				}
				text4 = LibString.bite(ref tags, true, ' ', false, false, '\0');
			}
		}
		if (flag)
		{
			Singleton<SystemConsole>.Instance.SetValuesFromHighlight(ref colour, ref text, ref text2);
		}
		string text5 = "\n";
		if (concise)
		{
			text5 = " ";
		}
		if (text != "")
		{
			text += text5;
		}
		if (displayTag && text3 != "")
		{
			text = string.Concat(new string[]
			{
				"[6A6A6A]:",
				text3,
				" :[-]",
				text5,
				text
			});
		}
		if (text2 != "")
		{
			text2 = text5 + text2;
		}
		string str = this.MessageFragmentFromDynValue(value, "", "");
		if (label == null)
		{
			label = "";
		}
		if (label != "")
		{
			label += text5;
		}
		return text + label + str + text2;
	}

	// Token: 0x06001467 RID: 5223 RVA: 0x000867F8 File Offset: 0x000849F8
	public bool Log(DynValue value, string label, string tags)
	{
		Colour colour = Singleton<SystemConsole>.Instance.DefaultLogTag.colour;
		string text = Singleton<SystemConsole>.Instance.DefaultLogTag.prefix;
		string text2 = Singleton<SystemConsole>.Instance.DefaultLogTag.postfix;
		bool flag = false;
		string text3 = "";
		string text4 = null;
		if (tags != null)
		{
			text4 = LibString.bite(ref tags, true, ' ', false, false, '\0');
		}
		bool flag2;
		if (tags == null || text4 == null)
		{
			flag2 = (Singleton<SystemConsole>.Instance.LogTagsIncluded.Count == 0);
		}
		else
		{
			flag2 = false;
			while (text4 != null)
			{
				text3 = text3 + " " + text4;
				SystemConsole.LogTag value2;
				if (!Singleton<SystemConsole>.Instance.LogTags.TryGetValue(text4, out value2))
				{
					value2 = new SystemConsole.LogTag(text4, colour, text, text2);
					Singleton<SystemConsole>.Instance.LogTags[text4] = value2;
				}
				if (Singleton<SystemConsole>.Instance.LogTagShouldDisplay(text4) && !flag2)
				{
					Singleton<SystemConsole>.Instance.SetValuesFromLogTag(text4, ref colour, ref text, ref text2);
					flag2 = true;
				}
				if (Singleton<SystemConsole>.Instance.LogTagShouldHighlight(text4))
				{
					flag = true;
				}
				text4 = LibString.bite(ref tags, true, ' ', false, false, '\0');
			}
		}
		if (!flag2)
		{
			return false;
		}
		if (flag)
		{
			Singleton<SystemConsole>.Instance.SetValuesFromHighlight(ref colour, ref text, ref text2);
		}
		string text5 = "\n";
		if (Singleton<SystemConsole>.Instance.logFormat != SystemConsole.LogFormat.Expansive)
		{
			text5 = " ";
		}
		if (text != "")
		{
			text += text5;
		}
		if (Singleton<SystemConsole>.Instance.logDisplayTag && text3 != "")
		{
			text = string.Concat(new string[]
			{
				"[6A6A6A]:",
				text3,
				" :[-]",
				text5,
				text
			});
		}
		if (text2 != "")
		{
			text2 = text5 + text2;
		}
		string text6 = this.MessageFragmentFromDynValue(value, "", "");
		if (label == null)
		{
			label = "";
		}
		if (label != "")
		{
			label += text5;
		}
		Singleton<SystemConsole>.Instance.Log(text + label + text6 + text2, colour, Singleton<SystemConsole>.Instance.logFormat == SystemConsole.LogFormat.Truncated);
		this.PushLuaPrintMessage(text6);
		return true;
	}

	// Token: 0x06001468 RID: 5224 RVA: 0x00086A14 File Offset: 0x00084C14
	private string LogMessageFromTable(Table table, string prefix)
	{
		string text = "";
		string text2 = "\n";
		if (Singleton<SystemConsole>.Instance.logFormat != SystemConsole.LogFormat.Expansive)
		{
			text2 = " ";
		}
		foreach (DynValue dynValue in table.Keys)
		{
			text = string.Concat(new object[]
			{
				text,
				prefix,
				dynValue,
				": "
			});
			DynValue value = table.Get(dynValue);
			text += this.MessageFragmentFromDynValue(value, prefix, text2);
			text += text2;
		}
		if (text.Length > 0)
		{
			text = text.Substring(0, text.Length - 1);
		}
		return text;
	}

	// Token: 0x06001469 RID: 5225 RVA: 0x00086AD8 File Offset: 0x00084CD8
	private string MessageFragmentFromDynValue(DynValue value, string prefix, string sep)
	{
		switch (value.Type)
		{
		case DataType.Nil:
			return "nil";
		case DataType.Boolean:
			return value.ToObject<bool>().ToString().ToLower();
		case DataType.Number:
			return value.ToObject<float>().ToString();
		case DataType.String:
		{
			string text = value.ToObject<string>();
			return text.Replace("[", "[​​​​​​​");
		}
		case DataType.Function:
		case DataType.ClrFunction:
			return "<func>";
		case DataType.Table:
			if (prefix.Length < Singleton<SystemConsole>.Instance.logTablePrefix.Length * Singleton<SystemConsole>.Instance.logTableDepth)
			{
				return sep + this.LogMessageFromTable(value.ToObject<Table>(), prefix + Singleton<SystemConsole>.Instance.logTablePrefix);
			}
			return "<table>";
		case DataType.UserData:
		{
			object obj = value.ToObject();
			LuaGameObjectScript luaGameObjectScript;
			string str2;
			if ((luaGameObjectScript = (obj as LuaGameObjectScript)) != null)
			{
				string str;
				try
				{
					str = luaGameObjectScript.GetName();
				}
				catch
				{
					str = "#NONAME#";
				}
				str2 = "〔" + luaGameObjectScript.guid + "〕" + str;
			}
			else
			{
				LuaEnum luaEnum;
				if ((luaEnum = (obj as LuaEnum)) != null)
				{
					return this.MessageFragmentFromDynValue(luaEnum.Table, prefix, sep);
				}
				str2 = "";
			}
			string str3 = LibString.lookAhead(obj.ToString(), '(');
			return "<" + str3 + ">" + str2;
		}
		}
		return "<unhandled>";
	}

	// Token: 0x0600146A RID: 5226 RVA: 0x00086C80 File Offset: 0x00084E80
	public bool LogStyle(string tagName, Color color, string prefix, string postfix)
	{
		SystemConsole.LogTag defaultLogTag = Singleton<SystemConsole>.Instance.DefaultLogTag;
		Singleton<SystemConsole>.Instance.LogTags[tagName] = new SystemConsole.LogTag(tagName, color, prefix ?? defaultLogTag.prefix, postfix ?? defaultLogTag.postfix);
		return true;
	}

	// Token: 0x0600146B RID: 5227 RVA: 0x00086CCC File Offset: 0x00084ECC
	public static Color GetColor(Color? color)
	{
		Color? color2 = color;
		if (color2 == null)
		{
			return Color.white;
		}
		return color2.GetValueOrDefault();
	}

	// Token: 0x0600146C RID: 5228 RVA: 0x00086CF4 File Offset: 0x00084EF4
	public bool SetUITheme(string playerColor, string themeScript)
	{
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromColour(Colour.ColourFromLabel(playerColor));
		if (playerState != null)
		{
			base.networkView.RPC<string>(playerState.networkPlayer, new Action<string>(this.RPCSetUITheme), themeScript);
			return true;
		}
		return false;
	}

	// Token: 0x0600146D RID: 5229 RVA: 0x00086D36 File Offset: 0x00084F36
	[Remote(Permission.Server)]
	public void RPCSetUITheme(string themeScript)
	{
		Singleton<UIThemeEditor>.Instance.ImportGameThemeString(themeScript);
	}

	// Token: 0x0600146E RID: 5230 RVA: 0x00086D43 File Offset: 0x00084F43
	public bool PrintToAll(string message, Color? color)
	{
		base.networkView.RPC<string, Color, bool>(RPCTarget.All, new Action<string, Color, bool>(this.RPCPrint), message, LuaGlobalScriptManager.GetColor(color), false);
		return true;
	}

	// Token: 0x0600146F RID: 5231 RVA: 0x00086D68 File Offset: 0x00084F68
	public bool PrintToColor(string message, string playerColor, Color? color)
	{
		int num = NetworkSingleton<PlayerManager>.Instance.IDFromColour(playerColor);
		if (num < 0)
		{
			throw new ScriptRuntimeException("Error in printToColor: Player " + playerColor + " does not exist.");
		}
		if (NetworkID.ID == num)
		{
			this.RPCPrint(message, LuaGlobalScriptManager.GetColor(color), false);
			return true;
		}
		base.networkView.RPC<string, Color, bool>(Network.IdToNetworkPlayer(num), new Action<string, Color, bool>(this.RPCPrint), message, LuaGlobalScriptManager.GetColor(color), false);
		this.PushLuaPrintMessage(message);
		return true;
	}

	// Token: 0x06001470 RID: 5232 RVA: 0x00086DE0 File Offset: 0x00084FE0
	public bool BroadcastToAll(string message, Color? color)
	{
		base.networkView.RPC<string, Color, bool>(RPCTarget.All, new Action<string, Color, bool>(this.RPCPrint), message, LuaGlobalScriptManager.GetColor(color), true);
		return true;
	}

	// Token: 0x06001471 RID: 5233 RVA: 0x00086E04 File Offset: 0x00085004
	public bool BroadcastToColor(string Message, string PlayerColor, Color? color)
	{
		int num = NetworkSingleton<PlayerManager>.Instance.IDFromColour(PlayerColor);
		if (num < 0)
		{
			throw new ScriptRuntimeException("Error in BroadcastToColor: Player " + PlayerColor + " does not exist.");
		}
		if (NetworkID.ID == num)
		{
			this.RPCPrint(Message, LuaGlobalScriptManager.GetColor(color), true);
			return true;
		}
		base.networkView.RPC<string, Color, bool>(Network.IdToNetworkPlayer(num), new Action<string, Color, bool>(this.RPCPrint), Message, LuaGlobalScriptManager.GetColor(color), true);
		this.PushLuaPrintMessage(Message);
		return true;
	}

	// Token: 0x06001472 RID: 5234 RVA: 0x00086E7C File Offset: 0x0008507C
	public List<string> ChooseInHand(string label, List<string> playerColorLabels, int minCount, int maxCount, string prompt)
	{
		return this.chooseInHand(label, false, minCount, maxCount, playerColorLabels, prompt);
	}

	// Token: 0x06001473 RID: 5235 RVA: 0x00086E8C File Offset: 0x0008508C
	public List<string> ChooseInHandOrCancel(string label, List<string> playerColorLabels, int minCount, int maxCount, string prompt)
	{
		return this.chooseInHand(label, true, minCount, maxCount, playerColorLabels, prompt);
	}

	// Token: 0x06001474 RID: 5236 RVA: 0x00086E9C File Offset: 0x0008509C
	private List<string> chooseInHand(string label, bool showCancel, int minCount, int maxCount, List<string> playerColorLabels, string prompt)
	{
		if (prompt == null)
		{
			prompt = "";
		}
		if (minCount < 0)
		{
			minCount = 0;
		}
		maxCount = Mathf.Max(new int[]
		{
			maxCount,
			1,
			minCount
		});
		List<string> list;
		if (playerColorLabels == null || playerColorLabels.Count == 0)
		{
			list = LuaGlobalScriptManager.GetSeatedPlayers();
		}
		else
		{
			list = new List<string>();
			for (int i = 0; i < playerColorLabels.Count; i++)
			{
				string text = playerColorLabels[i];
				if (HandZone.GetHandZone(text, 0, true))
				{
					list.Add(text);
				}
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			NetworkSingleton<PlayerManager>.Instance.StartHandSelectMode(list[j], new HandSelectModeSettings(label, showCancel, minCount, maxCount, prompt));
		}
		return list;
	}

	// Token: 0x06001475 RID: 5237 RVA: 0x00086F50 File Offset: 0x00085150
	private List<string> clearChooseInHand(List<string> playerColorLabels)
	{
		List<string> list;
		if (playerColorLabels == null || playerColorLabels.Count == 0)
		{
			list = LuaGlobalScriptManager.GetSeatedPlayers();
		}
		else
		{
			list = new List<string>();
			for (int i = 0; i < playerColorLabels.Count; i++)
			{
				string text = playerColorLabels[i];
				if (HandZone.GetHandZone(text, 0, true))
				{
					list.Add(text);
				}
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			NetworkSingleton<PlayerManager>.Instance.ClearHandSelectMode(list[j]);
		}
		return list;
	}

	// Token: 0x06001476 RID: 5238 RVA: 0x00086FC8 File Offset: 0x000851C8
	private string currentChooseInHand(string playerColorLabel)
	{
		HandZone handZone = HandZone.GetHandZone(playerColorLabel, 0, true);
		if (!handZone || !handZone.HasQueuedHandSelectMode)
		{
			return "";
		}
		return handZone.HandSelectModeSettingsQueue[0].label;
	}

	// Token: 0x06001477 RID: 5239 RVA: 0x00087005 File Offset: 0x00085205
	[Remote(Permission.Server)]
	public void RPCPrint(string Message, Color color, bool Broadcast)
	{
		Chat.Log(Message, color, ChatMessageType.Game, false);
		if (Broadcast)
		{
			UIBroadcast.Log(Message, color, 2f, 0f);
		}
		this.PushLuaPrintMessage(Message);
	}

	// Token: 0x06001478 RID: 5240 RVA: 0x00087030 File Offset: 0x00085230
	public static Color? StringColorToRGB(string Color)
	{
		return Colour.NullableColorFromLabel(Color);
	}

	// Token: 0x06001479 RID: 5241 RVA: 0x00087038 File Offset: 0x00085238
	public static string StringColorToHex(string Color)
	{
		string text = Colour.HexFromLabel(Color);
		if (text == "[FFFFFF]" && Color != "White")
		{
			return null;
		}
		return text;
	}

	// Token: 0x0600147A RID: 5242 RVA: 0x00087069 File Offset: 0x00085269
	public static string ColorToHex(Color color)
	{
		return Colour.RGBHexFromColour(color);
	}

	// Token: 0x0600147B RID: 5243 RVA: 0x00087076 File Offset: 0x00085276
	public static Color HexToColor(string hex)
	{
		return Colour.ColourFromRGBHex(hex);
	}

	// Token: 0x0600147C RID: 5244 RVA: 0x00087083 File Offset: 0x00085283
	public static bool FlipTable()
	{
		NetworkSingleton<ManagerPhysicsObject>.Instance.TableScript.TableFlip(null);
		return true;
	}

	// Token: 0x0600147D RID: 5245 RVA: 0x00087096 File Offset: 0x00085296
	public bool ClearContextMenu()
	{
		NetworkSingleton<UserDefinedContextualManager>.Instance.ClearGlobalItems();
		return true;
	}

	// Token: 0x0600147E RID: 5246 RVA: 0x000870A3 File Offset: 0x000852A3
	public bool AddContextMenuItem(string label, Closure function, bool keepOpen = false, bool requireTable = false)
	{
		if (NetworkSingleton<UserDefinedContextualManager>.Instance.AddGlobalItem(label, function, keepOpen, requireTable) == -1)
		{
			base.LogError("addContextMenuItem", "Function required for context menu (label = \"" + label + "\")", null);
			return false;
		}
		return true;
	}

	// Token: 0x0600147F RID: 5247 RVA: 0x000870D6 File Offset: 0x000852D6
	public bool ClearConsoleCommands()
	{
		NetworkSingleton<UserDefinedConsoleCommandManager>.Instance.ClearLuaConsoleCommands();
		return true;
	}

	// Token: 0x06001480 RID: 5248 RVA: 0x000870E3 File Offset: 0x000852E3
	public bool AddConsoleCommand(string label, Closure function, bool adminOnly = false)
	{
		NetworkSingleton<UserDefinedConsoleCommandManager>.Instance.AddConsoleCommand(label, function, adminOnly);
		return true;
	}

	// Token: 0x06001481 RID: 5249 RVA: 0x000870F4 File Offset: 0x000852F4
	public bool ClearHotkeys()
	{
		NetworkSingleton<UserDefinedHotkeyManager>.Instance.ClearHotkeys();
		return true;
	}

	// Token: 0x06001482 RID: 5250 RVA: 0x00087101 File Offset: 0x00085301
	public bool AddHotkey(string label, Closure function, bool triggerOnKeyUp = false)
	{
		NetworkSingleton<UserDefinedHotkeyManager>.Instance.AddHotkey(label, function, triggerOnKeyUp);
		return true;
	}

	// Token: 0x06001483 RID: 5251 RVA: 0x00087114 File Offset: 0x00085314
	public bool ShowHotkeyConfig(string playerColor)
	{
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromColour(Colour.ColourFromLabel(playerColor));
		if (playerState != null)
		{
			base.networkView.RPC(playerState.networkPlayer, new Action(this.RPCShowHotkeyConfig));
			return true;
		}
		return false;
	}

	// Token: 0x06001484 RID: 5252 RVA: 0x00087155 File Offset: 0x00085355
	[Remote(Permission.Server)]
	public void RPCShowHotkeyConfig()
	{
		NetworkSingleton<UserDefinedHotkeyManager>.Instance.ShowSettings(true);
	}

	// Token: 0x06001485 RID: 5253 RVA: 0x00087162 File Offset: 0x00085362
	public static List<LuaGameObjectScript> GetObjects()
	{
		return NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs.ToLGOS();
	}

	// Token: 0x06001486 RID: 5254 RVA: 0x00087174 File Offset: 0x00085374
	public static List<LuaGameObjectScript> GetObjectsWithTag(string label)
	{
		List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
		List<LuaGameObjectScript> list = new List<LuaGameObjectScript>();
		TagLabel label2 = new TagLabel(label);
		int num = NetworkSingleton<ComponentTags>.Instance.TagIndexFromLabel(label2);
		if (num >= 0)
		{
			for (int i = 0; i < grabbableNPOs.Count; i++)
			{
				if (!grabbableNPOs[i].luaGameObjectScript.IsGlobalDummyObject && ComponentTags.GetFlag(grabbableNPOs[i].tags, num))
				{
					list.Add(grabbableNPOs[i].luaGameObjectScript);
				}
			}
		}
		return list;
	}

	// Token: 0x06001487 RID: 5255 RVA: 0x000871FC File Offset: 0x000853FC
	public static List<LuaGameObjectScript> GetObjectsWithAnyTags(List<string> labels)
	{
		List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
		List<LuaGameObjectScript> list = new List<LuaGameObjectScript>();
		List<ulong> flagsB = NetworkSingleton<ComponentTags>.Instance.FlagsFromDisplayedTagLabels(labels);
		for (int i = 0; i < grabbableNPOs.Count; i++)
		{
			if (!grabbableNPOs[i].luaGameObjectScript.IsGlobalDummyObject && ComponentTags.HaveMatchingFlag(grabbableNPOs[i].tags, flagsB))
			{
				list.Add(grabbableNPOs[i].luaGameObjectScript);
			}
		}
		return list;
	}

	// Token: 0x06001488 RID: 5256 RVA: 0x00087274 File Offset: 0x00085474
	public static List<LuaGameObjectScript> GetObjectsWithAllTags(List<string> labels)
	{
		List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
		List<LuaGameObjectScript> list = new List<LuaGameObjectScript>();
		List<ulong> list2 = NetworkSingleton<ComponentTags>.Instance.FlagsFromDisplayedTagLabels(labels);
		for (int i = 0; i < grabbableNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = grabbableNPOs[i];
			List<ulong> flagsA = ComponentTags.NewCopyOfFlags(networkPhysicsObject.tags);
			ComponentTags.AndFlags(ref flagsA, list2);
			if (!networkPhysicsObject.luaGameObjectScript.IsGlobalDummyObject && ComponentTags.FlagsAreIdentical(flagsA, list2))
			{
				list.Add(networkPhysicsObject.luaGameObjectScript);
			}
		}
		return list;
	}

	// Token: 0x06001489 RID: 5257 RVA: 0x000872F4 File Offset: 0x000854F4
	public static List<string> GetAllTags()
	{
		List<int> list = new List<int>();
		List<string> list2 = new List<string>();
		foreach (KeyValuePair<int, TagLabel> keyValuePair in NetworkSingleton<ComponentTags>.Instance.activeLabels)
		{
			list.Add(keyValuePair.Key);
		}
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(NetworkSingleton<ComponentTags>.Instance.activeLabels[list[i]].displayed);
		}
		return list2;
	}

	// Token: 0x0600148A RID: 5258 RVA: 0x00087398 File Offset: 0x00085598
	public static bool Copy(List<LuaGameObjectScript> ObjectsToCopy)
	{
		LuaGlobalScriptManager.Instance.copyObjectStates = new List<ObjectState>();
		foreach (LuaGameObjectScript luaGameObjectScript in ObjectsToCopy)
		{
			if (luaGameObjectScript)
			{
				NetworkPhysicsObject npo = luaGameObjectScript.NPO;
				if (npo.IsSaved)
				{
					ObjectState objectState = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(npo);
					if (true && LuaGlobalScriptManager.Instance.copyObjectStates.Count > 0)
					{
						objectState.Transform.posX -= LuaGlobalScriptManager.Instance.copyObjectStates[0].Transform.posX;
						objectState.Transform.posY -= LuaGlobalScriptManager.Instance.copyObjectStates[0].Transform.posY;
						objectState.Transform.posZ -= LuaGlobalScriptManager.Instance.copyObjectStates[0].Transform.posZ;
					}
					LuaGlobalScriptManager.Instance.copyObjectStates.Add(objectState);
				}
			}
		}
		return true;
	}

	// Token: 0x0600148B RID: 5259 RVA: 0x000874D0 File Offset: 0x000856D0
	public static List<LuaGameObjectScript> Paste(LuaGlobalScriptManager.LuaObjectParameters parameters)
	{
		if (LuaGlobalScriptManager.Instance.copyObjectStates.Count < 1)
		{
			return null;
		}
		List<GameObject> list = NetworkSingleton<ManagerPhysicsObject>.Instance.SpawnObjectStatesOffset(LuaGlobalScriptManager.Instance.copyObjectStates, global::Pointer.GetSpawnPosition(parameters.position, true, 1f), parameters.snap_to_grid, false, true, true);
		List<LuaGameObjectScript> list2 = new List<LuaGameObjectScript>();
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(list[i].GetComponent<LuaGameObjectScript>());
		}
		return list2;
	}

	// Token: 0x0600148C RID: 5260 RVA: 0x0008754A File Offset: 0x0008574A
	public static bool SetLookingForPlayers(bool lookingForPlayers)
	{
		NetworkSingleton<ServerOptions>.Instance.LookingForPlayers = lookingForPlayers;
		return true;
	}

	// Token: 0x0600148D RID: 5261 RVA: 0x00087558 File Offset: 0x00085758
	[Remote(Permission.Server)]
	public void RPCLookAt(float[] Params)
	{
		CameraController instance = Singleton<CameraController>.Instance;
		instance.StartThirdPerson();
		Vector3 position = new Vector3(Params[0], Params[1], Params[2]);
		instance.target.position = position;
		float num = Params[3];
		if (num > 0f)
		{
			instance.distance = num;
		}
		float num2 = Params[4];
		if (num2 >= 0f)
		{
			instance.AngleTwo = num2;
			instance.bRotateVertical = true;
		}
		float num3 = Params[5];
		if (num3 >= 0f)
		{
			instance.Angle = num3;
			instance.bRotateHorizontal = true;
		}
	}

	// Token: 0x0600148E RID: 5262 RVA: 0x000875D8 File Offset: 0x000857D8
	[Remote(Permission.Server)]
	public void RPCAttachCameraToObject(float[] Params, int NPOid, bool Rotate)
	{
		CameraController instance = Singleton<CameraController>.Instance;
		instance.StartFirstPerson();
		Vector3 luaAttachmentOffset = new Vector3(Params[0], Params[1], Params[2]);
		float num = Params[3];
		if (num >= 0f)
		{
			instance.AngleTwo = num;
		}
		float angle = Params[4];
		if (num >= 0f)
		{
			instance.Angle = angle;
		}
		instance.LuaAttachment = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromID(NPOid);
		instance.LuaAttachmentOffset = luaAttachmentOffset;
		instance.bLuaAttached = true;
		instance.bLuaAttachmentRotate = Rotate;
	}

	// Token: 0x0600148F RID: 5263 RVA: 0x00087650 File Offset: 0x00085850
	[Remote(Permission.Server)]
	public void RPCSetCameraMode(CameraController.CameraMode cameraMode)
	{
		CameraController instance = Singleton<CameraController>.Instance;
		switch (cameraMode)
		{
		case CameraController.CameraMode.ThirdPerson:
			instance.StartThirdPerson();
			return;
		case CameraController.CameraMode.FirstPerson:
			instance.StartFirstPerson();
			return;
		case CameraController.CameraMode.TopDown:
			instance.StartTopDown();
			return;
		default:
			instance.StartThirdPerson();
			return;
		}
	}

	// Token: 0x06001490 RID: 5264 RVA: 0x00087694 File Offset: 0x00085894
	public Color ConstructColor(Script script, float? r, float? g, float? b, float? a)
	{
		if (r == null || g == null || b == null)
		{
			throw new ScriptRuntimeException("Error in color: You must specify all of r, g, b.");
		}
		if (a == null)
		{
			a = new float?(1f);
		}
		return new Color(r.Value, g.Value, b.Value, a.Value);
	}

	// Token: 0x06001491 RID: 5265 RVA: 0x00087700 File Offset: 0x00085900
	public Vector3 ConstructVector(Script script, float? x, float? y, float? z)
	{
		if (x == null || y == null || z == null)
		{
			throw new ScriptRuntimeException("Error in vector: You must specify all of x, y, z.");
		}
		return new Vector3(x.Value, y.Value, z.Value);
	}

	// Token: 0x06001492 RID: 5266 RVA: 0x00087750 File Offset: 0x00085950
	public static LuaGameObjectScript GetObjectFromGUID(string GUID)
	{
		NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGUID(GUID);
		if (networkPhysicsObject)
		{
			return networkPhysicsObject.luaGameObjectScript;
		}
		return null;
	}

	// Token: 0x06001493 RID: 5267 RVA: 0x00087779 File Offset: 0x00085979
	private static void CheckThrowSpawnTable(string name)
	{
		if (name.StartsWith("Table_", StringComparison.OrdinalIgnoreCase))
		{
			throw new ScriptRuntimeException("Error in spawnObject you cannot spawn a Table.");
		}
	}

	// Token: 0x06001494 RID: 5268 RVA: 0x00087794 File Offset: 0x00085994
	public LuaGameObjectScript SpawnObject(LuaGlobalScriptManager.LuaObjectParameters parameters)
	{
		if (string.IsNullOrEmpty(parameters.type))
		{
			return null;
		}
		LuaGlobalScriptManager.CheckThrowSpawnTable(parameters.type);
		LuaGameObjectScript component = NetworkSingleton<GameMode>.Instance.SpawnNameCoroutine(parameters.type, parameters.position, false, parameters.snap_to_grid, parameters.sound, false).GetComponent<LuaGameObjectScript>();
		component.transform.eulerAngles = parameters.rotation;
		component.NPO.SetScale(parameters.scale, false);
		if (!string.IsNullOrEmpty(parameters.guid))
		{
			component.NPO.GUID = parameters.guid;
		}
		if (parameters.HasCallback())
		{
			this.spawnObjectCallbacks.Add(component, parameters);
		}
		return component;
	}

	// Token: 0x06001495 RID: 5269 RVA: 0x0008783C File Offset: 0x00085A3C
	public LuaGameObjectScript SpawnObjectJSON(LuaGlobalScriptManager.LuaJsonObjectParameters parameters)
	{
		return this.SpawnObjectSerialized(Json.Load<ObjectState>(parameters.json), parameters);
	}

	// Token: 0x06001496 RID: 5270 RVA: 0x00087850 File Offset: 0x00085A50
	public LuaGameObjectScript SpawnObjectData(LuaGlobalScriptManager.LuaDataObjectParameters parameters)
	{
		return this.SpawnObjectSerialized(parameters.data, parameters);
	}

	// Token: 0x06001497 RID: 5271 RVA: 0x00087860 File Offset: 0x00085A60
	private LuaGameObjectScript SpawnObjectSerialized(ObjectState os, LuaGlobalScriptManager.LuaSerializedObjectParameters parameters)
	{
		if (os == null)
		{
			return null;
		}
		LuaGlobalScriptManager.CheckThrowSpawnTable(os.Name);
		LuaGameObjectScript component = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(os, false, false).GetComponent<LuaGameObjectScript>();
		if (parameters.position != null)
		{
			component.transform.position = parameters.position.Value;
		}
		if (parameters.rotation != null)
		{
			component.transform.eulerAngles = parameters.rotation.Value;
		}
		if (parameters.scale != null)
		{
			component.NPO.SetScale(parameters.scale.Value, false);
		}
		if (!string.IsNullOrEmpty(parameters.guid))
		{
			component.NPO.GUID = parameters.guid;
		}
		if (parameters.HasCallback())
		{
			this.spawnObjectCallbacks.Add(component, parameters);
		}
		return component;
	}

	// Token: 0x06001498 RID: 5272 RVA: 0x00087934 File Offset: 0x00085B34
	public override void OnObjectSpawn(LuaGameObjectScript LGOS)
	{
		base.OnObjectSpawn(LGOS);
		if (LGOS == null)
		{
			return;
		}
		if (this.spawnObjectCallbacks.ContainsKey(LGOS))
		{
			this.spawnObjectCallbacks[LGOS].TryCall(new object[]
			{
				LGOS
			});
			this.spawnObjectCallbacks.Remove(LGOS);
		}
	}

	// Token: 0x06001499 RID: 5273 RVA: 0x0008798C File Offset: 0x00085B8C
	public LuaGameObjectScript DealCardToColorWithOffset(LuaGameObjectScript Deck, List<float> Offset, bool Flip, string Color)
	{
		if (!LuaGlobalScriptManager.HasCorrectNumOfParams<float>(Offset, 3, "dealCardToColorWithOffset"))
		{
			return null;
		}
		DeckScript component = Deck.GetComponent<DeckScript>();
		if (component == null)
		{
			return null;
		}
		GameObject gameObject = component.TakeCard(true, true);
		NetworkPhysicsObject component2 = gameObject.GetComponent<NetworkPhysicsObject>();
		GameObject hand = HandZone.GetHand(Color, 0);
		if (hand == null)
		{
			throw new ScriptRuntimeException("Error in dealCardToColorWithOffset: Player hand color doesn't exist: " + Color);
		}
		Vector3 vector = new Vector3(hand.transform.position.x + hand.transform.right.x * Offset[0] + hand.transform.up.x * Offset[1] + hand.transform.forward.x * Offset[2], hand.transform.position.y, hand.transform.position.z + hand.transform.right.z * Offset[0] + hand.transform.up.z * Offset[1] + hand.transform.forward.z * Offset[2]);
		Vector3 vector2 = NetworkSingleton<ManagerPhysicsObject>.Instance.SurfacePointBelowWorldPos(new Vector3(vector.x, vector.y, vector.z));
		component2.SetCollision(false);
		component2.SetSmoothPosition(new Vector3(vector2.x, vector2.y + Offset[1], vector2.z), false, false, false, true, null, false, false, null);
		if (Flip)
		{
			component2.SetSmoothRotation(new Vector3(hand.transform.localEulerAngles.x, hand.transform.localEulerAngles.y, gameObject.transform.localEulerAngles.z + 180f), false, false, false, true, null, false);
		}
		else
		{
			component2.SetSmoothRotation(new Vector3(hand.transform.localEulerAngles.x, hand.transform.localEulerAngles.y, gameObject.transform.localEulerAngles.z), false, false, false, true, null, false);
		}
		return gameObject.GetComponent<LuaGameObjectScript>();
	}

	// Token: 0x0600149A RID: 5274 RVA: 0x00087BC8 File Offset: 0x00085DC8
	public List<LuaGameObjectScript> Group(List<LuaGameObjectScript> groupObjects)
	{
		List<GameObject> list = new List<GameObject>();
		foreach (LuaGameObjectScript luaGameObjectScript in groupObjects)
		{
			if (luaGameObjectScript)
			{
				list.Add(luaGameObjectScript.gameObject);
			}
		}
		return NetworkSingleton<ManagerPhysicsObject>.Instance.Group(list).ToLGOS();
	}

	// Token: 0x0600149B RID: 5275 RVA: 0x00087C3C File Offset: 0x00085E3C
	private static void walkDict(Dictionary<object, object> dict)
	{
		foreach (KeyValuePair<object, object> keyValuePair in dict)
		{
			try
			{
				LuaGlobalScriptManager.walkDict((Dictionary<object, object>)keyValuePair.Value);
			}
			catch (InvalidCastException)
			{
				Debug.Log(string.Format("{0}: {1}", keyValuePair.Key, keyValuePair.Value));
			}
		}
	}

	// Token: 0x0600149C RID: 5276 RVA: 0x00087CC4 File Offset: 0x00085EC4
	public bool SendExternalCustomMessage(Dictionary<object, object> message)
	{
		base.StartCoroutine(this.PushLuaMessage(new LuaExternalCustomMessage(message), null));
		return true;
	}

	// Token: 0x0600149D RID: 5277 RVA: 0x00087CDB File Offset: 0x00085EDB
	public bool SendExternalGameSaved(string filepath)
	{
		base.StartCoroutine(this.PushLuaMessage(new LuaExternalGameSaved(filepath), null));
		return true;
	}

	// Token: 0x0600149E RID: 5278 RVA: 0x00087CF2 File Offset: 0x00085EF2
	public bool SendExternalObjectCreated(string guid)
	{
		base.StartCoroutine(this.PushLuaMessage(new LuaExternalObjectCreated(guid), null));
		return true;
	}

	// Token: 0x0600149F RID: 5279 RVA: 0x00087D0C File Offset: 0x00085F0C
	private void SendLuaExternalNewGame()
	{
		string savePath = "";
		if (!SerializationScript.LastLoadedJsonFilePath.Contains(DirectoryScript.workshopFilePath))
		{
			savePath = SerializationScript.LastLoadedJsonFilePath;
		}
		base.StartCoroutine(this.PushLuaMessage(new LuaExternalNewGame(LuaGlobalScriptManager.GetAllLuaScriptStates(), savePath), null));
	}

	// Token: 0x060014A0 RID: 5280 RVA: 0x00087D4F File Offset: 0x00085F4F
	private IEnumerator StartLuaEditorServer()
	{
		this.luaJob = new LuaEditorJob();
		this.luaJob.Start(false, false);
		bool requiresRecompile = false;
		while (!this.luaJob.Update())
		{
			switch (this.luaJob.messageID)
			{
			case LuaReceiveExternalMessageType.None:
				yield return null;
				continue;
			case LuaReceiveExternalMessageType.GetScripts:
				this.SendLuaExternalNewGame();
				break;
			case LuaReceiveExternalMessageType.SavePlay:
				this.SetAllLuaObjectsFromStates(this.luaJob.response.scriptStates);
				requiresRecompile = true;
				break;
			case LuaReceiveExternalMessageType.CustomMessage:
				LuaGlobalScriptManager.TriggerExternalCustomMessage(this.luaJob.response.customMessage);
				break;
			case LuaReceiveExternalMessageType.ExecuteScript:
			{
				string guid = this.luaJob.response.guid;
				string script = this.luaJob.response.script;
				int returnID = this.luaJob.response.returnID;
				LuaScript luaScript = null;
				if (guid == "-1")
				{
					luaScript = this;
				}
				else
				{
					NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGUID(guid);
					if (networkPhysicsObject)
					{
						luaScript = networkPhysicsObject.luaGameObjectScript;
					}
				}
				if (luaScript)
				{
					DynValue dynValue = luaScript.ExecuteScript(script, false);
					if (returnID != -1)
					{
						base.StartCoroutine(this.PushLuaMessage(new LuaExternalReturnExecuteScript((dynValue != null) ? dynValue.ToObject() : null, returnID), null));
					}
				}
				else
				{
					Chat.LogError("Error finding guid for execute script: " + guid, true);
					this.PushLuaErrorMessage(guid, "-1", "Error finding guid for execute script: ");
				}
				break;
			}
			}
			SystemConsole.UserDebug(DebugType.External_API, "▕ Handled message: " + this.luaJob.messageID);
			if (!string.IsNullOrEmpty(this.luaJob.Ex))
			{
				Debug.LogError(this.luaJob.Ex);
			}
			this.luaJob.messageID = LuaReceiveExternalMessageType.None;
			this.luaJob.mainThreadDone = true;
			if (requiresRecompile)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.Recompile(true);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060014A1 RID: 5281 RVA: 0x00087D5E File Offset: 0x00085F5E
	private IEnumerator PushLuaMessage(LuaExternalMessage externalMessage, Action<LuaEditorPushJob> callback = null)
	{
		LuaEditorPushJob luaPushJob = new LuaEditorPushJob
		{
			message = Json.GetJson(externalMessage, true)
		};
		if (externalMessage.messageID == LuaSendExternalMessageType.CustomMessage)
		{
			Debug.Log(luaPushJob.message);
		}
		luaPushJob.Start(true, false);
		while (!luaPushJob.Update())
		{
			yield return null;
		}
		if (callback != null)
		{
			callback(luaPushJob);
		}
		yield break;
	}

	// Token: 0x17000379 RID: 889
	// (get) Token: 0x060014A2 RID: 5282 RVA: 0x00087D74 File Offset: 0x00085F74
	public static LuaGlobalScriptManager Instance
	{
		get
		{
			if (!LuaGlobalScriptManager._Instance)
			{
				LuaGlobalScriptManager._Instance = UnityEngine.Object.FindObjectOfType<LuaGlobalScriptManager>();
			}
			return LuaGlobalScriptManager._Instance;
		}
	}

	// Token: 0x060014A3 RID: 5283 RVA: 0x00087D91 File Offset: 0x00085F91
	public void OnFileSave(string filepath)
	{
		this.SendExternalGameSaved(filepath);
	}

	// Token: 0x060014A4 RID: 5284 RVA: 0x00087D9B File Offset: 0x00085F9B
	public void OnNetworkObjectSpawnFromUI(NetworkPhysicsObject NPO)
	{
		this.SendExternalObjectCreated(NPO.GUID);
	}

	// Token: 0x060014A5 RID: 5285 RVA: 0x00087DAC File Offset: 0x00085FAC
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCCallbackFromClient(int requestedCallbackHash)
	{
		if (!Network.isServer)
		{
			return;
		}
		Action action;
		if (this.ActionCallbackFromPlayerID.TryGetValue((int)Network.sender.id, out action) && action.GetHashCode() == requestedCallbackHash)
		{
			action();
			this.ActionCallbackFromPlayerID.Remove((int)Network.sender.id);
		}
	}

	// Token: 0x060014A6 RID: 5286 RVA: 0x00087E08 File Offset: 0x00086008
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCCallbackFromClient(int requestedCallbackHash, string text)
	{
		if (!Network.isServer)
		{
			return;
		}
		Action<string> action;
		if (this.ActionStringCallbackFromPlayerID.TryGetValue((int)Network.sender.id, out action) && action.GetHashCode() == requestedCallbackHash)
		{
			action(text);
			this.ActionStringCallbackFromPlayerID.Remove((int)Network.sender.id);
		}
	}

	// Token: 0x060014A7 RID: 5287 RVA: 0x00087E64 File Offset: 0x00086064
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCCallbackFromClient(int requestedCallbackHash, Color color)
	{
		if (!Network.isServer)
		{
			return;
		}
		Action<Color> action;
		if (this.ActionColorCallbackFromPlayerID.TryGetValue((int)Network.sender.id, out action))
		{
			if (action.GetHashCode() == requestedCallbackHash)
			{
				action(color);
			}
			this.ActionColorCallbackFromPlayerID.Remove((int)Network.sender.id);
		}
	}

	// Token: 0x060014A8 RID: 5288 RVA: 0x00087EC0 File Offset: 0x000860C0
	[Remote(Permission.Server)]
	public void RPCShowColorPicker(int callbackHash, Color color)
	{
		Singleton<UIColorPickerStandaloneDialog>.Instance.DefaultColor = color;
		NetworkSingleton<NetworkUI>.Instance.GUIColorPickerScript.Show(color, delegate(Color pickedColor)
		{
			Singleton<UIColorPickerStandaloneDialog>.Instance.DefaultColor = pickedColor;
			this.networkView.RPC<int, Color>(RPCTarget.Server, new Action<int, Color>(this.RPCCallbackFromClient), callbackHash, pickedColor);
		});
	}

	// Token: 0x060014A9 RID: 5289 RVA: 0x00087F08 File Offset: 0x00086108
	[Remote(Permission.Server)]
	public void RPCInfoShow(string info)
	{
		UIDialog.Show(UIDialog.LuaDialog, info, "OK", null);
	}

	// Token: 0x060014AA RID: 5290 RVA: 0x00087F1C File Offset: 0x0008611C
	[Remote(Permission.Server)]
	public void RPCShowConfirm(string info, int callbackHash)
	{
		UIDialog.Show(UIDialog.LuaDialog, info, "OK", "Cancel", delegate()
		{
			this.networkView.RPC<int>(RPCTarget.Server, new Action<int>(this.RPCCallbackFromClient), callbackHash);
		}, null);
	}

	// Token: 0x060014AB RID: 5291 RVA: 0x00087F60 File Offset: 0x00086160
	[Remote(Permission.Server)]
	public void RPCShowInput(int callbackHash, string description, string defaultString)
	{
		UIDialog.ShowInput(UIDialog.LuaDialog, description, "OK", "Cancel", delegate(string text)
		{
			this.networkView.RPC<int, string>(RPCTarget.Server, new Action<int, string>(this.RPCCallbackFromClient), callbackHash, text);
		}, null, defaultString, "");
	}

	// Token: 0x060014AC RID: 5292 RVA: 0x00087FAC File Offset: 0x000861AC
	[Remote(Permission.Server)]
	public void RPCShowMemoInput(int callbackHash, string description, string defaultString)
	{
		UIDialog.ShowMemoInput(UIDialog.LuaDialog, description, "OK", "Cancel", delegate(string text, string _)
		{
			this.networkView.RPC<int, string>(RPCTarget.Server, new Action<int, string>(this.RPCCallbackFromClient), callbackHash, text);
		}, null, defaultString, "");
	}

	// Token: 0x060014AD RID: 5293 RVA: 0x00087FF8 File Offset: 0x000861F8
	[Remote(Permission.Server)]
	public void RPCShowDropDown(int callbackHash, string description, List<string> dropDownOptions, string dropDownValue)
	{
		UIDialog.ShowDropDown(UIDialog.LuaDialog, description, "OK", "Cancel", dropDownOptions, delegate(string text)
		{
			this.networkView.RPC<int, string>(RPCTarget.Server, new Action<int, string>(this.RPCCallbackFromClient), callbackHash, text);
		}, null, dropDownValue);
	}

	// Token: 0x060014AE RID: 5294 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
	[Obsolete("Pixel paint removed.")]
	public static bool ClearPixelPaint()
	{
		return false;
	}

	// Token: 0x060014AF RID: 5295 RVA: 0x0008803E File Offset: 0x0008623E
	[Obsolete("Use Global.setVectorLines() instead.")]
	public static bool ClearVectorPaint()
	{
		NetworkSingleton<ToolVector>.Instance.RemoveAll();
		return true;
	}

	// Token: 0x060014B0 RID: 5296 RVA: 0x0008804C File Offset: 0x0008624C
	[Obsolete("Moved inside of LuaPlayer.")]
	public static List<string> GetSeatedPlayers()
	{
		List<PlayerState> playersList = NetworkSingleton<PlayerManager>.Instance.PlayersList;
		List<string> list = new List<string>();
		for (int i = 0; i < playersList.Count; i++)
		{
			if (playersList[i].stringColor != "Black" && playersList[i].stringColor != "Grey")
			{
				list.Add(playersList[i].stringColor);
			}
		}
		return list;
	}

	// Token: 0x060014B1 RID: 5297 RVA: 0x000880C0 File Offset: 0x000862C0
	[Obsolete("Moved inside of LuaPlayer.")]
	public string ColorToPlayerName(string Color)
	{
		string result = "";
		try
		{
			result = NetworkSingleton<PlayerManager>.Instance.NameFromColour(Colour.ColourFromLabel(Color));
		}
		catch (Exception ex)
		{
			throw new ScriptRuntimeException("Error in ColorToPlayerName (probably color isn't seated/doesn't exist): " + ex.Message);
		}
		return result;
	}

	// Token: 0x060014B2 RID: 5298 RVA: 0x00088110 File Offset: 0x00086310
	[Obsolete("Moved inside of LuaPlayer.")]
	public static Table GetPlayerHandPositionAndRotation(Script script, string Color)
	{
		Table table = new Table(script);
		GameObject hand = HandZone.GetHand(Color, 0);
		if (hand == null)
		{
			return null;
		}
		Vector3 worldPos = new Vector3(hand.transform.position.x, hand.transform.position.y, hand.transform.position.z);
		Vector3 vector = NetworkSingleton<ManagerPhysicsObject>.Instance.SurfacePointBelowWorldPos(worldPos);
		Vector3 eulerAngles = hand.transform.rotation.eulerAngles;
		table["pos_x"] = vector.x;
		table["pos_y"] = vector.y;
		table["pos_z"] = vector.z;
		table["rot_x"] = eulerAngles.x;
		table["rot_y"] = eulerAngles.y;
		table["rot_z"] = eulerAngles.z;
		table["trigger_forward_x"] = hand.transform.forward.normalized.x;
		table["trigger_forward_y"] = hand.transform.forward.normalized.y;
		table["trigger_forward_z"] = hand.transform.forward.normalized.z;
		table["trigger_right_x"] = hand.transform.right.normalized.x;
		table["trigger_right_y"] = hand.transform.right.normalized.y;
		table["trigger_right_z"] = hand.transform.right.normalized.z;
		table["trigger_up_x"] = hand.transform.up.normalized.x;
		table["trigger_up_y"] = hand.transform.up.normalized.y;
		table["trigger_up_z"] = hand.transform.up.normalized.z;
		return table;
	}

	// Token: 0x060014B3 RID: 5299 RVA: 0x00088389 File Offset: 0x00086589
	[Obsolete("Made into static global class.")]
	public static LuaPlayer GetPlayer(string color)
	{
		return LuaPlayer.GetHandPlayer(color);
	}

	// Token: 0x060014B4 RID: 5300 RVA: 0x00088394 File Offset: 0x00086594
	[Obsolete("Moved inside of LuaPlayer.")]
	public static Table GetPointerPosition(Script script, string PlayerColor)
	{
		global::Pointer pointer = NetworkSingleton<ManagerPhysicsObject>.Instance.PointerFromColorLabel(PlayerColor);
		if (pointer == null)
		{
			return null;
		}
		return LuaCustomConverter.GetTable(pointer.gameObject.transform.position, script);
	}

	// Token: 0x060014B5 RID: 5301 RVA: 0x000883D0 File Offset: 0x000865D0
	[Obsolete("Moved inside of LuaPlayer.")]
	public static float? GetPointerRotation(Script script, string PlayerColor)
	{
		global::Pointer pointer = NetworkSingleton<ManagerPhysicsObject>.Instance.PointerFromColorLabel(PlayerColor);
		if (pointer == null)
		{
			return null;
		}
		return new float?(pointer.gameObject.transform.localRotation.eulerAngles.y);
	}

	// Token: 0x060014B6 RID: 5302 RVA: 0x0008841E File Offset: 0x0008661E
	[Obsolete("Use Global.getVar() instead.")]
	public static object GetGlobalVar(string VarName)
	{
		if (LuaGlobalScriptManager.Instance.lua == null)
		{
			return null;
		}
		return LuaGlobalScriptManager.Instance.lua.Globals[VarName];
	}

	// Token: 0x060014B7 RID: 5303 RVA: 0x00088444 File Offset: 0x00086644
	[Obsolete("Use Global.getTable() instead.")]
	public static Table GetGlobalTable(Script script, string TableName)
	{
		if (LuaGlobalScriptManager.Instance.lua == null)
		{
			return null;
		}
		Table table = (Table)LuaGlobalScriptManager.Instance.lua.Globals[TableName];
		Table table2 = new Table(script);
		foreach (DynValue key in table.Keys)
		{
			table2[key] = table[key];
		}
		return table2;
	}

	// Token: 0x060014B8 RID: 5304 RVA: 0x000884CC File Offset: 0x000866CC
	[Obsolete("Use Global.setTable() instead.")]
	public static bool SetGlobalTable(string TableName, Table SourceTable)
	{
		if (LuaGlobalScriptManager.Instance.lua == null)
		{
			return false;
		}
		Table table = new Table(LuaGlobalScriptManager.Instance.lua);
		foreach (DynValue key in SourceTable.Keys)
		{
			table[key] = SourceTable[key];
		}
		LuaGlobalScriptManager.Instance.lua.Globals[TableName] = table;
		return true;
	}

	// Token: 0x060014B9 RID: 5305 RVA: 0x00088558 File Offset: 0x00086758
	[Obsolete("Use Global.setVar() instead.")]
	public static bool SetGlobalScriptVar(string VarName, object Value)
	{
		LuaGlobalScriptManager.Instance.lua.Globals[VarName] = Value;
		return true;
	}

	// Token: 0x060014BA RID: 5306 RVA: 0x00088574 File Offset: 0x00086774
	[Obsolete("Use obj/Global.call() instead.")]
	public DynValue CallLuaFunctionInOtherScript(Script CallingScript, LuaGameObjectScript FunctionOwner, string LuaFunction)
	{
		DynValue dynValue = null;
		if (this.loaded)
		{
			LuaScript luaScript = LuaScript.ScriptToLuaScript(CallingScript);
			Script script = LuaGlobalScriptManager.FunctionOwnerToScript(ref FunctionOwner);
			if (script != null && script.Globals[LuaFunction] != null)
			{
				try
				{
					dynValue = script.Call(script.Globals[LuaFunction]);
				}
				catch (Exception e)
				{
					luaScript.LogError("call/" + LuaFunction, e, null);
				}
			}
		}
		if (dynValue != null && dynValue.Type == DataType.Table)
		{
			dynValue = DynValue.NewTable(LuaGlobalScriptManager.CopyLuaTable(dynValue.Table, CallingScript));
		}
		return dynValue;
	}

	// Token: 0x060014BB RID: 5307 RVA: 0x00088608 File Offset: 0x00086808
	[Obsolete("Use obj/Global.call() instead.")]
	public DynValue CallLuaFunctionInOtherScript(Script CallingScript, LuaGameObjectScript FunctionOwner, string LuaFunction, Table param)
	{
		DynValue dynValue = null;
		if (this.loaded)
		{
			LuaScript luaScript = LuaScript.ScriptToLuaScript(CallingScript);
			Script script = LuaGlobalScriptManager.FunctionOwnerToScript(ref FunctionOwner);
			if (script != null && script.Globals[LuaFunction] != null)
			{
				try
				{
					Table table = LuaGlobalScriptManager.CopyLuaTable(param, script);
					dynValue = script.Call(script.Globals[LuaFunction], new object[]
					{
						table
					});
				}
				catch (Exception e)
				{
					luaScript.LogError("call/" + LuaFunction, e, null);
				}
			}
		}
		if (dynValue != null && dynValue.Type == DataType.Table)
		{
			dynValue = DynValue.NewTable(LuaGlobalScriptManager.CopyLuaTable(dynValue.Table, CallingScript));
		}
		return dynValue;
	}

	// Token: 0x060014BC RID: 5308 RVA: 0x000886B0 File Offset: 0x000868B0
	[Obsolete("Use getObjects() instead. We ignore Hand Zones in this old function for compatability")]
	public static List<LuaGameObjectScript> GetAllObjects()
	{
		List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
		List<LuaGameObjectScript> list = new List<LuaGameObjectScript>(grabbableNPOs.Count);
		for (int i = 0; i < grabbableNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = grabbableNPOs[i];
			if (!networkPhysicsObject.handZone)
			{
				list.Add(networkPhysicsObject.luaGameObjectScript);
			}
		}
		return list;
	}

	// Token: 0x04000BC6 RID: 3014
	private readonly List<string> OldDefaultGlobalLuaCodes = new List<string>
	{
		"--[[ Lua code. See documentation: http://berserk-games.com/knowledgebase/scripting/ --]]\n\n--[[ The OnLoad function. This is called after everything in the game save finishes loading.\nMost of your script code goes here. --]]\nfunction onload()\n    --[[ print('Onload!') --]]\nend\n\n--[[ The Update function. This is called once per frame. --]]\nfunction update ()\n    --[[ print('Update loop!') --]]\nend\n\n--[[ The OnPlayerChangedColor function. This is mostly used when a player joins the game and chooses a color. --]]\nfunction onPlayerChangedColor(color)\n    --[[ print('Player changed color! ' .. color .. ' ' .. colorToPlayerName(color)) --]]\nend",
		"--[[ Lua code. See documentation: http://berserk-games.com/knowledgebase/scripting/ --]]\n\n--[[ The OnLoad function. This is called after everything in the game save finishes loading.\nMost of your script code goes here. --]]\nfunction onload()\n    --[[ print('Onload!') --]]\nend\n\n--[[ The Update function. This is called once per frame. --]]\nfunction update ()\n    --[[ print('Update loop!') --]]\nend",
		"--[[ Lua code. See documentation: http://berserk-games.com/knowledgebase/scripting/ --]]\n\n--[[ The onLoad event is called after the game save finishes loading. --]]\nfunction onLoad()\n    --[[ print('onLoad!') --]]\nend\n\n--[[ The onUpdate event is called once per frame. --]]\nfunction onUpdate ()\n    --[[ print('onUpdate loop!') --]]\nend"
	};

	// Token: 0x04000BC7 RID: 3015
	private const string DefaultGlobalLuaCode = "--[[ Lua code. See documentation: https://api.tabletopsimulator.com/ --]]\n\n--[[ The onLoad event is called after the game save finishes loading. --]]\nfunction onLoad()\n    --[[ print('onLoad!') --]]\nend\n\n--[[ The onUpdate event is called once per frame. --]]\nfunction onUpdate()\n    --[[ print('onUpdate loop!') --]]\nend";

	// Token: 0x04000BC8 RID: 3016
	public bool autoLoadOnce;

	// Token: 0x04000BC9 RID: 3017
	private List<ObjectState> copyObjectStates;

	// Token: 0x04000BCA RID: 3018
	public const float LuaUIScaleMulti = 3f;

	// Token: 0x04000BCB RID: 3019
	[MoonSharpHidden]
	public LuaGameObjectScript GlobalDummyObject;

	// Token: 0x04000BCC RID: 3020
	[MoonSharpHidden]
	public LuaGlobalPlayer GlobalPlayer;

	// Token: 0x04000BCD RID: 3021
	[MoonSharpHidden]
	public LuaTimer GlobalTimer;

	// Token: 0x04000BCE RID: 3022
	[MoonSharpHidden]
	public LuaLighting GlobalLighting;

	// Token: 0x04000BCF RID: 3023
	[MoonSharpHidden]
	public LuaGrid GlobalGrid;

	// Token: 0x04000BD0 RID: 3024
	[MoonSharpHidden]
	public LuaTurns GlobalTurns;

	// Token: 0x04000BD1 RID: 3025
	[MoonSharpHidden]
	public LuaPhysics GlobalPhysics;

	// Token: 0x04000BD2 RID: 3026
	[MoonSharpHidden]
	public LuaWebRequestManager GlobalWebRequest;

	// Token: 0x04000BD3 RID: 3027
	[MoonSharpHidden]
	public LuaUI GlobalUI;

	// Token: 0x04000BD4 RID: 3028
	[MoonSharpHidden]
	public LuaNotes GlobalNotes;

	// Token: 0x04000BD5 RID: 3029
	[MoonSharpHidden]
	public LuaWait GlobalWait;

	// Token: 0x04000BD6 RID: 3030
	[MoonSharpHidden]
	public LuaMusicPlayer GlobalMusicPlayer;

	// Token: 0x04000BD7 RID: 3031
	[MoonSharpHidden]
	public LuaTime GlobalTime;

	// Token: 0x04000BD8 RID: 3032
	[MoonSharpHidden]
	public LuaHands GlobalHands;

	// Token: 0x04000BD9 RID: 3033
	[MoonSharpHidden]
	public LuaTables GlobalTables;

	// Token: 0x04000BDA RID: 3034
	[MoonSharpHidden]
	public LuaBackgrounds GlobalBackgrounds;

	// Token: 0x04000BDB RID: 3035
	[MoonSharpHidden]
	public LuaInfo GlobalInfo;

	// Token: 0x04000BDC RID: 3036
	[MoonSharpHidden]
	private UnityEngine.Coroutine editorServerCoroutine;

	// Token: 0x04000BDD RID: 3037
	[MoonSharpHidden]
	private LuaEditorJob luaJob;

	// Token: 0x04000BDF RID: 3039
	private static readonly char[] scriptTrimChars = new char[]
	{
		'\r',
		'\n'
	};

	// Token: 0x04000BE0 RID: 3040
	private GameObject pushNewLuaObject;

	// Token: 0x04000BE1 RID: 3041
	private List<UnityEngine.Coroutine> LuaCoroutines = new List<UnityEngine.Coroutine>();

	// Token: 0x04000BE2 RID: 3042
	private Dictionary<LuaGameObjectScript, LuaGlobalScriptManager.LuaCallbackDeprecated> spawnObjectCallbacks = new Dictionary<LuaGameObjectScript, LuaGlobalScriptManager.LuaCallbackDeprecated>();

	// Token: 0x04000BE3 RID: 3043
	private static LuaGlobalScriptManager _Instance;

	// Token: 0x04000BE4 RID: 3044
	[NonSerialized]
	public Dictionary<int, Action> ActionCallbackFromPlayerID = new Dictionary<int, Action>();

	// Token: 0x04000BE5 RID: 3045
	[NonSerialized]
	public Dictionary<int, Action<string>> ActionStringCallbackFromPlayerID = new Dictionary<int, Action<string>>();

	// Token: 0x04000BE6 RID: 3046
	[NonSerialized]
	public Dictionary<int, Action<Color>> ActionColorCallbackFromPlayerID = new Dictionary<int, Action<Color>>();

	// Token: 0x02000676 RID: 1654
	// (Invoke) Token: 0x06003B8D RID: 15245
	public delegate void ExternalCustomMessage(Dictionary<object, object> message);

	// Token: 0x02000677 RID: 1655
	public class ObjectInfo
	{
		// Token: 0x0400283F RID: 10303
		public string name;

		// Token: 0x04002840 RID: 10304
		public List<LuaGlobalScriptManager.VariableInfo> variables = new List<LuaGlobalScriptManager.VariableInfo>();

		// Token: 0x04002841 RID: 10305
		public List<LuaGlobalScriptManager.FunctionInfo> functions = new List<LuaGlobalScriptManager.FunctionInfo>();
	}

	// Token: 0x02000678 RID: 1656
	public class VariableInfo
	{
		// Token: 0x04002842 RID: 10306
		public string name;

		// Token: 0x04002843 RID: 10307
		public string type;
	}

	// Token: 0x02000679 RID: 1657
	public class FunctionInfo
	{
		// Token: 0x04002844 RID: 10308
		public string name;

		// Token: 0x04002845 RID: 10309
		public string returnType;

		// Token: 0x04002846 RID: 10310
		public List<LuaGlobalScriptManager.VariableInfo> parameters;
	}

	// Token: 0x0200067A RID: 1658
	public class LuaCallbackBase
	{
		// Token: 0x06003B93 RID: 15251 RVA: 0x0017754A File Offset: 0x0017574A
		[MoonSharpHidden]
		public virtual DynValue TryCall()
		{
			if (this.callback_function != null)
			{
				return LuaScript.TryCall(this.callback_function);
			}
			return null;
		}

		// Token: 0x06003B94 RID: 15252 RVA: 0x00177561 File Offset: 0x00175761
		[MoonSharpHidden]
		public virtual DynValue TryCall(params object[] args)
		{
			if (this.callback_function != null)
			{
				return LuaScript.TryCall(this.callback_function, args);
			}
			return null;
		}

		// Token: 0x06003B95 RID: 15253 RVA: 0x00177579 File Offset: 0x00175779
		[MoonSharpHidden]
		public virtual bool HasCallback()
		{
			return this.callback_function != null;
		}

		// Token: 0x04002847 RID: 10311
		public Closure callback_function;
	}

	// Token: 0x0200067B RID: 1659
	public class LuaCallbackDeprecated : LuaGlobalScriptManager.LuaCallbackBase
	{
		// Token: 0x06003B97 RID: 15255 RVA: 0x00177584 File Offset: 0x00175784
		[MoonSharpHidden]
		public override DynValue TryCall()
		{
			if (base.HasCallback())
			{
				return base.TryCall();
			}
			if (!string.IsNullOrEmpty(this.callback))
			{
				LuaGameObjectScript luaGameObjectScript = this.callback_owner;
				LuaGlobalScriptManager.FunctionOwnerToScript(ref luaGameObjectScript);
				if (luaGameObjectScript != null)
				{
					if (this.@params != null)
					{
						luaGameObjectScript.Call(this.callback, new object[]
						{
							this.@params
						});
					}
					else
					{
						luaGameObjectScript.Call(this.callback);
					}
				}
			}
			return null;
		}

		// Token: 0x06003B98 RID: 15256 RVA: 0x001775FC File Offset: 0x001757FC
		[MoonSharpHidden]
		public override DynValue TryCall(params object[] args)
		{
			if (base.HasCallback())
			{
				return base.TryCall(args);
			}
			if (!string.IsNullOrEmpty(this.callback))
			{
				LuaGameObjectScript luaGameObjectScript = this.callback_owner;
				if (LuaGlobalScriptManager.FunctionOwnerToScript(ref luaGameObjectScript) != null)
				{
					if (this.@params != null)
					{
						if (args.Length == 1)
						{
							luaGameObjectScript.Call(this.callback, new object[]
							{
								args[0],
								this.@params
							});
						}
						else
						{
							luaGameObjectScript.Call(this.callback, new object[]
							{
								args,
								this.@params
							});
						}
					}
					else
					{
						luaGameObjectScript.Call(this.callback, args);
					}
				}
			}
			return null;
		}

		// Token: 0x06003B99 RID: 15257 RVA: 0x00177699 File Offset: 0x00175899
		[MoonSharpHidden]
		public override bool HasCallback()
		{
			return base.HasCallback() || !string.IsNullOrEmpty(this.callback);
		}

		// Token: 0x04002848 RID: 10312
		public string callback;

		// Token: 0x04002849 RID: 10313
		public LuaGameObjectScript callback_owner;

		// Token: 0x0400284A RID: 10314
		public Table @params;
	}

	// Token: 0x0200067C RID: 1660
	public class LuaObjectBaseParameters : LuaGlobalScriptManager.LuaCallbackDeprecated
	{
		// Token: 0x0400284B RID: 10315
		public bool sound = true;

		// Token: 0x0400284C RID: 10316
		public bool snap_to_grid;

		// Token: 0x0400284D RID: 10317
		public string guid;
	}

	// Token: 0x0200067D RID: 1661
	public class LuaObjectParameters : LuaGlobalScriptManager.LuaObjectBaseParameters
	{
		// Token: 0x0400284E RID: 10318
		public string type;

		// Token: 0x0400284F RID: 10319
		public Vector3 position = new Vector3(0f, 3f, 0f);

		// Token: 0x04002850 RID: 10320
		public Vector3 rotation = Vector3.zero;

		// Token: 0x04002851 RID: 10321
		public Vector3 scale = Vector3.one;
	}

	// Token: 0x0200067E RID: 1662
	public class LuaSerializedObjectParameters : LuaGlobalScriptManager.LuaObjectBaseParameters
	{
		// Token: 0x04002852 RID: 10322
		public Vector3? position;

		// Token: 0x04002853 RID: 10323
		public Vector3? rotation;

		// Token: 0x04002854 RID: 10324
		public Vector3? scale;
	}

	// Token: 0x0200067F RID: 1663
	public class LuaJsonObjectParameters : LuaGlobalScriptManager.LuaSerializedObjectParameters
	{
		// Token: 0x04002855 RID: 10325
		public string json;
	}

	// Token: 0x02000680 RID: 1664
	public class LuaDataObjectParameters : LuaGlobalScriptManager.LuaSerializedObjectParameters
	{
		// Token: 0x04002856 RID: 10326
		public ObjectState data;
	}
}
