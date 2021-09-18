// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: grpc/v1/himo/proto/user.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Himo.V1 {
  public static partial class UserManager
  {
    static readonly string __ServiceName = "himo.v1.UserManager";

    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    static readonly grpc::Marshaller<global::Himo.V1.SignUpRequest> __Marshaller_himo_v1_SignUpRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Himo.V1.SignUpRequest.Parser));
    static readonly grpc::Marshaller<global::Himo.V1.SignUpResponse> __Marshaller_himo_v1_SignUpResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Himo.V1.SignUpResponse.Parser));
    static readonly grpc::Marshaller<global::Himo.V1.UpdateUserNameRequest> __Marshaller_himo_v1_UpdateUserNameRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Himo.V1.UpdateUserNameRequest.Parser));
    static readonly grpc::Marshaller<global::Himo.V1.UpdateUserNameResponse> __Marshaller_himo_v1_UpdateUserNameResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Himo.V1.UpdateUserNameResponse.Parser));

    static readonly grpc::Method<global::Himo.V1.SignUpRequest, global::Himo.V1.SignUpResponse> __Method_SignUp = new grpc::Method<global::Himo.V1.SignUpRequest, global::Himo.V1.SignUpResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SignUp",
        __Marshaller_himo_v1_SignUpRequest,
        __Marshaller_himo_v1_SignUpResponse);

    static readonly grpc::Method<global::Himo.V1.UpdateUserNameRequest, global::Himo.V1.UpdateUserNameResponse> __Method_UpdateUserName = new grpc::Method<global::Himo.V1.UpdateUserNameRequest, global::Himo.V1.UpdateUserNameResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "UpdateUserName",
        __Marshaller_himo_v1_UpdateUserNameRequest,
        __Marshaller_himo_v1_UpdateUserNameResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Himo.V1.UserReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of UserManager</summary>
    [grpc::BindServiceMethod(typeof(UserManager), "BindService")]
    public abstract partial class UserManagerBase
    {
      public virtual global::System.Threading.Tasks.Task<global::Himo.V1.SignUpResponse> SignUp(global::Himo.V1.SignUpRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Himo.V1.UpdateUserNameResponse> UpdateUserName(global::Himo.V1.UpdateUserNameRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for UserManager</summary>
    public partial class UserManagerClient : grpc::ClientBase<UserManagerClient>
    {
      /// <summary>Creates a new client for UserManager</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public UserManagerClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for UserManager that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public UserManagerClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected UserManagerClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected UserManagerClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::Himo.V1.SignUpResponse SignUp(global::Himo.V1.SignUpRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SignUp(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Himo.V1.SignUpResponse SignUp(global::Himo.V1.SignUpRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_SignUp, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Himo.V1.SignUpResponse> SignUpAsync(global::Himo.V1.SignUpRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SignUpAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Himo.V1.SignUpResponse> SignUpAsync(global::Himo.V1.SignUpRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_SignUp, null, options, request);
      }
      public virtual global::Himo.V1.UpdateUserNameResponse UpdateUserName(global::Himo.V1.UpdateUserNameRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return UpdateUserName(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Himo.V1.UpdateUserNameResponse UpdateUserName(global::Himo.V1.UpdateUserNameRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_UpdateUserName, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Himo.V1.UpdateUserNameResponse> UpdateUserNameAsync(global::Himo.V1.UpdateUserNameRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return UpdateUserNameAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Himo.V1.UpdateUserNameResponse> UpdateUserNameAsync(global::Himo.V1.UpdateUserNameRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_UpdateUserName, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override UserManagerClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new UserManagerClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(UserManagerBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_SignUp, serviceImpl.SignUp)
          .AddMethod(__Method_UpdateUserName, serviceImpl.UpdateUserName).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, UserManagerBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_SignUp, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Himo.V1.SignUpRequest, global::Himo.V1.SignUpResponse>(serviceImpl.SignUp));
      serviceBinder.AddMethod(__Method_UpdateUserName, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Himo.V1.UpdateUserNameRequest, global::Himo.V1.UpdateUserNameResponse>(serviceImpl.UpdateUserName));
    }

  }
}
#endregion
