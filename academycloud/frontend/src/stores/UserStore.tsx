import { useState, useCallback } from "react";
import { Scope } from "src/models/account";

const STORAGE_KEY = "User";

interface User {
  username: string;
  token: string;
  scope: Scope;
}

export function getUserInfoInStorage(): User | null {
  const data = localStorage.getItem(STORAGE_KEY);
  if (data) {
    return JSON.parse(data) as User;
  } else {
    return null;
  }
}

export function UserStore() {
  const [user, setUser] = useState(getUserInfoInStorage);
  const [loggedIn, setLoggedIn] = useState(!!user);

  const logout = useCallback(() => {
    localStorage.removeItem(STORAGE_KEY);
    setLoggedIn(false);
    setUser(null);
  }, []);

  const login = useCallback((user: User, remember: boolean) => {
    setLoggedIn(true);
    setUser(user);
    if (remember) {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(user));
    }
  }, []);

  return { loggedIn, user, logout, login };
}
