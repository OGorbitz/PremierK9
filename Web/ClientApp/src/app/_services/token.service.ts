import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { share } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { NavMenuService } from '../nav-menu/nav-menu.service';
import { LoginRequest } from '../_requests/login-request';
import { TokenResponse } from '../_responses/token-response';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor(private router: Router, private http: HttpClient, private nav: NavMenuService) { }

  saveSession(tokenResponse: TokenResponse) {

    window.localStorage.setItem('AT', tokenResponse.accessToken);
    window.localStorage.setItem('RT', tokenResponse.refreshToken);
    if (tokenResponse.userId) {
      window.localStorage.setItem('UN', tokenResponse.userName);
      window.localStorage.setItem('ID', tokenResponse.userId.toString());
    }

  }

  getSession(): TokenResponse | null {
    if (window.localStorage.getItem('AT')) {
      const tokenResponse: TokenResponse = {
        accessToken: window.localStorage.getItem('AT') || '',
        refreshToken: window.localStorage.getItem('RT') || '',
        userName: window.localStorage.getItem('UN') || '',
        userId: window.localStorage.getItem('ID') || '',
      };

      return tokenResponse;
    }
    return null;
  }

  logout() {
    window.localStorage.clear();
    this.router.navigate(["/login"]);
  }

  isLoggedIn(): boolean {
    let session = this.getSession();
    if (!session) {
      return false;
    }

    // check if token is expired
    const jwtToken = JSON.parse(atob(session.accessToken.split('.')[1]));
    const tokenExpired = Date.now() > (jwtToken.exp * 1000);

    return !tokenExpired;

  }

  login(loginRequest: LoginRequest): Observable<TokenResponse> {
    let obs = this.http.post<TokenResponse>(environment.apiUrl + "auth/login", loginRequest, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    }).pipe(share())
    obs.subscribe({
      next: (response: TokenResponse) => {
        console.debug(`logged in successfully ${response}`);
        this.saveSession(response);
      }
    })
    return obs;
  }

  refreshToken(): Observable<TokenResponse> | undefined{
    let session = this.getSession();
    if (!session) {
      return undefined;
      window.localStorage.clear();
      this.router.navigate(["/login"]);
    }

    let refreshTokenRequest: any = {
      UserId: session.userId,
      RefreshToken: session.refreshToken
    };
    let obs = this.http.post<TokenResponse>(`${environment.apiUrl}auth/refreshToken`, refreshTokenRequest).pipe(share());
    obs.subscribe({
      next: (response: TokenResponse) => {
        console.debug(`Refreshed session successfully ${response}`);
        this.saveSession(response);
      }
    })
    return obs;
  }

}
