default:

gen-grpc:
	protoc -I . --csharp_out=./Assets/Scripts/Utils --grpc_out=./Assets/Scripts/Utils grpc/v1/himo/proto/* --plugin=protoc-gen-grpc=../Grpc.Tools.2.35.0/tools/macosx_x64/grpc_csharp_plugin
