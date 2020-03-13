export interface User {
  id: string;
  name: string;
}

export interface UserForSystem extends User {
  active: boolean;
}
