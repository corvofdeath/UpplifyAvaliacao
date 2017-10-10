import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { User } from '../models/user';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-head-shop',
  templateUrl: './head-shop.component.html',
  styleUrls: ['./head-shop.component.css']
})
export class HeadShopComponent implements OnInit {

  user: User;
  userName: string;

constructor(private authenticationService: AuthenticationService, private route: ActivatedRoute, private router: Router) {
    this.user = JSON.parse(localStorage.getItem('currentUser')).user;

    this.userName = this.user.firstName + ' ' + this.user.lastName;
  }

  ngOnInit() {
  }

  logout() {
    this.authenticationService.logout();

    // get return url from route parameters or default to '/'
    this.router.navigate(['/login']);
  }

}
