import { useState, useCallback } from "react";

const STORAGE_KEY = "User"

interface User {
  username: string;
  token: string;
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
  const [loggedIn, setLoggedIn] = useState(false);
  const [user, setUser] = useState(getUserInfoInStorage);

  const logout = useCallback(() => {
    localStorage.removeItem(STORAGE_KEY);
    setLoggedIn(false);
    setUser(null);
  }, []);

  const login = useCallback((username: string, token: string) => {
    const user = { username, token };
    setLoggedIn(true);
    setUser(user);
  }, []);

  return { loggedIn, user, logout, login };
}
