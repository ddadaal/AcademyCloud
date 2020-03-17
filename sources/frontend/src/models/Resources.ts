export interface Resources {
  cpu: number;
  memory: number;
  storage: number;
}

export function resourcesString({ cpu, memory, storage }: Resources) {
  return `${cpu} | ${memory} GB | ${storage} GB`;
}

export const ZeroResources = {
  cpu: 0,
  memory: 0,
  storage: 0,
};

export function minus(op1: Resources, op2: Resources) {
  return { cpu: op1.cpu - op2.cpu, memory: op1.memory - op2.memory, storage: op1.storage - op2.storage };
}
