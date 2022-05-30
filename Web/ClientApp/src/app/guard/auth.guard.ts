import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { EMPTY, Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { NavMenuService } from '../nav-menu/nav-menu.service';
import { ErrorResponse } from '../_responses/error-response';
import { TokenResponse } from '../_responses/token-response';
import { TokenService } from '../_services/token.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private tokenService: TokenService, private nav: NavMenuService) { }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    let session = this.tokenService.getSession();
    if (session == null) {
      this.router.navigate(['/login']);
      return false;
    }
    return true;
  }

}
