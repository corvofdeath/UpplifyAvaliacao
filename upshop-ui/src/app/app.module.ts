import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule} from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { routing } from './app.routing';

import { HeadShopComponent } from './head-shop/head-shop.component';
import { AuthGuard } from './guard/auth.guard';
import { AlertService, AuthenticationService, UserService, ProductService } from './services/index';
import { HomepageComponent } from './homepage/homepage.component';
import { FooterComponent } from './footer/footer.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AlertComponent } from './alert/alert.component';
import { UserDetailComponent } from './user-detail/user-detail.component';

@NgModule({
  declarations: [
    AppComponent,
    HeadShopComponent,
    HomepageComponent,
    FooterComponent,
    LoginComponent,
    RegisterComponent,
    AlertComponent,
    UserDetailComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    routing
  ],
  providers: [
    AuthGuard,
    AlertService,
    AuthenticationService,
    UserService,
    ProductService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
