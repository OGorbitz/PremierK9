import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HTTP_INTERCEPTORS,
  HttpErrorResponse
} from '@angular/common/http';
import { EMPTY, Observable, throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators'
import { TokenService } from '../_services/token.service';
import { UserService } from '../_services/user.service';
import { ErrorResponse } from '../_responses/error-response';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  isRefreshingToken: boolean = false;
  constructor(private tokenService: TokenService, private userService: UserService, private router: Router) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request)
      .pipe(
        tap(response => console.log(JSON.stringify(response))),
        catchError((error: HttpErrorResponse) => {

          console.log(JSON.stringify(error));
          let session = this.tokenService.getSession();
          if (error.status === 401 && session != null && !this.tokenService.isLoggedIn() && !this.isRefreshingToken) {
            this.isRefreshingToken = true;
            console.log('Access Token is expired, we need to renew it');
            this.userService.refreshToken(session).subscribe({
              next: data => {
                console.info('Tokens renewed, we will save them into the local storage');
                this.tokenService.saveSession(data);
              },
              error: () => { },
              complete: () => { this.isRefreshingToken = false }
            });
          } else if (error.status === 400 && error.error.errorCode === 'invalid_grant') {
            console.log('the refresh token has expired, the user must login again');
            this.tokenService.logout();
            this.router.navigate(['login']);


          } else {
            let errorResponse: ErrorResponse = error.error;
            console.log(JSON.stringify(errorResponse));

            return throwError(() => errorResponse);
          }

          return EMPTY;
        })
      ) as Observable<any>;
  }
}
export const ErrorInterceptorProvider = { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true };
