export interface Volume {
  id: string;
  size: number;
  createTime: string;
  attachedToInstanceId: string;
  attachedToInstanceName: string;
  attachedToDevice: string;
}
