import { Component, OnInit } from '@angular/core';

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

  constructor(private userService: UserService, private alertService: AlertService) { }

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
        // TODO: back end service not changes the hash
        // latestUser.password = this.model.password;
        latestUser.email = this.model.email;
        latestUser.phone = this.model.phone;

        this.userService.update(latestUser)
          .subscribe(dataUpdate => {
            this.alertService.success('Atualizado com sucesso');
          }, error => {
            this.alertService.error(error);
          });
      }, error => {
        this.alertService.error(error);
      });
  }

}
