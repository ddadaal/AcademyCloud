import { useState, useCallback } from "react";

export function useRefreshToken() {
  const [refreshToken, setRefreshToken] = useState(false);

  const updateRefreshToken = useCallback(() => setRefreshToken((original) => !original), []);

  return [refreshToken, updateRefreshToken] as const;
}

export interface Refreshable {
  refreshToken: boolean;
}
