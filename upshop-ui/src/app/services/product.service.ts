import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

import { Product } from '../models/product';
import { RequestService} from './request.service';

@Injectable()
export class ProductService extends RequestService<Product> {

    constructor(http: Http) { super(http, 'product/'); }
}
