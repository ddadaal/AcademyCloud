# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: volume.proto

from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from google.protobuf import reflection as _reflection
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()




DESCRIPTOR = _descriptor.FileDescriptor(
  name='volume.proto',
  package='volume',
  syntax='proto3',
  serialized_options=b'\252\002-AcademyCloud.ResourceManagement.Protos.Volume',
  serialized_pb=b'\n\x0cvolume.proto\x12\x06volume\"\x13\n\x11GetVolumesRequest\"\x8e\x01\n\x06Volume\x12\n\n\x02id\x18\x01 \x01(\t\x12\x0c\n\x04size\x18\x02 \x01(\x05\x12\x12\n\ncreateTime\x18\x03 \x01(\t\x12\x1c\n\x14\x61ttachedToInstanceId\x18\x04 \x01(\t\x12\x1e\n\x16\x61ttachedToInstanceName\x18\x05 \x01(\t\x12\x18\n\x10\x61ttachedToDevice\x18\x06 \x01(\t\"5\n\x12GetVolumesResponse\x12\x1f\n\x07volumes\x18\x01 \x03(\x0b\x32\x0e.volume.Volume2T\n\rVolumeService\x12\x43\n\nGetVolumes\x12\x19.volume.GetVolumesRequest\x1a\x1a.volume.GetVolumesResponseB0\xaa\x02-AcademyCloud.ResourceManagement.Protos.Volumeb\x06proto3'
)




_GETVOLUMESREQUEST = _descriptor.Descriptor(
  name='GetVolumesRequest',
  full_name='volume.GetVolumesRequest',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=24,
  serialized_end=43,
)


_VOLUME = _descriptor.Descriptor(
  name='Volume',
  full_name='volume.Volume',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='id', full_name='volume.Volume.id', index=0,
      number=1, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=b"".decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='size', full_name='volume.Volume.size', index=1,
      number=2, type=5, cpp_type=1, label=1,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='createTime', full_name='volume.Volume.createTime', index=2,
      number=3, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=b"".decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='attachedToInstanceId', full_name='volume.Volume.attachedToInstanceId', index=3,
      number=4, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=b"".decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='attachedToInstanceName', full_name='volume.Volume.attachedToInstanceName', index=4,
      number=5, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=b"".decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='attachedToDevice', full_name='volume.Volume.attachedToDevice', index=5,
      number=6, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=b"".decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=46,
  serialized_end=188,
)


_GETVOLUMESRESPONSE = _descriptor.Descriptor(
  name='GetVolumesResponse',
  full_name='volume.GetVolumesResponse',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='volumes', full_name='volume.GetVolumesResponse.volumes', index=0,
      number=1, type=11, cpp_type=10, label=3,
      has_default_value=False, default_value=[],
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=190,
  serialized_end=243,
)

_GETVOLUMESRESPONSE.fields_by_name['volumes'].message_type = _VOLUME
DESCRIPTOR.message_types_by_name['GetVolumesRequest'] = _GETVOLUMESREQUEST
DESCRIPTOR.message_types_by_name['Volume'] = _VOLUME
DESCRIPTOR.message_types_by_name['GetVolumesResponse'] = _GETVOLUMESRESPONSE
_sym_db.RegisterFileDescriptor(DESCRIPTOR)

GetVolumesRequest = _reflection.GeneratedProtocolMessageType('GetVolumesRequest', (_message.Message,), {
  'DESCRIPTOR' : _GETVOLUMESREQUEST,
  '__module__' : 'volume_pb2'
  # @@protoc_insertion_point(class_scope:volume.GetVolumesRequest)
  })
_sym_db.RegisterMessage(GetVolumesRequest)

Volume = _reflection.GeneratedProtocolMessageType('Volume', (_message.Message,), {
  'DESCRIPTOR' : _VOLUME,
  '__module__' : 'volume_pb2'
  # @@protoc_insertion_point(class_scope:volume.Volume)
  })
_sym_db.RegisterMessage(Volume)

GetVolumesResponse = _reflection.GeneratedProtocolMessageType('GetVolumesResponse', (_message.Message,), {
  'DESCRIPTOR' : _GETVOLUMESRESPONSE,
  '__module__' : 'volume_pb2'
  # @@protoc_insertion_point(class_scope:volume.GetVolumesResponse)
  })
_sym_db.RegisterMessage(GetVolumesResponse)


DESCRIPTOR._options = None

_VOLUMESERVICE = _descriptor.ServiceDescriptor(
  name='VolumeService',
  full_name='volume.VolumeService',
  file=DESCRIPTOR,
  index=0,
  serialized_options=None,
  serialized_start=245,
  serialized_end=329,
  methods=[
  _descriptor.MethodDescriptor(
    name='GetVolumes',
    full_name='volume.VolumeService.GetVolumes',
    index=0,
    containing_service=None,
    input_type=_GETVOLUMESREQUEST,
    output_type=_GETVOLUMESRESPONSE,
    serialized_options=None,
  ),
])
_sym_db.RegisterServiceDescriptor(_VOLUMESERVICE)

DESCRIPTOR.services_by_name['VolumeService'] = _VOLUMESERVICE

# @@protoc_insertion_point(module_scope)
