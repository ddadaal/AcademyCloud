export function flatten<T>(nestedArray: T[][]) {
  return nestedArray.reduce((prev, curr) => [...prev, ...curr], []);
}

export function arrayContainsElement<T>(array: T[] | null | undefined) {
  return !!array && array.length > 0;
}

export function removeFalsy<T>(array: (T | null | undefined)[]): T[] {
  return array.filter(x => !!x) as T[];
}
