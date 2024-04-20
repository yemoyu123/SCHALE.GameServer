using SCHALE.Common.NetworkProtocol;
using Serilog;
using System.Reflection;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal class ProtocolHandlerAttribute : Attribute
    {
        public Protocol Protocol { get; }

        public ProtocolHandlerAttribute(Protocol protocol)
        {
            Protocol = protocol;
        }
    }

    public interface IProtocolHandlerFactory
    {
        public MethodInfo? GetProtocolHandler(Protocol protocol);
        public Type? GetRequestPacketTypeByProtocol(Protocol protocol);
    }

    public class ProtocolHandlerFactory : IProtocolHandlerFactory
    {
        private readonly Dictionary<Protocol, MethodInfo> handlers = [];
        private readonly Dictionary<Protocol, Type> requestPacketTypes = [];

        public ProtocolHandlerFactory()
        {
            foreach (var method in Assembly.GetExecutingAssembly().GetTypes().SelectMany(x => x.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Where(x => x.GetCustomAttribute<ProtocolHandlerAttribute>() is not null)))
            {
                var attr = method.GetCustomAttribute<ProtocolHandlerAttribute>()!;
                if (handlers.ContainsKey(attr.Protocol))
                    continue;

                handlers.Add(attr.Protocol, method);
                Log.Debug($"Loaded {method.Name} for {attr.Protocol}");
            }

            foreach (var requestPacketType in Assembly.GetAssembly(typeof(RequestPacket))!.GetTypes().Where(x => x.IsAssignableTo(typeof(RequestPacket))))
            {
                if (requestPacketType == typeof(RequestPacket))
                    continue;

                var obj = Activator.CreateInstance(requestPacketType);
                var protocol = requestPacketType.GetProperty("Protocol")!.GetValue(obj);

                if (!requestPacketTypes.ContainsKey((Protocol)protocol!))
                {
                    requestPacketTypes.Add((Protocol)protocol!, requestPacketType);
                }
            }
        }

        public MethodInfo? GetProtocolHandler(Protocol protocol)
        {
            handlers.TryGetValue(protocol, out var handler);

            return handler;
        }

        public Type? GetRequestPacketTypeByProtocol(Protocol protocol)
        {
            requestPacketTypes.TryGetValue(protocol, out var type);

            return type;
        }
    }

    internal static class ServiceExtensions
    {
        public static void AddProtocolHandlerFactory(this IServiceCollection services)
        {
            services.AddSingleton<IProtocolHandlerFactory, ProtocolHandlerFactory>();
        }
    }
}
