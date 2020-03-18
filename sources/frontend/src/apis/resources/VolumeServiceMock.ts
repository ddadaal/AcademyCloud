import { VolumeService } from './VolumeService';
import { Volume } from "src/models/Volume";

export class VolumeServiceMock extends VolumeService {
  async getVolumes(): Promise<{ volumes: Volume[] }> {
    return {
      volumes: [
        {
          "id": "b937cd1b-1dea-4440-b072-83d0d011d202",
          "size": 1,
          "createTime": "2020-03-17T15:58:58.000000",
          "attachedToInstanceId": "6bc2a6f8-6cbb-4996-9d20-58c68a27776a",
          "attachedToInstanceName": "test",
          "attachedToDevice": "/dev/vda"
        }
      ]
    }
  }
}
