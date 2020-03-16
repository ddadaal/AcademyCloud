export interface Flavor {
  id: string;
  name: string;
  cpu: number;
  memory: number;
  //https://docs.openstack.org/nova/latest/user/flavors.html
  rootDisk: number;
}


export function flavorString({ name, cpu, memory, rootDisk }: Flavor) {
  return `${name} | ${cpu} CPU | ${memory} MB | ${rootDisk} GB`;
}

export enum InstanceStatus {
  Shutoff = "Shutoff",
}

export interface Instance {
  id: string;
  name: string;
  flavor: Flavor;
  status: InstanceStatus;
  ip: string;
  imageName: string;
  createTime: string;
  totalStartupHours: number;

}
