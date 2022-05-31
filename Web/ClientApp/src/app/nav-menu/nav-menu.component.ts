import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from '../_services/token.service';
import { NavMenuService } from './nav-menu.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  constructor(private router: Router, public nav: NavMenuService, private tokenService: TokenService) { }

  ngOnInit() {
  }

  logOut = () => {
    this.tokenService.logout();
  }
}
