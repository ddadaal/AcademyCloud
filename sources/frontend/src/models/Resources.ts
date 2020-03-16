export interface Resources {
  cpu: number;
  memory: number;
  storage: number;
}

export function resourcesString({cpu,memory, storage}: Resources) {
  return `${cpu} | ${memory} GB | ${storage} GB`;
}

export const ZeroResources = {
  cpu: 0,
  memory: 0,
  storage: 0,
};
