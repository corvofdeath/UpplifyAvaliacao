import { Injectable } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/throw';

import { ErrorMessage } from '../models/errorMessage';
import { RequestService } from './request.service';

@Injectable()
export class AuthenticationService extends RequestService<any> {

    constructor(http: Http) { super(http, 'user/login'); }

    login(email: string, password: string) {

      const headers = new Headers({ 'Access-Control-Allow-Origin': '*', 'Content-Type': 'application/json' });
      const requestOption = new RequestOptions({ headers: headers });

      return this.http.post(this.baseUrl, JSON.stringify({ email: email, password: password }), requestOption)
          .map((response: Response) => {
              // login successful if there's a jwt token in the response
              const user = response.json();
              if (user && user.token) {
                  // store user details and jwt token in local storage to keep user logged in between page refreshes
                  localStorage.setItem('currentUser', JSON.stringify(user));
              }
          }).catch(this.handleError);
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
    }
}
