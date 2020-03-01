import { Scope } from "src/models/Scope";
import { useState, useCallback } from "react";
import { getApiService } from "src/apis";
import { AccountService } from "src/apis/identity/AccountService";

const AVAILABLE_SCOPES_KEY = "available_scopes";

function read(): Scope[] {
  const data = localStorage.getItem(AVAILABLE_SCOPES_KEY);

  if (data) {
    return JSON.parse(data);
  } else {
    return [];
  }
}

function save(scopes: Scope[]) {
  localStorage.setItem(AVAILABLE_SCOPES_KEY, JSON.stringify(scopes));
}

const service = getApiService(AccountService);

export function AvailableScopesStore() {
  const [scopes, setScopes] = useState<Scope[]>(read);
  const [reloading, setReloading] = useState(false);


  const set = useCallback((scopes: Scope[], remember: boolean)=> {
    setScopes(scopes);
    if (remember) { save(scopes);}
  }, []);

  const updateScopes = useCallback(async () => {
    try {
      setReloading(true);
      const resp = await service.getScopes();
      setScopes(resp.scopes);
    } catch (e) {
      console.log(e);
    } finally {
      setReloading(false)
    }
  }, []);

  const logout = useCallback(() => {
    localStorage.removeItem(AVAILABLE_SCOPES_KEY);
    setScopes([]);
  }, []);

  return { scopes, setScopes: set, updateScopes, reloading, logout };

}
