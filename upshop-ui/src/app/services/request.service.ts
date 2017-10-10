import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/observable/throw';

@Injectable()
export class RequestService<T> {

    protected baseUrl = 'http://localhost:58994/api/';

    constructor(protected http: Http, private endpoint: string) {
      this.baseUrl = this.baseUrl + endpoint;
     }

    getAll(): Observable<T[]> {
        return this.http.get(this.baseUrl, this.jwt())
          .map((response: Response) => <T[]>response.json())
          .do(data => console.log('All: ' + JSON.stringify(data)))
          .catch(this.handleError);
    }

    getById(id: string): Observable<T> {
        return this.http.get(this.baseUrl + id, this.jwt())
          .map((response: Response) => <T>response.json())
          .do(data => console.log('All: ' + JSON.stringify(data)))
          .catch(this.handleError);
    }

    create(model: T): Observable<T> {
        return this.http.post(this.baseUrl + '/create', model)
          .map((response: Response) => <T>response.json())
          .do(data => console.log('All: ' + JSON.stringify(data)))
          .catch(this.handleError);
    }

    update(model: T): Observable<T> {
        return this.http.put(this.baseUrl + 'update', model, this.jwt())
          .catch(this.handleError);
    }

    delete(id: string): Observable<T> {
        return this.http.delete(this.baseUrl + 'remove' + + id, this.jwt())
          .catch(this.handleError);
    }

    // private helper methods

    private jwt() {
        // create authorization header with jwt token
        const currentUser = JSON.parse(localStorage.getItem('currentUser'));
        if (currentUser && currentUser.token) {
            const headers = new Headers({ 'Authorization': 'Bearer ' + currentUser.token, 'Access-Control-Allow-Origin': '*' });
            return new RequestOptions({ headers: headers });
        }
    }

    protected handleError(error: Response) {
      console.error(error);
      return Observable.throw(error.json() || 'Server error');
    }
}
