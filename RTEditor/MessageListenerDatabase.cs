using System;
using System.Collections.Generic;

namespace RTEditor
{
	// Token: 0x0200043D RID: 1085
	public class MessageListenerDatabase : MonoSingletonBase<MessageListenerDatabase>
	{
		// Token: 0x060031CD RID: 12749 RVA: 0x00151018 File Offset: 0x0014F218
		public void SendMessageToInterestedListeners(Message message)
		{
			HashSet<IMessageListener> listeners = null;
			if (this.TryGetListenersForMessage(message, out listeners))
			{
				this.SendMessageToListeners(message, listeners);
			}
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x0015103A File Offset: 0x0014F23A
		public void RegisterListenerForMessage(MessageType messageType, IMessageListener messageListener)
		{
			if (this.DoesListenerListenToMessage(messageType, messageListener))
			{
				return;
			}
			this.RegisterNewMessageTypeIfNecessary(messageType);
			this._messageTypeToMessageListeners[messageType].Add(messageListener);
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x00151064 File Offset: 0x0014F264
		private bool DoesListenerListenToMessage(MessageType messageType, IMessageListener messageListener)
		{
			HashSet<IMessageListener> hashSet = null;
			return this._messageTypeToMessageListeners.TryGetValue(messageType, out hashSet) && hashSet.Contains(messageListener);
		}

		// Token: 0x060031D0 RID: 12752 RVA: 0x0015108C File Offset: 0x0014F28C
		private void RegisterNewMessageTypeIfNecessary(MessageType messageType)
		{
			if (!this._messageTypeToMessageListeners.ContainsKey(messageType))
			{
				this._messageTypeToMessageListeners.Add(messageType, new HashSet<IMessageListener>());
			}
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x001510AD File Offset: 0x0014F2AD
		private bool TryGetListenersForMessage(Message message, out HashSet<IMessageListener> listeners)
		{
			listeners = null;
			if (this._messageTypeToMessageListeners.ContainsKey(message.Type))
			{
				listeners = this._messageTypeToMessageListeners[message.Type];
				return true;
			}
			return false;
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x001510DC File Offset: 0x0014F2DC
		private void SendMessageToListeners(Message message, HashSet<IMessageListener> listeners)
		{
			foreach (IMessageListener messageListener in listeners)
			{
				messageListener.RespondToMessage(message);
			}
		}

		// Token: 0x04002021 RID: 8225
		private Dictionary<MessageType, HashSet<IMessageListener>> _messageTypeToMessageListeners = new Dictionary<MessageType, HashSet<IMessageListener>>();
	}
}
