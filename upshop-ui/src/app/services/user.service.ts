import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

import { User } from '../models/user';
import { RequestService} from './request.service';

@Injectable()
export class UserService extends RequestService<User> {

    constructor(http: Http) { super(http, 'user/'); }
}
