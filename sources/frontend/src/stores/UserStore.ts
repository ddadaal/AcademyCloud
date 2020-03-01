import { useState, useCallback } from "react";
import { Scope } from "src/models/Scope";
import { getApiService } from "src/apis";
import { AccountService } from "src/apis/account/AccountService";

const STORAGE_KEY = "User";


interface User {
  username: string;
  scope: Scope;
  token: string;
  remember: boolean;
}

const accountService = getApiService(AccountService);

export function getUserInfoInStorage(): User | null {
  const data = localStorage.getItem(STORAGE_KEY);
  if (data) {
    const user =  JSON.parse(data) as User;
    accountService.setToken(user.token);
    return user;
  } else {
    return null;
  }
}

function saveUserInfo(user: User) {
  localStorage.setItem(STORAGE_KEY, JSON.stringify(user));
}

export function UserStore() {
  const [user, setUser] = useState(getUserInfoInStorage);

  const loggedIn = !!user;

  const logout = useCallback(() => {
    localStorage.removeItem(STORAGE_KEY);
    setUser(null);
  }, []);

  const login = useCallback((user: User) => {
    setUser(user);
    if (user.remember) {
      saveUserInfo(user);
    }
  }, []);

  return { loggedIn, user, logout, login };
}
