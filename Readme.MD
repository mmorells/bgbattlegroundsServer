compile:

mcs -r:System.Numerics.dll Client.cs Constants.cs GameLogic.cs Packet.cs Player.cs Program.cs Server.cs ServerHandle.cs ServerSend.cs ThreadManager.cs -out:Server.exe