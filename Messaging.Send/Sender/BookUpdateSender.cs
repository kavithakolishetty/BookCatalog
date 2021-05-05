﻿
using System;
using System.Text;
using BookCatalog.MicroService.Messaging.Send.Options;
using BookCatalog.MicroService.Models;
//using CustomerApi.Data.Entities;
//using CustomerApi.Messaging.Send.Options.v1;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace BookCatalog.MicroService.Messaging.Send.Sender
{
    public class BookUpdateSender: IBookUpdateSender

    {

        private readonly string _hostname;
            private readonly string _password;
            private readonly string _queueName;
            private readonly string _username;
            private IConnection _connection;

            public BookUpdateSender(IOptions<RabbitMqConfiguration> rabbitMqOptions)
            {
                _queueName = rabbitMqOptions.Value.QueueName;
                _hostname = rabbitMqOptions.Value.Hostname;
                _username = rabbitMqOptions.Value.UserName;
                _password = rabbitMqOptions.Value.Password;

                CreateConnection();
            }

            public void SendBook(Book book)
            {
                if (ConnectionExists())
                {
                    using (var channel = _connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                        var json = JsonConvert.SerializeObject(book);
                        var body = Encoding.UTF8.GetBytes(json);

                        channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
                    }
                }
            }

            private void CreateConnection()
            {
                try
                {
                    var factory = new ConnectionFactory
                    {
                        HostName = _hostname,
                        UserName = _username,
                        Password = _password
                    };
                    _connection = factory.CreateConnection();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not create connection: {ex.Message}");
                }
            }

            private bool ConnectionExists()
            {
                if (_connection != null)
                {
                    return true;
                }

                CreateConnection();

                return _connection != null;
            }
        }
    
}
