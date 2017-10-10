import { Historic } from './historic';

export class User {
  id: string;
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phone: string;
  historic: Historic[];
}
