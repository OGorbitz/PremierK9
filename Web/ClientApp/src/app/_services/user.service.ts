import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { LoginRequest } from '../_requests/login-request';
import { SignupRequest } from '../_requests/signup-request';
import { TokenResponse } from '../_responses/token-response';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClient: HttpClient, private router: Router) { }

  login(loginRequest: LoginRequest): Observable<TokenResponse> {
    return this.httpClient.post<TokenResponse>(`${environment.apiUrl}/Auth/Login`, loginRequest);
  }

  signup(signupRequest: SignupRequest) {
    return this.httpClient.post(`${environment.apiUrl}/Auth/Signup`, signupRequest, { responseType: 'text' }); // response type specified, because the API response here is just a plain text (email address) not JSON
  }

  refreshToken(session: TokenResponse) {
    let refreshTokenRequest: any = {
      UserId: session.userId,
      RefreshToken: session.refreshToken
    };
    return this.httpClient.post<TokenResponse>(`${environment.apiUrl}/Auth/RefreshToken`, refreshTokenRequest);
  }

  logout() {
    this.router.navigate(["/login"]);
    return this.httpClient.post(`${environment.apiUrl}/users/signup`, null);
  }

/*  getUserInfo(): Observable<UserResponse> {
    return this.httpClient.get<UserResponse>(`${environment.apiUrl}/users/info`);
  }*/
}
