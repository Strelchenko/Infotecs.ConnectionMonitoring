using System;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Core.Models;
using Data.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using Xunit;

namespace Tests;

/// <summary>
/// Test for RabbitMq.
/// </summary>
public class RabbitMqTest
{
    private readonly Mock<IConfiguration> configuration;
    private readonly RabbitMqProducerMock rabbitMqProducerMock;

    /// <summary>
    /// Mock RabbitMqProducer.
    /// </summary>
    public class RabbitMqProducerMock : RabbitMqProducer
    {
        public ReadOnlyMemory<byte> Body;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqProducerMock"/> class.
        /// </summary>
        /// <param name="configuration">IConfiguration.</param>
        public RabbitMqProducerMock(IConfiguration configuration)
            : base(configuration)
        {
        }

        /// <summary>
        /// Creating connection and channel.
        /// </summary>
        /// <param name="configuration">IConfiguration.</param>
        /// <returns></returns>
        protected override (IConnection, IModel) CreateConnection(IConfiguration configuration)
        {
            var connectionMock = new Mock<IConnection>();
            var channelMock = new Mock<IModel>();

            var basicProperties = new Mock<IBasicProperties>();

            channelMock.Setup(x => x.CreateBasicProperties()).Returns(() => basicProperties.Object);

            return (connectionMock.Object, channelMock.Object);
        }

        /// <summary>
        /// Publish message to Rabbit.
        /// </summary>
        /// <param name="exchange">Exchange name.</param>
        /// <param name="routingKey">Routing key</param>
        /// <param name="props">Properties.</param>
        /// <param name="body">Body.</param>
        protected override void BasicPublish(string exchange, string routingKey, IBasicProperties props, ReadOnlyMemory<byte> body) => Body = body;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RabbitMqTest"/> class.
    /// </summary>
    public RabbitMqTest()
    {
        configuration = new Mock<IConfiguration>();

        rabbitMqProducerMock = new RabbitMqProducerMock(configuration.Object);
    }

    /// <summary>
    /// Checking the message validation.
    /// </summary>
    /// <param name="connectionEvent">ConnectionEvent</param>
    [Theory, AutoData]
    public void CheckRabbitMqMessage_SuccessValidation(ConnectionEvent connectionEvent)
    {
        // Arrange
        string jsonData = "{\"eventId\":\"" + connectionEvent.Id +
            "\",\"name\":\"" + connectionEvent.Name +
            "\",\"nodeId\":\"" + connectionEvent.ConnectionId +
            "\",\"date\":\"" + connectionEvent.EventTime.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFFK") + "\"}";

        // Act
        rabbitMqProducerMock.SendSuccessEventMessage(connectionEvent);

        string result = Encoding.UTF8.GetString(rabbitMqProducerMock.Body.ToArray());

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(jsonData);
    }
}
