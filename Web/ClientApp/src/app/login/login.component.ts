import { HttpClient, HttpErrorResponse, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { Location } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, NgForm, Validators, FormsModule, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { NavMenuService } from '../nav-menu/nav-menu.service';
import { LoginRequest } from '../_requests/login-request';
import { ErrorResponse } from '../_responses/error-response';
import { TokenResponse } from '../_responses/token-response';
import { TokenService } from '../_services/token.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  email: FormControl;
  password: FormControl;

  invalidLogin: boolean = false;
  errorMessage: string | undefined = '';

  constructor(private router: Router, private http: HttpClient, public nav: NavMenuService, private location: Location, private tokenService: TokenService) {

    this.email = new FormControl('', [
      Validators.required,
      Validators.pattern("[^ @]*@[^ @]*")
    ]);
    this.password = new FormControl('', [
      Validators.required,
      Validators.minLength(8)
    ]);
    this.loginForm = new FormGroup({
      email: this.email,
      password: this.password
    });
  }

  ngOnInit(): void {
    this.location.subscribe((value) => {
      this.nav.hide();
    });
  }

  validate(): void {

  }

  login = (form: FormGroup) => {
    if (form.valid) {
      let credentials: LoginRequest = {
        email: this.email.value,
        password: this.password.value
      }
      this.tokenService.login(credentials)
        .subscribe({
          next: (response: TokenResponse) => {
            console.debug(`logged in successfully ${response}`);
            this.invalidLogin = false;
            this.router.navigate(["/"]);
            this.nav.show();
          },
          error: (response: HttpErrorResponse) => {
            this.invalidLogin = true;
            this.errorMessage = response.error.error;
          }
        })
    }
  }
}


