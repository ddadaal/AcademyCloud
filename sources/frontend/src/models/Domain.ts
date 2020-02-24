import { Resources } from "src/models/Resources";

export interface Domain {
  id: string;
  name: string;
  admins: { id: string; name: string }[];
  resources: Resources;
}
