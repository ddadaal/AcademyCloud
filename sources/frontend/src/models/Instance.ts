export interface Flavor {
  name: string;
  cpu: number;
  memory: number;
  //https://docs.openstack.org/nova/latest/user/flavors.html
  rootDisk: number;
}


export function flavorString({ name, cpu, memory, rootDisk }: Flavor) {
  return `${name} | ${cpu} CPU | ${memory} MB | ${rootDisk} GB`;
}

export type InstanceStatus = "SHUTOFF" | "ACTIVE" | "BUILD";

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

export interface Image {
  id: string;
  name: string;
  minDisk: number;
}
