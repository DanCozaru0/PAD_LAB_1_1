using Broker;
using Common;

Console.WriteLine("Broker");

var socket = new BrokerSocket();
socket.Start(Settings.BROKER_IP, Settings.BROKER_PORT);

var worker = new MessageWorker();
Task.Factory.StartNew(worker.DoSendMessageWork, TaskCreationOptions.LongRunning);

Console.ReadLine();