using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

// Token: 0x02000131 RID: 305
public static class Proxy
{
	// Token: 0x0600101E RID: 4126 RVA: 0x0006D894 File Offset: 0x0006BA94
	public static TcpClient Socks5Connect(string _proxyAddress, int _proxyPort, string _DestAddress, int _DestPort)
	{
		TcpClient tcpClient = new TcpClient(_proxyAddress, _proxyPort);
		Socket client = tcpClient.Client;
		TcpClient result;
		using (NetworkStream networkStream = new NetworkStream(client))
		{
			BinaryReader binaryReader = new BinaryReader(networkStream);
			Socket socket = client;
			byte[] array = new byte[3];
			array[0] = 5;
			array[1] = 1;
			socket.Send(array);
			byte[] array2 = binaryReader.ReadBytes(2);
			Debug.Log("<<<<<<<socks5 received1>>>>>>>>");
			if (array2[0] != 5 && array2[1] != 1)
			{
				throw new Exception();
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
				binaryWriter.Write(new byte[]
				{
					5,
					1,
					0,
					3
				});
				binaryWriter.Write(_DestAddress);
				binaryWriter.Write(BitConverter.GetBytes((ushort)_DestPort).ReverseA<byte>());
				client.Send(memoryStream.ToArray());
			}
			Debug.Log("<<<<<<<<socks5 received2>>>>>>>>");
			byte[] array3 = binaryReader.ReadBytes(10);
			if (array3[1] != 0)
			{
				throw new Exception("socket Error: " + array3[1]);
			}
			result = tcpClient;
		}
		return result;
	}
}
