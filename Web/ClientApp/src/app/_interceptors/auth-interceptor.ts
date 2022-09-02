import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HTTP_INTERCEPTORS } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, EMPTY } from "rxjs";
import { catchError, map, switchMap } from 'rxjs/operators';
import { environment } from "../../environments/environment";
import { NavMenuService } from "../nav-menu/nav-menu.service";
import { ErrorResponse } from "../_responses/error-response";
import { TokenService } from "../_services/token.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private tokenService: TokenService, private router: Router, private nav: NavMenuService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const requestForApis = request.url.startsWith(environment.apiUrl);
    const isLoggedIn = this.tokenService.isLoggedIn();

    if (request.url == `${environment.apiUrl}auth/refreshToken`)
      return next.handle(request);

    if (isLoggedIn && requestForApis) {
      let session = this.tokenService.getSession();
      if (session) {
        request = request.clone({ headers: request.headers.set('Authorization', `Bearer ${session.accessToken}`) });
      }
    }


    if (!this.tokenService.isLoggedIn() && this.tokenService.getSession()) {

      console.log(`Refreshing session`);

      // refresh token
      let obs = this.tokenService.refreshToken();
      if (obs instanceof Observable) {
        return obs.pipe(
          //Good response pipe method
          switchMap(response => {
          request = request.clone({ headers: request.headers.set('Authorization', `Bearer ${response.accessToken}`) });

          return next.handle(request);
          }),
          //Error pipe method
          catchError((error: ErrorResponse) => {
            console.log(`Error refreshing session ${JSON.stringify(error)}`);
            this.router.navigate(['/login']);
            return EMPTY;
          })
        );
      }

    }
    return next.handle(request);
  }
}
