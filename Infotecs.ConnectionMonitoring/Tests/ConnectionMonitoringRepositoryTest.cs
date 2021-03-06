using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Dapper;
using Data;
using Data.Models;
using Data.Repositories;
using Data.UnitOfWork;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Npgsql;
using Xunit;

namespace Tests
{
    /// <summary>
    /// Test ConnectionMonitoringRepository.
    /// </summary>
    public class ConnectionMonitoringRepositoryTest : IAsyncLifetime
    {
        private readonly IConfiguration config;
        private readonly DapperUnitOfWork unitOfWork;
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionMonitoringRepositoryTest"/> class.
        /// </summary>
        public ConnectionMonitoringRepositoryTest()
        {
            config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            unitOfWork = new DapperUnitOfWork(config);
            connectionString = config.GetConnectionString("InfotecsMonitoring");
        }

        /// <inheritdoc />
        public Task InitializeAsync() => Task.CompletedTask;

        /// <inheritdoc />
        public Task DisposeAsync() => DeleteDbData();

        private async Task DeleteDbData()
        {
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                await connection.ExecuteAsync("DELETE FROM \"ConnectionEvent\"");
                await connection.ExecuteAsync("DELETE FROM \"ConnectionInfo\"");
            }
        }

        /// <summary>
        /// Checking if ConnectionInfo was created successfully.
        /// </summary>
        /// <param name="connectionInfo">Test connection info.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
        [Theory, AutoData]
        public async Task CreateConnectionInfoAsync_SuccessfulCreation(ConnectionInfoEntity connectionInfo)
        {
            // Act
            await unitOfWork.ConnectionMonitoringRepository.CreateConnectionInfoAsync(connectionInfo);

            ConnectionInfoEntity? result = await unitOfWork.ConnectionMonitoringRepository.GetConnectionInfoByIdAsync(connectionInfo.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(connectionInfo, options => options
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, new TimeSpan(1000)))
                .WhenTypeIs<DateTime>());
        }

        /// <summary>
        /// Checking if ConnectionInfo was updated successfully.
        /// </summary>
        /// <param name="baseConnectionInfo">Base object.</param>
        /// <param name="newConnectionInfo">Object with updated fields.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
        [Theory, AutoData]
        public async Task UpdateConnectionInfoAsync_SuccessfulUpdating(ConnectionInfoEntity baseConnectionInfo, ConnectionInfoEntity newConnectionInfo)
        {
            // Arrange
            await unitOfWork.ConnectionMonitoringRepository.CreateConnectionInfoAsync(baseConnectionInfo);

            newConnectionInfo.Id = baseConnectionInfo.Id;

            // Act
            await unitOfWork.ConnectionMonitoringRepository.UpdateConnectionInfoAsync(newConnectionInfo);

            ConnectionInfoEntity? result = await unitOfWork.ConnectionMonitoringRepository.GetConnectionInfoByIdAsync(newConnectionInfo.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(newConnectionInfo, options => options
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, new TimeSpan(1000)))
                .WhenTypeIs<DateTime>());
            result.Should().NotBeEquivalentTo(baseConnectionInfo, options => options
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, new TimeSpan(1000)))
                .WhenTypeIs<DateTime>());
        }

        /// <summary>
        /// Checking if ConnectionInfo were successfully received.
        /// </summary>
        /// <param name="connections">List of ConnectionInfo.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
        [Theory, AutoData]
        public async Task GetAllConnectionsInfoAsync_SuccessfulReturnAllConnectionInfo(ConnectionInfoEntity[] connections)
        {
            // Arrange
            foreach (ConnectionInfoEntity connection in connections)
            {
                await unitOfWork.ConnectionMonitoringRepository.CreateConnectionInfoAsync(connection);
            }

            // Act
            IEnumerable<ConnectionInfoEntity> result = await unitOfWork.ConnectionMonitoringRepository.GetAllConnectionsInfoAsync();

            // Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(connections.Length);
        }

        /// <summary>
        /// Checking if ConnectionInfo was deleted successfully.
        /// </summary>
        /// <param name="connectionInfo">Test connection info.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
        [Theory, AutoData]
        public async Task DeleteConnectionInfoAsync_SuccessfulDelete(ConnectionInfoEntity connectionInfo)
        {
            // Arrange
            await unitOfWork.ConnectionMonitoringRepository.CreateConnectionInfoAsync(connectionInfo);

            ConnectionInfoEntity? newConnectionInfo = await unitOfWork.ConnectionMonitoringRepository.GetConnectionInfoByIdAsync(connectionInfo.Id);

            // Act
            await unitOfWork.ConnectionMonitoringRepository.DeleteConnectionInfoAsync(connectionInfo.Id);

            ConnectionInfoEntity? result = await unitOfWork.ConnectionMonitoringRepository.GetConnectionInfoByIdAsync(connectionInfo.Id);

            // Assert
            newConnectionInfo.Should().NotBeNull();
            result.Should().BeNull();
        }

        /// <summary>
        /// Checking if ConnectionEvent was created successfully.
        /// </summary>
        /// <param name="connectionEvent">Test connection event.</param>
        /// <param name="connectionInfo">Test connection info.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
        [Theory, AutoData]
        public async Task CreateConnectionEventAsync_SuccessfulCreation(ConnectionEventEntity connectionEvent, ConnectionInfoEntity connectionInfo)
        {
            // Arrange
            await unitOfWork.ConnectionMonitoringRepository.CreateConnectionInfoAsync(connectionInfo);

            connectionEvent.ConnectionId = connectionInfo.Id;

            // Act
            await unitOfWork.ConnectionMonitoringRepository.CreateConnectionEventAsync(connectionEvent);

            connectionEvent.Id.Should().NotBeNull();

            ConnectionEventEntity? result = await unitOfWork.ConnectionMonitoringRepository.GetConnectionEventByIdAsync(connectionEvent.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(connectionEvent, options => options
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, new TimeSpan(1000)))
                .WhenTypeIs<DateTime>());
        }

        /// <summary>
        /// Checking if ConnectionEvent were successfully received.
        /// </summary>
        /// <param name="connectionEvents">List of ConnectionEvent.</param>
        /// <param name="connectionInfo">Test connection info.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
        [Theory, AutoData]
        public async Task GetEventsByConnectionIdAsync_SuccessfulReturnAllConnectionEvent(ConnectionEventEntity[] connectionEvents, ConnectionInfoEntity connectionInfo)
        {
            // Arrange
            await unitOfWork.ConnectionMonitoringRepository.CreateConnectionInfoAsync(connectionInfo);

            foreach (ConnectionEventEntity connectionEvent in connectionEvents)
            {
                connectionEvent.ConnectionId = connectionInfo.Id;
                await unitOfWork.ConnectionMonitoringRepository.CreateConnectionEventAsync(connectionEvent);
            }

            // Act
            IEnumerable<ConnectionEventEntity> result = await unitOfWork.ConnectionMonitoringRepository.GetEventsByConnectionIdAsync(connectionInfo.Id);

            // Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(connectionEvents.Length);
        }
    }
}
