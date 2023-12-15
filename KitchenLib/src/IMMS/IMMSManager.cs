using Kitchen;
using KitchenMods;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KitchenLib.IMMS
{
	/// <summary>
	/// Inter-mod messaging system. Register listeners with Register(). Send messages with SendMessage().
	/// </summary>
	public class IMMSManager : GenericSystemBase, IModSystem
	{
		/// <summary>
		/// Delegate representing a message listener. Return null if this is only a consumer.
		/// </summary>
		/// <param name="key">The key of the message sent on the channel, representing the action that the recieving mod should take. May be null.</param>
		/// <param name="ctx">The context of the message being sent, such as the channel and the source/target clients.</param>
		/// <param name="args">Arguments of the message.</param>
		/// <returns>The result of handling the message. May be null.</returns>
		public delegate object MessageHandler(string key, IMMSContext ctx, object[] args);

		private static readonly Dictionary<string, MessageHandler> Handlers = new();
		private static MessageHandler GeneralHandlers;

		internal static Queue<IMMSNetworkMessage> HostboundMessages = new();
		internal static Queue<IMMSNetworkMessage> ClientboundMessages = new();

		private static int _nextId = 0;
		private static int NextId => ++_nextId;

		/// <summary>
		/// Send a message on the specified channel.
		/// </summary>
		/// <param name="channel">The channel to send the message on.</param>
		/// <param name="key">The key of the message, representing the action that the recieving mod should take.</param>
		/// <param name="args">Arguments of the message.</param>
		/// <returns>The result from the handler which handled the message. Will be null if there are no listeners on that channel.</returns>
		public static object SendLocalMessage(string channel, string key, params object[] args)
		{
			var ctx = new IMMSContext
			{
				Id = -1,
				Timestamp = DateTime.UtcNow.Ticks,
				Channel = channel,
				Type = IMMSMessageType.Internal,
				Source = -1,
				Target = -1
			};

			return HandleMessage(channel, key, ctx, args);
		}

		/// <summary>
		/// Send a message on the specified channel.
		/// </summary>
		/// <param name="channel">The channel to send the message on.</param>
		/// <param name="key">The key of the message, representing the action that the recieving mod should take.</param>
		/// <param name="target">The target client of the message. See IMMSTarget for specialty targets.</param>
		/// <param name="args">Arguments of the message.</param>
		/// <returns>The result from the handler which handled the message. Will be null if there are no listeners on that channel.</returns>
		public static void SendNetworkMessage(string channel, string key, int target, params object[] args)
		{
			// Check if we are client or server
			if ((Session.CurrentGameNetworkMode == GameNetworkMode.Host && target == IMMSTarget.Host) || (target == IMMSTarget.Me && IMMSTarget.Me != -1))
			{
				var ctx = new IMMSContext
				{
					Id = -1,
					Timestamp = DateTime.UtcNow.Ticks,
					Channel = channel,
					Type = IMMSMessageType.Internal,
					Source = IMMSTarget.Me,
					Target = target
				};

				HandleMessage(channel, key, ctx, args);
			}
			else if (Session.CurrentGameNetworkMode == GameNetworkMode.Host)
			{
				ClientboundMessages.Enqueue(new IMMSNetworkMessage
				{
					Id = NextId,
					Timestamp = DateTime.UtcNow.Ticks,
					Channel = channel,
					Key = key,
					Source = IMMSTarget.Me,
					Target = target,
					Arguments = args,
					Type = IMMSMessageType.HostToClient
				});
			}
			else
			{
				HostboundMessages.Enqueue(new IMMSNetworkMessage
				{
					Id = -1,
					Timestamp = DateTime.UtcNow.Ticks,
					Channel = channel,
					Key = key,
					Source = IMMSTarget.Me,
					Target = target,
					Arguments = args,
					Type = IMMSMessageType.ClientToHost
				});
			}
		}

		/// <summary>
		/// Register a listener on a specific channel.
		/// </summary>
		/// <param name="channel">The channel to listen to.</param>
		/// <param name="listener">The listener.</param>
		public static void Register(string channel, MessageHandler listener)
		{
			if (Handlers.ContainsKey(channel))
			{
				Handlers[channel] += listener;
			}
			else
			{
				Handlers.Add(channel, listener);
			}
		}

		/// <summary>
		/// Register a listener to listen to all channels.
		/// </summary>
		/// <param name="listener">The listener.</param>
		public static void RegisterAll(MessageHandler listener)
		{
			GeneralHandlers += listener;
		}

		internal static object HandleMessage(string channel, string key, IMMSContext ctx, object[] args)
		{
			var listeners = GeneralHandlers.GetInvocationList();

			if (Handlers.ContainsKey(channel))
			{
				listeners = listeners.Concat(Handlers[channel].GetInvocationList()).ToArray();
			}

			object result = null;
			foreach (var listener in listeners)
			{
				try
				{
					result = listener.DynamicInvoke(key, ctx, args);
				}
				catch (Exception e)
				{
					Main.LogWarning($"IMMS Listener {listener.GetType().FullName} threw an exception while handling message {channel}:{key} with arguments [{string.Join(", ", args.Select(Convert.ToString))}]: {e.Message}");
					Main.LogWarning(e.StackTrace.Replace("\n", "\n" + $"[{Main.MOD_NAME}] "));
				}
			}

			return result;
		}

		internal static void HandleIncomingClientboundNetworkMessage(IMMSNetworkMessage message)
		{
			if (message.Target == IMMSTarget.Broadcast || message.Target == IMMSTarget.Me)
			{
				HandleMessage(message.Channel, message.Key, new IMMSContext
				{
					Id = message.Id,
					Timestamp = message.Timestamp,
					Channel = message.Channel,
					Source = message.Source,
					Target = message.Target,
					Type = message.Type
				}, message.Arguments);
			}
		}

		internal static void HandleIncomingHostboundNetworkMessage(IMMSNetworkMessage message)
		{
			if (message.Target == IMMSTarget.Host || message.Target == IMMSTarget.Me)
			{
				// If message intended for the host, just handle it directly
				HandleMessage(message.Channel, message.Key, new IMMSContext
				{
					Id = message.Id,
					Timestamp = message.Timestamp,
					Channel = message.Channel,
					Source = message.Source,
					Target = message.Target,
					Type = IMMSMessageType.ClientToHost
				}, message.Arguments);
			}
			else
			{
				// Otherwise, pass it on to the clients
				message.Id = NextId;
				message.Type = IMMSMessageType.ClientToClient;
				ClientboundMessages.Enqueue(message);
			}
		}

		protected override void OnUpdate()
		{
			/*
			if (!HasSingleton<SIMMSManager>())
			{
				var entity = EntityManager.CreateEntity(typeof(CRequiresView), typeof(SIMMSManager), typeof(CPersistThroughSceneChanges));
				EntityManager.SetComponentData(entity, new CRequiresView
				{
					Type = IMMSView.ViewType,
					ViewMode = ViewMode.Screen // screen since this is purely a communication mechanism, not a world object
				});
			}
			*/
		}
	}
}
