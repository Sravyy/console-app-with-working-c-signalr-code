using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class testSignalR
    {
        public async void TestSignalR()
        {
            var hubConnection = new HubConnection("http://app.crelatedev.com");
            var smsHubProxy = hubConnection.CreateHubProxy("SmsHub");

            smsHubProxy.On<int, int>("triggerEvent", (userId, numberId) =>
            {
                //triggerEvent has been called from server
            });

            try
            {
                await hubConnection.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            //Add a button and to this event, write an Invoke method like below

            smsHubProxy.Invoke<>("TriggerEvent", hubConnection.userId, hubConnection.numberId)
        }   


    }
}


public void Init()
{
    IHubProxy _hub;

    var connection = new HubConnection("http://192.168.43.116:8088/");
    _hub = connection.CreateHubProxy("ChatHub");
    connection.Start().Wait();

    _hub.On<string, string>("MessageReceived", (x, y) =>
    {
        x += y;
        signalRMess = x;
        MessagingCenter.Send<InvokeSignalR, string>(this, "SignalR", signalRMess);
    });


}