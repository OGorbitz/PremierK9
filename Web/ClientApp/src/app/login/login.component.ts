import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { NavMenuService } from '../nav-menu/nav-menu.service';
import { LoginModel } from '../_interfaces/loginModel';
import { LoginResult } from '../_interfaces/loginResponseModel';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  credentials: LoginModel = { email: '', password: '' };
  invalidLogin: boolean = false;
  errorMessage: string = '';

  constructor(private router: Router, private http: HttpClient, public nav: NavMenuService) { }

  ngOnInit(): void {
    this.nav.hide();
  }

  login = (form: NgForm) => {
    if (form.valid) {
      this.http.post<LoginResult>("https://localhost:44312/auth/login", this.credentials, {
        headers: new HttpHeaders({"Content-Type": "application/json"})
      })
        .subscribe({
          next: (response: LoginResult) => {
            const token = response.token;
            localStorage.setItem("jwt", token);
            this.invalidLogin = false;
            this.router.navigate(["/"]);
            this.nav.show();
          },
          error: (response: LoginResult) => {
            this.invalidLogin = true;
            this.errorMessage = response.errorMessage;
          }
        })
    }
  }
}
