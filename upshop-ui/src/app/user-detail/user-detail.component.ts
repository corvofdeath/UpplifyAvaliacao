import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { User } from '../models/user';
import { UserService } from '../services/user.service';
import { AlertService } from '../services/alert.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {

  model: any = {};
  loading = false;

  constructor(private userService: UserService, private alertService: AlertService, private router: Router) { }

  ngOnInit() {
    this.model = JSON.parse(localStorage.getItem('currentUser')).user;
    this.model.password = '';
  }

  update() {
    // get the last user update from database because we update by replacement
    let latestUser: User;

    this.userService.getById(this.model.id)
      .subscribe(data => {
        latestUser = data;

        latestUser.firstName = this.model.firstName;
        latestUser.lastName = this.model.lastName;

        if (this.model.password !== '') {
          latestUser.password = this.model.password;
        } else {
          latestUser.password = '';
        }

        latestUser.email = this.model.email;
        latestUser.phone = this.model.phone;

        this.userService.update(latestUser)
          .subscribe(dataUpdate => {
            this.router.navigate(['/']);
          }, error => {
            this.alertService.error(error);
          });
      }, error => {
        this.alertService.error(error);
      });
  }

}
