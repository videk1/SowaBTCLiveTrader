using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SowaBTCLiveTrader.Models;
using SowaBTCLiveTrader.Services.OrderBookService;
using SowaBTCLiveTrader.WebSockets.BtcSignalRHub;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SowaBTCLiveTrader.WebSockets.BitstampWebSocket
{
    public class BitstampBackgroundWebSocket : BackgroundService
    {
        private readonly IOrderBookService _orderBookService;
        private readonly IHubContext<BtcHub> _hub;
        private readonly ILogger _logger;
        private static readonly string Connection = "wss://ws.bitstamp.net/";

        public BitstampBackgroundWebSocket(ILogger<BitstampBackgroundWebSocket> logger, IOrderBookService orderBookService, IHubContext<BtcHub> hub)
        {
            _logger = logger;
            _orderBookService = orderBookService;
            _hub = hub;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
                using (var socket = new ClientWebSocket())
                    try
                    {
                        await socket.ConnectAsync(new Uri(Connection), stoppingToken);
                        var data = JsonSerializer.Serialize(new BitstampConnectionClass(), new JsonSerializerOptions { PropertyNamingPolicy = null });
                        await Send(socket, data, stoppingToken);
                        await Receive(socket, stoppingToken);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR - {ex.Message}");
                    }
        }

        private async Task Send(ClientWebSocket socket, string data, CancellationToken stoppingToken) =>
            await socket.SendAsync(Encoding.UTF8.GetBytes(data), WebSocketMessageType.Text, true, stoppingToken);

        private async Task Receive(ClientWebSocket socket, CancellationToken stoppingToken)
        {
            var buffer = new ArraySegment<byte>(new byte[2048]);
            while (!stoppingToken.IsCancellationRequested)
            {
                WebSocketReceiveResult result;
                using (var ms = new MemoryStream())
                {
                    do
                    {
                        result = await socket.ReceiveAsync(buffer, stoppingToken);
                        ms.Write(buffer.Array, buffer.Offset, result.Count);
                    } while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Close)
                        break;

                    ms.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(ms, Encoding.UTF8))
                    {
                        var data = await reader.ReadToEndAsync();
                        var deserializedData = JsonSerializer.Deserialize<BitstampOrderBook>(data);
                        var orderBookDto = _orderBookService.CalculateAndReturnOrderBookData(deserializedData);

                        // Log some data
                        var logData = orderBookDto.ToString();
                        _logger.LogInformation(logData);

                        await _hub.Clients.All.SendAsync("sendbtcdata", JsonSerializer.Serialize(orderBookDto));
                    }
                }
            };
        }
    }
}
