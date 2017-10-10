import { Component, OnInit } from '@angular/core';

import { ProductService, AlertService } from '../services/';
import { Product } from '../models/product';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.css']
})
export class HomepageComponent implements OnInit {

  products: Product[];
  constructor(private productService: ProductService, private alertService: AlertService) {}

  ngOnInit() {
    this.productService.getAll()
      .subscribe(data => {
        this.products = data;
      }, error => {
        this.alertService.error(error);
      });
  }
}
