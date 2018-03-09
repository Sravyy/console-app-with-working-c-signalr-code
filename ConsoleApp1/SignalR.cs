using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class SignalR : INotifyPropertyChanged
    {
        public HubConnection Connection;

        public event PropertyChangedEventHandler PropertyChanged;

        public SignalR()
        {
            var hubConnection = new HubConnection("http://www.contoso.com/");
            IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("SmsHub");

            stockTickerHubProxy.On<Stock>("UpdateStockPrice", stock => Console.WriteLine("Stock update for {0} new price {1}", stock.Symbol, stock.Price));
            ServicePointManager.DefaultConnectionLimit = 10;

            var contacts = stockTickerHubProxy.Invoke<IEnumerable<SmsContactState>>("Contacts", hubConnection.userId, hubConnection.numberId);
            foreach (SmsContactState smsState in contacts)
            {
                Console.WriteLine("contacts={0}", contacts);
            }


            var contacts = stockTickerHubProxy.Invoke<TriggerEvent>("triggerEvent", hubConnection.userId, hubConnection.numberId);
            hubConnection.TraceWriter.WriteLine()

            try
            {
                IEnumerable<Stock> stocks = await stockTickerHub.Invoke<IEnumerable<Stock>>("GetAllStocks");
                foreach (Stock stock in stocks)
                {
                    Console.WriteLine("Symbol: {0} price: {1}", stock.Symbol, stock.Price);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error invoking GetAllStocks: {0}", ex.Message);
            }
        }



            public Task Start()
            {
                return Connection.Start();
            }

            public bool IsConnectedOrConnecting
            {
                get
                {
                    return Connection.State != ConnectionState.Disconnected;
                }
            }

            public ConnectionState ConnectionState { get { return Connection.State; } }

            public static async Task<SignalR> CreateAndStart(string url)
            {
                var client = new SignalR();
                await client.Start();
                return client;
            }

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                var handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(propertyName));
            }

        private void TriggerEvent( SmsHubEventType type, Guid? userId, Guid? numberId,
            SmsContactState contactState = null,
            SmsMessage message = null,
            string excludeConnectionId = null)
        {
            Clients
                .Group($"SmsHub:Source:{userId}/{numberId}", excludeConnectionId)
                .triggerEvent(
                    new SmsHubEventPayload
                    {
                        Type = type,
                        TypeName = type.ToString(),
                        Message = message,
                        ContactState = contactState
                    });
        }
        //connection.Start().ContinueWith(task =>
        //{
        //    if (task.IsFaulted)
        //    {
        //        Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());
        //    }
        //    else
        //    {
        //        Console.WriteLine("Connected");

        //        myHub.On<string, string>("addMessage", (s1, s2) =>
        //        {
        //            Console.WriteLine(s1 + ": " + s2);
        //        });

        //        while (true)
        //        {
        //            string message = Console.ReadLine();

        //            if (string.IsNullOrEmpty(message))
        //            {
        //                break;
        //            }

        //            myHub.Invoke<string>("Send", name, message).ContinueWith(task1 =>
        //            {
        //                if (task1.IsFaulted)
        //                {
        //                    Console.WriteLine("There was an error calling send: {0}", task1.Exception.GetBaseException());
        //                }
        //                else
        //                {
        //                    Console.WriteLine(task1.Result);
        //                }
        //            });
        //        }
        //    }

        //}).Wait();

        //Console.Read();
        //connection.Stop();


    }
}
