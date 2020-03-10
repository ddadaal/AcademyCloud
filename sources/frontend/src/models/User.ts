export interface User {
  id: string;
  username: string;
  name: string;
}

export interface UserForSystem extends User {
  active: boolean;
}
