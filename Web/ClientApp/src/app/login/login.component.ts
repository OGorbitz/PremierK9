import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { NavMenuService } from '../nav-menu/nav-menu.service';
import { LoginRequest } from '../_requests/login-request';
import { ErrorResponse } from '../_responses/error-response';
import { TokenResponse } from '../_responses/token-response';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  credentials: LoginRequest = { email: '', password: '' };
  invalidLogin: boolean = false;
  errorMessage: string = '';

  constructor(private router: Router, private http: HttpClient, public nav: NavMenuService) { }

  ngOnInit(): void {
    this.nav.hide();
  }

  login = (form: NgForm) => {
    if (form.valid) {
      this.http.post<TokenResponse>(environment.apiUrl + "auth/login", this.credentials, {
        headers: new HttpHeaders({"Content-Type": "application/json"})
      })
        .subscribe({
          next: (response: TokenResponse) => {
            const token = response.accessToken;
            localStorage.setItem("jwt", token);
            this.invalidLogin = false;
            this.router.navigate(["/"]);
            this.nav.show();
          },
          error: (response: ErrorResponse) => {
            this.invalidLogin = true;
            this.errorMessage = response.error;
          }
        })
    }
  }
}
