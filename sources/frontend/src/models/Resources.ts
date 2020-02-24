export interface Resources {
  cpu: number;
  memory: number;
  storage: number;
}

export function resourcesString({cpu,memory, storage}: Resources) {
  return `${cpu} | ${memory} MB | ${storage} GB`;
}
